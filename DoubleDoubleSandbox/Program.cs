using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            const int n = 4;

            for (ddouble q = 0; q <= 16; q += 1d / 256) {
                ddouble a = ddouble.MathieuA(n, q);
                ddouble b = ddouble.MathieuB(n, q);

                Console.WriteLine($"{q},{a},{b}");
            }

            //for (int exp = -48; exp <= 0; exp++) {
            //    ddouble m2 = ddouble.MathieuM(2, ddouble.Ldexp(1, exp));
            //
            //    Console.WriteLine(m2);
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
