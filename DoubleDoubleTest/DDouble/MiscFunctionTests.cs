using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;

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

                PrecisionAssert.AreEqual(0, u, ddouble.Abs((double)d) * 1e-30, $"{d}");
                Assert.IsTrue(ddouble.IsRegulared(v));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = -10000m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Rcp(v) * (ddouble)d - 1;

                PrecisionAssert.AreEqual(0, u, ddouble.Abs((double)d) * 1e-32, $"{d}");
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

            PrecisionAssert.IsPositiveInfinity(rcp_pzero, nameof(rcp_pzero));
            PrecisionAssert.IsNegativeInfinity(rcp_mzero, nameof(rcp_mzero));
            PrecisionAssert.IsPlusZero(rcp_pinf, nameof(rcp_pinf));
            PrecisionAssert.IsMinusZero(rcp_ninf, nameof(rcp_ninf));
            PrecisionAssert.IsNaN(rcp_nan, nameof(rcp_nan));
            PrecisionAssert.AreEqual(1 / double.MaxValue, (double)rcp_pval, double.Epsilon, nameof(rcp_pval));
            PrecisionAssert.AreEqual(1 / double.MinValue, (double)rcp_mval, double.Epsilon, nameof(rcp_mval));
            PrecisionAssert.IsPositiveInfinity(rcp_peps, nameof(rcp_peps));
            PrecisionAssert.IsNegativeInfinity(rcp_meps, nameof(rcp_meps));
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
        public void CopySignTest() {
            ddouble[] tests = new ddouble[] {
                ddouble.NegativeInfinity, -10, -1, -ddouble.Rcp(10), -0d, 0d, ddouble.Rcp(10), 1, 10, ddouble.PositiveInfinity, ddouble.NaN
            };

            foreach (ddouble value in tests) {
                foreach (ddouble sign in tests) {
                    double expected = double.CopySign(value.Hi, sign.Hi);
                    ddouble actual = ddouble.CopySign(value, sign);

                    Assert.AreEqual(double.IsPositive(expected), ddouble.IsPositive(actual), $"{value},{sign}");
                    Assert.AreEqual(double.IsNegative(expected), ddouble.IsNegative(actual), $"{value},{sign}");
                    Assert.AreEqual(ddouble.IsPositive(sign) ? ddouble.Abs(value) : -ddouble.Abs(value), actual);
                }
            }
        }

        [TestMethod]
        public void SquareTest() {
            PrecisionAssert.AreEqual(4, ddouble.Square(2));
            PrecisionAssert.AreEqual(9, ddouble.Square(3));
        }

        [TestMethod]
        public void CubeTest() {
            PrecisionAssert.AreEqual(8, ddouble.Cube(2));
            PrecisionAssert.AreEqual(27, ddouble.Cube(3));
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

            Assert.AreEqual((-2, (-0.0625d, 0.125d)), ddouble.AdjustScale(-3, (-0.25d, 0.5d)));
            Assert.AreEqual((4, (-4d, 8d)), ddouble.AdjustScale(3, (-0.25d, 0.5d)));

            Assert.AreEqual((-2, (0.0625d, -0.125d)), ddouble.AdjustScale(-3, (0.25d, -0.5d)));
            Assert.AreEqual((4, (4d, -8d)), ddouble.AdjustScale(3, (0.25d, -0.5d)));

            Assert.AreEqual((-2, (0.125d, -0.0625d)), ddouble.AdjustScale(-3, (0.5d, -0.25d)));
            Assert.AreEqual((4, (8d, -4d)), ddouble.AdjustScale(3, (0.5d, -0.25d)));

            Assert.AreEqual((-2, (-0.125d, 0.0625d)), ddouble.AdjustScale(-3, (-0.5d, 0.25d)));
            Assert.AreEqual((4, (-8d, 4d)), ddouble.AdjustScale(3, (-0.5d, 0.25d)));

            Assert.AreEqual((-3, (0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1)));
            Assert.AreEqual((3, (2d, 4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1)));

            Assert.AreEqual((-3, (0.03125d, 0.0625d, -0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, -1)));
            Assert.AreEqual((3, (2d, 4d, -8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, -1)));

            Assert.AreEqual((-3, (0.03125d, -0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, -0.5d, 1)));
            Assert.AreEqual((3, (2d, -4d, 8d)), ddouble.AdjustScale(3, (0.25d, -0.5d, 1)));

            Assert.AreEqual((-3, (-0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (-0.25d, 0.5d, 1)));
            Assert.AreEqual((3, (-2d, 4d, 8d)), ddouble.AdjustScale(3, (-0.25d, 0.5d, 1)));

            Assert.AreEqual((-3, (0.03125d, -0.125d, 0.0625d)), ddouble.AdjustScale(-3, (0.25d, -1, 0.5d)));
            Assert.AreEqual((3, (2d, -8d, 4d)), ddouble.AdjustScale(3, (0.25d, -1, 0.5d)));

            Assert.AreEqual((-3, (-0.125d, 0.03125d, 0.0625d)), ddouble.AdjustScale(-3, (-1, 0.25d, 0.5d)));
            Assert.AreEqual((3, (-8d, 2d, 4d)), ddouble.AdjustScale(3, (-1, 0.25d, 0.5d)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1, 2)));
            Assert.AreEqual((2, (1d, 2d, 4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1, 2)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, 0.0625d, -0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, 1, -2)));
            Assert.AreEqual((2, (1d, 2d, 4d, -8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, 1, -2)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, -0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, -1, 2)));
            Assert.AreEqual((2, (1d, 2d, -4d, 8d)), ddouble.AdjustScale(3, (0.25d, 0.5d, -1, 2)));

            Assert.AreEqual((-4, (0.015625d, -0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (0.25d, -0.5d, 1, 2)));
            Assert.AreEqual((2, (1d, -2d, 4d, 8d)), ddouble.AdjustScale(3, (0.25d, -0.5d, 1, 2)));

            Assert.AreEqual((-4, (-0.015625d, 0.03125d, 0.0625d, 0.125d)), ddouble.AdjustScale(-3, (-0.25d, 0.5d, 1, 2)));
            Assert.AreEqual((2, (-1d, 2d, 4d, 8d)), ddouble.AdjustScale(3, (-0.25d, 0.5d, 1, 2)));

            Assert.AreEqual((-4, (0.015625d, 0.03125d, -0.125d, 0.0625d)), ddouble.AdjustScale(-3, (0.25d, 0.5d, -2, 1)));
            Assert.AreEqual((2, (1d, 2d, -8d, 4d)), ddouble.AdjustScale(3, (0.25d, 0.5d, -2, 1)));

            Assert.AreEqual((-4, (0.015625d, -0.125d, 0.03125d, 0.0625d)), ddouble.AdjustScale(-3, (0.25d, -2, 0.5d, 1)));
            Assert.AreEqual((2, (1d, -8d, 2d, 4d)), ddouble.AdjustScale(3, (0.25d, -2, 0.5d, 1)));

            Assert.AreEqual((-4, (-0.125d, 0.015625d, 0.03125d, 0.0625d)), ddouble.AdjustScale(-3, (-2, 0.25d, 0.5d, 1)));
            Assert.AreEqual((2, (-8d, 1d, 2d, 4d)), ddouble.AdjustScale(3, (-2, 0.25d, 0.5d, 1)));

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
            PrecisionAssert.AlmostEqual(3 * ddouble.Pi / (4 * ddouble.EllipticK(ddouble.Rcp(9))), ddouble.Agm(1, 2), 1e-31);
            PrecisionAssert.AlmostEqual(5 * ddouble.Pi / (2 * ddouble.EllipticK(4 * ddouble.Rcp(25))), ddouble.Agm(3, 7), 1e-31);
            PrecisionAssert.AlmostEqual(7 * ddouble.Pi / (40 * ddouble.EllipticK(9 * ddouble.Rcp(49))), ddouble.Agm(0.5m, 0.2m), 1e-31);
            PrecisionAssert.AlmostEqual(65 * ddouble.Pi / (4 * ddouble.EllipticK(3969 * ddouble.Rcp(4225))), ddouble.Agm(64, 1), 1e-31);
            Assert.IsTrue(ddouble.Agm(1, double.Epsilon) > 0);
            PrecisionAssert.AreEqual(0d, ddouble.Agm(1, 0));
        }

        [TestMethod]
        public void LdexpTest() {
            PrecisionAssert.AreEqual(4, ddouble.Ldexp(2, 1));
            PrecisionAssert.AreEqual(4, ddouble.Ldexp(2, 1L));
            PrecisionAssert.AreEqual(4, ddouble.ScaleB(2, 1));
            PrecisionAssert.AreEqual(4, ddouble.ScaleB(2, 1L));

            PrecisionAssert.AreEqual(0, ddouble.Ldexp(2, long.MinValue));
            PrecisionAssert.IsPositiveInfinity(ddouble.Ldexp(2, long.MaxValue));
        }

        [TestMethod]
        public void GeometricMeanTest() {
            for (ddouble a = 0.125; a < 20; a *= 1.5) {
                for (ddouble b = 0.125; b < 20; b *= 1.5) {
                    PrecisionAssert.AreEqual(ddouble.Sqrt(a * b), ddouble.GeometricMean(a, b), 4e-32);
                }
            }

            PrecisionAssert.AreEqual(double.ScaleB(1, 950), ddouble.GeometricMean(double.ScaleB(1, 950), double.ScaleB(1, 950)));
            PrecisionAssert.AreEqual(double.ScaleB(1, -950), ddouble.GeometricMean(double.ScaleB(1, -950), double.ScaleB(1, -950)));

            for (ddouble a = 0.125; a < 20; a *= 1.5) {
                for (ddouble b = 0.125; b < 20; b *= 1.5) {
                    for (ddouble c = 0.125; c < 20; c *= 1.5) {
                        PrecisionAssert.AreEqual(ddouble.Cbrt(a * b * c), ddouble.GeometricMean(a, b, c), 4e-32);
                    }
                }
            }

            PrecisionAssert.AreEqual(double.ScaleB(1, 950), ddouble.GeometricMean(double.ScaleB(1, 950), double.ScaleB(1, 950), double.ScaleB(1, 950)));
            PrecisionAssert.AreEqual(double.ScaleB(1, -950), ddouble.GeometricMean(double.ScaleB(1, -950), double.ScaleB(1, -950), double.ScaleB(1, -950)));
        }
    }
}
