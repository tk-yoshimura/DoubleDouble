using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class TrigonFunctionTests {

        [TestMethod]
        public void SinPiHalfTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPiHalf(v);

                PrecisionAssert.AreEqual(double.Sin((double)d * double.Pi / 2), (double)u, 1e-14);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (int n = -8; n <= 8; n++) {
                ddouble nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPiHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    PrecisionAssert.AreEqual(double.Sin(n * double.Pi / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitDecrement(nearn);
                }

                nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPiHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    PrecisionAssert.AreEqual(double.Sin(n * double.Pi / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitIncrement(nearn);
                }
            }

            PrecisionAssert.AlmostEqual("6.13588464915447535964023459037258092e-3", ddouble.SinPiHalf(ddouble.Rcp(256)), 1e-31);
            PrecisionAssert.AlmostEqual("1.56434465040230869010105319467166892e-1", ddouble.SinPiHalf(ddouble.Rcp(10)), 1e-31);
            PrecisionAssert.AlmostEqual("9.87688340595137726190040247693437261e-1", ddouble.SinPiHalf(9 * ddouble.Rcp(10)), 1e-31);
            PrecisionAssert.AlmostEqual(0.5d, ddouble.SinPiHalf(ddouble.Rcp(3)), 1e-31);
            PrecisionAssert.AlmostEqual(ddouble.Sqrt(3) / 2, ddouble.SinPiHalf(2 * ddouble.Rcp(3)), 1e-31);
            PrecisionAssert.AreEqual(ddouble.Sqrt2 / 2, ddouble.SinPiHalf(0.5d));
            PrecisionAssert.AlmostEqual(ddouble.Sqrt2 / 2, ddouble.SinPiHalf(ddouble.BitDecrement(0.5d)), 1e-31);
            PrecisionAssert.AlmostEqual(ddouble.Sqrt2 / 2, ddouble.SinPiHalf(ddouble.BitIncrement(0.5d)), 1e-31);

            ddouble sin_pzero = ddouble.SinPiHalf(0d);
            ddouble sin_mzero = ddouble.SinPiHalf(-0d);
            ddouble sin_pinf = ddouble.SinPiHalf(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPiHalf(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPiHalf(double.NaN);

            PrecisionAssert.IsPlusZero(sin_pzero, nameof(sin_pzero));
            PrecisionAssert.IsMinusZero(sin_mzero, nameof(sin_mzero));
            PrecisionAssert.IsNaN(sin_pinf, nameof(sin_pinf));
            PrecisionAssert.IsNaN(sin_ninf, nameof(sin_ninf));
            PrecisionAssert.IsNaN(sin_nan, nameof(sin_nan));
        }

        [TestMethod]
        public void SinPiTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPi(v);

                PrecisionAssert.AreEqual(double.Sin((double)d * double.Pi), (double)u, 1e-14);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.SinPi(0d);
            ddouble sin_mzero = ddouble.SinPi(-0d);
            ddouble sin_pinf = ddouble.SinPi(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPi(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPi(double.NaN);

            PrecisionAssert.IsPlusZero(sin_pzero, nameof(sin_pzero));
            PrecisionAssert.IsMinusZero(sin_mzero, nameof(sin_mzero));
            PrecisionAssert.IsNaN(sin_pinf, nameof(sin_pinf));
            PrecisionAssert.IsNaN(sin_ninf, nameof(sin_ninf));
            PrecisionAssert.IsNaN(sin_nan, nameof(sin_nan));

            for (int n = 1; n <= 32; n++) {
                PrecisionAssert.IsPlusZero(ddouble.SinPi(n), "sin intn");
                PrecisionAssert.IsMinusZero(ddouble.SinPi(-n), "sin intn");
            }
        }

        [TestMethod]
        public void CosPiTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.CosPi(v);

                PrecisionAssert.AreEqual(double.Cos((double)d * double.Pi), (double)u, 1e-14);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.CosPi(0d);
            ddouble cos_mzero = ddouble.CosPi(-0d);
            ddouble cos_pinf = ddouble.CosPi(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.CosPi(double.NegativeInfinity);
            ddouble cos_nan = ddouble.CosPi(double.NaN);

            PrecisionAssert.AreEqual(1, cos_pzero, nameof(cos_pzero));
            PrecisionAssert.AreEqual(1, cos_mzero, nameof(cos_mzero));
            PrecisionAssert.IsNaN(cos_pinf, nameof(cos_pinf));
            PrecisionAssert.IsNaN(cos_ninf, nameof(cos_ninf));
            PrecisionAssert.IsNaN(cos_nan, nameof(cos_nan));

            for (int n = 0; n <= 32; n++) {
                PrecisionAssert.IsPlusZero(ddouble.CosPi(n + 0.5), "cos intn");
                PrecisionAssert.IsPlusZero(ddouble.CosPi(-n - 0.5), "cos intn");
            }
        }

        [TestMethod]
        public void TanPiTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (decimal.Abs(d) % 1m == 0.5m) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.TanPi(v);

                double w = double.Tan((double)d * double.Pi);

                PrecisionAssert.AreEqual(w, (double)u, double.Max(1e-14, double.Abs(w) * 1e-13));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.TanPi(0d);
            ddouble tan_mzero = ddouble.TanPi(-0d);
            ddouble tan_pinf = ddouble.TanPi(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.TanPi(double.NegativeInfinity);
            ddouble tan_nan = ddouble.TanPi(double.NaN);

            PrecisionAssert.IsPlusZero(tan_pzero, nameof(tan_pzero));
            PrecisionAssert.IsMinusZero(tan_mzero, nameof(tan_mzero));
            PrecisionAssert.IsNaN(tan_pinf, nameof(tan_pinf));
            PrecisionAssert.IsNaN(tan_ninf, nameof(tan_ninf));
            PrecisionAssert.IsNaN(tan_nan, nameof(tan_nan));

            for (int n = 1; n <= 32; n++) {
                Assert.AreEqual(double.Sign(double.TanPi(n)), ddouble.Sign(ddouble.TanPi(n)), "tan intn");
                Assert.AreEqual(double.Sign(double.TanPi(-n)), ddouble.Sign(ddouble.TanPi(-n)), "tan intn");
            }
        }

        [TestMethod]
        public void SinTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sin(v);

                PrecisionAssert.AreEqual(double.Sin((double)d), (double)u, 1e-14);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.Sin(0d);
            ddouble sin_mzero = ddouble.Sin(-0d);
            ddouble sin_pinf = ddouble.Sin(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.Sin(double.NegativeInfinity);
            ddouble sin_nan = ddouble.Sin(double.NaN);

            PrecisionAssert.IsPlusZero(sin_pzero, nameof(sin_pzero));
            PrecisionAssert.IsMinusZero(sin_mzero, nameof(sin_mzero));
            PrecisionAssert.IsNaN(sin_pinf, nameof(sin_pinf));
            PrecisionAssert.IsNaN(sin_ninf, nameof(sin_ninf));
            PrecisionAssert.IsNaN(sin_nan, nameof(sin_nan));

            PrecisionAssert.AlmostEqual("2.220446049250313080847263336181622378926e-16", ddouble.Sin(ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("1.110223024625156540423631668090818031741e-16", ddouble.Sin(ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("5.551115123125782702118158340454098711551e-17", ddouble.Sin(ddouble.Ldexp(1, -54)), 4e-32);

            PrecisionAssert.AlmostEqual("-2.220446049250313080847263336181622378926e-16", ddouble.Sin(-ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("-1.110223024625156540423631668090818031741e-16", ddouble.Sin(-ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("-5.551115123125782702118158340454098711551e-17", ddouble.Sin(-ddouble.Ldexp(1, -54)), 4e-32);

            PrecisionAssert.AlmostEqual("6.109872726999209364103958786427235399579e-151", ddouble.Sin(ddouble.Ldexp(1, -499)), 4e-32);
            PrecisionAssert.AlmostEqual("3.054936363499604682051979393213617699789e-151", ddouble.Sin(ddouble.Ldexp(1, -500)), 4e-32);
            PrecisionAssert.AlmostEqual("1.527468181749802341025989696606808849895e-151", ddouble.Sin(ddouble.Ldexp(1, -501)), 4e-32);

            PrecisionAssert.AlmostEqual("-6.109872726999209364103958786427235399579e-151", ddouble.Sin(-ddouble.Ldexp(1, -499)), 4e-32);
            PrecisionAssert.AlmostEqual("-3.054936363499604682051979393213617699789e-151", ddouble.Sin(-ddouble.Ldexp(1, -500)), 4e-32);
            PrecisionAssert.AlmostEqual("-1.527468181749802341025989696606808849895e-151", ddouble.Sin(-ddouble.Ldexp(1, -501)), 4e-32);
        }

        [TestMethod]
        public void CosTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cos(v);

                PrecisionAssert.AreEqual(double.Cos((double)d), (double)u, 1e-14);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.Cos(0d);
            ddouble cos_mzero = ddouble.Cos(-0d);
            ddouble cos_pinf = ddouble.Cos(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.Cos(double.NegativeInfinity);
            ddouble cos_nan = ddouble.Cos(double.NaN);

            PrecisionAssert.AreEqual(1, cos_pzero, nameof(cos_pzero));
            PrecisionAssert.AreEqual(1, cos_mzero, nameof(cos_mzero));
            PrecisionAssert.IsNaN(cos_pinf, nameof(cos_pinf));
            PrecisionAssert.IsNaN(cos_ninf, nameof(cos_ninf));
            PrecisionAssert.IsNaN(cos_nan, nameof(cos_nan));

            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999975348097", ddouble.Cos(ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999993837024", ddouble.Cos(ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999998459256", ddouble.Cos(ddouble.Ldexp(1, -54)), 4e-32);

            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999975348097", ddouble.Cos(-ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999993837024", ddouble.Cos(-ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("0.999999999999999999999999999999998459256", ddouble.Cos(-ddouble.Ldexp(1, -54)), 4e-32);
        }

        [TestMethod]
        public void TanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tan(v);

                double w = double.Tan((double)d);

                PrecisionAssert.AreEqual(w, (double)u, double.Max(1e-14, double.Abs(w) * 1e-13));
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.Tan(0d);
            ddouble tan_mzero = ddouble.Tan(-0d);
            ddouble tan_pinf = ddouble.Tan(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.Tan(double.NegativeInfinity);
            ddouble tan_nan = ddouble.Tan(double.NaN);

            PrecisionAssert.IsPlusZero(tan_pzero, nameof(tan_pzero));
            PrecisionAssert.IsMinusZero(tan_mzero, nameof(tan_mzero));
            PrecisionAssert.IsNaN(tan_pinf, nameof(tan_pinf));
            PrecisionAssert.IsNaN(tan_ninf, nameof(tan_ninf));
            PrecisionAssert.IsNaN(tan_nan, nameof(tan_nan));

            PrecisionAssert.AlmostEqual("2.220446049250313080847263336181677117148e-16", ddouble.Tan(ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("1.110223024625156540423631668090824874018e-16", ddouble.Tan(ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("5.551115123125782702118158340454107264398e-17", ddouble.Tan(ddouble.Ldexp(1, -54)), 4e-32);

            PrecisionAssert.AlmostEqual("-2.220446049250313080847263336181677117148e-16", ddouble.Tan(-ddouble.Ldexp(1, -52)), 4e-32);
            PrecisionAssert.AlmostEqual("-1.110223024625156540423631668090824874018e-16", ddouble.Tan(-ddouble.Ldexp(1, -53)), 4e-32);
            PrecisionAssert.AlmostEqual("-5.551115123125782702118158340454107264398e-17", ddouble.Tan(-ddouble.Ldexp(1, -54)), 4e-32);

            PrecisionAssert.AlmostEqual("6.109872726999209364103958786427235399579e-151", ddouble.Tan(ddouble.Ldexp(1, -499)), 4e-32);
            PrecisionAssert.AlmostEqual("3.054936363499604682051979393213617699789e-151", ddouble.Tan(ddouble.Ldexp(1, -500)), 4e-32);
            PrecisionAssert.AlmostEqual("1.527468181749802341025989696606808849895e-151", ddouble.Tan(ddouble.Ldexp(1, -501)), 4e-32);

            PrecisionAssert.AlmostEqual("-6.109872726999209364103958786427235399579e-151", ddouble.Tan(-ddouble.Ldexp(1, -499)), 4e-32);
            PrecisionAssert.AlmostEqual("-3.054936363499604682051979393213617699789e-151", ddouble.Tan(-ddouble.Ldexp(1, -500)), 4e-32);
            PrecisionAssert.AlmostEqual("-1.527468181749802341025989696606808849895e-151", ddouble.Tan(-ddouble.Ldexp(1, -501)), 4e-32);
        }

        [TestMethod]
        public void AsinTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Asin(v);
                ddouble w = ddouble.AsinPi(v);

                PrecisionAssert.AreEqual(double.Asin((double)d), (double)u, 1e-15);
                PrecisionAssert.AreEqual(double.AsinPi((double)d), (double)w, 1e-15);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            PrecisionAssert.AlmostEqual("3.125508849949515468409146989332854974298e-2", ddouble.Asin(ddouble.BitDecrement(ddouble.Rcp(32))), 1e-32);
            PrecisionAssert.AlmostEqual("3.125508849949515468409146989332854974298e-2", ddouble.Asin(ddouble.Rcp(32)), 1e-32);
            PrecisionAssert.AlmostEqual("3.125508849949515468409146989332854974298e-2", ddouble.Asin(ddouble.BitIncrement(ddouble.Rcp(32))), 1e-32);

            PrecisionAssert.AlmostEqual("-3.125508849949515468409146989332854974298e-2", ddouble.Asin(-ddouble.BitDecrement(ddouble.Rcp(32))), 1e-32);
            PrecisionAssert.AlmostEqual("-3.125508849949515468409146989332854974298e-2", ddouble.Asin(-ddouble.Rcp(32)), 1e-32);
            PrecisionAssert.AlmostEqual("-3.125508849949515468409146989332854974298e-2", ddouble.Asin(-ddouble.BitIncrement(ddouble.Rcp(32))), 1e-32);

            PrecisionAssert.AlmostEqual("0.003906259934175675287287668546931409343529", ddouble.Asin(ddouble.Rcp(256)), 1e-31);
            PrecisionAssert.AlmostEqual("0.0009765626552204957159990441089610155724292", ddouble.Asin(ddouble.Rcp(1024)), 1e-31);
            PrecisionAssert.AlmostEqual("0.0002441406274253192697799412915520836753299", ddouble.Asin(ddouble.Rcp(4096)), 1e-31);
            PrecisionAssert.AlmostEqual("0.00003051757812973695157371923471036862626770", ddouble.Asin(ddouble.Rcp(32768)), 1e-31);
            PrecisionAssert.AlmostEqual("7.629394531324014868310282473922313774258e-6", ddouble.Asin(ddouble.Rcp(131072)), 1e-31);
            PrecisionAssert.AlmostEqual("2.328306436538696289083536290805893648145e-10", ddouble.Asin(double.ScaleB(1, -32)), 1e-31);
            PrecisionAssert.AlmostEqual("5.421010862427522170037264004349708557132e-20", ddouble.Asin(double.ScaleB(1, -64)), 1e-31);
            PrecisionAssert.AlmostEqual("8.636168555094444625386351862800399571116e-78", ddouble.Asin(double.ScaleB(1, -256)), 1e-31);
            PrecisionAssert.AlmostEqual("8.900295434028805532360930869329616256877e-308", ddouble.Asin(double.ScaleB(1, -1020)), 1e-31);

            PrecisionAssert.AlmostEqual(ddouble.Pi / 6, ddouble.Asin(ddouble.Rcp(2)), 1e-31);
            PrecisionAssert.AlmostEqual("1.11976951499863418668667705584539962", ddouble.Asin((ddouble)9 / 10), 1e-31);
            PrecisionAssert.AlmostEqual("1.42925685347046940048553233466472443", ddouble.Asin((ddouble)99 / 100), 1e-31);
            PrecisionAssert.AlmostEqual("1.52607123962616318798162545896820037", ddouble.Asin((ddouble)999 / 1000), 1e-31);
            PrecisionAssert.AlmostEqual("1.55665407331738374163508146582209533", ddouble.Asin((ddouble)9999 / 10000), 2e-31);
            PrecisionAssert.AlmostEqual("1.56632418711310869205898202533489875", ddouble.Asin((ddouble)99999 / 100000), 2e-31);

            ddouble asin_pzero = ddouble.Asin(0d);
            ddouble asin_mzero = ddouble.Asin(-0d);
            ddouble asin_pinf = ddouble.Asin(double.PositiveInfinity);
            ddouble asin_ninf = ddouble.Asin(double.NegativeInfinity);
            ddouble asin_pout = ddouble.Asin(ddouble.BitIncrement(1));
            ddouble asin_nout = ddouble.Asin(ddouble.BitDecrement(-1));
            ddouble asin_nan = ddouble.Asin(double.NaN);

            PrecisionAssert.IsPlusZero(asin_pzero, nameof(asin_pzero));
            PrecisionAssert.IsMinusZero(asin_mzero, nameof(asin_mzero));
            PrecisionAssert.IsNaN(asin_pinf, nameof(asin_pinf));
            PrecisionAssert.IsNaN(asin_ninf, nameof(asin_ninf));
            PrecisionAssert.IsNaN(asin_pout, nameof(asin_pout));
            PrecisionAssert.IsNaN(asin_nout, nameof(asin_nout));
            PrecisionAssert.IsNaN(asin_nan, nameof(asin_nan));
        }

        [TestMethod]
        public void AcosTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Acos(v);
                ddouble w = ddouble.AcosPi(v);

                PrecisionAssert.AreEqual(double.Acos((double)d), (double)u, 1e-15);
                PrecisionAssert.AreEqual(double.AcosPi((double)d), (double)w, 1e-15);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble acos_pinf = ddouble.Acos(double.PositiveInfinity);
            ddouble acos_ninf = ddouble.Acos(double.NegativeInfinity);
            ddouble acos_pout = ddouble.Acos(ddouble.BitIncrement(1));
            ddouble acos_nout = ddouble.Acos(ddouble.BitDecrement(-1));
            ddouble acos_nan = ddouble.Acos(double.NaN);

            PrecisionAssert.IsNaN(acos_pinf, nameof(acos_pinf));
            PrecisionAssert.IsNaN(acos_ninf, nameof(acos_ninf));
            PrecisionAssert.IsNaN(acos_pout, nameof(acos_pout));
            PrecisionAssert.IsNaN(acos_nout, nameof(acos_nout));
            PrecisionAssert.IsNaN(acos_nan, nameof(acos_nan));
        }

        [TestMethod]
        public void AtanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Atan(v);
                ddouble w = ddouble.AtanPi(v);

                PrecisionAssert.AreEqual(double.Atan((double)d), (double)u, 1e-15);
                PrecisionAssert.AreEqual(double.AtanPi((double)d), (double)w, 1e-15);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            PrecisionAssert.AlmostEqual(ddouble.Pi / 4, ddouble.Atan(1), 1e-31);
            PrecisionAssert.AlmostEqual(-ddouble.Pi / 4, ddouble.Atan(-1), 1e-31);
            PrecisionAssert.AlmostEqual("1.10714871779409050301706546017853704", ddouble.Atan(2), 1e-31);
            PrecisionAssert.AlmostEqual("-1.10714871779409050301706546017853704", ddouble.Atan(-2), 1e-31);
            PrecisionAssert.AlmostEqual("1.24904577239825442582991707728109012", ddouble.Atan(3), 1e-31);
            PrecisionAssert.AlmostEqual("-1.24904577239825442582991707728109012", ddouble.Atan(-3), 1e-31);

            ddouble atan_pzero = ddouble.Atan(0d);
            ddouble atan_mzero = ddouble.Atan(-0d);
            ddouble atan_pinf = ddouble.Atan(double.PositiveInfinity);
            ddouble atan_ninf = ddouble.Atan(double.NegativeInfinity);
            ddouble atan_nan = ddouble.Atan(double.NaN);

            PrecisionAssert.IsPlusZero(atan_pzero, nameof(atan_pzero));
            PrecisionAssert.IsMinusZero(atan_mzero, nameof(atan_mzero));
            PrecisionAssert.AreEqual(ddouble.Pi / 2, atan_pinf, nameof(atan_pinf));
            PrecisionAssert.AreEqual(-ddouble.Pi / 2, atan_ninf, nameof(atan_ninf));
            PrecisionAssert.IsNaN(atan_nan, nameof(atan_nan));
        }

        [TestMethod]
        public void Atan2Test() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = -10m; x <= +10m; x += 0.1m) {
                    if (x == 0m && y == 0m) {
                        continue;
                    }

                    double v = double.Atan2((double)y, (double)x);
                    ddouble u = ddouble.Atan2((ddouble)y, (ddouble)x);
                    ddouble w = ddouble.Atan2Pi((ddouble)y, (ddouble)x);

                    if (u == ddouble.Pi) {
                        PrecisionAssert.AreEqual(double.Pi, double.Abs(v));
                    }
                    else {
                        PrecisionAssert.AreEqual(v, (double)u, 1e-15, $"{y}, {x}");
                        PrecisionAssert.AreEqual(v / double.Pi, (double)w, 1e-15, $"{y}, {x}");
                    }
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }

            PrecisionAssert.IsPlusZero(ddouble.Atan2(0d, 0d));
            PrecisionAssert.AreEqual(ddouble.Pi, ddouble.Atan2(0d, -0d));
            PrecisionAssert.IsMinusZero(ddouble.Atan2(-0d, 0d));
            PrecisionAssert.AreEqual(-ddouble.Pi, ddouble.Atan2(-0d, -0d));

            PrecisionAssert.IsPlusZero(ddouble.Atan2Pi(0d, 0d));
            PrecisionAssert.AreEqual(1d, ddouble.Atan2Pi(0d, -0d));
            PrecisionAssert.IsMinusZero(ddouble.Atan2Pi(-0d, 0d));
            PrecisionAssert.AreEqual(-1d, ddouble.Atan2Pi(-0d, -0d));
        }

        [TestMethod]
        public void SinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sinh(v);

                PrecisionAssert.AreEqual(double.Sinh((double)d), (double)u, double.Abs(double.Sinh((double)d)) * 1e-15);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sinh_pzero = ddouble.Sinh(0d);
            ddouble sinh_mzero = ddouble.Sinh(-0d);
            ddouble sinh_pinf = ddouble.Sinh(double.PositiveInfinity);
            ddouble sinh_ninf = ddouble.Sinh(double.NegativeInfinity);
            ddouble sinh_nan = ddouble.Sinh(double.NaN);

            PrecisionAssert.IsPlusZero(sinh_pzero, nameof(sinh_pzero));
            PrecisionAssert.IsMinusZero(sinh_mzero, nameof(sinh_mzero));
            PrecisionAssert.IsPositiveInfinity(sinh_pinf, nameof(sinh_pinf));
            PrecisionAssert.IsNegativeInfinity(sinh_ninf, nameof(sinh_ninf));
            PrecisionAssert.IsNaN(sinh_nan, nameof(sinh_nan));
        }

        [TestMethod]
        public void CoshTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cosh(v);

                PrecisionAssert.AreEqual(double.Cosh((double)d), (double)u, double.Abs(double.Cosh((double)d)) * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cosh_pzero = ddouble.Cosh(0d);
            ddouble cosh_mzero = ddouble.Cosh(-0d);
            ddouble cosh_pinf = ddouble.Cosh(double.PositiveInfinity);
            ddouble cosh_ninf = ddouble.Cosh(double.NegativeInfinity);
            ddouble cosh_nan = ddouble.Cosh(double.NaN);

            PrecisionAssert.AreEqual(1, cosh_pzero, nameof(cosh_pzero));
            PrecisionAssert.AreEqual(1, cosh_mzero, nameof(cosh_mzero));
            PrecisionAssert.IsPositiveInfinity(cosh_pinf, nameof(cosh_pinf));
            PrecisionAssert.IsPositiveInfinity(cosh_ninf, nameof(cosh_ninf));
            PrecisionAssert.IsNaN(cosh_nan, nameof(cosh_nan));
        }

        [TestMethod]
        public void TanhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tanh(v);

                PrecisionAssert.AreEqual(double.Tanh((double)d), (double)u, 1e-15);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tanh_pzero = ddouble.Tanh(0d);
            ddouble tanh_mzero = ddouble.Tanh(-0d);
            ddouble tanh_pinf = ddouble.Tanh(double.PositiveInfinity);
            ddouble tanh_ninf = ddouble.Tanh(double.NegativeInfinity);
            ddouble tanh_nan = ddouble.Tanh(double.NaN);

            PrecisionAssert.IsPlusZero(tanh_pzero, nameof(tanh_pzero));
            PrecisionAssert.IsMinusZero(tanh_mzero, nameof(tanh_mzero));
            PrecisionAssert.AreEqual(1d, tanh_pinf, nameof(tanh_pinf));
            PrecisionAssert.AreEqual(-1d, tanh_ninf, nameof(tanh_ninf));
            PrecisionAssert.IsNaN(tanh_nan, nameof(tanh_nan));
        }

        [TestMethod]
        public void AsinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Asinh(v);

                PrecisionAssert.AreEqual(double.Asinh((double)d), (double)u, double.Abs(double.Asinh((double)d)) * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arsinh_pzero = ddouble.Asinh(0d);
            ddouble arsinh_mzero = ddouble.Asinh(-0d);
            ddouble arsinh_pinf = ddouble.Asinh(double.PositiveInfinity);
            ddouble arsinh_ninf = ddouble.Asinh(double.NegativeInfinity);
            ddouble arsinh_nan = ddouble.Asinh(double.NaN);

            PrecisionAssert.IsPlusZero(arsinh_pzero, nameof(arsinh_pzero));
            PrecisionAssert.IsMinusZero(arsinh_mzero, nameof(arsinh_mzero));
            PrecisionAssert.IsPositiveInfinity(arsinh_pinf, nameof(arsinh_pinf));
            PrecisionAssert.IsNegativeInfinity(arsinh_ninf, nameof(arsinh_ninf));
            PrecisionAssert.IsNaN(arsinh_nan, nameof(arsinh_nan));
        }

        [TestMethod]
        public void AcoshTest() {
            for (decimal d = 1m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Acosh(v);

                PrecisionAssert.AreEqual(double.Acosh((double)d), (double)u, double.Abs(double.Acosh((double)d)) * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arcosh_pzero = ddouble.Acosh(0d);
            ddouble arcosh_mzero = ddouble.Acosh(-0d);
            ddouble arcosh_pinf = ddouble.Acosh(double.PositiveInfinity);
            ddouble arcosh_ninf = ddouble.Acosh(double.NegativeInfinity);
            ddouble arcosh_nan = ddouble.Acosh(double.NaN);

            PrecisionAssert.IsNaN(arcosh_pzero, nameof(arcosh_pzero));
            PrecisionAssert.IsNaN(arcosh_mzero, nameof(arcosh_mzero));
            PrecisionAssert.IsPositiveInfinity(arcosh_pinf, nameof(arcosh_pinf));
            PrecisionAssert.IsNaN(arcosh_ninf, nameof(arcosh_ninf));
            PrecisionAssert.IsNaN(arcosh_nan, nameof(arcosh_nan));
        }

        [TestMethod]
        public void AtanhTest() {
            for (decimal d = -0.99m; d <= +0.99m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Atanh(v);

                PrecisionAssert.AreEqual(double.Atanh((double)d), (double)u, double.Abs(double.Atanh((double)d)) * 1e-12);
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble artanh_pzero = ddouble.Atanh(0d);
            ddouble artanh_mzero = ddouble.Atanh(-0d);
            ddouble artanh_pone = ddouble.Atanh(1d);
            ddouble artanh_mone = ddouble.Atanh(-1d);
            ddouble artanh_pinf = ddouble.Atanh(double.PositiveInfinity);
            ddouble artanh_ninf = ddouble.Atanh(double.NegativeInfinity);
            ddouble artanh_nan = ddouble.Atanh(double.NaN);

            PrecisionAssert.IsPlusZero(artanh_pzero, nameof(artanh_pzero));
            PrecisionAssert.IsMinusZero(artanh_mzero, nameof(artanh_mzero));
            PrecisionAssert.IsPositiveInfinity(artanh_pone, nameof(artanh_pone));
            PrecisionAssert.IsNegativeInfinity(artanh_mone, nameof(artanh_mone));
            PrecisionAssert.IsNaN(artanh_pinf, nameof(artanh_pinf));
            PrecisionAssert.IsNaN(artanh_ninf, nameof(artanh_ninf));
            PrecisionAssert.IsNaN(artanh_nan, nameof(artanh_nan));
        }

        [TestMethod]
        public void SinPiHalfMonotoneTest() {
            for (ddouble x = 1d / 8192; x < 1; x += 1d / 8192) {
                ddouble y = ddouble.SinPiHalf(x);

                for (int exp = -110; exp < -95; exp++) {
                    ddouble ydec = ddouble.SinPiHalf(x - ddouble.Ldexp(1, exp));
                    ddouble yinc = ddouble.SinPiHalf(x + ddouble.Ldexp(1, exp));

                    Console.WriteLine(x);
                    Console.WriteLine(y - ydec);
                    Console.WriteLine(yinc - y);

                    if (x > 0 && ydec > y) {
                        BitAssert.NeighborBits(y, ydec, $"{x},{exp},dec", 1);
                    }

                    if (x < 1 && yinc < y) {
                        BitAssert.NeighborBits(y, yinc, $"{x},{exp},inc", 1);
                    }
                }
            }
        }

        [TestMethod]
        public void SinPiHalfExpectedTest() {
            ddouble[] expecteds = {
                0,
                "0.0007669903187427045269385683579485766431409",
                "0.001533980186284765612303697150264079079955",
                "0.002300969151425805244235552340867254177462",
                "0.003067956762965976270145365490919842518945",
                "0.003834942569706227825960602994634806402034",
                "0.004601926120448570764901699296911967372885",
                "0.005368906963996343085634209182070247978597",
                "0.006135884649154475359640234590372580917058",
                "0.006902858724729756157652981878320229109709",
                "0.007669828739531097474998305989829482235286",
                "0.008436794242369800155687098648395134722153",
                "0.009203754782059819315102378415191428848183",
                "0.009970709907418029761124941284761832148990",
                "0.01073765916726449141354143140850362446112",
                "0.01150460211042271472157869254656125626442",
                "0.01227153828571992607940826195100321214037",
                "0.01303846724198733323946486957727002269882",
                "0.01380538852806039072342280680684541277898",
                "0.01457230169277906523067403024191759660119",
                "0.01533920628498810104415186760246262130175",
                "0.01610610185353728543334419431769555435712",
                "0.01687298794728171405433995105719728919843",
                "0.01763986411508205634675287419223383420147",
                "0.01840672990580482092736631301484012655050",
                "0.01917358486832262098034300947114059352315",
                "0.01994042855151443964384371818612273480691",
                "0.02070726050426589539289854666966673178304",
                "0.02147408027546950741837489779806226052094",
                "0.02224088741402496100188589896151199770722",
                "0.02300768146883937288648320465622841205269",
                "0.02377446198882755664297806177967492301579",
                "0.02454122852291228803173452945928292506547",
                "0.02530798062002457035977874790859100626713",
                "0.02607471782910389983306815356020036777718",
                "0.02684143969909853090376454057221935491480",
                "0.02760814577896574161235487174397846586416",
                "0.02837483561767209892446374490773348174395",
                "0.02914150876419372406220142398583667874198",
                "0.02990816476751655782989134711744260221098",
                "0.03067480317663662593402102756522371299772",
                "0.03144142354056030429726026651080041275775",
                "0.03220802540830458436639060033763751850021",
                "0.03297460832889733841398990858202313400730",
                "0.03374117185137758483371611240642394996219",
                "0.03450771552479575342903389721500113446787",
                "0.03527423889821395069522839688837093600611",
                "0.03604074152070622509454978106380164262084",
                "0.03680722294135883232433269092795130105523",
                "0.03757368270927050057793447312196622056180",
                "0.03834012037355269579833616558327633696999",
                "0.03910653548332988692425019346473750816164",
                "0.03987292758773981112857873767988021319561",
                "0.04063929623593373904906674312292634076115",
                "0.04140564097707674001099353820392813124866",
                "0.04217196136034794724174704202286317933271",
                "0.04293825693494082307712454028178395539393",
                "0.04370452725006342415920401590116675519981",
                "0.04447077185493866662563002526543047486009",
                "0.04523699029880459129015811607319721502267",
                "0.04600318213091462881430178791024147344036",
                "0.04676934690053786486992600189721956329206",
                "0.04753548415695930529263125109018281687067",
                "0.04830159344948014122577220872955396735802",
                "0.04906767432741801425495497694268265831475",
                "0.04983372634010728153285696410629007016102",
                "0.05059974903689928089421342476806088027464",
                "0.05136574196716259596081470181133983937009",
                "0.05213170468028332123635821642333674855831",
                "0.05289763672566532719099925739543410195813",
                "0.05366353765273052533544462734412260800532",
                "0.05442940701091913328443320959275765378172",
                "0.05519524434968993980944752569773091189810",
                "0.05596104921852056988050035993778203587768",
                "0.05672682116690774969684053351203202080835",
                "0.05749255974436757170642191771089854050700",
                "0.05825826450043575961397978193435057585641",
                "0.05902393498466793337755857913397103266682",
                "0.05978957074663987419333527804901786875989",
                "0.06055517133594778946858235849210251240072",
                "0.06132073630220857778261459291723500719398",
                "0.06208626519506009383556374457181425764656",
                "0.06285175756416141338482531969466481637203",
                "0.06361721295919309816902151847443463601966",
                "0.06438263092985746081932453682656684016355",
                "0.06514801102587882975798437848263852735842",
                "0.06591335279700381408390534441311653719838",
                "0.06667865579300156844511537422350954841177",
                "0.06744391956366405789797242187449336476910",
                "0.06820914365880632275295205587884723892667",
                "0.06897432762826674340686048202195899740638",
                "0.06973947102190730516131719463823091793214",
                "0.07050457338961386302735147055294207559925",
                "0.07126963428129640651595692796799146522046",
                "0.07203465324688932441444838083045481813801",
                "0.07279962983635166954846522757503180336916",
                "0.07356456359966742352946562157523432181330",
                "0.07432945408684576148755567917356590113645",
                "0.07509430084792131678949798978796175670902",
                "0.07585910343295444574174370131039382555063",
                "0.07662386139203149227833246282378987668485",
                "0.07738857427526505263350451556526546978561",
                "0.07815324163279423999886923205711683777750",
                "0.07891786301478494916497441241206641752015",
                "0.07968243797143012114712065599588540136395",
                "0.08044696605295000779526513589873393283576",
                "0.08121144680959243638785911302635396974428",
                "0.08197587979163307420946353607361688146105",
                "0.08274026454937569311198708318586197403799",
                "0.08350460063315243405939101074795773330402",
                "0.08426888759332407165570518446706897441593",
                "0.08503312498028027865619967773271456493722",
                "0.08579731234443989046155633214684617194053",
                "0.08656144923625116959488468511736274257979",
                "0.08732553520619207016142667950069227891248",
                "0.08808956980477050229079458046281597447874",
                "0.08885355258252459656158653500337395142933",
                "0.08961748309002296840822421995427062816480",
                "0.09038136087786498250985703472248505999507",
                "0.09114518549668101716117730559658127691119",
                "0.09190895649713272862499097907769948700924",
                "0.09267267342991331546638829242858476142685",
                "0.09343633584574778286835892045846915302897",
                "0.09419994329539320692869610847735975887969",
                "0.09496349532963899893803431236049259431342",
                "0.09572699149930716963886487776238382891612",
                "0.09649043135525259346537430171003941288157",
                "0.09725381444836327276394963108646481315602",
                "0.09801714032956060199419556388864184586114",
                "0.09878040854979963191030783060860274423503",
                "0.09954361866006933372264744464212589550328",
                "0.1003067702113928632393604222768963176537",
                "0.1010698627548278249878875845507100762214",
                "0.1018328958414665363162090651004485562972",
                "0.1025958690224362914736681600440988466883",
                "0.1033587818488996256712191679510434483312",
                "0.1041216338720545791209438800601790207960",
                "0.1048844246431349610546813931011428083350",
                "0.1056471537134106137216159293610195660153",
                "0.1064098206341876763646673610173640088275",
                "0.1071724249568088491755291482281967503779",
                "0.1079349662326536572281984130308080471461",
                "0.1086974440131387143908428837537260299334",
                "0.1094598578497179872158494573900670428652",
                "0.1102222072938830588078991402156777252745",
                "0.1109844918971633926699131398619940123569",
                "0.1117467112111265965267148950713736923276",
                "0.1125088647873786861262528424717988828592",
                "0.1132709521775643490182287329082850660798",
                "0.1140329729333672083099763231600663838731",
                "0.1147949266065100863994352822556449244719",
                "0.1155568127487552686850651650720868544800",
                "0.1163186309119047672525443194705125409239",
                "0.1170803806478005845380986068765532854415",
                "0.1178420615083249769683048299626259233144",
                "0.1186036730454007185762137749282002360895",
                "0.1193652148109913645936377898047947467110",
                "0.1201266863571015150194478342342258153920",
                "0.1208880872357770781637249502816447842492",
                "0.1216494169991055341676111180491199266766",
                "0.1224106751992161984987044741509457875752",
                "0.1231718613882804854218438854984837432425",
                "0.1239329751185121714451278853201458016959",
                "0.1246940159421676587410129929111202885187",
                "0.1254549834115462385423364532675945494325",
                "0.1262158770789903545131084475115475245105",
                "0.1269766964968858660939188398546553324421",
                "0.1277374412176623118218035417834671150908",
                "0.1284981107937931726244155891727575476975",
                "0.1292587047777961350883460431498377797946",
                "0.1300192227222333547014398407395992439395",
                "0.1307796641797117190689517366181658078542",
                "0.1315400287028831111033874926922301510074",
                "0.1323003158444446721878754877014409660152",
                "0.1330605251571390653129139346125795118146",
                "0.1338206561937547381863389092367080191436",
                "0.1345807085071261863163584092539792556314",
                "0.1353406816501342160674976786753569303733",
                "0.1361005751757062076893010487061012353037",
                "0.1368603886368163783176355620025133084665",
                "0.1376201215864860449484416634310973343663",
                "0.1383797735777838873837762566479798873578",
                "0.1391393441638262111499934421161134403396",
                "0.1398988328977772103879082685684751141818",
                "0.1406582393328492307147888464071430911198",
                "0.1414175630223030320580221881007819620317",
                "0.1421768035194480514602991573066848738469",
                "0.1429359603776426658561639251980948865030",
                "0.1436950331502944548197733493230505826203",
                "0.1444540213908604632837117072574617875882",
                "0.1452129246528474642287062343425102897883",
                "0.1459717424898122213440889319147776834316",
                "0.1467304744553617516588501296467178197062",
                "0.1474891201031535881431293029152047218791",
                "0.1482476789868960422799886635068880194592",
                "0.1490061506603484666073150594509667548318",
                "0.1497645346773215172296957373427385287768",
                "0.1505228305916774163001135381848840413931",
                "0.1512810379573302144713071155278967611226",
                "0.1520391563282460533166417825363532723200",
                "0.1527971852584434277203366125438313132217",
                "0.1535551243019934482368934356862090698763",
                "0.1543129730130201034195733923208103328430",
                "0.1550707309457005221177667221473849940337",
                "0.1558283976542652357431014862462223493748",
                "0.1565859726929984405041369376387750139838",
                "0.1573434556162382596094872744570131318732",
                "0.1581008459783770054392215283793211000213",
                "0.1588581433338614416843853596530813017855",
                "0.1596153472371930454544905487771503785121",
                "0.1603724572429282693528179937612123288822",
                "0.1611294729056788035193800408134781277031",
                "0.1618863937801118376413879953333824608490",
                "0.1626432194209503229310706792017933888512",
                "0.1633999493829732340706899195677890328517",
                "0.1641565832210158311245988736282554245730",
                "0.1649131204899699214181891132844101245585",
                "0.1656695607447841213835724130378456848402",
                "0.1664259035404641183718432040578040546923",
                "0.1671821484320729324317676770111260892308",
                "0.1679382949747311780547455359966578550000",
                "0.1686943427236173258858904257668258157328",
                "0.1694502912339679644010750743506045532813",
                "0.1702061400610780615497872132141817080798",
                "0.1709618887603012263636423572082635319663",
                "0.1717175368870499705303995467541490019863",
                "0.1724730839967959699333261750144189619362",
                "0.1732285296450703261557580431783273005902",
                "0.1739838733874638279507008074667317502780",
                "0.1747391147796272126753190020266504519679",
                "0.1754942533772714276901588425412648852583",
                "0.1762492887361678917229510361273979524516",
                "0.1770042204121487561968398439291657307198",
                "0.1777590479611071665228846637436204075933",
                "0.1785137709389975233566804210317578817320",
                "0.1792683889018357438189430777762440790754",
                "0.1800229014056995226799065898456067867505",
                "0.1807773080067285935073776648134322730553",
                "0.1815316082611249897782946935602856178005",
                "0.1822858017251533059536372504556279464503",
                "0.1830398879551409585165325784769200138776",
                "0.1837938665074784469734054972733671359884",
                "0.1845477369386196148180181939223625836902",
                "0.1853014988050819104582463779576124442327",
                "0.1860551516634466481054383041691617772795",
                "0.1868086950703592686262031886870767514594",
                "0.1875621285825296003564755659623573927950",
                "0.1883154517567321198777021564507476073704",
                "0.1890686641498062127549978370874602018496",
                "0.1898217653186564342371173290134316088544",
                "0.1905747548202527699180892394765507685405",
                "0.1913276322116308963603591173843559086593",
                "0.1920803970498924416792882046279485355947",
                "0.1928330488922052460888545880303224778591",
                "0.1935855872958036224084034795959339378372",
                "0.1943380118179886165302933756521327806148",
                "0.1950903220161282678482848684770222409277",
                "0.1958425174476578696465189071024003279960",
                "0.1965945976700802294489313271646478675998",
                "0.1973465622409659293289504929507517025396",
                "0.1980984107179535861793249181510733849221",
                "0.1988501426587501119419277552849799918277",
                "0.1996017576211309737973850673100306821730",
                "0.2003532551629404543143748185600464363113",
                "0.2011046348420919115584435458820671774655",
                "0.2018558962165680391601876946569062088530",
                "0.2026070388444211263426466282927326027542",
                "0.2033580622837733179077543437758337852742",
                "0.2041089660928168741816969499474189576619",
                "0.2048597498298144309190229893500050133823",
                "0.2056104130530992591653537087515660283321",
                "0.2063609553210755250785404078102109453958",
                "0.2071113761922185497081160197866674167674",
                "0.2078616752250750687328881027462785280616",
                "0.2086118519782634921565204443165488742010",
                "0.2093619060104741639609505077804927025694",
                "0.2101118368804696217174899720901250451971",
                "0.2108616441470848561554556432783823372129",
                "0.2116113273692275706881780397315473189708",
                "0.2123608861058784408962349788578693468330",
                "0.2131103199160913739677575178515008423980",
                "0.2138596283589937680956556265040986889991",
                "0.2146088109937867718316109953594510817546",
                "0.2153578673797455433966844079392707446704",
                "0.2161067970762195099483851312908295838457",
                "0.2168555996426326268040498047193827354434",
                "0.2176042746384836366203783322703265237413",
                "0.2183528216233463285289743103177399484367",
                "0.2191012401568697972277375474973577988484",
                "0.2198495297987787020279562601931001203921",
                "0.2205976901088735258569465528470222575656",
                "0.2213457206470308342160868185129367368262",
                "0.2220936209732035340940947213139774485665",
                "0.2228413906474211328353944487940125035622",
                "0.2235890292297899969634219485720492867488",
                "0.2243365362804936109587158902175980682059",
                "0.2250839113597928359916421198633534633451",
                "0.2258311540280261686095994017595004143569",
                "0.2265782638456099993785542677514374931364",
                "0.2273252403730388714787528225297194496813",
                "0.2280720831708857392544573794575372441627",
                "0.2288187917998022267175558288270614476847",
                "0.2295653658205188860048916675314579657108",
                "0.2303118047938454557891626463643275661294",
                "0.2310581082806711196432360184727066652304",
                "0.2318042758419647643577283998935801711174",
                "0.2325503070387752382116982805970817843324",
                "0.2332962014322316091962992520421768361604",
                "0.2340419585835434231912420449226212800691",
                "0.2347875780540009620939134985423515623023",
                "0.2355330594049755019010006111101674524848",
                "0.2362784021979195707424678481836071303645",
                "0.2370236059943672068677359145212644641040",
                "0.2377686703559342165839102227214459899029",
                "0.2385135948443184321459073202329930706962",
                "0.2392583790212999695983275646212864792447",
                "0.2400030224487414865689223653588895716013",
                "0.2407475246885884400135043388859555916847",
                "0.2414918853028693439121487522504077250239",
                "0.2422361038536960269165346592919804956681",
                "0.2429801799032638899482741620774711183210",
                "0.2437241130138521637480782591269725690514",
                "0.2444679027478241663756077708924294610686",
                "0.2452115486676275606598578619605562932242",
                "0.2459550503357946115999247085519682118633",
                "0.2466984073149424437160028890772799727303",
                "0.2474416191677732983504621047889121377526",
                "0.2481846854570747909188518669343874682003",
                "0.2489276057457201681106828162729877050769",
                "0.2496703795966685650398333703627541313704",
                "0.2504130065729652623444304236589371083665",
                "0.2511554862377419432360528551881127366015",
                "0.2518978181542169504981066283742714258674",
                "0.2526400018856955434332202974942259302982",
                "0.2533820369955701547595097652296677239487",
                "0.2541239230473206474555611658621028170938",
                "0.2548656596045145715539807788247035490779",
                "0.2556072462308074208833609075818037930023",
                "0.2563486824899428897585106891523235715985",
                "0.2570899679457531296188008300278174608166",
                "0.2578311021621590056144712947590814441497",
                "0.2585720847031703531407510040973071205129",
                "0.2593129151328862343196386302766223532891",
                "0.2600535930154951944291936078144855174445",
                "0.2607941179152755182801865090847883426488",
                "0.2615344893965954865399579648856517981486",
                "0.2622747070239136320033353412797523025822",
                "0.2630147703617789958104564151295734996168",
                "0.2637546789748313836113493219832235967120",
                "0.2644944324278016216771180812883714457335",
                "0.2652340302855118129575830353224177247043",
                "0.2659734721128755930852255697262122816416",
                "0.2667127574748983863252865151164363940421",
                "0.2674518859366776614718676609281707988312",
                "0.2681908570634031876898858444041482230943",
                "0.2689296704203572903027291095007241091233",
                "0.2696683255729151065254644624226725396347",
                "0.2704068220865448411434467825295072397503",
                "0.2711451595268080221361784794751211186967",
                "0.2718833374593597562462695196501132213464",
                "0.2726213554499489844933474772922102408042",
                "0.2733592130644187376327672980146718993091",
                "0.2740969098687063915589704949754764848163",
                "0.2748344454288439226533435304713955317736",
                "0.2755718193109581630764251683907659018327",
                "0.2763090310812710560043126156968341465543",
                "0.2770460803060999108091163039399627606229",
                "0.2777829665518576581833131947117314497442",
                "0.2785196893850531052078485259570194793627",
                "0.2792562483722911903638359491514981237601",
                "0.2799926430802732384877060405315757331942",
                "0.2807288730757972156696532028317024608028",
                "0.2814649379257579840952310073400376703952",
                "0.2822008371971475568299460595277908584603",
                "0.2829365704570553525467005050400468943030",
                "0.2836721372726684501959333264565587848800",
                "0.2844075372112718436183106149398162274616",
                "0.2851427698402486960998150346846560936729",
                "0.2858778347270805948690847319687518108706",
                "0.2866127314393478055368519745764824374969",
                "0.2873474595447295264773318414299190824647",
                "0.2880820186110041431514113164099561574309",
                "0.2888164082060494823714891745879376858558",
                "0.2895506278978430665078170834134643830930",
                "0.2902846772544623676361923758173952746915",
                "0.2910185558440850616268529866903579911558",
                "0.2917522632349892821744250787863342703579",
                "0.2924857989955538747687739187780712656794",
                "0.2932191626942586506066085989561645990750",
                "0.2939523538996846404436912349166452621866",
                "0.2946853721805143483875013045227589522873",
                "0.2954182171055320056302058284553326783848",
                "0.2961508882436238241217861277826588797865",
                "0.2968833851637782501831719291851712066678",
                "0.2976157074350862180592336237623178098763",
                "0.2983478546267414034114835207289367139095",
                "0.2990798263080404767503369727760827896235",
                "0.2998116220483833568067842854266261370777",
                "0.3005432414172734638433243583590164176761",
                "0.3012746839843179729040110424033658705328",
                "0.3020059493192280670034632317324243912848",
                "0.3027370369918191902546897466760820713394",
                "0.3034679465720113009355800985817168691671",
                "0.3041986776298291244939122642239864385848",
                "0.3049292297354024064907286334365223463193",
                "0.3056596024589661654819313298944006952936",
                "0.3063897953708609458379481413202142023338",
                "0.3071198080415330705013203318180356791166",
                "0.3078496400415348936820636455585202015094",
                "0.3085792909415250534906538476448215417994",
                "0.3093087603122687245084881846828761460741",
                "0.3100380477246378702956741843609134791267",
                "0.3107671527496114958359972502117632942196",
                "0.3114960749582758999189185446876266259475",
                "0.3122248139218249274584546907204363109637",
                "0.3129533692115602217487908590717328217667",
                "0.3136817403988914766564788459941003099934",
                "0.3144099270553366887490717830316241032687",
                "0.3151379287525224093600471581795225378032",
                "0.3158657450621839965898698651030509404569",
                "0.3165933755561658672430470346829517504097",
                "0.3173208198064217487010264408091100920939",
                "0.3180480773850149307307903100856464265528",
                "0.3187751478641185172289964029394150154896",
                "0.3195020308160156779015182715397565772982",
                "0.3202287258130998998782366379403533923810",
                "0.3209552324278752392629338739441328698770",
                "0.3216815502329565726181436023693388121212",
                "0.3224076788010698483848074776591158480871",
                "0.3231336177050523382365912421282092428675",
                "0.3238593665178528883687121925786469658720",
                "0.3245849248125321707211302305415209036192",
                "0.3253102921622629341359547080141967703657",
                "0.3260354681403302554489193192614348867736",
                "0.3267604523201317905147773280349737976602",
                "0.3274852442751780251664694584390928677482",
                "0.3282098435790925261079168166295056612838",
                "0.3289342498056121917402912495796201583409",
                "0.3296584625285875029216155862817117174299",
                "0.3303824813219827736595462459708671394379",
                "0.3311063057598764017371907372666501496459",
                "0.3318299354164611192718126115212869692539",
                "0.3325533698660442432062764731437522210981",
                "0.3332766086830479257330856892364269806730",
                "0.3339996514420094046508654805349790667399",
                "0.3347224977175812536531441153827573383154",
                "0.3354451470845316325492849682982734482763",
                "0.3361675991177445374174222446082427688425",
                "0.3368898533922200506892532126191475704778",
                "0.3376119094830745911665398248873464720283",
                "0.3383337669655411639691726503213611040106",
                "0.3390554254149696104146500791101002057696",
                "0.3397768844068268578288258028174093267567",
                "0.3404981435166971692877776124164371875411",
                "0.3412192023202823932906505975568637916202",
                "0.3419400603934022133633278709640167280181",
                "0.3426607173119943975927819825612868796116",
                "0.3433811726521150480919602286860190148100",
                "0.3441014259899388503950571026341725015499",
                "0.3448214769017593227830271737204986026453",
                "0.3455413249639890655391917230787393996813",
                "0.3462609697531600101347925055503952073895",
                "0.3469804108459236683443460482209081895886",
                "0.3476996478190513812906519374586455283606",
                "0.3484186802494345684193085876948116364531",
                "0.3491375077140849764025900266513511945793",
                "0.3498561297901349279725372732789988451937",
                "0.3505745460548375706831179263088627366856",
                "0.3512927560855671256013076230482732902747",
                "0.3520107594598191359269470698650610091066",
                "0.3527285557552107155412283877039232673472",
                "0.3534461445494807974836645579640751580909",
                "0.3541635254204903823573957961389289436032",
                "0.3548806979462227866626867227760856902106",
                "0.3555976617047838910584682435594264828401",
                "0.3563144162744023885517780926445343500249",
                "0.3570309612334300326149540357940367878148",
                "0.3577472961603418852304337723607075985954",
                "0.3584634206337365648630156177532806559434",
                "0.3591793342323364943594340906928821164162",
                "0.3598950365349881487751045723267564202023",
                "0.3606105271206623031278912471105200009567",
                "0.3613258055684542800787525783004997565648",
                "0.3620408714575841975391186139137757622166",
                "0.3627557243673972162048544621153241297874",
                "0.3634703638773637870166643181791209774479",
                "0.3641847895670798985467904684431967645952",
                "0.3648990010162673243118617400373963131344",
                "0.3656129978047738700117459086069781700085",
                "0.3663267795125736206942606197851520104632",
                "0.3670403457197671878455974237831789292824",
                "0.3677536960065819564063135661677210602518",
                "0.3684668299533723317127462216816982941984",
                "0.3691797471406199863637039018369651745859",
                "0.3698924471489341070122898109646345280431",
                "0.3706049295590516410827119694518201762689",
                "0.3713171939518375434119349670219232661776",
                "0.3720292399082850228160282531293193873452",
                "0.3727410670095157885810659158383907099913",
                "0.3734526748367802968784329449412628128667",
                "0.3741640629714579971043930195383235683168",
                "0.3748752309950575781437729048605952510508",
                "0.3755861784892172145576185877532747272271",
                "0.3762969050357048126946783249652239006463",
                "0.3770074102164182567265678231998572323015",
                "0.3777176936133856546064728147787087472902",
                "0.3784277548087655839512443377499410881038",
                "0.3791375933848473378467420743401574044064",
                "0.3798472089240511705762811467990666753860",
                "0.3805566010089285432720378149228078803113",
                "0.3812657692221623694892695648630316547536",
                "0.3819747131465672607032051242351430326604",
                "0.3826834323650897717284599840303988667613",
                "0.3833919264608086460608330534128017802754",
                "0.3841001950169350611413401191429122202496",
                "0.3848082376168128735423398271167844957924",
                "0.3855160538439188640756079493391946817281",
                "0.3862236432818629828222157455661409886199",
                "0.3869310055143885940840682748522336325970",
                "0.3876381401253727212569585583240243219829",
                "0.3883450466988262916249935406705281014963",
                "0.3890517248188943810762478440971352980250",
                "0.3897581740698564587395013548287714840814",
                "0.3904643940361266315419167286725114555951",
                "0.3911703843022538886875129486588618994435",
                "0.3918761444529223460562911143745693430085",
                "0.3925816740729514905238686892780577232417",
                "0.3932869727472964242014784790514260266068",
                "0.3939920400610481085961886608903134244857",
                "0.3946968755994336086912002305648396020743",
                "0.3954014789478163369460782811012239436306",
                "0.3961058496916962972167735740345512346334",
                "0.3968099874167103285952909113684558637494",
                "0.3975138917086323491688608636472133965742",
                "0.3982175621533735996984714568998300413695",
                "0.3989209983369828872166164686541790772578",
                "0.3996241998456468285441170307420208607299",
                "0.4003271662656900937258732842228316104552",
                "0.4010298971835756493854028794457277872090",
                "0.4017323921859050019980231620443794796783",
                "0.4024346508594184410825339335196296727888",
                "0.4031366727909952823112577230085484583317",
                "0.4038384575676541105382945548668239438221",
                "0.4045400047765530227458482448036965709541",
                "0.4052413140049898709084813055050524665119",
                "0.4059423848404025047751555909607759634275",
                "0.4066432168703690145689158570769931457007",
                "0.4073438096826079736040734646023887364905",
                "0.4080441628649786808207474989303193628209",
                "0.4087442760054814032366206299549486425807",
                "0.4094441486922576183157670838600650499899",
                "0.4101437805135902562544101475035834752521",
                "0.4108431710579039421834666749289471041367",
                "0.4115423199137652382877361144867089558301",
                "0.4122412266698828858415916240854533353156",
                "0.4129398909151080471610308912108877332555",
                "0.4136383122384345474719443235553664460012",
                "0.4143364902289991166944583253872694458579",
                "0.4150344244760816311432114241605248126931",
                "0.4157321145691053551434210613191012988084",
                "0.4164295600976371825625989107894802468556",
                "0.4171267606513878782577726382759139734308",
                "0.4178237158202123194380720641786616776119",
                "0.4185204251941097369425377427443346936997",
                "0.4192168883632239564330100199299511912473",
                "0.4199131049178436395019566824182668944189",
                "0.4206090744484025246950973602613836590005",
                "0.4213047965454796684486828957525123720852",
                "0.4220002707997996859412879413320511405711",
                "0.4226954968022329918599750996238045750884",
                "0.4233904741437960410806889690701855716840",
                "0.4240852024156515692627385090915777114093",
                "0.4247796812091088333572261892346645556946",
                "0.4254739101156238520292824373974219819143",
                "0.4261678887267996459939639529245914986838",
                "0.4268616166343864782656745011567763751041",
                "0.4275550934302820943209668568887985343046",
                "0.4282483187065319621745846151485925722745",
                "0.4289412920553295123686026387466639922119",
                "0.4296340130690163778745249631679717676849",
                "0.4303264813400826339081990305829805911285",
                "0.4310186964611670376574051760425355050839",
                "0.4317106580250572679219803402921108807625",
                "0.4324023656246901646663350350948466818194",
                "0.4330938188531519684842226384895773616555",
                "0.4337850173036785599756201490297522723812",
                "0.4344759605696556990355795797517107462195",
                "0.4351666482446192640549092244061796303255",
                "0.4358570799222554910325440803550755497284",
                "0.4365472551964012125994647644866880276234",
                "0.4372371736610440969540243105360622461374",
                "0.4379268349103228867085422883138610738322",
                "0.4386162385385276376470257375461343598431",
                "0.4393053841400999573938764613092286912496",
                "0.4399942713096332439934442764085020740041",
                "0.4406828996418729244002858704965345238635",
                "0.4413712687317166928799889682561164890337",
                "0.4420593781742147493204215615854214688511",
                "0.4427472275645700374532660114173961924246",
                "0.4434348164981384829856978815825002788644",
                "0.4441221445704292316420694179834663691061",
                "0.4448092113771048871154576392927002025191",
                "0.4454960165139817489289370584072668650558",
                "0.4461825595770300502064371070030832668326",
                "0.4468688401623741953530443887189265744266",
                "0.4475548578662929976446099397721425371264",
                "0.4482406122852199167265217291614650687630",
                "0.4489261030157432960215026840481076910682",
                "0.4496113296546066000462945794242270758832",
                "0.4502962917987086516370881847779574587533",
                "0.4509809890451038690835601141464406039241",
                "0.4516654209910025031713768797125977006471",
                "0.4523495872337708741330267029477754348291",
                "0.4530334873709316085068396912308168319850",
                "0.4537171210001638759040570418845265824799",
                "0.4544004877193036256838099896628886652296",
                "0.4550835871263438235358692678967193602180",
                "0.4557664188194346879710259077616693140193",
                "0.4564489823968839267189642544705922784425",
                "0.4571312774571569730334881336122435147421",
                "0.4578133035988772219049611553600256307008",
                "0.4584950604208262661798221988580317333237",
                "0.4591765475219441325870371737569131257861",
                "0.4598577645013295176713482106190891954915",
                "0.4605387109582400236331814867414884243655",
                "0.4612193864920923940750749488543293485479",
                "0.4618997907024627496544872491463865001192",
                "0.4625799231890868236428492661407065280723",
                "0.4632597835518601973907196370998114210798",
                "0.4639393713908385356989057838760165944090",
                "0.4646186863062378220954119694405690633268",
                "0.4652977278984345940180759777248424633334",
                "0.4659764957679661779027560648877787034269",
                "0.4666549895155309241769298856861079143390",
                "0.4673332087419884421585671542675763933807",
                "0.4680111530483598348601378544324337174558",
                "0.4686888220358279336976178702147423143398",
                "0.4693662153057375331043539635226427200622",
                "0.4700433324595956250496500815455026299675",
                "0.4707201730990716334619370326858647477436",
                "0.4713967368259976485563876259052543776575",
                "0.4720730232423686610668394245851796698938",
                "0.4727490319503427963818873222980223490465",
                "0.4734247625522415485850082042569415155067",
                "0.4741002146505500143985800146693645451236",
                "0.4747753878479171270316576067550840127766",
                "0.4754502817471558899313678088073846714795",
                "0.4761248959512436104377861963739565237528",
                "0.4767992300633221333421581174135755721061",
                "0.4774732836866980743483265741446195287917",
                "0.4781470564248430534372296222423981522168",
                "0.4788205478813939281343300050639834616489",
                "0.4794937576601530266798397976816903187313",
                "0.4801666853650883811016028926895491619184",
                "0.4808393306003339601904982170109963988604",
                "0.4815116929701899023782266262805504151366",
                "0.4821837720791227485173444807974086112396",
                "0.4828555675317656745634069645546595516144",
                "0.4835270789329187241590842664341203706782",
                "0.4841983058875490411201138003236491545681",
                "0.4848692480007911018229516986611111799490",
                "0.4855399048779469474939868717369626713089",
                "0.4862102761244864164001809829956221171667",
                "0.4868803613460473759409977485643930976301",
                "0.4875501601484359546414850273076499075501",
                "0.4882196721376267740463732258532638506759",
                "0.4888888969197631805150536012677997229991",
                "0.4895578341011574769173001023668144429398",
                "0.4902264832882911542295984490366087200184",
                "0.4908948440878151230319462074139837328369",
                "0.4915629161065499449049876773209036150379",
                "0.4922306989514860637273474669814266807670",
                "0.4928981922297840368730266887588092682397",
                "0.4935653955487747663087257684412713034102",
                "0.4942323085159597295909579194755076004688",
                "0.4948989307390112107628173924975988899026",
                "0.4955652618257725311502666695414869190434",
                "0.4962313013842582800578068314155939782620",
                "0.4968970490226545453633953859284540951197",
                "0.4975625043493191440124759039143460887357",
                "0.4982276669727818524109838693598428202796",
                "0.4988925365017446367171932093618813988311",
                "0.4995571125450818830322680291573808438992",
                "0.5002213947118406274893841370535517588066",
                "0.5008853826112407862412850037568218781730",
                "0.5015490758526753853461368603467068162798",
                "0.5022124740457107905515476989689518127033",
                "0.5028755768000869369766150002298225520111",
                "0.5035383837257175586918670712604959834511",
                "0.5042008944326904181969629384870601955669",
                "0.5048631085312675357960157992876404739599",
                "0.5055250256318854188704050969435913056132",
                "0.5061866453451552910489423435964958612549",
                "0.5068479672818633212752558763068599123923",
                "0.5075089910529708527722597917738406994625",
                "0.5081697162696146319035723658180773929283",
                "0.5088301425431070369317493243516528643706",
                "0.5094902694849363066731973932603808473558",
                "0.5101500967067667690496336154039425091534",
                "0.5108096238204390695359559837988562187468",
                "0.5114688504379703995043910009878180819646",
                "0.5121277761715547244647838356165627723798",
                "0.5127864006335630122008968083360284217919",
                "0.5134447234365434608025820003232299050457",
                "0.5141027441932217265936938389688157726080",
                "0.5147604625165011519556075766127693162392",
                "0.5154178780194629930462096396220777173381",
                "0.5160749903153666474142258865953987968160",
                "0.5167317990176498815087538760497663859531",
                "0.5173883037399290580838653055931565495758",
                "0.5180445040959993634981448463142515496662",
                "0.5187003996998350349090316579269502264729",
                "0.5193559901655895873618299320920460363301",
                "0.5200112751075960407732548733019909154403",
                "0.5206662541403671468093805887567479852951",
                "0.5213209268785956156578564207793723804531",
                "0.5219752929371543426942583175191106190748",
                "0.5226293519310966350424418999674383607982",
                "0.5232831034756564380287639456685274688734",
                "0.5239365471862485615300390719401093315139",
                "0.5245896826784689062150984639335456725563",
                "0.5252425095680946896798175554530938190616",
                "0.5258950274710846724754796331238237726623",
                "0.5265472360035793840303423972453725854225",
                "0.5271991347819013484642745754946705568645",
                "0.5278507234225553102963297485449066877404",
                "0.5285020015422284600451246096502806431981",
                "0.5291529687577906597218889433064781423760",
                "0.5298036246862946682160546712352691063333",
                "0.5304539689449763665732513771581259188732",
                "0.5311040011512549831655757851182555965925",
                "0.5317537209227333187540027294828972908813",
                "0.5324031278771979714428052182081180752113",
                "0.5330522216326195615258512544766080796666",
                "0.5337010018071529562246451454250933492473",
                "0.5343494680191374943179810903619139051666",
                "0.5349976198870972106630769046370179155603",
                "0.5356454570297410606080557991660631243585",
                "0.5362929790659631442956441995274561894394",
                "0.5369401856148429308579536525459617427878",
                "0.5375870762956454825022149323489381597210",
                "0.5382336507278216784873325220312685191689",
                "0.5388799085310084389911277112926153147001",
                "0.5395258493250289488681386147156983530570",
                "0.5401714727298928812978454797368391243805",
                "0.5408167783657966213231897178199938714808",
                "0.5414617658531234892792551568828737042948",
                "0.5421064348124439641119800776384854343920",
                "0.5427507848645159065867686612074833318665",
                "0.5433948156302847823868705401260616765033",
                "0.5440385267308838851013972097207026846125",
                "0.5446819177876345591028441217448859947984",
                "0.5453249884220464223139873471738261971864",
                "0.5459677382558175888640237601313956528686",
                "0.5466101669108348916338237600785727898035",
                "0.5472522740091741046901656146249928489722",
                "0.5478940591731001656088205706344303158432",
                "0.5485355220250673976863579466812715873458",
                "0.5491766621877197320405394853782043285940",
                "0.5498174792838909295991723096354179562185",
                "0.5504579729366048029772898925285391908408",
                "0.5510981427690754382425305161462790491981",
                "0.5517379884047074165685827605593043507626",
                "0.5523775094670960357765676298991291032867",
                "0.5530167055800275317642269884598102702775",
                "0.5536555763674792998227880457358896295838",
                "0.5542941214536201158413736953873098795765",
                "0.5549323404628103573988285792759099662521",
                "0.5555702330196022247428308139485328743749",
                "0.5562077987487399616561593832487198868307",
                "0.5568450372751600762099872671223795620379",
                "0.5574819482239915614040704431426683578980",
                "0.5581185312205561156937029638155638457395",
                "0.5587547858903683634033083793402116674752",
                "0.5593907118591360750265378421870446072618",
                "0.5600263087527603874127452966218671439619",
                "0.5606615761973360238397102231455323464062",
                "0.5612965138191515139724784747364706487604",
                "0.5619311212446894137081918087771225322802",
                "0.5625653981006265249067767856152400316806",
                "0.5631993440138341150073637718570158237456",
                "0.5638329586113781365303068537110339423981",
                "0.5644662415205194464646755330000733532588",
                "0.5650991923687140255410891458317951220305",
                "0.5657318107836131973897650113692660904743",
                "0.5663640963930638475836513856680771522393",
                "0.5669960488251086425665163631484626746774",
                "0.5676276677079862484658639359482795737138",
                "0.5682589526701315497905484891559202039444",
                "0.5688899033401758680129590777511727179321",
                "0.5695205193469471800356448989866659751479",
                "0.5701508003194703365422534419228034956126",
                "0.5707807458869672802326528638849623650066",
                "0.5714103556788572639421102117431683713025",
                "0.5720396293247570686443971741214179078787",
                "0.5726685664544812213386951189262601901897",
                "0.5732971666980422128201712389421399336044",
                "0.5739254296856507153340976966742906157841",
                "0.5745533550477158001133857281286215404280",
                "0.5751809424148451547994067338020178284786",
                "0.5758081914178453007459724538157308417760",
                "0.5764351016877218102063463928590370021996",
                "0.5770616728556795234031587294200450718036",
                "0.5776879045531227654810970126653952273701",
                "0.5783137964116555633422450192905771729678",
                "0.5789393480630818623639422116976585191477",
                "0.5795645591394057429990363079673181057693",
                "0.5801894292728316372584015432771812014450",
                "0.5808139580957645450755952716785138763991",
                "0.5814381452408102505535256264783115732546",
                "0.5820619903407755380930030268836712039073",
                "0.5826854930286684084030483880500261481835",
                "0.5833086529376982943928309612343084453657",
                "0.5839314697012762769451088003883413328491",
                "0.5845539429530153005710449212367171331275",
                "0.5851760723267303889462722886670393130354",
                "0.5857978574564388603280808381186622857671",
                "0.5864192979763605428535998065889071008564",
                "0.5870403935209179897188487188831244303335",
                "0.5876611437247366942385304448168770271375",
                "0.5882815482226453047864398132348808771714",
                "0.5889016066496758396163613389421363179679",
                "0.5895213186410639015633296889478561398105",
                "0.5901406838322488926251265848023157606156",
                "0.5907597018588742284238879082605695714561",
                "0.5913783723567875525476948480350560391437",
                "0.5919966949620409507720229960014106359200",
                "0.5926146693108911651609233718982786105364",
                "0.5932322950397998080478094263125274426680",
                "0.5938495717854335758957241425659599129653",
                "0.5944664991846644630369614290183814097305",
                "0.5950830768745699752919160642746376723259",
                "0.5956993044924333434670365288299698895119",
                "0.5963151816757437367317551278086911330297",
                "0.5969307080621964758742698806457296744178",
                "0.5975458832896932464360527248289689243904",
                "0.5981607069963423117249586521624986237341",
                "0.5987751788204587257068104674268355338417",
                "0.5993892984005645457753339308018322115353",
                "0.6000030653753890454003181169813274512048",
                "0.6006164793838689266538758955455595386440",
                "0.6012295400651485326146795088679214431183",
                "0.6018422470585800596500462956167432916696",
                "0.6024546000037237695757496797703996937211",
                "0.6030665985403482016934306169951154427009",
                "0.6036782423084303847054847622393404994599",
                "0.6042895309481560485073006944764416013301",
                "0.6049004640999198358567246066786709391905",
                "0.6055110414043255139206269413298796581826",
                "0.6061212625021861856984465240822029779326",
                "0.6067311270345245013225878205329119643756",
                "0.6073406346425728692355470135417618435700",
                "0.6079497849677736672436426710264256112166",
                "0.6085585776517794534472268467639418999217",
                "0.6091670123364531770472525293894849165389",
                "0.6097750886638683890280734275201390082018",
                "0.6103828062763094527163521517406882689737",
                "0.6109901648162717542159529270706697275541",
                "0.6115971639264619127186950424870441854346",
                "0.6122038032497979906908433171047687866977",
                "0.6128100824294097039352119357182669348358",
                "0.6134160011086386315287580795802412252795",
                "0.6140215589310384256355418515404205887059",
                "0.6146267555403750211949290679856307508247",
                "0.6152315905806268454849135634139842776594",
                "0.6158360636959850275604357269399597170731",
                "0.6164401745308536075665740635636354504756",
                "0.6170439227298497459264866466463195736592",
                "0.6176473079378039324039794017162291302857",
                "0.6182503297997601950405782354816769808082",
                "0.6188529879609763089669820977553791160939",
                "0.6194552820669240050887741378919558886977",
                "0.6200572117632891786462681913114239630541",
                "0.6206587766959720976483679057244182637443",
                "0.6212599765110876111803158907900012708913",
                "0.6218608108549653575852103491241670736344",
                "0.6224612793741499725191667208364860040791",
                "0.6230613817154012968800019481037187502553",
                "0.6236611175256945846093190406926128602187",
                "0.6242604864522207103678696978194357308810",
                "0.6248594881423863770840728162810527125579",
                "0.6254581222438143233755667894114829993246",
                "0.6260563884043435308436735761088156195782",
                "0.6266542862720294312406525939400991519496",
                "0.6272518154951441135096225651662877879064",
                "0.6278489757221765306970295194354890208444",
                "0.6284457666018327067375392318705704968198",
                "0.6290421877830359431112324503266013068053",
                "0.6296382389149270253729813407145820848290",
                "0.6302339196468644295538856544804145088338",
                "0.6308292296284245284346471975920299477702",
                "0.6314241685094017976907612557229947834522",
                "0.6320187359398090219094037057276920411962",
                "0.6326129315698775004778926189813010293478",
                "0.6332067550500572533436032377072143126599",
                "0.6338002060310172266452152810352000843329",
                "0.6343932841636454982151716132254933706757",
                "0.6349859890990494829532273822570369732136",
                "0.6355783204885561380709688128122480562156",
                "0.6361702779837121682071809135959142117571",
                "0.6367618612362842304139434349020790264832",
                "0.6373530698982591390133344883900169393253",
                "0.6379439036218440703246213171485757809069",
                "0.6385343620594667672618177803172380449035",
                "0.6391244448637757438014881927921738391133",
                "0.6397141516876404893206772368762847377151",
                "0.6403034821841516728048457391337235027326",
                "0.6408924360066213469256921821815749170596",
                "0.6414810128085831519887398976942527869153",
                "0.6420692122437925197505699635096625000826",
                "0.6426570339662268771055799044102522979977",
                "0.6432444776300858496421483729066845234380",
                "0.6438315428897914650680860631769553884420",
                "0.6444182293999883565052531882093330749593",
                "0.6450045368155439656532239271634239979127",
                "0.6455904647915487458218783269999705497658",
                "0.6461760129833163648328022195365852888822",
                "0.6467611810463839077893757922634909710809",
                "0.6473459686365120797154315285004176537080",
                "0.6479303754096854080623623097930618817587",
                "0.6485144010221124450845605508348932123722",
                "0.6490980451302259700830693146575545223676",
                "0.6496813073906831915173264333605991071689",
                "0.6502641874603659489848827372487939110537",
                "0.6508466849963809150689755729126486795481",
                "0.6514287996560597970538388685251597052322",
                "0.6520105310969595385076310824349384173514",
                "0.6525918789768625207328624490128835714847",
                "0.6531728429537767640842030136563054150769",
                "0.6537534226859361291535530268708750838372",
                "0.6543336178318005178222573454369067277578",
                "0.6549134280500560741803455668222375377583",
                "0.6554928529996153853126797012293059676952",
                "0.6560718923396176819518912639588950037387",
                "0.6566505457294290389979897491373592650613",
                "0.6572288128286425759045245242879459266743",
                "0.6578066932970786569311822637300037965084",
                "0.6583841867947850912627021173624061730700",
                "0.6589612929820373329939908900293451350059",
                "0.6595380115193386809813205853777414067295",
                "0.6601143420674204785594907468958086011814",
                "0.6606902842872423131248381076717671174101",
                "0.6612658378399922155839761393302748818523",
                "0.6618410023870868596681471695917830422144",
                "0.6624157775901717611130698169566881852974",
                "0.6629901631111214767041645701417921479911",
                "0.6635641586120398031870404190911494763874",
                "0.6641377637552599760431255236468344734313",
                "0.6647109782033448681303249852974479320667",
                "0.6652838016190871881885888668232614093283",
                "0.6658562336655096792102736841267175476491",
                "0.6664282740058653166751806740755217427639",
                "0.6669999223036375066501542217927266025220",
                "0.6675711782225402837531239105039793068409",
                "0.6681420414265185089814737367964262947260",
                "0.6687125115797480674046221139566027582255",
                "0.6692825883466360657206963659359292635497",
                "0.6698522713918210296771854944421474542444",
                "0.6704215603801731013554550816731041879316",
                "0.6709904549767942363190082712956915441234",
                "0.6715589548470184006253768504274218032288",
                "0.6721270596564117677015265356010145831702",
                "0.6726947690707729150826606459834506369503",
                "0.6732620827561330210143064274801561288764",
                "0.6738290003787560609175683717822752457224",
                "0.6743955216051390037174329549103204485697",
                "0.6749616461020120080340093003708111780926",
                "0.6755273735363386182365903526737759973180",
                "0.6760927035753159603604192276581525580928",
                "0.6766576358863749378860454868391269374021",
                "0.6772221701371804273811561638262612941489",
                "0.6777863059956314740047664517638188929895",
                "0.6783500431298614868736550417149607364422",
                "0.6789133812082384342909291829494107070060",
                "0.6794763198993650388366046171997185749457",
                "0.6800388588720789723200856201243447624011",
                "0.6806009977954530505944304644563996185349",
                "0.6811627363387954282322877006249453617882",
                "0.6817240741716497930633887320112629525093",
                "0.6822850109637955605734822434453511011659",
                "0.6828455463852480681645961230581124832961",
                "0.6834056801062587692765125991821450719580",
                "0.6839654117973154273693423956387463217829",
                "0.6845247411291423097670837904606067163557",
                "0.6850836677727003813620525448786688535463",
                "0.6856421913991874981800687512477106831299",
                "0.6862003116800386008062867304983285796036",
                "0.6867580282869259076715541916830994431769",
                "0.6873153408917591081991869482317427466776",
                "0.6878722491666855558120455676440351194353",
                "0.6884287527840904607998004135300033755529",
                "0.6889848514165970830462716211544885085313",
                "0.6895405447370669246167306299574847028455",
                "0.6900958324185999222050499789026654323954",
                "0.6906507141345346394405881529541647631417",
                "0.6912051895584484590546963514959375508647",
                "0.6917592583641577749067341320888287838269",
                "0.6923129202257181838694809656077912840335",
                "0.6928661748174246775738308215154537304725",
                "0.6934190218138118340126569848084088448124",
                "0.6939714608896540090037343890191158870916",
                "0.6945234917199655275116068325691436094854",
                "0.6950751139800008748282865287485707347868",
                "0.6956263273452548876126735226416620483367",
                "0.6961771314914629447885825914304004643728",
                "0.6967275260946011583012653276850300577975",
                "0.6972775108308865637323151884944031089788",
                "0.6978270853767773107728433765985767222992",
                "0.6983762494089728535548135030617225398363",
                "0.6989250026044141408404230644649474320693",
                "0.6994733446402838060694198511060247213498",
                "0.7000212751940063572642414862662553558027",
                "0.7005687939432483667928663802436673373587",
                "0.7011159005659186609892644665564704151080",
                "0.7016625947401685096313361714910623427718",
                "0.7022088761443918152762281520048835868674",
                "0.7027547444572253024529144208959899415322",
                "0.7033001993575487067119315621183076998324",
                "0.7038452405244849635321568231541044540476",
                "0.7043898676374003970845179554532008146819",
                "0.7049340803759049088525237581118148771393",
                "0.7054778784198521661095043641926226149618",
                "0.7060212614493397902524503933815839897078",
                "0.7065642291447095449923401790362768519611",
                "0.7071067811865475244008443621048490392848",
                "0.7076489172556843408132972288841937904756",
                "0.7081906370331953125878242541405249882449",
                "0.7087319402004006517205153957351270731820",
                "0.7092728264388656513165337715826299609991",
                "0.7098132954304008729170494345186621248823",
                "0.7103533468570623336818880454681153193102",
                "0.7108929804011516934277843301844623268490",
                "0.7114321957452164415221302897745546729365",
                "0.7119709925720500836321082202320405051646",
                "0.7125093705646923283290986812759337646322",
                "0.7130473294064292735482536399288843410749",
                "0.7135848687807935929031250994722950161935",
                "0.7141219883715647218552396096825545383937",
                "0.7146586878627690437385091395842569765778",
                "0.7151949669386800756383688793523053722967",
                "0.7157308252838186541255326234552024122146",
                "0.7162662625829531208442564726565621022823",
                "0.7168012785210995079550016780808839292805",
                "0.7173358727835217234313875362028643946668",
                "0.7178700450557317362113253293369297123442",
                "0.7184037950234897612022243919852074507325",
                "0.7189371223728044441401614692477635072898",
                "0.7194700267899330463029046194085635296484",
                "0.7200025079613816290766829987842242033653",
                "0.7205345655739052383765939529601491358969",
                "0.7210661993145080889205389246400457661129",
                "0.7215974088704437483565797745000431672446",
                "0.7221281939292153212436071976676250998279",
                "0.7226585541785756328852130047383074836577",
                "0.7231884893065274130166581225993738302355",
                "0.7237179990013234793448282567499853084375",
                "0.7242470829514669209410692432905531674831",
                "0.7247757408457112814867942053013493366947",
                "0.7253039723730607423717547149408852475892",
                "0.7258317772227703056448682492685583297616",
                "0.7263591550843459768174943145333992297607",
                "0.7268861056475449475190517004713995663672",
                "0.7274126286023757780048694130178089037919",
                "0.7279387236390985795161639207679094965016",
                "0.7284643904482251964920354375100570986875",
                "0.7289896287205193886333760502081645636563",
                "0.7295144381469970128185825889272508723924",
                "0.7300388184189262048709672223751303778510",
                "0.7305627692278275611777588499757241467644",
                "0.7310862902654743201605884486947860014193",
                "0.7316093812238925435973516202069988505356",
                "0.7321320417953612977953416714243607545869",
                "0.7326542716724128346155466488994934632930",
                "0.7331760705478327723480038351739174212638",
                "0.7336974381146602764381053027603949547166",
                "0.7342183740661882400637482091300959837827",
                "0.7347388780959634645632236038195365703151",
                "0.7352589498977868397137376065789282926435",
                "0.7357785891657135238604589033527041820022",
                "0.7362977955940531238959865948145030692193",
                "0.7368165688773698750901325201727469468679",
                "0.7373349087104828207699122670190835869220",
                "0.7378528147884659918496391661103373658704",
                "0.7383702868066485862110156581551632004620",
                "0.7388873244606151479331165079192798134315",
                "0.7394039274462057463721584292679173187923",
                "0.7399200954595161550909507731308983931715",
                "0.7404358281968980306379220188045300992927",
                "0.7409511253549590911756168974951627297290",
                "0.7414659866305632949585590655618207866681",
                "0.7419804117208310186603743335296773134595",
                "0.7424944003231392355500695456222741166150",
                "0.7430079521351216935173622932982347968200",
                "0.7435210668546690929469567350777227421870",
                "0.7440337441799292644416608838050100880053",
                "0.7445459838093073463942408114161938398820",
                "0.7450577854414659624079073102652695805364",
                "0.7455691487753253985653306391073990874882",
                "0.7460800735100637805460790709452333962358",
                "0.7465905593451172505923770491125249318746",
                "0.7471006059801801443230788471989288460126",
                "0.7476102131152051673957537177108021437778",
                "0.7481193804504035720167786037149070401454",
                "0.7486281076862453332993345771251596822023",
                "0.7491363945234593254692032567668843148057",
                "0.7496442406630334979182595488883835159585",
                "0.7501516458062150511055571423859646137555",
                "0.7506586096545106123059032806658181117621",
                "0.7511651319096864112058194217842731511005",
                "0.7516712122737684553467844882869049464788",
                "0.7521768504490427054156574980066869352693",
                "0.7526820461380552503821764569818132231201",
                "0.7531867990436124824834304856149119103704",
                "0.7536911088687812720552022392170741189375",
                "0.7541949753168891422100777741623840518984",
                "0.7546983980915244433622211010213992146376",
                "0.7552013768965365275987107562452439785002",
                "0.7557039114360359228973358142355909111542",
                "0.7562060014143945071907488519597596325716",
                "0.7567076465362456822768734686544092464244",
                "0.7572088465064845475754640536057844730404",
                "0.7577096010302680737307155854991442657115",
                "0.7582099098130152760598213373948016855312",
                "0.7587097725604073878473764520130818641198",
                "0.7592091889783880334855254426954076862902",
                "0.7597081587731634014597517661535970283341",
                "0.7602066816512024171802077039242476092761",
                "0.7607047573192369156584828803097423340565",
                "0.7612023854842618140297098355118759758055",
                "0.7616995658535352839199051636483296706487",
                "0.7621962981345789236584448163861494591664",
                "0.7626925820351779303355722640299654449014",
                "0.7631884172633812717048382970658654583073",
                "0.7636838035275018579303713423845577852760",
                "0.7641787405361167131788772596896678789294",
                "0.7646732279980671470562676749386603227112",
                "0.7651672656224589258888159990649059180490",
                "0.7656608531186624438487403716897708620953",
                "0.7661539901963128939241128610532367565307",
                "0.7666466765653104387329943429704128084810",
                "0.7671389119358203811816945732593211757841",
                "0.7676306960182733349670570597824690628512",
                "0.7681220285233653949226684320009129471502",
                "0.7686129091620583072088920977547172320788",
                "0.7691033376455796393466260688578576671916",
                "0.7695933136854229500946849290286650041912",
                "0.7700828369933479591707060096687924849578",
                "0.7705719072813807168154799310543677697071",
                "0.7710605242618137732006057586124016581018",
                "0.7715486876472063476793711161236182481906",
                "0.7720363971503844978807576899195897973103",
                "0.7725236524844412886464726504273502409484",
                "0.7730104533627369608109066097584698009710",
                "0.7734967994988990998239188264418450500912",
                "0.7739826906068228042163504608601398371084",
                "0.7744681264006708539081667774688482250542",
                "0.7749531065948738783591292824542866253703",
                "0.7754376309041305245618988781224041647538",
                "0.7759216990434076248774712080040735022190",
                "0.7764053107279403647128454594144342928658",
                "0.7768884656732324500408279830138537082772",
                "0.7773711635950562747618721817860882736425",
                "0.7778534042094530879078562147752241367711",
                "0.7783351872327331606877001539068840781510",
                "0.7788165123814759533747243252609644040753",
                "0.7792973793725302820356506592627486025323",
                "0.7797777879230144851011489674165825159470",
                "0.7802577377503165897778301564213329939020",
                "0.7807372285720944783015884837795336966664",
                "0.7812162601062760540321950523423930567825",
                "0.7816948320710594073890448346206454673368",
                "0.7821729441849129816279596111365126142067",
                "0.7826505961665757384589493005947525340923",
                "0.7831277877350573235048342532108544552676",
                "0.7836045186096382316006311721518327247396",
                "0.7840807885098699719336054217197280711770",
                "0.7845565971555752330238925746397839911870",
                "0.7850319442668480475455921446042750372309",
                "0.7855068295640539569882365440690670550275",
                "0.7859812527678301761585384012031317594457",
                "0.7864552135990857575223194638513642098426",
                "0.7869287117790017553865244123881064742327",
                "0.7874017470290313899212229974127087487683",
                "0.7878743190709002110215040123692050835095",
                "0.7883464276266062620091647053596892826565",
                "0.7888180724184202431740993286651920892423",
                "0.7892892531688856751552906187887279892793",
                "0.7897599696008190621613080941926434037572",
                "0.7902302214373100550302171523164021706370",
                "0.7907000084017216141288030419314335117479",
                "0.7911693302176901720910138814165865567144",
                "0.7916381866091257963955269881210283499769",
                "0.7921065773002123517823428786210334566942",
                "0.7925745020154076625083113943729871347983",
                "0.7930419604794436744414945020170048465558",
                "0.7935089524173266169942704123938029034797",
                "0.7939754775543371648950837572017825000808",
                "0.7944415356160305997987466571416564885147",
                "0.7949071263282369717351956103722991384614",
                "0.7953722494170612603966092251337779424279",
                "0.7958369046088835362627919154816773610505",
                "0.7963010916303591215647287742207913179826",
                "0.7967648102084187510862169323259882788429",
                "0.7972280600702687328034788093934838799157",
                "0.7976908409433911083626627549768352735345",
                "0.7981531525555438133951366760286425592778",
                "0.7986149946347608376704803410911497715135",
                "0.7990763669093523850870821473566247600198",
                "0.7995372691079050335002462322515077643195",
                "0.7999977009592818943877159067867963443233",
                "0.8004576621926227723525194835609233576543",
                "0.8009171525373443244630446680004286028326",
                "0.8013761717231402194302477771779672800629",
                "0.8018347194799812966219041463565832279311",
                "0.8022927955381157249138061792736456126883",
                "0.8027503996280691613778155940973479823772",
                "0.8032075314806449098066765129631419238796",
                "0.8036641908269240790754961390268675177051",
                "0.8041203773982657413397998610555928814076",
                "0.8045760909263070900700677217162288039948",
                "0.8050313311429635979226592819157852444787",
                "0.8054860977804291744470340097956277094556",
                "0.8059403905711763236291744192852166167959",
                "0.8063942092479563012711192794785150425268",
                "0.8068475535437992722065143125084730510426",
                "0.8073004231920144673520878940616834080544",
                "0.8077528179261903405949593671963971223303",
                "0.8082047374801947255156876767025311754024",
                "0.8086561815881749919469681278720381651798",
                "0.8091071499845582023678851702319815650335",
                "0.8095576424040512681336292035308140077041",
                "0.8100076585816411055405855000606325347761",
                "0.8104571982525947917267034342445271670501",
                "0.8109062611524597204070543073184904767302",
                "0.8113548470170637574444861518916590749323",
                "0.8118029555825153962552839981768560319113",
                "0.8122505865852039130497441807454391551551",
                "0.8126977397617995219075713617762766630300",
                "0.8131444148492535296880070439382119782277",
                "0.8135906115847984907745984432685859635469",
                "0.8140363297059483616545166896872007386115",
                "0.8144815689504986553323334201154770245562",
                "0.8149263290565265955781649265544195075421",
                "0.8153706097623912710100931189123047027827",
                "0.8158144108067337890107726598636859187152",
                "0.8162577319284774294781337265653128147838",
                "0.8167005728668277984100899516518313333918",
                "0.8171429333612729813231611935846060529621",
                "0.8175848131515836965049208841306338094710",
                "0.8180262119778134481001777985052372917525",
                "0.8184671295802986790308021915219827235581",
                "0.8189075656996589237491063409559991638357",
                "0.8193475200767969608246896372425308158364",
                "0.8197869924528989653646584566010704342182",
                "0.8202259825694346612671311526967438249307",
                "0.8206644901681574733079386000246848819186",
                "0.8211025149911046790604308203298999031279",
                "0.8215400567805975606483003215545113504917",
                "0.8219771152792415563313328770362370081024",
                "0.8224136902299264119239965709664428623158",
                "0.8228497813758263320467800344530491588481",
                "0.8232853884604001312101908949229111359261",
                "0.8237205112273913847313255600399810093283",
                "0.8241551494208285794829215558095279839955",
                "0.8245893027850252644748037370848904463286",
                "0.8250229710645802012676357872916010791502",
                "0.8254561540043775142188885228342034346863",
                "0.8258888513495868405609366163536094663180",
                "0.8263210628456634803111954517573735930749",
                "0.8267527882383485460142099227517219571733",
                "0.8271840272736691123156070854625175090335",
                "0.8276147796979383653678246746425042603263",
                "0.8280450452577557520675275919240992974059",
                "0.8284748237000071291246245735906307282292",
                "0.8289041147718649119627973444041953983668",
                "0.8293329182207882234514546631451736784479",
                "0.8297612337945230424690237646868315884265",
                "0.8301890612411023522974918026483046452264",
                "0.8306164003088462888481099959405347311044",
                "0.8310432507463622887181732818423625817479",
                "0.8314696123025452370787883776179057567386",
                "0.8318954847265776153935432521115167214520",
                "0.8323208677679296489679911082329594494608",
                "0.8327457611763594543298620767729072286924",
                "0.8331701647019131864399159215673905735273",
                "0.8335940780949251857333491556593537292151",
                "0.8340175011060181249916700677859525949630",
                "0.8344404334861031560449552582515873483500",
                "0.8348628749863800563044013830288509569496",
                "0.8352848253583373751250859047625314139254",
                "0.8357062843537525799988507492354722037251",
                "0.8361272517246922025772228657894134328629",
                "0.8365477272235119845242857901788464603636",
                "0.8369677106028570231994164083713589141859",
                "0.8373872016156619171698012198938658355897",
                "0.8378062000151509115526464994604574749805",
                "0.8382247055548380431869968558042860626363",
                "0.8386427179885272856350767868729037586449",
                "0.8390602370703126940130699308336929815181",
                "0.8394772625545785496512508126734394279019",
                "0.8398937941959995045833839865636283006706",
                "0.8403098317495407258653055746006365040633",
                "0.8407253749704580397226023030175887575577",
                "0.8411404236142980755273032375021846106133",
                "0.8415549774368984096034995198422270646027",
                "0.8419690361943877088618075087578327532071",
                "0.8423825996431858742625908284663191959713",
                "0.8427956675400041841078569292624872900173",
                "0.8432082396418454371617438651833876812949",
                "0.8436203157060040955995130946626186691621",
                "0.8440318954900664277849642109646915273566",
                "0.8444429787519106508761876101249572221987",
                "0.8448535652497070732595712051049570977198",
                "0.8452636547419182368119773959067797708480",
                "0.8456732469872990589910066064730178022547",
                "0.8460823417448969747532638003311612154502",
                "0.8464909387740520783005444881226811347222",
                "0.8468990378343972646538568413875861791063",
                "0.8473066386858583710551966282548172334642",
                "0.8477137410886543181969917880174232387448",
                "0.8481203448032972512791335629489720926245",
                "0.8485264495905926808935112071440369911965",
                "0.8489320552116396237359673936407999056617",
                "0.8493371614278307431455915426077706798315",
                "0.8497417680008524894712683949492727307512",
                "0.8501458746926852402653992573056347833150",
                "0.8505494812656034403047134460938926891937",
                "0.8509525874821757414380875599531863609594",
                "0.8513551931052651422612903117258743556752",
                "0.8517572978980291276185707529206228016109",
                "0.8521589016239198079310078254672962818619",
                "0.8525600040466840583515392774853260457885",
                "0.8529606049303636577465880817472955612247",
                "0.8533607040392954275042045975277049668651",
                "0.8537603011381113701686428185831944263075",
                "0.8541593959917388079012891521148616858098",
                "0.8545579883654005207678622757156412315482",
                "0.8549560780246148848518027215059612354569",
                "0.8553536647351960101937709389089998498111",
                "0.8557507482632538785571726898127642096640",
                "0.8561473283751944810196307322098535510157",
                "0.8565434048377199553903218507970819450543",
                "0.8569389774178287234530983954560660507983",
                "0.8573340458828156280353135910223687377957",
                "0.8577286100002720699022699842847701370425",
                "0.8581226695380861444772104967376533238768",
                "0.8585162242644427783867716542382820752700",
                "0.8589092739478238658318186673968526018145",
                "0.8593018183570084047835821392505594300777",
                "0.8596938572610726330050162795434672743578",
                "0.8600853904293901638972986077516653309715",
                "0.8604764176316321221713912298579374680214",
                "0.8608669386377672793445838767919507564613",
                "0.8612569532180621890619389954106851538462",
                "0.8616464611430813222425592858994383424595",
                "0.8620354621836872020505981825261811430164",
                "0.8624239561110405386909338777812499507349",
                "0.8628119426966003640294275930802826216538",
                "0.8631994217121241660376869024008724955604",
                "0.8635863929296680230622550184625710722427",
                "0.8639728561215867379181470543455525270743",
                "0.8643588110605339718066543767754019998543",
                "0.8647442575194623780573382706800436297334",
                "0.8651291952716237356941342380497228240096",
                "0.8655136240905690828254883576021393973799",
                "0.8658975437501488498584472352722331314429",
                "0.8662809540245129925366231791096900852593",
                "0.8666638546881111248019563357769057127931",
                "0.8670462455156926514801956294958485611966",
                "0.8674281262823069007900204479939550571688",
                "0.8678094967633032566757251237467906319943",
                "0.8681903567343312909633883626086741573021",
                "0.8685707059713408953404498757617203040594",
                "0.8689505442505824131586165747987469108724",
                "0.8693298713486067710600207936861606414414",
                "0.8697086870422656104265531053292129852704",
                "0.8700869911087114186522924044838488439108",
                "0.8704647833253976602389560328266903537455",
                "0.8708420634700789077142928261074480054814",
                "0.8712188313208109723733420674661682832640",
                "0.8715950866559510348424814352011506891190",
                "0.8719708292541577754661871375220358410739",
                "0.8723460588943915045164295311164190090146",
                "0.8727207753559142922246276246973186276966",
                "0.8730949784182900986360859730828656214302",
                "0.8734686678613849032868375717886153833524",
                "0.8738418434653668347028164665868585409804",
                "0.8742145050107062997212838970061577495493",
                "0.8745866522781761126344318973080041997487",
                "0.8749582850488516241550883830859077947457",
                "0.8753294031041108502044478562853475044107",
                "0.8757000062256346005217519661407516776885",
                "0.8760700941954066070958442682679904961153",
                "0.8764396667957136524185236289376826653598",
                "0.8768087238091456975596208263858842239779",
                "0.8771772650185960100637230058943773297843",
                "0.8775452902072612916684707502927493687195",
                "0.8779127991586418058443526324986859971057",
                "0.8782797916565415051559222217213340279881",
                "0.8786462674850681584443626200051596321008",
                "0.8790122264286334778313237108883723441412",
                "0.8793776682719532455439574070906440198602",
                "0.8797425928000474405610762893294623274084",
                "0.8801069997982403650803611335929587000526",
                "0.8804708890521607708065429294693790235531",
                "0.8808342603477419850604850974494597511895",
                "0.8811971134712220367090917184777706778825",
                "0.8815594482091437819159676944325262816390",
                "0.8819212643483550297127568636603883495084",
                "0.8822825616760086673910842011833215146211",
                "0.8826433399795627857150283387285582833140",
                "0.8830035990467808039540507453101190439112",
                "0.8833633386657315947363080147110533129482",
                "0.8837225586247896087222738118795589565750",
                "0.8840812587126349990985971359593341651246",
                "0.8844394387182537458921236634248603725863",
                "0.8847970984309377801040070405857408897591",
                "0.8851542376402851076638371005606675246429",
                "0.8855108561361999332037120857009936352961",
                "0.8858669537088927836521820623661946128166",
                "0.8862225301488806316479908209186334139362",
                "0.8865775852469870187735436598129571173858",
                "0.8869321187943421786080285587060682039789",
                "0.8872861305823831596001183516068789704221",
                "0.8876396204028539477601816172209067692593",
                "0.8879925880478055891719301098231391851090",
                "0.8883450333095963123234306602134293484588",
                "0.8886969559808916502574095825719098657589",
                "0.8890483558546645625407777293374767964899",
                "0.8893992327241955570533044425802301974446",
                "0.8897495863830728115953687567288024199682",
                "0.8900994166251922953147163139456970008199",
                "0.8904487232447578899521505599180370203448",
                "0.8907975060362815109060868943474195766942",
                "0.8911457647945832281158985569808298811145",
                "0.8914934993147913867639831366247217313719",
                "0.8918407093923427277964786972263580580505",
                "0.8921873948229825082625586217902631270394",
                "0.8925335554027646214722343816231040616931",
                "0.8928791909280517169725955451674308360721",
                "0.8932243011955153203424164474933979780006",
                "0.8935688860021359528050590483678050746229",
                "0.8939129451452032506596016137114639364573",
                "0.8942564784223160845301229621889650670366",
                "0.8945994856313826784330721256493119981034",
                "0.8949419665706207286626533791515561547314",
                "0.8952839210385575224941567033664342561019",
                "0.8956253488340300567051638492430218571824",
                "0.8959662497561851559145602819685074912350",
                "0.8963066236044795907392833884292989557538",
                "0.8966464701786801957687374396027335437975",
                "0.8969857892788639873568059065706143833675",
                "0.8973245807054182812313918361485724100228",
                "0.8976628442590408099214170994687947388867",
                "0.8980005797407398400012114342379021741203",
                "0.8983377869518342891522223088166381254528",
                "0.8986744656939538430419767437334850971686",
                "0.8990106157690390720202263337502899558994",
                "0.8993462369793415476322068211443921266693",
                "0.8996813291274239589489436784585464451102",
                "0.9000158920161602287145352665970513170516",
                "0.9003499254487356293103452418138697905508",
                "0.9006834292286468985360359928461027919006",
                "0.9010164031597023552073749971938767546646",
                "0.9013488470460220145707460933354787941263",
                "0.9016807606920377035342977734943480399479",
                "0.9020121439024931757156607094422483050023",
                "0.9023429964824442263061668317305414824730",
                "0.9026733182372588067515023906888894471609",
                "0.9030031089726171392487275355178712877982",
                "0.9033323684945118310595950558288488890069",
                "0.9036610966092479886401010380508836649209",
                "0.9039892931234433315862002972305370487101",
                "0.9043169578440283063956195528959135673096",
                "0.9046440905782462000457014258412643593266",
                "0.9049706911336532533872124409127971839717",
                "0.9052967593181187743540483291399726543292",
                "0.9056222949398252509887700308594419206616",
                "0.9059472978072684642839039098208346169213",
                "0.9062717677292576008389397966447738314040",
                "0.9065957045149153653329605884237134126506",
                "0.9069191079736780928128372397153982934833",
                "0.9072419779152958607969230886768769056843",
                "0.9075643141498326011941815706239823386393",
                "0.9078861164876662120386814798769818177571",
                "0.9082073847394886690393940493676084703117",
                "0.9085281187163061369452262261358713124067",
                "0.9088483182294390807252246295368250131708",
                "0.9091679830905223765638847877078063304795",
                "0.9094871131115054226715003566154452032092",
                "0.9098057081046522499094871348089713423912",
                "0.9101237678825416322306167958518977791523",
                "0.9104412922580671969340953692880071699803",
                "0.9107582810444375347354216099196306760629",
                "0.9110747340551763096509605041364288471866",
                "0.9113906511041223686971672710311950534461",
                "0.9117060320054298514043973250755404989167",
                "0.9120208765735682991452377762026215772052",
                "0.9123351846233227642772961522562711238234",
                "0.9126489559697939191003821379159307967214",
                "0.9129621904283981646280182333945881639288",
                "0.9132748878148677391732153454324348636928",
                "0.9135870479452508267484494323721171732400",
                "0.9138986706359116652797754344021832006025",
                "0.9142097557035306546350148293935774010447",
                "0.9145203029651044644659532641297288946375",
                "0.9148303122379461418644848201438607815289",
                "0.9151397833396852188326395828275489515773",
                "0.9154487160882678195664312919622163783863",
                "0.9157571103019567675534619613500981656340",
                "0.9160649657993316924842204647831892452924",
                "0.9163722823992891369770121951877261603829",
                "0.9166790599210426631164570134177923250274",
                "0.9169852981841229588054928128446080456725",
                "0.9172909970083779439308221355979078917043",
                "0.9175961562139728763417393860624541817600",
                "0.9179007756213904576422762970161218427528",
                "0.9182048550514309387966034136160521192271",
                "0.9185083943252122255476254702960459568297",
                "0.9188113932641699836487086455315877295785",
                "0.9191138516900577439084777893585916788760",
                "0.9194157694249470070486218284980823140408",
                "0.9197171462912273483746456639414913992885",
                "0.9200179821116065222595069858900133089514",
                "0.9203182767091105664400765410164427383300",
                "0.9206180299070839061263604971290592576357",
                "0.9209172415291894579234236604643572154653",
                "0.9212159113994087335659524110186822393706",
                "0.9215140393420419434653963315480622263672",
                "0.9218116251817081000696276161206464265432",
                "0.9221086687433451210350574543971261380207",
                "0.9224051698522099322111486981412397700753",
                "0.9227011283338785704372642268248986908391",
                "0.9229965440142462861517905395905434333471",
                "0.9232914167195276458134762112669875443719",
                "0.9235857462762566341349249606041636592763",
                "0.9238795325112867561281831893967882868224",
                "0.9241727752517911389623619617069432835133",
                "0.9244654743252626336332335029708680597726",
                "0.9247576295595139164447424093858021415057",
                "0.9250492407826775903023718686184477409811",
                "0.9253403078232062858183053035574713805112",
                "0.9256308305098727622283239615483672503830",
                "0.9259208086717700081203810822998977376017",
                "0.9262102421383113419747933884371432823411",
                "0.9264991307392305125159907534968692321300",
                "0.9267874743045817988757650130163864767691",
                "0.9270752726647401105679589952572801463724",
                "0.9273625256504010872745369590302413069028",
                "0.9276492330925811984429777370466951378884",
                "0.9279353948226178426949319942169102510773",
                "0.9282210106721694470460851213427323117044",
                "0.9285060804732155659371673957159456406234",
                "0.9287906040580569800760531512304636703823",
                "0.9290745812593157950908908117480176457754",
                "0.9293580119099355399942057526226874236996",
                "0.9296408958431812654579180664894332423899",
                "0.9299232328926396418992174206556774803092",
                "0.9302050228922190573772373047028852740947",
                "0.9304862656761497153004710782069369569828",
                "0.9307669610789837319448723398218081380298",
                "0.9310471089355952337825822503406093722852",
                "0.9313267090811804546212265537513212570342",
                "0.9316057613512578325537251517415268936091",
                "0.9318842655816681067185571985770264225996",
                "0.9321622216085744138704247947833521972812",
                "0.9324396292684623847612584695968225107257",
                "0.9327164883981402403315077537228110216584",
                "0.9329927988347388877116602555433024982950",
                "0.9332685604157120160339327655534875641665",
                "0.9335437729788361920540780254780541117184",
                "0.9338184363622109555832509102218952551662",
                "0.9340925504042589147298778825471074110554",
                "0.9343661149437258409514736921383315911381",
                "0.9346391298196807639163494025216305160168",
                "0.9349115948715160661751559411381279386121",
                "0.9351835099389475776422074797424988136309",
                "0.9354548748620146698865290641980238532729",
                "0.9357256894810803502325730246742437415729",
                "0.9359959536368313556705488092202009919622",
                "0.9362656671702782465763109956857752575591",
                "0.9365348299227555002407503489956349549526",
                "0.9368034417359216042086329028447784342570",
                "0.9370715024517591494268321569814556963744",
                "0.9373390119125749232018995933723808790288",
                "0.9376059699610000019669188267065004435605",
                "0.9378723764399898438575888168871062130570",
                "0.9381382311928243810974816833877101385885",
                "0.9384035340631081121924207736047628846644",
                "0.9386682848947701939339247496299349892402",
                "0.9389324835320645332116635701862214105870",
                "0.9391961298195698786348723568255116472076",
                "0.9394592236021899119626692458704222131584",
                "0.9397217647251533393432234400000499563131",
                "0.9399837530340139823617197858278073893437",
                "0.9402451883746508688970663162995787119412",
                "0.9405060705932683237872913092520213689164",
                "0.9407663995363960593035765260138666246461",
                "0.9410261750508892654328734065074775337004",
                "0.9412853969839286999690491099136376257702",
                "0.9415440651830207784125094025995023571856",
                "0.9418021794959976636782455076777816499389",
                "0.9420597397710173556122521432644693624491",
                "0.9423167458565637803162640892327280162609",
                "0.9425731976014468792807587350218082231327",
                "0.9428290948548026983261721738520656829861",
                "0.9430844374660934763522765215201680022297",
                "0.9433392252851077338956662508023925431435",
                "0.9435934581619603614953014453784386693434",
                "0.9438471359470927078660559901033466927657",
                "0.9441002584912726678802188274008651290453",
                "0.9443528256455947703568965225278710875313",
                "0.9446048372614802656592654934661592984650",
                "0.9448562931906772130996223742350069340972",
                "0.9451071932852605681521810934853275159473",
                "0.9453575373976322694735653633338813023476",
                "0.9456073253805213257309453865238450864520",
                "0.9458565570869839022377677031559947548172",
                "0.9461052323704034073970272114227526925860",
                "0.9463533510844905789520305099953325876291",
                "0.9466009130832835700445998229621097795148",
                "0.9468479182211480350806668814940893990578",
                "0.9470943663527772154032062497208705120645",
                "0.9473402573331920247724576956377456608530",
                "0.9475855910177411346533873212314649157949",
                "0.9478303672621010593103372794086651156877",
                "0.9480745859222762407088140187369516819949",
                "0.9483182468545991332243651104640555123044",
                "0.9485613499157302881584948257653042498390",
                "0.9488038949626584380615687446837789283196",
                "0.9490458818527005808626577917709068274846",
                "0.9492873104435020638062722070078025494721",
                "0.9495281805930366671959360741893450282522",
                "0.9497684921596066879445531425837015844305",
                "0.9500082450018430229315147913397143899862",
                "0.9502474389787052521665010998031829488015",
                "0.9504860739494817217599261006205415490837",
                "0.9507241497737896266999784062546762059304",
                "0.9509616663115750934362085133125844723556",
                "0.9511986234231132622696142028881867241981",
                "0.9514350209690083695491755689557821707501",
                "0.9516708588101938296747913207103399441313",
                "0.9519061368079323169065681186399581950728",
                "0.9521408548238158469804148180333451728217",
                "0.9523750127197658585298936075710087775911",
                "0.9526086103580332943142801446229180139554",
                "0.9528416476011986822527849028776540948229",
                "0.9530741243121722162648880619584335835136",
                "0.9533060403541938369167403827397938349041",
                "0.9535373955908333118735826261651150079528",
                "0.9537681898859903161581361874794459571042",
                "0.9539984231038945122149177319342362385961",
                "0.9542280951091056297804307321904861426773",
                "0.9544572057665135455591869218444439209704",
                "0.9546857549413383627055107937252370401711",
                "0.9549137424991304901110803858666551631832",
                "0.9551411683057707214981577123356394246484",
                "0.9553680322274703143184623104078082014735",
                "0.9555943341307710684576414899154967360519",
                "0.9558200738825454047452909849562393900101",
                "0.9560452513499964432704798225393117130006",
                "0.9562698664006580815027333371648076117900",
                "0.9564939189023950722184283747736873911853",
                "0.9567174087234031012325548439782279857530",
                "0.9569403357322088649357978869802699694828",
                "0.9571626997976701476368950571095195633342",
                "0.9573845007889758987102230044658604859395",
                "0.9576057385756463095485682857280927228283",
                "0.9578264130275328903210370287966757208084",
                "0.9580465240148185465360582975698447273385",
                "0.9582660714080176554094361168108235549264",
                "0.9584850550779761420374052317487075072176",
                "0.9587034748958715553746457917668690903737",
                "0.9589213307332131440172122622703789688824",
                "0.9591386224618419317903319835878679118857",
                "0.9593553499539307931410289105534147018128",
                "0.9595715130819845283355281812303626134486",
                "0.9597871117188399384613972780813755750852",
                "0.9600021457376659002343796597574769352620",
                "0.9602166150119634406098768565732013046579",
                "0.9604305194155658111990351376552656335496",
                "0.9606438588226385624893929726982619162600",
                "0.9608566331076796178700456262327230653533",
                "0.9610688421455193474612833373084479350258",
                "0.9612804858113206417486596525191235450944",
                "0.9614915639805789850214465953429845990464",
                "0.9617020765291225286154334698484347216033",
                "0.9619120233331121639600262119131527679746",
                "0.9621214042690415954296043162301533683259",
                "0.9623302192137374129990924825244958419028",
                "0.9625384680443591647037042395797720058369",
                "0.9627461506383994289028149208740834591688",
                "0.9629532668736838863479214808508748735205",
                "0.9631598166283713920546467561006538838535",
                "0.9633657997809540469787458910052325468620",
                "0.9635712162102572694960727626966022071031",
                "0.9637760657954398666864643555078351536631",
                "0.9639803484159941054215011504434248201017",
                "0.9641840639517457832561017105711636201681",
                "0.9643872122828542991239097586369459493794",
                "0.9645897932898127238364321586277055315770",
                "0.9647918068534478703858863284559832491545",
                "0.9649932528549203640517157264123059505430",
                "0.9651941311757247123107321695285705491326",
                "0.9653944416976893745508438575169030701868",
                "0.9655941843029768315883280914939312082799",
                "0.9657933588740836549886077922700034549379",
                "0.9659919652938405761904910385765399578645",
                "0.9661900034454125554338329612223419786344",
                "0.9663874732122988504905794448112501101421",
                "0.9665843744783330851991522043189582694605",
                "0.9667807071276833178021349195159928531066",
                "0.9669764710448521090872202259367862730480",
                "0.9671716661146765903313774768313433121909",
                "0.9673662922223285310482013062961492627216",
                "0.9675603492533144065384011395646325408175",
                "0.9677538370934754652433919122446032943537",
                "0.9679467556289877959019463761205753099653",
                "0.9681391047463623945098694849926731421031",
                "0.9683308843324452310826554699008616757231",
                "0.9685220942744173162210883289834431285218",
                "0.9687127344597947674797465731420786170470",
                "0.9689028047764288755383731846319396588938",
                "0.9690923051125061701760718616649111522775",
                "0.9692812353565484860482907381059832428043",
                "0.9694695953974130282665548833580168200046",
                "0.9696573851242924377809090035678779344878",
                "0.9698446044267148565650318813474419283632",
                "0.9700312531945439926039842072861002514569",
                "0.9702173313179791846845515726370935051058",
                "0.9704028386875554669881445086881749119559",
                "0.9705877751941436334862175744777108390072",
                "0.9707721407289503021381696106902808737283",
                "0.9709559351835179788916873937610809278542",
                "0.9711391584497251214854950404358905658104",
                "0.9713218104197862030544716292729718568448",
                "0.9715038909862517755370996218349531511233",
                "0.9716854000420085328852067826024488865114",
                "0.9718663374802793740759644129468074378043",
                "0.9720467031946234659261048308268947041884",
                "0.9722264970789363057083211442241431612174",
                "0.9724057190274497835698124827011560352777",
                "0.9725843689347322447529379678618856332851",
                "0.9727624466956885516179428199057352073718",
                "0.9729399522055601454677201139037965700244",
                "0.9731168853599251081745718158827625033078",
                "0.9732932460546982236089328452807753176140",
                "0.9734690341861310388700220268395221893709",
                "0.9736442496508119253183839115181956093636",
                "0.9738188923456661394102855625574348572697",
                "0.9739929621679558833339325193849833247504",
                "0.9741664590152803654474682686394681675847",
                "0.9743393827855758605187216681943645931426",
                "0.9745117333771157697666668866907784868427",
                "0.9746835106885106807045605377350994417435",
                "0.9748547146187084267847208045857729583821",
                "0.9750253450669941468449134678423469945313",
                "0.9751954019329903443563098653594955215901",
                "0.9753648851166569464729819303388416286607",
                "0.9755337945182913628828995703020263376554",
                "0.9757021300385285444603957664195279716440",
                "0.9758698915783410417200648894611619606920",
                "0.9760370790390390630720598454459136692629",
                "0.9762036923222705328787537809007084695356",
                "0.9763697313300211493127321944898351365001",
                "0.9765351959646144420160814186489419623471",
                "0.9767000861287118295609395517497510265580",
                "0.9768644017253126767112760382338160495447",
                "0.9770281426577543514858662110857144252620",
                "0.9771913088297122820224272279679455837949",
                "0.9773539001452000132428819493114369787322",
                "0.9775159165085692633197174236468669147820",
                "0.9776773578245099799434047624729313055873",
                "0.9778382239980503963908473039881404455577",
                "0.9779985149345570873948240820626631467400",
                "0.9781582305397350248143957338960702748455",
                "0.9783173707196276331062400968954989486663",
                "0.9784759353806168445968848624166935639962",
                "0.9786339244294231545558047711375114752429",
                "0.9787913377731056760693509519797407123038",
                "0.9789481753190621947154801236603956128550",
                "0.9791044369750292230392514951379647856272",
                "0.9792601226490820548290593184223154520073",
                "0.9794152322496348191935691644390399937745",
                "0.9795697656854405344393261098798955052132",
                "0.9797237228655911617490031402305663384731",
                "0.9798771036995176586602581914452040610744",
                "0.9800299080969900323451683700339999221246",
                "0.9801821359681173926902100086453528464397",
                "0.9803337872233480051767533315579421297748",
                "0.9804848617734693435620406218501293693408",
                "0.9806353595296081423606168993845296964976",
                "0.9807852804032304491261822361342390369739",
                "0.9809346243061416765338349527840128514497",
                "0.9810833911504866542626750579645935384594",
                "0.9812315808487496806787374089213093342750",
                "0.9813791933137545743182241898789480320709",
                "0.9815262284586647251710064218436750660416",
                "0.9816726861969831457643643350793483555106",
                "0.9818185664425525220469365530099126592191",
                "0.9819638691095552640728481538315649202652",
                "0.9821085941125135564859877936680000465280",
                "0.9822527413662894088044041926692046184358",
                "0.9823963107860847055047924030388949839064",
                "0.9825393022874412559070403955777269171829",
                "0.9826817157862408438588066189487673018659",
                "0.9828235511987052772200993035083449562703",
                "0.9829648084413964371478283991992185487037",
                "0.9831054874312163271803011546739453386706",
                "0.9832455880854071221216324625043360045649",
                "0.9833851103215512167260412130378688462975",
                "0.9835240540575712741820040171838419397166",
                "0.9836624192117302743962377761507951158350",
                "0.9838002057026315620774826939122656791261",
                "0.9839374134492188946200574459501832929198",
                "0.9840740423707764897871583356140911638724",
                "0.9842100923869290731938743872398332594066",
                "0.9843455634176419255898904429933014900372",
                "0.9844804553832209299418504482432232709665",
                "0.9846147682043126183153532281217193229878",
                "0.9847485018019042185565531758024046506444",
                "0.9848816560973237007733383909130730052720",
                "0.9850142310122398236160589244034274519502",
                "0.9851462264686621803577779041078275529689",
                "0.9852776423889412447740184331785477871601",
                "0.9854084786957684168219792715165127659340",
                "0.9855387353121760681191924282938231952658",
                "0.9856684121615375872215959116455429710743",
                "0.9857975091675674247009949996071128769398",
                "0.9859260262543211380218855143883206622402",
                "0.9860539633461954362176127001049213905960",
                "0.9861813203679282243658394221346964289130",
                "0.9863080972445986478632975243258948530479",
                "0.9864342939016271364997962983625479248678",
                "0.9865599102647754483314621376830161858327",
                "0.9866849462601467133531835664552501438030",
                "0.9868094018141854769702359522345500231768",
                "0.9869332768536777432690603290670281153979",
                "0.9870565713057510180871708759544394101328",
                "0.9871792850978743518821657137634829009535",
                "0.9873014181578583823998158018450177283204",
                "0.9874229704138553771412068338258156234087",
                "0.9875439417943592756289091502474144165113",
                "0.9876643322282057314721508039532771323355",
                "0.9877841416445721542309690323667278618122",
                "0.9879033699729777510793155090579596177513",
                "0.9880220171432835682670908652687211829675",
                "0.9881400830856925323810840903480199689726",
                "0.9882575677307494914047925383512565371365",
                "0.9883744710093412555770983863685641000559",
                "0.9884907928526966380497775084756934180352",
                "0.9886065331923864953438168475424904250183",
                "0.9887216919603237676045164854897910426446",
                "0.9888362690887635186553527309553353446704",
                "0.9889502645103029758505786617130118829158",
                "0.9890636781578815697265386775873129380813",
                "0.9891765099647809734516737380162430639837",
                "0.9892887598646251420751940768410068936362",
                "0.9894004277913803515743963053395381037211",
                "0.9895115136793552377006019329732500159081",
                "0.9896220174632008346236944537822198667250",
                "0.9897319390779106133752322648432936118774",
                "0.9898412784588205200901148016982465477492",
                "0.9899500355416090140467793941650873261145",
                "0.9900582102622971055059064644647793938596",
                "0.9901658025572483933476108081280047859693",
                "0.9902728123631691025070968166920408076395",
                "0.9903792396171081212087556197562907229147",
                "0.9904850842564570379986822425364353778167",
                "0.9905903462189501785755909936414839730132",
                "0.9906950254426646424201074163951292045822",
                "0.9907991218660203392224152556326849437213",
                "0.9909026354277800251082370105274337521976",
                "0.9910055660670493386631267626353670473505",
                "0.9911079137232768367550540869949928485321",
                "0.9912096783362540301552579727790449586259",
                "0.9913108598461154189573497986674833550035",
                "0.9914114581933385277946445267960586719580",
                "0.9915114733187439408556993978318541320317",
                "0.9916109051634953366980395284365463002633",
                "0.9917097536690995228600499310995717595784",
                "0.9918080187774064702710135950568803965734",
                "0.9919057004306093474592753857564275969270",
                "0.9920027985712445545585116390889374237376",
                "0.9920993131421917571120854453716869282951",
                "0.9921952440866739196754677368540482523056",
                "0.9922905913482573392167044113062102475852",
                "0.9923853548708516783149098430568150810573",
                "0.9924795345987099981567672516611178200108",
                "0.9925731304764287913310165172086384086777",
                "0.9926661424489480144209101501180558492097",
                "0.9927585704615511203946182421172238782619",
                "0.9928504144598650907935633439675960681743",
                "0.9929416743898604677186663343649661560686",
                "0.9930323501978513856144844633321865853457",
                "0.9931224418304956028512228723143547958354",
                "0.9932119492347945331046010120927827784470",
                "0.9933008723580932765335554985508208683595",
                "0.9933892111480806507557610652522217319190",
                "0.9934769655527892216209513907311350444181",
                "0.9935641355205953337820216973419474904244",
                "0.9936507210002191410638951374769564705921",
                "0.9937367219407246366301351019302192879719",
                "0.9938221382915196829472857041667826255191",
                "0.9939069700023560415469228132477998214356",
                "0.9939912170233294025853981271637158006254",
                "0.9940748793048794142012588973396715232974",
                "0.9941579567977897116703260340994814539259",
                "0.9942404494531879463584134419068988176870",
                "0.9943223572225458144716715522453342734571",
                "0.9944036800576790856045381410496640628791",
                "0.9944844179107476310852796366661836583761",
                "0.9945645707342554521191062433890623879592",
                "0.9946441384810507077288443247037634140799",
                "0.9947231211043257424931496094597417346734",
                "0.9948015185576171140822449032962504982639",
                "0.9948793307948056205911661067562028172052",
                "0.9949565577701163276705004606436823538026",
                "0.9950331994381185954546010583108011676040",
                "0.9951092557537261052872617836990975673833",
                "0.9951847266721968862448369531094799215755",
                "0.9952596121491333414567900578327844539826",
                "0.9953339121404822742236561239402558898320",
                "0.9954076266025349139324023247096093177239",
                "0.9954807554919269417691716003477196872654",
                "0.9955532987656385162293941588653418565742",
                "0.9956252563809942984252518511625189193658",
                "0.9956966282956634771904805325954195538176",
                "0.9957674144676597939824956425161862154313",
                "0.9958376148553415675818263525069050091434",
                "0.9959072294174117185888437532669548823013",
                "0.9959762581129177937177686693596882351022",
                "0.9960447009012519898879448102795669989216",
                "0.9961125577421511781123630855644575308972",
                "0.9961798285956969271834230309497041554896",
                "0.9962465134223155271559174118407846859138",
                "0.9963126121827780126272261896697316108763",
                "0.9963781248382001858147061559970096606690",
                "0.9964430513500426394302626585251039923095",
                "0.9965073916801107793520899625026230722771",
                "0.9965711457905548470935669103181862989689",
                "0.9966343136438699420692946614116783021414",
                "0.9966968952028960436582634139665394855394",
                "0.9967588904308180330641351291905555392910",
                "0.9968202992911657149726293983440371335986",
                "0.9968811217478138390059997120332746034011",
                "0.9969413577649821209745875106536409292054",
                "0.9970010073072352639254415142406294919568",
                "0.9970600703394829789879899493683807050366",
                "0.9971185468269800060167534101238034753232",
                "0.9971764367353261340310862095801632845136",
                "0.9972337400304662214519341975969182741304",
                "0.9972904566786902161355971401825678211717",
                "0.9973465866466331752044838750742644603927",
                "0.9974021299012752846748485776118593937846",
                "0.9974570864099418788814965904148339739436",
                "0.9975114561403034596994483898081441976324",
                "0.9975652390603757155625503813873021368900",
                "0.9976184351385195402790213365639671015344",
                "0.9976710443434410516439234013908498989182",
                "0.9977230666441916098485467284287755605433",
                "0.9977745020101678356866969018892330630071",
                "0.9978253504111116285578744457625946013280",
                "0.9978756118171101842673358241253415908282",
                "0.9979252861985960126230254623090194905749",
                "0.9979743735263469548293684371091884643732",
                "0.9980228737714862006779136037142715283101",
                "0.9980707869054823055348170465418558771414",
                "0.9981181129001492071251558606836062321141",
                "0.9981648517276462421140623901794310033547",
                "0.9982110033604781624846691688668325004978",
                "0.9982565677714951517128549290824010421249",
                "0.9983015449338928407387821630291092908159",
                "0.9983459348212123237352168411653571558166",
                "0.9983897374073401736726210115195388358485",
                "0.9984329526665084576810091233871816935279",
                "0.9984755805732947522085590384263713301082",
                "0.9985176211026221579769688117311581315254",
                "0.9985590742297593147335504450318673476075",
                "0.9985999399303204158000519337456371063086",
                "0.9986402181802652224181990491800163114017",
                "0.9986799089558990778919484167769967796747",
                "0.9987190122338729215264435708743608938133",
                "0.9987575279911833023636657860556271283929",
                "0.9987954562051723927147716047591006944432",
                "0.9988327968535280014891091004205148857423",
                "0.9988695499142835873199050350324101374086",
                "0.9989057153658182714866151896166719617929",
                "0.9989412931868568506339302657244654427194",
                "0.9989762833564698092874298757000924787143",
                "0.9990106858540733321658772590719880982634",
                "0.9990445006594293162901474820650935520820",
                "0.9990777277526453828887819968641261429899",
                "0.9991103671141748891001625568967385049231",
                "0.9991424187248169394712976040491529080554",
                "0.9991738825657163972532143633744987637056",
                "0.9992047586183638954929500005057034450442",
                "0.9992350468645958479221353166403174396237",
                "0.9992647472865944596421645756240243171643",
                "0.9992938598668877376059451773227236383222",
                "0.9993223845883495008962210111399103525911",
                "0.9993503214341993908004634432065370391453",
                "0.9993776703880028806823240104445651342497",
                "0.9994044314336712856496430143829176617227",
                "0.9994306045554617720190083272854685356212",
                "0.9994561897379773665768588428349718316432",
                "0.9994811869661669656371271233043781185278",
                "0.9995055962253253438954159148377335938779",
                "0.9995294175010931630797033221567409693598",
                "0.9995526507794569803975715537060083853110",
                "0.9995752960467492567799542679499536897910",
                "0.9995973532896483649213976712371957756378",
                "0.9996188224951785971168306373539819130463",
                "0.9996397036507101728948392385966997247626",
                "0.9996599967439592464474411979047342047763",
                "0.9996797017629879138563558913087835563560",
                "0.9996988186962042201157656496661721968501",
                "0.9997173475323621659515642283736246187653",
                "0.9997352882605617144370884334693194816886",
                "0.9997526408702487974053290122597589086391",
                "0.9997694053512153216576170363329930430512",
                "0.9997855816935991749687821245479640570773",
                "0.9998011698878842318887789733201065552173",
                "0.9998161699249003593407787812557922551560",
                "0.9998305817958234220157222749226655145859",
                "0.9998444054921752875633311622793122720258",
                "0.9998576410058238315795749600259678433745",
                "0.9998702883289829423905902608780283252256",
                "0.9998823474542125256330496265059156608293",
                "0.9998938183744185086309774116282872840055",
                "0.9999047010828528445690099444906072283126",
                "0.9999149955731135164620976087076362304435",
                "0.9999247018391445409216464911963832243506",
                "0.9999338198752359717180973806754192713391",
                "0.9999423496760239031399400209571169558493",
                "0.9999502912364904731491606430112731506515",
                "0.9999576445519638663331209195316303717369",
                "0.9999644096181183166528666054909612546738",
                "0.9999705864309741099878642479255515423298",
                "0.9999761749868975864771644679460389297469",
                "0.9999811752826011426569904377285677161739",
                "0.9999855873151432333947502949980320168265",
                "0.9999894110819283736194723572737328374494",
                "0.9999926465807071398486621179069961623047",
                "0.9999952938095761715115801257001198995530",
                "0.9999973527669781720689399696563666155140",
                "0.9999988234517019099290257101715260190483",
                "0.9999997058628822191602282177387656771163",
                1
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 2048) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.SinPiHalf(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(expected, y, 8e-32, $"{x}");
            }
        }

        [TestMethod]
        public void AtanExpectedTest() {
            ddouble[] expecteds = {
                "0",
                "0.003906230131966971827628665311424387140357",
                "0.007812341060101111296463391842199281621223",
                "0.01171821360239412863618316229152908705745",
                "0.01562372862047683080280152125657031891111",
                "0.01952876704141370724753787341046141184236",
                "0.02343320987946758445107229863741947545394",
                "0.02733693825782441190743189833607194312597",
                "0.03123983343026827625371174489249097703250",
                "0.03514177680279678266925872642407145125046",
                "0.03904264995516699345275445132693162747221",
                "0.04294233466236217275837618398091923343025",
                "0.04684071291596965375222376000185734486705",
                "0.05073766694546021984574714406291167925745",
                "0.05463307923935947507138879374732662585599",
                "0.05852683256630176997379022550417838224049",
                "0.06241880999595734847397911298550511360627",
                "0.06630889491982348789002609340337138255113",
                "0.07019697107187051852187630470349658568048",
                "0.07408292254903373077690284326396102238771",
                "0.07796663383154230656332864878102746090632",
                "0.08184798980307654743911654486417946087087",
                "0.08572687577074481459363165573197294311644",
                "0.08960317748487174496938422265339464981976",
                "0.09347678115858946350452719331206047882316",
                "0.09734757348722367338896828792992000890516",
                "0.1012154416674666741662006965791130933352",
                "0.1050802734163295312606859592204016320109",
                "0.1089419569898657998418608611626007547402",
                "0.1128003812016593906196311931548322366358",
                "0.1166554354410693549660528495092536213281",
                "0.1205070096912245614321661869604250914029",
                "0.1243549945467614350313548491638710255732",
                "0.1281992812312981343406366401487441634099",
                "0.1320397616146387492746844065265695273028",
                "0.1358763282297013140565290035557399655190",
                "0.1397088742891636451833677767390950657688",
                "0.1435372937018212328028454120253374836894",
                "0.1473614810886516356098027603968455300911",
                "0.1511813317985800538795306620047925805779",
                "0.1549967419239409823037143749334921910556",
                "0.1588076083156310736216211412495308911328",
                "0.1626138285979485753736415637615578020999",
                "0.1664153011831149351793188330008522128921",
                "0.1702119252854744044904966070997617094785",
                "0.1740036009353677065263544909191767769416",
                "0.1777902289926760707966247992158246962377",
                "0.1815717111600321739999264179127003307741",
                "0.1853479499956947648860259612285446445153",
                "0.1891188489260839886368298551169047995706",
                "0.1928843122579746641970587106902272935472",
                "0.1966442451903450055214056097745109154248",
                "0.2003985538258785146539457850343783805550",
                "0.2041471451821170106752632263240606531620",
                "0.2078899272022629936053349831029943302823",
                "0.2116268087656297761026504298655588365874",
                "0.2153576996977380480244596271664896504990",
                "0.2190825107800577693869771827013752101651",
                "0.2228011537593945157710321221404325470735",
                "0.2265135413569196265471977704743161470936",
                "0.2302195872768437302401709596798029893211",
                "0.2339192062147334427167919233326140721708",
                "0.2376123138654712524738836343256377747062",
                "0.2412988269308588229360892277580776203881",
                "0.2449786631268641541720824812112758109141",
                "0.2486517411905132556289916349649685377411",
                "0.2523179808864271872059075075476730799702",
                "0.2559773030130055280815819531263685250424",
                "0.2596296294082575310299464431839718889130",
                "0.2632748829552824143585086477820948933589",
                "0.2669129875874004339552348564915996733623",
                "0.2705438682929365641030717758984053588728",
                "0.2741674511196587975993718983421758015542",
                "0.2777836631788732531894506483442895038135",
                "0.2813924326491784512863373252052941836192",
                "0.2849936887798812873072243051277165592404",
                "0.2885873618940773956236114199582183226535",
                "0.2921733833913987560167951968726367411638",
                "0.2957516857504315485808979994626896776271",
                "0.2993222025308074121573644515551242327077",
                "0.3028848683749714055605560945055582132915",
                "0.3064396190096301100139399123201573764816",
                "0.3099863912468834453178920488852007919329",
                "0.3135251229850439012780280901117115921113",
                "0.3170557532091470098090155766744673154223",
                "0.3205782219911570018717726029489116950657",
                "0.3240924704898717069873818070036072212290",
                "0.3275984409505308614912040439765353192600",
                "0.3310960767041320949443387877569445454686",
                "0.3345853221664589622118924171278064328685",
                "0.3380661228368254816598223422680471566460",
                "0.3415384252965417277317893663729817616564",
                "0.3450021772071051088676812869000516900114",
                "0.3484573273081220393451342288521158295214",
                "0.3519038254149647861988092323316355118600",
                "0.3553416224161683399389883226315691738534",
                "0.3587706702705722203959200639264604997770",
                "0.3621909220042121867088266710704666583017",
                "0.3656023317069668733128101132586805962551",
                "0.3690048545289644218117537619741308586922",
                "0.3723984466767542219236550382837018324887",
                "0.3757830654092489133136356474707120104766",
                "0.3791586690334418341599883683469451773482",
                "0.3825252168999051318044884559523289405922",
                "0.3858826693980737758976954846072314117540",
                "0.3892309879513207351441770104499275087527",
                "0.3925701350118285951655766597158163785588",
                "0.3959000740552629072180958561072232799966",
                "0.3992207695752525656147166961588647858871",
                "0.4025321870776825158032032803259728288128",
                "0.4058342930748040952326027825138523405910",
                "0.4091270550791683054996333411334889838630",
                "0.4124104415973873068997912896671269370468",
                "0.4156844221237294155156318130122540489800",
                "0.4189489671335528684572821440930714724075",
                "0.4222040480765836049298690477153089839873",
                "0.4254496373700422895422636051807923498355",
                "0.4286857083916257797953184816987778679690",
                "0.4319122354723482121002482623679622487467",
                "0.4351291938892468500841679534844144634260",
                "0.4383365598579578054456160492147713300291",
                "0.4415343105251667053339193697444980856883",
                "0.4447224239609393412483431930515242815744",
                "0.4479008791509372928918647669485635189391",
                "0.4510696559885234763756392572821934177106",
                "0.4542287352667625197589621479305591729010",
                "0.4573780986703208202304474687766769716587",
                "0.4605177287672710863932970462960607207189",
                "0.4636476090008061162142562314612144020285",
                "0.4667677236808665063346099251426359649405",
                "0.4698780579756869317238837674289664081621",
                "0.4729785979032655761832312720130461133865",
                "0.4760693303227612340751004202614715119088",
                "0.4791502429258225419667220630355108222681",
                "0.4822213242278537357239469689630996105611",
                "0.4852825635592212640742566088670985635541",
                "0.4883339510564055238671649607470648373584",
                "0.4913754776531019152889257501002322926417",
                "0.4944071350712753472270372523319383605722",
                "0.4974289158121722539173742536110067127379",
                "0.5004408131472941140300005149792245065381",
                "0.5034428211093363925441476327936602337980",
                "0.5064349344830967542119594467474585608039",
                "0.5094171487963563251959744277409747631301",
                "0.5123894603107377066666010205842592544212",
                "0.5153518660125433708407187982195112412048",
                "0.5183043636035779962067055995151580099999",
                "0.5212469514919582245883250019548067267644",
                "0.5241796287829132483216496175045301573782",
                "0.5271023952695795612251297573119216216759",
                "0.5300152514237931323005627690087055918848",
                "0.5329181983868821862775179633192198840775",
                "0.5358112379604637002690850687076914070989",
                "0.5386943725972466510039148974642466203683",
                "0.5415676053918449723975989999416057929879",
                "0.5444309400716031086825990730769215588418",
                "0.5472843809874369739852207703127568758090",
                "0.5501279331046930551734990823658377065369",
                "0.5529616019940283210522390754781720159728",
                "0.5557853938223135275997130995671668741461",
                "0.5585993153435624359715082164016612703464",
                "0.5614033738898893874855972097983260121200",
                "0.5641975773624976077917430252672946629224",
                "0.5669819342227005409587511404179037954205",
                "0.5697564534829784433238348916655609510164",
                "0.5725211446980723966765260267316798238160",
                "0.5752760179561178307303390604991627189109",
                "0.5780210838698195759021270589513997742864",
                "0.5807563535676703992032744750015008620986",
                "0.5834818386852149085783008273144323162244",
                "0.5861975513563606443330871822473274046393",
                "0.5889035042047381104030586751240156526948",
                "0.5915997103351114331458526589590323175429",
                "0.5942861833248412711262207563990554236516",
                "0.5969629372154015360144938318679089098453",
                "0.5996299865039514222636516828564008092694",
                "0.6022873461349641816821226942042329099995",
                "0.6049350314919140183966388969744620743264",
                "0.6075730583890224200170815974795400481267",
                "0.6102014430630651820871509675178297209907",
                "0.6128202021652413251433846354956891657256",
                "0.6154293527531050469217061540627728568485",
                "0.6180289122825617964551808283642598385877",
                "0.6206188985999295020076161695170168216470",
                "0.6231993299340659309924753490603745950874",
                "0.6257702248885631072415082255529805456997",
                "0.6283316024340096592176448127738759960601",
                "0.6308834819003219220160441936770348306501",
                "0.6334258829691445662686954830592997672811",
                "0.6359588256663214783635568873842285169798",
                "0.6384823303544375687098458370248680148784",
                "0.6409964177254321381267685531137761829210",
                "0.6435011087932843868028092287173226380415",
                "0.6459964248867716056649293813883652506305",
                "0.6484823876423005464090698119162580842323",
                "0.6509590189968124238718208417639584588625",
                "0.6534263411807619628638934113116296719752",
                "0.6558843767111708610342239012183110274034",
                "0.6583331483847559997836305857185611307126",
                "0.6607726792711326966927061982515851196484",
                "0.6632029927060932553632543102382758732633",
                "0.6656241122849610319886429029299603188076",
                "0.6680360618560202023579847712093188934589",
                "0.6704388655140213783535625858059100689745",
                "0.6728325475937631893114013292615277361144",
                "0.6752171326637499108718900745721984405281",
                "0.6775926455199251921409679777758410904743",
                "0.6799591111794819011023047982900000015663",
                "0.6823165548747480782564299817111529931429",
                "0.6846650020471489594028454128556358493588",
                "0.6870044783412450003144135183116685785917",
                "0.6893350095988458087680566710138536846492",
                "0.6916566218531998629800663181103529683531",
                "0.6939693413232598699358731861827298287251",
                "0.6962731944080235923905100965443800214206",
                "0.6985682076809499494345296402629417634284",
                "0.7008544078844501724579512817867512680901",
                "0.7031318219244537760888657487000538044404",
                "0.7054004768650490822204263019559126349162",
                "0.7076603999231980145567813502053069830927",
                "0.7099116184635248611916111509362458563820",
                "0.7121541599931786835687865912615918621031",
                "0.7143880521567690317496541110864489285949",
                "0.7166133227313746082118881845094079714486",
                "0.7188299996216245054170141515259046539514",
                "0.7210381108548516260938296152692867128631",
                "0.7232376845763178795792637634429094912168",
                "0.7254287490445107326229278945065020673688",
                "0.7276113326265106787829526909499757625740",
                "0.7297854637934291769049249618444641766731",
                "0.7319511711159165961691081923069698302312",
                "0.7341084832597396927999810897813561563778",
                "0.7362574289814281317428352710891466628320",
                "0.7383980371239895554111814479755606444941",
                "0.7405303366126926909825420855291840454921",
                "0.7426543564509179776554608249000207477314",
                "0.7447701257160751857639310909741182291100",
                "0.7468776735555874906637353378326703200015",
                "0.7489770291829414558452980730009161352346",
                "0.7510682218738023717766117770551174557866",
                "0.7531512809621943895247393702690288816001",
                "0.7552262358367448812326032083852174468922",
                "0.7572931159369924530266468114325073269043",
                "0.7593519507497580298880500838021747314358",
                "0.7614027698055784264231855420876226364336",
                "0.7634456026752018123057660317131236856017",
                "0.7654804789661444764216520449312483128885",
                "0.7675074283193082894157159394099851523699",
                "0.7695264804056582604068200359856540188685",
                "0.7715376649229595800903400220773541836991",
                "0.7735410115925735392764068852829523393121",
                "0.7755365501563117091049732955865416609616",
                "0.7775243103733477667249308161224373249978",
                "0.7795043220171863481129848258979314140518",
                "0.7814766148726883079281840634961382904955",
                "0.7834412187331517648394294758877039800823",
                "0.7853981633974483096156608458198757210493"
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 256) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Atan(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(expected, y, 8e-32, $"{x}");
            }
        }

        [TestMethod]
        public void AsinExpectedTest() {
            ddouble[] expecteds = {
                "0",
                "0.003906259934175675287287668546931409343529",
                "0.007812579475042567110027142257578671844849",
                "0.01171901823747838526721356173831077499337",
                "0.01562563585273694956017838338823830597324",
                "0.01953249197664405581795959191881576995973",
                "0.02343964629780272169562124638067709757702",
                "0.02734715854581094975168814230709350458265",
                "0.03125508849949515468409146989332854974298",
                "0.03516349599516241334229582905825122224303",
                "0.03907244093487471025114699808651452963653",
                "0.04298198329474836789696277854686207802572",
                "0.04689218313328186995900705965465033194107",
                "0.05080310059971530704330076815496365618617",
                "0.05471479594242469831737706967748560858542",
                "0.05862732951735446878385991103529517223892",
                "0.06254076179649139080060163689638076004133",
                "0.06645515337638332989177472160773896164044",
                "0.07037056498670616893731129318471204449146",
                "0.07428705749888232152035221551815598319060",
                "0.07820469193475428460030981145625920120544",
                "0.08212352947531672281273234336217306226832",
                "0.08604363146951062163000949425738080336166",
                "0.08996505944308309440646210350301783850324",
                "0.09388787510751647903877268153164573263307",
                "0.09781214036903041366328081764646783416252",
                "0.1017379173376606375547495999321221482605",
                "0.1056652683364183232604156718348023340398",
                "0.1095942559105338090764705474794992553781",
                "0.1135249428367886673341354769732330340681",
                "0.1174573921329401136964479312300439257475",
                "0.1213916670672418358669196703011410194516",
                "0.1253278311680653968745669863570847180481",
                "0.1292659482336264485289285853577547977134",
                "0.1332060823418200748415211878736808301212",
                "0.1371482978601696733003722535570196685350",
                "0.1410926594558938739813702016415589977553",
                "0.1450392321060960927099102238465111709482",
                "0.1489880811080814149808421744700172419865",
                "0.1529392720898056122429045815432122094204",
                "0.1568928710204612016015104570950593770612",
                "0.1608489442212055741440962651470743162644",
                "0.1648075583760363361060466359864792352408",
                "0.1687687805428191311412324788042769038273",
                "0.1727326781644733412165591957409295628279",
                "0.1766993190803211983004528125019462416136",
                "0.1806687715376059792558892177354315027495",
                "0.1846411042031851023839373256374437208433",
                "0.1886163861754040961084070452519641483367",
                "0.1925946869961575685711405727313305153454",
                "0.1965760766631434716568512449931370272023",
                "0.2005606256423171244338458822445346687588",
                "0.2045484048805516394422405887861309359922",
                "0.2085394858185115809569055219609536901488",
                "0.2125339404037468775842324367131522285664",
                "0.2165318411040142126198398395290504229358",
                "0.2205332609208333248132091610509754136902",
                "0.2245382734032858698851992688113716158431",
                "0.2285469526620647196719593580684131786389",
                "0.2325593733837818114876710296273931436883",
                "0.2365756108455429055905946977130702849170",
                "0.2405957409297978639028991809727182775696",
                "0.2446198401394753287955835604994005875088",
                "0.2486479856134109572474372856924278506332",
                "0.2526802551420786534856574369937109722522",
                "0.2567167271836345428031345590415307510414",
                "0.2607574808802837411359464702811864924225",
                "0.2648025960749802997127674346684127541568",
                "0.2688521533284710422217405403401486672708",
                "0.2729062339366943640749626695481852502371",
                "0.2769649199485454301118615187576436032549",
                "0.2810282941840195891285727803756350728403",
                "0.2850964402527462216433569990763495563917",
                "0.2891694425729256520367147352724854879815",
                "0.2932473863906821884060009370167145148049",
                "0.2973303577998468039552918394693619534677",
                "0.3014184437621834433521004786003275544437",
                "0.3055117321280734271186353451051687848601",
                "0.3096103116576729377299539724656328069290",
                "0.3137142720425591036586451485028100978812",
                "0.3178237039278807531834174706363592781162",
                "0.3219386989350304894719937771888834161740",
                "0.3260593496848553434222222690349842870236",
                "0.3301857498214238922285682127304410358923",
                "0.3343179940363683909313256344427958051845",
                "0.3384561780938211526722438106151267604230",
                "0.3426003988559651324685612103763941172772",
                "0.3467507543092194205546763935700792078808",
                "0.3509073435910811363401433267225790340648",
                "0.3550702670176460344993237179376363833087",
                "0.3592396261118309924442825344725439681049",
                "0.3634155236323224453444234220928953489891",
                "0.3675980636032757729602077073407663418409",
                "0.3717873513447916239876621246550289883076",
                "0.3759834935041961906237009590712541541444",
                "0.3801865980881535210509854797284254561932",
                "0.3843967744956390830381948729670469737528",
                "0.3886141335518049705412388452242971100718",
                "0.3928387875427683799181144061647540776198",
                "0.3970708502513562761514882350336829996766",
                "0.4013104369938405255085442187467771759545",
                "0.4055576646576991927525828690893478139004",
                "0.4098126517404411919595151488832510409171",
                "0.4140755183895330440120545375835619335388",
                "0.4183463864434681350107935985271370296060",
                "0.4226253794740205924750948767325269558976",
                "0.4269126228297277049011606185010618512845",
                "0.4312082436806467098849224640298795946117",
                "0.4355123710644337718010939276084429913917",
                "0.4398251359337950674890868931658404902851",
                "0.4441466712053621034223479421508224257158",
                "0.4484771118100457067053189518835410528421",
                "0.4528165947449255716393313619497002902231",
                "0.4571652591267348106554716709092778402586",
                "0.4615232462470016607340484619199142390918",
                "0.4658906996289133421322785870964870483999",
                "0.4702677650859700639881040874239981128468",
                "0.4746545907825003304121762040331537375699",
                "0.4790513272961120309097846717303552372763",
                "0.4834581276821573109602975194473982080632",
                "0.4878751475402929236290300545964123867789",
                "0.4923025450832216732928933769784896588531",
                "0.4967404812077046908779415631728737828267",
                "0.5011891195679386403061185432673112450497",
                "0.5056486266513965629958776023838748082943",
                "0.5101191718572359371956361634625671317083",
                "0.5146009275773826787481393752322969386228",
                "0.5190940692804052579386887969738400670870",
                "0.5235987755982988730771072305465838140329",
                "0.5281152284163057265753002535816550796926",
                "0.5326436129659039162707583429625473541829",
                "0.5371841179211043080938366298639872371870",
                "0.5417369354982020222317281648121468178415",
                "0.5463022615591368720777895330816356560738",
                "0.5508802957186252740421615758829000764602",
                "0.5554712414552348296968027952282068676894",
                "0.5600753062265820052886919436649075530898",
                "0.5646927015888431357555690521386392898849",
                "0.5693236433207794024683959890163246195940",
                "0.5739683515524875207998577518488933508312",
                "0.5786270508990996737229397897902127051085",
                "0.5832999705996687934014237842436563124132",
                "0.5879873446614886809122014508628154277367",
                "0.5926894120101127263496315401727560050983",
                "0.5974064166453502143038103662842486439771",
                "0.6021386078035354454513758290237644231202",
                "0.6068862401263822523362109417050545528395",
                "0.6116495738367550217429734770915306489598",
                "0.6164288749217071502392547294666934718650",
                "0.6212244153231590545365466167910073108153",
                "0.6260364731366105443504114014911187940101",
                "0.6308653328183066623750211921554148420040",
                "0.6357112854013021346621194158193140341748",
                "0.6405746287208974979343699862542359686473",
                "0.6454556676499499342082762754589916291367",
                "0.6503547143445940181726448491939968307176",
                "0.6552720885009421557857086059206296827962",
                "0.6602081176233716680346722458117859887293",
                "0.6651631373050454759526589677403508616524",
                "0.6701374915213564178231776271982280710522",
                "0.6751315329370316472090562652943880142042",
                "0.6801456232276836180361606084956045914553",
                "0.6851801334166481872589332878320222600421",
                "0.6902354442280087165935540124511137465495",
                "0.6953119464567681292802645047117809539990",
                "0.7004100413571991138054505987348144413568",
                "0.7055301410504765478350423987966384452036",
                "0.7106726689527762773958636346893517182653",
                "0.7158380602251112210241587816759142230088",
                "0.7210267622462700327986505159522530462326",
                "0.7262392351103259804653969991082819774620",
                "0.7314759521502950845947946216384676974878",
                "0.7367374004896438220276848151752473255485",
                "0.7420240816234788240868172523402631749228",
                "0.7473365120313951146233490856735816406844",
                "0.7526752238241167823590148563881016705984",
                "0.7580407654262359604569723109811329167566",
                "0.7634337022975441543007049992557271306610",
                "0.7688546176956560649573573184547442929338",
                "0.7743041134828520634673429801785282255997",
                "0.7797828109803135857690031347124698385213",
                "0.7852913518731984224044255546879811074837",
                "0.7908303991703029695768322120810368675039",
                "0.7964006382223891477883290213712650150263",
                "0.8020027778036184531549319457056574945930",
                "0.8076375512609385298310155378509910258339",
                "0.8133057177367133286023428129112218621252",
                "0.8190080634703815608985966590712436558139",
                "0.8247454031854757045400850330793470212866",
                "0.8305185815689420356316178048577202337355",
                "0.8363284748503787822656646940658545879012",
                "0.8421759924895633702092882087403873966885",
                "0.8480620789814810080529443389984180800734",
                "0.8539877157890072025045114817201492032536",
                "0.8599539234144496337544123250935902988793",
                "0.8659617636223356539519390281100206185340",
                "0.8720123418271584219821376452040330178147",
                "0.8781068096612881299181353521501984416559",
                "0.8842463677399390476388086580355355179643",
                "0.8904322686419863226256207033838837041136",
                "0.8966658201275814416891785608655790567493",
                "0.9029483886159604070685542269384289682943",
                "0.9092814029496190888681766656962885011799",
                "0.9156663584741989523426979173727540564675",
                "0.9221048214670460493586432071665818496032",
                "0.9285984339515509418161636893399134743588",
                "0.9351489189391351128644867503670328858663",
                "0.9417580861462252636001957423855189929895",
                "0.9484278382398759813267719803373678645611",
                "0.9551601776730138419781806914155494647410",
                "0.9619572141787628727999595440031742221080",
                "0.9688211730031908272684204593254387989506",
                "0.9757544039673526894278496539912440053381",
                "0.9827593914630245545024557902314415867002",
                "0.9898387655024115484394657474813429857417",
                "0.9969953139608615183504528352747170154302",
                "1.004231996173818509979557825486239442130",
                "1.011551958075645954999062487240485901016",
                "1.018958549099459895734510824032683225627",
                "1.026455341094890352696902868827528229593",
                "1.034046149566185327872463169019098172937",
                "1.041735057588124269808027016408919391535",
                "1.049526442824156349771683682838258585679",
                "1.057425008153023431465778099389135638896",
                "1.065435816510739312260006817652329497594",
                "1.073564330679206728406920698379868802064",
                "1.081816458907549374390992966955874006649",
                "1.090198607446112228910660976800616721687",
                "1.098717741317611801802078311712587516170",
                "1.107381454960638799701035437033370715058",
                "1.116198054778698296835474587396338841585",
                "1.125176656142073790682789535440877958183",
                "1.134327298059977363645134328532349782111",
                "1.143661079622671311520821931960013533998",
                "1.153190323486917249114584618901140921258",
                "1.162928773257394266976827256211791869094",
                "1.172891833768206223433732214874891546307",
                "1.183096866239209065870611707362459846418",
                "1.193563554444088427014507601135823145425",
                "1.204314363953405934600652863909734519948",
                "1.215375125104673126492867008366670870489",
                "1.226775783045638695916874656883352842591",
                "1.238551377372834293236525371277895113086",
                "1.250743343572335366255545804360328692847",
                "1.263401275710393267428287184968554861261",
                "1.276585367403010439449050200857982811660",
                "1.290369880193318497512365857282532013926",
                "1.304848223052124963688436298483528135943",
                "1.320140664458765825482187718925582006002",
                "1.336406565293874847253698275775642558513",
                "1.353864866255387759225795408567242393221",
                "1.372830867595242664063041324323526078432",
                "1.393788640506865692236158402655266009622",
                "1.417553317298780423609800889337716538834",
                "1.445714803201913837630118417846989922016",
                "1.482379181580447192616146065818376562797",
                "1.570796326794896619231321691639751442099"
            };

            for ((int i, ddouble x) = (0, 0); i < expecteds.Length; i++, x += 1d / 256) {
                ddouble expected = expecteds[i];

                ddouble y = ddouble.Asin(x);

                Console.WriteLine(x);
                Console.WriteLine(y);

                PrecisionAssert.AlmostEqual(expected, y, 2e-31, $"{x}");
            }
        }
    }
}
