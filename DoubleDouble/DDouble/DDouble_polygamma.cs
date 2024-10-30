using DoubleDouble.Utils;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Polygamma(int n, ddouble x) {
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));
            if (n > 16) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Polygamma function, n greater than 16 is not supported."
                );
            }
            if (n == 0) {
                return Digamma(x);
            }
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsPositiveInfinity(x)) {
                return 0d;
            }

            if (x >= 0d) {
                ddouble y = PolygammaPlusX.Polygamma(n, x);

                return ((n & 1) == 1) ? y : -y;
            }
            else {
                return PolygammaMinusX.Polygamma(n, x);
            }
        }

        private static class PolygammaPlusX {

            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> Legendre40Table, Laguerre40Table;

            static PolygammaPlusX() {
                Dictionary<string, ReadOnlyCollection<(ddouble, ddouble)>> tables =
                    ResourceUnpack.NumTableX2(Resource.GaussIntegralTable);

                Legendre40Table = tables[nameof(Legendre40Table)];
                Laguerre40Table = tables[nameof(Laguerre40Table)];
            }

            public static ddouble Polygamma(int n, ddouble x) {
                if (x <= double.ScaleB(1, -96)) {
                    return ((n & 1) == 1) ? PositiveInfinity : NaN;
                }
                if (x <= 64d) {
                    return PolygammaPlusX.PolygammaNearZero(n, x);
                }
                else {
                    return PolygammaPlusX.PolygammaLimit(n, x);
                }
            }

            public static ddouble PolygammaNearZero(int n, ddouble x) {
                if (x < 1d) {
                    ddouble v = PolygammaNearZero(n, x + 1d);
                    ddouble y = v + Factorial[n] / Pow(x, n + 1);

                    return y;
                }

                ddouble scale = double.Max(1, 8d / n), r = scale * n / x;
                ddouble ir = 0d, it = 0d;

                Func<ddouble, ddouble> polygamma_ir =
                (n > 1) ? (t) => {
                    ddouble y = Pow(t, n) * Exp(-x * t) / (1d - Exp(-t));

                    return y;
                }
                : (t) => {
                    ddouble y = t * Exp(-x * t) / (1d - Exp(-t));

                    return y;
                };

                Func<ddouble, ddouble> polygamma_it =
                (n > 1) ? (u) => {
                    ddouble v = (u + scale * n) / x;
                    ddouble y = Pow(v, n) / (1d - Exp(-v));

                    return y;
                }
                : (u) => {
                    ddouble v = (u + scale) / x;
                    ddouble y = v / (1d - Exp(-v));

                    return y;
                };

                foreach ((ddouble t, ddouble w) in Legendre40Table) {
                    ir += w * polygamma_ir(t * r);
                }
                foreach ((ddouble t, ddouble w) in Laguerre40Table) {
                    it += w * polygamma_it(t);
                }

                ir *= r;
                it *= Exp(-scale * n) / x;

                ddouble i = ir + it;

                return i;
            }

            public static ddouble PolygammaLimit(int n, ddouble x) {
                ddouble u = 1d / (x * x), c = Pow(x, -n);
                ddouble v = c * Factorial[n - 1] * (1d + n / Ldexp(x, 1));
                ddouble w = c * Factorial[n + 1] / 2 * u;
                ddouble dv = BernoulliSequence[1] * w;

                v += dv;

                for (int k = 2; k <= 20; k++) {
                    w *= u * ((n + 2 * k - 2) * (n + 2 * k - 1)) / ((2 * k) * (2 * k - 1));

                    v = SeriesUtil.Add(v, w, BernoulliSequence[k], out bool convergence);

                    if (convergence) {
                        break;
                    }
                }

                return v;
            }
        }

        private static class PolygammaMinusX {
            public static ddouble Polygamma(int n, ddouble x) {
                if (Abs(x - Round(x)) <= double.ScaleB(1, -96)) {
                    return ((n & 1) == 1) ? PositiveInfinity : NaN;
                }

                ddouble p = PolygammaPlusX.Polygamma(n, 1d - x);
                ddouble g = Pow(Pi, n + 1) * reflecs[n](x);

                ddouble y = -(g + p);

                return y;
            }

            static readonly ReadOnlyCollection<Func<ddouble, ddouble>> reflecs = new(new Func<ddouble, ddouble>[] {
               (x) => 1d / TanPi(x),
               (x) => -1d / Square(SinPi(x)),
               (x) => 2d / (TanPi(x) * Square(SinPi(x))),
               (x) => -2d * (2d + CosPi(2d * x)) / Pow(SinPi(x), 4),
               (x) => 4d * (5d + CosPi(2d * x)) / (TanPi(x) * Pow(SinPi(x), 4)),
               (x) => -2d * (33d + 26d * CosPi(2d * x) + CosPi(4d * x)) / Pow(SinPi(x), 6),
               (x) => 4d * (123d + 56d * CosPi(2d * x) + CosPi(4d * x)) / (TanPi(x) * Pow(SinPi(x), 6)),
               (x) => -2d * (1208d + 1191d * CosPi(2d * x) + 120d * CosPi(4d * x) + CosPi(6d * x)) / Pow(SinPi(x), 8),
               (x) => 4d * (5786d + 4047d * CosPi(2d * x) + 246d * CosPi(4d * x) + CosPi(6d * x)) / (TanPi(x) * Pow(SinPi(x), 8)),
               (x) => -2d * (78095d + 88234d * CosPi(2d * x) + 14608d * CosPi(4d * x) + 502d * CosPi(6d * x) + CosPi(8d * x)) / Pow(SinPi(x), 10),
               (x) => 4d * (450995d + 408364d * CosPi(2d * x) + 46828d * CosPi(4d * x) + 1012d * CosPi(6d * x) + CosPi(8d * x)) / (TanPi(x) * Pow(SinPi(x), 10)),
               (x) => -2d * (7862124d + 9738114d * CosPi(2d * x) + 2203488d * CosPi(4d * x) + 152637d * CosPi(6d * x) + 2036d * CosPi(8d * x) + CosPi(10d * x)) / Pow(SinPi(x), 12),
               (x) => 4d * (52953654d + 56604978d * CosPi(2d * x) + 9713496d * CosPi(4d * x) + 474189d * CosPi(6d * x) + 4082d * CosPi(8d * x) + CosPi(10d * x)) / (TanPi(x) * Pow(SinPi(x), 12)),
               (x) => -2d * (1137586002d + 1505621508d * CosPi(2d * x) + 423281535d * CosPi(4d * x) + 45533450d * CosPi(6d * x) + 1479726d * CosPi(8d * x) + 8178d * CosPi(10d * x) + CosPi(12d * x)) / Pow(SinPi(x), 14),
               (x) => 4d * (8752882782d + 10465410528d * CosPi(2d * x) + 2377852335d * CosPi(4d * x) + 193889840d * CosPi(6d * x) + 4520946d * CosPi(8d * x) + 16368d * CosPi(10d * x) + CosPi(12d * x)) / (TanPi(x) * Pow(SinPi(x), 14)),
               (x) => -2d * (223769408736d + 311387598411d * CosPi(2d * x) + 102776998928d * CosPi(4d * x) + 15041229521d * CosPi(6d * x) + 848090912d * CosPi(8d * x) + 13824739d * CosPi(10d * x) + 32752d * CosPi(12d * x) + CosPi(14d * x)) / Pow(SinPi(x), 16),
               (x) => 4d * (1937789122548d + 2507220680379d * CosPi(2d * x) + 700262497778d * CosPi(4d * x) + 81853020521d * CosPi(6d * x) + 3530218028d * CosPi(8d * x) + 41867227d * CosPi(10d * x) + 65518d * CosPi(12d * x) + CosPi(14d * x)) / (TanPi(x) * Pow(SinPi(x), 16)),
            });
        }
    }
}