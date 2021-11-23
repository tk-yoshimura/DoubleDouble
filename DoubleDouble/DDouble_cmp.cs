using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace DoubleDouble {
    public partial struct ddouble {
        public static bool operator <(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo < b.lo);
        }

        public static bool operator >(ddouble a, ddouble b) {
            return (b < a);
        }

        public static bool operator <=(ddouble a, ddouble b) {
            return a.hi < b.hi || (a.hi <= b.hi && a.lo <= b.lo);
        }

        public static bool operator >=(ddouble a, ddouble b) {
            return (b <= a);
        }

        public static bool operator ==(ddouble a, ddouble b) {
            return a.hi == b.hi && a.lo == b.lo;
        }

        public static bool operator !=(ddouble a, ddouble b) {
            return !(a == b);
        }

        public static ddouble BitIncrement(ddouble v) {
            return new ddouble(v.hi, Math.BitIncrement(v.lo));
        }

        public static ddouble BitDecrement(ddouble v) {
            return new ddouble(v.hi, Math.BitDecrement(v.lo));
        }

        public override bool Equals(object obj) {
            return obj is ddouble v && this == v;
        }

        public override int GetHashCode() {
            return hi.GetHashCode() ^ lo.GetHashCode();
        }
    }
}
