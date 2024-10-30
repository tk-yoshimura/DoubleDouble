using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class FloatSplitterTests {
        [TestMethod]
        public void SplitDoubleTest() {
            foreach (double v in new double[] {
                -10, -1, -0.1, -0.01, 0.01, 0.1, 1, 10,
                double.MaxValue, double.MinValue,
                1 / (double.MaxValue / 8), 1 / (double.MinValue / 8),
                0d, -0d,
                double.BitDecrement(2d), 2, double.BitIncrement(2d),
                double.PositiveInfinity, double.NegativeInfinity,
                double.NaN
            }) {

                (int sign, int exponent, UInt64 mantissa, bool iszero) = FloatSplitter.Split(v);

                Console.WriteLine(v);
                Console.WriteLine($"  {sign} {exponent} 0x{mantissa:X14} iszero:{iszero}");

                Assert.AreEqual((int)double.CopySign(1, v), sign, nameof(sign));

                Assert.AreEqual(v == 0, iszero);

                if (long.Abs((long)double.ILogB(v)) < 10L) {
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

                (int sign, int exponent, DoubleDouble.UInt128 mantissa, bool iszero) = FloatSplitter.Split(v);

                Console.WriteLine(v);
                Console.WriteLine($"  {sign} {exponent} 0x{mantissa:X27} iszero:{iszero}");

                Assert.AreEqual((int)double.CopySign(1, v.Hi), sign, nameof(sign));

                Assert.AreEqual(v == 0, iszero);

                if (long.Abs((long)double.ILogB(v.Hi)) < 10L) {
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
        public void SplitSubnormalTest() {
            const UInt64 mantissa_mask = 0x000F_FFFF_FFFF_FFFFuL;
            const UInt64 mantissa_topbit = 0x0010_0000_0000_0000uL;

            for ((double x, int exp) = (double.ScaleB(1, -500), -500); x > 0; x /= 2, exp--) {
                (int sign, int exponent, UInt64 mantissa, bool iszero) x_split = FloatSplitter.Split(x);
                (int sign, int exponent, UInt64 mantissa, bool iszero) xinc_split = FloatSplitter.Split(double.BitIncrement(x));
                (int sign, int exponent, UInt64 mantissa, bool iszero) xdec_split = FloatSplitter.Split(double.BitDecrement(x));

                Assert.AreEqual(1, x_split.sign);
                Assert.AreEqual(1, xinc_split.sign);
                Assert.AreEqual(1, xdec_split.sign);

                Assert.AreEqual(exp, x_split.exponent);
                Assert.AreEqual(int.Max(-1073, exp), xinc_split.exponent);

                if (double.BitDecrement(x) > 0) {
                    Assert.AreEqual(exp - 1, xdec_split.exponent);
                    Assert.IsFalse(xdec_split.iszero);
                }
                else {
                    Assert.AreEqual(0, xdec_split.exponent);
                    Assert.IsTrue(xdec_split.iszero);
                }

                {
                    int sfts = int.Max(0, -(exp + 1022));
                    UInt64 mantissa_expected = (1uL << sfts) | mantissa_topbit;

                    Assert.AreEqual(mantissa_expected, xinc_split.mantissa, $"{exp}");
                }

                if (double.BitDecrement(x) > 0) {
                    int sfts = int.Max(0, -(exp + 1021));
                    UInt64 mantissa_expected = ((mantissa_mask >> sfts) << sfts) | mantissa_topbit;

                    Assert.AreEqual(mantissa_expected, xdec_split.mantissa, $"{exp}");
                }
                else {
                    Assert.AreEqual(0uL, xdec_split.mantissa, $"{exp}");
                }
            }
        }
    }
}
