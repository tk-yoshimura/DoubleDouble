using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class BinomialTests {
        [TestMethod]
        public void BinomialTest() {
            for (int n = 0; n <= 1000; n++) {
                for (int k = 0; k <= n; k += n / 64 + 1) {
                    Console.WriteLine($"checked {n},{k}");

                    if (k == 0 || k == n) {
                        PrecisionAssert.AreEqual(1, ddouble.Binomial(n, k), $"{n},{k}");
                    }
                    else {
                        ddouble expected = ddouble.Binomial(n - 1, k - 1) + ddouble.Binomial(n - 1, k);
                        ddouble actual = ddouble.Binomial(n, k);

                        if (expected < 1e+29) {
                            PrecisionAssert.AreEqual(expected, actual, $"{n},{k}");
                        }
                        else {
                            PrecisionAssert.AlmostEqual(expected, actual, 4e-31, $"{n},{k}");
                        }
                    }
                }
            }

            PrecisionAssert.IsFinite(ddouble.Binomial(999, 499));
            PrecisionAssert.IsFinite(ddouble.Binomial(999, 500));
            PrecisionAssert.IsFinite(ddouble.Binomial(1000, 500));
        }
    }
}
