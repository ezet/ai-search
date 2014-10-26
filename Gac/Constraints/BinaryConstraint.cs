using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace eZet.Csp.Constraints {
    /// <summary>
    /// Represents a binary constraint
    /// </summary>
    public class BinaryConstraint : IConstraint {
        /// <summary>
        /// Gets all the involved variables
        /// </summary>
        public IEnumerable<IVariable> Variables { get; private set; }

        /// <summary>
        /// Gets the string expression
        /// </summary>
        public string Expression { get; private set; }

        /// <summary>
        /// The delegate used for evaluation
        /// </summary>
        public Func<IDomainValue, IDomainValue, bool> Delegate { get; private set; }

        /// <summary>
        /// Creates a new constraint for the given variables
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="expression"></param>
        public BinaryConstraint(IEnumerable<IVariable> variables, string expression) {
            Variables = variables;
            Expression = expression;
        }

        /// <summary>
        /// Instanties the delegate
        /// </summary>
        /// <returns></returns>
        private Func<IDomainValue, IDomainValue, bool> createDelegate() {
            var list = new List<ParameterExpression>();
            foreach (var node in Variables) {
                list.Add(System.Linq.Expressions.Expression.Parameter(typeof(IDomainValue),
                    "@" + node.Identifier.ToString(CultureInfo.InvariantCulture)));
            }
            var lambda = DynamicExpression.ParseLambda(list.ToArray(), typeof(bool), Expression, null);
            return (Func<IDomainValue, IDomainValue, bool>)lambda.Compile();
        }

        /// <summary>
        /// Evaluates the constraint for all possible domain combinations.
        /// Returns a list of valid domain values.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDomainValue> Eval(IVariable focusVariable) {
            if (Delegate == null) {
                Delegate = createDelegate();
            }
            var newDomain = new List<IDomainValue>();
            foreach (var value1 in focusVariable.DomainValues) {
                bool satisfied = false;
                foreach (var value2 in Variables.Single(v => v != focusVariable).DomainValues) {
                    satisfied = Delegate.Invoke(value1, value2);
                    if (satisfied) {
                        newDomain.Add(value1);
                        break;
                    }
                }
            }
            return newDomain;
        }
    }
}