namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble KeplerE(ddouble m, ddouble e, bool centered = false) {
            if (!(e >= 0) || ddouble.IsNaN(m) || !ddouble.IsFinite(e)) {
                return NaN;
            }

            if (m.Sign < 0) {
                return -KeplerE(-m, e, centered);
            }

            if (e > 256) {
                throw new ArgumentOutOfRangeException(
                    nameof(e),
                    "In the calculation of the KeplerE function, eccentricity greater than 256 is not supported."
                );
            }

            if (e <= 1) {
                if (!ddouble.IsFinite(m)) {
                    return centered ? NaN : m.Sign * PositiveInfinity;
                }

                ddouble m_cycle = (m * RcpPI) % 2d;

                if (centered) {
                    ddouble y = (m_cycle <= 1d)
                        ? KeplerEUtil.Elliptic.Value(m_cycle, e) - m_cycle
                        : (2d - m_cycle) - KeplerEUtil.Elliptic.Value(2d - m_cycle, e);

                    y *= PI;

                    return y;
                }
                else {
                    ddouble y = (m_cycle <= 1d)
                        ? KeplerEUtil.Elliptic.Value(m_cycle, e)
                        : 2d - KeplerEUtil.Elliptic.Value(2d - m_cycle, e);

                    y *= PI;
                    y += m - m_cycle * PI;

                    return y;
                }
            }
            else {
                if (!ddouble.IsFinite(m)) {
                    return PositiveInfinity;
                }

                ddouble y = KeplerEUtil.Hyperbolic.Value(m, e);

                return centered ? y - m : y;
            }
        }

        internal static class KeplerEUtil {
            public static class Elliptic {
                public static ddouble Value(ddouble m, ddouble e) {
#if DEBUG
                    if (!(m >= 0 && m <= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 0 && e <= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    ddouble m_pi = m * PI;

                    if (double.ILogB(m.Hi) > -52 || e.Hi < 0.9375) {
                        double ed = Math.Min(1, e.Hi);
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

                        for (int i = 0; i < 8; i++) {
                            (x, bool convergenced) = TrigonIter(x, m_pi, e);
                            if (convergenced) {
                                break;
                            }
                        }

                        return x;
                    }
                }

                public static double InitValue(double m, double e) {
                    double em1 = 1d - e;

                    double x = (e < 3.90625e-3)
                        ? m
                        : (em1 - double.Sqrt(em1 * em1 + 4 * e * m)) / (-2 * e);

                    double x2 = x * x;
                    double delta = x * (em1 - e * x2 * (-2d + x)) - m;

                    if (delta == 0d) {
                        return x;
                    }

                    double g1 = em1 - e * x2 * (-6d + 4 * x);
                    double g2 = -12 * e * x * (-1d + x);

                    double dx = (2 * delta * g1) / (2 * g1 * g1 - delta * g2);

                    if (!double.IsFinite(dx)) {
                        return x;
                    }

                    x -= dx;

                    return x;
                }

                public static ddouble NearZero(ddouble m, ddouble e) {
#if DEBUG
                    if (double.ILogB(m.Hi) > -52) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
#endif

                    ddouble em1 = 1d - e;

                    ddouble x = (double.ILogB(e.Hi) < -26)
                        ? m
                        : (em1 - Sqrt(em1 * em1 + 4 * e * m)) / (-2 * e);

                    x = IsFinite(x) ? x : Zero;

                    return x;
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
                    double dx = Householder4(delta, g1, g2, g3);

                    if (!double.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                    x = double.Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) TrigonIter(ddouble x, ddouble m, ddouble e) {
                    ddouble x_pi = x * ddouble.PI;
                    ddouble sin = ddouble.SinPI(x), cos = ddouble.CosPI(x);
                    ddouble esin = e * sin, ecos = e * cos;

                    ddouble delta = x_pi - esin - m;
                    if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                        return (x, convergenced: true);
                    }

                    ddouble g1 = -ecos + 1d;
                    ddouble g2 = esin;
                    ddouble g3 = ecos;
                    ddouble dx = Householder4(delta, g1, g2, g3);

                    if (!ddouble.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                    x = ddouble.Max(0d, x - dx);

                    return (x, convergenced);
                }

                private static double Householder4(double delta, double g1, double g2, double g3) {
                    double sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                    double dx = 3 * delta * (sqg1 + sqg1_deltag2) /
                        (double.Pi * (6 * g1 * sqg1_deltag2 + delta * delta * g3));
                    return dx;
                }

                private static ddouble Householder4(ddouble delta, ddouble g1, ddouble g2, ddouble g3) {
                    ddouble sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                    ddouble dx = 3 * delta * (sqg1 + sqg1_deltag2) /
                        (ddouble.PI * (6 * g1 * sqg1_deltag2 + delta * delta * g3));
                    return dx;
                }
            }

            public static class Hyperbolic {

                public static ddouble Value(ddouble m, ddouble e) {
#if DEBUG
                    if (!(m >= 0)) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
                    if (!(e >= 1)) {
                        throw new ArgumentOutOfRangeException(nameof(e));
                    }
#endif

                    if (double.ILogB(m.Hi) > -52 || e.Hi > 1.0625) {
                        double md = m.Hi, ed = Math.Max(1, e.Hi);
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

                        for (int i = 0; i < 4; i++) {
                            (x, bool convergenced) = HyperbolicIter(x, m, e);
                            if (convergenced) {
                                break;
                            }
                        }

                        return x;
                    }
                }

                public static double InitValue(double m, double e) {
                    if (double.ILogB(m) <= -52) {
                        return 0;
                    }

                    double x = double.Asinh(m / e);

                    if (m >= 10d) {
                        return x;
                    }

                    double u = m / 10, em1 = e - 1;
                    double t = double.Cbrt((double.Sqrt((9 * e * m * m + 8 * em1 * em1 * em1) / e) + 3 * m) / e);
                    double v = t - 6 * em1 / (3 * e * t);

                    x = x * double.Min(1, u) + v * double.Max(0, 1 - u);

                    return x;
                }

                public static ddouble InitValue(ddouble m, ddouble e) {
#if DEBUG
                    if (double.ILogB(m.Hi) > -52) {
                        throw new ArgumentOutOfRangeException(nameof(m));
                    }
#endif

                    ddouble x;
                    ddouble em1 = e - 1d;

                    if (double.ILogB(m.Hi) > -104) {
                        ddouble t = Cbrt((Sqrt((9 * e * m * m + 8 * em1 * em1 * em1) / e) + 3 * m) / e);
                        x = t - 6 * em1 / (3 * e * t);
                    }
                    else {
                        x = m / em1;
                    }

                    x = IsFinite(x) ? x : Zero;

                    return x;
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
                    double dx = Householder4(delta, g1, g2, g3);

                    if (!double.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                    x = double.Max(0d, x - dx);

                    return (x, convergenced);
                }

                public static (ddouble x, bool convergenced) HyperbolicIter(ddouble x, ddouble m, ddouble e) {
                    ddouble sinh = ddouble.Sinh(x), cosh = ddouble.Cosh(x);
                    ddouble esinh = e * sinh, ecosh = e * cosh;

                    ddouble delta = esinh - x - m;
                    if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                        return (x, convergenced: true);
                    }

                    ddouble g1 = ecosh - 1d;
                    ddouble g2 = esinh;
                    ddouble g3 = ecosh;
                    ddouble dx = Householder4(delta, g1, g2, g3);

                    if (!ddouble.IsFinite(dx)) {
                        return (x, convergenced: true);
                    }

                    bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                    x = ddouble.Max(0d, x - dx);

                    return (x, convergenced);
                }

                private static double Householder4(double delta, double g1, double g2, double g3) {
                    double sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                    double dx = 3 * delta * (sqg1 + sqg1_deltag2) /
                        (6 * g1 * sqg1_deltag2 + delta * delta * g3);
                    return dx;
                }

                private static ddouble Householder4(ddouble delta, ddouble g1, ddouble g2, ddouble g3) {
                    ddouble sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                    ddouble dx = 3 * delta * (sqg1 + sqg1_deltag2) /
                        (6 * g1 * sqg1_deltag2 + delta * delta * g3);
                    return dx;
                }
            }
        }
    }
}