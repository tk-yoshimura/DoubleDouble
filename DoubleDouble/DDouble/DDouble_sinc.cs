namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinc(ddouble x, bool normalized = true) {
            if (normalized) {
                if (ILogB(x) < -64) {
                    return 1d - Square(x * Pi) / 6d;
                }

                ddouble c = Pi * x;

                if (IsInfinity(c)) {
                    return 0d;
                }

                return SinPi(x) / c;
            }
            else {
                if (ILogB(x) < -64) {
                    return 1d - x * x / 6d;
                }

                if (IsInfinity(x)) {
                    return 0d;
                }

                return Sin(x) / x;
            }
        }

        public static ddouble Sinhc(ddouble x) {
            if (ILogB(x) < -64) {
                return 1d + x * x / 6d;
            }

            ddouble sinh = Sinh(x);

            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            return sinh / x;
        }

        public static ddouble Jinc(ddouble x) {
            if (ILogB(x) < -64) {
                return 0.5d - Ldexp(x * x, -4);
            }

            ddouble x_abs = Abs(x);

            ddouble j1 = BesselJ(1, x_abs);

            return j1 / x_abs;
        }
    }
}