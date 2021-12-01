using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class RootFunctionTest {
        [TestMethod]
        public void SqrtTest() {
            for (decimal d = 0; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (ddouble)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (ddouble)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-32, $"{d}");
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

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-31, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble w = ddouble.Cbrt(v);
                ddouble u = w * w * w - (ddouble)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-31, $"{d}");
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
    }
}
