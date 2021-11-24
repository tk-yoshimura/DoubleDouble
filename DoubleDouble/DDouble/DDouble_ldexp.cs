﻿using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Ldexp(ddouble x, int n) {
            return new ddouble(Math.ScaleB(x.hi, n), Math.ScaleB(x.lo, n));
        }
    }
}
