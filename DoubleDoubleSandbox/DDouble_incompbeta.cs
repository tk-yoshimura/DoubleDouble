using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompBetaPrototype {

        public static ddouble Beta(ddouble x, ddouble a, ddouble b, int m) { 
            ddouble c = a + b + 2d;

            ddouble t0 = x / (a + 1d) * c, t1 = (1d - x) / (b + 1d) * c;

            if (t0 < t1) {
                return Cfrac2(x, a, b, m);
            }
            else {
                return ddouble.Beta(a, b) - Cfrac2(1d - x, b, a, m);
            }
        }

        public static ddouble CBeta(ddouble x, ddouble a, ddouble b, int m) { 
            return Cfrac2(1d - x, b, a, m);
        }

        public static ddouble Cfrac(ddouble x, ddouble a, ddouble b, int m) {
            ddouble f = 0d;
            
            for (int n = m; n >= 0; n--) {
                ddouble na = n + a, nb = n - b, nab = n + a + b;
                ddouble n2a = 2 * n + a, n2a1 = n2a + 1d, n2a2 = n2a + 2d;

                ddouble v = (f + 1d) * n2a1 * n2a2;

                f = -(na * nab * x * v) / (n2a * n2a1 * (-(n + 1d) * (nb + 1d) * x + v));
            }

            f = 1d / (1d + f);

            ddouble y = Pow(x, a) * Pow(1d - x, b) / a * f;

            return y;
        }

        public static ddouble Cfrac2(ddouble x, ddouble a, ddouble b, int m) {
            (ddouble s2, ddouble s1, ddouble s0) = (1, 1, 0);
            (ddouble t2, ddouble t1, ddouble t0) = (1, 0, 0);
            
            for (int n = 0; n <= m; n++) {
                ddouble na = n + a, nb = n - b, nab = n + a + b;
                ddouble n2a = 2 * n + a, n2a1 = n2a + 1d, n2a2 = n2a + 2d;

                ddouble p0 = (na * nab * x) / (n2a * n2a1), p1 = ((n + 1d) * (nb + 1d) * x) / (n2a1 * n2a2);

                (s2, s1, s0) = (s2 - p0 * s1, s2, s1);
                (t2, t1, t0) = (t2 - p0 * t1, t2, t1);
                (s2, s1, s0) = (s2 - p1 * s1, s2, s1);
                (t2, t1, t0) = (t2 - p1 * t1, t2, t1);

                ddouble c = 1d / t2;
                
                (s2, s1, s0) = (s2 * c, s1 * c, s0 * c);
                (t2, t1, t0) = (1d, t1 * c, t0 * c);
            }

            ddouble y = Pow(x, a) * Pow(1d - x, b) / (a * (1d + s2));

            return y;
        }

        public static (ddouble y, int m) BetaConvergence(ddouble x, ddouble a, ddouble b, int max_m = 1024, int convchecks = 4) {
            ddouble prev_y = CBeta(x, a, b, 0);
            
            for (int m = 1, convtimes = 0; m <= max_m; m++) {
                ddouble y = CBeta(x, a, b, m);

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
    }
}