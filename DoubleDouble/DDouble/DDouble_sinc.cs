namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinc(ddouble x, bool normalized = true) {
            if (normalized) {
                if (Abs(x) < double.ScaleB(1, -64)) {
                    return 1d - Square(x * PI) / 6d;
                }

                ddouble c = PI * x;

                if (IsInfinity(c)) {
                    return 0d;
                }

                return SinPI(x) / c;
            }
            else {
                if (Abs(x) < double.ScaleB(1, -64)) {
                    return 1d - x * x / 6d;
                }

                if (IsInfinity(x)) {
                    return 0d;
                }

                return Sin(x) / x;
            }
        }

        public static ddouble Sinhc(ddouble x) {
            if (Abs(x) < double.ScaleB(1, -64)) {
                return 1d + x * x / 6d;
            }

            ddouble sinh = Sinh(x);

            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            return sinh / x;
        }
    }
}