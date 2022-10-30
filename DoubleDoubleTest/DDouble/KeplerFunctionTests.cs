using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class KeplerFunctionTests {
        [TestMethod]
        public void KeplerEEllipticTest() {
            for (ddouble e = 0; e < 1; e += 1d / 16) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30 + 1e-250, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }
            }

            for (ddouble e = 1d / 32; e > Math.ScaleB(1, -107); e /= 2) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30 + 1e-250, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }
            }

            for (ddouble e = 31d / 32; e < (ddouble)1 - Math.ScaleB(1, -107); e = (e - 1) / 2 + 1) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > Math.ScaleB(1, -32); m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-20 + 1e-250, $"{m},{e}"); // x - e sin(x) digits loss
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }
            }
        }

        [TestMethod]
        public void KeplerEHyperbolicTest() {
            for (ddouble e = 17d / 16; e <= 128; e = (e < 2) ? e + 1d / 16 : e * 2) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 64; m += 1d / 8) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30, $"{m},{e}");
                }

                for (ddouble m = 128; m <= ddouble.MaxValue; m *= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (ddouble.IsFinite(y)) {
                        HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-5, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        Assert.IsFalse(ddouble.IsNaN(x), $"{m},{e}");
                    }
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30 + 1e-250, $"{m},{e}");
                }
            }

            for (ddouble e = 33d / 32; e > (ddouble)1 + Math.ScaleB(1, -107); e = (e - 1) / 2 + 1) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 64; m += 1d / 8) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-30, $"{m},{e}");
                }

                for (ddouble m = 128; m <= ddouble.MaxValue; m *= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (ddouble.IsFinite(y)) {
                        HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-5, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        Assert.IsFalse(ddouble.IsNaN(x), $"{m},{e}");
                    }
                }

                for (ddouble m = 1d / 64; m > Math.ScaleB(1, -32); m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    HPAssert.AreEqual(m, y, ddouble.Abs(m) * 1e-20 + 1e-250, $"{m},{e}"); // e sinh(x) - x digits loss
                }
            }
        }
    }
}
