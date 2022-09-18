using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = -1; x <= 1; x += 1d / 32) {
                Console.Write($"{x}");

                for (int n = 0; n <= 8; n++) {
                    Console.Write($",{ddouble.GegenbauerC(n, -0.5, x)}");
                }

                Console.Write("\n");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
