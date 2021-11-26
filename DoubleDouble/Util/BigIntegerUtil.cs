using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
    }
}
