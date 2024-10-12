using DoubleDouble;
using System;
using System.Diagnostics;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Recurrence.BesselJ(27.75, 2.5);

            Console.WriteLine("END");
            Console.Read();
        }

        const int MaxN = 16;

        public static class Recurrence {

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                Debug.Assert(nu > MaxN || nu < -MaxN);

                ddouble nu_abs = ddouble.Abs(nu);
                int n = (int)ddouble.Floor(nu_abs);
                ddouble alpha = nu_abs - n;

                ddouble v = 1d / x;

                if (ddouble.IsPositive(nu)) {
                    (ddouble a0, ddouble b0, ddouble a1, ddouble b1) = (1d, 0d, 0d, 1d);

                    ddouble r = ddouble.Ldexp(nu_abs * v, 1);
                    (a0, b0, a1, b1) = (a1, b1, r * a1 + a0, r * b1 + b0);

                    ddouble s = 1d;

                    for (int i = 1; i <= 1024; i++) {
                        r = ddouble.Ldexp((nu_abs + i) * v, 1);

                        (a0, b0, a1, b1) = (a1, b1, r * a1 - a0, r * b1 - b0);
                        s = a1 / b1;

                        (int exp, (a1, b1)) = ddouble.AdjustScale(0, (a1, b1));
                        (a0, b0) = (ddouble.Ldexp(a0, exp), ddouble.Ldexp(b0, exp));

                        if (i > 0 && (i & 3) == 0) {
                            ddouble r0 = a0 * b1, r1 = a1 * b0;
                            if (!(ddouble.Abs(r0 - r1) > ddouble.Min(ddouble.Abs(r0), ddouble.Abs(r1)) * 1e-30)) {
                                break;
                            }
                        }
                    }

                    long exp_sum = 0;
                    (ddouble j0, ddouble j1) = (ddouble.Abs(s) > 1d) ? ((ddouble)1d, 1d / s) : (s, 1d);

                    for (int k = n - 1; k >= MaxN; k--) {
                        (j1, j0) = (ddouble.Ldexp(k + alpha, 1) * v * j1 - j0, j1);

                        if (int.Sign(ddouble.ILogB(j0)) == int.Sign(ddouble.ILogB(j1))) {
                            int exp = ddouble.ILogB(j1);
                            exp_sum += exp;
                            (j0, j1) = (ddouble.Ldexp(j0, -exp), ddouble.Ldexp(j1, -exp));
                        }
                    }

                    ddouble y = ddouble.Ldexp(
                        ddouble.BesselJ(alpha + (MaxN - 1), x) / j1,
                        (int)long.Clamp(-exp_sum, int.MinValue, int.MaxValue)
                    ) * ((ddouble.Abs(s) > 1d) ? 1d : s);

                    return y;
                }
                else { 
                    ddouble j0 = ddouble.BesselJ(-(alpha + (MaxN - 2)), x);
                    ddouble j1 = ddouble.BesselJ(-(alpha + (MaxN - 1)), x);

                    for (int k = MaxN - 1; k < n; k++) {
                        (j1, j0) = (-ddouble.Ldexp(k + alpha, 1) * v * j1 - j0, j1);
                    }

                    return j1;
                }
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                Debug.Assert(nu > MaxN || nu < -MaxN);

                ddouble nu_abs = ddouble.Abs(nu);
                int n = (int)ddouble.Floor(nu_abs);
                ddouble alpha = nu_abs - n;

                ddouble v = 1d / x;

                if (ddouble.IsPositive(nu)) {
                    ddouble y0 = ddouble.BesselY(alpha + (MaxN - 2), x);
                    ddouble y1 = ddouble.BesselY(alpha + (MaxN - 1), x);

                    for (int k = MaxN - 1; k < n; k++) {
                        (y1, y0) = (ddouble.Ldexp(k + alpha, 1) * v * y1 - y0, y1);
                    }

                    return y1;
                }
                else {
                    if (ddouble.ILogB(alpha - 0.5d) < -106) {
                        return ((n & 1) == 0 ? BesselJ(-nu, x) : -BesselJ(-nu, x));
                    }

                    ddouble y0 = ddouble.BesselY(-(alpha + (MaxN - 2)), x);
                    ddouble y1 = ddouble.BesselY(-(alpha + (MaxN - 1)), x);

                    for (int k = MaxN - 1; k < n; k++) {
                        (y1, y0) = (-ddouble.Ldexp(k + alpha, 1) * v * y1 - y0, y1);
                    }

                    return y1;
                }
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale) {
                Debug.Assert(nu > MaxN || nu < -MaxN);

                ddouble nu_abs = ddouble.Abs(nu);
                int n = (int)ddouble.Floor(nu_abs);
                ddouble alpha = nu_abs - n;

                ddouble v = 1d / x;

                (ddouble a0, ddouble b0, ddouble a1, ddouble b1) = (1d, 0d, 0d, 1d);
                ddouble s = 1d;

                for (int i = 0; i <= 1024; i++) {
                    ddouble r = ddouble.Ldexp((nu_abs + i) * v, 1);

                    (a0, b0, a1, b1) = (a1, b1, r * a1 + a0, r * b1 + b0);
                    s = a1 / b1;

                    (int exp, (a1, b1)) = ddouble.AdjustScale(0, (a1, b1));
                    (a0, b0) = (ddouble.Ldexp(a0, exp), ddouble.Ldexp(b0, exp));

                    if (i > 0 && (i & 3) == 0) {
                        ddouble r0 = a0 * b1, r1 = a1 * b0;
                        if (!(ddouble.Abs(r0 - r1) > ddouble.Min(ddouble.Abs(r0), ddouble.Abs(r1)) * 1e-30)) {
                            break;
                        }
                    }
                }

                if (!ddouble.IsFinite(s)) {
                    return 0d;
                }

                long exp_sum = 0;
                ddouble i0 = 1d, i1 = 1d / s;

                for (int k = n - 1; k >= MaxN; k--) {
                    (i1, i0) = (ddouble.Ldexp(k + alpha, 1) * v * i1 + i0, i1);

                    if (ddouble.ILogB(i1) > 0) {
                        int exp = ddouble.ILogB(i1);
                        exp_sum += exp;
                        (i0, i1) = (ddouble.Ldexp(i0, -exp), ddouble.Ldexp(i1, -exp));
                    }
                }

                ddouble y = ddouble.Ldexp(
                    ddouble.BesselI(alpha + (MaxN - 1), x, scale: true) / i1,
                    (int)long.Max(-exp_sum, int.MinValue)
                );

                if (ddouble.IsPositive(nu)) {
                    if (!scale) {
                        y *= ddouble.Exp(x);
                    }
                }
                else if (!ddouble.IsInteger(nu_abs)) {
                    ddouble bk = 2d * ddouble.RcpPI * ddouble.SinPI(nu_abs) * BesselK(nu_abs, x, scale: true);

                    y += bk * (scale ? ddouble.Exp(-2d * x) : ddouble.Exp(-x));
                }

                return y;
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale) {
                nu = ddouble.Abs(nu);

                Debug.Assert(nu > MaxN);

                int n = (int)ddouble.Floor(nu);
                ddouble alpha = nu - n;

                ddouble k0 = ddouble.BesselK(alpha + (MaxN - 2), x, scale: true);
                ddouble k1 = ddouble.BesselK(alpha + (MaxN - 1), x, scale: true);

                ddouble v = 1d / x;

                for (int k = MaxN - 1; k < n; k++) {
                    (k1, k0) = (ddouble.Ldexp(k + alpha, 1) * v * k1 + k0, k1);
                }

                if (!scale) {
                    k1 *= ddouble.Exp(-x);
                }

                return k1;
            }
        }
    }
}
