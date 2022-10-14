using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble x = "1e+308";
            ddouble y = ddouble.InverseGamma(x);
            ddouble z = ddouble.Gamma(y);

            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
