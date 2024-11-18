using static DoubleDouble.ddouble.Consts.IncompleteGamma;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseLowerIncompleteGamma(ddouble nu, ddouble x) {
            if (nu > MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {MaxNuRegularized} is not supported."
                );
            }

            if (!(nu >= 0d && x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 1d) {
                return PositiveInfinity;
            }

            if (x == 0d || nu < MinNu) {
                return 0d;
            }

            return InverseIncompleteGammaUtil.Kernel(nu, x, Log(x), Log1p(-x));
        }

        public static ddouble InverseUpperIncompleteGamma(ddouble nu, ddouble x) {
            if (nu > MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {MaxNuRegularized} is not supported."
                );
            }

            if (!(nu >= 0d && x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 0d) {
                return PositiveInfinity;
            }

            if (x == 1d || nu < MinNu) {
                return 0d;
            }

            return InverseIncompleteGammaUtil.Kernel(nu, 1d - x, Log1p(-x), Log(x));
        }


        internal static class InverseIncompleteGammaUtil {
            const int RootFindMaxIter = 32;

            public static ddouble Kernel(ddouble nu, ddouble p, ddouble lnp_lower, ddouble lnp_upper) {
                double p5 = InverseIncompleteGammaP5RoughApprox(nu.hi);

                ddouble lngamma = LogGamma(nu), num1 = nu - 1d;
                ddouble prev_dx = 0d;

                ddouble x = (nu > 1d) ? Min(p5, Exp((Log(nu) + lnp_lower + lngamma) / nu)) : Pow(p, 1d / nu);

                for (int i = 0, convergence_times = 0; i < RootFindMaxIter && convergence_times < 2; i++) {
                    bool lower = x < (double)nu + ULBias;

                    ddouble f = lower
                        ? LowerIncompleteGammaCFrac.Value(nu, x)
                        : UpperIncompleteGammaCFrac.Value(nu, x);

                    ddouble lnv = Log(x);
                    ddouble y = nu * lnv - (x + lngamma) - Log(f);

                    ddouble delta = lower ? (y - lnp_lower) : (y - lnp_upper);
                    ddouble r = delta * (x - num1 + f);

                    ddouble dx = (double.Abs(r.hi) <= double.Abs(f.hi) * 0.125)
                        ? (delta * x) / (f + Ldexp(r, -1))
                        : (delta * x) / f;

                    if (IsNaN(dx)) {
                        break;
                    }
                    if (dx.hi * prev_dx.hi < 0d) {
                        dx = Ldexp(dx, -1);
                    }

                    x = Clamp(lower ? (x - dx) : (x + dx), Ldexp(x, -2), Ldexp(x, 2));

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

            public static double InverseIncompleteGammaP5RoughApprox(double nu) {
                if (nu < 1) {
                    if (double.ILogB(nu) < -10) {
                        return 0d;
                    }
                    double lnnu = double.Log2(nu);

                    if (lnnu < -5d) {
                        double x = lnnu + 10d;

                        double y = double.Exp2(
                            (-1.0248306602097177e3 + x *
                            (2.6143104084235887e2 + x *
                            (-2.4843362513105458e1 + x *
                            (8.6210811743975114e-1)))) /
                            (1.0000000000000000e0 + x *
                            (4.3747475653644826e-1 + x *
                            (8.7255090723854334e-2 + x *
                            (9.9424760331912608e-3 + x *
                            (6.9090681630088890e-4))))));

                        return y;
                    }
                    else {
                        double x = lnnu + 5d;

                        double y = double.Exp2(
                            (-3.2796218087516107e1 + x *
                            (1.6421847712959973e1 + x *
                            (-5.0385514986506849e0 + x *
                            (3.6317787662606308e-1 + x *
                            (4.5317428534706485e-2 + x *
                            (-1.8282066421184825e-2 + x *
                            (1.7302366594107276e-3 + x *
                            (2.7329974939713755e-4)))))))) /
                            (1.0000000000000000e0 + x *
                            (1.7635490513821452e-1 + x *
                            (3.8910540764790875e-2 + x *
                            (2.8141637896196611e-2 + x *
                            (8.8767301085413771e-3 + x *
                            (1.5365519043210276e-3 + x *
                            (3.4291020734304705e-4))))))));

                        return y;
                    }
                }
                else {
                    double x = nu - 1d;

                    double y =
                        (6.9314716453657309e-1 + x *
                        (3.2001874529617926e0 + x *
                        (5.9592048056324707e0 + x *
                        (5.6693633060543100e0 + x *
                        (2.8183920385221197e0 + x *
                        (6.0713065842872752e-1)))))) /
                        (1.0000000000000000e0 + x *
                        (3.2202901330430764e0 + x *
                        (4.0482784241994182e0 + x *
                        (2.4136382662363014e0 + x *
                        (6.0713065842872752e-1)))));

                    return y;
                }
            }
        }
    }
}