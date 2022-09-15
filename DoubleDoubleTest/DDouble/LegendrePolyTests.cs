using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class LegendrePolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> LegendrePolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => x,
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(-1d + x2 * 3d, -1);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((-6d + x2 * 10d) * x, -2);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(3d + x2 * (-30d + x2 * 35d), -3);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((30d + x2 * (-140d + x2 * 126d)) * x, -4);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(-10d + x2 * (210d + x2 * (-630d + x2 * 462d)), -5);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((-140d + x2 * (1260d + x2 * (-2772d + x2 * 1716d))) * x, -6);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(35d + x2 * (-1260d + x2 * (6930d + x2 * (-12012d + x2 * 6435d))), -7);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((630d + x2 * (-9240d + x2 * (36036d + x2 * (-51480d + x2 * 24310d)))) * x, -8);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(-126d + x2 * (6930d + x2 * (-60060d + x2 * (180180d + x2 * (-218790d + x2 * 92378d)))), -9);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((-2772d + x2 * (60060d + x2 * (-360360d + x2 * (875160d + x2 * (-923780d + x2 * 352716d))))) * x, -10);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(462d + x2 * (-36036d + x2 * (450450d + x2 * (-2042040d + x2 * (4157010d + x2 * (-3879876d + x2 * 1352078d))))), -11);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((12012d + x2 * (-360360d + x2 * (3063060d + x2 * (-11085360d + x2 * (19399380d + x2 * (-16224936d + x2 * 5200300d)))))) * x, -12);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(-1716d + x2 * (180180d + x2 * (-3063060d + x2 * (19399380d + x2 * (-58198140d + x2 * (89237148d + x2 * (-67603900d + x2 * 20058300d)))))), -13);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp((-51480d + x2 * (2042040d + x2 * (-23279256d + x2 * (116396280d + x2 * (-297457160d + x2 * (405623400d + x2 * (-280816200d + x2 * 77558760d))))))) * x, -14);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return ddouble.Ldexp(6435d + x2 * (-875160d + x2 * (19399380d + x2 * (-162954792d + x2 * (669278610d + x2 * (-1487285800d + x2 * (1825305300d + x2 * (-1163381400d + x2 * 300540195d))))))), -15);
                },
            });

        [TestMethod]
        public void LegendreTest() {
            for (int n = 64; n >= LegendrePolynomials.Count; n--) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble actual = ddouble.Legendre(n, x);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{x}");
                }
            }

            for (int n = 0; n < LegendrePolynomials.Count; n++) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble expected = LegendrePolynomials[n](x);
                    ddouble actual = ddouble.Legendre(n, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{x}");
                }
            }
        }
    }
}
