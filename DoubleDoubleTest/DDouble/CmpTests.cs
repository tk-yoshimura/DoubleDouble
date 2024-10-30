using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class CmpTests {
        [TestMethod]
        public void BitIncrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }
        }

        [TestMethod]
        public void BitDecrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }
        }

        [TestMethod]
        public void DDoubleCmpTest() {
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

        [TestMethod]
        public void DoubleCmpTest() {
            ddouble prcp3 = ddouble.Rcp(3);
            double prcp3_bitdec = prcp3.Hi;
            double prcp3_bitinc = double.BitIncrement(prcp3_bitdec);
            ddouble nrcp3 = -prcp3;
            double nrcp3_bitdec = -prcp3_bitdec;
            double nrcp3_bitinc = -prcp3_bitinc;

            Assert.IsTrue(prcp3 > prcp3_bitdec);
            Assert.IsFalse(prcp3 <= prcp3_bitdec);
            Assert.IsTrue(prcp3 < prcp3_bitinc);
            Assert.IsFalse(prcp3 >= prcp3_bitinc);

            Assert.IsTrue(nrcp3 < nrcp3_bitdec);
            Assert.IsFalse(nrcp3 >= nrcp3_bitdec);
            Assert.IsTrue(nrcp3 > nrcp3_bitinc);
            Assert.IsFalse(nrcp3 <= nrcp3_bitinc);

#pragma warning disable CA2242
            Assert.IsFalse(prcp3 > double.NaN);
            Assert.IsFalse(prcp3 < double.NaN);
            Assert.IsFalse(prcp3 >= double.NaN);
            Assert.IsFalse(prcp3 <= double.NaN);
            Assert.IsFalse(prcp3 == double.NaN);
            Assert.IsTrue(prcp3 != double.NaN);

            Assert.IsFalse(nrcp3 > double.NaN);
            Assert.IsFalse(nrcp3 < double.NaN);
            Assert.IsFalse(nrcp3 >= double.NaN);
            Assert.IsFalse(nrcp3 <= double.NaN);
            Assert.IsFalse(nrcp3 == double.NaN);
            Assert.IsTrue(nrcp3 != double.NaN);
#pragma warning restore CA2242

            Assert.IsFalse(prcp3 > double.PositiveInfinity);
            Assert.IsTrue(prcp3 < double.PositiveInfinity);
            Assert.IsFalse(prcp3 >= double.PositiveInfinity);
            Assert.IsTrue(prcp3 <= double.PositiveInfinity);
            Assert.IsFalse(prcp3 == double.PositiveInfinity);
            Assert.IsTrue(prcp3 != double.PositiveInfinity);

            Assert.IsFalse(nrcp3 > double.PositiveInfinity);
            Assert.IsTrue(nrcp3 < double.PositiveInfinity);
            Assert.IsFalse(nrcp3 >= double.PositiveInfinity);
            Assert.IsTrue(nrcp3 <= double.PositiveInfinity);
            Assert.IsFalse(nrcp3 == double.PositiveInfinity);
            Assert.IsTrue(nrcp3 != double.PositiveInfinity);

            Assert.IsTrue(prcp3 > double.NegativeInfinity);
            Assert.IsFalse(prcp3 < double.NegativeInfinity);
            Assert.IsTrue(prcp3 >= double.NegativeInfinity);
            Assert.IsFalse(prcp3 <= double.NegativeInfinity);
            Assert.IsFalse(prcp3 == double.NegativeInfinity);
            Assert.IsTrue(prcp3 != double.NegativeInfinity);

            Assert.IsTrue(nrcp3 > double.NegativeInfinity);
            Assert.IsFalse(nrcp3 < double.NegativeInfinity);
            Assert.IsTrue(nrcp3 >= double.NegativeInfinity);
            Assert.IsFalse(nrcp3 <= double.NegativeInfinity);
            Assert.IsFalse(nrcp3 == double.NegativeInfinity);
            Assert.IsTrue(nrcp3 != double.NegativeInfinity);

            Assert.IsFalse(prcp3_bitdec > prcp3);
            Assert.IsTrue(prcp3_bitdec <= prcp3);
            Assert.IsFalse(prcp3_bitinc < prcp3);
            Assert.IsTrue(prcp3_bitinc >= prcp3);

            Assert.IsFalse(nrcp3_bitdec < nrcp3);
            Assert.IsTrue(nrcp3_bitdec >= nrcp3);
            Assert.IsFalse(nrcp3_bitinc > nrcp3);
            Assert.IsTrue(nrcp3_bitinc <= nrcp3);

#pragma warning disable CA2242
            Assert.IsFalse(double.NaN > prcp3);
            Assert.IsFalse(double.NaN < prcp3);
            Assert.IsFalse(double.NaN >= prcp3);
            Assert.IsFalse(double.NaN <= prcp3);
            Assert.IsFalse(double.NaN == prcp3);
            Assert.IsTrue(double.NaN != prcp3);
            Assert.IsFalse(double.NaN > nrcp3);
            Assert.IsFalse(double.NaN < nrcp3);
            Assert.IsFalse(double.NaN >= nrcp3);
            Assert.IsFalse(double.NaN <= nrcp3);
            Assert.IsFalse(double.NaN == nrcp3);
            Assert.IsTrue(double.NaN != nrcp3);
#pragma warning restore CA2242

            Assert.IsTrue(double.PositiveInfinity > prcp3);
            Assert.IsFalse(double.PositiveInfinity < prcp3);
            Assert.IsTrue(double.PositiveInfinity >= prcp3);
            Assert.IsFalse(double.PositiveInfinity <= prcp3);
            Assert.IsFalse(double.PositiveInfinity == prcp3);
            Assert.IsTrue(double.PositiveInfinity != prcp3);

            Assert.IsTrue(double.PositiveInfinity > nrcp3);
            Assert.IsFalse(double.PositiveInfinity < nrcp3);
            Assert.IsTrue(double.PositiveInfinity >= nrcp3);
            Assert.IsFalse(double.PositiveInfinity <= nrcp3);
            Assert.IsFalse(double.PositiveInfinity == nrcp3);
            Assert.IsTrue(double.PositiveInfinity != nrcp3);

            Assert.IsFalse(double.NegativeInfinity > prcp3);
            Assert.IsTrue(double.NegativeInfinity < prcp3);
            Assert.IsFalse(double.NegativeInfinity >= prcp3);
            Assert.IsTrue(double.NegativeInfinity <= prcp3);
            Assert.IsFalse(double.NegativeInfinity == prcp3);
            Assert.IsTrue(double.NegativeInfinity != prcp3);

            Assert.IsFalse(double.NegativeInfinity > nrcp3);
            Assert.IsTrue(double.NegativeInfinity < nrcp3);
            Assert.IsFalse(double.NegativeInfinity >= nrcp3);
            Assert.IsTrue(double.NegativeInfinity <= nrcp3);
            Assert.IsFalse(double.NegativeInfinity == nrcp3);
            Assert.IsTrue(double.NegativeInfinity != nrcp3);
        }

        [TestMethod]
        public void MaxTest() {
            ddouble prcp3 = ddouble.Rcp(3);
            ddouble nrcp3 = -prcp3;

            Assert.AreEqual(prcp3, ddouble.Max(prcp3, nrcp3));

            Assert.IsTrue(ddouble.IsNaN(ddouble.Max(prcp3, double.NaN)));
            Assert.IsTrue(ddouble.IsNaN(ddouble.Max(double.NaN, nrcp3)));

            Assert.AreEqual(double.PositiveInfinity, ddouble.Max(prcp3, double.PositiveInfinity));
            Assert.AreEqual(double.PositiveInfinity, ddouble.Max(nrcp3, double.PositiveInfinity));
            Assert.AreEqual(prcp3, ddouble.Max(prcp3, double.NegativeInfinity));
            Assert.AreEqual(nrcp3, ddouble.Max(nrcp3, double.NegativeInfinity));

            Assert.AreEqual(double.PositiveInfinity, ddouble.Max(double.PositiveInfinity, prcp3));
            Assert.AreEqual(double.PositiveInfinity, ddouble.Max(double.PositiveInfinity, nrcp3));
            Assert.AreEqual(prcp3, ddouble.Max(double.NegativeInfinity, prcp3));
            Assert.AreEqual(nrcp3, ddouble.Max(double.NegativeInfinity, nrcp3));

            Assert.AreEqual(3, ddouble.Max(1, 2, 3));
            Assert.AreEqual(3, ddouble.Max(3, 1, 2));
            Assert.AreEqual(3, ddouble.Max(2, 3, 1));

            Assert.AreEqual(4, ddouble.Max(1, 2, 3, 4));
            Assert.AreEqual(4, ddouble.Max(4, 1, 2, 3));
            Assert.AreEqual(4, ddouble.Max(3, 4, 1, 2));
            Assert.AreEqual(4, ddouble.Max(2, 3, 4, 1));
        }

        [TestMethod]
        public void MinTest() {
            ddouble prcp3 = ddouble.Rcp(3);
            ddouble nrcp3 = -prcp3;

            Assert.AreEqual(nrcp3, ddouble.Min(prcp3, nrcp3));

            Assert.IsTrue(ddouble.IsNaN(ddouble.Min(prcp3, double.NaN)));
            Assert.IsTrue(ddouble.IsNaN(ddouble.Min(double.NaN, nrcp3)));

            Assert.AreEqual(prcp3, ddouble.Min(prcp3, double.PositiveInfinity));
            Assert.AreEqual(nrcp3, ddouble.Min(nrcp3, double.PositiveInfinity));
            Assert.AreEqual(double.NegativeInfinity, ddouble.Min(prcp3, double.NegativeInfinity));
            Assert.AreEqual(double.NegativeInfinity, ddouble.Min(nrcp3, double.NegativeInfinity));

            Assert.AreEqual(prcp3, ddouble.Min(double.PositiveInfinity, prcp3));
            Assert.AreEqual(nrcp3, ddouble.Min(double.PositiveInfinity, nrcp3));
            Assert.AreEqual(double.NegativeInfinity, ddouble.Min(double.NegativeInfinity, prcp3));
            Assert.AreEqual(double.NegativeInfinity, ddouble.Min(double.NegativeInfinity, nrcp3));

            Assert.AreEqual(1, ddouble.Min(1, 2, 3));
            Assert.AreEqual(1, ddouble.Min(3, 1, 2));
            Assert.AreEqual(1, ddouble.Min(2, 3, 1));

            Assert.AreEqual(1, ddouble.Min(1, 2, 3, 4));
            Assert.AreEqual(1, ddouble.Min(4, 1, 2, 3));
            Assert.AreEqual(1, ddouble.Min(3, 4, 1, 2));
            Assert.AreEqual(1, ddouble.Min(2, 3, 4, 1));
        }

        [TestMethod]
        public void LongCmpTest() {
            ddouble pn = 0x4FFFFFFFFFFFFFFFL;
            long pn_bitdec = 0x4FFFFFFFFFFFFFFEL;
            long pn_equal = 0x4FFFFFFFFFFFFFFFL;
            long pn_bitinc = 0x5000000000000000L;
            ddouble nn = -pn;
            long nn_bitdec = -pn_bitdec;
            long nn_bitinc = -pn_bitinc;

            Assert.IsTrue(pn > pn_bitdec);
            Assert.IsFalse(pn <= pn_bitdec);
            Assert.IsTrue(pn < pn_bitinc);
            Assert.IsFalse(pn >= pn_bitinc);

            Assert.IsTrue(pn != pn_bitdec);
            Assert.IsFalse(pn != pn_equal);
            Assert.IsTrue(pn != pn_bitinc);

            Assert.IsFalse(pn == pn_bitdec);
            Assert.IsTrue(pn == pn_equal);
            Assert.IsFalse(pn == pn_bitinc);

            Assert.IsTrue(pn_bitdec != pn);
            Assert.IsFalse(pn_equal != pn);
            Assert.IsTrue(pn_bitinc != pn);

            Assert.IsFalse(pn_bitdec == pn);
            Assert.IsTrue(pn_equal == pn);
            Assert.IsFalse(pn_bitinc == pn);

            Assert.IsTrue(nn < nn_bitdec);
            Assert.IsFalse(nn >= nn_bitdec);
            Assert.IsTrue(nn > nn_bitinc);
            Assert.IsFalse(nn <= nn_bitinc);

            Assert.IsFalse(pn_bitdec > pn);
            Assert.IsTrue(pn_bitdec <= pn);
            Assert.IsFalse(pn_bitinc < pn);
            Assert.IsTrue(pn_bitinc >= pn);

            Assert.IsFalse(nn_bitdec < nn);
            Assert.IsTrue(nn_bitdec >= nn);
            Assert.IsFalse(nn_bitinc > nn);
            Assert.IsTrue(nn_bitinc <= nn);
        }

        [TestMethod]
        public void ULongCmpTest() {
            ddouble pn = 0x4FFFFFFFFFFFFFFFuL;
            ulong pn_bitdec = 0x4FFFFFFFFFFFFFFEuL;
            ulong pn_equal = 0x4FFFFFFFFFFFFFFFuL;
            ulong pn_bitinc = 0x5000000000000000uL;

            Assert.IsTrue(pn > pn_bitdec);
            Assert.IsFalse(pn <= pn_bitdec);
            Assert.IsTrue(pn < pn_bitinc);
            Assert.IsFalse(pn >= pn_bitinc);

            Assert.IsTrue(pn != pn_bitdec);
            Assert.IsFalse(pn != pn_equal);
            Assert.IsTrue(pn != pn_bitinc);

            Assert.IsFalse(pn == pn_bitdec);
            Assert.IsTrue(pn == pn_equal);
            Assert.IsFalse(pn == pn_bitinc);

            Assert.IsTrue(pn_bitdec != pn);
            Assert.IsFalse(pn_equal != pn);
            Assert.IsTrue(pn_bitinc != pn);

            Assert.IsFalse(pn_bitdec == pn);
            Assert.IsTrue(pn_equal == pn);
            Assert.IsFalse(pn_bitinc == pn);

            Assert.IsFalse(pn_bitdec > pn);
            Assert.IsTrue(pn_bitdec <= pn);
            Assert.IsFalse(pn_bitinc < pn);
            Assert.IsTrue(pn_bitinc >= pn);
        }

        [TestMethod]
        public void IntCmpTest() {
            ddouble pn = 0x4FFFFFFF;
            int pn_bitdec = 0x4FFFFFFE;
            int pn_equal = 0x4FFFFFFF;
            int pn_bitinc = 0x50000000;
            ddouble nn = -pn;
            int nn_bitdec = -pn_bitdec;
            int nn_bitinc = -pn_bitinc;

            Assert.IsTrue(pn > pn_bitdec);
            Assert.IsFalse(pn <= pn_bitdec);
            Assert.IsTrue(pn < pn_bitinc);
            Assert.IsFalse(pn >= pn_bitinc);

            Assert.IsTrue(pn != pn_bitdec);
            Assert.IsFalse(pn != pn_equal);
            Assert.IsTrue(pn != pn_bitinc);

            Assert.IsFalse(pn == pn_bitdec);
            Assert.IsTrue(pn == pn_equal);
            Assert.IsFalse(pn == pn_bitinc);

            Assert.IsTrue(pn_bitdec != pn);
            Assert.IsFalse(pn_equal != pn);
            Assert.IsTrue(pn_bitinc != pn);

            Assert.IsFalse(pn_bitdec == pn);
            Assert.IsTrue(pn_equal == pn);
            Assert.IsFalse(pn_bitinc == pn);

            Assert.IsTrue(nn < nn_bitdec);
            Assert.IsFalse(nn >= nn_bitdec);
            Assert.IsTrue(nn > nn_bitinc);
            Assert.IsFalse(nn <= nn_bitinc);

            Assert.IsFalse(pn_bitdec > pn);
            Assert.IsTrue(pn_bitdec <= pn);
            Assert.IsFalse(pn_bitinc < pn);
            Assert.IsTrue(pn_bitinc >= pn);

            Assert.IsFalse(nn_bitdec < nn);
            Assert.IsTrue(nn_bitdec >= nn);
            Assert.IsFalse(nn_bitinc > nn);
            Assert.IsTrue(nn_bitinc <= nn);
        }

        [TestMethod]
        public void UIntCmpTest() {
            ddouble pn = 0x4FFFFFFFu;
            uint pn_bitdec = 0x4FFFFFFEu;
            uint pn_equal = 0x4FFFFFFFu;
            uint pn_bitinc = 0x50000000u;

            Assert.IsTrue(pn > pn_bitdec);
            Assert.IsFalse(pn <= pn_bitdec);
            Assert.IsTrue(pn < pn_bitinc);
            Assert.IsFalse(pn >= pn_bitinc);

            Assert.IsTrue(pn != pn_bitdec);
            Assert.IsFalse(pn != pn_equal);
            Assert.IsTrue(pn != pn_bitinc);

            Assert.IsFalse(pn == pn_bitdec);
            Assert.IsTrue(pn == pn_equal);
            Assert.IsFalse(pn == pn_bitinc);

            Assert.IsTrue(pn_bitdec != pn);
            Assert.IsFalse(pn_equal != pn);
            Assert.IsTrue(pn_bitinc != pn);

            Assert.IsFalse(pn_bitdec == pn);
            Assert.IsTrue(pn_equal == pn);
            Assert.IsFalse(pn_bitinc == pn);

            Assert.IsFalse(pn_bitdec > pn);
            Assert.IsTrue(pn_bitdec <= pn);
            Assert.IsFalse(pn_bitinc < pn);
            Assert.IsTrue(pn_bitinc >= pn);
        }
    }
}
