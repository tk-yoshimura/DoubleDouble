using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using StreamWriter sw = new("../besseljy_limit.csv");

            sw.WriteLine("nu,x");

            for (ddouble nu = -16; nu <= 16; nu += 0.125) {

                for (ddouble x = 2; x <= 256; x += 0.125) {
                    ddouble y0 = ddouble.BesselLimit.BesselK(nu, x);
                    ddouble y1 = ddouble.BesselYoshidaPade.BesselK(nu, x);

                    if (ddouble.Abs(y0 - y1) < ddouble.Abs(y0) * 1e-31) {
                        sw.WriteLine($"{nu},{x}");
                        Console.WriteLine($"{nu},{x}");
                        break;
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
