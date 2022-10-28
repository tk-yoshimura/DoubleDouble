using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseGamma(ddouble x) {
            if (!(x >= 1)) {
                return NaN;
            }

            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            const double c = 0.036533814484900416, s = 0.3989422804014327;

            static double crude_lambertw(double x) {
                double y;

                if (x < 8) {
                    y = x * (60.0 + x * (114.0 + x * 17.0)) / (60.0 + x * (174.0 + x * 101.0));
                }
                else {
                    double logx = Math.Log(x), loglogx = Math.Log(logx);

                    y = logx - loglogx + loglogx / (logx + logx);
                }

                double exp_y, d;

                exp_y = Math.Exp(y);
                d = y * exp_y - x;
                y -= d / (exp_y * (y + 1d) - (y + 2d) * d / (y + y + 2d));

                return y;
            };

            double l = Math.Log((x.hi + c) * s);
            ddouble y = l / (crude_lambertw(l / Math.E)) + 0.5;

            ddouble lnx = Log(x);

            for (int i = 0; i < 8; i++) {
                ddouble lng = LogGamma(y), psi = Digamma(y);
                ddouble delta = (lnx - lng) / psi;

                y += delta;

                if (Math.Abs(delta.hi) < y.hi * 5e-32) {
                    break;
                }
            }

            for (int i = 0; i < 8; i++) {
                ddouble g = Gamma(y);
                if (x == g || !ddouble.IsFinite(g)) {
                    break;
                }

                ddouble psi = Digamma(y);
                ddouble delta = (x / g - 1.0) / psi;

                y += delta;

                if (Math.Abs(delta.hi) < y.hi * 5e-32) {
                    break;
                }
            }

            y = RoundMantissa(y, keep_bits: 105);

            return y;
        }
    }
}