using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.QDouble {
    [TestClass]
    public class StringTests {
        [TestMethod]
        public void ToStringTest() {
            Assert.AreEqual("0", (qdouble)(0).ToString());
            Assert.AreEqual("-0", (qdouble)(-0d).ToString());

            Assert.AreEqual("0.3333333333333333333333333333333", qdouble.Rcp(3).ToString());
            Assert.AreEqual("3.333333333333333333333333333333", (qdouble.Rcp(3) * 10).ToString());
            Assert.AreEqual("0.2", qdouble.Rcp(5).ToString());
            Assert.AreEqual("2", (qdouble.Rcp(5) * 10).ToString());
            Assert.AreEqual("0.1666666666666666666666666666667", qdouble.Rcp(6).ToString());
            Assert.AreEqual("1.666666666666666666666666666667", (qdouble.Rcp(6) * 10).ToString());
            Assert.AreEqual("0.1428571428571428571428571428571", qdouble.Rcp(7).ToString());
            Assert.AreEqual("1.428571428571428571428571428571", (qdouble.Rcp(7) * 10).ToString());

            Assert.AreEqual("-0.3333333333333333333333333333333", (-qdouble.Rcp(3)).ToString());
            Assert.AreEqual("-3.333333333333333333333333333333", (-qdouble.Rcp(3) * 10).ToString());
            Assert.AreEqual("-0.2", (-qdouble.Rcp(5)).ToString());
            Assert.AreEqual("-2", (-qdouble.Rcp(5) * 10).ToString());
            Assert.AreEqual("-0.1666666666666666666666666666667", (-qdouble.Rcp(6)).ToString());
            Assert.AreEqual("-1.666666666666666666666666666667", (-qdouble.Rcp(6) * 10).ToString());
            Assert.AreEqual("-0.1428571428571428571428571428571", (-qdouble.Rcp(7)).ToString());
            Assert.AreEqual("-1.428571428571428571428571428571", (-qdouble.Rcp(7) * 10).ToString());

            qdouble p = 1, n = 1, radix = 10;
            for (int i = 1; i < 20; i++) {
                p *= radix;
                n /= radix;

                Assert.AreEqual($"1.00e{i}", p.ToString("e2"));
                Assert.AreEqual($"1.00e-{i}", n.ToString("e2"));
            }

            for (int i = 90; i <= 110; i++) {
                Console.WriteLine($"exp {-i} {(1 - qdouble.Ldexp(1, -i))}");
            }
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

                Assert.AreEqual(ddouble.Parse(v), (ddouble)(qdouble)(v));
                Assert.AreEqual(ddouble.Parse(w0), (ddouble)(qdouble)(w0));
                Assert.AreEqual(ddouble.Parse(w1), (ddouble)(qdouble)(w1));
                Assert.AreEqual(ddouble.Parse(w2), (ddouble)(qdouble)(w2));
                Assert.AreEqual(ddouble.Parse(w3), (ddouble)(qdouble)(w3));
                Assert.AreEqual(ddouble.Parse(w4), (ddouble)(qdouble)(w4));
                Assert.AreEqual(ddouble.Parse(w5), (ddouble)(qdouble)(w5));
            }
        }

        [TestMethod]
        public void ParseLimitTest() {
            string p9 = "0.9";

            for (int i = 0; i < 80; i++) {
                qdouble v = p9;

                Console.WriteLine(p9);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p9 += "9";
            }

            string p3 = "0.3";

            for (int i = 0; i < 80; i++) {
                qdouble v = p3;

                Console.WriteLine(p3);
                Console.WriteLine(v);
                Console.WriteLine($"0x{FloatSplitter.Split(v).mantissa:X14}");

                p3 += "3";
            }
        }
    }
}
