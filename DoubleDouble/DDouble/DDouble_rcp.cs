using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Rcp(ddouble v) {
            if (v.hi == 0) {
                return Math.CopySign(double.PositiveInfinity, v.hi);
            }
            if (IsInfinity(v)) {
                return v.hi > 0d ? 0d : -0d;
            }
            if (IsNaN(v.hi)) {
                return NaN;
            }

            (int v_exponent, ddouble v_frac) = Frexp(v);

            ddouble a = 1 / v_frac.hi;
            ddouble h = 1 - v_frac * a;

            ddouble squa_h = h * h;
            a *= 1 + (1 + squa_h) * (h + squa_h);

            ddouble y = Ldexp(a, -v_exponent);

            return y;
        }
    }
}
