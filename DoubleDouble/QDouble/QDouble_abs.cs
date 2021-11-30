using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal partial struct qdouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble Abs(qdouble x) {
            return new qdouble(ddouble.Abs(x.hi), x.hi >= ddouble.Zero ? x.lo : -x.lo);
        }
    }
}
