using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using (StreamWriter sw = new("../../incompbeta9.csv")) {
                sw.WriteLine("x,a,b,y,ma,mb");
            
                for (ddouble b = 1d / 16; b <= 64d; b += 1d / 16) {
                    if (b % 1 == 0) {
                        continue;
                    }
            
                    for (ddouble a = 1d / 16; a <= 64d; a += 1d / 16) {
                        if (a % 1 == 0) {
                            continue;
                        }

                        ddouble x = IncompBetaPrototype.ConvergenceThreshold((double)a, (double)b);
            
                        (ddouble y, int ma) = IncompBetaPrototype.BetaConvergence(x, a, b, complementary: false);
                        (ddouble _, int mb) = IncompBetaPrototype.BetaConvergence(x, a, b, complementary: true);
            
                        sw.WriteLine($"{x},{a},{b},{y},{ma},{mb}");
            
                        Console.WriteLine($"{x},{a},{b},{y}\t\t{ma},{mb}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
