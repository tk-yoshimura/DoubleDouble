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

            (double hi, double lo) = TwoSum(a.hi, b);
            lo += a.lo;

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(ddouble a, int b) {
            return a + (double)b;
        }

        public static ddouble operator +(ddouble a, uint b) {
            return a + (double)b;
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

            (double hi, double lo) = TwoSum(a, b.hi);
            lo += b.lo;

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(int a, ddouble b) {
            return (double)a + b;
        }

        public static ddouble operator +(uint a, ddouble b) {
            return (double)a + b;
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

            (double hi, double lo) = TwoSum(a.hi, b.hi);
            lo += a.lo + b.lo;

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, double b) {
            if (IsInfinity(a) || double.IsInfinity(b)) {
                return a.hi - b;
            }

            (double hi, double lo) = TwoSum(a.hi, -b);
            lo += a.lo;

            return new ddouble(hi, lo);
        }

        public static ddouble operator -(ddouble a, int b) {
            return a - (double)b;
        }

        public static ddouble operator -(ddouble a, uint b) {
            return a - (double)b;
        }

        public static ddouble operator -(ddouble a, long b) {
            return a - (ddouble)b;
        }

        public static ddouble operator -(ddouble a, ulong b) {
            return a - (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(double a, ddouble b) {
            if (double.IsInfinity(a) || IsInfinity(b)) {
                return a - b.hi;
            }

            (double hi, double lo) = TwoSum(a, -b.hi);
            lo -= b.lo;

            return new ddouble(hi, lo);
        }

        public static ddouble operator -(int a, ddouble b) {
            return (double)a - b;
        }

        public static ddouble operator -(uint a, ddouble b) {
            return (double)a - b;
        }

        public static ddouble operator -(long a, ddouble b) {
            return (ddouble)a - b;
        }

        public static ddouble operator -(ulong a, ddouble b) {
            return (ddouble)a - b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator -(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b)) {
                return a.hi - b.hi;
            }

            (double hi, double lo) = TwoSum(a.hi, -b.hi);
            lo += a.lo - b.lo;

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, double b) {
            if (IsInfinity(a) || double.IsInfinity(b) || IsZero(a) || b == 0d) {
                return a.hi * b;
            }

            ddouble y = MultiplyAdd(0d, a.hi, b);
            y = MultiplyAdd(y, a.lo, b);

            return y;
        }

        public static ddouble operator *(ddouble a, int b) {
            uint abs_b = UIntUtil.Abs(b);
            if (UIntUtil.IsPower2(abs_b)) {
                int exp = UIntUtil.Power2(abs_b);
                return (b >= 0) ? Ldexp(a, exp) : -Ldexp(a, exp);
            }

            return a * (double)b;
        }

        public static ddouble operator *(ddouble a, uint b) {
            if (UIntUtil.IsPower2(b)) {
                int exp = UIntUtil.Power2(b);
                return Ldexp(a, exp);
            }

            return a * (double)b;
        }

        public static ddouble operator *(ddouble a, long b) {
            ulong abs_b = UIntUtil.Abs(b);
            if (UIntUtil.IsPower2(abs_b)) {
                int exp = UIntUtil.Power2(abs_b);
                return (b >= 0) ? Ldexp(a, exp) : -Ldexp(a, exp);
            }

            return a * (ddouble)b;
        }

        public static ddouble operator *(ddouble a, ulong b) {
            if (UIntUtil.IsPower2(b)) {
                int exp = UIntUtil.Power2(b);
                return Ldexp(a, exp);
            }

            return a * (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(double a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(int a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(uint a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(long a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(ulong a, ddouble b) {
            return b * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator *(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
                return a.hi * b.hi;
            }

            ddouble y = Multiply(a.hi, b.hi);
            y = MultiplyAdd(y, a.hi, b.lo);
            y = MultiplyAdd(y, a.lo, b.hi);
            y = MultiplyAdd(y, a.lo, b.lo);

            return y;
        }

        public static ddouble operator /(ddouble a, double b) {
            if (IsInfinity(a) || double.IsInfinity(b) || IsZero(a) || b == 0d) {
                return a.hi / b;
            }

            double hi = a.hi / b;
            ddouble hirem = a - Multiply(hi, b);

            double lo = hirem.hi / b;
            ddouble lorem = hirem - Multiply(lo, b);

            double c = lorem.hi / b;

            if (double.IsInfinity(hi)) {
                return hi;
            }

            return new ddouble(hi, lo) + c;
        }

        public static ddouble operator /(ddouble a, int b) {
            uint abs_b = UIntUtil.Abs(b);
            if (UIntUtil.IsPower2(abs_b)) {
                int exp = UIntUtil.Power2(abs_b);
                return (b >= 0) ? Ldexp(a, -exp) : -Ldexp(a, -exp);
            }

            return a / (double)b;
        }

        public static ddouble operator /(ddouble a, uint b) {
            if (UIntUtil.IsPower2(b)) {
                int exp = UIntUtil.Power2(b);
                return Ldexp(a, -exp);
            }

            return a / (double)b;
        }

        public static ddouble operator /(ddouble a, long b) {
            ulong abs_b = UIntUtil.Abs(b);
            if (UIntUtil.IsPower2(abs_b)) {
                int exp = UIntUtil.Power2(abs_b);
                return (b >= 0) ? Ldexp(a, -exp) : -Ldexp(a, -exp);
            }

            return a / (ddouble)b;
        }

        public static ddouble operator /(ddouble a, ulong b) {
            if (UIntUtil.IsPower2(b)) {
                int exp = UIntUtil.Power2(b);
                return Ldexp(a, -exp);
            }

            return a / (ddouble)b;
        }

        public static ddouble operator /(double a, ddouble b) {
            if (double.IsInfinity(a) || IsInfinity(b) || a == 0d || IsZero(b)) {
                return a / b.hi;
            }

            double hi = a / b.hi;
            ddouble hirem = a - hi * b;

            double lo = hirem.hi / b.hi;
            ddouble lorem = hirem - lo * b;

            double c = lorem.hi / b.hi;

            if (double.IsInfinity(hi)) {
                return hi;
            }

            return new ddouble(hi, lo) + c;
        }

        public static ddouble operator /(int a, ddouble b) {
            return (double)a / b;
        }

        public static ddouble operator /(uint a, ddouble b) {
            return (double)a / b;
        }

        public static ddouble operator /(long a, ddouble b) {
            return (ddouble)a / b;
        }

        public static ddouble operator /(ulong a, ddouble b) {
            return (ddouble)a / b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator /(ddouble a, ddouble b) {
            if (IsInfinity(a) || IsInfinity(b) || IsZero(a) || IsZero(b)) {
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
            if (IsInfinity(a) || IsInfinity(b) || IsZero(b)) {
                return a.hi % b.hi;
            }

            ddouble abs_a = Abs(a), abs_b = Abs(b);

            ddouble abs_y = abs_a - Truncate(abs_a / abs_b) * abs_b;
            if (IsNegative(abs_y) || abs_y >= abs_b) {
                abs_y = 0d;
            }

            ddouble y = IsPositive(a) ? abs_y : -abs_y;

            return y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ddouble MultiplyAdd(ddouble v, double a, double b) {
            double hi = double.FusedMultiplyAdd(a, b, v.hi);
            double lo = v.lo + double.FusedMultiplyAdd(a, b, v.hi - hi);

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ddouble Multiply(double a, double b) {
            double hi = a * b;
            double lo = double.FusedMultiplyAdd(a, b, -hi);

            return new ddouble(hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double s, double c) TwoSum(double a, double b) {
            double s = a + b;
            double t = s - a;
            double c = (a - (s - t)) + (b - t);

            return (s, c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator ++(ddouble v) {
            return v + 1d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator --(ddouble v) {
            return v - 1d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator checked ++(ddouble v) {
            if (double.ILogB(v.hi) > 102) {
                throw new ArithmeticException();
            }

            return v + 1d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble operator checked --(ddouble v) {
            if (double.ILogB(v.hi) > 102) {
                throw new ArithmeticException();
            }

            return v - 1d;
        }
    }
}
