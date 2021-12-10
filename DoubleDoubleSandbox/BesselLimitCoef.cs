using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class BesselLimitCoef {
        private readonly ddouble squa_nu4;
        private readonly List<ddouble> a_table = new();

        public BesselLimitCoef(ddouble nu) {
            this.squa_nu4 = 4 * nu * nu;

            ddouble a1 = ddouble.Ldexp(squa_nu4 - 1, -3);

            this.a_table.Add(1);
            this.a_table.Add(a1);
        }

        public ddouble Value(int n) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (n < a_table.Count) {
                return a_table[n];
            }

            for (int k = a_table.Count; k <= n; k++) {
                ddouble a = a_table.Last() * (squa_nu4 - checked((2 * k - 1) * (2 * k - 1)) / checked(k * 8));

                a_table.Add(a);
            }

            return a_table[n];
        }
    }
}
