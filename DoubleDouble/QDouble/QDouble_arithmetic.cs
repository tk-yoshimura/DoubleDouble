﻿using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal partial struct qdouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(qdouble v) {
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(qdouble v) {
            return new qdouble(-v.hi, -v.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(qdouble a, ddouble b) {
            if (IsInfinity(a) || ddouble.IsInfinity(b)) {
                return a.hi + b;
            }

            (ddouble hi, ddouble lo) = TwoSum(a.hi, b);
            lo += a.lo;

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(ddouble a, qdouble b) {
            if (ddouble.IsInfinity(a) || IsInfinity(b)) {
                return a + b.hi;
            }

            (ddouble hi, ddouble lo) = TwoSum(a, b.hi);
            lo += b.lo;

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi + b.hi;
            }

            (ddouble hi, ddouble lo) = TwoSum(a.hi, b.hi);
            lo += a.lo + b.lo;

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(qdouble a, ddouble b) {
            if (IsInfinity(a) || ddouble.IsInfinity(b)) {
                return a.hi - b;           
            }            
            (ddouble hi, ddouble lo) = TwoSum(a.hi, -b);
            lo += a.lo;  
                      
            return new qdouble(hi, lo);        
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(ddouble a, qdouble b) {
            if (ddouble.IsInfinity(a) || IsInfinity(b)) {
                return a - b.hi;
            }

            (ddouble hi, ddouble lo) = TwoSum(a, -b.hi);
            lo -= b.lo;

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi - b.hi;
            }

            (ddouble hi, ddouble lo) = TwoSum(a.hi, -b.hi);
            lo += a.lo - b.lo;

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(qdouble a, ddouble b) {
            if (IsInfinity(a) || ddouble.IsInfinity(b) || IsZero(a) || ddouble.IsZero(b)) {
                return a.hi * b;
            }

            qdouble y = MultiplyAdd(Zero, a.hi, b);
            y = MultiplyAdd(y, a.lo, b);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(ddouble a, qdouble b) {
            return b * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
                return a.hi * b.hi;
            }

            qdouble y = MultiplyAdd(Zero, a.hi, b.hi);
            y = MultiplyAdd(y, a.hi, b.lo);
            y = MultiplyAdd(y, a.lo, b.hi);
            y = MultiplyAdd(y, a.lo, b.lo);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator /(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
                return a.hi / b.hi;
            }

            ddouble hi = a.hi / b.hi;
            qdouble hirem = a - hi * b;

            ddouble lo = hirem.hi / b.hi;
            qdouble lorem = hirem - lo * b;

            ddouble c = lorem.hi / b.hi;

            if (ddouble.IsInfinity(hi)) {
                return hi;
            }

            return new qdouble(hi, lo) + c;
        }

        public static qdouble operator %(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(b)) {
                return a.hi % b.hi;
            }

            qdouble abs_a = Abs(a), abs_b = Abs(b);

            qdouble abs_y = abs_a - qdouble.Truncate(abs_a / abs_b) * abs_b;
            if (abs_y.Sign < 0 || abs_y >= abs_b) {
                abs_y = 0;
            }

            qdouble y = a.Sign >= 0 ? abs_y : -abs_y;

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static qdouble MultiplyAdd(qdouble v, ddouble a, ddouble b) {
            ddouble a_hi = (ddouble)a.Hi, a_lo = (ddouble)a.Lo;
            ddouble b_hi = (ddouble)b.Hi, b_lo = (ddouble)b.Lo;
            
            v += ddouble.Multiply(a.Hi, b.Hi);
            v += ddouble.Multiply(a.Hi, b.Lo);
            v += ddouble.Multiply(a.Lo, b.Hi);
            v += ddouble.Multiply(a.Lo, b.Lo);

			return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static qdouble Multiply(ddouble a, ddouble b) {
            ddouble hi = a * b;
            ddouble lo = ddouble.MultiplyAdd(-hi, a, b);

            return new ddouble(hi, lo);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (ddouble s, ddouble c) TwoSum(ddouble a, ddouble b) {
            ddouble s = a + b;
            ddouble t = s - a;
            ddouble c = (a - (s - t)) + (b - t);

            return (s, c);
        }
    }
}