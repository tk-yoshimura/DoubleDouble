namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Bump(ddouble x) {
            if (x <= 0.00140380859375d) {
                return Zero;
            }
            if (x >= 0.98828125d) {
                return 1d;
            }

            ddouble v = 1d / x - 1d / (1d - x);
            ddouble y = 1d / (Exp(v) + 1d);

            return y;
        }
    }
}