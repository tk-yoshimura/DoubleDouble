using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (int n = 2; n <= 8; n++) {
                ddouble x = -1.5;
                ddouble y = PolylogSanbox.PolylogPowerSeries.PolylogMinusLimit(n, x);
            
                Console.WriteLine($"{n},{x},{y}");
            }

            for (int n = 2; n <= 8; n++) {
                ddouble x = -0.5;
                ddouble y = PolylogSanbox.PolylogPowerSeries.PolylogNearZero(n, x);
            
                Console.WriteLine($"{n},{x},{y}");
            }

            for (int n = 2; n <= 8; n++) {
                ddouble x = 0.5;
                ddouble y = PolylogSanbox.PolylogPowerSeries.PolylogNearZero(n, x);
            
                Console.WriteLine($"{n},{x},{y}");
            }

            //for (int n = 2; n <= 8; n++) {
            //    for (ddouble x = 0.5; x > Math.ScaleB(1, -102); x /= 2) {
            //        ddouble y = PolylogSanbox.PolylogNearOne.Polylog(n, 1 - x);
            //
            //        Console.WriteLine($"{n},{1 - x},{y}");
            //    }
            //}

            Console.WriteLine("END");
            Console.Read();
        }

        static double PolylogPositiveIntegrandPeak(int n, double x) {
            if (n <= 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n == 1) {
                return 0;
            }

            double t;
            if (n == 2) {
                double u = Math.Sqrt(-Math.Log(x));
                t = u * (1.38264783 - u * 0.55707189); 
            }
            else {
                t = n - 1;
            }

            for (int i = 0; i < 8; i++) {
                double xexp = Math.Exp(-t) * x;
                double d = (1 - n) * (xexp - 1) - t;
                double dv = (1 - n) * xexp + 1;
                double dt = d / dv;

                t += dt;

                Console.WriteLine(dt);

                if (Math.Abs(dt / t) < 1e-14) {
                    break;
                }
            }

            return t;
        }
    }
}
