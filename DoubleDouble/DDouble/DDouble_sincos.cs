using System;
using System.Collections.Generic;

namespace DoubleDouble {
    public partial struct ddouble {
        internal static ddouble SinPIHalf(ddouble x) { 
            if (!IsFinite(x)) {
                return NaN;
            }
            if (x.Sign < 0) {
                return -SinPIHalf(-x);
            }
            if (x >= 4d) {
                x %= 4d;
            }

            int sign = 1;
            if (x >= 2d) {
                sign = -1;
                x = 4d - x;
            }
            if (x >= 1d) {
                x = 2d - x;
            }
            if (x == 1d) {
                return sign;
            }

            ddouble s = x - Floor(x);

            int index = (int)ddouble.Floor(s * Consts.Sin.SinPIHalfTableN);
            ddouble v = (s - Consts.Sin.SinPIHalfTableDx * index) * PI;
            ddouble sna = Consts.Sin.SinPIHalfTable[index];
            ddouble cna = Consts.Sin.SinPIHalfTable[Consts.Sin.SinPIHalfTableN - index];

            ddouble w = v * v, u = w;
            ddouble y = Ldexp(1d, -1);

            for (int i = 3, n = TaylorSequence.Count - 1; i < n; i += 2) {
                ddouble f = TaylorSequence[i];
                ddouble dy = Ldexp(u * f, -i);

                if ((i & 2) > 0) {
                    dy = -dy;
                }

                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= w;
                y = y_next;
            }

            ddouble snb = v * y, cnb = Sqrt(1 - snb * snb);

            ddouble z = sna * cnb + cna * snb;

            return sign > 0 ? z : -z;
        }

        private static partial class Consts {
            public static class Sin {

                public static readonly IReadOnlyList<ddouble> SinPIHalfTable = GenerateSinPITable();

                public static readonly ddouble SinPIHalfTableDx = Rcp(SinPIHalfTable.Count - 1);

                public static readonly int SinPIHalfTableN = SinPIHalfTable.Count - 1;

                public static ddouble[] GenerateSinPITable() {
                    const int n = 2048;
                    ddouble dx = Rcp(n);
                    ddouble[] table = new ddouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        ddouble x = dx * i;
                        table[i] = SinPIPrime(x);
                    }

                    return table;
                }

                private static ddouble SinPIPrime(ddouble x) {
                    if (!(x >= 0) || x > 1) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x < 0.5d) {
                        ddouble v = x * PI, w = v * v, u = w;
                        KahanSum y = Ldexp(1d, -1);

                        for (int i = 3, n = TaylorSequence.Count - 1; i < n; i += 2) {
                            ddouble f = TaylorSequence[i];
                            ddouble dy = Ldexp(u * f, -i);

                            if ((i & 2) > 0) {
                                dy = -dy;
                            }

                            y.Add(dy);

                            if (y.IsConvergence) {
                                break;
                            }

                            u *= w;
                        }

                        return v * y.Sum;
                    }
                    else {
                        ddouble v = (x - 1) * PI, w = v * v, r = w;
                        KahanSum y = (ddouble)1;

                        for (int i = 2, n = TaylorSequence.Count - 1; i < n; i += 2) {
                            ddouble f = TaylorSequence[i];
                            ddouble dy = Ldexp(r * f, -i);

                            if ((i & 2) > 0) {
                                dy = -dy;
                            }

                            y.Add(dy);

                            if (y.IsConvergence) {
                                break;
                            }

                            r *= w;
                        }

                        return y.Sum;
                    }
                }
            }
        }
    }
}
