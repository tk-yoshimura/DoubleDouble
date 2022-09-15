using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class ChebyshevPolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> ChebyshevTPolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => x,
                (x) => {
                    ddouble x2 = x * x;
                    return -1d + x2 * 2d;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-3d + x2 * 4d) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return 1d + x2 * (-8d + x2 * 8d);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (5d + x2 * (-20d + x2 * 16d)) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return -1d + x2 * (18d + x2 * (-48d + x2 * 32d));
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-7d + x2 * (56d + x2 * (-112d + x2 * 64d))) * x;
                },
            });

        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> ChebyshevUPolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => 2 * x,
                (x) => {
                    ddouble x2 = x * x;
                    return -1d + x2 * 4d;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-4d + x2 * 8d) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return 1d + x2 * (-12d + x2 * 16d);
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (6d + x2 * (-32d + x2 * 32d)) * x;
                },
                (x) => {
                    ddouble x2 = x * x;
                    return -1d + x2 * (24d + x2 * (-80d + x2 * 64d));
                },
                (x) => {
                    ddouble x2 = x * x;
                    return (-8d + x2 * (80d + x2 * (-192d + x2 * 128d))) * x;
                },
            });

        [TestMethod]
        public void ChebyshevTTest() {
            for (int n = 64; n >= ChebyshevTPolynomials.Count; n--) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble actual = ddouble.ChebyshevT(n, x);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{x}");
                }
            }

            for (int n = 0; n < ChebyshevTPolynomials.Count; n++) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble expected = ChebyshevTPolynomials[n](x);
                    ddouble actual = ddouble.ChebyshevT(n, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{x}");
                }
            }
        }

        [TestMethod]
        public void ChebyshevUTest() {
            for (int n = 64; n >= ChebyshevUPolynomials.Count; n--) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble actual = ddouble.ChebyshevU(n, x);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{x}");
                }
            }

            for (int n = 0; n < ChebyshevUPolynomials.Count; n++) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble expected = ChebyshevUPolynomials[n](x);
                    ddouble actual = ddouble.ChebyshevU(n, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{x}");
                }
            }
        }
    }
}
