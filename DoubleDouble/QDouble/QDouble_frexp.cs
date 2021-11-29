using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal partial struct qdouble {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int exp, qdouble value) Frexp(qdouble x) {
            if (!IsFinite(x)) {
                return (0, NaN);
            }
            if (IsZero(x)) {
                return (0, Zero);
            }

            (int n, ddouble f) = ddouble.Frexp(x.hi);
            qdouble y = new qdouble(ddouble.Ldexp(x.hi, -n), ddouble.Ldexp(x.lo, -n));

            if (y.hi == 1 && y.lo < 0) {
                n -= 1;
                y = new qdouble(2, y.lo * 2);
            }

            return (n, y);
        }
    }
}
