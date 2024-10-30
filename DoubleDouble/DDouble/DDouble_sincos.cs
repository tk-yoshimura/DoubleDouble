using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.SinCos;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble SinPi(ddouble x) {
            return SinPiHalf(Ldexp(x, 1));
        }

        public static ddouble Sin(ddouble x) {
            if (ILogB(x) < EpsExponent) {
                return x;
            }

            return SinPi(x * RcpPi);
        }

        public static ddouble CosPi(ddouble x) {
            return SinPiHalf(Ldexp(Abs(x), 1) + 1d);
        }

        public static ddouble Cos(ddouble x) {
            if (ILogB(x) < EpsExponent) {
                return 1d - x * x * 0.5d;
            }

            return CosPi(x * RcpPi);
        }

        public static ddouble TanPi(ddouble x) {
            if (IsNegative(x)) {
                return -TanPi(-x);
            }

            ddouble s = x - Floor(x);

            if (s <= 0.25d) {
                ddouble sn = SinPi(s), cn = Sqrt(1d - sn * sn);
                return sn / cn;
            }
            else if (s <= 0.75d) {
                ddouble cn = CosPi(s), sn = Sqrt(1d - cn * cn);
                return sn / cn;
            }
            else {
                ddouble sn = SinPi(s), cn = -Sqrt(1d - sn * sn);
                return sn / cn;
            }
        }

        public static ddouble Tan(ddouble x) {
            if (ILogB(x) < EpsExponent) {
                return x;
            }

            return TanPi(x * RcpPi);
        }

        internal static ddouble SinPiHalf(ddouble x) {
            if (!IsFinite(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -SinPiHalf(-x);
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
            if (IsZero(x)) {
                return PlusZero;
            }

            Debug.Assert((x >= 0d && x <= 1d), nameof(x));

            int index = (((int)Floor(Ldexp(x, SinPiHalfLevel + 1))) + 1) >> 1;
            ddouble v = x - Ldexp(index, -SinPiHalfLevel);
            ddouble sna = SinPiHalfTable[index];
            ddouble cna = SinPiHalfTable[SinPiHalfTableN - index];

            ddouble u = v * PiHalf, u2 = u * u;

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
                public static readonly ddouble PiHalf = Ldexp(Pi, -1);

                public const int SinPiHalfLevel = 10;
                public const int SinPiHalfTableN = 1 << SinPiHalfLevel;
                public const int EpsExponent = -52;

                public static readonly ReadOnlyCollection<ddouble> SinPiHalfTable = GenerateSinPiTable();

                public static ReadOnlyCollection<ddouble> GenerateSinPiTable() {
                    Debug.WriteLine($"SinCos initialize.");

                    ddouble[] table = new ddouble[SinPiHalfTableN + 1];

                    for (int i = 0; i <= SinPiHalfTableN; i++) {
                        ddouble x = Ldexp(i, -SinPiHalfLevel);
                        table[i] = SinPiHalfPrime(x);
                    }

                    return Array.AsReadOnly(table);
                }

                private static ddouble SinPiHalfPrime(ddouble x) {
                    Debug.Assert((x >= 0d && x <= 1d), nameof(x));

                    if (x == 0.5d) {
                        return Ldexp(Sqrt(2), -1);
                    }

                    if (x < 0.5d) {
                        ddouble w = x * PiHalf, w2 = w * w, w4 = w2 * w2, u = 1;
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
                        ddouble w = (x - 1d) * PiHalf, w2 = w * w, w4 = w2 * w2, u = w2;
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
