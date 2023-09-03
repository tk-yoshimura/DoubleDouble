namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Floor(ddouble x) {
            double nhi = double.Floor(x.hi), nlo = double.Floor(x.lo);

            return (nhi < x.hi) ? nhi : new ddouble(nhi, nlo);
        }

        public static ddouble Ceiling(ddouble x) {
            double nhi = double.Ceiling(x.hi), nlo = double.Ceiling(x.lo);

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

            int exponent = double.ILogB(x.hi);
            double rem = double.ScaleB(1, exponent - keep_bits);

            if (!double.IsFinite(rem) || rem == 0) {
                return x;
            }

            double hi = double.Truncate(x.hi / rem) * rem;
            double lo = double.Truncate(x.lo / rem) * rem;

            return new ddouble(hi, lo);
        }

        public static ddouble RoundMantissa(ddouble x, int keep_bits) {
            if (keep_bits <= 0) {
                throw new ArgumentOutOfRangeException(nameof(keep_bits));
            }
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int exponent = double.ILogB(x.hi);
            double rem = double.ScaleB(1, exponent - keep_bits);

            if (!double.IsFinite(rem) || rem == 0) {
                return x;
            }

            double hi = double.Round(x.hi / rem) * rem;
            double lo = double.Round(x.lo / rem) * rem;

            return new ddouble(hi, lo);
        }
    }
}
