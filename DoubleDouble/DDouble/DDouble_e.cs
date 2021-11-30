namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ddouble E = GenerateE();

        private static ddouble GenerateE() {
            KahanSum x = ddouble.Zero;

            foreach (ddouble f in TaylorSequence) {
                x.Add(f);

                if (x.IsConvergence) {
                    break;
                }
            }

            return x.Sum;
        }
    }
}
