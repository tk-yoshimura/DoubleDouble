using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class UInt128Test {
        static readonly BigInteger maxvalue = (((BigInteger)UInt64.MaxValue) << 64) + UInt64.MaxValue;
        static readonly ReadOnlyCollection<BigInteger> testcases;

        static UInt128Test() {
            List<BigInteger> vs = new();
            BigInteger v;

            v = maxvalue;
            while (v > 0) {
                vs.Add(v);
                v /= 2;
                vs.Add(v + 1);
                vs.Add(v + 2);
            }
            v = maxvalue - 1;
            while (v > 0) {
                vs.Add(v);
                v /= 3;
            }
            v = maxvalue - 3;
            while (v > 0) {
                vs.Add(v);
                v /= 7;
            }
            v = maxvalue - 5;
            while (v > 0) {
                vs.Add(v);
                v /= 17;
            }
            v = maxvalue - 7;
            while (v > 0) {
                vs.Add(v);
                v /= 23;
            }
            v = maxvalue / 8;
            while (v > 0) {
                vs.Add(v + 2);
                vs.Add(v + 3);
                vs.Add(v + 5);
                v /= 2;
            }
            v = BigInteger.Parse("12345678901234567890123456789012345678");
            while (v > 0) {
                vs.Add(v);
                v /= 10;
            }
            v = BigInteger.Parse("10000000000000000000000000000000000000");
            while (v > 0) {
                vs.Add(v);
                v /= 10;
            }
            v = BigInteger.Parse("98765432109876543210987654321098765432");
            while (v > 0) {
                vs.Add(v);
                v /= 10;
            }
            v = BigInteger.Parse("31415926535897932384626433832795028842");
            while (v > 0) {
                vs.Add(v);
                v /= 10;
            }
            vs.Add(0);

            testcases = Array.AsReadOnly(vs.ToArray());
        }

        [TestMethod]
        public void CreateTest() {
            UInt128 v1 = new(1u, 2u, 3u, 4u);
            UInt128 v2 = new(0x0000000500000006uL, 0x0000000700000008uL);
            UInt128 v3 = UInt128.MaxValue;

            Assert.AreEqual(1u, v1.E3);
            Assert.AreEqual(2u, v1.E2);
            Assert.AreEqual(3u, v1.E1);
            Assert.AreEqual(4u, v1.E0);
            Assert.AreEqual(5u, v2.E3);
            Assert.AreEqual(6u, v2.E2);
            Assert.AreEqual(7u, v2.E1);
            Assert.AreEqual(8u, v2.E0);

            Assert.AreEqual(0x0000000100000002uL, v1.Hi);
            Assert.AreEqual(0x0000000300000004uL, v1.Lo);
            Assert.AreEqual(0x0000000500000006uL, v2.Hi);
            Assert.AreEqual(0x0000000700000008uL, v2.Lo);
            Assert.AreEqual(0xFFFFFFFFFFFFFFFFuL, v3.Hi);
            Assert.AreEqual(0xFFFFFFFFFFFFFFFFuL, v3.Lo);
        }

        [TestMethod]
        public void MaxDigitTest() {
            Assert.ThrowsException<OverflowException>(() => {
                UInt128 _ = UInt128.MaxDigit * 10;
            }, $"maxdigit * 10");

            Assert.AreEqual(UInt128.MaxDigit.ToString().Length, UInt128.MaxValueDigits);
        }

        [TestMethod]
        public void BigIntegarTest() {
            BigInteger v = (((BigInteger)UInt64.MaxValue) << 64) + UInt64.MaxValue;
            BigInteger mask = UInt64.MaxValue;

            while (v > 0) {
                UInt128 u = new((UInt64)((v >> 64) & mask), (UInt64)(v & mask));
                UInt128 w = u.ToString();

                Console.WriteLine(v);

                Assert.AreEqual(v.ToString(), u.ToString());
                Assert.AreEqual(w, u);

                v /= 3;
            }
        }

        [TestMethod]
        public void ToStringTest() {
            UInt128 v0 = new(0uL, 0uL);
            UInt128 v1 = new(0uL, 1uL);
            UInt128 v2 = new(0uL, 10uL);
            UInt128 v3 = new(0uL, 100uL);
            UInt128 v4 = new(0uL, 10000000000000uL);
            UInt128 v5 = new(0x00000000033b2e3cuL, 0x9fd0803ce8000000uL);
            UInt128 v6 = new(0x0001ed09bead87c0uL, 0x378d8e6400000000uL);
            UInt128 v7 = new(UInt64.MaxValue, UInt64.MaxValue);

            Assert.AreEqual("0", v0.ToString());
            Assert.AreEqual("1", v1.ToString());
            Assert.AreEqual("10", v2.ToString());
            Assert.AreEqual("100", v3.ToString());
            Assert.AreEqual("10000000000000", v4.ToString());
            Assert.AreEqual("1000000000000000000000000000", v5.ToString());
            Assert.AreEqual("10000000000000000000000000000000000", v6.ToString());
            Assert.AreEqual("340282366920938463463374607431768211455", v7.ToString());

            Assert.AreEqual("0", v0.ToString("D"));
            Assert.AreEqual("00", v0.ToString("D2"));
            Assert.AreEqual("1", v1.ToString("D"));
            Assert.AreEqual("0001", v1.ToString("D4"));
            Assert.AreEqual("100", v3.ToString("d"));
            Assert.AreEqual("100", v3.ToString("d2"));
            Assert.AreEqual("100", v3.ToString("D3"));
            Assert.AreEqual("0100", v3.ToString("D4"));
            Assert.AreEqual("33b2e3c9fd0803ce8000000", v5.ToString("x"));
            Assert.AreEqual("33B2E3C9FD0803CE8000000", v5.ToString("X"));
            Assert.AreEqual("0033b2e3c9fd0803ce8000000", v5.ToString("x25"));
            Assert.AreEqual("00033B2E3C9FD0803CE8000000", v5.ToString("X26"));
            Assert.AreEqual("33b2e3c9fd0803ce8000000", v5.ToString("x22"));
        }

        [TestMethod]
        public void ParseTest() {
            UInt128 v0 = "0";
            UInt128 v1 = "1";
            UInt128 v2 = "10";
            UInt128 v3 = "100";
            UInt128 v4 = "10000000000000";
            UInt128 v5 = "1000000000000000000000000000";
            UInt128 v6 = "10000000000000000000000000000000000";
            UInt128 v7 = "340282366920938463463374607431768211455";

            Assert.AreEqual("0", v0.ToString());
            Assert.AreEqual("1", v1.ToString());
            Assert.AreEqual("10", v2.ToString());
            Assert.AreEqual("100", v3.ToString());
            Assert.AreEqual("10000000000000", v4.ToString());
            Assert.AreEqual("1000000000000000000000000000", v5.ToString());
            Assert.AreEqual("10000000000000000000000000000000000", v6.ToString());
            Assert.AreEqual("340282366920938463463374607431768211455", v7.ToString());

            Assert.ThrowsException<OverflowException>(() => {
                UInt128 v7 = "340282366920938463463374607431768211456";
            });

            Assert.ThrowsException<OverflowException>(() => {
                UInt128 v7 = "3402823669209384634633746074317682114560";
            });

            Assert.ThrowsException<FormatException>(() => {
#pragma warning disable CS8604
                UInt128 v7 = null;
#pragma warning restore CS8604
            });

            Assert.ThrowsException<FormatException>(() => {
                UInt128 v7 = "abc";
            });

            Assert.ThrowsException<FormatException>(() => {
                UInt128 v7 = "";
            });
        }

        [TestMethod]
        public void CmpTest() {
            UInt128 v1 = new(1u, 2u, 3u, 4u);
            UInt128 v2 = new(2u, 2u, 3u, 4u);
            UInt128 v3 = new(1u, 3u, 3u, 4u);
            UInt128 v4 = new(1u, 2u, 4u, 4u);
            UInt128 v5 = new(1u, 2u, 3u, 5u);
            UInt128 v6 = new(1u, 2u, 3u, 4u);
            object obj = null;

            Assert.IsTrue(v1 < v2);
            Assert.IsTrue(v1 < v3);
            Assert.IsTrue(v1 < v4);
            Assert.IsTrue(v1 < v5);
            Assert.IsTrue(v2 > v3);
            Assert.IsTrue(v2 > v4);
            Assert.IsTrue(v2 > v5);
            Assert.IsTrue(v3 > v4);
            Assert.IsTrue(v3 > v5);
            Assert.IsTrue(v4 > v5);

            Assert.IsTrue(v1 <= v2);
            Assert.IsTrue(v1 <= v3);
            Assert.IsTrue(v1 <= v4);
            Assert.IsTrue(v1 <= v5);
            Assert.IsTrue(v2 >= v3);
            Assert.IsTrue(v2 >= v4);
            Assert.IsTrue(v2 >= v5);
            Assert.IsTrue(v3 >= v4);
            Assert.IsTrue(v3 >= v5);
            Assert.IsTrue(v4 >= v5);

            Assert.IsFalse(v1 > v2);
            Assert.IsFalse(v1 > v3);
            Assert.IsFalse(v1 > v4);
            Assert.IsFalse(v1 > v5);
            Assert.IsFalse(v2 < v3);
            Assert.IsFalse(v2 < v4);
            Assert.IsFalse(v2 < v5);
            Assert.IsFalse(v3 < v4);
            Assert.IsFalse(v3 < v5);
            Assert.IsFalse(v4 < v5);

            Assert.IsFalse(v1 >= v2);
            Assert.IsFalse(v1 >= v3);
            Assert.IsFalse(v1 >= v4);
            Assert.IsFalse(v1 >= v5);
            Assert.IsFalse(v2 <= v3);
            Assert.IsFalse(v2 <= v4);
            Assert.IsFalse(v2 <= v5);
            Assert.IsFalse(v3 <= v4);
            Assert.IsFalse(v3 <= v5);
            Assert.IsFalse(v4 <= v5);

            Assert.IsTrue(v1 == v6);
            Assert.IsFalse(v1 != v6);
            Assert.IsTrue(v1 <= v6);
            Assert.IsTrue(v1 >= v6);

            Assert.IsTrue(v1.Equals(v1));
            Assert.IsFalse(v1.Equals(v2));
            Assert.IsTrue(v1.Equals(v6));
            Assert.IsFalse(v1.Equals(obj));

            Assert.AreEqual(-1, v1.CompareTo(v5));
            Assert.AreEqual(0, v1.CompareTo(v6));
            Assert.AreEqual(+1, v2.CompareTo(v3));
        }

        [TestMethod]
        public void RightShiftTest() {
            BigInteger v = BigInteger.Parse("234567891234567891234567891234567891234");

            for (int sft = 0; sft <= 130; sft++) {
                BigInteger u = v >> sft;
                UInt128 w = (UInt128)v.ToString() >> sft;

                Assert.AreEqual(u.ToString(), w.ToString(), $"{sft}");
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                UInt128 b = new(0u, 0u, 0u, 123456u);

                UInt128 c = b >> -1;
            });
        }

        [TestMethod]
        public void LeftShiftTest() {
            BigInteger v1 = BigInteger.Parse("234567891234567891234567891234567891234");
            BigInteger v2 = BigInteger.Parse("2345678912345");

            for (int sft = 0; sft <= 130; sft++) {
                BigInteger u = (v1 >> sft) << sft;
                UInt128 w = (UInt128)(v1 >> sft).ToString() << sft;

                Assert.AreEqual(u.ToString(), w.ToString(), $"{sft}");
            }

            for (int sft = 0; sft <= 130; sft++) {
                BigInteger u = (v2 << sft) & maxvalue;
                UInt128 w = (UInt128)(v2).ToString() << sft;

                Assert.AreEqual(u.ToString(), w.ToString(), $"{sft}");
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                UInt128 b = new(0u, 0u, 0u, 123456u);

                UInt128 c = b << -1;
            });
        }

        [TestMethod]
        public void AddTest() {
            UInt128 v1 = new(~0u, 0u, 0u, 1u), v2 = new(0u, ~0u, ~0u, ~0u - 1u);

            Assert.AreEqual(new UInt128(~0u, ~0u, ~0u, ~0u), v1 + v2);

            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();

                    Console.WriteLine($"{i0} + {i1}");

                    if (testcases[i] + testcases[j] > maxvalue) {
                        Assert.ThrowsException<OverflowException>(() => {
                            UInt128 _ = i0 + i1;
                        }, $"{i0} + {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.Lo + i1;
                            }, $"{i0} + {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.E0 + i1;
                            }, $"{i0} + {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 + i1.Lo;
                            }, $"{i0} + {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 + i1.E0;
                            }, $"{i0} + {i1}");
                        }
                    }
                    else {
                        Assert.AreEqual((testcases[i] + testcases[j]).ToString(), (i0 + i1).ToString(), $"{i0} + {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] + testcases[j]).ToString(), (i0.Lo + i1).ToString(), $"{i0} + {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] + testcases[j]).ToString(), (i0.E0 + i1).ToString(), $"{i0} + {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] + testcases[j]).ToString(), (i0 + i1.Lo).ToString(), $"{i0} + {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] + testcases[j]).ToString(), (i0 + i1.E0).ToString(), $"{i0} + {i1}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SubTest() {
            UInt128 v1 = new(1u, 0u, 0u, 0u), v2 = new(0u, ~0u, ~0u, ~0u), v3 = new(1u, 0u, 0u, 1u);

            Assert.AreEqual((UInt128)1, v1 - v2);

            Assert.ThrowsException<OverflowException>(() => {
                UInt128 _ = v1 - v3;
            });

            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();

                    Console.WriteLine($"{i0} - {i1}");

                    if (i0 < i1) {
                        Assert.ThrowsException<OverflowException>(() => {
                            UInt128 _ = i0 - i1;
                        }, $"{i0} - {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.Lo - i1;
                            }, $"{i0} - {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.E0 - i1;
                            }, $"{i0} - {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 - i1.Lo;
                            }, $"{i0} - {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 - i1.E0;
                            }, $"{i0} - {i1}");
                        }
                    }
                    else {
                        Assert.AreEqual((testcases[i] - testcases[j]).ToString(), (i0 - i1).ToString(), $"{i0} - {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] - testcases[j]).ToString(), (i0.Lo - i1).ToString(), $"{i0} - {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] - testcases[j]).ToString(), (i0.E0 - i1).ToString(), $"{i0} - {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] - testcases[j]).ToString(), (i0 - i1.Lo).ToString(), $"{i0} - {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] - testcases[j]).ToString(), (i0 - i1.E0).ToString(), $"{i0} - {i1}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void MulTest() {
            UInt128 v1 = new(0u, 1u, 0u, 1u), v2 = new(0u, 0u, ~0u, ~0u);

            Assert.AreEqual(new UInt128(~0u, ~0u, ~0u, ~0u), v1 * v2);

            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();

                    Console.WriteLine($"{i0} * {i1}");

                    if (testcases[i] * testcases[j] > maxvalue) {
                        Assert.ThrowsException<OverflowException>(() => {
                            UInt128 _ = i0 * i1;
                        }, $"{i0} * {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.Lo * i1;
                            }, $"{i0} * {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0.E0 * i1;
                            }, $"{i0} * {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 * i1.Lo;
                            }, $"{i0} * {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = i0 * i1.E0;
                            }, $"{i0} * {i1}");
                        }
                    }
                    else {
                        Assert.AreEqual((testcases[i] * testcases[j]).ToString(), (i0 * i1).ToString(), $"{i0} * {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] * testcases[j]).ToString(), (i0.Lo * i1).ToString(), $"{i0} * {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] * testcases[j]).ToString(), (i0.E0 * i1).ToString(), $"{i0} * {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] * testcases[j]).ToString(), (i0 * i1.Lo).ToString(), $"{i0} * {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] * testcases[j]).ToString(), (i0 * i1.E0).ToString(), $"{i0} * {i1}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void DivTest() {
            UInt128 v1 = new(~0u, ~0u, ~0u, ~0u), v2 = new(0u, 0u, 0u, 1u);

            Assert.AreEqual(new UInt128(~0u, ~0u, ~0u, ~0u), v1 / v2);

            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();

                    Console.WriteLine($"{i0} / {i1}");

                    if (testcases[j] <= 0) {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            UInt128 _ = i0 / i1;
                        }, $"{i0} / {i1}");
                    }
                    else {
                        Assert.AreEqual((testcases[i] / testcases[j]).ToString(), (i0 / i1).ToString(), $"{i0} / {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] / testcases[j]).ToString(), (i0.Lo / i1).ToString(), $"{i0} / {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] / testcases[j]).ToString(), (i0.E0 / i1).ToString(), $"{i0} / {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] / testcases[j]).ToString(), (i0 / i1.Lo).ToString(), $"{i0} / {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] / testcases[j]).ToString(), (i0 / i1.E0).ToString(), $"{i0} / {i1}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RemTest() {
            UInt128 v1 = new(~0u, ~0u, ~0u, ~0u), v2 = new(0u, 0u, 0u, 1u);

            Assert.AreEqual(UInt128.Zero, v1 % v2);

            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();

                    Console.WriteLine($"{i0} % {i1}");

                    if (testcases[j] <= 0) {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            UInt128 _ = i0 % i1;
                        }, $"{i0} % {i1}");
                    }
                    else {
                        Assert.AreEqual((testcases[i] % testcases[j]).ToString(), (i0 % i1).ToString(), $"{i0} % {i1}");

                        if (i0 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] % testcases[j]).ToString(), (i0.Lo % i1).ToString(), $"{i0} % {i1}");
                        }
                        if (i0 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] % testcases[j]).ToString(), (i0.E0 % i1).ToString(), $"{i0} % {i1}");
                        }

                        if (i1 <= UInt64.MaxValue) {
                            Assert.AreEqual((testcases[i] % testcases[j]).ToString(), (i0 % i1.Lo).ToString(), $"{i0} % {i1}");
                        }
                        if (i1 <= UInt32.MaxValue) {
                            Assert.AreEqual((testcases[i] % testcases[j]).ToString(), (i0 % i1.E0).ToString(), $"{i0} % {i1}");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ExpandMulTest() {
            for (int i = 0; i < testcases.Count; i++) {
                for (int j = 0; j < testcases.Count; j++) {
                    UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();
                    (UInt128 hi, UInt128 lo) = UInt128.ExpandMul(i0, i1);

                    Console.WriteLine($"{i0} * {i1}");

                    BigInteger m = ((BigInteger)hi << 128) | lo;

                    Assert.AreEqual(testcases[i] * testcases[j], m, $"{i0} * {i1}");
                }
            }
        }

        [TestMethod]
        public void MulShiftTest() {
            foreach (int sft in new int[] { 0, 3, 9, 16, 32, 33, 63, 64, 65, 95, 96, 110, 127, 128 }) {
                for (int i = 0; i < testcases.Count; i += 4) {
                    for (int j = 0; j < testcases.Count; j += 4) {
                        UInt128 i0 = testcases[i].ToString(), i1 = testcases[j].ToString();
                        BigInteger n = (testcases[i] * testcases[j]) >> sft;

                        Console.WriteLine($"{i0} * {i1}");

                        if (n > maxvalue) {
                            Assert.ThrowsException<OverflowException>(() => {
                                UInt128 _ = UInt128.MulShift(i0, i1, sft);
                            }, $"({i0} * {i1}) >> sft");
                        }
                        else {
                            UInt128 v = UInt128.MulShift(i0, i1, sft);

                            Assert.AreEqual(n.ToString(), v.ToString(), $"({i0} * {i1}) >> sft");
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void BitwiseOpTest() {
            UInt128 v1 = new(0x0000001000000020uL, 0x0000003000000040uL);
            UInt128 v2 = new(0x0000000500000006uL, 0x0000000700000008uL);

            Assert.AreEqual(UInt128.Zero, v1 & v2);
            Assert.AreEqual(v1, v1 & v1);
            Assert.AreEqual(v2, v2 & v2);

            Assert.AreEqual(new UInt128(0x0000001500000026uL, 0x0000003700000048uL), v1 | v2);
            Assert.AreEqual(v1, v1 | v1);
            Assert.AreEqual(v2, v2 | v2);

            Assert.AreEqual(new UInt128(~0x0000001000000020uL, ~0x0000003000000040uL), ~v1);

            Assert.AreEqual(UInt128.Zero, v1 ^ v1);
            Assert.AreEqual(UInt128.MaxValue, v1 ^ ~v1);
            Assert.AreEqual(new UInt128(0x0000001500000026uL, 0x0000003700000048uL), v1 ^ v2);
        }
    }
}