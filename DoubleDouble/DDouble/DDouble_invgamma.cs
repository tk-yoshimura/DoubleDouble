using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.InverseGamma;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseGamma(ddouble x) {
            if (!(x >= XMin)) {
                return NaN;
            }

            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x >= 1d) {
                ddouble u = Log2(x);

                foreach ((double umin, double umax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in PadeTables) {
                    if (u >= umax) {
                        continue;
                    }

                    ddouble v = u - umin;

                    Debug.Assert(v >= 0d, $"[InverseGamma x={x}] Invalid pade v!!");

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    Debug.Assert(sd > 0.5d, $"[InverseGamma x={x}] Too small pade denom!!");

                    ddouble y = sc / sd;

                    ddouble g = Gamma(y);

                    if (x != g && IsFinite(g)) {
                        ddouble psi = Digamma(y);
                        ddouble delta = (x / g - 1d) / psi;

                        y += delta;
                    }

                    y = TruncateMantissa(y, keep_bits: 105);

                    return y;
                }
            }
            else if (x >= XMin) {
                ddouble u = Max(0d, x - HalfSqrtPI);

                foreach ((double umin, double umax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in SingularPadeTables) {
                    if (u >= umax) {
                        continue;
                    }

                    ddouble v = u - umin;

                    Debug.Assert(v >= 0d, $"[InverseGamma x={x}] Invalid pade v!!");

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    Debug.Assert(sd > 0.5d, $"[InverseGamma x={x}] Too small pade denom!!");

                    ddouble y = Sqrt(sc / sd) + 1.5d;

                    y = TruncateMantissa(y, keep_bits: 105);

                    return y;
                }
            }

            return NaN;
        }

        internal static partial class Consts {
            public static class InverseGamma {
                public static readonly ddouble HalfSqrtPI = Ldexp(Sqrt(PI), -1);
                public static readonly ddouble XMin = Min(Gamma(1.5d), HalfSqrtPI);

                public static readonly ReadOnlyCollection<(double umin, double umax, ReadOnlyCollection<(ddouble c, ddouble d)>)> PadeTables, SingularPadeTables;

                static InverseGamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.InverseGammaTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0d, 0.5d, tables["PadeX0Table"]),
                        (0.5d, 1d, tables["PadeX0p5Table"]),
                        (1d, 2d, tables["PadeX1Table"]),
                        (2d, 4d, tables["PadeX2Table"]),
                        (4d, 8d, tables["PadeX4Table"]),
                        (8d, 16d, tables["PadeX8Table"]),
                        (16d, 32d, tables["PadeX16Table"]),
                        (32d, 64d, tables["PadeX32Table"]),
                        (64d, 128d, tables["PadeX64Table"]),
                        (128d, 256d, tables["PadeX128Table"]),
                        (256d, 512d, tables["PadeX256Table"]),
                        (512d, double.MaxValue, tables["PadeX512Table"]),
                    });

                    SingularPadeTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0d, 1 / 256d, tables["PadeXSingularRcp256Table"]),
                        (1 / 256d, 1 / 128d, tables["PadeXSingularRcp128Table"]),
                        (1 / 128d, 1 / 32d, tables["PadeXSingularRcp32Table"]),
                        (1 / 32d, 1 / 8d, tables["PadeXSingularRcp8Table"]),
                    });
                }
            }
        }
    }
}