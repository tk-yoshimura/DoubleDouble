using DoubleDouble;
using System;
using System.Diagnostics;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Recurrence.BesselI(20.25, 2.5, scale: false);

            Console.WriteLine("END");
            Console.Read();
        }

        const int MaxN = 16;

        public static class Recurrence {

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale) {
                bool is_plus = ddouble.IsPositive(nu);
                nu = ddouble.Abs(nu);

                Debug.Assert(nu > MaxN);

                int n = (int)ddouble.Floor(nu);
                ddouble alpha = nu - n;

                ddouble v = 1d / x;

                (ddouble a0, ddouble b0, ddouble a1, ddouble b1) = (1d, 0d, 0d, 1d);
                ddouble s = 1d;

                for (int i = 0; i <= 1024; i++) {
                    ddouble r = ddouble.Ldexp((nu + i) * v, 1);

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

                (ddouble i0, ddouble i1) = (1d, 1d / s);

                for (int k = n; k > MaxN; k--) {
                    (i1, i0) = (ddouble.Ldexp((k - 1) + alpha, 1) * v * i1 + i0, i1);

                    if (ddouble.IsPositiveInfinity(i1)) {
                        return 0d;
                    }
                }

                ddouble y = ddouble.BesselI(alpha + (MaxN - 1), x, scale: true) / i1;

                if (!scale) {
                    y *= ddouble.Exp(x);
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
