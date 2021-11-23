using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoubleDouble;
using System;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleArithmeticTest {
        [TestMethod]
        public void RcpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Rcp(v) * (double)d - 1;

                Assert.AreEqual(0, (double)u, 1e-32);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (double)d;
                ddouble u = ddouble.Rcp(v) * (double)d - 1;

                Assert.AreEqual(0, (double)u, 1e-32);
            }

            ddouble rcp_pzero = ddouble.Rcp(0d);
            ddouble rcp_mzero = ddouble.Rcp(-0d);
            ddouble rcp_pinf = ddouble.Rcp(double.PositiveInfinity);
            ddouble rcp_ninf = ddouble.Rcp(double.NegativeInfinity);
            ddouble rcp_nan = ddouble.Rcp(double.NaN);
            ddouble rcp_pval = ddouble.Rcp(double.MaxValue);
            ddouble rcp_mval = ddouble.Rcp(double.MinValue);
            ddouble rcp_eps = ddouble.Rcp(double.Epsilon);

            Assert.IsTrue(double.IsInfinity((double)rcp_pzero) && Math.CopySign(1, (double)(rcp_pzero)) > 0, nameof(rcp_pzero));
            Assert.IsTrue(double.IsInfinity((double)rcp_mzero) && Math.CopySign(1, (double)(rcp_mzero)) < 0, nameof(rcp_mzero));
            Assert.IsTrue((double)rcp_pinf == 0 && Math.CopySign(1, (double)(rcp_pinf)) > 0, nameof(rcp_pinf));
            Assert.IsTrue((double)rcp_ninf == 0 && Math.CopySign(1, (double)(rcp_ninf)) < 0, nameof(rcp_ninf));
            Assert.IsTrue(double.IsNaN((double)rcp_nan), nameof(rcp_nan));
            Assert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            Assert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(double.IsInfinity((double)rcp_eps) && Math.CopySign(1, (double)(rcp_eps)) > 0, nameof(rcp_eps));
        }
    }
}
