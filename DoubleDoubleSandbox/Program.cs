using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            //ddouble x = 2;
            //
            //for (ddouble nu = -8; nu <= 8d; nu += 0.25d) {
            //    ddouble y = BesselMillerBackward.BesselJ(nu, x, 32);
            //
            //    Console.WriteLine($"besselJ({nu}, {x})={y}");
            //}
            //for (ddouble nu = -8; nu <= 8d; nu += 0.25d) {
            //    ddouble y = BesselMillerBackward.BesselY(nu, x, 32);
            //
            //    Console.WriteLine($"besselY({nu}, {x})={y}");
            //}
            //for (ddouble nu = -8; nu <= 8d; nu += 0.25d) {
            //    ddouble y = BesselMillerBackward.BesselI(nu, x, 32);
            //
            //    Console.WriteLine($"besselI({nu}, {x})={y}");
            //}
            //for (ddouble nu = 0; nu <= 8d; nu += 0.25d) {
            //    ddouble y = BesselYoshidaPade.BesselK(nu, x);
            //
            //    Console.WriteLine($"besselK({nu}, {x})={y}");
            //}

            ddouble[] borders = new ddouble[] { 0.5d, 1d, 2d, 
                2.125d, 2.375d, 2.625d, 2.875d, 3.125d, 3.375d, 3.625d, 3.875d, 
                4.125d, 4.375d, 4.625d, 4.875d, 5.125d, 5.375d, 5.625d, 5.875d, 
                6.125d, 6.375d, 6.625d, 6.875d, 7.125d, 7.375d, 7.625d, 7.875d, 8.125d };

            foreach (var x in borders) {
                ddouble x_dec = ddouble.BitDecrement(x);
                ddouble x_inc = ddouble.BitIncrement(x);

                Console.WriteLine(x);

                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erf(x_dec)).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erf(x)).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erf(x_inc)).mantissa:X14}");
            }

            foreach (var x in borders) {
                ddouble x_dec = ddouble.BitDecrement(x);
                ddouble x_inc = ddouble.BitIncrement(x);

                Console.WriteLine(x);

                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erfc(x_dec)).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erfc(x)).mantissa:X14}");
                Console.WriteLine($"{FloatSplitter.Split(ErfMark2.Erfc(x_inc)).mantissa:X14}");
            }

            using (StreamWriter sw = new StreamWriter("../../erfc.csv")) {
                sw.WriteLine("x,m,y");
                for (double z = 0.0d; z <= 4d; z += 1d / 64) {
                    ddouble y = ErfMark2.Erfc(z);

                    sw.WriteLine($"{z},{y}");
                    Console.WriteLine($"{z},{y}");
                }
            }

            //using (StreamWriter sw = new StreamWriter("../../bessel_j_millerbw.csv")) {
            //    sw.WriteLine("n,z,m,y");
            //    for (int n = 0; n <= 4; n++) {
            //        for (double z = 2d; z <= 40d; z += 0.25d) {
            //            (ddouble y, int m) = BesselMillerBackward.BesselJ(n, z, eps: 1e-25);
            //
            //            sw.WriteLine($"{n},{z},{m},{y}");
            //            Console.WriteLine($"{n},{z},{m},{y}");
            //        }
            //    }
            //}
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_j_millerbw_2.csv")) {
            //    sw.WriteLine("n,z,y");
            //    for (int n = 0; n <= 8; n++) {
            //        for (double z = 2d; z <= 40d; z += 0.25d) {
            //            ddouble y = BesselMillerBackward.BesselJ(n, z);
            //
            //            sw.WriteLine($"{n},{z},{y}");
            //            Console.WriteLine($"{n},{z},{y}");
            //        }
            //    }
            //}
            //
            //
            //BesselNearZero.CoefTable coef = new BesselNearZero.CoefTable(2.5);
            //BesselNearZero.BesselJ(0, 40);
            //
            //coef.Value(16);
            //
            //using (StreamWriter sw = new StreamWriter("../../bessel_j.csv")) {
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
            //using (StreamWriter sw = new StreamWriter("../../bessel_j_nz.csv")) {
            //    sw.WriteLine("nu,z,terms,y");
            //    for (double nu = -4d; nu <= 4d; nu += 0.125d) {
            //        for (double z = 0d; z <= 256d; z += 0.5d) {
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
            //using (StreamWriter sw = new StreamWriter("../../bessel_y.csv")) {
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
            //using (StreamWriter sw = new StreamWriter("../../bessel_i.csv")) {
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
            //using (StreamWriter sw = new StreamWriter("../../bessel_k.csv")) {
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

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
