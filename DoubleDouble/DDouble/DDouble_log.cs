using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return NegativeInfinity;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int n, ddouble v) = Frexp(x);

            int index = (int)Floor((v - 1d) * Consts.Log.Log2TableN);
            ddouble v_offset = 1d + Consts.Log.Log2TableDx * index;

            ddouble u = v / v_offset;

            ddouble sc = 131d + u * (1281d + u * (1881d + u * (481d + u * 6d)));
            ddouble sd = 30d + u * (600d + u * (1800d + u * (1200d + u * 150d)));

            ddouble w = (u - 1d) * sc / (sd * Ln2);

            ddouble y = n + Consts.Log.Log2Table[index] + w;

            return y;
        }

        public static ddouble Log10(ddouble x) {
            return RoundMantissa(Log2(x) * Lg2, keep_bits: 103);
        }

        public static ddouble Log(ddouble x) {
            return Log2(x) * Ln2;
        }

        public static ddouble Log1p(ddouble x) {
            if (!(x >= -0.0625d) || x > 0.0625d) {
                return Log(1d + x);
            }
            if (IsPlusZero(x)) {
                return PlusZero;
            }
            if (IsMinusZero(x)) {
                return MinusZero;
            }

            ddouble sc = 116396280d + x * (493152660d + x * (865824960d + x * (814143330d + x * (442197756d
                       + x * (139339200d + x * (24195600d + x * (2031975d + x * (59950d + x * 126d))))))));
            ddouble sd = 116396280d + x * (551350800d + x * (1102701600d + x * (1210809600d + x * (794593800d
                       + x * (317837520d + x * (75675600d + x * (9979200d + x * (623700d + x * 12600d))))))));

            ddouble y = x * sc / sd;

            return y;
        }

        internal static partial class Consts {
            public static class Log {
                public const int Log2TableN = 2048;

                public static readonly IReadOnlyList<ddouble> Log2Table = GenerateLog2Table();

                public static readonly ddouble Log2TableDx = Rcp(Log2TableN);

                public static ddouble[] GenerateLog2Table() {
#if DEBUG
                    Trace.WriteLine($"Log2 initialize.");
#endif

                    ddouble dx = Rcp(Log2TableN);
                    ddouble[] table = new ddouble[Log2TableN + 1];

                    for (int i = 0; i <= Log2TableN; i++) {
                        ddouble x = 1 + dx * i;
                        table[i] = Log2Prime(x);
                    }

                    return table;
                }

                private static ddouble Log2Prime(ddouble x) {
                    if (!(x >= 1d) || x > 2d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x == 2d) {
                        return 1d;
                    }

                    ddouble y = 0d;
                    ddouble p = Point5;

                    for (int i = 128; i > 0; i--) {
                        x *= x;

                        if (x >= 2) {
                            y += p;
                            x /= 2;
                        }
                        p /= 2;

                        if (y == (y + p) || x == 1) {
                            break;
                        }
                    }

                    return y;
                }
            }
        }
    }
}
