namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinh(ddouble x) {
            if (IsZero(x)) {
                return x.Sign > 0 ? PlusZero : MinusZero;
            }

            return (Expm1(x) - Expm1(-x)) / 2;
        }

        public static ddouble Cosh(ddouble x) {
            return (Exp(x) + Exp(-x)) / 2;
        }

        public static ddouble Tanh(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            ddouble x2 = x * 2;
            ddouble x2_expm1 = Expm1(x2), x2_expp1 = Exp(x2) + 1;

            if (IsFinite(x2_expm1) && IsFinite(x2_expp1)) {
                return x2_expm1 / x2_expp1;
            }
            else {
                return 1d;
            }
        }
    }
}
