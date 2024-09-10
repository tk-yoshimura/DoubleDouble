using DoubleDouble;
using System;
using System.Collections.Generic;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {

            for (ddouble x = -8; x <= 8; x += 1 / 32d) {
                ddouble y = ddouble.InverseDigamma(x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
