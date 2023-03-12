using System;
using System.Runtime.Intrinsics.X86;
using DoubleDouble;
using static DoubleDouble.ddouble;
using static DoubleDouble.ddouble.Consts;

namespace DoubleDoubleSandbox {
    public static class Program {
        static double thr = Math.ScaleB(1, -30);
        static void Main() {
            for (ddouble x = 1; x <= 32; x += 0.5) {
                ddouble minerr = ddouble.MaxValue;
                int minexp = 0;

                for (thr = Math.ScaleB(1, -64); thr < Math.ScaleB(1, -16); thr *= 2) {
                    ddouble eta1 = BesselYEta0(Math.BitIncrement(thr), x);
                    ddouble eta2 = BesselYEta0(thr, x);

                    if (minerr > Abs(eta1 - eta2)) {
                        minerr = Abs(eta1 - eta2);
                        minexp = Math.ILogB(thr);
                    }
                }

                Console.WriteLine($"{x},{minexp},{minerr}");
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
                ddouble ln2 = Ln2, sqln2 = ln2 * ln2, cbln2 = sqln2 * ln2, qdln2 = sqln2 * sqln2, pdln2 = qdln2 * ln2;
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
                    + g * ((sqlnx - ln2 * lnx * 2 + sqln2) * 3 + g * (lnhalfx * 3 + g)) * 4;
                ddouble r3 =
                    (-qdln2 + lnx * (cbln2 * 4 + lnx * (sqln2 * -6 + lnx * (ln2 * 4 - lnx)))) * 16
                    - Zeta3 * (lnhalfx + g) * 32
                    - sqpi * (((sqln2 + lnx * (-ln2 * 2 + lnx) + g * (lnhalfx * 2 + g)) * 8) + sqpi)
                    + g * ((cbln2 + lnx * (sqln2 * -3 + lnx * (ln2 * 3 + -lnx))) * 4
                    + g * ((sqln2 + lnx * (ln2 * -2 + lnx)) * -6
                    + g * (lnhalfx * -4
                    - g))) * 16;
                ddouble r4 =
                    (-pdln2 + lnx * (qdln2 * 5 + lnx * (cbln2 * -10 + lnx * (sqln2 * 10 + lnx * (ln2 * -5 + lnx))))) * 48
                    + Zeta3 * (sqln2 * 12 + lnx * (ln2 * -24 + lnx * 12) + sqpi + g * lnhalfx * 24 + g * g * 12) * 20
                    + Zeta5 * 72
                    + sqpi * ((-cbln2 + lnx * (sqln2 * 3 + lnx * (ln2 * -3 + lnx)) + g * ((sqln2 + lnx * (ln2 * -2 + lnx)) * 3 + g * (lnhalfx * 3 + g))) * 40 + sqpi * (lnhalfx + g) * 7)
                    + g * ((qdln2 + lnx * (cbln2 * -4 + lnx * (sqln2 * 6 + lnx * (ln2 * -4 + lnx)))) * 5
                    + g * ((-cbln2 + lnx * (sqln2 * 3 + lnx * (ln2 * -3 + lnx))) * 10
                    + g * ((sqln2 + lnx * (ln2 * -2 + lnx)) * 10
                    + g * (lnhalfx * 5
                    + g)))) * 48;

                //ddouble eta0 = (r0 * 720 + alpha * (r1 * 180 + alpha * (r2 * 120 + alpha * (r3 * 15 + alpha * r4 * 2)))) / (360 * PI);
                ddouble eta0 = (r0 * 720 + alpha * (r1 * 180 + alpha * (r2 * 120 + alpha * (r3 * 15)))) / (360 * PI);

                return eta0;
            }
        }
    }
}
