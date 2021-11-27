using System;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {


        public static implicit operator ddouble(double v) {
            return new ddouble(v);
        }

        public static explicit operator double(ddouble v) {
            return v.hi;
        }

        public static implicit operator ddouble(int n) {
            return new ddouble(n);
        }

        public static explicit operator int(ddouble v) {
            if (ddouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            double d = (double)v;

            if (d < int.MinValue || d > int.MaxValue) {
                throw new OverflowException();
            }

            return (int)d;
        }

        public static implicit operator ddouble(uint n) {
            return new ddouble(n);
        }

        public static explicit operator uint(ddouble v) {
            if (ddouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            double d = (double)v;

            if (d < uint.MinValue || d > uint.MaxValue) {
                throw new OverflowException();
            }

            return (uint)d;
        }

        public static implicit operator ddouble(long n) {
            (int sign, UInt64 hi, UInt64 lo) = IntegerSplitter.Split(n);

            ddouble v = new ddouble(Math.ScaleB((double)hi, IntegerSplitter.MantissaBits), (double)lo);

            return sign >= 0 ? v : -v;
        }

        public static explicit operator long(ddouble v) {
            if (ddouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            (int sign, int exponent, BigInteger mantissa, bool iszero) = FloatSplitter.Split(v);

            if (iszero) {
                return 0L;
            }
            if (exponent > 64) {
                throw new OverflowException();
            }

            BigInteger n = BigIntegerUtil.LeftShift(mantissa, exponent - FloatSplitter.MantissaBits * 2);
            if (sign <= 0) {
                n = BigInteger.Negate(n);
            }
            if (n < long.MinValue || n > long.MaxValue) {
                throw new OverflowException();
            }

            return (long)n;
        }

        public static implicit operator ddouble(ulong n) {
            (int sign, UInt64 hi, UInt64 lo) = IntegerSplitter.Split(n);

            ddouble v = new ddouble(Math.ScaleB((double)hi, IntegerSplitter.MantissaBits), (double)lo);

            return sign >= 0 ? v : -v;
        }

        public static explicit operator ulong(ddouble v) {
            if (ddouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            (int sign, int exponent, BigInteger mantissa, bool iszero) = FloatSplitter.Split(v);

            if (sign < 0) {
                throw new OverflowException();
            }
            if (iszero) {
                return 0L;
            }
            if (exponent > 64) {
                throw new OverflowException();
            }

            BigInteger n = BigIntegerUtil.LeftShift(mantissa, exponent - FloatSplitter.MantissaBits * 2);
            if (n > ulong.MaxValue) {
                throw new OverflowException();
            }

            return (ulong)n;
        }

        public static implicit operator ddouble(BigInteger n) {
            (int sign, UInt64 hi, UInt64 lo, int sfts) = IntegerSplitter.Split(n);

            ddouble v = Ldexp(new ddouble(Math.ScaleB((double)hi, IntegerSplitter.MantissaBits), (double)lo), sfts);

            return sign >= 0 ? v : -v;
        }

        public static explicit operator ddouble(decimal v) {
            return $"{v:e32}";
        }

        public static explicit operator decimal(ddouble v) {
            return decimal.Parse($"{v}");
        }
    }
}
