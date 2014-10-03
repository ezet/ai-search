using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using eZet.Csp.Constraints;
using eZet.Csp.Properties;

namespace eZet.Csp.GraphColouring {

    /// <summary>
    /// Represents a variable that can be drawn and bound
    /// </summary>
    public class SimpleVariable : IVariable, ICanvasObject {


        /// <summary>
        /// Creates a new variable
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SimpleVariable(string identifier, double x, double y) {
            Constraints = new List<IConstraint>();
            DomainValues = new ObservableCollection<IDomainValue>();
            Identifier = identifier;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the string identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Gets a list of all domain values
        /// </summary>
        public IList<IDomainValue> DomainValues { get; private set; }

        /// <summary>
        /// Gets a list of constraints on this variable
        /// </summary>
        public IList<IConstraint> Constraints { get; private set; }

        /// <summary>
        /// Sets the domain values
        /// </summary>
        /// <param name="values"></param>
        public void SetValues(IEnumerable<IDomainValue> values) {
            foreach (var value in values) {
                DomainValues.Add(value);
            }
            OnPropertyChanged("DomainValues");
        }

        /// <summary>
        /// Removes a domain value
        /// </summary>
        /// <param name="value"></param>
        public void RemoveValue(IDomainValue value) {
            DomainValues.Remove(value);
            if (DomainValues.Count < 2)
                OnPropertyChanged("DomainValues");
        }

        /// <summary>
        /// Adds a domain value
        /// </summary>
        /// <param name="value"></param>
        public void AddValue(IDomainValue value) {
            DomainValues.Add(value);
        }

        /// <summary>
        /// Gets an X coordinate used to draw the node
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets a Y coordinate for drawing the node
        /// </summary>
        public double Y { get; set; }

        public override string ToString() {
            return Identifier.ToString(CultureInfo.InvariantCulture);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}