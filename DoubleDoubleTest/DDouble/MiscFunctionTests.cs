using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class MiscFunctionTests {
        [TestMethod]
        public void RcpTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Rcp(v) * v - 1;

                HPAssert.AreEqual(0, u, ddouble.Abs((double)d) * 1e-30, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Rcp(v) * (ddouble)d - 1;

                HPAssert.AreEqual(0, u, ddouble.Abs((double)d) * 1e-32, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble rcp_pzero = ddouble.Rcp(0d);
            ddouble rcp_mzero = ddouble.Rcp(-0d);
            ddouble rcp_pinf = ddouble.Rcp(double.PositiveInfinity);
            ddouble rcp_ninf = ddouble.Rcp(double.NegativeInfinity);
            ddouble rcp_nan = ddouble.Rcp(double.NaN);
            ddouble rcp_pval = ddouble.Rcp(double.MaxValue);
            ddouble rcp_mval = ddouble.Rcp(double.MinValue);
            ddouble rcp_peps = ddouble.Rcp(double.Epsilon);
            ddouble rcp_meps = ddouble.Rcp(-double.Epsilon);

            Assert.IsTrue(ddouble.IsPositiveInfinity(rcp_pzero), nameof(rcp_pzero));
            Assert.IsTrue(ddouble.IsNegativeInfinity(rcp_mzero), nameof(rcp_mzero));
            Assert.IsTrue(ddouble.IsPlusZero(rcp_pinf), nameof(rcp_pinf));
            Assert.IsTrue(ddouble.IsMinusZero(rcp_ninf), nameof(rcp_ninf));
            Assert.IsTrue(ddouble.IsNaN(rcp_nan), nameof(rcp_nan));
            Assert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            Assert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            Assert.IsTrue(ddouble.IsPositiveInfinity(rcp_peps), nameof(rcp_peps));
            Assert.IsTrue(ddouble.IsNegativeInfinity(rcp_meps), nameof(rcp_meps));
        }

        [TestMethod]
        public void AbsTest() {
            foreach (ddouble v in new ddouble[] { 1, 2,
                ddouble.Rcp(3), ddouble.Rcp(5), ddouble.Rcp(7), ddouble.Rcp(9), ddouble.Rcp(11) }) {

                ddouble v_dec = ddouble.BitDecrement(v), v_inc = ddouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(ddouble.Abs(v_dec) < ddouble.Abs(v) && ddouble.Abs(v) < ddouble.Abs(v_inc));
                Assert.IsTrue(ddouble.Abs(-v_dec) < ddouble.Abs(-v) && ddouble.Abs(-v) < ddouble.Abs(-v_inc));

                Assert.AreEqual(ddouble.Abs(v_dec), ddouble.Abs(-v_dec));
                Assert.AreEqual(ddouble.Abs(v), ddouble.Abs(-v));
                Assert.AreEqual(ddouble.Abs(v_inc), ddouble.Abs(-v_inc));
            }

            foreach (ddouble v in new ddouble[] { -1, -2,
                -ddouble.Rcp(3), -ddouble.Rcp(5), -ddouble.Rcp(7), -ddouble.Rcp(9), -ddouble.Rcp(11) }) {

                ddouble v_dec = ddouble.BitDecrement(v), v_inc = ddouble.BitIncrement(v);

                Assert.IsTrue(v_dec < v && v < v_inc);

                Assert.IsTrue(ddouble.Abs(v_dec) > ddouble.Abs(v) && ddouble.Abs(v) > ddouble.Abs(v_inc));
                Assert.IsTrue(ddouble.Abs(-v_dec) > ddouble.Abs(-v) && ddouble.Abs(-v) > ddouble.Abs(-v_inc));

                Assert.AreEqual(ddouble.Abs(v_dec), ddouble.Abs(-v_dec));
                Assert.AreEqual(ddouble.Abs(v), ddouble.Abs(-v));
                Assert.AreEqual(ddouble.Abs(v_inc), ddouble.Abs(-v_inc));
            }

            Assert.AreEqual(ddouble.Abs(ddouble.BitDecrement(0)), ddouble.Abs(ddouble.BitIncrement(0)));
        }

        [TestMethod]
        public void SquareTest() {
            Assert.AreEqual(4, ddouble.Square(2));
            Assert.AreEqual(9, ddouble.Square(3));
        }

        [TestMethod]
        public void CubeTest() {
            Assert.AreEqual(8, ddouble.Cube(2));
            Assert.AreEqual(27, ddouble.Cube(3));
        }

        [TestMethod]
        public void FrexpTest() {
            Assert.AreEqual((-2, 1), ddouble.Frexp(0.25d));
            Assert.AreEqual(1, ddouble.Frexp(2).exp);
            Assert.AreEqual(0, ddouble.Frexp(1).exp);

            Assert.AreEqual(1, ddouble.Frexp(ddouble.BitDecrement(2)).exp);
            Assert.IsTrue(ddouble.Frexp(ddouble.BitDecrement(2)).value < 2);

            Assert.AreEqual(-1, ddouble.Frexp(ddouble.BitDecrement(1)).exp);
            Assert.IsTrue(ddouble.Frexp(ddouble.BitDecrement(1)).value < 2);

            (int exp, ddouble value) = ddouble.Frexp(ddouble.BitDecrement(1));
            Assert.AreEqual(ddouble.BitDecrement(1), ddouble.Ldexp(value, exp));
        }

        [TestMethod]
        public void AjdustScaleTest() {
            Assert.AreEqual((-1, 0.125d), ddouble.AdjustScale(-3, 0.25d));
            Assert.AreEqual((5, 8d), ddouble.AdjustScale(3, 0.25d));

            Assert.AreEqual((-2, (0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d)));
            Assert.AreEqual((4, (4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d)));

            Assert.AreEqual((-3, (0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1)));
            Assert.AreEqual((3, (2d, 4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1, 2)));
            Assert.AreEqual((2, (1d, 2d, 4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1, 2)));

            Assert.AreEqual((-1, 0.125d), ddouble.AdjustScale(-3, 0.25d));
            Assert.AreEqual((5, 8d), ddouble.AdjustScale(3, 0.25d));

            Assert.AreEqual((-2, (0.0625d, 0.1875d)), ddouble.AdjustScale(-3, (0.25d, 0.75d)));
            Assert.AreEqual((4, (4d, 12d)), ddouble.AdjustScale(3, (0.25d, 0.75d)));

            Assert.AreEqual((-3, (0.03125d, 0.0625d, 0.1875d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1.5d)));
            Assert.AreEqual((3, (2d, 4d, 12d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1.5d)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, 0.0625d, 0.1875d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1, 3)));
            Assert.AreEqual((2, (1d, 2d, 4d, 12d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1, 3)));

            Assert.AreEqual((-2, (0.1875d, 0.0625d)), ddouble.AdjustScale(-3, (0.75d, 0.25d)));
            Assert.AreEqual((4, (12d, 4d)), ddouble.AdjustScale(3, (0.75d, 0.25d)));

            Assert.AreEqual((-3, (0.0625d, 0.03125d, 0.1875d)), ddouble.AdjustScale(-3, (0.5d, 0.25d, 1.5d)));
            Assert.AreEqual((3, (4d, 2d, 12d)), ddouble.AdjustScale(3, (0.5d, 0.25d, 1.5d)));

            Assert.AreEqual((-4, (0.015625d, 0.1875d, 0.03125d, 0.0625d)), ddouble.AdjustScale(-3, (0.25d, 3, 0.5d, 1)));
            Assert.AreEqual((2, (1d, 12d, 2d, 4d)), ddouble.AdjustScale(3, (0.25d, 3, 0.5d, 1)));

            Assert.AreEqual((-4, 0.1875d), ddouble.AdjustScale(-3, 3));
            Assert.AreEqual((2, 12), ddouble.AdjustScale(3, 3));

            Assert.AreEqual((-5, (0.09375d, 0.1875d)), ddouble.AdjustScale(-3, (3, 6)));
            Assert.AreEqual((1, (6d, 12d)), ddouble.AdjustScale(3, (3, 6)));

            Assert.AreEqual((-6, (0.046875d, 0.1875d, 0.09375d)), ddouble.AdjustScale(-3, (3, 12, 6)));
            Assert.AreEqual((0, (3d, 12d, 6d)), ddouble.AdjustScale(3, (3, 12, 6)));

            Assert.AreEqual((-7, (0.1875d, 0.0234375d, 0.09375d, 0.046875d)), ddouble.AdjustScale(-3, (24, 3, 12, 6d)));
            Assert.AreEqual((-1, (12d, 1.5d, 6d, 3d)), ddouble.AdjustScale(3, (24, 3, 12, 6d)));
        }

        [TestMethod]
        public void AgmTest() {
            HPAssert.AreEqual(3 * ddouble.PI / (4 * ddouble.EllipticK(ddouble.Rcp(9))), ddouble.Agm(1, 2), 1e-30);
            HPAssert.AreEqual(5 * ddouble.PI / (2 * ddouble.EllipticK(4 * ddouble.Rcp(25))), ddouble.Agm(3, 7), 1e-30);
            HPAssert.AreEqual(7 * ddouble.PI / (40 * ddouble.EllipticK(9 * ddouble.Rcp(49))), ddouble.Agm(0.5m, 0.2m), 1e-30);
            HPAssert.AreEqual(65 * ddouble.PI / (4 * ddouble.EllipticK(3969 * ddouble.Rcp(4225))), ddouble.Agm(64, 1), 1e-29);
            Assert.IsTrue(ddouble.Agm(1, double.Epsilon) > 0);
            Assert.IsTrue(ddouble.IsZero(ddouble.Agm(1, 0)));
        }
    }
}
