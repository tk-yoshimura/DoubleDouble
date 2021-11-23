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

                Assert.AreEqual(0, (double)u, 1e-32);
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Rcp(v) * (double)d - 1;

                Assert.AreEqual(0, (double)u, 1e-32);
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
            ddouble rcp_eps = ddouble.Rcp(double.Epsilon);

            Assert.IsTrue(ddouble.IsInfinity(rcp_pzero) && Math.CopySign(1, (double)(rcp_pzero)) > 0, nameof(rcp_pzero));
            Assert.IsTrue(ddouble.IsInfinity(rcp_mzero) && Math.CopySign(1, (double)(rcp_mzero)) < 0, nameof(rcp_mzero));
            Assert.IsTrue((double)rcp_pinf == 0 && Math.CopySign(1, (double)(rcp_pinf)) > 0, nameof(rcp_pinf));
            Assert.IsTrue((double)rcp_ninf == 0 && Math.CopySign(1, (double)(rcp_ninf)) < 0, nameof(rcp_ninf));
            Assert.IsTrue(ddouble.IsNaN(rcp_nan), nameof(rcp_nan));
            Assert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            Assert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(ddouble.IsInfinity(rcp_eps) && Math.CopySign(1, (double)(rcp_eps)) > 0, nameof(rcp_eps));
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
            Assert.IsTrue(ddouble.IsInfinity(log2_mzero) && Math.CopySign(1, (double)(log2_mzero)) < 0, nameof(log2_mzero));
            Assert.IsTrue(ddouble.IsInfinity(log2_pinf) && Math.CopySign(1, (double)(log2_pinf)) > 0, nameof(log2_pinf));
            Assert.IsTrue(ddouble.IsNaN(log2_ninf), nameof(log2_ninf));
            Assert.IsTrue(ddouble.IsNaN(log2_nan), nameof(log2_nan));
        }
    }
}
