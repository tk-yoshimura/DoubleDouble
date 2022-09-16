using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class JacobiPolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> JacobiPAlpha1Beta2Polynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => (-1 + x * 5) / 2,
                (x) => (-3 + x * (-6 + x * 21)) / 4,
                (x) => (1 + x * (-7 + x * (-7 + x * 21))) / 2,
                (x) => (5 + x * (20 + x * (-90 + x * (-60 + x * 165)))) / 8,
                (x) => (-15 + x * (135 + x * (270 + x * (-990 + x * (-495 + x * 1287))))) / 32,
                (x) => (-35 + x * (-210 + x * (1155 + x * (1540 + x * (-5005 + x * (-2002 + x * 5005)))))) / 64,
            });

        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> JacobiPAlpha2Beta1Polynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => (1 + x * 5) / 2,
                (x) => (-3 + x * (6 + x * 21)) / 4,
                (x) => (-1 + x * (-7 + x * (7 + x * 21))) / 2,
                (x) => (5 + x * (-20 + x * (-90 + x * (60 + x * 165)))) / 8,
                (x) => (15 + x * (135 + x * (-270 + x * (-990 + x * (495 + x * 1287))))) / 32,
                (x) => (-35 + x * (210 + x * (1155 + x * (-1540 + x * (-5005 + x * (2002 + x * 5005)))))) / 64,
            });

        [TestMethod]
        public void JacobiPTest() {
            for (int n = 64; n >= 0; n--) {
                for (ddouble alpha = -0.75; alpha <= 4; alpha += 0.25) {
                    for (ddouble beta = -0.75; beta <= 4; beta += 0.25) {
                        for (ddouble x = -1; x <= 1; x += 0.125) {
                            ddouble actual = ddouble.JacobiP(n, alpha, beta, x);

                            Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{alpha},{beta},{x}");
                        }
                    }
                }
            }

            for (int n = 0; n < JacobiPAlpha1Beta2Polynomials.Count; n++) {
                for (ddouble x = -1; x <= 1; x += 0.125) {
                    ddouble expected = JacobiPAlpha1Beta2Polynomials[n](x);
                    ddouble actual = ddouble.JacobiP(n, 1, 2, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},1,2,{x}");
                }
            }

            for (int n = 0; n < JacobiPAlpha2Beta1Polynomials.Count; n++) {
                for (ddouble x = -1; x <= 1; x += 0.125) {
                    ddouble expected = JacobiPAlpha2Beta1Polynomials[n](x);
                    ddouble actual = ddouble.JacobiP(n, 2, 1, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},2,1,{x}");
                }
            }
        }
    }
}
