using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 40; x += 0.25) {
                ddouble y_dec = ddouble.Erfc(ddouble.BitDecrement(x));
                ddouble y = ddouble.Erfc(x);
                ddouble y_inc = ddouble.Erfc(ddouble.BitIncrement(x));

                Console.WriteLine(x);
                Console.WriteLine($"0x{FloatSplitter.Split(y_dec).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(y).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(y_inc).mantissa:X14}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
