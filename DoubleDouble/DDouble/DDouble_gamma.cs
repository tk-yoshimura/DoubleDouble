﻿using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
                x = RoundMantissa(x, 104);
                if (x < Consts.Gamma.ExtremeLarge && IsInteger(x)) {
                    return Factorial[((int)x) - 1];
                }

                ddouble p = (x - 0.5d) * Log2(x);
                ddouble s = SterlingTerm(x);

                ddouble y = Consts.Gamma.SqrtPI2 * Pow2(p + (s - x) * LbE);

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
                public const double Threshold = 36.25;
                public const double ExtremeLarge = 170.625;
                public static readonly ddouble SqrtPI2 = (+1, 1, 0xA06C98FFB1382CB2uL, 0xBE520FD739167717uL);

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
