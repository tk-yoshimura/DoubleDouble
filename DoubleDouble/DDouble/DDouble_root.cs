﻿namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble RootN(ddouble x, int n) {
            if (n == 0) {
                return NaN;
            }

            int n_abs = int.Abs(n);

            ddouble y = n_abs switch {
                1 => x,
                2 => Sqrt(x),
                3 => Cbrt(x),
                4 => Sqrt(Sqrt(x)),
                6 => Sqrt(Cbrt(x)),
                8 => Sqrt(Sqrt(Sqrt(x))),
                9 => Cbrt(Cbrt(x)),
                <= 256 => ((n_abs & 1) == 0) ? RootNUtil.Value(x, n) : CopySign(RootNUtil.Value(Abs(x), n), x),
                _ => ((n_abs & 1) == 0) ? Pow(x, Rcp(n)) : CopySign(Pow(Abs(x), Rcp(n)), x)
            };

            y = (n > 0) ? y : Rcp(y);

            return y;
        }

        public static ddouble Hypot(ddouble x, ddouble y) {
            if (IsInfinity(x) || IsInfinity(y)) {
                return PositiveInfinity;
            }

            (int scale, (x, y)) = AdjustScale(0, (x, y));

            ddouble z = Ldexp(Sqrt(x * x + y * y), -scale);

            return z;
        }

        public static ddouble Hypot(ddouble x, ddouble y, ddouble z) {
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return PositiveInfinity;
            }

            (int scale, (x, y, z)) = AdjustScale(0, (x, y, z));

            ddouble w = Ldexp(Sqrt(x * x + y * y + z * z), -scale);

            return w;
        }

        internal static class RootNUtil {
            public static ddouble Value(ddouble x, int n) {
#if DEBUG
                if (n <= 4 || n > 256) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }
#endif

                if (IsNegative(x) || IsNaN(x)) {
                    return NaN;
                }
                if (ddouble.IsZero(x)) {
                    return 0d;
                }
                if (IsInfinity(x)) {
                    return PositiveInfinity;
                }

                int exp = double.ILogB(x.Hi);
                int exp_n = (exp >= 0) ? (exp % n) : ((n - (-exp) % n) % n);
                int exp_scale = exp - exp_n;

#if DEBUG
                Debug<ArgumentException>.Assert(exp_n >= 0 && exp_n < n);
                Debug<ArgumentException>.Assert(exp_scale % n == 0);
#endif

                x = Ldexp(x, -exp_scale);

                ddouble y = double.RootN(x.Hi, n);
                ddouble yn = Pow(y, n), delta = yn - x;

                ddouble dy = 3 * delta * y * ((n - 1) * delta - 2 * n * yn)
                    / ((n - 1) * (n - 2) * delta * delta + yn * (-6 * n * (n - 1) * delta + yn * (6 * n * n)));

                y += dy;

                y = Ldexp(y, exp_scale / n);

                return y;

            }
        }
    }
}