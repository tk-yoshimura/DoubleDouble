using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Atan(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (!IsFinite(x)) {
                return x.Sign >= 0 ? PI / 2 : -PI / 2;
            }

            if (x <= 1d && x >= -1d) {
                ddouble z = ddouble.Abs(x) / ddouble.Sqrt(x * x + 1);
                ddouble w = ddouble.Sqrt(ddouble.SquareAsin(z));

                if (IsZero(w)) {
                    return x;
                }

                return x.Sign >= 0 ? w : -w;
            }
            else {
                ddouble invx = 1 / x;
                ddouble z = ddouble.Abs(invx) / ddouble.Sqrt(invx * invx + 1);
                ddouble w = ddouble.Sqrt(ddouble.SquareAsin(z));

                if (x.Sign >= 0) {
                    return PI / 2 - w;
                }
                else {
                    return w - PI / 2;
                }
            }
        }

        public static ddouble Asin(ddouble x) {
            if (!(x >= -1d && x <= 1d)) {
                return NaN;
            }

            if (x == -1) {
                return -PI / 2;
            }
            if (x == 1) {
                return PI / 2;
            }

            if (Abs(x) <= Ldexp(Consts.SquareAsin.Sqrt2, -1)) {
                ddouble w = ddouble.Sqrt(ddouble.SquareAsin(ddouble.Abs(x)));

                if (IsZero(w)) {
                    return x;
                }

                return x.Sign >= 0 ? w : -w;
            }
            else {
                ddouble z = x / (Sqrt(1 - x * x) + 1);
                return Atan(z) * 2;
            }
        }

        public static ddouble Acos(ddouble x) {
            return PI / 2 - Asin(x);
        }

        public static ddouble Atan2(ddouble y, ddouble x) {
            if (IsZero(x) && IsZero(y)) {
                return Zero;
            }
            if (!IsFinite(x) || !IsFinite(y)) {
                return NaN;
            }
            if (Abs(x) >= Abs(y)) {
                ddouble yx = y / x;
                return x.Sign >= 0 ? Atan(yx) : ((y.Sign >= 0) ? (Atan(yx) + PI) : (Atan(yx) - PI));
            }
            else {
                ddouble xy = x / y;
                return y.Sign >= 0 ? (PI / 2 - Atan(xy)) : (-PI / 2 - Atan(xy));
            }
        }

        internal static ddouble SquareAsin(ddouble x) {
#if DEBUG
            Debug<ArithmeticException>.Assert(x >= 0d && x < 1d);
#endif

            ddouble z = 0, s = 4 * x * x, t = s;

            foreach (ddouble f in Consts.SquareAsin.FracTable) {
                ddouble dz = t * f;
                ddouble z_next = z + dz;

                if (z == z_next) {
                    break;
                }

                t *= s;
                z = z_next;
            }

            ddouble y = Ldexp(z, -1);

            return y;
        }

        private static partial class Consts {
            public static class SquareAsin {
                public static readonly IReadOnlyList<ddouble> FracTable = GenerateFracTable();

                public static readonly ddouble Sqrt2 = Sqrt(2);

                private static IReadOnlyList<ddouble> GenerateFracTable() {
#if DEBUG
                    Trace.WriteLine($"SquareAsin initialize.");
#endif

                    ddouble n_frac = 1, n2_frac = 2;
                    List<ddouble> fracs = new();

                    for (int n = 1; n <= 128;) {
                        fracs.Add((n_frac * n_frac) / (n * n * n2_frac));

                        n++;
                        n_frac *= n;
                        n2_frac *= (2 * n - 1) * (2 * n);

                        if (!IsFinite(n2_frac)) {
                            break;
                        }
                    }

                    return fracs;
                }
            }
        }
    }
}
