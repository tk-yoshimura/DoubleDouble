using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Struve.StruveMIntegral.Value(0, 90);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
