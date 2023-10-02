using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble.InverseLowerIncompleteGamma(64, 0.25);

            //for (ddouble x = 0; x <= 1d / (4096 * 4096); x += 1d / (4096 * 4096 * 4096L)) {
            //    ddouble t1 = ddouble.LowerIncompleteGammaRegularized(4, x);
            //    ddouble t2 = ddouble.UpperIncompleteGammaRegularized(4, x);
            //    Console.WriteLine($"{x},{t1},{t2}");
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
