using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble x = ddouble.InverseIncompleteBeta("0.99999999999999999999", 2, 3);
            ddouble y = ddouble.IncompleteBetaRegularized(x, 2, 3);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
