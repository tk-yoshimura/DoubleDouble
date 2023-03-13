using System;
using System.Runtime.Intrinsics.X86;
using DoubleDouble;
using static DoubleDouble.ddouble;
using static DoubleDouble.ddouble.Consts;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 2; x <= 64; x += 1d / 4) {
                ddouble y = ddouble.BesselY((ddouble)(-15) - Math.ScaleB(1, -26), x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
