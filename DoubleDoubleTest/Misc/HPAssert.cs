using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace DoubleDoubleTest {

    internal static class HPAssert {

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta) {
            AreEqual(expected, actual, delta, string.Empty);
        }

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta, string message) {
            if (ddouble.IsInfinity(expected)) {
                Assert.IsTrue(ddouble.IsInfinity(actual), $"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
                Assert.AreEqual(expected.Sign, actual.Sign, $"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
                return;
            }

            Assert.AreEqual(ddouble.IsNaN(expected), ddouble.IsNaN(actual), $"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");

            if (ddouble.Abs(expected - actual) > delta) {
                throw new AssertFailedException($"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
            }
        }

        public static void NeighborBits(ddouble expected, ddouble actual, int dist = 1) {
            NeighborBits(expected, actual, string.Empty, dist);
        }

        public static void NeighborBits(ddouble expected, ddouble actual, string message, int dist = 1) {
            BigInteger n_expected = FloatSplitter.Split(expected).mantissa;
            BigInteger n_actual = FloatSplitter.Split(actual).mantissa;

            if (BigInteger.Abs(n_expected - n_actual) > dist) {
                throw new AssertFailedException($"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
            }
        }
    }
}
