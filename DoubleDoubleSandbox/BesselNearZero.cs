using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class BesselNearZero {
        private static Dictionary<ddouble, CoefTable> coef_table = new();

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            if (nu.Sign < 0 && nu == ddouble.Floor(nu)) {
                int n = (int)ddouble.Floor(nu);
                (ddouble y, int terms) = BesselJ(-nu, z, max_terms);

                return ((n & 2) == 0) ? (y, terms) : (-y, terms);
            }
            else {
                (ddouble c, int terms) = BesselJICoef(nu, z, sign_switch: true, max_terms);

                ddouble t = ddouble.Pow(ddouble.Ldexp(z, -1), nu) * c;

                return (t, terms);
            }
        }

        public static (ddouble c, int terms) BesselJICoef(ddouble nu, ddouble z, bool sign_switch, int max_terms = 64) {
            if (!coef_table.ContainsKey(nu)) {
                coef_table.Add(nu, new CoefTable(nu));
            }

            CoefTable table = coef_table[nu];

            ddouble z2 = z * z, z4 = z2 * z2;

            ddouble c = 0d, u = 1d;

            for (int k = 0; k <= max_terms; k++) {
                ddouble w = z2 / (4 * (2 * k + 1) * ((2 * k + 1) + nu));
                ddouble dc = u * table.Value(k) * (sign_switch ? (1d - w) : (1d + w));

                ddouble c_next = c + dc;

                if (c == c_next) {
                    return (c, k);
                }

                c = c_next;
                u *= z4;
            }

            return (ddouble.NaN, int.MaxValue);
        }

        public class CoefTable {
            private readonly ddouble nu;
            private readonly List<ddouble> c_table = new(), a_table = new();

            public CoefTable(ddouble nu) {
                ddouble c = ddouble.Gamma(nu + 1);

                this.nu = nu;
                this.c_table.Add(c);
                this.a_table.Add(1d / c);
            }

            public ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < a_table.Count) {
                    return a_table[n];
                }

                for (int k = c_table.Count; k <= n; k++) {
                    ddouble c = c_table.Last() * (nu + (2 * k - 1)) * (nu + (2 * k)) * checked(32 * k * (2 * k - 1));

                    c_table.Add(c);
                    a_table.Add(1d / c);
                }

                return a_table[n];
            }
        }
    }
}
