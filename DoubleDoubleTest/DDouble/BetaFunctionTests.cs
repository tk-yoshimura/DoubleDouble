using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class BetaFunctionTests {
        [TestMethod]
        public void BetaTest() {
            for (ddouble b = 1d / 32; b <= 64; b *= 2) {
                for (ddouble a = 1d / 32; a <= 64; a *= 2) {
                    Console.WriteLine($"{a},{b}");

                    ddouble f = ddouble.Beta(a, b);
                    ddouble g = ddouble.Beta(a + 1, b);
                    ddouble h = ddouble.Beta(a, b + 1);

                    HPAssert.AreEqual(a * h, b * g, f * 1e-27);
                    HPAssert.AreEqual(f, g + h, f * 1e-27);
                }

                HPAssert.AreEqual(1d / b, ddouble.Beta(b, 1), 1e-27 / b);
                HPAssert.AreEqual(1d / b, ddouble.Beta(1, b), 1e-27 / b);
            }
        }

        [TestMethod]
        public void IncompleteBetaTest() {
            ddouble r = ddouble.IncompleteBeta(0.25,0.0625,32);

            for (ddouble b = 1d / 64; b <= 32; b *= 2) {
                for (ddouble a = 1d / 64; a <= 32; a *= 2) {

                    for (ddouble x = 0; x <= 1; x += 1d / 8) {
                        Console.WriteLine($"{x},{a},{b}");

                        ddouble f = ddouble.IncompleteBeta(x, a, b);
                        ddouble g = ddouble.IncompleteBeta(x, a + 1, b);
                        ddouble h = ddouble.IncompleteBeta(x, a, b + 1);

                        ddouble p = ddouble.Pow(x, a) * ddouble.Pow(1d - x, b);

                        HPAssert.AreEqual(g, (a * f - p) / (a + b), g * 1e-27, $"beta({x},{a},{b})");
                        HPAssert.AreEqual(h, (b * f + p) / (a + b), h * 1e-27, $"beta({x},{a},{b})");
                    }
                }
            }

            for (ddouble a = 1d / 32; a <= 64; a += 1d / 16) {
                for (ddouble x = 1d / 16; x <= 1; x += 1d / 16) {
                    Console.WriteLine($"{x},{a}");

                    ddouble v = ddouble.Pow(x, a) / a;
                    ddouble y = ddouble.IncompleteBeta(x, a, 1);

                    ddouble err = ddouble.Abs(v / y - 1);

                    Console.WriteLine(err);

                    HPAssert.AreEqual(v, y, v * 1e-28, $"beta({x},{a},1)");
                }
            }

            for (ddouble b = 1d / 16; b <= 64; b += 1d / 16) {
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
        }
    }
}
