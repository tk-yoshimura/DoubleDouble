using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble AngerJ(int n, ddouble x) {
            return BesselJ(n, x);
        }

        public static ddouble WeberE(int n, ddouble x) {
            if (n < 0 || n > 8) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the WeberE function, n greater than 8 and negative integer are not supported."
                );
            }

            if (IsNegative(x)) {
                return ((n & 1) == 0) ? -WeberE(n, -x) : WeberE(n, -x);
            }

            if (n == 0) {
                return -StruveH(0, x);
            }
            if (n == 1) {
                return AngerWeberUtil.RcpPI2 - StruveH(1, x);
            }

            if (x < AngerWeberUtil.NZThreshold) {
                return AngerWeberUtil.WeberENearZero(n, x);
            }
            else if (IsFinite(x)) {
                return AngerWeberUtil.WeberERecurrence(n, x);
            }
            else {
                return NaN;
            }
        }

        internal static class AngerWeberUtil {
            public const double NZThreshold = 4d;
            public static readonly ddouble RcpPI2 = Ldexp(RcpPI, 1);

            static readonly List<ddouble> nz_gamma_neg = new() { 0d, -1d / Ldexp(Sqrt(PI), 1), 0d };
            static readonly List<ddouble> nz_gamma_pos = new() { 0d, 1d / Sqrt(PI), 1d };

            public static ddouble WeberENearZero(int n, ddouble x) {
                Debug.Assert(n >= 2, nameof(n));

                return ((n & 1) == 0)
                    ? WeberENearZeroEvenN(n, x)
                    : WeberENearZeroOddN(n, x);
            }

            public static ddouble WeberENearZeroEvenN(int n, ddouble x) {
                Debug.Assert((n & 1) == 0, nameof(n));
                Debug.Assert(x <= NZThreshold, nameof(x));

                ddouble x_half = Ldexp(x, -1), v = x_half * x_half;
                ddouble t = 0d;
                ddouble u = x_half, w = v * v;

                for (int k = 0; k <= 256; k += 2) {
                    t = SeriesUtil.Add(t, u,
                        NzGammaDenom(2 * k + n + 3) * NzGammaDenom(2 * k - n + 3),
                        -v * NzGammaDenom(2 * k + n + 5) * NzGammaDenom(2 * k - n + 5),
                        out bool convergence
                    );

                    if (2 * k > n && convergence) {
                        break;
                    }

                    u *= w;
                }

                ddouble y = ((n / 2) & 1) == 0 ? -t : t;

                return y;
            }

            public static ddouble WeberENearZeroOddN(int n, ddouble x) {
                Debug.Assert((n & 1) == 1, nameof(n));
                Debug.Assert(x <= NZThreshold, nameof(x));

                ddouble x_half = Ldexp(x, -1), v = x_half * x_half;
                ddouble s = 0d;
                ddouble u = 1d, w = v * v;

                for (int k = 0; k <= 256; k += 2) {
                    s = SeriesUtil.Add(s, u,
                        NzGammaDenom(2 * k + n + 2) * NzGammaDenom(2 * k - n + 2),
                        -v * NzGammaDenom(2 * k + n + 4) * NzGammaDenom(2 * k - n + 4),
                        out bool convergence
                    );

                    if (2 * k > n && convergence) {
                        break;
                    }

                    u *= w;
                }

                ddouble y = ((n / 2) & 1) == 0 ? s : -s;

                return y;
            }

            public static ddouble WeberERecurrence(int n, ddouble x) {
                Debug.Assert(n >= 1, nameof(n));
                Debug.Assert(x >= NZThreshold, nameof(x));

                (ddouble y0, ddouble y1) = (-StruveH(0, x), RcpPI2 - StruveH(1, x));
                ddouble v = 2d / x;

                for (int k = 1; k < n; k++) {
                    ddouble v2 = ((k & 1) == 0)
                        ? (k * y1 * v - y0)
                        : ((k * y1 - RcpPI2) * v - y0);

                    (y0, y1) = (y1, v2);
                }

                return y1;
            }

            public static ddouble NzGammaDenom(int n) {
                if (n >= 0) {
                    for (int k = nz_gamma_pos.Count; k <= n; k++) {
                        ddouble c = nz_gamma_pos[^2] / (k * 0.5d - 1d);

                        nz_gamma_pos.Add(c);
                    }

                    return nz_gamma_pos[n];
                }
                else {
                    n = -n;

                    for (int k = nz_gamma_neg.Count; k <= n; k++) {
                        ddouble c = nz_gamma_neg[^2] * -(k * 0.5d);

                        nz_gamma_neg.Add(c);
                    }

                    return nz_gamma_neg[n];
                }
            }
        }
    }
}