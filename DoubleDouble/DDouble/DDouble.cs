﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {

    [DebuggerDisplay("{ToString(),nq}")]
    public readonly partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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

        public static int Sign(ddouble value) => double.Sign(value.hi);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal double Hi => hi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal double Lo => lo;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble Zero { get; } = new ddouble(0d);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble One { get; } = new ddouble(1d);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MinusOne { get; } = new ddouble(-1d);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble PlusZero => Zero;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MinusZero { get; } = new ddouble(-0d);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble Epsilon { get; } = double.ScaleB(1, NormalMinExponent);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MaxValue { get; } = double.MaxValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MinValue { get; } = double.MinValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble NaN { get; } = double.NaN;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble NegativeInfinity { get; } = double.NegativeInfinity;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble PositiveInfinity { get; } = double.PositiveInfinity;

        public static bool IsNaN(ddouble v) => double.IsNaN(v.hi);

        public static bool IsInfinity(ddouble v) => double.IsInfinity(v.hi);

        public static bool IsPositiveInfinity(ddouble v) => double.IsPositiveInfinity(v.hi);

        public static bool IsNegativeInfinity(ddouble v) => double.IsNegativeInfinity(v.hi);

        public static bool IsNormal(ddouble v) => !IsNaN(v) && !IsInfinity(v) && double.ILogB(v.Hi) >= NormalMinExponent;
        public static bool IsSubnormal(ddouble v) => !IsZero(v) && !IsNaN(v) && !IsInfinity(v) && double.ILogB(v.Hi) < NormalMinExponent;

        public static bool IsFinite(ddouble v) => double.IsFinite(v.hi);

        public static bool IsZero(ddouble v) => v.hi == 0d;

        public static bool IsPlusZero(ddouble v) => IsZero(v) && IsPositive(v);

        public static bool IsMinusZero(ddouble v) => IsZero(v) && IsNegative(v);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const int NormalMinExponent = -968;

        internal static bool IsRegulared(ddouble v) {
            if (v.lo < 0) {
                double vd = double.BitDecrement(v.hi) - v.hi;
                return vd < v.lo;
            }
            if (v.lo > 0) {
                double vi = double.BitIncrement(v.hi) - v.hi;
                return vi > v.lo;
            }
            return true;
        }
    }
}
