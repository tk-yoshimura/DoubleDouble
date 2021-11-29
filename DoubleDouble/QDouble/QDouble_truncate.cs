using System;

namespace DoubleDouble {
    internal partial struct qdouble {
        public static qdouble Floor(qdouble x) {
            ddouble nhi = ddouble.Floor(x.hi), nlo = ddouble.Floor(x.lo);

            return (nhi < x.hi) ? nhi : new qdouble(nhi, nlo);
        }

        public static qdouble Ceiling(qdouble x) {
            ddouble nhi = ddouble.Ceiling(x.hi), nlo = ddouble.Ceiling(x.lo);

            return (nhi > x.hi) ? nhi : new qdouble(nhi, nlo);
        }

        public static qdouble Round(qdouble x) {
            return Floor(x + 0.5d);
        }

        public static qdouble Truncate(qdouble x) {
            return (x < 0) ? Ceiling(x) : Floor(x);
        }

        public static qdouble TruncateMantissa(qdouble x, int keep_bits) {
            if (keep_bits <= 0) {
                throw new ArgumentOutOfRangeException(nameof(keep_bits));
            }
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int exponent = ddouble.Frexp(x.hi).exp;
            ddouble rem = Math.ScaleB(1, exponent - keep_bits);

            ddouble hi = ddouble.Truncate(x.hi / rem) * rem;
            ddouble lo = ddouble.Truncate(x.lo / rem) * rem;

            return new qdouble(hi, lo);
        }

        public static qdouble RoundMantissa(qdouble x, int keep_bits) {
            if (keep_bits <= 0) {
                throw new ArgumentOutOfRangeException(nameof(keep_bits));
            }
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int exponent = ddouble.Frexp(x.hi).exp;
            ddouble rem = Math.ScaleB(1, exponent - keep_bits);

            ddouble hi = ddouble.Round(x.hi / rem) * rem;
            ddouble lo = ddouble.Round(x.lo / rem) * rem;

            return new qdouble(hi, lo);
        }
    }
}
