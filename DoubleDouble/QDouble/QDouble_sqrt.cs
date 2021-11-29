using System;

namespace DoubleDouble {
    internal partial struct qdouble {
        public static qdouble Sqrt(qdouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return Zero;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int x_exponent, qdouble x_frac) = Frexp(x);
            int exponent_rem = Math.Abs(x_exponent) % 2;

            if (exponent_rem != 0) {
                x_frac = Ldexp(x_frac, exponent_rem);
            }

            qdouble a = 1 / ddouble.Sqrt(x_frac.hi);

            qdouble h = 1 - x_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            h = 1 - x_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            qdouble y = Ldexp(x_frac * a, (x_exponent - exponent_rem) / 2);

            return y;
        }
    }
}
