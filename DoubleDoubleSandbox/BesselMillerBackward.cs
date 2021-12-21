using DoubleDouble;
using System;
using System.Collections.Generic;

namespace DoubleDoubleSandbox {
    internal class BesselMillerBackward {
        private static Dictionary<ddouble, BesselJPhiTable> phi_table = new();
        private static Dictionary<ddouble, BesselIPsiTable> psi_table = new();
        private static Dictionary<ddouble, BesselYEtaTable> eta_table = new();
        private static Dictionary<ddouble, BesselYXiTable> xi_table = new();

        public static ddouble BesselJ(int n, ddouble x, int m) {
            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return ((n & 1) == 0) ? BesselJ(-n, x, m) : -BesselJ(-n, x, m);
            }
            if (n == 0) {
                return BesselJ0(x, m);
            }
            if (n == 1) {
                return BesselJ1(x, m);
            }

            ddouble f0 = 1e-256, f1 = ddouble.Zero, fn = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);

                if (k - 1 == n) {
                    fn = f0;
                }
            }

            lambda = 2 * lambda + f0;

            ddouble yn = fn / lambda;

            return yn;
        }

        public static ddouble BesselJ(ddouble nu, ddouble x, int m) {
            int n = (int)ddouble.Floor(nu);
            ddouble alpha = nu - n;

            if (alpha == 0) {
                return BesselJ(n, x, m);
            }

            if (ddouble.Abs(nu - ddouble.Round(nu)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(nu));
            }

            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!phi_table.ContainsKey(alpha)) {
                phi_table.Add(alpha, new BesselJPhiTable(alpha));
            }

            BesselJPhiTable phi = phi_table[alpha];

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            if (n >= 0) {
                ddouble fn = ddouble.Zero;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0 * phi[k / 2];
                    }

                    (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);

                    if (k - 1 == n) {
                        fn = f0;
                    }
                }

                lambda += f0 * phi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                ddouble yn = fn / lambda;

                return yn;
            }
            else {
                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0 * phi[k / 2];
                    }

                    (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
                }

                lambda += f0 * phi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                for (int k = 0; k > n; k--) {
                    (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
                }

                ddouble yn = f0 / lambda;

                return yn;
            }
        }

        public static ddouble BesselJ0(ddouble x, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);
            }

            lambda = 2 * lambda + f0;

            ddouble y0 = f0 / lambda;

            return y0;
        }

        public static ddouble BesselJ1(ddouble x, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);
            }

            lambda = 2 * lambda + f0;

            ddouble y1 = f1 / lambda;

            return y1;
        }

        public static ddouble BesselY(int n, ddouble x, int m) {
            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return ((n & 1) == 0) ? BesselY(-n, x, m) : -BesselY(-n, x, m);
            }
            if (n == 0) {
                return BesselY0(x, m);
            }
            if (n == 1) {
                return BesselY1(x, m);
            }

            if (!eta_table.ContainsKey(0)) {
                eta_table.Add(0, new BesselYEtaTable(0));
            }

            BesselYEtaTable eta = eta_table[0];

            if (!xi_table.ContainsKey(0)) {
                xi_table.Add(0, new BesselYXiTable(0, eta));
            }

            BesselYXiTable xi = xi_table[0];

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble se = ddouble.Zero, sx = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;

                    se += f0 * eta[k / 2];
                }
                else if (k >= 3) {
                    sx += f0 * xi[k];
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);
            }

            lambda = 2 * lambda + f0;

            ddouble c = ddouble.Log(x / 2) + ddouble.EulerGamma;

            ddouble y0 = se + f0 * c;
            ddouble y1 = sx - v * f0 + (c - 1) * f1;

            for (int k = 1; k < n; k++) {
                (y1, y0) = ((2 * k) * v * y1 - y0, y1);
            }

            ddouble yn = 2 * y1 / (lambda * ddouble.PI);

            return yn;
        }

        public static ddouble BesselY(ddouble nu, ddouble x, int m) {
            int n = (int)ddouble.Floor(nu);
            ddouble alpha = nu - n;

            if (alpha == 0) {
                return BesselY(n, x, m);
            }

            if (ddouble.Abs(nu - ddouble.Round(nu)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(nu));
            }

            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!eta_table.ContainsKey(alpha)) {
                eta_table.Add(alpha, new BesselYEtaTable(alpha));
            }

            BesselYEtaTable eta = eta_table[alpha];

            if (!xi_table.ContainsKey(alpha)) {
                xi_table.Add(alpha, new BesselYXiTable(alpha, eta));
            }

            BesselYXiTable xi = xi_table[alpha];

            if (!phi_table.ContainsKey(alpha)) {
                phi_table.Add(alpha, new BesselJPhiTable(alpha));
            }

            BesselJPhiTable phi = phi_table[alpha];

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble se = ddouble.Zero, sxo = ddouble.Zero, sxe = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0 * phi[k / 2];

                    se += f0 * eta[k / 2];
                    sxe += f0 * xi[k];
                }
                else if (k >= 3) {
                    sxo += f0 * xi[k];
                }

                (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
            }

            ddouble s = ddouble.Pow(2 * v, alpha), sqs = s * s;

            lambda += f0 * phi[0];
            lambda *= s;

            ddouble rcot = 1d / ddouble.TanPI(alpha), rgamma = ddouble.Gamma(1 + alpha), rsqgamma = rgamma * rgamma;
            ddouble r = 2 * ddouble.RcpPI * sqs;
            ddouble p = sqs * rsqgamma * ddouble.RcpPI;

            ddouble eta0 = rcot - p / alpha;
            ddouble xi0 = -2 * v * p;
            ddouble xi1 = rcot + p * (alpha * (alpha + 1) + 1) / (alpha * (alpha - 1));

            ddouble y0 = r * se + eta0 * f0;
            ddouble y1 = r * (3 * alpha * v * sxe + sxo) + xi0 * f0 + xi1 * f1;

            ddouble m0 = y0 / lambda;
            ddouble m1 = y1 / lambda;

            if (n == 0) {
                ddouble yn = y0 / lambda;

                return yn;
            }
            if (n == 1) {
                ddouble yn = y1 / lambda;

                return yn;
            }
            if (n >= 0) {
                for (int k = 1; k < n; k++) {
                    (y1, y0) = ((2 * (k + alpha)) * v * y1 - y0, y1);
                }

                ddouble yn = y1 / lambda;

                return yn;
            }
            else {
                for (int k = 0; k > n; k--) {
                    (y0, y1) = ((2 * (k + alpha)) * v * y0 - y1, y0);
                }

                ddouble yn = y0 / lambda;

                return yn;
            }
        }

        public static ddouble BesselY0(ddouble x, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!eta_table.ContainsKey(0)) {
                eta_table.Add(0, new BesselYEtaTable(0));
            }

            BesselYEtaTable eta = eta_table[0];

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble se = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;

                    se += f0 * eta[k / 2];
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);
            }

            lambda = 2 * lambda + f0;

            ddouble y0 = 2 * (se + f0 * (ddouble.Log(x / 2) + ddouble.EulerGamma)) / (ddouble.PI * lambda);

            return y0;
        }

        public static ddouble BesselY1(ddouble x, int m) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!xi_table.ContainsKey(0)) {
                if (!eta_table.ContainsKey(0)) {
                    eta_table.Add(0, new BesselYEtaTable(0));
                }

                xi_table.Add(0, new BesselYXiTable(0, eta_table[0]));
            }

            BesselYXiTable xi = xi_table[0];

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble sx = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                if ((k & 1) == 0) {
                    lambda += f0;
                }
                else if (k >= 3) {
                    sx += f0 * xi[k];
                }

                (f0, f1) = ((2 * k) * v * f0 - f1, f0);
            }

            lambda = 2 * lambda + f0;

            ddouble y1 = 2 * (sx - v * f0 + (ddouble.Log(x / 2) + ddouble.EulerGamma - 1) * f1) / (lambda * ddouble.PI);

            return y1;
        }

        public static ddouble BesselI(int n, ddouble x, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (n < 0) {
                return BesselI(-n, x, m, scale);
            }

            ddouble f0 = 1e-256, f1 = ddouble.Zero, lambda = ddouble.Zero, fn = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                lambda += f0;

                (f0, f1) = ((2 * k) * v * f0 + f1, f0);

                if (k - 1 == n) {
                    fn = f0;
                }
            }

            lambda = 2 * lambda + f0;

            ddouble yn = fn / lambda;

            if (!scale) {
                yn *= ddouble.Exp(x);
            }

            return yn;
        }

        public static ddouble BesselI(ddouble nu, ddouble x, int m, bool scale = false) {
            int n = (int)ddouble.Floor(nu);
            ddouble alpha = nu - n;

            if (alpha == 0) {
                return BesselI(n, x, m, scale);
            }

            if (ddouble.Abs(nu - ddouble.Round(nu)) < 1e-3) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 10^-3 and nu != round(nu))",
                    nameof(nu));
            }

            if (m < 2 || (m & 1) != 0 || n >= m) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            if (!psi_table.ContainsKey(alpha)) {
                psi_table.Add(alpha, new BesselIPsiTable(alpha));
            }

            BesselIPsiTable psi = psi_table[alpha];

            ddouble g0 = 1e-256, g1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            if (n >= 0) {
                ddouble gn = ddouble.Zero;

                for (int k = m; k >= 1; k--) {
                    lambda += g0 * psi[k];

                    (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);

                    if (k - 1 == n) {
                        gn = g0;
                    }
                }

                lambda += g0 * psi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                ddouble yn = gn / lambda;

                if (!scale) {
                    yn *= ddouble.Exp(x);
                }

                return yn;
            }
            else {
                for (int k = m; k >= 1; k--) {
                    lambda += g0 * psi[k];

                    (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);
                }

                lambda += g0 * psi[0];
                lambda *= ddouble.Pow(2 * v, alpha);

                for (int k = 0; k > n; k--) {
                    (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);
                }

                ddouble yn = g0 / lambda;

                if (!scale) {
                    yn *= ddouble.Exp(x);
                }

                return yn;
            }
        }

        public static ddouble BesselI0(ddouble x, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble g0 = 1e-256, g1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                lambda += g0;

                (g0, g1) = ((2 * k) * v * g0 + g1, g0);
            }

            lambda = 2 * lambda + g0;

            ddouble y0 = g0 / lambda;

            if (!scale) {
                y0 *= ddouble.Exp(x);
            }

            return y0;
        }

        public static ddouble BesselI1(ddouble x, int m, bool scale = false) {
            if (m < 2 || (m & 1) != 0) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble g0 = 1e-256, g1 = ddouble.Zero, lambda = ddouble.Zero;
            ddouble v = 1d / x;

            for (int k = m; k >= 1; k--) {
                lambda += g0;

                (g0, g1) = ((2 * k) * v * g0 + g1, g0);
            }

            lambda = 2 * lambda + g0;

            ddouble y1 = g1 / lambda;

            if (!scale) {
                y1 *= ddouble.Exp(x);
            }

            return y1;
        }

        public static (ddouble y, int m) BesselJ(ddouble nu, ddouble x, ddouble eps, int max_m = 512) {
            ddouble y_prev = ddouble.NaN, dy = ddouble.NaN, dy_prev = ddouble.NaN;

            int n = (int)ddouble.Ceiling(ddouble.Abs(nu));

            for (int m = n * 4 + 2; m <= max_m; m += 2) {
                ddouble y = BesselJ(nu, x, m);

                dy = ddouble.Abs(y - y_prev);
                y_prev = y;

                if (dy < eps && dy >= dy_prev) {
                    return (y, m - 2);
                }

                dy_prev = dy;
            }

            return (y_prev, max_m);
        }

        public static (ddouble y, int m) BesselY(ddouble nu, ddouble x, ddouble eps, int max_m = 512) {
            ddouble y_prev = ddouble.NaN, dy = ddouble.NaN, dy_prev = ddouble.NaN;

            int n = (int)ddouble.Ceiling(ddouble.Abs(nu));

            for (int m = 40; m <= max_m; m += 2) {
                ddouble y = BesselY(nu, x, m);

                dy = ddouble.Abs(y - y_prev);
                y_prev = y;

                if (dy < eps && dy >= dy_prev) {
                    return (y, m - 2);
                }

                dy_prev = dy;
            }

            return (y_prev, max_m);
        }

        public static (ddouble y, int m) BesselI(ddouble nu, ddouble x, ddouble eps, int max_m = 512) {
            ddouble y_prev = ddouble.NaN, dy = ddouble.NaN, dy_prev = ddouble.NaN;

            int n = (int)ddouble.Ceiling(ddouble.Abs(nu));

            for (int m = n * 4 + 2; m <= max_m; m += 2) {
                ddouble y = BesselI(nu, x, m, scale: true);

                dy = ddouble.Abs(y - y_prev);
                y_prev = y;

                if (dy < eps && dy >= dy_prev) {
                    return (y, m - 2);
                }

                dy_prev = dy;
            }

            return (y_prev, max_m);
        }

        public static ddouble BesselJ(int n, ddouble x) {
            int m = (int)Math.Ceiling(20 + (double)x * 0.825) * 2;

            ddouble m0 = 1e-256, m1 = ddouble.Zero, d = ddouble.Zero, f = ddouble.Zero;
            ddouble v = 1d / x;

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

        public class BesselJPhiTable {
            private readonly ddouble alpha;
            private readonly List<ddouble> table = new();

            private ddouble g;

            public BesselJPhiTable(ddouble alpha) {
                if (!(alpha > 0) || alpha >= 1) {
                    throw new ArgumentOutOfRangeException(nameof(alpha));
                }

                this.alpha = alpha;

                ddouble phi0 = ddouble.Gamma(1 + alpha);
                ddouble phi1 = phi0 * (alpha + 2);

                this.g = phi0;

                this.table.Add(phi0);
                this.table.Add(phi1);
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < table.Count) {
                    return table[n];
                }

                for (int m = table.Count; m <= n; m++) {
                    g = g * (alpha + m - 1) / m;

                    ddouble phi = g * (alpha + 2 * m);

                    table.Add(phi);
                }

                return table[n];
            }
        };

        public class BesselIPsiTable {
            private readonly ddouble alpha;
            private readonly List<ddouble> table = new();

            private ddouble g;

            public BesselIPsiTable(ddouble alpha) {
                if (!(alpha > 0) || alpha >= 1) {
                    throw new ArgumentOutOfRangeException(nameof(alpha));
                }

                this.alpha = alpha;

                ddouble psi0 = ddouble.Gamma(1 + alpha);
                ddouble psi1 = 2 * psi0 * (1 + alpha);

                this.g = 2 * psi0;

                this.table.Add(psi0);
                this.table.Add(psi1);
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < table.Count) {
                    return table[n];
                }

                for (int m = table.Count; m <= n; m++) {
                    g = g * (2 * alpha + m - 1) / m;

                    ddouble phi = g * (alpha + m);

                    table.Add(phi);
                }

                return table[n];
            }
        };

        public class BesselYEtaTable {
            private readonly ddouble alpha;
            private readonly List<ddouble> table = new();

            private ddouble g;

            public BesselYEtaTable(ddouble alpha) {
                if (!(alpha >= 0) || alpha >= 1) {
                    throw new ArgumentOutOfRangeException(nameof(alpha));
                }

                this.alpha = alpha;
                this.table.Add(ddouble.NaN);

                if (alpha > 0) {
                    ddouble c = ddouble.Gamma(1 + alpha);
                    c *= c;
                    this.g = 1 / (1 - alpha) * c;

                    ddouble eta1 = (alpha + 2) * g;

                    this.table.Add(eta1);
                }
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < table.Count) {
                    return table[n];
                }

                for (int m = table.Count; m <= n; m++) {
                    if (alpha > 0) {
                        g = -g * (alpha + m - 1) * (2 * alpha + m - 1) / (m * (m - alpha));

                        ddouble eta = g * (alpha + 2 * m);

                        table.Add(eta);
                    }
                    else {
                        ddouble eta = (ddouble)2d / m;

                        table.Add(((m & 1) == 1) ? eta : -eta);
                    }
                }

                return table[n];
            }
        };

        public class BesselYXiTable {
            private readonly ddouble alpha;
            private readonly List<ddouble> table = new();
            private readonly BesselYEtaTable eta;

            public BesselYXiTable(ddouble alpha, BesselYEtaTable eta) {
                if (!(alpha >= 0) || alpha >= 1) {
                    throw new ArgumentOutOfRangeException(nameof(alpha));
                }

                this.alpha = alpha;
                this.table.Add(ddouble.NaN);
                this.table.Add(ddouble.NaN);

                this.eta = eta;
            }

            public ddouble this[int n] => Value(n);

            private ddouble Value(int n) {
                if (n < 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                if (n < table.Count) {
                    return table[n];
                }

                for (int m = table.Count; m <= n; m++) {
                    if (alpha > 0) {
                        if ((m & 1) == 0) {
                            table.Add(eta[m / 2]);
                        }
                        else {
                            table.Add((eta[m / 2] - eta[m / 2 + 1]) / 2);
                        }
                    }
                    else {
                        if ((m & 1) == 1) {
                            ddouble xi = (ddouble)(2 * (m / 2) + 1) / ((m / 2) * ((m / 2) + 1));
                            table.Add(((m & 2) > 0) ? xi : -xi);
                        }
                        else {
                            table.Add(ddouble.NaN);
                        }
                    }
                }

                return table[n];
            }
        };
    }
}
