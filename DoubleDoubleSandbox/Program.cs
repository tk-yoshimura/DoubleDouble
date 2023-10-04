using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            //ddouble.InverseLowerIncompleteGamma(4, "1e-2");
            ddouble.InverseUpperIncompleteGamma("0.125", "1e-8");

            ddouble nu = "0.001";

            for (ddouble x = 0; x <= "1e-20"; x += "1e-23") {
                ddouble t1 = ddouble.LowerIncompleteGammaRegularized(nu, x);
                ddouble t2 = ddouble.Pow(x, nu) / (nu * ddouble.Gamma(nu));
                //ddouble v = ddouble.Pow(t2 * nu * ddouble.Gamma(nu), 1d / nu);
                //ddouble u = ddouble.Exp((ddouble.Log(t2) + ddouble.Log(nu) + ddouble.LogGamma(nu)) / nu);
                Console.WriteLine($"{x},{t1},{t2}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
