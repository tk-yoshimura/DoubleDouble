namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseIncompleteBeta(ddouble x, ddouble a, ddouble b) {
            if (x < 0d || !(x <= 1d) || !(a > 0d) || !(b > 0d)) {
                return NaN;
            }

            if (a + b - Max(a, b) > Consts.IncompleteBeta.MaxABRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteBetaRegularized function, " +
                    $"{nameof(a)}+{nameof(b)}-max({nameof(a)},{nameof(b)}) greater than" +
                    $" {Consts.IncompleteBeta.MaxABRegularized} is not supported."
                );
            }

            if (IsZero(x)) {
                return Zero;
            }
            if (x == 1d) {
                return One;
            }

            return InverseIncompleteBetaUtil.Kernel(a, b, x, Log(x), Log(1d - x));
        }

        internal static class InverseIncompleteBetaUtil {
            const int RootFindMaxIter = 32;

            public static ddouble Kernel(ddouble a, ddouble b, ddouble p, ddouble lnp_lower, ddouble lnp_upper) {
                double thr = (a.hi + 1d) / (a.hi + b.hi + 1d);

                ddouble lnbeta = LogBeta(a, b), abm2 = a + b - 2d, am1 = a - 1d;
                ddouble prev_dx = 0d;

                ddouble x = Clamp(p, 1 / 65536d, 65535 / 65536d);

                for (int i = 0, convergence_times = 0; i < RootFindMaxIter && convergence_times < 2; i++) {
                    bool lower = x < thr;

                    ddouble xr = 1d - x;

                    ddouble f = lower
                        ? IncompleteBetaCFrac.Value(x, a, b)
                        : IncompleteBetaCFrac.Value(xr, b, a);

                    ddouble lnv = Log(x);
                    ddouble y = a * Log(x) + b * Log(xr) - lnbeta - Log(f);

                    ddouble delta = lower ? (y - lnp_lower) : (y - lnp_upper);
                    ddouble r = delta * (abm2 * x - am1 + f);

                    ddouble dx = (double.Abs(r.hi) <= double.Abs(f.hi) * 0.125)
                        ? (delta * x * xr) / (f + Ldexp(r, -1))
                        : (delta * x * xr) / f;

                    if (IsNaN(dx)) {
                        break;
                    }
                    if (dx.hi * prev_dx.hi < 0d) {
                        dx = Ldexp(dx, -1);
                    }

                    x = lower ? Max(Ldexp(x, -16), x - dx) : Min(1d - Ldexp(xr, -16), x + dx);

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 5e-32) {
                        break;
                    }

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 1e-28) {
                        convergence_times++;
                    }

                    ddouble c = ddouble.IncompleteBetaRegularized(x, a, b);
                    Console.WriteLine($"{x},{dx},{c},{lower}");

                    prev_dx = dx;
                }

                return x;
            }
        }
    }
}