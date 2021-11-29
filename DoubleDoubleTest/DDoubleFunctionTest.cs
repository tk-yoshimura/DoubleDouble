using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleFunctionTest {
        [TestMethod]
        public void RcpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Rcp(v) * v - 1;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 1e-30, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Rcp(v) * (ddouble)d - 1;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 1e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble rcp_pzero = ddouble.Rcp(0d);
            ddouble rcp_mzero = ddouble.Rcp(-0d);
            ddouble rcp_pinf = ddouble.Rcp(double.PositiveInfinity);
            ddouble rcp_ninf = ddouble.Rcp(double.NegativeInfinity);
            ddouble rcp_nan = ddouble.Rcp(double.NaN);
            ddouble rcp_pval = ddouble.Rcp(double.MaxValue);
            ddouble rcp_mval = ddouble.Rcp(double.MinValue);
            ddouble rcp_peps = ddouble.Rcp(double.Epsilon);
            ddouble rcp_meps = ddouble.Rcp(-double.Epsilon);

            Assert.IsTrue(ddouble.IsPositiveInfinity(rcp_pzero), nameof(rcp_pzero));
            Assert.IsTrue(ddouble.IsNegativeInfinity(rcp_mzero), nameof(rcp_mzero));
            Assert.IsTrue(ddouble.IsPlusZero(rcp_pinf), nameof(rcp_pinf));
            Assert.IsTrue(ddouble.IsMinusZero(rcp_ninf), nameof(rcp_ninf));
            Assert.IsTrue(ddouble.IsNaN(rcp_nan), nameof(rcp_nan));
            Assert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            Assert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(ddouble.IsPositiveInfinity(rcp_peps), nameof(rcp_peps));
            Assert.IsTrue(ddouble.IsNegativeInfinity(rcp_meps), nameof(rcp_meps));
        }

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

        [TestMethod]
        public void AbsTest() {
            foreach (ddouble v in new ddouble[] { 1, 2,
                ddouble.Rcp(3), ddouble.Rcp(5), ddouble.Rcp(7), ddouble.Rcp(9), ddouble.Rcp(11) }) {

                ddouble v_dec = ddouble.BitDecrement(v), v_inc = ddouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(ddouble.Abs(v_dec) < ddouble.Abs(v) && ddouble.Abs(v) < ddouble.Abs(v_inc));
                Assert.IsTrue(ddouble.Abs(-v_dec) < ddouble.Abs(-v) && ddouble.Abs(-v) < ddouble.Abs(-v_inc));

                Assert.AreEqual(ddouble.Abs(v_dec), ddouble.Abs(-v_dec));
                Assert.AreEqual(ddouble.Abs(v), ddouble.Abs(-v));
                Assert.AreEqual(ddouble.Abs(v_inc), ddouble.Abs(-v_inc));
            }

            foreach (ddouble v in new ddouble[] { -1, -2,
                -ddouble.Rcp(3), -ddouble.Rcp(5), -ddouble.Rcp(7), -ddouble.Rcp(9), -ddouble.Rcp(11) }) {

                ddouble v_dec = ddouble.BitDecrement(v), v_inc = ddouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(ddouble.Abs(v_dec) > ddouble.Abs(v) && ddouble.Abs(v) > ddouble.Abs(v_inc));
                Assert.IsTrue(ddouble.Abs(-v_dec) > ddouble.Abs(-v) && ddouble.Abs(-v) > ddouble.Abs(-v_inc));

                Assert.AreEqual(ddouble.Abs(v_dec), ddouble.Abs(-v_dec));
                Assert.AreEqual(ddouble.Abs(v), ddouble.Abs(-v));
                Assert.AreEqual(ddouble.Abs(v_inc), ddouble.Abs(-v_inc));
            }

            Assert.AreEqual(ddouble.Abs(ddouble.BitDecrement(0)), ddouble.Abs(ddouble.BitIncrement(0)));
        }

        [TestMethod]
        public void FloorTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Floor(0));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Floor(1));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(1), ddouble.Floor(1 + ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitDecrement(1 + ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitIncrement(1 + ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("1e20"), ddouble.Floor("1e20"));
            Assert.AreEqual((ddouble)("0.99999999999999999999e20"), ddouble.Floor(ddouble.BitDecrement("1e20")));
            Assert.AreEqual((ddouble)("1e20"), ddouble.Floor(ddouble.BitIncrement("1e20")));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-1));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-2), ddouble.Floor(-1 - ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitDecrement(-1 - ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitIncrement(-1 - ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("-1e20"), ddouble.Floor("-1e20"));
            Assert.AreEqual((ddouble)("-1.00000000000000000001e20"), ddouble.Floor(ddouble.BitDecrement("-1e20")));
            Assert.AreEqual((ddouble)("-1e20"), ddouble.Floor(ddouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void CeilingTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(0));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(1));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(1 + ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitDecrement(1 + ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitIncrement(1 + ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("1e20"), ddouble.Ceiling("1e20"));
            Assert.AreEqual((ddouble)("1e20"), ddouble.Ceiling(ddouble.BitDecrement("1e20")));
            Assert.AreEqual((ddouble)("1.00000000000000000001e20"), ddouble.Ceiling(ddouble.BitIncrement("1e20")));

            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(-1 - ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitDecrement(-1 - ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitIncrement(-1 - ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("-1e20"), ddouble.Ceiling("-1e20"));
            Assert.AreEqual((ddouble)("-1e20"), ddouble.Ceiling(ddouble.BitDecrement("-1e20")));
            Assert.AreEqual((ddouble)("-0.99999999999999999999e20"), ddouble.Ceiling(ddouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void RoundTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Round(0));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Round(1));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Round(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(0));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Truncate(1));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Truncate(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Truncate(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Truncate(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateMantissaTest() {
            foreach (ddouble v in new ddouble[] {
                -1, -ddouble.Rcp(3), -ddouble.Rcp(7), -ddouble.BitDecrement(2),
                1, ddouble.Rcp(3), ddouble.Rcp(7), ddouble.BitDecrement(2) }) {

                for (int keep_bits = 1; keep_bits <= 110; keep_bits++) {
                    ddouble v_round = ddouble.TruncateMantissa(v, keep_bits);

                    Console.WriteLine(v_round);
                    Console.WriteLine($"0x{FloatSplitter.Split(v_round).mantissa:X14}");
                    Console.WriteLine(v_round - v);
                }
            }
        }

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

            Assert.IsTrue(ddouble.Abs(10 - ddouble.Pow10(1)) < 1e-31);

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

            Assert.AreEqual(0, (double)ddouble.Expm1(ddouble.BitDecrement(0)), 1e-300);
            Assert.AreEqual(0, (double)ddouble.Expm1(ddouble.BitIncrement(0)), 1e-300);

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

        [TestMethod]
        public void SinPIHalfTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPIHalf(v);

                Assert.AreEqual(Math.Sin((double)d * Math.PI / 2), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (int n = -8; n <= 8; n++) {
                ddouble nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPIHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    Assert.AreEqual(Math.Sin(n * Math.PI / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitDecrement(nearn);
                }

                nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPIHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    Assert.AreEqual(Math.Sin(n * Math.PI / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitIncrement(nearn);
                }
            }

            Assert.IsTrue(ddouble.Abs((ddouble)"1.56434465040230869010105319467166892e-1" - ddouble.SinPIHalf(ddouble.Rcp(10))) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"9.87688340595137726190040247693437261e-1" - ddouble.SinPIHalf(9 * ddouble.Rcp(10))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(0.5d - ddouble.SinPIHalf(ddouble.Rcp(3))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(ddouble.Sqrt(3) / 2 - ddouble.SinPIHalf(2 * ddouble.Rcp(3))) < 1e-31);

            ddouble sin_pzero = ddouble.SinPIHalf(0d);
            ddouble sin_mzero = ddouble.SinPIHalf(-0d);
            ddouble sin_pinf = ddouble.SinPIHalf(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPIHalf(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPIHalf(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void SinPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPI(v);

                Assert.AreEqual(Math.Sin((double)d * Math.PI), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.SinPI(0d);
            ddouble sin_mzero = ddouble.SinPI(-0d);
            ddouble sin_pinf = ddouble.SinPI(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPI(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPI(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void CosPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.CosPI(v);

                Assert.AreEqual(Math.Cos((double)d * Math.PI), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.CosPI(0d);
            ddouble cos_mzero = ddouble.CosPI(-0d);
            ddouble cos_pinf = ddouble.CosPI(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.CosPI(double.NegativeInfinity);
            ddouble cos_nan = ddouble.CosPI(double.NaN);

            Assert.IsTrue(cos_pzero == 1, nameof(cos_pzero));
            Assert.IsTrue(cos_mzero == 1, nameof(cos_mzero));
            Assert.IsTrue(ddouble.IsNaN(cos_pinf), nameof(cos_pinf));
            Assert.IsTrue(ddouble.IsNaN(cos_ninf), nameof(cos_ninf));
            Assert.IsTrue(ddouble.IsNaN(cos_nan), nameof(cos_nan));
        }

        [TestMethod]
        public void TanPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (Math.Abs(d) % 1m == 0.5m) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.TanPI(v);

                double w = Math.Tan((double)d * Math.PI);

                Assert.AreEqual(w, (double)u, Math.Max(1e-14, Math.Abs(w) * 1e-13), d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.TanPI(0d);
            ddouble tan_mzero = ddouble.TanPI(-0d);
            ddouble tan_pinf = ddouble.TanPI(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.TanPI(double.NegativeInfinity);
            ddouble tan_nan = ddouble.TanPI(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tan_pzero), nameof(tan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tan_mzero), nameof(tan_mzero));
            Assert.IsTrue(ddouble.IsNaN(tan_pinf), nameof(tan_pinf));
            Assert.IsTrue(ddouble.IsNaN(tan_ninf), nameof(tan_ninf));
            Assert.IsTrue(ddouble.IsNaN(tan_nan), nameof(tan_nan));
        }

        [TestMethod]
        public void SinTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sin(v);

                Assert.AreEqual(Math.Sin((double)d), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.Sin(0d);
            ddouble sin_mzero = ddouble.Sin(-0d);
            ddouble sin_pinf = ddouble.Sin(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.Sin(double.NegativeInfinity);
            ddouble sin_nan = ddouble.Sin(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void CosTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cos(v);

                Assert.AreEqual(Math.Cos((double)d), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.Cos(0d);
            ddouble cos_mzero = ddouble.Cos(-0d);
            ddouble cos_pinf = ddouble.Cos(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.Cos(double.NegativeInfinity);
            ddouble cos_nan = ddouble.Cos(double.NaN);

            Assert.IsTrue(cos_pzero == 1, nameof(cos_pzero));
            Assert.IsTrue(cos_mzero == 1, nameof(cos_mzero));
            Assert.IsTrue(ddouble.IsNaN(cos_pinf), nameof(cos_pinf));
            Assert.IsTrue(ddouble.IsNaN(cos_ninf), nameof(cos_ninf));
            Assert.IsTrue(ddouble.IsNaN(cos_nan), nameof(cos_nan));
        }

        [TestMethod]
        public void TanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tan(v);

                double w = Math.Tan((double)d);

                Assert.AreEqual(w, (double)u, Math.Max(1e-14, Math.Abs(w) * 1e-13), d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.Tan(0d);
            ddouble tan_mzero = ddouble.Tan(-0d);
            ddouble tan_pinf = ddouble.Tan(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.Tan(double.NegativeInfinity);
            ddouble tan_nan = ddouble.Tan(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tan_pzero), nameof(tan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tan_mzero), nameof(tan_mzero));
            Assert.IsTrue(ddouble.IsNaN(tan_pinf), nameof(tan_pinf));
            Assert.IsTrue(ddouble.IsNaN(tan_ninf), nameof(tan_ninf));
            Assert.IsTrue(ddouble.IsNaN(tan_nan), nameof(tan_nan));
        }

        [TestMethod]
        public void AsinTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Asin(v);

                Assert.AreEqual(Math.Asin((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.PI / 6 - ddouble.Asin(ddouble.Rcp(2))) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.11976951499863418668667705584539962" - ddouble.Asin((ddouble)9 / 10)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.42925685347046940048553233466472443" - ddouble.Asin((ddouble)99 / 100)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.52607123962616318798162545896820037" - ddouble.Asin((ddouble)999 / 1000)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.55665407331738374163508146582209533" - ddouble.Asin((ddouble)9999 / 10000)) < 1e-30);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.56632418711310869205898202533489875" - ddouble.Asin((ddouble)99999 / 100000)) < 1e-30);

            ddouble asin_pzero = ddouble.Asin(0d);
            ddouble asin_mzero = ddouble.Asin(-0d);
            ddouble asin_pinf = ddouble.Asin(double.PositiveInfinity);
            ddouble asin_ninf = ddouble.Asin(double.NegativeInfinity);
            ddouble asin_pout = ddouble.Asin(ddouble.BitIncrement(1));
            ddouble asin_nout = ddouble.Asin(ddouble.BitDecrement(-1));
            ddouble asin_nan = ddouble.Asin(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(asin_pzero), nameof(asin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(asin_mzero), nameof(asin_mzero));
            Assert.IsTrue(ddouble.IsNaN(asin_pinf), nameof(asin_pinf));
            Assert.IsTrue(ddouble.IsNaN(asin_ninf), nameof(asin_ninf));
            Assert.IsTrue(ddouble.IsNaN(asin_pout), nameof(asin_pout));
            Assert.IsTrue(ddouble.IsNaN(asin_nout), nameof(asin_nout));
            Assert.IsTrue(ddouble.IsNaN(asin_nan), nameof(asin_nan));
        }

        [TestMethod]
        public void AcosTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Acos(v);

                Assert.AreEqual(Math.Acos((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble acos_pinf = ddouble.Acos(double.PositiveInfinity);
            ddouble acos_ninf = ddouble.Acos(double.NegativeInfinity);
            ddouble acos_pout = ddouble.Acos(ddouble.BitIncrement(1));
            ddouble acos_nout = ddouble.Acos(ddouble.BitDecrement(-1));
            ddouble acos_nan = ddouble.Acos(double.NaN);

            Assert.IsTrue(ddouble.IsNaN(acos_pinf), nameof(acos_pinf));
            Assert.IsTrue(ddouble.IsNaN(acos_ninf), nameof(acos_ninf));
            Assert.IsTrue(ddouble.IsNaN(acos_pout), nameof(acos_pout));
            Assert.IsTrue(ddouble.IsNaN(acos_nout), nameof(acos_nout));
            Assert.IsTrue(ddouble.IsNaN(acos_nan), nameof(acos_nan));
        }

        [TestMethod]
        public void AtanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Atan(v);

                Assert.AreEqual(Math.Atan((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.PI / 4 - ddouble.Atan(1)) < 1e-31);
            Assert.IsTrue(ddouble.Abs(-ddouble.PI / 4 - ddouble.Atan(-1)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.10714871779409050301706546017853704" - ddouble.Atan(2)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"-1.10714871779409050301706546017853704" - ddouble.Atan(-2)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.24904577239825442582991707728109012" - ddouble.Atan(3)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"-1.24904577239825442582991707728109012" - ddouble.Atan(-3)) < 1e-31);

            ddouble atan_pzero = ddouble.Atan(0d);
            ddouble atan_mzero = ddouble.Atan(-0d);
            ddouble atan_pinf = ddouble.Atan(double.PositiveInfinity);
            ddouble atan_ninf = ddouble.Atan(double.NegativeInfinity);
            ddouble atan_nan = ddouble.Atan(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(atan_pzero), nameof(atan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(atan_mzero), nameof(atan_mzero));
            Assert.IsTrue(atan_pinf == ddouble.PI / 2, nameof(atan_pinf));
            Assert.IsTrue(atan_ninf == -ddouble.PI / 2, nameof(atan_ninf));
            Assert.IsTrue(ddouble.IsNaN(atan_nan), nameof(atan_nan));
        }

        [TestMethod]
        public void Atan2Test() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = -10m; x <= +10m; x += 0.1m) {
                    if (x == 0m && y == 0m) {
                        continue;
                    }

                    ddouble u = ddouble.Atan2((ddouble)y, (ddouble)x);
                    double v = Math.Atan2((double)y, (double)x);

                    if (u == ddouble.PI) {
                        Assert.IsTrue(Math.Abs(v) == Math.PI);
                    }
                    else {
                        Assert.AreEqual(v, (double)u, 1e-15, $"{y}, {x}");
                    }
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }
        }

        [TestMethod]
        public void SinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sinh(v);

                Assert.AreEqual(Math.Sinh((double)d), (double)u, Math.Abs(Math.Sinh((double)d)) * 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sinh_pzero = ddouble.Sinh(0d);
            ddouble sinh_mzero = ddouble.Sinh(-0d);
            ddouble sinh_pinf = ddouble.Sinh(double.PositiveInfinity);
            ddouble sinh_ninf = ddouble.Sinh(double.NegativeInfinity);
            ddouble sinh_nan = ddouble.Sinh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sinh_pzero), nameof(sinh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sinh_mzero), nameof(sinh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(sinh_pinf), nameof(sinh_pinf));
            Assert.IsTrue(ddouble.IsNegativeInfinity(sinh_ninf), nameof(sinh_ninf));
            Assert.IsTrue(ddouble.IsNaN(sinh_nan), nameof(sinh_nan));
        }

        [TestMethod]
        public void CoshTest() {
            Console.WriteLine($"{Math.Cosh(9.04):F16}");
            Console.WriteLine($"{ddouble.Cosh((ddouble)9.04m)}");
            Console.WriteLine("4.21688858728823488742026530603182233");

            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cosh(v);

                Assert.AreEqual(Math.Cosh((double)d), (double)u, Math.Abs(Math.Cosh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cosh_pzero = ddouble.Cosh(0d);
            ddouble cosh_mzero = ddouble.Cosh(-0d);
            ddouble cosh_pinf = ddouble.Cosh(double.PositiveInfinity);
            ddouble cosh_ninf = ddouble.Cosh(double.NegativeInfinity);
            ddouble cosh_nan = ddouble.Cosh(double.NaN);

            Assert.IsTrue(cosh_pzero == 1, nameof(cosh_pzero));
            Assert.IsTrue(cosh_mzero == 1, nameof(cosh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(cosh_pinf), nameof(cosh_pinf));
            Assert.IsTrue(ddouble.IsPositiveInfinity(cosh_ninf), nameof(cosh_ninf));
            Assert.IsTrue(ddouble.IsNaN(cosh_nan), nameof(cosh_nan));
        }

        [TestMethod]
        public void TanhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tanh(v);

                Assert.AreEqual(Math.Tanh((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tanh_pzero = ddouble.Tanh(0d);
            ddouble tanh_mzero = ddouble.Tanh(-0d);
            ddouble tanh_pinf = ddouble.Tanh(double.PositiveInfinity);
            ddouble tanh_ninf = ddouble.Tanh(double.NegativeInfinity);
            ddouble tanh_nan = ddouble.Tanh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tanh_pzero), nameof(tanh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tanh_mzero), nameof(tanh_mzero));
            Assert.IsTrue(tanh_pinf == 1d, nameof(tanh_pinf));
            Assert.IsTrue(tanh_ninf == -1d, nameof(tanh_ninf));
            Assert.IsTrue(ddouble.IsNaN(tanh_nan), nameof(tanh_nan));
        }

        [TestMethod]
        public void ArsinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Arsinh(v);

                Assert.AreEqual(Math.Asinh((double)d), (double)u, Math.Abs(Math.Asinh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arsinh_pzero = ddouble.Arsinh(0d);
            ddouble arsinh_mzero = ddouble.Arsinh(-0d);
            ddouble arsinh_pinf = ddouble.Arsinh(double.PositiveInfinity);
            ddouble arsinh_ninf = ddouble.Arsinh(double.NegativeInfinity);
            ddouble arsinh_nan = ddouble.Arsinh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(arsinh_pzero), nameof(arsinh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(arsinh_mzero), nameof(arsinh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(arsinh_pinf), nameof(arsinh_pinf));
            Assert.IsTrue(ddouble.IsNegativeInfinity(arsinh_ninf), nameof(arsinh_ninf));
            Assert.IsTrue(ddouble.IsNaN(arsinh_nan), nameof(arsinh_nan));
        }

        [TestMethod]
        public void ArcoshTest() {
            for (decimal d = 1m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Arcosh(v);

                Assert.AreEqual(Math.Acosh((double)d), (double)u, Math.Abs(Math.Acosh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arcosh_pzero = ddouble.Arcosh(0d);
            ddouble arcosh_mzero = ddouble.Arcosh(-0d);
            ddouble arcosh_pinf = ddouble.Arcosh(double.PositiveInfinity);
            ddouble arcosh_ninf = ddouble.Arcosh(double.NegativeInfinity);
            ddouble arcosh_nan = ddouble.Arcosh(double.NaN);

            Assert.IsTrue(ddouble.IsNaN(arcosh_pzero), nameof(arcosh_pzero));
            Assert.IsTrue(ddouble.IsNaN(arcosh_mzero), nameof(arcosh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(arcosh_pinf), nameof(arcosh_pinf));
            Assert.IsTrue(ddouble.IsNaN(arcosh_ninf), nameof(arcosh_ninf));
            Assert.IsTrue(ddouble.IsNaN(arcosh_nan), nameof(arcosh_nan));
        }

        [TestMethod]
        public void ArtanhTest() {
            for (decimal d = -0.99m; d <= +0.99m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Artanh(v);

                Assert.AreEqual(Math.Atanh((double)d), (double)u, Math.Abs(Math.Atanh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble artanh_pzero = ddouble.Artanh(0d);
            ddouble artanh_mzero = ddouble.Artanh(-0d);
            ddouble artanh_pone = ddouble.Artanh(1d);
            ddouble artanh_mone = ddouble.Artanh(-1d);
            ddouble artanh_pinf = ddouble.Artanh(double.PositiveInfinity);
            ddouble artanh_ninf = ddouble.Artanh(double.NegativeInfinity);
            ddouble artanh_nan = ddouble.Artanh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(artanh_pzero), nameof(artanh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(artanh_mzero), nameof(artanh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(artanh_pone), nameof(artanh_pone));
            Assert.IsTrue(ddouble.IsNegativeInfinity(artanh_mone), nameof(artanh_mone));
            Assert.IsTrue(ddouble.IsNaN(artanh_pinf), nameof(artanh_pinf));
            Assert.IsTrue(ddouble.IsNaN(artanh_ninf), nameof(artanh_ninf));
            Assert.IsTrue(ddouble.IsNaN(artanh_nan), nameof(artanh_nan));
        }

        [TestMethod]
        public void GammaTest() {
            for (BigInteger i = 1, y = 1; i <= 35; i++, y = y * (i - 1)) {
                ddouble x = ddouble.Gamma(i);

                Assert.AreEqual(y, x);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 30; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.Gamma((2 * (int)i - 1) * 0.5d);
                ddouble v = sqrtpi * z / y;

                DDoubleAssert.NeighborBits(v, x, 128);
            }

            DDoubleAssert.NeighborBits(sqrtpi * 4 / 3, ddouble.Gamma(-1.5), 128);
            DDoubleAssert.NeighborBits(sqrtpi * -2, ddouble.Gamma(-0.5), 128);

            DDoubleAssert.NeighborBits("1.2254167024651776451290983033628905268512", ddouble.Gamma(0.75), 128);
            DDoubleAssert.NeighborBits("9.3326215443944152681699238856266700490716e155", ddouble.Gamma(100), 128);
            DDoubleAssert.NeighborBits("2.9467022724950383265043395073512148621950e282", ddouble.Gamma(160), 160);

            ddouble gamma_pzero = ddouble.Gamma(0d);
            ddouble gamma_mzero = ddouble.Gamma(-0d);
            ddouble gamma_mone = ddouble.Gamma(-1d);
            ddouble gamma_pinf = ddouble.Gamma(double.PositiveInfinity);
            ddouble gamma_ninf = ddouble.Gamma(double.NegativeInfinity);
            ddouble gamma_nan = ddouble.Gamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_pzero), nameof(gamma_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_mzero), nameof(gamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(gamma_mone), nameof(gamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_pinf), nameof(gamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(gamma_ninf), nameof(gamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(gamma_nan), nameof(gamma_nan));
        }

        [TestMethod]
        public void LogGammaTest() {
            for (BigInteger i = 1, y = 1; i <= 35; i++, y = y * (i - 1)) {
                ddouble x = ddouble.LogGamma(i);
                ddouble v = ddouble.Log(y);

                DDoubleAssert.NeighborBits(v, x, 128);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 30; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.LogGamma((2 * (int)i - 1) * 0.5d);
                ddouble v = ddouble.Log(sqrtpi * z / y);

                DDoubleAssert.NeighborBits(v, x, 128);
            }

            DDoubleAssert.NeighborBits("1.288022524698077457370610440219717295925", ddouble.LogGamma(0.25), 128);
            DDoubleAssert.NeighborBits("2.032809514312953714814329718624296997597e-1", ddouble.LogGamma(0.75), 128);
            DDoubleAssert.NeighborBits("3.591342053695753987760440104602869096126e2", ddouble.LogGamma(100), 128);
            DDoubleAssert.NeighborBits("8.579336698258574368182534016573082801626e2", ddouble.LogGamma(200), 160);

            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (0.999755859375d, "1.409708218759223705137152436852900411083e-4"),
                (1.000244140625d, "-1.408727761632663509703058417841195184579e-4"),
                (1.999755859375d, "-1.0319961029799208618175017469614360195e-4"),
                (2.000244140625d, "1.032380513640963581901732440393341810164e-4"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x), x_dec2 = ddouble.BitDecrement(x_dec), x_dec3 = ddouble.BitDecrement(x_dec2);
                ddouble x_inc = ddouble.BitIncrement(x), x_inc2 = ddouble.BitIncrement(x_inc), x_inc3 = ddouble.BitIncrement(x_inc2);

                Console.WriteLine(ddouble.LogGamma(x_dec3));
                Console.WriteLine(ddouble.LogGamma(x_dec2));
                Console.WriteLine(ddouble.LogGamma(x_dec));
                Console.WriteLine(ddouble.LogGamma(x));
                Console.WriteLine(ddouble.LogGamma(x_inc));
                Console.WriteLine(ddouble.LogGamma(x_inc2));
                Console.WriteLine(ddouble.LogGamma(x_inc3));

                DDoubleAssert.AreEqual(expected, ddouble.LogGamma(x_dec), 1e-29);
                DDoubleAssert.AreEqual(expected, ddouble.LogGamma(x), 1e-29);
                DDoubleAssert.AreEqual(expected, ddouble.LogGamma(x_inc), 1e-29);
            }

            ddouble loggamma_pzero = ddouble.LogGamma(0d);
            ddouble loggamma_mzero = ddouble.LogGamma(-0d);
            ddouble loggamma_mone = ddouble.LogGamma(-1d);
            ddouble loggamma_pinf = ddouble.LogGamma(double.PositiveInfinity);
            ddouble loggamma_ninf = ddouble.LogGamma(double.NegativeInfinity);
            ddouble loggamma_nan = ddouble.LogGamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(loggamma_pzero), nameof(loggamma_pzero));
            Assert.IsTrue(ddouble.IsNaN(loggamma_mzero), nameof(loggamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(loggamma_mone), nameof(loggamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(loggamma_pinf), nameof(loggamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(loggamma_ninf), nameof(loggamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(loggamma_nan), nameof(loggamma_nan));
        }

        [TestMethod]
        public void DigammaTest() {
            for (int i = 1; i <= 35; i++) {
                ddouble x = ddouble.Digamma(i);
                ddouble y = ddouble.HarmonicNumber(i - 1) - ddouble.EulerGamma;

                DDoubleAssert.NeighborBits(y, x, 128);
            }

            DDoubleAssert.NeighborBits("-2.8941202000429320747561968127633502440339e0", ddouble.Digamma(-0.75), 128);
            DDoubleAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(-0.5), 128);
            DDoubleAssert.NeighborBits("2.9141391202135278303731132371828193068299e0", ddouble.Digamma(-0.25), 128);
            DDoubleAssert.NeighborBits("-4.2274535333762654080895301460966835773672e0", ddouble.Digamma(0.25), 128);
            DDoubleAssert.NeighborBits("-1.9635100260214234794409763329987555671932e0", ddouble.Digamma(0.5), 128);
            DDoubleAssert.NeighborBits("-1.0858608797864721696268867628171806931701e0", ddouble.Digamma(0.75), 128);
            DDoubleAssert.NeighborBits("-2.2745353337626540808953014609668357736724e-1", ddouble.Digamma(1.25), 128);
            DDoubleAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(1.5), 128);
            DDoubleAssert.NeighborBits("2.4747245354686116370644657051615264016326e-1", ddouble.Digamma(1.75), 128);
            DDoubleAssert.NeighborBits("4.6001618527380874001986055855758507268668e0", ddouble.Digamma(100), 128);
            DDoubleAssert.NeighborBits("5.2958152832199116154508743070484592057952e0", ddouble.Digamma(200), 160);

            ddouble digamma_pzero = ddouble.Digamma(0d);
            ddouble digamma_mzero = ddouble.Digamma(-0d);
            ddouble digamma_mone = ddouble.Digamma(-1d);
            ddouble digamma_pinf = ddouble.Digamma(double.PositiveInfinity);
            ddouble digamma_ninf = ddouble.Digamma(double.NegativeInfinity);
            ddouble digamma_nan = ddouble.Digamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pzero), nameof(digamma_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_mzero), nameof(digamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(digamma_mone), nameof(digamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pinf), nameof(digamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(digamma_ninf), nameof(digamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(digamma_nan), nameof(digamma_nan));
        }
    }
}
