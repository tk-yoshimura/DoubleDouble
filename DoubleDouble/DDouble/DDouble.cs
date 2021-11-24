using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {

    [DebuggerDisplay("{ToString()}")]
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

        public static implicit operator ddouble(double v) {
            return new ddouble(v, Math.CopySign(0d, v));
        }

        public static explicit operator double(ddouble v) {
            return v.hi;
        }

        public static implicit operator ddouble(int n) {
            return (BigInteger)n;
        }

        public static implicit operator ddouble(uint n) {
            return (BigInteger)n;
        }

        public static implicit operator ddouble(long n) {
            return (BigInteger)n;
        }

        public static implicit operator ddouble(ulong n) {
            return (BigInteger)n;
        }

        public static implicit operator ddouble(BigInteger n) {
            // C# is IEEE754 compliant.
            const UInt64 mantissa_mask = 0x000FFFFFFFFFFFFFuL;
            const int mantissa_bits = 52;

            int sign = n.Sign;
            if (sign == 0) {
                return Zero;
            }
            if (sign == -1) {
                n = BigInteger.Negate(n);
            }

            int bits = checked((int)n.GetBitLength());
            int sfts = mantissa_bits * 2 - bits;

            if (sfts > 0) {
                n <<= sfts;
            }
            else if(sfts < 0) {
                n >>= -sfts;
            }

            UInt64 hi = unchecked((UInt64)(n >> mantissa_bits));
            UInt64 lo = unchecked((UInt64)(n & mantissa_mask));

            ddouble v = Ldexp(new ddouble(
                Math.ScaleB((double)hi, mantissa_bits), 
                (double)lo), -sfts);

            return sign > 0 ? v : -v;
        }
    }
}
