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
                ddouble u = (ddouble.Rcp(n)) * n;
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
        }

        [TestMethod]
        public void DivTest() {
            foreach (int m in new int[] { -1, -5, -9, 1, 5, 9 }) {
                foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                    ddouble u = (m / (ddouble)n) * n;

                    Assert.AreEqual(0, (double)(m - u), 1e-30);
                    Assert.IsTrue(ddouble.IsRegulared(u));
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
        }
    }
}
