using System;
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
            for (int i = 1; i < 17; i += 2) {
                ddouble dy = r * ((i + 1) - i * w) / (i * (i + 1));
                ddouble y_next = y + dy;

                if (y == y_next) {
                    Console.WriteLine("conv " + i);

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

        private static partial class Consts {
            public static class Log {
                public static readonly ddouble Lg2 = Rcp(3 + Log2(Ldexp(5, -2)));
                public static readonly ddouble Ln2 = GenerateLn2();
                public static readonly ddouble LbE = Rcp(Ln2);

                private static ddouble GenerateLn2() {
                    int n = 3;
                    ddouble x = 0, dx = Rcp(3);
                    while (x != (x + dx)) {
                        x += dx;
                        dx = Rcp(n * Pow(3, n));
                        n += 2;
                    }

                    ddouble y = Ldexp(x, 1);

                    return y; 
                }

                public static readonly IReadOnlyList<ddouble> Log2Table = GenerateLog2Table();

                public static readonly ddouble Log2TableDx = Rcp(Log2Table.Count);

                public static readonly int Log2TableN = Log2Table.Count;

                public static ddouble[] GenerateLog2Table() { 
                    const int n = 1024;
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
                    ddouble p = 0.5;

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
            }
        }
    }
}
