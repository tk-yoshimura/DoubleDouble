using DoubleDouble;
using System;
using System.Diagnostics;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 32; x += 1d / 4) {
                ddouble y = ddouble.Erfc(x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
