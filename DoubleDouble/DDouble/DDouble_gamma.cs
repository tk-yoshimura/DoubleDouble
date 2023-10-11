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

            x = RoundMantissa(x, 105);

            if (x <= Consts.Gamma.Log2PadeWise2X0) {
                int n = int.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Gamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Gamma x={x}] Too small pade denom!!");
#endif

                ddouble y = sc / sd;

                return y;
            }
            else {
                if (IsInteger(x)) {
                    return Factorial[((int)x) - 1];
                }

                int exp;
                ddouble v;
                ReadOnlyCollection<(ddouble c, ddouble d)> table;

                if (x < Consts.Gamma.Log2PadeWise4X0) {
                    int n = int.Max(0, (int)Floor(Ldexp(x - Consts.Gamma.Log2PadeWise2X0, -1)));
                    v = x - Consts.Gamma.Log2PadeWise2X0 - n * 2;
                    (exp, table) = Consts.Gamma.Log2PadeWise2Tables[n];
#if DEBUG
                    Trace.Assert(v >= 0d && v < 2d, $"[Gamma x={x}] Invalid pade v!!");
#endif
                }
                else {
                    int n = int.Max(0, (int)Floor(Ldexp(x - Consts.Gamma.Log2PadeWise4X0, -2)));
                    v = x - Consts.Gamma.Log2PadeWise4X0 - n * 4;
                    (exp, table) = Consts.Gamma.Log2PadeWise4Tables[n];

#if DEBUG
                    Trace.Assert(v >= 0d && v < 4d, $"[Gamma x={x}] Invalid pade v!!");
#endif
                }

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Gamma x={x}] Too small pade denom!!");
#endif

                ddouble y = Ldexp(Pow2(sc / sd), exp);

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

            if (x < 1.5d) {
                ddouble v = x - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[0];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[LogGamma x={x}] Too small pade denom!!");
#endif

                ddouble y = sc / sd - Log1p(v);

                return y;
            }

            if (x < Consts.LogGamma.Threshold) {
                int n = int.Max(0, (int)Round(x - 2d));
                ddouble v = x - n - 2d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[LogGamma x={x}] Too small pade denom!!");
#endif

                ddouble y = sc / sd;

                return y;
            }
            else {
                ddouble p = (x - 0.5d) * Log(x);
                ddouble s = SterlingTerm(x);

                ddouble y = Consts.LogGamma.LogPI2Half + p + s - x;

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

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Digamma x={x}] Too small pade denom!!");
#endif

                ddouble y = sc / sd;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = int.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Digamma x={x}] Too small pade denom!!");
#endif

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

        private static ddouble SterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Gamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        private static ddouble DiffLogSterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v2;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Digamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        internal static partial class Consts {
            public static class Gamma {
                public const double Threshold = 16.25, Log2PadeWise2X0 = 4, Log2PadeWise4X0 = 16;
                public const double ExtremeLarge = 171.625;

                public static readonly ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(int exp, ReadOnlyCollection<(ddouble c, ddouble d)>)> Log2PadeWise2Tables, Log2PadeWise4Tables;

                static Gamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.GammaTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX1Table"],
                        tables["PadeX2Table"],
                        tables["PadeX3Table"],
                        tables["PadeX4Table"],
                    });

                    Log2PadeWise2Tables = Array.AsReadOnly(new (int, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (2, tables["PadeX4to6Table"]),
                        (6, tables["PadeX6to8Table"]),
                        (12, tables["PadeX8to10Table"]),
                        (18, tables["PadeX10to12Table"]),
                        (25, tables["PadeX12to14Table"]),
                        (32, tables["PadeX14to16Table"]),
                    });

                    Log2PadeWise4Tables = Array.AsReadOnly(new (int, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (40, tables["PadeX16to20Table"]),
                        (56, tables["PadeX20to24Table"]),
                        (74, tables["PadeX24to28Table"]),
                        (93, tables["PadeX28to32Table"]),
                        (112, tables["PadeX32to36Table"]),
                        (132, tables["PadeX36to40Table"]),
                        (153, tables["PadeX40to44Table"]),
                        (175, tables["PadeX44to48Table"]),
                        (197, tables["PadeX48to52Table"]),
                        (219, tables["PadeX52to56Table"]),
                        (242, tables["PadeX56to60Table"]),
                        (266, tables["PadeX60to64Table"]),
                        (289, tables["PadeX64to68Table"]),
                        (314, tables["PadeX68to72Table"]),
                        (338, tables["PadeX72to76Table"]),
                        (363, tables["PadeX76to80Table"]),
                        (388, tables["PadeX80to84Table"]),
                        (413, tables["PadeX84to88Table"]),
                        (439, tables["PadeX88to92Table"]),
                        (465, tables["PadeX92to96Table"]),
                        (491, tables["PadeX96to100Table"]),
                        (518, tables["PadeX100to104Table"]),
                        (544, tables["PadeX104to108Table"]),
                        (571, tables["PadeX108to112Table"]),
                        (598, tables["PadeX112to116Table"]),
                        (626, tables["PadeX116to120Table"]),
                        (653, tables["PadeX120to124Table"]),
                        (681, tables["PadeX124to128Table"]),
                        (709, tables["PadeX128to132Table"]),
                        (737, tables["PadeX132to136Table"]),
                        (765, tables["PadeX136to140Table"]),
                        (793, tables["PadeX140to144Table"]),
                        (822, tables["PadeX144to148Table"]),
                        (851, tables["PadeX148to152Table"]),
                        (880, tables["PadeX152to156Table"]),
                        (909, tables["PadeX156to160Table"]),
                        (938, tables["PadeX160to164Table"]),
                        (967, tables["PadeX164to168Table"]),
                        (997, tables["PadeX168to172Table"]),
                    });
                }
            }

            public static class LogGamma {
                public static readonly ddouble LogPI2Half = (+1, -1, 0xEB3F8E4325F5A534uL, 0x94BC900144192023uL);
                public const int Threshold = 16;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;

                static LogGamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.LogGammaTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
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
                        tables["PadeX16Table"],
                    });
                }
            }

            public static class Digamma {
                public const int Threshold = 16;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable, PadeZeroPointTable;

                static Digamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.DigammaTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());
                    PadeZeroPointTable = tables[nameof(PadeZeroPointTable)];

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
                        tables["PadeX16Table"],
                    });
                }
            }
        }
    }
}
