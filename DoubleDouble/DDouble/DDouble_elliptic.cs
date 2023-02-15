using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble EllipticK(ddouble m) {
            if (!IsFinite(m) || m < -1d || m > 1d) {
                return NaN;
            }

            if (IsZero(m)) {
                return Consts.AsinAcos.HalfPI;
            }

            if (m == 1d) {
                return PositiveInfinity;
            }

            if (m >= 0d) {
                ddouble y = EllipticIntegral.EllipticKCore(m);

                return y;
            }
            else {
                ddouble y = CarlsonRF(0, 1d - m, 1d);

                return y;
            }
        }

        public static ddouble EllipticE(ddouble m) {
            if (!IsFinite(m) || m < -1d || m > 1d) {
                return NaN;
            }

            if (IsZero(m)) {
                return Consts.AsinAcos.HalfPI;
            }

            if (m == 1d) {
                return 1d;
            }

            if (m >= 0d) {
                ddouble y = EllipticIntegral.EllipticECore(m);

                return Max(1, y);
            }
            else {
                ddouble y = 2 * CarlsonRG(0, 1d - m, 1d);

                return y;
            }
        }

        public static ddouble EllipticPi(ddouble n, ddouble m) {
            if (!IsFinite(m) || m < -1d || m > 1d || n > 1d) {
                return NaN;
            }

            if (IsZero(n)) {
                return EllipticK(m);
            }

            if (n == 1d) {
                return PositiveInfinity;
            }

            if (IsZero(m)) {
                return PI / (2 * Sqrt(1d - n));
            }

            if (m == 1d) {
                return PositiveInfinity;
            }

            if (m >= 0d) {
                ddouble y = EllipticIntegral.EllipticPiCore(n, m);

                return y;
            }
            else {
                ddouble y = CarlsonRF(0, 1d - m, 1d) + n / 3d * CarlsonRJ(0, 1d - m, 1d, 1d - n);

                return y;
            }
        }

        public static ddouble EllipticF(ddouble x, ddouble m) {
            ddouble sinx = Sin(x), cosx = Cos(x);
            ddouble sqsinx = Square(sinx), sqcosx = Square(cosx), cmsqsinx = 1d - m * sqsinx;

            return sinx * CarlsonRF(sqcosx, cmsqsinx, 1d);
        }

        public static ddouble EllipticE(ddouble x, ddouble m) {
            ddouble sinx = Sin(x), cosx = Cos(x);
            ddouble sqsinx = Square(sinx), sqcosx = Square(cosx), cmsqsinx = 1d - m * sqsinx;

            return sinx * (CarlsonRF(sqcosx, cmsqsinx, 1d) - m * sqsinx * CarlsonRD(sqcosx, cmsqsinx, 1d) / 3d);
        }

        public static ddouble EllipticPi(ddouble n, ddouble x, ddouble m) {
            ddouble sinx = Sin(x), cosx = Cos(x);
            ddouble sqsinx = Square(sinx), sqcosx = Square(cosx), cmsqsinx = 1d - m * sqsinx;

            return sinx * (CarlsonRF(sqcosx, cmsqsinx, 1d) + n * sqsinx * CarlsonRJ(sqcosx, cmsqsinx, 1d, 1d - n * sqsinx) / 3d);
        }

        internal static class EllipticIntegral {

            public static ddouble EllipticKCore(ddouble m) {
                ddouble y;

                if (m > Math.ScaleB(1, -8)) {
                    ddouble c = Sqrt(1d - m), cp1 = 1d + c, cm1 = 1d - c;

                    y = 2d / cp1 * EllipticKCore(Square(cm1 / cp1));
                }
                else {
                    ddouble x = 1, w = m;

                    for (int i = 1; i < int.MaxValue; i++) {
                        ddouble dx = Consts.Elliptic.KTable(i) * w;
                        ddouble x_next = x + dx;

                        if (x == x_next) {
                            break;
                        }

                        w *= m;
                        x = x_next;
                    }

                    y = x * Consts.AsinAcos.HalfPI;
                }

                return y;
            }

            public static ddouble EllipticECore(ddouble m) {
                ddouble a = 1d;
                ddouble b = Sqrt(1d - m);
                ddouble c = Sqrt(Abs(a * a - b * b));
                ddouble q = 1d;

                for (int n = 0; n < int.MaxValue && !IsZero(c) && a != b; n++) {
                    ddouble squa_c = c * c;
                    ddouble dq = Ldexp(squa_c, n - 1);
                    q -= dq;

                    (a, b) = (
                        (a + b) / 2,
                        Sqrt(a * b)
                    );

                    c = squa_c / (4 * a);
                }

                ddouble y = q * PI / (2 * a);

                return y;
            }

            public static ddouble EllipticPiCore(ddouble n, ddouble m) {
                ddouble a = 1d;
                ddouble b = Sqrt(1d - m);
                ddouble p = Sqrt(1d - n);
                ddouble q = 1d;
                ddouble sum_q = 1d;

                int iters = 0;

                while (Math.Abs(q.hi) > Math.Abs(sum_q.hi) * 8e-31 && iters < 256) {
                    ddouble ab = a * b, p_squa = p * p;
                    ddouble p_squa_pab = p_squa + ab, p_squa_mab = p_squa - ab;

                    (a, b, p, q) = (
                        (a + b) / 2,
                        Sqrt(ab),
                        p_squa_pab / (2 * p),
                        q * p_squa_mab / (2 * p_squa_pab)
                    );

                    sum_q += q;
                    iters++;
                }

                ddouble y = (2d + sum_q * n / (1d - n)) * EllipticK(m) / 2;

                return y;
            }
        }

        internal static partial class Consts {
            public static class Elliptic {
                private static readonly List<ddouble> k_table;

                static Elliptic() {
                    k_table = new() { 1 };

#if DEBUG
                    Trace.WriteLine($"Elliptic initialized.");
#endif
                }

                public static ddouble KTable(int n) {
                    for (int i = k_table.Count; i <= n; i++) {
                        ddouble k = k_table.Last() * (4 * i * (i - 1) + 1) / (4 * i * i);

                        k_table.Add(k);
                    }

                    return k_table[n];
                }
            }
        }
    }
}
