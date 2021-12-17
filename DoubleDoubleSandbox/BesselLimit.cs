using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal static class BesselLimit {
        private static Dictionary<ddouble, CoefTable> coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble x, int max_terms = 64) {
            (ddouble c, ddouble s, int terms) = BesselJYCoef(nu, x, max_terms);

            ddouble omega = x - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = c * ddouble.Cos(omega) - s * ddouble.Sin(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * x));

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble x, int max_terms = 64) {
            (ddouble s, ddouble c, int terms) = BesselJYCoef(nu, x, max_terms);

            ddouble omega = x - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = s * ddouble.Sin(omega) + c * ddouble.Cos(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * x));

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselI(ddouble nu, ddouble x, bool scale = false, int max_terms = 64) {
            (ddouble c, int terms) = BesselIKCoef(nu, x, sign_switch: true, max_terms);

            ddouble t = c / ddouble.Sqrt(2 * ddouble.PI * x);

            if (scale) {
                t *= ddouble.Exp(x);
            }

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselK(ddouble nu, ddouble x, bool scale = false, int max_terms = 256) {
            (ddouble c, int terms) = BesselIKCoef(nu, x, sign_switch: false, max_terms);

            ddouble t = c * ddouble.Sqrt(ddouble.PI / (2 * x));

            if (scale) {
                t *= ddouble.Exp(-x);
            }

            return (t, terms);
        }

        public static (ddouble s, ddouble t, int terms) BesselJYCoef(ddouble nu, ddouble x, int max_terms = 64) {
            if (!coef_table.ContainsKey(nu)) {
                coef_table.Add(nu, new CoefTable(nu));
            }

            CoefTable table = coef_table[nu];

            ddouble squa_nu4 = 4d * nu * nu;

            ddouble v = 1d / x, v2 = v * v, v4 = v2 * v2;
            ddouble s = 0d, t = 0d, p = 1d, q = v;

            static int square(int n) => checked(n * n);

            for (int k = 0; k <= max_terms; k++) {
                ddouble ds = p * table.Value(k * 4) *
                    (1d - v2 * (squa_nu4 - square(8 * k + 1)) * (squa_nu4 - square(8 * k + 3)) / (64 * (4 * k + 1) * (4 * k + 2)));
                ddouble dt = q * table.Value(k * 4 + 1) *
                    (1d - v2 * (squa_nu4 - square(8 * k + 3)) * (squa_nu4 - square(8 * k + 5)) / (64 * (4 * k + 2) * (4 * k + 3)));

                ddouble s_next = s + ds;
                ddouble t_next = t + dt;

                if (s == s_next && t == t_next) {
                    return ((ddouble)s, (ddouble)t, k);
                }

                p *= v4;
                q *= v4;
                s = s_next;
                t = t_next;
            }

            return (ddouble.NaN, ddouble.NaN, int.MaxValue);
        }

        public static (ddouble r, int terms) BesselIKCoef(ddouble nu, ddouble x, bool sign_switch, int max_terms = 256) {
            if (!coef_table.ContainsKey(nu)) {
                coef_table.Add(nu, new CoefTable(nu));
            }

            CoefTable table = coef_table[nu];

            ddouble squa_nu4 = 4d * nu * nu;

            ddouble v = 1d / x, v2 = v * v;
            ddouble r = 0d, u = 1d;

            static int square(int n) => checked(n * n);

            for (int k = 0; k <= max_terms; k++) {
                ddouble w = v * (squa_nu4 - square(4 * k + 1)) / (8 * (2 * k + 1));
                ddouble dr = u * table.Value(k * 2) * (sign_switch ? (1d - w) : (1d + w));

                ddouble r_next = r + dr;

                if (r == r_next) {
                    return ((ddouble)r, k);
                }

                r = r_next;
                u *= v2;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        private class CoefTable {
            private readonly ddouble squa_nu4;
            private readonly List<ddouble> a_table = new();

            public CoefTable(ddouble nu) {
                this.squa_nu4 = 4 * nu * nu;

                ddouble a1 = ddouble.Ldexp(squa_nu4 - 1, -3);

                this.a_table.Add(1d);
                this.a_table.Add(a1);
            }

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = a_table.Count; k <= n; k++) {
                    ddouble a = a_table.Last() * (squa_nu4 - checked((2 * k - 1) * (2 * k - 1))) / checked(k * 8);

                    a_table.Add(a);
                }

                return a_table[n];
            }
        }
    }
}
