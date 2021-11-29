namespace DoubleDouble {
    internal partial struct qdouble {
        public static qdouble Cbrt(qdouble x) {
            if (x.Sign < 0) {
                return -Cbrt(-x);
            }
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return x.Sign > 0 ? PlusZero : MinusZero;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int x_exponent, qdouble x_frac) = Frexp(x);
            int exponent_rem = (x_exponent >= 0) ? x_exponent % 3 : ((3 - (-x_exponent) % 3) % 3);

            if (exponent_rem != 0) {
                x_frac = Ldexp(x_frac, exponent_rem);
            }

            qdouble a = 1 / ddouble.Cbrt(x_frac.hi);

            qdouble h = 1 - x_frac * a * a * a;
            a *= 1 + h * (27 + h * (18 + h * 14)) * Consts.Rcp81;

            h = 1 - x_frac * a * a * a;
            a *= 1 + h * (27 + h * (18 + h * 14)) * Consts.Rcp81;

            qdouble y = Ldexp(x_frac * a * a, (x_exponent - exponent_rem) / 3);

            return y;
        }

        private static partial class Consts {
            public static qdouble Rcp81 { get; } = Rcp(81);
        }
    }
}
