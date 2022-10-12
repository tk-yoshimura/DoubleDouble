using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 10));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 100));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 1000));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 10000));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 100000));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 1000000));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 10000000));
            Console.WriteLine(ddouble.InverseErfc((ddouble)1 / 100000000));

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
