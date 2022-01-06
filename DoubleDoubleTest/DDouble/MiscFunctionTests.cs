using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class MiscFunctionTests {
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
        public void SquareTest() {
            Assert.AreEqual(4, ddouble.Square(2));
            Assert.AreEqual(9, ddouble.Square(3));
        }

        [TestMethod]
        public void CubeTest() {
            Assert.AreEqual(8, ddouble.Cube(2));
            Assert.AreEqual(27, ddouble.Cube(3));
        }
    }
}
