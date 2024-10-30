using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble KeplerE(ddouble m, ddouble e, bool centered = false) {
            if (IsNaN(m) || IsNegative(e) || !IsFinite(e)) {
                return NaN;
            }

            if (IsNegative(m)) {
                return -KeplerE(-m, e, centered);
            }

            if (e > 256d) {
                throw new ArgumentOutOfRangeException(
                    nameof(e),
                    "In the calculation of the KeplerE function, eccentricity greater than 256 is not supported."
                );
            }

            if (ILogB(m) < -1000) {
                return KeplerE(double.ScaleB(1, -1000), e, centered) * Ldexp(m, 1000);
            }

            if (e <= 1d) {
                if (!IsFinite(m)) {
                    return centered ? NaN : Sign(m) * PositiveInfinity;
                }

                ddouble m_cycle = (m * RcpPi) % 2d;

                if (centered) {
                    ddouble y = (m_cycle <= 1d)
                        ? KeplerEUtil.Elliptic.Value(m_cycle, e) - m_cycle
                        : (2d - m_cycle) - KeplerEUtil.Elliptic.Value(2d - m_cycle, e);

                    y *= Pi;

                    return y;
                }
                else {
                    ddouble y = (m_cycle <= 1d)
                        ? KeplerEUtil.Elliptic.Value(m_cycle, e)
                        : 2d - KeplerEUtil.Elliptic.Value(2d - m_cycle, e);

                    y *= Pi;
                    y += m - m_cycle * Pi;
                    y = Max(0d, y);

                    return y;
                }
            }
            else {
                if (!IsFinite(m)) {
                    return PositiveInfinity;
                }

                ddouble y = KeplerEUtil.Hyperbolic.Value(m, e);

                return centered ? y - m : y;
            }
        }

        internal static class KeplerEUtil {
            public static class Elliptic {
                public static ddouble Value(ddouble m, ddouble e) {
                    Debug.Assert((m >= 0d && m <= 1d), nameof(m));
                    Debug.Assert((e >= 0d && e <= 1d), nameof(e));

                    ddouble m_pi = m * Pi;

                    if (ILogB(m) > -32 || ILogB(e - 1d) > -16) {
                        double ed = double.Min(1d, e.Hi);
                        double xd = InitValue(m.Hi, ed);

                        for (int i = 0; i < 4; i++) {
                            (xd, bool convergenced) = TrigonIter(xd, m_pi.Hi, ed);
                            if (convergenced) {
                                break;
                            }
                        }
                        ddouble x = xd;
                        for (int i = 0; i < 2; i++) {
                            (x, bool convergenced) = TrigonIter(x, m_pi, e);
                            if (convergenced) {
                                break;
                            }
                        }

                        return x;
                    }
                    else {
                        ddouble x = NearZero(m, e);
                        (x, bool convergenced) = PadeNewton(x, m_pi, e, max_iters: 16);

                        return x;
                    }
                }

                public static double InitValue(double m, double e) {
                    double em1 = 1d - e;

                    double x = (e < 3.90625e-3d)
                        ? m
                        : (em1 - double.Sqrt(em1 * em1 + 4d * e * m)) / (-2d * e);

                    double x2 = x * x;
                    double delta = x * (em1 - e * x2 * (-2d + x)) - m;

                    if (delta == 0d) {
                        return x;
                    }

                    double g1 = em1 - e * x2 * (-6d + x * 4d);
                    double g2 = -12 * e * x * (-1d + x);

                    double dx = (2d * delta * g1) / (2d * g1 * g1 - delta * g2);

                    if (!double.IsFinite(dx)) {
                        return x;
                    }

                    x -= dx;

                    return x;
                }

                public static ddouble NearZero(ddouble m, ddouble e) {
                    Debug.Assert(ILogB(m) <= -32, nameof(m));
                    Debug.Assert(ILogB(e - 1d) <= -16, nameof(e));

                    return NearOneE.Value(m * Pi, e);
                }

                public static (double x, bool convergenced) TrigonIter(double x, double m, double e) {
                    double x_pi = x * double.Pi;
                    (double sin, double cos) = double.SinCosPi(x);
                    double esin = e * sin, ecos = e * cos;

                    double delta = x_pi - esin - m;
                    if (double.Abs(delta) <= m * 1e-12) {
                        return (x, convergenced: true);
                    }

                    double g1 = -ecos + 1d;
                    double g2 = esin;
                    double g3 = ecos;
                    double dx = RootFind.Iter(delta, g1, g2, g3) / double.Pi;

                    if (!double.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                    x = double.Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) TrigonIter(ddouble x, ddouble m, ddouble e) {
                    ddouble x_pi = x * Pi;
                    ddouble sin = SinPi(x), cos = CosPi(x);
                    ddouble esin = e * sin, ecos = e * cos;

                    ddouble delta = x_pi - esin - m;
                    if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                        return (x, convergenced: true);
                    }

                    ddouble g1 = -ecos + 1d;
                    ddouble g2 = esin;
                    ddouble g3 = ecos;
                    ddouble dx = RootFind.Iter(delta, g1, g2, g3) * RcpPi;

                    if (!IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                    x = Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) PadeNewton(ddouble x, ddouble m, ddouble e, int max_iters) {
                    ddouble r0 = 2520d + e * 5880d, r2 = 60d + e * 360d, r4 = e * 11d;
                    ddouble s1 = 20d * (126d + e * (168d + e * -294d)), s3 = 20d * (3d + e * (36d + e * 31d));

                    (ddouble f, ddouble g) pade(ddouble x, ddouble e) {
                        ddouble x2 = x * x;

                        ddouble r = r0 + x2 * ((r2) + x2 * (r4));
                        ddouble gr = x * ((r2 * 2d) + x2 * (r4 * 4d));

                        ddouble s = x * ((s1) + x2 * (s3));
                        ddouble gs = s1 + x2 * (s3 * 3d);

                        return (s / r, (gs * r - s * gr) / (r * r));
                    }

                    bool convergenced = false;

                    for (int i = 0; i < max_iters && !convergenced; i++) {
                        ddouble x_pi = x * Pi;
                        (ddouble f, ddouble g) = pade(x_pi, e);

                        ddouble delta = f - m;
                        if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                            return (x, convergenced: true);
                        }

                        const double eps = 1e-100;

                        ddouble dx = delta / (g * Pi + eps);

                        if (!IsFinite(dx)) {
                            return (x, convergenced: true);
                        }

                        convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                        x = Max(0d, x - dx);
                    }

                    return (x, convergenced);
                }
            }

            public static class Hyperbolic {

                public static ddouble Value(ddouble m, ddouble e) {
                    Debug.Assert(m >= 0d, nameof(m));
                    Debug.Assert(e >= 1d, nameof(e));

                    if (ILogB(m) > -32 || ILogB(1d - e) > -16) {
                        double md = m.Hi, ed = double.Max(1d, e.Hi);
                        double xd = InitValue(md, ed);

                        for (int i = 0; i < 8; i++) {
                            (xd, bool convergenced) = HyperbolicIter(xd, md, ed);
                            if (convergenced) {
                                break;
                            }
                        }
                        ddouble x = xd;
                        for (int i = 0; i < 4; i++) {
                            (x, bool convergenced) = HyperbolicIter(x, m, e);
                            if (convergenced) {
                                break;
                            }
                        }

                        return x;
                    }
                    else {
                        ddouble x = InitValue(m, e);
                        (x, bool convergenced) = PadeNewton(x, m, e, max_iters: 16);

                        return x;
                    }
                }

                public static double InitValue(double m, double e) {
                    if (double.ILogB(m) <= -32) {
                        return 0;
                    }

                    double x = double.Asinh(m / e);

                    if (m >= 10d) {
                        return x;
                    }

                    double u = m / 10, em1 = e - 1;
                    double t = double.Cbrt((double.Sqrt((9d * e * m * m + 8d * em1 * em1 * em1) / e) + 3d * m) / e);
                    double v = t - 6d * em1 / (3d * e * t);

                    x = x * double.Min(1, u) + v * double.Max(0, 1 - u);

                    return x;
                }

                public static ddouble InitValue(ddouble m, ddouble e) {
                    Debug.Assert(ILogB(m) <= -32, nameof(m));
                    Debug.Assert(ILogB(1d - e) <= -16, nameof(e));

                    return NearOneE.Value(m, e);
                }

                public static (double x, bool convergenced) HyperbolicIter(double x, double m, double e) {
                    double sinh = double.Sinh(x), cosh = double.Cosh(x);
                    double esinh = e * sinh, ecosh = e * cosh;

                    double delta = esinh - x - m;
                    if (double.Abs(delta) <= m * 1e-12) {
                        return (x, convergenced: true);
                    }

                    double g1 = ecosh - 1d;
                    double g2 = esinh;
                    double g3 = ecosh;
                    double dx = RootFind.Iter(delta, g1, g2, g3);

                    if (!double.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                    x = double.Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) HyperbolicIter(ddouble x, ddouble m, ddouble e) {
                    ddouble sinh = Sinh(x), cosh = Cosh(x);
                    ddouble esinh = e * sinh, ecosh = e * cosh;

                    ddouble delta = esinh - x - m;
                    if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                        return (x, convergenced: true);
                    }

                    ddouble g1 = ecosh - 1d;
                    ddouble g2 = esinh;
                    ddouble g3 = ecosh;
                    ddouble dx = RootFind.Iter(delta, g1, g2, g3);

                    if (!IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                    x = Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) PadeNewton(ddouble x, ddouble m, ddouble e, int max_iters) {
                    ddouble r0 = 2520d + e * 5880d, r2 = -60d + e * -360d, r4 = e * 11d;
                    ddouble s1 = 20d * (-126d + e * (-168d + e * 294d)), s3 = 20d * (3d + e * (36d + e * 31d));

                    (ddouble f, ddouble g) pade(ddouble x, ddouble e) {
                        ddouble x2 = x * x;

                        ddouble r = r0 + x2 * ((r2) + x2 * (r4));
                        ddouble gr = x * ((r2 * 2d) + x2 * (r4 * 4d));

                        ddouble s = x * ((s1) + x2 * (s3));
                        ddouble gs = s1 + x2 * (s3 * 3d);

                        return (s / r, (gs * r - s * gr) / (r * r));
                    }

                    bool convergenced = false;

                    for (int i = 0; i < max_iters && !convergenced; i++) {
                        (ddouble f, ddouble g) = pade(x, e);

                        ddouble delta = f - m;
                        if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                            return (x, convergenced: true);
                        }

                        const double eps = 1e-100;

                        ddouble dx = delta / (g + eps);

                        if (!IsFinite(dx)) {
                            return (x, convergenced: true);
                        }

                        convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                        x = Max(0d, x - dx);
                    }

                    return (x, convergenced);
                }
            }

            public static class RootFind {
                public static double Iter(double delta, double g1, double g2, double g3) {
                    Debug.Assert(g1 >= 0d, nameof(g1));

                    double omax = g1 * 0.125d;

                    double o3 = delta * (3d * g1 * g2 - delta * g3) / (3d * (2d * g1 * g1 - delta * g2));
                    if (o3 <= omax) {
                        double dx = delta / (g1 - o3);
                        return dx;
                    }

                    double o2 = delta * g2 / (2d * g1);
                    if (o2 <= omax) {
                        double dx = delta / (g1 - o2);
                        return dx;
                    }

                    return delta / g1;
                }

                public static ddouble Iter(ddouble delta, ddouble g1, ddouble g2, ddouble g3) {
                    Debug.Assert(g1 >= 0d, nameof(g1));

                    ddouble omax = Ldexp(g1, -3);

                    ddouble o3 = delta * (3d * g1 * g2 - delta * g3) / (3d * (Ldexp(g1 * g1, 1) - delta * g2));
                    if (o3 <= omax) {
                        ddouble dx = delta / (g1 - o3);
                        return dx;
                    }

                    ddouble o2 = delta * g2 / Ldexp(g1, 1);
                    if (o2 <= omax) {
                        ddouble dx = delta / (g1 - o2);
                        return dx;
                    }

                    return delta / g1;
                }
            }

            public static class NearOneE {
                public static ddouble Value(ddouble m, ddouble e) {
                    ddouble s = Cbrt(6d * m), s2 = s * s;

                    ddouble x = s * (155232000d + s2 * (2587200d + s2 * (110880d + s2 * (6160d + s2 * 387d)))) / 155232000d;

                    return x;
                }
            }
        }
    }
}