using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    internal partial struct qdouble {
        public static qdouble Log2(qdouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return NegativeInfinity;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int n, qdouble v) = Frexp(x);

            int index = (int)qdouble.Floor((v - 1) * Consts.Log.Log2TableN);
            qdouble v_offset = 1 + Consts.Log.Log2TableDx * index;
            qdouble y_offset = n + Consts.Log.Log2Table[index];

            qdouble w = v / v_offset - 1, squa_w = w * w, r = Consts.Log.LbE * w;

            qdouble y = y_offset;
            for (int i = 0; i < Consts.Log.LogConvergenceRemTable.Count; i++) {
                qdouble dy = r * ((2 * i + 2) - (2 * i + 1) * w) * Consts.Log.LogConvergenceRemTable[i];
                qdouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                r *= squa_w;
                y = y_next;
            }

            return y;
        }

        public static qdouble Log10(qdouble x) {
            return Log2(x) * Consts.Log.Lg2;
        }

        public static qdouble Log(qdouble x) {
            return Log2(x) * Consts.Log.Ln2;
        }

        private static partial class Consts {
            public static class Log {
                public static readonly qdouble Lg2 = Rcp(3 + Log2Prime(Ldexp(5, -2)));
                public static readonly qdouble Ln2 = GenerateLn2();
                public static readonly qdouble LbE = Rcp(Ln2);
                public static readonly qdouble Lb10 = Rcp(Lg2);

                private static qdouble GenerateLn2() {
                    int n = 3;
                    qdouble x = Rcp(3d);

                    for (int i = 0; i < 256; i++) {
                        qdouble dx = Rcp(n * Pow(3d, n));
                        qdouble x_next = x + dx;

                        if (x == x_next) {
                            break;
                        }

                        n += 2;
                        x = x_next;
                    }

                    qdouble y = Ldexp(x, 1);

                    return y;
                }

                public static readonly IReadOnlyList<qdouble> Log2Table = GenerateLog2Table();

                public static readonly qdouble Log2TableDx = Rcp(Log2Table.Count - 1);

                public static readonly int Log2TableN = Log2Table.Count - 1;

                public static readonly IReadOnlyList<qdouble> LogConvergenceRemTable = GenerateLogConvergenceRemTable();

                public static qdouble[] GenerateLog2Table() {
#if DEBUG
                    Trace.WriteLine($"Log2 initialize.");
#endif

                    const int n = 2048;
                    qdouble dx = Rcp(n);
                    qdouble[] table = new qdouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        qdouble x = 1 + dx * i;
                        table[i] = Log2Prime(x);
                    }

                    return table;
                }

                private static qdouble Log2Prime(qdouble x) {
                    if (!(x >= 1d) || x > 2d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x == 2d) {
                        return 1d;
                    }

                    qdouble y = 0d;
                    qdouble p = Ldexp(1d, -1);

                    for (int i = 256; i > 0; i--) {
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

                private static IReadOnlyList<qdouble> GenerateLogConvergenceRemTable() {
                    const int n = 32;
                    qdouble[] table = new qdouble[n];

                    for (int i = 0; i < n; i++) {
                        table[i] = Rcp(checked((2 * i + 1) * (2 * i + 2)));
                    }

                    return table;
                }
            }
        }
    }
}
