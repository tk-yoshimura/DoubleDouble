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

            public static ReadOnlyCollection<ddouble> TaylorA1ZeroTable
             = new(new ddouble[] {
                (+1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
                (-1, -1, 0xA7E7A01357D16E75uL, 0xC24856F3BD611D64uL),
                (-1, -5, 0xAC0AF47D13823E47uL, 0xA15A643C9C84B042uL),
                (+1, -3, 0xAA891905A1FDF2EFuL, 0xD37FB09A1EF84DA1uL),
                (-1, -5, 0xACD7881E1A0493DFuL, 0x1B4048388CACA42EuL),
                (-1, -7, 0x9DA5794241F10A71uL, 0xB79E818273539ADCuL),
                (+1, -8, 0xEC8CE293FB058CADuL, 0xD1D69DD3DC1E1318uL),
                (-1, -10, 0x98B889671D153DE9uL, 0x6EDBA83588E959C6uL),
                (-1, -13, 0xE1B27F378AB1E74CuL, 0x7C04703C8691B6EDuL),
                (+1, -13, 0x86453C66CFCE8D3CuL, 0xD83432FACDB927F8uL),
                (-1, -16, 0xA8E7457A3F55EFEDuL, 0x92173AFE34B827DAuL),
                (-1, -20, 0xA7D6A0FE1A7DD901uL, 0x776AB160DC7CCB6CuL),
                (+1, -20, 0x981284EDE06F1640uL, 0xF6FFCB7E4E64F0B8uL),
                (-1, -23, 0xDCCC33336112E8E8uL, 0x9722EF2CE809707CuL),
                (+1, -28, 0xD225BDD116B14565uL, 0x21CC6FD93419DC08uL),
                (+1, -28, 0xABDE1FE1C2199FD9uL, 0xEAFBBF416E76D1E0uL),
                (-1, -30, 0xA25A676E51C47BE3uL, 0x89819C393897FFDEuL),
                (+1, -34, 0xE573B3AE0C30362FuL, 0xBF7D38399563B603uL),
                (+1, -37, 0x88E832DFD7833A2DuL, 0x6B19F0B77E71C665uL),
                (-1, -38, 0x8211DD64651FD552uL, 0x333C5E6F89FA0414uL),
                (+1, -41, 0x8F900A8991E681C8uL, 0xF6862A8BDDBA9233uL),
                (-1, -46, 0xB965C4752D7373BDuL, 0x2A90DA417F9F6A64uL),
            });
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