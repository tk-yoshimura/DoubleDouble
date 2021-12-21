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
        private static KCoefTable k_coef_table = new();
        private static K0CoefTable k0_coef_table = new();
        private static K1CoefTable k1_coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble x, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselJ(-nu, x, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                (ddouble y, int terms) = BesselJICoef(nu, x, sign_switch: true, max_terms);

                return (y, terms);
            }
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble x, int max_terms = 64) {
            if (nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble c, int terms) = BesselYCoef(n, x, max_terms);

                return (c, terms);
            }
            else {
                (ddouble c, int terms) = BesselYCoef(nu, x, max_terms);

                return (c, terms);
            }
        }

        public static (ddouble y, int terms) BesselI(ddouble nu, ddouble x, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselI(-nu, x, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                (ddouble y, int terms) = BesselJICoef(nu, x, sign_switch: false, max_terms);

                return (y, terms);
            }
        }

        public static (ddouble y, int terms) BesselK(ddouble nu, ddouble x, int max_terms = 64) {
            if (nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble c, int terms) = BesselKCoef(n, x, max_terms);

                return (c, terms);
            }
            else {
                (ddouble c, int terms) = BesselKCoef(nu, x, max_terms);

                return (c, terms);
            }
        }

        public static (ddouble c, int terms) BesselJICoef(ddouble nu, ddouble x, bool sign_switch, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(nu)) {
                dfactdenom_coef_table.Add(nu, new DoubleFactDenomTable(nu));
            }
            if (!x2denom_coef_table.ContainsKey(nu)) {
                x2denom_coef_table.Add(nu, new X2DenomTable(nu));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[nu];
            X2DenomTable d = x2denom_coef_table[nu];

            ddouble x2 = x * x, x4 = x2 * x2;

            ddouble c = 0d, u = ddouble.Pow(x / 2, nu);

            for (int k = 0; k <= max_terms; k++) {
                ddouble w = x2 * d[k];
                ddouble dc = u * r[k] * (sign_switch ? (1d - w) : (1d + w));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselYCoef(ddouble nu, ddouble x, int max_terms = 64) {
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
            ddouble tp = ddouble.Pow(x / 2, nu), tn = 1d / tp, fp = tp * cos;

            ddouble x2 = x * x, x4 = x2 * x2;

            ddouble c = 0d, u = 1d / sin;

            for (int k = 0; k <= max_terms; k++) {
                ddouble dc = u * r[k] * (fp * gp[2 * k] * (1 - x2 * dp[k]) - tn * gn[2 * k] * (1 - x2 * dn[k]));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselYCoef(int n, ddouble x, int max_terms = 64) {
            if (n < 0) {
                (ddouble y, int terms) = BesselYCoef(-n, x, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                if (n == 0) {
                    return BesselY0Coef(x, max_terms);
                }
                if (n == 1) {
                    return BesselY1Coef(x, max_terms);
                }
            }

            ddouble v = 1d / x;
            (ddouble y0, int y0_terms) = BesselY0Coef(x, max_terms);
            (ddouble y1, int y1_terms) = BesselY1Coef(x, max_terms);

            for (int k = 1; k < n; k++) {
                (y1, y0) = ((2 * k) * v * y1 - y0, y1);
            }

            return (y1, Math.Max(y0_terms, y1_terms));
        }

        public static (ddouble c, int terms) BesselY0Coef(ddouble x, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(0)) {
                dfactdenom_coef_table.Add(0, new DoubleFactDenomTable(0));
            }
            if (!x2denom_coef_table.ContainsKey(0)) {
                x2denom_coef_table.Add(0, new X2DenomTable(0));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[0];
            X2DenomTable d = x2denom_coef_table[0];
            Y0CoefTable q = y0_coef_table;

            ddouble h = ddouble.Log(x / 2) + ddouble.EulerGamma;

            ddouble x2 = x * x, x4 = x2 * x2;

            ddouble c = 0d, u = 2 * ddouble.RcpPI;

            for (int k = 0; k <= max_terms; k++) {                
                ddouble dc = u * r[k] * ((h - ddouble.HarmonicNumber(2 * k)) * (1 - x2 * d[k]) + x2 * q[k]);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselY1Coef(ddouble x, int max_terms = 64) {
            if (!dfactdenom_coef_table.ContainsKey(1)) {
                dfactdenom_coef_table.Add(1, new DoubleFactDenomTable(1));
            }
            if (!x2denom_coef_table.ContainsKey(1)) {
                x2denom_coef_table.Add(1, new X2DenomTable(1));
            }

            DoubleFactDenomTable r = dfactdenom_coef_table[1];
            X2DenomTable d = x2denom_coef_table[1];
            Y1CoefTable q = y1_coef_table;

            ddouble h = ddouble.Ldexp(ddouble.Log(x / 2) + ddouble.EulerGamma, 1);

            ddouble x2 = x * x, x4 = x2 * x2;

            ddouble c = -2 / (x * ddouble.PI), u = x / (2 * ddouble.PI);

            for (int k = 0; k <= max_terms; k++) {                
                ddouble dc = u * r[k] * ((h - ddouble.HarmonicNumber(2 * k) - ddouble.HarmonicNumber(2 * k + 1)) * (1 - x2 * d[k]) + x2 * q[k]);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselKCoef(ddouble nu, ddouble x, int max_terms = 64) {
            if (!gammadenom_coef_table.ContainsKey(nu)) {
                gammadenom_coef_table.Add(nu, new GammaDenomTable(nu));
            }
            if (!gammadenom_coef_table.ContainsKey(-nu)) {
                gammadenom_coef_table.Add(-nu, new GammaDenomTable(-nu));
            }

            KCoefTable r = k_coef_table;
            GammaDenomTable gp = gammadenom_coef_table[nu], gn = gammadenom_coef_table[-nu];

            ddouble tp = ddouble.Pow(x / 2, nu), tn = 1d / tp;

            ddouble x2 = x * x;

            ddouble c = 0d, u = ddouble.PI / (2 * ddouble.SinPI(nu));

            for (int k = 0; k <= max_terms; k++) {
                ddouble dc = u * r[k] * (tn * gn[k] - tp * gp[k]);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x2;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselKCoef(int n, ddouble x, int max_terms = 64) {
            n = Math.Abs(n);
            if (n == 0) {
                return BesselK0Coef(x, max_terms);
            }
            if (n == 1) {
                return BesselK1Coef(x, max_terms);
            }
            
            ddouble v = 1d / x;
            (ddouble y0, int y0_terms) = BesselK0Coef(x, max_terms);
            (ddouble y1, int y1_terms) = BesselK1Coef(x, max_terms);
            
            for (int k = 1; k < n; k++) {
                (y1, y0) = ((2 * k) * v * y1 + y0, y1);
            }
            
            return (y1, Math.Max(y0_terms, y1_terms));
        }

        public static (ddouble c, int terms) BesselK0Coef(ddouble x, int max_terms = 64) {
            K0CoefTable r = k0_coef_table;
            ddouble h = - ddouble.Log(x / 2) - ddouble.EulerGamma;

            ddouble x2 = x * x;

            ddouble c = 0d, u = 1d;

            for (int k = 0; k <= max_terms; k++) {
                ddouble dc = u * r[k] * (h + ddouble.HarmonicNumber(k));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x2;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselK1Coef(ddouble x, int max_terms = 64) {
            K1CoefTable r = k1_coef_table;
            ddouble h = ddouble.Log(x / 2) + ddouble.EulerGamma;

            ddouble x2 = x * x;

            ddouble c = 1d / x, u = x / 2d;

            for (int k = 0; k <= max_terms; k++) {
                ddouble dc = u * r[k] * (h - (ddouble.HarmonicNumber(k) + ddouble.HarmonicNumber(k + 1)) / 2);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= x2;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public class DoubleFactDenomTable {
            private ddouble c;
            private readonly ddouble nu;
            private readonly List<ddouble> a_table = new();

            public DoubleFactDenomTable(ddouble nu) {
                this.c = ddouble.Gamma(nu + 1);
                this.nu = nu;
                this.a_table.Add(ddouble.Rcp(c));
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
                    c *= (nu + (2 * k)) * (nu + (2 * k - 1)) * checked(32 * k * (2 * k - 1));

                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }

        public class X2DenomTable {
            private readonly ddouble nu;
            private readonly List<ddouble> a_table = new();

            public X2DenomTable(ddouble nu) {
                ddouble a = ddouble.Rcp(4 * (nu + 1));

                this.nu = nu;
                this.a_table.Add(a);
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
                    ddouble a = ddouble.Rcp(4 * (2 * k + 1) * (2 * k + nu + 1));

                    a_table.Add(a);
                }

                return a_table[n];
            }
        }

        public class GammaDenomTable {
            private ddouble c;
            private readonly ddouble nu;
            private readonly List<ddouble> a_table = new();

            public GammaDenomTable(ddouble nu) {
                this.c = ddouble.Gamma(nu + 1);
                this.nu = nu;
                this.a_table.Add(ddouble.Rcp(c));
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
                    c *= nu + k;

                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }

        public class YCoefTable {
            private ddouble c;
            private readonly List<ddouble> a_table = new();

            public YCoefTable() {
                this.c = 1d;
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

                for (int k = a_table.Count; k <= n; k++) {
                    c *= checked(32 * k * (2 * k - 1));

                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }

        public class Y0CoefTable {
            private readonly List<ddouble> c_table = new();

            public Y0CoefTable() {
                this.c_table.Add(ddouble.Rcp(4));
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
                this.c_table.Add(ddouble.Ldexp(3, -4));
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
                    ddouble c = (ddouble)(4 * k + 3) / (ddouble)checked(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 2) * (2 * k + 2));

                    c_table.Add(c);
                }

                return c_table[n];
            }
        }

        public class KCoefTable {
            private ddouble c;
            private readonly List<ddouble> a_table = new();

            public KCoefTable() {
                this.c = 1d;
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

                for (int k = a_table.Count; k <= n; k++) {
                    c *= checked(4 * k);
                    
                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }

        public class K0CoefTable {
            private ddouble c;
            private readonly List<ddouble> a_table = new();

            public K0CoefTable() {
                this.c = 1;
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

                for (int k = a_table.Count; k <= n; k++) {
                    c *= checked(4 * k * k);

                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }

        public class K1CoefTable {
            private ddouble c;
            private readonly List<ddouble> a_table = new();

            public K1CoefTable() {
                this.c = 1;
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

                for (int k = a_table.Count; k <= n; k++) {
                    c *= checked(4 * k * (k + 1));

                    a_table.Add(ddouble.Rcp(c));
                }

                return a_table[n];
            }
        }
    }
}
