using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            Console.WriteLine($"x,y");

            for (ddouble x = -4; x <= 16; x += 1d / 32) {
                Console.WriteLine($"{x},{ddouble.Ti(x)}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
