using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.RiemannZeta;

namespace DoubleDouble {

    public partial struct ddouble {
        public static ddouble RiemannZeta(ddouble x) {
            if (x < -Eps) {
                ddouble y = Pow(Ldexp(Pi, 1), x) * RcpPi * SinPiHalf(x) * Gamma(1d - x) * RiemannZeta(1d - x);

                return y;
            }

            if (x <= Eps) {
                ddouble y = -Ldexp(1d + x * Log(Ldexp(Pi, 1)), -1);

                return TruncateMantissa(y, keep_bits: 105);
            }

            static ddouble pade(ddouble x, (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) table) {
                ddouble c = table.cs[0], d = table.ds[0];
                for (int i = 1; i < table.cs.Count; i++) {
                    c = c * x + table.cs[i];
                }
                for (int i = 1; i < table.ds.Count; i++) {
                    d = d * x + table.ds[i];
                }

                Debug.Assert(d > 0.5d, $"[RiemannZeta x={x}] Too small pade denom!!");

                return c / d;
            }

            if (x < 1d) {
                ddouble y = 1d + pade(1d - x, PadeX0Table) / (1d - x);

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 2d) {
                ddouble y = pade(x - 1d, PadeX1Table) + 1d / (x - 1d);

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 4d) {
                ddouble b = (+1, -1, 0xB2DB600000000000uL, 0x0000000000000000uL);
                ddouble y = pade(x - 2d, PadeX2Table) + 1d / (x - 1d) + b;

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 6d) {
                ddouble b = (-1, 1, 0xD224A00000000000uL, 0x0000000000000000uL);
                ddouble y = 1d + Exp(pade(x - 4d, PadeX4Table) + b);

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 10d) {
                ddouble y = 1d + Exp(pade(x - 6d, PadeX6Table));

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 17d) {
                ddouble y = 1d + Exp(pade(x - 10d, PadeX10Table));

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 30d) {
                ddouble y = 1d + Exp(pade(x - 17d, PadeX17Table));

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 74d) {
                ddouble y = 1d + Exp(pade(x - 30d, PadeX30Table));

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (x < 106d) {
                ddouble y = 1d + Pow2(-x);

                return TruncateMantissa(y, keep_bits: 105);
            }
            if (!IsNaN(x)) {
                return 1d;
            }

            return NaN;
        }

        public static ddouble DirichletEta(ddouble x) {
            ddouble v = x - 1d;

            if (ILogB(v) >= EpsExponent) {
                ddouble y = (1d - Pow2(-v)) * RiemannZeta(x);

                return y;
            }
            else {
                ddouble y = Ln2 * (1d + v * (EulerGamma - Ldexp(Ln2, -1)));

                return y;
            }
        }

        internal static partial class Consts {
            public static class RiemannZeta {
                public const int EpsExponent = -64;
                public static readonly double Eps = double.ScaleB(1, EpsExponent);

                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX0Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX1Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX2Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX4Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX6Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX10Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX17Table;
                public static readonly (ReadOnlyCollection<ddouble> cs, ReadOnlyCollection<ddouble> ds) PadeX30Table;

                static RiemannZeta() {
                    Dictionary<string, ReadOnlyCollection<ddouble>> tables
                        = ResourceUnpack.NumTable(Resource.RiemannZetaTable, reverse: true);

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