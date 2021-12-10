using DoubleDouble;

namespace DoubleDoubleSandbox {
    internal static class BesselLimit {

        public static (ddouble y, int terms) BesselJ(ddouble nu, ddouble z, int max_terms = 64) {
            BesselLimitCoef table = new BesselLimitCoef(nu);

            ddouble v = 1 / z;
            ddouble w = v * v;

            ddouble x = 0, y = 0, p = 1, q = v;

            int k = 0;
            for (int conv_cnt = 0; k <= max_terms && conv_cnt < 2; k++, p *= w, q *= w) {
                ddouble dx = p * table.Value(k * 2);
                ddouble dy = q * table.Value(k * 2 + 1);

                if ((k & 1) == 0) {
                    ddouble x_next = x + dx;
                    ddouble y_next = y + dy;

                    if (x == x_next && y == y_next) {
                        conv_cnt++;
                    }

                    x = x_next;
                    y = y_next;
                }
                else {
                    ddouble x_next = x - dx;
                    ddouble y_next = y - dy;

                    if (x == x_next && y == y_next) {
                        conv_cnt++;
                    }

                    x = x_next;
                    y = y_next;
                }

                if (k == max_terms) {
                    return (ddouble.NaN, k);
                }
            }

            ddouble omega = z - (2 * nu + 1) * ddouble.PI / 4;
            ddouble m = x * ddouble.Cos(omega) - y * ddouble.Sin(omega);
            ddouble t = m * ddouble.Sqrt(2 / (ddouble.PI * z));

            return (t, k);
        }
    }
}
