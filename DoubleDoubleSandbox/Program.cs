using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = -8; x <= 8; x += 1d / 32) {
                ddouble y1 = ddouble.KeplerE(x, 0.999, centerize:true);
                ddouble y2 = ddouble.KeplerE(x, 1.001, centerize:true);
            
                Console.WriteLine($"{x},{y1},{y2}");
            }
            
            Console.WriteLine("END");
            Console.Read();
        }
    }
}
