using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = 0; x < 24; x += 1d / 32) {
                ddouble y = ddouble.JacobiAm(x, 0.25);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
