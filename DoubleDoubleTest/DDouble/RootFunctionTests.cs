using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class RootFunctionTests {
        [TestMethod]
        public void SqrtTest() {
            for (decimal d = 0; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (ddouble)d;

                PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (ddouble)d;

                PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sqrt_pzero = ddouble.Sqrt(0d);
            ddouble sqrt_mzero = ddouble.Sqrt(-0d);
            ddouble sqrt_pinf = ddouble.Sqrt(double.PositiveInfinity);
            ddouble sqrt_ninf = ddouble.Sqrt(double.NegativeInfinity);
            ddouble sqrt_nan = ddouble.Sqrt(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sqrt_pzero), nameof(sqrt_pzero));
            Assert.IsTrue(ddouble.IsNaN(sqrt_mzero), nameof(sqrt_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(sqrt_pinf), nameof(sqrt_pinf));
            Assert.IsTrue(ddouble.IsNaN(sqrt_ninf), nameof(sqrt_ninf));
            Assert.IsTrue(ddouble.IsNaN(sqrt_nan), nameof(sqrt_nan));
        }

        [TestMethod]
        public void CbrtTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Cbrt(v);
                ddouble u = w * w * w - (ddouble)d;

                PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 8e-31, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Cbrt(v);
                ddouble u = w * w * w - (ddouble)d;

                PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 8e-31, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cbrt_pzero = ddouble.Cbrt(0d);
            ddouble cbrt_mzero = ddouble.Cbrt(-0d);
            ddouble cbrt_pinf = ddouble.Cbrt(double.PositiveInfinity);
            ddouble cbrt_ninf = ddouble.Cbrt(double.NegativeInfinity);
            ddouble cbrt_nan = ddouble.Cbrt(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(cbrt_pzero), nameof(cbrt_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(cbrt_mzero), nameof(cbrt_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(cbrt_pinf), nameof(cbrt_pinf));
            Assert.IsTrue(ddouble.IsNegativeInfinity(cbrt_ninf), nameof(cbrt_ninf));
            Assert.IsTrue(ddouble.IsNaN(cbrt_nan), nameof(cbrt_nan));
        }

        [TestMethod]
        public void RootOddNTest() {
            for (int n = 1; n <= 63; n += 2) {
                for (decimal d = -10m; d <= +10m; d += 0.01m) {
                    ddouble v = (ddouble)d;
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)d;

                    PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 2e-30, $"{d},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }

                for (decimal d = -10000m; d <= +10000m; d += 10m) {
                    ddouble v = (ddouble)d;
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)d;

                    PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 2e-30, $"{d},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }

                ddouble nroot_pzero = ddouble.RootN(0d, n);
                ddouble nroot_mzero = ddouble.RootN(-0d, n);
                ddouble nroot_pinf = ddouble.RootN(double.PositiveInfinity, n);
                ddouble nroot_ninf = ddouble.RootN(double.NegativeInfinity, n);
                ddouble nroot_nan = ddouble.RootN(double.NaN, n);

                Assert.IsTrue(ddouble.IsPlusZero(nroot_pzero), nameof(nroot_pzero));
                Assert.IsTrue(ddouble.IsMinusZero(nroot_mzero), nameof(nroot_mzero));
                Assert.IsTrue(ddouble.IsPositiveInfinity(nroot_pinf), nameof(nroot_pinf));
                Assert.IsTrue(ddouble.IsNegativeInfinity(nroot_ninf), nameof(nroot_ninf));
                Assert.IsTrue(ddouble.IsNaN(nroot_nan), nameof(nroot_nan));
            }

            for (int n = 9; n <= 951; n += 2) {
                ddouble p = ddouble.Ldexp(1, +n);
                foreach (ddouble v in new ddouble[] { p - 1, ddouble.BitDecrement(p), p, ddouble.BitIncrement(p), p + 1 }) {
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)v;

                    PrecisionAssert.AreEqual(0, u, v * 8e-30, $"{v},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }
        }

        [TestMethod]
        public void RootEvenNTest() {
            for (int n = 2; n <= 64; n += 2) {
                for (decimal d = 0; d <= +10m; d += 0.01m) {
                    ddouble v = (ddouble)d;
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)d;

                    PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 2e-30, $"{d},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }

                for (decimal d = 10m; d <= +10000m; d += 10m) {
                    ddouble v = (ddouble)d;
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)d;

                    PrecisionAssert.AreEqual(0, u, Math.Abs((double)d) * 2e-30, $"{d},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }

                ddouble nroot_pzero = ddouble.RootN(0d, n);
                ddouble nroot_mzero = ddouble.RootN(-0d, n);
                ddouble nroot_pinf = ddouble.RootN(double.PositiveInfinity, n);
                ddouble nroot_ninf = ddouble.RootN(double.NegativeInfinity, n);
                ddouble nroot_nan = ddouble.RootN(double.NaN, n);

                Assert.IsTrue(ddouble.IsPlusZero(nroot_pzero), nameof(nroot_pzero));
                Assert.IsTrue(ddouble.IsNaN(nroot_mzero), nameof(nroot_mzero));
                Assert.IsTrue(ddouble.IsPositiveInfinity(nroot_pinf), nameof(nroot_pinf));
                Assert.IsTrue(ddouble.IsNaN(nroot_ninf), nameof(nroot_ninf));
                Assert.IsTrue(ddouble.IsNaN(nroot_nan), nameof(nroot_nan));
            }

            for (int n = 10; n <= 952; n += 2) {
                ddouble p = ddouble.Ldexp(1, +n);
                foreach (ddouble v in new ddouble[] { p - 1, ddouble.BitDecrement(p), p, ddouble.BitIncrement(p), p + 1 }) {
                    ddouble w = ddouble.RootN(v, n);
                    ddouble u = ddouble.Pow(w, n) - (ddouble)v;

                    PrecisionAssert.AreEqual(0, u, v * 8e-30, $"{v},{n}");
                    Assert.IsTrue(ddouble.IsRegulared(v));
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }
        }

        [TestMethod]
        public void HypotTest() {
            Assert.AreEqual(ddouble.Zero, ddouble.Hypot(0, 0));
            Assert.AreEqual(ddouble.Zero, ddouble.Hypot(0, 0, 0));

            Assert.AreEqual(5, ddouble.Hypot(3, 4));
            Assert.AreEqual(ddouble.Sqrt(195), ddouble.Hypot(5, 7, 11));

            Assert.AreEqual(5, ddouble.Hypot(-3, -4));
            Assert.AreEqual(ddouble.Sqrt(195), ddouble.Hypot(-5, -7, -11));

            Assert.AreEqual(3, ddouble.Hypot(3, 0));
            Assert.AreEqual(4, ddouble.Hypot(0, 4));
            Assert.AreEqual(5, ddouble.Hypot(5, 0, 0));
            Assert.AreEqual(7, ddouble.Hypot(0, 7, 0));
            Assert.AreEqual(11, ddouble.Hypot(0, 0, 11));

            Assert.AreEqual(5, ddouble.Hypot(-3, 4));
            Assert.AreEqual(ddouble.Sqrt(195), ddouble.Hypot(-5, 7, 11));
            Assert.AreEqual(ddouble.Sqrt(195), ddouble.Hypot(5, -7, 11));
            Assert.AreEqual(ddouble.Sqrt(195), ddouble.Hypot(5, 7, -11));

            Assert.AreEqual(3, ddouble.Hypot(-3, 0));
            Assert.AreEqual(5, ddouble.Hypot(-5, 0, 0));
            Assert.AreEqual(7, ddouble.Hypot(0, -7, 0));
            Assert.AreEqual(11, ddouble.Hypot(0, 0, -11));

            PrecisionAssert.AlmostEqual("5e-250", ddouble.Hypot("3e-250", "4e-250"), 1e-30);
            PrecisionAssert.AlmostEqual(ddouble.Sqrt(195) * "1e-250", ddouble.Hypot("5e-250", "7e-250", "11e-250"), 1e-30);

            PrecisionAssert.AlmostEqual("5e+250", ddouble.Hypot("3e+250", "4e+250"), 1e-30);
            PrecisionAssert.AlmostEqual(ddouble.Sqrt(195) * "1e+250", ddouble.Hypot("5e+250", "7e+250", "11e+250"), 1e-30);

            PrecisionAssert.AlmostEqual("3e+250", ddouble.Hypot("3e+250", "4e-250"), 1e-30);
            PrecisionAssert.AlmostEqual("5e+250", ddouble.Hypot("5e+250", "7e-250", "11e-250"), 1e-30);

            Assert.IsTrue(ddouble.IsNaN(ddouble.Hypot(0, ddouble.NaN)));
            Assert.IsTrue(ddouble.IsNaN(ddouble.Hypot(0, 0, ddouble.NaN)));
        }
    }
}
