using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class BesselNearZero {
        private static Dictionary<ddouble, JICoefTable> ji_coef_table = new();
        private static Dictionary<ddouble, X2DenomCoefTable> x2denom_coef_table = new();
        private static Dictionary<ddouble, GammaDenomCoefTable> gammadenom_coef_table = new();
        private static YCoefTable y_coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselJ(-nu, z, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                (ddouble c, int terms) = BesselJICoef(nu, z, sign_switch: true, max_terms);

                ddouble t = ddouble.Pow(ddouble.Ldexp(z, -1), nu) * c;

                return (t, terms);
            }
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble z, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselY(-nu, z, max_terms);
                
                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else if (nu == ddouble.Floor(nu)) {
                return (ddouble.NaN, max_terms);
            }
            else {
                (ddouble c, int terms) = BesselYCoef(nu, z, sign_switch: true, max_terms);

                return (c, terms);
            }
        }

        public static (ddouble c, int terms) BesselJICoef(ddouble nu, ddouble z, bool sign_switch, int max_terms = 64) {
            if (!ji_coef_table.ContainsKey(nu)) {
                ji_coef_table.Add(nu, new JICoefTable(nu));
            }
            if (!x2denom_coef_table.ContainsKey(nu)) {
                x2denom_coef_table.Add(nu, new X2DenomCoefTable(nu));
            }

            JICoefTable r = ji_coef_table[nu];
            X2DenomCoefTable d = x2denom_coef_table[nu];

            ddouble z2 = z * z, z4 = z2 * z2;

            ddouble c = 0d, u = 1d;

            for (int k = 0; k <= max_terms; k++) {
                ddouble w = z2 * d[k];
                ddouble dc = u * r[k] * (sign_switch ? (1d - w) : (1d + w));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= z4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselYCoef(ddouble nu, ddouble z, bool sign_switch, int max_terms = 64) {
            if (!gammadenom_coef_table.ContainsKey(nu)) {
                gammadenom_coef_table.Add(nu, new GammaDenomCoefTable(nu));
            }
            if (!gammadenom_coef_table.ContainsKey(-nu)) {
                gammadenom_coef_table.Add(-nu, new GammaDenomCoefTable(-nu));
            }
            if (!x2denom_coef_table.ContainsKey(nu)) {
                x2denom_coef_table.Add(nu, new X2DenomCoefTable(nu));
            }
            if (!x2denom_coef_table.ContainsKey(-nu)) {
                x2denom_coef_table.Add(-nu, new X2DenomCoefTable(-nu));
            }

            YCoefTable r = y_coef_table;
            X2DenomCoefTable dp = x2denom_coef_table[nu], dn = x2denom_coef_table[-nu];
            GammaDenomCoefTable gp = gammadenom_coef_table[nu], gn = gammadenom_coef_table[-nu];

            ddouble cos = ddouble.CosPI(nu), sin = ddouble.SinPI(nu);
            ddouble tp = ddouble.Pow(ddouble.Ldexp(z, -1), nu), tn = 1d / tp, fp = tp * cos;

            ddouble z2 = z * z, z4 = z2 * z2;

            ddouble c = 0d, u = 1d;

            for (int k = 0; k <= max_terms; k++) {
                ddouble dc = u * r[k] * (fp * gp[k] * (1 - z2 * dp[k]) - tn * gn[k] * (1 - z2 * dn[k]));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c / sin, k);
                }

                c = c_next;
                u *= z4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public class JICoefTable {
            private readonly ddouble nu;
            private readonly List<ddouble> c_table = new(), a_table = new();

            public JICoefTable(ddouble nu) {
                ddouble c = ddouble.Gamma(nu + 1);

                this.nu = nu;
                this.c_table.Add(c);
                this.a_table.Add(1d / c);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = c_table.Count; k <= n; k++) {
                    ddouble c = c_table.Last() * (nu + (2 * k - 1)) * (nu + (2 * k)) * checked(32 * k * (2 * k - 1));

                    c_table.Add(c);
                    a_table.Add(1d / c);
                }

                return a_table[n];
            }
        }

        public class X2DenomCoefTable {
            private readonly ddouble nu;
            private readonly List<ddouble> a_table = new();

            public X2DenomCoefTable(ddouble nu) {
                ddouble f = 1d / (4 * (nu + 1));

                this.nu = nu;
                this.a_table.Add(f);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = a_table.Count; k <= n; k++) {
                    ddouble f = 1d / (4 * (2 * k + 1) * (2 * k + nu + 1));

                    a_table.Add(f);
                }

                return a_table[n];
            }
        }

        public class GammaDenomCoefTable {
            private readonly ddouble nu;
            private readonly List<ddouble> c_table = new(), a_table = new();

            public GammaDenomCoefTable(ddouble nu) {
                ddouble c = ddouble.Gamma(nu + 1);

                this.nu = nu;
                this.c_table.Add(c);
                this.a_table.Add(1d / c);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = a_table.Count; k <= n; k++) {
                    ddouble c = c_table.Last() * (nu + (2 * k - 1)) * (nu + (2 * k));

                    c_table.Add(c);
                    a_table.Add(1d / c);
                }

                return a_table[n];
            }
        }

        public class YCoefTable {
            private readonly List<ddouble> c_table = new(), a_table = new();

            public YCoefTable() {
                this.c_table.Add(1d);
                this.a_table.Add(1d);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = c_table.Count; k <= n; k++) {
                    ddouble c = c_table.Last() * checked(32 * k * (2 * k - 1));
                    ddouble f = 1d / c;

                    c_table.Add(c);
                    a_table.Add(1d / c);
                }

                return a_table[n];
            }
        }
    }
}
