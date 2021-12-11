using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    internal class BesselMillerBackward {
        public static ddouble BesselJ(int n, ddouble z, int m) {
            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, f = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);

                if (k - 1 == n) {
                    f = m0;
                }
            }

            d = ddouble.Ldexp(d, 1) + m0;

            ddouble y = f / d;

            return y;
        }

        public static (ddouble y, int m) BesselJ(int n, ddouble z, ddouble eps, int max_m = 512) {
            ddouble y_prev = ddouble.NaN, dy = ddouble.NaN, dy_prev = ddouble.NaN;
            
            for (int m = n * 4; m <= max_m; m += 2) {
                ddouble y = BesselJ(n, z, m);
                
                dy = ddouble.Abs(y - y_prev);
                y_prev = y;

                if (dy < eps && dy >= dy_prev) {
                    return (y, m - 2);
                }

                dy_prev = dy;
            }

            return (y_prev, max_m);
        }

        public static ddouble BesselJ(int n, ddouble z) {
            int m = (int)Math.Ceiling(20 + (double)z * 0.825) * 2; 
 
            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, f = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);

                if (k - 1 == n) {
                    f = m0;
                }
            }

            d = ddouble.Ldexp(d, 1) + m0;

            ddouble y = f / d;

            return y;
        }
    }
}
