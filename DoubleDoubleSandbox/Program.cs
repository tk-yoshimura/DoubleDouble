using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            ddouble c = "3.8317059702075123156144358863081607665645452742878019287622989899";

            for (ddouble x = c - 1e-29; x < c + 1e-29; x += 1e-30) {
                ddouble y = ddouble.BesselJ(1, x);

                Console.WriteLine(y);
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
