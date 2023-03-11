using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble ZernikeR(int n, int m, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the ZernikeR function, n greater than 64 is not supported."
                );
            }
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (x < 0d || x > 1d) {
                return NaN;
            }

            m = Math.Abs(m);

            if (n < m || ((n - m) & 1) == 1) {
                return Zero;
            }

            if (n == m) {
                return Pow(x, m);
            }

            if (n == m + 2) {
                return Pow(x, m) * (-(m + 1) + x * x * (m + 2));
            }

            ReadOnlyCollection<ddouble> coefs = Consts.ZernikeR.Table(n, m);

            ddouble x2 = x * x;
            ddouble s = coefs[^1];

            for (int i = coefs.Count - 2; i >= 0; i--) {
                s = s * x2 + coefs[i];
            }

            s *= Pow(x, m);

            return s;
        }

        internal static partial class Consts {
            public static class ZernikeR {
                private static readonly Dictionary<(int n, int m), ReadOnlyCollection<ddouble>> table = new();

                public static ReadOnlyCollection<ddouble> Table(int n, int m) {
                    if (!table.ContainsKey((n, m))) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n, m);
                        table.Add((n, m), coefs);
                    }

                    return table[(n, m)];
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, int m) {
                    if (m < 0 || n < m || ((n - m) & 1) == 1) {
                        throw new ArgumentException($"{nameof(n)},{nameof(m)}");
                    }

                    if (n == m) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { 1 });
                    }
                    if (n == m + 2) {
                        return new ReadOnlyCollection<ddouble>(new ddouble[] { -m - 1, m + 2 });
                    }

                    ReadOnlyCollection<ddouble> p0 = Table(n - 4, m);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 2, m);

                    int k = (n - m) / 2;

                    int c0 = (-n * (n + m - 2) * (n - m - 2));
                    int c10 = (-2 * (n - 1) * (m * m + n * (n - 2)));
                    int c11 = (4 * n * (n - 1) * (n - 2));
                    int r = ((n + m) * (n - m) * (n - 2));

                    ddouble[] p2 = new ddouble[k + 1];

                    p2[0] = (p0[0] * c0 + p1[0] * c10) / r;
                    for (int i = 1; i < k - 1; i++) {
                        p2[i] = (p0[i] * c0 + p1[i] * c10 + p1[i - 1] * c11) / r;
                    }
                    p2[k - 1] = (p1[k - 1] * c10 + p1[k - 2] * c11) / r;
                    p2[k] = (p1[k - 1] * c11) / r;

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}