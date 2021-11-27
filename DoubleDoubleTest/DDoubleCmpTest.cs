using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleCmpTest {
        [TestMethod]
        public void BitIncrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (double)d;
                ddouble u = ddouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (double)d;
                ddouble u = ddouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }
        }

        [TestMethod]
        public void BitDecrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (double)d;
                ddouble u = ddouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (double)d;
                ddouble u = ddouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }
        }

        [TestMethod]
        public void CmpTest() {
            ddouble prcp3 = ddouble.Rcp(3);
            ddouble prcp3_bitdec = ddouble.BitDecrement(prcp3);
            ddouble prcp3_bitinc = ddouble.BitIncrement(prcp3);
            ddouble nrcp3 = -prcp3;
            ddouble nrcp3_bitdec = -prcp3_bitdec;
            ddouble nrcp3_bitinc = -prcp3_bitinc;

            Assert.IsTrue(prcp3 > prcp3_bitdec);
            Assert.IsFalse(prcp3 <= prcp3_bitdec);
            Assert.IsTrue(prcp3 < prcp3_bitinc);
            Assert.IsFalse(prcp3 >= prcp3_bitinc);

            Assert.IsTrue(nrcp3 < nrcp3_bitdec);
            Assert.IsFalse(nrcp3 >= nrcp3_bitdec);
            Assert.IsTrue(nrcp3 > nrcp3_bitinc);
            Assert.IsFalse(nrcp3 <= nrcp3_bitinc);

            Assert.IsFalse(prcp3 > ddouble.NaN);
            Assert.IsFalse(prcp3 < ddouble.NaN);
            Assert.IsFalse(prcp3 >= ddouble.NaN);
            Assert.IsFalse(prcp3 <= ddouble.NaN);
            Assert.IsFalse(prcp3 == ddouble.NaN);
            Assert.IsTrue(prcp3 != ddouble.NaN);

            Assert.IsFalse(nrcp3 > ddouble.NaN);
            Assert.IsFalse(nrcp3 < ddouble.NaN);
            Assert.IsFalse(nrcp3 >= ddouble.NaN);
            Assert.IsFalse(nrcp3 <= ddouble.NaN);
            Assert.IsFalse(nrcp3 == ddouble.NaN);
            Assert.IsTrue(nrcp3 != ddouble.NaN);

            Assert.IsFalse(prcp3 > ddouble.PositiveInfinity);
            Assert.IsTrue(prcp3 < ddouble.PositiveInfinity);
            Assert.IsFalse(prcp3 >= ddouble.PositiveInfinity);
            Assert.IsTrue(prcp3 <= ddouble.PositiveInfinity);
            Assert.IsFalse(prcp3 == ddouble.PositiveInfinity);
            Assert.IsTrue(prcp3 != ddouble.PositiveInfinity);

            Assert.IsFalse(nrcp3 > ddouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 < ddouble.PositiveInfinity);
            Assert.IsFalse(nrcp3 >= ddouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 <= ddouble.PositiveInfinity);
            Assert.IsFalse(nrcp3 == ddouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 != ddouble.PositiveInfinity);

            Assert.IsTrue(prcp3 > ddouble.NegativeInfinity);
            Assert.IsFalse(prcp3 < ddouble.NegativeInfinity);
            Assert.IsTrue(prcp3 >= ddouble.NegativeInfinity);
            Assert.IsFalse(prcp3 <= ddouble.NegativeInfinity);
            Assert.IsFalse(prcp3 == ddouble.NegativeInfinity);
            Assert.IsTrue(prcp3 != ddouble.NegativeInfinity);

            Assert.IsTrue(nrcp3 > ddouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 < ddouble.NegativeInfinity);
            Assert.IsTrue(nrcp3 >= ddouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 <= ddouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 == ddouble.NegativeInfinity);
            Assert.IsTrue(nrcp3 != ddouble.NegativeInfinity);

            Assert.IsTrue(prcp3.Equals(prcp3));
            Assert.IsFalse(prcp3.Equals((object)null));

            Assert.IsTrue(prcp3.GetHashCode() == ddouble.Rcp(3).GetHashCode());
        }
    }
}
