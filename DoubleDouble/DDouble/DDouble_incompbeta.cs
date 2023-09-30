using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble IncompleteBeta(ddouble x, ddouble a, ddouble b) {
            if (x < 0d || !(x <= 1d) || !(a > 0d) || !(b > 0d)) {
                return NaN;
            }

            if (a + b - Max(a, b) > Consts.IncompleteBeta.MaxAB) {
                throw new ArgumentOutOfRangeException(
                    $"In the calculation of the IncompleteBeta function, " +
                    $"{nameof(a)}+{nameof(b)}-max({nameof(a)},{nameof(b)}) greater than " +
                    $"{Consts.IncompleteBeta.MaxAB} is not supported."
                );
            }

            if (IsZero(x)) {
                return Zero;
            }
            if (x == 1d) {
                return Beta(a, b);
            }

            double thr = (a.hi + 1d) / (a.hi + b.hi + 1d);

            if (x < thr) {
                ddouble f = IncompleteBetaCFrac.Value(x, a, b);

                ddouble y = Exp(a * Log(x) + b * Log(1d - x)) / f;

                return y;
            }
            else {
                ddouble f = IncompleteBetaCFrac.Value(1d - x, b, a);

                ddouble y = Beta(a, b) - Exp(a * Log(x) + b * Log(1d - x)) / f;
                y = Max(y, Zero);

                return y;
            }
        }

        public static ddouble IncompleteBetaRegularized(ddouble x, ddouble a, ddouble b) {
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

            double thr = (a.hi + 1d) / (a.hi + b.hi + 1d);

            if (x < thr) {
                ddouble f = IncompleteBetaCFrac.Value(x, a, b);

                ddouble y = Exp(a * Log(x) + b * Log(1d - x) - LogBeta(a, b)) / f;
                y = Min(y, One);

                return y;
            }
            else {
                ddouble f = IncompleteBetaCFrac.Value(1d - x, b, a);

                ddouble y = 1d - Exp(a * Log(x) + b * Log(1d - x) - LogBeta(a, b)) / f;
                y = Max(y, Zero);

                return y;
            }
        }

        internal static partial class Consts {
            internal static class IncompleteBeta {
                public const double MaxAB = 512d;
                public const double MaxABRegularized = 8192d;
                public const int CFracMaxIter = 8192;
            }
        }

        internal static class IncompleteBetaCFrac {
            public static ddouble Value(ddouble x, ddouble a, ddouble b) {
                ddouble ab = a + b;
                ddouble p0 = 0d, p1 = 0d, p2 = -(ab * x) / (a + 1d), p3 = 0d;
                ddouble q0 = 0d, q1 = 0d, q2 = 1d, q3 = 1d;

                ddouble a2i = a, ai = a, bi = b, abi = ab;

                bool convergenced = false;
                for (int i = 1; i < Consts.IncompleteBeta.CFracMaxIter; i++) {
                    a2i += 2d; ai += 1d; bi -= 1d; abi += 1d;

                    ddouble a1 = (bi * i * x) / ((a2i - 1d) * a2i);
                    ddouble a2 = (ai * abi * x) / ((a2i + 1d) * a2i);

                    p1 = p2 + a1 * p3;
                    q1 = q2 + a1 * q3;
                    p0 = p1 - a2 * p2;
                    q0 = q1 - a2 * q2;

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
                Trace.Assert(convergenced, $"[IncompleteBeta x={x},a={a},b={b}] Continued fraction not convergenced!!");
#endif

                ddouble f = a * (1d + p0 / q0);

                return f;
            }
        }
    }
}