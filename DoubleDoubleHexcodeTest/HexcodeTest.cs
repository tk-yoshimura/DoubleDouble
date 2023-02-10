using DoubleDoubleHexcode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleHexcodeTest {
    [TestClass]
    public class HexcodeTest {
        [TestMethod]
        public void ZeroTest() {
            Hexcode hex = (0, 0, 0x0000000000000000uL, 0x0000000000000000uL);

            Assert.AreEqual((0x0000000000000000uL, 0x0000000000000000uL), hex);

            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (0, 1, 0x0000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (0, 0, 0x1000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (0, 0, 0x0000000000000000uL, 0x1000000000000000uL);
            });
        }

        [TestMethod]
        public void NormalTest() {
            Hexcode hex1 = (1, 12, 0x8123456789ABCDEFuL, 0x123456789ABCE000uL);
            Hexcode hex2 = (-1, -24, 0x89ABCDEF12345678uL, 0x9ABCEF1234567000uL);
            Hexcode hex3 = (-1, -200, 0x8010010001000010uL, 0x0000100001008000uL);

            Assert.AreEqual((0x40B8123456789ABCuL, 0xDEF123456789ABCEuL), hex1);
            Assert.AreEqual((0xBE789ABCDEF12345uL, 0x6789ABCEF1234567uL), hex2);
            Assert.AreEqual((0xB378010010001000uL, 0x0100000100001008uL), hex3);
        }

        [TestMethod]
        public void LimitTest() {
            Hexcode hex_maxexp_plus = (1, 1023, 0x8000000000000000uL, 0x0000000000000000uL);
            Hexcode hex_minexp_plus = (1, -1022, 0x8000000000000000uL, 0x0000000000000000uL);
            Hexcode hex_maxexp_minus = (-1, 1023, 0x8000000000000000uL, 0x0000000000000000uL);
            Hexcode hex_minexp_minus = (-1, -1022, 0x8000000000000000uL, 0x0000000000000000uL);

            Assert.AreEqual((0x7FE8000000000000uL, 0x0000000000000000uL), hex_maxexp_plus);
            Assert.AreEqual((0x0018000000000000uL, 0x0000000000000000uL), hex_minexp_plus);
            Assert.AreEqual((0xFFE8000000000000uL, 0x0000000000000000uL), hex_maxexp_minus);
            Assert.AreEqual((0x8018000000000000uL, 0x0000000000000000uL), hex_minexp_minus);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                Hexcode _ = (1, 1024, 0x8000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                Hexcode _ = (1, -1023, 0x8000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                Hexcode _ = (-1, 1024, 0x8000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                Hexcode _ = (-1, -1023, 0x8000000000000000uL, 0x0000000000000000uL);
            });
        }

        [TestMethod]
        public void RoundTest() {
            Hexcode hex1 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFFFuL);
            Hexcode hex2 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFF7uL);
            Hexcode hex3 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFF7FuL);
            Hexcode hex4 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFF7FFuL);
            Hexcode hex5 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFF7FFFuL);
            Hexcode hex6 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFF7FFFFuL);
            Hexcode hex7 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF7FFFFFuL);

            Hexcode hex8 = (1, 0, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFF800uL);
            Hexcode hex9 = (1, 1023, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFF7FFuL);

            Assert.AreEqual((0x4008000000000000uL, 0x0000000000000000uL), hex1);
            Assert.AreEqual((0x4008000000000000uL, 0x0000000000000000uL), hex2);
            Assert.AreEqual((0x4008000000000000uL, 0x0000000000000000uL), hex3);
            Assert.AreEqual((0x3FFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFFFuL), hex4);
            Assert.AreEqual((0x3FFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFF8uL), hex5);
            Assert.AreEqual((0x3FFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFF80uL), hex6);
            Assert.AreEqual((0x3FFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFF800uL), hex7);

            Assert.AreEqual((0x4008000000000000uL, 0x0000000000000000uL), hex8);
            Assert.AreEqual((0x7FEFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFFFuL), hex9);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                Hexcode _ = (1, 1023, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFF800uL);
            });
        }

        [TestMethod]
        public void InvalidMantissaTest() {
            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (1, 1, 0x7000000000000000uL, 0x0000000000000000uL);
            });
            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (1, 2, 0x7000000000000000uL, 0x7000000000000000uL);
            });
            Assert.ThrowsException<ArgumentException>(() => {
                Hexcode _ = (1, 3, 0x0000000000000000uL, 0x7000000000000000uL);
            });
        }
    }
}