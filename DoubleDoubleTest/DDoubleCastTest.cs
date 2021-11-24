using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleCastTest {
        [TestMethod]
        public void BigIntegerTest() {
            Assert.AreEqual(0d, (double)(ddouble)(BigInteger)(0));
            Assert.AreEqual(1d, (double)(ddouble)(BigInteger)(1));
            Assert.AreEqual(-1d, (double)(ddouble)(BigInteger)(-1));
            Assert.AreEqual(1000d, (double)(ddouble)(BigInteger)(1000));
            Assert.AreEqual(-1000d, (double)(ddouble)(BigInteger)(-1000));

            Assert.AreEqual((ddouble)0x0000FFFFFFFFFFFFuL, (double)(ddouble)(BigInteger)(0x0000FFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0001000000000000uL, (double)(ddouble)(BigInteger)(0x0001000000000000uL));
            Assert.AreEqual((ddouble)0x000FFFFFFFFFFFFFuL, (double)(ddouble)(BigInteger)(0x000FFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0010000000000000uL, (double)(ddouble)(BigInteger)(0x0010000000000000uL));
            Assert.AreEqual((ddouble)0x00FFFFFFFFFFFFFFuL, (double)(ddouble)(BigInteger)(0x00FFFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0100000000000000uL, (double)(ddouble)(BigInteger)(0x0100000000000000uL));
            Assert.AreEqual((ddouble)0x0FFFFFFFFFFFFFFFuL, (double)(ddouble)(BigInteger)(0x0FFFFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x1000000000000000uL, (double)(ddouble)(BigInteger)(0x1000000000000000uL));
        }
    }
}
