using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest {
    [TestClass]
    public class DDoubleConstantTests {
        [TestMethod]
        public void ETest() {
            ddouble expected = (ddouble)"2.71828182845904523536028747135266250";
            ddouble actual = ddouble.E;
            ddouble error = expected - actual;

            Console.WriteLine(actual);
            Console.WriteLine(error);

            Assert.IsTrue(ddouble.Abs(error) < 1e-31);
        }

        [TestMethod]
        public void PITest() {
            ddouble expected = (ddouble)"3.14159265358979323846264338327950288";
            ddouble actual = ddouble.PI;
            ddouble error = expected - actual;

            Console.WriteLine(actual);
            Console.WriteLine(error);

            Assert.IsTrue(ddouble.Abs(error) < 1e-31);
        }
    }
}
