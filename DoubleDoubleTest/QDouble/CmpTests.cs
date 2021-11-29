using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class CmpTests {
        [TestMethod]
        public void BitIncrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                qdouble v = (qdouble)d;
                qdouble u = qdouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                qdouble v = (qdouble)d;
                qdouble u = qdouble.BitIncrement(v);

                Assert.IsTrue(v < u);
                Assert.IsFalse(v >= u);
            }
        }

        [TestMethod]
        public void BitDecrementTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                qdouble v = (qdouble)d;
                qdouble u = qdouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                qdouble v = (qdouble)d;
                qdouble u = qdouble.BitDecrement(v);

                Assert.IsTrue(v > u);
                Assert.IsFalse(v <= u);
            }
        }

        [TestMethod]
        public void QDoubleCmpTest() {
            qdouble prcp3 = qdouble.Rcp(3);
            qdouble prcp3_bitdec = qdouble.BitDecrement(prcp3);
            qdouble prcp3_bitinc = qdouble.BitIncrement(prcp3);
            qdouble nrcp3 = -prcp3;
            qdouble nrcp3_bitdec = -prcp3_bitdec;
            qdouble nrcp3_bitinc = -prcp3_bitinc;

            Assert.IsTrue(prcp3 > prcp3_bitdec);
            Assert.IsFalse(prcp3 <= prcp3_bitdec);
            Assert.IsTrue(prcp3 < prcp3_bitinc);
            Assert.IsFalse(prcp3 >= prcp3_bitinc);

            Assert.IsTrue(nrcp3 < nrcp3_bitdec);
            Assert.IsFalse(nrcp3 >= nrcp3_bitdec);
            Assert.IsTrue(nrcp3 > nrcp3_bitinc);
            Assert.IsFalse(nrcp3 <= nrcp3_bitinc);

            Assert.IsFalse(prcp3 > qdouble.NaN);
            Assert.IsFalse(prcp3 < qdouble.NaN);
            Assert.IsFalse(prcp3 >= qdouble.NaN);
            Assert.IsFalse(prcp3 <= qdouble.NaN);
            Assert.IsFalse(prcp3 == qdouble.NaN);
            Assert.IsTrue(prcp3 != qdouble.NaN);

            Assert.IsFalse(nrcp3 > qdouble.NaN);
            Assert.IsFalse(nrcp3 < qdouble.NaN);
            Assert.IsFalse(nrcp3 >= qdouble.NaN);
            Assert.IsFalse(nrcp3 <= qdouble.NaN);
            Assert.IsFalse(nrcp3 == qdouble.NaN);
            Assert.IsTrue(nrcp3 != qdouble.NaN);

            Assert.IsFalse(prcp3 > qdouble.PositiveInfinity);
            Assert.IsTrue(prcp3 < qdouble.PositiveInfinity);
            Assert.IsFalse(prcp3 >= qdouble.PositiveInfinity);
            Assert.IsTrue(prcp3 <= qdouble.PositiveInfinity);
            Assert.IsFalse(prcp3 == qdouble.PositiveInfinity);
            Assert.IsTrue(prcp3 != qdouble.PositiveInfinity);

            Assert.IsFalse(nrcp3 > qdouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 < qdouble.PositiveInfinity);
            Assert.IsFalse(nrcp3 >= qdouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 <= qdouble.PositiveInfinity);
            Assert.IsFalse(nrcp3 == qdouble.PositiveInfinity);
            Assert.IsTrue(nrcp3 != qdouble.PositiveInfinity);

            Assert.IsTrue(prcp3 > qdouble.NegativeInfinity);
            Assert.IsFalse(prcp3 < qdouble.NegativeInfinity);
            Assert.IsTrue(prcp3 >= qdouble.NegativeInfinity);
            Assert.IsFalse(prcp3 <= qdouble.NegativeInfinity);
            Assert.IsFalse(prcp3 == qdouble.NegativeInfinity);
            Assert.IsTrue(prcp3 != qdouble.NegativeInfinity);

            Assert.IsTrue(nrcp3 > qdouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 < qdouble.NegativeInfinity);
            Assert.IsTrue(nrcp3 >= qdouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 <= qdouble.NegativeInfinity);
            Assert.IsFalse(nrcp3 == qdouble.NegativeInfinity);
            Assert.IsTrue(nrcp3 != qdouble.NegativeInfinity);

            Assert.IsTrue(prcp3.Equals(prcp3));
            Assert.IsFalse(prcp3.Equals((object)null));

            Assert.IsTrue(prcp3.GetHashCode() == qdouble.Rcp(3).GetHashCode());
        }

        [TestMethod]
        public void DDoubleCmpTest() {
            qdouble prcp3 = qdouble.Rcp(3);
            ddouble prcp3_bitdec = prcp3.Hi;
            ddouble prcp3_bitinc = ddouble.BitIncrement(prcp3_bitdec);
            qdouble nrcp3 = -prcp3;
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

            Assert.IsFalse(prcp3_bitdec > prcp3);
            Assert.IsTrue(prcp3_bitdec <= prcp3);
            Assert.IsFalse(prcp3_bitinc < prcp3);
            Assert.IsTrue(prcp3_bitinc >= prcp3);

            Assert.IsFalse(nrcp3_bitdec < nrcp3);
            Assert.IsTrue(nrcp3_bitdec >= nrcp3);
            Assert.IsFalse(nrcp3_bitinc > nrcp3);
            Assert.IsTrue(nrcp3_bitinc <= nrcp3);

            Assert.IsFalse(ddouble.NaN > prcp3);
            Assert.IsFalse(ddouble.NaN < prcp3);
            Assert.IsFalse(ddouble.NaN >= prcp3);
            Assert.IsFalse(ddouble.NaN <= prcp3);
            Assert.IsFalse(ddouble.NaN == prcp3);
            Assert.IsTrue(ddouble.NaN != prcp3);
            Assert.IsFalse(ddouble.NaN > nrcp3);
            Assert.IsFalse(ddouble.NaN < nrcp3);
            Assert.IsFalse(ddouble.NaN >= nrcp3);
            Assert.IsFalse(ddouble.NaN <= nrcp3);
            Assert.IsFalse(ddouble.NaN == nrcp3);
            Assert.IsTrue(ddouble.NaN != nrcp3);

            Assert.IsTrue(ddouble.PositiveInfinity > prcp3);
            Assert.IsFalse(ddouble.PositiveInfinity < prcp3);
            Assert.IsTrue(ddouble.PositiveInfinity >= prcp3);
            Assert.IsFalse(ddouble.PositiveInfinity <= prcp3);
            Assert.IsFalse(ddouble.PositiveInfinity == prcp3);
            Assert.IsTrue(ddouble.PositiveInfinity != prcp3);

            Assert.IsTrue(ddouble.PositiveInfinity > nrcp3);
            Assert.IsFalse(ddouble.PositiveInfinity < nrcp3);
            Assert.IsTrue(ddouble.PositiveInfinity >= nrcp3);
            Assert.IsFalse(ddouble.PositiveInfinity <= nrcp3);
            Assert.IsFalse(ddouble.PositiveInfinity == nrcp3);
            Assert.IsTrue(ddouble.PositiveInfinity != nrcp3);

            Assert.IsFalse(ddouble.NegativeInfinity > prcp3);
            Assert.IsTrue(ddouble.NegativeInfinity < prcp3);
            Assert.IsFalse(ddouble.NegativeInfinity >= prcp3);
            Assert.IsTrue(ddouble.NegativeInfinity <= prcp3);
            Assert.IsFalse(ddouble.NegativeInfinity == prcp3);
            Assert.IsTrue(ddouble.NegativeInfinity != prcp3);

            Assert.IsFalse(ddouble.NegativeInfinity > nrcp3);
            Assert.IsTrue(ddouble.NegativeInfinity < nrcp3);
            Assert.IsFalse(ddouble.NegativeInfinity >= nrcp3);
            Assert.IsTrue(ddouble.NegativeInfinity <= nrcp3);
            Assert.IsFalse(ddouble.NegativeInfinity == nrcp3);
            Assert.IsTrue(ddouble.NegativeInfinity != nrcp3);
        }

        [TestMethod]
        public void LongCmpTest() {
            qdouble pn = 0x4FFFFFFFFFFFFFFFL;
            long pn_bitdec = 0x4FFFFFFFFFFFFFFEL;
            long pn_equal = 0x4FFFFFFFFFFFFFFFL;
            long pn_bitinc = 0x5000000000000000L;
            qdouble nn = -pn;
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
            qdouble pn = 0x4FFFFFFFFFFFFFFFuL;
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
            qdouble pn = 0x4FFFFFFF;
            int pn_bitdec = 0x4FFFFFFE;
            int pn_equal = 0x4FFFFFFF;
            int pn_bitinc = 0x50000000;
            qdouble nn = -pn;
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
            qdouble pn = 0x4FFFFFFFu;
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
