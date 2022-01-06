using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class CarlsonIntegralTests {
        [TestMethod]
        public void CarlsonRDTest() {
            for (ddouble z = 0.25; z <= 4; z += 0.25) {
                for (ddouble y = 0.25; y <= 4; y += 0.25) {
                    for (ddouble x = 0.25; x <= 4; x += 0.25) {
                        ddouble v1 = ddouble.CarlsonRD(x, y, z);
                        ddouble v2 = ddouble.CarlsonRD(y, z, x);
                        ddouble v3 = ddouble.CarlsonRD(z, x, y);

                        ddouble f = v1 + v2 + v3;
                        ddouble e = 3 / ddouble.Sqrt(x * y * z);

                        Console.WriteLine($"{x},{y},{z}");

                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30);
                    }
                }
            }
        }

        [TestMethod]
        public void CarlsonRCTest() {
            for (ddouble y = 0.25; y <= 4; y += 0.25) {
                for (ddouble x = 0.25; x <= 4; x += 0.25) {
                    ddouble z = ddouble.Sqrt(x * y);

                    ddouble v1 = ddouble.CarlsonRC(x, x + z);
                    ddouble v2 = ddouble.CarlsonRC(y, y + z);
                    ddouble v3 = ddouble.CarlsonRC(0, z);

                    ddouble f = v1 + v2;
                    ddouble e = v3;

                    Console.WriteLine($"{x},{y}");
                    HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30);
                }
            }
        }

        [TestMethod]
        public void CarlsonRFTest() {
            for (ddouble z = 0.25; z <= 4; z += 0.25) {
                for (ddouble y = 0.25; y <= 4; y += 0.25) {
                    for (ddouble x = 0.25; x <= 4; x += 0.25) {
                        ddouble w = (x * y) / z;

                        ddouble v1 = ddouble.CarlsonRF(x, x + z, x + w);
                        ddouble v2 = ddouble.CarlsonRF(y, y + z, y + w);
                        ddouble v3 = ddouble.CarlsonRF(0, z, w);

                        ddouble f = v1 + v2;
                        ddouble e = v3;

                        Console.WriteLine($"{x},{y},{z}");
                        HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30);
                    }
                }
            }
        }

        [TestMethod]
        public void CarlsonRJTest() {
            for (ddouble p = 0.25; p <= 4; p += 0.25) {
                for (ddouble z = 0.25; z <= 4; z += 0.25) {
                    for (ddouble y = 0.25; y <= 4; y += 0.25) {
                        for (ddouble x = 0.25; x <= 4; x += 0.25) {
                            ddouble w = x * y / z;

                            ddouble a = p * p * (x + y + z + w);
                            ddouble b = p * (p + x) * (p + y);

                            ddouble v1 = ddouble.CarlsonRJ(x, x + z, x + w, x + p);
                            ddouble v2 = ddouble.CarlsonRJ(y, y + z, y + w, y + p);
                            ddouble v3 = ddouble.CarlsonRJ(a, b, b, a);
                            ddouble v4 = ddouble.CarlsonRJ(0, z, w, p);

                            ddouble f = v1 + v2 + (a - b) * v3 + 3 / ddouble.Sqrt(a);
                            ddouble e = v4;

                            Console.WriteLine($"{x},{y}");
                            HPAssert.AreEqual(e, f, ddouble.Abs(e) * 1e-30);
                        }
                    }
                }
            }
        }
    }
}
