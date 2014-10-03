using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace eZet.Csp.Constraints {
    public class GeneralConstraint : IConstraint {
        public IEnumerable<IVariable> Variables { get; private set; }

        public String Expression { get; private set; }

        public Delegate Delegate { get; private set; }

        public Dictionary<string, object> _rewrite;

        public GeneralConstraint(IEnumerable<IVariable> variables, string expression) {
            Variables = variables;
            Expression = expression;
            _rewrite.Add("#", "it[\" ");
        }

        //private Func<Dictionary<string, object>, bool> create() {
        //    var l = DynamicExpression.ParseLambda<Dictionary<string, IDomainValue>, bool>(Expression);
        //    return l.Compile();
        //}

        private Delegate createDelegate() {
            var list = new List<ParameterExpression>();
            foreach (var node in Variables) {
                list.Add(System.Linq.Expressions.Expression.Parameter(typeof (IDomainValue),
                    "@" + node.Identifier.ToString(CultureInfo.InvariantCulture)));
            }
            var l = DynamicExpression.ParseLambda(list.ToArray(), typeof (bool), Expression);
            return l.Compile();
        }


        public bool Eval(params IDomainValue[] values) {
            if (Delegate == null) {
                Delegate = createDelegate();
            }
            return (bool) Delegate.DynamicInvoke();
        }
    }
}