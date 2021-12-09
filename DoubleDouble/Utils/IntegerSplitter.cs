using System;
using System.Numerics;

namespace DoubleDouble {
    internal static class IntegerSplitter {
        // C# is IEEE754 compliant.
        public static int MantissaBits => 52;
        public static UInt64 MantissaMask => 0x000FFFFFFFFFFFFFuL;

        public static (int sign, UInt64 hi, UInt64 lo, int sfts) SplitX2(BigInteger n) {
            int sign = n.Sign;
            if (sign == 0) {
                return (0, 0uL, 0uL, 0);
            }
            if (sign == -1) {
                n = BigInteger.Negate(n);
            }

            int bits = checked((int)n.GetBitLength());
            int sfts = bits - MantissaBits * 2;

            n = BigIntegerUtil.RightShift(n, sfts);

            UInt64 hi = unchecked((UInt64)(n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(n & MantissaMask));

            return (sign, hi, lo, sfts);
        }

        public static (int sign, UInt64 v3, UInt64 v2, UInt64 v1, UInt64 v0, int sfts) SplitX4(BigInteger n) {
            int sign = n.Sign;
            if (sign == 0) {
                return (0, 0uL, 0uL, 0uL, 0uL, 0);
            }
            if (sign == -1) {
                n = BigInteger.Negate(n);
            }

            int bits = checked((int)n.GetBitLength());
            int sfts = bits - MantissaBits * 4;

            n = BigIntegerUtil.RightShift(n, sfts);

            UInt64 v3 = unchecked((UInt64)((n >> (MantissaBits * 3)) & MantissaMask));
            UInt64 v2 = unchecked((UInt64)((n >> (MantissaBits * 2)) & MantissaMask));
            UInt64 v1 = unchecked((UInt64)((n >> MantissaBits) & MantissaMask));
            UInt64 v0 = unchecked((UInt64)(n & MantissaMask));

            return (sign, v3, v2, v1, v0, sfts);
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
