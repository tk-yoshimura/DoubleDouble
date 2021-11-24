using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleStringTest {
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
    }
}
