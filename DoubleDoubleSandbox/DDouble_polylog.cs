using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class PolylogSanbox {
        public static ddouble Polylog(int n, ddouble x) {
            throw new NotImplementedException();
        }

        public static class PolylogNearOne {
            public static ddouble Polylog(int n, ddouble x) {
                if (x < 0.5 || x > 1) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }

                if (x > RegardedOneThreshold) {
                    return RiemannZeta(n);
                }


                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble v = Log(x), v2 = v * v;
                ddouble y = Pow(v, n - 1) * TaylorSequence[n - 1] * (HarmonicNumber(n - 1) - Log(-v)); 
                ddouble u = 1;

                for(int k = 0; k <= n; k++) {
                    if (k == n - 1) {
                        u *= v;
                        continue;
                    }

                    ddouble dy = coef[k] * u;
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        return y;
                    }

                    u *= v;
                    y = y_next;
                }

                for(int k = n + 1; k < coef.Count - 1; k += 2) {
                    ddouble dy = coef[k] * u;
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        return y;
                    }

                    u *= v2;
                    y = y_next;
                }

                return NaN;
            }

            private static readonly ddouble RegardedOneThreshold = 1 - Math.ScaleB(1, -100);

            public static class CoefTable {
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new();
                
                public static ReadOnlyCollection<ddouble> Coef(int n) {
                    if (table.ContainsKey(n)) {
                        return table[n];
                    }
                    
                    List<ddouble> coef = new List<ddouble>();
                    coef.Add(RiemannZeta(n));
                    for (int k = 1; k < TaylorSequence.Count; k++) {
                        coef.Add(RiemannZeta(n - k) * TaylorSequence[k]);
                    }

                    table.Add(n, new ReadOnlyCollection<ddouble>(coef));

                    return table[n];
                }
            }
        }


        public static class PolylogPowerSeries {
            public static ddouble PolylogNearZero(int n, ddouble x) {
                if (x < -0.5 || x > 0.5) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }

                if (x == 0) {
                    return x;
                }

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = x;
                ddouble u = x * x, x2 = u;

                for (int i = 1; i < coef.Count - 1; i += 2) {
                    ddouble dy = u * (coef[i] + x * coef[i + 1]);
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        return y;
                    }

                    y = y_next;
                    u *= x2;
                }

                return NaN;
            }
            
            public static ddouble PolylogMinusLimit(int n, ddouble x) {
                if (x > -1.5) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }

                x = -x;

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = mlimit_bias[n](Log(x));
                ddouble u = 1 / x, v = u, v2 = v * v;

                for (int i = 0; i < coef.Count - 1; i += 2) {
                    ddouble dy = u * (coef[i] - v * coef[i + 1]);
                    ddouble y_next = ((n & 1) == 0) ? (y - dy) : (y + dy);

                    if (y == y_next) {
                        return -y;
                    }

                    y = y_next;
                    u *= v2;
                }

                return NaN;
            }

            static ddouble pi2 = Square(PI);
            static ddouble pi4 = Pow(PI, 4);
            static ddouble pi6 = Pow(PI, 6);
            static ddouble pi8 = Pow(PI, 8);

            static ReadOnlyCollection<Func<ddouble, ddouble>> mlimit_bias = new(new Func<ddouble, ddouble>[] {
               (logx) => throw new NotImplementedException(),
               (logx) => throw new NotImplementedException(),
               (logx) => (pi2 + 3 * Square(logx)) / 6,
               (logx) => logx * (pi2 + Square(logx)) / 6,
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (7 * pi4 + logx2 * (30 * pi2 + 15 * logx2)) / 360;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (7 * pi4 + logx2 * (10 * pi2 + 3 * logx2)) / 360;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (31 * pi6 + logx2 * (147 * pi4 + logx2 * (105 * pi2 + 21 * logx2))) / 15120;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (31 * pi6 + logx2 * (49 * pi4 + logx2 * (21 * pi2 + 3 * logx2))) / 15120;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (127 * pi8 + logx2 * (620 * pi6 + logx2 * (490 * pi4 + logx2 * (140 * pi2 + 15 * logx2)))) / 604800;
               },
            });

            public static class CoefTable {
                public const int Terms = 201;
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new();
                
                public static ReadOnlyCollection<ddouble> Coef(int n) {                    
                    if (table.ContainsKey(n)) {
                        return table[n];
                    }
                    
                    List<ddouble> coef = new List<ddouble>();
                    for (int k = 1; k < Terms; k++) {
                        coef.Add(Pow(k, -n));
                    }

                    table.Add(n, new ReadOnlyCollection<ddouble>(coef));

                    return table[n];
                }
            }
        }
    }
}