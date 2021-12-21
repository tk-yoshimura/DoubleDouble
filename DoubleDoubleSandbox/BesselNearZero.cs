using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class BesselNearZero {
        private static Dictionary<ddouble, DoubleFactDenomTable> dfactdenom_coef_table = new();
        private static Dictionary<ddouble, X2DenomTable> x2denom_coef_table = new();
        private static Dictionary<ddouble, GammaDenomTable> gammadenom_coef_table = new();
        private static YCoefTable y_coef_table = new();
        private static Y0CoefTable y0_coef_table = new();
        private static Y1CoefTable y1_coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselJ(-nu, z, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                (ddouble c, int terms) = BesselJICoef(nu, z, sign_switch: true, max_terms);

                ddouble t = ddouble.Pow(z / 2, nu) * c;

                return (t, terms);
            }
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble z, int max_terms = 64) {
            if (nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);

                (ddouble c, int terms) = BesselYCoef(n, z, max_terms);

                return (c, terms);
            }
            else {
                (ddouble c, int terms) = BesselYCoef(nu, z, max_terms);

                return (c, terms);
            }
        }

        public static (ddouble c, int terms) BesselJICoef(ddouble nu, ddouble z, bool sign_switch, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(nu)) {
                dfactdenom_coef_table.Add(nu, new DoubleFactDenomTable(nu));
            }
            if (!x2denom_coef_table.ContainsKey(nu)) {
                x2denom_coef_table.Add(nu, new X2DenomTable(nu));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[nu];
            X2DenomTable d = x2denom_coef_table[nu];

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

        public static (ddouble c, int terms) BesselYCoef(ddouble nu, ddouble z, int max_terms = 64) {
            if (!gammadenom_coef_table.ContainsKey(nu)) {
                gammadenom_coef_table.Add(nu, new GammaDenomTable(nu));
            }
            if (!gammadenom_coef_table.ContainsKey(-nu)) {
                gammadenom_coef_table.Add(-nu, new GammaDenomTable(-nu));
            }
            if (!x2denom_coef_table.ContainsKey(nu)) {
                x2denom_coef_table.Add(nu, new X2DenomTable(nu));
            }
            if (!x2denom_coef_table.ContainsKey(-nu)) {
                x2denom_coef_table.Add(-nu, new X2DenomTable(-nu));
            }

            YCoefTable r = y_coef_table;
            X2DenomTable dp = x2denom_coef_table[nu], dn = x2denom_coef_table[-nu];
            GammaDenomTable gp = gammadenom_coef_table[nu], gn = gammadenom_coef_table[-nu];

            ddouble cos = ddouble.CosPI(nu), sin = ddouble.SinPI(nu);
            ddouble tp = ddouble.Pow(z / 2, nu), tn = 1d / tp, fp = tp * cos;

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

        public static (ddouble c, int terms) BesselYCoef(int n, ddouble z, int max_terms = 64) {
            if (n < 0) {
                (ddouble y, int terms) = BesselYCoef(-n, z, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                if (n == 0) {
                    return BesselY0Coef(z, max_terms);
                }
                if (n == 1) {
                    return BesselY1Coef(z, max_terms);
                }
            }

            ddouble v = 1d / z;
            (ddouble y0, int y0_terms) = BesselY0Coef(z, max_terms);
            (ddouble y1, int y1_terms) = BesselY1Coef(z, max_terms);

            for (int k = 1; k < n; k++) {
                (y1, y0) = ((2 * k) * v * y1 - y0, y1);
            }

            return (y1, Math.Max(y0_terms, y1_terms));
        }

        public static (ddouble c, int terms) BesselY0Coef(ddouble z, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(0)) {
                dfactdenom_coef_table.Add(0, new DoubleFactDenomTable(0));
            }
            if (!x2denom_coef_table.ContainsKey(0)) {
                x2denom_coef_table.Add(0, new X2DenomTable(0));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[0];
            X2DenomTable d = x2denom_coef_table[0];
            Y0CoefTable q = y0_coef_table;

            ddouble h = ddouble.Log(z / 2) + ddouble.EulerGamma;

            ddouble z2 = z * z, z4 = z2 * z2;

            ddouble c = 0d, u = 2 * ddouble.RcpPI;

            for (int k = 0; k <= max_terms; k++) {                
                ddouble dc = u * r[k] * ((h - ddouble.HarmonicNumber(2 * k)) * (1 - z2 * d[k]) + z2 * q[k]);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= z4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselY1Coef(ddouble z, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(1)) {
                dfactdenom_coef_table.Add(1, new DoubleFactDenomTable(1));
            }
            if (!x2denom_coef_table.ContainsKey(1)) {
                x2denom_coef_table.Add(1, new X2DenomTable(1));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[1];
            X2DenomTable d = x2denom_coef_table[1];
            Y1CoefTable q = y1_coef_table;

            ddouble h = ddouble.Ldexp(ddouble.Log(z / 2) + ddouble.EulerGamma, 1);

            ddouble z2 = z * z, z4 = z2 * z2;

            ddouble c = -2 / (z * ddouble.PI), u = z / (2 * ddouble.PI);

            for (int k = 0; k <= max_terms; k++) {                
                ddouble dc = u * r[k] * ((h - ddouble.HarmonicNumber(2 * k) - ddouble.HarmonicNumber(2 * k + 1)) * (1 - z2 * d[k]) + z2 * q[k]);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= z4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public class DoubleFactDenomTable {
            private readonly ddouble nu;
            private readonly List<ddouble> c_table = new(), a_table = new();

            public DoubleFactDenomTable(ddouble nu) {
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

        public class X2DenomTable {
            private readonly ddouble nu;
            private readonly List<ddouble> a_table = new();

            public X2DenomTable(ddouble nu) {
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

        public class GammaDenomTable {
            private readonly ddouble nu;
            private readonly List<ddouble> c_table = new(), a_table = new();

            public GammaDenomTable(ddouble nu) {
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

        public class Y0CoefTable {
            private readonly List<ddouble> c_table = new();

            public Y0CoefTable() {
                this.c_table.Add(1d / 4d);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < c_table.Count) {
                    return c_table[n];
                }

                for (int k = c_table.Count; k <= n; k++) {
                    ddouble c = ddouble.Rcp(checked(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 1)));

                    c_table.Add(c);
                }

                return c_table[n];
            }
        }

        public class Y1CoefTable {
            private readonly List<ddouble> c_table = new();

            public Y1CoefTable() {
                this.c_table.Add(3d / 16d);
            }

            public ddouble this[int n] => Value(n);

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < c_table.Count) {
                    return c_table[n];
                }

                for (int k = c_table.Count; k <= n; k++) {
                    ddouble c = (ddouble)(4 * k + 3) / checked(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 2) * (2 * k + 2));

                    c_table.Add(c);
                }

                return c_table[n];
            }
        }
    }
}
