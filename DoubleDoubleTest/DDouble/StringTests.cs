using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
    }
}
