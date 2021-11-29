using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal partial struct qdouble : IComparable, IComparable<qdouble>, IEquatable<qdouble> {

        #region cmp qdouble qdouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(qdouble a, qdouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo < b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(qdouble a, qdouble b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(qdouble a, qdouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo <= b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(qdouble a, qdouble b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(qdouble a, qdouble b) {
            return a.hi == b.hi && a.lo == b.lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(qdouble a, qdouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp qdouble ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(qdouble a, ddouble b) {
            return a.hi < b || (a.hi <= b && a.lo < 0d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(qdouble a, ddouble b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(qdouble a, ddouble b) {
            return a.hi < b || (a.hi <= b && a.lo <= 0d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(qdouble a, ddouble b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(qdouble a, ddouble b) {
            return a.hi == b && a.lo == 0d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(qdouble a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble qdouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, qdouble b) {
            return a < b.hi || (a <= b.hi && 0d < b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, qdouble b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, qdouble b) {
            return a < b.hi || (a <= b.hi && 0d <= b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, qdouble b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, qdouble b) {
            return a == b.hi && 0d == b.lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, qdouble b) {
            return !(a == b);
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble Min(qdouble a, qdouble b) {
            return a < b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble Max(qdouble a, qdouble b) {
            return a > b ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble BitIncrement(qdouble v) {
            return new qdouble(v.hi, ddouble.BitIncrement(v.lo));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static qdouble BitDecrement(qdouble v) {
            return new qdouble(v.hi, ddouble.BitDecrement(v.lo));
        }

        public override bool Equals(object obj) {
            return obj is qdouble v && this == v;
        }

        public bool Equals(qdouble other) {
            return this == other;
        }

        public override int GetHashCode() {
            return hi.GetHashCode() ^ lo.GetHashCode();
        }

        public int CompareTo(object obj) {
            if (obj is qdouble value) {
                return CompareTo(value);
            }

            throw new ArgumentException(null, nameof(obj));
        }

        public int CompareTo(qdouble value) {
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
