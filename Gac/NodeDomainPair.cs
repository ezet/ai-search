namespace eZet.Csp {
    /// <summary>
    /// This class represents a Node and IDomainValue pair
    /// </summary>
    public struct NodeDomainPair {

        /// <summary>
        /// Creates a new NodeDomainPair
        /// </summary>
        /// <param name="node"></param>
        /// <param name="domainValue"></param>
        public NodeDomainPair(IVariable node, IDomainValue domainValue) : this() {
            Node = node;
            DomainValue = domainValue;
        }

        /// <summary>
        /// Gets the node
        /// </summary>
        public IVariable Node { get; private set; }

        /// <summary>
        /// Gets the domain value
        /// </summary>
        public IDomainValue DomainValue { get; private set; }
    }
}