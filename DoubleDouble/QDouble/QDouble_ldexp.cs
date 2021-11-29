using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal partial struct qdouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble Ldexp(qdouble x, int n) {
            if (IsZero(x) || !IsFinite(x)) {
                return x;
            }

            return new qdouble(ddouble.Ldexp(x.Hi, n), ddouble.Ldexp(x.Lo, n));
        }
    }
}
