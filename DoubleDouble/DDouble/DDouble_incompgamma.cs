using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.IncompleteGamma;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble LowerIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu > MaxNu) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {MaxNu} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < (double)nu + ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Pow2(nu * Log2(x) - x * LbE) / f;

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Gamma(nu) - Pow2(nu * Log2(x) - x * LbE) / f;

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

            if (nu > MaxNu) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGamma function, " +
                    $"{nameof(nu)} greater than {MaxNu} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (nu < Eps) {
                return -Ei(-x);
            }

            if (x < (double)nu + ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Gamma(nu) - Pow2(nu * Log2(x) - x * LbE) / f;

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Pow2(nu * Log2(x) - x * LbE) / f;

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

            if (nu > MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGammaRegularized function, " +
                    $"{nameof(nu)} greater than {MaxNuRegularized} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (nu < MinNu) {
                return IsZero(x) ? 0d : 1d;
            }

            if (x < (double)nu + ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Pow2(nu * Log2(x) - (x + LogGamma(nu)) * LbE) / f;
                y = Min(y, 1d);

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = 1d - Pow2(nu * Log2(x) - (x + LogGamma(nu)) * LbE) / f;
                y = Max(y, 0d);

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

            if (nu > MaxNuRegularized) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteGammaRegularized function, " +
                    $"{nameof(nu)} greater than {MaxNuRegularized} is not supported."
                );
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (nu < MinNu) {
                return IsZero(x) ? 1d : 0d;
            }

            if (x < (double)nu + ULBias) {
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, x);
                ddouble y = 1d - Pow2(nu * Log2(x) - (x + LogGamma(nu)) * LbE) / f;
                y = Max(y, 0d);

                return y;
            }
            else {
                ddouble f = UpperIncompleteGammaCFrac.Value(nu, x);
                ddouble y = Pow2(nu * Log2(x) - (x + LogGamma(nu)) * LbE) / f;
                y = Min(y, 1d);

                return y;
            }
        }

        internal static partial class Consts {
            internal static class IncompleteGamma {
                public static readonly double Eps = double.ScaleB(1, -105);
                public const double MinNu = 1d / 1024;
                public const double MaxNu = Consts.Gamma.ExtremeLarge;
                public const double MaxNuRegularized = 8192d;
                public const double ULBias = 0.125d;
                public const int CFracMaxIter = 8192;
            }
        }

        internal static class UpperIncompleteGammaCFrac {
            public static double Eps = double.ScaleB(1, -105);

            public static ddouble Value(ddouble nu, ddouble x) {
                ddouble xmnu = x - nu;
                ddouble p0 = 0d, p1 = 0d, p2 = nu - 1d, p3 = 0d;
                ddouble q0 = 0d, q1 = 0d, q2 = xmnu + 3d, q3 = 1d;

                ddouble nu2i = nu, xmnu4i = xmnu + 1d;

                bool convergenced = false;
                for (int i = 1; i <= CFracMaxIter; i++) {
                    nu2i -= 2d; xmnu4i += 4d;

                    ddouble a1 = (2 * i) * nu2i, a2 = (2 * i + 1) * (nu2i - 1d);
                    ddouble b1 = xmnu4i, b2 = xmnu4i + 2d;

                    p1 = a1 * p3 + b1 * p2;
                    q1 = a1 * q3 + b1 * q2;
                    p0 = a2 * p2 + b2 * p1;
                    q0 = a2 * q2 + b2 * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (convergenced || (i > 0 && (i & 3) == 0)) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            if (convergenced) {
                                break;
                            }
                            convergenced = true;
                        }
                        else {
                            convergenced = false;
                        }
                    }

                    (p2, p3) = (p0, p1);
                    (q2, q3) = (q0, q1);
                }

#if DEBUG
                Trace.Assert(convergenced, $"[UpperIncompleteGamma nu={nu},x={x}] Continued fraction not convergenced!!");
#endif

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

                bool convergenced = false;
                for (int i = 1; i <= CFracMaxIter; i++) {
                    ix += x; nu2i += 2d;

                    ddouble a1 = ix, a2 = -nux - ix;
                    ddouble b1 = nu2i, b2 = b1 + 1d;

                    p1 = a1 * p3 + b1 * p2;
                    q1 = a1 * q3 + b1 * q2;
                    p0 = a2 * p2 + b2 * p1;
                    q0 = a2 * q2 + b2 * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (convergenced || (i > 0 && (i & 3) == 0)) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            if (convergenced) {
                                break;
                            }
                            convergenced = true;
                        }
                        else {
                            convergenced = false;
                        }
                    }

                    (p2, p3) = (p0, p1);
                    (q2, q3) = (q0, q1);
                }

#if DEBUG
                Trace.Assert(convergenced, $"[LowerIncompleteGamma nu={nu},x={x}] Continued fraction not convergenced!!");
#endif

                ddouble f = nu + p0 / q0;

                return f;
            }
        }
    }
}