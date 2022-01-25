using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = -8; x <= 8; x += 1d / 32) {
                ddouble y = ddouble.StruveL(0, x);

                Console.WriteLine($"{x},{y}");
            }


            Console.WriteLine("END");
            Console.Read();
        }
    }
}
