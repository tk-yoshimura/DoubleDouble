using DoubleDouble;
using static DoubleDouble.ddouble;
using System;
using System.IO;
using System.Diagnostics;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble t1 = UpperIncompleteGammaCFrac.Value(3.5, 4, 1024);
            ddouble t2 = UpperIncompleteGammaCFrac.ValueType2(3.5, 4);

            Console.WriteLine("END");
            Console.Read();
        }

        internal static class UpperIncompleteGammaCFrac {
            public static ddouble Value(ddouble nu, ddouble x, int m) {
                ddouble f = 1;

                for (int i = m; i >= 1; i--) {
                    f = x + f * (i - nu) / (f + i);
                }

                return f;
            }

            public static ddouble ValueType2(ddouble nu, ddouble x) {
                ddouble xmnu = x - nu;
                ddouble p0 = 0d, p1 = nu - 1, p2 = 0;
                ddouble q0 = 0d, q1 = 3 + xmnu, q2 = 1;

                for (int i = 2; i < 4096; i++) {
                    ddouble a = i * (nu - i);
                    ddouble b = (2 * i + 1) + xmnu;

                    p0 = a * p2 + b * p1;
                    q0 = a * q2 + b * q1;

                    (int exp, (p0, q0)) = AdjustScale(0, (p0, q0));
                    (p1, q1) = (Ldexp(p1, exp), Ldexp(q1, exp));

                    if (i >= 16 && (i & 3) == 0) {
                        ddouble r0 = p0 * q1, r1 = p1 * q0;
                        if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-31)) {
                            break;
                        }
                    }

                    (p1, p2) = (p0, p1);
                    (q1, q2) = (q0, q1);
                }

                ddouble s = 1 + xmnu, c = p0 / q0;
                ddouble f = s + c;

#if DEBUG
                if (Sign(s) != Sign(c) && int.Abs(double.ILogB(s.Hi) - double.ILogB(c.Hi)) <= 1) {
                    Trace.WriteLine("digit loss!");
                }
#endif

                return f;
            }
        }
    }
}
