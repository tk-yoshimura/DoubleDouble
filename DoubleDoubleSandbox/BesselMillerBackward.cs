using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    internal class BesselMillerBackward {
        public static ddouble BesselJ0(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            d = ddouble.Ldexp(d, 1) + m0;

            ddouble y = m0 / d;

            return y;
        }

        public static ddouble BesselJ1(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            d = ddouble.Ldexp(d, 1) + m0;

            ddouble y = m1 / d;

            return y;
        }

        public static ddouble BesselJ(int n, ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return ((n & 1) == 0) ? BesselJ(-n, z, m) : -BesselJ(-n, z, m);
            }
            if (n == 0) {
                return BesselJ0(z, m);
            }
            if (n == 1) {
                return BesselJ1(z, m);
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
            if (n == 0) {
                return BesselY0(z, m);
            }
            if (n == 1) {
                return BesselY1(z, m);
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, y0 = ddouble.Zero, y1 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;

                    int t = k / 2;
                    ddouble r = m0 / t;

                    y0 = ((k & 2) == 0) ? y0 - r : y0 + r;
                }
                else if(k >= 3){
                    int t = k / 2;
                    ddouble r = m0 * (2 * t + 1) / (2 * t * (t + 1));

                    y1 = ((k & 2) == 0) ? y1 - r : y1 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            d = ddouble.Ldexp(d, 1) + m0;
            y0 = 2 * (2 * y0 + m0 * (ddouble.Log(z / 2) + ddouble.EulerGamma));
            y1 = 2 * (2 * y1 - v * m0 + (ddouble.Log(z / 2) + ddouble.EulerGamma - 1) * m1);

            for (int k = 1; k < n; k++) { 
                (y1, y0) = ((2 * k) * v * y1 - y0, y1);
            }

            y1 /= d * ddouble.PI;

            return y1;
        }

        public static ddouble BesselY0(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, y0 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;

                    int t = k / 2;
                    ddouble r = m0 / t;

                    y0 = ((k & 2) == 0) ? y0 - r : y0 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            d = ddouble.Ldexp(d, 1) + m0;
            y0 = 2 * (2 * y0 + m0 * (ddouble.Log(z / 2) + ddouble.EulerGamma)) / (ddouble.PI * d);

            return y0;
        }

        public static ddouble BesselY1(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, y1 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    d += m0;
                }
                else if(k >= 3){
                    int t = k / 2;
                    ddouble r = m0 * (2 * t + 1) / (2 * t * (t + 1));

                    y1 = ((k & 2) == 0) ? y1 - r : y1 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            d = ddouble.Ldexp(d, 1) + m0;
            y1 = 2 * (2 * y1 - v * m0 + (ddouble.Log(z / 2) + ddouble.EulerGamma - 1) * m1) / (d * ddouble.PI);

            return y1;
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
