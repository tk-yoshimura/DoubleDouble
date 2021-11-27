using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int exp, ddouble value) Frexp(ddouble x) {
            if (!IsFinite(x)) {
                return (0, NaN);
            }
            if (IsZero(x)) {
                return (0, Zero);
            }

            int n = Math.ILogB(x.hi);
            ddouble f = new ddouble(Math.ScaleB(x.hi, -n), Math.ScaleB(x.lo, -n));

            if (f.hi == 1 && f.lo < 0) {
                n -= 1;
                f = new ddouble(2, f.lo * 2);
            }

            return (n, f);
        }
    }
}
