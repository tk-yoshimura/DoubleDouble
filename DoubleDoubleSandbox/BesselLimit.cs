using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal static class BesselLimit {
        private static Dictionary<ddouble, CoefTable> coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            (ddouble x, ddouble y, int terms) = BesselJYCoef(nu, z, max_terms);

            ddouble omega = z - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = x * ddouble.Cos(omega) - y * ddouble.Sin(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * z));

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble z, int max_terms = 64) {
            (ddouble x, ddouble y, int terms) = BesselJYCoef(nu, z, max_terms);

            ddouble omega = z - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = x * ddouble.Sin(omega) + y * ddouble.Cos(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * z));

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselI(ddouble nu, ddouble z, int max_terms = 64) {
            (ddouble c, int terms) = BesselIKCoef(nu, z, sign_switch: true, max_terms);

            ddouble t = c * ddouble.Exp(z) / ddouble.Sqrt(2 * ddouble.PI * z);

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselK(ddouble nu, ddouble z, int max_terms = 256) {
            (ddouble c, int terms) = BesselIKCoef(nu, z, sign_switch: false, max_terms);

            ddouble t = c * ddouble.Exp(-z) * ddouble.Sqrt(ddouble.PI / (2 * z));

            return (t, terms);
        }

        public static (ddouble x, ddouble y, int terms) BesselJYCoef(ddouble nu, ddouble z, int max_terms = 64) {
            if (!coef_table.ContainsKey(nu)) {
                coef_table.Add(nu, new CoefTable(nu));
            }

            CoefTable table = coef_table[nu];

            ddouble squa_nu4 = 4d * nu * nu;
            ddouble v = 1d / z, v2 = v * v, v4 = v2 * v2;

            ddouble x = 0d, y = 0d, p = 1d, q = v;

            static int square(int n) => checked(n * n);

            for (int k = 0; k <= max_terms; k++) {
                (int xexp, ddouble xf) = table.Value(k * 4);
                (int yexp, ddouble yf) = table.Value(k * 4 + 1);

                ddouble dx = p * xf *
                    (1d - v2 * (squa_nu4 - square(8 * k + 1)) * (squa_nu4 - square(8 * k + 3)) / (64 * (4 * k + 1) * (4 * k + 2)));
                ddouble dy = q * yf *
                    (1d - v2 * (squa_nu4 - square(8 * k + 3)) * (squa_nu4 - square(8 * k + 5)) / (64 * (4 * k + 2) * (4 * k + 3)));

                dx = ddouble.Ldexp(dx, xexp);
                dy = ddouble.Ldexp(dy, yexp);

                ddouble x_next = x + dx;
                ddouble y_next = y + dy;

                if (x == x_next && y == y_next) {
                    return (x, y, k);
                }

                p *= v4;
                q *= v4;
                x = x_next;
                y = y_next;
            }

            return (ddouble.NaN, ddouble.NaN, int.MaxValue);
        }

        public static (ddouble c, int terms) BesselIKCoef(ddouble nu, ddouble z, bool sign_switch, int max_terms = 256) {
            if (!coef_table.ContainsKey(nu)) {
                coef_table.Add(nu, new CoefTable(nu));
            }

            CoefTable table = coef_table[nu];

            ddouble squa_nu4 = 4d * nu * nu;
            ddouble v = 1d / z, v2 = v * v;

            ddouble c = 0d, u = 1d;

            static int square(int n) => checked(n * n);

            for (int k = 0; k <= max_terms; k++) {
                (int exp, ddouble f) = table.Value(k * 2);

                ddouble w = v * (squa_nu4 - square(4 * k + 1)) / (8 * (2 * k + 1));
                ddouble dc = u * f * (sign_switch ? (1d - w) : (1d + w));

                dc = ddouble.Ldexp(dc, exp);

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= v2;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        private class CoefTable {
            private readonly ddouble squa_nu4;
            private readonly List<(int exp, ddouble value)> a_table = new();

            public CoefTable(ddouble nu) {
                this.squa_nu4 = 4 * nu * nu;

                ddouble a1 = ddouble.Ldexp(squa_nu4 - 1, -3);

                this.a_table.Add((0, 1));
                this.a_table.Add(ddouble.Frexp(a1));
            }

            public (int exp, ddouble value) Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = a_table.Count; k <= n; k++) {
                    ddouble a = a_table.Last().value * (squa_nu4 - checked((2 * k - 1) * (2 * k - 1))) / checked(k * 8);

                    (int exp, ddouble value) = ddouble.Frexp(a);
                    exp += a_table.Last().exp;

                    a_table.Add((exp, value));
                }

                return a_table[n];
            }
        }
    }
}
