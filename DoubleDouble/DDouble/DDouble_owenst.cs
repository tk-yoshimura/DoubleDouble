using DoubleDouble.Utils;
using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble OwenT(ddouble h, ddouble a) {
            if (a.Sign < 0) {
                return -OwenT(h, -a);
            }
            if (IsNaN(h) || IsNaN(a)) {
                return NaN;
            }

            h = Abs(h);

            if (h <= OwenTIntegrate.Eps) {
                return Atan(a) / (2 * PI);
            }
            if (h > 36d) {
                return Zero;
            }

            if ((double)a >= 11.5380 / Math.Pow((double)h, 0.9892)) {
                return Erfc(h / Sqrt2) / 4;
            }

            if (a <= OwenTIntegrate.Eps) {
                return OwenTIntegrate.NearZeroA(h, a);
            }

            if (a < 4d || h > 0.675d) {
                return OwenTIntegrate.GaussQuadrature(h, a);
            }
            else {
                ddouble c = (1d - Erf(h / Sqrt2) * Erf(h * a / Sqrt2)) / 4;
                ddouble t = OwenTIntegrate.GaussQuadrature(h * a, 1d / a);

                ddouble y = c - t;

                return y;
            }
        }

        internal static class OwenTIntegrate {

            public static readonly double Eps = Math.ScaleB(1, -64);

            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> Legendre45Table;

            static OwenTIntegrate() {
                Legendre45Table = ResourceUnpack.NumTableX2(Resource.GaussIntegralTable)[nameof(Legendre45Table)];
            }

            public static ddouble GaussQuadrature(ddouble h, ddouble a) {
                ddouble h2 = h * h, n_half_h2 = -h2 / 2;

                ddouble ig = Sqrt(PI / 2) / h * Exp(n_half_h2) * Erf(h * a / Sqrt2);

                ddouble x_peak = Sqrt((Sqrt(8d / h2 + 1d) - 1d) / 2);
                ddouble ap = Min(a, x_peak * 2), ad = a - ap;

                ddouble sp = 0, sd = 0;

                for (int k = 0; k < Legendre45Table.Count; k++) {
                    (ddouble x, ddouble w) = Legendre45Table[k];
                    ddouble x_sft = x * ap;

                    ddouble p = x_sft * x_sft;
                    ddouble r = 1d + p;

                    ddouble u = w * Exp(n_half_h2 * r) * p / r;

                    sp += u;
                }

                if (ad > 0) {
                    for (int k = 0; k < Legendre45Table.Count; k++) {
                        (ddouble x, ddouble w) = Legendre45Table[k];
                        ddouble x_sft = x * ad + ap;

                        ddouble p = x_sft * x_sft;
                        ddouble r = 1d + p;

                        ddouble u = w * Exp(n_half_h2 * r) * p / r;

                        sd += u;
                    }
                }

                ddouble y = (ig - sp * ap - sd * ad) / (2 * PI);

                if (y < Epsilon) {
                    return a * Exp(n_half_h2) / (2 * PI);
                }

                return y;
            }

            public static ddouble NearZeroA(ddouble h, ddouble a) {
                ddouble a2 = a * a;
                ddouble h2 = h * h, h4 = h2 * h2, h6 = h2 * h4;

                ddouble p1 = h2 + 2d;
                ddouble p2 = h4 + 4 * p1;
                ddouble p3 = h6 + 6 * p2;

                ddouble s = a * (1680d + (a2 * (-280d * p1 + (a2 * (42d * p2 - a2 * (5d * p3)))))) / 1680d;
                ddouble c = Exp(-h2 / 2) / (2 * PI);

                ddouble y = s * c;

                if (y < Epsilon) {
                    return a * c;
                }

                return y;
            }
        }
    }
}
