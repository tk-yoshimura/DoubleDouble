using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble f = 1;

            for (double x = 0; x <= 36; x += 0.5d) {
                ddouble y_dec = ddouble.Gamma(ddouble.BitDecrement(x));
                ddouble y = ddouble.Gamma(x);
                ddouble y_inc = ddouble.Gamma(ddouble.BitIncrement(x));

                Console.WriteLine($"{x},{y_inc}");
                Console.WriteLine($"{x},{y}");
                Console.WriteLine($"{x},{y_dec}");

                //Console.WriteLine($"{FloatSplitter.Split(y).mantissa:X14}");

                //Console.WriteLine($"{x}");
                //Console.WriteLine($"{(FloatSplitter.Split(y).mantissa):X14}");
                //Console.WriteLine($"{(FloatSplitter.Split(f).mantissa):X14}");
                //
                //f *= x + 1;
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
