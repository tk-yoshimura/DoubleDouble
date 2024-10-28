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

        private static ddouble GeometricMean(ddouble a, ddouble b) {
            (long exp_a, a) = AdjustScale(0, a); 
            (long exp_b, b) = AdjustScale(0, b);

            long exp = exp_a + exp_b;

            if ((exp & 1) == 0) {
                return Ldexp(Sqrt(a * b), (int)long.Clamp(-exp / 2, int.MinValue, int.MaxValue));
            }
            else if(exp > 0){
                return Ldexp(Sqrt(a * b * 0.5d), (int)long.Clamp(-exp / 2, int.MinValue, int.MaxValue));
            }
            else {
                return Ldexp(Sqrt(a * b * 2d), (int)long.Clamp(-exp / 2, int.MinValue, int.MaxValue));
            }
        }
    }
}