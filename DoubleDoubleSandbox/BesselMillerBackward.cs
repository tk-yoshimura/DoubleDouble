using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    internal class BesselMillerBackward {
        public static ddouble BesselJ(int n, ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return ((n & 1) == 0) ? BesselJ(-n, z, m) : -BesselJ(-n, z, m);
            }

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

        public static ddouble BesselJ(ddouble nu, ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            int n = (int)ddouble.Floor(nu);
            nu -= n;

            if (nu == 0) {
                return BesselJ(n, z, m);
            }

            if (n >= 0) {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, f = ddouble.Zero;
                ddouble g = ddouble.Exp(ddouble.LogGamma(nu + m / 2) - ddouble.LogGamma(m / 2 + 1));
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        d += (nu + k) * m0 * g;
                        g *= (k / 2) / (nu + (k / 2) - 1);
                    }

                    (m0, m1) = ((2 * (k + nu)) * v * m0 - m1, m0);

                    if (k - 1 == n) {
                        f = m0;
                    }
                }

                d += nu * m0 * g;

                d *= ddouble.Pow2(nu) * ddouble.Pow(v, nu);

                ddouble y = f / d;

                return y;
            }
            else {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero;
                ddouble g = ddouble.Exp(ddouble.LogGamma(nu + m / 2) - ddouble.LogGamma(m / 2 + 1));
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        d += (nu + k) * m0 * g;
                        g *= (k / 2) / (nu + (k / 2) - 1);
                    }

                    (m0, m1) = ((2 * (k + nu)) * v * m0 - m1, m0);
                }

                d += nu * m0 * g;
                d *= ddouble.Pow2(nu) * ddouble.Pow(v, nu);

                for (int k = 0; k > n; k--) { 
                    (m0, m1) = ((2 * (k + nu)) * v * m0 - m1, m0);
                }

                ddouble y = m0 / d;

                return y;
            }
        }

        public static ddouble BesselY(int n, ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return ((n & 1) == 0) ? BesselY(-n, z, m) : -BesselY(-n, z, m);
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero;
            ddouble[] fs = new ddouble[m];
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);

                fs[k - 1] = m0;
            }

            d = ddouble.Ldexp(d, 1) + m0;

            ddouble[] gs = new ddouble[m / 2 + 1], xs = new ddouble[m];
            gs[0] = (2 * (ddouble.Log(z / 2) + ddouble.EulerGamma)) / ddouble.PI;
            gs[1] = 4 / ddouble.PI;
            xs[0] = -2 / (z * ddouble.PI);
            xs[1] = gs[0] - gs[1] / 2;

            for (int k = 2; k < gs.Length; k++) {
                gs[k] = -gs[k - 1] * (k - 1) / k; 
            }
            for (int k = 3; k < xs.Length; k+= 2) {
                xs[k] = (gs[k / 2] - gs[k / 2 + 1]) / 2;
            }

            ddouble y0 = gs[0] * fs[0] + gs[1] * fs[2], y1 = xs[0] * fs[0] + xs[1] * fs[1];
            for (int k = 4; k < fs.Length; k += 2) {
                y0 += gs[k / 2] * fs[k];
                y1 += xs[k - 1] * fs[k - 1];
            }

            y0 /= d;
            y1 /= d;

            return y0;
        }


        public static (ddouble y, int m) BesselJ(int n, ddouble z, ddouble eps, int max_m = 512) {
            ddouble y_prev = ddouble.NaN, dy = ddouble.NaN, dy_prev = ddouble.NaN;
            
            for (int m = n * 4 + 2; m <= max_m; m += 2) {
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
