using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Gamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble sinpi = SinPI(x);

                if (IsZero(sinpi)) {
                    return NaN;
                }

                ddouble y = PI / (sinpi * Gamma(1d - x));

                return y;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                ddouble v = Gamma(z);

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                return v / w;
            }
            else {
                ddouble r = ddouble.Sqrt(Ldexp(ddouble.PI, 1) / x);
                ddouble p = ddouble.Pow(x / ddouble.E, x);
                ddouble s = ddouble.Exp(SterlingTerm(x));

                ddouble y = RoundMantissa(r * p * s, Consts.Gamma.SterlingPrecision);

                return y;
            }
        }

        public static ddouble LogGamma(ddouble x) {
            if (IsNaN(x) || IsMinusZero(x) || x.Sign < 0) {
                return NaN;
            }
            if (IsPlusZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                return Log(Gamma(x));
            }

            if (x >= 0.999755859375d && x <= 1.000244140625d) {
                x -= 1d;

                return x * (-226800 * EulerGamma
                        + x * (18900 * (PI * PI)
                        + x * (-75600 * Zeta3
                        + x * (630 * Pow(PI, 4)
                        + x * (-45360 * Zeta5
                        + x * (40 * Pow(PI, 6)
                        + x * (-32400 * Zeta7
                        + x * (3 * Pow(PI, 8))))))))) / 226800;
            }
            if (x >= 1.999755859375d && x <= 2.000244140625d) {
                x -= 2d;

                return x * (226800 * (1 - EulerGamma)
                        + x * (18900 * ((PI * PI) - 6)
                        + x * (75600 * (1 - Zeta3)
                        + x * (630 * (Pow(PI, 4) - 90)
                        + x * (45360 * (1 - Zeta5)
                        + x * (40 * (Pow(PI, 6) - 945)
                        + x * (32400 * (1 - Zeta7)
                        + x * (3 * (Pow(PI, 8) - 9450))))))))) / 226800;
            }

            static ddouble sterling_loggamma(ddouble x) {
                ddouble p = (x - 0.5d) * ddouble.Log(x);
                ddouble s = SterlingTerm(x);

                ddouble k = Consts.Gamma.SterlingLogBias;
                k += p;
                k += s;
                k -= x;

                return k;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                ddouble v = sterling_loggamma(z);

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                v -= Log(w);

                ddouble y = v;

                return y;
            }
            else {
                ddouble y = sterling_loggamma(x);

                return y;
            }
        }

        public static ddouble Digamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble tanpi = TanPI(x);

                if (IsZero(tanpi)) {
                    return NaN;
                }

                ddouble y = Digamma(1 - x) - PI / tanpi;

                return y;
            }

            ddouble x_zsft = x - DigammaZero;
            if (Abs(x_zsft) < Math.ScaleB(1, -6)) {
                ReadOnlyCollection<ddouble> table = Consts.Digamma.DiffCoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x_zsft + table[i];
                }
                s *= x_zsft;

                return s;
            }

            static ddouble sterling_digamma(ddouble x) {
                ddouble s = -DiffLogSterlingTerm(x);
                ddouble p = ddouble.Log(x);
                ddouble c = Ldexp(Rcp(x), -1);

                s += p;
                s -= c;

                return s;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Digamma.Threshold;
                ddouble v = sterling_digamma(z);
                ddouble s = SumFraction(-v, f + n, Consts.Digamma.Threshold - n);

                ddouble y = -s;

                return y;
            }
            else {
                ddouble y = sterling_digamma(x);

                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble z) {
            ddouble v = Rcp(z), w = v * v, u = w, dx_prev = PositiveInfinity;

            ddouble x = Consts.Gamma.SterlingTable[0];
            foreach (ddouble s in Consts.Gamma.SterlingTable.Skip(1)) {
                ddouble dx = u * s;
                ddouble x_next = x + dx;

                if (Abs(dx) > Abs(dx_prev) || x == x_next) {
                    break;
                }

                u *= w;
                dx_prev = dx;
                x = x_next;
            }

            ddouble y = x * v;

            return y;
        }

        private static ddouble DiffLogSterlingTerm(ddouble z) {
            ddouble v = Rcp(z), w = v * v, u = w * w, dx_prev = PositiveInfinity;

            ddouble x = Consts.Digamma.SterlingTable[0] * w;
            foreach (ddouble s in Consts.Digamma.SterlingTable.Skip(1)) {
                ddouble dx = u * s;
                ddouble x_next = x + dx;

                if (Abs(dx) > Abs(dx_prev) || x == x_next) {
                    break;
                }

                u *= w;
                dx_prev = dx;
                x = x_next;
            }

            return x;
        }

        private static ddouble SumFraction(ddouble s, ddouble x, int n) {
            // sum( 1 / (x + i) , i = 0, n)

            static ddouble sum1(ddouble s, ddouble x) {
                s += Rcp(x);
                return s;
            }

            static ddouble sum2(ddouble s, ddouble x) {
                s += (2d * x + 1d) / (x * (x + 1d));
                return s;
            }

            static ddouble sum4(ddouble s, ddouble x) {
                s += (2d * (2d * x + 3d) * (x * (x + 3d) + 1d)) / (x * (x + 1) * (x + 2) * (x + 3));
                return s;
            }

            int i = 0;
            for (; i < n - 3; i += 4) {
                s = sum4(s, x + i);
            }
            for (; i < n - 1; i += 2) {
                s = sum2(s, x + i);
            }
            for (; i < n; i++) {
                s = sum1(s, x + i);
            }

            return s;
        }

        private static partial class Consts {
            public static class Gamma {
                public const int Threshold = 16;
                public const int SterlingPrecision = 96;
                public static readonly ddouble SterlingLogBias = Log(Ldexp(PI, 1)) / 2;
                public static readonly ReadOnlyCollection<ddouble> SterlingTable = GenerateSterlingTable();

                private static ReadOnlyCollection<ddouble> GenerateSterlingTable() {
                    List<ddouble> table = new();

                    for (int k = 1; k <= 24; k++) {
                        ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k) * (2 * k - 1));
                        table.Add(c);
                    }

                    return table.AsReadOnly();
                }
            }

            public static class Digamma {
                public const int Threshold = 18;
                public static readonly ReadOnlyCollection<ddouble> SterlingTable = GenerateSterlingTable();
                public static readonly ReadOnlyCollection<ddouble> DiffCoefTable = GenerateDiffCoefTable();

                private static ReadOnlyCollection<ddouble> GenerateSterlingTable() {
                    List<ddouble> table = new();

                    for (int k = 1; k <= 32; k++) {
                        ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k));
                        table.Add(c);
                    }

                    return table.AsReadOnly();
                }

                private static ReadOnlyCollection<ddouble> GenerateDiffCoefTable() {
                    ddouble[] table = new ddouble[] {
                        (+1, -1, 0xF7B95E4771C55D8FuL, 0x0C28D0814E530F17uL),
                        (-1, -2, 0xE2B1DAA550D1AB8EuL, 0xC060D20DBFB8B839uL),
                        (+1, -2, 0x845A14A6A81C05D6uL, 0x6D7B6900032EA171uL),
                        (-1, -3, 0xA7E098B92BED4186uL, 0xB1CC35DBC77717C5uL),
                        (+1, -4, 0xDCD2DB1B879D54BEuL, 0xA5BB6701257D9AE9uL),
                        (-1, -4, 0x93DD5D130E615E39uL, 0x112646269ACB6FADuL),
                        (+1, -5, 0xC7E701591CE534BDuL, 0xCE5785C71958A783uL),
                        (-1, -5, 0x87D3F61B53EC74F7uL, 0xB974CEEC1C8E4479uL),
                        (+1, -6, 0xB91EB403F6E601F3uL, 0x03AB27FC78546609uL),
                        (-1, -7, 0xFCB828470DB50E3BuL, 0xDE9CB3BBCBD07F19uL),
                        (+1, -7, 0xACAAE5554B1799D3uL, 0x608DED8B2BC7C2DBuL),
                        (-1, -8, 0xEC1403C94175BC26uL, 0xD38DD7FDE3176ECEuL),
                        (+1, -8, 0xA170D67C0EC6E1A2uL, 0xC77E2ED9FBB6EB4DuL),
                        (-1, -9, 0xDCD7E3E774509DBBuL, 0x8A3BE79375D38117uL),
                        (+1, -9, 0x97119A2FBCD575A0uL, 0xDB3291E0A10B65E2uL),
                        (-1, -10, 0xCEB137B8E8FBD21EuL, 0xCF44BBCA27C8C934uL),
                        (+1, -10, 0x8D675DEBB08444B6uL, 0xFBD3A05C1AB57FD1uL),
                        (-1, -11, 0xC17B1A2E32D9A67CuL, 0xEA3F72854D6949FFuL),
                        (+1, -11, 0x845ED70D130EA396uL, 0x040CF3D77D20E1ABuL),
                        (-1, -12, 0xB51FEEF50898212DuL, 0x6D777952AF76243EuL),
                        (+1, -13, 0xF7D655B7DC4C2743uL, 0x5893CEC3C2777BE2uL),
                        (-1, -13, 0xA98FAEE2F5AB1930uL, 0x68DF0FDDDE360A15uL),
                        (+1, -14, 0xE80407C087D5AB71uL, 0x1808B6CE80E54B44uL),
                        (-1, -14, 0x9EBCB934457B3718uL, 0xF9DD497E02D9ED9AuL),
                        (+1, -15, 0xD93488C162E19F3DuL, 0x81B14BA0058FE529uL),
                        (-1, -15, 0x949ABFBEB65C3335uL, 0x30F69A4CF3A566ADuL),
                        (+1, -16, 0xCB572547195A4657uL, 0x8628291C2E71B177uL),
                        (-1, -16, 0x8B1E639B9DF4552DuL, 0xF04BBB7240BEB178uL),
                        (+1, -17, 0xBE5C59C8B7E9489CuL, 0x6E3524C368BB5487uL),
                        (-1, -17, 0x823D0C4A707EF06EuL, 0x7BC750E6D8B75220uL),
                        (+1, -18, 0xB235AA7A085EF65DuL, 0xDFFE6198BB04CDDAuL),
                        (-1, -19, 0xF3D9A52DBC51E21FuL, 0x315F0A58002D8212uL),
                    };

                    return Array.AsReadOnly(table);
                }
            }
        }
    }
}
