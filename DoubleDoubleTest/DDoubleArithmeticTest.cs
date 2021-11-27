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
