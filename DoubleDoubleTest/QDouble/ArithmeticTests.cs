using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class ArithmeticTests {
        [TestMethod]
        public void AddTest() {
            foreach (int n in new int[] { 7, 13, 17 }) {
                qdouble u = qdouble.Rcp(n);
                ddouble v = ddouble.Rcp(n);

                qdouble su = 0;
                ddouble sv = 0;

                for (int i = 0; i < n * 100; i++) {
                    su += u;
                    sv += v;
                }

                Assert.IsTrue(qdouble.Abs(100 - su) <= qdouble.Abs(100 - sv));
                HPAssert.AreEqual(100, (ddouble)su, 1e-60);
            }

            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity + qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity + qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity + qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity + qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity + qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity + qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN + qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN + qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN + qdouble.NaN));
        }

        [TestMethod]
        public void SubTest() {
            foreach (int n in new int[] { 7, 13, 17 }) {
                qdouble u = qdouble.Rcp(n);
                ddouble v = ddouble.Rcp(n);

                qdouble su = 0;
                ddouble sv = 0;

                for (int i = 0; i < n * 100; i++) {
                    su -= u;
                    sv -= v;
                }

                Assert.IsTrue(ddouble.Abs(-100 - (ddouble)su) <= ddouble.Abs(-100 - sv));
                HPAssert.AreEqual(-100, (ddouble)su, 1e-60);
            }

            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity - qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity - qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity - qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity - qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity - qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity - qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN - qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN - qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN - qdouble.NaN));
        }

        [TestMethod]
        public void MulTest() {
            qdouble.IsMinusZero(qdouble.PlusZero * qdouble.MinusZero);

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                qdouble u = (qdouble.Rcp(n)) * (qdouble)n;
                ddouble v = ((ddouble)(1d) / n) * n;

                Assert.IsTrue(ddouble.Abs((ddouble)(1 - u)) <= ddouble.Abs(1 - v));
                HPAssert.AreEqual(0, (ddouble)(1 - u), 1e-60);
            }

            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity * qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity * qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * qdouble.NaN));

            Assert.IsTrue(qdouble.IsPlusZero(qdouble.PlusZero * qdouble.PlusZero));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.PlusZero * qdouble.MinusZero));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.MinusZero * qdouble.PlusZero));
            Assert.IsTrue(qdouble.IsPlusZero(qdouble.MinusZero * qdouble.MinusZero));

            Assert.IsTrue(qdouble.IsPlusZero(qdouble.PlusZero * 1d));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.MinusZero * 1d));

            Assert.IsTrue(qdouble.IsPlusZero(1d * qdouble.PlusZero));
            Assert.IsTrue(qdouble.IsMinusZero(1d * qdouble.MinusZero));

            Assert.IsTrue(qdouble.IsPlusZero(qdouble.PlusZero * qdouble.PI));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.MinusZero * qdouble.PI));

            Assert.IsTrue(qdouble.IsPlusZero(qdouble.PI * qdouble.PlusZero));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.PI * qdouble.MinusZero));

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                qdouble u = (qdouble.Rcp(n)) * (ddouble)n;
                ddouble v = (1d / n) * n;

                Assert.IsTrue(ddouble.Abs((ddouble)(1 - u)) <= ddouble.Abs(1 - v));
                HPAssert.AreEqual(0, (ddouble)(1 - u), 1e-60);
            }

            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.PositiveInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity * ddouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(qdouble.NegativeInfinity * ddouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity * ddouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity * ddouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * ddouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * ddouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN * ddouble.NaN));

            foreach (int n in new int[] { -7, -13, -17, -257, 7, 13, 17, 257 }) {
                qdouble u = (ddouble)n * (qdouble.Rcp(n));
                ddouble v = ((ddouble)(1d) / n) * n;

                Assert.IsTrue(ddouble.Abs((ddouble)(1 - u)) <= ddouble.Abs(1 - v));
                HPAssert.AreEqual(0, (ddouble)(1 - u), 1e-60);
            }

            Assert.IsTrue(qdouble.IsInfinity(ddouble.PositiveInfinity * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(ddouble.PositiveInfinity * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsInfinity(ddouble.NegativeInfinity * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsInfinity(ddouble.NegativeInfinity * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(ddouble.PositiveInfinity * qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(ddouble.NegativeInfinity * qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(ddouble.NaN * qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(ddouble.NaN * qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(ddouble.NaN * qdouble.NaN));
        }

        [TestMethod]
        public void DivTest() {
            foreach (int m in new int[] { -1, -3, -5, -7, -9, 1, 3, 5, 7, 9 }) {
                foreach (int n in new int[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    qdouble v = (qdouble)m / (qdouble)n;
                    qdouble u = v * n;

                    HPAssert.AreEqual(0, (ddouble)(m - u), 1e-60);
                }
            }

            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity / qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity / qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity / qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity / qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity / qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity / qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN / qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN / qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN / qdouble.NaN));

            Assert.IsTrue(qdouble.IsPlusZero(qdouble.PlusZero / 1));
            Assert.IsTrue(qdouble.IsMinusZero(qdouble.MinusZero / 1));
        }

        [TestMethod]
        public void RemTest() {
            foreach (ddouble m in new ddouble[] { -0d, -1, -2, -3, -5, -6, -7, -9, -10, 0d, 1, 2, 3, 5, 6, 7, 9, 10 }) {
                foreach (ddouble n in new ddouble[] { -3, -7, -13, -17, -257, 3, 7, 13, 17, 257 }) {
                    qdouble v = (qdouble)m % (qdouble)n;
                    ddouble u = m % n;

                    Console.WriteLine($"{(m.Sign >= 0 ? m : "-" + ddouble.Abs(m))} % {(n.Sign >= 0 ? n : "-" + ddouble.Abs(n))}");

                    Console.WriteLine($"= sign {v.Sign}");
                    Console.WriteLine($"= sign {u.Sign}");

                    HPAssert.AreEqual(0, u - (ddouble)v, 1e-60);
                    Assert.AreEqual(u.Sign, v.Sign);

                    if (m == 0) {
                        continue;
                    }

                    qdouble mdec = qdouble.BitDecrement(m), minc = qdouble.BitIncrement(m);
                    qdouble ndec = qdouble.BitDecrement(n), ninc = qdouble.BitIncrement(n);

                    Assert.AreEqual(u.Sign, (mdec % ndec).Sign);
                    Assert.AreEqual(u.Sign, (mdec % ninc).Sign);
                    Assert.AreEqual(u.Sign, (minc % ndec).Sign);
                    Assert.AreEqual(u.Sign, (minc % ninc).Sign);

                    qdouble dd = mdec % ndec;
                    qdouble di = mdec % ninc;
                    qdouble id = minc % ndec;
                    qdouble ii = minc % ninc;

                    Assert.IsTrue(qdouble.Abs(dd) < qdouble.Abs(ndec));
                    Assert.IsTrue(qdouble.Abs(di) < qdouble.Abs(ninc));
                    Assert.IsTrue(qdouble.Abs(id) < qdouble.Abs(ndec));
                    Assert.IsTrue(qdouble.Abs(ii) < qdouble.Abs(ninc));
                }
            }

            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity % qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity % qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity % qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity % qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.PositiveInfinity % qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NegativeInfinity % qdouble.NaN));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN % qdouble.PositiveInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN % qdouble.NegativeInfinity));
            Assert.IsTrue(qdouble.IsNaN(qdouble.NaN % qdouble.NaN));
        }
    }
}
