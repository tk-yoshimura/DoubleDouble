using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.InverseErf;
using static DoubleDouble.ddouble.Consts.InverseErfc;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseErf(ddouble x) {
            if (IsNegative(x)) {
                return -InverseErf(Abs(x));
            }

            if (x < 0.5d) {
                return InverseErfUtil.InverseErfNearZero(x);
            }
            if (x < 1d) {
                return InverseErfc(1d - x);
            }
            if (x == 1d) {
                return PositiveInfinity;
            }

            return NaN;
        }

        public static ddouble InverseErfc(ddouble x) {
            if (x > 1d) {
                return -InverseErfc(2d - x);
            }

            if (IsNegative(x) || IsNaN(x)) {
                return NaN;
            }

            if (x > 0.5d) {
                return InverseErfUtil.InverseErfNearZero(1d - x);
            }
            if (x == 0d) {
                return PositiveInfinity;
            }

            int exp = Frexp(x).exp;

            if (exp >= -4) {
                return InverseErfUtil.InverseErfcLtRcpBinpow1(x);
            }
            if (exp >= -16) {
                return InverseErfUtil.InverseErfcLtRcpBinpow4(x);
            }
            if (exp >= -64) {
                return InverseErfUtil.InverseErfcLtRcpBinpow16(x);
            }
            if (exp >= -256) {
                return InverseErfUtil.InverseErfcLtRcpBinpow64(x);
            }
            
            return InverseErfUtil.InverseErfcLtRcpBinpow256(x);
        }

        internal static class InverseErfUtil {
            public static ddouble InverseErfNearZero(ddouble x) {
                ddouble w = x * x;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<ddouble> table = CoefTable;

                ddouble s = table[0];
                for (int i = 1; i < table.Count; i++) {
                    ddouble c = table[i];

                    s = s * w + c;
                }

                ddouble y = x * s;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow1(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 1d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LtRcpBinpow1PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[InverseErfc x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow4(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 2d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LtRcpBinpow4PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[InverseErfc x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow16(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 4d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LtRcpBinpow16PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[InverseErfc x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow64(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 8d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LtRcpBinpow64PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[InverseErfc x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow256(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 16d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = LtRcpBinpow256PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.0625d, $"[InverseErfc x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }
        }

        internal static partial class Consts {
            public static class InverseErf {
                public static readonly ReadOnlyCollection<ddouble> CoefTable;

                static InverseErf() {
                    Dictionary<string, ReadOnlyCollection<ddouble>> tables =
                        ResourceUnpack.NumTable(Resource.InverseErfTable, reverse: true);

                    CoefTable = tables[nameof(CoefTable)];
                }
            }

            public static class InverseErfc {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> LtRcpBinpow1PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> LtRcpBinpow4PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> LtRcpBinpow16PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> LtRcpBinpow64PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> LtRcpBinpow256PadeTable;

                static InverseErfc() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.InverseErfcTable, reverse: true);

                    LtRcpBinpow1PadeTable = tables[nameof(LtRcpBinpow1PadeTable)];
                    LtRcpBinpow4PadeTable = tables[nameof(LtRcpBinpow4PadeTable)];
                    LtRcpBinpow16PadeTable = tables[nameof(LtRcpBinpow16PadeTable)];
                    LtRcpBinpow64PadeTable = tables[nameof(LtRcpBinpow64PadeTable)];
                    LtRcpBinpow256PadeTable = tables[nameof(LtRcpBinpow256PadeTable)];
                }
            }
        }
    }
}