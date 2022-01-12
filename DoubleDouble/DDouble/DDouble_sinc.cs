using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinc(ddouble x, bool normalized = true) {
            if (Abs(x) < Math.ScaleB(1, -105)) {
                return 1d;
            }

            if (normalized) {
                return SinPI(x) / (PI * x);
            }
            else {
                return Sin(x) / x;
            }
        }
    }
}