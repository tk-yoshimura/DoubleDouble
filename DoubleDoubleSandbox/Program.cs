using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            ddouble pi_prev = 0;

            for (int n = 1; n < 8; n++) {
                ddouble pi = GeneratePI(n);

                Console.WriteLine(n);
                Console.WriteLine(pi - pi_prev);
                Console.WriteLine(pi - ddouble.PI);
                Console.WriteLine(pi);

                pi_prev = pi;
            }

            Console.WriteLine();
            Console.Read();
        }

        private static ddouble GeneratePI(int n) {
            ddouble a = 1;
            ddouble b = ddouble.Ldexp(ddouble.Sqrt(2), -1);
            ddouble t = ddouble.Ldexp(1, -2);
            ddouble p = 1;

            for (int i = 0; i < n; i++) {
                ddouble a_next = ddouble.Ldexp(a + b, -1);
                ddouble b_next = ddouble.Sqrt(a * b);
                ddouble t_next = t - p * (a - a_next) * (a - a_next);
                ddouble p_next = ddouble.Ldexp(p, 1);

                a = a_next;
                b = b_next;
                t = t_next;
                p = p_next;
            }

            ddouble c = a + b;
            ddouble y = c * c / ddouble.Ldexp(t, 2);

            return y;
        }
    }
}
