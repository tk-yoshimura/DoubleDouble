using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class BernoulliPolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> BernoulliPolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => -0.5 + x,
                (x) => {
                    return (1 + x * (-6 + x * 6)) / 6;
                },
                (x) => {
                    return (1 + x * (-3 + x * 2)) * x / 2;
                },
                (x) => {
                    return (-1 + x * x * (30 + x * (-60 + x * 30))) / 30;
                },
                (x) => {
                    return (-1 + x * x * (10 + x * (-15 + x * 6))) * x / 6;
                },
                (x) => {
                    return (1 + x * x * (-21 + x * x * (105 + x * (-126 + x * 42)))) / 42;
                },
                (x) => {
                    return (1 + x * x * (-7 + x * x * (21 + x * (-21 + x * 6)))) * x / 6;
                },
                (x) => {
                    return (-1 + x * x * (20 + x * x * (-70 + x * x * (140 + x * (-120 + x * 30))))) / 30;
                },
                (x) => {
                    return (-3 + x * x * (20 + x * x * (-42 + x * x * (60 + x * (-45 + x * 10))))) * x / 10;
                },
            });

        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> BernoulliCenteredPolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => x,
                (x) => {
                    return (-1 + x * x * 12) / 12;
                },
                (x) => {
                    return (-1 + x * x * 4) * x / 4;
                },
                (x) => {
                    return (7 + x * x * (-120 + x * x * 240)) / 240;
                },
                (x) => {
                    return (7 + x * x * (-40 + x * x * 48)) * x / 48;
                },
                (x) => {
                    return (-31 + x * x * (588 + x * x * (-1680 + x * x * 1344))) / 1344;
                },
                (x) => {
                    return (-31 + x * x * (196 + x * x * (-336 + x * x * 192))) * x / 192;
                },
                (x) => {
                    return (127 + x * x * (-2480 + x * x * (7840 + x * x * (-8960 + x * x * 3840)))) / 3840;
                },
                (x) => {
                    return (381 + x * x * (-2480 + x * x * (4704 + x * x * (-3840 + x * x * 1280)))) * x / 1280;
                },
            });

        [TestMethod]
        public void BernoulliPolyTest() {
            for (int n = 64; n >= BernoulliPolynomials.Count; n--) {
                for (ddouble x = 0; x <= 1; x += 1d / 32) {
                    ddouble actual = ddouble.Bernoulli(n, x, centered: false);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"normal {n},{x}");
                }
            }

            for (int n = 64; n >= BernoulliCenteredPolynomials.Count; n--) {
                for (ddouble x = -0.5; x <= 0.5; x += 1d / 32) {
                    ddouble actual = ddouble.Bernoulli(n, x, centered: true);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"centered {n},{x}");
                }
            }

            for (int n = 0; n < BernoulliPolynomials.Count; n++) {
                for (ddouble x = 0; x <= 1; x += 1d / 32) {
                    ddouble expected = BernoulliPolynomials[n](x);
                    ddouble actual = ddouble.Bernoulli(n, x, centered: false);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-27, $"normal {n},{x}");
                }
            }

            for (int n = 0; n < BernoulliCenteredPolynomials.Count; n++) {
                for (ddouble x = -0.5; x <= 0.5; x += 1d / 32) {
                    ddouble expected = BernoulliCenteredPolynomials[n](x);
                    ddouble actual = ddouble.Bernoulli(n, x, centered: true);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-28, $"centered {n},{x}");
                }
            }

            for (int n = 0; n <= 8; n++) {
                for (ddouble x = 0.125; x <= 0.875; x += 0.125) {
                    if (x == 0.5) {
                        continue;
                    }

                    ddouble x_dec = ddouble.BitDecrement(x);
                    ddouble x_inc = ddouble.BitIncrement(x);

                    ddouble y = ddouble.Bernoulli(n, x, centered: false);
                    ddouble y_dec = ddouble.Bernoulli(n, x_dec, centered: false);
                    ddouble y_inc = ddouble.Bernoulli(n, x_inc, centered: false);

                    HPAssert.AreEqual(y, y_dec, ddouble.Abs(y) * 2e-28, $"normal border {n},{x}");
                    HPAssert.AreEqual(y, y_inc, ddouble.Abs(y) * 2e-28, $"normal border {n},{x}");
                }

                for (ddouble x = -0.375; x <= 0.375; x += 0.125) {
                    if (x == 0) {
                        continue;
                    }

                    ddouble x_dec = ddouble.BitDecrement(x);
                    ddouble x_inc = ddouble.BitIncrement(x);

                    ddouble y = ddouble.Bernoulli(n, x, centered: true);
                    ddouble y_dec = ddouble.Bernoulli(n, x_dec, centered: true);
                    ddouble y_inc = ddouble.Bernoulli(n, x_inc, centered: true);

                    HPAssert.AreEqual(y, y_dec, ddouble.Abs(y) * 2e-28, $"normal border {n},{x}");
                    HPAssert.AreEqual(y, y_inc, ddouble.Abs(y) * 2e-28, $"normal border {n},{x}");
                }
            }
        }
    }
}
