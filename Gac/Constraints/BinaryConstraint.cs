using System;
using System.Collections.Generic;
using System.Globalization;
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
                list.Add(System.Linq.Expressions.Expression.Parameter(typeof (IDomainValue),
                    "@" + node.Identifier.ToString(CultureInfo.InvariantCulture)));
            }
            var lambda = DynamicExpression.ParseLambda(list.ToArray(), typeof (bool), Expression, null);
            return (Func<IDomainValue, IDomainValue, bool>) lambda.Compile();
        }

        /// <summary>
        /// Evaluates the constraint with the given values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Eval(params IDomainValue[] values) {
            if (Delegate == null) {
                Delegate = createDelegate();
            }
            return Delegate.Invoke(values[0], values[1]);
        }
    }
}