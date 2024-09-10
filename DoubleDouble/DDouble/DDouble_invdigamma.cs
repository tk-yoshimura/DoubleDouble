using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.InverseDigamma;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseDigamma(ddouble x) {
            if (!IsFinite(x)) {
                if (IsPositiveInfinity(x)) {
                    return PositiveInfinity;
                }
                if (IsNegativeInfinity(x)) {
                    return PlusZero;
                }

                return NaN;
            }

            if (IsPositive(x)) {
                if (x < 1d) {
                    foreach ((double xmin, double xmax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in PadePlusNearZeroTables) {
                        if (x >= xmax) {
                            continue;
                        }

                        ddouble v = x - xmin;

                        Debug.Assert(v >= 0d, $"[InverseDigamma x={x}] Invalid pade v!!");

                        (ddouble sc, ddouble sd) = table[0];
                        for (int i = 1; i < table.Count; i++) {
                            (ddouble c, ddouble d) = table[i];

                            sc = sc * v + c;
                            sd = sd * v + d;
                        }

                        Debug.Assert(sd > 0.0625d, $"[InverseDigamma x={x}] Too small pade denom!!");

                        ddouble y = sc / sd;

                        return y;
                    }
                }
                else if (x < 64d) {
                    foreach ((double xmin, double xmax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in PadePlusLargeTables) {
                        if (x >= xmax) {
                            continue;
                        }

                        ddouble v = x - xmin;

                        Debug.Assert(v >= 0d, $"[InverseDigamma x={x}] Invalid pade v!!");

                        (ddouble sc, ddouble sd) = table[0];
                        for (int i = 1; i < table.Count; i++) {
                            (ddouble c, ddouble d) = table[i];

                            sc = sc * v + c;
                            sd = sd * v + d;
                        }

                        Debug.Assert(sd > 0.0625d, $"[InverseDigamma x={x}] Too small pade denom!!");

                        ddouble y = Exp(sc / sd);

                        return y;
                    }
                }
                else {
                    ddouble y = Exp(x);

                    return y;
                }
            }
            else {
                x = -x;

                if (x < 4d) {
                    foreach ((double xmin, double xmax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in PadeMinusNearZeroTables) {
                        if (x >= xmax) {
                            continue;
                        }

                        ddouble v = x - xmin;

                        Debug.Assert(v >= 0d, $"[InverseDigamma x={x}] Invalid pade v!!");

                        (ddouble sc, ddouble sd) = table[0];
                        for (int i = 1; i < table.Count; i++) {
                            (ddouble c, ddouble d) = table[i];

                            sc = sc * v + c;
                            sd = sd * v + d;
                        }

                        Debug.Assert(sd > 0.0625d, $"[InverseDigamma x={x}] Too small pade denom!!");

                        ddouble y = sc / sd;

                        return y;
                    }
                }
                else if (ILogB(x) < 128d) {
                    ddouble u = Log2(x);

                    foreach ((double umin, double umax, ReadOnlyCollection<(ddouble c, ddouble d)> table) in PadeMinusLargeTables) {
                        if (u >= umax) {
                            continue;
                        }

                        ddouble v = u - umin;

                        Debug.Assert(v >= 0d, $"[InverseDigamma x={x}] Invalid pade v!!");

                        (ddouble sc, ddouble sd) = table[0];
                        for (int i = 1; i < table.Count; i++) {
                            (ddouble c, ddouble d) = table[i];

                            sc = sc * v + c;
                            sd = sd * v + d;
                        }

                        Debug.Assert(sd > 0.0625d, $"[InverseDigamma x={x}] Too small pade denom!!");

                        ddouble y = (sc / sd) / x;

                        return y;
                    }
                }
                else {
                    ddouble y = 1d / x;

                    return y;
                }
            }

            return NaN;
        }

        internal static partial class Consts {
            public static class InverseDigamma {
                public static readonly ReadOnlyCollection<(double umin, double umax, ReadOnlyCollection<(ddouble c, ddouble d)>)>
                    PadePlusNearZeroTables, PadePlusLargeTables, PadeMinusNearZeroTables, PadeMinusLargeTables;

                static InverseDigamma() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.InverseDigammaTable, reverse: true);

                    PadePlusNearZeroTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0d, 0.5d, tables["PadeX0to0p5Table"]),
                        (0.5d, 1d, tables["PadeX0p5to1Table"]),
                    });

                    PadePlusLargeTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (1d, 2d, tables["PadeX1to2Table"]),
                        (2d, 4d, tables["PadeX2to4Table"]),
                        (4d, 8d, tables["PadeX4to8Table"]),
                        (8d, 16d, tables["PadeX8to16Table"]),
                        (16d, 32d, tables["PadeX16to32Table"]),
                        (32d, 64d, tables["PadeX32to64Table"]),
                    });

                    PadeMinusNearZeroTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (0d, 1d, tables["PadeXm0to1Table"]),
                        (1d, 2d, tables["PadeXm1to2Table"]),
                        (2d, 4d, tables["PadeXm2to4Table"]),
                    });

                    PadeMinusLargeTables = Array.AsReadOnly(new (double, double, ReadOnlyCollection<(ddouble c, ddouble d)>)[] {
                        (2d, 4d, tables["PadeXmExp2toExp4Table"]),
                        (4d, 8d, tables["PadeXmExp4toExp8Table"]),
                        (8d, 16d, tables["PadeXmExp8toExp16Table"]),
                        (16d, 32d, tables["PadeXmExp16toExp32Table"]),
                        (32d, 64d, tables["PadeXmExp32toExp64Table"]),
                        (64d, 128d, tables["PadeXmExp64toExp128Table"]),
                    });
                }
            }
        }
    }
}