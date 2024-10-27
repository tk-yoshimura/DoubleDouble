using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrecisionTestTools;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class CarlsonIntegralTests {
        [TestMethod]
        public void CarlsonRDTest() {
            for (ddouble z = 0; z <= 4; z += 0.25) {
                for (ddouble y = 0; y <= 4; y += 0.25) {
                    for (ddouble x = 0; x <= 4; x += 0.25) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble v1 = ddouble.CarlsonRD(x, y, z);
                        ddouble v2 = ddouble.CarlsonRD(y, z, x);
                        ddouble v3 = ddouble.CarlsonRD(z, x, y);

                        ddouble f = v1 + v2 + v3;
                        ddouble e = 3 / ddouble.Sqrt(x * y * z);

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble z = Math.ScaleB(1, -256); z <= 1; z *= Math.ScaleB(1, 32)) {
                for (ddouble y = Math.ScaleB(1, -256); y <= 1; y *= Math.ScaleB(1, 32)) {
                    for (ddouble x = Math.ScaleB(1, -256); x <= 1; x *= Math.ScaleB(1, 32)) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble v1 = ddouble.CarlsonRD(x, y, z);
                        ddouble v2 = ddouble.CarlsonRD(y, z, x);
                        ddouble v3 = ddouble.CarlsonRD(z, x, y);

                        ddouble f = v1 + v2 + v3;
                        ddouble e = 3 / ddouble.Sqrt(x * y * z);

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble z = 1; z <= Math.ScaleB(1, 256); z *= Math.ScaleB(1, 32)) {
                for (ddouble y = 1; y <= Math.ScaleB(1, 256); y *= Math.ScaleB(1, 32)) {
                    for (ddouble x = 1; x <= Math.ScaleB(1, 256); x *= Math.ScaleB(1, 32)) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble v1 = ddouble.CarlsonRD(x, y, z);
                        ddouble v2 = ddouble.CarlsonRD(y, z, x);
                        ddouble v3 = ddouble.CarlsonRD(z, x, y);

                        ddouble f = v1 + v2 + v3;
                        ddouble e = 3 / ddouble.Sqrt(x * y * z);

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble v = Math.ScaleB(1, -64); v > 0; v *= Math.ScaleB(1, -16)) {
                ddouble y = ddouble.CarlsonRD(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "eps");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRD(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y < 1, "largeval");
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRD(ddouble.NaN, ddouble.NaN, ddouble.NaN)), "nan");
            Assert.IsFalse(ddouble.IsNaN(ddouble.CarlsonRD(ddouble.Epsilon, ddouble.Epsilon, ddouble.Epsilon)), "eps");
            Assert.IsTrue(ddouble.IsFinite(ddouble.CarlsonRD(ddouble.MaxValue, ddouble.MaxValue, ddouble.MaxValue)), "largeval");
        }

        [TestMethod]
        public void CarlsonRCTest() {
            for (ddouble y = 0; y <= 4; y += 0.25) {
                for (ddouble x = 0; x <= 4; x += 0.25) {
                    Console.WriteLine($"{x},{y}");

                    ddouble z = ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, x + z);
                    ddouble v2 = ddouble.CarlsonRC(y, y + z);
                    ddouble v3 = ddouble.CarlsonRC(0, z);

                    ddouble f = v1 + v2;
                    ddouble e = v3;

                    Console.WriteLine(f);
                    Console.WriteLine(e);

                    PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y}");
                }
            }

            for (ddouble y = 0; y <= 4; y += 0.25) {
                for (ddouble x = 0; x <= 4; x += 0.25) {
                    Console.WriteLine($"{x},{y}");

                    ddouble lambda = y + 2 * ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, y);
                    ddouble v2 = ddouble.CarlsonRC(x + lambda, y + lambda);

                    ddouble f = 2 * v2;
                    ddouble e = v1;

                    Console.WriteLine(f);
                    Console.WriteLine(e);

                    PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y}");
                }
            }

            for (ddouble y = Math.ScaleB(1, -256); y <= 1; y *= Math.ScaleB(1, 32)) {
                for (ddouble x = Math.ScaleB(1, -256); x <= 1; x *= Math.ScaleB(1, 32)) {
                    Console.WriteLine($"{x},{y}");

                    ddouble z = ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, x + z);
                    ddouble v2 = ddouble.CarlsonRC(y, y + z);
                    ddouble v3 = ddouble.CarlsonRC(0, z);

                    ddouble f = v1 + v2;
                    ddouble e = v3;

                    Console.WriteLine(f);
                    Console.WriteLine(e);

                    PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y}");
                }
            }

            for (ddouble y = Math.ScaleB(1, -256); y <= 1; y *= Math.ScaleB(1, 32)) {
                for (ddouble x = Math.ScaleB(1, -256); x <= 1; x *= Math.ScaleB(1, 32)) {
                    Console.WriteLine($"{x},{y}");

                    ddouble z = ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, x + z);
                    ddouble v2 = ddouble.CarlsonRC(y, y + z);
                    ddouble v3 = ddouble.CarlsonRC(0, z);

                    ddouble f = v1 + v2;
                    ddouble e = v3;

                    Console.WriteLine(f);
                    Console.WriteLine(e);

                    PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y}");
                }
            }

            for (ddouble y = 1; y <= Math.ScaleB(1, 256); y *= Math.ScaleB(1, 32)) {
                for (ddouble x = 1; x <= Math.ScaleB(1, 256); x *= Math.ScaleB(1, 32)) {
                    Console.WriteLine($"{x},{y}");

                    ddouble z = ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, x + z);
                    ddouble v2 = ddouble.CarlsonRC(y, y + z);
                    ddouble v3 = ddouble.CarlsonRC(0, z);

                    ddouble f = v1 + v2;
                    ddouble e = v3;

                    Console.WriteLine(f);
                    Console.WriteLine(e);

                    PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y}");
                }
            }

            for (ddouble v = Math.ScaleB(1, -64); v > 0; v *= Math.ScaleB(1, -16)) {
                ddouble y = ddouble.CarlsonRC(v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "eps");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRC(v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y < 1, "largeval");
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRC(ddouble.NaN, ddouble.NaN)), "nan");
            Assert.IsFalse(ddouble.IsNaN(ddouble.CarlsonRC(ddouble.Epsilon, ddouble.Epsilon)), "eps");
            Assert.IsTrue(ddouble.IsFinite(ddouble.CarlsonRC(ddouble.MaxValue, ddouble.MaxValue)), "largeval");
        }

        [TestMethod]
        public void CarlsonRFTest() {
            for (ddouble z = 0.25d; z <= 4; z += 0.25) {
                for (ddouble y = 0; y <= 4; y += 0.25) {
                    for (ddouble x = 0; x <= 4; x += 0.25) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble w = (x * y) / z;

                        ddouble v1 = ddouble.CarlsonRF(x, x + z, x + w);
                        ddouble v2 = ddouble.CarlsonRF(y, y + z, y + w);
                        ddouble v3 = ddouble.CarlsonRF(0, z, w);

                        ddouble f = v1 + v2;
                        ddouble e = v3;

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble z = 0; z <= 4; z += 0.25) {
                for (ddouble y = 0; y <= 4; y += 0.25) {
                    for (ddouble x = 0; x <= 4; x += 0.25) {
                        Console.WriteLine($"{x},{y},{z}");

                        (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                            ddouble.Sqrt(x), ddouble.Sqrt(y), ddouble.Sqrt(z)
                        );

                        ddouble lambda = sqrtx * sqrty + sqrty * sqrtz + sqrtz * sqrtx;

                        ddouble v1 = ddouble.CarlsonRF(x, y, z);
                        ddouble v2 = ddouble.CarlsonRF(x + lambda, y + lambda, z + lambda);

                        ddouble f = 2 * v2;
                        ddouble e = v1;

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble z = Math.ScaleB(1, -256); z <= 1; z *= Math.ScaleB(1, 32)) {
                for (ddouble y = Math.ScaleB(1, -256); y <= 1; y *= Math.ScaleB(1, 32)) {
                    for (ddouble x = Math.ScaleB(1, -256); x <= 1; x *= Math.ScaleB(1, 32)) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble w = (x * y) / z;

                        ddouble v1 = ddouble.CarlsonRF(x, x + z, x + w);
                        ddouble v2 = ddouble.CarlsonRF(y, y + z, y + w);
                        ddouble v3 = ddouble.CarlsonRF(0, z, w);

                        ddouble f = v1 + v2;
                        ddouble e = v3;

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble z = 1; z <= Math.ScaleB(1, 256); z *= Math.ScaleB(1, 32)) {
                for (ddouble y = 1; y <= Math.ScaleB(1, 256); y *= Math.ScaleB(1, 32)) {
                    for (ddouble x = 1; x <= Math.ScaleB(1, 256); x *= Math.ScaleB(1, 32)) {
                        Console.WriteLine($"{x},{y},{z}");

                        ddouble w = (x * y) / z;

                        ddouble v1 = ddouble.CarlsonRF(x, x + z, x + w);
                        ddouble v2 = ddouble.CarlsonRF(y, y + z, y + w);
                        ddouble v3 = ddouble.CarlsonRF(0, z, w);

                        ddouble f = v1 + v2;
                        ddouble e = v3;

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble v = Math.ScaleB(1, -64); v > 0; v *= Math.ScaleB(1, -16)) {
                ddouble y = ddouble.CarlsonRF(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "eps");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRF(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y < 1, "largeval");
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRF(ddouble.NaN, ddouble.NaN, ddouble.NaN)), "nan");
            Assert.IsFalse(ddouble.IsNaN(ddouble.CarlsonRF(ddouble.Epsilon, ddouble.Epsilon, ddouble.Epsilon)), "eps");
            Assert.IsTrue(ddouble.IsFinite(ddouble.CarlsonRF(ddouble.MaxValue, ddouble.MaxValue, ddouble.MaxValue)), "largeval");
        }

        [TestMethod]
        public void CarlsonRJTest() {
            for (ddouble rho = 0; rho <= 4; rho += 0.5) {
                for (ddouble z = 0.5d; z <= 4; z += 0.5) {
                    for (ddouble y = 0.5; y <= 4; y += 0.5) {
                        for (ddouble x = 0.5; x <= 4; x += 0.5) {
                            Console.WriteLine($"{x},{y},{z},{rho}");

                            ddouble d = x * y / z;

                            ddouble a = rho * rho * (x + y + z + d);
                            ddouble b = rho * (rho + x) * (rho + y);

                            ddouble v1 = ddouble.CarlsonRJ(x, x + z, x + d, x + rho);
                            ddouble v2 = ddouble.CarlsonRJ(y, y + z, y + d, y + rho);
                            ddouble v3 = ddouble.CarlsonRJ(a, b, b, a);
                            ddouble v4 = ddouble.CarlsonRJ(0, z, d, rho);

                            ddouble f = v1 + v2 + (a - b) * v3 + 3 / ddouble.Sqrt(a);
                            ddouble e = v4;

                            Console.WriteLine(f);
                            Console.WriteLine(e);

                            if (ddouble.IsPositiveInfinity(v4)) {
                                Assert.IsTrue(
                                    ddouble.IsPositiveInfinity(v1) ||
                                    ddouble.IsPositiveInfinity(v2) ||
                                    ddouble.IsPositiveInfinity(v3)
                                );
                            }
                            else {
                                PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z},{rho}");
                            }
                        }
                    }
                }
            }

            for (ddouble z = 0; z <= 4; z += 0.5) {
                for (ddouble y = 0; y <= 4; y += 0.5) {
                    for (ddouble x = 0; x <= 4; x += 0.5) {
                        for (ddouble rho = ddouble.Max(x, y, z) + 0.5; rho <= 4; rho += 0.5) {
                            Console.WriteLine($"{x},{y},{z},{rho}");

                            (ddouble sqrtx, ddouble sqrty, ddouble sqrtz, ddouble sqrtrho) = (
                                ddouble.Sqrt(x), ddouble.Sqrt(y), ddouble.Sqrt(z), ddouble.Sqrt(rho)
                            );

                            ddouble lambda = sqrtx * sqrty + sqrty * sqrtz + sqrtz * sqrtx;
                            ddouble m = (rho - x) * (rho - y) * (rho - z);
                            ddouble d = (sqrtrho + sqrtx) * (sqrtrho + sqrty) * (sqrtrho + sqrtz);

                            ddouble v1 = ddouble.CarlsonRJ(x, y, z, rho);
                            ddouble v2 = ddouble.CarlsonRJ(x + lambda, y + lambda, z + lambda, rho + lambda);
                            ddouble v3 = ddouble.CarlsonRC(1, 1 + m / (d * d));

                            ddouble f = 2 * v2 + 6 / d * v3;
                            ddouble e = v1;

                            Console.WriteLine(f);
                            Console.WriteLine(e);

                            PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z},{rho}");
                        }
                    }
                }
            }

            for (ddouble v = Math.ScaleB(1, -64); v > 0; v *= Math.ScaleB(1, -16)) {
                ddouble y = ddouble.CarlsonRJ(v, v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "eps");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRJ(v, v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y < 1, "largeval");
            }

            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRJ(ddouble.NaN, ddouble.NaN, ddouble.NaN, ddouble.NaN)), "nan");
            Assert.IsFalse(ddouble.IsNaN(ddouble.CarlsonRJ(ddouble.Epsilon, ddouble.Epsilon, ddouble.Epsilon, ddouble.Epsilon)), "eps");
            Assert.IsTrue(ddouble.IsFinite(ddouble.CarlsonRJ(ddouble.MaxValue, ddouble.MaxValue, ddouble.MaxValue, ddouble.MaxValue)), "largeval");
        }

        [TestMethod]
        public void CarlsonRGTest() {
            Assert.AreEqual(0, ddouble.CarlsonRG(0, 0, 0));
            PrecisionAssert.AlmostEqual(0.5, ddouble.CarlsonRG(0, 0, 1), 1e-30);
            PrecisionAssert.AlmostEqual(ddouble.PI / 4, ddouble.CarlsonRG(0, 1, 1), 1e-30);
            PrecisionAssert.AlmostEqual(1, ddouble.CarlsonRG(1, 1, 1), 1e-30);
            PrecisionAssert.AlmostEqual(1 / ddouble.Sqrt(2) + ddouble.Log(1 + ddouble.Sqrt(2)) / 2, ddouble.CarlsonRG(1, 1, 2), 1e-30);
            PrecisionAssert.AlmostEqual(ddouble.PI / 4 + 0.5, ddouble.CarlsonRG(1, 2, 2), 1e-30);
            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRG(ddouble.NaN, ddouble.NaN, ddouble.NaN)));

            for (ddouble z = 0; z <= 4; z += 0.25) {
                for (ddouble y = 0; y <= 4; y += 0.25) {
                    for (ddouble x = 0; x <= 4; x += 0.25) {
                        if ((x == 0 && y == 0) || (y == 0 && z == 0) || (z == 0 && x == 0)) {
                            continue;
                        }

                        Console.WriteLine($"{x},{y},{z}");

                        (ddouble sqrtx, ddouble sqrty, ddouble sqrtz) = (
                            ddouble.Sqrt(x), ddouble.Sqrt(y), ddouble.Sqrt(z)
                        );

                        ddouble lambda = sqrtx * sqrty + sqrty * sqrtz + sqrtz * sqrtx;

                        ddouble v1 = ddouble.CarlsonRG(x, y, z);
                        ddouble v2 = ddouble.CarlsonRG(x + lambda, y + lambda, z + lambda);
                        ddouble v3 = ddouble.CarlsonRF(x, y, z);

                        ddouble f = 2 * v2 - (lambda * v3 + sqrtx + sqrty + sqrtz) / 2;
                        ddouble e = v1;

                        Console.WriteLine(f);
                        Console.WriteLine(e);

                        PrecisionAssert.AlmostEqual(e, f, 1e-30d, $"{x},{y},{z}");
                    }
                }
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(4, v, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, 4, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(Math.ScaleB(1, -80), v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, Math.ScaleB(1, -80), 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v * 16, v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, v * 16, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                PrecisionAssert.AlmostEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -64); v > 0; v *= Math.ScaleB(1, -16)) {
                ddouble y = ddouble.CarlsonRG(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y < 1, "eps");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRG(v, v, v);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "largeval");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRG(v, v, 1);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "largeval");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRG(v, 1, 1);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "largeval");
            }

            for (ddouble v = Math.ScaleB(1, 32); v < ddouble.PositiveInfinity; v *= Math.ScaleB(1, 32)) {
                ddouble y = ddouble.CarlsonRG(v, v, 0);

                Console.WriteLine(y);

                Assert.IsTrue(y > 1, "largeval");
            }
        }
    }
}
