using DoubleDouble.Utils;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble OwenT(ddouble h, ddouble a) {
            if (IsNaN(h) || IsNaN(a)) {
                return NaN;
            }
            if (IsNegative(a)) {
                return -OwenT(h, -a);
            }

            h = Abs(h);

            if (ILogB(h) < OwenTIntegrate.EpsExponent) {
                return Atan(a) / Ldexp(PI, 1);
            }
            if (h > 36d) {
                return 0d;
            }

            if ((double)a >= 11.5380 / double.Pow((double)h, 0.9892)) {
                return Ldexp(Erfc(h / Sqrt2), -2);
            }

            if (ILogB(a) < OwenTIntegrate.EpsExponent) {
                return OwenTIntegrate.NearZeroA(h, a);
            }

            if (a < 4d || h > 0.675d) {
                return OwenTIntegrate.GaussQuadrature(h, a);
            }
            else {
                ddouble c = Ldexp((1d - Erf(h / Sqrt2) * Erf(h * a / Sqrt2)), -2);
                ddouble t = OwenTIntegrate.GaussQuadrature(h * a, 1d / a);

                ddouble y = c - t;

                return y;
            }
        }

        internal static class OwenTIntegrate {

            public const int EpsExponent = -64;

            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> Legendre45Table;

            static OwenTIntegrate() {
                Legendre45Table =
                    ResourceUnpack.NumTableX2(Resource.GaussIntegralTable)[nameof(Legendre45Table)];
            }

            public static ddouble GaussQuadrature(ddouble h, ddouble a) {
                ddouble h2 = h * h, n_half_h2 = -Ldexp(h2, -1);

                ddouble ig = Sqrt(Ldexp(PI, -1)) / h * Exp(n_half_h2) * Erf(h * a / Sqrt2);

                ddouble x_peak = Sqrt(Ldexp(Sqrt(8d / h2 + 1d) - 1d, -1));
                ddouble ap = Min(a, x_peak * 2d), ad = a - ap;

                ddouble sp = 0d, sd = 0d;

                for (int k = 0; k < Legendre45Table.Count; k++) {
                    (ddouble x, ddouble w) = Legendre45Table[k];
                    ddouble x_sft = x * ap;

                    ddouble p = x_sft * x_sft;
                    ddouble r = 1d + p;

                    ddouble u = w * Exp(n_half_h2 * r) * p / r;

                    sp += u;
                }

                if (ad > 0d) {
                    for (int k = 0; k < Legendre45Table.Count; k++) {
                        (ddouble x, ddouble w) = Legendre45Table[k];
                        ddouble x_sft = x * ad + ap;

                        ddouble p = x_sft * x_sft;
                        ddouble r = 1d + p;

                        ddouble u = w * Exp(n_half_h2 * r) * p / r;

                        sd += u;
                    }
                }

                ddouble y = (ig - sp * ap - sd * ad) / Ldexp(PI, 1);

                if (y < Epsilon) {
                    return a * Exp(n_half_h2) / Ldexp(PI, 1);
                }

                return y;
            }

            public static ddouble NearZeroA(ddouble h, ddouble a) {
                ddouble a2 = a * a;
                ddouble h2 = h * h, h4 = h2 * h2, h6 = h2 * h4;

                ddouble p1 = h2 + 2d;
                ddouble p2 = h4 + 4d * p1;
                ddouble p3 = h6 + 6d * p2;

                ddouble s = a * (1680d + (a2 * (-280d * p1 + (a2 * (42d * p2 - a2 * (5d * p3)))))) / 1680d;
                ddouble c = Exp(-Ldexp(h2, -1)) / Ldexp(PI, 1);

                ddouble y = s * c;

                if (y < Epsilon) {
                    return a * c;
                }

                return y;
            }
        }
    }
}
