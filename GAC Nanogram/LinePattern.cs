using System;
using System.Collections.Generic;

namespace eZet.Csp.Nonogram {
    public class LinePattern : IDomainValue {

        public bool[] BlockArray;

        public LinePattern(SortedList<int, Block> blocks, int size) {
            Blocks = blocks;
            BlockArray = new bool[size];
            foreach (var block in blocks) {
                for (int i = block.Key; i < block.Key + block.Value.Length; ++i) {
                    BlockArray[i] = true;
                }
                Id += block.Key;
            }
        }

        public String Id { get; private set; }

        public SortedList<int, Block> Blocks { get; set; }

 
    }
}
