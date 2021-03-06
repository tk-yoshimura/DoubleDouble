using System;
using System.Numerics;

namespace DoubleDouble {
    internal static class FloatSplitter {
        // C# is IEEE754 compliant.

        private const UInt64 bitshifts = 0x100000000uL;
        private const UInt32 signmask = 0x80000000u, expmask = 0x7FFFFFFFu;
        private const int mantissa_shiftbits = 12, exponent_shiftbits = 20, exponent_bias = 1023;
        private const UInt64 mantissa_topbit = 0x0010000000000000uL;

        private static UInt64 ToUInt64(double v) {
            return unchecked((UInt64)BitConverter.DoubleToInt64Bits(v));
        }

        public static UInt64 MantissaTopBit => mantissa_topbit;

        public static int MantissaBits => exponent_shiftbits + 32;

        public static (int sign, int exponent, UInt64 mantissa, bool iszero) Split(double v, bool hidden_bit = false) {
            UInt64 val = ToUInt64(v);
            UInt32 val_hi = unchecked((UInt32)(val / bitshifts));

            int sign = val_hi >= signmask ? -1 : 1;

            if (v == 0) {
                return (sign, 0, 0uL, iszero: true);
            }

            int exponent = unchecked((int)((val_hi & expmask) >> exponent_shiftbits)) - exponent_bias;

            UInt64 mantissa = (val << mantissa_shiftbits) >> mantissa_shiftbits;

            if (!hidden_bit) {
                mantissa += MantissaTopBit;
            }

            return (sign, exponent, mantissa, iszero: false);
        }

        public static (int sign, int exponent, BigInteger mantissa, bool iszero) Split(ddouble v) {
            (int sign, int exponent, UInt64 mantissa, bool iszero) hi = Split(v.Hi, hidden_bit: false);
            (int sign, int exponent, UInt64 mantissa, bool iszero) lo = Split(v.Lo, hidden_bit: false);

            if (hi.iszero) {
                return (hi.sign, 0, 0, iszero: true);
            }

            BigInteger mantissa = new BigInteger(hi.mantissa) << MantissaBits;
            if (lo.iszero) {
                return (hi.sign, hi.exponent, mantissa, iszero: false);
            }

            int sfts = hi.exponent - lo.exponent - MantissaBits;

            if (hi.sign == lo.sign) {
                mantissa += BigIntegerUtil.RightShift(lo.mantissa, sfts);
            }
            else {
                mantissa -= BigIntegerUtil.RightShift(lo.mantissa, sfts);
            }

            return (hi.sign, hi.exponent, mantissa, iszero: false);
        }
    }
}
