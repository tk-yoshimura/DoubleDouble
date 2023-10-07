using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble.InverseLowerIncompleteGamma(0.001, 0.125);
            ddouble.InverseLowerIncompleteGamma(0.001, 0.875);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
