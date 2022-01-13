using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class PowFunctionTests {
        [TestMethod]
        public void PowNTest() {
            ddouble v = ddouble.Pow(5, 308);

            Assert.IsTrue(ddouble.Abs((ddouble)"1.917614634881924434803035919916513923037e215" - v) < "1e185");
        }

        [TestMethod]
        public void Pow2Test() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Pow2(v);

                Assert.AreEqual(Math.Pow(2, (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Pow2(v);

                Assert.AreEqual(Math.Pow(2, (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            HPAssert.AreEqual("1.99932321299248736788439373752563131122422617961", ddouble.Pow2(2047d / 2048), 1e-31);
            HPAssert.AreEqual("1.25992104989487316476721060727822835057025146470", ddouble.Pow2(ddouble.Rcp(3)), 1e-31);

            Assert.AreEqual(FloatSplitter.Split(ddouble.Pow2(0.5d)).mantissa, FloatSplitter.Split(ddouble.Pow2(ddouble.BitIncrement(0.5d))).mantissa);
            Assert.AreEqual(FloatSplitter.Split(ddouble.Pow2(0.5d)).mantissa, FloatSplitter.Split(ddouble.Pow2(ddouble.BitDecrement(0.5d))).mantissa);
            Assert.AreEqual(FloatSplitter.Split(ddouble.Pow2(1d)).mantissa, FloatSplitter.Split(ddouble.Pow2(ddouble.BitDecrement(1d))).mantissa);

            HPAssert.AreEqual(ddouble.Sqrt(2), ddouble.Pow2(ddouble.BitIncrement(0.5d)), 1e-31, "0.5+eps");
            HPAssert.AreEqual(ddouble.Sqrt(2), ddouble.Pow2(0.5d), 1e-31);
            HPAssert.AreEqual(ddouble.Sqrt(2), ddouble.Pow2(ddouble.BitDecrement(0.5d)), 1e-31, "0.5-eps");
            HPAssert.AreEqual("1.18920711500272106671749997056047591529297209246e0", ddouble.Pow2(1d / 4), 1e-31);
            HPAssert.AreEqual("1.09050773266525765920701065576070797899270271854e0", ddouble.Pow2(1d / 8), 1e-31);
            HPAssert.AreEqual("1.00033850805268231295330548185621640403555852068e0", ddouble.Pow2(1d / 2048), 1e-31);

            for (long i = 0; i < 19; i++) {
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(i)).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(ddouble.BitDecrement(i))).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(ddouble.BitIncrement(i))).mantissa:X14}");

                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(-i)).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(ddouble.BitDecrement(-i))).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow2(ddouble.BitIncrement(-i))).mantissa:X14}");
            }

            ddouble pow2_pzero = ddouble.Pow2(0d);
            ddouble pow2_mzero = ddouble.Pow2(-0d);
            ddouble pow2_pinf = ddouble.Pow2(double.PositiveInfinity);
            ddouble pow2_ninf = ddouble.Pow2(double.NegativeInfinity);
            ddouble pow2_nan = ddouble.Pow2(double.NaN);

            Assert.IsTrue(pow2_pzero == 1, nameof(pow2_pzero));
            Assert.IsTrue(pow2_mzero == 1, nameof(pow2_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(pow2_pinf), nameof(pow2_pinf));
            Assert.IsTrue(ddouble.IsPlusZero(pow2_ninf), nameof(pow2_ninf));
            Assert.IsTrue(ddouble.IsNaN(pow2_nan), nameof(pow2_nan));
        }

        [TestMethod]
        public void PowTest() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = 0.1m; x <= +10m; x += 0.1m) {
                    if (y == 0) {
                        continue;
                    }

                    ddouble u = ddouble.Pow((ddouble)x, (ddouble)y);

                    Assert.AreEqual(Math.Pow((double)x, (double)y), (double)u, (double)u * 1e-12);
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }
        }

        [TestMethod]
        public void Pow10Test() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Pow10(v);

                Assert.AreEqual(Math.Pow(10, (double)d), (double)u, (double)u * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (long i = 0, n = 1; i < 19; i++, n *= 10) {
                Assert.AreEqual(n, ddouble.Pow10(i));
                Assert.AreEqual(ddouble.Rcp(n), ddouble.Pow10(-i));
            }

            for (long i = 0; i < 19; i++) {
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(i)).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(ddouble.BitDecrement(i))).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(ddouble.BitIncrement(i))).mantissa:X14}");

                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(-i)).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(ddouble.BitDecrement(-i))).mantissa:X14}");
                Console.WriteLine($"0x{FloatSplitter.Split(ddouble.Pow10(ddouble.BitIncrement(-i))).mantissa:X14}");
            }

            ddouble pow10_pzero = ddouble.Pow10(0d);
            ddouble pow10_mzero = ddouble.Pow10(-0d);
            ddouble pow10_pinf = ddouble.Pow10(double.PositiveInfinity);
            ddouble pow10_ninf = ddouble.Pow10(double.NegativeInfinity);
            ddouble pow10_nan = ddouble.Pow10(double.NaN);

            Assert.IsTrue(pow10_pzero == 1, nameof(pow10_pzero));
            Assert.IsTrue(pow10_mzero == 1, nameof(pow10_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(pow10_pinf), nameof(pow10_pinf));
            Assert.IsTrue(ddouble.IsPlusZero(pow10_ninf), nameof(pow10_ninf));
            Assert.IsTrue(ddouble.IsNaN(pow10_nan), nameof(pow10_nan));
        }

        [TestMethod]
        public void ExpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Exp(v);

                Assert.AreEqual(Math.Exp((double)d), (double)u, (double)u * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.E - ddouble.Exp(1)) < 1e-31);

            ddouble exp_pzero = ddouble.Exp(0d);
            ddouble exp_mzero = ddouble.Exp(-0d);
            ddouble exp_pinf = ddouble.Exp(double.PositiveInfinity);
            ddouble exp_ninf = ddouble.Exp(double.NegativeInfinity);
            ddouble exp_nan = ddouble.Exp(double.NaN);

            Assert.IsTrue(exp_pzero == 1, nameof(exp_pzero));
            Assert.IsTrue(exp_mzero == 1, nameof(exp_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(exp_pinf), nameof(exp_pinf));
            Assert.IsTrue(ddouble.IsPlusZero(exp_ninf), nameof(exp_ninf));
            Assert.IsTrue(ddouble.IsNaN(exp_nan), nameof(exp_nan));
        }

        [TestMethod]
        public void Expm1Test() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Expm1(v);

                Assert.AreEqual(Math.Exp((double)d) - 1, (double)u, Math.Abs((double)u) * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            HPAssert.AreEqual(0, ddouble.Expm1(ddouble.BitDecrement(0)), 1e-300);
            HPAssert.AreEqual(0, ddouble.Expm1(ddouble.BitIncrement(0)), 1e-300);

            Console.WriteLine(ddouble.Expm1(ddouble.BitDecrement(0)));
            Console.WriteLine(ddouble.Expm1(0));
            Console.WriteLine(ddouble.Expm1(ddouble.BitIncrement(0)));

            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(ddouble.BitDecrement(-0.25d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(-0.25d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(ddouble.BitIncrement(-0.25d))).mantissa);

            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(ddouble.BitDecrement(0.25d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(0.25d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Expm1(ddouble.BitIncrement(0.25d))).mantissa);

            Console.WriteLine(ddouble.Expm1(ddouble.BitDecrement(-0.25d)));
            Console.WriteLine(ddouble.Expm1(-0.25d));
            Console.WriteLine(ddouble.Expm1(ddouble.BitIncrement(-0.25d)));

            Console.WriteLine(ddouble.Expm1(ddouble.BitDecrement(0.25d)));
            Console.WriteLine(ddouble.Expm1(0.25d));
            Console.WriteLine(ddouble.Expm1(ddouble.BitIncrement(0.25d)));

            ddouble exp_pzero = ddouble.Expm1(0d);
            ddouble exp_mzero = ddouble.Expm1(-0d);
            ddouble exp_pinf = ddouble.Expm1(double.PositiveInfinity);
            ddouble exp_ninf = ddouble.Expm1(double.NegativeInfinity);
            ddouble exp_nan = ddouble.Expm1(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(exp_pzero), nameof(exp_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(exp_mzero), nameof(exp_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(exp_pinf), nameof(exp_pinf));
            Assert.IsTrue(exp_ninf == -1, nameof(exp_ninf));
            Assert.IsTrue(ddouble.IsNaN(exp_nan), nameof(exp_nan));
        }
    }
}
