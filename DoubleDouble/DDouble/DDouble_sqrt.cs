using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sqrt(ddouble v) {
            if (v.Sign < 0 || IsNaN(v)) {
                return NaN;
            }
            if (v.hi == 0) {
                return 0;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            (int v_exponent, ddouble v_frac) = Frexp(v);

            if ((v_exponent & 1) == 1) {
                v_frac = Ldexp(v_frac, 1);
            }

            ddouble a = 1 / Math.Sqrt(v_frac.hi);
            
            ddouble h = 1 - v_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            h = 1 - v_frac * a * a;
            a *= 1 + Ldexp(h * (4 + h * 3), -3);

            a = Ldexp(v_frac * a, (v_exponent - (Math.Abs(v_exponent) % 2)) / 2);

            return a;
        }
    }
}
