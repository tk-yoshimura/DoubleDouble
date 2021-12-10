using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            BesselLimit.BesselI(0, 50);

            using (StreamWriter sw = new StreamWriter("../../bessel_j.csv")) {
                sw.WriteLine("nu,z,terms,y");
                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 1d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselLimit.BesselJ(nu, z);

                        if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
                            continue;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");

                        break;
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("../../bessel_y.csv")) {
                sw.WriteLine("nu,z,terms,y");
                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 1d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselLimit.BesselY(nu, z);

                        if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
                            continue;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");

                        break;
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("../../bessel_i.csv")) {
                sw.WriteLine("nu,z,terms,y");
                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 1d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselLimit.BesselI(nu, z);

                        if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
                            continue;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");

                        break;
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("../../bessel_k.csv")) {
                sw.WriteLine("nu,z,terms,y");
                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 1d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselLimit.BesselK(nu, z);

                        if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
                            continue;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");

                        break;
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
