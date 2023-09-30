using DoubleDouble;
using System;
using System.Diagnostics;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 16; x += 1d / 64) {
                ddouble t1 = ddouble.LowerIncompleteGammaRegularized(4, x);
                ddouble t2 = ddouble.UpperIncompleteGammaRegularized(4, x);

                Console.WriteLine($"{x},{t1},{t2}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
