using System;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble IncompleteBeta(ddouble x, ddouble a, ddouble b) {
            if (x < 0d || !(x <= 1d) || !(a > 0d) || !(b > 0d)) {
                return NaN;
            }

            if (a > IncompleteBetaCFrac.MaxAB || b > IncompleteBetaCFrac.MaxAB) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteBeta function, " +
                    $"{nameof(a)} or {nameof(b)} greater than {IncompleteBetaCFrac.MaxAB} is not supported."
                );
            }

            if (IsZero(x)) {
                return Zero;
            }
            if (x == 1d) {
                return Beta(a, b);
            }

            return IncompleteBetaCFrac.Value(x, a, b);
        }

        internal static class IncompleteBetaCFrac {
            public const double MaxAB = 64;

            public static ddouble Value(ddouble x, ddouble a, ddouble b) {
                int m = ConvergenceCfracM((double)a, (double)b);
                double thr = ConvergenceThreshold((double)a, (double)b);

                if (x < thr) {
                    return Kernel(x, a, b, m);
                }
                else {
                    ddouble f = Beta(a, b), c = Kernel(1d - x, b, a, m);

                    if (f * 0.75 > c) {
                        return f - c;
                    }

                    // When a << b and x near threshold has a large digit loss, recalculation is performed.
                    return Kernel(x, a, b, m: 128);
                }
            }

            private static ddouble Kernel(ddouble x, ddouble a, ddouble b, int m) {
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

            private static double ConvergenceThreshold(double a, double b) {
                const double p1 = -3.06204161e-02;
                const double p2 = 4.97686689e-03;
                const double p3 = -8.02053481e-05;
                const double p4 = 4.39088250e-07;

                double ap = 1d + (a * (p1 + a * (p2 + a * (p3 + a * p4))));
                double bp = 1d + (b * (p1 + b * (p2 + b * (p3 + b * p4))));

                double y = ap / (ap + bp);

                return y;
            }

            private static int ConvergenceCfracM(double a, double b) {
                double c = Math.Max(a, b);

                if (c <= 21.5d) {
                    return 21;
                }
                if (c <= 23.5d) {
                    return 22;
                }
                if (c <= 25.5d) {
                    return 23;
                }
                if (c <= 28.5d) {
                    return 24;
                }
                if (c <= 31.0d) {
                    return 25;
                }
                if (c <= 34.0d) {
                    return 26;
                }
                if (c <= 37.5d) {
                    return 27;
                }
                if (c <= 41.5d) {
                    return 28;
                }
                if (c <= 45.0d) {
                    return 29;
                }
                if (c <= 49.5d) {
                    return 30;
                }
                if (c <= 54.0d) {
                    return 31;
                }
                if (c <= 59.0d) {
                    return 32;
                }
                if (c <= MaxAB) {
                    return 33;
                }

                throw new NotImplementedException();
            }
        }
    }
}