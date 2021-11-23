using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(ddouble v) {
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble v) {
            return new ddouble(-v.hi, -v.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(ddouble a, double b) {
            double hi = a.hi + b;
            double lo = a.lo + (b - (hi - a.hi));

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(double a, ddouble b) {
            double hi = a + b.hi;
            double lo = b.lo + (b.hi - (hi - a));

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(ddouble a, ddouble b) {
            double hi = a.hi + b.hi;
            double lo = a.lo + (b.lo + (b.hi - (hi - a.hi)));

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, double b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(double a, ddouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, ddouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, double b) {
            ddouble y = MultiplyAdd(Zero, a.hi, b);
            y = MultiplyAdd(y, a.lo, b);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(double a, ddouble b) {
            return b * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, ddouble b) {
            ddouble y = MultiplyAdd(Zero, a.hi, b.hi);
            y = MultiplyAdd(y, a.hi, b.lo);
            y = MultiplyAdd(y, a.lo, b.hi);
            y = MultiplyAdd(y, a.lo, b.lo);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator /(ddouble a, double b) {
            return a * Rcp(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator /(double a, ddouble b) {
            return a * Rcp(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator /(ddouble a, ddouble b) {
            return a * Rcp(b);
        }
               
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ddouble MultiplyAdd(ddouble v, double a, double b) {
            double hi = Math.FusedMultiplyAdd(a, b, v.hi);
            double lo = v.lo + Math.FusedMultiplyAdd(a, b, v.hi - hi);

            return new ddouble(hi, lo);
        }
    }
}
