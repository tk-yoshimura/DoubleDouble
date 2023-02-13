using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            const int n = 4;

            for (ddouble u = 0; u <= 8192; u += 16) {
                ddouble q = ddouble.Sqrt(u) * n * n;

                ddouble a = ddouble.MathieuA(n, q);
                ddouble b = ddouble.MathieuB(n, q);

                Console.WriteLine($"{u},{a},{b}");
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
