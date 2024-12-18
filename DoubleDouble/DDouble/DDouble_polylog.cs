﻿using DoubleDouble.Utils;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Polylog(int n, ddouble x) {
            if (n < -4) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Polylog function, n less than -4 is not supported."
                );
            }
            if (n > 8) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Polylog function, n greater than 8 is not supported."
                );
            }

            if (!(x <= 1d)) {
                return NaN;
            }

            if (n >= 2) {
                if (x >= 0.5d) {
                    return PolylogNearOne.Polylog(n, x);
                }
                if (x >= -0.5d) {
                    return PolylogPowerSeries.PolylogNearZero(n, x);
                }
                if (x >= -1.5d) {
                    return PolylogIntegral.Polylog(n, x);
                }
                if (IsNegativeInfinity(x)) {
                    return NegativeInfinity;
                }

                return PolylogPowerSeries.PolylogMinusLimit(n, x);
            }

            return PolylogParticularN.Polylog(n, x);
        }

        private static class PolylogNearOne {
            public static ddouble Polylog(int n, ddouble x) {
                Debug.Assert((x >= 0.5d && x <= 1d), nameof(x));

                if (x > RegardedOneThreshold) {
                    return RiemannZeta(n);
                }


                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble v = Log(x), v2 = v * v;
                ddouble y = Pow(v, n - 1) * TaylorSequence[n - 1] * (HarmonicNumber(n - 1) - Log(-v));
                ddouble u = 1d;

                for (int k = 0; k <= n; k++) {
                    if (k == n - 1) {
                        u *= v;
                        continue;
                    }

                    y = SeriesUtil.Add(y, u, coef[k], out bool convergence);

                    if (convergence) {
                        return y;
                    }

                    u *= v;
                }

                for (int k = n + 1; k < coef.Count - 1; k += 2) {
                    y = SeriesUtil.Add(y, u, coef[k], out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= v2;
                }

                return y;
            }

            private static readonly ddouble RegardedOneThreshold = 1 - (ddouble)double.ScaleB(1, -100);

            public static class CoefTable {
                private static readonly ConcurrentDictionary<int, ReadOnlyCollection<ddouble>> table = [];

                public static ReadOnlyCollection<ddouble> Coef(int n) {
                    if (!table.TryGetValue(n, out ReadOnlyCollection<ddouble> value)) {

                        List<ddouble> coef = new List<ddouble>();
                        coef.Add(RiemannZeta(n));
                        for (int k = 1; k < TaylorSequence.Count; k++) {
                            coef.Add(RiemannZeta(n - k) * TaylorSequence[k]);
                        }

                        value = new ReadOnlyCollection<ddouble>(coef);
                        table[n] = value;
                    }

                    return value;
                }
            }
        }


        private static class PolylogPowerSeries {
            public static ddouble PolylogNearZero(int n, ddouble x) {
                Debug.Assert((x >= -0.5d && x <= 0.5d), nameof(x));

                if (x == 0d) {
                    return x;
                }

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = x;
                ddouble u = x * x, x2 = u;

                for (int i = 1; i < coef.Count - 1; i += 2) {
                    y = SeriesUtil.Add(y, u, coef[i], x * coef[i + 1], out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= x2;
                }

                return y;
            }

            public static ddouble PolylogMinusLimit(int n, ddouble x) {
                Debug.Assert(x <= -1.5d, nameof(x));

                x = -x;

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = mlimit_bias[n](Log(x));
                ddouble u = 1d / x, v = u, v2 = v * v;

                for (int i = 0; i < coef.Count - 1; i += 2) {
                    y = SeriesUtil.Add(y, ((n & 1) == 0) ? -u : u, coef[i], -v * coef[i + 1], out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= v2;
                }

                return -y;
            }

            static ddouble pi2 = Square(Pi);
            static ddouble pi4 = Pow(Pi, 4);
            static ddouble pi6 = Pow(Pi, 6);
            static ddouble pi8 = Pow(Pi, 8);

            static ReadOnlyCollection<Func<ddouble, ddouble>> mlimit_bias = new(new Func<ddouble, ddouble>[] {
               (logx) => throw new NotImplementedException(),
               (logx) => throw new NotImplementedException(),
               (logx) => (pi2 + 3d * Square(logx)) / 6d,
               (logx) => logx * (pi2 + Square(logx)) / 6d,
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (7d * pi4 + logx2 * (30d * pi2 + 15d * logx2)) / 360d;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (7d * pi4 + logx2 * (10d * pi2 + 3d * logx2)) / 360d;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (31d * pi6 + logx2 * (147d * pi4 + logx2 * (105d * pi2 + 21d * logx2))) / 15120d;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (31d * pi6 + logx2 * (49d * pi4 + logx2 * (21d * pi2 + 3d * logx2))) / 15120d;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (127d * pi8 + logx2 * (620d * pi6 + logx2 * (490d * pi4 + logx2 * (140d * pi2 + 15d * logx2)))) / 604800d;
               },
            });

            public static class CoefTable {
                public const int Terms = 201;
                private static readonly ConcurrentDictionary<int, ReadOnlyCollection<ddouble>> table = [];

                public static ReadOnlyCollection<ddouble> Coef(int n) {
                    if (!table.TryGetValue(n, out ReadOnlyCollection<ddouble> value)) {

                        List<ddouble> coef = new List<ddouble>();
                        for (int k = 1; k < Terms; k++) {
                            coef.Add(Pow(k, -n));
                        }

                        value = new ReadOnlyCollection<ddouble>(coef);
                        table[n] = value;
                    }

                    return value;
                }
            }
        }

        private static class PolylogIntegral {

            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> Legendre37Table, Laguerre38Table;

            static PolylogIntegral() {
                Dictionary<string, ReadOnlyCollection<(ddouble, ddouble)>> tables =
                    ResourceUnpack.NumTableX2(Resource.GaussIntegralTable);

                Legendre37Table = tables[nameof(Legendre37Table)];
                Laguerre38Table = tables[nameof(Laguerre38Table)];
            }

            public static ddouble Polylog(int n, ddouble x) {
                Debug.Assert((x >= -1.5d && x <= -0.5d), nameof(x));

                ddouble h = IntegrandPeak(n, -(double)x);
                ddouble r = 1d / -x;

                Func<ddouble, ddouble> polylog_ir =
                (n > 2) ? (t) => {
                    ddouble y = Pow(t, n - 1) / (Exp(t) * r + 1d);

                    return y;
                }
                : (t) => {
                    ddouble y = t / (Exp(t) * r + 1d);

                    return y;
                };

                int iter = Iters[n];
                ddouble ir = 0d, it = 0d;

                for (int k = 0; k < iter; k++) {
                    foreach ((ddouble t, ddouble w) in Legendre37Table) {
                        ir += w * polylog_ir((k + t) * h);
                    }
                }

                ddouble sh = iter * h;

                Func<ddouble, ddouble> polylog_it =
                (n > 2) ? (u) => {
                    ddouble v = u + sh;
                    ddouble y = Pow(v, n - 1) / (r + Exp(-v));

                    return y;
                }
                : (u) => {
                    ddouble v = u + sh;
                    ddouble y = v / (r + Exp(-v));

                    return y;
                };

                foreach ((ddouble t, ddouble w) in Laguerre38Table) {
                    it += w * polylog_it(t);
                }

                ir *= h;
                it *= Exp(-sh);

                ddouble i = ir + it;

                i *= -TaylorSequence[n - 1];

                return i;
            }

            static readonly ReadOnlyCollection<int> Iters = new ReadOnlyCollection<int>(
                new int[] { -1, -1, 7, 4, 3, 2, 2, 2, 2 });

            static double IntegrandPeak(int n, double x) {
                Debug.Assert(n > 0);

                if (n == 1) {
                    return 0;
                }

                double t = n - 1;

                for (int i = 0; i < 8; i++) {
                    double xexp = double.Exp(-t) * x;
                    double d = (n - 1) * (xexp + 1) - t;
                    double dv = (n - 1) * xexp + 1;
                    double dt = d / dv;

                    t += dt;

                    if (double.Abs(dt / t) < 1e-15) {
                        break;
                    }
                }

                return t;
            }
        }

        private static class PolylogParticularN {
            public static ddouble Polylog(int n, ddouble x) {
                Debug.Assert(x < 1d, nameof(x));

                ddouble y;

                if (n == 1) {
                    y = -Log1p(-x);

                    return IsNaN(y) ? NegativeInfinity : y;
                }
                if (n == 0) {
                    y = x / (1d - x);

                    return IsNaN(y) ? -1 : y;
                }
                if (n >= -4) {
                    if (n == -1) {
                        y = x / Square(1d - x);
                    }
                    else if (n == -2) {
                        y = x * (1d + x) / Cube(1d - x);
                    }
                    else if (n == -3) {
                        y = x * (1d + x * (4d + x)) / Pow(1d - x, 4);
                    }
                    else {
                        y = x * (1d + x) * (1d + x * (10d + x)) / Pow(1d - x, 5);
                    }

                    return IsNaN(y) ? 0d : y;
                }

                throw new NotImplementedException();
            }
        }
    }
}