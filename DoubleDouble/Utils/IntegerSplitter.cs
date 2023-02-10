using System;

namespace DoubleDouble {
    internal static class IntegerSplitter {
        // C# is IEEE754 compliant.
        public const int MantissaBits = 52, UInt64Bits = 64;

        private const UInt64 Bits52Mask = 0x000FFFFFFFFFFFFFuL, Bits53Mask = 0x001FFFFFFFFFFFFFuL;
        private const UInt64 RoundBit = 0x0000000000400000uL, MantissaTopBit = 0x8000000000000000uL;

        public static (int sign, UInt64 hi, UInt64 lo) Split(long n) {
            if (n == 0) {
                return (0, 0uL, 0uL);
            }

            int sign = n >= 0 ? 1 : -1;
            UInt64 abs_n = UIntUtil.Abs(n);

            UInt64 hi = unchecked((UInt64)(abs_n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(abs_n & Bits52Mask));

            return (sign, hi, lo);
        }

        public static (int sign, UInt64 hi, UInt64 lo) Split(ulong n) {
            if (n == 0) {
                return (0, 0uL, 0uL);
            }

            UInt64 hi = unchecked((UInt64)(n >> MantissaBits));
            UInt64 lo = unchecked((UInt64)(n & Bits52Mask));

            return (1, hi, lo);
        }

        public static (UInt64 hi52, UInt64 lo53) Split(UInt64 hi, UInt64 lo) {
            if (hi < MantissaTopBit) {
                throw new ArgumentException("Illegal mantissa.");
            }

            UInt64 hi52 = unchecked((UInt64)(hi >> (UInt64Bits - MantissaBits)));
            UInt64 lo53 = unchecked((UInt64)(hi << (2 * MantissaBits - UInt64Bits + 1)) |
                                            (lo >> ((UInt64Bits - MantissaBits) * 2 - 1))) & Bits53Mask;

            bool round = (lo & RoundBit) != 0;

            if (round) {
                if (lo53 < Bits53Mask) {
                    lo53 += 1;
                }
                else {
                    (hi52, lo53) = (hi52 + 1, 0uL);
                }
            }

            return (hi52, lo53);
        }
    }
}
