namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Logit(ddouble x) {
            if (x < 0.375d || x > 0.625d) {
                ddouble y = Log(x / (1d - x));

                return y;
            }
            else {
                ddouble s = 2d * (x - 0.5d);
                ddouble y = Log1p(s) - Log1p(-s);

                return y;
            }
        }

        public static ddouble Expit(ddouble x) {
            ddouble y = 1d / (1d + Exp(-x));

            return y;
        }
    }
}