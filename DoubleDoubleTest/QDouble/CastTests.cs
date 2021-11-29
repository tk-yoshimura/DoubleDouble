using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class CastTests {
        [TestMethod]
        public void BigIntegerTest() {
            Assert.AreEqual(0d, (ddouble)(qdouble)(BigInteger)(0));
            Assert.AreEqual(1d, (ddouble)(qdouble)(BigInteger)(1));
            Assert.AreEqual(-1d, (ddouble)(qdouble)(BigInteger)(-1));
            Assert.AreEqual(1000d, (ddouble)(qdouble)(BigInteger)(1000));
            Assert.AreEqual(-1000d, (ddouble)(qdouble)(BigInteger)(-1000));

            Assert.AreEqual((qdouble)0x0000FFFFFFFFFFFFuL, (qdouble)(BigInteger)(0x0000FFFFFFFFFFFFuL));
            Assert.AreEqual((qdouble)0x0001000000000000uL, (qdouble)(BigInteger)(0x0001000000000000uL));
            Assert.AreEqual((qdouble)0x000FFFFFFFFFFFFFuL, (qdouble)(BigInteger)(0x000FFFFFFFFFFFFFuL));
            Assert.AreEqual((qdouble)0x0010000000000000uL, (qdouble)(BigInteger)(0x0010000000000000uL));
            Assert.AreEqual((qdouble)0x00FFFFFFFFFFFFFFuL, (qdouble)(BigInteger)(0x00FFFFFFFFFFFFFFuL));
            Assert.AreEqual((qdouble)0x0100000000000000uL, (qdouble)(BigInteger)(0x0100000000000000uL));
            Assert.AreEqual((qdouble)0x0FFFFFFFFFFFFFFFuL, (qdouble)(BigInteger)(0x0FFFFFFFFFFFFFFFuL));
            Assert.AreEqual((qdouble)0x1000000000000000uL, (qdouble)(BigInteger)(0x1000000000000000uL));

            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0000FFFFFFFFFFFFuL, 50), (qdouble)((BigInteger)(0x0000FFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0001000000000000uL, 50), (qdouble)((BigInteger)(0x0001000000000000uL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x000FFFFFFFFFFFFFuL, 50), (qdouble)((BigInteger)(0x000FFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0010000000000000uL, 50), (qdouble)((BigInteger)(0x0010000000000000uL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x00FFFFFFFFFFFFFFuL, 50), (qdouble)((BigInteger)(0x00FFFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0100000000000000uL, 50), (qdouble)((BigInteger)(0x0100000000000000uL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0FFFFFFFFFFFFFFFuL, 50), (qdouble)((BigInteger)(0x0FFFFFFFFFFFFFFFuL) << 50));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x1000000000000000uL, 50), (qdouble)((BigInteger)(0x1000000000000000uL) << 50));

            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0000FFFFFFFFFFFFuL, 100), (qdouble)((BigInteger)(0x0000FFFFFFFFFFFFuL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0001000000000000uL, 100), (qdouble)((BigInteger)(0x0001000000000000uL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x000FFFFFFFFFFFFFuL, 100), (qdouble)((BigInteger)(0x000FFFFFFFFFFFFFuL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0010000000000000uL, 100), (qdouble)((BigInteger)(0x0010000000000000uL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x00FFFFFFFFFFFFFFuL, 100), (qdouble)((BigInteger)(0x00FFFFFFFFFFFFFFuL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0100000000000000uL, 100), (qdouble)((BigInteger)(0x0100000000000000uL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0FFFFFFFFFFFFFFFuL, 100), (qdouble)((BigInteger)(0x0FFFFFFFFFFFFFFFuL) << 100));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x1000000000000000uL, 100), (qdouble)((BigInteger)(0x1000000000000000uL) << 100));

            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0000FFFFFFFFFFFFuL, 150), (qdouble)((BigInteger)(0x0000FFFFFFFFFFFFuL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0001000000000000uL, 150), (qdouble)((BigInteger)(0x0001000000000000uL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x000FFFFFFFFFFFFFuL, 150), (qdouble)((BigInteger)(0x000FFFFFFFFFFFFFuL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0010000000000000uL, 150), (qdouble)((BigInteger)(0x0010000000000000uL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x00FFFFFFFFFFFFFFuL, 150), (qdouble)((BigInteger)(0x00FFFFFFFFFFFFFFuL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0100000000000000uL, 150), (qdouble)((BigInteger)(0x0100000000000000uL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x0FFFFFFFFFFFFFFFuL, 150), (qdouble)((BigInteger)(0x0FFFFFFFFFFFFFFFuL) << 150));
            Assert.AreEqual(qdouble.Ldexp((qdouble)0x1000000000000000uL, 150), (qdouble)((BigInteger)(0x1000000000000000uL) << 150));
        }

        [TestMethod]
        public void LongTest() {
            Assert.AreEqual(0d, (ddouble)(qdouble)(0L));
            Assert.AreEqual(1d, (ddouble)(qdouble)(1L));
            Assert.AreEqual(-1d, (ddouble)(qdouble)(-1L));
            Assert.AreEqual(1000d, (ddouble)(qdouble)(1000L));
            Assert.AreEqual(-1000d, (ddouble)(qdouble)(-1000L));

            foreach (long v in new long[] { 0x0000FFFFFFFFFFFFL, 0x000FFFFFFFFFFFFFL, 0x00FFFFFFFFFFFFFFL, 0x0FFFFFFFFFFFFFFFL }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            foreach (long v in new long[] { -0x0000FFFFFFFFFFFFL, -0x000FFFFFFFFFFFFFL, -0x00FFFFFFFFFFFFFFL, -0x0FFFFFFFFFFFFFFFL }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            Assert.AreEqual(long.MaxValue, (long)(qdouble)long.MaxValue);
            Assert.AreEqual(long.MinValue, (long)(qdouble)long.MinValue);

            Assert.AreEqual((qdouble)0, ((qdouble)long.MaxValue) - qdouble.Floor((qdouble)long.MaxValue));
            Assert.AreEqual((qdouble)0, ((qdouble)long.MinValue) - qdouble.Floor((qdouble)long.MinValue));

            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)qdouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)qdouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                long n = (long)qdouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)(((qdouble)long.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                long n = (long)(((qdouble)long.MinValue) - 1);
            });
        }

        [TestMethod]
        public void ULongTest() {
            Assert.AreEqual(0d, (ddouble)(qdouble)(0uL));
            Assert.AreEqual(1d, (ddouble)(qdouble)(1uL));
            Assert.AreEqual(1000d, (ddouble)(qdouble)(1000uL));

            foreach (ulong v in new ulong[] { 0x0000FFFFFFFFFFFFuL, 0x000FFFFFFFFFFFFFuL, 0x00FFFFFFFFFFFFFFuL, 0x0FFFFFFFFFFFFFFFuL }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            Assert.AreEqual(ulong.MaxValue, (ulong)(qdouble)ulong.MaxValue);
            Assert.AreEqual(ulong.MinValue, (ulong)(qdouble)ulong.MinValue);

            Assert.AreEqual((qdouble)0, ((qdouble)ulong.MaxValue) - qdouble.Floor((qdouble)ulong.MaxValue));
            Assert.AreEqual((qdouble)0, ((qdouble)ulong.MinValue) - qdouble.Floor((qdouble)ulong.MinValue));

            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)qdouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)qdouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                ulong n = (ulong)qdouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)(((qdouble)ulong.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                ulong n = (ulong)(((qdouble)ulong.MinValue) - 1);
            });
        }

        [TestMethod]
        public void IntTest() {
            Assert.AreEqual(0d, (ddouble)(qdouble)(0));
            Assert.AreEqual(1d, (ddouble)(qdouble)(1));
            Assert.AreEqual(-1d, (ddouble)(qdouble)(-1));
            Assert.AreEqual(1000d, (ddouble)(qdouble)(1000));
            Assert.AreEqual(-1000d, (ddouble)(qdouble)(-1000));

            foreach (int v in new int[] { 0x0000FFFF, 0x000FFFFF, 0x00FFFFFF, 0x0FFFFFFF }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            foreach (int v in new int[] { -0x0000FFFF, -0x000FFFFF, -0x00FFFFFF, -0x0FFFFFFF }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            Assert.AreEqual(int.MaxValue, (int)(qdouble)int.MaxValue);
            Assert.AreEqual(int.MinValue, (int)(qdouble)int.MinValue);

            Assert.AreEqual((qdouble)0, ((qdouble)int.MaxValue) - qdouble.Floor((qdouble)int.MaxValue));
            Assert.AreEqual((qdouble)0, ((qdouble)int.MinValue) - qdouble.Floor((qdouble)int.MinValue));

            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)qdouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)qdouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                int n = (int)qdouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)(((qdouble)int.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                int n = (int)(((qdouble)int.MinValue) - 1);
            });
        }

        [TestMethod]
        public void UIntTest() {
            Assert.AreEqual(0d, (ddouble)(qdouble)(0u));
            Assert.AreEqual(1d, (ddouble)(qdouble)(1u));
            Assert.AreEqual(1000d, (ddouble)(qdouble)(1000u));

            foreach (int v in new int[] { 0x0000FFFF, 0x000FFFFF, 0x00FFFFFF, 0x0FFFFFFF }) {
                Assert.AreEqual(1d, (qdouble)(v + 1) - (qdouble)v);
                Assert.AreEqual(-1d, (qdouble)(v - 1) - (qdouble)v);
            }

            Assert.AreEqual(uint.MaxValue, (uint)(qdouble)uint.MaxValue);
            Assert.AreEqual(uint.MinValue, (uint)(qdouble)uint.MinValue);

            Assert.AreEqual((qdouble)0, ((qdouble)uint.MaxValue) - qdouble.Floor((qdouble)uint.MaxValue));
            Assert.AreEqual((qdouble)0, ((qdouble)uint.MinValue) - qdouble.Floor((qdouble)uint.MinValue));

            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)qdouble.PositiveInfinity;
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)qdouble.NegativeInfinity;
            });
            Assert.ThrowsException<InvalidCastException>(() => {
                uint n = (uint)qdouble.NaN;
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)(((qdouble)uint.MaxValue) + 1);
            });
            Assert.ThrowsException<OverflowException>(() => {
                uint n = (uint)(((qdouble)uint.MinValue) - 1);
            });
        }

        [TestMethod]
        public void DecimalTest() {
            qdouble w = (qdouble)0.0625m;

            for (decimal x = -0.01m; x <= 0.01m; x += 0.00001m) {
                qdouble y = (qdouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (decimal x = -10.00m; x <= 10.00m; x += 0.01m) {
                qdouble y = (qdouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (decimal x = -10000m; x <= 10000m; x += 10m) {
                qdouble y = (qdouble)x;
                decimal z = (decimal)y;

                Assert.AreEqual(x, z, x.ToString());
            }

            for (int i = 1; i <= 1000; i++) {
                decimal x = 1m / i;
                qdouble y = (qdouble)x;
                decimal z = (decimal)y;

                Console.WriteLine(x);
                Console.WriteLine(y);

                Assert.AreEqual(x, z, x.ToString());
            }

            ddouble v = 1;
            decimal d = 1;
            for (int i = 0; i < 24; i++) {
                qdouble x = (qdouble)d;
                qdouble zero = x - v;

                Assert.IsTrue(qdouble.IsZero(zero), $"{d}, {v}, {zero}");

                v /= 2;
                d /= 2;
            }
        }
    }
}
