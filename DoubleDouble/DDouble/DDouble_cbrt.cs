using static DoubleDouble.ddouble.Consts.Cbrt;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Cbrt(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Cbrt(-x);
            }
            if (IsZero(x)) {
                return 0d;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int x_exponent, ddouble x_frac) = Frexp(x);
            int exponent_rem = (x_exponent >= 0) ? x_exponent % 3 : ((3 - (-x_exponent) % 3) % 3);

            if (exponent_rem != 0) {
                x_frac = Ldexp(x_frac, exponent_rem);
            }

            ddouble a = 1d / double.Cbrt(x_frac.hi);

            ddouble h = 1d - x_frac * a * a * a;
            a *= 1d + h * (27d + h * (18d + h * 14d)) * Rcp81;

            h = 1d - x_frac * a * a * a;
            a *= 1d + h * (27d + h * (18d + h * 14d)) * Rcp81;

            ddouble y = Ldexp(x_frac * a * a, (x_exponent - exponent_rem) / 3);

            return y;
        }

        internal static partial class Consts {
            public static class Cbrt {
                public static ddouble Rcp81 { get; } = Rcp(81);
            }
        }
    }
}
