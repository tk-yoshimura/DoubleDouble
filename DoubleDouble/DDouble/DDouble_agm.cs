namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Agm(ddouble a, ddouble b) {
            if (IsNegative(a) || IsNegative(b) || IsNaN(a) || IsNaN(b)) {
                return NaN;
            }

            if (IsZero(a) || IsZero(b)) {
                return Zero;
            }

            if (IsInfinity(a) || IsInfinity(b)) {
                return PositiveInfinity;
            }

            int scale;
            (scale, (a, b)) = AdjustScale(exp: 0, (a, b));

            if (IsZero(a) || IsZero(b)) {
                return Zero;
            }

            for (int i = 0; i < 16; i++) {
                (a, b) = (Ldexp(a + b, -1), Sqrt(a * b));

                if (a == b) {
                    break;
                }
            }

            ddouble y = Ldexp(a, -scale);

            return y;
        }
    }
}