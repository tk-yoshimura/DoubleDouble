﻿using System;
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
            if (IsInfinity(a) || double.IsInfinity(b)) {
                return a.hi + b;
            }

            double hi = a.hi + b;
            double lo = a.lo + (b - (hi - a.hi));

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(ddouble a, int b) {
            return a + (ddouble)b;
        }

        public static ddouble operator +(ddouble a, uint b) {
            return a + (ddouble)b;
        }

        public static ddouble operator +(ddouble a, long b) {
            return a + (ddouble)b;
        }

        public static ddouble operator +(ddouble a, ulong b) {
            return a + (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(double a, ddouble b) {
            if (double.IsInfinity(a) || IsInfinity(b)) {
                return a + b.hi;
            }

            double hi = a + b.hi;
            double lo = b.lo + (b.hi - (hi - a));

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(int a, ddouble b) {
            return (ddouble)a + b;
        }

        public static ddouble operator +(uint a, ddouble b) {
            return (ddouble)a + b;
        }

        public static ddouble operator +(long a, ddouble b) {
            return (ddouble)a + b;
        }

        public static ddouble operator +(ulong a, ddouble b) {
            return (ddouble)a + b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator +(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi + b.hi;
            }

            double hi = a.hi + b.hi;
            double lo = a.lo + (b.lo + (b.hi - (hi - a.hi)));

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, double b) {
            return a + (-b);
        }

        public static ddouble operator -(ddouble a, int b) {
            return a - (ddouble)b;
        }

        public static ddouble operator -(ddouble a, uint b) {
            return a - (ddouble)b;
        }

        public static ddouble operator -(ddouble a, long b) {
            return a - (ddouble)b;
        }

        public static ddouble operator -(ddouble a, ulong b) {
            return a - (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(double a, ddouble b) {
            return a + (-b);
        }

        public static ddouble operator -(int a, ddouble b) {
            return (ddouble)a - b;
        }

        public static ddouble operator -(uint a, ddouble b) {
            return (ddouble)a - b;
        }

        public static ddouble operator -(long a, ddouble b) {
            return (ddouble)a - b;
        }

        public static ddouble operator -(ulong a, ddouble b) {
            return (ddouble)a - b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, ddouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, double b) {
            if (IsInfinity(a) || double.IsInfinity(b)) {
                return a.hi * b;
            }

            ddouble y = MultiplyAdd(Zero, a.hi, b);
            y = MultiplyAdd(y, a.lo, b);

            return y;
        }

        public static ddouble operator *(ddouble a, int b) {
            return a * (ddouble)b;
        }

        public static ddouble operator *(ddouble a, uint b) {
            return a * (ddouble)b;
        }

        public static ddouble operator *(ddouble a, long b) {
            return a * (ddouble)b;
        }

        public static ddouble operator *(ddouble a, ulong b) {
            return a * (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(double a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(int a, ddouble b) {
            return (ddouble)a * b;
        }

        public static ddouble operator *(uint a, ddouble b) {
            return (ddouble)a * b;
        }

        public static ddouble operator *(long a, ddouble b) {
            return (ddouble)a * b;
        }

        public static ddouble operator *(ulong a, ddouble b) {
            return (ddouble)a * b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi * b.hi;
            }

            ddouble y = MultiplyAdd(Zero, a.hi, b.hi);
            y = MultiplyAdd(y, a.hi, b.lo);
            y = MultiplyAdd(y, a.lo, b.hi);
            y = MultiplyAdd(y, a.lo, b.lo);

            return y;
        }

        public static ddouble operator /(ddouble a, double b) {
            return a / (ddouble)b;
        }

        public static ddouble operator /(ddouble a, int b) {
            return a / (ddouble)b;
        }

        public static ddouble operator /(ddouble a, uint b) {
            return a / (ddouble)b;
        }

        public static ddouble operator /(ddouble a, long b) {
            return a / (ddouble)b;
        }

        public static ddouble operator /(ddouble a, ulong b) {
            return a / (ddouble)b;
        }

        public static ddouble operator /(double a, ddouble b) {
            return (ddouble)a / b;
        }

        public static ddouble operator /(int a, ddouble b) {
            return (ddouble)a / b;
        }

        public static ddouble operator /(uint a, ddouble b) {
            return (ddouble)a / b;
        }

        public static ddouble operator /(long a, ddouble b) {
            return (ddouble)a / b;
        }

        public static ddouble operator /(ulong a, ddouble b) {
            return (ddouble)a / b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator /(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(b)) {
                return a.hi / b.hi;
            }

            double hi = a.hi / b.hi;
            ddouble hirem = a - hi * b;

            double lo = hirem.hi / b.hi;
            ddouble lorem = hirem - lo * b;

            double c = lorem.hi / b.hi;

            if (double.IsInfinity(hi)) {
                return hi;
            }

            return new ddouble(hi, lo) + c;
        }

        public static ddouble operator %(ddouble a, double b) {
            return a % (ddouble)b;
        }

        public static ddouble operator %(ddouble a, int b) {
            return a % (ddouble)b;
        }

        public static ddouble operator %(ddouble a, uint b) {
            return a % (ddouble)b;
        }

        public static ddouble operator %(ddouble a, long b) {
            return a % (ddouble)b;
        }

        public static ddouble operator %(ddouble a, ulong b) {
            return a % (ddouble)b;
        }

        public static ddouble operator %(double a, ddouble b) {
            return (ddouble)a % b;
        }

        public static ddouble operator %(int a, ddouble b) {
            return (ddouble)a % b;
        }

        public static ddouble operator %(uint a, ddouble b) {
            return (ddouble)a % b;
        }

        public static ddouble operator %(long a, ddouble b) {
            return (ddouble)a % b;
        }

        public static ddouble operator %(ulong a, ddouble b) {
            return (ddouble)a % b;
        }

        public static ddouble operator %(ddouble a, ddouble b) {
            ddouble y = a - ddouble.Truncate(a / b) * b;
            if (y.Sign != a.Sign) {
                y += a.Sign * Abs(b);
                if (y == b) {
                    return 0;
                }
            }

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ddouble MultiplyAdd(ddouble v, double a, double b) {
            double hi = Math.FusedMultiplyAdd(a, b, v.hi);
            double lo = v.lo + Math.FusedMultiplyAdd(a, b, v.hi - hi);

            return new ddouble(hi, lo);
        }
    }
}
