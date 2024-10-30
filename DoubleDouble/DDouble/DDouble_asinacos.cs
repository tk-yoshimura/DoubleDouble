using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Asin;
using static DoubleDouble.ddouble.Consts.Atan;
using static DoubleDouble.ddouble.Consts.SinCos;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Atan(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (!IsFinite(x)) {
                return IsPositive(x) ? PiHalf : -PiHalf;
            }
            if (IsNegative(x)) {
                return -Atan(-x);
            }

            if (x > 1d) {
                return PiHalf - Atan(Rcp(x));
            }

            ddouble x2 = x * x;

            if (x > AtanThreshold) {
                return Ldexp(Atan(x / (1d + Sqrt(1d + x2))), 1);
            }

            ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeXZeroCoefTable;

            (ddouble sc, ddouble sd) = table[0];
            for (int i = 1; i < table.Count; i++) {
                (ddouble c, ddouble d) = table[i];

                sc = sc * x2 + c;
                sd = sd * x2 + d;
            }

            Debug.Assert(sd > 0.5d, $"[Atan x={x}] Too small pade denom!!");

            ddouble y = sc * x / sd;

            return y;
        }

        public static ddouble Asin(ddouble x) {
            if (!(x >= -1d && x <= 1d)) {
                return NaN;
            }
            if (x == -1d) {
                return -PiHalf;
            }
            if (x == 1d) {
                return PiHalf;
            }

            ddouble x2 = x * x;

            if (x > -AsinThreshold && x < AsinThreshold) {
                ReadOnlyCollection<ddouble> table = TaylorXZeroCoefTable;

                ddouble s = table[0];
                for (int i = 1; i < table.Count; i++) {
                    s = s * x2 + table[i];
                }

                s = s * x2 + 1d;
                s *= x;

                return s;
            }

            ddouble y = Atan(Sqrt(Rcp(1d - x2) - 1d));

            return IsPositive(x) ? y : -y;
        }

        public static ddouble Acos(ddouble x) {
            return PiHalf - Asin(x);
        }

        public static ddouble Atan2(ddouble y, ddouble x) {
            if (IsZero(x) && IsZero(y)) {
                return IsPositive(y)
                    ? (IsPositive(x) ? 0d : Pi)
                    : (IsPositive(x) ? -0d : -Pi);
            }
            if (!IsFinite(x) || !IsFinite(y)) {
                return NaN;
            }
            if (Abs(x) >= Abs(y)) {
                ddouble yx = y / x;
                return IsPositive(x) ? Atan(yx) : (IsPositive(y) ? (Atan(yx) + Pi) : (Atan(yx) - Pi));
            }
            else {
                ddouble xy = x / y;
                return IsPositive(y) ? (PiHalf - Atan(xy)) : (-PiHalf - Atan(xy));
            }
        }

        public static ddouble AsinPi(ddouble x) => TruncateMantissa(Asin(x) * RcpPi, 105);

        public static ddouble AcosPi(ddouble x) => TruncateMantissa(Acos(x) * RcpPi, 105);

        public static ddouble AtanPi(ddouble x) => TruncateMantissa(Atan(x) * RcpPi, 103);

        public static ddouble Atan2Pi(ddouble y, ddouble x) {
            if (IsZero(x) && IsZero(y)) {
                return IsPositive(y)
                    ? (IsPositive(x) ? 0d : 1d)
                    : (IsPositive(x) ? -0d : -1d);
            }
            if (!IsFinite(x) || !IsFinite(y)) {
                return NaN;
            }
            if (Abs(x) >= Abs(y)) {
                ddouble yx = y / x;
                return IsPositive(x) ? AtanPi(yx) : (IsPositive(y) ? (AtanPi(yx) + 1d) : (AtanPi(yx) - 1d));
            }
            else {
                ddouble xy = x / y;
                return IsPositive(y) ? (0.5d - AtanPi(xy)) : (-0.5d - AtanPi(xy));
            }
        }

        internal static partial class Consts {
            public static class Atan {
                public const double AtanThreshold = 0.25;

                public static readonly ReadOnlyCollection<(ddouble, ddouble)> PadeXZeroCoefTable = new(new (ddouble, ddouble)[] {
                    (21427381364263875L, 21427381364263875L),
                    (91886788553059500L, 99029249007814125L),
                    (163675410390191700L, 192399683786610300L),
                    (156671838074852100L, 204060270682768500L),
                    (87054123957610810L, 128360492848838250L),
                    (28283323008669300L, 48688462804731750L),
                    (5134145876036100L, 10819658401051500L),
                    (463911017673180L, 1298359008126180L),
                    (16016872057515L, 70562989572075L),
                    (90194313216L, 1120047453525L),
                }.Reverse().ToArray());
            }

            public static class Asin {
                public const double AsinThreshold = 0.125;

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
                    (ddouble)46189 / 5505024,
                    (ddouble)88179 / 12058624,
                    (ddouble)676039 / 104857600,
                    (ddouble)1300075 / 226492416,
                    (ddouble)5014575 / 973078528,
                    (ddouble)9694845 / 2080374784,
                    (ddouble)100180065 / 23622320128
                }.Reverse().ToArray());
            }
        }
    }
}
