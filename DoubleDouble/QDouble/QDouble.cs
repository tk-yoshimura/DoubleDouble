using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    [DebuggerDisplay("{ToString(),nq}")]
    internal partial struct qdouble {
        private readonly ddouble hi, lo;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal qdouble(ddouble hi, ddouble lo) {
            this.hi = hi + lo;
            this.lo = lo - (this.hi - hi);

            if (!ddouble.IsFinite(this.lo)) {
                this.hi = hi;
                this.lo = ddouble.Zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal qdouble(ddouble v) {
            this.hi = v;
            this.lo = ddouble.Zero;
        }

        public static qdouble Zero => PlusZero;
        public static qdouble PlusZero { private set; get; } = ddouble.PlusZero;
        public static qdouble MinusZero { private set; get; } = ddouble.MinusZero;
        public static qdouble Epsilon { private set; get; } = ddouble.Epsilon;
        public static qdouble MaxValue { private set; get; } = ddouble.MaxValue;
        public static qdouble MinValue { private set; get; } = ddouble.MinValue;
        public static qdouble NaN { private set; get; } = ddouble.NaN;
        public static qdouble NegativeInfinity = ddouble.NegativeInfinity;
        public static qdouble PositiveInfinity = ddouble.PositiveInfinity;

        public static bool IsNaN(qdouble v) => ddouble.IsNaN(v.Hi);

        public static bool IsInfinity(qdouble v) => ddouble.IsInfinity(v.Hi);

        public static bool IsPositiveInfinity(qdouble v) => ddouble.IsPositiveInfinity(v.Hi);

        public static bool IsNegativeInfinity(qdouble v) => ddouble.IsNegativeInfinity(v.Hi);

        public static bool IsNormal(qdouble v) => ddouble.IsNormal(v.Hi);

        public static bool IsFinite(qdouble v) => ddouble.IsFinite(v.Hi);

        public static bool IsZero(qdouble v) => ddouble.IsZero(v.Hi);

        public static bool IsPlusZero(qdouble v) => ddouble.IsPlusZero(v.Hi);

        public static bool IsMinusZero(qdouble v) => ddouble.IsMinusZero(v.Hi);

        public int Sign => hi.Sign;

        internal ddouble Hi => hi;
        internal ddouble Lo => lo;
    }
}
