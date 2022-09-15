﻿using System;
using System.Collections.Generic;
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

            if (n == 1) {
                return x;
            }

            throw new ArgumentOutOfRangeException(nameof(n));
        }

        public static ddouble ChebyshevU(int n, ddouble x) {
            if (n > 64) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the ChebyshevU function, n greater than 64 is not supported."
                );
            }

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

            if (n == 1) {
                return 2 * x;
            }

            throw new ArgumentOutOfRangeException(nameof(n));
        }

        internal static partial class Consts {
            public static class ChebyshevT {
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new Dictionary<int, ReadOnlyCollection<ddouble>>{
                    { 0, new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 })},
                    { 1, new ReadOnlyCollection<ddouble>(new ddouble[]{ 1 })},
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
                        p2[0] = -p0[0];

                        for (int m = 2; m < n; m += 2) {
                            p2[m / 2] = 2 * p1[(m - 1) / 2] - p0[m / 2];
                        }
                    }
                    else {
                        for (int m = 1; m < n; m += 2) {
                            p2[m / 2] = 2 * p1[(m - 1) / 2] - p0[m / 2];
                        }
                    }

                    p2[n / 2] = 2 * p1[(n - 1) / 2];

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }

            public static class ChebyshevU {
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
                        p2[0] = -p0[0];

                        for (int m = 2; m < n; m += 2) {
                            p2[m / 2] = 2 * p1[(m - 1) / 2] - p0[m / 2];
                        }
                    }
                    else {
                        for (int m = 1; m < n; m += 2) {
                            p2[m / 2] = 2 * p1[(m - 1) / 2] - p0[m / 2];
                        }
                    }

                    p2[n / 2] = 2 * p1[(n - 1) / 2];

                    return new ReadOnlyCollection<ddouble>(p2);
                }
            }
        }
    }
}