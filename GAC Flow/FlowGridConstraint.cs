using System.Collections.Generic;
using System.Linq;
using eZet.Csp.Constraints;

namespace eZet.Csp.Flow {
    public class FlowGridConstraint : IConstraint {
        private readonly List<IConnectedVariable> _variables;
        private readonly IConnectedVariable _centerVariable;
        private readonly List<IConnectedVariable> _neighbours;

        public FlowGridConstraint(IConnectedVariable centerVariable, IEnumerable<IConnectedVariable> neighbours) {
            _centerVariable = centerVariable;
            _neighbours = neighbours.ToList();
            var list = _neighbours.ToList();
            list.Add(centerVariable);
            _variables = list;
        }

        public IEnumerable<IVariable> Variables {
            get { return _variables; }
        }

        public IEnumerable<IDomainValue> Eval(IVariable focusVariable) {
            var focus = (IConnectedVariable)focusVariable;

            var connectableNeighbours = _neighbours.Where(n => n.DomainValues.Any(v => _centerVariable.DomainValues.Contains(v))).ToList();
            var inputs = connectableNeighbours.Where(n => n.Out == null && !n.IsEnd || n.Out == _centerVariable).ToList();
            var outputs = connectableNeighbours.Where(n => n.In == null && !n.IsStart || n.In == _centerVariable).ToList();

            // ensure we have have possible inputs and outputs
            if (!_centerVariable.IsStart && !inputs.Any() || !_centerVariable.IsEnd && !outputs.Any()) {
                //Thread.Sleep(500);
                return new IDomainValue[0];
            }

            // if center has only one possible input, remove it from possible outputs
            if (!_centerVariable.IsStart && _centerVariable.In == null && inputs.Count() == 1) {
                outputs.Remove(inputs.Single());
            }

            // if center has only one possible output, remove it from possible inputs
            if (!_centerVariable.IsEnd && _centerVariable.Out == null && outputs.Count() == 1) {
                inputs.Remove(outputs.Single());
            }

            IList<IDomainValue> newDomain = focusVariable.DomainValues.ToList();

            // 
            if (focusVariable != _centerVariable) {
                if (inputs.Count == 1 && inputs.Single() == focusVariable && !_centerVariable.IsStart) {
                    focus.Out = _centerVariable;
                    newDomain = newDomain.Intersect(_centerVariable.DomainValues).ToList();
                }
                if (outputs.Count == 1 && outputs.Single() == focusVariable && !_centerVariable.IsEnd) {
                    focus.In = _centerVariable;
                    newDomain = newDomain.Intersect(_centerVariable.DomainValues).ToList();
                }
                return newDomain;
            }


            if (_centerVariable.In != null && _centerVariable.DomainValues.Intersect(_centerVariable.In.DomainValues).Any()) {
                newDomain = newDomain.Intersect(_centerVariable.In.DomainValues).ToList();
            }

            if (_centerVariable.Out != null && _centerVariable.DomainValues.Intersect(_centerVariable.Out.DomainValues).Any()) {
                newDomain = newDomain.Intersect(_centerVariable.Out.DomainValues).ToList();
            }

            if (!_centerVariable.IsStart && inputs.Count() == 1) {
                _centerVariable.In = inputs.Single();
                newDomain = newDomain.Intersect(_centerVariable.In.DomainValues).ToList();
            }

            if (!_centerVariable.IsEnd && outputs.Count() == 1) {
                _centerVariable.Out = outputs.Single();
                newDomain = newDomain.Intersect(_centerVariable.Out.DomainValues).ToList();
            }

            return newDomain;
        }
    }
}

