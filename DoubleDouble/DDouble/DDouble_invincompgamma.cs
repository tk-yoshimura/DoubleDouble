namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseLowerIncompleteGamma(ddouble nu, ddouble x) {
            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 0d || nu < Consts.IncompleteGamma.MinNu) {
                return Zero;
            }

            return InverseIncompleteGamma.Kernel(nu, x, Log(x), Log(1d - x));
        }

        public static ddouble InverseUpperIncompleteGamma(ddouble nu, ddouble x) {
            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 0d || nu < Consts.IncompleteGamma.MinNu) {
                return PositiveInfinity;
            }

            return InverseIncompleteGamma.Kernel(nu, 1d - x, Log(1d - x), Log(x));
        }


        internal static class InverseIncompleteGamma {
            public static ddouble Kernel(ddouble nu, ddouble p, ddouble lnp_lower, ddouble lnp_upper) {
                double p5 = IncompleteGammaP5(nu.hi);

                ddouble lngamma = LogGamma(nu);
                ddouble prev_dx = 1;

                ddouble x = Min(p5, nu > 1d / 32 ? Exp((Log(nu) + lnp_lower + lngamma) / nu) : Pow(p, 1d / nu));

                ddouble v0 = LowerIncompleteGammaRegularized(nu, x);
                Console.WriteLine($"{x},{v0}");

                for (int i = 0, convergence_times = 0; i < 32 && convergence_times < 4; i++) {
                    bool lower = x < (double)nu + Consts.IncompleteGamma.ULBias;

                    ddouble f = lower
                        ? LowerIncompleteGammaCFrac.Value(nu, x)
                        : UpperIncompleteGammaCFrac.Value(nu, x);

                    ddouble lnv = Log(x);
                    ddouble y = nu * lnv - (x + lngamma) - Log(f);

                    ddouble delta = lower ? (y - lnp_lower) : (y - lnp_upper);

                    ddouble r = delta * (x + f - nu + 1d);
                    ddouble c = Ldexp(delta * x, 1) / (delta * (x + f - nu + 1d) + Ldexp(f, 1));
                    ddouble d = delta * x / f;

                    Console.WriteLine($"{f},{r},{c},{d}");

                    ddouble dx = x >= 0.25
                        ? Ldexp(delta * x, 1) / (delta * (x + f - nu + 1d) + Ldexp(f, 1))
                        : delta * x / f;

                    if (IsNaN(dx)) {
                        break;
                    }
                    if (Sign(dx) != Sign(prev_dx)) {
                        dx /= 2;
                    }

                    x = lower ? Max(Ldexp(x, -4), x - dx) : Min(Ldexp(x, 4), x + dx);

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 5e-32) {
                        break;
                    }

                    if (double.Abs(dx.hi) <= double.Abs(x.hi) * 1e-28) {
                        convergence_times++;
                    }

                    ddouble v = LowerIncompleteGammaRegularized(nu, x);
                    Console.WriteLine($"{x},{dx},{v},{lower}");

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