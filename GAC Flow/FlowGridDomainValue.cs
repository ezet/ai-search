using System;
using System.Globalization;

namespace eZet.Csp.Flow {
    public class FlowGridDomainValue : IDomainValue {

        public FlowGridDomainValue(int value, FlowGridVariable invar, FlowGridVariable outvar) {
            Value = value;
            In = invar;
            Out = outvar;
        }

        public int Value { get; private set; }

        public FlowGridVariable In { get; private set; }

        public FlowGridVariable Out { get; private set; }

        public override String ToString() {
            return Value.ToString(CultureInfo.InvariantCulture) + ", In: " + In + "Out: " + Out;
        }
    }
}
