using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Abs(ddouble x) {
            return new ddouble(double.Abs(x.hi), x.hi >= 0d ? x.lo : -x.lo);
        }
    }
}
