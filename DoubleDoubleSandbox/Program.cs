using System;
using System.Runtime.Intrinsics.X86;
using DoubleDouble;
using static DoubleDouble.ddouble;
using static DoubleDouble.ddouble.Consts;

namespace DoubleDoubleSandbox {
    public static class Program {
        static double thr = Math.ScaleB(1, -30);
        static void Main() {

            for (ddouble x = 2; x <= 64; x += 1d / 32) {
                ddouble y = ddouble.BesselY((ddouble)(16) - Math.ScaleB(1, -31), x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }

        static ddouble BesselYEta0(ddouble alpha, ddouble x) {
            if (Math.Abs(alpha.Hi) > thr) {
                ddouble v = 1d / x;

                ddouble s = Pow(2 * v, alpha), sqs = s * s;

                ddouble rcot = 1d / TanPI(alpha), rgamma = Gamma(1 + alpha), rsqgamma = rgamma * rgamma;
                ddouble r = 2 * RcpPI * sqs;
                ddouble p = sqs * rsqgamma * RcpPI;

                ddouble eta0 = rcot - p / alpha;

                return eta0;
            }
            else {
                ddouble lnx = Log(x), lnhalfx = Log(x / 2), sqlnx = lnx * lnx;
                ddouble pi = PI, sqpi = pi * pi;
                ddouble ln2 = Ln2, sqln2 = ln2 * ln2, cbln2 = sqln2 * ln2, qdln2 = sqln2 * sqln2;
                ddouble g = EulerGamma;

                ddouble r0 = lnhalfx + g;
                ddouble r1 =
                    (-sqln2 + lnx * (ln2 * 2 - lnx)) * 4
                    - sqpi
                    - g * (lnhalfx * 2 + g) * 4;
                ddouble r2 =
                    (-cbln2 + lnx * (sqln2 * 3 + lnx * (ln2 * -3 + lnx))) * 4
                    + Zeta3 * 2
                    + sqpi * (lnhalfx + g)
                    + g * ((sqln2 + lnx * (ln2 * -2 + lnx)) * 3 + g * (lnhalfx * 3 + g)) * 4;
                ddouble r3 =
                    (-qdln2 + lnx * (cbln2 * 4 + lnx * (sqln2 * -6 + lnx * (ln2 * 4 - lnx)))) * 16
                    - Zeta3 * (lnhalfx + g) * 32
                    - sqpi * (((sqln2 + lnx * (-ln2 * 2 + lnx) + g * (lnhalfx * 2 + g)) * 8) + sqpi)
                    + g * ((cbln2 + lnx * (sqln2 * -3 + lnx * (ln2 * 3 + -lnx))) * 4
                    + g * ((sqln2 + lnx * (ln2 * -2 + lnx)) * -6
                    + g * (lnhalfx * -4
                    - g))) * 16;

                ddouble eta0 = (r0 * 48 + alpha * (r1 * 12 + alpha * (r2 * 8 + alpha * r3))) / (24 * PI);

                return eta0;
            }
        }

        static ddouble BesselYXi1(ddouble alpha, ddouble x) {
            if (Math.Abs(alpha.Hi) > thr) {
                ddouble v = 1d / x;

                ddouble s = Pow(2 * v, alpha), sqs = s * s;

                ddouble rcot = 1d / TanPI(alpha), rgamma = Gamma(1 + alpha), rsqgamma = rgamma * rgamma;
                ddouble r = 2 * RcpPI * sqs;
                ddouble p = sqs * rsqgamma * RcpPI;

                ddouble xi1 = rcot + p * (alpha * (alpha + 1d) + 1d) / (alpha * (alpha - 1d));

                return xi1;
            }
            else {
                ddouble lnx = Log(x), lnhalfx = Log(x / 2), lnxm1 = lnx - 1, lnhalfxm1 = lnhalfx - 1;
                ddouble pi = PI, sqpi = pi * pi;
                ddouble ln2 = Ln2, sqln2 = ln2 * ln2, cbln2 = sqln2 * ln2, qdln2 = sqln2 * sqln2;
                ddouble g = EulerGamma;

                ddouble r0 = lnhalfxm1 + g;
                ddouble r1 =
                    (-sqln2 + ln2 * lnxm1 * 2 + lnx * (2 - lnx)) * 4
                    - sqpi
                    - g * (lnhalfxm1 * 2 + g) * 4
                    - 6;
                ddouble r2 =
                    (-cbln2 * 4 + sqln2 * lnxm1 * 12 + lnx * (18 + lnx * (-12 + lnx * 4)))
                    + ln2 * (lnx * (2 - lnx) * 12 - 18)
                    + Zeta3 * 2
                    + sqpi * (lnhalfxm1 + g)
                    + g * ((((sqln2 - ln2 * lnxm1 * 2) + lnx * (-2 + lnx)) * 12 + 18)
                    + g * (lnhalfxm1 * 12
                    + g * 4))
                    - 9;
                ddouble r3 =
                    -qdln2 * 16
                    + cbln2 * lnxm1 * 64
                    + sqln2 * (lnx * (2 - lnx) * 96 - 144)
                    + ln2 * (lnx * (9 + lnx * (-6 + lnx * 2)) * 32 - 144)
                    + lnx * (9 + lnx * (-9 + lnx * (4 - lnx))) * 16
                    + Zeta3 * (lnhalfxm1 + g) * -32
                    + sqpi * (((-sqln2 + ln2 * lnxm1 * 2 + lnx * (2 - lnx) - g * (lnhalfxm1 * 2 + g)) * 8 - 12) - sqpi)
                    + g * (((cbln2 - sqln2 * lnxm1 * 3) * 64 + ln2 * (lnx * (-2 + lnx) * 192 + 288) + lnx * (-9 + lnx * (6 - lnx * 2)) * 32 + 144)
                    + g * (((-sqln2 + ln2 * lnxm1 * 2 + lnx * (2 - lnx)) * 96 - 144)
                    + g * (lnhalfxm1 * -64
                    - g * 16)))
                    - 72;

                ddouble xi1 = (r0 * 48 + alpha * (r1 * 12 + alpha * (r2 * 8 + alpha * r3))) / (24 * PI);

                return xi1;
            }
        }
    }
}
