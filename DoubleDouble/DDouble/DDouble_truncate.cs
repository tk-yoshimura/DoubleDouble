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
            return (IsNegative(x)) ? Ceiling(x) : Floor(x);
        }

        public static ddouble TruncateMantissa(ddouble x, int keep_bits) {
            if (keep_bits <= 0) {
                throw new ArgumentOutOfRangeException(nameof(keep_bits));
            }
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int shifts = double.ILogB(x.hi) - keep_bits;
            double hi = double.ScaleB(double.Round(double.ScaleB(x.hi, -shifts)), shifts);
            double lo = double.ScaleB(double.Round(double.ScaleB(x.lo, -shifts)), shifts);

            return new ddouble(hi, lo);
        }
    }
}
