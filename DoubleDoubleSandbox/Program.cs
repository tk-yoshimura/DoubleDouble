using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = -7; x < 7; x += 1d / 32) {
                ddouble ke = KeplerSandbox.KeplerE(x, 1, centerize: true);
            
                Console.WriteLine($"{x},{ke}");
            }

            //for (double x = 1d / 32; x > 0; x /= 16) {
            //    ddouble ke = KeplerSandbox.KeplerE(x, 1);
            //
            //    Console.WriteLine($"{x},{ke}");
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
