using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public readonly partial struct UInt128 {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 a, UInt128 b) {
            UInt32 carry, e3, e2, e1, e0;

            (carry, e0) = UIntUtil.Unpack(unchecked((UInt64)a.e0 + (UInt64)b.e0));
            (carry, e1) = UIntUtil.Unpack(unchecked((UInt64)a.e1 + (UInt64)b.e1 + carry));
            (carry, e2) = UIntUtil.Unpack(unchecked((UInt64)a.e2 + (UInt64)b.e2 + carry));
            (carry, e3) = UIntUtil.Unpack(unchecked((UInt64)a.e3 + (UInt64)b.e3 + carry));

            if (carry > 0u) {
                throw new OverflowException();
            }

            return new(e3, e2, e1, e0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 a, UInt64 b) {
            return (b <= unchecked(a.Lo + b)) ? new(a.Hi, a.Lo + b) : (a.Hi < ~0uL) ? new(a.Hi + 1uL, unchecked(a.Lo + b))
                : throw new OverflowException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt64 a, UInt128 b) {
            return b + a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator -(UInt128 a, UInt128 b) {
            UInt64 hi = ~b.Hi, lo = ~b.Lo;
            (hi, lo) = (lo < ~0uL) ? (hi, lo + 1uL) : (hi < ~0uL) ? (hi + 1uL, 0uL) : (0uL, 0uL);

            UInt128 b_comp = new(hi, lo);

            UInt32 carry, e3, e2, e1, e0;

            (carry, e0) = UIntUtil.Unpack(unchecked((UInt64)a.e0 + (UInt64)b_comp.e0));
            (carry, e1) = UIntUtil.Unpack(unchecked((UInt64)a.e1 + (UInt64)b_comp.e1 + carry));
            (carry, e2) = UIntUtil.Unpack(unchecked((UInt64)a.e2 + (UInt64)b_comp.e2 + carry));
            (carry, e3) = UIntUtil.Unpack(unchecked((UInt64)a.e3 + (UInt64)b_comp.e3 + carry));

            if (carry < 1u && !b.IsZero) {
                throw new OverflowException();
            }

            return new(e3, e2, e1, e0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator -(UInt128 a, UInt64 b) {
            return (a.Lo >= b) ? new(a.Hi, a.Lo - b) : (a.Hi > 0uL) ? new(a.Hi - 1uL, unchecked(a.Lo - b))
                : throw new OverflowException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator -(UInt64 a, UInt128 b) {
            return (b.Hi == 0uL && a >= b.Lo) ? (a - b.Lo)
                : throw new OverflowException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 a, UInt128 b) {
            if ((a.e1 > 0u && b.e3 > 0u) || (a.e2 > 0u && b.e2 > 0u) || (a.e3 > 0u && b.e1 > 0u) ||
                (a.e2 > 0u && b.e3 > 0u) || (a.e3 > 0u && b.e2 > 0u) || (a.e3 > 0u && b.e3 > 0u)) {
                throw new OverflowException();
            }

            (UInt64 v00_hi, UInt32 e0) = UIntUtil.Unpack((UInt64)a.e0 * b.e0);

            (UInt64 v01_hi, UInt64 v01_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e1);
            (UInt64 v10_hi, UInt64 v10_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e0);

            (UInt64 v02_hi, UInt64 v02_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e2);
            (UInt64 v11_hi, UInt64 v11_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e1);
            (UInt64 v20_hi, UInt64 v20_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e0);

            (UInt32 v03_hi, UInt64 v03_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e3);
            (UInt32 v12_hi, UInt64 v12_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e2);
            (UInt32 v21_hi, UInt64 v21_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e1);
            (UInt32 v30_hi, UInt64 v30_lo) = UIntUtil.Unpack((UInt64)a.e3 * b.e0);

            (UInt32 carry1, UInt32 e1) = UIntUtil.Unpack(v00_hi + v01_lo + v10_lo);
            (UInt32 carry2, UInt32 e2) = UIntUtil.Unpack(v01_hi + v10_hi + v02_lo + v11_lo + v20_lo + carry1);
            (UInt32 carry3, UInt32 e3) = UIntUtil.Unpack(v02_hi + v11_hi + v20_hi + v03_lo + v12_lo + v21_lo + v30_lo + carry2);

            if (carry3 > 0u || v03_hi > 0u || v12_hi > 0u || v21_hi > 0u || v30_hi > 0u) {
                throw new OverflowException();
            }

            return new UInt128(e3, e2, e1, e0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 a, UInt64 b) {
            (UInt32 b_hi, UInt32 b_lo) = UIntUtil.Unpack(b);

            if (a.e3 > 0u && b_hi > 0u) {
                throw new OverflowException();
            }

            (UInt64 v00_hi, UInt32 e0) = UIntUtil.Unpack((UInt64)a.e0 * b_lo);

            (UInt64 v01_hi, UInt64 v01_lo) = UIntUtil.Unpack((UInt64)a.e0 * b_hi);
            (UInt64 v10_hi, UInt64 v10_lo) = UIntUtil.Unpack((UInt64)a.e1 * b_lo);

            (UInt64 v11_hi, UInt64 v11_lo) = UIntUtil.Unpack((UInt64)a.e1 * b_hi);
            (UInt64 v20_hi, UInt64 v20_lo) = UIntUtil.Unpack((UInt64)a.e2 * b_lo);

            (UInt32 v21_hi, UInt64 v21_lo) = UIntUtil.Unpack((UInt64)a.e2 * b_hi);
            (UInt32 v30_hi, UInt64 v30_lo) = UIntUtil.Unpack((UInt64)a.e3 * b_lo);

            (UInt32 carry1, UInt32 e1) = UIntUtil.Unpack(v00_hi + v01_lo + v10_lo);
            (UInt32 carry2, UInt32 e2) = UIntUtil.Unpack(v01_hi + v10_hi + v11_lo + v20_lo + carry1);
            (UInt32 carry3, UInt32 e3) = UIntUtil.Unpack(v11_hi + v20_hi + v21_lo + v30_lo + carry2);

            if (carry3 > 0u || v21_hi > 0u || v30_hi > 0u) {
                throw new OverflowException();
            }

            return new UInt128(e3, e2, e1, e0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 a, UInt32 b) {
            (UInt64 v00_hi, UInt32 e0) = UIntUtil.Unpack((UInt64)a.e0 * b);
            (UInt64 v10_hi, UInt64 v10_lo) = UIntUtil.Unpack((UInt64)a.e1 * b);
            (UInt64 v20_hi, UInt64 v20_lo) = UIntUtil.Unpack((UInt64)a.e2 * b);
            (UInt32 v30_hi, UInt64 v30_lo) = UIntUtil.Unpack((UInt64)a.e3 * b);

            (UInt32 carry1, UInt32 e1) = UIntUtil.Unpack(v00_hi + v10_lo);
            (UInt32 carry2, UInt32 e2) = UIntUtil.Unpack(v10_hi + v20_lo + carry1);
            (UInt32 carry3, UInt32 e3) = UIntUtil.Unpack(v20_hi + v30_lo + carry2);

            if (carry3 > 0u || v30_hi > 0u) {
                throw new OverflowException();
            }

            return new UInt128(e3, e2, e1, e0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt64 a, UInt128 b) {
            return b * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt32 a, UInt128 b) {
            return b * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (UInt128 q, UInt128 r) DivRem(UInt128 a, UInt128 b) {
            if (a < b) {
                return (Zero, a);
            }
            if (b.IsZero) {
                throw new DivideByZeroException();
            }
            if (a.Hi == 0uL) {
                return a.e1 > 0u ? (a.Lo / b.Lo, a.Lo % b.Lo) : (a.e0 / b.e0, a.e0 % b.e0);
            }
            if (b.e3 >= 0x80000000u) {
                return (a < b) ? (0u, a) : (1u, a - b);
            }

            UInt128 q = Zero, r = a;

            int b_offset = LeadingZeroCount(b);
            UInt128 b_sft = b << b_offset;

            UInt64 div = b_sft.e3 + (((b_sft.e2 | b_sft.e1 | b_sft.e0) > 0u) ? 1uL : 0uL);

            for (; ; ) {
                int r_offset = LeadingZeroCount(r);
                if (r_offset >= b_offset) {
                    break;
                }

                int sft = b_offset - r_offset - UIntUtil.UInt32Bits;

                UInt64 n = (r << r_offset).Hi / div;
                UInt128 n_sft = sft > 0 ? ((UInt128)n << sft) : ((UInt128)n >> (-sft));

                q += n_sft;
                r -= n_sft * b;
            }

            if (r >= b) {
                q += 1u;
                r -= b;
            }

#if DEBUG
            Trace.Assert(r < b && a == r + q * b, "Detected divide bug.");
#endif

            return (q, r);
        }

        public static UInt128 operator /(UInt128 a, UInt128 b) => DivRem(a, b).q;

        public static UInt128 operator %(UInt128 a, UInt128 b) => DivRem(a, b).r;

        public static UInt128 RoundDiv(UInt128 x, UInt128 y) {
            (UInt128 n, UInt128 r) = DivRem(x, y);

            if (r >= (y >> 1)) {
                n += 1;
            }

            return n;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (UInt128 hi, UInt128 lo) ExpandMul(UInt128 a, UInt128 b) {
            (UInt64 v00_hi, UInt32 e0) = UIntUtil.Unpack((UInt64)a.e0 * b.e0);

            (UInt64 v01_hi, UInt64 v01_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e1);
            (UInt64 v10_hi, UInt64 v10_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e0);

            (UInt64 v02_hi, UInt64 v02_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e2);
            (UInt64 v11_hi, UInt64 v11_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e1);
            (UInt64 v20_hi, UInt64 v20_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e0);

            (UInt64 v03_hi, UInt64 v03_lo) = UIntUtil.Unpack((UInt64)a.e0 * b.e3);
            (UInt64 v12_hi, UInt64 v12_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e2);
            (UInt64 v21_hi, UInt64 v21_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e1);
            (UInt64 v30_hi, UInt64 v30_lo) = UIntUtil.Unpack((UInt64)a.e3 * b.e0);

            (UInt64 v13_hi, UInt64 v13_lo) = UIntUtil.Unpack((UInt64)a.e1 * b.e3);
            (UInt64 v22_hi, UInt64 v22_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e2);
            (UInt64 v31_hi, UInt64 v31_lo) = UIntUtil.Unpack((UInt64)a.e3 * b.e1);

            (UInt64 v23_hi, UInt64 v23_lo) = UIntUtil.Unpack((UInt64)a.e2 * b.e3);
            (UInt64 v32_hi, UInt64 v32_lo) = UIntUtil.Unpack((UInt64)a.e3 * b.e2);

            (UInt64 v33_hi, UInt64 v33_lo) = UIntUtil.Unpack((UInt64)a.e3 * b.e3);

            (UInt32 carry1, UInt32 e1) = UIntUtil.Unpack(v00_hi + v01_lo + v10_lo);
            (UInt32 carry2, UInt32 e2) = UIntUtil.Unpack(v01_hi + v10_hi + v02_lo + v11_lo + v20_lo + carry1);
            (UInt32 carry3, UInt32 e3) = UIntUtil.Unpack(v02_hi + v11_hi + v20_hi + v03_lo + v12_lo + v21_lo + v30_lo + carry2);
            (UInt32 carry4, UInt32 e4) = UIntUtil.Unpack(v03_hi + v12_hi + v21_hi + v30_hi + v13_lo + v22_lo + v31_lo + carry3);
            (UInt32 carry5, UInt32 e5) = UIntUtil.Unpack(v13_hi + v22_hi + v31_hi + v23_lo + v32_lo + carry4);
            (UInt32 carry6, UInt32 e6) = UIntUtil.Unpack(v23_hi + v32_hi + v33_lo + carry5);
            (UInt32 _, UInt32 e7) = UIntUtil.Unpack(v33_hi + carry6);

            UInt128 hi = new(e7, e6, e5, e4), lo = new(e3, e2, e1, e0);

            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 MulShift(UInt128 a, UInt128 b, int right_sfts) {
            if (right_sfts < 0) {
                throw new ArgumentOutOfRangeException(nameof(right_sfts));
            }

            (UInt128 hi, UInt128 lo) = ExpandMul(a, b);

            if (right_sfts < Bits) {
                int lzc = LeadingZeroCount(hi);

                if (lzc + right_sfts < Bits) {
                    throw new OverflowException();
                }

                UInt128 c = (hi << (Bits - right_sfts)) | (lo >> right_sfts);

                return c;
            }
            else {
                return hi >> (right_sfts - Bits);
            }
        }
    }
}