using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //const double e = 0.75;

            //for (ddouble m = 0; m <= 1; m += 1d / 256) { 
            //    ddouble x = KeplerEUtil.Elliptic.Value(m, e);

            //    ddouble y = x + e * ddouble.SinPI(x) / ddouble.PI;

            //    Console.WriteLine($"{m},{x},{y}");
            //}

            const double e = 1.25;

            for (ddouble m = 0; m <= 2; m += 1d / 256) {
                ddouble x = KeplerEUtil.Hyperbolic.Value(m, e);

                ddouble y = x + e * ddouble.Sinh(x);

                Console.WriteLine($"{m},{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
