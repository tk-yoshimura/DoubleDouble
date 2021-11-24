namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble v) {
            if (v.hi < 0 || IsNaN(v)) {
                return NaN;
            }
            if (v.hi == 0) {
                return NegativeInfinity;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            (int n, ddouble f) = Frexp(v);

            ddouble y = n;
            ddouble p = 0.5;

            for (int i = 128; i > 0; i--) {
                f *= f;

                if (f >= 2) {
                    y += p;
                    f = Ldexp(f, -1);
                }
                p = Ldexp(p, -1);

                if (y == (y + p) || f == 1) {
                    break;
                }
            }

            return y;
        }

        public static ddouble Log10(ddouble v) {
            return Log2(v) * Consts.Lg2;
        }

        private static partial class Consts {
            public static ddouble Lg2 { private set; get; } = Rcp(Log2(10));
        }
    }
}
