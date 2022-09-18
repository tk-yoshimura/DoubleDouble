using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDoubleTest.DDouble {
    [TestClass]
    public class LaguerrePolyTests {
        private static readonly ReadOnlyCollection<Func<ddouble, ddouble>> LaguerrePolynomials =
            new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                (x) => 1d,
                (x) => 1d - x,
                (x) => {
                    return (2d + x * (-4d + x)) / 2;
                },
                (x) => {
                    return (6d + x * (-18d + x * (9d - x))) / 6;
                },
                (x) => {
                    return (24d + x * (-96d + x * (72d + x * (-16d + x)))) / 24;
                },
                (x) => {
                    return (120d + x * (-600d + x * (600d + x * (-200d + x * (25d - x))))) / 120;
                },
                (x) => {
                    return (720d + x * (-4320d + x * (5400d + x * (-2400d + x * (450d + x * (-36d + x)))))) / 720;
                },
                (x) => {
                    return (5040d + x * (-35280d + x * (52920d + x * (-29400d + x * (7350d + x * (-882d + x * (49d - x))))))) / 5040;
                },
                (x) => {
                    return (40320d + x * (-322560d + x * (564480d + x * (-376320d + x * (117600d + x * (-18816d + x * (1568d + x * (-64d + x)))))))) / 40320;
                },
                (x) => {
                    return (362880d + x * (-3265920d + x * (6531840d + x * (-5080320d + x * (1905120d + x * (-381024d + x * (42336d + x * (-2592d + x * (81d - x))))))))) / 362880;
                },
                (x) => {
                    return (3628800d + x * (-36288000d + x * (81648000d + x * (-72576000d + x * (31752000d + x * (-7620480d + x * (1058400d + x * (-86400d + x * (4050d + x * (-100d + x)))))))))) / 3628800;
                }
            });

        private static readonly Dictionary<(int, ddouble), Func<ddouble, ddouble>> LaguerreTables =
            new Dictionary<(int, ddouble), Func<ddouble, ddouble>>{
                { (0,  0.5), (x) => 1d },
                { (1,  0.5), (x) => -(-3 + x * 2) / 2 },
                { (2,  0.5), (x) => (15 + x * (-20 + x * 4)) / 8 },
                { (3,  0.5), (x) => -(-105 + x * (210 + x * (-84 + x * 8))) / 48 },
                { (4,  0.5), (x) => (945 + x * (-2520 + x * (1512 + x * (-288 + x * 16)))) / 384 },
                { (5,  0.5), (x) => -(-10395 + x * (34650 + x * (-27720 + x * (7920 + x * (-880 + x * 32))))) / 3840 },
                { (6,  0.5), (x) => (135135 + x * (-540540 + x * (540540 + x * (-205920 + x * (34320 + x * (-2496 + x * 64)))))) / 46080 },

                { (0,  2.5), (x) => 1d },
                { (1,  2.5), (x) => -(-7 + x * 2) / 2 },
                { (2,  2.5), (x) => (63 + x * (-36 + x * 4))/8 },
                { (3,  2.5), (x) => -(-693 + x * (594 + x * (-132 + x * 8))) / 48 },
                { (4,  2.5), (x) => (9009 + x * (-10296 + x * (3432 + x * (-416 + x * 16)))) / 384 },
                { (5,  2.5), (x) => -(-135135 + x * (193050 + x * (-85800 + x * (15600 + x * (-1200 + x * 32)))))/3840 },
                { (6,  2.5), (x) => (2297295 + x * (-3938220 + x * (2187900 + x * (-530400 + x * (61200 + x * (-3264 + x * 64)))))) / 46080 },
            };

        [TestMethod]
        public void LaguerreTest() {
            for (int n = 64; n >= LaguerrePolynomials.Count; n--) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble actual = ddouble.LaguerreL(n, x);

                    Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{x}");
                }
            }

            for (int n = 0; n < LaguerrePolynomials.Count; n++) {
                for (ddouble x = -8; x <= 8; x += 0.125) {
                    ddouble expected = LaguerrePolynomials[n](x);
                    ddouble actual = ddouble.LaguerreL(n, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 1e-31, $"{n},{x}");
                }
            }
        }

        [TestMethod]
        public void AssociatedLaguerreTest() {
            for (int n = 64; n >= 0; n--) {
                for (ddouble alpha = 0; alpha <= 4; alpha += 0.25) {
                    for (ddouble x = 0; x <= 1; x += 0.0625) {
                        ddouble actual = ddouble.LaguerreL(n, alpha, x);

                        Assert.IsTrue(ddouble.IsFinite(actual), $"{n},{alpha},{x}");
                    }
                }
            }

            for (int n = 64; n >= 0; n--) {
                for (ddouble x = 0; x <= 1; x += 0.0625) {
                    ddouble expected = ddouble.LaguerreL(n, x);
                    ddouble actual = ddouble.LaguerreL(n, 0, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-31, $"{n},0,{x}");
                }
            }

            foreach ((int n, ddouble alpha) in LaguerreTables.Keys) {
                for (ddouble x = 0; x <= 1; x += 0.0625) {
                    ddouble expected = LaguerreTables[(n, alpha)](x);
                    ddouble actual = ddouble.LaguerreL(n, alpha, x);

                    HPAssert.AreEqual(expected, actual, ddouble.Abs(expected) * 2e-31, $"{n},{alpha},{x}");
                }
            }
        }
    }
}
