using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble m = 1; m > 0; m /= 2) {
                ddouble x = ddouble.KeplerE(m, 1);

                Console.WriteLine($"{m},{m.Hi},{x}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
