using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //for (double x = 0; x <= 7; x += 1d / 32) {
            //    ddouble ke = KeplerSandbox.KeplerE(x, 0.99999);
            //
            //    Console.WriteLine($"{x},{ke}");
            //}

            for (double x = 1d / 32; x > 0; x /= 16) {
                ddouble ke = KeplerSandbox.KeplerE(x, 0.99999);
            
                Console.WriteLine($"{x},{ke}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
