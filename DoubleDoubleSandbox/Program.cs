using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble.InverseLowerIncompleteGamma("0.001", "1e-32");

            //ddouble nu = 4;

            //for (ddouble x = 0; x <= 4; x += 1d / 64) {
            //    ddouble t1 = ddouble.LowerIncompleteGammaRegularized(nu, x);
            //    ddouble t2 = ddouble.Pow(x, nu) / (nu * ddouble.Gamma(nu));
            //    ddouble v = ddouble.Pow(t2 * nu * ddouble.Gamma(nu), 1d / nu);
            //    ddouble u = ddouble.Exp((ddouble.Log(t2) + ddouble.Log(nu) + ddouble.LogGamma(nu)) / nu);
            //    Console.WriteLine($"{x},{t1},{t2},{v},{u}");
            //}

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
