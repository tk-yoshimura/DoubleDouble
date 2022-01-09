using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble z = ddouble.JacobiArcCn(-0.5, 0.5);

            ddouble x = ddouble.JacobiCn(z, 0.5);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
