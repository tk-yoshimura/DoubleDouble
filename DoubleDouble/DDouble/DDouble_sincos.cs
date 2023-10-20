using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.SinCos;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble SinPI(ddouble x) {
            return SinPIHalf(Ldexp(x, 1));
        }

        public static ddouble Sin(ddouble x) {
            return SinPI(x * RcpPI);
        }

        public static ddouble CosPI(ddouble x) {
            return SinPIHalf(Ldexp(x, 1) + 1d);
        }

        public static ddouble Cos(ddouble x) {
            return CosPI(x * RcpPI);
        }

        public static ddouble TanPI(ddouble x) {
            if (IsNegative(x)) {
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
            if (IsNegative(x)) {
                return -SinPIHalf(-x);
            }

            if (x >= 4d) {
                x -= Ldexp(Floor(Ldexp(x, -2)), 2);
                if (IsNegative(x)) {
                    x += 4d;
                }
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

            Debug.Assert((x >= 0d && x <= 1d), nameof(x));

            int index = (int)Round(x * SinPIHalfTableN);
            ddouble v = x - SinPIHalfTableDx * index;
            ddouble sna = SinPIHalfTable[index];
            ddouble cna = SinPIHalfTable[SinPIHalfTableN - index];

            ddouble u = v * PIHalf, u2 = u * u;

            ddouble ssc = 166320d + u2 * (-22260d + u2 * 551d);
            ddouble ssd = 166320d + u2 * (5460d + u2 * 75d);

            ddouble scc = 15120d + u2 * (-6900d + u2 * 313d);
            ddouble scd = 15120d + u2 * (660d + u2 * 13d);

            ddouble snb = u * ssc / ssd, cnb = scc / scd;

            ddouble y = sna * cnb + cna * snb;

            return sign > 0 ? y : -y;
        }

        internal static partial class Consts {
            public static class SinCos {
                public static readonly ddouble PIHalf = Ldexp(PI, -1);

                public const int SinPIHalfTableN = 1024;

                public static readonly IReadOnlyList<ddouble> SinPIHalfTable = GenerateSinPITable();

                public static readonly ddouble SinPIHalfTableDx = Rcp(SinPIHalfTableN);

                public static ddouble[] GenerateSinPITable() {
                    Debug.WriteLine($"SinCos initialize.");

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

                    if (x == 0.5d) {
                        return Ldexp(Sqrt(2), -1);
                    }

                    if (x < 0.5d) {
                        ddouble w = x * PIHalf, w2 = w * w, w4 = w2 * w2, u = 1;
                        ddouble y = 0d, c = 0d;

                        for (int i = 0, n = TaylorSequence.Count - 3; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 3];
                            ddouble dy = u * f * ((i + 2) * (i + 3) - w2);
                            ddouble d = dy - c;
                            ddouble y_next = y + d;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            c = (y_next - y) - d;
                            y = y_next;
                        }

                        return w * y;
                    }
                    else {
                        ddouble w = (x - 1d) * PIHalf, w2 = w * w, w4 = w2 * w2, u = w2;
                        ddouble y = 1d, c = 0d;

                        for (int i = 0, n = TaylorSequence.Count - 4; i < n; i += 4) {
                            ddouble f = TaylorSequence[i + 4];
                            ddouble dy = u * f * (w2 - (i + 3) * (i + 4));
                            ddouble d = dy - c;
                            ddouble y_next = y + d;

                            if (y == y_next) {
                                break;
                            }

                            u *= w4;
                            c = (y_next - y) - d;
                            y = y_next;
                        }

                        return y;
                    }
                }
            }
        }
    }
}
