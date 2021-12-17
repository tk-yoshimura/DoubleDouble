using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class BesselMillerBackward {
        private static Dictionary<ddouble, BesselJTable> phi_table = new();
        private static Dictionary<ddouble, BesselITable> psi_table = new();

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

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, f = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);

                if (k - 1 == n) {
                    f = m0;
                }
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = f / lambda;

            return y;
        }

        public static ddouble BesselJ(ddouble nu, ddouble z, int m) {
            int n = (int)ddouble.Floor(nu);
            ddouble alpha = nu - n;

            if (alpha == 0) {
                return BesselJ(n, z, m);
            }

            if (ddouble.Abs(alpha - ddouble.Round(alpha)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(alpha));
            }

            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!phi_table.ContainsKey(alpha)) {
                phi_table.Add(alpha, new BesselJTable(alpha));
            }

            BesselJTable phi = phi_table[alpha];

            if (n >= 0) {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, f = ddouble.Zero;
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        int t = k / 2;

                        lambda += m0 * phi[t];
                    }

                    (m0, m1) = ((2 * (k + alpha)) * v * m0 - m1, m0);

                    if (k - 1 == n) {
                        f = m0;
                    }
                }

                lambda += m0 * phi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                ddouble y = f / lambda;

                return y;
            }
            else {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        int t = k / 2;

                        lambda += m0 * phi[t];
                    }

                    (m0, m1) = ((2 * (k + alpha)) * v * m0 - m1, m0);
                }

                lambda += m0 * phi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                for (int k = 0; k > n; k--) {
                    (m0, m1) = ((2 * (k + alpha)) * v * m0 - m1, m0);
                }

                ddouble y = m0 / lambda;

                return y;
            }
        }

        public static ddouble BesselJ0(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = m0 / lambda;

            return y;
        }

        public static ddouble BesselJ1(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = m1 / lambda;

            return y;
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

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, y0 = ddouble.Zero, y1 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;

                    int t = k / 2;
                    ddouble r = m0 / t;

                    y0 = ((k & 2) == 0) ? y0 - r : y0 + r;
                }
                else if (k >= 3) {
                    int t = k / 2;
                    ddouble r = m0 * (2 * t + 1) / (2 * t * (t + 1));

                    y1 = ((k & 2) == 0) ? y1 - r : y1 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;
            y0 = 2 * (2 * y0 + m0 * (ddouble.Log(z / 2) + ddouble.EulerGamma));
            y1 = 2 * (2 * y1 - v * m0 + (ddouble.Log(z / 2) + ddouble.EulerGamma - 1) * m1);

            for (int k = 1; k < n; k++) {
                (y1, y0) = ((2 * k) * v * y1 - y0, y1);
            }

            y1 /= lambda * ddouble.PI;

            return y1;
        }

        public static ddouble BesselY(ddouble nu, ddouble z, int m) {
            if ((nu - ddouble.Round(nu)) == 0) {
                int n = (int)nu;
                return BesselY(n, z, m);
            }

            if (ddouble.Abs(nu - ddouble.Round(nu)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(nu));
            }

            return (BesselJ(nu, z, m) * ddouble.CosPI(nu) - BesselJ(-nu, z, m)) / ddouble.SinPI(nu);
        }

        public static ddouble BesselY0(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, y0 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;

                    int t = k / 2;
                    ddouble r = m0 / t;

                    y0 = ((k & 2) == 0) ? y0 - r : y0 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;
            y0 = 2 * (2 * y0 + m0 * (ddouble.Log(z / 2) + ddouble.EulerGamma)) / (ddouble.PI * lambda);

            return y0;
        }

        public static ddouble BesselY1(ddouble z, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, y1 = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += m0;
                }
                else if (k >= 3) {
                    int t = k / 2;
                    ddouble r = m0 * (2 * t + 1) / (t * (t + 1));

                    y1 = ((k & 2) == 0) ? y1 - r : y1 + r;
                }

                (m0, m1) = ((2 * k) * v * m0 - m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;
            y1 = 2 * (y1 - v * m0 + (ddouble.Log(z / 2) + ddouble.EulerGamma - 1) * m1) / (lambda * ddouble.PI);

            return y1;
        }

        public static ddouble BesselI(int n, ddouble z, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return BesselI(-n, z, m, scale);
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, f = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                lambda += m0;

                (m0, m1) = ((2 * k) * v * m0 + m1, m0);

                if (k - 1 == n) {
                    f = m0;
                }
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = f / lambda;

            if (!scale) {
                y *= ddouble.Exp(z);
            }

            return y;
        }

        public static ddouble BesselI(ddouble nu, ddouble z, int m, bool scale = false) {
            int n = (int)ddouble.Floor(nu);
            ddouble alpha = nu - n;

            if (alpha == 0) {
                return BesselI(n, z, m, scale);
            }

            if (ddouble.Abs(alpha - ddouble.Round(alpha)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(alpha));
            }

            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!psi_table.ContainsKey(alpha)) {
                psi_table.Add(alpha, new BesselITable(alpha));
            }

            BesselITable psi = psi_table[alpha];

            if (n >= 0) {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero, f = ddouble.Zero;
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    lambda += m0 * psi[k];

                    (m0, m1) = ((2 * (k + alpha)) * v * m0 + m1, m0);

                    if (k - 1 == n) {
                        f = m0;
                    }
                }

                lambda += m0 * psi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                ddouble y = f / lambda;

                if (!scale) {
                    y *= ddouble.Exp(z);
                }

                return y;
            }
            else {
                ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
                ddouble v = 1d / z;

                for (int k = m; k >= 1; k--) {
                    lambda += m0 * psi[k];

                    (m0, m1) = ((2 * (k + alpha)) * v * m0 + m1, m0);
                }

                lambda += m0 * psi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                for (int k = 0; k > n; k--) {
                    (m0, m1) = ((2 * (k + alpha)) * v * m0 + m1, m0);
                }

                ddouble y = m0 / lambda;

                if (!scale) {
                    y *= ddouble.Exp(z);
                }

                return y;
            }
        }

        public static ddouble BesselI0(ddouble z, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                lambda += m0;

                (m0, m1) = ((2 * k) * v * m0 + m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = m0 / lambda;

            if (!scale) {
                y *= ddouble.Exp(z);
            }

            return y;
        }

        public static ddouble BesselI1(ddouble z, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble m0 = 1e-256, m1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / z;

            for (int k = m; k >= 1; k--) {
                lambda += m0;

                (m0, m1) = ((2 * k) * v * m0 + m1, m0);
            }

            lambda = ddouble.Ldexp(lambda, 1) + m0;

            ddouble y = m1 / lambda;

            if (!scale) {
                y *= ddouble.Exp(z);
            }

            return y;
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

        public class BesselJTable {
            private readonly ddouble alpha;
            private readonly List<ddouble> phi_table = new();

            private ddouble g;

            public BesselJTable(ddouble alpha) {
                this.alpha = alpha;
                
                ddouble phi0 = ddouble.Gamma(1 + alpha);
                ddouble phi1 = phi0 * (alpha + 2);
                
                this.g = phi0;
                
                this.phi_table.Add(phi0);
                this.phi_table.Add(phi1);
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < phi_table.Count) {
                    return phi_table[n];
                }

                for (int m = phi_table.Count; m <= n; m++) {
                    g = g * (alpha + m - 1) / m;

                    ddouble phi = g * (alpha + 2 * m);

                    phi_table.Add(phi);
                }

                return phi_table[n];
            }
        };

        public class BesselITable {
            private readonly ddouble alpha;
            private readonly List<ddouble> psi_table = new();

            private ddouble g;

            public BesselITable(ddouble alpha) {
                this.alpha = alpha;
                
                ddouble psi0 = ddouble.Gamma(1 + alpha);
                ddouble psi1 = 2 * psi0 * (1 + alpha);
                
                this.g = 2 * psi0;
                
                this.psi_table.Add(psi0);
                this.psi_table.Add(psi1);
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < psi_table.Count) {
                    return psi_table[n];
                }

                for (int m = psi_table.Count; m <= n; m++) {
                    g = g * (2 * alpha + m - 1) / m;

                    ddouble phi = g * (alpha + m);

                    psi_table.Add(phi);
                }

                return psi_table[n];
            }
        };
    }
}
