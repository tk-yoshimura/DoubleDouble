using System;
using System.Numerics;

namespace DoubleDouble {
    internal static class IntegerSplitter {
        // C# is IEEE754 compliant.
        public static int MantissaBits => 52;
        public static UInt64 MantissaMask => 0x000FFFFFFFFFFFFFuL;

        public static (int sign, UInt64 hi, UInt64 lo, int sfts) Split(BigInteger n) {
            int sign = n.Sign;
            if (sign == 0) {
                return (0, 0uL, 0uL, 0);
            }
            if (sign == -1) {
                n = BigInteger.Negate(n);
            }

            int bits = checked((int)n.GetBitLength());
            int sfts = MantissaBits * 2 - bits;

            n = BigIntegerUtil.LeftShift(n, sfts);

            UInt64 hi = unchecked((UInt64)(n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(n & MantissaMask));

            return (sign, hi, lo, sfts);
        }

        public static (int sign, UInt64 hi, UInt64 lo) Split(long n) {
            if (n == 0) {
                return (0, 0uL, 0uL);
            }

            int sign = n >= 0 ? 1 : -1;
            UInt64 abs_n = UIntUtil.Abs(n);

            UInt64 hi = unchecked((UInt64)(abs_n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(abs_n & MantissaMask));

            return (sign, hi, lo);
        }

        public static (int sign, UInt64 hi, UInt64 lo) Split(ulong n) {
            if (n == 0) {
                return (0, 0uL, 0uL);
            }

            UInt64 hi = unchecked((UInt64)(n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(n & MantissaMask));

            return (1, hi, lo);
        }
    }
}
