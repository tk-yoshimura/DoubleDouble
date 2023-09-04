using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {    
    internal static class KeplerEUtil {
        public static ddouble KeplerE(ddouble m, ddouble e, bool centered = false) {
            if (!(e >= 0) || ddouble.IsNaN(m) || !ddouble.IsFinite(e)) {
                return NaN;
            }

            if (m.Sign < 0) {
                return -KeplerE(-m, e, centered);
            }

            if (e > 1024) {
                throw new ArgumentOutOfRangeException(
                    nameof(e),
                    "In the calculation of the KeplerE function, eccentricity greater than 1024 is not supported."
                );
            }

            if (e <= 1) {
                if (!ddouble.IsFinite(m)) {
                    return centered ? NaN : m.Sign * PositiveInfinity;
                }

                ddouble m_cycle = (m * RcpPI) % 2d;

                ddouble y = (m_cycle <= 1d)
                    ? KeplerEUtil.Elliptic.Value(m_cycle, e)
                    : -KeplerEUtil.Elliptic.Value(2d - m_cycle, e);

                y *= PI;

                return centered ? y : y + m;
            }
            else {
                if (!ddouble.IsFinite(m)) {
                    return PositiveInfinity;
                }

                ddouble y = KeplerEUtil.Hyperbolic.Value(m, e);

                return centered ? y - m : y;
            }
        }


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

                double md = m.Hi, ed = Math.Min(1, e.Hi);
                ddouble x;

                if (md <= 0.75 || ed < 0.9375) {
                    double xd = InitValue(md, ed);

                    for (int i = 0; i < 4; i++) {
                        (xd, bool convergenced) = TrigonIter(xd, md, ed);
                        if (convergenced) {
                            break;
                        }
                    }
                    x = xd;
                    for (int i = 0; i < 2; i++) {
                        (x, bool convergenced) = TrigonIter(x, m, e);
                        if (convergenced) {
                            break;
                        }
                    }

                    x = x - m;
                }
                else {
                    ddouble mc = 1d - m;
                    double xd = InitValue(mc.Hi, -ed);

                    if (ed < 0.999755859375) {
                        for (int i = 0; i < 4; i++) {
                            (xd, bool convergenced) = SqrtTrigonIter(xd, mc.Hi, -ed);
                            if (convergenced) {
                                break;
                            }
                        }
                        x = xd;
                        for (int i = 0; i < 2; i++) {
                            (x, bool convergenced) = SqrtTrigonIter(x, mc, -e);
                            if (convergenced) {
                                break;
                            }
                        }
                    }
                    else {
                        for (int i = 0; i < 4; i++) {
                            (xd, bool convergenced) = CbrtTrigonIter(xd, mc.Hi, -ed);
                            if (convergenced) {
                                break;
                            }
                        }
                        x = xd;
                        for (int i = 0; i < 2; i++) {
                            (x, bool convergenced) = CbrtTrigonIter(x, mc, -e);
                            if (convergenced) {
                                break;
                            }
                        }
                    }

                    x = mc - x;
                }

                return x;
            }

            public static double InitValue(double m, double e) {
                double ep1 = e + 1d;

                double x = (e < 3.90625e-3)
                    ? m
                    : (ep1 - double.Sqrt(ep1 * ep1 - 4 * e * m)) / (2 * e);

                double x2 = x * x;
                double delta = x * (ep1 + e * x2 * (-2d + x)) - m;

                if (delta == 0d) {
                    return x;
                }

                double g1 = ep1 + e * x2 * (-6d + 4 * x);
                double g2 = 12 * e * x * (-1d + x);

                double dx = (2 * delta * g1) / (2 * g1 * g1 - delta * g2);

                if (!double.IsFinite(dx)) {
                    return x;
                }

                x -= dx;

                return x;
            }

            public static (double x, bool convergenced) TrigonIter(double x, double m, double e) {
                double x_pi = x * double.Pi, m_pi = m * double.Pi;
                (double sin, double cos) = double.SinCosPi(x);
                double esin = e * sin, ecos = e * cos;

                double delta = x_pi + esin - m_pi;
                if (double.Abs(delta) <= m_pi * 1e-12) {
                    return (x, convergenced: true);
                }

                double g1 = ecos + 1d;
                double g2 = -esin;
                double g3 = -ecos;
                double dx = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                return (x, convergenced);
            }

            public static (ddouble x, bool convergenced) TrigonIter(ddouble x, ddouble m, ddouble e) {
                ddouble x_pi = x * ddouble.PI, m_pi = m * ddouble.PI;
                ddouble sin = ddouble.SinPI(x), cos = ddouble.CosPI(x);
                ddouble esin = e * sin, ecos = e * cos;

                ddouble delta = x_pi + esin - m_pi;
                if (double.Abs(delta.Hi) <= m_pi.Hi * 1e-31) {
                    return (x, convergenced: true);
                }

                ddouble g1 = ecos + 1d;
                ddouble g2 = -esin;
                ddouble g3 = -ecos;
                ddouble dx = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                return (x, convergenced);
            }

            public static (double x, bool convergenced) SqrtTrigonIter(double x, double m, double e) {
                double x_pi = x * double.Pi, m_pi = m * double.Pi;
                (double sin, double cos) = double.SinCosPi(x);
                double esin = e * sin, ecos = e * cos;

                double f = x_pi + esin;
                double f_sqrt = double.Sqrt(f), m_sqrt = double.Sqrt(m_pi);

                double delta = f_sqrt - m_sqrt;
                if (double.Abs(delta) <= m_sqrt * 1e-12) {
                    return (x, convergenced: true);
                }

                double ecosp1 = ecos + 1d, sqecosp1 = ecosp1 * ecosp1;

                double f_pow3d2 = f_sqrt * f, f_pow5d2 = f_pow3d2 * f;

                double g1 = ecosp1 / (2 * f_sqrt);
                double g2 = (-sqecosp1 - 2 * f * esin) / (4 * f_pow3d2);
                double g3 = (3 * ecosp1 * sqecosp1 + 2 * f * (3 * esin * ecosp1 - 2 * f * ecos)) / (8 * f_pow5d2);
                double dx = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x = double.Max(0d, x - dx);

                bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                return (x, convergenced);
            }

            public static (ddouble x, bool convergenced) SqrtTrigonIter(ddouble x, ddouble m, ddouble e) {
                ddouble x_pi = x * ddouble.PI, m_pi = m * ddouble.PI;
                ddouble sin = ddouble.SinPI(x), cos = ddouble.CosPI(x);
                ddouble esin = e * sin, ecos = e * cos;

                ddouble f = x_pi + esin;
                ddouble f_sqrt = ddouble.Sqrt(f), m_sqrt = ddouble.Sqrt(m_pi);

                ddouble delta = f_sqrt - m_sqrt;
                if (double.Abs(delta.Hi) <= m_sqrt.Hi * 1e-31) {
                    return (x, convergenced: true);
                }

                ddouble ecosp1 = ecos + 1d, sqecosp1 = ecosp1 * ecosp1;

                ddouble f_pow3d2 = f_sqrt * f, f_pow5d2 = f_pow3d2 * f;

                ddouble g1 = ecosp1 / (2 * f_sqrt);
                ddouble g2 = (-sqecosp1 - 2 * f * esin) / (4 * f_pow3d2);
                ddouble g3 = (3 * ecosp1 * sqecosp1 + 2 * f * (3 * esin * ecosp1 - 2 * f * ecos)) / (8 * f_pow5d2);
                ddouble dx = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x = ddouble.Max(0d, x - dx);

                bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
                return (x, convergenced);
            }

            public static (double x, bool convergenced) CbrtTrigonIter(double x, double m, double e) {
                double x_pi = x * double.Pi, m_pi = m * double.Pi;
                (double sin, double cos) = double.SinCosPi(x);
                double esin = e * sin, ecos = e * cos;

                double f = x_pi + esin;
                double f_cbrt = double.Cbrt(f), m_cbrt = double.Cbrt(m_pi);

                double delta = f_cbrt - m_cbrt;
                if (double.Abs(delta) <= m_cbrt * 1e-12) {
                    return (x, convergenced: true);
                }

                double ecosp1 = ecos + 1d, sqecosp1 = ecosp1 * ecosp1;

                double f_pow2d3 = f_cbrt * f_cbrt, f_pow5d3 = f_pow2d3 * f, f_pow8d3 = f_pow5d3 * f;

                double g1 = ecosp1 / (3 * f_pow2d3);
                double g2 = (-2 * sqecosp1 - 3 * f * esin) / (9 * f_pow5d3);
                double g3 = (10 * ecosp1 * sqecosp1 + 9 * f * (2 * esin * ecosp1 - f * ecos)) / (27 * f_pow8d3);
                double dx = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                return (x, convergenced);
            }

            public static (ddouble x, bool convergenced) CbrtTrigonIter(ddouble x, ddouble m, ddouble e) {
                ddouble x_pi = x * ddouble.PI, m_pi = m * ddouble.PI;
                ddouble sin = ddouble.SinPI(x), cos = ddouble.CosPI(x);
                ddouble esin = e * sin, ecos = e * cos;

                ddouble f = x_pi + esin;
                ddouble f_cbrt = ddouble.Cbrt(f), m_cbrt = ddouble.Cbrt(m_pi);

                ddouble delta = f_cbrt - m_cbrt;
                if (double.Abs(delta.Hi) <= m_cbrt.Hi * 1e-31) {
                    return (x, convergenced: true);
                }

                ddouble ecosp1 = ecos + 1d, sqecosp1 = ecosp1 * ecosp1;

                ddouble f_pow2d3 = f_cbrt * f_cbrt, f_pow5d3 = f_pow2d3 * f, f_pow8d3 = f_pow5d3 * f;

                ddouble g1 = ecosp1 / (3 * f_pow2d3);
                ddouble g2 = (-2 * sqecosp1 - 3 * f * esin) / (9 * f_pow5d3);
                ddouble g3 = (10 * ecosp1 * sqecosp1 + 9 * f * (2 * esin * ecosp1 - f * ecos)) / (27 * f_pow8d3);
                ddouble dx = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
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

                double md = m.Hi, ed = Math.Min(1, e.Hi);
                double xd = InitValue(md, ed);

                for (int i = 0; i < 4; i++) {
                    (xd, bool convergenced) = HyperbolicIter(xd, md, ed);
                    if (convergenced) {
                        Console.WriteLine(i);
                        break;
                    }
                }
                ddouble x = xd;
                for (int i = 0; i < 4; i++) {
                    (x, bool convergenced) = HyperbolicIter(x, m, e);
                    if (convergenced) {
                        Console.WriteLine(i);
                        break;
                    }
                }

                return x;
            }

            public static double InitValue(double m, double e) {
                double x = double.Asinh(m * (1 - 1 / ((e + 1) * (m * 0.1 + 1))) / e);
                return x;
            }

            public static (double x, bool convergenced) HyperbolicIter(double x, double m, double e) {
                double sinh = double.Sinh(x), cosh = double.Cosh(x);
                double esinh = e * sinh, ecosh = e * cosh;

                double delta = x + esinh - m;
                if (double.Abs(delta) <= m * 1e-12) {
                    return (x, convergenced: true);
                }

                double g1 = ecosh + 1d;
                double g2 = esinh;
                double g3 = ecosh;
                double dx = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx) <= double.Abs(x) * 1e-12;
                return (x, convergenced);
            }

            public static (ddouble x, bool convergenced) HyperbolicIter(ddouble x, ddouble m, ddouble e) {
                ddouble sinh = ddouble.Sinh(x), cosh = ddouble.Cosh(x);
                ddouble esinh = e * sinh, ecosh = e * cosh;

                ddouble delta = x + esinh - m;
                if (double.Abs(delta.Hi) <= m.Hi * 1e-31) {
                    return (x, convergenced: true);
                }

                ddouble g1 = ecosh + 1d;
                ddouble g2 = esinh;
                ddouble g3 = ecosh;
                ddouble dx = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dx)) {
                    return (x, convergenced: true);
                }

                x -= dx;

                bool convergenced = double.Abs(dx.Hi) <= double.Abs(x.Hi) * 1e-31;
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