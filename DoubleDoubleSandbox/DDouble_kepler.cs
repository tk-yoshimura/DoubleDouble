using DoubleDouble;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class KeplerSandbox {
        public static ddouble KeplerE(ddouble m, ddouble e) {
            if (!(e >= 0) || e > 1 || !ddouble.IsFinite(m)) {
                return NaN;
            }

            //if (e > 0.96875) {
            //    throw new ArgumentOutOfRangeException(
            //        nameof(e),
            //        "In the calculation of the KeplerE function, eccentricity greater than 0.96875 is not supported."
            //    );
            //}

            if (m == 0) {
                return Zero;
            }


            ddouble t = m * RcpPI * 0.5, s = (t - Round(t)) * (2 * PI);
            ddouble r;

            if (s < -PI) {
                r = -KeplerEUtil.RootFinding(-2 * PI - s, e);
            }
            else if (s < 0) {
                r = -KeplerEUtil.RootFinding(-s, e);
            }
            else if (s < PI) {
                r = KeplerEUtil.RootFinding(s, e);
            }
            else {
                r = -KeplerEUtil.RootFinding(2 * PI - s, e);
            }

            ddouble y = m + r;

            return y;
        }

        public static class KeplerEUtil {

            public static ddouble RootFinding(ddouble m, ddouble e) {
#if DEBUG
                if (m < 0 || !(m <= PI)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
                if (!(e >= 0) || e > 1) {
                    throw new ArgumentOutOfRangeException(nameof(e));
                }
#endif

                if (m >= PI / 4) { 
                    return HalleyRootFinding(m, e);
                }

                if (e < 0.9375) {
                    return HalleyRootFinding(m, e);
                }
                else {
                    return SqrtNewtonRootFinding(m, e);
                }
            }

            public static ddouble HalleyRootFinding(ddouble m, ddouble e) {
#if DEBUG
                if (m < 0 || !(m <= PI)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
                if (!(e >= 0) || e > 1) {
                    throw new ArgumentOutOfRangeException(nameof(e));
                }
#endif
                double xd = InitValue(m.Hi, e.Hi);

                for (int i = 0; i < 16; i++) {
                    (double sin, double cos) = (Math.Sin(xd), Math.Cos(xd));
                    double esin = e.Hi * sin, ecos = e.Hi * cos, u = xd - esin;
                    double dx = (m.Hi - u) * (1.0 + ecos) / (1.0 - ecos * ecos);

                    if (!double.IsFinite(dx)) {
                        break;
                    }

                    xd += dx;

                    Console.WriteLine($"double iter {dx}");

                    if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                        break;
                    }
                }

                ddouble x = xd;

                for (int i = 0; i < 2; i++) {
                    (ddouble sin, ddouble cos) = (Sin(x), Cos(x));
                    ddouble esin = e * sin, ecos = e * cos, u = x - esin;
                    ddouble dx = (m - u) * (1.0 + ecos) / (1.0 - ecos * ecos);

                    if (!IsFinite(dx)) {
                        break;
                    }

                    x += dx;

                    Console.WriteLine($"ddouble iter {dx}");

                    if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                        break;
                    }
                }

                ddouble y = x - m;

                return y;
            }

            public static ddouble SqrtNewtonRootFinding(ddouble m, ddouble e) {
#if DEBUG
                if (m < 0 || !(m <= PI / 4)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
                if (!(e >= 0.9375) || e > 1) {
                    throw new ArgumentOutOfRangeException(nameof(e));
                }
#endif

                ddouble sqrt_m = Sqrt(m);

                double xd = Math.Sqrt(InitValue(m.Hi, e.Hi));

                for (int i = 0; i < 16; i++) {
                    (double sin, double cos) = (Math.Sin(xd), Math.Cos(xd));
                    double esin = e.Hi * sin, ecos = e.Hi * cos, u = xd - esin;
                    double v = Math.Sign(u) * Math.Sqrt(Math.Abs(u));

                    double dx = (sqrt_m.Hi - v) * (2.0 * v) / (1.0 - ecos);

                    if (!double.IsFinite(dx)) {
                        break;
                    }

                    xd = Math.Max(0.0, xd + dx);

                    Console.WriteLine($"double iter {dx}");

                    if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                        break;
                    }
                }

                ddouble x = xd;

                for (int i = 0; i < 3; i++) {
                    (ddouble sin, ddouble cos) = (Sin(x), Cos(x));
                    ddouble esin = e * sin, ecos = e * cos, u = x - esin;
                    ddouble v = u.Sign * Sqrt(Abs(u));

                    ddouble dx = (sqrt_m - v) * (2.0 * v) / (1.0 - ecos);

                    if (!ddouble.IsFinite(dx)) {
                        break;
                    }

                    x = Max(0.0, x + dx);

                    Console.WriteLine($"ddouble iter {dx}");

                    if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                        break;
                    }
                }

                ddouble y = x - m;

                return y;
            }

            public static double InitValue(double m, double e) {
#if DEBUG
                if (m < 0 || !(m <= PI)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
#endif

                double s = Math.Cbrt(6.0 * m), s2 = s * s;
                double d = s * (1.0 + 
                           s2 * (1.6666666666666666667e-2 + 
                           s2 * (7.1428571428571428571e-4 + 
                           s2 * (7.4801639817849724809e-5))));

                double x = Math.Min(m / (1.0 - e), d * e + m * (1.0 - e));

                return x;
            }
        }
    }
}