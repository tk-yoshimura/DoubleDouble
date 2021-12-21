using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

                Assert.IsTrue(Math.Abs(100 - (double)su) <= Math.Abs(100 - sv));
                Assert.AreEqual(100, (double)su, 1e-30);
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
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue + double.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue + double.MinValue));

            Assert.IsTrue(ddouble.IsPositiveInfinity(ddouble.MaxValue + double.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(ddouble.MinValue + double.MinValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue + double.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue + double.MinValue));

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
            Assert.IsTrue(ddouble.IsZero(double.MinValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(double.MaxValue + ddouble.MinValue));

            Assert.IsTrue(ddouble.IsPositiveInfinity(double.MaxValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(double.MinValue + ddouble.MinValue));
            Assert.IsTrue(ddouble.IsZero(double.MinValue + ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(double.MaxValue + ddouble.MinValue));
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

                Assert.IsTrue(Math.Abs(-100 - (double)su) <= Math.Abs(-100 - sv));
                Assert.AreEqual(-100, (double)su, 1e-30);
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

            Assert.IsTrue(ddouble.IsZero(ddouble.MaxValue - double.MaxValue));
            Assert.IsTrue(ddouble.IsZero(ddouble.MinValue - double.MinValue));
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

            Assert.IsTrue(ddouble.IsZero(double.MaxValue - ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsZero(double.MinValue - ddouble.MinValue));
            Assert.IsTrue(ddouble.IsNegativeInfinity(double.MinValue - ddouble.MaxValue));
            Assert.IsTrue(ddouble.IsPositiveInfinity(double.MaxValue - ddouble.MinValue));
        }

        [TestMethod]
        public void MulTest() {
            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                ddouble u = (ddouble.Rcp(n)) * (ddouble)n;
                double v = (1d / n) * n;

                Assert.IsTrue(Math.Abs((double)(1 - u)) <= Math.Abs(1 - v));
                Assert.AreEqual(0, (double)(1 - u), 1e-30);
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

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PlusZero * ddouble.PI));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.MinusZero * ddouble.PI));

            Assert.IsTrue(ddouble.IsPlusZero(ddouble.PI * ddouble.PlusZero));
            Assert.IsTrue(ddouble.IsMinusZero(ddouble.PI * ddouble.MinusZero));

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                ddouble u = (ddouble.Rcp(n)) * (double)n;
                double v = (1d / n) * n;

                Assert.IsTrue(Math.Abs((double)(1 - u)) <= Math.Abs(1 - v));
                Assert.AreEqual(0, (double)(1 - u), 1e-30);
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

                Assert.IsTrue(Math.Abs((double)(1 - u)) <= Math.Abs(1 - v));
                Assert.AreEqual(0, (double)(1 - u), 1e-30);
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

            foreach (int n in new int[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537 }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                Assert.AreEqual(0, (double)(1 - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (uint n in new uint[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537 }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                Assert.AreEqual(0, (double)(1 - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (long n in new long[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295L, 4294967296L, 4294967297L, 
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, -4294967295L, -4294967296L, -4294967297L, }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                Assert.AreEqual(0, (double)(1 - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ulong n in new ulong[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295uL, 4294967296uL, 4294967297uL }) {

                ddouble u = (ddouble.Rcp(n)) * n;

                Assert.AreEqual(0, (double)(1 - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }
        }

        [TestMethod]
        public void DivTest() {
            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (int n in new int[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (ddouble)m / (ddouble)n;
                    ddouble u = v * n;

                    Assert.AreEqual(0, (double)(m - u), 1e-30);
                    Assert.IsTrue(ddouble.IsRegulared(u));
                    Assert.IsTrue(ddouble.IsRegulared(v));
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

                    Assert.AreEqual(0, (double)(m - u), 1e-30);
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

                    Assert.AreEqual(0, (double)(m - u), 1e-30);
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

            foreach (int n in new int[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537 }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                Assert.AreEqual(0, (double)(0.25d - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (uint n in new uint[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537 }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                Assert.AreEqual(0, (double)(0.25d - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (long n in new long[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295L, 4294967296L, 4294967297L, 
                                          -1, -2, -3, -4, -5, -15, -16, -17, -255, -256, -257, -65535, -65536, -65537, -4294967295L, -4294967296L, -4294967297L, }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                Assert.AreEqual(0, (double)(0.25d - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            foreach (ulong n in new ulong[] { 1, 2, 3, 4, 5, 15, 16, 17, 255, 256, 257, 65535, 65536, 65537, 4294967295uL, 4294967296uL, 4294967297uL }) {

                ddouble u = ((ddouble)0.25d / n) * n;

                Assert.AreEqual(0, (double)(0.25d - u), 1e-30);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }
        }

        [TestMethod]
        public void RemTest() {
            foreach (double m in new double[] { -0d, -1, -2, -3, -5, -6, -7, -9, -10, 0d, 1, 2, 3, 5, 6, 7, 9, 10 }) {
                foreach (double n in new double[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    ddouble v = (ddouble)m % (ddouble)n;
                    double u = m % n;

                    Console.WriteLine($"{(Math.CopySign(1, m) >= 0 ? m : "-" + Math.Abs(m))} % {(Math.CopySign(1, n) >= 0 ? n : "-" + Math.Abs(n))}");

                    Console.WriteLine($"= sign {v.Sign}");
                    Console.WriteLine($"= sign {Math.CopySign(1, u)}");

                    Assert.AreEqual(0, u - (double)v, 1e-30);
                    Assert.AreEqual((int)Math.CopySign(1, u), v.Sign);
                    Assert.IsTrue(ddouble.IsRegulared(v));

                    if (m == 0) {
                        continue;
                    }

                    ddouble mdec = ddouble.BitDecrement(m), minc = ddouble.BitIncrement(m);
                    ddouble ndec = ddouble.BitDecrement(n), ninc = ddouble.BitIncrement(n);

                    Assert.AreEqual((int)Math.CopySign(1, u), (mdec % ndec).Sign);
                    Assert.AreEqual((int)Math.CopySign(1, u), (mdec % ninc).Sign);
                    Assert.AreEqual((int)Math.CopySign(1, u), (minc % ndec).Sign);
                    Assert.AreEqual((int)Math.CopySign(1, u), (minc % ninc).Sign);

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
        }
    }
}
