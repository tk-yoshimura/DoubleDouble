using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //using (StreamWriter sw = new("../../incompbeta2.csv")) {
            //    sw.WriteLine("x,a,b,y,m");
            //
            //    for (ddouble b = 1d / 64; b <= 64d; b *= 2) {
            //        for (ddouble a = 1d / 64; a <= 64d; a *= 2) {
            //            for (ddouble x = 1d / 32; x <= 31d / 32; x += 1d / 32) {
            //                (ddouble y, int m) = IncompBetaPrototype.BetaConvergence(x, a, b);
            //
            //                sw.WriteLine($"{x},{a},{b},{y},{m}");
            //
            //                Console.WriteLine($"{x},{a},{b},{y},{m}");
            //            }
            //        }
            //    }
            //}

            //using (StreamWriter sw = new("../../incompbeta3.csv")) {
            //    sw.WriteLine("x,a=b,y,m");
            //
            //    ddouble x = 0.5d;
            //
            //    for (ddouble a = 20; a <= 160d; a += 1d / 64) {
            //        (ddouble y, int m) = IncompBetaPrototype.BetaConvergence(x, a, a);
            //
            //        sw.WriteLine($"{x},{a},{y},{m}");
            //
            //        Console.WriteLine($"{x},{a},{y},{m}");
            //    }
            //}

            //using (StreamWriter sw = new("../../incompbeta_complementary.csv")) {
            //    sw.WriteLine("x,a,b,y,m");
            //
            //    for (ddouble b = 1d / 16; b <= 32d; b += 1d / 16) {
            //        if (b % 1 == 0) {
            //            continue;
            //        }
            //
            //        for (ddouble a = 1d / 16; a <= 32d; a += 1d / 16) {
            //            if (a % 1 == 0) {
            //                continue;
            //            }
            //            
            //            ddouble x = (a + 1) / (a + b + 2);
            //
            //            (ddouble y, int m) = IncompBetaPrototype.BetaConvergence(x, a, b);
            //
            //            sw.WriteLine($"{x},{a},{b},{y},{m}");
            //
            //            Console.WriteLine($"{x},{a},{b},{y},{m}");
            //        }
            //    }
            //}

            using (StreamWriter sw = new("../../incompbeta8.csv")) {
                sw.WriteLine("x,a,b,m");
            
                for (ddouble b = 1d / 16; b <= 32d; b += 1d / 16) {
                    if (b % 1 == 0) {
                        continue;
                    }

                    for (ddouble a = 1d / 16; a <= 32d; a += 1d / 16) {
                        if (a % 1 == 0) {
                            continue;
                        }

                        (ddouble x, int m) = IncompBetaPrototype.BetaConvergence(a, b);
                        
                        sw.WriteLine($"{x},{a},{b},{m}");
            
                        Console.WriteLine($"{x},{a},{b}\t{m}");
                    }
                }
            }

            using (StreamWriter sw = new("../../incompbeta7.csv")) {
                sw.WriteLine("x,a,b,y,ma,mb");
            
                for (ddouble b = 1d / 16; b <= 32d; b += 1d / 16) {
                    if (b % 1 == 0) {
                        continue;
                    }

                    for (ddouble a = 1d / 16; a <= 32d; a += 1d / 16) {
                        if (a % 1 == 0) {
                            continue;
                        }
                        
                        ddouble x = (ddouble.Sqrt(a) + 4) / (ddouble.Sqrt(a) + ddouble.Sqrt(b) + 8);

                        (ddouble y, int ma) = IncompBetaPrototype.BetaConvergence(x, a, b, complementary: false);
                        (ddouble _, int mb) = IncompBetaPrototype.BetaConvergence(x, a, b, complementary: true);
            
                        sw.WriteLine($"{x},{a},{b},{y},{ma},{mb}");
            
                        Console.WriteLine($"{x},{a},{b},{y}\t\t{ma},{mb}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
