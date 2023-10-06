using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            StreamWriter sw = new("../../lower_incomp_gamma_p5_approx.csv");

            sw.WriteLine("nu,x,y");

            for (int exp = -32; exp <= 14; exp++) {
                for (ddouble v = 1; v < 2; v += 1d / 256) {
                    ddouble nu = ddouble.Ldexp(v, exp);

                    if (nu > 8192) {
                        continue;
                    }

                    ddouble x = ddouble.InverseLowerIncompleteGamma(nu, 0.5);
                    ddouble y = ddouble.LowerIncompleteGammaRegularized(nu, x);

                    ddouble x_approx = IncompleteGammaP5(nu.Hi);
                    ddouble y_approx = ddouble.LowerIncompleteGammaRegularized(nu, x_approx);


                    if (x < ddouble.Epsilon) { 
                        continue;
                    }

                    Console.WriteLine($"{nu},{x},{y},{x_approx},{y_approx}");
                    sw.WriteLine($"{nu},{x},{y},{x_approx},{y_approx}");
                }
            }

            sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }

        static double IncompleteGammaP5(double nu) {
            double nu_ln2 = double.Log2(nu);

            if (nu <= 1) {
                double b = nu * (1.184797 + nu * (-1.64793 + nu * (-1.87704 + nu * (3.354884 + nu * (-1.93304)))));
                double c = b - nu_ln2;
                double x = double.Exp2(-double.Exp2(c));

                return x;
            }
            else {
                double b = 
                    (-0.92055 + nu_ln2 * (-1.34724 + nu_ln2 * (-0.41283 + nu_ln2 * (-0.11429))))
                    / (1 + nu_ln2 * (0.295360 + nu_ln2 * (0.114197)));

                double x = nu * double.Exp2(-double.Exp2(b));

                return x;
            }
        }
    }
}
