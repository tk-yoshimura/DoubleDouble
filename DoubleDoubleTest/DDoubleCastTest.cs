using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleCastTest {
        [TestMethod]
        public void BigIntegerTest() {
            Assert.AreEqual(0d, (double)(ddouble)(BigInteger)(0));
            Assert.AreEqual(1d, (double)(ddouble)(BigInteger)(1));
            Assert.AreEqual(-1d, (double)(ddouble)(BigInteger)(-1));
            Assert.AreEqual(1000d, (double)(ddouble)(BigInteger)(1000));
            Assert.AreEqual(-1000d, (double)(ddouble)(BigInteger)(-1000));

            Assert.AreEqual((ddouble)0x0000FFFFFFFFFFFFuL, (ddouble)(BigInteger)(0x0000FFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0001000000000000uL, (ddouble)(BigInteger)(0x0001000000000000uL));
            Assert.AreEqual((ddouble)0x000FFFFFFFFFFFFFuL, (ddouble)(BigInteger)(0x000FFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0010000000000000uL, (ddouble)(BigInteger)(0x0010000000000000uL));
            Assert.AreEqual((ddouble)0x00FFFFFFFFFFFFFFuL, (ddouble)(BigInteger)(0x00FFFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x0100000000000000uL, (ddouble)(BigInteger)(0x0100000000000000uL));
            Assert.AreEqual((ddouble)0x0FFFFFFFFFFFFFFFuL, (ddouble)(BigInteger)(0x0FFFFFFFFFFFFFFFuL));
            Assert.AreEqual((ddouble)0x1000000000000000uL, (ddouble)(BigInteger)(0x1000000000000000uL));

            Assert.AreEqual(ddouble.Ldexp((ddouble)0x0000FFFFFFFFFFFFuL, 50), (ddouble)((BigInteger)(0x0000FFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x0001000000000000uL, 50), (ddouble)((BigInteger)(0x0001000000000000uL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x000FFFFFFFFFFFFFuL, 50), (ddouble)((BigInteger)(0x000FFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x0010000000000000uL, 50), (ddouble)((BigInteger)(0x0010000000000000uL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x00FFFFFFFFFFFFFFuL, 50), (ddouble)((BigInteger)(0x00FFFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x0100000000000000uL, 50), (ddouble)((BigInteger)(0x0100000000000000uL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x0FFFFFFFFFFFFFFFuL, 50), (ddouble)((BigInteger)(0x0FFFFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(ddouble.Ldexp((ddouble)0x1000000000000000uL, 50), (ddouble)((BigInteger)(0x1000000000000000uL) << 50));
        }

        [TestMethod]
        public void LongTest() {
            Assert.AreEqual(0d, (double)(ddouble)(0L));
            Assert.AreEqual(1d, (double)(ddouble)(1L));
            Assert.AreEqual(-1d, (double)(ddouble)(-1L));
            Assert.AreEqual(1000d, (double)(ddouble)(1000L));
            Assert.AreEqual(-1000d, (double)(ddouble)(-1000L));

            foreach (long v in new long[] { 0x0000FFFFFFFFFFFFL, 0x000FFFFFFFFFFFFFL, 0x00FFFFFFFFFFFFFFL, 0x0FFFFFFFFFFFFFFFL }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            foreach (long v in new long[] { -0x0000FFFFFFFFFFFFL, -0x000FFFFFFFFFFFFFL, -0x00FFFFFFFFFFFFFFL, -0x0FFFFFFFFFFFFFFFL }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            Assert.AreEqual(long.MaxValue, (long)(ddouble)long.MaxValue);
            Assert.AreEqual(long.MinValue, (long)(ddouble)long.MinValue);

            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)ddouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)ddouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                long n = (long)ddouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)(((ddouble)long.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)(((ddouble)long.MinValue) - 1);
            });
        }

        [TestMethod]
        public void ULongTest() {
            Assert.AreEqual(0d, (double)(ddouble)(0uL));
            Assert.AreEqual(1d, (double)(ddouble)(1uL));
            Assert.AreEqual(1000d, (double)(ddouble)(1000uL));

            foreach (ulong v in new ulong[] { 0x0000FFFFFFFFFFFFuL, 0x000FFFFFFFFFFFFFuL, 0x00FFFFFFFFFFFFFFuL, 0x0FFFFFFFFFFFFFFFuL }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            Assert.AreEqual(ulong.MaxValue, (ulong)(ddouble)ulong.MaxValue);
            Assert.AreEqual(ulong.MinValue, (ulong)(ddouble)ulong.MinValue);

            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)ddouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)ddouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                ulong n = (ulong)ddouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)(((ddouble)ulong.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)(((ddouble)ulong.MinValue) - 1);
            });
        }

        [TestMethod]
        public void IntTest() {
            Assert.AreEqual(0d, (double)(ddouble)(0));
            Assert.AreEqual(1d, (double)(ddouble)(1));
            Assert.AreEqual(-1d, (double)(ddouble)(-1));
            Assert.AreEqual(1000d, (double)(ddouble)(1000));
            Assert.AreEqual(-1000d, (double)(ddouble)(-1000));

            foreach (int v in new int[] { 0x0000FFFF, 0x000FFFFF, 0x00FFFFFF, 0x0FFFFFFF }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            foreach (int v in new int[] { -0x0000FFFF, -0x000FFFFF, -0x00FFFFFF, -0x0FFFFFFF }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            Assert.AreEqual(int.MaxValue, (int)(ddouble)int.MaxValue);
            Assert.AreEqual(int.MinValue, (int)(ddouble)int.MinValue);

            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)ddouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)ddouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                int n = (int)ddouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)(((ddouble)int.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)(((ddouble)int.MinValue) - 1);
            });
        }

        [TestMethod]
        public void UIntTest() {
            Assert.AreEqual(0d, (double)(ddouble)(0u));
            Assert.AreEqual(1d, (double)(ddouble)(1u));
            Assert.AreEqual(1000d, (double)(ddouble)(1000u));

            foreach (int v in new int[] { 0x0000FFFF, 0x000FFFFF, 0x00FFFFFF, 0x0FFFFFFF }) {
                Assert.AreEqual(1d, (ddouble)(v + 1) - (ddouble)v);
                Assert.AreEqual(-1d, (ddouble)(v - 1) - (ddouble)v);
            }

            Assert.AreEqual(uint.MaxValue, (uint)(ddouble)uint.MaxValue);
            Assert.AreEqual(uint.MinValue, (uint)(ddouble)uint.MinValue);

            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)ddouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)ddouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                uint n = (uint)ddouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)(((ddouble)uint.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)(((ddouble)uint.MinValue) - 1);
            });
        }

        [TestMethod]
        public void DecimalTest() {
            for (decimal x = 0.01m; x <= 0.01m; x += 0.00001m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (decimal x = 10m; x <= 10m; x += 0.01m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (decimal x = 10000m; x <= 10000m; x += 10m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (int i = 1; i <= 1000; i++) {
                decimal x = 1m / i;
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(x);
                Console.WriteLine(y);

                Assert.AreEqual(x, z, x.ToString());
            }
        }
    }
}
