using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using (StreamWriter sw = new StreamWriter("../../ei_table.csv")) {

                for (double x = -8; x <= 8; x += 1d / 512) {
                    ddouble y = EiPrototype.Ei(x);

                    sw.WriteLine($"{x},{y:e29}");
                }
            }

            using (StreamWriter sw = new StreamWriter("../../li_table.csv")) {

                for (double x = 0; x <= 8; x += 1d / 512) {
                    ddouble y = EiPrototype.Li(x);

                    sw.WriteLine($"{x},{y:e29}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
