﻿using System.Runtime.CompilerServices;

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
            return a < (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, int b) {
            return a > (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, int b) {
            return a <= (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, int b) {
            return a >= (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, int b) {
            return a == (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, int b) {
            return !(a == b);
        }
        #endregion

        #region cmp int ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, ddouble b) {
            return (double)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, ddouble b) {
            return (double)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, ddouble b) {
            return (double)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, ddouble b) {
            return (double)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, ddouble b) {
            return (double)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, ddouble b) {
            return !(a == b);
        }
        #endregion

        #region cmp ddouble uint
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ddouble a, uint b) {
            return a < (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ddouble a, uint b) {
            return a > (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ddouble a, uint b) {
            return a <= (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ddouble a, uint b) {
            return a >= (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ddouble a, uint b) {
            return a == (double)b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ddouble a, uint b) {
            return !(a == b);
        }
        #endregion

        #region cmp uint ddouble
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(uint a, ddouble b) {
            return (double)a < b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(uint a, ddouble b) {
            return (double)a > b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(uint a, ddouble b) {
            return (double)a <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(uint a, ddouble b) {
            return (double)a >= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(uint a, ddouble b) {
            return (double)a == b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(uint a, ddouble b) {
            return !(a == b);
        }
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Min(ddouble a, ddouble b) {
            return (IsNaN(a) || a < b) ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Min(ddouble a, ddouble b, ddouble c) {
            return Min(Min(a, b), c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Min(ddouble a, ddouble b, ddouble c, ddouble d) {
            return Min(Min(a, b), Min(c, d));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Max(ddouble a, ddouble b) {
            return (IsNaN(a) || a > b) ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Max(ddouble a, ddouble b, ddouble c) {
            return Max(Max(a, b), c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Max(ddouble a, ddouble b, ddouble c, ddouble d) {
            return Max(Max(a, b), Max(c, d));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Clamp(ddouble v, ddouble min, ddouble max) {
            if (!(min <= max)) {
                throw new ArgumentException($"{nameof(min)},{nameof(max)}");
            }

            return Min(Max(v, min), max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble BitIncrement(ddouble v) {
            return new ddouble(v.hi, double.BitIncrement(v.lo));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble BitDecrement(ddouble v) {
            return new ddouble(v.hi, double.BitDecrement(v.lo));
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
