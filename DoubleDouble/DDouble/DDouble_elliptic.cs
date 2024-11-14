using static DoubleDouble.ddouble.Consts.SinCos;

namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble EllipticK(ddouble m) {
            if (!IsFinite(m) || m < -1d || m > 1d) {
                return NaN;
            }

            if (IsZero(m)) {
                return PiHalf;
            }

            if (m == 1d) {
                return PositiveInfinity;
            }

            if (IsPositive(m)) {
                ddouble y = EllipticIntegral.EllipticKCore(m);

                return y;
            }
            else {
                ddouble y = CarlsonRF(0d, 1d - m, 1d);

                return y;
            }
        }

        public static ddouble EllipticE(ddouble m) {
            if (!IsFinite(m) || m < -1d || m > 1d) {
                return NaN;
            }

            if (IsZero(m)) {
                return PiHalf;
            }

            if (m == 1d) {
                return 1d;
            }

            if (IsPositive(m)) {
                ddouble y = EllipticIntegral.EllipticECore(m);

                return Max(1, y);
            }
            else {
                ddouble y = Ldexp(CarlsonRG(0d, 1d - m, 1d), 1);

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
                return Pi / Ldexp(Sqrt(1d - n), 1);
            }

            if (m == 1d) {
                return PositiveInfinity;
            }

            if (IsPositive(m)) {
                ddouble y = EllipticIntegral.EllipticPiCore(n, m);

                return y;
            }
            else {
                ddouble y = CarlsonRF(0d, 1d - m, 1d) + n / 3d * CarlsonRJ(0d, 1d - m, 1d, 1d - n);

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

                if (m > 3.90625e-3) {
                    ddouble c = Sqrt(1d - m), cp1 = 1d + c, cm1 = 1d - c;

                    y = 2d / cp1 * EllipticKCore(Square(cm1 / cp1));
                }
                else {
                    ddouble x = 1d, w = m;

                    for (int i = 1; i < int.MaxValue; i++) {
                        x = SeriesUtil.Add(x, w, Consts.Elliptic.KTable(i), out bool convergence);

                        if (convergence) {
                            break;
                        }

                        w *= m;
                    }

                    y = x * PiHalf;
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

                    (a, b) = (Ldexp(a + b, -1), GeometricMean(a, b));

                    c = squa_c / Ldexp(a, 2);
                }

                ddouble y = q * Pi / Ldexp(a, 1);

                return y;
            }

            public static ddouble EllipticPiCore(ddouble n, ddouble m) {
                ddouble a = 1d;
                ddouble b = Sqrt(1d - m);
                ddouble p = Sqrt(1d - n);
                ddouble q = 1d;
                ddouble sum_q = 1d;

                int iters = 0;

                while (double.Abs(q.hi) > double.Abs(sum_q.hi) * 8e-31 && iters < 256) {
                    ddouble ab = a * b, p_squa = p * p;
                    ddouble p_squa_pab = p_squa + ab, p_squa_mab = p_squa - ab;

                    (a, b, p, q) = (
                        Ldexp(a + b, -1),
                        Sqrt(ab),
                        p_squa_pab / Ldexp(p, 1),
                        q * p_squa_mab / Ldexp(p_squa_pab, 1)
                    );

                    sum_q += q;
                    iters++;
                }

                ddouble y = Ldexp((2d + sum_q * n / (1d - n)) * EllipticK(m), -1);

                return y;
            }
        }

        internal static partial class Consts {
            public static class Elliptic {
                private static readonly List<ddouble> k_table = [1d];

                public static ddouble KTable(int n) {
                    if (n < k_table.Count) {
                        return k_table[n];
                    }

                    lock (k_table) {
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
}
