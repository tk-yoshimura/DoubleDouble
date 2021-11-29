using System;
using System.Numerics;

namespace DoubleDouble {
    internal partial struct qdouble {

        public static implicit operator qdouble(double v) {
            return new qdouble(v);
        }

        public static implicit operator qdouble(ddouble v) {
            return new qdouble(v);
        }

        public static explicit operator ddouble(qdouble v) {
            return v.hi;
        }

        public static explicit operator int(qdouble v) {
            if (qdouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            ddouble d = (ddouble)v;

            if (d < int.MinValue || d > int.MaxValue) {
                throw new OverflowException();
            }

            return (int)d;
        }

        public static explicit operator uint(qdouble v) {
            if (qdouble.IsNaN(v)) {
                throw new InvalidCastException();
            }

            ddouble d = (ddouble)v;

            if (d < uint.MinValue || d > uint.MaxValue) {
                throw new OverflowException();
            }

            return (uint)d;
        }

        public static explicit operator long(qdouble v) {
            if (qdouble.IsNaN(v)) {
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

        public static explicit operator ulong(qdouble v) {
            if (qdouble.IsNaN(v)) {
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

        public static implicit operator qdouble(BigInteger n) {
            (int sign, UInt64 v3, UInt64 v2, UInt64 v1, UInt64 v0, int sfts) = IntegerSplitter.SplitX4(n);

            qdouble v = new qdouble(
                new ddouble(
                    Math.ScaleB((double)v3, IntegerSplitter.MantissaBits * 3 + sfts),
                    Math.ScaleB((double)v2, IntegerSplitter.MantissaBits * 2 + sfts)
                ),
                new ddouble(
                    Math.ScaleB((double)v1, IntegerSplitter.MantissaBits + sfts),
                    Math.ScaleB((double)v0, sfts)
                )
            );

            return sign >= 0 ? v : -v;
        }

        public static explicit operator qdouble(decimal v) {
            return (ddouble)v;
        }

        public static explicit operator decimal(qdouble v) {
            return (decimal)(ddouble)v;
        }
    }
}
