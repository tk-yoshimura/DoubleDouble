using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble BarnesG(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (x >= Consts.BarnesG.ExtremeLarge) {
                return PositiveInfinity;
            }

            x = TruncateMantissa(x, 105);

            if (x <= 0 && IsInteger(x)) {
                return Zero;
            }

            if (x < Consts.BarnesG.PadeXMin) {
                throw new NotImplementedException();
            }
            if (x >= Consts.BarnesG.PadeXMax) {
                throw new NotImplementedException();
            }

            int n = (int)Floor(x - Consts.BarnesG.PadeXMin);
            ddouble v = x - n;

            ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.BarnesG.PadeTables[n];

            (ddouble sc, ddouble sd) = table[0];
            for (int i = 1; i < table.Count; i++) {
                (ddouble c, ddouble d) = table[i];

                sc = sc * v + c;
                sd = sd * v + d;
            }

#if DEBUG
            Trace.Assert(sd > 0.0625d, $"[BarnesG x={x}] Too small pade denom!!");
#endif

            ddouble y = sc / sd;

            return y;
        }

        public static ddouble LogBarnesG(ddouble x) {
            if (!IsFinite(x) || IsNegative(x)) {
                return NaN;
            }
            if (x < Consts.BarnesG.LogPadeXMin) {
                return Log(BarnesG(x));
            }
            if (x >= Consts.BarnesG.LogPadeXMax) {
                throw new NotImplementedException();
            }

            int n = (int)Floor(Ldexp(x - Consts.BarnesG.LogPadeXMin, 1));

            (double x0, ReadOnlyCollection<(ddouble c, ddouble d)> table) = Consts.BarnesG.LogPadeTables[n];
            ddouble v = x - x0;

            (ddouble sc, ddouble sd) = table[0];
            for (int i = 1; i < table.Count; i++) {
                (ddouble c, ddouble d) = table[i];

                sc = sc * v + c;
                sd = sd * v + d;
            }

#if DEBUG
            Trace.Assert(sd > 0.0625d, $"[LogBarnesG x={x}] Too small pade denom!!");
#endif

            ddouble y = sc / sd;

            return y;
        }

        internal static partial class Consts {
            public static class BarnesG {
                public const double ExtremeLarge = 30d;
                public const double PadeXMin = -0.5d, PadeXMax = 3.5d;
                public const double LogPadeXMin = 0.25d, LogPadeXMax = 3.25d;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ReadOnlyCollection<(double x0, ReadOnlyCollection<(ddouble c, ddouble d)>)> LogPadeTables;
                public static readonly ddouble Rcp12 = 1.0 / (ddouble)12;

                static BarnesG() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> pade_tables =
                        ResourceUnpack.NumTableX2(Resource.BarnesGTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        pade_tables["PadeX0Table"],
                        pade_tables["PadeX1Table"],
                        pade_tables["PadeX2Table"],
                        pade_tables["PadeX3Table"],
                    });

                    LogPadeTables = Array.AsReadOnly(new (double x0, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0.25d, pade_tables["LogPadeX0p25Table"]),
                        (1d, pade_tables["LogPadeX0p75Table"]),
                        (1.25d, pade_tables["LogPadeX1p25Table"]),
                        (2d, pade_tables["LogPadeX1p75Table"]),
                        (2.25d, pade_tables["LogPadeX2p25Table"]),
                        (3d, pade_tables["LogPadeX2p75Table"]),
                    });
                }
            }
        }
    }
}