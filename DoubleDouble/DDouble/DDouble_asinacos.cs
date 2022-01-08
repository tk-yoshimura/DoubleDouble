using System.Collections.ObjectModel;

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
                return 2 * Atan(x / (1d + ddouble.Sqrt(1d + x * x)));
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
            if (x > -0.03125d && x < 0.03125d) {
                ddouble x2 = x * x;

                ddouble s = Consts.Asin.TaylorXZeroCoefTable[^1];
                for (int i = Consts.Asin.TaylorXZeroCoefTable.Count - 2; i >= 0; i--) {
                    s = s * x2 + Consts.Asin.TaylorXZeroCoefTable[i];
                }
                s = s * x2 + 1d;
                s *= x;

                return s;
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

        internal static partial class Consts {
            public static class AsinAcos {
                public static readonly ddouble HalfPI = PI / 2;
            }

            public static class Asin {
                public static ReadOnlyCollection<ddouble> TaylorXZeroCoefTable = new(new ddouble[] {
                    (ddouble)1 / 6,
                    (ddouble)3 / 40,
                    (ddouble)5 / 112,
                    (ddouble)35 / 1152,
                    (ddouble)63 / 2816,
                    (ddouble)231 / 13312,
                    (ddouble)143 / 10240,
                    (ddouble)6435 / 557056,
                    (ddouble)12155 / 1245184,
                    (ddouble)46189 / 5505024
                });
            }
        }
    }
}
