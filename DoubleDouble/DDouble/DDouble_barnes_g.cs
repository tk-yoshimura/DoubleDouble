using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.BarnesG;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble BarnesG(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (x >= ExtremeLarge) {
                return PositiveInfinity;
            }

            x = TruncateMantissa(x, 105);

            if (x <= 0d && IsInteger(x)) {
                return Zero;
            }
            if (x < ExtremeMinusLarge) {
                return NaN;
            }

            if (x >= PolyXMin && x < PadeXMax) {
                return BarnesGUtil.SeriesValue(x);
            }

            ddouble x_p25 = x - PolyXMin;

            if (x < PolyXMin) {
                int m = int.Min(-1, (int)Floor(x_p25));

                ddouble f = x - m;

                ddouble y = BarnesGUtil.SeriesValue(f);
                ddouble g = Gamma(f);

                for (int k = -1; k >= m; k--) {
                    g /= f + k;
                    y /= g;
                }

                return y;
            }
            else {
                int m = (int)Floor(x_p25);

                ddouble f = x - m;

                ddouble y = BarnesGUtil.SeriesValue(f + (PadeXMax - 1d));
                ddouble g = Gamma(f + (PadeXMax - 2d));

                for (int k = 2; k < m - 1; k++) {
                    g *= f + k;
                    y *= g;
                }

                return y;
            }
        }

        public static ddouble LogBarnesG(ddouble x) {
            if (IsNaN(x) || IsNegative(x)) {
                return NaN;
            }
            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }


            if (x >= LogPolyXMin && x <= LogPolyXMax) {
                return BarnesGUtil.LogSeriesValue(x);
            }

            if (x < LogPolyXMin) {
                int m = (int)Floor(x);

                ddouble f = x - m;

                ddouble y = BarnesGUtil.LogSeriesValue(f + 2d);

                for (int k = 1; k >= m; k--) {
                    y -= LogGamma(k + f);
                }

                return y;
            }
            else if (x < LogSterlingXMin) {
                int m = (int)Floor(x);

                ddouble f = x - m;

                ddouble y = BarnesGUtil.LogSeriesValue(f + 2d);
                ddouble d = 1d, g = Gamma(f + 1d);

                for (int k = 1; k < m - 1; k++) {
                    g *= f + k;
                    d *= g;
                }

                y += Log(d);

                return y;
            }
            else {
                return BarnesGUtil.LogSterlingValue(x);
            }
        }

        internal static class BarnesGUtil {
            public static ddouble SeriesValue(ddouble x) {
                Debug.Assert(x >= PolyXMin && x <= PadeXMax, nameof(x));

                if (IsPositive(x)) {
                    int n = int.Min((int)Floor(x), PadeTables.Count - 1);

                    ddouble v = x - n;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTables[n];

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    Debug.Assert(sd > 0.5d, $"[BarnesG x={x}] Too small pade denom!!");

                    ddouble y = sc / sd;

                    if (n == 0) {
                        y *= v;
                    }

                    return y;
                }
                else {
                    ddouble v = -x;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = PolyXNegativeTable;

                    ddouble s = table[0].c + v * table[0].d;
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        s = c + v * (d + v * s);
                    }

                    ddouble y = s * v;

                    return y;
                }
            }

            public static ddouble LogSeriesValue(ddouble x) {
                Debug.Assert(x >= LogPolyXMin && x <= LogPolyXMax, nameof(x));

                int table_index;
                bool is_poly;
                ddouble v;

                if (x < 2.25d) {
                    table_index = 0;
                    is_poly = true;
                    v = x - 2d;
                }
                else if (x < 2.50d) {
                    table_index = 1;
                    is_poly = false;
                    v = x - 2.25d;
                }
                else if (x < 2.75d) {
                    table_index = 2;
                    is_poly = false;
                    v = x - 2.50d;
                }
                else {
                    table_index = 3;
                    is_poly = true;
                    v = x - 3d;
                }

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LogTables[table_index];

                if (is_poly) {
                    ddouble s = table[0].c + v * table[0].d;
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        s = c + v * (d + v * s);
                    }

                    ddouble y = s * v;

                    return y;
                }
                else {
                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    Debug.Assert(sd > 0.5d, $"[LogBarnesG x={x}] Too small pade denom!!");

                    ddouble y = sc / sd;

                    return y;
                }
            }

            public static ddouble LogSterlingValue(ddouble x) {
                x -= 1d;

                ddouble lnx = Log(x);

                ddouble c = LogBias - lnx * Rcp12 + x * (Consts.LogGamma.LogPI2Half + x * Ldexp((Ldexp(lnx, 1) - 3d), -2));

                ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v2;

                ddouble y = c;

                foreach ((ddouble s, ddouble r) in SterlingTable) {
                    y = SeriesUtil.Add(y, u * s, 1d, -v2 * r, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= v4;
                }

                return y;
            }
        }

        internal static partial class Consts {
            public static class BarnesG {
                public const double ExtremeLarge = 28.5d, ExtremeMinusLarge = -64d;
                public const double PolyXMin = -0.25d, PadeXMax = 4d;
                public const double LogPolyXMin = 1.75d, LogPolyXMax = 3.25d, LogSterlingXMin = 12d, LogPolyRange = 0.5d;

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> SterlingTable, PolyXNegativeTable;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> LogTables;
                public static readonly ddouble Rcp12 = 1.0 / (ddouble)12;
                public static readonly ddouble LogBias = (-1, -3, 0xA96429090A9A04E6uL, 0xF323011BFAF4EC40uL);

                static BarnesG() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.BarnesGTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());
                    PolyXNegativeTable = Array.AsReadOnly(tables[nameof(PolyXNegativeTable)].ToArray());

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX0Table"],
                        tables["PadeX1Table"],
                        tables["PadeX2Table"],
                        tables["PadeX3Table"],
                    });

                    LogTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["LogPolyX2Table"],
                        tables["LogPadeX2p25Table"],
                        tables["LogPadeX2p50Table"],
                        tables["LogPolyX3Table"],
                    });
                }
            }
        }
    }
}