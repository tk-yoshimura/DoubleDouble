using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;
using System.Numerics;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class INumberTests {
        [TestMethod]
        public void SignTest() {
            Assert.AreEqual(double.Sign(0d), ddouble.Sign(0d));
            Assert.AreEqual(double.Sign(-0d), ddouble.Sign(-0d));
            Assert.AreEqual(double.Sign(1d), ddouble.Sign(1d));
            Assert.AreEqual(double.Sign(-1d), ddouble.Sign(-1d));
            Assert.AreEqual(double.Sign(double.MaxValue), ddouble.Sign(double.MaxValue));
            Assert.AreEqual(double.Sign(double.MinValue), ddouble.Sign(double.MinValue));
            Assert.AreEqual(double.Sign(double.PositiveInfinity), ddouble.Sign(double.PositiveInfinity));
            Assert.AreEqual(double.Sign(double.NegativeInfinity), ddouble.Sign(double.NegativeInfinity));

            Assert.ThrowsExactly<ArithmeticException>(() => {
                _ = double.Sign(double.NaN);
            });

            Assert.ThrowsExactly<ArithmeticException>(() => {
                _ = ddouble.Sign(double.NaN);
            });
        }

        [TestMethod]
        public void IsNormalTest() {
            Assert.AreEqual(double.IsNormal(0d), ddouble.IsNormal(0d));
            Assert.AreEqual(double.IsNormal(-0d), ddouble.IsNormal(-0d));
            Assert.AreEqual(double.IsNormal(1d), ddouble.IsNormal(1d));
            Assert.AreEqual(double.IsNormal(-1d), ddouble.IsNormal(-1d));
            Assert.AreEqual(double.IsNormal(double.MaxValue), ddouble.IsNormal(double.MaxValue));
            Assert.AreEqual(double.IsNormal(double.MinValue), ddouble.IsNormal(double.MinValue));
            Assert.AreEqual(double.IsNormal(double.PositiveInfinity), ddouble.IsNormal(double.PositiveInfinity));
            Assert.AreEqual(double.IsNormal(double.NegativeInfinity), ddouble.IsNormal(double.NegativeInfinity));
            Assert.AreEqual(double.IsNormal(double.NaN), ddouble.IsNormal(double.NaN));

            Assert.IsTrue(ddouble.IsNormal(double.ScaleB(1, -967)));
            Assert.IsTrue(ddouble.IsNormal(double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsNormal(double.ScaleB(1, -969)));

            Assert.IsTrue(ddouble.IsNormal(ddouble.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsTrue(ddouble.IsNormal(double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsNormal(ddouble.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsTrue(ddouble.IsNormal(double.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsTrue(ddouble.IsNormal(double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsNormal(double.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsTrue(ddouble.IsNormal(-double.ScaleB(1, -967)));
            Assert.IsTrue(ddouble.IsNormal(-double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsNormal(-double.ScaleB(1, -969)));

            Assert.IsTrue(ddouble.IsNormal(-ddouble.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsTrue(ddouble.IsNormal(-double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsNormal(-ddouble.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsTrue(ddouble.IsNormal(-double.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsTrue(ddouble.IsNormal(-double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsNormal(-double.BitDecrement(double.ScaleB(1, -968))));
        }

        [TestMethod]
        public void IsSubnormalTest() {
            Assert.AreEqual(double.IsSubnormal(0d), ddouble.IsSubnormal(0d));
            Assert.AreEqual(double.IsSubnormal(-0d), ddouble.IsSubnormal(-0d));
            Assert.AreEqual(double.IsSubnormal(1d), ddouble.IsSubnormal(1d));
            Assert.AreEqual(double.IsSubnormal(-1d), ddouble.IsSubnormal(-1d));
            Assert.AreEqual(double.IsSubnormal(double.MaxValue), ddouble.IsSubnormal(double.MaxValue));
            Assert.AreEqual(double.IsSubnormal(double.MinValue), ddouble.IsSubnormal(double.MinValue));
            Assert.AreEqual(double.IsSubnormal(double.PositiveInfinity), ddouble.IsSubnormal(double.PositiveInfinity));
            Assert.AreEqual(double.IsSubnormal(double.NegativeInfinity), ddouble.IsSubnormal(double.NegativeInfinity));
            Assert.AreEqual(double.IsSubnormal(double.NaN), ddouble.IsSubnormal(double.NaN));

            Assert.IsFalse(ddouble.IsSubnormal(double.ScaleB(1, -967)));
            Assert.IsFalse(ddouble.IsSubnormal(double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsSubnormal(double.ScaleB(1, -969)));

            Assert.IsFalse(ddouble.IsSubnormal(ddouble.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsFalse(ddouble.IsSubnormal(double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsSubnormal(ddouble.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsFalse(ddouble.IsSubnormal(double.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsFalse(ddouble.IsSubnormal(double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsSubnormal(double.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsFalse(ddouble.IsSubnormal(-double.ScaleB(1, -967)));
            Assert.IsFalse(ddouble.IsSubnormal(-double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsSubnormal(-double.ScaleB(1, -969)));

            Assert.IsFalse(ddouble.IsSubnormal(-ddouble.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsFalse(ddouble.IsSubnormal(-double.ScaleB(1, -968)));
            Assert.IsFalse(ddouble.IsSubnormal(-ddouble.BitDecrement(double.ScaleB(1, -968))));

            Assert.IsFalse(ddouble.IsSubnormal(-double.BitIncrement(double.ScaleB(1, -968))));
            Assert.IsFalse(ddouble.IsSubnormal(-double.ScaleB(1, -968)));
            Assert.IsTrue(ddouble.IsSubnormal(-double.BitDecrement(double.ScaleB(1, -968))));
        }

        [TestMethod]
        public void IsCanonicalTest() {
            Assert.IsTrue(ddouble.IsCanonical(0d));
            Assert.IsTrue(ddouble.IsCanonical(-0d));
            Assert.IsTrue(ddouble.IsCanonical(1d));
            Assert.IsTrue(ddouble.IsCanonical(-1d));
            Assert.IsTrue(ddouble.IsCanonical(double.MaxValue));
            Assert.IsTrue(ddouble.IsCanonical(double.MinValue));
            Assert.IsFalse(ddouble.IsCanonical(double.PositiveInfinity));
            Assert.IsFalse(ddouble.IsCanonical(double.NegativeInfinity));
            Assert.IsFalse(ddouble.IsCanonical(double.NaN));
        }

        [TestMethod]
        public void IsComplexTest() {
            Assert.IsTrue(ddouble.IsRealNumber(0d));
            Assert.IsTrue(ddouble.IsRealNumber(-0d));
            Assert.IsTrue(ddouble.IsRealNumber(1d));
            Assert.IsTrue(ddouble.IsRealNumber(-1d));
            Assert.IsTrue(ddouble.IsRealNumber(double.MaxValue));
            Assert.IsTrue(ddouble.IsRealNumber(double.MinValue));
            Assert.IsTrue(ddouble.IsRealNumber(double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsRealNumber(double.NegativeInfinity));
            Assert.IsFalse(ddouble.IsRealNumber(double.NaN));

            Assert.IsFalse(ddouble.IsImaginaryNumber(0d));
            Assert.IsFalse(ddouble.IsImaginaryNumber(-0d));
            Assert.IsFalse(ddouble.IsImaginaryNumber(1d));
            Assert.IsFalse(ddouble.IsImaginaryNumber(-1d));
            Assert.IsFalse(ddouble.IsImaginaryNumber(double.MaxValue));
            Assert.IsFalse(ddouble.IsImaginaryNumber(double.MinValue));
            Assert.IsFalse(ddouble.IsImaginaryNumber(double.PositiveInfinity));
            Assert.IsFalse(ddouble.IsImaginaryNumber(double.NegativeInfinity));
            Assert.IsFalse(ddouble.IsImaginaryNumber(double.NaN));

            Assert.IsFalse(ddouble.IsComplexNumber(0d));
            Assert.IsFalse(ddouble.IsComplexNumber(-0d));
            Assert.IsFalse(ddouble.IsComplexNumber(1d));
            Assert.IsFalse(ddouble.IsComplexNumber(-1d));
            Assert.IsFalse(ddouble.IsComplexNumber(double.MaxValue));
            Assert.IsFalse(ddouble.IsComplexNumber(double.MinValue));
            Assert.IsFalse(ddouble.IsComplexNumber(double.PositiveInfinity));
            Assert.IsFalse(ddouble.IsComplexNumber(double.NegativeInfinity));
            Assert.IsFalse(ddouble.IsComplexNumber(double.NaN));
        }

        [TestMethod]
        public void IsPositiveNegativeTest() {
            Assert.IsTrue(ddouble.IsPositive(0d));
            Assert.IsFalse(ddouble.IsPositive(-0d));
            Assert.IsTrue(ddouble.IsPositive(1d));
            Assert.IsFalse(ddouble.IsPositive(-1d));
            Assert.IsTrue(ddouble.IsPositive(double.MaxValue));
            Assert.IsFalse(ddouble.IsPositive(double.MinValue));
            Assert.IsTrue(ddouble.IsPositive(double.PositiveInfinity));
            Assert.IsFalse(ddouble.IsPositive(double.NegativeInfinity));
            Assert.AreEqual(double.IsPositive(double.NaN), ddouble.IsPositive(double.NaN));

            Assert.IsFalse(ddouble.IsNegative(0d));
            Assert.IsTrue(ddouble.IsNegative(-0d));
            Assert.IsFalse(ddouble.IsNegative(1d));
            Assert.IsTrue(ddouble.IsNegative(-1d));
            Assert.IsFalse(ddouble.IsNegative(double.MaxValue));
            Assert.IsTrue(ddouble.IsNegative(double.MinValue));
            Assert.IsFalse(ddouble.IsNegative(double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNegative(double.NegativeInfinity));
            Assert.AreEqual(double.IsNegative(double.NaN), ddouble.IsNegative(double.NaN));
        }

        [TestMethod]
        public void IsIntegerTest() {
            Assert.IsTrue(ddouble.IsInteger(0d));
            Assert.IsTrue(ddouble.IsInteger(-0d));
            Assert.IsTrue(ddouble.IsInteger(1d));
            Assert.IsTrue(ddouble.IsInteger(-1d));
            Assert.IsTrue(ddouble.IsInteger(2d));
            Assert.IsTrue(ddouble.IsInteger(-2d));
            Assert.IsTrue(ddouble.IsInteger(3d));
            Assert.IsTrue(ddouble.IsInteger(-3d));
            Assert.IsTrue(ddouble.IsInteger(4d));
            Assert.IsTrue(ddouble.IsInteger(-4d));

            Assert.IsFalse(ddouble.IsOddInteger(0d));
            Assert.IsFalse(ddouble.IsOddInteger(-0d));
            Assert.IsTrue(ddouble.IsOddInteger(1d));
            Assert.IsTrue(ddouble.IsOddInteger(-1d));
            Assert.IsFalse(ddouble.IsOddInteger(2d));
            Assert.IsFalse(ddouble.IsOddInteger(-2d));
            Assert.IsTrue(ddouble.IsOddInteger(3d));
            Assert.IsTrue(ddouble.IsOddInteger(-3d));
            Assert.IsFalse(ddouble.IsOddInteger(4d));
            Assert.IsFalse(ddouble.IsOddInteger(-4d));

            Assert.IsTrue(ddouble.IsEvenInteger(0d));
            Assert.IsTrue(ddouble.IsEvenInteger(-0d));
            Assert.IsFalse(ddouble.IsEvenInteger(1d));
            Assert.IsFalse(ddouble.IsEvenInteger(-1d));
            Assert.IsTrue(ddouble.IsEvenInteger(2d));
            Assert.IsTrue(ddouble.IsEvenInteger(-2d));
            Assert.IsFalse(ddouble.IsEvenInteger(3d));
            Assert.IsFalse(ddouble.IsEvenInteger(-3d));
            Assert.IsTrue(ddouble.IsEvenInteger(4d));
            Assert.IsTrue(ddouble.IsEvenInteger(-4d));

            for (BigInteger n = 10; n.ToString().Length <= 40; n *= 10) {
                Assert.IsTrue(ddouble.IsInteger(n));
                Assert.IsTrue(ddouble.IsInteger(-n));

                Assert.IsFalse(ddouble.IsOddInteger(n));
                Assert.IsFalse(ddouble.IsOddInteger(-n));

                Assert.IsTrue(ddouble.IsEvenInteger(n));
                Assert.IsTrue(ddouble.IsEvenInteger(-n));
            }

            for (BigInteger n = 10; n.ToString().Length <= 31; n *= 10) {
                Assert.IsTrue(ddouble.IsOddInteger(n + 1));
                Assert.IsTrue(ddouble.IsOddInteger(-n - 1));

                Assert.IsFalse(ddouble.IsEvenInteger(n + 1));
                Assert.IsFalse(ddouble.IsEvenInteger(-n - 1));
            }
        }

        [TestMethod]
        public void MagnitudeTest() {
            Assert.AreEqual((ddouble)4, ddouble.MaxMagnitude(3, 4));
            Assert.AreEqual((ddouble)(-4), ddouble.MaxMagnitude(3, -4));
            Assert.AreEqual((ddouble)4, ddouble.MaxMagnitude(-3, 4));
            Assert.AreEqual((ddouble)(-4), ddouble.MaxMagnitude(-3, -4));

            Assert.AreEqual(ddouble.NaN, ddouble.MaxMagnitude(3, ddouble.NaN));
            Assert.AreEqual(ddouble.NaN, ddouble.MaxMagnitude(ddouble.NaN, 4));
            Assert.AreEqual(ddouble.NaN, ddouble.MaxMagnitude(ddouble.NaN, ddouble.NaN));

            Assert.AreEqual((ddouble)3, ddouble.MinMagnitude(3, 4));
            Assert.AreEqual((ddouble)3, ddouble.MinMagnitude(3, -4));
            Assert.AreEqual((ddouble)(-3), ddouble.MinMagnitude(-3, 4));
            Assert.AreEqual((ddouble)(-3), ddouble.MinMagnitude(-3, -4));

            Assert.AreEqual(ddouble.NaN, ddouble.MinMagnitude(3, ddouble.NaN));
            Assert.AreEqual(ddouble.NaN, ddouble.MinMagnitude(ddouble.NaN, 4));
            Assert.AreEqual(ddouble.NaN, ddouble.MinMagnitude(ddouble.NaN, ddouble.NaN));

            Assert.AreEqual((ddouble)4, ddouble.MaxMagnitudeNumber(3, 4));
            Assert.AreEqual((ddouble)(-4), ddouble.MaxMagnitudeNumber(3, -4));
            Assert.AreEqual((ddouble)4, ddouble.MaxMagnitudeNumber(-3, 4));
            Assert.AreEqual((ddouble)(-4), ddouble.MaxMagnitudeNumber(-3, -4));

            Assert.AreEqual((ddouble)3, ddouble.MaxMagnitudeNumber(3, ddouble.NaN));
            Assert.AreEqual((ddouble)4, ddouble.MaxMagnitudeNumber(ddouble.NaN, 4));
            Assert.AreEqual(ddouble.NaN, ddouble.MaxMagnitudeNumber(ddouble.NaN, ddouble.NaN));

            Assert.AreEqual((ddouble)3, ddouble.MinMagnitudeNumber(3, 4));
            Assert.AreEqual((ddouble)3, ddouble.MinMagnitudeNumber(3, -4));
            Assert.AreEqual((ddouble)(-3), ddouble.MinMagnitudeNumber(-3, 4));
            Assert.AreEqual((ddouble)(-3), ddouble.MinMagnitudeNumber(-3, -4));

            Assert.AreEqual((ddouble)3, ddouble.MinMagnitudeNumber(3, ddouble.NaN));
            Assert.AreEqual((ddouble)4, ddouble.MinMagnitudeNumber(ddouble.NaN, 4));
            Assert.AreEqual(ddouble.NaN, ddouble.MinMagnitudeNumber(ddouble.NaN, ddouble.NaN));
        }

        [TestMethod]
        public void MathFunctionTest() {
            PrecisionAssert.AlmostEqual(double.Exp10(0.25), ddouble.Exp10(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp10(0.5), ddouble.Exp10(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp10(1), ddouble.Exp10(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp10(2), ddouble.Exp10(2), 1e-15);

            PrecisionAssert.AlmostEqual(double.Exp2(0.25), ddouble.Exp2(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp2(0.5), ddouble.Exp2(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp2(1), ddouble.Exp2(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp2(2), ddouble.Exp2(2), 1e-15);

            PrecisionAssert.AlmostEqual(double.SinPi(-0.25), ddouble.SinPi(-0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.SinPi(0.25), ddouble.SinPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.SinPi(0.5), ddouble.SinPi(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.SinPi(1), ddouble.SinPi(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.SinPi(2), ddouble.SinPi(2), 1e-15);

            PrecisionAssert.AlmostEqual(double.CosPi(-0.25), ddouble.CosPi(-0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.CosPi(0.25), ddouble.CosPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.CosPi(0.5), ddouble.CosPi(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.CosPi(1), ddouble.CosPi(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.CosPi(2), ddouble.CosPi(2), 1e-15);

            PrecisionAssert.AlmostEqual(double.TanPi(-0.25), ddouble.TanPi(-0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.TanPi(0.25), ddouble.TanPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.TanPi(0.5), ddouble.TanPi(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.TanPi(1), ddouble.TanPi(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.TanPi(2), ddouble.TanPi(2), 1e-15);

            PrecisionAssert.AreEqual(double.AsinPi(-1), ddouble.AsinPi(-1));
            PrecisionAssert.AreEqual(double.AsinPi(0), ddouble.AsinPi(0));
            PrecisionAssert.AlmostEqual(double.AsinPi(0.25), ddouble.AsinPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.AsinPi(0.5), ddouble.AsinPi(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.AsinPi(1), ddouble.AsinPi(1));

            PrecisionAssert.AreEqual(double.AcosPi(-1), ddouble.AcosPi(-1));
            PrecisionAssert.AreEqual(double.AcosPi(0), ddouble.AcosPi(0));
            PrecisionAssert.AlmostEqual(double.AcosPi(0.25), ddouble.AcosPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.AcosPi(0.5), ddouble.AcosPi(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.AcosPi(1), ddouble.AcosPi(1));

            PrecisionAssert.AreEqual(double.AtanPi(-1), ddouble.AtanPi(-1));
            PrecisionAssert.AreEqual(double.AtanPi(0), ddouble.AtanPi(0));
            PrecisionAssert.AlmostEqual(double.AtanPi(0.25), ddouble.AtanPi(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.AtanPi(0.5), ddouble.AtanPi(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.AtanPi(1), ddouble.AtanPi(1));

            PrecisionAssert.AlmostEqual(double.SinCos(-0.25).Sin, ddouble.SinCos(-0.25).Sin, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(0.25).Sin, ddouble.SinCos(0.25).Sin, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(0.5).Sin, ddouble.SinCos(0.5).Sin, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(1).Sin, ddouble.SinCos(1).Sin, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(2).Sin, ddouble.SinCos(2).Sin, 1e-15);

            PrecisionAssert.AlmostEqual(double.SinCos(-0.25).Cos, ddouble.SinCos(-0.25).Cos, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(0.25).Cos, ddouble.SinCos(0.25).Cos, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(0.5).Cos, ddouble.SinCos(0.5).Cos, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(1).Cos, ddouble.SinCos(1).Cos, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCos(2).Cos, ddouble.SinCos(2).Cos, 1e-15);

            PrecisionAssert.AlmostEqual(double.SinCosPi(-0.25).SinPi, ddouble.SinCosPi(-0.25).SinPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(0.25).SinPi, ddouble.SinCosPi(0.25).SinPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(0.5).SinPi, ddouble.SinCosPi(0.5).SinPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(1).SinPi, ddouble.SinCosPi(1).SinPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(2).SinPi, ddouble.SinCosPi(2).SinPi, 1e-15);

            PrecisionAssert.AlmostEqual(double.SinCosPi(-0.25).CosPi, ddouble.SinCosPi(-0.25).CosPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(0.25).CosPi, ddouble.SinCosPi(0.25).CosPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(0.5).CosPi, ddouble.SinCosPi(0.5).CosPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(1).CosPi, ddouble.SinCosPi(1).CosPi, 1e-15);
            PrecisionAssert.AlmostEqual(double.SinCosPi(2).CosPi, ddouble.SinCosPi(2).CosPi, 1e-15);

            PrecisionAssert.AlmostEqual(double.Asinh(0.25), ddouble.Asinh(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Asinh(0.5), ddouble.Asinh(0.5), 1e-15);

            PrecisionAssert.AlmostEqual(double.Acosh(0.25), ddouble.Acosh(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Acosh(0.5), ddouble.Acosh(0.5), 1e-15);

            PrecisionAssert.AlmostEqual(double.Atanh(0.25), ddouble.Atanh(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Atanh(0.5), ddouble.Atanh(0.5), 1e-15);

            PrecisionAssert.AlmostEqual(double.LogP1(0.25), ddouble.LogP1(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.LogP1(0.5), ddouble.LogP1(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.LogP1(1), ddouble.LogP1(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.LogP1(2), ddouble.LogP1(2), 1e-15);

            PrecisionAssert.AlmostEqual(double.Log2P1(0.25), ddouble.Log2P1(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Log2P1(0.5), ddouble.Log2P1(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.Log2P1(-0.5), ddouble.Log2P1(-0.5));
            PrecisionAssert.AreEqual(double.Log2P1(1), ddouble.Log2P1(1));
            PrecisionAssert.AlmostEqual(double.Log2P1(2), ddouble.Log2P1(2), 1e-15);
            PrecisionAssert.AreEqual(double.Log2P1(3), ddouble.Log2P1(3));
            PrecisionAssert.AreEqual(double.Log2P1(7), ddouble.Log2P1(7));

            PrecisionAssert.AlmostEqual(double.Log10P1(0.25), ddouble.Log10P1(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Log10P1(0.5), ddouble.Log10P1(0.5), 1e-15);
            PrecisionAssert.AlmostEqual(double.Log10P1(1), ddouble.Log10P1(1), 1e-15);
            PrecisionAssert.AlmostEqual(double.Log10P1(2), ddouble.Log10P1(2), 1e-15);
            PrecisionAssert.AreEqual(double.Log10P1(9), ddouble.Log10P1(9));
            PrecisionAssert.AreEqual(double.Log10P1(99), ddouble.Log10P1(99));

            PrecisionAssert.AreEqual(double.Exp2M1(-1), ddouble.Exp2M1(-1));
            PrecisionAssert.AreEqual(double.Exp2M1(0), ddouble.Exp2M1(0));
            PrecisionAssert.AlmostEqual(double.Exp2M1(0.25), ddouble.Exp2M1(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp2M1(0.5), ddouble.Exp2M1(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.Exp2M1(1), ddouble.Exp2M1(1));

            PrecisionAssert.AlmostEqual(double.Exp10M1(-1), ddouble.Exp10M1(-1), 1e-15);
            PrecisionAssert.AreEqual(double.Exp10M1(0), ddouble.Exp10M1(0));
            PrecisionAssert.AlmostEqual(double.Exp10M1(0.25), ddouble.Exp10M1(0.25), 1e-15);
            PrecisionAssert.AlmostEqual(double.Exp10M1(0.5), ddouble.Exp10M1(0.5), 1e-15);
            PrecisionAssert.AreEqual(double.Exp10M1(1), ddouble.Exp10M1(1));

            PrecisionAssert.AlmostEqual("-2.501057910695717338568327143157314851812e-1", ddouble.Exp10M1(-0.125 - double.ScaleB(1, -32)), 5e-32);
            PrecisionAssert.AlmostEqual("-2.501057906675441726978157243848635615581e-1", ddouble.Exp10M1(-0.125), 5e-32);
            PrecisionAssert.AlmostEqual("-2.501057902655166113232667860715777686238e-1", ddouble.Exp10M1(-0.125 + double.ScaleB(1, -32)), 5e-32);

            PrecisionAssert.AlmostEqual("3.335214314484066913986880274109580594160e-1", ddouble.Exp10M1(0.125 - double.ScaleB(1, -32)), 5e-32);
            PrecisionAssert.AlmostEqual("3.335214321633240256759317152953310924157e-1", ddouble.Exp10M1(0.125), 5e-32);
            PrecisionAssert.AlmostEqual("3.335214328782413603364514291937297356126e-1", ddouble.Exp10M1(0.125 + double.ScaleB(1, -32)), 5e-32);
        }

        [TestMethod]
        public void TryConvertFromChecked() {
            Assert.IsTrue(ddouble.TryConvertFromChecked((int)3, out ddouble v1));
            Assert.AreEqual((ddouble)3, v1);

            Assert.IsTrue(ddouble.TryConvertFromChecked((long)3, out ddouble v2));
            Assert.AreEqual((ddouble)3, v2);

            Assert.IsTrue(ddouble.TryConvertFromChecked((float)3, out ddouble v3));
            Assert.AreEqual((ddouble)3, v3);

            Assert.IsTrue(ddouble.TryConvertFromChecked((double)3, out ddouble v4));
            Assert.AreEqual((ddouble)3, v4);

            Assert.IsTrue(ddouble.TryConvertFromChecked((BigInteger)3, out ddouble v5));
            Assert.AreEqual((ddouble)3, v5);

            Assert.IsFalse(ddouble.TryConvertFromChecked((char)3, out ddouble v6));
            Assert.AreEqual((ddouble)0, v6);
        }

        [TestMethod]
        public void TryConvertToChecked() {
            Assert.IsTrue(ddouble.TryConvertToChecked(3, out int v1));
            Assert.AreEqual((int)3, v1);
            Assert.ThrowsExactly<OverflowException>(() => {
                _ = ddouble.TryConvertToChecked(ddouble.MaxValue, out int _);
            });

            Assert.IsTrue(ddouble.TryConvertToChecked(3, out long v2));
            Assert.AreEqual((long)3, v2);
            Assert.ThrowsExactly<OverflowException>(() => {
                _ = ddouble.TryConvertToChecked(ddouble.MaxValue, out long _);
            });

            Assert.IsTrue(ddouble.TryConvertToChecked(3, out float v3));
            Assert.AreEqual((float)3, v3);
            Assert.IsTrue(ddouble.TryConvertToChecked(ddouble.MaxValue, out float vmax3));
            Assert.IsTrue(ddouble.IsPositiveInfinity(vmax3));

            Assert.IsTrue(ddouble.TryConvertToChecked(3, out double v4));
            Assert.AreEqual((double)3, v4);
            Assert.IsTrue(ddouble.TryConvertToChecked(ddouble.MaxValue, out double vmax4));
            Assert.AreEqual(double.MaxValue, vmax4);

            Assert.IsTrue(ddouble.TryConvertToChecked(3, out decimal v5));
            Assert.AreEqual((decimal)3, v5);
            Assert.ThrowsExactly<OverflowException>(() => {
                _ = ddouble.TryConvertToChecked(ddouble.MaxValue, out decimal _);
            });
        }

        [TestMethod]
        public void TryConvertToSaturating() {
            Assert.IsTrue(ddouble.TryConvertToSaturating(3, out int v1));
            Assert.AreEqual((int)3, v1);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MaxValue, out int vmax1));
            Assert.AreEqual(int.MaxValue, vmax1);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MinValue, out int vmin1));
            Assert.AreEqual(int.MinValue, vmin1);

            Assert.IsTrue(ddouble.TryConvertToSaturating(3, out long v2));
            Assert.AreEqual((long)3, v2);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MaxValue, out long vmax2));
            Assert.AreEqual(long.MaxValue, vmax2);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MinValue, out long vmin2));
            Assert.AreEqual(long.MinValue, vmin2);

            Assert.IsTrue(ddouble.TryConvertToSaturating(3, out float v3));
            Assert.AreEqual((float)3, v3);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MaxValue, out float vmax3));
            Assert.IsTrue(ddouble.IsPositiveInfinity(vmax3));
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MinValue, out float vmin3));
            Assert.IsTrue(ddouble.IsNegativeInfinity(vmin3));

            Assert.IsTrue(ddouble.TryConvertToSaturating(3, out double v4));
            Assert.AreEqual((double)3, v4);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MaxValue, out double vmax4));
            Assert.AreEqual(double.MaxValue, vmax4);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MinValue, out double vmin4));
            Assert.AreEqual(double.MinValue, vmin4);

            Assert.IsTrue(ddouble.TryConvertToSaturating(3, out decimal v5));
            Assert.AreEqual((decimal)3, v5);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MaxValue, out decimal vmax5));
            Assert.AreEqual(decimal.MaxValue, vmax5);
            Assert.IsTrue(ddouble.TryConvertToSaturating(ddouble.MinValue, out decimal vmin5));
            Assert.AreEqual(decimal.MinValue, vmin5);
        }
    }
}
