using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            const int max_terms = 1024;

            const int n = 4;
            ddouble q = 10;

            ddouble a = ddouble.MathieuB(n, q);

            ddouble[] cs = MathieuSCoef(n, q, a, max_terms);

            ddouble[] ms = new ddouble[cs.Length];

            /*
            ms[0] = a * cs[0] - q * cs[1];
            ms[1] = (a - 4) * cs[1] - q * (2 * cs[0] + cs[2]);

            for (int m = 2; m < cs.Length - 1; m++) {
                ms[m] = (a - 4 * m * m) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }
            */

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

            ms[0] = (a - 4) * cs[0] - q * cs[1];
            
            for (int m = 1; m < cs.Length - 1; m++) {
                ms[m] = (a - 4 * (m + 1) * (m + 1)) * cs[m] - q * (cs[m - 1] + cs[m + 1]);
            }

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
                ddouble s = q / (a * (a - 4d) - 2 * q * q) * cs[2];

                cs[1] = s * a;
                cs[0] = s * q;
            }
            else {
                ddouble s = q / ((a - q - 1d) * (a - 9d) - q * q) * cs[2];

                cs[1] = s * (a - q - 1d);
                cs[0] = s * q;
            }

            if (ddouble.IsInfinity(cs[0])) {
                return new ddouble[] { cs[0].Sign };
            }

            ddouble norm = (cs[0] * cs[0]) * (((n & 1) == 0) ? 2 : 1);
            for (int i = 1; i < cs.Length; i++) {
                norm += cs[i] * cs[i];
            }

            ddouble r = 1d / ddouble.Sqrt(norm);
            for (int i = 0; i < cs.Length; i++) {
                cs[i] *= r;
            }

            ddouble threshold = ddouble.Ldexp(cs.Select(c => ddouble.Abs(c)).Max(), -128);
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
                ddouble s = q / ((a - 4d) * (a - 16d) - q * q) * cs[2];

                cs[1] = s * (a - 4d);
                cs[0] = s * q;
            }
            else {
                ddouble s = q / ((a + q - 1d) * (a - 9d) - q * q) * cs[2];

                cs[1] = s * (a + q - 1d);
                cs[0] = s * q;
            }

            if (ddouble.IsInfinity(cs[0])) {
                return new ddouble[] { cs[0].Sign };
            }

            ddouble norm = ddouble.Zero;
            for (int i = 0; i < cs.Length; i++) {
                norm += cs[i] * cs[i];
            }

            ddouble r = 1d / ddouble.Sqrt(norm);
            for (int i = 0; i < cs.Length; i++) {
                cs[i] *= r;
            }

            ddouble threshold = ddouble.Ldexp(cs.Select(c => ddouble.Abs(c)).Max(), -128);
            for (int i = cs.Length - 1; i > 0; i--) {
                if (ddouble.Abs(cs[i]) > threshold) {
                    cs = cs[..i];
                    break;
                }
            }

            return cs;
        }
    }
}
