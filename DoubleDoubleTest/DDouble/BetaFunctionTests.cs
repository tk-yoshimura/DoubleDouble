using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class BetaFunctionTests {
        readonly ddouble[] tests_ab = new ddouble[] {
            1d / 64, 1d / 32, 1d / 16, 1d / 8, 1d / 4, 1d / 2, 1,
            3d / 2, 2, 9d / 4, 5d / 2, 11d / 4, 3,
            4, 7.5, 7.75, 8, 15.5, 15.75, 16, 31.5, 31.75,
            32, 63.5, 63.75, 64, 127.5, 127.75, 128
        };

        [TestMethod]
        public void BetaTest() {
            foreach (ddouble b in tests_ab) {
                foreach (ddouble a in tests_ab) {
                    Console.WriteLine($"{a},{b}");

                    ddouble f = ddouble.Beta(a, b);
                    ddouble g = ddouble.Beta(a + 1, b);
                    ddouble h = ddouble.Beta(a, b + 1);

                    HPAssert.AreEqual(a * h, b * g, a * h * 4e-28);
                    HPAssert.AreEqual(f, g + h, f * 4e-28);
                }

                HPAssert.AreEqual(1d / b, ddouble.Beta(b, 1), 4e-28 / b);
                HPAssert.AreEqual(1d / b, ddouble.Beta(1, b), 4e-28 / b);
            }
        }

        [TestMethod]
        public void IncompleteBetaTest() {

            foreach (ddouble b in tests_ab) {
                foreach (ddouble a in tests_ab) {
                    for (ddouble x = 0; x <= 1; x += 1d / 8) {
                        Console.WriteLine($"{x},{a},{b}");

                        ddouble f = ddouble.IncompleteBeta(x, a, b);
                        ddouble g = ddouble.IncompleteBeta(x, a + 1, b);
                        ddouble h = ddouble.IncompleteBeta(x, a, b + 1);

                        ddouble p = ddouble.Pow(x, a) * ddouble.Pow(1d - x, b);

                        HPAssert.AreEqual(g, (a * f - p) / (a + b), g * 4e-28, $"beta({x},{a},{b})");
                        HPAssert.AreEqual(h, (b * f + p) / (a + b), h * 4e-28, $"beta({x},{a},{b})");
                    }
                }
            }

            foreach (ddouble a in tests_ab) {
                for (ddouble x = 1d / 16; x <= 1; x += 1d / 16) {
                    Console.WriteLine($"{x},{a}");

                    ddouble v = ddouble.Pow(x, a) / a;
                    ddouble y = ddouble.IncompleteBeta(x, a, 1);

                    ddouble err = ddouble.Abs(v / y - 1);

                    Console.WriteLine(err);

                    HPAssert.AreEqual(v, y, v * 1e-28, $"beta({x},{a},1)");
                }
            }

            foreach (ddouble b in tests_ab) {
                for (ddouble x = 1d / 16; x <= 1; x += 1d / 16) {
                    Console.WriteLine($"{x},{b}");

                    ddouble v = (1 - ddouble.Pow(1 - x, b)) / b;
                    ddouble y = ddouble.IncompleteBeta(x, 1, b);

                    ddouble err = ddouble.Abs(v / y - 1);
                    Console.WriteLine(err);

                    HPAssert.AreEqual(v, y, v * 1e-28, $"beta({x},1,{b})");
                }
            }

            HPAssert.AreEqual(
                "0.0107206939812333955315378535295139067594857737069262075239118707",
                ddouble.IncompleteBeta(1 / 8d, 15 / 8d, 35 / 32d), 1e-30);

            for (ddouble ab = 500; ab <= 512; ab += 0.125) {
                ddouble v = ddouble.IncompleteBeta(0.5, ab, ab);

                Console.WriteLine(v);
                Assert.IsTrue(ddouble.IsFinite(v));
            }
        }

        [TestMethod]
        public void LogBetaTest() {
            foreach (ddouble b in tests_ab) {
                foreach (ddouble a in tests_ab) {
                    ddouble expected = ddouble.Log(ddouble.Beta(a, b));
                    ddouble actual = ddouble.LogBeta(a, b);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-29, $"{a}, {b}");
                }
            }
        }

        [TestMethod]
        public void IncompleteBetaRegularizedTest() {
            foreach (ddouble b in tests_ab) {
                foreach (ddouble a in tests_ab) {
                    for (ddouble x = 0; x <= 1; x += 0.125) {
                        ddouble expected = ddouble.IncompleteBeta(x, a, b) / ddouble.Beta(a, b);
                        ddouble actual = ddouble.IncompleteBetaRegularized(x, a, b);

                        HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-29, $"{x},{a},{b}");
                    }
                }
            }

            for (ddouble ab = 4000; ab <= 4096; ab += 0.5) {
                ddouble v = ddouble.IncompleteBetaRegularized(0.5, ab, ab);

                Console.WriteLine(v);
                Assert.IsTrue(ddouble.IsFinite(v));
            }
        }
    }
}
