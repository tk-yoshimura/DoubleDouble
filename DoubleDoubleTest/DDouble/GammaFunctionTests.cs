using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using static DoubleDouble.ddouble.Consts;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class GammaFunctionTests {

        [TestMethod]
        public void GammaTest() {
            for (BigInteger i = 1, y = 1; i <= 36; i++, y = y * (i - 1)) {
                ddouble x = ddouble.Gamma(i);

                Assert.AreEqual(y, x);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 40; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.Gamma((2 * (int)i - 1) * 0.5d);
                ddouble v = sqrtpi * z / y;

                HPAssert.NeighborBits(v, x, 17);
            }

            HPAssert.NeighborBits(sqrtpi * 4 / 3, ddouble.Gamma(-1.5), 16);
            HPAssert.NeighborBits(sqrtpi * -2, ddouble.Gamma(-0.5), 16);

            HPAssert.NeighborBits("1.2254167024651776451290983033628905268512", ddouble.Gamma(0.75), 16);
            HPAssert.NeighborBits("9.3326215443944152681699238856266700490716e155", ddouble.Gamma(100), 64);
            HPAssert.NeighborBits("2.9467022724950383265043395073512148621950e282", ddouble.Gamma(160), 64);

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

                HPAssert.NeighborBits(v, x, 32);
            }

            ddouble sqrtpi = ddouble.Sqrt(ddouble.PI);

            for (BigInteger i = 1, y = 1, z = 1; i <= 30; i++, y *= 2, z *= 2 * i - 3) {
                ddouble x = ddouble.LogGamma((2 * (int)i - 1) * 0.5d);
                ddouble v = ddouble.Log(sqrtpi * z / y);

                HPAssert.NeighborBits(v, x, 32);
            }

            HPAssert.NeighborBits("1.288022524698077457370610440219717295925", ddouble.LogGamma(0.25), 32);
            HPAssert.NeighborBits("2.032809514312953714814329718624296997597e-1", ddouble.LogGamma(0.75), 32);
            HPAssert.NeighborBits("3.591342053695753987760440104602869096126e2", ddouble.LogGamma(100), 128);
            HPAssert.NeighborBits("8.579336698258574368182534016573082801626e2", ddouble.LogGamma(200), 128);

            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (1 - Math.ScaleB(1, -3), "8.5858707225334323502365583769487702270e-2"),
                (1 + Math.ScaleB(1, -3), "-6.0023184126039582931405843207430114278e-2"),
                (2 - Math.ScaleB(1, -3), "-4.7672685399188299643978037161862272345e-2"),
                (2 + Math.ScaleB(1, -3), "5.7759851530343871607388266263091590793e-2"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x), x_dec2 = ddouble.BitDecrement(x_dec), x_dec3 = ddouble.BitDecrement(x_dec2);
                ddouble x_inc = ddouble.BitIncrement(x), x_inc2 = ddouble.BitIncrement(x_inc), x_inc3 = ddouble.BitIncrement(x_inc2);

                Console.WriteLine(expected);
                Console.WriteLine(ddouble.LogGamma(x_dec3));
                Console.WriteLine(ddouble.LogGamma(x_dec2));
                Console.WriteLine(ddouble.LogGamma(x_dec));
                Console.WriteLine(ddouble.LogGamma(x));
                Console.WriteLine(ddouble.LogGamma(x_inc));
                Console.WriteLine(ddouble.LogGamma(x_inc2));
                Console.WriteLine(ddouble.LogGamma(x_inc3));
                Console.WriteLine("");

                HPAssert.AreEqual(expected, ddouble.LogGamma(x_dec), 1e-30);
                HPAssert.AreEqual(expected, ddouble.LogGamma(x), 1e-30);
                HPAssert.AreEqual(expected, ddouble.LogGamma(x_inc), 1e-30);
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

                HPAssert.NeighborBits(y, x, 32);
            }

            HPAssert.NeighborBits(-2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(0.5d), 32);
            HPAssert.NeighborBits(2 * (ddouble)340028535787 / 145568097675
                - 2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(15.5d), 32);

            HPAssert.NeighborBits(2 * (ddouble)10686452707072 / 4512611027925
                - 2 * ddouble.Log(2) - ddouble.EulerGamma, ddouble.Digamma(16.5d), 32);

            HPAssert.NeighborBits("-2.8941202000429320747561968127633502440339e0", ddouble.Digamma(-0.75), 32);
            HPAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(-0.5), 32);
            HPAssert.NeighborBits("2.9141391202135278303731132371828193068299e0", ddouble.Digamma(-0.25), 2);
            HPAssert.NeighborBits("-4.2274535333762654080895301460966835773672e0", ddouble.Digamma(0.25), 32);
            HPAssert.NeighborBits("-1.9635100260214234794409763329987555671932e0", ddouble.Digamma(0.5), 32);
            HPAssert.NeighborBits("-1.0858608797864721696268867628171806931701e0", ddouble.Digamma(0.75), 32);
            HPAssert.NeighborBits("-2.2745353337626540808953014609668357736724e-1", ddouble.Digamma(1.25), 32);
            HPAssert.NeighborBits("3.6489973978576520559023667001244432806840e-2", ddouble.Digamma(1.5), 32);
            HPAssert.NeighborBits("2.4747245354686116370644657051615264016326e-1", ddouble.Digamma(1.75), 32);
            HPAssert.NeighborBits("4.6001618527380874001986055855758507268668e0", ddouble.Digamma(100), 128);
            HPAssert.NeighborBits("5.2958152832199116154508743070484592057952e0", ddouble.Digamma(200), 128);

            ddouble zeropoint = "1.461632144968362341262659542325721328468196204006446351295988408598";

            Console.WriteLine(ddouble.Digamma(zeropoint));

            foreach ((ddouble x, ddouble expected) in new (ddouble, ddouble)[] {
                (zeropoint - Math.ScaleB(1, -3), "-0.128425703997196096307000073904848726435618594702456926759961027"),
                (zeropoint + Math.ScaleB(1, -3), "+0.114508749996322817880325006697673040445328929804493417473187312"),
            }) {
                ddouble x_dec = ddouble.BitDecrement(x), x_dec2 = ddouble.BitDecrement(x_dec), x_dec3 = ddouble.BitDecrement(x_dec2);
                ddouble x_inc = ddouble.BitIncrement(x), x_inc2 = ddouble.BitIncrement(x_inc), x_inc3 = ddouble.BitIncrement(x_inc2);

                Console.WriteLine(expected);
                Console.WriteLine(ddouble.Digamma(x_dec3));
                Console.WriteLine(ddouble.Digamma(x_dec2));
                Console.WriteLine(ddouble.Digamma(x_dec));
                Console.WriteLine(ddouble.Digamma(x));
                Console.WriteLine(ddouble.Digamma(x_inc));
                Console.WriteLine(ddouble.Digamma(x_inc2));
                Console.WriteLine(ddouble.Digamma(x_inc3));
                Console.WriteLine("");

                HPAssert.AreEqual(expected, ddouble.Digamma(x_dec), 1e-31);
                HPAssert.AreEqual(expected, ddouble.Digamma(x), 1e-31);
                HPAssert.AreEqual(expected, ddouble.Digamma(x_inc), 1e-31);
            }

            ddouble digamma_pzero = ddouble.Digamma(0d);
            ddouble digamma_mzero = ddouble.Digamma(-0d);
            ddouble digamma_mone = ddouble.Digamma(-1d);
            ddouble digamma_pinf = ddouble.Digamma(double.PositiveInfinity);
            ddouble digamma_pmax = ddouble.Digamma(double.MaxValue);
            ddouble digamma_ninf = ddouble.Digamma(double.NegativeInfinity);
            ddouble digamma_nan = ddouble.Digamma(double.NaN);

            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pzero), nameof(digamma_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_mzero), nameof(digamma_mzero));
            Assert.IsTrue(ddouble.IsNaN(digamma_mone), nameof(digamma_mone));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pinf), nameof(digamma_pinf));
            Assert.IsTrue(ddouble.IsFinite(digamma_pmax), nameof(digamma_pmax));
            Assert.IsTrue(ddouble.IsNaN(digamma_ninf), nameof(digamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(digamma_nan), nameof(digamma_nan));
        }

        [TestMethod]
        public void InverseGammaTest() {
            for (double h = 1; h <= Math.ScaleB(1, 400); h *= 2) {
                for (double x = h; x < h * 2; x += h / 64) {
                    ddouble y = ddouble.InverseGamma(x);
                    ddouble z = ddouble.Gamma(y);

                    Console.WriteLine(x);
                    Console.WriteLine(y);
                    Console.WriteLine(z);

                    HPAssert.AreEqual(x, z, x * 8e-30);
                }
            }

            for (double h = Math.ScaleB(1, 401); h <= Math.ScaleB(1, 1020); h *= 2) {
                for (double x = h; x < h * 2; x += h / 64) {
                    ddouble y = ddouble.InverseGamma(x);
                    ddouble z = ddouble.Gamma(y);

                    Console.WriteLine(x);
                    Console.WriteLine(y);
                    Console.WriteLine(z);

                    HPAssert.AreEqual(x, z, x * 2e-29);
                }
            }

            for (ddouble x = 2; x <= 160; x += 1) {
                ddouble y = ddouble.Gamma(x);
                ddouble z = ddouble.InverseGamma(y);

                Console.WriteLine(x);
                Console.WriteLine(y);
                Console.WriteLine(z);

                Assert.AreEqual(x, z);
            }

            ddouble digamma_p0p999 = ddouble.InverseGamma(0.999);
            ddouble digamma_pinf = ddouble.InverseGamma(double.PositiveInfinity);
            ddouble digamma_ninf = ddouble.InverseGamma(double.NegativeInfinity);
            ddouble digamma_nan = ddouble.InverseGamma(double.NaN);

            Assert.IsTrue(ddouble.IsNaN(digamma_p0p999), nameof(digamma_p0p999));
            Assert.IsTrue(ddouble.IsPositiveInfinity(digamma_pinf), nameof(digamma_pinf));
            Assert.IsTrue(ddouble.IsNaN(digamma_ninf), nameof(digamma_ninf));
            Assert.IsTrue(ddouble.IsNaN(digamma_nan), nameof(digamma_nan));
        }
    }
}
