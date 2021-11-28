using DoubleDouble;
using System;
using System.Collections.Generic;

namespace DoubleDoubleSandbox {
    static class SterlingApprox {
        static readonly Dictionary<int, ddouble> table = new();

        public static ddouble Gamma(ddouble z, int terms) {
            if (z < 0.5) {
                throw new ArgumentException(nameof(z));
            }

            ddouble r = ddouble.Sqrt(2 * ddouble.PI / z);
            ddouble p = ddouble.Pow(z / ddouble.E, z);
            ddouble s = ddouble.Exp(SterlingTerm(z, terms));

            ddouble y = r * p * s;

            return y;
        }

        private static ddouble SterlingTerm(ddouble z, int terms) {
            ddouble v = 1 / z;
            ddouble w = v * v;

            ddouble x = 0, u = 1;

            for (int k = 1; k <= terms; k++) {
                ddouble c = u * SterlingCoef(k);

                x += c;
                u *= w;
            }

            ddouble y = x * v;

            return y;
        }

        public static int SterlingTermConvergence(ddouble z) {
            ddouble v = 1 / z;
            ddouble w = v * v;

            KahanSum x = (ddouble)0d;
            ddouble u = 1;

            for (int terms = 1; terms < ddouble.BernoulliSequence.Count; terms++) {
                ddouble dx = u * SterlingCoef(terms);

                x.Add(dx);

                if (x.IsConvergence) {
                    return terms;
                }

                u *= w;
            }

            return int.MaxValue;
        }

        private static ddouble SterlingCoef(int k) {
            if (!table.ContainsKey(k)) {
                ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k) * (2 * k - 1));

                table.Add(k, c);
            }

            return table[k];
        }
    }
}