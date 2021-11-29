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
            else {
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
                x -= 1;

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
                x -= 2;

                return x * (226800 * (1 - EulerGamma)
                        + x * (18900 * ((PI * PI) - 6)
                        + x * (75600 * (1 - Zeta3)
                        + x * (630 * (Pow(PI, 4) - 90)
                        + x * (45360 * (1 - Zeta5)
                        + x * (40 * (Pow(PI, 6) - 945)
                        + x * (32400 * (1 - Zeta7)
                        + x * (3 * (Pow(PI, 8) - 9450))))))))) / 226800;
            }

            static KahanSum sterling_loggamma(ddouble x) { 
                ddouble p = (x - 0.5d) * ddouble.Log(x);
                ddouble s = SterlingTerm(x);

                KahanSum k = Consts.Gamma.SterlingLogBias;
                k.Add(p);
                k.Add(s);
                k.Add(-x);

                return k;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                KahanSum v = sterling_loggamma(z).Sum;

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                v.Add(-Log(w));

                ddouble y = v.Sum;

                return y;
            }
            else {
                ddouble y = sterling_loggamma(x).Sum;

                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble z) {
            KahanSum x = Consts.Gamma.SterlingTable[0];

            ddouble v = Rcp(z), w = v * v, u = w;

            foreach (ddouble s in Consts.Gamma.SterlingTable.Skip(1)) {
                x.Add(u * s);

                if (x.IsConvergence) {
                    break;
                }

                u *= w;
            }

            ddouble y = x.Sum * v;

            return y;
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
        }
    }
}
