using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for(ddouble x = -8; x <= 8; x += 1.0 / 256){
                ddouble y = ddouble.Polygamma(1, x);
                Console.WriteLine($"{x},{y}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
