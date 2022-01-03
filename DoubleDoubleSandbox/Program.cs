using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y_lim = EiPrototype.Ei(-Math.ScaleB(1, -1023));

            using (StreamWriter sw = new StreamWriter("../../ei_table.csv")) {

                for (double x = -8; x <= 8; x += 1d / 512) {
                    ddouble y = EiPrototype.Ei(x);

                    sw.WriteLine($"{x},{y:e29}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
