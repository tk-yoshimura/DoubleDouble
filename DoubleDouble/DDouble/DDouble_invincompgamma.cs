namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseLowerIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }

            if (nu > Consts.IncompleteGamma.MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNuRegularized} is not supported."
                );
            }

            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 1d) {
                return PositiveInfinity;
            }

            if (x == 0d || nu < Consts.IncompleteGamma.MinNu) {
                return Zero;
            }

            return InverseIncompleteGammaUtil.Kernel(nu, x, Log(x), Log(1d - x));
        }

        public static ddouble InverseUpperIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }

            if (nu > Consts.IncompleteGamma.MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNuRegularized} is not supported."
                );
            }

            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 0d) {
                return PositiveInfinity;
            }

            if (x == 1d || nu < Consts.IncompleteGamma.MinNu) {
                return Zero;
            }

            return InverseIncompleteGammaUtil.Kernel(nu, 1d - x, Log(1d - x), Log(x));
        }


        internal static class InverseIncompleteGammaUtil {
            const int RootFindMaxIter = 32;

            public static ddouble Kernel(ddouble nu, ddouble p, ddouble lnp_lower, ddouble lnp_upper) {
                double p5 = IncompleteGammaP5(nu.hi);

                ddouble lngamma = LogGamma(nu);
                ddouble prev_dx = 0d;

                ddouble x = (nu > 1d) ? Min(p5, Exp((Log(nu) + lnp_lower + lngamma) / nu)) : Pow(p, 1d / nu);

                for (int i = 0, convergence_times = 0; i < RootFindMaxIter && convergence_times < 2; i++) {
                    bool lower = x < (double)nu + Consts.IncompleteGamma.ULBias;

                    ddouble f = lower
                        ? LowerIncompleteGammaCFrac.Value(nu, x)
                        : UpperIncompleteGammaCFrac.Value(nu, x);

                    ddouble lnv = Log(x);
                    ddouble y = nu * lnv - (x + lngamma) - Log(f);

                    ddouble delta = lower ? (y - lnp_lower) : (y - lnp_upper);
                    ddouble r = delta * (x + f - nu + 1d);

                    ddouble dx = (double.Abs(r.hi) <= double.Abs(f.hi) * 0.125)
                        ? Ldexp(delta * x, 1) / (r + Ldexp(f, 1))
                        : delta * x / f;

                    if (IsNaN(dx)) {
                        break;
                    }
                    if (dx.hi * prev_dx.hi < 0d) {
                        dx = Ldexp(dx, -1);
                    }

                    x = lower ? Max(Ldexp(x, -2), x - dx) : Min(Ldexp(x, 2), x + dx);

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 5e-32) {
                        break;
                    }

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 1e-28) {
                        convergence_times++;
                    }

                    prev_dx = dx;
                }

                return x;
            }

            public static double IncompleteGammaP5(double nu) {
                double nu_ln2 = double.Log2(nu);

                if (nu <= 1) {
                    double b = nu * (1.184797 + nu * (-1.64793 + nu * (-1.87704 + nu * (3.354884 + nu * (-1.93304)))));
                    double c = b - nu_ln2;
                    double x = double.Exp2(-double.Exp2(c));

                    return x;
                }
                else {
                    double b =
                        (-0.92055 + nu_ln2 * (-1.34724 + nu_ln2 * (-0.41283 + nu_ln2 * (-0.11429))))
                        / (1 + nu_ln2 * (0.295360 + nu_ln2 * (0.114197)));

                    double x = nu * double.Exp2(-double.Exp2(b));

                    return x;
                }
            }
        }
    }
}