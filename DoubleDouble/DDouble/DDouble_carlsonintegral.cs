﻿using System;
using static DoubleDouble.ddouble.Consts.CarlsonIntegrals;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble CarlsonRC(ddouble x, ddouble y) {
            if (!(x >= 0d) || !(y >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y)) {
                return Zero;
            }

            bool lessval = x * y * y < 1d;

            const int max_iters = 64;

            ddouble s = 0, mu = 0, invmu = 0;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty) = (
                    Sqrt(x), Sqrt(y)
                );

                ddouble lambda = y + 2 * sqrtx * sqrty;

                (x, y) = (
                    (x + lambda) / 4,
                    (y + lambda) / 4
                );

                if ((i & 1) == 0) {
                    mu = (x + 2 * y) * Rcp3;
                    invmu = Rcp(mu);

                    s = (y + mu) * invmu - 2d;

                    ddouble eps = Abs(s);

                    if (eps < 6e-6 || IsNaN(eps) || eps_prev <= eps) {
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble v = (1d + s * s * (C3d10 + s * (Rcp7 + s * (C3d8 + s * C9d22)))) * Sqrt(invmu);

            if (IsNaN(v)) {
                return lessval ? PositiveInfinity : Zero;
            }

            return v;
        }

        public static ddouble CarlsonRD(ddouble x, ddouble y, ddouble z) {
            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return Zero;
            }

            const int max_iters = 64;

            bool lessval = x * y * z * z * z < 1d;

            ddouble s = 0, exp4 = 1;
            ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                    Sqrt(x), Sqrt(y), Sqrt(z)
                );

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;
                s += exp4 / (sqrtz * (z + lambda));
                exp4 /= 4;

                (x, y, z) = (
                    (x + lambda) / 4,
                    (y + lambda) / 4,
                    (z + lambda) / 4
                );

                if ((i & 1) == 0) {
                    mu = (x + y + 3d * z) * Rcp5;
                    invmu = Rcp(mu);

                    (xd, yd, zd) = (
                        (mu - x) * invmu,
                        (mu - y) * invmu,
                        (mu - z) * invmu
                    );

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd));

                    if (eps < 8e-6 || IsNaN(eps) || eps_prev <= eps) {
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xy = xd * yd, zz = zd * zd;
            ddouble xymzz6 = xy - 6d * zz, xy3mzz8 = 3d * xy - 8 * zz;

            ddouble v1 = xymzz6 * (-C3d14 + xymzz6 * C9d88 - zd * xy3mzz8 * C9d52);
            ddouble v2 = zd * (xy3mzz8 * Rcp6 + zd * (zd * xy * C3d26 - (xy - zz) * C9d22));

            ddouble v = 3d * s + exp4 * (1d + v1 + v2) * (invmu * Sqrt(invmu));

            if (IsNaN(v)) {
                return lessval ? PositiveInfinity : Zero;
            }

            return v;
        }

        public static ddouble CarlsonRF(ddouble x, ddouble y, ddouble z) {
            if (y == z) {
                return CarlsonRC(x, y);
            }

            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return Zero;
            }

            if (x == 0d && z == 0d) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            bool lessval = x * y * z < 1d;

            ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                    Sqrt(x), Sqrt(y), Sqrt(z)
                );

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;

                (x, y, z) = (
                    (x + lambda) / 4,
                    (y + lambda) / 4,
                    (z + lambda) / 4
                );

                if ((i & 1) == 0) {
                    mu = (x + y + z) * Rcp3;
                    invmu = Rcp(mu);

                    (xd, yd, zd) = (
                        2d - (mu + x) * invmu,
                        2d - (mu + y) * invmu,
                        2d - (mu + z) * invmu
                    );

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd));

                    if (eps < 1e-5 || IsNaN(eps) || eps_prev <= eps) {
                        i = max_iters;
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xymzz = xd * yd - zd * zd, xyz = xd * yd * zd;

            ddouble v = (1d + xyz * Rcp14 - xymzz * (Rcp10 - xymzz * Rcp24 + xyz * C3d44)) * Sqrt(invmu);

            if (IsNaN(v)) {
                return lessval ? PositiveInfinity : Zero;
            }

            return v;
        }

        public static ddouble CarlsonRJ(ddouble x, ddouble y, ddouble z, ddouble w) {
            if (z == w) {
                return CarlsonRD(x, y, z);
            }

            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d) || !(w >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z) || IsInfinity(w)) {
                return Zero;
            }

            if ((x == 0d && y == 0d) || (y == 0d && z == 0d) || (z == 0d && x == 0d)) {
                return PositiveInfinity;
            }

            const int max_iters = 64;

            bool lessval = x * y * z * w * w < 1d;

            ddouble s = 0, exp4 = 1;
            ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0, wd = 0;
            ddouble eps_prev = NaN;

            for (int i = 0; i <= max_iters; i++) {
                (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                    Sqrt(x), Sqrt(y), Sqrt(z)
                );

                ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;
                ddouble alpha = Square(w * (sqrtx + sqrty + sqrtz) + (sqrtx * sqrty * sqrtz));
                ddouble beta = w * Square(w + lambda);

                s += exp4 * CarlsonRC(alpha, beta);
                exp4 /= 4;

                (x, y, z, w) = (
                    (x + lambda) / 4,
                    (y + lambda) / 4,
                    (z + lambda) / 4,
                    (w + lambda) / 4
                );

                if ((i & 1) == 0) {
                    mu = (x + y + z + 2 * w) * Rcp5;
                    invmu = Rcp(mu);

                    (xd, yd, zd, wd) = (
                        (mu - x) * invmu,
                        (mu - y) * invmu,
                        (mu - z) * invmu,
                        (mu - w) * invmu
                    );

                    ddouble eps = Max(Abs(xd), Abs(yd), Abs(zd), Abs(wd));

                    if (eps < 8e-6 || IsNaN(eps) || eps_prev <= eps) {
                        i = max_iters;
                        break;
                    }
                    eps_prev = eps;
                }
            }

            ddouble xyz = xd * yd * zd, xyyzzx = (xd + zd) * yd + xd * zd, ww = wd * wd;
            ddouble xyyzzxmww3 = xyyzzx - 3d * ww;

            ddouble v1 = xyz * (Rcp6 - wd * (C3d11 - wd * C3d26));
            ddouble v2 = wd * ((xyyzzx - ww) * Rcp3 - xyyzzx * wd * C3d22);
            ddouble v3 = xyyzzxmww3 * (-C3d14 + xyyzzxmww3 * C9d88 - (xyz + 2 * wd * (xyyzzx - ww)) * C9d52);

            ddouble v = 3d * s + exp4 * (1d + v1 + v2 + v3) * (invmu * Sqrt(invmu));

            if (IsNaN(v)) {
                return lessval ? PositiveInfinity : Zero;
            }

            return v;
        }

        public static ddouble CarlsonRG(ddouble x, ddouble y, ddouble z) {
            if (!(x >= 0d) || !(y >= 0d) || !(z >= 0d)) {
                return NaN;
            }
            if (IsInfinity(x) || IsInfinity(y) || IsInfinity(z)) {
                return PositiveInfinity;
            }

            if (Max(x, y) < (z + 1) * Math.ScaleB(1, -105)) {
                return Sqrt(z) / 2;
            }

            ddouble v = (z * CarlsonRF(x, y, z) - (x - z) * (y - z) * Rcp3 * CarlsonRD(x, y, z) + Sqrt(x * y / z)) / 2;

            return v;
        }

        internal static partial class Consts {
            internal static class CarlsonIntegrals {
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