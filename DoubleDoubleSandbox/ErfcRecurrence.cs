using DoubleDouble;
using System;

namespace DoubleDoubleSandbox {
    internal static class ErfcRecurrence {
        public static ddouble Erfc(ddouble x, int m) {
            if (m < 2) {
                throw new ArgumentOutOfRangeException(nameof(m));
            }

            ddouble f0 = 1e-256, f1 = ddouble.Zero;

            for (int k = m; k >= 0; k--) {
                (f0, f1) = (2 * x * f0 + (2 * (k + 1)) * f1, f0);
            }

            ddouble y = 2 * ddouble.Exp(-x * x) / ddouble.Sqrt(ddouble.PI) * f1 / f0;

            return y;
        }
    }
}
