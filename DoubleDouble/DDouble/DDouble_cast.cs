using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static implicit operator ddouble(double v) {
            return new ddouble(v);
        }

        public static explicit operator double(ddouble v) {
            return v.hi;
        }

        public static explicit operator float(ddouble v) {
            return (float)v.hi;
        }

        public static implicit operator ddouble(int n) {
            return new ddouble(n);
        }

        public static explicit operator int(ddouble v) {
            if (IsNaN(v)) {
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
            if (IsNaN(v)) {
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

            ddouble v = new ddouble(double.ScaleB((double)hi, IntegerSplitter.MantissaBits), (double)lo);

            return sign >= 0 ? v : -v;
        }

        public static explicit operator long(ddouble v) {
            if (IsNaN(v)) {
                throw new InvalidCastException();
            }

            (int sign, int exponent, UInt128 mantissa, bool iszero) = FloatSplitter.Split(v);

            if (iszero) {
                return 0L;
            }
            if (exponent > 64) {
                throw new OverflowException();
            }

            UInt128 n = UInt128.LeftShift(mantissa, exponent - FloatSplitter.MantissaBits * 2);
            if (n.Hi > 0uL || (sign >= 0 && n.Lo > 0x7FFFFFFFFFFFFFFFuL) || (sign < 0 && n.Lo > 0x8000000000000000uL)) {
                throw new OverflowException();
            }

            if (sign >= 0) {
                return (long)n.Lo;
            }
            else {
                return (n.Lo <= 0x7FFFFFFFFFFFFFFFuL) ? -(long)n.Lo : long.MinValue;
            }
        }

        public static implicit operator ddouble(ulong n) {
            (int sign, UInt64 hi, UInt64 lo) = IntegerSplitter.Split(n);

            ddouble v = new ddouble(double.ScaleB((double)hi, IntegerSplitter.MantissaBits), (double)lo);

            return sign >= 0 ? v : -v;
        }

        public static explicit operator ulong(ddouble v) {
            if (IsNaN(v)) {
                throw new InvalidCastException();
            }

            (int sign, int exponent, UInt128 mantissa, bool iszero) = FloatSplitter.Split(v);

            if (sign < 0) {
                throw new OverflowException();
            }
            if (iszero) {
                return 0L;
            }
            if (exponent > 64) {
                throw new OverflowException();
            }

            UInt128 n = UInt128.LeftShift(mantissa, exponent - FloatSplitter.MantissaBits * 2);
            if (n.Hi > 0uL) {
                throw new OverflowException();
            }

            return n.Lo;
        }

        public static implicit operator ddouble(BigInteger n) {
            int sign = n.Sign;

            if (sign == 0) {
                return Zero;
            }
            if (sign == -1) {
                return -(ddouble)BigInteger.Negate(n);
            }

            int bits = checked((int)n.GetBitLength());
            int sfts = checked(bits - IntegerSplitter.UInt64Bits * 2);

            n = (sfts >= 0) ? (n >> sfts) : (n << (-sfts));

            UInt64 hi = unchecked((UInt64)(n >> IntegerSplitter.UInt64Bits));
            UInt64 lo = unchecked((UInt64)(n & (~0uL)));

            ddouble v = (sign: +1, exponent: bits - 1, hi, lo);

            return v;
        }

        public static implicit operator ddouble(decimal v) {
            int[] arr = decimal.GetBits(v);

            int sign = arr[3] >= 0 ? 1 : -1;
            int exponent = (arr[3] >> 16) & 0xFF;

            UInt32[] mantissa = new UInt32[3];

            mantissa[0] = unchecked((UInt32)arr[0]);
            mantissa[1] = unchecked((UInt32)arr[1]);
            mantissa[2] = unchecked((UInt32)arr[2]);

            UInt128 num = new UInt128(0u, mantissa[2], mantissa[1], mantissa[0]);

            while (exponent > 0 && num % 10 == 0) {
                exponent--;
                num /= 10;
            }

            ddouble x = FromStringCore(sign, 0, num, exponent);

            return x;
        }

        public static explicit operator decimal(ddouble v) {
            const int digits = 28;

            (int sign, int exponent, UInt128 num) = v.ToStringCore(digits);

            exponent -= digits;

            while (exponent < 0 && num % 10 == 0) {
                exponent++;
                num /= 10;
            }

            decimal d = new decimal(
                unchecked((Int32)num.E0),
                unchecked((Int32)num.E1),
                unchecked((Int32)num.E2),
                isNegative: sign < 0, scale: checked((byte)(-exponent))
            );

            return d;
        }

        public static implicit operator ddouble((int sign, int exponent, UInt64 hi, UInt64 lo) bits) {
            if (bits.sign == 0) {
                if (bits.exponent != 0 || bits.hi != 0 || bits.lo != 0) {
                    throw new ArgumentException("Invalid zero set.");
                }

                return Zero;
            }

            (UInt64 hi52, UInt64 lo53) = IntegerSplitter.Split(bits.hi, bits.lo);

            ddouble v = new ddouble(
                double.ScaleB((double)hi52, checked(bits.exponent + 1 - IntegerSplitter.MantissaBits)),
                double.ScaleB((double)lo53, checked(bits.exponent - IntegerSplitter.MantissaBits * 2))
            );

            return bits.sign >= 0 ? v : -v;
        }


        internal static ddouble FromUInt128(UInt128 n) {
            if (n.IsZero) {
                return Zero;
            }

            int lzc = UInt128.LeadingZeroCount(n);
            UInt128 n_sft = n << lzc;

            ddouble v = (sign: +1, exponent: UInt128.Bits - lzc - 1, n_sft.Hi, n_sft.Lo);

            return v;
        }
    }
}
