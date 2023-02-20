using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            const int max_terms = 1024;

            const int n = 0;
            ddouble q = 1;
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

            ddouble c_sum = cs.Sum();

            Console.WriteLine(c_sum);


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
                        if (Math.ILogB(cs[j - 1].Hi) < -512 && Math.ILogB(cs[j].Hi) < -512 && m >= 32) {
                            cs = cs[..j];
                            cs[^1] = ddouble.Zero;
                            break;
                        }

                        cs[j] = ddouble.Ldexp(cs[j], -256);
                    }
                }
            }

            for (int m = 8; m >= 2; m--) {
                if (m == 2 || ddouble.Abs(cs[m]) > ddouble.Abs(cs[m - 1])) {
                    ddouble[] scs;
                    ddouble rm, d;

                    if ((n & 1) == 0) {
                        (scs, rm, d) = Solve(a, q, m, 2, 0, 0, cs[m]);
                    }
                    else {
                        (scs, rm, d) = Solve(a, q, m, 1, 1, 1 + q, cs[m]);
                    }

                    if (m == 2 || d < 1) {
                        Array.Copy(scs, cs, m);
                        break;
                    }
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
                        if (Math.ILogB(cs[j - 1].Hi) < -512 && Math.ILogB(cs[j].Hi) < -512 && m >= 32) {
                            cs = cs[..j];
                            cs[^1] = ddouble.Zero;
                            break;
                        }

                        cs[j] = ddouble.Ldexp(cs[j], -256);
                    }
                }
            }

            for (int m = 8; m >= 2; m--) {
                if (m == 2 || ddouble.Abs(cs[m]) > ddouble.Abs(cs[m - 1])) {
                    ddouble[] scs;
                    ddouble rm, d;

                    if ((n & 1) == 0) {
                        (scs, rm, d) = Solve(a, q, m, 1, 2, 4, cs[m]);
                    }
                    else {
                        (scs, rm, d) = Solve(a, q, m, 1, 1, 1 - q, cs[m]);
                    }

                    if (m == 2 || d < 1) {
                        Array.Copy(scs, cs, m);
                        break;
                    }
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

        private static (ddouble[] cs, ddouble r, ddouble d) Solve(ddouble a, ddouble q, int m, int k, int s, ddouble r0, ddouble cn) {
            if (m < 2) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble[] cs = new ddouble[m], ts = new ddouble[m + 1], qs = new ddouble[m];
            ddouble sq_q = q * q;

            (ts[0], ts[1], ts[2]) = (1d, a - r0, (a - r0) * (a - checked((2 + s) * (2 + s))) - k * q * q);
            (qs[0], qs[1]) = (cn * q, cn * sq_q);

            for (int i = 2; i < m; i++) {
                qs[i] = qs[i - 1] * q;
                ts[i + 1] = ts[i] * (a - checked((2 * i + s) * (2 * i + s))) - ts[i - 1] * sq_q;
            }

            for (int i = 0; i < cs.Length; i++) {
                cs[i] = qs[m - i - 1] * ts[i] / ts[m];
            }

            ddouble d = ddouble.Abs(ts[m - 1] / ts[m]);

            return (cs, ts[m], d);
        }
    }
}
