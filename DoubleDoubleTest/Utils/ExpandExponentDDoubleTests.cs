using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.Utils {
    [TestClass]
    public class ExpandExponentDDoubleTests {
        [TestMethod]
        public void ToStringTest() {
            ExpandExponentDDouble v1 = (ddouble)"0.00001";
            ExpandExponentDDouble v2 = (ddouble)"0.125";
            ExpandExponentDDouble v3 = 1 / (ddouble)"0.00001";
            ExpandExponentDDouble v4 = 1 / (ddouble)"0.125";
            ExpandExponentDDouble v5 = (ddouble)"-0.00001";
            ExpandExponentDDouble v6 = (ddouble)"-0.125";
            ExpandExponentDDouble v7 = 1 / (ddouble)"-0.00001";
            ExpandExponentDDouble v8 = 1 / (ddouble)"-0.125";

            Assert.AreEqual("1e-5", v1.ToString());
            Assert.AreEqual("1.25e-1", v2.ToString());
            Assert.AreEqual("1e5", v3.ToString());
            Assert.AreEqual("8e0", v4.ToString());
            Assert.AreEqual("-1e-5", v5.ToString());
            Assert.AreEqual("-1.25e-1", v6.ToString());
            Assert.AreEqual("-1e5", v7.ToString());
            Assert.AreEqual("-8e0", v8.ToString());
        }

        [TestMethod]
        public void MulTest() {
            ExpandExponentDDouble v1 = (ddouble)"0.00001";
            ExpandExponentDDouble v2 = (ddouble)"0.125";
            ExpandExponentDDouble v3 = (ddouble)"1.5";
            ExpandExponentDDouble v4 = (ddouble)"0.15";
            ExpandExponentDDouble v5 = (ddouble)"1e150";
            ExpandExponentDDouble v6 = (ddouble)"1e175";

            Assert.AreEqual("1.25e-6", (v1 * v2).ToString());
            Assert.AreEqual("2.25e-1", (v3 * v4).ToString());
            HPAssert.AreEqual("1e325", (v5 * v6).ToString(), 1e-294);
        }

        [TestMethod]
        public void DivTest() {
            ExpandExponentDDouble v1 = (ddouble)"0.00001";
            ExpandExponentDDouble v2 = (ddouble)"0.125";
            ExpandExponentDDouble v3 = (ddouble)"1.5";
            ExpandExponentDDouble v4 = (ddouble)"0.15";
            ExpandExponentDDouble v5 = (ddouble)"1e150";
            ExpandExponentDDouble v6 = (ddouble)"1e175";

            Assert.AreEqual("8e-5", (v1 / v2).ToString());
            Assert.AreEqual("1e1", (v3 / v4).ToString());
            HPAssert.AreEqual("1e-25", (v5 / v6).ToString(), 1e-54);
        }

        [TestMethod]
        public void AddTest() {
            ExpandExponentDDouble v1 = (ddouble)"0.00001";
            ExpandExponentDDouble v2 = (ddouble)"0.125";
            ExpandExponentDDouble v3 = (ddouble)"1.5";
            ExpandExponentDDouble v4 = (ddouble)"0.15";
            ExpandExponentDDouble v5 = (ddouble)"1e150";
            ExpandExponentDDouble v6 = (ddouble)"1e175";

            Assert.AreEqual("1.2501e-1", (v1 + v2).ToString());
            Assert.AreEqual("1.65e0", (v3 + v4).ToString());
            HPAssert.AreEqual("1.0000000000000000000000001e175", (v5 + v6).ToString(), 1e146);
        }

        [TestMethod]
        public void SubTest() {
            ExpandExponentDDouble v1 = (ddouble)"0.00001";
            ExpandExponentDDouble v2 = (ddouble)"0.125";
            ExpandExponentDDouble v3 = (ddouble)"1.5";
            ExpandExponentDDouble v4 = (ddouble)"0.15";
            ExpandExponentDDouble v5 = (ddouble)"1e150";
            ExpandExponentDDouble v6 = (ddouble)"1e175";

            Assert.AreEqual("-1.2499e-1", (v1 - v2).ToString());
            Assert.AreEqual("1.35e0", (v3 - v4).ToString());
            HPAssert.AreEqual("-9.999999999999999999999999e174", (v5 - v6).ToString(), 1e145);
        }
    }
}
