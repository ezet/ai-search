using System.Collections.Generic;
using System.Linq;
using eZet.AStar;
using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// Represents a GAC state, with data to recreate the state from a root state.
    /// </summary>
    public class GacState : ISearchNode {
        /// <summary>
        /// Constructor for creating a new state
        /// </summary>
        public GacState() {
            DomainReductions = new List<NodeDomainPair>();
            UnsatisfiedConstraints = new List<IConstraint>();
        }

        /// <summary>
        /// Constructor used to add a child state
        /// </summary>
        /// <param name="assumedVariable"></param>
        /// <param name="assumedValue"></param>
        /// <param name="reductions"></param>
        /// <param name="parentState"></param>
        private GacState(IVariable assumedVariable, IDomainValue assumedValue, IEnumerable<NodeDomainPair> reductions,
            GacState parentState) {
            AssumedVariable = assumedVariable;
            AssumedValue = assumedValue;
            DomainReductions = reductions.ToList();
            ParentState = parentState;
            UnsatisfiedConstraints = new List<IConstraint>();
        }

        /// <summary>
        /// Implements ISearchNode
        /// </summary>
        public NodeState State { get; set; }

        /// <summary>
        /// Gets the parent state
        /// </summary>
        public GacState ParentState { get; private set; }

        /// <summary>
        /// Gets the IVariable that was assumed for this state
        /// </summary>
        public IVariable AssumedVariable { get; private set; }

        /// <summary>
        /// Gets the IDomainValue that was assigned to the AssumedVariable for this state
        /// </summary>
        public IDomainValue AssumedValue { get; private set; }

        /// <summary>
        /// Gets a list of all reductions that was caused by the assumption
        /// </summary>
        public IList<NodeDomainPair> DomainReductions { get; private set; }

        /// <summary>
        /// Gets a list of all constraints that couldn't be satisfied in this state
        /// </summary>
        public List<IConstraint> UnsatisfiedConstraints { get; private set; }

        public GacState AddChild(IVariable variable, IDomainValue value) {
            var removedValues = new List<NodeDomainPair>();
            foreach (var v in variable.DomainValues.Where(dv => dv != value))
                removedValues.Add(new NodeDomainPair(variable, v));
            return new GacState(variable, value, removedValues, this);
        }

        /// <summary>
        /// Gets an enumerable for enumerating all parent states
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GacState> Path() {
            for (var p = this; p != null; p = p.ParentState) {
                yield return p;
            }
        }
    }
}