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

        private static class KeplerEUtil {

            public static ddouble RootFinding(ddouble m, ddouble e) {
#if DEBUG
                if (m < 0 || !(m <= PI)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
                if (!(e >= 0) || e > 1) {
                    throw new ArgumentOutOfRangeException(nameof(e));
                }
#endif

                if (e < 0.9375 || m >= PI / 2) {
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

                for (int i = 0; i < 32; i++) {
                    (double sin, double cos) = CrudeSinCos(xd);
                    double ecos = e.Hi * cos;
                    double dx = (m.Hi + e.Hi * sin - xd) * (1.0 + ecos) / (1.0 - ecos * ecos);

                    if (!double.IsFinite(dx)) {
                        break;
                    }

                    xd += dx;

                    //Console.WriteLine($"crude iter {dx}");

                    if (Math.Abs(dx) <= Math.Abs(xd) * 1e-4) {
                        break;
                    }
                }

                for (int i = 0; i < 4; i++) {
                    (double sin, double cos) = (Math.Sin(xd), Math.Cos(xd));
                    double ecos = e.Hi * cos;
                    double dx = (m.Hi + e.Hi * sin - xd) * (1.0 + ecos) / (1.0 - ecos * ecos);

                    if (!double.IsFinite(dx)) {
                        break;
                    }

                    xd += dx;

                    //Console.WriteLine($"double iter {dx}");

                    if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                        break;
                    }
                }

                ddouble x = xd;

                for (int i = 0; i < 2; i++) {
                    (ddouble sin, ddouble cos) = (Sin(x), Cos(x));
                    ddouble ecos = e * cos;
                    ddouble dx = (m + e * sin - x) * (1.0 + ecos) / (1.0 - ecos * ecos);

                    if (!IsFinite(dx)) {
                        break;
                    }

                    x += dx;

                    //Console.WriteLine($"ddouble iter {dx}");

                    if (Abs(dx) <= Abs(xd) * 2.5e-31) {
                        break;
                    }
                }

                ddouble y = x - m;

                return y;
            }

            public static ddouble SqrtNewtonRootFinding(ddouble m, ddouble e) {
#if DEBUG
                if (m < 0 || !(m <= PI / 2)) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }
                if (!(e >= 0.9375) || e > 1) {
                    throw new ArgumentOutOfRangeException(nameof(e));
                }
#endif

                ddouble sqrt_m = Sqrt(m);

                double xd = Math.Sqrt(InitValue(m.Hi, e.Hi));

                for (int i = 0; i < 32; i++) {
                    (double sin, double cos) = CrudeSinCos(xd);
                    double esin = e.Hi * sin, ecos = e.Hi * cos;
                    double v = Math.Sqrt(xd - esin);

                    double dx = (2.0 * v) * (sqrt_m.Hi - v) / (1.0 - ecos);

                    if (!double.IsFinite(dx)) {
                        break;
                    }

                    xd = Math.Max(0.0, xd + dx);

                    Console.WriteLine($"crude iter {dx}");

                    if (Math.Abs(dx) <= Math.Abs(xd) * 1e-4) {
                        break;
                    }
                }

                for (int i = 0; i < 4; i++) {
                    (double sin, double cos) = (Math.Sin(xd), Math.Cos(xd));
                    double esin = e.Hi * sin, ecos = e.Hi * cos;
                    double v = Math.Sqrt(xd - esin);

                    double dx = (2.0 * v) * (sqrt_m.Hi - v) / (1.0 - ecos);

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

                for (int i = 0; i < 2; i++) {
                    (ddouble sin, ddouble cos) = (Sin(x), Cos(x));
                    ddouble esin = e * sin, ecos = e * cos;
                    ddouble v = Sqrt(x - esin);

                    ddouble dx = (2.0 * v) * (sqrt_m - v) / (1.0 - ecos);

                    if (!ddouble.IsFinite(dx)) {
                        break;
                    }

                    x = Max(0.0, x + dx);

                    Console.WriteLine($"ddouble iter {dx}");

                    if (Abs(dx) <= Abs(xd) * 2.5e-31) {
                        break;
                    }
                }

                ddouble y = x - m;

                return y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static (double sin, double cos) CrudeSinCos(double x) {
#if DEBUG
                if (x < 0 || !(x <= PI)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                (double s, double sgn) = (x < Math.PI / 2) ? (x, 1) : (Math.PI - x, -1);

                double s2 = s * s;

                double sin = s * (166320.0 + s2 * (-22260.0 + s2 * 551.0))
                                / (166320.0 + s2 * (5460.0 + s2 * 75.0));

                double cos = (131040.0 + s2 * (-62160.0 + s2 * (3814.0 + s2 * -59.0)))
                            / (131040.0 + s2 * (3360.0 + s2 * 34.0)) * sgn;

                return (sin, cos);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static double InitValue(double m, double e) {
                double a = m * (1.0 - e), b = m + e, c = (m + Math.PI * e) * (1.0 + e);
                double x = Math.Min(a, Math.Min(b, c));

                return x;
            }
        }
    }
}