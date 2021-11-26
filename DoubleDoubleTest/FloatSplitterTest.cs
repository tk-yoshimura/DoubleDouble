using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest {
    [TestClass]
    public class FloatSplitterTest {
        [TestMethod]
        public void SplitDoubleTest() {
            foreach (double v in new double[] {
                -10, -1, -0.1, -0.01, 0.01, 0.1, 1, 10,
                double.MaxValue, double.MinValue,
                1 / (double.MaxValue / 8), 1 / (double.MinValue / 8),
                0d, -0d,
                Math.BitDecrement(2d), 2, Math.BitIncrement(2d),
                double.PositiveInfinity, double.NegativeInfinity,
                double.NaN
            }) {

                (int sign, int exponent, UInt64 mantissa, bool iszero) = FloatSplitter.Split(v);

                Console.WriteLine(v);
                Console.WriteLine($"  {sign} {exponent} 0x{mantissa:X14} iszero:{iszero}");

                Assert.AreEqual((int)Math.CopySign(1, v), sign, nameof(sign));

                Assert.AreEqual(v == 0, iszero);

                if (Math.Abs((long)Math.ILogB(v)) < 10L) {
                    Assert.AreEqual(mantissa, FloatSplitter.Split(v * 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(v / 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(-v * 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(-v / 2d).mantissa, nameof(mantissa));

                    Assert.AreEqual(exponent + 1, FloatSplitter.Split(v * 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent - 1, FloatSplitter.Split(v / 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent + 1, FloatSplitter.Split(-v * 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent - 1, FloatSplitter.Split(-v / 2d).exponent, nameof(mantissa));
                }
            }
        }

        [TestMethod]
        public void SplitDDoubleTest() {
            foreach (ddouble v in new ddouble[] {
                -10, -1, -1 / (ddouble)(10), -1 / (ddouble)(7),
                -1 / (ddouble)(5),  -1 / (ddouble)(3),  -1 / (ddouble)(100),
                1 / (ddouble)(100), 1 / (ddouble)(10), 1 / (ddouble)(3),
                1 / (ddouble)(5), 1 / (ddouble)(7), 1, 10,
                ddouble.BitDecrement(2d), 2, ddouble.BitIncrement(2d),
                double.PositiveInfinity, double.NegativeInfinity,
                double.NaN
            }) {

                (int sign, int exponent, BigInteger mantissa, bool iszero) = FloatSplitter.Split(v);

                Console.WriteLine(v);
                Console.WriteLine($"  {sign} {exponent} 0x{mantissa:X27} iszero:{iszero}");

                Assert.AreEqual((int)Math.CopySign(1, v.Hi), sign, nameof(sign));

                Assert.AreEqual(v == 0, iszero);

                if (Math.Abs((long)Math.ILogB(v.Hi)) < 10L) {
                    Assert.AreEqual(mantissa, FloatSplitter.Split(v * 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(v / 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(-v * 2d).mantissa, nameof(mantissa));
                    Assert.AreEqual(mantissa, FloatSplitter.Split(-v / 2d).mantissa, nameof(mantissa));

                    Assert.AreEqual(exponent + 1, FloatSplitter.Split(v * 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent - 1, FloatSplitter.Split(v / 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent + 1, FloatSplitter.Split(-v * 2d).exponent, nameof(mantissa));
                    Assert.AreEqual(exponent - 1, FloatSplitter.Split(-v / 2d).exponent, nameof(mantissa));
                }
            }
        }
    }
}
