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
                KahanSum v = sterling_loggamma(z);

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

            static KahanSum sterling_digamma(ddouble x) {
                KahanSum s = KahanSum.Negate(DiffLogSterlingTerm(x));
                ddouble p = ddouble.Log(x);
                ddouble c = Ldexp(Rcp(x), -1);

                s.Add(p);
                s.Add(-c);

                return s;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Digamma.Threshold;
                KahanSum v = sterling_digamma(z);
                KahanSum s = SumFraction(KahanSum.Negate(v), f + n, Consts.Digamma.Threshold - n);

                ddouble y = -s.Sum;

                return y;
            }
            else {
                ddouble y = sterling_digamma(x).Sum;

                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble z) {
            ddouble v = Rcp(z), w = v * v, u = w, dx_prev = PositiveInfinity;

            KahanSum x = Consts.Gamma.SterlingTable[0];
            foreach (ddouble s in Consts.Gamma.SterlingTable.Skip(1)) {
                ddouble dx = u * s;

                if (Abs(dx) > Abs(dx_prev) || x.IsConvergence) {
                    break;
                }

                x.Add(dx);
                if (x.IsConvergence) {
                    break;
                }

                u *= w;
            }

            ddouble y = x.Sum * v;

            return y;
        }

        private static KahanSum DiffLogSterlingTerm(ddouble z) {
            ddouble v = Rcp(z), w = v * v, u = w * w, dx_prev = PositiveInfinity;

            KahanSum x = Consts.Digamma.SterlingTable[0] * w;
            foreach (ddouble s in Consts.Digamma.SterlingTable.Skip(1)) {
                ddouble dx = u * s;

                if (Abs(dx) > Abs(dx_prev)) {
                    break;
                }

                x.Add(dx);
                if (Ldexp(Abs(x.Sum), -128) > Abs(dx)) {
                    break;
                }

                u *= w;
                dx_prev = dx;
            }

            return x;
        }

        private static KahanSum SumFraction(KahanSum s, ddouble x, int n) {
            // sum( 1 / (x + i) , i = 0, n)
            
            static KahanSum sum1(KahanSum s, ddouble x) {
                s.Add(Rcp(x));
                return s;
            }

            static KahanSum sum2(KahanSum s, ddouble x) {
                s.Add((2d * x + 1d) / (x * (x + 1d)));
                return s;
            }

            static KahanSum sum4(KahanSum s, ddouble x) {
                s.Add((2d * (2d * x + 3d) * (x * (x + 3d) + 1d)) / (x * (x + 1) * (x + 2) * (x + 3)));
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

                private static ReadOnlyCollection<ddouble> GenerateSterlingTable() {
                    List<ddouble> table = new();

                    for (int k = 1; k <= 32; k++) {
                        ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k));
                        table.Add(c);
                    }

                    return table.AsReadOnly();
                }
            }
        }
    }
}
