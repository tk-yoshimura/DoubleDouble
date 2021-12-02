namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Atan(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (!IsFinite(x)) {
                return x.Sign >= 0 ? Consts.AsinAcos.HalfPI : -Consts.AsinAcos.HalfPI;
            }
            if (x.Sign < 0) {
                return -Atan(-x);
            }

            if (x > 1d) {
                return Consts.AsinAcos.HalfPI - Atan(Rcp(x));
            }
            if (x > 0.25d) {
                return ddouble.Ldexp(Atan(x / (1d + ddouble.Sqrt(1d + x * x))), 1);
            }

            ddouble f = 86d * x + 13.5d;
            int n = (int)ddouble.Floor(33.5d * x + 9d);

            for (int i = n; i >= 1; i--) {
                f = (2 * i - 1) + (i * i) * x * x / f;
            }

            return x / f;
        }

        public static ddouble Asin(ddouble x) {
            if (!(x >= -1d && x <= 1d)) {
                return NaN;
            }
            if (x == -1d) {
                return -Consts.AsinAcos.HalfPI;
            }
            if (x == 1d) {
                return Consts.AsinAcos.HalfPI;
            }

            ddouble y = Atan(Sqrt(Rcp(1d - x * x) - 1d));

            return x.Sign >= 0 ? y : -y;
        }

        public static ddouble Acos(ddouble x) {
            return Consts.AsinAcos.HalfPI - Asin(x);
        }

        public static ddouble Atan2(ddouble y, ddouble x) {
            if (IsZero(x) && IsZero(y)) {
                return Zero;
            }
            if (!IsFinite(x) || !IsFinite(y)) {
                return NaN;
            }
            if (Abs(x) >= Abs(y)) {
                ddouble yx = y / x;
                return x.Sign >= 0 ? Atan(yx) : ((y.Sign >= 0) ? (Atan(yx) + PI) : (Atan(yx) - PI));
            }
            else {
                ddouble xy = x / y;
                return y.Sign >= 0 ? (Consts.AsinAcos.HalfPI - Atan(xy)) : (-Consts.AsinAcos.HalfPI - Atan(xy));
            }
        }

        private static partial class Consts {
            public static class AsinAcos {
                public static readonly ddouble HalfPI = Ldexp(PI, -1);
            }
        }
    }
}
