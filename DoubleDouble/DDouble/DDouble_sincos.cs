using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble SinPI(ddouble x) {
            return SinPIHalf(Ldexp(x, 1));
        }

        public static ddouble Sin(ddouble x) {
            return SinPI(x * Consts.SinCos.RcpPI);
        }

        public static ddouble CosPI(ddouble x) {
            return SinPIHalf(Ldexp(x, 1) + 1d);
        }

        public static ddouble Cos(ddouble x) {
            return CosPI(x * Consts.SinCos.RcpPI);
        }

        public static ddouble TanPI(ddouble x) {
            if (x.Sign < 0) {
                return -TanPI(-x);
            }

            ddouble s = x - Floor(x);

            if (s <= 0.25d) {
                ddouble sn = SinPI(s), cn = Sqrt(1 - sn * sn);
                return sn / cn;
            }
            else if (s <= 0.75d) {
                ddouble cn = CosPI(s), sn = Sqrt(1 - cn * cn);
                return sn / cn;
            }
            else {
                ddouble sn = SinPI(s), cn = -Sqrt(1 - sn * sn);
                return sn / cn;
            }
        }

        public static ddouble Tan(ddouble x) {
            return TanPI(x * Consts.SinCos.RcpPI);
        }

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

            int index = (int)ddouble.Floor(s * Consts.SinCos.SinPIHalfTableN);
            ddouble v = (s - Consts.SinCos.SinPIHalfTableDx * index) * PI;
            ddouble sna = Consts.SinCos.SinPIHalfTable[index];
            ddouble cna = Consts.SinCos.SinPIHalfTable[Consts.SinCos.SinPIHalfTableN - index];

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
            public static class SinCos {

                public static readonly IReadOnlyList<ddouble> SinPIHalfTable = GenerateSinPITable();

                public static readonly ddouble SinPIHalfTableDx = Rcp(SinPIHalfTable.Count - 1);

                public static readonly int SinPIHalfTableN = SinPIHalfTable.Count - 1;

                public static readonly ddouble RcpPI = Rcp(PI);

                public static ddouble[] GenerateSinPITable() {
#if DEBUG
                    Trace.WriteLine($"SinCos initialize.");
#endif

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
                    if (!(x >= 0d) || x > 1d) {
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
