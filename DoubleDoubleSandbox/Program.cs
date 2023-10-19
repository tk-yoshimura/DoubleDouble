using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = -0.5; x <= 3.375; x += 1d / 32) {
                ddouble y = ddouble.LogBarnesG(x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
