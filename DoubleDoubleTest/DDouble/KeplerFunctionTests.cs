using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
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

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, 1e-120, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }
            }

            for (ddouble e = 1d / 32; e > Math.ScaleB(1, -128); e /= 2) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, 1e-120, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }
            }

            for (ddouble e = 31d / 32; e < (ddouble)1 - Math.ScaleB(1, -108); e = (e - 1) / 2 + 1) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > Math.ScaleB(1, -72); m /= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    if (e < (ddouble)1 - Math.ScaleB(1, -18)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-25, 1e-120, $"{m},{e}"); // x - e sin(x) digits loss
                        Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                    }
                    else if (e < (ddouble)1 - Math.ScaleB(1, -36)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-20, 1e-120, $"{m},{e}"); // x - e sin(x) digits loss
                        Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                    }
                    else {
                        PrecisionAssert.AlmostEqual(m, y, 1e-15, 1e-120, $"{m},{e}"); // x - e sin(x) digits loss
                        Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                    }
                }

                for (ddouble m = Math.ScaleB(1, -72); m > 0; m /= 2) {
                    ddouble x1 = ddouble.KeplerE(m, e);
                    ddouble x2 = ddouble.KeplerE(m * 2, e);

                    Assert.IsTrue(x1 >= 0, $"{m},{e}\n{x1}");
                    Assert.IsTrue(x2 >= x1, $"{m},{e}\n{x1}\n{x2}");
                    Assert.IsTrue(ddouble.Abs(m - x1) <= 1, $"{m},{e}\n{x1}");
                }
            }

            {
                ddouble e = 1;

                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = x - e * ddouble.Sin(x);

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                    Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 2) {
                    ddouble x1 = ddouble.KeplerE(m, e);
                    ddouble x2 = ddouble.KeplerE(m * 2, e);

                    Assert.IsTrue(x1 >= 0, $"{m},{e}\n{x1}");
                    Assert.IsTrue(x2 >= x1, $"{m},{e}\n{x1}\n{x2}");
                    Assert.IsTrue(ddouble.Abs(m - x1) <= 1, $"{m},{e}\n{x1}");
                }
            }

            for (ddouble m = -32; m <= 32; m += 1d / 32) {
                ddouble x = ddouble.KeplerE(m, 0.5, centered: true) + m;
                ddouble y = x - 0.5 * ddouble.Sin(x);

                PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},centered");
                Assert.IsTrue(ddouble.Abs(m - x) <= 1, $"{m},centered");
            }
        }

        [TestMethod]
        public void KeplerEHyperbolicTest() {
            for (ddouble e = 17d / 16; e <= 16; e = (e < 2) ? e + 1d / 16 : e * 2) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 64; m += 1d / 8) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                }

                for (ddouble m = 128; m <= 16777216; m *= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (ddouble.IsFinite(y)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-20, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        Assert.IsFalse(ddouble.IsNaN(x), $"{m},{e}");
                    }
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, 1e-250, $"{m},{e}");
                }
            }

            for (ddouble e = 16; e <= 256; e *= 2) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 64; m += 1d / 8) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-20, $"{m},{e}");
                }

                for (ddouble m = 128; m <= 16777216; m *= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (ddouble.IsFinite(y)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-10, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        Assert.IsFalse(ddouble.IsNaN(x), $"{m},{e}");
                    }
                }

                for (ddouble m = 1d / 64; m > 0; m /= 16) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-20, 1e-250, $"{m},{e}");
                }
            }

            for (ddouble e = 33d / 32; e > (ddouble)1 + Math.ScaleB(1, -108); e = (e - 1) / 2 + 1) {
                Console.WriteLine(e);

                for (ddouble m = -16; m <= 64; m += 1d / 8) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                }

                for (ddouble m = 128; m <= 16777216; m *= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (ddouble.IsFinite(y)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-20, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        Assert.IsFalse(ddouble.IsNaN(x), $"{m},{e}");
                    }
                }

                for (ddouble m = 1d / 64; m > Math.ScaleB(1, -72); m /= 2) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    if (e > (ddouble)1 + Math.ScaleB(1, -18)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-25, 1e-250, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else if (e > (ddouble)1 + Math.ScaleB(1, -36)) {
                        PrecisionAssert.AlmostEqual(m, y, 1e-20, 1e-250, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                    else {
                        PrecisionAssert.AlmostEqual(m, y, 1e-15, 1e-250, $"{m},{e}"); // e sinh(x) - x digits loss
                    }
                }

                for (ddouble m = Math.ScaleB(1, -72); m > 0; m /= 2) {
                    ddouble x1 = ddouble.KeplerE(m, e);
                    ddouble x2 = ddouble.KeplerE(m * 2, e);

                    Assert.IsTrue(x1 >= 0, $"{m},{e}\n{x1}");
                    Assert.IsTrue(x2 >= x1, $"{m},{e}\n{x1}\n{x2}");
                    Assert.IsTrue(ddouble.Abs(m - x1) <= 1, $"{m},{e}\n{x1}");
                }
            }

            {
                ddouble e = 1 + ddouble.Ldexp(1, -108);

                Console.WriteLine(e);

                for (ddouble m = -16; m <= 16; m += 1d / 32) {
                    ddouble x = ddouble.KeplerE(m, e);
                    ddouble y = e * ddouble.Sinh(x) - x;

                    PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},{e}");
                }

                for (ddouble m = 1d / 64; m > 0; m /= 2) {
                    ddouble x1 = ddouble.KeplerE(m, e);
                    ddouble x2 = ddouble.KeplerE(m * 2, e);

                    Assert.IsTrue(x1 >= 0, $"{m},{e}\n{x1}");
                    Assert.IsTrue(x2 >= x1, $"{m},{e}\n{x1}\n{x2}");
                    Assert.IsTrue(ddouble.Abs(m - x1) <= 1, $"{m},{e}\n{x1}");
                }
            }

            for (ddouble m = -32; m <= 32; m += 1d / 32) {
                ddouble x = ddouble.KeplerE(m, 2, centered: true) + m;
                ddouble y = 2 * ddouble.Sinh(x) - x;

                PrecisionAssert.AlmostEqual(m, y, 1e-30, $"{m},centered");
            }
        }
    }
}
