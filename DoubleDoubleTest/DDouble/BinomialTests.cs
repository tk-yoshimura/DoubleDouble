using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                        Assert.AreEqual(1, ddouble.Binomial(n, k), $"{n},{k}");
                    }
                    else {
                        ddouble expected = ddouble.Binomial(n - 1, k - 1) + ddouble.Binomial(n - 1, k);
                        ddouble actual = ddouble.Binomial(n, k);

                        if (expected < 1e+29) {
                            Assert.AreEqual(expected, actual, $"{n},{k}");
                        }
                        else {
                            HPAssert.AreEqual(expected, actual, expected * 8e-31, $"{n},{k}");
                        }
                    }
                }
            }

            Assert.IsTrue(ddouble.IsFinite(ddouble.Binomial(999, 499)));
            Assert.IsTrue(ddouble.IsFinite(ddouble.Binomial(999, 500)));
            Assert.IsTrue(ddouble.IsFinite(ddouble.Binomial(1000, 500)));
        }
    }
}
