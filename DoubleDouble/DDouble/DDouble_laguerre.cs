using System;
using System.Collections.Generic;
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
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

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

        internal static partial class Consts {
            public static class LaguerreL {
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new Dictionary<int, ReadOnlyCollection<ddouble>>{
                    { 0, new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 })},
                    { 1, new ReadOnlyCollection<ddouble>(new ddouble[]{ 1, -1 })},
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

                    ddouble[] p2 = new ddouble[n + 1];

                    p2[0] = p1[0] * checked(2 * n - 1) - p0[0] * checked((n - 1) * (n - 1));

                    for (int m = 1; m < n - 1; m++) {
                        p2[m] = p1[m] * checked(2 * n - 1) - p1[m - 1] - p0[m] * checked((n - 1) * (n - 1));
                    }

                    p2[n - 1] = p1[n - 1] * checked(2 * n - 1) - p1[n - 2];
                    p2[n] = -p1[n - 1];

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}