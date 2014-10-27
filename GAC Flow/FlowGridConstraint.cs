using System.Collections.Generic;
using System.Linq;
using eZet.Csp.Constraints;

namespace eZet.Csp.Flow {
    public class FlowGridConstraint : IConstraint {

        private readonly int _minMatches;
        private readonly IVariable _centerVariable;

        public FlowGridConstraint(int minMatches, IVariable focusVariable, IEnumerable<IVariable> variables) {
            _centerVariable = focusVariable;
            var list = variables.ToList();
            list.Add(focusVariable);
            Variables = list;
            _minMatches = minMatches;
        }

        public IEnumerable<IVariable> Variables { get; private set; }

        public IEnumerable<IDomainValue> Eval(IVariable focusVariable) {
            // we only prune the center variable
            if (focusVariable != _centerVariable)
                return focusVariable.DomainValues;
 
            
            var focus = (FlowGridVariable) focusVariable;
            var maxNonMatches = Variables.Count() - _minMatches - 1;

            var newDomain = focusVariable.DomainValues.Cast<FlowGridDomainValue>().ToList();

            var neighbours = Variables.Where(v => v != focusVariable).Cast<FlowGridVariable>().ToList();

            // if a neighbour has this cell as it's only valid input or output, prune all domain values that doesn't have that neighbour as it's output or input respectively
            foreach (var n in neighbours) {
                var values = n.DomainValues.Cast<FlowGridDomainValue>().ToList();
                if (!n.IsStart && values.All(v => v.In == focus)) {
                    newDomain.RemoveAll(v => v.Out != n);
                }
                if (!n.IsEnd && values.All(v => v.Out == focus)) {
                    newDomain.RemoveAll(v => v.In != n);
                }
            }

            foreach (var value in newDomain.ToList()) {
                // remove values that try to flow into a start node or get flow from an end node
                if (!focus.IsEnd && value.Out.IsStart || !focus.IsStart && value.In.IsEnd) {
                    newDomain.Remove(value);
                    continue;
                }
                // remove values that try to flow into a neighbour that doesn't accept flow from us
                if (!focus.IsStart && !value.In.DomainValues.Cast<FlowGridDomainValue>().Any(v => v.Out == focusVariable && v.Value == value.Value)) {
                    newDomain.Remove(value);
                    continue;
                }
                // remove values that require flow from a neighbour that cannot flow to us
                if (!focus.IsEnd && !value.Out.DomainValues.Cast<FlowGridDomainValue>().Any(v => v.In == focusVariable && v.Value == value.Value)) {
                    newDomain.Remove(value);
                    continue;
                }
            }

            // if number of determined neighbouring colours is higher than max non-matching neighbouring colours, this node must have that colour
            var decidedValues =
              neighbours.Where(
                  n =>
                      n.DomainValues.Cast<FlowGridDomainValue>()
                          .All(d => d.Value == ((FlowGridDomainValue)n.DomainValues.First()).Value))
                  .Select(n => n.DomainValues.First()).GroupBy(v => ((FlowGridDomainValue)v).Value);
            var newValue = decidedValues.Where(group => @group.Count() > maxNonMatches).ToList();
            if (newValue.Count() == 1) {
                newDomain.RemoveAll(v => v.Value != newValue.First().Key);
                return newDomain;
            }
            
            if (newValue.Count() > 1) {
                // failed, more than one color with more than maxNonMatches neighbours
                return new IDomainValue[0];
            }

            // new domain is all possible domainValues that appear atleast <minMatches> times in neighbours
            var allValues = neighbours.SelectMany(n => n.DomainValues.GroupBy(v => ((FlowGridDomainValue)v).Value)).ToList();
            newDomain.RemoveAll(value => allValues.Count(v => v.Key == value.Value) < _minMatches);
  
 

            return newDomain;
        }

    }
}
