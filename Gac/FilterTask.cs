using eZet.Csp.Constraints;

namespace eZet.Csp {
    /// <summary>
    /// This class represents a task for performing filtering on a specified node, using a specified constraint
    /// </summary>
    public struct FilterTask {
        public FilterTask(IVariable node, IConstraint constraint) : this() {
            Node = node;
            Constraint = constraint;
        }

        /// <summary>
        /// Gets the node that should be filtered
        /// </summary>
        public IVariable Node { get; private set; }

        /// <summary>
        /// Gets the contraint used for filtering
        /// </summary>
        public IConstraint Constraint { get; private set; }
    }
}