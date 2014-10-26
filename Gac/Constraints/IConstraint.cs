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
        /// Evaluates the constraint. Returns a list of all domain values for the focus variable that does not satisfy the constriant.
        /// </summary>
        /// <param name="focusVariable"></param>
        /// <returns></returns>
        IEnumerable<IDomainValue> Eval(IVariable focusVariable);
    }
}