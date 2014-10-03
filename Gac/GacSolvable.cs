using System.Collections.Generic;
using System.Linq;
using eZet.AStar;
using eZet.Csp.Constraints;
using MoreLinq;

namespace eZet.Csp {
    public class GacSolvable : ISearchSolvable {
        /// <summary>
        /// Represents the problem status
        /// </summary>
        public enum Result {
            Solved,
            Unsolved,
            Failed
        }

        /// <summary>
        /// A list of constraints
        /// </summary>
        private readonly List<IConstraint> _constraints;

        /// <summary>
        /// A qeueu of filter tasks
        /// </summary>
        private readonly Queue<FilterTask> _reviseQueue;

        public GacSolvable(Graph graph, IEnumerable<IDomainValue> domainValues) {
            Graph = graph;
            Algorithm = new AStar.Algorithms.AStar(0);
            StartNode = new GacState();
            AppliedStates = new Stack<GacState>();
            AppliedStates.Push((GacState) StartNode);
            _reviseQueue = new Queue<FilterTask>(100);
            _constraints = new List<IConstraint>();
            DomainValues = domainValues.ToList();
            foreach (var node in graph.Nodes) {
                node.SetValues(DomainValues);
            }
        }

        /// <summary>
        /// Gets a list of all possible domain values
        /// </summary>
        public List<IDomainValue> DomainValues { get; private set; }

        /// <summary>
        /// Gets the graph model
        /// </summary>
        public Graph Graph { get; private set; }

        /// <summary>
        /// Gets a stack of all currently applied states
        /// </summary>
        public Stack<GacState> AppliedStates { get; private set; }

        /// <summary>
        /// Gets or sets the ISearchAlgorithm used for incremental search
        /// </summary>
        public ISearchAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Gets the search path
        /// </summary>
        public Path SearchPath { get; private set; }

        /// <summary>
        /// Adds a collection of constraints
        /// </summary>
        /// <param name="constraints"></param>
        public void AddConstraints(IEnumerable<IConstraint> constraints) {
            foreach (var constraint in constraints) {
                _constraints.Add(constraint);
                foreach (var node in constraint.Variables) {
                    node.Constraints.Add(constraint);
                }
            }
        }

        /// <summary>
        /// Adds a single constraint
        /// </summary>
        /// <param name="constraint"></param>
        public void AddConstraint(IConstraint constraint) {
            foreach (var node in constraint.Variables) {
                node.Constraints.Add(constraint);
            }
            _constraints.Add(constraint);
        }

        /// <summary>
        /// Solves the CSP 
        /// </summary>
        /// <returns></returns>
        public Result Solve() {
            // initialize
            enqueueAllConstraints(Graph.Nodes);
            var result = filterLoop();
            if (result == Result.Unsolved) {
                SearchPath = runIncrementalSearch();
                result = Status;
            }
            return result;
        }

        /// <summary>
        /// A loop that runs through all filter tasks on the revise queue.
        /// </summary>
        /// <returns></returns>
        private Result filterLoop() {
            while (_reviseQueue.Any()) {
                var task = _reviseQueue.Dequeue();
                bool reduced = reduce(task.Node, task.Constraint);
                var status = Status;
                if (status == Result.Solved) {
                    return Result.Solved;
                }
                if (Status == Result.Failed) {
                    AppliedStates.Peek().UnsatisfiedConstraints.Add(task.Constraint);
                    return Result.Failed;
                }
                if (reduced) {
                    enqueueRelatedConstraints(task.Node, task.Constraint);
                }
            }
            return Result.Unsolved;
        }

        /// <summary>
        /// Evaluates a variable against a constraint, and reduces the domain if the constraint is not satisfied
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        private bool reduce(IVariable variable, IConstraint constraint) {
            var reduced = false;
            foreach (var focusValue in variable.DomainValues.ToList()) {
                bool satisfied = false;
                foreach (var testValue in constraint.Variables.Single(v => v != variable).DomainValues) {
                    satisfied = constraint.Eval(new[] {focusValue, testValue});
                    if (satisfied) break;
                }
                if (!satisfied) {
                    removeDomainValue(variable, focusValue);
                    reduced = true;
                    // Add the reductions to the state tracker
                    AppliedStates.Peek().DomainReductions.Add(new NodeDomainPair(variable, focusValue));
                }
            }
            return reduced;
        }

