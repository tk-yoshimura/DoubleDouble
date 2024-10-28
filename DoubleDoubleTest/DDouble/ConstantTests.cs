using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ConstantTests {
        [TestMethod]
        public void BasicTest() {
            Assert.IsTrue(ddouble.One == 1);
            Assert.IsTrue(ddouble.MinusOne == -1);
        }

        [TestMethod]
        public void ETest() {
            ddouble expected = (ddouble)"2.71828182845904523536028747135266250";
            ddouble actual = ddouble.E;

            Console.WriteLine(actual);

            PrecisionAssert.AlmostEqual(expected, actual, 2e-32);
            BitAssert.NeighborBits(expected, actual, 1);

            Console.WriteLine(ddouble.BitDecrement(expected) - actual);
            Console.WriteLine(ddouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");
        }

        [TestMethod]
        public void PITest() {
            ddouble expected = (ddouble)"3.14159265358979323846264338327950288";
            ddouble actual = ddouble.PI;
            Console.WriteLine(actual);

            PrecisionAssert.AlmostEqual(expected, actual, 2e-32);
            BitAssert.NeighborBits(expected, actual, 1);

            Console.WriteLine(ddouble.BitDecrement(expected) - actual);
            Console.WriteLine(ddouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");
        }

        [TestMethod]
        public void Sqrt2Test() {
            PrecisionAssert.AlmostEqual(2, ddouble.Sqrt2 * ddouble.Sqrt2, 1e-31);
        }

        [TestMethod]
        public void ErdosBorweinTest() {
            PrecisionAssert.AlmostEqual(
                "1.6066951524152917637833015231909245804805796715057564357780795536",
                ddouble.ErdosBorwein,
                1e-31
            );
        }

        [TestMethod]
        public void FeigenbaumDeltaTest() {
            PrecisionAssert.AlmostEqual(
                "4.6692016091029906718532038204662016172581855774757686327456513430",
                ddouble.FeigenbaumDelta,
                1e-31
            );
        }

        [TestMethod]
        public void LemniscatePITest() {
            PrecisionAssert.AlmostEqual(
                "2.6220575542921198104648395898911194136827549514316231628168217038",
                ddouble.LemniscatePI,
                1e-31
            );
        }
    }
}
