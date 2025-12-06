using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class BitAssertTests {
        [TestMethod]
        public void NeighborBitsTest() {
            BitAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80EC1CD1uL), 1);

            BitAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B81DC1CD1uL), 2);

            Assert.ThrowsExactly<AssertFailedException>(() => {
                BitAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B90DC1CD1uL), 4);
            });

            Assert.ThrowsExactly<AssertFailedException>(() => {
                BitAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B81DC1CD1uL), 1);
            });

            Assert.ThrowsExactly<AssertFailedException>(() => {
                BitAssert.NeighborBits(
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL),
                    (+1, +1, 0xC90FDAA22168C234uL, 0xC4C6628B90DC1CD1uL), 2);
            });
        }
    }
}
