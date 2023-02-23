using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            int n = 0;

            for (ddouble x = -4; x <= 4; x += 1d / 32) {
                ddouble ym2 = ddouble.MathieuC(n, -2, x);
                ddouble ym1 = ddouble.MathieuC(n, -1, x);
                ddouble yz0 = ddouble.MathieuC(n, 0, x);
                ddouble yp1 = ddouble.MathieuC(n, +1, x);
                ddouble yp2 = ddouble.MathieuC(n, +2, x);

                Console.WriteLine($"{x},{ym2},{ym1},{yz0},{yp1},{yp2}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
