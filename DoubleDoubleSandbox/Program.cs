using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using (StreamWriter sw = new("../../incompbeta2.csv")) {
                sw.WriteLine("x,a,b,y,m");

                for (ddouble b = 1d / 64; b <= 64d; b *= 2) {
                    for (ddouble a = 1d / 64; a <= 64d; a *= 2) {
                        for (ddouble x = 1d / 32; x <= 31d / 32; x += 1d / 32) {
                            (ddouble y, int m) = IncompBetaPrototype.BetaConvergence(x, a, b);

                            sw.WriteLine($"{x},{a},{b},{y},{m}");

                            Console.WriteLine($"{x},{a},{b},{y},{m}");
                        }
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
