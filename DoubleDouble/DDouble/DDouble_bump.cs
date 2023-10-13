namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Bump(ddouble x) {
            if (x <= 0.00140380859375d) {
                return 0d;
            }
            if (x >= 0.98828125d) {
                return 1d;
            }

            ddouble v = Ldexp(0.5d - x, 1) / (x * (1d - x));
            ddouble y = 1d / (Exp(v) + 1d);

            return y;
        }
    }
}