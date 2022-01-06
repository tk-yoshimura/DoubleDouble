using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class HPAssertTests {
        [TestMethod]
        public void HPAssertTest() {
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)1, (ddouble)1 + (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)2, (ddouble)2 + (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.PI, ddouble.PI + (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(-ddouble.PI, -ddouble.PI + (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)1, (ddouble)1 - (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)2, (ddouble)2 - (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.PI, ddouble.PI - (ddouble)1e-28, 1e-29);
            });
            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(-ddouble.PI, -ddouble.PI - (ddouble)1e-28, 1e-29);
            });


            HPAssert.AreEqual((ddouble)2, (ddouble)2 + (ddouble)1e-28, 1e-27);

            HPAssert.AreEqual(-(ddouble)2, -(ddouble)2 + (ddouble)1e-28, 1e-27);

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(-(ddouble)2, (ddouble)2 + (ddouble)1e-28, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)2, -(ddouble)2 + (ddouble)1e-28, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.NaN, (ddouble)2 + (ddouble)1e-28, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual((ddouble)2 + (ddouble)1e-28, ddouble.NaN, 1e-27);
            });

            HPAssert.AreEqual(ddouble.NaN, ddouble.NaN, 1e-27);

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.NaN, ddouble.PositiveInfinity, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.NaN, ddouble.NegativeInfinity, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.PositiveInfinity, ddouble.NaN, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.NegativeInfinity, ddouble.NaN, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.PositiveInfinity, ddouble.NegativeInfinity, 1e-27);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.AreEqual(ddouble.NegativeInfinity, ddouble.PositiveInfinity, 1e-27);
            });

            HPAssert.AreEqual(ddouble.PositiveInfinity, ddouble.PositiveInfinity, 1e-27);
            HPAssert.AreEqual(ddouble.NegativeInfinity, ddouble.NegativeInfinity, 1e-27);
        }

        [TestMethod]
        public void NeighborBitsTest() {
            HPAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80EC1CD1uL), 1);

            HPAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B81DC1CD1uL), 2);

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B90DC1CD1uL), 4);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B81DC1CD1uL), 1);
            });

            Assert.ThrowsException<AssertFailedException>(() => {
                HPAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B90DC1CD1uL), 2);
            });
        }
    }
}
