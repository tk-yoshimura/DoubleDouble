using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ArithmeticTests {
        [TestMethod]
        public void AddTest() {
            foreach (int n in new int[] { 7, 13, 17 }) {
                ddouble u = ddouble.Rcp(n);
                double v = 1d / n;

                ddouble su = 0;
                double sv = 0;

                for (int i = 0; i < n * 100; i++) {
                    su += u;
                    sv += v;

                    Assert.IsTrue(ddouble.IsRegulared(su));
                }

                Assert.IsLessThanOrEqualTo(double.Abs(100 - sv), double.Abs(100 - (double)su));
                PrecisionAssert.AreEqual(100, su, 2e-28);
            }

            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity + ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity + ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + ddouble.NaN));

            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue + ddouble.MinValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue + ddouble.MinValue));

            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue + ddouble.MinValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue + ddouble.MinValue));

            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity + double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity + double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity + double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity + double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity + double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity + double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN + double.NaN));

            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue + double.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue + double.MinValue));

            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue + double.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue + double.MinValue));

            Assert.IsTrue(ddouble.IsInfinity(double.PositiveInfinity + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.NegativeInfinity + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity + ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity + ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NaN + ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN + ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN + ddouble.NaN));

            Assert.IsTrue(ddouble.IsPositiveInfinity(double.MaxValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(double.MinValue + ddouble.MinValue));

            Assert.IsTrue(ddouble.IsPositiveInfinity(double.MaxValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(double.MinValue + ddouble.MinValue));

            Assert.AreEqual(double.CopySign(1, 0d + double.NegativeZero), ddouble.CopySign(1, ddouble.PlusZero + ddouble.MinusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero + 0d), ddouble.CopySign(1, ddouble.MinusZero + ddouble.PlusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero + double.NegativeZero), ddouble.CopySign(1, ddouble.MinusZero + ddouble.MinusZero));

            Assert.IsTrue(ddouble.IsInfinity(ddouble.MaxValue + ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MaxValue + ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsInfinity(ddouble.MinValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MinValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsFinite(ddouble.MaxValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MaxValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(double.MaxValue < (ddouble.MaxValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.MaxValue > (ddouble.MaxValue + -ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsFinite(ddouble.MinValue + ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MinValue + ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(double.MinValue > (ddouble.MinValue + ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.MinValue < (ddouble.MinValue + ddouble.Ldexp(ddouble.MaxValue, -100)));

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (int y in new int[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x + (ddouble)y, x + y);
                    Assert.AreEqual((ddouble)y + x, y + x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (uint y in new uint[] { 0u, 1u, 32u, 65535u, uint.MaxValue - 1, uint.MaxValue }) {
                    Assert.AreEqual(x + (ddouble)y, x + y);
                    Assert.AreEqual((ddouble)y + x, y + x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (long y in new long[] { long.MinValue, long.MinValue + 1, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue, long.MaxValue - 1, long.MaxValue }) {
                    Assert.AreEqual(x + (ddouble)y, x + y);
                    Assert.AreEqual((ddouble)y + x, y + x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (ulong y in new ulong[] { 0uL, 1uL, 32uL, 65535uL, uint.MaxValue - 1, uint.MaxValue, ulong.MaxValue - 1, ulong.MaxValue }) {
                    Assert.AreEqual(x + (ddouble)y, x + y);
                    Assert.AreEqual((ddouble)y + x, y + x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (double y in new double[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x + (ddouble)y, x + y);
                    Assert.AreEqual((ddouble)y + x, y + x);
                }
            }
        }

        [TestMethod]
        public void SubTest() {
            foreach (int n in new int[] { 7, 13, 17 }) {
                ddouble u = ddouble.Rcp(n);
                double v = 1d / n;

                ddouble su = 0;
                double sv = 0;

                for (int i = 0; i < n * 100; i++) {
                    su -= u;
                    sv -= v;

                    Assert.IsTrue(ddouble.IsRegulared(su));
                }

                Assert.IsLessThanOrEqualTo(double.Abs(-100 - sv), double.Abs(-100 - (double)su));
                PrecisionAssert.AreEqual(-100, su, 2e-28);
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity - ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity - ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - ddouble.NaN));

            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue - ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue - ddouble.MinValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue - ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue - ddouble.MinValue));

            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity - double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity - double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity - double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity - double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity - double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity - double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN - double.NaN));

            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue - double.MaxValue));
            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue - double.MinValue));

            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.PositiveInfinity - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.NegativeInfinity - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity - ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity - ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NaN - ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN - ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN - ddouble.NaN));

            Assert.IsTrue(ddouble.IsNegativeInfinity(double.MinValue - ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsPositiveInfinity(double.MaxValue - ddouble.MinValue));

            Assert.AreEqual(double.CopySign(1, 0d - double.NegativeZero), ddouble.CopySign(1, ddouble.PlusZero - ddouble.MinusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero - 0d), ddouble.CopySign(1, ddouble.MinusZero - ddouble.PlusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero - double.NegativeZero), ddouble.CopySign(1, ddouble.MinusZero - ddouble.MinusZero));

            Assert.IsTrue(ddouble.IsInfinity(ddouble.MaxValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MaxValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsInfinity(ddouble.MinValue - ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MinValue - ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsFinite(ddouble.MaxValue - ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MaxValue - ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(double.MaxValue < (ddouble.MaxValue - ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.MaxValue > (ddouble.MaxValue - ddouble.Ldexp(ddouble.MaxValue, -100)));

            Assert.IsTrue(ddouble.IsFinite(ddouble.MinValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.IsRegulared(ddouble.MinValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(double.MinValue > (ddouble.MinValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));
            Assert.IsTrue(ddouble.MinValue < (ddouble.MinValue - -ddouble.Ldexp(ddouble.MaxValue, -100)));

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (int y in new int[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x - (ddouble)y, x - y);
                    Assert.AreEqual((ddouble)y - x, y - x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (uint y in new uint[] { 0u, 1u, 32u, 65535u, uint.MaxValue - 1, uint.MaxValue }) {
                    Assert.AreEqual(x - (ddouble)y, x - y);
                    Assert.AreEqual((ddouble)y - x, y - x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (long y in new long[] { long.MinValue, long.MinValue + 1, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue, long.MaxValue - 1, long.MaxValue }) {
                    Assert.AreEqual(x - (ddouble)y, x - y);
                    Assert.AreEqual((ddouble)y - x, y - x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (ulong y in new ulong[] { 0uL, 1uL, 32uL, 65535uL, uint.MaxValue - 1, uint.MaxValue, ulong.MaxValue - 1, ulong.MaxValue }) {
                    Assert.AreEqual(x - (ddouble)y, x - y);
                    Assert.AreEqual((ddouble)y - x, y - x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (double y in new double[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x - (ddouble)y, x - y);
                    Assert.AreEqual((ddouble)y - x, y - x);
                }
            }
        }

        [TestMethod]
        public void MulTest() {
            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                ddouble u = (ddouble.Rcp(n)) * (ddouble)n;
                double v = (1d / n) * n;

                Assert.IsLessThanOrEqualTo(double.Abs(1 - v), double.Abs((double)(1 - u)));
                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity * ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity * ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * ddouble.NaN));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PlusZero * ddouble.PlusZero));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.PlusZero * ddouble.MinusZero));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.MinusZero * ddouble.PlusZero));
            Assert.IsTrue(ddouble.IsPlusZero(ddouble.MinusZero * ddouble.MinusZero));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PlusZero * 1d));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.MinusZero * 1d));

            Assert.IsTrue(ddouble.IsPlusZero(1d * ddouble.PlusZero));
            Assert.IsTrue(ddouble.IsMinusZero(1d * ddouble.MinusZero));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PlusZero * ddouble.Pi));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.MinusZero * ddouble.Pi));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.Pi * ddouble.PlusZero));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.Pi * ddouble.MinusZero));

            Assert.AreEqual(double.CopySign(1, 0d * double.NegativeZero), ddouble.CopySign(1, ddouble.PlusZero * ddouble.MinusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero * 0d), ddouble.CopySign(1, ddouble.MinusZero * ddouble.PlusZero));
            Assert.AreEqual(double.CopySign(1, double.NegativeZero * double.NegativeZero), ddouble.CopySign(1, ddouble.MinusZero * ddouble.MinusZero));

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                ddouble u = (ddouble.Rcp(n)) * (double)n;
                double v = (1d / n) * n;

                Assert.IsLessThanOrEqualTo(double.Abs(1 - v), double.Abs((double)(1 - u)));
                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity * double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.PositiveInfinity * double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity * double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(ddouble.NegativeInfinity * double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity * double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity * double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN * double.NaN));

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                ddouble u = (double)n * (ddouble.Rcp(n));
                double v = (1d / n) * n;

                Assert.IsLessThanOrEqualTo(double.Abs(1 - v), double.Abs((double)(1 - u)));
                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.IsInfinity(double.PositiveInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.PositiveInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.NegativeInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsInfinity(double.NegativeInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity * ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity * ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NaN * ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN * ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN * ddouble.NaN));

            foreach (int n in new int[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, int.MaxValue,
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, int.MinValue }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (uint n in new uint[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, uint.MaxValue }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (long n in new long[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295L, 4294967296L, 4294967297L, long.MaxValue,
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, -4294967295L, -4294967296L, -4294967297L, long.MinValue }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ulong n in new ulong[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295uL, 4294967296uL, 4294967297uL, ulong.MaxValue }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                PrecisionAssert.AreEqual(0, 1 - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (int y in new int[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x * (ddouble)y, x * y);
                    Assert.AreEqual((ddouble)y * x, y * x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (uint y in new uint[] { 0u, 1u, 32u, 65535u, uint.MaxValue - 1, uint.MaxValue }) {
                    Assert.AreEqual(x * (ddouble)y, x * y);
                    Assert.AreEqual((ddouble)y * x, y * x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (long y in new long[] { long.MinValue, long.MinValue + 1, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue, long.MaxValue - 1, long.MaxValue }) {
                    Assert.AreEqual(x * (ddouble)y, x * y);
                    Assert.AreEqual((ddouble)y * x, y * x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (ulong y in new ulong[] { 0uL, 1uL, 32uL, 65535uL, uint.MaxValue - 1, uint.MaxValue, ulong.MaxValue - 1, ulong.MaxValue }) {
                    Assert.AreEqual(x * (ddouble)y, x * y);
                    Assert.AreEqual((ddouble)y * x, y * x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (double y in new double[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x * (ddouble)y, x * y);
                    Assert.AreEqual((ddouble)y * x, y * x);
                }
            }

            for (ddouble x = ddouble.Ldexp(1, -250); x > 0; x *= 0.15) {
                for (ddouble y = ddouble.Ldexp(1, -250); y > 0; y *= 0.17) {
                    Assert.IsTrue(ddouble.IsPositive(x * y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, -250); x < 0; x *= 0.15) {
                for (ddouble y = ddouble.Ldexp(1, -250); y > 0; y *= 0.17) {
                    Assert.IsTrue(ddouble.IsNegative(x * y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, -250); x > 0; x *= 0.15) {
                for (ddouble y = -ddouble.Ldexp(1, -250); y < 0; y *= 0.17) {
                    Assert.IsTrue(ddouble.IsNegative(x * y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, -250); x < 0; x *= 0.15) {
                for (ddouble y = -ddouble.Ldexp(1, -250); y < 0; y *= 0.17) {
                    Assert.IsTrue(ddouble.IsPositive(x * y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, -250); x < 0; x *= 0.15) {
                for (ddouble y = -ddouble.Ldexp(1, -250); y < 0; y *= 0.17) {
                    Assert.IsTrue(!ddouble.IsFinite(x * y) || ddouble.IsPositive(x * y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x * y) || ddouble.IsPositive(x * y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x * y) || ddouble.IsNegative(x * y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = -ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x * y) || ddouble.IsNegative(x * y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = -ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x * y) || ddouble.IsPositive(x * y));
                }
            }
        }

        [TestMethod]
        public void DivTest() {
            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (int n in new int[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (ddouble)m / (ddouble)n;
                    ddouble u = v * n;

                    PrecisionAssert.AreEqual(0, m - u, 1e-30);
                    Assert.IsTrue(ddouble.IsRegulared(u));
                    Assert.IsTrue(ddouble.IsRegulared(v));
                }
            }

            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (ddouble n in new ddouble[] { "1e+100", "1e+150", "1e+200", "1e+250" }) {
                    ddouble v = (ddouble)m / (ddouble)n;
                    ddouble u = v * n;

                    PrecisionAssert.AreEqual(0, m - u, double.Abs(m) * 1e-30);
                }
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / ddouble.NaN));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PlusZero / 1));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.MinusZero / 1));

            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (int n in new int[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (double)m / (ddouble)n;
                    ddouble u = v * n;

                    PrecisionAssert.AreEqual(0, m - u, 1e-30);
                    Assert.IsTrue(ddouble.IsRegulared(u));
                    Assert.IsTrue(ddouble.IsRegulared(v));
                }
            }

            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.PositiveInfinity / ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NegativeInfinity / ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(double.NaN / ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN / ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(double.NaN / ddouble.NaN));

            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (int n in new int[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (ddouble)m / (double)n;
                    ddouble u = v * n;

                    PrecisionAssert.AreEqual(0, m - u, 1e-30);
                    Assert.IsTrue(ddouble.IsRegulared(u));
                    Assert.IsTrue(ddouble.IsRegulared(v));
                }
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity / double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity / double.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / double.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / double.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN / double.NaN));

            foreach (int n in new int[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, int.MaxValue,
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, int.MinValue }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                PrecisionAssert.AreEqual(0, 0.25d - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (uint n in new uint[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, uint.MaxValue }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                PrecisionAssert.AreEqual(0, 0.25d - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (long n in new long[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295L, 4294967296L, 4294967297L, long.MaxValue,
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, -4294967295L, -4294967296L, -4294967297L, long.MinValue }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                PrecisionAssert.AreEqual(0, 0.25d - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ulong n in new ulong[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295uL, 4294967296uL, 4294967297uL, ulong.MaxValue }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                PrecisionAssert.AreEqual(0, 0.25d - u, 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ddouble x in new[] { ddouble.Pi, ddouble.E, ddouble.EulerGamma, ddouble.One + ddouble.Ldexp(1, -900) }) {
                Assert.AreEqual(ddouble.One, x / x);
                Assert.AreEqual(ddouble.One, -x / -x);
                Assert.AreEqual(ddouble.MinusOne, -x / x);
                Assert.AreEqual(ddouble.MinusOne, x / -x);

                Assert.IsTrue(ddouble.One < x / (x - 1e-30));
                Assert.IsTrue(ddouble.One > x / (x + 1e-30));
                Assert.IsTrue(ddouble.One > (x - 1e-30) / x);
                Assert.IsTrue(ddouble.One < (x + 1e-30) / x);
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (int y in new int[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x / (ddouble)y, x / y);
                    Assert.AreEqual((ddouble)y / x, y / x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (uint y in new uint[] { 0u, 1u, 32u, 65535u, uint.MaxValue - 1, uint.MaxValue }) {
                    Assert.AreEqual(x / (ddouble)y, x / y);
                    Assert.AreEqual((ddouble)y / x, y / x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (long y in new long[] { long.MinValue, long.MinValue + 1, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue, long.MaxValue - 1, long.MaxValue }) {
                    Assert.AreEqual(x / (ddouble)y, x / y);
                    Assert.AreEqual((ddouble)y / x, y / x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (ulong y in new ulong[] { 0uL, 1uL, 32uL, 65535uL, uint.MaxValue - 1, uint.MaxValue, ulong.MaxValue - 1, ulong.MaxValue }) {
                    Assert.AreEqual(x / (ddouble)y, x / y);
                    Assert.AreEqual((ddouble)y / x, y / x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (double y in new double[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x / (ddouble)y, x / y);
                    Assert.AreEqual((ddouble)y / x, y / x);
                }
            }

            for (ddouble x = ddouble.Ldexp(1, -250); x > 0; x *= 0.15) {
                for (ddouble y = ddouble.Ldexp(1, -250); y > 0; y *= 0.17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsPositive(x / y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, -250); x < 0; x *= 0.15) {
                for (ddouble y = ddouble.Ldexp(1, -250); y > 0; y *= 0.17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsNegative(x / y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, -250); x > 0; x *= 0.15) {
                for (ddouble y = -ddouble.Ldexp(1, -250); y < 0; y *= 0.17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsNegative(x / y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, -250); x < 0; x *= 0.15) {
                for (ddouble y = -ddouble.Ldexp(1, -250); y < 0; y *= 0.17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsPositive(x / y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsPositive(x / y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsNegative(x / y));
                }
            }

            for (ddouble x = ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = -ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsNegative(x / y));
                }
            }

            for (ddouble x = -ddouble.Ldexp(1, 250); ddouble.IsFinite(x); x *= 15) {
                for (ddouble y = -ddouble.Ldexp(1, 250); ddouble.IsFinite(y); y *= 17) {
                    Assert.IsTrue(!ddouble.IsFinite(x / y) || ddouble.IsPositive(x / y));
                }
            }
        }

        [TestMethod]
        public void RemTest() {
            foreach (double m in new double[] { -0d, -1, -2, -3, -5, -6, -7, -9, -10, 0d, 1, 2, 3, 5, 6, 7, 9, 10 }) {
                foreach (double n in new double[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (ddouble)m % (ddouble)n;
                    double u = m % n;

                    Console.WriteLine($"{(double.CopySign(1, m) >= 0 ? m : "-" + double.Abs(m))} % {(double.CopySign(1, n) >= 0 ? n : "-" + double.Abs(n))}");

                    Console.WriteLine($"= sign {ddouble.Sign(v)}");
                    Console.WriteLine($"= sign {double.CopySign(1, u)}");

                    PrecisionAssert.AreEqual(0, u - v, 1e-30);
                    Assert.AreEqual(double.IsPositive(u), ddouble.IsPositive(v));
                    Assert.IsTrue(ddouble.IsRegulared(v));

                    if (m == 0) {
                        continue;
                    }

                    ddouble mdec = ddouble.BitDecrement(m), minc = ddouble.BitIncrement(m);
                    ddouble ndec = ddouble.BitDecrement(n), ninc = ddouble.BitIncrement(n);

                    Assert.AreEqual(double.IsPositive(u), ddouble.IsPositive(mdec % ndec));
                    Assert.AreEqual(double.IsPositive(u), ddouble.IsPositive(mdec % ninc));
                    Assert.AreEqual(double.IsPositive(u), ddouble.IsPositive(minc % ndec));
                    Assert.AreEqual(double.IsPositive(u), ddouble.IsPositive(minc % ninc));

                    ddouble dd = mdec % ndec;
                    ddouble di = mdec % ninc;
                    ddouble id = minc % ndec;
                    ddouble ii = minc % ninc;

                    Assert.IsTrue(ddouble.Abs(dd) < ddouble.Abs(ndec));
                    Assert.IsTrue(ddouble.Abs(di) < ddouble.Abs(ninc));
                    Assert.IsTrue(ddouble.Abs(id) < ddouble.Abs(ndec));
                    Assert.IsTrue(ddouble.Abs(ii) < ddouble.Abs(ninc));
                }
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity % ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity % ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity % ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity % ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.PositiveInfinity % ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NegativeInfinity % ddouble.NaN));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN % ddouble.PositiveInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN % ddouble.NegativeInfinity));
            Assert.IsTrue(ddouble.IsNaN(ddouble.NaN % ddouble.NaN));

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (int y in new int[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x % (ddouble)y, x % y);
                    Assert.AreEqual((ddouble)y % x, y % x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (uint y in new uint[] { 0u, 1u, 32u, 65535u, uint.MaxValue - 1, uint.MaxValue }) {
                    Assert.AreEqual(x % (ddouble)y, x % y);
                    Assert.AreEqual((ddouble)y % x, y % x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (long y in new long[] { long.MinValue, long.MinValue + 1, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue, long.MaxValue - 1, long.MaxValue }) {
                    Assert.AreEqual(x % (ddouble)y, x % y);
                    Assert.AreEqual((ddouble)y % x, y % x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (ulong y in new ulong[] { 0uL, 1uL, 32uL, 65535uL, uint.MaxValue - 1, uint.MaxValue, ulong.MaxValue - 1, ulong.MaxValue }) {
                    Assert.AreEqual(x % (ddouble)y, x % y);
                    Assert.AreEqual((ddouble)y % x, y % x);
                }
            }

            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                foreach (double y in new double[] { int.MinValue, int.MinValue + 1, -65535, -32, 0, 1, 32, 65535, int.MaxValue - 1, int.MaxValue }) {
                    Assert.AreEqual(x % (ddouble)y, x % y);
                    Assert.AreEqual((ddouble)y % x, y % x);
                }
            }
        }

        [TestMethod]
        public void IncrementTest() {
            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                ddouble y = x;
                y++;

                Assert.AreEqual(x + 1d, y);
                Assert.AreEqual(x + 2d, ++y);
            }

            checked {
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = ddouble.Ldexp(1, 103);
                    x++;
                });
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 49);
                    x++;
                });
                {
                    ddouble x = ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 50);
                    x++;
                }

                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = -ddouble.Ldexp(1, 103);
                    x++;
                });
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = -(ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 49));
                    x++;
                });
                {
                    ddouble x = -(ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 50));
                    x++;
                }
            }
        }

        [TestMethod]
        public void DecrementTest() {
            foreach (ddouble x in new ddouble[] { long.MinValue, int.MinValue, -10, 1, ddouble.Pi, 65535, 65536, 65537, int.MaxValue, long.MaxValue }) {
                ddouble y = x;
                y--;

                Assert.AreEqual(x - 1d, y);
                Assert.AreEqual(x - 2d, --y);
            }

            checked {
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = ddouble.Ldexp(1, 103);
                    x--;
                });
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 49);
                    x--;
                });
                {
                    ddouble x = ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 50);
                    x--;
                }

                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = -ddouble.Ldexp(1, 103);
                    x--;
                });
                Assert.ThrowsExactly<ArithmeticException>(() => {
                    ddouble x = -(ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 49));
                    x--;
                });
                {
                    ddouble x = -(ddouble.Ldexp(1, 103) - ddouble.Ldexp(1, 50));
                    x--;
                }
            }
        }
    }
}
