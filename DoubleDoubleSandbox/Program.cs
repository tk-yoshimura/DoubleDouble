using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            ddouble y5 = SinCosMark2.SinPIPrime((ddouble)0.5m);
            ddouble y6 = SinCosMark2.SinPIPrime((ddouble)0.6m);

            for (ddouble x = 0d; x < 1d; x += 1d / 2048) {
                ddouble y = SinCosMark2.SinPIPrime(x);

                Console.WriteLine($"{x},{y}");
            }

            for (int length = 1; length <= 25; length++) {
                int[] p = new int[length];

                for (int i = 0, n = length - 3; i < n; i += 4) {
                    p[i + 3] = 1;
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
