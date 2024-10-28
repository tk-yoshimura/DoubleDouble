namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Agm(ddouble a, ddouble b) {
            if (IsNegative(a) || IsNegative(b) || IsNaN(a) || IsNaN(b)) {
                return NaN;
            }

            if (IsZero(a) || IsZero(b)) {
                return 0d;
            }

            if (IsInfinity(a) || IsInfinity(b)) {
                return PositiveInfinity;
            }

            int scale;
            (scale, (a, b)) = AdjustScale(exp: 0, (a, b));

            if (IsZero(a) || IsZero(b)) {
                return 0d;
            }

            for (int i = 0; i < 16; i++) {
                (a, b) = (Ldexp(a + b, -1), GeometricMean(a, b));

                if (a == b) {
                    break;
                }
            }

            ddouble y = Ldexp(a, -scale);

            return y;
        }

        public static ddouble GeometricMean(ddouble a, ddouble b) {
            if (IsNaN(a) || IsNaN(b)) {
                return NaN;
            }

            if (IsZero(a) || IsZero(b)) {
                return 0d;
            }

            (int exp_a, a) = AdjustScale(0, a);
            (int exp_b, b) = AdjustScale(0, b);

            int exp = exp_a + exp_b;
            int exp_c = exp / 2;

            b = Ldexp(b, exp_c * 2 - exp);

            ddouble c = Ldexp(Sqrt(a * b), -exp_c);

            return c;
        }

        public static ddouble GeometricMean(ddouble a, ddouble b, ddouble c) {
            if (IsNaN(a) || IsNaN(b) || IsNaN(c)) {
                return NaN;
            }

            if (IsZero(a) || IsZero(b) || IsZero(c)) {
                return 0d;
            }

            (int exp_a, a) = AdjustScale(0, a);
            (int exp_b, b) = AdjustScale(0, b);
            (int exp_c, c) = AdjustScale(0, c);

            int exp = exp_a + exp_b + exp_c;
            int exp_d = exp / 3;

            c = Ldexp(c, exp_d * 3 - exp);

            ddouble d = Ldexp(Cbrt(a * b * c), -exp_d);

            return d;
        }
    }
}