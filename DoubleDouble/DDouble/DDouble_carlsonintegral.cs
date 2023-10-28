using static DoubleDouble.ddouble.Consts.CarlsonIntegrals;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble CarlsonRC(ddouble x, ddouble y) {
            if (!(x >= 0d) || !(y >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y)) {
                return 0d;
            }

            int scale;
            (scale, (x, y)) = AdjustScale(exp: 0, (x, y));
            ddouble kappa = Pow2(Ldexp(scale, -1));

            if (y <= Eps) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            ddouble s = 0d, mu = 0d, invmu = 0d;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty) = (
                    Sqrt(x), Sqrt(y)
                );

                ddouble lambda = y + Ldexp(sqrtx * sqrty, 1);

                (x, y) = (Ldexp(x + lambda, -2), Ldexp(y + lambda, -2));

                if ((i & 1) == 0) {
                    mu = (x + Ldexp(y, 1)) * Rcp3;
                    invmu = Rcp(mu);

                    s = (y + mu) * invmu - 2d;

                    ddouble eps = Abs(s);

                    if (eps < 6e-6 || IsNaN(eps) || eps_prev <= eps) {
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble v = kappa * ((1d + s * s * (C3d10 + s * (Rcp7 + s * (C3d8 + s * C9d22)))) * Sqrt(invmu));

            return v;
        }

        public static ddouble CarlsonRD(ddouble x, ddouble y, ddouble z) {
            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return 0d;
            }

            int scale;
            (scale, (x, y, z)) = AdjustScale(exp: 0, (x, y, z));
            ddouble kappa = Pow2(Ldexp(scale * 3, -1));

            if (x <= Eps && y <= Eps && z <= Eps) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            int exp4 = 0;

            ddouble s = 0d;
            ddouble mu = 0d, invmu = 0d, xd = 0d, yd = 0d, zd = 0d;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                    Sqrt(x), Sqrt(y), Sqrt(z)
                );

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;
                s += Ldexp(1d / (sqrtz * (z + lambda)), exp4);
                exp4 -= 2;

                (x, y, z) = (Ldexp(x + lambda, -2), Ldexp(y + lambda, -2), Ldexp(z + lambda, -2));

                if ((i & 1) == 0) {
                    mu = (x + y + 3d * z) * Rcp5;
                    invmu = Rcp(mu);

                    (xd, yd, zd) = ((mu - x) * invmu, (mu - y) * invmu, (mu - z) * invmu);

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd));

                    if (eps < 8e-6 || IsNaN(eps) || eps_prev <= eps) {
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xy = xd * yd, zz = zd * zd;
            ddouble xymzz6 = xy - 6d * zz, xy3mzz8 = 3d * xy - 8d * zz;

            ddouble v1 = xymzz6 * (-C3d14 + xymzz6 * C9d88 - zd * xy3mzz8 * C9d52);
            ddouble v2 = zd * (xy3mzz8 * Rcp6 + zd * (zd * xy * C3d26 - (xy - zz) * C9d22));

            ddouble v = kappa * (3d * s + Ldexp((1d + v1 + v2) * (invmu * Sqrt(invmu)), exp4));

            return v;
        }

        public static ddouble CarlsonRF(ddouble x, ddouble y, ddouble z) {
            if (x == y) {
                return CarlsonRC(z, x);
            }
            if (y == z) {
                return CarlsonRC(x, y);
            }
            if (z == x) {
                return CarlsonRC(y, x);
            }

            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return 0d;
            }

            int scale;
            (scale, (x, y, z)) = AdjustScale(exp: 0, (x, y, z));
            ddouble kappa = Pow2(Ldexp(scale, -1));

            if ((x <= Eps && y <= Eps) || (y <= Eps && z <= Eps) || (z <= Eps && x <= Eps)) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            ddouble mu = 0d, invmu = 0d, xd = 0d, yd = 0d, zd = 0d;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                    Sqrt(x), Sqrt(y), Sqrt(z)
                );

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;

                (x, y, z) = (Ldexp(x + lambda, -2), Ldexp(y + lambda, -2), Ldexp(z + lambda, -2));

                if ((i & 1) == 0) {
                    mu = (x + y + z) * Rcp3;
                    invmu = Rcp(mu);

                    (xd, yd, zd) = (2d - (mu + x) * invmu, 2d - (mu + y) * invmu, 2d - (mu + z) * invmu);

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd));

                    if (eps < 1e-5 || IsNaN(eps) || eps_prev <= eps) {
                        i = max_iters;
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xymzz = xd * yd - zd * zd, xyz = xd * yd * zd;

            ddouble v = kappa * ((1d + xyz * Rcp14 - xymzz * (Rcp10 - xymzz * Rcp24 + xyz * C3d44)) * Sqrt(invmu));

            return v;
        }

        public static ddouble CarlsonRJ(ddouble x, ddouble y, ddouble z, ddouble pho) {
            if (x == pho) {
                return CarlsonRD(y, z, x);
            }
            if (y == pho) {
                return CarlsonRD(z, x, y);
            }
            if (z == pho) {
                return CarlsonRD(x, y, z);
            }

            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d) || !(pho >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z) || IsInfinity(pho)) {
                return 0d;
            }

            int scale;
            (scale, (x, y, z, pho)) = AdjustScale(exp: 0, (x, y, z, pho));
            ddouble kappa = Pow2(Ldexp(scale * 3, -1));

            if ((x <= Eps && y <= Eps) || (y <= Eps && z <= Eps) || (z <= Eps && x <= Eps)) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            int exp4 = 0;

            ddouble s = 0d;
            ddouble mu = 0d, invmu = 0d, xd = 0d, yd = 0d, zd = 0d, phod = 0d;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (Sqrt(x), Sqrt(y), Sqrt(z));

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;
                ddouble alpha = Square(pho * (sqrtx + sqrty + sqrtz) + (sqrtx * sqrty * sqrtz));
                ddouble beta = pho * Square(pho + lambda);

                s += Ldexp(CarlsonRC(alpha, beta), exp4);
                exp4 -= 2;

                (x, y, z, pho) = (Ldexp(x + lambda, -2), Ldexp(y + lambda, -2), Ldexp(z + lambda, -2), Ldexp(pho + lambda, -2));

                if ((i & 1) == 0) {
                    mu = (x + y + z + Ldexp(pho, 1)) * Rcp5;
                    invmu = Rcp(mu);

                    (xd, yd, zd, phod) = ((mu - x) * invmu, (mu - y) * invmu, (mu - z) * invmu, (mu - pho) * invmu);

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd), Abs(phod));

                    if (eps < 8e-6 || IsNaN(eps) || eps_prev <= eps) {
                        i = max_iters;
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xyz = xd * yd * zd, xyyzzx = (xd + zd) * yd + xd * zd, pho2 = phod * phod;
            ddouble xyyzzxmpho2x3 = xyyzzx - 3d * pho2;

            ddouble v1 = xyz * (Rcp6 - phod * (C3d11 - phod * C3d26));
            ddouble v2 = phod * ((xyyzzx - pho2) * Rcp3 - xyyzzx * phod * C3d22);
            ddouble v3 = xyyzzxmpho2x3 * (-C3d14 + xyyzzxmpho2x3 * C9d88 - (xyz + 2d * phod * (xyyzzx - pho2)) * C9d52);

            ddouble v = kappa * (3d * s + Ldexp((1d + v1 + v2 + v3) * (invmu * Sqrt(invmu)), exp4));

            return v;
        }

        public static ddouble CarlsonRG(ddouble x, ddouble y, ddouble z) {
            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return PositiveInfinity;
            }

            int scale;
            (scale, (x, y, z)) = AdjustScale(exp: 0, (x, y, z));
            ddouble kappa = Pow2(-Ldexp(scale, -1));

            if (x <= Eps && y <= Eps && z <= Eps) {
                return 0d;
            }

            if (Max(x, y) <= RGLimitEps) {
                return Ldexp(kappa * Sqrt(z), -1);
            }
            if (Max(y, z) <= RGLimitEps) {
                return Ldexp(kappa * Sqrt(x), -1);
            }
            if (Max(z, x) <= RGLimitEps) {
                return Ldexp(kappa * Sqrt(y), -1);
            }

            if (x <= RGLimitEps) {
                return kappa * CarlsonRG(y, z);
            }
            if (y <= RGLimitEps) {
                return kappa * CarlsonRG(z, x);
            }
            if (z <= RGLimitEps) {
                return kappa * CarlsonRG(x, y);
            }

            ddouble v = Ldexp(z * CarlsonRF(x, y, z) - (x - z) * (y - z) * Rcp3 * CarlsonRD(x, y, z) + Sqrt(x * y / z), -1);
            v *= kappa;

            if (IsNaN(v)) {
                // pinf1 - pinf2, pinf1 >> pinf2
                return PositiveInfinity;
            }

            return v;
        }

        private static ddouble CarlsonRG(ddouble a, ddouble b) {
            return (a < b) ? Sqrt(b) * Ldexp(EllipticE(1d - a / b), -1)
                           : Sqrt(a) * Ldexp(EllipticE(1d - b / a), -1);
        }

        internal static partial class Consts {
            internal static class CarlsonIntegrals {
                public static double Eps = double.ScaleB(1, -1000);
                public static double RGLimitEps = double.ScaleB(1, -105);

                public static ddouble Rcp3 = Rcp(3d);
                public static ddouble Rcp5 = Rcp(5d);
                public static ddouble Rcp6 = Rcp(6d);
                public static ddouble Rcp7 = Rcp(7d);
                public static ddouble Rcp10 = Rcp(10d);
                public static ddouble Rcp14 = Rcp(14d);
                public static ddouble Rcp24 = Rcp(24d);

                public static ddouble C3d8 = (ddouble)3 / 8;
                public static ddouble C3d10 = (ddouble)3 / 10;
                public static ddouble C3d11 = (ddouble)3 / 11;
                public static ddouble C3d14 = (ddouble)3 / 14;
                public static ddouble C3d22 = (ddouble)3 / 22;
                public static ddouble C3d26 = (ddouble)3 / 26;
                public static ddouble C3d44 = (ddouble)3 / 44;
                public static ddouble C9d22 = (ddouble)9 / 22;
                public static ddouble C9d52 = (ddouble)9 / 52;
                public static ddouble C9d88 = (ddouble)9 / 88;
            }
        }
    }
}