namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinh(ddouble x) {
            if (IsZero(x)) {
                return IsPositive(x) ? 0d : -0d;
            }

            return Ldexp(Expm1(x) - Expm1(-x), -1);
        }

        public static ddouble Cosh(ddouble x) {
            return Ldexp(Exp(x) + Exp(-x), -1);
        }

        public static ddouble Tanh(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            ddouble abs_x = Abs(x);

            if (abs_x < 18.25d) {
                ddouble x2 = Ldexp(x, 1);
                ddouble x2_expm1 = Expm1(x2), x2_expp1 = Exp(x2) + 1d;

                return x2_expm1 / x2_expp1;
            }
            else if (abs_x < 353.4d) {
                return Sign(x) * (1d - Ldexp(Exp(-Ldexp(abs_x, 1)), 1));
            }
            else {
                return Sign(x);
            }
        }
    }
}
