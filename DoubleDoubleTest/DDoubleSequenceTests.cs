using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleSequenceTests {
        [TestMethod]
        public void TaylorTest() {
            for (int n = 0; n < ddouble.TaylorSequence.Count; n++) {
                Console.WriteLine($"1/{n}! = {ddouble.TaylorSequence[n]}");
            }

            Assert.AreEqual(1, ddouble.TaylorSequence[0]);
            Assert.AreEqual(1, ddouble.TaylorSequence[1]);
            Assert.AreEqual(ddouble.Rcp(2), ddouble.TaylorSequence[2]);
            Assert.AreEqual(ddouble.Rcp(6), ddouble.TaylorSequence[3]);
            Assert.AreEqual(ddouble.Rcp(24), ddouble.TaylorSequence[4]);
            Assert.AreEqual(ddouble.Rcp(120), ddouble.TaylorSequence[5]);
            Assert.AreEqual(ddouble.Rcp(720), ddouble.TaylorSequence[6]);
        }

        [TestMethod]
        public void BernoulliTest() {
            for (int n = 0; n <= 32; n += 4) {
                Console.WriteLine($"B({2 * n}) = {ddouble.BernoulliSequence[n]}");
            }

            for (int n = 0; n <= 32; n++) {
                Console.WriteLine($"B({2 * n}) = {ddouble.BernoulliSequence[n]}");
            }

            Assert.AreEqual(1, ddouble.BernoulliSequence[0]);
            DDoubleAssert.NeighborBits(ddouble.Rcp(6), ddouble.BernoulliSequence[1]);
            DDoubleAssert.NeighborBits((ddouble)(-1) / 30, ddouble.BernoulliSequence[2]);
            DDoubleAssert.NeighborBits((ddouble)(1) / 42, ddouble.BernoulliSequence[3]);
            DDoubleAssert.NeighborBits((ddouble)(-1) / 30, ddouble.BernoulliSequence[4]);
            DDoubleAssert.NeighborBits((ddouble)(5) / 66, ddouble.BernoulliSequence[5]);
            DDoubleAssert.NeighborBits((ddouble)(-691) / 2730, ddouble.BernoulliSequence[6]);
            DDoubleAssert.NeighborBits((ddouble)(7) / 6, ddouble.BernoulliSequence[7]);
        }

        [TestMethod]
        public void HarmonicTest() {
            for (int n = 0; n <= 64; n += 4) {
                Console.WriteLine($"H({n}) = {ddouble.HarmonicNumber(n)}");
            }

            for (int n = 0; n <= 64; n++) {
                Console.WriteLine($"H({n}) = {ddouble.HarmonicNumber(n)}");
            }

            Assert.AreEqual(0, ddouble.HarmonicNumber(0));
            Assert.AreEqual(1, ddouble.HarmonicNumber(1));
            DDoubleAssert.NeighborBits((ddouble)(3) / 2, ddouble.HarmonicNumber(2));
            DDoubleAssert.NeighborBits((ddouble)(11) / 6, ddouble.HarmonicNumber(3));
            DDoubleAssert.NeighborBits((ddouble)(25) / 12, ddouble.HarmonicNumber(4));
        }
    }
}
