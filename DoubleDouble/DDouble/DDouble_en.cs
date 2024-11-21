using Microsoft.VisualBasic;
using static DoubleDouble.ddouble.Consts;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble En(int n, ddouble x) {
            const int max_n = 256, max_iter = 4096;

            if (n < 0 || IsNaN(x) || IsNegative(x)) {
                return NaN;
            }

            if (n > max_n) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    $"In the calculation of the En function, {nameof(n)} greater than {max_n} is not supported."
                );
            }

            if (ILogB(x) < -105) {
                return Rcp(n - 1);
            }

            ddouble t = Exp(-x);

            if (n == 0) {
                return t / x;
            }

            if (IsZero(t)) {
                return 0d;
            }

            if (x >= 1d) {
                (ddouble a0, ddouble a1) = (0d, 1d);
                (ddouble b0, ddouble b1) = (1d, x);

                ddouble s = a1 / b1;

                for (int k = 0; k < max_iter; k++) {
                    (a0, a1) = (a1, (k + n) * a0 + a1);
                    (b0, b1) = (b1, (k + n) * b0 + b1);
                    (a0, a1) = (a1, (k + 1) * a0 + x * a1);
                    (b0, b1) = (b1, (k + 1) * b0 + x * b1);

                    s = a1 / b1;

                    (int exp, (a1, b1)) = AdjustScale(0, (a1, b1));
                    (a0, b0) = (Ldexp(a0, exp), Ldexp(b0, exp));

                    if (k > 0 && (k & 3) == 0) {
                        ddouble r0 = a0 * b1, r1 = a1 * b0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-30)) {
                            break;
                        }
                    }
                }

                ddouble y = s * t;

                return y;
            }
            else {
                ddouble c = -Log(x) - EulerGamma;
                ddouble s = (n != 1) ? Rcp(n - 1) : c;
                ddouble f = 1d;

                for (int k = 1; k <= max_iter; k++) {
                    ddouble ds;

                    if (k != n - 1) {
                        ds = Rcp(n - k - 1);
                    }
                    else {
                        ds = c;
                        for (int m = 1; m < n; m++) {
                            ds += Rcp(m);
                        }
                    }

                    f *= -x / k;
                    s = SeriesUtil.Add(s, f, ds, out bool convergence);

                    if ((k > n && convergence) || IsZero(f)) {
                        break;
                    }
                }

                return s;
            }
        }
    }
}