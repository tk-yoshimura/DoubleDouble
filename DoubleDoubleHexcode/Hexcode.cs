using System.Diagnostics;

namespace DoubleDoubleHexcode {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct Hexcode {
        public const int MantissaBits = 116, UInt64Bits = 64, MantissaBias = 1023;

        private const UInt64 MantissaOneHi = 0x0008000000000000uL;
        private const UInt64 Bits51Mask = 0x000FFFFFFFFFFFFFuL, Bits52Mask = 0x0010000000000000uL;
        private const UInt64 RoundBit = 0x0000000000000800uL, MantissaTopBit = 0x8000000000000000uL;

        private readonly UInt64 hi, lo;

        private Hexcode(UInt64 hi, UInt64 lo) {
            this.hi = hi;
            this.lo = lo;
        }

        public static Hexcode Zero { get; } = new Hexcode(0uL, 0uL);

        public UInt64 Hi => hi;
        public UInt64 Lo => lo;

        public static implicit operator (UInt64 hi, UInt64 lo)(Hexcode hex) {
            return (hex.hi, hex.lo);
        }

        public static implicit operator Hexcode((int sign, int exponent, UInt64 hi, UInt64 lo) bits) {
            if (bits.sign == 0) {
                if (bits.exponent != 0 || bits.hi != 0 || bits.lo != 0) {
                    throw new ArgumentException("Invalid zero set.");
                }

                return Zero;
            }


            (UInt64 hi52, UInt64 lo64) mantissabits = Split(bits.hi, bits.lo);

            (int exponent, mantissabits.hi52) = mantissabits.hi52 < Bits52Mask
                ? (bits.exponent, mantissabits.hi52)
                : (checked(bits.exponent + 1), MantissaOneHi);

            if (exponent < -MantissaBias + 1 || exponent > MantissaBias) {
                throw new ArgumentOutOfRangeException(nameof(exponent), "Too large exponent.");
            }

            UInt32 signbit = (bits.sign < 0 ? 1u : 0u) << 31;
            UInt32 exponentbits = (UInt32)(exponent + MantissaBias) << 20;

            UInt64 hi = mantissabits.hi52 | ((UInt64)(signbit | exponentbits) << 32);
            UInt64 lo = mantissabits.lo64;

            return new Hexcode(hi, lo);
        }

        private static (UInt64 hi52, UInt64 lo64) Split(UInt64 hi, UInt64 lo) {
            if (hi < MantissaTopBit) {
                throw new ArgumentException("Invalid mantissa.");
            }

            UInt64 hi52 = unchecked((UInt64)(hi >> (2 * UInt64Bits - MantissaBits)));
            UInt64 lo64 = unchecked((UInt64)(hi << (MantissaBits - UInt64Bits)) |
                                            (lo >> (2 * UInt64Bits - MantissaBits)) & Bits51Mask);

            bool round = (lo & RoundBit) != 0;

            if (round) {
                if (lo64 < UInt64.MaxValue) {
                    lo64 += 1;
                }
                else {
                    (hi52, lo64) = (hi52 + 1, 0uL);
                }
            }

            return (hi52, lo64);
        }

        public override string ToString() {
            return $"0x{hi:X16}{lo:X16}";
        }
    }
}