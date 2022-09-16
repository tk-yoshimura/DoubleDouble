using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble GegenbauerC(int n, ddouble alpha, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the GegenbauerC function, n greater than 64 is not supported."
                );
            }

            if (!IsFinite(alpha)) {
                return NaN;
            }

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.GegenbauerC.Table(n, alpha);

                ddouble x2 = x * x;
                ddouble s = coefs[n / 2];

                for (int i = n / 2 - 1; i >= 0; i--) {
                    s = s * x2 + coefs[i];
                }

                if ((n & 1) == 1) {
                    s *= x;
                }

                return s;
            }

            if (n == 0) {
                return 1d;
            }

            if (n == 1) {
                return 2 * alpha * x;
            }

            throw new ArgumentOutOfRangeException(nameof(n));
        }

        internal static partial class Consts {
            public static class GegenbauerC {
                private static readonly Dictionary<(int n, ddouble alpha), ReadOnlyCollection<ddouble>> table = new();

                public static ReadOnlyCollection<ddouble> Table(int n, ddouble alpha) {
                    if (!table.ContainsKey((n, alpha))) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n, alpha);
                        table.Add((n, alpha), coefs);
                    }

                    return table[(n, alpha)];
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, ddouble alpha) {
                    if (n >= 2) {
                        ReadOnlyCollection<ddouble> p0 = Table(n - 2, alpha);
                        ReadOnlyCollection<ddouble> p1 = Table(n - 1, alpha);

                        ddouble[] p2 = new ddouble[n / 2 + 1];

                        ddouble c0 = n + 2 * alpha - 2d;
                        ddouble c1 = 2 * (n + alpha - 1d);

                        if ((n & 1) == 0) {
                            p2[0] = (-p0[0] * c0) / n;

                            for (int m = 2; m < n; m += 2) {
                                p2[m / 2] = (p1[(m - 1) / 2] * c1 - p0[m / 2] * c0) / n;
                            }
                        }
                        else {
                            for (int m = 1; m < n; m += 2) {
                                p2[m / 2] = (p1[(m - 1) / 2] * c1 - p0[m / 2] * c0) / n;
                            }
                        }

                        p2[n / 2] = (p1[(n - 1) / 2] * c1) / n;

                        return new ReadOnlyCollection<ddouble>(p2);
                    }

                    if (n == 0) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 1 });
                    }
                    else {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 2 * alpha });
                    }
                }
            }
        }
    }
}