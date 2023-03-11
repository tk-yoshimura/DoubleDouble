using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 32; x += 1d / 32) {
                ddouble y = ddouble.BesselY(-15.5, x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
