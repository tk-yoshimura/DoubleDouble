using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DoubleDouble {
    [DebuggerDisplay("{ToString(),nq}")]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public readonly partial struct UInt128 : IEquatable<UInt128>, IComparable<UInt128> {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt32 e3, e2, e1, e0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public const int Bits = UIntUtil.UInt64Bits * 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(UInt64 hi, UInt64 lo) {
            (this.e3, this.e2) = UIntUtil.Unpack(hi);
            (this.e1, this.e0) = UIntUtil.Unpack(lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt128(UInt32 e3, UInt32 e2, UInt32 e1, UInt32 e0) {
            this.e3 = e3;
            this.e2 = e2;
            this.e1 = e1;
            this.e0 = e0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt128(UInt32 n) {
            return new UInt128(0u, 0u, 0u, n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt128(UInt64 n) {
            return new UInt128(0uL, n);
        }

        public static implicit operator BigInteger(UInt128 n) {
            return ((BigInteger)n.Hi << UIntUtil.UInt64Bits) | n.Lo;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt64 Hi => UIntUtil.Pack(e3, e2);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt64 Lo => UIntUtil.Pack(e1, e0);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt32 E3 => e3;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt32 E2 => e2;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt32 E1 => e1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public UInt32 E0 => e0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static UInt128 Zero { get; } = new(0u, 0u, 0u, 0u);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static UInt128 MaxValue { get; } = new(UInt64.MaxValue, UInt64.MaxValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static UInt128 MaxDigit { get; } = new(0x4B3B4CA8u, 0x5A86C47Au, 0x098A2240u, 0x00000000u);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static int MaxValueDigits { get; } = MaxValue.ToString().Length;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsZero => (e3 | e2 | e1 | e0) == 0u;

        public static bool operator ==(UInt128 a, UInt128 b) {
            return a.E3 == b.E3 && a.E2 == b.E2 && a.E1 == b.E1 && a.E0 == b.E0;
        }

        public static bool operator !=(UInt128 a, UInt128 b) {
            return !(a == b);
        }

        public static bool operator <(UInt128 a, UInt128 b) {
            return
                a.E3 < b.E3 ? true : a.E3 > b.E3 ? false :
                a.E2 < b.E2 ? true : a.E2 > b.E2 ? false :
                a.E1 < b.E1 ? true : a.E1 > b.E1 ? false :
                a.E0 < b.E0;
        }

        public static bool operator <=(UInt128 a, UInt128 b) {
            return
                a.E3 < b.E3 ? true : a.E3 > b.E3 ? false :
                a.E2 < b.E2 ? true : a.E2 > b.E2 ? false :
                a.E1 < b.E1 ? true : a.E1 > b.E1 ? false :
                a.E0 <= b.E0;
        }

        public static bool operator >(UInt128 a, UInt128 b) {
            return
                a.E3 > b.E3 ? true : a.E3 < b.E3 ? false :
                a.E2 > b.E2 ? true : a.E2 < b.E2 ? false :
                a.E1 > b.E1 ? true : a.E1 < b.E1 ? false :
                a.E0 > b.E0;
        }

        public static bool operator >=(UInt128 a, UInt128 b) {
            return
                a.E3 > b.E3 ? true : a.E3 < b.E3 ? false :
                a.E2 > b.E2 ? true : a.E2 < b.E2 ? false :
                a.E1 > b.E1 ? true : a.E1 < b.E1 ? false :
                a.E0 >= b.E0;
        }

        public static UInt128 operator &(UInt128 a, UInt128 b) {
            return new(a.Hi & b.Hi, a.Lo & b.Lo);
        }

        public static UInt128 operator |(UInt128 a, UInt128 b) {
            return new(a.Hi | b.Hi, a.Lo | b.Lo);
        }

        public static UInt128 operator ^(UInt128 a, UInt128 b) {
            return new(a.Hi ^ b.Hi, a.Lo ^ b.Lo);
        }

        public static UInt128 operator ~(UInt128 a) {
            return new(~a.Hi, ~a.Lo);
        }

        public override bool Equals([AllowNull] object obj) {
            return obj is not null && obj is UInt128 v && this == v;
        }

        public bool Equals(UInt128 other) {
            return this == other;
        }

        public int CompareTo(UInt128 other) {
            return this < other ? -1 : this == other ? 0 : +1;
        }

        public override int GetHashCode() {
            return (int)unchecked(E3 ^ E2 ^ E1 ^ E0);
        }
    }
}