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
                public static readonly ddouble LanczosG = 15;
                public static readonly ReadOnlyCollection<ddouble> SterlingTable = GenerateSterlingTable();

                private static ReadOnlyCollection<ddouble> GenerateSterlingTable() {
                    List<ddouble> table = new();
                    
                    for (int k = 1; k <= 28; k++) {
                        ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k) * (2 * k - 1));
                        table.Add(c);
                    }

                    return table.AsReadOnly();
                }
            }
        }
    }
}
