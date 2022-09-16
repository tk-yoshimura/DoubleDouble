using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

        private static readonly Dictionary<(int, int), Func<ddouble, ddouble>> LegendreTables =
            new Dictionary<(int, int), Func<ddouble, ddouble>>{
                { (0,  0), (x) => 1d },

                { (1, -1), (x) => ddouble.Sqrt(1 - x * x) / 2 },
                { (1,  0), (x) => x },
                { (1, +1), (x) => -ddouble.Sqrt(1 - x * x) },

                { (2, -2), (x) => (1 - x * x) / 8 },
                { (2, -1), (x) => x * ddouble.Sqrt(1 - x * x) / 2 },
                { (2,  0), (x) => (-1 + x * x * 3) / 2 },
                { (2, +1), (x) => -x * ddouble.Sqrt(1 - x * x) * 3 },
                { (2, +2), (x) => (1 - x * x) * 3 },

                { (3, -3), (x) => ddouble.Pow(1 - x * x, 1.5d) / 48 },
                { (3, -2), (x) => x * (1 - x * x) / 8 },
                { (3, -1), (x) => -(1 - x * x * 5) * ddouble.Sqrt(1 - x * x) / 8 },
                { (3,  0), (x) => x * (-3 + x * x * 5) / 2 },
                { (3, +1), (x) => (3 - x * x * 15) * ddouble.Sqrt(1 - x * x) / 2 },
                { (3, +2), (x) => x * (1 - x * x) * 15 },
                { (3, +3), (x) => -ddouble.Pow(1 - x * x, 1.5d) * 15 },

                { (4, -4), (x) => ddouble.Pow(1 - x * x, 2) / 384 },
                { (4, -3), (x) => x * ddouble.Pow(1 - x * x, 1.5d) / 48 },
                { (4, -2), (x) => (-1 + x * x * 7) * (1 - x * x) / 48 },
                { (4, -1), (x) => x * (-3 + x * x * 7) * ddouble.Sqrt(1 - x * x) / 8 },
                { (4,  0), (x) => (3 + x * x * (-30 + x * x * 35)) / 8 },
                { (4, +1), (x) => x * (15 - x * x * 35) * ddouble.Sqrt(1 - x * x) / 2 },
                { (4, +2), (x) => (-15 + x * x * 105) * (1 - x * x) / 2 },
                { (4, +3), (x) => -x * ddouble.Pow(1 - x * x, 1.5d) * 105 },
                { (4, +4), (x) => ddouble.Pow(1 - x * x, 2) * 105 },

                { (8, -2), (x) => (-1 + x * x * (33 + x * x * (-143 + x * x * 143))) * (1 - x * x) / 256 },
                { (8, -1), (x) => x * (-35 + x * x * (385 + x * x * (-1001 + x * x * 715))) * ddouble.Sqrt(1 - x * x) / 128 },
                { (8, +1), (x) => -x * (-35 + x * x * (385 + x * x * (-1001 + x * x * 715))) * ddouble.Sqrt(1 - x * x) * 9 / 16 },
                { (8, +2), (x) => (-1 + x * x * (33 + x * x * (-143 + x * x * 143))) * (1 - x * x) * 315 / 16 },
            };

        [TestMethod]
        public void LegendreTest() {
            for (int n = 64; n >= LegendrePolynomials.Count; n--) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble actual = ddouble.LegendreP(n, x);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{x}");
                }
            }

            for (int n = 0; n < LegendrePolynomials.Count; n++) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble expected = LegendrePolynomials[n](x);
                    ddouble actual = ddouble.LegendreP(n, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{x}");
                }
            }
        }

        [TestMethod]
        public void AssociatedLegendreTest() {
            for(int n = 64; n >= 0; n--) {
                for (int m = -n; m <= n; m++) {
                    for (ddouble x = -1; x <= 1; x += 0.0625) {
                        ddouble actual = ddouble.LegendreP(n, m, x);

                        Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{m},{x}");
                    }
                }
            }

            foreach ((int n, int m) in LegendreTables.Keys) {
                for (ddouble x = -1; x <= 1; x += 0.0625) {
                    ddouble expected = LegendreTables[(n, m)](x);
                    ddouble actual = ddouble.LegendreP(n, m, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-31, $"{n},{m},{x}");
                }
            }
        }
    }
}
