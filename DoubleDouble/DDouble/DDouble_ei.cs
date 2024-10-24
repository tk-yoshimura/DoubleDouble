﻿using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Ei;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Ei(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsInfinity(x)) {
                return IsPositive(x) ? PositiveInfinity : 0d;
            }

            if (x <= PadeApproxMin || x >= PadeApproxMax) {
                ddouble g = EiPade.Coef(x) + x;
                ddouble y = Exp(x) / g;

                return y;
            }

            if (x == 0d) {
                return NegativeInfinity;
            }

            if (IsPositive(x)) {
                return EiNearZero.Positive(x);
            }
            else {
                return EiNearZero.Negative(x);
            }
        }

        public static ddouble Ein(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsInfinity(x)) {
                return IsPositive(x) ? PositiveInfinity : NegativeInfinity;
            }

            if (x <= PadeApproxMin || x >= PadeApproxMax) {
                ddouble g = EiPade.Coef(-x) - x;
                ddouble ei = Exp(-x) / g;

                ddouble y = IsPositive(x) ? (EulerGamma + Log(x) - ei) : (EulerGamma + Log(-x) - ei);

                return y;
            }
            if (x == 0d) {
                return 0d;
            }

            if (IsPositive(x)) {
                return -EiNearZero.Negative(-x, offset: false);
            }
            else {
                return -EiNearZero.Positive(-x, offset: false);
            }
        }

        public static ddouble Li(ddouble x) {
            return Ei(Log(x));
        }

        internal static class EiNearZero {
            public static ddouble Positive(ddouble x, bool offset = true, int max_terms = 12) {
                ddouble x2 = x * x;

                ddouble s = (offset) ? (EulerGamma + Log(x)) : 0d;
                ddouble u = x * Exp(Ldexp(x, -1));

                for (int k = 0; k < max_terms; k++) {
                    s = SeriesUtil.Add(s,
                        Ldexp(u * TaylorSequence[2 * k + 1], -2 * k) * FSeries.Value(k),
                        1d, -x * K4Series.Value(k), out bool convergence
                    );

                    if (convergence) {
                        break;
                    }

                    u *= x2;
                }

                return s;
            }

            public static ddouble Negative(ddouble x, bool offset = true, int max_terms = 12) {
                Debug.Assert(x <= 0d, nameof(x));

                ddouble x2 = x * x;

                ddouble s = (offset) ? (EulerGamma + Log(-x)) : 0d;
                ddouble u = x;

                for (int k = 0; k < max_terms; k++) {
                    (ddouble r1, ddouble r2) = NRcpSeries.Value(k);

                    s = SeriesUtil.Add(s, u * TaylorSequence[2 * k + 1], r1, x * r2, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= x2;
                }

                return s;
            }

            internal class FSeries {
                private static ddouble v = 0d;
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        v += Rcp((2 * k + 1));
                        table.Add(v);
                    }

                    return table[n];
                }
            }

            internal class K4Series {
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        table.Add(Rcp((4 * k + 4)));
                    }

                    return table[n];
                }
            }

            internal class NRcpSeries {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble r1, ddouble r2) Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble r1 = Rcp((2 * k + 1));
                        ddouble r2 = Rcp(((2 * k + 2) * (2 * k + 2)));
                        table.Add((r1, r2));
                    }

                    return table[n];
                }
            }
        }

        internal static class EiPade {
            public static ddouble Coef(ddouble x) {
                ddouble v = 1d / x;

                int table_index = SegmentIndex(v);
                ddouble w = v - PadeCenterTable[table_index];
                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTables[table_index];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Ei x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            private static int SegmentIndex(ddouble x) {
                if (PadeThresholdTable[0] >= x) {
                    return 0;
                }
                if (PadeThresholdTable[^1] <= x) {
                    return PadeThresholdTable.Count - 1;
                }

                int index = 0;

                for (int h = int.Max(1, PadeThresholdTable.Count / 2); h >= 1; h /= 2) {
                    for (int i = index; i < PadeThresholdTable.Count - h; i += h) {
                        if (PadeThresholdTable[i + h] > x) {
                            index = i;
                            break;
                        }
                    }
                }

                return index;
            }
        }

        internal static partial class Consts {
            public static class Ei {
                public static readonly ddouble PadeApproxMin = -1 / 2d, PadeApproxMax = 1 / 2d;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<double> PadeCenterTable, PadeThresholdTable;

                static Ei() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.EiTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeXM2Table"],
                        tables["PadeXM1p75Table"],
                        tables["PadeXM1p5Table"],
                        tables["PadeXM1p25Table"],
                        tables["PadeXM1Table"],
                        tables["PadeXM0p75Table"],
                        tables["PadeXM0p5Table"],
                        tables["PadeXM0p375Table"],
                        tables["PadeXM0p25Table"],
                        tables["PadeXM0p1875Table"],
                        tables["PadeXM0p125Table"],
                        tables["PadeXM0p0625Table"],
                        tables["PadeXM0p03125Table"],
                        tables["PadeXM0p015625Table"],
                        tables["PadeXZeroTable"],
                        tables["PadeXP0p0078125Table"],
                        tables["PadeXP0p01171875Table"],
                        tables["PadeXP0p015625Table"],
                        tables["PadeXP0p01953125Table"],
                        tables["PadeXP0p0234375Table"],
                        tables["PadeXP0p02734375Table"],
                        tables["PadeXP0p03125Table"],
                        tables["PadeXP0p03515625Table"],
                        tables["PadeXP0p0390625Table"],
                        tables["PadeXP0p04296875Table"],
                        tables["PadeXP0p046875Table"],
                        tables["PadeXP0p0546875Table"],
                        tables["PadeXP0p0625Table"],
                        tables["PadeXP0p0703125Table"],
                        tables["PadeXP0p078125Table"],
                        tables["PadeXP0p0859375Table"],
                        tables["PadeXP0p09375Table"],
                        tables["PadeXP0p109375Table"],
                        tables["PadeXP0p125Table"],
                        tables["PadeXP0p140625Table"],
                        tables["PadeXP0p15625Table"],
                        tables["PadeXP0p171875Table"],
                        tables["PadeXP0p1875Table"],
                        tables["PadeXP0p203125Table"],
                        tables["PadeXP0p21875Table"],
                        tables["PadeXP0p234375Table"],
                        tables["PadeXP0p25Table"],
                        tables["PadeXP0p28125Table"],
                        tables["PadeXP0p3125Table"],
                        tables["PadeXP0p375Table"],
                        tables["PadeXP0p4375Table"],
                        tables["PadeXP0p5Table"],
                        tables["PadeXP0p5625Table"],
                        tables["PadeXP0p625Table"],
                        tables["PadeXP0p6875Table"],
                        tables["PadeXP0p75Table"],
                        tables["PadeXP0p875Table"],
                        tables["PadeXP1Table"],
                        tables["PadeXP1p125Table"],
                        tables["PadeXP1p25Table"],
                        tables["PadeXP1p5Table"],
                        tables["PadeXP1p75Table"],
                        tables["PadeXP2Table"],
                    });

                    PadeCenterTable = new ReadOnlyCollection<double>(new double[]{
                        -2d,
                        -1.75d,
                        -1.5d,
                        -1.25d,
                        -1d,
                        -0.75d,
                        -0.5d,
                        -0.375d,
                        -0.25d,
                        -0.1875d,
                        -0.125d,
                        -0.0625d,
                        -0.03125d,
                        -0.015625d,
                        0d,
                        0.0078125d,
                        0.01171875d,
                        0.015625d,
                        0.01953125d,
                        0.0234375d,
                        0.02734375d,
                        0.03125d,
                        0.03515625d,
                        0.0390625d,
                        0.04296875d,
                        0.046875d,
                        0.0546875d,
                        0.0625d,
                        0.0703125d,
                        0.078125d,
                        0.0859375d,
                        0.09375d,
                        0.109375d,
                        0.125d,
                        0.140625d,
                        0.15625d,
                        0.171875d,
                        0.1875d,
                        0.203125d,
                        0.21875d,
                        0.234375d,
                        0.25d,
                        0.28125d,
                        0.3125d,
                        0.375d,
                        0.4375d,
                        0.5d,
                        0.5625d,
                        0.625d,
                        0.6875d,
                        0.75d,
                        0.875d,
                        1d,
                        1.125d,
                        1.25d,
                        1.5d,
                        1.75d,
                        2d,
                    });

                    double[] threshold = new double[PadeCenterTable.Count];

                    threshold[0] = PadeCenterTable[0];
                    for (int i = 1; i < threshold.Length; i++) {
                        threshold[i] = (PadeCenterTable[i] + PadeCenterTable[i - 1]) / 2;
                    }

                    PadeThresholdTable = Array.AsReadOnly(threshold);
                }
            }
        }
    }
}
