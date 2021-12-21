using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {

            //using (StreamWriter sw = new StreamWriter("../../bessel_j_nz.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 0d; z <= 2d; z += 0.125d) {
            //            (ddouble y, int terms) = BesselNearZero.BesselJ(nu, z);
            //
            //            if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
            //                break;
            //            }
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_y_nz.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 0d; z <= 2d; z += 0.125d) {
            //            (ddouble y, int terms) = BesselNearZero.BesselY(nu, z);
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_i_nz.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 0d; z <= 2d; z += 0.125d) {
            //            (ddouble y, int terms) = BesselNearZero.BesselI(nu, z);
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_k_nz.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 0d; z <= 2d; z += 0.125d) {
            //            (ddouble y, int terms) = BesselNearZero.BesselK(nu, z);
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_j_limit.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 1d; z <= 256d; z += 0.5d) {
            //            (ddouble y, int terms) = BesselLimit.BesselJ(nu, z);
            //
            //            if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
            //                continue;
            //            }
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //
            //            break;
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_y_limit.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 1d; z <= 256d; z += 0.5d) {
            //            (ddouble y, int terms) = BesselLimit.BesselY(nu, z);
            //
            //            if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
            //                continue;
            //            }
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //
            //            break;
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_i_limit.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 1d; z <= 256d; z += 0.5d) {
            //            (ddouble y, int terms) = BesselLimit.BesselI(nu, z);
            //
            //            if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
            //                continue;
            //            }
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //
            //            break;
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_k_limit.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 1d; z <= 256d; z += 0.5d) {
            //            (ddouble y, int terms) = BesselLimit.BesselK(nu, z);
            //
            //            if (ddouble.IsNaN(y) || !ddouble.IsFinite(y)) {
            //                continue;
            //            }
            //
            //            sw.WriteLine($"{nu},{z},{terms},{y}");
            //            Console.WriteLine($"{nu},{z},{terms},{y}");
            //
            //            break;
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_j_millerbw.csv")) {
            //    sw.WriteLine("n,z,m,y");
            //    for (ddouble nu = 0; nu <= 8; nu += 0.125d) {
            //        for (double z = 2d; z <= 40d; z += 0.25d) {
            //            (ddouble y, int m) = BesselMillerBackward.BesselJ(nu, z, eps: 1e-25);
            //
            //            sw.WriteLine($"{nu},{z},{m},{y}");
            //            Console.WriteLine($"{nu},{z},{m},{y}");
            //        }
            //    }
            //}

            using (StreamWriter sw = new StreamWriter("../../bessel_y_millerbw.csv")) {
                sw.WriteLine("n,z,m,y");
                for (ddouble nu = 0; nu <= 8; nu += 0.125d) {
                    for (double z = 2d; z <= 40d; z += 0.25d) {
                        (ddouble y, int m) = BesselMillerBackward.BesselY(nu, z, eps: 1e-25);
            
                        sw.WriteLine($"{nu},{z},{m},{y}");
                        Console.WriteLine($"{nu},{z},{m},{y}");
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter("../../bessel_i_millerbw.csv")) {
                sw.WriteLine("n,z,m,y");
                for (ddouble nu = 0; nu <= 8; nu += 0.125d) {
                    for (double z = 2d; z <= 40d; z += 0.25d) {
                        (ddouble y, int m) = BesselMillerBackward.BesselI(nu, z, eps: 1e-25);
            
                        sw.WriteLine($"{nu},{z},{m},{y}");
                        Console.WriteLine($"{nu},{z},{m},{y}");
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
