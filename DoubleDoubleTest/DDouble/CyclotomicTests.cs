using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class CyclotomicTests {
        [TestMethod]
        public void CyclotomicTest() {
            for (int n = 1; n <= 32; n++) {
                Console.WriteLine($"n = {n}");

                for (ddouble x = 0; x <= 4; x += 0.25) {
                    ddouble y = 1;

                    for (int k = 1; k <= n; k++) {
                        if ((n % k) != 0) {
                            continue;
                        }

                        y *= ddouble.Cyclotomic(k, x);
                    }

                    ddouble expected = ddouble.Pow(x, n) - 1;

                    Console.WriteLine($"{x}");
                    Console.WriteLine($"{expected}");
                    Console.WriteLine($"{y}");

                    PrecisionAssert.AlmostEqual(expected, y, 1e-31, $"{n}, {x}");
                }
            }
        }
    }
}
