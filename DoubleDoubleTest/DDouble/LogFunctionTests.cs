using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class LogFunctionTests {
        [TestMethod]
        public void Log2Test() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log2(v);

                Assert.AreEqual(Math.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log2_pzero = ddouble.Log2(0d);
            ddouble log2_mzero = ddouble.Log2(-0d);
            ddouble log2_pinf = ddouble.Log2(double.PositiveInfinity);
            ddouble log2_ninf = ddouble.Log2(double.NegativeInfinity);
            ddouble log2_nan = ddouble.Log2(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log2_pzero), nameof(log2_pzero));
            Assert.IsTrue(ddouble.IsNaN(log2_mzero), nameof(log2_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log2_pinf), nameof(log2_pinf));
            Assert.IsTrue(ddouble.IsNaN(log2_ninf), nameof(log2_ninf));
            Assert.IsTrue(ddouble.IsNaN(log2_nan), nameof(log2_nan));

            ddouble near2 = 2;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 = ddouble.BitDecrement(near2);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -100);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                Assert.AreEqual(Math.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -50);
            }

            ddouble near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                Assert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 -= ddouble.Ldexp(1, -100);
            }

            near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                Assert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 += ddouble.Ldexp(1, -100);
            }
        }

        [TestMethod]
        public void Log10Test() {
            Assert.AreEqual(0, (double)(ddouble.Log10(10) - 1), 1e-32);

            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                Assert.AreEqual(Math.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (long i = 0, n = 1; i < 19; i++, n *= 10) {
                Assert.AreEqual(i, ddouble.Log10(n));
                Assert.AreEqual(-i, ddouble.Log10(ddouble.Rcp(n)));
            }

            ddouble log10_pzero = ddouble.Log10(0d);
            ddouble log10_mzero = ddouble.Log10(-0d);
            ddouble log10_pinf = ddouble.Log10(double.PositiveInfinity);
            ddouble log10_ninf = ddouble.Log10(double.NegativeInfinity);
            ddouble log10_nan = ddouble.Log10(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log10_pzero), nameof(log10_pzero));
            Assert.IsTrue(ddouble.IsNaN(log10_mzero), nameof(log10_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log10_pinf), nameof(log10_pinf));
            Assert.IsTrue(ddouble.IsNaN(log10_ninf), nameof(log10_ninf));
            Assert.IsTrue(ddouble.IsNaN(log10_nan), nameof(log10_nan));
        }

        [TestMethod]
        public void LogTest() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                Assert.AreEqual(Math.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log_pzero = ddouble.Log(0d);
            ddouble log_mzero = ddouble.Log(-0d);
            ddouble log_pinf = ddouble.Log(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Log(double.NegativeInfinity);
            ddouble log_nan = ddouble.Log(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log_pzero), nameof(log_pzero));
            Assert.IsTrue(ddouble.IsNaN(log_mzero), nameof(log_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log_pinf), nameof(log_pinf));
            Assert.IsTrue(ddouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(ddouble.IsNaN(log_nan), nameof(log_nan));
        }

        [TestMethod]
        public void LogBaseTest() {
            for (double b = 2; b <= 16; b += 0.5) {
                for (double p = -4; p <= 4; p += 0.25) {
                    ddouble x = ddouble.Pow(b, p);
                    ddouble y = ddouble.Log(x, b);

                    ddouble err = p - y;
                    if (ddouble.IsInteger(p)) {
                        Assert.AreEqual((ddouble)p, y, $"log_{b}({x}) err={err}");
                    }
                    else {
                        HPAssert.AreEqual((ddouble)p, y, 1e-31, $"log_{b}({x}) err={err}");
                    }
                }
            }

            for (double p = -4; p <= 4; p += 0.25) {
                for (double b = 2; b <= 16; b += 0.5) {
                    ddouble x = ddouble.Pow(b, p);
                    ddouble y = ddouble.Log(x, b);

                    ddouble err = p - y;
                    if (ddouble.IsInteger(p)) {
                        Assert.AreEqual((ddouble)p, y, $"log_{b}({x}) err={err}");
                    }
                    else {
                        HPAssert.AreEqual((ddouble)p, y, 1e-31, $"log_{b}({x}) err={err}");
                    }
                }
            }
        }

        [TestMethod]
        public void Log1pTest() {
            for (decimal d = -0.99m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log1p(v);

                Assert.AreEqual(Math.Log(1 + (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            HPAssert.AreEqual(0, ddouble.Log1p(ddouble.BitDecrement(0)), 1e-300);
            HPAssert.AreEqual(0, ddouble.Log1p(ddouble.BitIncrement(0)), 1e-300);

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(0)));
            Console.WriteLine(ddouble.Log1p(0));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(0)));

            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitDecrement(-0.0625d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(-0.0625d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitIncrement(-0.0625d))).mantissa);

            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitDecrement(0.0625d))).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(0.0625d)).mantissa);
            Console.WriteLine(FloatSplitter.Split(ddouble.Log1p(ddouble.BitIncrement(0.0625d))).mantissa);

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(-0.0625d)));
            Console.WriteLine(ddouble.Log1p(-0.0625d));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(-0.0625d)));

            Console.WriteLine(ddouble.Log1p(ddouble.BitDecrement(0.0625d)));
            Console.WriteLine(ddouble.Log1p(0.0625d));
            Console.WriteLine(ddouble.Log1p(ddouble.BitIncrement(0.0625d)));

            ddouble log_pzero = ddouble.Log1p(0d);
            ddouble log_mzero = ddouble.Log1p(-0d);
            ddouble log_pinf = ddouble.Log1p(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Log1p(double.NegativeInfinity);
            ddouble log_nan = ddouble.Log1p(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(log_pzero), nameof(log_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(log_mzero), nameof(log_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log_pinf), nameof(log_pinf));
            Assert.IsTrue(ddouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(ddouble.IsNaN(log_nan), nameof(log_nan));
        }

        [TestMethod]
        public void LogitTest() {
            ddouble[] expecteds = {
                "-3.43398720448514624592916432454235721044994e0",
                "-2.70805020110221006599600457014871334417309e0",
                "-2.26868354131836433578802679543938590084702e0",
                "-1.94591014905531330510535274344317972963709e0",
                "-1.68639895357022869958497637754138947441687e0",
                "-1.46633706879342704465824220464279290015778e0",
                "-1.27296567581288744409616592300919554941412e0",
                "-1.09861228866810969139524523692252570464749e0",
                "-9.38269638592930308016262357965144709147399e-1",
                "-7.88457360364270169461184244738941660296105e-1",
                "-6.46627164925052452438654402400576134462869e-1",
                "-5.10825623765990683205514096303661934878111e-1",
                "-3.79489621704903723955539990322534932432111e-1",
                "-2.51314428280906077685137730401871679657897e-1",
                "-1.25163142954006014253530047724413191415111e-1",
                0,
                "1.25163142954006014253530047724413191415111e-1",
                "2.51314428280906077685137730401871679657896e-1",
                "3.79489621704903723955539990322534932432111e-1",
                "5.10825623765990683205514096303661934878111e-1",
                "6.46627164925052452438654402400576134462869e-1",
                "7.88457360364270169461184244738941660296105e-1",
                "9.38269638592930308016262357965144709147399e-1",
                "1.09861228866810969139524523692252570464749e0",
                "1.27296567581288744409616592300919554941412e0",
                "1.46633706879342704465824220464279290015778e0",
                "1.68639895357022869958497637754138947441687e0",
                "1.94591014905531330510535274344317972963709e0",
                "2.26868354131836433578802679543938590084702e0",
                "2.70805020110221006599600457014871334417309e0",
                "3.43398720448514624592916432454235721044994e0"
            };

            for ((int i, ddouble x) = (0, 1d / 32); i < expecteds.Length; i++, x += 1d / 32) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Logit(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                HPAssert.AreEqual(expected, y, ddouble.Abs(expected) * 1e-31d);
            }

            ddouble log_pzero = ddouble.Logit(0d);
            ddouble log_pone = ddouble.Logit(1d);
            ddouble log_ponepeps = ddouble.Logit(ddouble.BitIncrement(1d));
            ddouble log_mzero = ddouble.Logit(-0d);
            ddouble log_pinf = ddouble.Logit(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Logit(double.NegativeInfinity);
            ddouble log_nan = ddouble.Logit(double.NaN);

            Assert.IsTrue(ddouble.IsNegativeInfinity(log_pzero), nameof(log_pzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(log_pone), nameof(log_pone));
            Assert.IsTrue(ddouble.IsNaN(log_ponepeps), nameof(log_ponepeps));
            Assert.IsTrue(ddouble.IsNaN(log_mzero), nameof(log_mzero));
            Assert.IsTrue(ddouble.IsNaN(log_pinf), nameof(log_pinf));
            Assert.IsTrue(ddouble.IsNaN(log_ninf), nameof(log_ninf));
            Assert.IsTrue(ddouble.IsNaN(log_nan), nameof(log_nan));
        }

        [TestMethod]
        public void ExpitTest() {
            ddouble[] expecteds = {
                "1.798620996209155802679313794953842487249e-2",
                "2.297736991002561495390388669015859139171e-2",
                "2.931223075135631886528955811418158389418e-2",
                "3.732688734412946019771416603849814526022e-2",
                "4.742587317756678087884815177175220138618e-2",
                "6.008665017400762201972344559490871336252e-2",
                "7.585818002124355119330617664624777313071e-2",
                "9.534946489910949396961476322833650130171e-2",
                "1.192029220221175559402708586976032047936e-1",
                "1.480471980316894697061848651843231254462e-1",
                "1.824255238063563403927828213437517524538e-1",
                "2.22700138825308853000480452338458201971e-1",
                "2.689414213699951207488407581781637256349e-1",
                "3.208213008246070268403198842234209787658e-1",
                "3.775406687981454353610994342544915212467e-1",
                "4.378234991142018959726763620970536461621e-1",
                0.5,
                "5.62176500885798104027323637902946353838e-1",
                "6.224593312018545646389005657455084787533e-1",
                "6.791786991753929731596801157765790212342e-1",
                "7.310585786300048792511592418218362743651e-1",
                "7.77299861174691146999519547661541798029e-1",
                "8.175744761936436596072171786562482475462e-1",
                "8.519528019683105302938151348156768745538e-1",
                "8.807970779778824440597291413023967952064e-1",
                "9.046505351008905060303852367716634986983e-1",
                "9.241418199787564488066938233537522268693e-1",
                "9.399133498259923779802765544050912866375e-1",
                "9.525741268224332191211518482282477986138e-1",
                "9.626731126558705398022858339615018547398e-1",
                "9.706877692486436811347104418858184161058e-1",
                "9.770226300899743850460961133098414086083e-1",
                "9.820137900379084419732068620504615751275e-1"
            };

            for ((int i, ddouble x) = (0, -4); i < expecteds.Length; i++, x += 1d / 4) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Expit(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                HPAssert.AreEqual(expected, y, ddouble.Abs(expected) * 1e-31d);
            }

            ddouble exp_pinf = ddouble.Expit(double.PositiveInfinity);
            ddouble exp_ninf = ddouble.Expit(double.NegativeInfinity);
            ddouble exp_nan = ddouble.Expit(double.NaN);

            Assert.IsTrue(exp_pinf == 1d, nameof(exp_pinf));
            Assert.IsTrue(ddouble.IsPlusZero(exp_ninf), nameof(exp_ninf));
            Assert.IsTrue(ddouble.IsNaN(exp_nan), nameof(exp_nan));
        }
    }
}
