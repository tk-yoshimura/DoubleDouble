using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 40; x <= 64; x += 1d / 32) {
                ddouble y = ddouble.BesselY((ddouble)(-15) + Math.ScaleB(1, -41), x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
