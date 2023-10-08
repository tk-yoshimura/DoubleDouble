using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble x = ddouble.InverseIncompleteBeta(ddouble.Epsilon, 2, 2);
            ddouble y = ddouble.IncompleteBetaRegularized(x, 2, 2);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
