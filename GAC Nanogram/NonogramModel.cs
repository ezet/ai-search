using System.Collections.Generic;
using System.Linq;
using eZet.Csp.Constraints;

namespace eZet.Csp.Nonogram {
    public class NonogramModel : CspModel {

        public NonogramModel(IEnumerable<NonogramLine> rows, IEnumerable<NonogramLine> cols) {
            var list = rows.ToList();
            list.AddRange(cols);
            Nodes = list;
            List<IDomainValue> domainValues = new List<IDomainValue>();
            DomainValues = domainValues;
            Nodes.ToList().ForEach(l => l.DomainValues.ToList().ForEach(domainValues.Add));
            Constraints = new List<IConstraint>();
            foreach (var row in rows) {
                foreach (var col in cols) {
                    Constraints.Add(new NonogramConstraint(row, col));
                }
            }
        }


        public IEnumerable<IVariable> Nodes { get; private set; }
        public IEnumerable<IDomainValue> DomainValues { get; private set; }
        public List<IConstraint> Constraints { get; private set; }


    }
}
