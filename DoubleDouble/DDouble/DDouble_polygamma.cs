﻿using DoubleDouble.Utils;
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
                ddouble g = Pow(PI, n + 1) * reflecs[n](x);

                ddouble y = -(g + p);

                return y;
            }

            static readonly ReadOnlyCollection<Func<ddouble, ddouble>> reflecs = new(new Func<ddouble, ddouble>[] {
               (x) => 1d / TanPI(x),
               (x) => -1d / Square(SinPI(x)),
               (x) => 2d / (TanPI(x) * Square(SinPI(x))),
               (x) => -2d * (2d + CosPI(2d * x)) / Pow(SinPI(x), 4),
               (x) => 4d * (5d + CosPI(2d * x)) / (TanPI(x) * Pow(SinPI(x), 4)),
               (x) => -2d * (33d + 26d * CosPI(2d * x) + CosPI(4d * x)) / Pow(SinPI(x), 6),
               (x) => 4d * (123d + 56d * CosPI(2d * x) + CosPI(4d * x)) / (TanPI(x) * Pow(SinPI(x), 6)),
               (x) => -2d * (1208d + 1191d * CosPI(2d * x) + 120d * CosPI(4d * x) + CosPI(6d * x)) / Pow(SinPI(x), 8),
               (x) => 4d * (5786d + 4047d * CosPI(2d * x) + 246d * CosPI(4d * x) + CosPI(6d * x)) / (TanPI(x) * Pow(SinPI(x), 8)),
               (x) => -2d * (78095d + 88234d * CosPI(2d * x) + 14608d * CosPI(4d * x) + 502d * CosPI(6d * x) + CosPI(8d * x)) / Pow(SinPI(x), 10),
               (x) => 4d * (450995d + 408364d * CosPI(2d * x) + 46828d * CosPI(4d * x) + 1012d * CosPI(6d * x) + CosPI(8d * x)) / (TanPI(x) * Pow(SinPI(x), 10)),
               (x) => -2d * (7862124d + 9738114d * CosPI(2d * x) + 2203488d * CosPI(4d * x) + 152637d * CosPI(6d * x) + 2036d * CosPI(8d * x) + CosPI(10d * x)) / Pow(SinPI(x), 12),
               (x) => 4d * (52953654d + 56604978d * CosPI(2d * x) + 9713496d * CosPI(4d * x) + 474189d * CosPI(6d * x) + 4082d * CosPI(8d * x) + CosPI(10d * x)) / (TanPI(x) * Pow(SinPI(x), 12)),
               (x) => -2d * (1137586002d + 1505621508d * CosPI(2d * x) + 423281535d * CosPI(4d * x) + 45533450d * CosPI(6d * x) + 1479726d * CosPI(8d * x) + 8178d * CosPI(10d * x) + CosPI(12d * x)) / Pow(SinPI(x), 14),
               (x) => 4d * (8752882782d + 10465410528d * CosPI(2d * x) + 2377852335d * CosPI(4d * x) + 193889840d * CosPI(6d * x) + 4520946d * CosPI(8d * x) + 16368d * CosPI(10d * x) + CosPI(12d * x)) / (TanPI(x) * Pow(SinPI(x), 14)),
               (x) => -2d * (223769408736d + 311387598411d * CosPI(2d * x) + 102776998928d * CosPI(4d * x) + 15041229521d * CosPI(6d * x) + 848090912d * CosPI(8d * x) + 13824739d * CosPI(10d * x) + 32752d * CosPI(12d * x) + CosPI(14d * x)) / Pow(SinPI(x), 16),
               (x) => 4d * (1937789122548d + 2507220680379d * CosPI(2d * x) + 700262497778d * CosPI(4d * x) + 81853020521d * CosPI(6d * x) + 3530218028d * CosPI(8d * x) + 41867227d * CosPI(10d * x) + 65518d * CosPI(12d * x) + CosPI(14d * x)) / (TanPI(x) * Pow(SinPI(x), 16)),
            });
        }
    }
}