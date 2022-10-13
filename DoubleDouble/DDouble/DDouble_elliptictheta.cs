using System;
using System.Collections.Generic;

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
            if (q < 0d || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (q0 < EllipticThetaUtil.Eps) {
                return NaN;
            }

            ddouble sinx = Sin(x), cos2x = Cos(2 * x);
            ddouble qdrt_q = Sqrt(Sqrt(q));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1;
            ddouble u = 2 * q2 * cos2x, v = q4, s = 1d - u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && p > EllipticThetaUtil.Eps) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d - u + v;
            }

            ddouble y = 2 * qdrt_q * q0 * sinx * p;

            return y;
        }

        private static ddouble EllipticTheta2(ddouble x, ddouble q) {
            if (q < 0d || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (q0 < EllipticThetaUtil.Eps) {
                return NaN;
            }

            ddouble cosx = Cos(x), cos2x = Cos(2 * x);
            ddouble qdrt_q = Sqrt(Sqrt(q));

            ddouble q2 = q * q, q4 = q2 * q2, p = 1;
            ddouble u = 2 * q2 * cos2x, v = q4, s = 1d + u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && p > EllipticThetaUtil.Eps) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d + u + v;
            }

            ddouble y = 2 * qdrt_q * q0 * cosx * p;

            return y;
        }

        private static ddouble EllipticTheta3(ddouble x, ddouble q) {
            if (q < 0d || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (q0 < EllipticThetaUtil.Eps) {
                return NaN;
            }

            ddouble cos2x = Cos(2 * x);

            ddouble q2 = q * q, q4 = q2 * q2, p = 1;
            ddouble u = 2 * q * cos2x, v = q2, s = 1d + u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && p > EllipticThetaUtil.Eps) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d + u + v;
            }

            ddouble y = q0 * p;

            return y;
        }

        private static ddouble EllipticTheta4(ddouble x, ddouble q) {
            if (q < 0d || !(q <= 1d)) {
                return NaN;
            }

            ddouble q0 = EllipticThetaUtil.Q0(q);

            if (q0 < EllipticThetaUtil.Eps) {
                return NaN;
            }

            ddouble cos2x = Cos(2 * x);

            ddouble q2 = q * q, q4 = q2 * q2, p = 1;
            ddouble u = 2 * q * cos2x, v = q2, s = 1d - u + v;

            while ((s <= EllipticThetaUtil.NearOne.lower || s >= EllipticThetaUtil.NearOne.upper) && p > EllipticThetaUtil.Eps) {

                p *= s;
                u *= q2;
                v *= q4;

                s = 1d - u + v;
            }

            ddouble y = q0 * p;

            return y;
        }

        internal static class EllipticThetaUtil {
            public static (ddouble upper, ddouble lower) NearOne = (
                (+1, 0, 0x8000000000000000uL, 0x0000000001000000uL),
                (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL)
            );

            public static double Eps = Math.ScaleB(1, -994);

            private static Dictionary<ddouble, ddouble> q0_table = new() {
                { 0, 1 }
            };

            public static ddouble Q0(ddouble q) {
                if (q < 0d || !(q <= 1d)) {
                    return NaN;
                }

                if (q0_table.ContainsKey(q)) {
                    return q0_table[q];
                }

                ddouble q2 = q * q, u = q2, q0 = 1, s = 1d - u;

                while (s <= NearOne.lower && q0 > Eps) {
                    q0 *= s;
                    u *= q2;
                    s = 1d - u;
                }

                q0_table.Add(q, q0);
                return q0;
            }
        }
    }
}