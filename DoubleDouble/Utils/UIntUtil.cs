using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace DoubleDouble {
    internal static partial class UIntUtil {
        const int UInt32Bits = 32, UInt64Bits = 64;

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
    }
}
