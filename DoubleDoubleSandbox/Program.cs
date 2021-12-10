using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            for (ddouble x = 0d; x < 1d; x += 1d / 256) {
                (ddouble y, int terms) = SinCosMark2.SinPIPrime(x);

                Console.WriteLine($"{x},{y},{terms}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
