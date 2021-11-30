using System;
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

            ddouble hi = a.hi + b;
            ddouble lo = a.lo + (b - (hi - a.hi));

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(ddouble a, qdouble b) {
            if (ddouble.IsInfinity(a) || IsInfinity(b)) {
                return a + b.hi;
            }

            ddouble hi = a + b.hi;
            ddouble lo = b.lo + (b.hi - (hi - a));

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator +(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi + b.hi;
            }

            ddouble hi = a.hi + b.hi;
            ddouble lo = a.lo + (b.lo + (b.hi - (hi - a.hi)));

            return new qdouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(qdouble a, ddouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(ddouble a, qdouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator -(qdouble a, qdouble b) {
            return a + (-b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(qdouble a, ddouble b) {
            if (IsInfinity(a) || ddouble.IsInfinity(b) || IsZero(a) || ddouble.IsZero(b)) {
                return a.hi * b;
            }

            qdouble y = MultiplyAdd(Zero, a.hi.Hi, b.Hi);
            y = MultiplyAdd(y, a.hi.Hi, b.Lo);
            y = MultiplyAdd(y, a.hi.Lo, b.Hi);
            y = MultiplyAdd(y, a.hi.Lo, b.Lo);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(ddouble a, qdouble b) {
            if (ddouble.IsInfinity(a) || IsInfinity(b) || ddouble.IsZero(a) || IsZero(b)) {
                return a * b.hi;
            }

            qdouble y = MultiplyAdd(Zero, a.Hi, b.hi.Hi);
            y = MultiplyAdd(y, a.Hi, b.hi.Lo);
            y = MultiplyAdd(y, a.Lo, b.hi.Hi);
            y = MultiplyAdd(y, a.Lo, b.hi.Lo);

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
                return a.hi * b.hi;
            }

            qdouble y = MultiplyAdd(Zero, a.hi.Hi, b.hi.Hi);
            y = MultiplyAdd(y, a.hi.Hi, b.hi.Lo);
            y = MultiplyAdd(y, a.hi.Lo, b.hi.Hi);
            y = MultiplyAdd(y, a.hi.Lo, b.hi.Lo);
            y = MultiplyAdd(y, a.lo.Hi, b.lo.Hi);
            y = MultiplyAdd(y, a.lo.Hi, b.lo.Lo);
            y = MultiplyAdd(y, a.lo.Lo, b.lo.Hi);
            y = MultiplyAdd(y, a.lo.Lo, b.lo.Lo);

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
        private static qdouble MultiplyAdd(qdouble v, double a, double b) {
            return v + (ddouble)a * b;
        }
    }
}
