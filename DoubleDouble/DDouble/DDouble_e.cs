namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ddouble E = GenerateE();

        private static ddouble GenerateE() {
            Accumulator x = ddouble.Zero;

            foreach (ddouble f in TaylorSequence) {
                x += f;

                if (x.IsConvergence) {
                    break;
                }
            }

            return x.Sum;
        }
    }
}
