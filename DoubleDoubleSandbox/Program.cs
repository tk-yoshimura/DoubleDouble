using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (double x = 0; x <= 42; x += 0.0625d) {
                ddouble y_dec = ddouble.Gamma(ddouble.BitDecrement(x));
                ddouble y = ddouble.Gamma(x);
                ddouble y_inc = ddouble.Gamma(ddouble.BitIncrement(x));

                Console.WriteLine($"{x}");

                //Console.WriteLine($"{y_inc}");
                //Console.WriteLine($"{y}");
                //Console.WriteLine($"{y_dec}");

                Console.WriteLine($"{FloatSplitter.Split(y_inc).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(y).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(y_dec).mantissa:X14}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
