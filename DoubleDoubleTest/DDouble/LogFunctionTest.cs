using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class LogFunctionTest {
        [TestMethod]
        public void Log2Test() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log2_pzero = ddouble.Log2(0d);
            ddouble log2_mzero = ddouble.Log2(-0d);
            ddouble log2_pinf = ddouble.Log2(double.PositiveInfinity);
            ddouble log2_ninf = ddouble.Log2(double.NegativeInfinity);
            ddouble log2_nan = ddouble.Log2(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log2_pzero), nameof(log2_pzero));
            Assert.IsTrue(ddouble.IsNaN(log2_mzero), nameof(log2_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log2_pinf), nameof(log2_pinf));
            Assert.IsTrue(ddouble.IsNaN(log2_ninf), nameof(log2_ninf));
            Assert.IsTrue(ddouble.IsNaN(log2_nan), nameof(log2_nan));

            ddouble near2 = 2;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 = ddouble.BitDecrement(near2);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -100);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -50);
            }

            ddouble near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                Assert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 -= ddouble.Ldexp(1, -100);
            }

            near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                Assert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 += ddouble.Ldexp(1, -100);
            }
        }

        [TestMethod]
        public void Log10Test() {
            Assert.AreEqual(0, (double)(ddouble.Log10(10) - 1), 1e-32);

            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.AreEqual(0, ddouble.Log10(1));
            Assert.AreEqual(1, ddouble.Log10(10));
            Assert.AreEqual(2, ddouble.Log10(100));
            Assert.AreEqual(3, ddouble.Log10(1000));
            Assert.AreEqual(4, ddouble.Log10(10000));
            Assert.AreEqual(8, ddouble.Log10(100000000));

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log10_pzero = ddouble.Log10(0d);
            ddouble log10_mzero = ddouble.Log10(-0d);
            ddouble log10_pinf = ddouble.Log10(double.PositiveInfinity);
            ddouble log10_ninf = ddouble.Log10(double.NegativeInfinity);
            ddouble log10_nan = ddouble.Log10(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log10_pzero), nameof(log10_pzero));
            Assert.IsTrue(ddouble.IsNaN(log10_mzero), nameof(log10_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log10_pinf), nameof(log10_pinf));
            Assert.IsTrue(ddouble.IsNaN(log10_ninf), nameof(log10_ninf));
            Assert.IsTrue(ddouble.IsNaN(log10_nan), nameof(log10_nan));
        }

        [TestMethod]
        public void LogTest() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log_pzero = ddouble.Log(0d);
            ddouble log_mzero = ddouble.Log(-0d);
            ddouble log_pinf = ddouble.Log(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Log(double.NegativeInfinity);
            ddouble log_nan = ddouble.Log(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log_pzero), nameof(log_pzero));
            Assert.IsTrue(ddouble.IsNaN(log_mzero), nameof(log_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log_pinf), nameof(log_pinf));
            Assert.IsTrue(ddouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(ddouble.IsNaN(log_nan), nameof(log_nan));
        }

        [TestMethod]
        public void Log1pTest() {
            for (decimal d = -0.99m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log1p(v);

                Assert.AreEqual(Math.Log(1 + (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.AreEqual(0, (double)ddouble.Log1p(ddouble.BitDecrement(0)), 1e-300);
            Assert.AreEqual(0, (double)ddouble.Log1p(ddouble.BitIncrement(0)), 1e-300);

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(0)));
            Console.WriteLine(ddouble.Log1p(0));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(0)));

            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitDecrement(-0.0625d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(-0.0625d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitIncrement(-0.0625d))).mantissa);

            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitDecrement(0.0625d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(0.0625d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitIncrement(0.0625d))).mantissa);

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(-0.0625d)));
            Console.WriteLine(ddouble.Log1p(-0.0625d));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(-0.0625d)));

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(0.0625d)));
            Console.WriteLine(ddouble.Log1p(0.0625d));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(0.0625d)));

            ddouble log_pzero = ddouble.Log1p(0d);
            ddouble log_mzero = ddouble.Log1p(-0d);
            ddouble log_pinf = ddouble.Log(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Log(double.NegativeInfinity);
            ddouble log_nan = ddouble.Log(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(log_pzero), nameof(log_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(log_mzero), nameof(log_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log_pinf), nameof(log_pinf));
            Assert.IsTrue(ddouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(ddouble.IsNaN(log_nan), nameof(log_nan));
        }
    }
}
