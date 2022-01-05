using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {

            JacobiTrigonPrototype.JacobiSn((ddouble)0.8m, (ddouble)0.65m);

            for (ddouble k = 0; k <= 8; k += 1d / 8) {
                for (ddouble x = 0; x <= 4; x += 1d / 8) {
                    ddouble y = JacobiTrigonPrototype.JacobiSn(x, k);

                    Console.WriteLine($"{x},{k},{y}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
