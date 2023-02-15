using DoubleDouble.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseErf(ddouble x) {
            if (x < 0.0) {
                return -InverseErf(Abs(x));
            }

            if (x < 0.5) {
                return InverseErfUtil.InverseErfNearZero(x);
            }
            if (x < 1.0) {
                return InverseErfc(1.0 - x);
            }
            if (x == 1.0) {
                return PositiveInfinity;
            }

            return NaN;
        }

        public static ddouble InverseErfc(ddouble x) {
            if (x > 1.0) {
                return -InverseErfc(2.0 - x);
            }

            if (!(x >= 0)) {
                return NaN;
            }

            if (x > 0.5) {
                return InverseErfUtil.InverseErfNearZero(1.0 - x);
            }
            if (x == 0.0) {
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
            if (x > 0) {
                return InverseErfUtil.InverseErfcLtRcpBinpow256(x);
            }

            return PositiveInfinity;
        }

        internal static class InverseErfUtil {
            public static ddouble InverseErfNearZero(ddouble x) {
                ddouble w = x * x;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<ddouble> table = Consts.InverseErf.CoefTable;

                ddouble s = table[0];
                for (int i = 1; i < table.Count; i++) {
                    ddouble c = table[i];

                    s = s * w + c;
                }

                ddouble y = x * s;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow1(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 1.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.InverseErfc.LtRcpBinpow1PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow4(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 2.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.InverseErfc.LtRcpBinpow4PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow16(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 4.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.InverseErfc.LtRcpBinpow16PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow64(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 8.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.InverseErfc.LtRcpBinpow64PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble InverseErfcLtRcpBinpow256(ddouble x) {
                ddouble w = Sqrt(-Log2(x)) - 16.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.InverseErfc.LtRcpBinpow256PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

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