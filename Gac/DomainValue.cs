using System;
using System.Globalization;

namespace eZet.Csp {
    /// <summary>
    /// Represents a integer domain value
    /// </summary>
    public class DomainValue : IDomainValue {

        /// <summary>
        /// Creates a new domain value
        /// </summary>
        /// <param name="value"></param>
        public DomainValue(int value) {
            Value = value;
        }

        /// <summary>
        /// Gets the domain value
        /// </summary>
        public int Value { get; private set; }

        public override String ToString() {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}