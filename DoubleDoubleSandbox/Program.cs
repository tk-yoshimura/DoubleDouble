using System;
using DoubleDouble;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            for (ddouble x = -1d / 16; x <= 1d / 16; x += 1d / 256) {
                ddouble y0 = ddouble.Log1p(x);
                ddouble y1 = Log1pPadeNearZero(x);
                ddouble y2 = ddouble.Log(x + 1);

                Console.WriteLine(x);
                Console.WriteLine(y0);
                Console.WriteLine(y1);
                Console.WriteLine(y2);

                Console.WriteLine("");
            }

            for (ddouble x = -1d / 17; x <= 1d / 17; x += 1d / 170) {
                ddouble y0 = ddouble.Log1p(x);
                ddouble y1 = Log1pPadeNearZero(x);
                ddouble y2 = ddouble.Log(x + 1);

                Console.WriteLine(x);
                Console.WriteLine(y0);
                Console.WriteLine(y1);
                Console.WriteLine(y2);

                Console.WriteLine("");
            }

            for (ddouble x = -1d / 1700; x <= 1d / 1700; x += 1d / 17000) {
                ddouble y0 = ddouble.Log1p(x);
                ddouble y1 = Log1pPadeNearZero(x);
                ddouble y2 = ddouble.Log(x + ddouble.Rcp(65535) + 65534 * ddouble.Rcp(65535));

                Console.WriteLine(x);
                Console.WriteLine(y0);
                Console.WriteLine(y1);
                Console.WriteLine(y2);

                Console.WriteLine("");
            }

            Console.WriteLine("END");
            Console.Read();
        }

        static ddouble Log1pPadeNearZero(ddouble x) {
            const int n = 9;
            double[] c = new double[n] { 140, 51561, 1382040, 12885180, 56936880, 133963830, 172372200, 114414300, 30630600 };
            double[] d = new double[n] { 9, 360, 4620, 27720, 90090, 168168, 180180, 102960, 24310 };

            ddouble sc = x * c[0] + c[1], sd = x * d[0] + d[1];

            for (int i = 2; i < n; i++) {
                sc = x * sc + c[i];
                sd = x * sd + d[i];
            }

            ddouble y = x * sc / (sd * 1260);

            return y;
        }
    }
}
