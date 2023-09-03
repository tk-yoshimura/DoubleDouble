using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Ldexp(ddouble x, int n) {
            if (IsZero(x) || !IsFinite(x)) {
                return x;
            }

            return new ddouble(double.ScaleB(x.hi, n), double.ScaleB(x.lo, n));
        }
    }
}
