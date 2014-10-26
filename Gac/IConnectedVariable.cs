using System.Collections.Generic;

namespace eZet.Csp {
    public interface IConnectedVariable : IVariable {

        IEnumerable<IConnectedVariable> Neighbours { get; }
        
        IConnectedVariable Out { get; set; }

        IConnectedVariable In { get; set; }

        bool IsStart { get; set; }

        bool IsEnd { get; set; }


    }
}
