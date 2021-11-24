using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Abs(ddouble v) {
            return new ddouble(Math.Abs(v.hi), v.lo);
        }
    }
}
