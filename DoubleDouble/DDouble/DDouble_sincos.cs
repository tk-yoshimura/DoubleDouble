using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble SinPI(ddouble x) {
            return SinPIHalf(x * 2);
        }

        public static ddouble Sin(ddouble x) {
            return SinPI(x * RcpPI);
        }

        public static ddouble CosPI(ddouble x) {
            return SinPIHalf(x * 2 + 1d);
        }

        public static ddouble Cos(ddouble x) {
            return CosPI(x * RcpPI);
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
            return TanPI(x * RcpPI);
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

            int index = (int)Floor(s * Consts.SinCos.SinPIHalfTableN);
            ddouble v = s - Consts.SinCos.SinPIHalfTableDx * index;
            ddouble sna = Consts.SinCos.SinPIHalfTable[index];
            ddouble cna = Consts.SinCos.SinPIHalfTable[Consts.SinCos.SinPIHalfTableN - index];

            ddouble u = Ldexp(v * PI, -1), u2 = u * u;

            ddouble sc = 166320d + u2 * (-22260d + u2 * 551d);
            ddouble sd = 166320d + u2 * (  5460d + u2 *  75d);

            ddouble snb = u * sc / sd, cnb = Sqrt(1d - snb * snb);

            ddouble y = sna * cnb + cna * snb;

            return sign > 0 ? y : -y;
        }

        internal static partial class Consts {
            public static class SinCos {
                public const int SinPIHalfTableN = 512;

                public static readonly IReadOnlyList<ddouble> SinPIHalfTable = GenerateSinPITable();

                public static readonly ddouble SinPIHalfTableDx = Rcp(SinPIHalfTableN);

                public static ddouble[] GenerateSinPITable() {
#if DEBUG
                    Trace.WriteLine($"SinCos initialize.");
#endif

                    ddouble dx = Rcp(SinPIHalfTableN);
                    ddouble[] table = new ddouble[SinPIHalfTableN + 1];

                    for (int i = 0; i <= SinPIHalfTableN; i++) {
                        ddouble x = dx * i;
                        table[i] = SinPIHalfPrime(x);
                    }

                    return table;
                }

                private static ddouble SinPIHalfPrime(ddouble x) {
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x < 0.5d) {
                        ddouble w = Ldexp(x * PI, -1), w2 = w * w, w4 = w2 * w2, u = 1;
                        ddouble y = Zero;

                        for (int i = 0, n = TaylorSequence.Count - 3; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 3];
                            ddouble dy = u * f * ((i + 2) * (i + 3) - w2);
                            ddouble y_next = y + dy;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            y = y_next;
                        }

                        return w * y;
                    }
                    else {
                        ddouble w = Ldexp((x - 1d) * PI, -1), w2 = w * w, w4 = w2 * w2, u = w2;
                        ddouble y = 1d;

                        for (int i = 0, n = TaylorSequence.Count - 4; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 4];
                            ddouble dy = u * f * ((i + 3) * (i + 4) - w2);
                            ddouble y_next = y - dy;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            y = y_next;
                        }

                        return y;
                    }
                }
            }
        }
    }
}
