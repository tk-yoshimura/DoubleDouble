using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            using (StreamWriter sw = new StreamWriter("ddouble_convterms.csv")) {
                sw.WriteLine($"z,terms,approx gamma(z),expected gamma(z)");
                
                for (ddouble z = 1; z <= 32; z += 0.5) {
                    int terms = SterlingApprox.SterlingTermConvergence(z);
                    ddouble y_approx = terms < 32 ? SterlingApprox.Gamma(z, terms) : ddouble.NaN;
                    ddouble y_expected = GammaExpects.Gamma((int)(z * 2));

                    sw.WriteLine($"{z},{terms},{y_approx},{y_expected}");
                    Console.WriteLine($"{z},{terms},{y_approx},{y_expected}");
                }
            }

            Console.WriteLine();
            Console.Read();
        }
    }
}
