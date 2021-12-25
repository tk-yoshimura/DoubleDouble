using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble nu = -8; nu <= 8; nu += 1d / 4) {
                ddouble y0 = ddouble.BesselK(nu, Math.ScaleB(1, -96));
                ddouble y1 = ddouble.BesselK(nu, Math.BitIncrement(Math.ScaleB(1, -96)));
                ddouble y2 = ddouble.BesselK(nu, Math.ScaleB(1, -95));

                Console.WriteLine($"{y0}\n{y1}\n{y2}\n");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
