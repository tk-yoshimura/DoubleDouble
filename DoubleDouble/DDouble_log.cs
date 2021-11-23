using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Log2(ddouble v) {
            if (v.hi < 0 || IsNaN(v)) {
                return NaN;
            }
            if (v.hi == 0) {
                return NegativeInfinity;
            }
            if (IsInfinity(v)) {
                return PositiveInfinity;
            }

            int n = Math.ILogB(v.hi);
            ddouble f = new ddouble(
                Math.ScaleB(v.hi, -n),
                Math.ScaleB(v.lo, -n)
            );

            ddouble y = n;
            ddouble p = 0.5;

            for (int i = 128; i > 0; i--) {
                f *= f;

                if (f >= 2) {
                    y += p;
                    f = new ddouble(f.hi / 2, f.lo / 2);
                }
                p = new ddouble(p.hi / 2, p.lo / 2);
                
                if (y == (y + p) || f == 1) {
                    break;
                }
            }

            return y;
        }

        public static ddouble Log10(ddouble v) {
            return Log2(v) * Consts.Lg2;
        }

        private static partial class Consts {
            public static ddouble Lg2 { private set; get; } = Rcp(Log2(10));
        }
    }
}
