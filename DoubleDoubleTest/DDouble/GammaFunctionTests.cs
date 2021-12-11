using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class GammaFunctionTests {

        [TestMethod]
        public void GammaTest() {
            for (BigInteger i = 1, y = 1; i <= 35; i++, y = y * (i - 1)) {
                ddouble x = ddouble.Gamma(i);

                Assert.AreEqual(y, x);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 30; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.Gamma((2 * (int)i - 1) * 0.5d);
                ddouble v = sqrtpi * z / y;

                HPAssert.NeighborBits(v, x, 128);
            }

            HPAssert.NeighborBits(sqrtpi * 4 / 3, ddouble.Gamma(-1.5), 128);
            HPAssert.NeighborBits(sqrtpi * -2, ddouble.Gamma(-0.5), 128);

            HPAssert.NeighborBits("1.2254167024651776451290983033628905268512", ddouble.Gamma(0.75), 128);
            HPAssert.NeighborBits("9.3326215443944152681699238856266700490716e155", ddouble.Gamma(100), 128);
            HPAssert.NeighborBits("2.9467022724950383265043395073512148621950e282", ddouble.Gamma(160), 128);

            ddouble gamma_pzero = ddouble.Gamma(0d);
            ddouble gamma_mzero = ddouble.Gamma(-0d);
            ddouble gamma_mone = ddouble.Gamma(-1d);
            ddouble gamma_pinf = ddouble.Gamma(double.PositiveInfinity);
            ddouble gamma_ninf = ddouble.Gamma(double.NegativeInfinity);
            ddouble gamma_nan = ddouble.Gamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_pzero), nameof(gamma_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_mzero), nameof(gamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(gamma_mone), nameof(gamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(gamma_pinf), nameof(gamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(gamma_ninf), nameof(gamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(gamma_nan), nameof(gamma_nan));
        }

        [TestMethod]
        public void LogGammaTest() {
            for (BigInteger i = 1, y = 1; i <= 35; i++, y = y * (i - 1)) {
                ddouble x = ddouble.LogGamma(i);
                ddouble v = ddouble.Log(y);

                HPAssert.NeighborBits(v, x, 128);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 30; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.LogGamma((2 * (int)i - 1) * 0.5d);
                ddouble v = ddouble.Log(sqrtpi * z / y);

                HPAssert.NeighborBits(v, x, 128);
            }

            HPAssert.NeighborBits("1.288022524698077457370610440219717295925", ddouble.LogGamma(0.25), 192);
            HPAssert.NeighborBits("2.032809514312953714814329718624296997597e-1", ddouble.LogGamma(0.75), 192);
            HPAssert.NeighborBits("3.591342053695753987760440104602869096126e2", ddouble.LogGamma(100), 128);
            HPAssert.NeighborBits("8.579336698258574368182534016573082801626e2", ddouble.LogGamma(200), 128);

            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (0.999755859375d, "1.409708218759223705137152436852900411083e-4"),
                (1.000244140625d, "-1.408727761632663509703058417841195184579e-4"),
                (1.999755859375d, "-1.0319961029799208618175017469614360195e-4"),
                (2.000244140625d, "1.032380513640963581901732440393341810164e-4"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x), x_dec2 = ddouble.BitDecrement(x_dec), x_dec3 = ddouble.BitDecrement(x_dec2);
                ddouble x_inc = ddouble.BitIncrement(x), x_inc2 = ddouble.BitIncrement(x_inc), x_inc3 = ddouble.BitIncrement(x_inc2);

                Console.WriteLine(ddouble.LogGamma(x_dec3));
                Console.WriteLine(ddouble.LogGamma(x_dec2));
                Console.WriteLine(ddouble.LogGamma(x_dec));
                Console.WriteLine(ddouble.LogGamma(x));
                Console.WriteLine(ddouble.LogGamma(x_inc));
                Console.WriteLine(ddouble.LogGamma(x_inc2));
                Console.WriteLine(ddouble.LogGamma(x_inc3));

                HPAssert.AreEqual(expected, ddouble.LogGamma(x_dec), 1e-29);
                HPAssert.AreEqual(expected, ddouble.LogGamma(x), 1e-29);
                HPAssert.AreEqual(expected, ddouble.LogGamma(x_inc), 1e-29);
            }

            ddouble loggamma_pzero = ddouble.LogGamma(0d);
            ddouble loggamma_mzero = ddouble.LogGamma(-0d);
            ddouble loggamma_mone = ddouble.LogGamma(-1d);
            ddouble loggamma_pinf = ddouble.LogGamma(double.PositiveInfinity);
            ddouble loggamma_ninf = ddouble.LogGamma(double.NegativeInfinity);
            ddouble loggamma_nan = ddouble.LogGamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(loggamma_pzero), nameof(loggamma_pzero));
            Assert.IsTrue(ddouble.IsNaN(loggamma_mzero), nameof(loggamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(loggamma_mone), nameof(loggamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(loggamma_pinf), nameof(loggamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(loggamma_ninf), nameof(loggamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(loggamma_nan), nameof(loggamma_nan));
        }

        [TestMethod]
        public void DigammaTest() {
            for (int i = 1; i <= 35; i++) {
                ddouble x = ddouble.Digamma(i);
                ddouble y = ddouble.HarmonicNumber(i - 1) - ddouble.EulerGamma;

                HPAssert.NeighborBits(y, x, 128);
            }

            HPAssert.NeighborBits(-2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(0.5d), 128);
            HPAssert.NeighborBits(2 * (ddouble)340028535787 / 145568097675
                - 2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(15.5d), 128);

            HPAssert.NeighborBits(2 * (ddouble)10686452707072 / 4512611027925
                - 2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(16.5d), 128);

            HPAssert.NeighborBits("-2.8941202000429320747561968127633502440339e0", ddouble.Digamma(-0.75), 128);
            HPAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(-0.5), 128);
            HPAssert.NeighborBits("2.9141391202135278303731132371828193068299e0", ddouble.Digamma(-0.25), 128);
            HPAssert.NeighborBits("-4.2274535333762654080895301460966835773672e0", ddouble.Digamma(0.25), 128);
            HPAssert.NeighborBits("-1.9635100260214234794409763329987555671932e0", ddouble.Digamma(0.5), 128);
            HPAssert.NeighborBits("-1.0858608797864721696268867628171806931701e0", ddouble.Digamma(0.75), 128);
            HPAssert.NeighborBits("-2.2745353337626540808953014609668357736724e-1", ddouble.Digamma(1.25), 128);
            HPAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(1.5), 128);
            HPAssert.NeighborBits("2.4747245354686116370644657051615264016326e-1", ddouble.Digamma(1.75), 128);
            HPAssert.NeighborBits("4.6001618527380874001986055855758507268668e0", ddouble.Digamma(100), 128);
            HPAssert.NeighborBits("5.2958152832199116154508743070484592057952e0", ddouble.Digamma(200), 128);

            ddouble zeropoint = "1.461632144968362341262659542325721328468196204006446351295988408598";

            Console.WriteLine(ddouble.Digamma(zeropoint));

            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (zeropoint - 9.765625e-4d, "-9.4541491995331660375624084239222499346063e-4"),
                (zeropoint + 9.765625e-4d, "+9.4457041593008585021273363326293833498992e-4"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x), x_dec2 = ddouble.BitDecrement(x_dec), x_dec3 = ddouble.BitDecrement(x_dec2);
                ddouble x_inc = ddouble.BitIncrement(x), x_inc2 = ddouble.BitIncrement(x_inc), x_inc3 = ddouble.BitIncrement(x_inc2);

                Console.WriteLine(ddouble.Digamma(x_dec3));
                Console.WriteLine(ddouble.Digamma(x_dec2));
                Console.WriteLine(ddouble.Digamma(x_dec));
                Console.WriteLine(ddouble.Digamma(x));
                Console.WriteLine(ddouble.Digamma(x_inc));
                Console.WriteLine(ddouble.Digamma(x_inc2));
                Console.WriteLine(ddouble.Digamma(x_inc3));

                HPAssert.AreEqual(expected, ddouble.Digamma(x_dec), 1e-28);
                HPAssert.AreEqual(expected, ddouble.Digamma(x), 1e-28);
                HPAssert.AreEqual(expected, ddouble.Digamma(x_inc), 1e-28);
            }

            ddouble digamma_pzero = ddouble.Digamma(0d);
            ddouble digamma_mzero = ddouble.Digamma(-0d);
            ddouble digamma_mone = ddouble.Digamma(-1d);
            ddouble digamma_pinf = ddouble.Digamma(double.PositiveInfinity);
            ddouble digamma_ninf = ddouble.Digamma(double.NegativeInfinity);
            ddouble digamma_nan = ddouble.Digamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pzero), nameof(digamma_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_mzero), nameof(digamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(digamma_mone), nameof(digamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pinf), nameof(digamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(digamma_ninf), nameof(digamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(digamma_nan), nameof(digamma_nan));
        }
    }
}
