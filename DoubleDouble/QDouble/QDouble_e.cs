namespace DoubleDouble {
    internal partial struct qdouble {
        public static readonly qdouble E = GenerateE();

        private static qdouble GenerateE() {
            qdouble x = Zero;

            foreach (qdouble f in TaylorSequence) {
                qdouble x_next = x + f;
                
                if (x == x_next) {
                    break;
                }

                x = x_next;
            }

            return x;
        }
    }
}
