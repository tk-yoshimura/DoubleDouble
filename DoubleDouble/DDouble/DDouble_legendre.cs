﻿using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble LegendreP(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the LegendreP function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.LegendreP.Table(n);

                ddouble x2 = x * x;
                ddouble s = coefs[n / 2];

                for (int i = n / 2 - 1; i >= 0; i--) {
                    s = s * x2 + coefs[i];
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
            else {
                return x;
            }
        }

        public static ddouble LegendreP(int n, int m, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the LegendreP function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (x < -1d || x > 1d) {
                return NaN;
            }

            if (m == 0) {
                return LegendreP(n, x);
            }

            if (n < m || -n > m) {
                return 0d;
            }

            ReadOnlyCollection<ddouble> coefs = Consts.LegendreP.Table(n, m);

            ddouble x2 = x * x;
            ddouble s = coefs[^1];

            for (int i = coefs.Count - 2; i >= 0; i--) {
                s = s * x2 + coefs[i];
            }

            int m_abs = int.Abs(m);

            if (((n - m_abs) & 1) == 1) {
                s *= x;
            }

            s = Ldexp(s, -n) * Pow(1d - x2, m_abs / 2);

            if ((m & 1) != 0) {
                s *= Sqrt(1d - x2);
            }

            return s;
        }

        internal static partial class Consts {
            public static class LegendreP {
                private static readonly ConcurrentDictionary<(int n, int m), ReadOnlyCollection<ddouble>> table = [];

                static LegendreP() {
                    table[(0, 0)] = new ReadOnlyCollection<ddouble>(new ddouble[] { 1d });
                    table[(1, 0)] = new ReadOnlyCollection<ddouble>(new ddouble[] { 2d });
                }

                public static ReadOnlyCollection<ddouble> Table(int n) {
                    if (!table.TryGetValue((n, 0), out ReadOnlyCollection<ddouble> coef)) {
                        coef = GenerateTable(n);
                        table[(n, 0)] = coef;
                    }

                    return coef;
                }

                public static ReadOnlyCollection<ddouble> Table(int n, int m) {
                    if (!table.TryGetValue((n, m), out ReadOnlyCollection<ddouble> coef)) {
                        coef = (m != 0) ? GenerateTable(n, m) : GenerateTable(n);
                        table[(n, m)] = coef;
                    }

                    return coef;
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 2);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1);

                    ddouble[] p2 = new ddouble[n / 2 + 1];

                    if ((n & 1) == 0) {
                        p2[0] = -p0[0] * (4 * n - 4) / n;

                        for (int i = 2; i < n; i += 2) {
                            p2[i / 2] = (p1[(i - 1) / 2] * (4 * n - 2) - p0[i / 2] * (4 * n - 4)) / n;
                        }
                    }
                    else {
                        for (int i = 1; i < n; i += 2) {
                            p2[i / 2] = (p1[(i - 1) / 2] * (4 * n - 2) - p0[i / 2] * (4 * n - 4)) / n;
                        }
                    }

                    p2[n / 2] = p1[(n - 1) / 2] * (4 * n - 2) / n;

                    return new ReadOnlyCollection<ddouble>(p2);
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, int m) {
                    int m_abs = int.Abs(m);

                    if (n - 2 >= m_abs) {
                        ReadOnlyCollection<ddouble> p0 = Table(n - 2, m);
                        ReadOnlyCollection<ddouble> p1 = Table(n - 1, m);

                        int c = n - m_abs;

                        ddouble[] p2 = new ddouble[c / 2 + 1];

                        if ((c & 1) == 0) {
                            p2[0] = -p0[0] * (4 * (n + m) - 4) / (n - m);

                            for (int i = 2; i < c; i += 2) {
                                p2[i / 2] = (p1[(i - 1) / 2] * (4 * n - 2) - p0[i / 2] * (4 * (n + m) - 4)) / (n - m);
                            }
                        }
                        else {
                            for (int i = 1; i < c; i += 2) {
                                p2[i / 2] = (p1[(i - 1) / 2] * (4 * n - 2) - p0[i / 2] * (4 * (n + m) - 4)) / (n - m);
                            }
                        }

                        p2[c / 2] = p1[(c - 1) / 2] * (4 * n - 2) / (n - m);

                        return new ReadOnlyCollection<ddouble>(p2);
                    }
                    else if (n - 1 >= m_abs) {
                        ReadOnlyCollection<ddouble> p1 = Table(n - 1, m);

                        ddouble[] p2 = new ddouble[1] {
                            p1[0] * (4 * n - 2) / (n - m)
                        };

                        return new ReadOnlyCollection<ddouble>(p2);
                    }
                    else {
                        if (m > 0) {
                            ReadOnlyCollection<ddouble> p0 = Table(n - 1, m - 1);

                            ddouble[] p1 = new ddouble[p0.Count];

                            for (int i = 0; i < p1.Length; i++) {
                                p1[i] = -p0[i] * (4 * n - 2);
                            }

                            return new ReadOnlyCollection<ddouble>(p1);
                        }
                        else {
                            ReadOnlyCollection<ddouble> p0 = Table(n - 1, m + 1);

                            ddouble[] p1 = new ddouble[p0.Count];

                            for (int i = 0; i < p1.Length; i++) {
                                p1[i] = p0[i] / n;
                            }

                            return new ReadOnlyCollection<ddouble>(p1);
                        }
                    }
                }
            }
        }
    }
}