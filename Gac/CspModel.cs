using System.Collections.Generic;
using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// Represents a graph
    /// </summary>
    public interface CspModel {

        /// <summary>
        /// Gets all nodes the nodes in the graph
        /// </summary>
        IEnumerable<IVariable> Nodes { get; }

        IEnumerable<IDomainValue> DomainValues { get; }

        List<IConstraint> Constraints { get; }

    }
}