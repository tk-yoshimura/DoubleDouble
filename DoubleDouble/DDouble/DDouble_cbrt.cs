using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Cbrt(ddouble v) {
            if (v.Sign < 0) {
                return -Cbrt(-v);
            }
            if (IsNaN(v)) {
                return NaN;
            }
            if (IsZero(v)) {
                return 0;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            (int v_exponent, ddouble v_frac) = Frexp(v);
            int exponent_rem = (v_exponent >= 0) ? v_exponent % 3 : ((3 - (-v_exponent) % 3) % 3);

            if (exponent_rem != 0) {
                v_frac = Ldexp(v_frac, exponent_rem);
            }

            ddouble a = 1 / Math.Cbrt(v_frac.hi);

            ddouble h = 1 - v_frac * a * a * a;
            a *= 1 + h * (27 + h * (18 + h * 14)) * Consts.Rcp81;

            h = 1 - v_frac * a * a * a;
            a *= 1 + h * (27 + h * (18 + h * 14)) * Consts.Rcp81;

            ddouble y = Ldexp(v_frac * a * a, (v_exponent - exponent_rem) / 3);

            return y;
        }

        private static partial class Consts {
            public static ddouble Rcp81 { get; } = Rcp(81);
        }
    }
}
