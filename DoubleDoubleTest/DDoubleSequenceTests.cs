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
    }
}
