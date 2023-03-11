using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace DoubleDouble {
    internal readonly partial struct UInt128 {
        public static UInt128 operator >>(UInt128 v, int sft) {
            unchecked {
                if (sft >= UIntUtil.UInt32Bits * 4) {
                    return Zero;
                }
                else if (sft > UIntUtil.UInt32Bits * 3) {
                    return new UInt128(0u, 0u, 0u, v.e3 >> (sft - UIntUtil.UInt32Bits * 3));
                }
                else if (sft == UIntUtil.UInt32Bits * 3) {
                    return new UInt128(0u, 0u, 0u, v.e3);
                }
                else if (sft > UIntUtil.UInt32Bits * 2) {
                    UInt32 e1 = v.e3 >> (sft - UIntUtil.UInt32Bits * 2);
                    UInt32 e0 =
                        (v.e3 << (UIntUtil.UInt32Bits * 3 - sft)) |
                        (v.e2 >> (sft - UIntUtil.UInt32Bits * 2));

                    return new UInt128(0u, 0u, e1, e0);
                }
                else if (sft == UIntUtil.UInt32Bits * 2) {
                    return new UInt128(0u, 0u, v.e3, v.e2);
                }
                else if (sft > UIntUtil.UInt32Bits) {
                    UInt32 e2 = v.e3 >> (sft - UIntUtil.UInt32Bits);
                    UInt32 e1 =
                        (v.e3 << (UIntUtil.UInt32Bits * 2 - sft)) |
                        (v.e2 >> (sft - UIntUtil.UInt32Bits));
                    UInt32 e0 =
                        (v.e2 << (UIntUtil.UInt32Bits * 2 - sft)) |
                        (v.e1 >> (sft - UIntUtil.UInt32Bits));

                    return new UInt128(0u, e2, e1, e0);
                }
                else if (sft == UIntUtil.UInt32Bits) {
                    return new UInt128(0u, v.e3, v.e2, v.e1);
                }
                else if (sft > 0) {
                    UInt32 e3 = v.e3 >> sft;
                    UInt32 e2 =
                        (v.e3 << (UIntUtil.UInt32Bits - sft)) |
                        (v.e2 >> sft);
                    UInt32 e1 =
                        (v.e2 << (UIntUtil.UInt32Bits - sft)) |
                        (v.e1 >> sft);
                    UInt32 e0 =
                        (v.e1 << (UIntUtil.UInt32Bits - sft)) |
                        (v.e0 >> sft);

                    return new UInt128(e3, e2, e1, e0);
                }
                else if (sft == 0) {
                    return v;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(sft));
        }

        public static UInt128 operator <<(UInt128 v, int sft) {
            unchecked {
                if (sft >= UIntUtil.UInt32Bits * 4) {
                    return Zero;
                }
                else if (sft > UIntUtil.UInt32Bits * 3) {
                    return new UInt128(v.e0 << (sft - UIntUtil.UInt32Bits * 3), 0u, 0u, 0u);
                }
                else if (sft == UIntUtil.UInt32Bits * 3) {
                    return new UInt128(v.e0, 0u, 0u, 0u);
                }
                else if (sft > UIntUtil.UInt32Bits * 2) {
                    UInt32 e3 =
                        (v.e1 << (sft - UIntUtil.UInt32Bits * 2)) |
                        (v.e0 >> (UIntUtil.UInt32Bits * 3 - sft));
                    UInt32 e2 = v.e0 << (sft - UIntUtil.UInt32Bits * 2);

                    return new UInt128(e3, e2, 0u, 0u);
                }
                else if (sft == UIntUtil.UInt32Bits * 2) {
                    return new UInt128(v.e1, v.e0, 0u, 0u);
                }
                else if (sft > UIntUtil.UInt32Bits) {
                    UInt32 e3 =
                        (v.e2 << (sft - UIntUtil.UInt32Bits)) |
                        (v.e1 >> (UIntUtil.UInt32Bits * 2 - sft));
                    UInt32 e2 =
                        (v.e1 << (sft - UIntUtil.UInt32Bits)) |
                        (v.e0 >> (UIntUtil.UInt32Bits * 2 - sft));
                    UInt32 e1 = v.e0 << (sft - UIntUtil.UInt32Bits);

                    return new UInt128(e3, e2, e1, 0u);
                }
                else if (sft == UIntUtil.UInt32Bits) {
                    return new UInt128(v.e2, v.e1, v.e0, 0u);
                }
                else if (sft > 0) {
                    UInt32 e3 =
                        (v.e3 << sft) |
                        (v.e2 >> (UIntUtil.UInt32Bits - sft));
                    UInt32 e2 =
                        (v.e2 << sft) |
                        (v.e1 >> (UIntUtil.UInt32Bits - sft));
                    UInt32 e1 =
                        (v.e1 << sft) |
                        (v.e0 >> (UIntUtil.UInt32Bits - sft));
                    UInt32 e0 = v.e0 << sft;

                    return new UInt128(e3, e2, e1, e0);
                }
                else if (sft == 0) {
                    return v;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(sft));
        }

        public static UInt128 RightShift(UInt128 n, int sfts) {
            if (sfts > 0) {
                return n >> sfts;
            }
            if (sfts < 0) {
                return n << -sfts;
            }
            return n;
        }

        public static UInt128 LeftShift(UInt128 n, int sfts) {
            if (sfts > 0) {
                return n << sfts;
            }
            if (sfts < 0) {
                return n >> -sfts;
            }
            return n;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeadingZeroCount(UInt128 v) {
            uint cnt =
                (v.e3 > 0u) ? Lzcnt.LeadingZeroCount(v.e3) :
                (v.e2 > 0u) ? Lzcnt.LeadingZeroCount(v.e2) + UIntUtil.UInt32Bits :
                (v.e1 > 0u) ? Lzcnt.LeadingZeroCount(v.e1) + UIntUtil.UInt32Bits * 2 :
                (v.e0 > 0u) ? Lzcnt.LeadingZeroCount(v.e0) + UIntUtil.UInt32Bits * 3 :
                UIntUtil.UInt32Bits * 4;

            return unchecked((int)cnt);
        }
    }
}