namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ddouble E = GenerateE();

        private static ddouble GenerateE() {
            ddouble x = ddouble.Zero;

            foreach (ddouble f in TaylorSequence) {
                ddouble x_next = x + f;

                if (x == x_next) {
                    break;
                }

                x = x_next;
            }

            return x;
        }
    }
}
