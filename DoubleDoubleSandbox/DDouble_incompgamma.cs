using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompleteGammaPrototype {

        public static ddouble LowerIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return PositiveInfinity;
            }

            ddouble g = Gamma(s);
            if (IsInfinity(g)) {
                return NaN;
            }

            ddouble f = 1;

            for (int n = m; n >= 0; n--) {
                f = s + (2 * n) - (f * (s + n) * x) / (((n + 1) * x) + f * (s + (2 * n + 1)));
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static ddouble UpperIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return -Ei(-x);
            }

            ddouble f = 1;

            for (int n = m; n >= 1; n--) {
                f = x + f * (n - s) / (f + n);
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static (ddouble y, int m) LowerIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = LowerIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = LowerIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static (ddouble y, int m) UpperIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = UpperIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = UpperIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static ddouble UpperIncompleteGamma(ddouble nu, ddouble x) {
            if (x <= 3) {
                return UpperIncompleteGammaNearZero.Value(nu, x);
            }

            throw new NotImplementedException();
        }

        internal static class UpperIncompleteGammaNearZero {

            public static ddouble Value(ddouble nu, ddouble x) {
                ddouble a = UpperIncompleteGammaNearZero.A1(nu);
                ddouble a0 = (1 + nu) * a - 1;
                ddouble phi = UpperIncompleteGammaNearZero.Phi(nu, x);
                ddouble g0 = Gamma(1 + nu), g = g0 * (1 + nu);

                ddouble s = a0 + phi / g0 + x * (a + phi / g);

                ddouble u = x * x;

                for (int k = 2; k < TaylorSequence.Count; k++) {
                    a = 1d / (k + nu) * (a + TaylorSequence[k]);
                    g *= k + nu;

                    ddouble ds = u * (a + phi / g);
                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x;
                    s = s_next;
                }

                ddouble y = g0 * Exp(-x) * s;

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

                    return (1 - s) / (1 + nu);
                }
            }

            public static ddouble Phi(ddouble nu, ddouble x) {
                ddouble x_nu = Pow(x, nu);

                if (x_nu <= 0.5d || x_nu >= 2d) {
                    return (1 - x_nu) / nu;
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
    }
}