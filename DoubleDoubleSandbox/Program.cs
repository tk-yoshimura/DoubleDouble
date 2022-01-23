using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Struve.StruveKIntegral.Value(3, 2);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
