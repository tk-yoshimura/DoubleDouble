using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            const int max_terms = 1024;

            const int n = 4;
            ddouble q = 17.4478865152084;
            ddouble a = ddouble.MathieuA(n, q);
            
            ddouble[] cs = MathieuCCoef(n, q, a, max_terms);
            
            ddouble[] ms = new ddouble[cs.Length];

            ms[0] = a * cs[0] - q * cs[1];
            ms[1] = (a - 4) * cs[1] - q * (2 * cs[0] + cs[2]);

            for (int m = 2; m < cs.Length - 1; m++) {
                ms[m] = (a - 4 * m * m) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }

            /*
            ms[0] = (a - 1 - q) * cs[0] - q * cs[1];

            for (int m = 1; m < cs.Length - 1; m++) {
                ms[m] = (a - (2 * m + 1) * (2 * m + 1)) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }
            */

            /*
            ms[0] = (a - 1 + q) * cs[0] - q * cs[1];

            for (int m = 1; m < cs.Length - 1; m++) {
                ms[m] = (a - (2 * m + 1) * (2 * m + 1)) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }
            */

            /*
            ms[0] = (a - 4) * cs[0] - q * cs[1];
            
            for (int m = 1; m < cs.Length - 1; m++) {
                ms[m] = (a - 4 * (m + 1) * (m + 1)) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }
            */

            Console.WriteLine("END");
            Console.Read();
        }

        private static ddouble[] MathieuCCoef(int n, ddouble q, ddouble a, int max_terms) {
            ddouble inv_q = 1d / q;

            ddouble[] cs = new ddouble[max_terms];
            (cs[^2], cs[^1]) = (ddouble.Epsilon, ddouble.Zero);

            for (int m = max_terms - 2, k = checked(2 * m + (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                ddouble c = (a - k * k) * cs[m] * inv_q - cs[m + 1];

                cs[m - 1] = c;

                if (Math.ILogB(cs[m - 1].Hi) > -256) {
                    for (int j = m - 1; j < cs.Length; j++) {
                        if (Math.ILogB(cs[j - 1].Hi) < -512 && Math.ILogB(cs[j].Hi) < -512) {
                            cs = cs[..j];
                            cs[^1] = ddouble.Zero;
                            break;
                        }

                        cs[j] = ddouble.Ldexp(cs[j], -256);
                    }
                }
            }

            if ((n & 1) == 0) {
                if (ddouble.Abs(cs[2]) > ddouble.Abs(cs[3])) {
                    (cs[0], cs[1], ddouble r2) = SolveC2(2, a, q, 0, 4, cs[2]);
                }
                else {
                    (cs[0], cs[1], cs[2], ddouble r3) = SolveC3(2, a, q, 0, 4, 16, cs[3]);
                }
            }
            else {
                if (ddouble.Abs(cs[2]) > ddouble.Abs(cs[3])) {
                    (cs[0], cs[1], ddouble r2) = SolveC2(1, a, q, 1 + q, 9, cs[2]);
                }
                else {
                    (cs[0], cs[1], cs[2], ddouble r3) = SolveC3(1, a, q, 1 + q, 9, 25, cs[3]);
                }
            }

            if (ddouble.IsInfinity(cs[0])) {
                return new ddouble[] { 1 };
            }

            ddouble norm = (cs[0] * cs[0]) * (((n & 1) == 0) ? 2 : 1);
            for (int i = 1; i < cs.Length; i++) {
                norm += cs[i] * cs[i];
            }

            ddouble r = 1d / ddouble.Sqrt(norm) * cs[0].Sign;
            for (int i = 0; i < cs.Length; i++) {
                cs[i] *= r;
            }

            ddouble threshold = ddouble.Ldexp(cs.Select(c => ddouble.Abs(c)).Max(), -256);
            for (int i = cs.Length - 1; i > 0; i--) {
                if (ddouble.Abs(cs[i]) > threshold) {
                    cs = cs[..i];
                    break;
                }
            }

            return cs;
        }

        private static ddouble[] MathieuSCoef(int n, ddouble q, ddouble a, int max_terms) {
            ddouble inv_q = 1d / q;

            ddouble[] cs = new ddouble[max_terms];
            (cs[^2], cs[^1]) = (ddouble.Epsilon, ddouble.Zero);

            for (int m = max_terms - 2, k = checked(2 * m + 2 - (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                ddouble c = (a - k * k) * cs[m] * inv_q - cs[m + 1];

                cs[m - 1] = c;

                if (Math.ILogB(cs[m - 1].Hi) > -256) {
                    for (int j = m - 1; j < cs.Length; j++) {
                        if (Math.ILogB(cs[j - 1].Hi) < -512 && Math.ILogB(cs[j].Hi) < -512) {
                            cs = cs[..j];
                            cs[^1] = ddouble.Zero;
                            break;
                        }

                        cs[j] = ddouble.Ldexp(cs[j], -256);
                    }
                }
            }

            if ((n & 1) == 0) {
                if (ddouble.Abs(cs[2]) > ddouble.Abs(cs[3])) {
                    (cs[0], cs[1], ddouble r2) = SolveC2(1, a, q, 4, 16, cs[2]);
                }
                else {
                    (cs[0], cs[1], cs[2], ddouble r3) = SolveC3(1, a, q, 4, 16, 36, cs[3]);
                }
            }
            else {
                if (ddouble.Abs(cs[2]) > ddouble.Abs(cs[3])) {
                    (cs[0], cs[1], ddouble r2) = SolveC2(1, a, q, 1 - q, 9, cs[2]);
                }
                else {
                    (cs[0], cs[1], cs[2], ddouble r3) = SolveC3(1, a, q, 1 - q, 9, 25, cs[3]);
                }
            }

            if (ddouble.IsInfinity(cs[0])) {
                return new ddouble[] { 1 };
            }

            ddouble norm = ddouble.Zero;
            for (int i = 0; i < cs.Length; i++) {
                norm += cs[i] * cs[i];
            }

            ddouble r = 1d / ddouble.Sqrt(norm) * cs[0].Sign;
            for (int i = 0; i < cs.Length; i++) {
                cs[i] *= r;
            }

            ddouble threshold = ddouble.Ldexp(cs.Select(c => ddouble.Abs(c)).Max(), -256);
            for (int i = cs.Length - 1; i > 0; i--) {
                if (ddouble.Abs(cs[i]) > threshold) {
                    cs = cs[..i];
                    break;
                }
            }

            return cs;
        }

        private static (ddouble c0, ddouble c1, ddouble r) SolveC2(int k, ddouble a, ddouble q, ddouble b0, ddouble b1, ddouble c2) {
            ddouble r = (a - b0) * (a - b1) - k * q * q;
            ddouble m = c2 * q / r;

            ddouble c0 = m * q;
            ddouble c1 = m * (a - b0);

            return (c0, c1, r);
        }

        private static (ddouble c0, ddouble c1, ddouble c2, ddouble r) SolveC3(int k, ddouble a, ddouble q, ddouble b0, ddouble b1, ddouble b2, ddouble c3) {
            ddouble r = (a - b0) * (a - b1) * (a - b2) - ((a - b0) + k * (a - b2)) * q * q;
            ddouble m = c3 * q / r;

            ddouble c0 = m * q * q;
            ddouble c1 = m * q * (a - b0);
            ddouble c2 = m * ((a - b0) * (a - b1) - k * q * q);

            return (c0, c1, c2, r);
        }
    }
}
