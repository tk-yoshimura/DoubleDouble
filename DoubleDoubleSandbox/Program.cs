using DoubleDouble;
using System;

using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {            
            ddouble y = HurwitzZetaProto.HurwitzZeta(25.5, 16);

            Console.WriteLine(y);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
