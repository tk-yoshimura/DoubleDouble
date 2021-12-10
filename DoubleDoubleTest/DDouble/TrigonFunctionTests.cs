using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class TrigonFunctionTests {

        [TestMethod]
        public void SinPIHalfTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (d == 0) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPIHalf(v);

                Assert.AreEqual(Math.Sin((double)d * Math.PI / 2), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            for (int n = -8; n <= 8; n++) {
                ddouble nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPIHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    Assert.AreEqual(Math.Sin(n * Math.PI / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitDecrement(nearn);
                }

                nearn = n;

                for (int i = 0; i < 64; i++) {
                    ddouble u = ddouble.SinPIHalf(nearn);

                    Console.WriteLine($"{nearn} {nearn.Hi} {nearn.Lo}");
                    Console.WriteLine($"{u} {u.Hi} {u.Lo}");

                    Assert.AreEqual(Math.Sin(n * Math.PI / 2), (double)u, 1e-12, n.ToString());

                    nearn = ddouble.BitIncrement(nearn);
                }
            }

            Assert.IsTrue(ddouble.Abs((ddouble)"6.13588464915447535964023459037258092e-3" - ddouble.SinPIHalf(ddouble.Rcp(256))) < 1e-34);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.56434465040230869010105319467166892e-1" - ddouble.SinPIHalf(ddouble.Rcp(10))) < 1e-32);
            Assert.IsTrue(ddouble.Abs((ddouble)"9.87688340595137726190040247693437261e-1" - ddouble.SinPIHalf(9 * ddouble.Rcp(10))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(0.5d - ddouble.SinPIHalf(ddouble.Rcp(3))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(ddouble.Sqrt(3) / 2 - ddouble.SinPIHalf(2 * ddouble.Rcp(3))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(ddouble.Sqrt(2) / 2 - ddouble.SinPIHalf(0.5d)) < 1e-31);
            Assert.IsTrue(ddouble.Abs(ddouble.Sqrt(2) / 2 - ddouble.SinPIHalf(ddouble.BitDecrement(0.5d))) < 1e-31);
            Assert.IsTrue(ddouble.Abs(ddouble.Sqrt(2) / 2 - ddouble.SinPIHalf(ddouble.BitIncrement(0.5d))) < 1e-31);

            ddouble sin_pzero = ddouble.SinPIHalf(0d);
            ddouble sin_mzero = ddouble.SinPIHalf(-0d);
            ddouble sin_pinf = ddouble.SinPIHalf(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPIHalf(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPIHalf(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void SinPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.SinPI(v);

                Assert.AreEqual(Math.Sin((double)d * Math.PI), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.SinPI(0d);
            ddouble sin_mzero = ddouble.SinPI(-0d);
            ddouble sin_pinf = ddouble.SinPI(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.SinPI(double.NegativeInfinity);
            ddouble sin_nan = ddouble.SinPI(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void CosPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.CosPI(v);

                Assert.AreEqual(Math.Cos((double)d * Math.PI), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.CosPI(0d);
            ddouble cos_mzero = ddouble.CosPI(-0d);
            ddouble cos_pinf = ddouble.CosPI(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.CosPI(double.NegativeInfinity);
            ddouble cos_nan = ddouble.CosPI(double.NaN);

            Assert.IsTrue(cos_pzero == 1, nameof(cos_pzero));
            Assert.IsTrue(cos_mzero == 1, nameof(cos_mzero));
            Assert.IsTrue(ddouble.IsNaN(cos_pinf), nameof(cos_pinf));
            Assert.IsTrue(ddouble.IsNaN(cos_ninf), nameof(cos_ninf));
            Assert.IsTrue(ddouble.IsNaN(cos_nan), nameof(cos_nan));
        }

        [TestMethod]
        public void TanPITest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                if (Math.Abs(d) % 1m == 0.5m) {
                    continue;
                }

                ddouble v = (ddouble)d;
                ddouble u = ddouble.TanPI(v);

                double w = Math.Tan((double)d * Math.PI);

                Assert.AreEqual(w, (double)u, Math.Max(1e-14, Math.Abs(w) * 1e-13), d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.TanPI(0d);
            ddouble tan_mzero = ddouble.TanPI(-0d);
            ddouble tan_pinf = ddouble.TanPI(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.TanPI(double.NegativeInfinity);
            ddouble tan_nan = ddouble.TanPI(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tan_pzero), nameof(tan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tan_mzero), nameof(tan_mzero));
            Assert.IsTrue(ddouble.IsNaN(tan_pinf), nameof(tan_pinf));
            Assert.IsTrue(ddouble.IsNaN(tan_ninf), nameof(tan_ninf));
            Assert.IsTrue(ddouble.IsNaN(tan_nan), nameof(tan_nan));
        }

        [TestMethod]
        public void SinTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sin(v);

                Assert.AreEqual(Math.Sin((double)d), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sin_pzero = ddouble.Sin(0d);
            ddouble sin_mzero = ddouble.Sin(-0d);
            ddouble sin_pinf = ddouble.Sin(double.PositiveInfinity);
            ddouble sin_ninf = ddouble.Sin(double.NegativeInfinity);
            ddouble sin_nan = ddouble.Sin(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sin_pzero), nameof(sin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sin_mzero), nameof(sin_mzero));
            Assert.IsTrue(ddouble.IsNaN(sin_pinf), nameof(sin_pinf));
            Assert.IsTrue(ddouble.IsNaN(sin_ninf), nameof(sin_ninf));
            Assert.IsTrue(ddouble.IsNaN(sin_nan), nameof(sin_nan));
        }

        [TestMethod]
        public void CosTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cos(v);

                Assert.AreEqual(Math.Cos((double)d), (double)u, 1e-14, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cos_pzero = ddouble.Cos(0d);
            ddouble cos_mzero = ddouble.Cos(-0d);
            ddouble cos_pinf = ddouble.Cos(double.PositiveInfinity);
            ddouble cos_ninf = ddouble.Cos(double.NegativeInfinity);
            ddouble cos_nan = ddouble.Cos(double.NaN);

            Assert.IsTrue(cos_pzero == 1, nameof(cos_pzero));
            Assert.IsTrue(cos_mzero == 1, nameof(cos_mzero));
            Assert.IsTrue(ddouble.IsNaN(cos_pinf), nameof(cos_pinf));
            Assert.IsTrue(ddouble.IsNaN(cos_ninf), nameof(cos_ninf));
            Assert.IsTrue(ddouble.IsNaN(cos_nan), nameof(cos_nan));
        }

        [TestMethod]
        public void TanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tan(v);

                double w = Math.Tan((double)d);

                Assert.AreEqual(w, (double)u, Math.Max(1e-14, Math.Abs(w) * 1e-13), d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tan_pzero = ddouble.Tan(0d);
            ddouble tan_mzero = ddouble.Tan(-0d);
            ddouble tan_pinf = ddouble.Tan(double.PositiveInfinity);
            ddouble tan_ninf = ddouble.Tan(double.NegativeInfinity);
            ddouble tan_nan = ddouble.Tan(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tan_pzero), nameof(tan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tan_mzero), nameof(tan_mzero));
            Assert.IsTrue(ddouble.IsNaN(tan_pinf), nameof(tan_pinf));
            Assert.IsTrue(ddouble.IsNaN(tan_ninf), nameof(tan_ninf));
            Assert.IsTrue(ddouble.IsNaN(tan_nan), nameof(tan_nan));
        }

        [TestMethod]
        public void AsinTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Asin(v);

                Assert.AreEqual(Math.Asin((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.PI / 6 - ddouble.Asin(ddouble.Rcp(2))) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.11976951499863418668667705584539962" - ddouble.Asin((ddouble)9 / 10)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.42925685347046940048553233466472443" - ddouble.Asin((ddouble)99 / 100)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.52607123962616318798162545896820037" - ddouble.Asin((ddouble)999 / 1000)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.55665407331738374163508146582209533" - ddouble.Asin((ddouble)9999 / 10000)) < 1e-30);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.56632418711310869205898202533489875" - ddouble.Asin((ddouble)99999 / 100000)) < 1e-30);

            ddouble asin_pzero = ddouble.Asin(0d);
            ddouble asin_mzero = ddouble.Asin(-0d);
            ddouble asin_pinf = ddouble.Asin(double.PositiveInfinity);
            ddouble asin_ninf = ddouble.Asin(double.NegativeInfinity);
            ddouble asin_pout = ddouble.Asin(ddouble.BitIncrement(1));
            ddouble asin_nout = ddouble.Asin(ddouble.BitDecrement(-1));
            ddouble asin_nan = ddouble.Asin(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(asin_pzero), nameof(asin_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(asin_mzero), nameof(asin_mzero));
            Assert.IsTrue(ddouble.IsNaN(asin_pinf), nameof(asin_pinf));
            Assert.IsTrue(ddouble.IsNaN(asin_ninf), nameof(asin_ninf));
            Assert.IsTrue(ddouble.IsNaN(asin_pout), nameof(asin_pout));
            Assert.IsTrue(ddouble.IsNaN(asin_nout), nameof(asin_nout));
            Assert.IsTrue(ddouble.IsNaN(asin_nan), nameof(asin_nan));
        }

        [TestMethod]
        public void AcosTest() {
            for (decimal d = -1m; d <= +1m; d += 0.001m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Acos(v);

                Assert.AreEqual(Math.Acos((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble acos_pinf = ddouble.Acos(double.PositiveInfinity);
            ddouble acos_ninf = ddouble.Acos(double.NegativeInfinity);
            ddouble acos_pout = ddouble.Acos(ddouble.BitIncrement(1));
            ddouble acos_nout = ddouble.Acos(ddouble.BitDecrement(-1));
            ddouble acos_nan = ddouble.Acos(double.NaN);

            Assert.IsTrue(ddouble.IsNaN(acos_pinf), nameof(acos_pinf));
            Assert.IsTrue(ddouble.IsNaN(acos_ninf), nameof(acos_ninf));
            Assert.IsTrue(ddouble.IsNaN(acos_pout), nameof(acos_pout));
            Assert.IsTrue(ddouble.IsNaN(acos_nout), nameof(acos_nout));
            Assert.IsTrue(ddouble.IsNaN(acos_nan), nameof(acos_nan));
        }

        [TestMethod]
        public void AtanTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Atan(v);

                Assert.AreEqual(Math.Atan((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            Assert.IsTrue(ddouble.Abs(ddouble.PI / 4 - ddouble.Atan(1)) < 1e-31);
            Assert.IsTrue(ddouble.Abs(-ddouble.PI / 4 - ddouble.Atan(-1)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.10714871779409050301706546017853704" - ddouble.Atan(2)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"-1.10714871779409050301706546017853704" - ddouble.Atan(-2)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"1.24904577239825442582991707728109012" - ddouble.Atan(3)) < 1e-31);
            Assert.IsTrue(ddouble.Abs((ddouble)"-1.24904577239825442582991707728109012" - ddouble.Atan(-3)) < 1e-31);

            ddouble atan_pzero = ddouble.Atan(0d);
            ddouble atan_mzero = ddouble.Atan(-0d);
            ddouble atan_pinf = ddouble.Atan(double.PositiveInfinity);
            ddouble atan_ninf = ddouble.Atan(double.NegativeInfinity);
            ddouble atan_nan = ddouble.Atan(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(atan_pzero), nameof(atan_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(atan_mzero), nameof(atan_mzero));
            Assert.IsTrue(atan_pinf == ddouble.PI / 2, nameof(atan_pinf));
            Assert.IsTrue(atan_ninf == -ddouble.PI / 2, nameof(atan_ninf));
            Assert.IsTrue(ddouble.IsNaN(atan_nan), nameof(atan_nan));
        }

        [TestMethod]
        public void Atan2Test() {
            for (decimal y = -10m; y <= +10m; y += 0.1m) {
                for (decimal x = -10m; x <= +10m; x += 0.1m) {
                    if (x == 0m && y == 0m) {
                        continue;
                    }

                    ddouble u = ddouble.Atan2((ddouble)y, (ddouble)x);
                    double v = Math.Atan2((double)y, (double)x);

                    if (u == ddouble.PI) {
                        Assert.IsTrue(Math.Abs(v) == Math.PI);
                    }
                    else {
                        Assert.AreEqual(v, (double)u, 1e-15, $"{y}, {x}");
                    }
                    Assert.IsTrue(ddouble.IsRegulared(u));
                }
            }
        }

        [TestMethod]
        public void SinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Sinh(v);

                Assert.AreEqual(Math.Sinh((double)d), (double)u, Math.Abs(Math.Sinh((double)d)) * 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble sinh_pzero = ddouble.Sinh(0d);
            ddouble sinh_mzero = ddouble.Sinh(-0d);
            ddouble sinh_pinf = ddouble.Sinh(double.PositiveInfinity);
            ddouble sinh_ninf = ddouble.Sinh(double.NegativeInfinity);
            ddouble sinh_nan = ddouble.Sinh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(sinh_pzero), nameof(sinh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(sinh_mzero), nameof(sinh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(sinh_pinf), nameof(sinh_pinf));
            Assert.IsTrue(ddouble.IsNegativeInfinity(sinh_ninf), nameof(sinh_ninf));
            Assert.IsTrue(ddouble.IsNaN(sinh_nan), nameof(sinh_nan));
        }

        [TestMethod]
        public void CoshTest() {
            Console.WriteLine($"{Math.Cosh(9.04):F16}");
            Console.WriteLine($"{ddouble.Cosh((ddouble)9.04m)}");
            Console.WriteLine("4.21688858728823488742026530603182233");

            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Cosh(v);

                Assert.AreEqual(Math.Cosh((double)d), (double)u, Math.Abs(Math.Cosh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble cosh_pzero = ddouble.Cosh(0d);
            ddouble cosh_mzero = ddouble.Cosh(-0d);
            ddouble cosh_pinf = ddouble.Cosh(double.PositiveInfinity);
            ddouble cosh_ninf = ddouble.Cosh(double.NegativeInfinity);
            ddouble cosh_nan = ddouble.Cosh(double.NaN);

            Assert.IsTrue(cosh_pzero == 1, nameof(cosh_pzero));
            Assert.IsTrue(cosh_mzero == 1, nameof(cosh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(cosh_pinf), nameof(cosh_pinf));
            Assert.IsTrue(ddouble.IsPositiveInfinity(cosh_ninf), nameof(cosh_ninf));
            Assert.IsTrue(ddouble.IsNaN(cosh_nan), nameof(cosh_nan));
        }

        [TestMethod]
        public void TanhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Tanh(v);

                Assert.AreEqual(Math.Tanh((double)d), (double)u, 1e-15, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble tanh_pzero = ddouble.Tanh(0d);
            ddouble tanh_mzero = ddouble.Tanh(-0d);
            ddouble tanh_pinf = ddouble.Tanh(double.PositiveInfinity);
            ddouble tanh_ninf = ddouble.Tanh(double.NegativeInfinity);
            ddouble tanh_nan = ddouble.Tanh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(tanh_pzero), nameof(tanh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(tanh_mzero), nameof(tanh_mzero));
            Assert.IsTrue(tanh_pinf == 1d, nameof(tanh_pinf));
            Assert.IsTrue(tanh_ninf == -1d, nameof(tanh_ninf));
            Assert.IsTrue(ddouble.IsNaN(tanh_nan), nameof(tanh_nan));
        }

        [TestMethod]
        public void ArsinhTest() {
            for (decimal d = -10m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Arsinh(v);

                Assert.AreEqual(Math.Asinh((double)d), (double)u, Math.Abs(Math.Asinh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arsinh_pzero = ddouble.Arsinh(0d);
            ddouble arsinh_mzero = ddouble.Arsinh(-0d);
            ddouble arsinh_pinf = ddouble.Arsinh(double.PositiveInfinity);
            ddouble arsinh_ninf = ddouble.Arsinh(double.NegativeInfinity);
            ddouble arsinh_nan = ddouble.Arsinh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(arsinh_pzero), nameof(arsinh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(arsinh_mzero), nameof(arsinh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(arsinh_pinf), nameof(arsinh_pinf));
            Assert.IsTrue(ddouble.IsNegativeInfinity(arsinh_ninf), nameof(arsinh_ninf));
            Assert.IsTrue(ddouble.IsNaN(arsinh_nan), nameof(arsinh_nan));
        }

        [TestMethod]
        public void ArcoshTest() {
            for (decimal d = 1m; d <= +10m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Arcosh(v);

                Assert.AreEqual(Math.Acosh((double)d), (double)u, Math.Abs(Math.Acosh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble arcosh_pzero = ddouble.Arcosh(0d);
            ddouble arcosh_mzero = ddouble.Arcosh(-0d);
            ddouble arcosh_pinf = ddouble.Arcosh(double.PositiveInfinity);
            ddouble arcosh_ninf = ddouble.Arcosh(double.NegativeInfinity);
            ddouble arcosh_nan = ddouble.Arcosh(double.NaN);

            Assert.IsTrue(ddouble.IsNaN(arcosh_pzero), nameof(arcosh_pzero));
            Assert.IsTrue(ddouble.IsNaN(arcosh_mzero), nameof(arcosh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(arcosh_pinf), nameof(arcosh_pinf));
            Assert.IsTrue(ddouble.IsNaN(arcosh_ninf), nameof(arcosh_ninf));
            Assert.IsTrue(ddouble.IsNaN(arcosh_nan), nameof(arcosh_nan));
        }

        [TestMethod]
        public void ArtanhTest() {
            for (decimal d = -0.99m; d <= +0.99m; d += 0.01m) {
                ddouble v = (ddouble)d;
                ddouble u = ddouble.Artanh(v);

                Assert.AreEqual(Math.Atanh((double)d), (double)u, Math.Abs(Math.Atanh((double)d)) * 1e-12, d.ToString());
                Assert.IsTrue(ddouble.IsRegulared(u));
            }

            ddouble artanh_pzero = ddouble.Artanh(0d);
            ddouble artanh_mzero = ddouble.Artanh(-0d);
            ddouble artanh_pone = ddouble.Artanh(1d);
            ddouble artanh_mone = ddouble.Artanh(-1d);
            ddouble artanh_pinf = ddouble.Artanh(double.PositiveInfinity);
            ddouble artanh_ninf = ddouble.Artanh(double.NegativeInfinity);
            ddouble artanh_nan = ddouble.Artanh(double.NaN);

            Assert.IsTrue(ddouble.IsPlusZero(artanh_pzero), nameof(artanh_pzero));
            Assert.IsTrue(ddouble.IsMinusZero(artanh_mzero), nameof(artanh_mzero));
            Assert.IsTrue(ddouble.IsPositiveInfinity(artanh_pone), nameof(artanh_pone));
            Assert.IsTrue(ddouble.IsNegativeInfinity(artanh_mone), nameof(artanh_mone));
            Assert.IsTrue(ddouble.IsNaN(artanh_pinf), nameof(artanh_pinf));
            Assert.IsTrue(ddouble.IsNaN(artanh_ninf), nameof(artanh_ninf));
            Assert.IsTrue(ddouble.IsNaN(artanh_nan), nameof(artanh_nan));
        }
    }
}
