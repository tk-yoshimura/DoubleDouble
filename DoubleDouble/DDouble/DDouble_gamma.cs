using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Gamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || x > Consts.Gamma.ExtremeLarge) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble sinpi = SinPI(x);

                if (IsZero(sinpi)) {
                    return NaN;
                }

                ddouble y = PI / (sinpi * Gamma(1d - x));

                return y;
            }

            x = TruncateMantissa(x, 105);

            if (x < Consts.Gamma.PadeWise4X0) {
                int n = int.Clamp((int)Floor(Ldexp(x - 0.5d, 1)), 0, Consts.Gamma.PadeNearZeroTables.Count - 1);
                ddouble v = x - (n + 1) * 0.5d;

                Debug.Assert(v >= 0d && v < 0.5d, $"[Gamma x={x}] Invalid pade v!!");

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Gamma.PadeNearZeroTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Gamma x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }
            else {
                if (IsInteger(x)) {
                    return Factorial[((int)x) - 1];
                }

                int exp, inc;
                ddouble v, bias;
                ReadOnlyCollection<(ddouble c, ddouble d)> table;

                if (x < Consts.Gamma.PadeWise8X0) {
                    int n = int.Clamp((int)Floor(Ldexp(x - Consts.Gamma.PadeWise4X0, -2)), 0, Consts.Gamma.PadeWise4Tables.Count - 1);
                    v = x - Consts.Gamma.PadeWise4X0 - n * 4;
                    (exp, inc, table) = Consts.Gamma.PadeWise4Tables[n];

                    bias = inc * Ldexp(v, -2);

                    Debug.Assert(v >= 0d && v < 4d, $"[Gamma x={x}] Invalid pade v!!");
                }
                else if (x < Consts.Gamma.PadeWise16X0) {
                    int n = int.Clamp((int)Floor(Ldexp(x - Consts.Gamma.PadeWise8X0, -3)), 0, Consts.Gamma.PadeWise8Tables.Count - 1);
                    v = x - Consts.Gamma.PadeWise8X0 - n * 8;
                    (exp, inc, table) = Consts.Gamma.PadeWise8Tables[n];

                    bias = inc * Ldexp(v, -3);

                    Debug.Assert(v >= 0d && v < 8d, $"[Gamma x={x}] Invalid pade v!!");
                }
                else {
                    int n = int.Clamp((int)Floor(Ldexp(x - Consts.Gamma.PadeWise16X0, -4)), 0, Consts.Gamma.PadeWise16Tables.Count - 1);
                    v = x - Consts.Gamma.PadeWise16X0 - n * 16;
                    (exp, inc, table) = Consts.Gamma.PadeWise16Tables[n];

                    bias = inc * Ldexp(v, -4);

                    Debug.Assert(v >= 0d && v < 16d, $"[Gamma x={x}] Invalid pade v!!");
                }

                int exp_bias = (int)Ceiling(bias);
                ddouble b = bias - exp_bias;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Gamma x={x}] Too small pade denom!!");

                ddouble y = Ldexp(Pow2(sc / sd + b), exp + exp_bias);

                return y;
            }
        }

        public static ddouble LogGamma(ddouble x) {
            if (IsNaN(x) || IsNegative(x) || IsMinusZero(x)) {
                return NaN;
            }
            if (IsPlusZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                return Log(Gamma(x));
            }

            if (x < 1d) {
                ddouble v = 1d - x;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.Near2BackwardCoefTable;

                ddouble s = table[0];
                for (int i = 1; i < table.Count; i++) {
                    s = s * v + table[i];
                }

                s *= v;

                ddouble y = s - Log1p(-v);

                return y;
            }

            if (x < 1.5d) {
                ddouble v = x - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[0];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[LogGamma x={x}] Too small pade denom!!");

                ddouble y = sc * v / sd;

                return y;
            }

            if (x < 2d) {
                ddouble v = 2d - x;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.Near2BackwardCoefTable;

                ddouble s = table[0];
                for (int i = 1; i < table.Count; i++) {
                    s = s * v + table[i];
                }

                ddouble y = s * v;

                return y;
            }

            if (x < Consts.LogGamma.Threshold) {
                int n = int.Clamp((int)Floor(x - 1d), 0, Consts.LogGamma.PadeTables.Count - 1);
                ddouble v = x - n - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[LogGamma x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                if (n == 1) {
                    y *= v;
                }

                return y;
            }
            else {
                ddouble lnx = Log(x);
                ddouble p = x * (lnx - 1d) - Ldexp(lnx, -1);
                ddouble s = SterlingTerm(x);

                ddouble y = Consts.LogGamma.LogPI2Half + p + s;

                return y;
            }
        }

        public static ddouble Digamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble tanpi = TanPI(x);

                if (IsZero(tanpi)) {
                    return NaN;
                }

                ddouble y = Digamma(1d - x) - PI / tanpi;

                return y;
            }

            ddouble x_zsft = x - DigammaZero;
            if (Abs(x_zsft) < double.ScaleB(1, -3)) {
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeZeroPointTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * x_zsft + c;
                    sd = sd * x_zsft + d;
                }

                Debug.Assert(sd > 0.5d, $"[Digamma x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            if (x < 1d) {
                ddouble v = x - 0.5d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeTables[0];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Digamma x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = int.Clamp((int)Floor(x), 1, Consts.Digamma.PadeTables.Count - 1);
                ddouble v = x - n;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Digamma x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }
            else {
                ddouble p = Log(x);
                ddouble s = DiffLogSterlingTerm(x);
                ddouble c = Ldexp(Rcp(x), -1);

                ddouble y = p - s - c;

                return y;
            }
        }

        public static ddouble RcpGamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (x > 178.5d) {
                return 0d;
            }

            if (IsFinite(x) && double.ILogB(x.hi) < -64) {
                ddouble y = x * (1d + x * EulerGamma);
                return y;
            }
            else if (x <= Consts.Gamma.FiniteLarge) {
                ddouble y = 1d / Gamma(x);
                y = IsFinite(y) ? y : 0d;
                return y;
            }
            else {
                ddouble c = (139d + 180d * x * (1d + 24d * x * (-1d + 12d * x))) / (51840d * Sqrt(Ldexp(PI, 1)) * Pow(Sqrt(x), 5));
                ddouble y = Pow2(x * (LbE - Log2(x))) * c;
                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Gamma.SterlingTable) {
                y = SeriesUtil.Add(y, u * s, 1d, -v2 * r, out bool convergence);

                if (convergence) {
                    break;
                }

                u *= v4;
            }

            return y;
        }

        private static ddouble DiffLogSterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v2;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Digamma.SterlingTable) {
                y = SeriesUtil.Add(y, u * s, 1d, -v2 * r, out bool convergence);

                if (convergence) {
                    break;
                }

                u *= v4;
            }

            return y;
        }

        internal static partial class Consts {
            public static class Gamma {
                public const double PadeWise4X0 = 4d, PadeWise8X0 = 28d, PadeWise16X0 = 76d;
                public const double FiniteLarge = 171.6240234375d, ExtremeLarge = 171.625d;

                public static readonly ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeNearZeroTables;
                public static readonly ReadOnlyCollection<(int exp, int inc, ReadOnlyCollection<(ddouble c, ddouble d)>)> PadeWise4Tables, PadeWise8Tables, PadeWise16Tables;

                static Gamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.GammaTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());

                    PadeNearZeroTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX0p5Table"],
                        tables["PadeX1p0Table"],
                        tables["PadeX1p5Table"],
                        tables["PadeX2p0Table"],
                        tables["PadeX2p5Table"],
                        tables["PadeX3p0Table"],
                        tables["PadeX3p5Table"],
                    });

                    PadeWise4Tables = Array.AsReadOnly(new (int, int, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (2,  10, tables["PadeX4to8Table"]),
                        (12, 13, tables["PadeX8to12Table"]),
                        (25, 15, tables["PadeX12to16Table"]),
                        (40, 16, tables["PadeX16to20Table"]),
                        (56, 18, tables["PadeX20to24Table"]),
                        (74, 19, tables["PadeX24to28Table"]),
                    });

                    PadeWise8Tables = Array.AsReadOnly(new (int, int, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (93,  39, tables["PadeX28to36Table"]),
                        (132, 43, tables["PadeX36to44Table"]),
                        (175, 44, tables["PadeX44to52Table"]),
                        (219, 47, tables["PadeX52to60Table"]),
                        (266, 48, tables["PadeX60to68Table"]),
                        (314, 49, tables["PadeX68to76Table"]),
                    });

                    PadeWise16Tables = Array.AsReadOnly(new (int, int, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (363, 102, tables["PadeX76to92Table"]),
                        (465, 106, tables["PadeX92to108Table"]),
                        (571, 110, tables["PadeX108to124Table"]),
                        (681, 112, tables["PadeX124to140Table"]),
                        (793, 116, tables["PadeX140to156Table"]),
                        (909, 117, tables["PadeX156to172Table"]),
                    });
                }
            }

            public static class LogGamma {
                public static readonly ddouble LogPI2Half = (+1, -1, 0xEB3F8E4325F5A534uL, 0x94BC900144192023uL);
                public const double Threshold = 16d;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<ddouble> Near2BackwardCoefTable;

                static LogGamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.LogGammaTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX1Table"],
                        tables["PadeX2Table"],
                        tables["PadeX3Table"],
                        tables["PadeX4Table"],
                        tables["PadeX5Table"],
                        tables["PadeX6Table"],
                        tables["PadeX7Table"],
                        tables["PadeX8Table"],
                        tables["PadeX9Table"],
                        tables["PadeX10Table"],
                        tables["PadeX11Table"],
                        tables["PadeX12Table"],
                        tables["PadeX13Table"],
                        tables["PadeX14Table"],
                        tables["PadeX15Table"],
                    });

                    List<ddouble> near2_coef = [EulerGamma - 1d];

                    for (int i = 2; i < TaylorSequence.Count; i++) {
                        ddouble c = HurwitzZeta(i, 2d) / i;

                        if (ILogB(Ldexp(c, -i)) < -105) {
                            break;
                        }

                        near2_coef.Add(c);
                    }

                    near2_coef.Reverse();

                    Near2BackwardCoefTable = new(near2_coef.ToArray());
                }
            }

            public static class Digamma {
                public const double Threshold = 16d;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable, PadeZeroPointTable;

                static Digamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.DigammaTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());
                    PadeZeroPointTable = tables[nameof(PadeZeroPointTable)];

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX0p5Table"],
                        tables["PadeX1Table"],
                        tables["PadeX2Table"],
                        tables["PadeX3Table"],
                        tables["PadeX4Table"],
                        tables["PadeX5Table"],
                        tables["PadeX6Table"],
                        tables["PadeX7Table"],
                        tables["PadeX8Table"],
                        tables["PadeX9Table"],
                        tables["PadeX10Table"],
                        tables["PadeX11Table"],
                        tables["PadeX12Table"],
                        tables["PadeX13Table"],
                        tables["PadeX14Table"],
                        tables["PadeX15Table"],
                    });
                }
            }
        }
    }
}
