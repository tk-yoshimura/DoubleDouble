namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble HarmonicNumber(int n) {
            return Consts.Harmonic.Value(n);
        }

        internal static partial class Consts {
            public static class Harmonic {
                private static readonly List<ddouble> a_table = new();
                private static ddouble acc;

                static Harmonic() {
                    a_table.Add(0);
                    a_table.Add(1);
                    acc = 1d;
                }

                public static ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < a_table.Count) {
                        return a_table[n];
                    }

                    for (int k = a_table.Count; k <= n; k++) {
                        acc += Rcp(k);
                        a_table.Add(acc);
                    }

                    return a_table[n];
                }
            }
        }
    }
}
