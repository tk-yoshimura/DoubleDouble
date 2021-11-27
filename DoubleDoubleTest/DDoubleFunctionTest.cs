using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleFunctionTest {
        [TestMethod]
        public void RcpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Rcp(v) * (double)d - 1;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Rcp(v) * (double)d - 1;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-35, $"{d}");
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

            Assert.IsTrue(ddouble.IsInfinity(rcp_pzero) && Math.CopySign(1, (double)(rcp_pzero)) > 0, nameof(rcp_pzero));
            Assert.IsTrue(ddouble.IsInfinity(rcp_mzero) && Math.CopySign(1, (double)(rcp_mzero)) < 0, nameof(rcp_mzero));
            Assert.IsTrue((double)rcp_pinf == 0 && Math.CopySign(1, (double)(rcp_pinf)) > 0, nameof(rcp_pinf));
            Assert.IsTrue((double)rcp_ninf == 0 && Math.CopySign(1, (double)(rcp_ninf)) < 0, nameof(rcp_ninf));
            Assert.IsTrue(ddouble.IsNaN(rcp_nan), nameof(rcp_nan));
            Assert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            Assert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(ddouble.IsInfinity(rcp_peps) && Math.CopySign(1, (double)(rcp_peps)) > 0, nameof(rcp_peps));
            Assert.IsTrue(ddouble.IsInfinity(rcp_meps) && Math.CopySign(1, (double)(rcp_meps)) < 0, nameof(rcp_meps));
        }

        [TestMethod]
        public void Log2Test() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log2_pzero = ddouble.Log2(0d);
            ddouble log2_mzero = ddouble.Log2(-0d);
            ddouble log2_pinf = ddouble.Log2(double.PositiveInfinity);
            ddouble log2_ninf = ddouble.Log2(double.NegativeInfinity);
            ddouble log2_nan = ddouble.Log2(double.NaN);

            Assert.IsTrue(ddouble.IsInfinity(log2_pzero) && Math.CopySign(1, (double)(log2_pzero)) < 0, nameof(log2_pzero));
            Assert.IsTrue(ddouble.IsNaN(log2_mzero), nameof(log2_mzero));
            Assert.IsTrue(ddouble.IsInfinity(log2_pinf) && Math.CopySign(1, (double)(log2_pinf)) > 0, nameof(log2_pinf));
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

                ddouble v = (double)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log10_pzero = ddouble.Log10(0d);
            ddouble log10_mzero = ddouble.Log10(-0d);
            ddouble log10_pinf = ddouble.Log10(double.PositiveInfinity);
            ddouble log10_ninf = ddouble.Log10(double.NegativeInfinity);
            ddouble log10_nan = ddouble.Log10(double.NaN);

            Assert.IsTrue(ddouble.IsInfinity(log10_pzero) && Math.CopySign(1, (double)(log10_pzero)) < 0, nameof(log10_pzero));
            Assert.IsTrue(ddouble.IsNaN(log10_mzero), nameof(log10_mzero));
            Assert.IsTrue(ddouble.IsInfinity(log10_pinf) && Math.CopySign(1, (double)(log10_pinf)) > 0, nameof(log10_pinf));
            Assert.IsTrue(ddouble.IsNaN(log10_ninf), nameof(log10_ninf));
            Assert.IsTrue(ddouble.IsNaN(log10_nan), nameof(log10_nan));
        }

        [TestMethod]
        public void LogTest() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble Log_pzero = ddouble.Log(0d);
            ddouble Log_mzero = ddouble.Log(-0d);
            ddouble Log_pinf = ddouble.Log(double.PositiveInfinity);
            ddouble Log_ninf = ddouble.Log(double.NegativeInfinity);
            ddouble Log_nan = ddouble.Log(double.NaN);

            Assert.IsTrue(ddouble.IsInfinity(Log_pzero) && Math.CopySign(1, (double)(Log_pzero)) < 0, nameof(Log_pzero));
            Assert.IsTrue(ddouble.IsNaN(Log_mzero), nameof(Log_mzero));
            Assert.IsTrue(ddouble.IsInfinity(Log_pinf) && Math.CopySign(1, (double)(Log_pinf)) > 0, nameof(Log_pinf));
            Assert.IsTrue(ddouble.IsNaN(Log_ninf), nameof(Log_ninf));
            Assert.IsTrue(ddouble.IsNaN(Log_nan), nameof(Log_nan));
        }

        [TestMethod]
        public void AbsTest() {
            foreach (ddouble v in new ddouble[] { 1, 2, ddouble.Rcp(3), ddouble.Rcp(5), ddouble.Rcp(7), ddouble.Rcp(9), ddouble.Rcp(11) }) {
                ddouble v_dec = ddouble.BitDecrement(v), v_inc = ddouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(ddouble.Abs(v_dec) < ddouble.Abs(v) && ddouble.Abs(v) < ddouble.Abs(v_inc));

                Assert.IsTrue(ddouble.Abs(-v_dec) > ddouble.Abs(-v) && ddouble.Abs(-v) > ddouble.Abs(-v_inc));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.BitDecrement(0)) == ddouble.Abs(ddouble.BitIncrement(0)));
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
        public void SqrtTest() {
            for (decimal d = 0; d <= +10m; d += 0.01m) {
                ddouble v = (double)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (double)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                ddouble v = (double)d;
                ddouble w = ddouble.Sqrt(v);
                ddouble u = w * w - (double)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sqrt_pzero = ddouble.Sqrt(0d);
            ddouble sqrt_mzero = ddouble.Sqrt(-0d);
            ddouble sqrt_pinf = ddouble.Sqrt(double.PositiveInfinity);
            ddouble sqrt_ninf = ddouble.Sqrt(double.NegativeInfinity);
            ddouble sqrt_nan = ddouble.Sqrt(double.NaN);

            Assert.IsTrue(ddouble.IsZero(sqrt_pzero), nameof(sqrt_pzero));
            Assert.IsTrue(ddouble.IsNaN(sqrt_mzero), nameof(sqrt_mzero));
            Assert.IsTrue(ddouble.IsInfinity(sqrt_pinf) && Math.CopySign(1, (double)(sqrt_pinf)) > 0, nameof(sqrt_pinf));
            Assert.IsTrue(ddouble.IsNaN(sqrt_ninf), nameof(sqrt_ninf));
            Assert.IsTrue(ddouble.IsNaN(sqrt_nan), nameof(sqrt_nan));
        }

        [TestMethod]
        public void CbrtTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (double)d;
                ddouble w = ddouble.Cbrt(v);
                ddouble u = w * w * w - (double)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-31, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (double)d;
                ddouble w = ddouble.Cbrt(v);
                ddouble u = w * w * w - (double)d;

                Assert.AreEqual(0, (double)u, Math.Abs((double)d) * 8e-31, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cbrt_pzero = ddouble.Cbrt(0d);
            ddouble cbrt_mzero = ddouble.Cbrt(-0d);
            ddouble cbrt_pinf = ddouble.Cbrt(double.PositiveInfinity);
            ddouble cbrt_ninf = ddouble.Cbrt(double.NegativeInfinity);
            ddouble cbrt_nan = ddouble.Cbrt(double.NaN);

            Assert.IsTrue(ddouble.IsZero(cbrt_pzero) && cbrt_pzero.Sign == +1, nameof(cbrt_pzero));
            Assert.IsTrue(ddouble.IsZero(cbrt_mzero) && cbrt_mzero.Sign == -1, nameof(cbrt_mzero));
            Assert.IsTrue(ddouble.IsInfinity(cbrt_pinf) && Math.CopySign(1, (double)(cbrt_pinf)) > 0, nameof(cbrt_pinf));
            Assert.IsTrue(ddouble.IsInfinity(cbrt_ninf) && Math.CopySign(1, (double)(cbrt_ninf)) < 0, nameof(cbrt_ninf));
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

                ddouble v = (double)d;
                ddouble u = ddouble.Pow2(v);

                Assert.AreEqual(Math.Pow(2, (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
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
            Assert.IsTrue(ddouble.IsInfinity(pow2_pinf) && Math.CopySign(1, (double)(pow2_pinf)) > 0, nameof(pow2_pinf));
            Assert.IsTrue(pow2_ninf == 0, nameof(pow2_ninf));
            Assert.IsTrue(ddouble.IsNaN(pow2_nan), nameof(pow2_nan));
        }

        [TestMethod]
        public void PowTest() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = 0.1m; x <= +10m; x += 0.1m) {
                    if (y == 0) {
                        continue;
                    }

                    ddouble u = ddouble.Pow((double)x, (double)y);

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

                ddouble v = (double)d;
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
            Assert.IsTrue(ddouble.IsInfinity(pow10_pinf) && Math.CopySign(1, (double)(pow10_pinf)) > 0, nameof(pow10_pinf));
            Assert.IsTrue(pow10_ninf == 0, nameof(pow10_ninf));
            Assert.IsTrue(ddouble.IsNaN(pow10_nan), nameof(pow10_nan));
        }

        [TestMethod]
        public void ExpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Exp(v);

                Assert.AreEqual(Math.Exp((double)d), (double)u, (double)u * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.E - ddouble.Exp(1)) < 1e-31);

            ddouble exp_pzero = ddouble.Pow10(0d);
            ddouble exp_mzero = ddouble.Pow10(-0d);
            ddouble exp_pinf = ddouble.Pow10(double.PositiveInfinity);
            ddouble exp_ninf = ddouble.Pow10(double.NegativeInfinity);
            ddouble exp_nan = ddouble.Pow10(double.NaN);

            Assert.IsTrue(exp_pzero == 1, nameof(exp_pzero));
            Assert.IsTrue(exp_mzero == 1, nameof(exp_mzero));
            Assert.IsTrue(ddouble.IsInfinity(exp_pinf) && Math.CopySign(1, (double)(exp_pinf)) > 0, nameof(exp_pinf));
            Assert.IsTrue(exp_ninf == 0, nameof(exp_ninf));
            Assert.IsTrue(ddouble.IsNaN(exp_nan), nameof(exp_nan));
        }

        [TestMethod]
        public void SinPIHalfTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
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

            Assert.IsTrue(sin_pzero == 0, nameof(sin_pzero));
            Assert.IsTrue(sin_mzero == 0, nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }
    }
}
