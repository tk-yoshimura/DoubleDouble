using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {

    [DebuggerDisplay("{ToString(),nq}")]
    public partial struct ddouble {
        private readonly double hi, lo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ddouble(double hi, double lo) {
            this.hi = hi + lo;
            this.lo = lo - (this.hi - hi);

            if (!double.IsFinite(this.lo)) {
                this.hi = hi;
                this.lo = 0d;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ddouble(double v) {
            this.hi = v;
            this.lo = 0d;
        }

        public static ddouble Zero { private set; get; } = new ddouble(0d);
        public static ddouble PlusZero => Zero;
        public static ddouble MinusZero { private set; get; } = new ddouble(-0d);
        public static ddouble Epsilon { private set; get; } = Math.ScaleB(1, -968);
        public static ddouble MaxValue { private set; get; } = double.MaxValue;
        public static ddouble MinValue { private set; get; } = double.MinValue;
        public static ddouble NaN { private set; get; } = double.NaN;
        public static ddouble NegativeInfinity = double.NegativeInfinity;
        public static ddouble PositiveInfinity = double.PositiveInfinity;

        public static bool IsNaN(ddouble v) => double.IsNaN(v.hi);

        public static bool IsInfinity(ddouble v) => double.IsInfinity(v.hi);

        public static bool IsPositiveInfinity(ddouble v) => double.IsPositiveInfinity(v.hi);

        public static bool IsNegativeInfinity(ddouble v) => double.IsNegativeInfinity(v.hi);

        public static bool IsNormal(ddouble v) => !IsNaN(v) && !IsInfinity(v) && (v < -Epsilon || v > Epsilon);

        public static bool IsFinite(ddouble v) => double.IsFinite(v.hi);

        public static bool IsZero(ddouble v) => v.hi == 0d;

        public static bool IsPlusZero(ddouble v) => IsZero(v) && v.Sign > 0;

        public static bool IsMinusZero(ddouble v) => IsZero(v) && v.Sign < 0;

        public int Sign => (int)Math.CopySign(1, hi);

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
    }
}
