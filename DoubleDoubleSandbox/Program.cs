using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            using (StreamWriter sw = new StreamWriter("../../bessel_j.csv")) {

                sw.WriteLine("nu,z,terms,y");

                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 1d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselLimit.BesselJ(nu, z);

                        if (ddouble.IsNaN(y)) {
                            continue;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
