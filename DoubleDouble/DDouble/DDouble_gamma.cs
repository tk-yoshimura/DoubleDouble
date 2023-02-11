using DoubleDouble.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Gamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
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

            if (x <= Consts.Gamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Gamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            if (x <= Consts.Gamma.ExtremeLarge) {
                ddouble r = Sqrt(2 * PI / x);
                ddouble s = Exp(SterlingTerm(x));

                ddouble p = (x < 128.0)
                    ? (Pow(x, x) / Exp(x))
                    : Pow(Pow(x, 128) / Exp(128), Ldexp(x, -7));

                ddouble y = r * p * s;

                return y;
            }
            else {
                ddouble u = x - 1.0;

                if (u <= Consts.Gamma.ExtremeLarge) {
                    return u * Gamma(u);
                }
                else {
                    return PositiveInfinity;
                }
            }
        }

        public static ddouble LogGamma(ddouble x) {
            if (IsNaN(x) || IsMinusZero(x) || x.Sign < 0) {
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

                ddouble y = sc / sd - Log1p(v);

                return y;
            }

            if (x < Consts.LogGamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 2d));
                ddouble v = x - n - 2d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                ddouble y = sc / sd;

                return y;
            }
            else {
                ddouble p = (x - 0.5d) * Log(x);
                ddouble s = SterlingTerm(x);

                ddouble k = Consts.LogGamma.SterlingLogBias;

                ddouble y = k + p + s - x;

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
            if (Abs(x_zsft) < Math.ScaleB(1, -3)) {
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeZeroPointTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * x_zsft + c;
                    sd = sd * x_zsft + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                ddouble y = sc / sd;

                return y;
            }
            else {
                ddouble s = DiffLogSterlingTerm(x);
                ddouble p = Log(x);
                ddouble c = Rcp(x) / 2;

                ddouble y = -s + p - c;

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
                public const double Threshold = 36.25;
                public const double ExtremeLarge = 170.625;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable;

                static Gamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.GammaTable, reverse: true);

                    SterlingTable = Array.AsReadOnly(tables[nameof(SterlingTable)].Reverse().ToArray());

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
                        tables["PadeX17Table"],
                        tables["PadeX18Table"],
                        tables["PadeX19Table"],
                        tables["PadeX20Table"],
                        tables["PadeX21Table"],
                        tables["PadeX22Table"],
                        tables["PadeX23Table"],
                        tables["PadeX24Table"],
                        tables["PadeX25Table"],
                        tables["PadeX26Table"],
                        tables["PadeX27Table"],
                        tables["PadeX28Table"],
                        tables["PadeX29Table"],
                        tables["PadeX30Table"],
                        tables["PadeX31Table"],
                        tables["PadeX32Table"],
                        tables["PadeX33Table"],
                        tables["PadeX34Table"],
                        tables["PadeX35Table"],
                        tables["PadeX36Table"],
                    });
                }
            }

            public static class LogGamma {
                public static readonly ddouble SterlingLogBias = Log(PI * 2) / 2;
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
