using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    internal static class KeplerEUtil {

        public static class Elliptic {
            public static (double v, bool convergenced) InitValue(double u, double s) {
                double sp1 = s + 1d;

                double v = (sp1 - double.Sqrt(sp1 * sp1 - 4 * s * u)) / (2 * s);

                double v2 = v * v;
                double delta = v * (sp1 + s * v2 * (-2d + v)) - u;

                if (delta == 0d) {
                    return (v, convergenced: true);
                }

                double g1 = sp1 + s * v2 * (-6d + 4 * v);
                double g2 = 12 * s * v * (-1d + v);

                double dv = (2 * delta * g1) / (2 * g1 * g1 - delta * g2);

                if (!double.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (double v, bool convergenced) TrigonIter(double v, double u, double s) {
                double v_pi = v * double.Pi, u_pi = u * double.Pi;
                (double sin, double cos) = double.SinCosPi(v);
                double ssin = s * sin, scos = s * cos;

                double delta = v_pi + ssin - u_pi;
                if (double.Abs(delta) < u_pi * 1e-10) {
                    return (v, convergenced: true);
                }

                double g1 = scos + 1d;
                double g2 = -ssin;
                double g3 = -scos;
                double dv = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (ddouble v, bool convergenced) TrigonIter(ddouble v, ddouble u, ddouble s) {
                ddouble v_pi = v * ddouble.PI, u_pi = u * ddouble.PI;
                ddouble sin = ddouble.SinPI(v), cos = ddouble.CosPI(v);
                ddouble ssin = s * sin, scos = s * cos;

                ddouble delta = v_pi + ssin - u_pi;
                if (double.Abs(delta.Hi) < u_pi.Hi * 1e-30) {
                    return (v, convergenced: true);
                }

                ddouble g1 = scos + 1d;
                ddouble g2 = -ssin;
                ddouble g3 = -scos;
                ddouble dv = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (double v, bool convergenced) SqrtTrigonIter(double v, double u, double s) {
                double v_pi = v * double.Pi, u_pi = u * double.Pi;
                (double sin, double cos) = double.SinCosPi(v);
                double ssin = s * sin, scos = s * cos;

                double f = v_pi + ssin;
                double f_sqrt = double.Sqrt(f), u_sqrt = double.Sqrt(u_pi);

                double delta = f_sqrt - u_sqrt;
                if (double.Abs(delta) < u_sqrt * 1e-10) {
                    return (v, convergenced: true);
                }

                double scosp1 = scos + 1d, sqscosp1 = scosp1 * scosp1;

                double f_pow3d2 = f_sqrt * f, f_pow5d2 = f_pow3d2 * f;

                double g1 = scosp1 / (2 * f_sqrt);
                double g2 = (-sqscosp1 - 2 * f * ssin) / (4 * f_pow3d2);
                double g3 = (3 * scosp1 * sqscosp1 + 2 * f * (3 * ssin * scosp1 - 2 * f * scos)) / (8 * f_pow5d2);
                double dv = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (ddouble v, bool convergenced) SqrtTrigonIter(ddouble v, ddouble u, ddouble s) {
                ddouble v_pi = v * ddouble.PI, u_pi = u * ddouble.PI;
                ddouble sin = ddouble.SinPI(v), cos = ddouble.CosPI(v);
                ddouble ssin = s * sin, scos = s * cos;

                ddouble f = v_pi + ssin;
                ddouble f_sqrt = ddouble.Sqrt(f), u_sqrt = ddouble.Sqrt(u_pi);

                ddouble delta = f_sqrt - u_sqrt;
                if (double.Abs(delta.Hi) < u_sqrt.Hi * 1e-30) {
                    return (v, convergenced: true);
                }

                ddouble scosp1 = scos + 1d, sqscosp1 = scosp1 * scosp1;

                ddouble f_pow3d2 = f_sqrt * f, f_pow5d2 = f_pow3d2 * f;

                ddouble g1 = scosp1 / (2 * f_sqrt);
                ddouble g2 = (-sqscosp1 - 2 * f * ssin) / (4 * f_pow3d2);
                ddouble g3 = (3 * scosp1 * sqscosp1 + 2 * f * (3 * ssin * scosp1 - 2 * f * scos)) / (8 * f_pow5d2);
                ddouble dv = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (double v, bool convergenced) CbrtTrigonIter(double v, double u, double s) {
                double v_pi = v * double.Pi, u_pi = u * double.Pi;
                (double sin, double cos) = double.SinCosPi(v);
                double ssin = s * sin, scos = s * cos;

                double f = v_pi + ssin;
                double f_cbrt = double.Cbrt(f), u_cbrt = double.Cbrt(u_pi);

                double delta = f_cbrt - u_cbrt;
                if (double.Abs(delta) < u_cbrt * 1e-10) {
                    return (v, convergenced: true);
                }

                double scosp1 = scos + 1d, sqscosp1 = scosp1 * scosp1;

                double f_pow2d3 = f_cbrt * f_cbrt, f_pow5d3 = f_pow2d3 * f, f_pow8d3 = f_pow5d3 * f;

                double g1 = scosp1 / (3 * f_pow2d3);
                double g2 = (-2 * sqscosp1 - 3 * f * ssin) / (9 * f_pow5d3);
                double g3 = (10 * scosp1 * sqscosp1 + 9 * f * (2 * ssin * scosp1 - f * scos)) / (27 * f_pow8d3);
                double dv = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (ddouble v, bool convergenced) CbrtTrigonIter(ddouble v, ddouble u, ddouble s) {
                ddouble v_pi = v * ddouble.PI, u_pi = u * ddouble.PI;
                ddouble sin = ddouble.SinPI(v), cos = ddouble.CosPI(v);
                ddouble ssin = s * sin, scos = s * cos;

                ddouble f = v_pi + ssin;
                ddouble f_cbrt = ddouble.Cbrt(f), u_cbrt = ddouble.Cbrt(u_pi);

                ddouble delta = f_cbrt - u_cbrt;
                if (double.Abs(delta.Hi) < u_cbrt.Hi * 1e-30) {
                    return (v, convergenced: true);
                }

                ddouble scosp1 = scos + 1d, sqscosp1 = scosp1 * scosp1;

                ddouble f_pow2d3 = f_cbrt * f_cbrt, f_pow5d3 = f_pow2d3 * f, f_pow8d3 = f_pow5d3 * f;

                ddouble g1 = scosp1 / (3 * f_pow2d3);
                ddouble g2 = (-2 * sqscosp1 - 3 * f * ssin) / (9 * f_pow5d3);
                ddouble g3 = (10 * scosp1 * sqscosp1 + 9 * f * (2 * ssin * scosp1 - f * scos)) / (27 * f_pow8d3);
                ddouble dv = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            private static double Householder4(double delta, double g1, double g2, double g3) {
                double sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                double dv = 3 * delta * (sqg1 + sqg1_deltag2) /
                    (double.Pi * (6 * g1 * sqg1_deltag2 + delta * delta * g3));
                return dv;
            }

            private static ddouble Householder4(ddouble delta, ddouble g1, ddouble g2, ddouble g3) {
                ddouble sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                ddouble dv = 3 * delta * (sqg1 + sqg1_deltag2) /
                    (ddouble.PI * (6 * g1 * sqg1_deltag2 + delta * delta * g3));
                return dv;
            }
        }

        public static class Hyperbolic {
            public static double InitValue(double u, double s) {
                return double.Asinh(u / s);
            }

            public static (double v, bool convergenced) HyperbolicIter(double v, double u, double s) {
                double sinh = double.Sinh(v), cosh = double.Cosh(v);
                double ssinh = s * sinh, scosh = s * cosh;

                double delta = v + ssinh - u;
                if (double.Abs(delta) < u * 1e-10) {
                    return (v, convergenced: true);
                }

                double g1 = scosh + 1d;
                double g2 = ssinh;
                double g3 = scosh;
                double dv = Householder4(delta, g1, g2, g3);

                if (!double.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            public static (ddouble v, bool convergenced) HyperbolicIter(ddouble v, ddouble u, ddouble s) {
                ddouble sin = ddouble.Sinh(v), cos = ddouble.Cosh(v);
                ddouble ssinh = s * sin, scosh = s * cos;

                ddouble delta = v + ssinh - u;
                if (double.Abs(delta.Hi) < u.Hi * 1e-30) {
                    return (v, convergenced: true);
                }

                ddouble g1 = scosh + 1d;
                ddouble g2 = ssinh;
                ddouble g3 = scosh;
                ddouble dv = Householder4(delta, g1, g2, g3);

                if (!ddouble.IsFinite(dv)) {
                    return (v, convergenced: true);
                }

                v -= dv;

                return (v, convergenced: false);
            }

            private static double Householder4(double delta, double g1, double g2, double g3) {
                double sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                double dv = 3 * delta * (sqg1 + sqg1_deltag2) /
                    (6 * g1 * sqg1_deltag2 + delta * delta * g3);
                return dv;
            }

            private static ddouble Householder4(ddouble delta, ddouble g1, ddouble g2, ddouble g3) {
                ddouble sqg1 = g1 * g1, sqg1_deltag2 = sqg1 - delta * g2;

                ddouble dv = 3 * delta * (sqg1 + sqg1_deltag2) /
                    (6 * g1 * sqg1_deltag2 + delta * delta * g3);
                return dv;
            }
        }
    }
}