using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {
    internal partial struct qdouble {
        public static qdouble Pow(qdouble x, long n) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (n == 0) {
                return 1d;
            }

            ulong n_abs = UIntUtil.Abs(n);
            qdouble y = 1d, z = x;

            while (n_abs > 0) {
                if ((n_abs & 1) == 1) {
                    y *= z;
                }

                z *= z;
                n_abs >>= 1;
            }

            return (n > 0) ? y : Rcp(y);
        }

        public static qdouble Pow2(qdouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (IsInfinity(x)) {
                return (x.Sign < 0) ? 0 : PositiveInfinity;
            }
            if (x >= 1024) {
                return PositiveInfinity;
            }

            int exp = (int)Floor(x);
            qdouble s = x - exp, c = Ldexp(1, exp);

            int index = (int)qdouble.Floor(s * Consts.Pow.Pow2TableN);
            qdouble v = (s - Consts.Pow.Pow2TableDx * index) * Consts.Log.Ln2;
            qdouble r = Consts.Pow.Pow2Table[index];

            qdouble y = 1;
            qdouble w = v;

            foreach (qdouble f in TaylorSequence.Skip(1)) {
                qdouble dy = f * w;
                qdouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                w *= v;
                y = y_next;
            }

            return c * y * r;
        }

        public static qdouble Pow(qdouble x, qdouble y) {
            if (x.Sign < 0) {
                return NaN;
            }

            if (IsZero(y)) {
                return IsNaN(x) ? NaN : 1;
            }

            qdouble z = Pow2(y * Log2(x));

            return z;
        }

        public static qdouble Pow10(qdouble x) {
            qdouble z = Pow2(x * Consts.Log.Lb10);

            return z;
        }

        public static qdouble Exp(qdouble x) {
            qdouble z = Pow2(x * Consts.Log.LbE);

            return z;
        }

        private static partial class Consts {
            public static class Pow {

                public static readonly IReadOnlyList<qdouble> Pow2Table = GeneratePow2Table();

                public static readonly qdouble Pow2TableDx = Rcp(Pow2Table.Count - 1);

                public static readonly int Pow2TableN = Pow2Table.Count - 1;

                public static qdouble[] GeneratePow2Table() {
                    const int n = 2048;
                    qdouble dx = Rcp(n);
                    qdouble[] table = new qdouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        qdouble x = dx * i;
                        table[i] = Pow2Prime(x);
                    }

                    return table;
                }

                private static qdouble Pow2Prime(qdouble x) {
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x == 1d) {
                        return 2;
                    }

                    qdouble v = x * Consts.Log.Ln2;

                    qdouble y = 1d, w = v;

                    foreach (qdouble f in TaylorSequence.Skip(1)) {
                        qdouble dy = f * w;
                        qdouble y_next = y + dy;

                        if (y == y_next) {
                            break;
                        }

                        w *= v;
                        y = y_next;
                    }

                    return y;
                }
            }
        }
    }
}
