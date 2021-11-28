using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Floor(ddouble x) {
            double nhi = Math.Floor(x.hi), nlo = Math.Floor(x.lo);

            return (nhi < x.hi) ? nhi : new ddouble(nhi, nlo);
        }

        public static ddouble Ceiling(ddouble x) {
            double nhi = Math.Ceiling(x.hi), nlo = Math.Ceiling(x.lo);

            return (nhi > x.hi) ? nhi : new ddouble(nhi, nlo);
        }

        public static ddouble Round(ddouble x) {
            return Floor(x + 0.5d);
        }

        public static ddouble Truncate(ddouble x) {
            return (x < 0) ? Ceiling(x) : Floor(x);
        }

        public static ddouble TruncateMantissa(ddouble x, int keep_bits) {
            if (keep_bits <= 0) {
                throw new ArgumentOutOfRangeException(nameof(keep_bits));
            }
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int exponent = Math.ILogB(x.hi);
            double rem = Math.ScaleB(1, exponent - keep_bits);

            double hi = Math.Truncate(x.hi / rem) * rem;
            double lo = Math.Truncate(x.lo / rem) * rem;

            return new ddouble(hi, lo);
        }
    }
}
