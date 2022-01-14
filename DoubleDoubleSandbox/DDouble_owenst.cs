using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class OwenT {

        internal static class OwenTPatefieldTandyAlgo {
            public static ddouble T1(ddouble h, ddouble a, int n) { 
                ddouble h2 = h * h, a2 = a * a;
                                
                ddouble p = Pow(3 + 2 * Sqrt2, n);
                ddouble d = (p + 1d / p) / 2;

                ddouble half_h2 = h2 / 2;
                ddouble e = Exp(-half_h2);
                
                ddouble ds = e, dp = e, ap = a;
                ddouble b = checked(-n * n * 2), c = d - 1d;
                ddouble s = a * c * e;

                for (int k = 1, conv_times = 0; k < n && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    ap *= a2;
                    dp *= half_h2 / k;
                    ds += dp;
                    c = b - c;
                    
                    ddouble u = c * ds * ap / (2 * k + 1);
                    s += u;
                    b *= ((k + n) * (k - n)) / ((k + Point5) * (k + 1));

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= n - 1) {
                        return NaN;
                    }
                }

                ddouble y = s / (2 * d * PI);

                return y;
            }

            public static ddouble T2(ddouble h, ddouble a, int max_terms = 64) {
                ddouble h2 = h * h, na2 = -a * a, ha = h * a;
                
                ddouble v = 1d / h2;
                ddouble w = a * Exp(-ha * ha / 2) / Sqrt(2 * PI);
                ddouble u = Erf(ha / Sqrt2) / (2 * h);
                ddouble s = u;

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    u = v * (w - (2 * k + 1) * u);
                    s += u;
                    w *= na2;

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= max_terms - 1) {
                        return NaN;
                    }
                }

                ddouble y = s * Exp(-h2 / 2) / Sqrt(2 * PI);

                return y;
            }

            public static ddouble T4(ddouble h, ddouble a, int max_terms = 64) {
                ddouble h2 = h * h, na2 = -a * a;
                
                ddouble v = a * Exp(h2 * (na2 - 1d) / 2) / (2 * PI);
                ddouble w = 1d, u = v;
                ddouble s = v;

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    w = (1d - h2 * w) / (2 * k + 3);
                    v *= na2;

                    u = v * w;
                    s += u;

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= max_terms - 1) {
                        return NaN;
                    }
                }

                return s;
            }
        }
    }
}