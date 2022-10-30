using DoubleDouble;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class KeplerSandbox {
        public static ddouble KeplerE(ddouble m, ddouble e, bool centerize = false) {
            if (!(e >= 0) || e > 1 || !ddouble.IsFinite(m)) {  
                return NaN;
            }

            if (e <= 1) {                
                ddouble y = KeplerEUtil.Elliptic.Value(m, e);
                return centerize ? y : y + m;
            }

            throw new NotImplementedException();
        }

        public static class KeplerEUtil {

            public static class Elliptic {

                public static ddouble Value(ddouble m, ddouble e) {
                    ddouble t = m * RcpPI * 0.5, s = (t - Round(t)) * (2 * PI);

                    if (s < -PI) {
                        return -RootFinding(-2 * PI - s, e);
                    }
                    else if (s < 0) {
                        return -RootFinding(-s, e);
                    }
                    else if (s < PI) {
                        return RootFinding(s, e);
                    }
                    else {
                        return -RootFinding(2 * PI - s, e);
                    }
                }

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
                    else if (e < 0.999755859375) {
                        return SqrtNewtonRootFinding(m, e);
                    }
                    else {
                        return CbrtNewtonRootFinding(m, e);
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

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    ddouble y = x - m;

                    return y;
                }

                public static ddouble CbrtNewtonRootFinding(ddouble m, ddouble e) {
#if DEBUG
                    if (m < 0 || !(m <= PI / 4)) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 0.999755859375) || e > 1) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    ddouble cbrt_m = Cbrt(m);

                    double xd = Math.Cbrt(InitValue(m.Hi, e.Hi));

                    for (int i = 0; i < 16; i++) {
                        (double sin, double cos) = (Math.Sin(xd), Math.Cos(xd));
                        double esin = e.Hi * sin, ecos = e.Hi * cos, u = xd - esin;
                        double v = Math.Cbrt(u);

                        double dx = (cbrt_m.Hi - v) * (3.0 * v * v) / (1.0 - ecos);

                        if (!double.IsFinite(dx)) {
                            break;
                        }

                        xd = Math.Max(0.0, xd + dx);

                        if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                            break;
                        }
                    }

                    ddouble x = xd;

                    for (int i = 0; i < 3; i++) {
                        (ddouble sin, ddouble cos) = (Sin(x), Cos(x));
                        ddouble esin = e * sin, ecos = e * cos, u = x - esin;
                        ddouble v = Cbrt(u);

                        ddouble dx = (cbrt_m - v) * (3.0 * v * v) / (1.0 - ecos);

                        if (!ddouble.IsFinite(dx)) {
                            break;
                        }

                        x = Max(0.0, x + dx);

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
                    if (!(e >= 0) || e > 1) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    double s = Math.Cbrt(6.0 * m), s2 = s * s;
                    double d = s * (1.0 +
                               s2 * (1.6666666666666666667e-2 +
                               s2 * (7.1428571428571428571e-4 +
                               s2 * (7.4801639817849724809e-5))));

                    double a = (m > 0.0) ? (m / (1.0 - e)) : 0.0;
                    double b = d * e + m * (1.0 - e);

                    double x = Math.Min(a, b);

                    return x;
                }
            }
        }
    }
}