using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble b1 = ddouble.Erfcx(ddouble.BitDecrement(16));
            ddouble b2 = ddouble.Erfcx(16);
            ddouble b3 = ddouble.Erfcx(ddouble.BitIncrement(16));

            Console.WriteLine(FloatSplitter.Split(b1).mantissa);
            Console.WriteLine(FloatSplitter.Split(b2).mantissa);
            Console.WriteLine(FloatSplitter.Split(b3).mantissa);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
