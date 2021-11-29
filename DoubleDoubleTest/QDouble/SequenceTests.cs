using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class SequenceTests {
        [TestMethod]
        public void TaylorTest() {
            for (int n = 0; n < qdouble.TaylorSequence.Count; n++) {
                Console.WriteLine($"1/{n}! = {qdouble.TaylorSequence[n]}");
            }

            Assert.AreEqual(1, qdouble.TaylorSequence[0]);
            Assert.AreEqual(1, qdouble.TaylorSequence[1]);
            Assert.AreEqual(qdouble.Rcp(2), qdouble.TaylorSequence[2]);
            Assert.AreEqual(qdouble.Rcp(6), qdouble.TaylorSequence[3]);
            Assert.AreEqual(qdouble.Rcp(24), qdouble.TaylorSequence[4]);
            Assert.AreEqual(qdouble.Rcp(120), qdouble.TaylorSequence[5]);
            Assert.AreEqual(qdouble.Rcp(720), qdouble.TaylorSequence[6]);
        }

        [TestMethod]
        public void BernoulliTest() {
            for (int n = 0; n <= 32; n += 4) {
                Console.WriteLine($"B({2 * n}) = {qdouble.BernoulliSequence[n]}");
            }

            for (int n = 0; n <= 32; n++) {
                Console.WriteLine($"B({2 * n}) = {qdouble.BernoulliSequence[n]}");
            }

            Assert.AreEqual(1, qdouble.BernoulliSequence[0]);
            HPAssert.NeighborBits(qdouble.Rcp(6), qdouble.BernoulliSequence[1]);
            HPAssert.NeighborBits((qdouble)(-1) / 30, qdouble.BernoulliSequence[2]);
            HPAssert.NeighborBits((qdouble)(1) / 42, qdouble.BernoulliSequence[3]);
            HPAssert.NeighborBits((qdouble)(-1) / 30, qdouble.BernoulliSequence[4]);
            HPAssert.NeighborBits((qdouble)(5) / 66, qdouble.BernoulliSequence[5]);
            HPAssert.NeighborBits((qdouble)(-691) / 2730, qdouble.BernoulliSequence[6]);
            HPAssert.NeighborBits((qdouble)(7) / 6, qdouble.BernoulliSequence[7]);
        }

        [TestMethod]
        public void HarmonicTest() {
            for (int n = 0; n <= 64; n += 4) {
                Console.WriteLine($"H({n}) = {qdouble.HarmonicNumber(n)}");
            }

            for (int n = 0; n <= 64; n++) {
                Console.WriteLine($"H({n}) = {qdouble.HarmonicNumber(n)}");
            }

            Assert.AreEqual(0, qdouble.HarmonicNumber(0));
            Assert.AreEqual(1, qdouble.HarmonicNumber(1));
            HPAssert.NeighborBits((qdouble)(3) / 2, qdouble.HarmonicNumber(2));
            HPAssert.NeighborBits((qdouble)(11) / 6, qdouble.HarmonicNumber(3));
            HPAssert.NeighborBits((qdouble)(25) / 12, qdouble.HarmonicNumber(4));
        }
    }
}
