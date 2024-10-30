using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Erfi;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erfi(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Erfi(-x);
            }
            if (IsZero(x)) {
                return PlusZero;
            }
            if (x >= 26.65625d) {
                return PositiveInfinity;
            }

            if (x < PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: false) * RcpSqrtPi2;
            }
            if (x < PadeApproxMax) {
                return ErfiPade.Value(x, scale: false) * RcpSqrtPi2;
            }
            return ErfiLimit.Value(x, scale: false) * RcpSqrtPi2;
        }

        public static ddouble DawsonF(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -DawsonF(-x);
            }
            if (IsZero(x) || IsInfinity(x)) {
                return 0d;
            }

            if (x < PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: true);
            }
            if (x < PadeApproxMax) {
                return ErfiPade.Value(x, scale: true);
            }
            return ErfiLimit.Value(x, scale: true);
        }

        internal static class ErfiNearZero {
            public static ddouble Value(ddouble x, bool scale, int max_terms = 32) {
                ddouble x2 = x * x;

                ddouble s = 0d, u = x;

                for (int k = 0; k <= max_terms; k++) {
                    s = SeriesUtil.Add(s, u, TaylorSequence[k] / (2 * k + 1), out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= x2;
                }

                if (scale) {
                    s *= Exp(-x2);
                }

                return s;
            }
        }

        internal static class ErfiLimit {
            public static ddouble Value(ddouble x, bool scale, int max_terms = 32) {
                ddouble v = 1d / x, v2 = v * v;

                ddouble s = 0d, ds = Ldexp(v, -1);

                for (int k = 0; k <= max_terms; k++) {
                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s)) {
                        break;
                    }

                    s = s_next;
                    ds *= v2 * Ldexp(2 * k + 1, -1);
                }

                if (!scale) {
                    s *= Exp(x * x);
                }

                return s;
            }
        }

        internal static class ErfiPade {

            public static ddouble Value(ddouble x, bool scale) {
                ddouble y;

                ddouble v;
                ReadOnlyCollection<(ddouble c, ddouble d)> table;

                if (x < PadeWise0p5X0) {
                    v = x - 0.25d;
                    table = PadeX0p25to0p5Table;
                }
                else if (x < PadeWise1X0) {
                    int n = int.Clamp((int)Floor(Ldexp(x - PadeWise0p5X0, 1)), 0, PadeWise0p5Tables.Count - 1);
                    v = x - PadeWise0p5X0 - n * 0.5;
                    table = PadeWise0p5Tables[n];
                }
                else if (x < PadeWise2X0) {
                    int n = int.Clamp((int)Floor(x - PadeWise1X0), 0, PadeWise1Tables.Count - 1);
                    v = x - PadeWise1X0 - n;
                    table = PadeWise1Tables[n];
                }
                else if (x < PadeWise4X0) {
                    int n = int.Clamp((int)Floor(Ldexp(x - PadeWise2X0, -1)), 0, PadeWise2Tables.Count - 1);
                    v = x - PadeWise2X0 - n * 2;
                    table = PadeWise2Tables[n];
                }
                else {
                    int n = int.Clamp((int)Floor(Ldexp(x - PadeWise4X0, -2)), 0, PadeWise4Tables.Count - 1);
                    v = x - PadeWise4X0 - n * 4;
                    table = PadeWise4Tables[n];
                }

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sc > 0.5d, $"[Erfi x={x}] Too small pade denom!!");

                y = sd / sc;

                if (!scale) {
                    y *= Exp(x * x);
                }

                return y;
            }
        }

        internal static partial class Consts {
            public static class Erfi {
                public static readonly ddouble RcpSqrtPi2 = 2d / Sqrt(Pi);

                public const double PadeApproxMin = 0.25d, PadeApproxMax = 16d;
                public const double PadeWise0p5X0 = 0.5d, PadeWise1X0 = 2d, PadeWise2X0 = 4d, PadeWise4X0 = 8d;

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> PadeX0p25to0p5Table;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeWise0p5Tables;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeWise1Tables;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeWise2Tables;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeWise4Tables;

                static Erfi() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.ErfiTable, reverse: true);

                    PadeX0p25to0p5Table = tables["PadeX0p25to0p5Table"];

                    PadeWise0p5Tables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX0p5to1Table"],
                        tables["PadeX1to1p5Table"],
                        tables["PadeX1p5to2Table"],
                    });

                    PadeWise1Tables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX2to3Table"],
                        tables["PadeX3to4Table"],
                    });

                    PadeWise2Tables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX4to6Table"],
                        tables["PadeX6to8Table"],
                    });

                    PadeWise4Tables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX8to12Table"],
                        tables["PadeX12to16Table"],
                    });
                }
            }
        }
    }
}