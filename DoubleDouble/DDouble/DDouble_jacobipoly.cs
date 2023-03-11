using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble JacobiP(int n, ddouble alpha, ddouble beta, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the JacobiP function, n greater than 64 is not supported."
                );
            }
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (!(alpha > -1d && beta > -1d)) {
                return NaN;
            }

            ReadOnlyCollection<ddouble> coefs = Consts.JacobiP.Table(n, alpha, beta);

            ddouble s = coefs[n];

            for (int i = n - 1; i >= 0; i--) {
                s = s * x + coefs[i];
            }

            return s;
        }

        internal static partial class Consts {
            public static class JacobiP {
                private static readonly Dictionary<(int n, ddouble alpha, ddouble beta), ReadOnlyCollection<ddouble>> table = new();

                public static ReadOnlyCollection<ddouble> Table(int n, ddouble alpha, ddouble beta) {
                    if (!table.ContainsKey((n, alpha, beta))) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n, alpha, beta);
                        table.Add((n, alpha, beta), coefs);
                    }

                    return table[(n, alpha, beta)];
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, ddouble alpha, ddouble beta) {
                    if (n >= 2) {
                        ReadOnlyCollection<ddouble> p0 = Table(n - 2, alpha, beta);
                        ReadOnlyCollection<ddouble> p1 = Table(n - 1, alpha, beta);

                        ddouble[] p2 = new ddouble[n + 1];

                        ddouble a = n + alpha, b = n + beta, c = a + b, r = 2 * n * (c - n) * (c - 2);
                        ddouble s = c * (c - 1) * (c - 2), t = (a - b) * (c - 1) * (c - 2 * n), u = 2 * (a - 1) * (b - 1) * c;

                        p2[0] = (p1[0] * t - p0[0] * u) / r;
                        for (int i = 1; i < n - 1; i++) {
                            p2[i] = (p1[i - 1] * s + p1[i] * t - p0[i] * u) / r;
                        }
                        p2[n - 1] = (p1[n - 2] * s + p1[n - 1] * t) / r;
                        p2[n] = (p1[n - 1] * s) / r;

                        return new ReadOnlyCollection<ddouble>(p2);
                    }

                    if (n == 0) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 1 });
                    }
                    else {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { (alpha - beta) / 2, (alpha + beta + 2) / 2 });
                    }
                }
            }
        }
    }
}