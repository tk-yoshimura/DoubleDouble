using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompBetaPrototype {

        public static ddouble Beta(ddouble x, ddouble a, ddouble b, int m) { 
            ddouble c = a + b + 2d;

            ddouble t0 = x / (a + 1d) * c, t1 = (1d - x) / (b + 1d) * c;

            if (t0 < t1) {
                return Cfrac(x, a, b, m);
            }
            else {
                return ddouble.Beta(a, b) - Cfrac(1d - x, b, a, m);
            }
        }

        public static ddouble CBeta(ddouble x, ddouble a, ddouble b, int m, bool complementary) {
            if (!complementary) {
                return Cfrac(x, a, b, m);
            }
            else {
                return Cfrac(1d - x, b, a, m);
            }
        }

        public static ddouble Cfrac(ddouble x, ddouble a, ddouble b, int m) {
            ddouble f = 0d;
            
            for (int n = m; n >= 0; n--) {
                ddouble na = n + a, nb = n - b, nab = n + a + b;
                ddouble n2a = 2 * n + a, n2a1 = n2a + 1d, n2a2 = n2a + 2d;

                ddouble v = (f + 1d) * n2a1 * n2a2;

                f = (na * nab * x * v) / (n2a * n2a1 * ((n + 1d) * (nb + 1d) * x - v));
            }

            f = 1d / (1d + f);

            ddouble y = Pow(x, a) * Pow(1d - x, b) / a * f;

            return y;
        }

        public static (ddouble y, int m) BetaConvergence(ddouble x, ddouble a, ddouble b, bool complementary = false, int max_m = 1024, int convchecks = 4) {
            ddouble prev_y = CBeta(x, a, b, 0, complementary);
            
            for (int m = 1, convtimes = 0; m <= max_m; m++) {
                ddouble y = CBeta(x, a, b, m, complementary);

                if (ddouble.Abs(y / prev_y - 1) < 1e-30) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes > convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static (ddouble x, int m) BetaConvergence(ddouble a, ddouble b) {
            ddouble x = 0.5;
            int m = 0;

            for (ddouble dx = 1d / 4; dx >= Math.ScaleB(1, -32); dx /= 2) {
                int ma = BetaConvergence(x, a, b, complementary: false).m;
                int mb = BetaConvergence(x, a, b, complementary: true).m;

                m = (ma + mb) / 2;

                if (ma > mb + 1) {
                    x -= dx;
                }
                else if (ma < mb + 1) {
                    x += dx;
                }
                else {
                    return (x, m);
                }
            }

            return (x, m);
        }
    }
}