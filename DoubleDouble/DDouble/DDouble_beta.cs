namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Beta(ddouble a, ddouble b) {
            if (!(a > 0d) || !(b > 0d)) {
                return NaN;
            }

            ddouble c = a + b;

            if (c <= 36d) {
                ddouble y = Gamma(a) * Gamma(b) / Gamma(c);

                return y;
            }
            else {
                ddouble y = Exp(LogGamma(a) + LogGamma(b) - LogGamma(c));

                return y;
            }
        }

        public static ddouble LogBeta(ddouble a, ddouble b) {
            if (!(a > 0d) || !(b > 0d)) {
                return NaN;
            }

            ddouble y = LogGamma(a) + LogGamma(b) - LogGamma(a + b);

            return y;
        }
    }
}