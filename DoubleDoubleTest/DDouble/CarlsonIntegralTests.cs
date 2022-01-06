using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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

                    HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y}");
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

                    HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y}");
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

                    HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y}");
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

                    HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y}");
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z}");
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
            for (ddouble w = 0; w <= 4; w += 0.5) {
                for (ddouble z = 0.5d; z <= 4; z += 0.5) {
                    for (ddouble y = 0.5; y <= 4; y += 0.5) {
                        for (ddouble x = 0.5; x <= 4; x += 0.5) {
                            Console.WriteLine($"{x},{y},{z},{w}");

                            ddouble d = x * y / z;

                            ddouble a = w * w * (x + y + z + d);
                            ddouble b = w * (w + x) * (w + y);

                            ddouble v1 = ddouble.CarlsonRJ(x, x + z, x + d, x + w);
                            ddouble v2 = ddouble.CarlsonRJ(y, y + z, y + d, y + w);
                            ddouble v3 = ddouble.CarlsonRJ(a, b, b, a);
                            ddouble v4 = ddouble.CarlsonRJ(0, z, d, w);

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
                                HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30, $"{x},{y},{z},{w}");
                            }
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
            HPAssert.AreEqual(0.5, ddouble.CarlsonRG(0, 0, 1), 1e-30);
            HPAssert.AreEqual(ddouble.PI / 4, ddouble.CarlsonRG(0, 1, 1), 1e-30);
            HPAssert.AreEqual(1, ddouble.CarlsonRG(1, 1, 1), 1e-30);
            HPAssert.AreEqual(1 / ddouble.Sqrt(2) + ddouble.Log(1 + ddouble.Sqrt(2)) / 2, ddouble.CarlsonRG(1, 1, 2), 1e-30);
            HPAssert.AreEqual(ddouble.PI / 4 + 0.5, ddouble.CarlsonRG(1, 2, 2), 1e-30);
            Assert.IsTrue(ddouble.IsNaN(ddouble.CarlsonRG(ddouble.NaN, ddouble.NaN, ddouble.NaN)));

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(4, v, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(40000, v, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(100, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, 4, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, 40000, v);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(100, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -96); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, v, 40000);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(100, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(Math.ScaleB(1, -80), v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, Math.ScaleB(1, -80), 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v * 16, v, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
            }

            for (ddouble v = Math.ScaleB(1, -80); v > 0; v *= Math.ScaleB(1, -4)) {
                ddouble y = ddouble.CarlsonRG(v, v * 16, 4);

                Console.WriteLine($"{ddouble.Frexp(v).exp},{y}");

                HPAssert.AreEqual(1, y, 1e-20);
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
