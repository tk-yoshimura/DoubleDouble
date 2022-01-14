using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (int n = 4; n <= 256; n += 4) {
                ddouble y = OwenT.OwenTPatefieldTandyAlgo.T4(4.78125, 0.0625, n);

                Console.WriteLine($"{n},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
