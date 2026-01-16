using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;
using System.Numerics;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class StringTests {
        [TestMethod]
        public void ToStringTest() {
            Assert.AreEqual("0", (ddouble)(0).ToString());
            Assert.AreEqual("-0", (ddouble)(-0d).ToString());

            Assert.AreEqual("0.3333333333333333333333333333333", ddouble.Rcp(3).ToString());
            Assert.AreEqual("3.333333333333333333333333333333", (ddouble.Rcp(3) * 10).ToString());
            Assert.AreEqual("0.2", ddouble.Rcp(5).ToString());
            Assert.AreEqual("2", (ddouble.Rcp(5) * 10).ToString());
            Assert.AreEqual("0.1666666666666666666666666666667", ddouble.Rcp(6).ToString());
            Assert.AreEqual("1.666666666666666666666666666667", (ddouble.Rcp(6) * 10).ToString());
            Assert.AreEqual("0.1428571428571428571428571428571", ddouble.Rcp(7).ToString());
            Assert.AreEqual("1.428571428571428571428571428571", (ddouble.Rcp(7) * 10).ToString());

            Assert.AreEqual("-0.3333333333333333333333333333333", (-ddouble.Rcp(3)).ToString());
            Assert.AreEqual("-3.333333333333333333333333333333", (-ddouble.Rcp(3) * 10).ToString());
            Assert.AreEqual("-0.2", (-ddouble.Rcp(5)).ToString());
            Assert.AreEqual("-2", (-ddouble.Rcp(5) * 10).ToString());
            Assert.AreEqual("-0.1666666666666666666666666666667", (-ddouble.Rcp(6)).ToString());
            Assert.AreEqual("-1.666666666666666666666666666667", (-ddouble.Rcp(6) * 10).ToString());
            Assert.AreEqual("-0.1428571428571428571428571428571", (-ddouble.Rcp(7)).ToString());
            Assert.AreEqual("-1.428571428571428571428571428571", (-ddouble.Rcp(7) * 10).ToString());

            ddouble p = 1, n = 1, radix = 10;
            for (int i = 1; i < 20; i++) {
                p *= radix;
                n /= radix;

                Assert.AreEqual($"1.00e{i}", p.ToString("e2"));
                Assert.AreEqual($"1.00e-{i}", n.ToString("e2"));
            }

            for (int i = 90; i <= 110; i++) {
                Console.WriteLine($"exp {-i} {(1 - ddouble.Ldexp(1, -i))}");
            }
        }

        [TestMethod]
        public void ToStringFormatTest() {
            ddouble v1 = ddouble.Rcp(3);
            ddouble v2 = ddouble.One - "1e-31";
            ddouble v3 = ddouble.One - "1e-32";

            Console.WriteLine($"{v1:e30}");
            Console.WriteLine($"{v1:E30}");
            Console.WriteLine(v1.ToString("e30"));
            Console.WriteLine(v1.ToString("E30"));

            Console.WriteLine($"{v1:e20}");
            Console.WriteLine($"{v1:E20}");
            Console.WriteLine(v1.ToString("e20"));
            Console.WriteLine(v1.ToString("E20"));

            Assert.AreEqual($"{v1:e30}", v1.ToString("e30"));
            Assert.AreEqual($"{v1:E30}", v1.ToString("E30"));
            Assert.AreEqual($"{v1:e20}", v1.ToString("e20"));
            Assert.AreEqual($"{v1:E20}", v1.ToString("E20"));

            Console.WriteLine($"{v1:e31}");
            Console.WriteLine($"{v1:E31}");
            Console.WriteLine(v1.ToString("e31"));
            Console.WriteLine(v1.ToString("E31"));

            Console.WriteLine($"{v2:e30}");
            Console.WriteLine($"{v2:E30}");
            Console.WriteLine(v2.ToString("e30"));
            Console.WriteLine(v2.ToString("E30"));

            Console.WriteLine($"{v2:e20}");
            Console.WriteLine($"{v2:E20}");
            Console.WriteLine(v2.ToString("e20"));
            Console.WriteLine(v2.ToString("E20"));

            Assert.AreEqual($"{v2:e30}", v2.ToString("e30"));
            Assert.AreEqual($"{v2:E30}", v2.ToString("E30"));
            Assert.AreEqual($"{v2:e20}", v2.ToString("e20"));
            Assert.AreEqual($"{v2:E20}", v2.ToString("E20"));

            Console.WriteLine($"{v2:e31}");
            Console.WriteLine($"{v2:E31}");
            Console.WriteLine(v2.ToString("e31"));
            Console.WriteLine(v2.ToString("E31"));

            Console.WriteLine($"{v3:e30}");
            Console.WriteLine($"{v3:E30}");
            Console.WriteLine(v3.ToString("e30"));
            Console.WriteLine(v3.ToString("E30"));

            Console.WriteLine($"{v3:e20}");
            Console.WriteLine($"{v3:E20}");
            Console.WriteLine(v3.ToString("e20"));
            Console.WriteLine(v3.ToString("E20"));

            Assert.AreEqual($"{v3:e30}", v3.ToString("e30"));
            Assert.AreEqual($"{v3:E30}", v3.ToString("E30"));
            Assert.AreEqual($"{v3:e20}", v3.ToString("e20"));
            Assert.AreEqual($"{v3:E20}", v3.ToString("E20"));

            Console.WriteLine($"{v3:e31}");
            Console.WriteLine($"{v3:E31}");
            Console.WriteLine(v3.ToString("e31"));
            Console.WriteLine(v3.ToString("E31"));

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                Console.WriteLine($"{v1:e32}");
            });

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                Console.WriteLine($"{v1:E32}");
            });

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                Console.WriteLine(v1.ToString("e32"));
            });

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                Console.WriteLine(v1.ToString("E32"));
            });

            Assert.ThrowsExactly<FormatException>(() => {
                Console.WriteLine($"{v1:f30}");
            });

            Assert.ThrowsExactly<FormatException>(() => {
                Console.WriteLine($"{v1:f30}");
            });
        }

        [TestMethod]
        public void ToStringDigitTest() {
            string v1 = "3.14159265358979323846264338327950288";
            string v2 = "5.55555555555555555555555555555555555e-1";
            string v3 = "5.45454545454545454545454545454545454e-2";
            string v4 = "9.99999999999999000000000000000000000e-3";

            Assert.AreEqual("3.1415926535897932384626434e0", $"{((ddouble)v1):e25}");
            Assert.AreEqual("3.14159265358979323846e0", $"{((ddouble)v1):e20}");
            Assert.AreEqual("3.141592653589793e0", $"{((ddouble)v1):e15}");
            Assert.AreEqual("3.1415926536e0", $"{((ddouble)v1):e10}");
            Assert.AreEqual("3.14159e0", $"{((ddouble)v1):e5}");

            Assert.AreEqual("5.5555555555555555555555556e-1", $"{((ddouble)v2):e25}");
            Assert.AreEqual("5.55555555555555555556e-1", $"{((ddouble)v2):e20}");
            Assert.AreEqual("5.555555555555556e-1", $"{((ddouble)v2):e15}");
            Assert.AreEqual("5.5555555556e-1", $"{((ddouble)v2):e10}");
            Assert.AreEqual("5.55556e-1", $"{((ddouble)v2):e5}");

            Assert.AreEqual("5.4545454545454545454545455e-2", $"{((ddouble)v3):e25}");
            Assert.AreEqual("5.45454545454545454545e-2", $"{((ddouble)v3):e20}");
            Assert.AreEqual("5.454545454545455e-2", $"{((ddouble)v3):e15}");
            Assert.AreEqual("5.4545454545e-2", $"{((ddouble)v3):e10}");
            Assert.AreEqual("5.45455e-2", $"{((ddouble)v3):e5}");

            Assert.AreEqual("9.9999999999999900000000000e-3", $"{((ddouble)v4):e25}");
            Assert.AreEqual("9.99999999999999000000e-3", $"{((ddouble)v4):e20}");
            Assert.AreEqual("9.999999999999990e-3", $"{((ddouble)v4):e15}");
            Assert.AreEqual("1.0000000000e-2", $"{((ddouble)v4):e10}");
            Assert.AreEqual("1.00000e-2", $"{((ddouble)v4):e5}");

            for (int i = 7; i <= 100; i++) {
                BigInteger n = BigInteger.One << i;
                string s0 = n.ToString();
                string s1 = (n - 1).ToString();
                string s2 = (n + 1).ToString();

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 1}", $"{((ddouble)s0).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 1}", $"{((ddouble)s1).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 1}", $"{((ddouble)s2).ToString($"e{s2.Length - 1}")}");

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 6}", $"{((ddouble)s0 / 100000).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 6}", $"{((ddouble)s1 / 100000).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 6}", $"{((ddouble)s2 / 100000).ToString($"e{s2.Length - 1}")}");

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 12}", $"{((ddouble)s0 / 100000000000).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 12}", $"{((ddouble)s1 / 100000000000).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 12}", $"{((ddouble)s2 / 100000000000).ToString($"e{s2.Length - 1}")}");
            }

            for (int i = 3; i <= 30; i++) {
                BigInteger n = BigInteger.Parse($"1{new string('0', i)}");
                string s0 = n.ToString();
                string s1 = (n - 1).ToString();
                string s2 = (n + 1).ToString();

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 1}", $"{((ddouble)s0).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 1}", $"{((ddouble)s1).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 1}", $"{((ddouble)s2).ToString($"e{s2.Length - 1}")}");

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 6}", $"{((ddouble)s0 / 100000).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 6}", $"{((ddouble)s1 / 100000).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 6}", $"{((ddouble)s2 / 100000).ToString($"e{s2.Length - 1}")}");

                Assert.AreEqual($"{s0[0]}.{s0[1..]}e{s0.Length - 12}", $"{((ddouble)s0 / 100000000000).ToString($"e{s0.Length - 1}")}");
                Assert.AreEqual($"{s1[0]}.{s1[1..]}e{s1.Length - 12}", $"{((ddouble)s1 / 100000000000).ToString($"e{s1.Length - 1}")}");
                Assert.AreEqual($"{s2[0]}.{s2[1..]}e{s2.Length - 12}", $"{((ddouble)s2 / 100000000000).ToString($"e{s2.Length - 1}")}");
            }
        }

        [TestMethod]
        public void ParseFormatTest() {
            ddouble v2 = "1.234567890123456789012345678901234567890123456789e-20";
            ddouble v3 = "0.00000000000000000001234567890123456789012345678901234567890123456789";
            ddouble v4 = "0.0000000001234567890123456789012345678901234567890123456789e-10";
            ddouble v5 = "12345678901.23456789012345678901234567890123456789e-30";
            ddouble v6 = "123456789012345678901.2345678901234567890123456789e-40";

            Assert.AreEqual(v2, v3);
            Assert.AreEqual(v2, v4);
            Assert.AreEqual(v2, v5);
            Assert.AreEqual(v2, v6);
        }

        [TestMethod]
        public void ParseZeroTest() {
            ddouble v2 = "00000";
            ddouble v3 = "0.0000";
            ddouble v4 = "0";
            ddouble v5 = "0.0";
            ddouble v6 = "-0";
            ddouble v7 = "-0.0";
            ddouble v8 = "0e1";
            ddouble v9 = "0.0e1";
            ddouble v10 = "-0e1";
            ddouble v11 = "-0.0e1";
            ddouble v12 = "0e-1";
            ddouble v13 = "0.0e-1";
            ddouble v14 = "-0e-1";
            ddouble v15 = "-0.0e-1";

            Assert.AreEqual(0, v2);
            Assert.AreEqual(0, v3);
            Assert.AreEqual(0, v4);
            Assert.AreEqual(0, v5);
            Assert.AreEqual(0, v6);
            Assert.AreEqual(0, v7);
            Assert.IsTrue(ddouble.IsNegative(v6));
            Assert.IsTrue(ddouble.IsNegative(v7));
            Assert.AreEqual(0, v8);
            Assert.AreEqual(0, v9);
            Assert.AreEqual(0, v10);
            Assert.AreEqual(0, v11);
            Assert.IsTrue(ddouble.IsNegative(v10));
            Assert.IsTrue(ddouble.IsNegative(v11));
            Assert.AreEqual(0, v12);
            Assert.AreEqual(0, v13);
            Assert.AreEqual(0, v14);
            Assert.AreEqual(0, v15);
            Assert.IsTrue(ddouble.IsNegative(v14));
            Assert.IsTrue(ddouble.IsNegative(v15));
        }

        [TestMethod]
        public void ParseTest() {
            Random random = new Random(1234);
            for (int i = 0; i < 2048; i++) {
                string v =
                    $"{random.Next(1000000000):D9}" +
                    $"{random.Next(1000000000):D9}" +
                    $"{random.Next(1000000000):D9}" +
                    $"{random.Next(1000000000):D9}";

                int pos = random.Next(1, v.Length);

                string w0 = v[..pos] + '.' + v[pos..];
                string w1 = v[..1] + '.' + v[1..] + $"e{random.Next(20)}";
                string w2 = v[..1] + '.' + v[1..] + $"e+{random.Next(20)}";
                string w3 = v[..1] + '.' + v[1..] + $"e-{random.Next(20)}";
                string w4 = '+' + w0;
                string w5 = '-' + w0;

                Assert.AreEqual(double.Parse(v), (double)(ddouble)(v));
                Assert.AreEqual(double.Parse(w0), (double)(ddouble)(w0));
                Assert.AreEqual(double.Parse(w1), (double)(ddouble)(w1));
                Assert.AreEqual(double.Parse(w2), (double)(ddouble)(w2));
                Assert.AreEqual(double.Parse(w3), (double)(ddouble)(w3));
                Assert.AreEqual(double.Parse(w4), (double)(ddouble)(w4));
                Assert.AreEqual(double.Parse(w5), (double)(ddouble)(w5));
            }
        }

        [TestMethod]
        public void ParseLimitTest() {
            string p9 = "0.9";

            for (int i = 0; i < 40; i++) {
                ddouble v = p9;

                Console.WriteLine(p9);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p9 += "9";
            }

            string p3 = "0.3";

            for (int i = 0; i < 40; i++) {
                ddouble v = p3;

                Console.WriteLine(p3);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p3 += "3";
            }

            string p4 = "0.4";

            for (int i = 0; i < 40; i++) {
                ddouble v = p4;

                Console.WriteLine(p4);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p4 += "4";
            }

            string p5 = "0.5";

            for (int i = 0; i < 40; i++) {
                ddouble v = p5;

                Console.WriteLine(p5);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p5 += "5";
            }
        }

        [TestMethod]
        public void ParseIrregularTest() {
            ddouble nan1 = double.NaN.ToString();
            ddouble nan2 = "nan";
            ddouble nan3 = "NaN";

            ddouble pinf1 = double.PositiveInfinity.ToString();
            ddouble pinf2 = "inf";
            ddouble pinf3 = "+inf";
            ddouble pinf4 = "INF";
            ddouble pinf5 = "+INF";

            ddouble ninf1 = double.NegativeInfinity.ToString();
            ddouble ninf2 = "-inf";
            ddouble ninf3 = "-INF";

            PrecisionAssert.IsNaN(nan1);
            PrecisionAssert.IsNaN(nan2);
            PrecisionAssert.IsNaN(nan3);

            PrecisionAssert.IsPositiveInfinity(pinf1);
            PrecisionAssert.IsPositiveInfinity(pinf2);
            PrecisionAssert.IsPositiveInfinity(pinf3);
            PrecisionAssert.IsPositiveInfinity(pinf4);
            PrecisionAssert.IsPositiveInfinity(pinf5);

            PrecisionAssert.IsNegativeInfinity(ninf1);
            PrecisionAssert.IsNegativeInfinity(ninf2);
            PrecisionAssert.IsNegativeInfinity(ninf3);
        }

        [TestMethod]
        public void SubnormalTest() {
            for ((ddouble x, int exp) = (double.ScaleB(1, -950), -950); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (double.BitDecrement(double.ScaleB(1, -950)), -950); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (double.BitIncrement(double.ScaleB(1, -950)), -950); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -964), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -932), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -900), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -964) * ddouble.Rcp(7), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -932) * ddouble.Rcp(7), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) + ddouble.Ldexp(1, -900) * ddouble.Rcp(7), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
                Assert.AreEqual(exp, double.ILogB(y.Hi));
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -964) * ddouble.Rcp(15), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -932) * ddouble.Rcp(31), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -900) * ddouble.Rcp(63), -868); x > 0; x /= 2, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -964) * ddouble.Rcp(127), -868); x > 0; x /= 10, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -932) * ddouble.Rcp(255), -868); x > 0; x /= 10, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }

            for ((ddouble x, int exp) = (ddouble.Ldexp(1, -868) * ddouble.Pi + ddouble.Ldexp(1, -900) * ddouble.Rcp(512), -868); x > 0; x /= 10, exp--) {
                ddouble y = x.ToString();

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(x, y, 1e-30);
            }
        }
    }
}
