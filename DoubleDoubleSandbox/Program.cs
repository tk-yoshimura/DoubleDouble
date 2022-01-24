using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Struve.StruveMIntegral.Value(0, 1);
            ddouble y2 = Struve.StruveMIntegral.ValueMk2(0, 1);
            ddouble y3 = Struve.StruveHLNearZero.Value(0, 1, sign_switch: false, terms: 64) - ddouble.BesselI(0, 1);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
