using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble gmax = ddouble.InverseGamma(3);
            ddouble g = ddouble.Gamma(gmax);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
