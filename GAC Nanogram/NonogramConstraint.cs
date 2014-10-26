using System.Collections.Generic;
using System.Linq;
using eZet.Csp.Constraints;

namespace eZet.Csp.Nonogram {
    public class NonogramConstraint : IConstraint {
        public NonogramConstraint(IVariable row, IVariable column) {
            var variables = new List<IVariable>();
            variables.Add(row);
            variables.Add(column);
            Variables = variables;
        }


        public IEnumerable<IVariable> Variables { get; private set; }

        public IEnumerable<IDomainValue> Eval(IVariable focusVariable) {
            var focus = (NonogramLine)focusVariable;
            var other = (NonogramLine)Variables.Single(t => t != focusVariable);
            var newDomain = focus.DomainValues.ToList();
            var focusValues = focus.DomainValues.Cast<LinePattern>().ToList();
            var otherValues = other.DomainValues.Cast<LinePattern>().ToList();
            if (otherValues.TrueForAll(v => v.BlockArray[focus.Index])) {
                foreach (var value in focusValues) {
                    if (value.BlockArray[other.Index] == false) {
                        newDomain.Remove(value);
                    }
                }
            } else if (otherValues.TrueForAll(v => v.BlockArray[focus.Index] == false)) {
                foreach (var value in focusValues) {
                    if (value.BlockArray[other.Index] == true) {
                        newDomain.Remove(value);
                    }
                }
            }
            return newDomain;
        }
    }
}
