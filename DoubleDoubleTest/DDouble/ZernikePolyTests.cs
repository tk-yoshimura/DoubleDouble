using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ZernikePolyTests {
        private static readonly Dictionary<(int, int), Func<ddouble, ddouble>> ZernikeTables =
            new Dictionary<(int, int), Func<ddouble, ddouble>>{

                { (0,  0), (x) => 1d },

                { (1,  1), (x) => x },

                { (2,  0), (x) => -1 + x * x * 2 },
                { (2,  2), (x) => x * x },

                { (3,  1), (x) => x * (-2 + x * x * 3) },
                { (3,  3), (x) => ddouble.Pow(x, 3) },

                { (4,  0), (x) => 1 + x * x * (-6 + x * x * 6) },
                { (4,  2), (x) => ddouble.Pow(x, 2) * (-3 + x * x * 4) },
                { (4,  4), (x) => ddouble.Pow(x, 4) },

                { (5,  1), (x) => x * (3 + x * x * (-12 + x * x * 10)) },
                { (5,  3), (x) => ddouble.Pow(x, 3) * (-4 + x * x * 5) },
                { (5,  5), (x) => ddouble.Pow(x, 5) },

                { (6,  0), (x) => -1 + x * x * (12 + x * x * (-30 + x * x * 20)) },
                { (6,  2), (x) => ddouble.Pow(x, 2) * (6 + x * x * (-20 + x * x * 15)) },
                { (6,  4), (x) => ddouble.Pow(x, 4) * (-5 + x * x * 6) },
                { (6,  6), (x) => ddouble.Pow(x, 6) },

                { (7,  1), (x) => x * (-4 + x * x * (30 + x * x * (-60 + x * x * 35))) },
                { (7,  3), (x) => ddouble.Pow(x, 3) * (10 + x * x * (-30 + x * x * 21)) },
                { (7,  5), (x) => ddouble.Pow(x, 5) * (-6 + x * x * 7) },
                { (7,  7), (x) => ddouble.Pow(x, 7) },
        };

        [TestMethod]
        public void ZernikeRTest() {
            for (int n = 64; n >= 0; n--) {
                for (int m = -n; m <= n; m++) {
                    for (ddouble x = 0; x <= 1; x += 0.0625) {
                        ddouble actual = ddouble.ZernikeR(n, m, x);

                        Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{m},{x}");

                        if (((n + m) & 1) == 1) {
                            Assert.AreEqual(0, actual, $"{n},{m},{x}");
                        }
                    }
                }
            }

            foreach ((int n, int m) in ZernikeTables.Keys) {
                for (ddouble x = 0; x <= 1; x += 0.0625) {
                    ddouble expected = ZernikeTables[(n, m)](x);
                    ddouble actual = ddouble.ZernikeR(n, m, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{m},{x}");
                }
            }
        }
    }
}
