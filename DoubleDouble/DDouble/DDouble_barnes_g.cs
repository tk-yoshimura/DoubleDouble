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
                return BarnesGUtil.PadeValue(x);
            }

            ddouble x_p25 = x - PolyXMin;

            if (x < PolyXMin) {
                int m = int.Min(-1, (int)Floor(x_p25));

                ddouble f = x - m;

                ddouble y = BarnesGUtil.PadeValue(f);
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

                ddouble y = BarnesGUtil.PadeValue(f + (PadeXMax - 1d));
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

            if (x < LogPadeXMin) {
                return Log(BarnesG(x));
            }

            if (x < LogPadeXMax) {
                return BarnesGUtil.LogPadeValue(x);
            }
            else if (x < LogSterlingXMin) {
                int m = (int)Floor(x);

                ddouble f = x - m;

                ddouble y = BarnesGUtil.LogPadeValue(f + 2d);
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
            public static ddouble PadeValue(ddouble x) {
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

            public static ddouble LogPadeValue(ddouble x) {
                Debug.Assert(x >= LogPadeXMin && x <= LogPadeXMax, nameof(x));

                int n = int.Min((int)Floor(Ldexp(x - LogPadeXMin, 1)), LogPadeTables.Count - 1);

                (double x0, ReadOnlyCollection<(ddouble c, ddouble d)> table) = LogPadeTables[n];
                ddouble v = x - x0;

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
                public const double LogPadeXMin = 0.25d, LogPadeXMax = 3.25d, LogSterlingXMin = 12d;

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> SterlingTable, PolyXNegativeTable;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(double x0, ReadOnlyCollection<(ddouble c, ddouble d)>)> LogPadeTables;
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

                    LogPadeTables = Array.AsReadOnly(new (double x0, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0.25d, tables["LogPadeX0p25Table"]),
                        (1d, tables["LogPadeX0p75Table"]),
                        (1.25d, tables["LogPadeX1p25Table"]),
                        (2d, tables["LogPadeX1p75Table"]),
                        (2.25d, tables["LogPadeX2p25Table"]),
                        (3d, tables["LogPadeX2p75Table"]),
                    });
                }
            }
        }
    }
}