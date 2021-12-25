using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y_inf = ddouble.FresnelC(Math.ScaleB(1, 256));

            for (ddouble x = 0; x <= 10; x += 1d / 64) {
                ddouble y = ddouble.FresnelS(x);

                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
