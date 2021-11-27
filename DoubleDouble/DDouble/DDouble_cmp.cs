using System;
using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble : IComparable, IComparable<ddouble>, IEquatable<ddouble> {

        #region cmp ddouble ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo < b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, ddouble b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo <= b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, ddouble b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, ddouble b) {
            return a.hi == b.hi && a.lo == b.lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble double
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, double b) {
            return a.hi < b || (a.hi <= b && a.lo < 0d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, double b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, double b) {
            return a.hi < b || (a.hi <= b && a.lo <= 0d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, double b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, double b) {
            return a.hi == b && a.lo == 0d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, double b) {
            return !(a == b);
        }
        #endregion

        #region cmp double ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(double a, ddouble b) {
            return a < b.hi || (a <= b.hi && 0d < b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(double a, ddouble b) {
            return b < a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(double a, ddouble b) {
            return a < b.hi || (a <= b.hi && 0d <= b.lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(double a, ddouble b) {
            return b <= a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(double a, ddouble b) {
            return a == b.hi && 0d == b.lo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(double a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble long
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, long b) {
            return a < (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, long b) {
            return a > (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, long b) {
            return a <= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, long b) {
            return a >= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, long b) {
            return a == (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, long b) {
            return !(a == b);
        }
        #endregion

        #region cmp long ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(long a, ddouble b) {
            return (ddouble)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(long a, ddouble b) {
            return (ddouble)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(long a, ddouble b) {
            return (ddouble)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(long a, ddouble b) {
            return (ddouble)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long a, ddouble b) {
            return (ddouble)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble ulong
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, ulong b) {
            return a < (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, ulong b) {
            return a > (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, ulong b) {
            return a <= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, ulong b) {
            return a >= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, ulong b) {
            return a == (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, ulong b) {
            return !(a == b);
        }
        #endregion

        #region cmp ulong ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ulong a, ddouble b) {
            return (ddouble)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ulong a, ddouble b) {
            return (ddouble)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ulong a, ddouble b) {
            return (ddouble)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ulong a, ddouble b) {
            return (ddouble)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong a, ddouble b) {
            return (ddouble)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble int
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, int b) {
            return a < (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, int b) {
            return a > (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, int b) {
            return a <= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, int b) {
            return a >= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, int b) {
            return a == (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, int b) {
            return !(a == b);
        }
        #endregion

        #region cmp int ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, ddouble b) {
            return (ddouble)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, ddouble b) {
            return (ddouble)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, ddouble b) {
            return (ddouble)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, ddouble b) {
            return (ddouble)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, ddouble b) {
            return (ddouble)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble uint
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, uint b) {
            return a < (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, uint b) {
            return a > (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, uint b) {
            return a <= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, uint b) {
            return a >= (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, uint b) {
            return a == (ddouble)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, uint b) {
            return !(a == b);
        }
        #endregion

        #region cmp uint ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(uint a, ddouble b) {
            return (ddouble)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(uint a, ddouble b) {
            return (ddouble)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(uint a, ddouble b) {
            return (ddouble)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(uint a, ddouble b) {
            return (ddouble)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(uint a, ddouble b) {
            return (ddouble)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(uint a, ddouble b) {
            return !(a == b);
        }
        #endregion

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
