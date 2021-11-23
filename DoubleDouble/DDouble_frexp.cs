using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static (int exp, ddouble value) Frexp(ddouble v) {
            if (!IsFinite(v)) {
                return (0, NaN);
            }
            if (IsZero(v)) {
                return (0, Zero);
            }

            int n = Math.ILogB(v.hi);
            ddouble f = new ddouble(Math.ScaleB(v.hi, -n), Math.ScaleB(v.lo, -n));

            return (n, f);
        }
    }
}
