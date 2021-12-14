using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class IntegerSplitterTests {
        [TestMethod]
        public void SplitTest() {
            Assert.AreEqual((0x89ABCDEF01234uL, 0x56789ABCDEF01uL << 1), IntegerSplitter.Split(0x89ABCDEF01234567uL, 0x89ABCDEF01234567uL));
            Assert.AreEqual((0xA011222333344uL, (0x4445555556666uL << 1) + 1), IntegerSplitter.Split(0xA011222333344444uL, 0x5555556666666777uL));
            Assert.AreEqual((0xD76543210FEDCuL, 0xBA989ABCDEF01uL << 1), IntegerSplitter.Split(0xD76543210FEDCBA9uL, 0x89ABCDEF01234567uL));
            Assert.AreEqual((0xF123212321232uL, 0x1232123212321uL << 1), IntegerSplitter.Split(0xF123212321232123uL, 0x2123212321232123uL));
            Assert.AreEqual((0xF123212321232uL, (0x1232123212321uL << 1) + 1), IntegerSplitter.Split(0xF123212321232123uL, 0x2123212321632123uL));
            Assert.AreEqual((0xFABCBABCBABCBuL, (0xABCBABCBABCBAuL << 1) + 2), IntegerSplitter.Split(0xFABCBABCBABCBABCuL, 0xBABCBABCBAC00000uL));
            Assert.AreEqual((0xFABCBABCBABCBuL, (0xABCBABCBABCBAuL << 1) + 1), IntegerSplitter.Split(0xFABCBABCBABCBABCuL, 0xBABCBABCBA400000uL));
            Assert.AreEqual((0xFABCBABCBABCBuL, (0xABCBABCBABCBAuL << 1) + 1), IntegerSplitter.Split(0xFABCBABCBABCBABCuL, 0xBABCBABCBA800000uL));
            Assert.AreEqual((0xF123212321232uL, 0x1232123212321uL << 1), IntegerSplitter.Split(0xF123212321232123uL, 0x2123212321232123uL));

            Assert.AreEqual((0x89ABCDEF01235uL, 0x0uL), IntegerSplitter.Split(0x89ABCDEF01234FFFuL, 0xFFFFFFFFFFFFFFFFuL));
            Assert.AreEqual((0x89ABCDEF01234uL, (0xFFFFFFFFFFFFFuL << 1) + 1), IntegerSplitter.Split(0x89ABCDEF01234FFFuL, 0xFFFFFFFFFF400000uL));
            Assert.AreEqual((0x89ABCDEF01234uL, (0xFFFFFFFFFFFFFuL << 1) + 1), IntegerSplitter.Split(0x89ABCDEF01234FFFuL, 0xFFFFFFFFFF800000uL));
            Assert.AreEqual((0x89ABCDEF01235uL, 0x0uL), IntegerSplitter.Split(0x89ABCDEF01234FFFuL, 0xFFFFFFFFFFC00000uL));

            Assert.AreEqual((0x10000000000000uL, 0x0uL), IntegerSplitter.Split(0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFFFuL));
            Assert.AreEqual((0xFFFFFFFFFFFFFuL, (0xFFFFFFFFFFFFFuL << 1) + 1), IntegerSplitter.Split(0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF400000uL));
            Assert.AreEqual((0xFFFFFFFFFFFFFuL, (0xFFFFFFFFFFFFFuL << 1) + 1), IntegerSplitter.Split(0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF800000uL));
            Assert.AreEqual((0x10000000000000uL, 0x0uL), IntegerSplitter.Split(0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFC00000uL));
        }
    }
}
