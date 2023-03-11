using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using StreamWriter sw = new("../besseli_miller_bwd.csv");

            sw.WriteLine("nu,x,y,m");

            for (ddouble nu = -16; nu <= 16; nu += 0.125) {

                int m = 20;

                for (ddouble x = 2; x <= 64; x += 0.125) {    
                    ddouble y0 = MBwdBesselI(nu, x, m);
                    ddouble y1 = MBwdBesselI(nu, x, m + 2);
                    ddouble err01 = ddouble.Abs(y0 - y1);

                    for (; m <= 4096; m += 2) {
                        ddouble y2 = MBwdBesselI(nu, x, m + 4);
                        ddouble err12 = ddouble.Abs(y1 - y2);

                        if (err01 <= err12) {
                            break;
                        }

                        (y0, y1, err01) = (y1, y2, err12);
                    }

                    sw.WriteLine($"{nu},{x},{y1},{m + 2}");
                    Console.WriteLine($"{nu},{x},{y1},{m + 2}");

                    m = Math.Max(m - 10, 20);
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }

        private static ddouble MBwdBesselI(ddouble nu, ddouble x, int m) {
            ddouble y = nu == ddouble.Floor(nu)
               ? ddouble.BesselMillerBackward.BesselIKernel((int)ddouble.Floor(nu), x, m)
               : ddouble.BesselMillerBackward.BesselIKernel(nu, x, m);

            return y;
        }
    }
}
