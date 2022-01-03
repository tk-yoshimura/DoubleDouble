using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = -10; x <= 10; x += 1d / 256) {
                ddouble y = ddouble.AiryBi(x);

                Console.WriteLine($"{x},{y}");
            }


            using (StreamWriter sw = new("../../incompgamma.csv")) {
                sw.WriteLine("lower");
                sw.WriteLine("s,x,m,y");

                for (ddouble s = 1d / 32; s <= 1; s += 1d / 32) {
                    for (ddouble x = 0; x <= 32; x += 1d / 16) {
                        (ddouble y, int m) = IncompleteGammaPrototype.LowerIncompleteGammaConvergence(s, x);

                        Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
                        sw.WriteLine($"{s},{x},{m},{y}");
                    }
                }

                sw.WriteLine("upper");
                sw.WriteLine("s,x,m,y");

                for (ddouble s = 1d / 32; s <= 1; s += 1d / 32) {
                    for (ddouble x = 0; x <= 32; x += 1d / 16) {
                        (ddouble y, int m) = IncompleteGammaPrototype.UpperIncompleteGammaConvergence(s, x);

                        Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
                        sw.WriteLine($"{s},{x},{m},{y}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
