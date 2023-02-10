using DoubleDouble.Utils;
using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble LowerIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d || nu > 128d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < UpperIncompleteGammaNearZero.Eps) {
                return 0;
            }
            if (nu < UpperIncompleteGammaNearZero.Eps) {
                return PositiveInfinity;
            }

            if (x < 1.0975d * (double)nu + 0.7725d) {
                return LowerIncompleteGammaCFrac.Value(nu, x);
            }
            else {
                return Gamma(nu) - UpperIncompleteGamma(nu, x);
            }
        }

        public static ddouble UpperIncompleteGamma(ddouble nu, ddouble x) {
            if (nu < 0d || nu > 128d) {
                throw new ArgumentOutOfRangeException(nameof(nu));
            }
            if (x < 0d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(nu) || IsNaN(x)) {
                return NaN;
            }

            if (x < UpperIncompleteGammaNearZero.Eps) {
                return Gamma(nu);
            }
            if (nu < UpperIncompleteGammaNearZero.Eps) {
                return -Ei(-x);
            }

            if (x <= 3d) {
                return UpperIncompleteGammaNearZero.Value(nu, x);
            }
            else {
                return UpperIncompleteGammaCFrac.Value(nu, x);
            }
        }

        internal static class UpperIncompleteGammaNearZero {
            public static double Eps = Math.ScaleB(1, -105);

            public static ddouble Value(ddouble nu, ddouble x) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                ddouble a = UpperIncompleteGammaNearZero.A1(alpha);
                ddouble a0 = (1d + alpha) * a - 1d;
                ddouble phi = UpperIncompleteGammaNearZero.Phi(alpha, x);
                ddouble g0 = Gamma(1d + alpha), g = g0 * (1d + alpha);

                ddouble s = a0 + phi / g0 + x * (a + phi / g);

                ddouble u = x * x;

                for (int k = 2; k < TaylorSequence.Count; k++) {
                    a = 1d / (k + alpha) * (a + TaylorSequence[k]);
                    g *= k + alpha;

                    ddouble ds = u * (a + phi / g);
                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x;
                    s = s_next;
                }

                ddouble expx = Exp(-x);

                ddouble y = g0 * expx * s;

                if (n > 0) {
                    ddouble powx = Pow(x, alpha);

                    for (int k = 0; k < n; k++) {
                        y = (alpha + k) * y + powx * expx;
                        powx *= x;
                    }
                }

                return y;
            }

            public static ddouble A1(ddouble nu) {
                if (nu >= 0.15625d) {
                    return (1d - 1d / Gamma(2 + nu)) / nu;
                }
                else {
                    ddouble s = TaylorA1ZeroTable[^1];
                    for (int i = TaylorA1ZeroTable.Count - 2; i >= 0; i--) {
                        s = s * nu + TaylorA1ZeroTable[i];
                    }

                    return (1d - s) / (1d + nu);
                }
            }

            public static ddouble Phi(ddouble nu, ddouble x) {
                ddouble x_nu = Pow(x, nu);

                if (x_nu <= 0.5d || x_nu >= 2d) {
                    return (1d - x_nu) / nu;
                }

                ddouble logx = Log(x);
                ddouble v = nu * logx, s = 1, u = v;

                for (int k = 2; k < TaylorSequence.Count; k++) {
                    ddouble s_next = s + u * TaylorSequence[k];

                    if (s == s_next) {
                        break;
                    }

                    u *= v;
                    s = s_next;
                }

                ddouble y = -s * logx;

                return y;
            }

            public static ReadOnlyCollection<ddouble> TaylorA1ZeroTable { get; } =
                ResourceUnpack.NumTable(Resource.IncompGammaTable)[nameof(TaylorA1ZeroTable)];
        }

        internal static class UpperIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                double log2x = Math.Log2((double)x);

                int m = (x > 64) ? 14 : (int)Math.Pow(2, ((0.04525 * log2x - 1) * log2x + 8.250)) + 1;

                ddouble f = 1;

                for (int i = m; i >= 1; i--) {
                    f = x + f * (i - alpha) / (f + i);
                }

                ddouble powx = Pow(x, alpha), expx = Exp(-x);

                ddouble y = powx * expx / f;

                if (n > 0) {
                    for (int k = 0; k < n; k++) {
                        y = (alpha + k) * y + powx * expx;
                        powx *= x;
                    }
                }

                return y;
            }
        }

        internal static class LowerIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x) {
                double log2x = Math.Log2((double)x);

                int m = (int)Math.Pow(2, ((0.01478 * log2x + 0.2829) * log2x + 3.528)) + 1;

                ddouble f = 1;

                for (int i = m; i >= 0; i--) {
                    f = nu + (2 * i) - (f * (nu + i) * x) / (((i + 1) * x) + f * (nu + (2 * i + 1)));
                }

                ddouble powx = Pow(x, nu), expx = Exp(-x);

                ddouble y = powx * expx / f;

                return y;
            }
        }
    }
}