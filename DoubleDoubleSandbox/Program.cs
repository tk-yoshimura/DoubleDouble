using DoubleDouble;
using System;

using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble a = 1d;
            
            for (ddouble s = 2d; s <= 1024; s += 1d) {
                for (; a <= 128; a += 1d / 1024) {
                    ddouble y = HurwitzZetaProto.HurwitzZeta(s, a);                    

                    if (ddouble.IsZero(y)) {
                        break;
                    }

                    if (!ddouble.IsNaN(y)) {
                        Console.WriteLine($"{s},{a},{y}");
                        break;
                    }
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
