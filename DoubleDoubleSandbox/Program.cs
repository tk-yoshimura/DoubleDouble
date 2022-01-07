using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = -8; x < 8; x += 1d / 32) {
                ddouble y = ddouble.Ein(x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
