using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            using StreamWriter sw = new("../../mathieu_convergence.csv");

            sw.WriteLine("n,q,terms,s");

            for (int n = 0; n < 16; n++) {
                int terms = 64;
                for (ddouble q = 0; q < 256; q += 1d / 64) {
                    (terms, ddouble s) = SearchConvergence(n, q, Math.Max(64, terms - 16));

                    sw.WriteLine($"{n},{q},{terms},{s}");
                }
                for (ddouble q = 256; q <= 65536; q += 1d / 2) {
                    (terms, ddouble s) = SearchConvergence(n, q, Math.Max(64, terms - 16));

                    sw.WriteLine($"{n},{q},{terms},{s}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }

        private static (int, ddouble s0) SearchConvergence(int n, ddouble q, int init_terms) {
            ddouble a = ddouble.MathieuA(n, q);

            ReadOnlyCollection<ddouble> s0 = ddouble.Mathieu.GenerateCCoef(n, q, a, init_terms);
            ReadOnlyCollection<ddouble> s1 = ddouble.Mathieu.GenerateCCoef(n, q, a, init_terms + 1);

            for (int terms = init_terms; terms <= 65536; terms += 2) {
                ReadOnlyCollection<ddouble> s2 = ddouble.Mathieu.GenerateCCoef(n, q, a, terms + 2);

                if (s0.Count == s1.Count && s1.Count == s2.Count &&
                    ddouble.Abs(s0[0] - s1[0]) <= ddouble.Abs(s1[0] - s2[0]) &&
                    ddouble.Abs(s0[^1] - s1[^1]) <= ddouble.Abs(s1[^1] - s2[^1])) {
                    Console.WriteLine($"{n},{q},{terms},{s0[0]}");

                    return (terms, s0[0]);
                }

                (s0, s1) = (s1, s2);
            }

            return (-1, ddouble.NaN);
        }
    }
}
