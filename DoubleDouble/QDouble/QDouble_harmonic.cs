using System;
using System.Collections.Generic;

namespace DoubleDouble {

    internal partial struct qdouble {

        public static qdouble HarmonicNumber(int n) {
            return Consts.Harmonic.Value(n);
        }

        private static partial class Consts {
            public static class Harmonic {
                private static readonly List<qdouble> a_table = new();
                private static qdouble sum;

                static Harmonic() {
                    a_table.Add(0);
                    a_table.Add(1);
                    sum = (qdouble)1d;
                }

                public static qdouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < a_table.Count) {
                        return a_table[n];
                    }

                    for (int k = a_table.Count; k <= n; k++) {
                        sum += Rcp(k);
                        a_table.Add(sum);
                    }

                    return a_table[n];
                }
            }
        }
    }
}
