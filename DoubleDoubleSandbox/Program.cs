using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = 0; x <= 10; x += 1d / 256) {
                ddouble y = ddouble.BesselJ(0, x);
                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
