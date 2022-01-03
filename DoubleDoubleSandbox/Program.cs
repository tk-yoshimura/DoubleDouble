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
            using (StreamWriter sw = new StreamWriter("../../ei_coef_table.csv")) {

                for (double x = -2; x <= 2; x += 1d / 1024) {
                    ddouble y = EiPrototype.EiPade.Coef(x);

                    sw.WriteLine($"{x},{y:e29}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
