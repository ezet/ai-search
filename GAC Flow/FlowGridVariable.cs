﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using eZet.Csp.Constraints;
using eZet.Csp.Flow.Annotations;

namespace eZet.Csp.Flow {
    public class FlowGridVariable : IVariable, INotifyPropertyChanged {

        public FlowGridVariable(string identifier, int x, int y) {
            Identifier = identifier;
            X = x;
            Y = y;

            DomainValues = new List<IDomainValue>();
            Constraints = new List<IConstraint>();
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }

        public string Identifier { get; private set; }
        public IList<IDomainValue> DomainValues { get; private set; }
        public IList<IConstraint> Constraints { get; private set; }


        public void SetValues(IEnumerable<IDomainValue> values) {
            DomainValues.Clear();
            DomainValues = values.ToList();
            OnPropertyChanged("DomainValues");
        }

        public void RemoveValue(IDomainValue value) {
            DomainValues.Remove(value);
            if (DomainValues.Count < 2)
                OnPropertyChanged("DomainValues");
        }

        public void AddValue(IDomainValue value) {
            DomainValues.Add(value);
            if (DomainValues.Count < 2)
                OnPropertyChanged("DomainValues");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() {
            return Identifier;
        }
    }
}
