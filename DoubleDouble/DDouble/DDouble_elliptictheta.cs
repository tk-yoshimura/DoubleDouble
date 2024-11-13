using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble EllipticTheta(int a, ddouble x, ddouble q) {
            switch (a) {
                case 1:
                    return EllipticTheta1(x, q);
                case 2:
                    return EllipticTheta2(x, q);
                case 3:
                    return EllipticTheta3(x, q);
                case 4:
                    return EllipticTheta4(x, q);
                default:
                    throw new ArgumentOutOfRangeException(nameof(a));
            }
        }

        private static ddouble EllipticTheta1(ddouble x, ddouble q) {
            if (IsNegative(q) || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (ILogB(q0) <= EllipticThetaUtil.EpsExponent) {
                return NaN;
            }

            ddouble sinx = Sin(x), cos2x = Cos(Ldexp(x, 1));
            ddouble qdrt_q = Sqrt(Sqrt(q));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1d;
            ddouble u = Ldexp(q2, 1) * cos2x, v = q4, s = 1d - u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && ILogB(p) > EllipticThetaUtil.EpsExponent) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d - u + v;
            }

            ddouble y = Ldexp(qdrt_q, 1) * q0 * sinx * p;

            return y;
        }

        private static ddouble EllipticTheta2(ddouble x, ddouble q) {
            if (IsNegative(q) || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (ILogB(q0) <= EllipticThetaUtil.EpsExponent) {
                return NaN;
            }

            ddouble cosx = Cos(x), cos2x = Cos(Ldexp(x, 1));
            ddouble qdrt_q = Sqrt(Sqrt(q));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1d;
            ddouble u = Ldexp(q2, 1) * cos2x, v = q4, s = 1d + u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && ILogB(p) > EllipticThetaUtil.EpsExponent) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d + u + v;
            }

            ddouble y = Ldexp(qdrt_q, 1) * q0 * cosx * p;

            return y;
        }

        private static ddouble EllipticTheta3(ddouble x, ddouble q) {
            if (IsNegative(q) || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (ILogB(q0) <= EllipticThetaUtil.EpsExponent) {
                return NaN;
            }

            ddouble cos2x = Cos(Ldexp(x, 1));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1d;
            ddouble u = Ldexp(q, 1) * cos2x, v = q2, s = 1d + u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && ILogB(p) > EllipticThetaUtil.EpsExponent) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d + u + v;
            }

            ddouble y = q0 * p;

            return y;
        }

        private static ddouble EllipticTheta4(ddouble x, ddouble q) {
            if (IsNegative(q) || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (ILogB(q0) <= EllipticThetaUtil.EpsExponent) {
                return NaN;
            }

            ddouble cos2x = Cos(Ldexp(x, 1));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1d;
            ddouble u = Ldexp(q, 1) * cos2x, v = q2, s = 1d - u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && ILogB(p) > EllipticThetaUtil.EpsExponent) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d - u + v;
            }

            ddouble y = q0 * p;

            return y;
        }

        internal static class EllipticThetaUtil {
            public static readonly (ddouble upper, ddouble lower) NearOne = (
                (+1, 0, 0x8000000000000000uL, 0x0000000001000000uL),
                (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL)
            );

            public const int EpsExponent = -994;

            private static readonly Dictionary<ddouble, ddouble> q0_table = new() {
                { 0d, 1d }
            };

            public static ddouble Q0(ddouble q) {
                Debug.Assert(IsPositive(q) && q <= 1d, nameof(q));

                if (q0_table.TryGetValue(q, out ddouble value)) {
                    return value;
                }

                ddouble q0 = EulerQ(q * q);

                q0_table[q] = q0;
                return q0;
            }
        }
    }
}