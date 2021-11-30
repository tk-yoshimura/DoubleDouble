using DoubleDouble;
using System;
using System.IO;
using System.Linq;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            Random random = new Random(1234);

            ddouble[] vs = (new ddouble[256]).Select((_)=>
                (random.Next(2) == 0 ? +1 : -1) * ddouble.Rcp(0.1 + random.NextDouble())).ToArray();

            KahanSum h1 = ddouble.Zero;
            ddouble h2 = 0;

            foreach (ddouble v in vs) {
                KahanSum back = h1;

                h1.Add(v);
                h2 += v;

                if (ddouble.Abs(h1.Sum) * 1e-20 < ddouble.Abs(h1.C)) {
                    Console.WriteLine($"{back.Sum},{back.C} += {v}"); 
                    Console.WriteLine($"{h1.Sum},{h1.C}");
                    Console.WriteLine("whats happen?\n");
                }
            }

            foreach (ddouble v in vs) {
                h1.Add(-v);
                h2 -= v;
            }

            Console.WriteLine(h1.Sum);
            Console.WriteLine(h2);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
