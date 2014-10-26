namespace eZet.Csp.Nonogram {
    public class Block {

        public Block(int length) {
            Length = length;
            PaddedLength = Length;

        }

        public Block(int length, bool pad) {
            Length = length;
            Padded = pad;
            PaddedLength = Length;
            if (pad)
                ++PaddedLength;

        }

        public int Length { get; private set; }

        public int PaddedLength { get; private set; }

        public bool Padded { get; private set; }

    }
}
