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

            PrecisionAssert.AreEqual(0d, ddouble.Binomial(2, 3));
            PrecisionAssert.AreEqual(0d, ddouble.Binomial(2, -1));

            PrecisionAssert.AreEqual(0d, ddouble.Binomial(3, 4));
            PrecisionAssert.AreEqual(0d, ddouble.Binomial(3, -1));

            PrecisionAssert.AreEqual(0d, ddouble.Binomial(4, 5));
            PrecisionAssert.AreEqual(0d, ddouble.Binomial(4, -1));

            PrecisionAssert.AreEqual(-1d, ddouble.Binomial(-1, 1));

            PrecisionAssert.AreEqual(1, ddouble.Binomial(-2, 0));
            PrecisionAssert.AreEqual(-2d, ddouble.Binomial(-2, 1));
            PrecisionAssert.AreEqual(3d, ddouble.Binomial(-2, 2));
            PrecisionAssert.AreEqual(-4d, ddouble.Binomial(-2, 3));

            PrecisionAssert.AreEqual(1d, ddouble.Binomial(-3, 0));
            PrecisionAssert.AreEqual(-3d, ddouble.Binomial(-3, 1));
            PrecisionAssert.AreEqual(6d, ddouble.Binomial(-3, 2));
            PrecisionAssert.AreEqual(-10d, ddouble.Binomial(-3, 3));
            PrecisionAssert.AreEqual(15d, ddouble.Binomial(-3, 4));

            PrecisionAssert.AreEqual(1d, ddouble.Binomial(-4, 0));
            PrecisionAssert.AreEqual(-4d, ddouble.Binomial(-4, 1));
            PrecisionAssert.AreEqual(10d, ddouble.Binomial(-4, 2));
            PrecisionAssert.AreEqual(-20d, ddouble.Binomial(-4, 3));
            PrecisionAssert.AreEqual(35d, ddouble.Binomial(-4, 4));
            PrecisionAssert.AreEqual(-56d, ddouble.Binomial(-4, 5));

            PrecisionAssert.IsFinite(ddouble.Binomial(999, 499));
            PrecisionAssert.IsFinite(ddouble.Binomial(999, 500));
            PrecisionAssert.IsFinite(ddouble.Binomial(1000, 500));

            for (int n = -8; n <= 8; n++) {
                for (int k = -8; k <= 8; k++) {
                    ddouble x = ddouble.Binomial(n, k);

                    Console.WriteLine($"{n},{k}: {x}");
                }
            }
        }
    }
}
