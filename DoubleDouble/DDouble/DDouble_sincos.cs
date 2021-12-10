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
                ddouble sn = SinPI(s), cn = Sqrt(1d - sn * sn);
                return sn / cn;
            }
            else if (s <= 0.75d) {
                ddouble cn = CosPI(s), sn = Sqrt(1d - cn * cn);
                return sn / cn;
            }
            else {
                ddouble sn = SinPI(s), cn = -Sqrt(1d - sn * sn);
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
            ddouble v = s - Consts.SinCos.SinPIHalfTableDx * index;
            ddouble sna = Consts.SinCos.SinPIHalfTable[index];
            ddouble cna = Consts.SinCos.SinPIHalfTable[Consts.SinCos.SinPIHalfTableN - index];

            ddouble w = Ldexp(v * PI, -1), w2 = w * w, w4 = w2 * w2, u = 1;
            ddouble y = ddouble.Zero;

            for (int i = 0, terms = 0; terms < Consts.SinCos.SinPIHalfConvergenceTerms; i += 4, terms++) {
                ddouble f = TaylorSequence[i + 3];
                ddouble dy = u * f * ((i + 2) * (i + 3) - w2);

                y += dy;
                u *= w4;
            }

            ddouble snb = w * y, cnb = Sqrt(1 - snb * snb);

            ddouble z = sna * cnb + cna * snb;

            return sign > 0 ? z : -z;
        }

        private static partial class Consts {
            public static class SinCos {

                public static readonly IReadOnlyList<ddouble> SinPIHalfTable = GenerateSinPITable();

                public static readonly ddouble SinPIHalfTableDx = Rcp(SinPIHalfTable.Count - 1);

                public static readonly int SinPIHalfTableN = SinPIHalfTable.Count - 1;

                public static int SinPIHalfConvergenceTerms = SinPIHalfPrime(SinPIHalfTableDx).terms;

                public static readonly ddouble RcpPI = Rcp(PI);

                public static ddouble[] GenerateSinPITable() {
#if DEBUG
                    Trace.WriteLine($"SinCos initialize.");
#endif

                    const int n = 256;
                    ddouble dx = Rcp(n);
                    ddouble[] table = new ddouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        ddouble x = dx * i;
                        table[i] = SinPIHalfPrime(x).value;
                    }

                    return table;
                }

                private static (ddouble value, int terms) SinPIHalfPrime(ddouble x) {
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x < 0.5d) {
                        ddouble w = Ldexp(x * PI, -1), w2 = w * w, w4 = w2 * w2, u = 1;
                        ddouble y = ddouble.Zero;

                        int terms = 0;
                        for (int i = 0, n = TaylorSequence.Count - 3; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 3];
                            ddouble dy = u * f * ((i + 2) * (i + 3) - w2);
                            ddouble y_next = y + dy;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            y = y_next;
                            terms++;
                        }

                        return (w * y, terms);
                    }
                    else {
                        ddouble w = Ldexp((x - 1d) * PI, -1), w2 = w * w, w4 = w2 * w2, u = w2;
                        ddouble y = 1d;

                        int terms = 0;
                        for (int i = 0, n = TaylorSequence.Count - 4; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 4];
                            ddouble dy = u * f * ((i + 3) * (i + 4) - w2);
                            ddouble y_next = y - dy;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            y = y_next;
                            terms++;
                        }

                        return (y, terms);
                    }
                }
            }
        }
    }
}
