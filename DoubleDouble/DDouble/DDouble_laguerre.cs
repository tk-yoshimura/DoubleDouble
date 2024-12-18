﻿using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble LaguerreL(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the LaguerreL function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.LaguerreL.Table(n);

                ddouble s = coefs[n];

                for (int m = n - 1; m >= 0; m--) {
                    s = s * x + coefs[m];
                }

                s *= TaylorSequence[n];

                return s;
            }

            if (n == 0) {
                return 1d;
            }
            else {
                return 1d - x;
            }
        }

        public static ddouble LaguerreL(int n, ddouble alpha, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the LaguerreL function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (!IsFinite(alpha)) {
                return NaN;
            }

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.AssociatedLaguerreL.Table(n, alpha);

                ddouble s = coefs[n];

                for (int m = n - 1; m >= 0; m--) {
                    s = s * x + coefs[m];
                }

                s *= TaylorSequence[n];

                return s;
            }

            if (n == 0) {
                return 1d;
            }
            else {
                return 1d + alpha - x;
            }
        }

        internal static partial class Consts {
            public static class LaguerreL {
                private static readonly ConcurrentDictionary<int, ReadOnlyCollection<ddouble>> table = [];

                static LaguerreL() {
                    table[0] = new ReadOnlyCollection<ddouble>(new ddouble[] { 1d });
                    table[1] = new ReadOnlyCollection<ddouble>(new ddouble[] { 1d, -1d });
                }

                public static ReadOnlyCollection<ddouble> Table(int n) {
                    if (!table.TryGetValue(n, out ReadOnlyCollection<ddouble> coef)) {
                        coef = GenerateTable(n);
                        table[n] = coef;
                    }

                    return coef;
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 2);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1);

                    ddouble[] p2 = new ddouble[n + 1];

                    p2[0] = p1[0] * (2 * n - 1) - p0[0] * ((n - 1) * (n - 1));

                    for (int m = 1; m < n - 1; m++) {
                        p2[m] = p1[m] * (2 * n - 1) - p1[m - 1] - p0[m] * ((n - 1) * (n - 1));
                    }

                    p2[n - 1] = p1[n - 1] * (2 * n - 1) - p1[n - 2];
                    p2[n] = -p1[n - 1];

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }

            public static class AssociatedLaguerreL {
                private static readonly ConcurrentDictionary<(int n, ddouble alpha), ReadOnlyCollection<ddouble>> table = [];

                public static ReadOnlyCollection<ddouble> Table(int n, ddouble alpha) {
                    if (!table.TryGetValue((n, alpha), out ReadOnlyCollection<ddouble> coef)) {
                        coef = GenerateTable(n, alpha);
                        table[(n, alpha)] = coef;
                    }

                    return coef;
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, ddouble alpha) {
                    if (n == 0) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 1d });
                    }
                    if (n == 1) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 1d + alpha, -1d });
                    }

                    ReadOnlyCollection<ddouble> p0 = Table(n - 2, alpha);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1, alpha);

                    ddouble c0 = ((n - 1) + alpha) * (n - 1);
                    ddouble c1 = ((2 * n - 1) + alpha);

                    ddouble[] p2 = new ddouble[n + 1];

                    p2[0] = p1[0] * c1 - p0[0] * c0;

                    for (int m = 1; m < n - 1; m++) {
                        p2[m] = p1[m] * c1 - p1[m - 1] - p0[m] * c0;
                    }

                    p2[n - 1] = p1[n - 1] * c1 - p1[n - 2];
                    p2[n] = -p1[n - 1];

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}