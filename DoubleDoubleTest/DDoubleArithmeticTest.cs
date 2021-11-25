using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleArithmeticTest {
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
        }
    }
}
