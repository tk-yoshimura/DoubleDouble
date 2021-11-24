using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Ldexp(ddouble x, int n) {
            return new ddouble(Math.ScaleB(x.hi, n), Math.ScaleB(x.lo, n));
        }
    }
}
