using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sqrt(ddouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return Zero;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int x_exponent, ddouble x_frac) = Frexp(x);
            int exponent_rem = Math.Abs(x_exponent) % 2;

            if (exponent_rem != 0) {
                x_frac = Ldexp(x_frac, exponent_rem);
            }

            ddouble a = 1d / Math.Sqrt(x_frac.hi);

            ddouble h = 1d - x_frac * a * a;
            a *= 1d + Ldexp(h * (4d + h * 3d), -3);

            h = 1 - x_frac * a * a;
            a *= 1d + Ldexp(h * (4d + h * 3d), -3);

            ddouble y = Ldexp(x_frac * a, (x_exponent - exponent_rem) / 2);

            return y;
        }
    }
}
