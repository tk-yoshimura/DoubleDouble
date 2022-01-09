using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble x = 0.5, m = 1;

            ddouble y = ddouble.JacobiDn(x, m);

            ddouble z = ddouble.JacobiArcDn(y, m);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
