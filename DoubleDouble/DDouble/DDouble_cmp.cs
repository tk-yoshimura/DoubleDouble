using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble : IComparable, IComparable<ddouble>, IEquatable<ddouble> {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo < b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, ddouble b) {
            return (b < a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo <= b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, ddouble b) {
            return (b <= a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, ddouble b) {
            return a.hi == b.hi && a.lo == b.lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, ddouble b) {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Min(ddouble a, ddouble b) {
            return a < b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Max(ddouble a, ddouble b) {
            return a > b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble BitIncrement(ddouble v) {
            return new ddouble(v.hi, Math.BitIncrement(v.lo));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble BitDecrement(ddouble v) {
            return new ddouble(v.hi, Math.BitDecrement(v.lo));
        }

        public override bool Equals(object obj) {
            return obj is ddouble v && this == v;
        }

        public bool Equals(ddouble other) {
            return this == other;
        }

        public override int GetHashCode() {
            return hi.GetHashCode() ^ lo.GetHashCode();
        }

        public int CompareTo(object obj) {
            if (obj is ddouble value) {
                return CompareTo(value);
            }

            throw new ArgumentException(null, nameof(obj));
        }

        public int CompareTo(ddouble value) {
            if (this < value || (IsNaN(this) && IsFinite(value))) {
                return -1;
            }
            if (this > value || (IsFinite(this) && IsNaN(value))) {
                return +1;
            }
            return 0;
        }
    }
}
