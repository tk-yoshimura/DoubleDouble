using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace DoubleDoubleTest {

    internal static class HPAssert {

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta) {
            AreEqual(expected, actual, delta, $"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}");
        }

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta, string message) {
            if (ddouble.IsInfinity(expected)) {
                Assert.IsTrue(ddouble.IsInfinity(actual), message);
                Assert.AreEqual(expected.Sign, actual.Sign, message);
                return;
            }

            if (ddouble.Abs(expected - actual) > delta) {
                throw new AssertFailedException($"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
            }
        }

        public static void NeighborBits(ddouble expected, ddouble actual, int dist = 1) {
            NeighborBits(expected, actual, $"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}", dist);
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
