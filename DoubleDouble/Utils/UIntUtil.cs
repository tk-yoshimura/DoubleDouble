using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace DoubleDouble {
    internal static partial class UIntUtil {
        public const int UInt32Bits = sizeof(UInt32) * 8;
        public const int UInt64Bits = sizeof(UInt64) * 8;
        public const int UInt32MaxDecimalDigits = UInt32Bits * 30103 / 100000;
        public const int UInt64MaxDecimalDigits = UInt64Bits * 30103 / 100000;
        public const UInt32 UInt32MaxDecimal = 1000000000u;
        public const UInt64 UInt64MaxDecimal = 10000000000000000000ul;

        public const UInt32 UInt32Round = UInt32.MaxValue >> 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Abs(Int32 x) {
            return (x >= 0) ? unchecked((UInt32)x) : ~unchecked((UInt32)x) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 Abs(Int64 x) {
            return (x >= 0) ? unchecked((UInt64)x) : ~unchecked((UInt64)x) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPower2(UInt32 value) {
            return (value >= 1) && ((value & (value - 1)) == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPower2(UInt64 value) {
            return (value >= 1) && ((value & (value - 1)) == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Power2(UInt32 value) {
            return UIntUtil.UInt32Bits - LeadingZeroCount(value) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Power2(UInt64 value) {
            return UIntUtil.UInt64Bits - LeadingZeroCount(value) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(UInt32 value) {
            uint cnt = Lzcnt.LeadingZeroCount(value);

            return checked((int)cnt);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(UInt64 value) {
            (UInt32 hi, UInt32 lo) = Unpack(value);

            if (hi == 0) {
                return LeadingZeroCount(lo) + UInt32Bits;
            }

            return LeadingZeroCount(hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (UInt32 high, UInt32 low) Unpack(UInt64 v) {
            UInt32 low = unchecked((UInt32)v);
            UInt32 high = unchecked((UInt32)(v >> UInt32Bits));

            return (high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 Pack(UInt32 high, UInt32 low) {
            return (((UInt64)high) << UInt32Bits) | ((UInt64)low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (UInt32 high, UInt32 low) DecimalUnpack(UInt64 v) {

            UInt32 high, low;

#if DEBUG
            checked
#else
            unchecked
#endif
            {
                high = (UInt32)(v / UInt32MaxDecimal);
                low = (UInt32)(v - (UInt64)high * (UInt64)UInt32MaxDecimal);
            }

            return (high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (UInt32 high, UInt32 low) DecimalUnpack(UInt32 v) {
            UInt32 high = v / UInt32MaxDecimal;
            UInt32 low = v - high * UInt32MaxDecimal;

            return (high, low);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 DecimalPack(UInt32 high, UInt32 low) {
            return (UInt64)high * (UInt64)UInt32MaxDecimal + (UInt64)low;
        }
    }
}
