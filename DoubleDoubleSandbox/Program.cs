using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            for (ddouble x = 0d; x < 1d; x += 1d / 1024) {
                (ddouble y, int terms) = Pow2Mark2.Pow2Prime(x);

                Console.WriteLine($"{x},{y},{terms}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
