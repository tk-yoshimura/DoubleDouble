using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sqrt(ddouble v) {
            if (v.Sign < 0 || IsNaN(v)) {
                return NaN;
            }
            if (IsZero(v)) {
                return 0;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            (int v_exponent, ddouble v_frac) = Frexp(v);
            int exponent_rem = Math.Abs(v_exponent) % 2;

            if (exponent_rem != 0) {
                v_frac = Ldexp(v_frac, exponent_rem);
            }

            ddouble a = 1 / Math.Sqrt(v_frac.hi);

            ddouble h = 1 - v_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            h = 1 - v_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            ddouble y = Ldexp(v_frac * a, (v_exponent - exponent_rem) / 2);

            return y;
        }
    }
}
