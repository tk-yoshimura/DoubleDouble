using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return NegativeInfinity;
            }
            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            (int n, ddouble v) = Frexp(x);

            int index = (int)ddouble.Floor((v - 1) * Consts.Log.Log2TableN);
            ddouble v_offset = 1 + Consts.Log.Log2TableDx * index;
            ddouble y_offset = n + Consts.Log.Log2Table[index];

            ddouble w = v / v_offset - 1, squa_w = w * w, r = Consts.Log.LbE * w;

            ddouble y = y_offset;
            for (int i = 0; i < Consts.Log.LogConvergenceRemTable.Count; i++) {
                ddouble dy = r * ((2 * i + 2) - (2 * i + 1) * w) * Consts.Log.LogConvergenceRemTable[i];
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                r *= squa_w;
                y = y_next;
            }

            return y;
        }

        public static ddouble Log10(ddouble x) {
            return Log2(x) * Consts.Log.Lg2;
        }

        public static ddouble Log(ddouble x) {
            return Log2(x) * Consts.Log.Ln2;
        }

        public static ddouble Log1p(ddouble x) {
            if (!(x >= -0.0625d) || x > 0.0625d) {
                return Log(1d + x);
            }
            if (IsPlusZero(x)) {
                return PlusZero;
            }
            if (IsMinusZero(x)) {
                return MinusZero;
            }

            ddouble squa_x = x * x, r = x;
            ddouble y = 0;

            for (int i = 0; i < Consts.Log.LogConvergenceRemTable.Count; i++) {
                ddouble dy = r * ((2 * i + 2) - (2 * i + 1) * x) * Consts.Log.LogConvergenceRemTable[i];
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                r *= squa_x;
                y = y_next;
            }

            return y;
        }

        private static partial class Consts {
            public static class Log {
                public static readonly ddouble Lg2 = Rcp(3d + Log2Prime(Ldexp(5d, -2)));
                public static readonly ddouble Ln2 = GenerateLn2();
                public static readonly ddouble LbE = Rcp(Ln2);
                public static readonly ddouble Lb10 = Rcp(Lg2);

                private static ddouble GenerateLn2() {
                    int n = 3;
                    KahanSum x = Rcp(3);

                    while (!x.IsConvergence) {
                        x.Add(Rcp(n * Pow(3d, n)));
                        n += 2;
                    }

                    ddouble y = Ldexp(x.Sum, 1);

                    return y;
                }

                public static readonly IReadOnlyList<ddouble> Log2Table = GenerateLog2Table();

                public static readonly ddouble Log2TableDx = Rcp(Log2Table.Count - 1);

                public static readonly int Log2TableN = Log2Table.Count - 1;

                public static readonly IReadOnlyList<ddouble> LogConvergenceRemTable = GenerateLogConvergenceRemTable();

                public static ddouble[] GenerateLog2Table() {
#if DEBUG
                    Trace.WriteLine($"Log2 initialize.");
#endif

                    const int n = 2048;
                    ddouble dx = Rcp(n);
                    ddouble[] table = new ddouble[n + 1];

                    for (int i = 0; i < table.Length; i++) {
                        ddouble x = 1 + dx * i;
                        table[i] = Log2Prime(x);
                    }

                    return table;
                }

                private static ddouble Log2Prime(ddouble x) {
                    if (!(x >= 1d) || x > 2d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }

                    if (x == 2d) {
                        return 1d;
                    }

                    ddouble y = 0d;
                    ddouble p = Ldexp(1d, -1);

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

                private static IReadOnlyList<ddouble> GenerateLogConvergenceRemTable() {
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
