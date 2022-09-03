﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Square(ddouble x) => x * x;
        public static ddouble Cube(ddouble x) => x * x * x;

        public static ddouble Pow(ddouble x, long n) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (n == 0) {
                return 1d;
            }

            ulong n_abs = UIntUtil.Abs(n);
            ddouble y = 1d, z = x;

            while (n_abs > 0) {
                if ((n_abs & 1) == 1) {
                    y *= z;
                }

                z *= z;
                n_abs >>= 1;
            }

            return (n > 0) ? y : Rcp(y);
        }

        public static ddouble Pow2(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (IsInfinity(x)) {
                return (x.Sign < 0) ? PlusZero : PositiveInfinity;
            }
            if (x >= 1024d) {
                return PositiveInfinity;
            }
            if (x <= -1024d) {
                return Zero;
            }

            int exp = (int)Floor(x);
            ddouble s = x - exp, c = Ldexp(1d, exp);

            int index = (int)Floor(s * (Consts.Pow.Pow2TableN * Consts.Pow.Pow2TableN));
            ddouble v = s - Consts.Pow.Pow2TableDx * index;
            ddouble r0 = Consts.Pow.Pow2Table[index / Consts.Pow.Pow2TableN];
            ddouble r1 = Consts.Pow.Pow2Table[index % Consts.Pow.Pow2TableN + Consts.Pow.Pow2TableN + 1];

            ddouble w = 1d + v * (Consts.Pow.Pow2C1 + v * (Consts.Pow.Pow2C2 + v * (Consts.Pow.Pow2C3 + v * Consts.Pow.Pow2C4)));

            ddouble y = c * r0 * r1 * w;

            return y;
        }

        public static ddouble Pow(ddouble x, ddouble y) {
            if (x.Sign < 0) {
                return NaN;
            }
            if (IsZero(y)) {
                return IsNaN(x) ? NaN : 1d;
            }
            if (IsZero(x)) {
                return Zero;
            }

            if (y <= long.MinValue) {
                if (x == 1d) {
                    return 1;
                }

                return x < 1 ? PositiveInfinity : Zero;
            }
            if (y >= long.MaxValue) {
                if (x == 1d) {
                    return 1;
                }

                return x < 1 ? Zero : PositiveInfinity;
            }

            long n = (long)Truncate(y);
            ddouble f = y - n;

            ddouble z = Pow(x, n) * Pow2(f * Log2(x));

            return z;
        }

        public static ddouble Pow10(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= -310d) {
                return Zero;
            }
            if (x >= 310d) {
                return PositiveInfinity;
            }

            int n = (int)Truncate(x);
            ddouble f = x - n;

            ddouble pow10n = (n >= 0) ? Consts.Pow.Pow10NTable[n] : (1d / Consts.Pow.Pow10NTable[-n]);

            ddouble z = pow10n * Pow2(f * Lb10);

            return z;
        }

        public static ddouble Exp(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= -710d) {
                return Zero;
            }
            if (x >= 710d) {
                return PositiveInfinity;
            }

            int n = (int)Truncate(x);
            ddouble f = x - n;

            ddouble expn = (n >= 0) ? Consts.Pow.ExpNTable[n] : (1d / Consts.Pow.ExpNTable[-n]);

            ddouble z = expn * Pow2(f * LbE);

            return z;
        }

        public static ddouble Expm1(ddouble x) {
            if (x < -0.09375d || x > 0.1015625d) {
                return Exp(x) - 1d;
            }
            if (IsPlusZero(x)) {
                return PlusZero;
            }
            if (IsMinusZero(x)) {
                return MinusZero;
            }

            ddouble sc = 259459200d + x * (8648640d + x * (8648640d + x * (277200d + x * (55440d + x * (1512d + x * (72d + x * 1d))))));
            ddouble sd = 259459200d + x * (-121080960d + x * (25945920d + x * (-3326400d + x * (277200d + x * (-15120d + x * (504d + x * -8d))))));

            ddouble y = x * sc / sd;

            return y;
        }

        internal static partial class Consts {
            public static class Pow {
                public const int Pow2TableN = 1024;

                public static readonly IReadOnlyList<ddouble> Pow2Table = GeneratePow2Table();
                public static readonly ddouble Pow2TableDx = Rcp(Pow2TableN * Pow2TableN);
                public static readonly ddouble Pow2C1 = Ln2;
                public static readonly ddouble Pow2C2 = Ln2 * Ln2 / 2;
                public static readonly ddouble Pow2C3 = Ln2 * Ln2 * Ln2 / 6;
                public static readonly ddouble Pow2C4 = Ln2 * Ln2 * Ln2 * Ln2 / 24;

                public static readonly IReadOnlyList<ddouble> ExpNTable = GenerateExpNTable();

                public static readonly IReadOnlyList<ddouble> Pow10NTable = GeneratePow10NTable();

                public static ddouble[] GeneratePow2Table() {
                    ddouble dx = Rcp(Pow2TableN), ddx = Rcp(Pow2TableN * Pow2TableN);
                    ddouble[] table = new ddouble[Pow2TableN * 2 + 1];

                    for (int i = 0; i <= Pow2TableN; i++) {
                        ddouble x = dx * i;
                        table[i] = Pow2Prime(x);
                    }
                    for (int i = 0; i < Pow2TableN; i++) {
                        ddouble x = ddx * i;
                        table[i + Pow2TableN + 1] = Pow2Prime(x);
                    }

                    return table;
                }

                public static ddouble[] GenerateExpNTable() {
                    ddouble[] es = new ddouble[10];
                    es[0] = E;
                    for (int i = 1; i < es.Length; i++) {
                        es[i] = es[i - 1] * es[i - 1];
                    }

                    ddouble[] table = new ddouble[711];
                    for (int i = 0; i < table.Length; i++) {
                        int n = i;
                        ddouble y = 1d;

                        for (int j = 0; j < es.Length && n > 0; j++, n >>= 1) {
                            if ((n & 1) == 1) {
                                y *= es[j];
                            }
                        }

                        table[i] = y;
                    }

                    return table;
                }

                public static ddouble[] GeneratePow10NTable() {
                    ddouble[] pow10s = new ddouble[9];
                    pow10s[0] = 10d;
                    for (int i = 1; i < pow10s.Length; i++) {
                        pow10s[i] = pow10s[i - 1] * pow10s[i - 1];
                    }

                    ddouble[] table = new ddouble[310];
                    for (int i = 0; i < table.Length; i++) {
                        int n = i;
                        ddouble y = 1d;

                        for (int j = 0; j < pow10s.Length && n > 0; j++, n >>= 1) {
                            if ((n & 1) == 1) {
                                y *= pow10s[j];
                            }
                        }

                        table[i] = y;
                    }

                    return table;
                }

                private static ddouble Pow2Prime(ddouble x) {
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }
                    if (x == 1d) {
                        return 2;
                    }
                    if (x >= 0.5d) {
                        ddouble value = Pow2Prime(x - 0.5d);

                        return Sqrt2 * value;
                    }

                    ddouble w = x * Ln2, u = w, y = 1d;

                    foreach (ddouble f in TaylorSequence.Skip(1)) {
                        ddouble dy = f * u;
                        ddouble y_next = y + dy;

                        if (y == y_next) {
                            break;
                        }

                        u *= w;
                        y = y_next;
                    }

                    return y;
                }
            }
        }
    }
}
