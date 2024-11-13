using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble ChebyshevT(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the ChebyshevT function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.ChebyshevT.Table(n);

                ddouble x2 = x * x;
                ddouble s = coefs[n / 2];

                for (int m = n / 2 - 1; m >= 0; m--) {
                    s = s * x2 + coefs[m];
                }

                if ((n & 1) == 1) {
                    s *= x;
                }

                return s;
            }

            if (n == 0) {
                return 1d;
            }
            else {
                return x;
            }
        }

        public static ddouble ChebyshevU(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the ChebyshevU function, n greater than 64 is not supported."
                );
            }
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));

            if (n >= 2) {
                ReadOnlyCollection<ddouble> coefs = Consts.ChebyshevU.Table(n);

                ddouble x2 = x * x;
                ddouble s = coefs[n / 2];

                for (int m = n / 2 - 1; m >= 0; m--) {
                    s = s * x2 + coefs[m];
                }

                if ((n & 1) == 1) {
                    s *= x;
                }

                return s;
            }

            if (n == 0) {
                return 1d;
            }
            else {
                return Ldexp(x, 1);
            }
        }

        internal static partial class Consts {
            public static class ChebyshevT {
                private static readonly ConcurrentDictionary<int, ReadOnlyCollection<ddouble>> table = [];

                static ChebyshevT() {
                    table[0] = new ReadOnlyCollection<ddouble>(new ddouble[]{ 1d });
                    table[1] = new ReadOnlyCollection<ddouble>(new ddouble[]{ 1d });
                }

                public static ReadOnlyCollection<ddouble> Table(int n) {
                    if (!table.TryGetValue(n, out ReadOnlyCollection<ddouble> value)) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n);
                        value = coefs;
                        table[n] = value;
                    }

                    return value;
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 2);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1);

                    ddouble[] p2 = new ddouble[n / 2 + 1];

                    if ((n & 1) == 0) {
                        p2[0] = -p0[0];

                        for (int m = 2; m < n; m += 2) {
                            p2[m / 2] = Ldexp(p1[(m - 1) / 2], 1) - p0[m / 2];
                        }
                    }
                    else {
                        for (int m = 1; m < n; m += 2) {
                            p2[m / 2] = Ldexp(p1[(m - 1) / 2], 1) - p0[m / 2];
                        }
                    }

                    p2[n / 2] = Ldexp(p1[(n - 1) / 2], 1);

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }

            public static class ChebyshevU {
                private static readonly ConcurrentDictionary<int, ReadOnlyCollection<ddouble>> table = [];

                static ChebyshevU() {
                    table[0] = new ReadOnlyCollection<ddouble>(new ddouble[]{ 1d });
                    table[1] = new ReadOnlyCollection<ddouble>(new ddouble[]{ 2d });
                }

                public static ReadOnlyCollection<ddouble> Table(int n) {
                    if (!table.TryGetValue(n, out ReadOnlyCollection<ddouble> value)) {
                        ReadOnlyCollection<ddouble> coefs = GenerateTable(n);
                        value = coefs;
                        table[n] = value;
                    }

                    return value;
                }

                public static ReadOnlyCollection<ddouble> GenerateTable(int n) {
                    ReadOnlyCollection<ddouble> p0 = Table(n - 2);
                    ReadOnlyCollection<ddouble> p1 = Table(n - 1);

                    ddouble[] p2 = new ddouble[n / 2 + 1];

                    if ((n & 1) == 0) {
                        p2[0] = -p0[0];

                        for (int m = 2; m < n; m += 2) {
                            p2[m / 2] = Ldexp(p1[(m - 1) / 2], 1) - p0[m / 2];
                        }
                    }
                    else {
                        for (int m = 1; m < n; m += 2) {
                            p2[m / 2] = Ldexp(p1[(m - 1) / 2], 1) - p0[m / 2];
                        }
                    }

                    p2[n / 2] = Ldexp(p1[(n - 1) / 2], 1);

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}