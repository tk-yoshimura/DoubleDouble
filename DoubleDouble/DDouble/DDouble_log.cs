﻿using System;
using System.Collections.Generic;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble v) {
            if (v.hi < 0 || IsNaN(v)) {
                return NaN;
            }
            if (v.hi == 0) {
                return NegativeInfinity;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            (int n, ddouble x) = Frexp(v);

            int index = (int)ddouble.Floor((x - 1) * Consts.Log.Log2TableN);
            ddouble x_offset = 1 + Consts.Log.Log2TableDx * index;
            ddouble y_offset = n + Consts.Log.Log2Table[index];

            ddouble w = x / x_offset - 1, squa_w = w * w, r = Consts.Log.LbE * w;

            ddouble y = y_offset;
            for (int i = 0; i < Consts.Log.Log2ConvergenceRemTable.Count; i++) {
                ddouble dy = r * ((2 * i + 2) - (2 * i + 1) * w) * Consts.Log.Log2ConvergenceRemTable[i];
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                r *= squa_w;
                y = y_next;
            }

            return y;
        }

        public static ddouble Log10(ddouble v) {
            return Log2(v) * Consts.Log.Lg2;
        }

        public static ddouble Log(ddouble v) {
            return Log2(v) * Consts.Log.Ln2;
        }

        private static partial class Consts {
            public static class Log {
                public static readonly ddouble Lg2 = Rcp(3 + Log2(Ldexp(5, -2)));
                public static readonly ddouble Ln2 = GenerateLn2();
                public static readonly ddouble LbE = Rcp(Ln2);

                private static ddouble GenerateLn2() {
                    int n = 3;
                    KahanSum x = Rcp(3);

                    while (!x.IsConvergence) {
                        x.Add(Rcp(n * Pow(3, n)));
                        n += 2;
                    }

                    ddouble y = Ldexp(x.Sum, 1);

                    return y;
                }

                public static readonly IReadOnlyList<ddouble> Log2Table = GenerateLog2Table();

                public static readonly ddouble Log2TableDx = Rcp(Log2Table.Count);

                public static readonly int Log2TableN = Log2Table.Count;

                public static readonly IReadOnlyList<ddouble> Log2ConvergenceRemTable = GenerateLog2ConvergenceRemTable();

                public static ddouble[] GenerateLog2Table() {
                    const int n = 2048;
                    ddouble dx = Rcp(n);
                    ddouble[] table = new ddouble[n];

                    for (int i = 0; i < table.Length; i++) {
                        ddouble x = 1 + dx * i;
                        table[i] = Log2(x);
                    }

                    return table;
                }

                private static ddouble Log2(ddouble x) {
                    if (!(x >= 1) && x < 2) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    ddouble y = 0;
                    ddouble p = Ldexp(1, -1);

                    for (int i = 128; i > 0; i--) {
                        x *= x;

                        if (x >= 2) {
                            y += p;
                            x = Ldexp(x, -1);
                        }
                        p = Ldexp(p, -1);

                        if (y == (y + p) || x == 1) {
                            break;
                        }
                    }

                    return y;
                }

                private static IReadOnlyList<ddouble> GenerateLog2ConvergenceRemTable() {
                    const int n = 16;
                    ddouble[] table = new ddouble[n];

                    for (int i = 0; i < n; i++) {
                        table[i] = Rcp(checked((2 * i + 1) * (2 * i + 2)));
                    }

                    return table;
                }
            }
        }
    }
}
