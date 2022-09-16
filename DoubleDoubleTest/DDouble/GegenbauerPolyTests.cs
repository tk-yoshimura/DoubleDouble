using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class GegenbauerPolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> GegenbauerAlpha2Polynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => x * 4d,
                (x) => {
                    ddouble x2 = x * x;
                    return -2d + x2 * 12d;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-12d + x2 * 32d) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return 3d + x2 * (-48d + x2 * 80d);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (24d + x2 * (-160d + x2 * 192d)) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return -4d + x2 * (120d + x2 * (-480d + x2 * 448d));
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-40d + x2 * (480d + x2 * (-1344d + x2 * 1024d))) * x;
                },
            });

        [TestMethod]
        public void GegenbauerCTest() {
            for (int n = 64; n >= 0; n--) {
                for (ddouble alpha = -4; alpha <= 4; alpha += 0.25) {
                    for (ddouble x = -1; x <= 1; x += 0.125) {
                        ddouble actual = ddouble.GegenbauerC(n, alpha, x);

                        Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{alpha},{x}");
                    }
                }
            }

            for (int n = 0; n < GegenbauerAlpha2Polynomials.Count; n++) {
                for (ddouble x = -1; x <= 1; x += 0.125) {
                    ddouble expected = GegenbauerAlpha2Polynomials[n](x);
                    ddouble actual = ddouble.GegenbauerC(n, 2, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},2,{x}");
                }
            }

            for (int n = 0; n <= 6; n++) {
                for (ddouble x = -1; x <= 1; x += 0.125) {
                    ddouble expected = ddouble.ChebyshevU(n, x);
                    ddouble actual = ddouble.GegenbauerC(n, 1, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},1,{x}");
                }
            }

            for (int n = 0; n <= 6; n++) {
                for (ddouble x = -1; x <= 1; x += 0.125) {
                    ddouble expected = ddouble.LegendreP(n, x);
                    ddouble actual = ddouble.GegenbauerC(n, 0.5, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},0.5,{x}");
                }
            }
        }
    }
}
