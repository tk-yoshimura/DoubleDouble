using DoubleDouble;

namespace DoubleDoubleSandbox {
    internal static class BesselLimit {

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            (ddouble x, ddouble y, int terms) = BesselJYSinCosWeights(nu, z, max_terms);

            ddouble omega = z - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = x * ddouble.Cos(omega) - y * ddouble.Sin(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * z));

            return (t, terms);
        }

        public static (ddouble y, int terms) BesselY(ddouble nu, ddouble z, int max_terms = 64) {
            (ddouble x, ddouble y, int terms) = BesselJYSinCosWeights(nu, z, max_terms);

            ddouble omega = z - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = x * ddouble.Sin(omega) + y * ddouble.Cos(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * z));

            return (t, terms);
        }

        public static (ddouble x, ddouble y, int terms) BesselJYSinCosWeights(ddouble nu, ddouble z, int max_terms = 64) { 
            BesselLimitCoef table = new BesselLimitCoef(nu);

            ddouble squa_nu4 = 4d * nu * nu;
            ddouble v = 1d / z;
            ddouble v2 = v * v, v4 = v2 * v2;

            ddouble x = 0d, y = 0d, p = 1d, q = v;

            static int square(int x) => x * x;

            int k = 0;
            for (; k <= max_terms; k++, p *= v4, q *= v4) {
                ddouble dx = p * table.Value(k * 4) * 
                    (1d - v2 * (squa_nu4 - square(8 * k + 1)) * (squa_nu4 - square(8 * k + 3)) / (64 * (4 * k + 1) * (4 * k + 2)));
                ddouble dy = q * table.Value(k * 4 + 1) * 
                    (1d - v2 * (squa_nu4 - square(8 * k + 3)) * (squa_nu4 - square(8 * k + 5)) / (64 * (4 * k + 2) * (4 * k + 3)));

                ddouble x_next = x + dx;
                ddouble y_next = y + dy;

                if (x == x_next && y == y_next) {
                    break;
                }

                x = x_next;
                y = y_next;
            }

            return (x, y, k);
        }
    }
}
