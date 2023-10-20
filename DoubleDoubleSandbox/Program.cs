using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    public static class Program {
        static void Main() {
            Console.WriteLine($"x,Ai,Bi,Gi,Hi");
            
            for (ddouble x = -8; x <= 8; x += 1d / 32) {
                Console.WriteLine($"{x},{ddouble.AiryAi(x)},{ddouble.AiryBi(x)},{ddouble.ScorerGi(x)},{ddouble.ScorerHi(x)}");
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
