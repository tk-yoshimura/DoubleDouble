using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Legendre(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Legendre function, n greater than 64 is not supported."
                );
            }

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.Legendre.Table(n);

                ddouble x2 = x * x;
                ddouble s = coefs[n / 2];

                for (int m = n / 2 - 1; m >= 0; m--) {
                    s = s * x2 + coefs[m];
                }

                if ((n & 1) == 1) {
                    s *= x;
                }

                s = Ldexp(s, -n);

                return s;
            }

            if (n == 0) {
                return 1d;
            }

            if (n == 1) {
                return x;
            }

            throw new ArgumentOutOfRangeException(nameof(n));
        }

        internal static partial class Consts {
            public static class Legendre {
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new Dictionary<int, ReadOnlyCollection<ddouble>>{
                    { 0, new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 })},
                    { 1, new ReadOnlyCollection<ddouble>(new ddouble[]{ 2 })},
                };

                public static ReadOnlyCollection<ddouble> Table(int n) {
                    if (!table.ContainsKey(n)) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n);
                        table.Add(n, coefs);
                    }

                    return table[n];
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 2);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1);

                    ddouble[] p2 = new ddouble[n / 2 + 1];

                    if ((n & 1) == 0) {
                        p2[0] = -p0[0] * checked(4 * n - 4) / n;

                        for (int m = 2; m < n; m += 2) {
                            p2[m / 2] = (p1[(m - 1) / 2] * checked(4 * n - 2) - p0[m / 2] * checked(4 * n - 4)) / n;
                        }
                    }
                    else {
                        for (int m = 1; m < n; m += 2) {
                            p2[m / 2] = (p1[(m - 1) / 2] * checked(4 * n - 2) - p0[m / 2] * checked(4 * n - 4)) / n;
                        }
                    }

                    p2[n / 2] = p1[(n - 1) / 2] * checked(4 * n - 2) / n;

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}