using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Numerics;

namespace DoubleDoubleTest {

    public static class DDoubleAssert {

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta) {
            if (ddouble.Abs(expected - actual) > delta) {
                throw new AssertFailedException();
            }
        }

        public static void AreEqual(ddouble expected, ddouble actual, ddouble delta, string message) {
            if (ddouble.Abs(expected - actual) > delta) {
                throw new AssertFailedException(message);
            }
        }

        public static void AreNotEqual(ddouble notExpected, ddouble actual, ddouble delta) {
            if (ddouble.Abs(notExpected - actual) <= delta) {
                throw new AssertFailedException();
            }
        }

        public static void AreNotEqual(ddouble notExpected, ddouble actual, ddouble delta, string message) {
            if (ddouble.Abs(notExpected - actual) <= delta) {
                throw new AssertFailedException(message);
            }
        }

        public static void NeighborBits(ddouble expected, ddouble actual, int dist = 1) {
            BigInteger n_expected = FloatSplitter.Split(expected).mantissa;
            BigInteger n_actual = FloatSplitter.Split(actual).mantissa;

            if (BigInteger.Abs(n_expected - n_actual) > dist) {
                throw new AssertFailedException();
            }
        }

        public static void NeighborBits(ddouble expected, ddouble actual, string message, int dist = 1) {
            BigInteger n_expected = FloatSplitter.Split(expected).mantissa;
            BigInteger n_actual = FloatSplitter.Split(actual).mantissa;

            if (BigInteger.Abs(n_expected - n_actual) > dist) {
                throw new AssertFailedException(message);
            }
        }
    }
}
