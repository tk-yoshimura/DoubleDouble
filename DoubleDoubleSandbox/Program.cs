using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 256; x <= Math.ScaleB(1, 800); x *= 2) {
                ddouble y = ddouble.StruveM(0, x);

                Console.WriteLine($"{x},{y}");
            }


            Console.WriteLine("END");
            Console.Read();
        }
    }
}
