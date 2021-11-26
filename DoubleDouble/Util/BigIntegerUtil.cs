using System.Numerics;

namespace DoubleDouble {
    internal static class BigIntegerUtil {
        public static BigInteger RightShift(BigInteger n, int sfts) {
            if (sfts > 0) {
                return n >> sfts;
            }
            if (sfts < 0) {
                return n << -sfts;
            }
            return n;
        }

        public static BigInteger LeftShift(BigInteger n, int sfts) {
            if (sfts > 0) {
                return n << sfts;
            }
            if (sfts < 0) {
                return n >> -sfts;
            }
            return n;
        }

        public static BigInteger RoundDiv(BigInteger x, BigInteger y) {
            BigInteger n = x / y, r = x - y * n;

            if (r >= y / 2) {
                n += 1;
            }

            return n;
        }
    }
}
