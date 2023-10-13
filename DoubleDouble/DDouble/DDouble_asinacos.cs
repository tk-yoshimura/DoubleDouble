using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Atan(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (!IsFinite(x)) {
                return IsPositive(x) ? Consts.SinCos.PIHalf : -Consts.SinCos.PIHalf;
            }
            if (IsNegative(x)) {
                return -Atan(-x);
            }

            if (x > 1d) {
                return Consts.SinCos.PIHalf - Atan(Rcp(x));
            }

            ddouble x2 = x * x;

            if (x > 0.25d) {
                return Ldexp(Atan(x / (1d + Sqrt(1d + x2))), 1);
            }

            ddouble f = 86d * x.hi + 13.5d;
            int n = (int)double.Floor(33.5d * x.hi + 9d);

            for (int i = n; i >= 1; i--) {
                f = (2 * i - 1) + (i * i) * x2 / f;
            }

            return x / f;
        }

        public static ddouble Asin(ddouble x) {
            if (!(x >= -1d && x <= 1d)) {
                return NaN;
            }
            if (x == -1d) {
                return -Consts.SinCos.PIHalf;
            }
            if (x == 1d) {
                return Consts.SinCos.PIHalf;
            }
            if (x > -0.03125d && x < 0.03125d) {
                ddouble x2 = x * x;

                ddouble s = Consts.Asin.TaylorXZeroCoefTable[0];
                for (int i = 1; i < Consts.Asin.TaylorXZeroCoefTable.Count; i++) {
                    s = s * x2 + Consts.Asin.TaylorXZeroCoefTable[i];
                }
                s = s * x2 + 1d;
                s *= x;

                return s;
            }

            ddouble y = Atan(Sqrt(Rcp(1d - x * x) - 1d));

            return IsPositive(x) ? y : -y;
        }

        public static ddouble Acos(ddouble x) {
            return Consts.SinCos.PIHalf - Asin(x);
        }

        public static ddouble Atan2(ddouble y, ddouble x) {
            if (IsZero(x) && IsZero(y)) {
                return 0d;
            }
            if (!IsFinite(x) || !IsFinite(y)) {
                return NaN;
            }
            if (Abs(x) >= Abs(y)) {
                ddouble yx = y / x;
                return IsPositive(x) ? Atan(yx) : (IsPositive(y) ? (Atan(yx) + PI) : (Atan(yx) - PI));
            }
            else {
                ddouble xy = x / y;
                return IsPositive(y) ? (Consts.SinCos.PIHalf - Atan(xy)) : (-Consts.SinCos.PIHalf - Atan(xy));
            }
        }

        internal static partial class Consts {
            public static class Asin {
                public static readonly ReadOnlyCollection<ddouble> TaylorXZeroCoefTable = new(new ddouble[] {
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
                }.Reverse().ToArray());
            }
        }
    }
}
