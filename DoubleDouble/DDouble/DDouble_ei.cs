using DoubleDouble.Utils;
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
            if (IsFinite(x) && x >= 2d) {
                ddouble lnx = Log(x);
                ddouble g = EiPade.Coef(lnx) + lnx;
                ddouble y = x / g;

                return y;
            }

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
                private static readonly List<ddouble> table = [];

                public static ddouble Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    lock (table) {
                        for (int k = table.Count; k <= n; k++) {
                            v += Rcp((2 * k + 1));
                            table.Add(v);
                        }

                        return table[n];
                    }
                }
            }

            internal class K4Series {
                private static readonly List<ddouble> table = [];

                public static ddouble Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    lock (table) {
                        for (int k = table.Count; k <= n; k++) {
                            table.Add(Rcp((4 * k + 4)));
                        }

                        return table[n];
                    }
                }
            }

            internal class NRcpSeries {
                private static readonly List<(ddouble, ddouble)> table = [];

                public static (ddouble r1, ddouble r2) Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    lock (table) {
                        for (int k = table.Count; k <= n; k++) {
                            ddouble r1 = Rcp((2 * k + 1));
                            ddouble r2 = Rcp(((2 * k + 2) * (2 * k + 2)));
                            table.Add((r1, r2));
                        }

                        return table[n];
                    }
                }
            }
        }

        internal static class EiPade {
            public static ddouble Coef(ddouble x) {
                ddouble v = 1d / x;

                ddouble w;
                ReadOnlyCollection<(ddouble c, ddouble d)> table;

                if (v < PadeDirectionThreshold) {
                    int table_index = PadeBackwardTables.Count - 1;
                    for (int i = 0; i < PadeBackwardThresholdTable.Count; i++) {
                        if (v <= PadeBackwardThresholdTable[i]) {
                            table_index = i;
                            break;
                        }
                    }

                    w = PadeBackwardThresholdTable[table_index] - v;
                    table = PadeBackwardTables[table_index];
                }
                else {
                    int table_index = PadeForwardTables.Count - 1;
                    for (int i = PadeForwardThresholdTable.Count - 1; i >= 0; i--) {
                        if (v >= PadeForwardThresholdTable[i]) {
                            table_index = i;
                            break;
                        }
                    }

                    w = v - PadeForwardThresholdTable[table_index];
                    table = PadeForwardTables[table_index];
                }

                Debug.Assert(w >= 0d);

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Ei x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }
        }

        internal static partial class Consts {
            public static class Ei {
                public const double PadeApproxMin = -0.5d, PadeApproxMax = 0.5d, PadeDirectionThreshold = 0.03125d;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeBackwardTables, PadeForwardTables;
                public static readonly ReadOnlyCollection<double> PadeBackwardThresholdTable, PadeForwardThresholdTable;

                static Ei() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.EiTable, reverse: true);

                    PadeBackwardTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeXM1to2Table"],
                        tables["PadeXM0p5to1Table"],
                        tables["PadeXM0p25to0p5Table"],
                        tables["PadeXM0to0p25Table"],
                        tables["PadeXP0p015625to0Table"],
                        tables["PadeXP0p0234375to0p015625Table"],
                        tables["PadeXP0p03125to0p0234375Table"],
                    });

                    PadeForwardTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeXP0p03125to0p0625Table"],
                        tables["PadeXP0p0625to0p125Table"],
                        tables["PadeXP0p125to0p25Table"],
                        tables["PadeXP0p25to0p5Table"],
                        tables["PadeXP0p5to1Table"],
                        tables["PadeXP1to1p5Table"],
                        tables["PadeXP1p5to2Table"],
                    });

                    PadeBackwardThresholdTable = new ReadOnlyCollection<double>(new double[]{
                        -1,
                        -0.5,
                        -0.25,
                        -0,
                        0.015625,
                        0.0234375,
                        0.03125,
                    });

                    PadeForwardThresholdTable = new ReadOnlyCollection<double>(new double[]{
                        0.03125,
                        0.0625,
                        0.125,
                        0.25,
                        0.5,
                        1,
                        1.5,
                    });
                }
            }
        }
    }
}
