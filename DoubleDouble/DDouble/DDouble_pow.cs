using System.Collections.ObjectModel;
using static DoubleDouble.ddouble.Consts.Pow;

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
                return IsNegative(x) ? 0d : PositiveInfinity;
            }
            if (x >= 1024d) {
                return PositiveInfinity;
            }
            if (x <= -1024d) {
                return double.Exp2(x.hi);
            }

            int exp = (int)Floor(x);
            ddouble s = x - exp;

            int index = (int)Floor(s * (Pow2TableN * Pow2TableN));
            ddouble v = s - Pow2TableDx * index;
            ddouble r0 = Pow2Table[index / Pow2TableN];
            ddouble r1 = Pow2Table[index % Pow2TableN + Pow2TableN + 1];

            ddouble w = 1d + v * (Pow2C1 + v * (Pow2C2 + v * (Pow2C3 + v * Pow2C4)));

            ddouble y = Ldexp(r0 * r1 * w, exp);

            return y;
        }

        public static ddouble Pow2m1(ddouble x) {
            if (x < -0.1376953125d || x > 0.149658203125d) {
                return Pow2(x) - 1d;
            }
            if (IsPlusZero(x)) {
                return 0d;
            }
            if (IsMinusZero(x)) {
                return -0d;
            }

            x *= Ln2;

            ddouble sc = 259459200d + x * (8648640d + x * (8648640d + x * (277200d + x * (55440d + x * (1512d + x * (72d + x * 1d))))));
            ddouble sd = 259459200d + x * (-121080960d + x * (25945920d + x * (-3326400d + x * (277200d + x * (-15120d + x * (504d + x * -8d))))));

            ddouble y = x * sc / sd;

            return y;
        }

        public static ddouble Pow(ddouble x, ddouble y) {
            if (IsNegative(x)) {
                return NaN;
            }
            if (IsZero(y)) {
                return IsNaN(x) ? NaN : 1d;
            }
            if (IsZero(x)) {
                return 0d;
            }

            if (y <= long.MinValue) {
                if (x == 1d) {
                    return 1d;
                }

                return x < 1d ? PositiveInfinity : 0d;
            }
            if (y >= long.MaxValue) {
                if (x == 1d) {
                    return 1;
                }

                return x < 1d ? 0d : PositiveInfinity;
            }

            if (x != PowCache.X) {
                PowCache.X = x;
                PowCache.LbX = Log2(x);
            }

            long n = (long)Truncate(y);
            ddouble f = y - n;

            ddouble z = Pow(x, n) * Pow2(f * PowCache.LbX);

            return z;
        }

        public static ddouble Pow10(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x >= 310d) {
                return PositiveInfinity;
            }
            if (x <= -310d) {
                return double.Exp10(x.hi);
            }

            int n = (int)Truncate(x);
            ddouble f = x - n;

            ddouble pow10n = (n >= 0) ? Pow10NTable[n] : (1d / Pow10NTable[-n]);

            ddouble z = pow10n * Pow2(f * Lb10);

            return z;
        }

        public static ddouble Exp(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x >= 710d) {
                return PositiveInfinity;
            }
            if (x <= -710d) {
                return double.Exp(x.hi);
            }

            int n = (int)Truncate(x);
            ddouble f = x - n;

            ddouble expn = (n >= 0) ? ExpNTable[n] : (1d / ExpNTable[-n]);

            ddouble z = expn * Pow2(f * LbE);

            return z;
        }

        public static ddouble Expm1(ddouble x) {
            if (x < -0.09375d || x > 0.102294921875d) {
                return Exp(x) - 1d;
            }
            if (IsPlusZero(x)) {
                return 0d;
            }
            if (IsMinusZero(x)) {
                return -0d;
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
                public static readonly ddouble Pow2C2 = (+1, -3, 0xF5FDEFFC162C7543uL, 0x78B583764B9AFE55uL);
                public static readonly ddouble Pow2C3 = (+1, -5, 0xE35846B82505FC59uL, 0x9D3B15D995E96F74uL);
                public static readonly ddouble Pow2C4 = (+1, -7, 0x9D955B7DD273B94EuL, 0x65DF05A9F7562839uL);

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
                    ReadOnlyCollection<ddouble> es = new(new ddouble[10]{
                        E,
                        (+1, 2, 0xEC7325C6A6ED6E61uL, 0x9D1DD1035B455DD4uL),
                        (+1, 5, 0xDA64817139D2C33CuL, 0x6B69DFEDC9EDCB68uL),
                        (+1, 11, 0xBA4F53EA38636F85uL, 0xF007042540AE8EF3uL),
                        (+1, 23, 0x87975E8540010249uL, 0x11F8B84415AF72F7uL),
                        (+1, 46, 0x8FA1FE625B3163ECuL, 0x23C4200C210BA03AuL),
                        (+1, 92, 0xA12CC167ACBE6902uL, 0xE71EADA76D818BABuL),
                        (+1, 184, 0xCAF2A62EEA10BBFAuL, 0x9FA6A90CEF88E51DuL),
                        (+1, 369, 0xA0E3D440A5F5D071uL, 0x919807BBBED32C2AuL),
                        (+1, 738, 0xCA3B2825D4297360uL, 0x4E42B3E00E2D3324uL)
                    });

                    ddouble[] table = new ddouble[711];
                    for (int i = 0; i < table.Length; i++) {
                        int n = i;
                        ddouble y = 1d;

                        for (int j = 0; j < es.Count && n > 0; j++, n >>= 1) {
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
#if DEBUG
                    if (!(x >= 0d) || x > 1d) {
                        throw new ArgumentOutOfRangeException(nameof(x));
                    }
#endif
                    if (x == 1d) {
                        return 2;
                    }

                    ddouble w = x * Ln2, u = w, y = 1d, c = 0d;

                    foreach (ddouble f in TaylorSequence.Skip(1)) {
                        ddouble dy = f * u;
                        ddouble d = dy - c;
                        ddouble y_next = y + d;

                        if (y == y_next) {
                            break;
                        }

                        u *= w;
                        c = (y_next - y) - d;
                        y = y_next;
                    }

                    return y;
                }
            }
        }

        internal static class PowCache {
            public static ddouble X = NaN, LbX = NaN;
        }
    }
}
