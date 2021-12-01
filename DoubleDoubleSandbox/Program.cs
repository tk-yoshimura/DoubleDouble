using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            //using (StreamWriter sw = new StreamWriter("../../erfc_convergence_n.csv")) {
            //
            //    sw.WriteLine("z,f_init,f_init_prev,n,erfc(z)");
            //
            //    for (ddouble z = 1; z <= 10; z += 0.125d) {
            //
            //        int min_n = int.MaxValue;
            //        ddouble min_f_init = ddouble.NaN;
            //
            //        for (ddouble f_init = 1d; f_init <= 1000; f_init += 1d / 8) {
            //
            //            ddouble erfc_prev = ddouble.NaN;
            //            for (int n = 1; n <= 100; n++) {
            //                ddouble erfc = ErfcContinuedFraction.Erfc(z, n, f_init);
            //
            //                if (erfc != erfc_prev) {
            //                    erfc_prev = erfc;
            //                }
            //                else {
            //                    if (min_n > n) {
            //                        min_n = n;
            //                        min_f_init = f_init;
            //                    }
            //                    break;
            //                }
            //            }
            //        }
            //
            //        if(min_n < int.MaxValue){
            //            ddouble erfc = ErfcContinuedFraction.Erfc(z, min_n - 1, min_f_init);
            //
            //            ddouble w = z * z;
            //
            //            ddouble f = 
            //                (ddouble.Sqrt(25 + w * (440 + w * (488 + w * 16 * (10 + w))))
            //                 - 5 + w * 4 * (1 + w))
            //                / (20 + w * 8);
            //
            //            sw.WriteLine($"{z},{min_f_init},{f},{min_n - 1},{erfc}");
            //            Console.WriteLine($"{z},{min_f_init},{f},{min_n - 1},{erfc}");
            //        }
            //    }
            //}

            for (decimal v = 0.5m; v <= 6.25m; v += 0.125m) {
                ddouble x = (ddouble)v, x_dec = ddouble.BitDecrement(x), x_inc = ddouble.BitIncrement(x);

                ddouble erfc_dec = ddouble.Erfc(x_dec);
                ddouble erfc = ddouble.Erfc(x);
                ddouble erfc_inc = ddouble.Erfc(x_inc);

                Console.WriteLine($"erfc{x}");
                Console.WriteLine(erfc_dec);
                Console.WriteLine(erfc);
                Console.WriteLine(erfc_inc);

                Console.WriteLine($"0x{FloatSplitter.Split(erfc_dec).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(erfc).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(erfc_inc).mantissa:X14}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
