
using System;
using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompleteEllipticPrototype {
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

            public static ddouble Rc(ddouble x, ddouble y) {
                const int max_iters = 64;

                ddouble s = 0, mu = 0, invmu = 0;

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

                        if (!(eps >= 6e-6)) {
                            break;
                        }
                    }

                    if (i >= max_iters) {
                        throw new ArithmeticException("Carlson integral Rc not convergence.");
                    }
                }

                ddouble v = (1d + s * s * (C3d10 + s * (Rcp7 + s * (C3d8 + s * C9d22)))) * Sqrt(invmu);

                return v;
            }

            public static ddouble Rd(ddouble x, ddouble y, ddouble z) {
                const int max_iters = 64;

                ddouble s = 0, exp4 = 1;
                ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0;

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

                        ddouble eps = Max(Max(Abs(xd), Abs(yd)), Abs(zd));

                        if (!(eps >= 8e-6)) {
                            break;
                        }
                    }

                    if (i >= max_iters) {
                        throw new ArithmeticException("Carlson integral Rd not convergence.");
                    }
                }

                ddouble xy = xd * yd, zz = zd * zd;
                ddouble xymzz6 = xy - 6d * zz, xy3mzz8 = 3d * xy - 8 * zz;

                ddouble v1 = xymzz6 * (-C3d14 + xymzz6 * C9d88 - zd * xy3mzz8 * C9d52);
                ddouble v2 = zd * (xy3mzz8 * Rcp6 + zd * (zd * xy * C3d26 - (xy - zz) * C9d22));

                ddouble v = 3d * s + exp4 * (1d + v1 + v2) * (invmu * Sqrt(invmu));

                return v;
            }

            public static ddouble Rf(ddouble x, ddouble y, ddouble z) {
                const int max_iters = 64;

                ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0;

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

                        ddouble eps = Max(Max(Abs(xd), Abs(yd)), Abs(zd));

                        if (!(eps >= 1e-5)) {
                            break;
                        }
                    }

                    if (i >= max_iters) {
                        throw new ArithmeticException("Carlson integral Rf not convergence.");
                    }
                }

                ddouble xymzz = xd * yd - zd * zd, xyz = xd * yd * zd;

                ddouble v = (1d + xyz * Rcp14 - xymzz * (Rcp10 - xymzz * Rcp24 + xyz * C3d44)) * Sqrt(invmu);

                return v;
            }

            public static ddouble Rj(ddouble x, ddouble y, ddouble z, ddouble p) {
                const int max_iters = 64;

                ddouble s = 0, exp4 = 1;
                ddouble mu = 0, invmu = 0, xd = 0, yd = 0, zd = 0, pd = 0;

                for (int i = 0; i <= max_iters; i++) {
                    (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                        Sqrt(x), Sqrt(y), Sqrt(z)
                    );

                    ddouble lambda = (sqrtx + sqrtz) * sqrty + sqrtx * sqrtz;
                    ddouble alpha = Square(p * (sqrtx + sqrty + sqrtz) + (sqrtx * sqrty * sqrtz));
                    ddouble beta = p * Square(p + lambda);

                    s += exp4 * Rc(alpha, beta);
                    exp4 /= 4;

                    (x, y, z, p) = (
                        (x + lambda) / 4,
                        (y + lambda) / 4,
                        (z + lambda) / 4,
                        (p + lambda) / 4
                    );

                    if ((i & 1) == 0) {
                        mu = (x + y + z + 2 * p) * Rcp5;
                        invmu = Rcp(mu);

                        (xd, yd, zd, pd) = (
                            (mu - x) * invmu,
                            (mu - y) * invmu,
                            (mu - z) * invmu,
                            (mu - p) * invmu
                        );

                        ddouble eps = Max(Max(Abs(xd), Abs(yd)), Max(Abs(zd), Abs(pd)));

                        if (!(eps >= 8e-6)) {
                            break;
                        }
                    }

                    if (i >= max_iters) {
                        throw new ArithmeticException("Carlson integral Rj not convergence.");
                    }
                }

                ddouble xyz = xd * yd * zd, xyyzzx = (xd + zd) * yd + xd * zd, pp = pd * pd;
                ddouble xyyzzxmpp3 = xyyzzx - 3d * pp;

                ddouble v1 = xyz * (Rcp6 - pd * (C3d11 - pd * C3d26));
                ddouble v2 = pd * ((xyyzzx - pp) * Rcp3 - xyyzzx * pd * C3d22);
                ddouble v3 = xyyzzxmpp3 * (-C3d14 + xyyzzxmpp3 * C9d88 - (xyz + 2 * pd * (xyyzzx - pp)) * C9d52);

                ddouble v = 3d * s + exp4 * (1d + v1 + v2 + v3) * (invmu * Sqrt(invmu));

                return v;
            }
        }
    }
}