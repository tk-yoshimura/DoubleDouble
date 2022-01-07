using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = 0; x < 1; x += 1d / 32) {
                ddouble y = ddouble.EllipticTheta4(x, 0.995);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
