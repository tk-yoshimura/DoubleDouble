using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //using (StreamWriter sw = new("../../incompgamma.csv")) {
            //    sw.WriteLine("lower");
            //    sw.WriteLine("s,x,m,y");
            //
            //    for (ddouble s = 1d / 32; s <= 1; s += 1d / 32) {
            //        for (ddouble x = 0; x <= 32; x += 1d / 16) {
            //            (ddouble y, int m) = IncompleteGammaPrototype.LowerIncompleteGammaConvergence(s, x);
            //
            //            Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
            //            sw.WriteLine($"{s},{x},{m},{y}");
            //        }
            //    }
            //
            //    sw.WriteLine("upper");
            //    sw.WriteLine("s,x,m,y");
            //
            //    for (ddouble s = 1d / 32; s <= 1; s += 1d / 32) {
            //        for (ddouble x = 0; x <= 32; x += 1d / 16) {
            //            (ddouble y, int m) = IncompleteGammaPrototype.UpperIncompleteGammaConvergence(s, x);
            //
            //            Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
            //            sw.WriteLine($"{s},{x},{m},{y}");
            //        }
            //    }
            //}

            IncompleteGammaPrototype.UpperIncompleteGamma(31d / 32, 1d);
            IncompleteGammaPrototype.UpperIncompleteGamma(0.5d, 0.5d);

            Console.WriteLine(IncompleteGammaPrototype.UpperIncompleteGammaNearZero.A1(ddouble.BitDecrement(0.0625d)));
            Console.WriteLine(IncompleteGammaPrototype.UpperIncompleteGammaNearZero.A1(0.0625d));
            Console.WriteLine(IncompleteGammaPrototype.UpperIncompleteGammaNearZero.A1(ddouble.BitIncrement(0.0625d)));

            for (ddouble nu = 0; nu <= 1; nu += 1d / 256) {
                ddouble y = IncompleteGammaPrototype.UpperIncompleteGamma(nu, 0.5d);
            
                Console.WriteLine($"{nu},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
