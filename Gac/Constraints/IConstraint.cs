using System.Collections.Generic;

namespace eZet.Csp.Constraints {
    /// <summary>
    /// Represents a CSP constraint
    /// </summary>
    public interface IConstraint {
        /// <summary>
        /// Gets a list of variables involved in this constraint
        /// </summary>
        IEnumerable<IVariable> Variables { get; }

        /// <summary>
        /// Evaluetes this constraint, using the values passes as parameter.
        /// </summary>
        /// <param name="values"></param>
        /// <returns>Returns true if the constraint is satisfied, otherwise false.</returns>
        bool Eval(params IDomainValue[] values);
    }
}