using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 16; x += 1d / 128) {
                ddouble y = ddouble.JacobiDn(x, 0.95d);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
