namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Arsinh(ddouble x) {
            if (IsNegative(x)) {
                return -Arsinh(Abs(x));
            }

            ddouble y = Log1p(x + (Sqrt(x * x + 1d) - 1d));

            return y;
        }

        public static ddouble Arcosh(ddouble x) {
            if (x == 1d) {
                return 0d;
            }

            ddouble y = Log(x + Sqrt(x * x - 1d));

            return y;
        }

        public static ddouble Artanh(ddouble x) {
            if (IsNaN(x) || x > 1d || x < -1d) {
                return NaN;
            }
            if (IsZero(x)) {
                return IsPositive(x) ? 0d : -0d;
            }

            if (x == -1d) {
                return NegativeInfinity;
            }
            if (x == 1d) {
                return PositiveInfinity;
            }

            ddouble y = Ldexp(Log1p(x) - Log1p(-x), -1);

            return y;
        }
    }
}
