using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class ConstantTests {
        [TestMethod]
        public void ETest() {
            qdouble expected = (qdouble)"2.71828182845904523536028747135266250";
            qdouble actual = qdouble.E;
            qdouble error = expected - actual;

            Console.WriteLine(actual);
            Console.WriteLine(error);

            Assert.IsTrue(qdouble.Abs(error) < 1e-31);

            Console.WriteLine(qdouble.BitDecrement(expected) - actual);
            Console.WriteLine(qdouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");
        }

        [TestMethod]
        public void PITest() {
            qdouble expected = (qdouble)"3.14159265358979323846264338327950288";
            qdouble actual = qdouble.PI;
            qdouble error = expected - actual;

            Console.WriteLine(actual);
            Console.WriteLine(error);

            Assert.IsTrue(qdouble.Abs(error) < 1e-31);

            Console.WriteLine(qdouble.BitDecrement(expected) - actual);
            Console.WriteLine(qdouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");
        }
    }
}
