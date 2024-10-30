using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Log;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble x) {
            if (IsNegative(x) || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return NegativeInfinity;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int n, ddouble v) = Frexp(x);

            int index = (int)Floor(Ldexp(v - 1d, Log2Level));
            ddouble v_offset = 1d + Ldexp(index, -Log2Level);

            ddouble u = v / v_offset;

            ddouble sc = 131d + u * (1281d + u * (1881d + u * (481d + u * 6d)));
            ddouble sd = 30d + u * (600d + u * (1800d + u * (1200d + u * 150d)));

            ddouble w = (u - 1d) * (sc * LbE) / sd;

            ddouble y = n + Log2Table[index] + w;

            return y;
        }

        public static ddouble Log10(ddouble x) {
            ddouble y = Log2(x) * Lg2;

            if (IsFinite(y)) {
                int n = (int)Round(y);

                if (ILogB(y - n) < LogBaseRoundingExponent) {
                    y = TruncateMantissa(y, Log10TruncationBits);
                }
            }

            return y;
        }

        public static ddouble Log(ddouble x) {
            return Log2(x) * Ln2;
        }

        public static ddouble Log(ddouble x, ddouble b) {
            if (b != LogCache.B) {
                LogCache.B = b;
                LogCache.RcpLbB = Rcp(Log2(b));
            }

            ddouble y = Log2(x) * LogCache.RcpLbB;

            if (IsFinite(y) && ILogB(y) < 10) {
                int n = (int)Round(y);

                if (ILogB(y - n) < LogBaseRoundingExponent) {
                    y = TruncateMantissa(y, LogBaseTruncationBits);
                }
            }

            return y;
        }

        public static ddouble Log1p(ddouble x) {
            if (!(x >= -0.0625d) || x > 0.0625d) {
                return Log(1d + x);
            }
            if (IsPlusZero(x)) {
                return 0d;
            }
            if (IsMinusZero(x)) {
                return -0d;
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
                public const int Log2Level = 11;
                public const int Log2TableN = 1 << Log2Level;

                public const int LogBaseRoundingExponent = -101;
                public const int LogBaseTruncationBits = 103;
                public const int Log10TruncationBits = 104;

                public static readonly ReadOnlyCollection<ddouble> Log2Table = GenerateLog2Table();

                public static ReadOnlyCollection<ddouble> GenerateLog2Table() {
                    Debug.WriteLine($"Log2 initialize.");

                    ddouble[] table = new ddouble[Log2TableN + 1];

                    for (int i = 0; i <= Log2TableN; i++) {
                        ddouble x = Ldexp(i, -Log2Level);
                        table[i] = Log2Prime(x);
                    }

                    return Array.AsReadOnly(table);
                }

                private static ddouble Log2Prime(ddouble x) {
                    Debug.Assert((x >= 0d && x <= 1d), nameof(x));

                    if (x == 1d) {
                        return 1d;
                    }

                    ddouble y =
                        x * (5601466d * (2d + x)
                            * (1457411875920d
                        + x * (14574118759200d
                        + x * (67391177053200d
                        + x * (191158208841600d
                        + x * (372088783910844d
                        + x * (526767014565792d
                        + x * (560902167361008d
                        + x * (458229599616096d
                        + x * (290406718261352d
                        + x * (143437273906080d
                        + x * (55176711912336d
                        + x * (16438885357888d
                        + x * (3753218392581d
                        + x * (646000506372d
                        + x * (81863831406d
                        + x * (7386067164d
                        + x * (110109050232090d
                        + x * (4254540825080d
                        + x * (92421446460d
                        + x * (913741080d
                        + x * (2490417d
                        ))))) / 243542d
                        )))))))))))))))))
                        / (7759752d
                            * (2104098963720d
                        + x * (23145088600920d
                        + x * (118685861314020d
                        + x * (376780512108000d
                        + x * (829376615067000d
                        + x * (1343590116408540d
                        + x * (1659391212145590d
                        + x * (1597008083869440d
                        + x * (1213941955644000d
                        + x * (734359948476000d
                        + x * (354590946549840d
                        + x * (136526995463040d
                        + x * (41716581947040d
                        + x * (10028024506500d
                        + x * (1871589827250d
                        + x * (266181664320d
                        + x * (28109701620d
                        + x * (2125943820d
                        + x * (109359250d
                        + x * (3542000d
                        + x * (63756d
                        + x * (506d
                        + x
                        )))))))))))))))))))))));

                    y *= LbE;

                    return y;
                }
            }
        }

        internal static class LogCache {
            public static ddouble B = NaN, RcpLbB = NaN;
        }
    }
}