        /// <summary>
        /// Reverts to the root state
        /// </summary>
        private void resetState() {
            while (AppliedStates.Count > 1) {
                backtrack();
            }
        }

        /// <summary>
        /// Reverts the most recent state change
        /// </summary>
        private void backtrack() {
            var s = AppliedStates.Pop();
            foreach (var domain in s.DomainReductions) {
                domain.Node.AddValue(domain.DomainValue);
            }
        }

        /// <summary>
        /// Applies a state
        /// </summary>
        /// <param name="node"></param>
        private void applyState(GacState node) {
            if (!AppliedStates.Contains(node)) {
                foreach (var reduction in node.DomainReductions) {
                    reduction.Node.RemoveValue(reduction.DomainValue);
                }
                AppliedStates.Push(node);
            }
        }

        /// <summary>
        /// Applies a state and all it's parent states
        /// </summary>
        /// <param name="node"></param>
        private void setState(GacState node) {
            foreach (var n in node.Path().Reverse()) {
                applyState(n);
            }
        }

        /// <summary>
        /// Runs the search algorithm
        /// </summary>
        /// <returns></returns>
        private Path runIncrementalSearch() {
            return Algorithm.Run(this);
        }

        /// <summary>
        /// Removes a domain value from  a variable
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="domainValue"></param>
        private void removeDomainValue(IVariable variable, IDomainValue domainValue) {
            variable.RemoveValue(domainValue);
        }

        /// <summary>
        /// Adds tasks for every variable/constraint combination
        /// </summary>
        /// <param name="nodes"></param>
        private void enqueueAllConstraints(IEnumerable<IVariable> nodes) {
            foreach (var variable in nodes) {
                foreach (var constraint in variable.Constraints) {
                    _reviseQueue.Enqueue(new FilterTask(variable, constraint));
                }
            }
        }

        /// <summary>
        /// Adds tasks for all constraints affected by the given node. Optionally ignores a given constraint.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ignore"></param>
        private void enqueueRelatedConstraints(IVariable node, IConstraint ignore = null) {
            foreach (var constraint in node.Constraints.Where(c => c != ignore)) {
                foreach (var variable in constraint.Variables.Where(n => n != node)) {
                    _reviseQueue.Enqueue(new FilterTask(variable, constraint));
                }
            }
        }

        /// <summary>
        /// Returns the status of the solver
        /// </summary>
        private Result Status {
            get {
                if (Graph.Nodes.Any(p => !p.DomainValues.Any()))
                    return Result.Failed;
                if (Graph.Nodes.All(p => p.DomainValues.Count == 1))
                    return Result.Solved;
                return Result.Unsolved;
            }
        }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        public ISearchNode StartNode { get; private set; }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsSolution(ISearchNode node) {
            resetState();
            setState((GacState) node);
            var status = Status;
            return status == Result.Solved;
        }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        /// <param name="lastNode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public double Cost(ISearchNode lastNode, ISearchNode node) {
            return 1;
        }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public double Estimate(ISearchNode node) {
            var state = (GacState) node;
            applyState(state);
            double estimate = Graph.Nodes.Sum(n => n.DomainValues.Count - 1);
            backtrack();
            return estimate;
        }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<ISearchNode> Expand(ISearchNode node) {
            var state = (GacState) node;
            // Choose the variable with the smallest domain
            var variables = Graph.Nodes.Where(v => v.DomainValues.Count > 1).ToList();
            if (!variables.Any())
                return new List<ISearchNode>();
            IVariable assumedNode = variables.MinBy(v => v.DomainValues.Count);
            var neighbours = new List<ISearchNode>();
            // Create child states for each possible domain value
            foreach (var domainValue in assumedNode.DomainValues.ToList()) {
                var newNode = state.AddChild(assumedNode, domainValue);
                // apply the state and filter it
                applyState(newNode);
                enqueueRelatedConstraints(newNode.AssumedVariable);
                var result = filterLoop();
                backtrack();
                // if the state is valid, add it as a neighbour
                if (result != Result.Failed)
                    neighbours.Add(newNode);
            }
            return neighbours;
        }

        /// <summary>
        /// Implements ISearchSolvable
        /// </summary>
        public void Reset() {
            throw new System.NotImplementedException();
        }
    }
}