﻿using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Erf;
using static DoubleDouble.ddouble.Consts.Erfc;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erf(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Erf(-x);
            }

            if (x < 0.5d) {
                return ErfUtil.ErfNearZero(x);
            }
            if (x < 1d) {
                return 1d - ErfUtil.ErfcGtP5(x);
            }
            if (x < 2d) {
                return 1d - ErfUtil.ErfcGt1(x);
            }
            if (x < 4d) {
                return 1d - ErfUtil.ErfcGt2(x);
            }
            if (x < 8d) {
                return 1d - ErfUtil.ErfcGt4(x);
            }
            if (x < 8.5d) {
                return 1d - ErfUtil.ErfcGt8(x);
            }

            return 1d;
        }

        public static ddouble Erfc(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x < 0.5d) {
                return 1d - Erf(x);
            }
            if (x < 1d) {
                return ErfUtil.ErfcGtP5(x);
            }
            if (x < 2d) {
                return ErfUtil.ErfcGt1(x);
            }
            if (x < 4d) {
                return ErfUtil.ErfcGt2(x);
            }
            if (x < 8d) {
                return ErfUtil.ErfcGt4(x);
            }
            if (x < 16d) {
                return ErfUtil.ErfcGt8(x);
            }
            if (x < 27.25d) {
                return ErfUtil.ErfcGt16(x);
            }

            return 0d;
        }

        public static ddouble Erfcx(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x < 0.5) {
                return Erfc(x) * Exp(x * x);
            }
            if (x < 1d) {
                return ErfUtil.ErfcxGtP5(x);
            }
            if (x < 2d) {
                return ErfUtil.ErfcxGt1(x);
            }
            if (x < 4d) {
                return ErfUtil.ErfcxGt2(x);
            }
            if (x < 8d) {
                return ErfUtil.ErfcxGt4(x);
            }
            if (x < 16d) {
                return ErfUtil.ErfcxGt8(x);
            }

            return ErfUtil.ErfcxGt16(x);
        }

        internal static class ErfUtil {
            public static ddouble ErfNearZero(ddouble x) {
                ddouble w = x * x;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erf x={x}] Too small pade denom!!");

                ddouble y = x * (sc / sd);

                return y;
            }

            public static ddouble ErfcGtP5(ddouble x) {
                ddouble w = x - 0.5d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = GtP5PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt1(ddouble x) {
                ddouble w = x - 1d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt1PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt2(ddouble x) {
                ddouble w = x - 2d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt2PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt4(ddouble x) {
                ddouble w = x - 4d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt4PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt8(ddouble x) {
                ddouble w = x - 8d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt8PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt16(ddouble x) {
                ddouble w = x - 16d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt16PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfc x={x}] Too small pade denom!!");

                if (x <= 25.75d) {
                    ddouble y = sc / (sd * Exp(x * x));

                    return y;
                }
                else {
                    ddouble y = Pow2(Log2(sc / sd) - x * x * LbE);

                    return y;
                }
            }

            public static ddouble ErfcxGtP5(ddouble x) {
                ddouble w = x - 0.5d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = GtP5PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfcx x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble ErfcxGt1(ddouble x) {
                ddouble w = x - 1d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt1PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfcx x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble ErfcxGt2(ddouble x) {
                ddouble w = x - 2d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt2PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfcx x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble ErfcxGt4(ddouble x) {
                ddouble w = x - 4d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt4PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfcx x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble ErfcxGt8(ddouble x) {
                ddouble w = x - 8d;

                Debug.Assert(w >= 0d, nameof(w));

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Gt8PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Erfcx x={x}] Too small pade denom!!");

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble ErfcxGt16(ddouble x) {
                Debug.Assert(x >= 16d, nameof(x));

                ddouble f = 1d, w = x * x;

                const int n = 5;
                for (int k = 4 * n - 3; k >= 1; k -= 4) {
                    ddouble c0 = (k + 2) * f;
                    ddouble c1 = w * ((k + 3) + Ldexp(f, 1));
                    ddouble d0 = (k + 1) * (k + 3) + (4 * k + 6) * f;
                    ddouble d1 = Ldexp(c1, 1);

                    f = w + k * (c0 + c1) / (d0 + d1);
                }

                ddouble y = x * RcpSqrtPi / f;

                return IsNaN(y) ? PlusZero : y;
            }
        }

        internal static partial class Consts {
            public static class Erf {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> PadeTable;
                public static readonly ddouble RcpSqrtPi = 1d / Sqrt(Pi);

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
