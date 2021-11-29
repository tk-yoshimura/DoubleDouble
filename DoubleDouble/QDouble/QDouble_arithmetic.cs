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

            ddouble hh = (ddouble)a.hi * (ddouble)b;
            ddouble lh = (ddouble)a.lo * (ddouble)b;

            return new qdouble(hh, lh);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(ddouble a, qdouble b) {
            if (ddouble.IsInfinity(a) || IsInfinity(b) || ddouble.IsZero(a) || IsZero(b)) {
                return a * b.hi;
            }

            ddouble hh = (ddouble)a * (ddouble)b.hi;
            ddouble hl = (ddouble)a * (ddouble)b.lo;

            return new qdouble(hh, hl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble operator *(qdouble a, qdouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
                return a.hi * b.hi;
            }

            ddouble hh = (ddouble)a.hi * (ddouble)b.hi;
            ddouble hl = (ddouble)a.hi * (ddouble)b.lo;
            ddouble lh = (ddouble)a.lo * (ddouble)b.hi;
            ddouble ll = (ddouble)a.lo * (ddouble)b.lo;

            return (new qdouble(hh, hl) + lh) + ll;
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
    }
}
