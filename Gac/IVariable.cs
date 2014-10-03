using System.Collections.Generic;
using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// Inteface for an IVariable
    /// </summary>
    public interface IVariable {

        /// <summary>
        /// A string identifier used to identify the variable in constraint expressions
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Gets all domain values assigned to this variable
        /// </summary>
        IList<IDomainValue> DomainValues { get; }

        /// <summary>
        /// Gets all constraints involving this variable
        /// </summary>
        IList<IConstraint> Constraints { get; }

        /// <summary>
        /// Sets the domain values
        /// </summary>
        /// <param name="values"></param>
        void SetValues(IEnumerable<IDomainValue> values);

        /// <summary>
        /// Removes a domain value
        /// </summary>
        /// <param name="value"></param>
        void RemoveValue(IDomainValue value);

        /// <summary>
        /// Adds a domain value
        /// </summary>
        /// <param name="value"></param>
        void AddValue(IDomainValue value);
    }
}