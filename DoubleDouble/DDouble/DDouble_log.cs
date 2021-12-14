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

            int index = (int)ddouble.Floor((v - 1d) * Consts.Log.Log2TableN);
            ddouble v_offset = 1d + Consts.Log.Log2TableDx * index;

            ddouble w = v / v_offset - 1d, w2 = w * w, u = LbE * w;
            ddouble y = n + Consts.Log.Log2Table[index];

            for (int i = 0; i < Consts.Log.LogConvergenceRemTable.Count; i++) {
                ddouble dy = u * ((2 * i + 2) - (2 * i + 1) * w) * Consts.Log.LogConvergenceRemTable[i];
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= w2;
                y = y_next;
            }

            return y;
        }

        public static ddouble Log10(ddouble x) {
            return RoundMantissa(Log2(x) * Lg2, keep_bits: 103);
        }

        public static ddouble Log(ddouble x) {
            return Log2(x) * Ln2;
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

            ddouble x2 = x * x, u = x;
            ddouble y = ddouble.Zero;

            for (int i = 0; i < Consts.Log.LogConvergenceRemTable.Count; i++) {
                ddouble dy = u * ((2 * i + 2) - (2 * i + 1) * x) * Consts.Log.LogConvergenceRemTable[i];
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= x2;
                y = y_next;
            }

            return y;
        }

        private static partial class Consts {
            public static class Log {
                private static ddouble GenerateLn2() {
                    int n = 3;
                    ddouble x = Rcp(3d);

                    while (true) {
                        ddouble dx = Rcp(n * Pow(3d, n));
                        ddouble x_next = x + dx;

                        if (x == x_next) {
                            break;
                        }

                        n += 2;
                        x = x_next;
                    }

                    ddouble y = Ldexp(x, 1);

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
