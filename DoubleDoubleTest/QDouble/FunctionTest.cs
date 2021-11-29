using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class FunctionTest {
        [TestMethod]
        public void RcpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Rcp(v) * v - 1;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 1e-60, $"{d}");
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Rcp(v) * (qdouble)d - 1;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 1e-64, $"{d}");
            }

            qdouble rcp_pzero = qdouble.Rcp(0d);
            qdouble rcp_mzero = qdouble.Rcp(-0d);
            qdouble rcp_pinf = qdouble.Rcp(ddouble.PositiveInfinity);
            qdouble rcp_ninf = qdouble.Rcp(ddouble.NegativeInfinity);
            qdouble rcp_nan = qdouble.Rcp(ddouble.NaN);
            qdouble rcp_pval = qdouble.Rcp(ddouble.MaxValue);
            qdouble rcp_mval = qdouble.Rcp(ddouble.MinValue);
            qdouble rcp_peps = qdouble.Rcp(ddouble.Epsilon);
            qdouble rcp_meps = qdouble.Rcp(-ddouble.Epsilon);

            Assert.IsTrue(qdouble.IsPositiveInfinity(rcp_pzero), nameof(rcp_pzero));
            Assert.IsTrue(qdouble.IsNegativeInfinity(rcp_mzero), nameof(rcp_mzero));
            Assert.IsTrue(qdouble.IsPlusZero(rcp_pinf), nameof(rcp_pinf));
            Assert.IsTrue(qdouble.IsMinusZero(rcp_ninf), nameof(rcp_ninf));
            Assert.IsTrue(qdouble.IsNaN(rcp_nan), nameof(rcp_nan));
            HPAssert.AreEqual(1 / ddouble.MaxValue, (ddouble)rcp_pval, ddouble.Epsilon, nameof(rcp_pval));
            HPAssert.AreEqual(1 / ddouble.MinValue, (ddouble)rcp_mval, ddouble.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(qdouble.IsPositiveInfinity(rcp_peps), nameof(rcp_peps));
            Assert.IsTrue(qdouble.IsNegativeInfinity(rcp_meps), nameof(rcp_meps));
        }

        [TestMethod]
        public void Log2Test() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log2(v);

                HPAssert.AreEqual(ddouble.Log2((ddouble)d), (ddouble)u, 1e-25);
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log2(v);

                HPAssert.AreEqual(ddouble.Log2((ddouble)d), (ddouble)u, 1e-25);
            }

            qdouble log2_pzero = qdouble.Log2(0d);
            qdouble log2_mzero = qdouble.Log2(-0d);
            qdouble log2_pinf = qdouble.Log2(ddouble.PositiveInfinity);
            qdouble log2_ninf = qdouble.Log2(ddouble.NegativeInfinity);
            qdouble log2_nan = qdouble.Log2(ddouble.NaN);

            Assert.IsTrue(qdouble.IsNegativeInfinity(log2_pzero), nameof(log2_pzero));
            Assert.IsTrue(qdouble.IsNaN(log2_mzero), nameof(log2_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(log2_pinf), nameof(log2_pinf));
            Assert.IsTrue(qdouble.IsNaN(log2_ninf), nameof(log2_ninf));
            Assert.IsTrue(qdouble.IsNaN(log2_nan), nameof(log2_nan));

            qdouble near2 = 2;
            for (int i = 0; i < 256; i++) {
                qdouble u = qdouble.Log2(near2);
                HPAssert.AreEqual(ddouble.Log2(2), (ddouble)u, 1e-25);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 = qdouble.BitDecrement(near2);
            }
            for (int i = 0; i < 256; i++) {
                qdouble u = qdouble.Log2(near2);
                HPAssert.AreEqual(ddouble.Log2(2), (ddouble)u, 1e-25);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= qdouble.Ldexp(1, -100);
            }
            for (int i = 0; i < 256; i++) {
                qdouble u = qdouble.Log2(near2);
                HPAssert.AreEqual(ddouble.Log2(2), (ddouble)u, 1e-25);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= qdouble.Ldexp(1, -50);
            }

            qdouble near1 = 1;
            for (int i = 0; i < 256; i++) {
                qdouble u = qdouble.Log2(near1);
                HPAssert.AreEqual(0, (ddouble)u, 1e-25);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 -= qdouble.Ldexp(1, -100);
            }

            near1 = 1;
            for (int i = 0; i < 256; i++) {
                qdouble u = qdouble.Log2(near1);
                HPAssert.AreEqual(0, (ddouble)u, 1e-25);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 += qdouble.Ldexp(1, -100);
            }
        }

        [TestMethod]
        public void Log10Test() {
            HPAssert.AreEqual(0, (ddouble)(qdouble.Log10(10) - 1), 1e-64);

            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log10(v);

                HPAssert.AreEqual(ddouble.Log10((ddouble)d), (ddouble)u, 1e-25);
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log10(v);

                HPAssert.AreEqual(ddouble.Log10((ddouble)d), (ddouble)u, 1e-25);
            }

            qdouble log10_pzero = qdouble.Log10(0d);
            qdouble log10_mzero = qdouble.Log10(-0d);
            qdouble log10_pinf = qdouble.Log10(ddouble.PositiveInfinity);
            qdouble log10_ninf = qdouble.Log10(ddouble.NegativeInfinity);
            qdouble log10_nan = qdouble.Log10(ddouble.NaN);

            Assert.IsTrue(qdouble.IsNegativeInfinity(log10_pzero), nameof(log10_pzero));
            Assert.IsTrue(qdouble.IsNaN(log10_mzero), nameof(log10_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(log10_pinf), nameof(log10_pinf));
            Assert.IsTrue(qdouble.IsNaN(log10_ninf), nameof(log10_ninf));
            Assert.IsTrue(qdouble.IsNaN(log10_nan), nameof(log10_nan));
        }

        [TestMethod]
        public void LogTest() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log(v);

                HPAssert.AreEqual(ddouble.Log((ddouble)d), (ddouble)u, 1e-25);
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Log(v);

                HPAssert.AreEqual(ddouble.Log((ddouble)d), (ddouble)u, 1e-25);
            }

            qdouble log_pzero = qdouble.Log(0d);
            qdouble log_mzero = qdouble.Log(-0d);
            qdouble log_pinf = qdouble.Log(ddouble.PositiveInfinity);
            qdouble log_ninf = qdouble.Log(ddouble.NegativeInfinity);
            qdouble log_nan = qdouble.Log(ddouble.NaN);

            Assert.IsTrue(qdouble.IsNegativeInfinity(log_pzero), nameof(log_pzero));
            Assert.IsTrue(qdouble.IsNaN(log_mzero), nameof(log_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(log_pinf), nameof(log_pinf));
            Assert.IsTrue(qdouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(qdouble.IsNaN(log_nan), nameof(log_nan));
        }

        [TestMethod]
        public void AbsTest() {
            foreach (qdouble v in new qdouble[] { 1, 2,
                qdouble.Rcp(3), qdouble.Rcp(5), qdouble.Rcp(7), qdouble.Rcp(9), qdouble.Rcp(11) }) {

                qdouble v_dec = qdouble.BitDecrement(v), v_inc = qdouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(qdouble.Abs(v_dec) < qdouble.Abs(v) && qdouble.Abs(v) < qdouble.Abs(v_inc));
                Assert.IsTrue(qdouble.Abs(-v_dec) < qdouble.Abs(-v) && qdouble.Abs(-v) < qdouble.Abs(-v_inc));

                Assert.AreEqual(qdouble.Abs(v_dec), qdouble.Abs(-v_dec));
                Assert.AreEqual(qdouble.Abs(v), qdouble.Abs(-v));
                Assert.AreEqual(qdouble.Abs(v_inc), qdouble.Abs(-v_inc));
            }

            foreach (qdouble v in new qdouble[] { -1, -2,
                -qdouble.Rcp(3), -qdouble.Rcp(5), -qdouble.Rcp(7), -qdouble.Rcp(9), -qdouble.Rcp(11) }) {

                qdouble v_dec = qdouble.BitDecrement(v), v_inc = qdouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(qdouble.Abs(v_dec) > qdouble.Abs(v) && qdouble.Abs(v) > qdouble.Abs(v_inc));
                Assert.IsTrue(qdouble.Abs(-v_dec) > qdouble.Abs(-v) && qdouble.Abs(-v) > qdouble.Abs(-v_inc));

                Assert.AreEqual(qdouble.Abs(v_dec), qdouble.Abs(-v_dec));
                Assert.AreEqual(qdouble.Abs(v), qdouble.Abs(-v));
                Assert.AreEqual(qdouble.Abs(v_inc), qdouble.Abs(-v_inc));
            }

            Assert.AreEqual(qdouble.Abs(qdouble.BitDecrement(0)), qdouble.Abs(qdouble.BitIncrement(0)));
        }

        [TestMethod]
        public void FloorTest() {
            Assert.AreEqual((qdouble)(0), qdouble.Floor(0));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitDecrement(0)));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitIncrement(0)));

            Assert.AreEqual((qdouble)(1), qdouble.Floor(1));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitDecrement(1)));
            Assert.AreEqual((qdouble)(1), qdouble.Floor(qdouble.BitIncrement(1)));

            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitDecrement(qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitIncrement(qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitDecrement(qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Floor(qdouble.BitIncrement(qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(1), qdouble.Floor(1 + qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(1), qdouble.Floor(qdouble.BitDecrement(1 + qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(1), qdouble.Floor(qdouble.BitIncrement(1 + qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)("1e20"), qdouble.Floor("1e20"));
            Assert.AreEqual((qdouble)("0.99999999999999999999e20"), qdouble.Floor(qdouble.BitDecrement("1e20")));
            Assert.AreEqual((qdouble)("1e20"), qdouble.Floor(qdouble.BitIncrement("1e20")));

            Assert.AreEqual((qdouble)(-1), qdouble.Floor(-1));
            Assert.AreEqual((qdouble)(-2), qdouble.Floor(qdouble.BitDecrement(-1)));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitIncrement(-1)));

            Assert.AreEqual((qdouble)(-1), qdouble.Floor(-qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitDecrement(-qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitIncrement(-qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(-1), qdouble.Floor(-qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitDecrement(-qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(-1), qdouble.Floor(qdouble.BitIncrement(-qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(-2), qdouble.Floor(-1 - qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(-2), qdouble.Floor(qdouble.BitDecrement(-1 - qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(-2), qdouble.Floor(qdouble.BitIncrement(-1 - qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)("-1e20"), qdouble.Floor("-1e20"));
            Assert.AreEqual((qdouble)("-1.00000000000000000001e20"), qdouble.Floor(qdouble.BitDecrement("-1e20")));
            Assert.AreEqual((qdouble)("-1e20"), qdouble.Floor(qdouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void CeilingTest() {
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(0));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitDecrement(0)));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitIncrement(0)));

            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(1));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitDecrement(1)));
            Assert.AreEqual((qdouble)(2), qdouble.Ceiling(qdouble.BitIncrement(1)));

            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitDecrement(qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitIncrement(qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitDecrement(qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(1), qdouble.Ceiling(qdouble.BitIncrement(qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(2), qdouble.Ceiling(1 + qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(2), qdouble.Ceiling(qdouble.BitDecrement(1 + qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(2), qdouble.Ceiling(qdouble.BitIncrement(1 + qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)("1e20"), qdouble.Ceiling("1e20"));
            Assert.AreEqual((qdouble)("1e20"), qdouble.Ceiling(qdouble.BitDecrement("1e20")));
            Assert.AreEqual((qdouble)("1.00000000000000000001e20"), qdouble.Ceiling(qdouble.BitIncrement("1e20")));

            Assert.AreEqual((qdouble)(-1), qdouble.Ceiling(-1));
            Assert.AreEqual((qdouble)(-1), qdouble.Ceiling(qdouble.BitDecrement(-1)));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitIncrement(-1)));

            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(-qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitDecrement(-qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitIncrement(-qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(-qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitDecrement(-qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Ceiling(qdouble.BitIncrement(-qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(-1), qdouble.Ceiling(-1 - qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(-1), qdouble.Ceiling(qdouble.BitDecrement(-1 - qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(-1), qdouble.Ceiling(qdouble.BitIncrement(-1 - qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)("-1e20"), qdouble.Ceiling("-1e20"));
            Assert.AreEqual((qdouble)("-1e20"), qdouble.Ceiling(qdouble.BitDecrement("-1e20")));
            Assert.AreEqual((qdouble)("-0.99999999999999999999e20"), qdouble.Ceiling(qdouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void RoundTest() {
            Assert.AreEqual((qdouble)(0), qdouble.Round(0));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(0)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(0)));

            Assert.AreEqual((qdouble)(1), qdouble.Round(1));
            Assert.AreEqual((qdouble)(1), qdouble.Round(qdouble.BitDecrement(1)));
            Assert.AreEqual((qdouble)(1), qdouble.Round(qdouble.BitIncrement(1)));

            Assert.AreEqual((qdouble)(1), qdouble.Round(qdouble.Rcp(2)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(qdouble.Rcp(2))));
            Assert.AreEqual((qdouble)(1), qdouble.Round(qdouble.BitIncrement(qdouble.Rcp(2))));

            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(-1), qdouble.Round(-1));
            Assert.AreEqual((qdouble)(-1), qdouble.Round(qdouble.BitDecrement(-1)));
            Assert.AreEqual((qdouble)(-1), qdouble.Round(qdouble.BitIncrement(-1)));

            Assert.AreEqual((qdouble)(0), qdouble.Round(-qdouble.Rcp(2)));
            Assert.AreEqual((qdouble)(-1), qdouble.Round(qdouble.BitDecrement(-qdouble.Rcp(2))));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(-qdouble.Rcp(2))));

            Assert.AreEqual((qdouble)(0), qdouble.Round(-qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(-qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(-qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Round(-qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitDecrement(-qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Round(qdouble.BitIncrement(-qdouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateTest() {
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(0));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(0)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(0)));

            Assert.AreEqual((qdouble)(1), qdouble.Truncate(1));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(1)));
            Assert.AreEqual((qdouble)(1), qdouble.Truncate(qdouble.BitIncrement(1)));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.Rcp(2)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(qdouble.Rcp(2))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(qdouble.Rcp(2))));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(qdouble.Rcp(7))));

            Assert.AreEqual((qdouble)(-1), qdouble.Truncate(-1));
            Assert.AreEqual((qdouble)(-1), qdouble.Truncate(qdouble.BitDecrement(-1)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(-1)));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(-qdouble.Rcp(2)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(-qdouble.Rcp(2))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(-qdouble.Rcp(2))));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(-qdouble.Rcp(3)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(-qdouble.Rcp(3))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(-qdouble.Rcp(3))));

            Assert.AreEqual((qdouble)(0), qdouble.Truncate(-qdouble.Rcp(7)));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitDecrement(-qdouble.Rcp(7))));
            Assert.AreEqual((qdouble)(0), qdouble.Truncate(qdouble.BitIncrement(-qdouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateMantissaTest() {
            foreach (qdouble v in new qdouble[] {
                -1, -qdouble.Rcp(3), -qdouble.Rcp(7), -qdouble.BitDecrement(2),
                1, qdouble.Rcp(3), qdouble.Rcp(7), qdouble.BitDecrement(2) }) {

                for (int keep_bits = 1; keep_bits <= 110; keep_bits++) {
                    qdouble v_round = qdouble.TruncateMantissa(v, keep_bits);

                    Console.WriteLine(v_round);
                    Console.WriteLine($"0x{FloatSplitter.Split(v_round).mantissa:X14}");
                    Console.WriteLine(v_round - v);
                }
            }
        }

        [TestMethod]
        public void SqrtTest() {
            for (decimal d = 0; d <= +10m; d += 0.01m) {
                qdouble v = (qdouble)d;
                qdouble w = qdouble.Sqrt(v);
                qdouble u = w * w - (qdouble)d;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 8e-60, $"{d}");
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                qdouble v = (qdouble)d;
                qdouble w = qdouble.Sqrt(v);
                qdouble u = w * w - (qdouble)d;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 8e-60, $"{d}");
            }

            qdouble sqrt_pzero = qdouble.Sqrt(0d);
            qdouble sqrt_mzero = qdouble.Sqrt(-0d);
            qdouble sqrt_pinf = qdouble.Sqrt(ddouble.PositiveInfinity);
            qdouble sqrt_ninf = qdouble.Sqrt(ddouble.NegativeInfinity);
            qdouble sqrt_nan = qdouble.Sqrt(ddouble.NaN);

            Assert.IsTrue(qdouble.IsPlusZero(sqrt_pzero), nameof(sqrt_pzero));
            Assert.IsTrue(qdouble.IsNaN(sqrt_mzero), nameof(sqrt_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(sqrt_pinf), nameof(sqrt_pinf));
            Assert.IsTrue(qdouble.IsNaN(sqrt_ninf), nameof(sqrt_ninf));
            Assert.IsTrue(qdouble.IsNaN(sqrt_nan), nameof(sqrt_nan));
        }

        [TestMethod]
        public void CbrtTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                qdouble v = (qdouble)d;
                qdouble w = qdouble.Cbrt(v);
                qdouble u = w * w * w - (qdouble)d;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 8e-60, $"{d}");
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                qdouble v = (qdouble)d;
                qdouble w = qdouble.Cbrt(v);
                qdouble u = w * w * w - (qdouble)d;

                HPAssert.AreEqual(0, (ddouble)u, ddouble.Abs((ddouble)d) * 8e-60, $"{d}");
            }

            qdouble cbrt_pzero = qdouble.Cbrt(0d);
            qdouble cbrt_mzero = qdouble.Cbrt(-0d);
            qdouble cbrt_pinf = qdouble.Cbrt(ddouble.PositiveInfinity);
            qdouble cbrt_ninf = qdouble.Cbrt(ddouble.NegativeInfinity);
            qdouble cbrt_nan = qdouble.Cbrt(ddouble.NaN);

            Assert.IsTrue(qdouble.IsPlusZero(cbrt_pzero), nameof(cbrt_pzero));
            Assert.IsTrue(qdouble.IsMinusZero(cbrt_mzero), nameof(cbrt_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(cbrt_pinf), nameof(cbrt_pinf));
            Assert.IsTrue(qdouble.IsNegativeInfinity(cbrt_ninf), nameof(cbrt_ninf));
            Assert.IsTrue(qdouble.IsNaN(cbrt_nan), nameof(cbrt_nan));
        }

        [TestMethod]
        public void PowNTest() {
            qdouble v = qdouble.Pow(5, 308);

            Assert.IsTrue(qdouble.Abs((qdouble)"1.917614634881924434803035919916513923037e215" - v) < "1e185");
        }

        [TestMethod]
        public void Pow2Test() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Pow2(v);

                HPAssert.AreEqual(ddouble.Pow(2, (ddouble)d), (ddouble)u, 1e-24);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Pow2(v);

                HPAssert.AreEqual(ddouble.Pow(2, (ddouble)d), (ddouble)u, 1e-24);
            }

            qdouble pow2_pzero = qdouble.Pow2(0d);
            qdouble pow2_mzero = qdouble.Pow2(-0d);
            qdouble pow2_pinf = qdouble.Pow2(ddouble.PositiveInfinity);
            qdouble pow2_ninf = qdouble.Pow2(ddouble.NegativeInfinity);
            qdouble pow2_nan = qdouble.Pow2(ddouble.NaN);

            Assert.IsTrue(pow2_pzero == 1, nameof(pow2_pzero));
            Assert.IsTrue(pow2_mzero == 1, nameof(pow2_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(pow2_pinf), nameof(pow2_pinf));
            Assert.IsTrue(qdouble.IsPlusZero(pow2_ninf), nameof(pow2_ninf));
            Assert.IsTrue(qdouble.IsNaN(pow2_nan), nameof(pow2_nan));
        }

        [TestMethod]
        public void PowTest() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = 0.1m; x <= +10m; x += 0.1m) {
                    if (y == 0) {
                        continue;
                    }

                    qdouble u = qdouble.Pow((qdouble)x, (qdouble)y);

                    HPAssert.AreEqual(ddouble.Pow((ddouble)x, (ddouble)y), (ddouble)u, (ddouble)u * 1e-24);
                }
            }
        }

        [TestMethod]
        public void Pow10Test() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Pow10(v);

                HPAssert.AreEqual(ddouble.Pow(10, (ddouble)d), (ddouble)u, (ddouble)u * 1e-24);
            }

            Assert.IsTrue(qdouble.Abs(10 - qdouble.Pow10(1)) < 1e-31);

            qdouble pow10_pzero = qdouble.Pow10(0d);
            qdouble pow10_mzero = qdouble.Pow10(-0d);
            qdouble pow10_pinf = qdouble.Pow10(ddouble.PositiveInfinity);
            qdouble pow10_ninf = qdouble.Pow10(ddouble.NegativeInfinity);
            qdouble pow10_nan = qdouble.Pow10(ddouble.NaN);

            Assert.IsTrue(pow10_pzero == 1, nameof(pow10_pzero));
            Assert.IsTrue(pow10_mzero == 1, nameof(pow10_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(pow10_pinf), nameof(pow10_pinf));
            Assert.IsTrue(qdouble.IsPlusZero(pow10_ninf), nameof(pow10_ninf));
            Assert.IsTrue(qdouble.IsNaN(pow10_nan), nameof(pow10_nan));
        }

        [TestMethod]
        public void ExpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                qdouble v = (qdouble)d;
                qdouble u = qdouble.Exp(v);

                HPAssert.AreEqual(ddouble.Exp((ddouble)d), (ddouble)u, (ddouble)u * 1e-24);
            }

            Assert.IsTrue(qdouble.Abs(qdouble.E - qdouble.Exp(1)) < 1e-31);

            qdouble exp_pzero = qdouble.Exp(0d);
            qdouble exp_mzero = qdouble.Exp(-0d);
            qdouble exp_pinf = qdouble.Exp(ddouble.PositiveInfinity);
            qdouble exp_ninf = qdouble.Exp(ddouble.NegativeInfinity);
            qdouble exp_nan = qdouble.Exp(ddouble.NaN);

            Assert.IsTrue(exp_pzero == 1, nameof(exp_pzero));
            Assert.IsTrue(exp_mzero == 1, nameof(exp_mzero));
            Assert.IsTrue(qdouble.IsPositiveInfinity(exp_pinf), nameof(exp_pinf));
            Assert.IsTrue(qdouble.IsPlusZero(exp_ninf), nameof(exp_ninf));
            Assert.IsTrue(qdouble.IsNaN(exp_nan), nameof(exp_nan));
        }
    }
}
