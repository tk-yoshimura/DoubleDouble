﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {

    [DebuggerDisplay("{hi}")]
    public partial struct ddouble {
        private readonly double hi, lo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ddouble(double hi, double lo) {
            this.hi = hi + lo;
            this.lo = lo - (this.hi - hi);

            if (!double.IsFinite(this.lo)) {
                this.lo = 0;
            }
        }

        public static ddouble Zero { private set; get; } = new ddouble(0d, 0d);
        public static ddouble Epsilon { private set; get; } = double.Epsilon;
        public static ddouble MaxValue { private set; get; } = double.MaxValue;
        public static ddouble MinValue { private set; get; } = double.MinValue;
        public static ddouble NaN { private set; get; } = double.NaN;
        public static ddouble NegativeInfinity = double.NegativeInfinity;
        public static ddouble PositiveInfinity = double.PositiveInfinity;

        public static bool IsNaN(ddouble v) => double.IsNaN(v.hi);

        public static bool IsInfinity(ddouble v) => double.IsInfinity(v.hi);

        public static bool IsNormal(ddouble v) => double.IsNormal(v.hi);

        public static bool IsFinite(ddouble v) => double.IsFinite(v.hi);

        public static bool IsZero(ddouble v) => v.hi == 0;

        internal double Hi => hi;
        internal double Lo => lo;

        internal static bool IsRegulared(ddouble v) {
            if (v.lo < 0) {
                double vd = Math.BitDecrement(v.hi) - v.hi;
                return vd < v.lo;
            }
            if (v.lo > 0) {
                double vi = Math.BitIncrement(v.hi) - v.hi;
                return vi > v.lo;
            }
            return true;
        }

        public static implicit operator ddouble(double v) {
            return new ddouble(v, Math.CopySign(0d, v));
        }

        public static explicit operator double(ddouble v) {
            return v.hi;
        }
    }
}
