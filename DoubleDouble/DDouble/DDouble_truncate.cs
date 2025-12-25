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
            if (ddouble.IsNegative(x)) {
                ddouble n = ddouble.Ceiling(x);
                ddouble f = x - n;

                if (f < -0.5) {
                    return n - 1d;
                }
                if (f > -0.5) {
                    return n;
                }

                double nhi_half = double.ScaleB(n.Hi, -1), nlo_half = double.ScaleB(n.Lo, -1);
                bool is_odd = (nhi_half != double.Truncate(nhi_half)) ^ (nlo_half != double.Truncate(nlo_half));

                return is_odd ? n - 1d : n;
            }
            else {
                ddouble n = ddouble.Floor(x);
                ddouble f = x - n;

                if (f > 0.5) {
                    return n + 1d;
                }
                if (f < 0.5) {
                    return n;
                }

                double nhi_half = double.ScaleB(n.Hi, -1), nlo_half = double.ScaleB(n.Lo, -1);
                bool is_odd = (nhi_half != double.Truncate(nhi_half)) ^ (nlo_half != double.Truncate(nlo_half));

                return is_odd ? n + 1d : n;
            }
        }

        public static ddouble Truncate(ddouble x) {
            return IsNegative(x) ? Ceiling(x) : Floor(x);
        }

        public static ddouble TruncateMantissa(ddouble x, int keep_bits) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(keep_bits, nameof(keep_bits));
            if (!IsFinite(x) || IsZero(x)) {
                return x;
            }

            int shifts = ILogB(x) - keep_bits;
            double hi = double.ScaleB(double.Round(double.ScaleB(x.hi, -shifts)), shifts);
            double lo = double.ScaleB(double.Round(double.ScaleB(x.lo, -shifts)), shifts);

            return new ddouble(hi, lo);
        }
    }
}
