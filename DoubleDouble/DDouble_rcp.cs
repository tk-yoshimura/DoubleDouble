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

            int v_exponent = Math.ILogB(v.hi);
            ddouble v_frac = new ddouble(
                Math.ScaleB(v.hi, -v_exponent),
                Math.ScaleB(v.lo, -v_exponent)
            );

            ddouble a = 1 / (double)v_frac;
            ddouble h = 1 - v_frac * a;

            ddouble squa_h = h * h;
            a *= 1 + (1 + squa_h) * (h + squa_h);
            h = 1 - v_frac * a;

            squa_h = h * h;
            a *= 1 + (1 + squa_h) * (h + squa_h);

            a = new ddouble(
                Math.ScaleB(a.hi, -v_exponent),
                Math.ScaleB(a.lo, -v_exponent)
            );

            return a;
        }
    }
}
