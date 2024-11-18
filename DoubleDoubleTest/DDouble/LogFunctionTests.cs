using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
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

                PrecisionAssert.AreEqual(double.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log2(v);

                PrecisionAssert.AreEqual(double.Log2((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log2_pzero = ddouble.Log2(0d);
            ddouble log2_mzero = ddouble.Log2(-0d);
            ddouble log2_pinf = ddouble.Log2(double.PositiveInfinity);
            ddouble log2_ninf = ddouble.Log2(double.NegativeInfinity);
            ddouble log2_nan = ddouble.Log2(double.NaN);

            PrecisionAssert.IsNegativeInfinity(log2_pzero, nameof(log2_pzero));
            PrecisionAssert.IsNaN(log2_mzero, nameof(log2_mzero));
            PrecisionAssert.IsPositiveInfinity(log2_pinf, nameof(log2_pinf));
            PrecisionAssert.IsNaN(log2_ninf, nameof(log2_ninf));
            PrecisionAssert.IsNaN(log2_nan, nameof(log2_nan));

            ddouble near2 = 2;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                PrecisionAssert.AreEqual(double.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 = ddouble.BitDecrement(near2);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                PrecisionAssert.AreEqual(double.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -100);
            }
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near2);
                PrecisionAssert.AreEqual(double.Log2(2), (double)u, 1e-12);

                Console.WriteLine($"{near2} {near2.Hi} {near2.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near2 -= ddouble.Ldexp(1, -50);
            }

            ddouble near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                PrecisionAssert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 -= ddouble.Ldexp(1, -100);
            }

            near1 = 1;
            for (int i = 0; i < 256; i++) {
                ddouble u = ddouble.Log2(near1);
                PrecisionAssert.AreEqual(0, (double)u, 1e-12);

                Console.WriteLine($"{near1} {near1.Hi} {near1.Lo}");
                Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                near1 += ddouble.Ldexp(1, -100);
            }
        }

        [TestMethod]
        public void Log10Test() {
            PrecisionAssert.AreEqual(0, (double)(ddouble.Log10(10) - 1), 1e-32);

            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                PrecisionAssert.AreEqual(double.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log10(v);

                PrecisionAssert.AreEqual(double.Log10((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (long i = 0, n = 1; i < 19; i++, n *= 10) {
                PrecisionAssert.AreEqual(i, ddouble.Log10(n));
                PrecisionAssert.AreEqual(-i, ddouble.Log10(ddouble.Rcp(n)));
            }

            ddouble log10_pzero = ddouble.Log10(0d);
            ddouble log10_mzero = ddouble.Log10(-0d);
            ddouble log10_pinf = ddouble.Log10(double.PositiveInfinity);
            ddouble log10_ninf = ddouble.Log10(double.NegativeInfinity);
            ddouble log10_nan = ddouble.Log10(double.NaN);

            PrecisionAssert.IsNegativeInfinity(log10_pzero, nameof(log10_pzero));
            PrecisionAssert.IsNaN(log10_mzero, nameof(log10_mzero));
            PrecisionAssert.IsPositiveInfinity(log10_pinf, nameof(log10_pinf));
            PrecisionAssert.IsNaN(log10_ninf, nameof(log10_ninf));
            PrecisionAssert.IsNaN(log10_nan, nameof(log10_nan));
        }

        [TestMethod]
        public void LogTest() {
            for (decimal d = 0.01m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                PrecisionAssert.AreEqual(double.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (decimal d = 10m; d <= +10000m; d += 10m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.Log(v);

                PrecisionAssert.AreEqual(double.Log((double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble log_pzero = ddouble.Log(0d);
            ddouble log_mzero = ddouble.Log(-0d);
            ddouble log_pinf = ddouble.Log(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Log(double.NegativeInfinity);
            ddouble log_nan = ddouble.Log(double.NaN);

            PrecisionAssert.IsNegativeInfinity(log_pzero, nameof(log_pzero));
            PrecisionAssert.IsNaN(log_mzero, nameof(log_mzero));
            PrecisionAssert.IsPositiveInfinity(log_pinf, nameof(log_pinf));
            PrecisionAssert.IsNaN(log_ninf, nameof(log_ninf));
            PrecisionAssert.IsNaN(log_nan, nameof(log_nan));
        }

        [TestMethod]
        public void LogBaseTest() {
            for (double b = 2; b <= 16; b += 0.5) {
                for (double p = -4; p <= 4; p += 0.25) {
                    ddouble x = ddouble.Pow(b, p);
                    ddouble y = ddouble.Log(x, b);

                    ddouble err = p - y;
                    if (ddouble.IsInteger(p)) {
                        PrecisionAssert.AreEqual(p, y, $"log_{b}({x}) err={err}");
                    }
                    else {
                        PrecisionAssert.AlmostEqual(p, y, 1e-31, $"log_{b}({x}) err={err}");
                    }
                }
            }

            for (double p = -4; p <= 4; p += 0.25) {
                for (double b = 2; b <= 16; b += 0.5) {
                    ddouble x = ddouble.Pow(b, p);
                    ddouble y = ddouble.Log(x, b);

                    ddouble err = p - y;
                    if (ddouble.IsInteger(p)) {
                        PrecisionAssert.AreEqual(p, y, $"log_{b}({x}) err={err}");
                    }
                    else {
                        PrecisionAssert.AlmostEqual(p, y, 1e-31, $"log_{b}({x}) err={err}");
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

                PrecisionAssert.AreEqual(double.Log(1 + (double)d), (double)u, 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            PrecisionAssert.AreEqual(0, ddouble.Log1p(ddouble.BitDecrement(0)), 1e-300);
            PrecisionAssert.AreEqual(0, ddouble.Log1p(ddouble.BitIncrement(0)), 1e-300);

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

            PrecisionAssert.IsPlusZero(log_pzero, nameof(log_pzero));
            PrecisionAssert.IsMinusZero(log_mzero, nameof(log_mzero));
            PrecisionAssert.IsPositiveInfinity(log_pinf, nameof(log_pinf));
            PrecisionAssert.IsNaN(log_ninf, nameof(log_ninf));
            PrecisionAssert.IsNaN(log_nan, nameof(log_nan));
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

                PrecisionAssert.AlmostEqual(expected, y, 1e-31d);
            }

            ddouble log_pzero = ddouble.Logit(0d);
            ddouble log_pone = ddouble.Logit(1d);
            ddouble log_ponepeps = ddouble.Logit(ddouble.BitIncrement(1d));
            ddouble log_mzero = ddouble.Logit(-0d);
            ddouble log_pinf = ddouble.Logit(double.PositiveInfinity);
            ddouble log_ninf = ddouble.Logit(double.NegativeInfinity);
            ddouble log_nan = ddouble.Logit(double.NaN);

            PrecisionAssert.IsNegativeInfinity(log_pzero, nameof(log_pzero));
            PrecisionAssert.IsPositiveInfinity(log_pone, nameof(log_pone));
            PrecisionAssert.IsNaN(log_ponepeps, nameof(log_ponepeps));
            PrecisionAssert.IsNaN(log_mzero, nameof(log_mzero));
            PrecisionAssert.IsNaN(log_pinf, nameof(log_pinf));
            PrecisionAssert.IsNaN(log_ninf, nameof(log_ninf));
            PrecisionAssert.IsNaN(log_nan, nameof(log_nan));
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

                PrecisionAssert.AlmostEqual(expected, y, 1e-31d);
            }

            ddouble exp_pinf = ddouble.Expit(double.PositiveInfinity);
            ddouble exp_ninf = ddouble.Expit(double.NegativeInfinity);
            ddouble exp_nan = ddouble.Expit(double.NaN);

            PrecisionAssert.AreEqual(1, exp_pinf, nameof(exp_pinf));
            PrecisionAssert.IsPlusZero(exp_ninf, nameof(exp_ninf));
            PrecisionAssert.IsNaN(exp_nan, nameof(exp_nan));
        }

        [TestMethod]
        public void Log1pExpectedTest() {
            ddouble[] expecteds = {
                "-0.1335313926245226231463436209313499745894",
                "-0.1290770422751423433458478313679858562578",
                "-0.1246424452072765973384933565912143044992",
                "-0.1202274269981598003244753948319154994491",
                "-0.1158318155251217050991200599386801665681",
                "-0.1114554409253228268966213677328042273655",
                "-0.1070981355563671005131126851708522185605",
                "-0.1027597339577689347753154133345778104977",
                "-0.09844007281325251990288857492897123488299",
                "-0.09413899091386191003563209699652506601503",
                "-0.08985632912186104707664693479686596242825",
                "-0.08559193033540351391614696866705119618235",
                "-0.08134563945395240588734235502936178438965",
                "-0.07711730334443128976966619326147591778297",
                "-0.07290677080808778056573748889092971130321",
                "-0.06871389254805180837469337740350344816620",
                "-0.06453852113757117167292391568399292812891",
                "-0.06038051098890747987144565295459680958674",
                "-0.05623971832287607779673769427697737688509",
                "-0.05211600113901401836163078705278402136655",
                "-0.04800921918636060775200362532344466213732",
                "-0.04391923393483549052639215155286544580423",
                "-0.03984590854719967065861624024730268350460",
                "-0.03578910785158527927534209821224040256135",
                "-0.03174869831458030115699628274852562992756",
                "-0.02772454801485486046713951145151638692720",
                "-0.02371652661731604211834685052867305795170",
                "-0.01972450534777858911927173265715930332466",
                "-0.01574835696813916860754951146082826952093",
                "-0.01178795575204224046916056189008712633991",
                "-0.007843177461025892873184042490943581654592",
                "-0.003913899321136329092317783643572664842706",
                0,
                "0.003898640415657323013937343095842907010724",
                "0.007782140442054948947462900061136763678126",
                "0.01165061721997527413559144280921434893316",
                "0.01550418653596525415085404604244683587787",
                "0.01934296284313093463590553454155047018549",
                "0.02316705928153437822879916096228991657941",
                "0.02697658769820207574806929253965954578148",
                "0.03077165866675368837102820759677216409170",
                "0.03455238150665973340737150058983286528158",
                "0.03831886430213659919375532512379729034596",
                "0.04207121392068705437520380592696237944815",
                "0.04580953603129420316667926761466334211393",
                "0.04953393512227663088209620882982457326705",
                "0.05324451451881228286587019378652877693957",
                "0.05694137640013842475901310154044949430153",
                "0.06062462181643484258060613204042026328620",
                "0.06429435070539725721622845026561149448584",
                "0.06795066190850774939456527777262941403463",
                "0.07159365318700881792560527275209203426891",
                "0.07522342123758752569860533998366241463687",
                "0.07884006170777602453154057785919829455902",
                "0.08244366921107459126816006866830780591410",
                "0.08603433734180315338179782672199607514092",
                "0.08961215868968713261995146937848452878519",
                "0.09317722485418328976878135302775939621588",
                "0.09672962645855111229557105648746343701504",
                "0.1002694531636751493081301751297276601964",
                "0.1037967936816435648260618037639746883066",
                "0.1073117357890880506671750303711543368065",
                "0.1108143663402901141948061693232119280985",
                "0.1143047712800586336342591448151747734095",
                "0.1177830356563834545387941094705217050685"
            };

            for ((int i, ddouble x) = (0, -1d / 8); i < expecteds.Length; i++, x += 1d / 256) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Log1p(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(expected, y, 2.5e-31d);
            }
        }
    }
}
