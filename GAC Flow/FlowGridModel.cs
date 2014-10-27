using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using eZet.Csp.Constraints;
using MoreLinq;

namespace eZet.Csp.Flow {
    public class FlowGridModel : CspModel {

        private readonly FlowGridVariable[,] _nodes;


        private readonly int _dimension;

        private readonly int _domainValueCount;

        public FlowGridModel(int dimension, int domainValueCount) {
            _dimension = dimension;
            _domainValueCount = domainValueCount;
            var domainValues = new List<FlowGridDomainValue>();
            var list = new List<FlowGridVariable>();
            _nodes = new FlowGridVariable[dimension, dimension];

            DomainValues = domainValues;
            Nodes = list;

            for (int y = 0; y < dimension; ++y) {
                for (int x = 0; x < dimension; ++x) {
                    _nodes[y, x] = new FlowGridVariable(x + "," + y, x, y);
                    list.Add(_nodes[y, x]);
                }
            }

            foreach (var node in list) {
                var neighbours = getNeighbours(node).ToList();
                var nodeValues = new List<FlowGridDomainValue>();
                foreach (var inNode in neighbours) {
                    foreach (var outNode in neighbours.Except(new[] { inNode })) {
                        for (var i = 0; i < domainValueCount; ++i) {
                            var domainValue =
                                domainValues.SingleOrDefault(v => v.Value == i && v.In == inNode && v.Out == outNode);
                            if (domainValue == null) {
                                domainValue = new FlowGridDomainValue(i, inNode, outNode);
                                domainValues.Add(domainValue);
                            }
                            nodeValues.Add(domainValue);
                        }
                    }
                    node.SetValues(nodeValues);
                }
            }
        }

        public void Initialize() {
            Constraints = new List<IConstraint>();
            foreach (var variable in Nodes) {
                var node = (FlowGridVariable)variable;
                var nodes = new List<FlowGridVariable>(getNeighbours(node));
                Constraints.Add(node.IsStart || node.IsEnd
                    ? new FlowGridConstraint(1, node, nodes)
                    : new FlowGridConstraint(2, node, nodes));
            }
        }


        public void AddEndPoint(int x, int y, int value) {
            var node = GetNode(x, y);
            node.IsEnd = true;
            node.SetValues(node.DomainValues.Where(v => ((FlowGridDomainValue) v).Value == value).DistinctBy(v => ((FlowGridDomainValue)v).In).ToList());
            return;

            var neighbours = getNeighbours(node);
            var domainValues = new List<FlowGridDomainValue>();
            foreach (var inNode in neighbours) {
                            var domainValue = new FlowGridDomainValue(value, inNode, null);
                            domainValues.Add(domainValue);
                }
            node.SetValues(domainValues);
            domainValues.AddRange(DomainValues.Cast<FlowGridDomainValue>());
            DomainValues = domainValues;
        }

        public void AddStartPoint(int x, int y, int value) {
            var node = GetNode(x, y);
            node.IsStart = true;
            node.SetValues(node.DomainValues.Where(v => ((FlowGridDomainValue)v).Value == value).DistinctBy(v => ((FlowGridDomainValue)v).Out).ToList());
            return;
            var neighbours = getNeighbours(node);
            var domainValues = new List<FlowGridDomainValue>();
            foreach (var outNode in neighbours) {
                    var domainValue = new FlowGridDomainValue(value, null, outNode);
                    domainValues.Add(domainValue);
            }
            node.SetValues(domainValues);
            domainValues.AddRange(DomainValues.Cast<FlowGridDomainValue>());
            DomainValues = domainValues;
        }


        public FlowGridVariable GetNode(int x, int y) {
            if (isOnGrid(x, y)) {
                return _nodes[y, x];
            }
            return null;
        }

        private IEnumerable<FlowGridVariable> getNeighbours(FlowGridVariable node) {
            var neighbors = new List<FlowGridVariable> {
                GetNode(node.X + 1, node.Y),
                GetNode(node.X, node.Y - 1),
                GetNode(node.X - 1, node.Y),
                GetNode(node.X, node.Y + 1),
            };
            return neighbors.Where(neighbor => neighbor != null).ToList();

        }

        private bool isOnGrid(int x, int y) {
            return x >= 0 && y >= 0 && x < _dimension && y < _dimension;
        }


        public IEnumerable<IVariable> Nodes { get; private set; }

        public List<IConstraint> Constraints { get; private set; }

        public IEnumerable<IDomainValue> DomainValues { get; private set; }

    }
}
