using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble y = Struve.StruveMIntegral.Value(8, 513);
            ddouble y2 = Struve.StruveMIntegral.ValueMk2(8, 513);
            ddouble y3 = Struve.StruveMIntegral.ValueMk3(8, 513);
            ddouble y4 = Struve.StruveMIntegral.ValueMk4(8, 513);
            ddouble y5 = Struve.StruveHLNearZero.Value(8, 513, sign_switch: false, terms: 64) - ddouble.BesselI(8, 513);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
