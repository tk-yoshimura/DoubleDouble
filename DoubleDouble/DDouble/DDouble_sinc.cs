using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Sinc(ddouble x, bool normalized = true) {
            if (normalized) {
                if (Abs(x) < Math.ScaleB(1, -64)) {
                    return 1d - Square(x * PI) / 6d;
                }

                ddouble c = PI * x;

                if (IsInfinity(c)) {
                    return Zero;
                }

                return SinPI(x) / c;
            }
            else {
                if (Abs(x) < Math.ScaleB(1, -64)) {
                    return 1d - x * x / 6d;
                }

                if (IsInfinity(x)) {
                    return Zero;
                }

                return Sin(x) / x;
            }
        }
    }
}