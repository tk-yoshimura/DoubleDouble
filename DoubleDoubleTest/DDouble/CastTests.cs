using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class CastTests {
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

            Assert.AreEqual((ddouble)0, ((ddouble)long.MaxValue) - ddouble.Floor((ddouble)long.MaxValue));
            Assert.AreEqual((ddouble)0, ((ddouble)long.MinValue) - ddouble.Floor((ddouble)long.MinValue));

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

            Assert.AreEqual((ddouble)0, ((ddouble)ulong.MaxValue) - ddouble.Floor((ddouble)ulong.MaxValue));
            Assert.AreEqual((ddouble)0, ((ddouble)ulong.MinValue) - ddouble.Floor((ddouble)ulong.MinValue));

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

            Assert.AreEqual((ddouble)0, ((ddouble)int.MaxValue) - ddouble.Floor((ddouble)int.MaxValue));
            Assert.AreEqual((ddouble)0, ((ddouble)int.MinValue) - ddouble.Floor((ddouble)int.MinValue));

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

            Assert.AreEqual((ddouble)int.MaxValue, (ddouble)(double)int.MaxValue);
            Assert.AreEqual((ddouble)int.MinValue, (ddouble)(double)int.MinValue);

            Random random = new Random(1234);
            for (int i = 0; i < 4096; i++) {
                int n = random.Next();

                Assert.AreEqual((ddouble)(n), (ddouble)(double)(n), (n).ToString());
                Assert.AreEqual((ddouble)(-n), (ddouble)(double)(-n), (-n).ToString());
            }
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

            Assert.AreEqual((ddouble)0, ((ddouble)uint.MaxValue) - ddouble.Floor((ddouble)uint.MaxValue));
            Assert.AreEqual((ddouble)0, ((ddouble)uint.MinValue) - ddouble.Floor((ddouble)uint.MinValue));

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
            for (decimal x = -0.01m; x <= 0.01m; x += 0.00001m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(y);
                Assert.AreEqual(x, z, x.ToString());
                Assert.IsTrue(y.ToString().Length <= 8);

            }

            for (decimal x = -10.00m; x <= 10.00m; x += 0.01m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(y);
                Assert.AreEqual(x, z, x.ToString());
                Assert.IsTrue(y.ToString().Length <= 5);
            }

            for (decimal x = -10000m; x <= 10000m; x += 10m) {
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(y);
                Assert.AreEqual(x, z, x.ToString());
                Assert.IsTrue(y.ToString().Length <= 6);
            }

            for (int i = 1; i <= 1000; i++) {
                decimal x = 1m / i;
                ddouble y = (ddouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(x);
                Console.WriteLine(y);

                Assert.AreEqual(x, z, x.ToString());
            }

            double v = 1;
            decimal d = 1;
            for (int i = 0; i < 24; i++) {
                ddouble x = (ddouble)d;
                ddouble zero = x - v;

                Assert.IsTrue(ddouble.IsZero(zero), $"{d}, {v}, {zero}");

                v /= 2;
                d /= 2;
            }
        }

        [TestMethod]
        public void Bits128Test() {
            ddouble v1 = (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL);
            ddouble v2 = (+1, 0, 0x8000000000000000uL, 0x0000000000800000uL);
            ddouble v3 = (+1, 0, 0x8000000000000000uL, 0x00000000007FFFFFuL);
            ddouble v4 = (+1, +1, 0x8000000000000000uL, 0x0000000000000000uL);
            ddouble v5 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL);
            ddouble v6 = (-1, 0, 0x8000000000000000uL, 0x0000000000000000uL);
            ddouble v7 = (+1, 0, 0xC90FDAA22168C234uL, 0xC4C6628B80DC1CD1uL);
            ddouble v8 = (+1, 0, 0x8000000000000000uL, 0x00000000003FFFFFuL);
            ddouble v9 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF3FFFFFuL);
            ddouble v10 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF4FFFFFuL);
            ddouble v11 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF8FFFFFuL);
            ddouble v12 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFCFFFFFuL);
            ddouble v13 = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFFFFFFFFuL);

            BigInteger n = ((((BigInteger)(0xC90FDAA22168C234uL) << 40) + 0xC4C6628B80uL) << 1) + 2;
            BigInteger m = FloatSplitter.Split(v7).mantissa;

            Assert.AreEqual(n, m);
            Assert.AreEqual(1, FloatSplitter.Split(v2).mantissa - FloatSplitter.Split(v1).mantissa);

            Assert.AreEqual(1, v1);
            Assert.AreNotEqual(v2, v1);
            Assert.AreEqual(v2, v3);
            Assert.AreEqual(2, v4);
            Assert.AreNotEqual(v1, v5);
            Assert.AreEqual(-1, v6);
            Assert.AreEqual(1, v8);
            Assert.AreNotEqual(1, v9);
            Assert.AreNotEqual(1, v10);
            Assert.AreNotEqual(1, v11);
            Assert.AreEqual(1, v12);
            Assert.AreEqual(1, v13);
        }
    }
}
