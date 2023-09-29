namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble LowerIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu > Consts.IncompleteGamma.MaxNu) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNu} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < (double)nu + Consts.IncompleteGamma.ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Exp(nu * Log(x) - x) / f;

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Gamma(nu) - Exp(nu * Log(x) - x) / f;

                return y;
            }
        }

        public static ddouble UpperIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu > Consts.IncompleteGamma.MaxNu) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNu} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < Consts.IncompleteGamma.Eps) {
                return Gamma(nu);
            }
            if (nu < Consts.IncompleteGamma.Eps) {
                return -Ei(-x);
            }

            if (x < (double)nu + Consts.IncompleteGamma.ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Gamma(nu) - Exp(nu * Log(x) - x) / f;

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Exp(nu * Log(x) - x) / f;

                return y;
            }
        }

        public static ddouble LowerIncompleteGammaRegularized(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu > Consts.IncompleteGamma.MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGammaRegularized function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNuRegularized} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < (double)nu + Consts.IncompleteGamma.ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Exp(nu * Log(x) - x - LogGamma(nu)) / f;
                y = Min(y, One);

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = 1d - Exp(nu * Log(x) - x - LogGamma(nu)) / f;
                y = Max(y, Zero);

                return y;
            }
        }

        public static ddouble UpperIncompleteGammaRegularized(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu > Consts.IncompleteGamma.MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGammaRegularized function, " +
                    $"{nameof(nu)} greater than {Consts.IncompleteGamma.MaxNuRegularized} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < Consts.IncompleteGamma.Eps) {
                return One;
            }

            if (x < (double)nu + Consts.IncompleteGamma.ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = One - Exp(nu * Log(x) - x - LogGamma(nu)) / f;
                y = Max(y, Zero);

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Exp(nu * Log(x) - x - LogGamma(nu)) / f;
                y = Min(y, One);

                return y;
            }
        }

        internal static partial class Consts {
            internal static class IncompleteGamma {
                public static readonly double Eps = double.ScaleB(1, -105);
                public const double ULBias = 0.125;
                public const double MaxNu = Consts.Gamma.ExtremeLarge;
                public const double MaxNuRegularized = 8192d;
                public const int CFracMaxIter = 8192;
            }
        }

        internal static class UpperIncompleteGammaCFrac {
            public static double Eps = double.ScaleB(1, -105);

            public static ddouble Value(ddouble nu, ddouble x) {
                ddouble xmnu = x - nu, xmnui = x - nu + 3d, nui = nu - 1d;
                ddouble p0 = 0d, p1 = nui, p2 = 0;
                ddouble q0 = 0d, q1 = xmnui, q2 = 1;

                for (int i = 2; i < Consts.IncompleteGamma.CFracMaxIter; i++) {
                    nui -= 1d; xmnui += 2d;

                    ddouble a = i * nui;
                    ddouble b = xmnui;

                    p0 = a * p2 + b * p1;
                    q0 = a * q2 + b * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i > 0 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p1, p2) = (p0, p1);
                    (q1, q2) = (q0, q1);
                }

                ddouble f = xmnu + 1d + p0 / q0;

                return f;
            }
        }

        internal static class LowerIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x) {
                ddouble nux = nu * x;
                ddouble p0 = 0d, p1 = 0d, p2 = -nux, p3 = 0d;
                ddouble q0 = 0d, q1 = 0d, q2 = nu + 1d, q3 = 1d;

                ddouble ix = 0d, nu2i = nu;

                for (int i = 1; i < Consts.IncompleteGamma.CFracMaxIter; i++) {
                    ix += x; nu2i += 2d;

                    ddouble a1 = ix, a2 = -nux - ix;
                    ddouble b1 = nu2i, b2 = b1 + 1d;

                    p1 = a1 * p3 + b1 * p2;
                    q1 = a1 * q3 + b1 * q2;
                    p0 = a2 * p2 + b2 * p1;
                    q0 = a2 * q2 + b2 * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i > 0 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p2, p3) = (p0, p1);
                    (q2, q3) = (q0, q1);
                }

                ddouble f = nu + p0 / q0;

                return f;
            }
        }
    }
}