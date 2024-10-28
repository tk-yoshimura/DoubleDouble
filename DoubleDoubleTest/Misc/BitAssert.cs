using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoubleDoubleTest {

    internal static class BitAssert {
        public static void NeighborBits(ddouble expected, ddouble actual, uint dist = 1) {
            NeighborBits(expected, actual, string.Empty, dist);
        }

        public static void NeighborBits(ddouble expected, ddouble actual, string message, uint dist = 1) {
            UInt128 n_expected = FloatSplitter.Split(expected).mantissa;
            UInt128 n_actual = FloatSplitter.Split(actual).mantissa;

            if (n_expected >= n_actual && (n_expected - n_actual) > dist) {
                throw new AssertFailedException($"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
            }
            if (n_expected < n_actual && (n_actual - n_expected) > dist) {
                throw new AssertFailedException($"{nameof(expected)}:{expected}\n{nameof(actual)}:  {actual}\n{message}");
            }
        }
    }
}
