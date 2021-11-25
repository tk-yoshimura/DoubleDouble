using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int exp, ddouble value) Frexp(ddouble v) {
            if (!IsFinite(v)) {
                return (0, NaN);
            }
            if (IsZero(v)) {
                return (0, Zero);
            }

            int n = Math.ILogB(v.hi);
            ddouble f = new ddouble(Math.ScaleB(v.hi, -n), Math.ScaleB(v.lo, -n));

            if (f.hi == 1 && f.lo < 0) {
                n -= 1;
                f = new ddouble(2, f.lo * 2);
            }

            return (n, f);
        }
    }
}
