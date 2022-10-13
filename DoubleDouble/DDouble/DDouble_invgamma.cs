using System;

namespace DoubleDouble {
    public partial struct ddouble {
        private static readonly ddouble c = Sqrt(2 * PI) * RcpE - Gamma(DigammaZero);
        private static readonly ddouble s = 1 / Sqrt(2 * PI);

        public static ddouble InverseGamma(ddouble x) {
            if (!(x >= 1)) {
                return NaN;
            }

            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            ddouble l = Log((x + c) * s);
            ddouble y = l / (LambertW(l * RcpE)) + Point5;

            for (int i = 0; i < 8; i++) {
                ddouble g = Gamma(y), psi = Digamma(y);
                ddouble delta = (1.0 - x / g) / psi;

                if (!ddouble.IsFinite(delta)) {
                    return NaN;
                }

                y -= delta;

                if (Math.Abs(delta.hi) < y.hi * 5e-32) {
                    break;
                }
            }

            y = RoundMantissa(y, keep_bits: 105);

            return y;
        }
    }
}