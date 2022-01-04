using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //using (StreamWriter sw = new("../../incompgamma_2.csv")) {
            //    sw.WriteLine("lower");
            //    sw.WriteLine("s-x");
            //
            //    for (ddouble x = 0; x <= 16; x += 1d / 16) {
            //        sw.Write($",{x}");
            //    };
            //    sw.Write('\n');
            //
            //    for (ddouble s = 1d / 128; s <= 1; s += 1d / 128) {
            //        sw.Write($"{s}");
            //
            //        for (ddouble x = 0; x <= 16; x += 1d / 16) {
            //            (ddouble y, int m) = IncompleteGammaPrototype.LowerIncompleteGammaConvergence(s, x);
            //
            //            Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
            //            sw.Write($",{m}");
            //        }
            //        sw.Write('\n');
            //    }
            //
            //    sw.WriteLine("upper");
            //    sw.WriteLine("s-x");
            //
            //    for (ddouble x = 0; x <= 128; x += 1d / 16) {
            //        sw.Write($",{x}");
            //    };
            //    sw.Write('\n');
            //
            //    for (ddouble s = 1d / 128; s <= 1; s += 1d / 128) {
            //        sw.Write($"{s}");
            //
            //        for (ddouble x = 0; x <= 128; x += 1d / 16) {
            //            (ddouble y, int m) = IncompleteGammaPrototype.UpperIncompleteGammaConvergence(s, x);
            //
            //            Console.WriteLine($"{s}\t{x}\t{m}\t{y}");
            //            sw.Write($",{m}");
            //        }
            //        sw.Write('\n');
            //    }
            //}

            for (ddouble x = 0; x <= 64; x += 1d / 16) {
                ddouble y = IncompleteGammaPrototype.UpperIncompleteGamma(0.5d, x);
            
                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
