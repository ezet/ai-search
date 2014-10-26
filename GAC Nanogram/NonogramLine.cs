using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using eZet.Csp.Constraints;
using eZet.Csp.Nonogram.Properties;

namespace eZet.Csp.Nonogram {
    public class NonogramLine : IVariable, INotifyPropertyChanged {

        public enum LineType {
            Row, Column
        }

        public NonogramLine(int index, int length, LineType type, IEnumerable<Block> blocks) {
            Constraints = new List<IConstraint>();
            DomainValues = new List<IDomainValue>();
            Index = index;
            Identifier = type + ", " + index;
            Length = length;
            Type = type;
            Blocks = blocks;
            generatePatterns();
        }

        public int Index { get; private set; }

       

        private bool exists(LinePattern pattern) {
            return DomainValues.Any(value => ((LinePattern) value).Id == pattern.Id);
        }

        public IEnumerable<Block> Blocks { get; private set; }

        public LineType Type { get; private set; }

        public string Identifier { get; private set; }

        public int Length { get; private set; }

        public IList<IDomainValue> DomainValues { get; private set; }

        public IList<IConstraint> Constraints { get; private set; }

        public void SetValues(IEnumerable<IDomainValue> values) {
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
        }

        private void generatePatterns() {
            var dict = new SortedList<int, Block>();
            int count = 0;
            int position = 0;
            foreach (Block block in Blocks) {
                dict.Add(position, block);
                position += block.PaddedLength;
            }
            var pattern = new LinePattern(dict, Length);
            Debug.Assert(IsValid(pattern));
            DomainValues.Add(pattern);
            while (count < DomainValues.Count) {
                var patterns = permutate((LinePattern)DomainValues.ToList()[count]);
                patterns.ForEach(p => DomainValues.Add(p));
                ++count;
            }
        }

        public Boolean IsValid(LinePattern pattern) {
            foreach (var block in pattern.Blocks) {
                if (block.Key < 0) return false;
                if (block.Key + block.Value.Length > Length) return false;
                var next = pattern.Blocks.SkipWhile(t => t.Key != block.Key).Skip(1).ToList();
                if (next.Any() && block.Key + block.Value.PaddedLength > next.First().Key) return false;

            }
            return true;
        }


        private List<LinePattern> permutate(LinePattern pattern) {
            var patterns = new List<LinePattern>();
            foreach (var block in pattern.Blocks) {
                int newPos = block.Key + 1;
                int endpos = newPos + block.Value.PaddedLength - 1;
                var next = pattern.Blocks.SkipWhile(t => t.Key != block.Key).Skip(1).ToList();
                if ((next.Any() && endpos < next.First().Key) || (!next.Any() && endpos < Length)) {
                    var newList = new SortedList<int, Block>(pattern.Blocks);
                    newList.Remove(block.Key);
                    newList.Add(newPos, block.Value);
                    var newPattern = new LinePattern(newList, Length);
                    if (!exists(newPattern)) {
                        patterns.Add(newPattern);
                    }
                }
            }
            return patterns;
        }

        public override string ToString() {
            return Identifier;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
