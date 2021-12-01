using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace DoubleDoubleSandbox {
    public static class ErfTaylorSeries {
        private static readonly List<BigInteger> factorials = new() {
            1
        };

        private static readonly List<BigInteger> coefs = new() {
            1
        };

        public static ddouble Coef(int n) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (n < factorials.Count) {
                return coefs[n];
            }

            for (int k = factorials.Count; k <= n; k++) {
                BigInteger fact = factorials[k - 1] * k;
                BigInteger coef = fact * checked(2 * k + 1);

                if (k % 2 == 1) {
                    coef = -coef;
                }

                factorials.Add(fact);
                coefs.Add(coef);
            }

            return coefs[n];
        }
    }
}
