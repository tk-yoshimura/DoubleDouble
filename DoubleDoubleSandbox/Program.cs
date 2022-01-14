using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble t1 = OwenT.OwenTPatefieldTandyAlgo.T1(0.0625, 0.25);
            ddouble t2 = OwenT.OwenTPatefieldTandyAlgo.T2(6.5, 0.4375);
            ddouble t3 = OwenT.OwenTPatefieldTandyAlgo.T3(7, 0.96875);
            ddouble t4 = OwenT.OwenTPatefieldTandyAlgo.T4(4.78125, 0.0625);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
