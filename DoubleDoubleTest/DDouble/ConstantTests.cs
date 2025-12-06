using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ConstantTests {
        [TestMethod]
        public void ConstantsTest() {
            Console.WriteLine($"Pi={ddouble.Pi}");
            Console.WriteLine($"RcpPi={ddouble.RcpPi}");
            Console.WriteLine($"E={ddouble.E}");
            Console.WriteLine($"RcpE={ddouble.RcpE}");
            Console.WriteLine($"DigammaZero={ddouble.DigammaZero}");
            Console.WriteLine($"EulerGamma={ddouble.EulerGamma}");
            Console.WriteLine($"Zeta3={ddouble.Zeta3}");
            Console.WriteLine($"Zeta5={ddouble.Zeta5}");
            Console.WriteLine($"Zeta7={ddouble.Zeta7}");
            Console.WriteLine($"Zeta9={ddouble.Zeta9}");
            Console.WriteLine($"Ln2={ddouble.Ln2}");
            Console.WriteLine($"LbE={ddouble.LbE}");
            Console.WriteLine($"Lg2={ddouble.Lg2}");
            Console.WriteLine($"Lb10={ddouble.Lb10}");
            Console.WriteLine($"Sqrt2={ddouble.Sqrt2}");
            Console.WriteLine($"Point5={ddouble.Point5}");
            Console.WriteLine($"ErdosBorwein={ddouble.ErdosBorwein}");
            Console.WriteLine($"FeigenbaumDelta={ddouble.FeigenbaumDelta}");
            Console.WriteLine($"FeigenbaumAlpha={ddouble.FeigenbaumAlpha}");
            Console.WriteLine($"LemniscatePi={ddouble.LemniscatePi}");
            Console.WriteLine($"GlaisherA={ddouble.GlaisherA}");
            Console.WriteLine($"CatalanG={ddouble.CatalanG}");
            Console.WriteLine($"FransenRobinson={ddouble.FransenRobinson}");
            Console.WriteLine($"KhinchinK={ddouble.KhinchinK}");
            Console.WriteLine($"MeisselMertens={ddouble.MeisselMertens}");
            Console.WriteLine($"LambertOmega={ddouble.LambertOmega}");
            Console.WriteLine($"LandauRamanujan={ddouble.LandauRamanujan}");
            Console.WriteLine($"MillsTheta={ddouble.MillsTheta}");
            Console.WriteLine($"SoldnerMu={ddouble.SoldnerMu}");
            Console.WriteLine($"SierpinskiK={ddouble.SierpinskiK}");
            Console.WriteLine($"RcpFibonacci={ddouble.RcpFibonacci}");
            Console.WriteLine($"Niven={ddouble.Niven}");
            Console.WriteLine($"GolombDickman={ddouble.GolombDickman}");
            Console.WriteLine($"GoldenRatio={ddouble.GoldenRatio}");
            Console.WriteLine($"MalardiTheta={ddouble.MalardiTheta}");
        }

        [TestMethod]
        public void BasicTest() {
            PrecisionAssert.AreEqual(1, ddouble.One);
            PrecisionAssert.AreEqual(-1, ddouble.MinusOne);
            PrecisionAssert.IsPlusZero(ddouble.PlusZero);
            PrecisionAssert.IsMinusZero(ddouble.MinusZero);
        }

        [TestMethod]
        public void MaxValueTest() {
            PrecisionAssert.IsFinite(ddouble.MaxValue);
            PrecisionAssert.IsPositive(ddouble.MaxValue);
            PrecisionAssert.IsFinite(ddouble.MinValue);
            PrecisionAssert.IsNegative(ddouble.MinValue);
            PrecisionAssert.AreEqual(double.MaxValue, ddouble.MaxValue.Hi);
            Assert.IsTrue(ddouble.MaxValue.Lo > 0d);
            Assert.IsTrue(double.MaxValue < ddouble.MaxValue);
            PrecisionAssert.AreEqual(-ddouble.MinValue, ddouble.MaxValue);
            Assert.AreNotEqual(ddouble.MaxValue, ddouble.BitIncrement(ddouble.MaxValue));
            Assert.AreNotEqual(ddouble.MinValue, ddouble.BitDecrement(ddouble.MinValue));

            PrecisionAssert.IsPositiveInfinity(ddouble.BitIncrement(ddouble.MaxValue));
            PrecisionAssert.IsNegativeInfinity(ddouble.BitDecrement(ddouble.MinValue));

            Console.WriteLine(double.MaxValue);
            Console.WriteLine(ddouble.MaxValue);
            Console.WriteLine($"{FloatSplitter.Split(ddouble.MaxValue).mantissa:X14}");
        }

        [TestMethod]
        public void ETest() {
            ddouble expected = (ddouble)"2.71828182845904523536028747135266250";
            ddouble actual = ddouble.E;

            Console.WriteLine(actual);

            PrecisionAssert.AlmostEqual(expected, actual, 2e-32);
            BitAssert.NeighborBits(expected, actual, 1);

            Console.WriteLine(ddouble.BitDecrement(expected) - actual);
            Console.WriteLine(ddouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");
        }

        [TestMethod]
        public void PiTest() {
            ddouble expected = (ddouble)"3.14159265358979323846264338327950288";
            ddouble actual = ddouble.Pi;
            Console.WriteLine(actual);

            PrecisionAssert.AlmostEqual(expected, actual, 2e-32);
            BitAssert.NeighborBits(expected, actual, 1);

            Console.WriteLine(ddouble.BitDecrement(expected) - actual);
            Console.WriteLine(ddouble.BitIncrement(expected) - actual);

            Console.WriteLine($"0x{FloatSplitter.Split(expected).mantissa:X14}");
            Console.WriteLine($"0x{FloatSplitter.Split(actual).mantissa:X14}");

            PrecisionAssert.AreEqual(ddouble.Pi, ddouble.Pi);
        }

        [TestMethod]
        public void Sqrt2Test() {
            PrecisionAssert.AlmostEqual(2, ddouble.Sqrt2 * ddouble.Sqrt2, 1e-31);
        }

        [TestMethod]
        public void ErdosBorweinTest() {
            PrecisionAssert.AlmostEqual(
                "1.6066951524152917637833015231909245804805796715057564357780795536",
                ddouble.ErdosBorwein,
                1e-31
            );
        }

        [TestMethod]
        public void FeigenbaumDeltaTest() {
            PrecisionAssert.AlmostEqual(
                "4.6692016091029906718532038204662016172581855774757686327456513430",
                ddouble.FeigenbaumDelta,
                1e-31
            );
        }

        [TestMethod]
        public void LemniscatePiTest() {
            PrecisionAssert.AlmostEqual(
                "2.6220575542921198104648395898911194136827549514316231628168217038",
                ddouble.LemniscatePi,
                1e-31
            );
        }

        [TestMethod]
        public void GoldenRatioTest() {
            PrecisionAssert.AlmostEqual(
                "1.6180339887498948482045868343656381177203091798057628621354486227",
                ddouble.GoldenRatio,
                1e-31
            );
        }

        [TestMethod]
        public void MalardiThetaTest() {
            PrecisionAssert.AlmostEqual(
                -(ddouble)1 / 3,
                ddouble.Cos(ddouble.MalardiTheta),
                2e-31
            );
        }

        [TestMethod]
        public void ConstantsExpectedTest() {
            PrecisionAssert.AreEqual(3.141592653589793238462, ddouble.Pi, 1e-15);
            PrecisionAssert.AreEqual(2.718281828459045235360, ddouble.E, 1e-15);
            PrecisionAssert.AreEqual(1.414213562373095048801, ddouble.Sqrt2, 1e-15);
            PrecisionAssert.AreEqual(0.301029995663981195213, ddouble.Lg2, 1e-15);
            PrecisionAssert.AreEqual(3.321928094887362347870, ddouble.Lb10, 1e-15);
            PrecisionAssert.AreEqual(0.693147180559945309417, ddouble.Ln2, 1e-15);
            PrecisionAssert.AreEqual(1.442695040888963407359, ddouble.LbE, 1e-15);
            PrecisionAssert.AreEqual(0.577215664901532860606, ddouble.EulerGamma, 1e-15);
            PrecisionAssert.AreEqual(1.202056903159594285399, ddouble.Zeta3, 1e-15);
            PrecisionAssert.AreEqual(1.036927755143369926331, ddouble.Zeta5, 1e-15);
            PrecisionAssert.AreEqual(1.008349277381922826839, ddouble.Zeta7, 1e-15);
            PrecisionAssert.AreEqual(1.002008392826082214418, ddouble.Zeta9, 1e-15);
            PrecisionAssert.AreEqual(1.461632144968362341263, ddouble.DigammaZero, 1e-15);
            PrecisionAssert.AreEqual(1.606695152415291763783, ddouble.ErdosBorwein, 1e-15);
            PrecisionAssert.AreEqual(4.669201609102990671853, ddouble.FeigenbaumDelta, 1e-15);
            PrecisionAssert.AreEqual(2.502907875095892822283, ddouble.FeigenbaumAlpha, 1e-15);
            PrecisionAssert.AreEqual(2.622057554292119810465, ddouble.LemniscatePi, 1e-15);
            PrecisionAssert.AreEqual(1.282427129100622636875, ddouble.GlaisherA, 1e-15);
            PrecisionAssert.AreEqual(0.915965594177219015055, ddouble.CatalanG, 1e-15);
            PrecisionAssert.AreEqual(2.807770242028519365222, ddouble.FransenRobinson, 1e-15);
            PrecisionAssert.AreEqual(2.685452001065306445310, ddouble.KhinchinK, 1e-15);
            PrecisionAssert.AreEqual(0.261497212847642783755, ddouble.MeisselMertens, 1e-15);
            PrecisionAssert.AreEqual(0.567143290409783873000, ddouble.LambertOmega, 1e-15);
            PrecisionAssert.AreEqual(0.764223653589220662991, ddouble.LandauRamanujan, 1e-15);
            PrecisionAssert.AreEqual(1.306377883863080690469, ddouble.MillsTheta, 1e-15);
            PrecisionAssert.AreEqual(1.451369234883381050284, ddouble.SoldnerMu, 1e-15);
            PrecisionAssert.AreEqual(0.822825249678847032995, ddouble.SierpinskiK, 1e-15);
            PrecisionAssert.AreEqual(3.359885666243177553172, ddouble.RcpFibonacci, 1e-15);
            PrecisionAssert.AreEqual(1.705211140105367764289, ddouble.Niven, 1e-15);
            PrecisionAssert.AreEqual(0.624329988543550870992, ddouble.GolombDickman, 1e-15);
            PrecisionAssert.AreEqual(1.618033988749894848204, ddouble.GoldenRatio, 1e-15);
            PrecisionAssert.AreEqual(1.910633236249018556327, ddouble.MalardiTheta, 1e-15);
        }
    }
}
