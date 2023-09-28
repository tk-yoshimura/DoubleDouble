using DoubleDouble;
using static DoubleDouble.ddouble;
using System;
using System.IO;
using System.Diagnostics;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble t1 = IncompleteBetaCFrac.ValueType2(0.25, 3, 2);
            ddouble t2 = IncompleteBetaCFrac.ValueType2(0.75, 2, 3);

            ddouble v1 = ddouble.IncompleteBeta(0.75, 2, 3);

            //using StreamWriter sw = new StreamWriter("../../upper_imcomp_gamma3.csv");

            //sw.WriteLine("x,nu,v");

            //ddouble nu = 1d / 32;

            //for (ddouble x = 0.125; x <= 8192; x += 0.125) {
            //    for (; nu <= 8192; nu += 1d / 32) {
            //        (ddouble v, bool success) = UpperIncompleteGammaCFrac.ValueType2(nu, x);
            //        if (!success) {
            //            sw.WriteLine($"{x},{nu},{v}");
            //            Console.WriteLine($"{x},{nu},{v}");

            //            nu -= 1d / 32;
            //            break;
            //        }
            //    } 
            //}

            //sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }

        internal static class UpperIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x, int m) {
                ddouble f = 1;

                for (int i = m; i >= 1; i--) {
                    f = x + f * (i - nu) / (f + i);
                }

                return f;
            }

            public static (ddouble v, bool success) ValueType2(ddouble nu, ddouble x) {
                ddouble xmnu = x - nu, xmnui = x - nu + 3d, nui = nu - 1d;
                ddouble p0 = 0d, p1 = nui, p2 = 0;
                ddouble q0 = 0d, q1 = xmnui, q2 = 1;

                for (int i = 2; i < 8192; i++) {
                    nui -= 1d;  xmnui += 2d;

                    ddouble a = i * nui;
                    ddouble b = xmnui;

                    p0 = a * p2 + b * p1;
                    q0 = a * q2 + b * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i >= 16 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p1, p2) = (p0, p1);
                    (q1, q2) = (q0, q1);
                }

                ddouble s = xmnu + 1d, c = p0 / q0;
                ddouble f = s + c;

#if DEBUG
                if (s * c < 0 && int.Abs(double.ILogB(s.Hi) - double.ILogB(c.Hi)) <= 1) {
                    Trace.WriteLine("digit loss!");

                    return (f, false);
                }
#endif

                return (f, true);
            }
        }

        internal static class LowerIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x, bool log_scale = false) {
                double log2x = double.Log2((double)x);

                int m = (int)double.Pow(2, ((0.01478 * log2x + 0.2829) * log2x + 3.528)) + 1;

                ddouble f = 1;

                for (int i = m; i >= 0; i--) {
                    f = nu + (2 * i) - (f * (nu + i) * x) / (((i + 1) * x) + f * (nu + (2 * i + 1)));
                }

                return f;
            }

            public static (ddouble v, bool success) ValueType2(ddouble nu, ddouble x) {
                ddouble p0 = 0d, p1 = -nu * x, p2 = 0;
                ddouble q0 = 0d, q1 = nu + 1, q2 = 1;

                for (int i = 1; i < 8192; i++) {
                    ddouble a1 = i * x, a2 = -(nu + i) * x;
                    ddouble b1 = nu + (2 * i), b2 = b1 + 1;

                    p0 = a1 * p2 + b1 * p1;
                    q0 = a1 * q2 + b1 * q1;
                    (p1, p2) = (p0, p1);
                    (q1, q2) = (q0, q1);
                    p0 = a2 * p2 + b2 * p1;
                    q0 = a2 * q2 + b2 * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i >= 16 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p1, p2) = (p0, p1);
                    (q1, q2) = (q0, q1);
                }

                ddouble c = p0 / q0;
                ddouble f = nu + c;

#if DEBUG
                if (nu * c < 0 && int.Abs(double.ILogB(nu.Hi) - double.ILogB(c.Hi)) <= 1) {
                    Trace.WriteLine("digit loss!");

                    return (f, false);
                }
#endif

                return (f, true);
            }

            public static (ddouble v, bool success) ValueType3(ddouble nu, ddouble x) {
                ddouble nux = nu * x;
                ddouble p0 = 0d, p1 = 0d, p2 = -nux, p3 = 0d;
                ddouble q0 = 0d, q1 = 0d, q2 = nu + 1d, q3 = 1d;

                ddouble ix = 0d, nu2i = nu;

                for (int i = 1; i < 8192; i++) {
                    ix += x;  nu2i += 2d;

                    ddouble a1 = ix, a2 = -nux - ix;
                    ddouble b1 = nu2i, b2 = b1 + 1d;

                    p1 = a1 * p3 + b1 * p2;
                    q1 = a1 * q3 + b1 * q2;
                    p0 = a2 * p2 + b2 * p1;
                    q0 = a2 * q2 + b2 * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i >= 16 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p2, p3) = (p0, p1);
                    (q2, q3) = (q0, q1);
                }

                ddouble c = p0 / q0;
                ddouble f = nu + c;

#if DEBUG
                if (nu * c < 0 && int.Abs(double.ILogB(nu.Hi) - double.ILogB(c.Hi)) <= 1) {
                    Trace.WriteLine("digit loss!");

                    return (f, false);
                }
#endif

                return (f, true);
            }
        }

        internal static class IncompleteBetaCFrac {
            public static ddouble Value(ddouble x, ddouble a, ddouble b, int m) {
                ddouble f = 0d;

                for (int n = m; n >= 0; n--) {
                    ddouble na = n + a, nb = n - b, nab = n + a + b;
                    ddouble n2a = 2 * n + a, n2a1 = n2a + 1d, n2a2 = n2a + 2d;

                    ddouble v = (f + 1d) * n2a1 * n2a2;

                    f = (na * nab * x * v) / (n2a * n2a1 * ((n + 1d) * (nb + 1d) * x - v));
                }

                f = a * (1d + f);

                return f;
            }

            public static ddouble ValueType2(ddouble x, ddouble a, ddouble b) {
                ddouble ab = a + b;
                ddouble p0 = 0d, p1 = 0d, p2 = -(ab * x) / (a + 1d), p3 = 0d;
                ddouble q0 = 0d, q1 = 0d, q2 = 1d, q3 = 1d;

                ddouble a2i = a, ai = a, bi = b, abi = ab;

                for (int i = 1; i < 8192; i++) {
                    a2i += 2d; ai += 1d; bi -= 1d; abi += 1d;

                    ddouble a1 = (bi * i * x) / ((a2i - 1d) * a2i);
                    ddouble a2 = (ai * abi * x) / ((a2i + 1d) * a2i);

                    p1 = p2 + a1 * p3;
                    q1 = q2 + a1 * q3;
                    p0 = p1 - a2 * p2;
                    q0 = q1 - a2 * q2;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i >= 16 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p2, p3) = (p0, p1);
                    (q2, q3) = (q0, q1);
                }

                ddouble f = a * (1d + p0 / q0);

                return f;
            }
        }
    }
}
