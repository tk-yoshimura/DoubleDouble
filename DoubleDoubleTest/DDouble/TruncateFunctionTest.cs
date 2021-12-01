using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class TruncateFunctionTest {
        [TestMethod]
        public void FloorTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Floor(0));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Floor(1));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Floor(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(1), ddouble.Floor(1 + ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitDecrement(1 + ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(1), ddouble.Floor(ddouble.BitIncrement(1 + ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("1e20"), ddouble.Floor("1e20"));
            Assert.AreEqual((ddouble)("0.99999999999999999999e20"), ddouble.Floor(ddouble.BitDecrement("1e20")));
            Assert.AreEqual((ddouble)("1e20"), ddouble.Floor(ddouble.BitIncrement("1e20")));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-1));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(-1), ddouble.Floor(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(-1), ddouble.Floor(ddouble.BitIncrement(-ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-2), ddouble.Floor(-1 - ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitDecrement(-1 - ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-2), ddouble.Floor(ddouble.BitIncrement(-1 - ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("-1e20"), ddouble.Floor("-1e20"));
            Assert.AreEqual((ddouble)("-1.00000000000000000001e20"), ddouble.Floor(ddouble.BitDecrement("-1e20")));
            Assert.AreEqual((ddouble)("-1e20"), ddouble.Floor(ddouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void CeilingTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(0));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(1));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(1), ddouble.Ceiling(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(1 + ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitDecrement(1 + ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(2), ddouble.Ceiling(ddouble.BitIncrement(1 + ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("1e20"), ddouble.Ceiling("1e20"));
            Assert.AreEqual((ddouble)("1e20"), ddouble.Ceiling(ddouble.BitDecrement("1e20")));
            Assert.AreEqual((ddouble)("1.00000000000000000001e20"), ddouble.Ceiling(ddouble.BitIncrement("1e20")));

            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Ceiling(ddouble.BitIncrement(-ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(-1 - ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitDecrement(-1 - ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(-1), ddouble.Ceiling(ddouble.BitIncrement(-1 - ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)("-1e20"), ddouble.Ceiling("-1e20"));
            Assert.AreEqual((ddouble)("-1e20"), ddouble.Ceiling(ddouble.BitDecrement("-1e20")));
            Assert.AreEqual((ddouble)("-0.99999999999999999999e20"), ddouble.Ceiling(ddouble.BitIncrement("-1e20")));
        }

        [TestMethod]
        public void RoundTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Round(0));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Round(1));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(1), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Round(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(-1), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Round(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Round(ddouble.BitIncrement(-ddouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateTest() {
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(0));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(0)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(0)));

            Assert.AreEqual((ddouble)(1), ddouble.Truncate(1));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(1)));
            Assert.AreEqual((ddouble)(1), ddouble.Truncate(ddouble.BitIncrement(1)));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(ddouble.Rcp(7))));

            Assert.AreEqual((ddouble)(-1), ddouble.Truncate(-1));
            Assert.AreEqual((ddouble)(-1), ddouble.Truncate(ddouble.BitDecrement(-1)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-1)));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(2)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(2))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(2))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(3)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(3))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(3))));

            Assert.AreEqual((ddouble)(0), ddouble.Truncate(-ddouble.Rcp(7)));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitDecrement(-ddouble.Rcp(7))));
            Assert.AreEqual((ddouble)(0), ddouble.Truncate(ddouble.BitIncrement(-ddouble.Rcp(7))));
        }

        [TestMethod]
        public void TruncateMantissaTest() {
            foreach (ddouble v in new ddouble[] {
                -1, -ddouble.Rcp(3), -ddouble.Rcp(7), -ddouble.BitDecrement(2),
                1, ddouble.Rcp(3), ddouble.Rcp(7), ddouble.BitDecrement(2) }) {

                for (int keep_bits = 1; keep_bits <= 110; keep_bits++) {
                    ddouble v_round = ddouble.TruncateMantissa(v, keep_bits);

                    Console.WriteLine(v_round);
                    Console.WriteLine($"0x{FloatSplitter.Split(v_round).mantissa:X14}");
                    Console.WriteLine(v_round - v);
                }
            }
        }
    }
}
