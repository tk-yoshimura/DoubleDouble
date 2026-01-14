using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System.Linq;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public partial class EnumTests {
        [TestMethod]
        public void EnumStatsTest() {
            ddouble[] xs = new ddouble[] { 1, 3, 6, 2, 4 };
            ddouble[] xs_withnan = new ddouble[] { 1, 3, 6, 2, 4, double.NaN };
            ddouble[] xs_none = Enumerable.Empty<ddouble>().ToArray();

            Assert.AreEqual(16, xs.Sum());
            Assert.AreEqual(16 / 5.0, (double)xs.Average(), 1e-10);
            Assert.AreEqual(1, xs.Min());
            Assert.AreEqual(6, xs.Max());

            PrecisionAssert.IsNaN(xs_withnan.Min());
            PrecisionAssert.IsNaN(xs_withnan.Max());

            PrecisionAssert.IsNaN(xs_none.Min());
            PrecisionAssert.IsNaN(xs_none.Max());
        }

        [TestMethod]
        public void KahanSumTest() {
            const int n = 10000;
            ddouble v = ddouble.Rcp(n);

            ddouble[] xs = Enumerable.Repeat(v, n).ToArray();

            ddouble sum_raw = 0;
            foreach (ddouble x in xs) {
                sum_raw += x;
            }
            ddouble err_raw = ddouble.Abs(1 - sum_raw);

            ddouble sum_kahan = xs.Sum(), err_kahan = ddouble.Abs(1 - sum_kahan);

            Assert.IsTrue(err_kahan < sum_raw);
        }
    }
}
