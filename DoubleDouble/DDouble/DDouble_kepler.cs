namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble KeplerE(ddouble m, ddouble e, bool centered = false) {
            if (!(e >= 0) || ddouble.IsNaN(m) || !ddouble.IsFinite(e)) {
                return NaN;
            }

            if (e > 128) {
                throw new ArgumentOutOfRangeException(
                    nameof(e),
                    "In the calculation of the KeplerE function, eccentricity greater than 128 is not supported."
                );
            }

            e = RoundMantissa(e, 106);

            if (e <= 1) {
                if (!ddouble.IsFinite(m)) {
                    return NaN;
                }

                ddouble y = KeplerEUtil.Elliptic.Value(m, e);
                return centered ? y : y + m;
            }
            else {
                if (!ddouble.IsFinite(m)) {
                    return centered ? NaN : m.Sign * PositiveInfinity;
                }

                ddouble y = KeplerEUtil.Hyperbolic.Value(m, e);
                return centered ? y - m : y;
            }
        }

        private static class KeplerEUtil {

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

                private static ddouble RootFinding(ddouble m, ddouble e) {
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

                private static ddouble HalleyRootFinding(ddouble m, ddouble e) {
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
                        (double esin, double ecos) = (e.Hi * Math.Sin(xd), e.Hi * Math.Cos(xd));
                        double u = xd - esin;

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
                        (ddouble esin, ddouble ecos) = (e * Sin(x), e * Cos(x));
                        ddouble u = x - esin;

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

                private static ddouble SqrtNewtonRootFinding(ddouble m, ddouble e) {
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
                        (double esin, double ecos) = (e.Hi * Math.Sin(xd), e.Hi * Math.Cos(xd));
                        double u = xd - esin, v = Math.Sign(u) * Math.Sqrt(Math.Abs(u));

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

                    for (int i = 0; i < 2; i++) {
                        (ddouble esin, ddouble ecos) = (e * Sin(x), e * Cos(x));
                        ddouble u = x - esin, v = u.Sign * Sqrt(Abs(u));

                        ddouble dx = (sqrt_m - v) * (2.0 * v) / (1.0 - ecos);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x = Max(0.0, x + dx);

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    for (int i = 0; i < 4; i++) {
                        (ddouble esin, ddouble ecos) = (e * Sin(x), e * Cos(x));
                        ddouble u = x - esin;

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

                private static ddouble CbrtNewtonRootFinding(ddouble m, ddouble e) {
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
                        (double esin, double ecos) = (e.Hi * Math.Sin(xd), e.Hi * Math.Cos(xd));
                        double u = xd - esin, v = Math.Cbrt(u);

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

                    for (int i = 0; i < 4; i++) {
                        (ddouble esin, ddouble ecos) = (e * Sin(x), e * Cos(x));
                        ddouble u = x - esin, v = Cbrt(u);

                        ddouble dx = (cbrt_m - v) * (3.0 * v * v) / (1.0 - ecos);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x = Max(0.0, x + dx);

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    for (int i = 0; i < 8; i++) {
                        (ddouble esin, ddouble ecos) = (e * Sin(x), e * Cos(x));
                        ddouble u = x - esin;

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

                private static double InitValue(double m, double e) {
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

            public static class Hyperbolic {

                public static ddouble Value(ddouble m, ddouble e) {
                    if (m < 0) {
                        return -RootFinding(-m, e);
                    }
                    else {
                        return RootFinding(m, e);
                    }
                }

                private static ddouble RootFinding(ddouble m, ddouble e) {
#if DEBUG
                    if (m < 0) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    if (m >= PI / 4) {
                        return HalleyRootFinding(m, e);
                    }

                    if (e >= 1.0625) {
                        return HalleyRootFinding(m, e);
                    }
                    else if (e >= 1.000244140625) {
                        return SqrtNewtonRootFinding(m, e);
                    }
                    else {
                        return CbrtNewtonRootFinding(m, e);
                    }
                }

                private static ddouble HalleyRootFinding(ddouble m, ddouble e) {
#if DEBUG
                    if (m < 0) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    double xd = InitValue(m.Hi, e);

                    for (int i = 0; i < 16; i++) {
                        (double esinh, double ecosh) = (e.Hi * Math.Sinh(xd), e.Hi * Math.Cosh(xd));
                        double u = esinh - xd, ecosh_m1 = ecosh - 1.0, err = u - m.Hi;

                        double dx = err * (2.0 * ecosh_m1) / (err * esinh - 2.0 * ecosh_m1 * ecosh_m1);

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
                        (ddouble esinh, ddouble ecosh) = (e * Sinh(x), e * Cosh(x));
                        ddouble u = esinh - x, ecosh_m1 = ecosh - 1.0, err = u - m;

                        ddouble dx = err * (2.0 * ecosh_m1) / (err * esinh - 2.0 * ecosh_m1 * ecosh_m1);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x += dx;

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    return x;
                }

                private static ddouble SqrtNewtonRootFinding(ddouble m, ddouble e) {
#if DEBUG
                    if (m < 0) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1) || e >= 1.0625) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    ddouble sqrt_m = Sqrt(m);

                    double xd = Math.Sqrt(InitValue(m.Hi, e));

                    for (int i = 0; i < 16; i++) {
                        (double esinh, double ecosh) = (e.Hi * Math.Sinh(xd), e.Hi * Math.Cosh(xd));
                        double u = esinh - xd, v = Math.Sign(u) * Math.Sqrt(Math.Abs(u)), err = v - sqrt_m.Hi;

                        double dx = 2.0 * err * v / (1.0 - ecosh);

                        if (!double.IsFinite(dx)) {
                            break;
                        }

                        xd = Math.Max(0.0, xd + dx);

                        if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                            break;
                        }
                    }

                    ddouble x = xd;

                    for (int i = 0; i < 2; i++) {
                        (ddouble esinh, ddouble ecosh) = (e * Sinh(x), e * Cosh(x));
                        ddouble u = esinh - x, v = u.Sign * Sqrt(Abs(u)), err = v - sqrt_m;

                        ddouble dx = 2.0 * err * v / (1.0 - ecosh);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x = Max(0.0, x + dx);

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    for (int i = 0; i < 4; i++) {
                        (ddouble esinh, ddouble ecosh) = (e * Sinh(x), e * Cosh(x));
                        ddouble u = esinh - x, ecosh_m1 = ecosh - 1.0, err = u - m;

                        ddouble dx = err * (2.0 * ecosh_m1) / (err * esinh - 2.0 * ecosh_m1 * ecosh_m1);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x += dx;

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    return x;
                }

                private static ddouble CbrtNewtonRootFinding(ddouble m, ddouble e) {
#if DEBUG
                    if (m < 0) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1) || e >= 1.000244140625) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    ddouble cbrt_m = Cbrt(m);

                    double xd = Math.Cbrt(InitValue(m.Hi, e));

                    for (int i = 0; i < 16; i++) {
                        (double esinh, double ecosh) = (e.Hi * Math.Sinh(xd), e.Hi * Math.Cosh(xd));
                        double u = esinh - xd, v = Math.Cbrt(u), err = v - cbrt_m.Hi;

                        double dx = 3.0 * err * v * v / (1.0 - ecosh);

                        if (!double.IsFinite(dx)) {
                            break;
                        }

                        xd = Math.Max(0.0, xd + dx);

                        if (Math.Abs(dx) <= Math.Abs(xd) * 1e-15) {
                            break;
                        }
                    }

                    ddouble x = xd;

                    for (int i = 0; i < 4; i++) {
                        (ddouble esinh, ddouble ecosh) = (e * Sinh(x), e * Cosh(x));
                        ddouble u = esinh - x, v = Cbrt(u), err = v - cbrt_m;

                        ddouble dx = 3.0 * err * v * v / (1.0 - ecosh);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x = Max(0.0, x + dx);

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    for (int i = 0; i < 8; i++) {
                        (ddouble esinh, ddouble ecosh) = (e * Sinh(x), e * Cosh(x));
                        ddouble u = esinh - x, ecosh_m1 = ecosh - 1.0, err = u - m;

                        ddouble dx = err * (2.0 * ecosh_m1) / (err * esinh - 2.0 * ecosh_m1 * ecosh_m1);

                        if (!IsFinite(dx)) {
                            break;
                        }

                        x += dx;

                        if (Math.Abs(dx.Hi) <= Math.Abs(x.Hi) * 2.5e-31) {
                            break;
                        }
                    }

                    return x;
                }

                private static double InitValue(double m, ddouble e) {
#if DEBUG
                    if (m < 0) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    double a = (m > 0.0) ? (m / (e - 1.0).Hi) : 0.0;
                    double b = Math.Log(2 * m / e.Hi + 1 + Math.Sqrt(m / (Math.Sqrt((e - 1.0).Hi))) / 4);

                    double t = Math.Exp(-(e - 1.0).Hi / (2 * Math.Pow(m, 13.0 / 16.0)));

                    double x = (t < 0.9999) ? (a * (1.0 - t) + b * t) : b;

                    return x;
                }
            }
        }
    }
}