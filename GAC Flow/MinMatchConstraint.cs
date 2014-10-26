using System.Collections.Generic;
using System.Linq;
using eZet.Csp.Constraints;

namespace eZet.Csp.Flow {
    public class MinMatchConstraint : IConstraint {

        private readonly int _minMatches;
        private readonly IVariable _focusVariable;

        public MinMatchConstraint(int minMatches, IVariable focusVariable, IEnumerable<IVariable> variables) {
            _focusVariable = focusVariable;
            var list = variables.ToList();
            list.Add(focusVariable);
            Variables = list;
            _minMatches = minMatches;
        }

        public IEnumerable<IVariable> Variables { get; private set; }

        public IEnumerable<IDomainValue> Eval(IVariable focusVariable) {
            var maxNonMatches = Variables.Count() - _minMatches - 1;
            // we can only reason about the focus variable
            if (focusVariable != _focusVariable)
                return focusVariable.DomainValues;
            var newDomain = new List<IDomainValue>();
            var neighbours = Variables.Where(v => v != focusVariable).ToList();
            
            // if number of determined neighbours is higher than max non-matching neighbours, we can determine the domain value
            var decidedValues = neighbours.Where(n => n.DomainValues.Count == 1).Select(n => n.DomainValues.Single()).GroupBy(v => v);
            var newValue = decidedValues.Where(group => @group.Count() > maxNonMatches).ToList();
            if (newValue.Count() == 1) {
                newDomain.Add(newValue.Single().Key);
                return newDomain;
            } if (newValue.Count() > 1) {
                // FAILED
                return new IDomainValue[0];
            }
   
            // new domain is all possible domainValues that appear atleast <minMatches> times in neighbours
            var allValues = neighbours.SelectMany(v => v.DomainValues).ToList();
            newDomain.AddRange(_focusVariable.DomainValues.Where(value => allValues.Count(v => v == value) >= _minMatches));

            return newDomain;
        }
    }
}
