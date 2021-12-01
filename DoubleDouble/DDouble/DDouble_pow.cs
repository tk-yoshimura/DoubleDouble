using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Pow(ddouble x, long n) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (n == 0) {
                return 1d;
            }

            ulong n_abs = UIntUtil.Abs(n);
            ddouble y = 1d, z = x;

            while (n_abs > 0) {
                if ((n_abs & 1) == 1) {
                    y *= z;
                }

                z *= z;
                n_abs >>= 1;
            }

            return (n > 0) ? y : Rcp(y);
        }

        public static ddouble Pow2(ddouble x) {
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
            ddouble s = x - exp, c = Ldexp(1, exp);

            int index = (int)ddouble.Floor(s * Consts.Pow.Pow2TableN);
            ddouble v = (s - Consts.Pow.Pow2TableDx * index) * Consts.Log.Ln2;
            ddouble r = Consts.Pow.Pow2Table[index];

            ddouble y = 1;
            ddouble w = v;

            foreach (ddouble f in TaylorSequence.Skip(1)) {
                ddouble dy = f * w;
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                w *= v;
                y = y_next;
            }

            return c * y * r;
        }

        public static ddouble Pow(ddouble x, ddouble y) {
            if (x.Sign < 0) {
                return NaN;
            }

            if (IsZero(y)) {
                return IsNaN(x) ? NaN : 1;
            }

            ddouble z = Pow2(y * Log2(x));

            return z;
        }

        public static ddouble Pow10(ddouble x) {
            ddouble z = RoundMantissa(Pow2(Abs(x) * Consts.Log.Lb10), 100);

            return x.Sign >= 0 ? z : Rcp(z);
        }

        public static ddouble Exp(ddouble x) {
            ddouble z = Pow2(x * Consts.Log.LbE);

            return z;
        }

        public static ddouble Expm1(ddouble x) {
            if (x < -0.25d || x > 0.25d) {
                return Exp(x) - 1;
            }
            if (IsPlusZero(x)) {
                return PlusZero;
            }
            if (IsMinusZero(x)) {
                return MinusZero;
            }

            ddouble z = x;
            ddouble y = 0;

            foreach (ddouble f in TaylorSequence.Skip(1)) {
                ddouble dy = f * z;
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                z *= x;
                y = y_next;
            }

            return y;
        }

        private static partial class Consts {
            public static class Pow {

                public static readonly IReadOnlyList<ddouble> Pow2Table = GeneratePow2Table();

                public static readonly ddouble Pow2TableDx = Rcp(Pow2Table.Count - 1);

                public static readonly int Pow2TableN = Pow2Table.Count - 1;

                public static ddouble[] GeneratePow2Table() {
                    const int n = 2048;
                    ddouble dx = Rcp(n);
                    ddouble[] table = new ddouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        ddouble x = dx * i;
                        table[i] = Pow2Prime(x);
                    }

                    return table;
                }

                private static ddouble Pow2Prime(ddouble x) {
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x == 1d) {
                        return 2;
                    }

                    ddouble v = x * Consts.Log.Ln2;

                    Accumulator y = 1d;
                    ddouble w = v;

                    foreach (ddouble f in TaylorSequence.Skip(1)) {
                        y += f * w;

                        if (y.IsConvergence) {
                            break;
                        }

                        w *= v;
                    }

                    return y.Sum;
                }
            }
        }
    }
}
