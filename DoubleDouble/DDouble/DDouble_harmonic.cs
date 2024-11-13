namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble HarmonicNumber(int n) {
            return Consts.Harmonic.Value(n);
        }

        internal static partial class Consts {
            public static class Harmonic {
                private static readonly List<ddouble> a_table = new();
                private static ddouble acc, carry;

                static Harmonic() {
                    a_table.Add(0d);
                    a_table.Add(1d);
                    acc = 1d;
                    carry = 0d;
                }

                public static ddouble Value(int n) {
                    ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

                    if (n < a_table.Count) {
                        return a_table[n];
                    }

                    lock (a_table) {
                        for (int k = a_table.Count; k <= n; k++) {
                            ddouble v = Rcp(k);
                            ddouble d = v - carry;
                            ddouble acc_next = acc + d;

                            carry = (acc_next - acc) - d;
                            acc = acc_next;

                            a_table.Add(acc);
                        }

                        return a_table[n];
                    }
                }
            }
        }
    }
}
