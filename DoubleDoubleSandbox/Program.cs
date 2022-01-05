using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble k = 0; k <= 1; k += 1d / 8) {
                for (ddouble x = 0; x <= 4; x += 1d / 32) {
                    ddouble y = JacobiTrigonPrototype.JacobiDn(x, k);

                    Console.WriteLine($"{x},{k},{y}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
