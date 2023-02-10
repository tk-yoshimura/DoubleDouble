using DoubleDouble.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static DoubleDouble.ddouble.Consts.RiemannZeta;

namespace DoubleDouble {

    public partial struct ddouble {
        public static ddouble RiemannZeta(ddouble x) {
            if (x < -Eps) {
                ddouble y = Pow(2 * PI, x) * RcpPI * SinPIHalf(x) * Gamma(1d - x) * RiemannZeta(1d - x);

                return y;
            }

            if (x <= Eps) {
                ddouble y = -(1d + x * Log(2 * PI)) / 2;

                return RoundMantissa(y, keep_bits: 105);
            }

            static ddouble pade(ddouble x, (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) table) {
                ddouble c = table.cs[^1], d = table.ds[^1];
                for (int i = table.cs.Count - 2; i >= 0; i--) {
                    c = c * x + table.cs[i];
                }
                for (int i = table.ds.Count - 2; i >= 0; i--) {
                    d = d * x + table.ds[i];
                }

                return c / d;
            };

            if (x < 1d) {
                ddouble y = 1d + pade(1d - x, PadeX0Table) / (1d - x);

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 2d) {
                ddouble y = pade(x - 1d, PadeX1Table) + 1d / (x - 1d);

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 4d) {
                ddouble b = (+1, -1, 0xB2DB600000000000uL, 0x0000000000000000uL);
                ddouble y = pade(x - 2d, PadeX2Table) + 1d / (x - 1d) + b;

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 6d) {
                ddouble b = (-1, 1, 0xD224A00000000000uL, 0x0000000000000000uL);
                ddouble y = 1d + Exp(pade(x - 4d, PadeX4Table) + b);

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 10d) {
                ddouble y = 1d + Exp(pade(x - 6d, PadeX6Table));

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 17d) {
                ddouble y = 1d + Exp(pade(x - 10d, PadeX10Table));

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 30d) {
                ddouble y = 1d + Exp(pade(x - 17d, PadeX17Table));

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 74d) {
                ddouble y = 1d + Exp(pade(x - 30d, PadeX30Table));

                return RoundMantissa(y, keep_bits: 105);
            }
            if (x < 106d) {
                ddouble y = 1d + Pow2(-x);

                return RoundMantissa(y, keep_bits: 105);
            }
            if (!IsNaN(x)) {
                return 1d;
            }

            return NaN;
        }

        public static ddouble DirichletEta(ddouble x) {
            ddouble v = x - 1d;

            if (Abs(v) > Eps) {
                ddouble y = (1d - Pow2(-v)) * RiemannZeta(x);

                return y;
            }
            else {
                ddouble y = Ln2 * (1d + v * (EulerGamma - Ln2 / 2));

                return y;
            }
        }

        internal static partial class Consts {
            public static class RiemannZeta {
                public static readonly double Eps = Math.ScaleB(1, -64);

                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX0Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX1Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX2Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX4Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX6Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX10Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX17Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX30Table;

                static RiemannZeta() {
                    Dictionary<string, ReadOnlyCollection<ddouble>> tables = ResourceUnpack.NumTable(Resource.RiemannZetaTable);

                    PadeX0Table = (tables["PadeX0NumerTable"], tables["PadeX0DenomTable"]);
                    PadeX1Table = (tables["PadeX1NumerTable"], tables["PadeX1DenomTable"]);
                    PadeX2Table = (tables["PadeX2NumerTable"], tables["PadeX2DenomTable"]);
                    PadeX4Table = (tables["PadeX4NumerTable"], tables["PadeX4DenomTable"]);
                    PadeX6Table = (tables["PadeX6NumerTable"], tables["PadeX6DenomTable"]);
                    PadeX10Table = (tables["PadeX10NumerTable"], tables["PadeX10DenomTable"]);
                    PadeX17Table = (tables["PadeX17NumerTable"], tables["PadeX17DenomTable"]);
                    PadeX30Table = (tables["PadeX30NumerTable"], tables["PadeX30DenomTable"]);
                }
            }
        }
    }
}