using DoubleDouble;
using static DoubleDouble.ddouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using StreamWriter sw = new("../../upper_imcomp_gamma.csv");

            for (ddouble x = 0.125; x <= 64; x += 0.125) {
                sw.Write($",{x}");
            }
            sw.Write("\n");

            for (ddouble nu = 0.125; nu <= 64; nu += 0.125) {
                if (IsInteger(nu)) {
                    continue;
                }

                sw.Write($"{nu}");

                for (ddouble x = 0.125; x <= 64; x += 0.125) {
                    ddouble expected = UpperIncompleteGammaCFrac.Value(nu, x, 1024);

                    for (int m = 2; m <= 1024; m++) { 
                        ddouble actual = UpperIncompleteGammaCFrac.Value(nu, x, m);

                        ddouble err = Abs(expected - actual) / expected;

                        if (err < 1e-30) {
                            Console.WriteLine($"{nu},{x},{m}");
                            sw.Write($",{m}");
                            break;
                        }
                    }
                }
                sw.Write("\n");
            }

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
        }
    }
}
