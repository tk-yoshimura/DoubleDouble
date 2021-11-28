using DoubleDouble;
using System;
using System.Collections.Generic;

namespace DoubleDoubleSandbox {

    static class GammaExpects {
        private static readonly ddouble sqrt_pi;
        private static readonly Dictionary<int, ddouble> table;

        static GammaExpects() {
            sqrt_pi = ddouble.Sqrt(ddouble.PI);

            table = new Dictionary<int, ddouble>();

            table.Add(1, 1);
            table.Add(2, 1);
        }

        public static ddouble Gamma(int z2) {
            if (z2 < 1) {
                throw new ArgumentException(nameof(z2));
            }

            static ddouble gamma(int i) {
                if (table.ContainsKey(i)) {
                    return table[i];
                }

                ddouble y = ddouble.Ldexp(i - 2, -1) * gamma(i - 2);

                table.Add(i, y);
                return y;
            }

            ddouble g = gamma(z2);

            return (z2 % 2 == 0) ? g : (g * sqrt_pi);
        }
    }
}