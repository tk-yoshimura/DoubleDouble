using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompleteGammaPrototype {

        public static ddouble LowerIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return PositiveInfinity;
            }

            ddouble g = Gamma(s);
            if (IsInfinity(g)) {
                return NaN;
            }

            ddouble f = 1;

            for (int n = m; n >= 0; n--) {
                f = s + (2 * n) - (f * (s + n) * x) / (((n + 1) * x) + f * (s + (2 * n + 1)));
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static ddouble UpperIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return -Ei(-x);
            }

            ddouble f = 1;

            for (int n = m; n >= 1; n--) {
                f = x + f * (n - s) / (f + n);
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static (ddouble y, int m) LowerIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = LowerIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = LowerIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static (ddouble y, int m) UpperIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = UpperIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = UpperIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }
    }
}