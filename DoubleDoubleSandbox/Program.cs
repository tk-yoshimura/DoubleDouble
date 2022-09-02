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
                ddouble x = -1.5;
                ddouble y = PolylogSanbox.PolylogIntegral.Polylog(n, x);
            
                Console.WriteLine($"{n},{x},{y}");
            }

            for (int n = 2; n <= 8; n++) {
                ddouble x = -0.5;
                ddouble y = PolylogSanbox.PolylogIntegral.Polylog(n, x);
            
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

            for (int n = 2; n <= 8; n++) {
                ddouble x = 0.5;
                ddouble y = PolylogSanbox.PolylogNearOne.Polylog(n, x);
            
                Console.WriteLine($"{n},{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
