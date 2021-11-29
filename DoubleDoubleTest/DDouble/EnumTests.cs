using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public partial class EnumTests {
        [TestMethod]
        public void EnumStatsTest() {
            ddouble[] xs = new ddouble[] { 1, 3, 6, 2, 4 };

            Assert.AreEqual(16, xs.Sum());
            Assert.AreEqual(16 / 5.0, (double)xs.Average(), 1e-10);
            Assert.AreEqual(1, xs.Min());
            Assert.AreEqual(6, xs.Max());
        }
    }
}
