using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Bernoulli(int n, ddouble x, bool centered = false) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Bernoulli function, n greater than 64 is not supported."
                );
            }
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (centered) {
                if (!(x >= -0.5) || (x > 0.5)) {
                    return NaN;
                }
            }
            else {
                if (!(x >= 0d) || (x > 1d)) {
                    return NaN;
                }
            }

            if (n == 0) {
                return 1d;
            }
            else if (n == 1) {
                return centered ? x : x - 0.5d;
            }

            bool invert_flag = false;

            if (centered) {
                if (x < -0.375d || x > 0.375d) {
                    centered = false;
                    x += 0.5d;
                }
            }

            if (!centered) {
                if (x > 0.125d && x < 0.875d) {
                    centered = true;
                    x -= 0.5d;
                }
                else if (x >= 0.875d && (n & 1) == 1) {
                    x = 1d - x;
                    invert_flag = true;
                }
            }

            ReadOnlyCollection<ddouble> coefs = Consts.BernoulliPoly.Table(n, centered);

            if (centered) {
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
            else {
                ddouble s = coefs[n];

                for (int i = n - 1; i >= 0; i--) {
                    s = s * x + coefs[i];
                }

                if (invert_flag) {
                    s = -s;
                }

                return s;
            }
        }

        internal static partial class Consts {
            public static class BernoulliPoly {
                private static readonly List<ReadOnlyCollection<ddouble>> center_table = new List<ReadOnlyCollection<ddouble>>{
                    new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 }),
                    new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 }),
                };
                private static readonly List<ReadOnlyCollection<ddouble>> normal_table = new List<ReadOnlyCollection<ddouble>>{
                    new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 }),
                    new ReadOnlyCollection<ddouble>(new ddouble[]{ -0.5, 1 }),
                };

                public static ReadOnlyCollection<ddouble> Table(int n, bool centered) {
                    List<ReadOnlyCollection<ddouble>> table = centered ? center_table : normal_table;

                    if (n >= table.Count) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n, centered);
                        table.Add(coefs);
                    }

                    return table[n];
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n, bool centered) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 1, centered);

                    ddouble[] p1;

                    if (centered) {
                        p1 = new ddouble[n / 2 + 1];

                        if ((n & 1) == 0) {
                            for (int i = 1; i < n / 2; i++) {
                                p1[i] = p0[i - 1] * n / (i * 2);
                            }
                            p1[0] = (Ldexp(1, -n + 1) - 1d) * BernoulliSequence[n / 2];
                        }
                        else {
                            for (int i = 0; i < n / 2; i++) {
                                p1[i] = p0[i] * n / (i * 2 + 1);
                            }
                        }

                        p1[n / 2] = 1;
                    }
                    else {
                        p1 = new ddouble[n + 1];

                        for (int i = 1; i < n; i++) {
                            p1[i] = p0[i - 1] * n / i;
                        }

                        p1[0] = ((n & 1) == 0) ? BernoulliSequence[n / 2] : 0;
                        p1[n] = 1;
                    }

                    return new ReadOnlyCollection<ddouble>(p1);
                }
            }
        }
    }
}