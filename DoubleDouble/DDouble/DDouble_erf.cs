using DoubleDouble.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erf(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (x < 0.0) {
                return -Erf(Abs(x));
            }

            if (x < 0.5) {
                return ErfUtil.ErfNearZero(x);
            }
            if (x < 1.0) {
                return 1.0 - ErfUtil.ErfcGtP5(x);
            }
            if (x < 2.0) {
                return 1.0 - ErfUtil.ErfcGt1(x);
            }
            if (x < 4.0) {
                return 1.0 - ErfUtil.ErfcGt2(x);
            }
            if (x < 8.0) {
                return 1.0 - ErfUtil.ErfcGt4(x);
            }
            if (x < 8.5) {
                return 1.0 - ErfUtil.ErfcGt8(x);
            }

            return 1.0;
        }

        public static ddouble Erfc(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x < 0.5) {
                return 1.0 - Erf(x);
            }
            if (x < 1.0) {
                return ErfUtil.ErfcGtP5(x);
            }
            if (x < 2.0) {
                return ErfUtil.ErfcGt1(x);
            }
            if (x < 4.0) {
                return ErfUtil.ErfcGt2(x);
            }
            if (x < 8.0) {
                return ErfUtil.ErfcGt4(x);
            }
            if (x < 16.0) {
                return ErfUtil.ErfcGt8(x);
            }
            if (x < 27.25) {
                return ErfUtil.ErfcGt16(x);
            }

            return 0.0;
        }

        internal static class ErfUtil {
            public static ddouble ErfNearZero(ddouble x) {
                ddouble w = x * x;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erf.PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = x * (sc / sd);

                return y;
            }

            public static ddouble ErfcGtP5(ddouble x) {
                ddouble w = x - 0.5;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.GtP5PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt1(ddouble x) {
                ddouble w = x - 1.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt1PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt2(ddouble x) {
                ddouble w = x - 2.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt2PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt4(ddouble x) {
                ddouble w = x - 4.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt4PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt8(ddouble x) {
                ddouble w = x - 8.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt8PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt16(ddouble x) {
                ddouble w = x - 16.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt16PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }
        }

        internal static partial class Consts {
            public static class Erf {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> PadeTable;

                static Erf() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.ErfTable, reverse: true);

                    PadeTable = tables[nameof(PadeTable)];
                }
            }

            public static class Erfc {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> GtP5PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt1PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt2PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt4PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt8PadeTable;
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt16PadeTable;

                static Erfc() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.ErfcTable, reverse: true);

                    GtP5PadeTable = tables[nameof(GtP5PadeTable)];
                    Gt1PadeTable = tables[nameof(Gt1PadeTable)];
                    Gt2PadeTable = tables[nameof(Gt2PadeTable)];
                    Gt4PadeTable = tables[nameof(Gt4PadeTable)];
                    Gt8PadeTable = tables[nameof(Gt8PadeTable)];
                    Gt16PadeTable = tables[nameof(Gt16PadeTable)];
                }
            }
        }
    }
}
