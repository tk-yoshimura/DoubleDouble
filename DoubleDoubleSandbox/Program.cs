using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            BesselMillerBackward.BesselYXiTable table = new(0, new BesselMillerBackward.BesselYEtaTable(0));

            for (int i = 0; i <= 32; i++) {
                Console.WriteLine($"{i},{table[i]}");
            }

            for (int m = 32; m < 256; m += 2) {
                ddouble y = BesselMillerBackward.BesselY(-4.25, 2, m);

                Console.WriteLine($"{m},{y}");
            }

            for (int m = 32; m < 256; m += 2) {
                ddouble y = BesselMillerBackward.BesselJ(-2.25, 2, m);

                Console.WriteLine($"{m},{y}");
            }

            for (int m = 32; m < 256; m += 2) {
                ddouble y = BesselMillerBackward.BesselJ(-10.01, 2, m);

                Console.WriteLine($"{m},{y}");
            }

            for (int m = 32; m < 256; m += 2) {
                ddouble y = BesselMillerBackward.BesselI(2.25, 2, m);

                Console.WriteLine($"{m},{y}");
            }

            BesselLimit.BesselK(0, 24);

            using (StreamWriter sw = new StreamWriter("../../bessel_j_millerbw.csv")) {
                sw.WriteLine("n,z,m,y");
                for (int n = 0; n <= 4; n++) {
                    for (double z = 2d; z <= 40d; z += 0.25d) {
                        (ddouble y, int m) = BesselMillerBackward.BesselJ(n, z, eps: 1e-25);

                        sw.WriteLine($"{n},{z},{m},{y}");
                        Console.WriteLine($"{n},{z},{m},{y}");
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("../../bessel_j_millerbw_2.csv")) {
                sw.WriteLine("n,z,y");
                for (int n = 0; n <= 8; n++) {
                    for (double z = 2d; z <= 40d; z += 0.25d) {
                        ddouble y = BesselMillerBackward.BesselJ(n, z);

                        sw.WriteLine($"{n},{z},{y}");
                        Console.WriteLine($"{n},{z},{y}");
                    }
                }
            }


            BesselNearZero.CoefTable coef = new BesselNearZero.CoefTable(2.5);
            BesselNearZero.BesselJ(0, 40);

            coef.Value(16);

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

            using (StreamWriter sw = new StreamWriter("../../bessel_j_nz.csv")) {
                sw.WriteLine("nu,z,terms,y");
                for (double nu = -4d; nu <= 4d; nu += 0.125d) {
                    for (double z = 0d; z <= 256d; z += 0.5d) {
                        (ddouble y, int terms) = BesselNearZero.BesselJ(nu, z);

                        if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
                            break;
                        }

                        sw.WriteLine($"{nu},{z},{terms},{y}");
                        Console.WriteLine($"{nu},{z},{terms},{y}");
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
