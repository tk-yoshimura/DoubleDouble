using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

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

            if (x < Consts.Erfi.PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: false) * Consts.Erfi.C;
            }
            if (x < Consts.Erfi.PadeApproxMax) {
                return ErfiPade.Value(x, scale: false) * Consts.Erfi.C;
            }
            return ErfiLimit.Value(x, scale: false) * Consts.Erfi.C;
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

            if (x < Consts.Erfi.PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: true);
            }
            if (x < Consts.Erfi.PadeApproxMax) {
                return ErfiPade.Value(x, scale: true);
            }
            return ErfiLimit.Value(x, scale: true);
        }

        internal static class ErfiNearZero {
            public static ddouble Value(ddouble x, bool scale, int max_terms = 32) {
                ddouble x2 = x * x;

                ddouble s = 0d, u = x;

                for (int k = 0; k <= max_terms; k++) {
                    ddouble ds = u / (2 * k + 1) * TaylorSequence[k];
                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s)) {
                        break;
                    }

                    u *= x2;
                    s = s_next;
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

                if (x < 3.75) {
                    int n = int.Max(0, (int)Round(x * 2 - 1d));
                    ddouble v = x - (n + 1) * 0.5;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfi.Xp5PadeTables[n];

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

#if DEBUG
                    Trace.Assert(sd > 0.0625d, $"[Erfi x={x}] Too small pade denom!!");
#endif

                    y = sd / sc;
                }
                else {
                    int n = int.Max(0, (int)Round(x - 4d));
                    ddouble v = x - n - 4d;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfi.X1PadeTables[n];

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

#if DEBUG
                    Trace.Assert(sd > 0.0625d, $"[Erfi x={x}] Too small pade denom!!");
#endif

                    y = sd / sc;
                }

                if (!scale) {
                    y *= Exp(x * x);
                }

                return y;
            }
        }

        internal static partial class Consts {
            public static class Erfi {
                public static readonly ddouble C = 2d / Sqrt(PI);
                public static readonly ddouble PadeApproxMin = 0.25d, PadeApproxMax = 16d;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> Xp5PadeTables;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> X1PadeTables;

                static Erfi() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.ErfiTable, reverse: true);

                    Xp5PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["PadeX0p5Table"],
                        tables["PadeX1Table"],
                        tables["PadeX1p5Table"],
                        tables["PadeX2Table"],
                        tables["PadeX2p5Table"],
                        tables["PadeX3Table"],
                        tables["PadeX3p5Table"],
                    });

                    X1PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
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