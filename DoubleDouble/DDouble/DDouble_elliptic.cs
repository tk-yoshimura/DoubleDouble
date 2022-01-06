using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble EllipticK(ddouble k) {
            if (!IsFinite(k) || k.Sign < 0 || k > 1d) {
                return NaN;
            }

            if (IsZero(k)) {
                return Consts.AsinAcos.HalfPI;
            }

            if (k == 1d) {
                return PositiveInfinity;
            }

            ddouble y = EllipticKCore(k);
            return y;
        }

        public static ddouble EllipticE(ddouble k) {
            if (!IsFinite(k) || k.Sign < 0 || k > 1d) {
                return NaN;
            }

            if (IsZero(k)) {
                return Consts.AsinAcos.HalfPI;
            }

            if (k == 1d) {
                return 1d;
            }

            ddouble y = EllipticECore(k);

            return Max(1, y);
        }

        public static ddouble EllipticPi(ddouble n, ddouble k) {
            if (!IsFinite(k) || k.Sign < 0 || k > 1d || n > 1d) {
                return NaN;
            }

            if (IsZero(n)) {
                return EllipticK(k);
            }

            if (n == 1d) {
                return PositiveInfinity;
            }

            if (IsZero(k)) {
                return PI / (2 * Sqrt(1d - n));
            }

            if (k == 1d) {
                return PositiveInfinity;
            }

            ddouble y = EllipticPiCore(n, k);

            return y;
        }

        private static ddouble EllipticKCore(ddouble k, [AllowNull] Dictionary<ddouble, ddouble> kvalue_cache = null) {

            if (kvalue_cache is not null && kvalue_cache.ContainsKey(k)) {
                return kvalue_cache[k];
            }

            ddouble squa_k = k * k;
            ddouble y;

            if (squa_k > Math.ScaleB(1, -8)) {
                ddouble c = Sqrt(1d - squa_k), cp1 = 1d + c, cm1 = 1d - c;

                y = 2d / cp1 * EllipticKCore(cm1 / cp1, kvalue_cache);
            }
            else {
                ddouble x = 1, w = squa_k;

                for (int i = 1; i < int.MaxValue; i++) {
                    ddouble dx = Consts.Elliptic.KTable(i) * w;
                    ddouble x_next = x + dx;

                    if (x == x_next) {
                        break;
                    }

                    w *= squa_k;
                    x = x_next;
                }

                y = x * Consts.AsinAcos.HalfPI;
            }

            if (kvalue_cache is not null) {
                kvalue_cache.Add(k, y);
            }

            return y;
        }

        private static ddouble EllipticECore(ddouble k) {
            ddouble a = 1d;
            ddouble b = Sqrt(1d - k * k);
            ddouble c = Sqrt(Abs(a * a - b * b));
            ddouble q = 1d;

            for (int n = 0; n < int.MaxValue && !IsZero(c) && a != b; n++) {
                ddouble squa_c = c * c;
                ddouble dq = Ldexp(squa_c, n - 1);
                q -= dq;

                (a, b) = (
                    (a + b) / 2, 
                    Sqrt(a * b)
                );

                c = squa_c / (4 * a);
            }

            ddouble y = q * PI / (2 * a);

            return y;
        }

        private static ddouble EllipticPiCore(ddouble n, ddouble k) {
            ddouble a = 1d;
            ddouble b = Sqrt(1d - k * k);
            ddouble p = Sqrt(1d - n);
            ddouble q = 1d;
            ddouble sum_q = 1d;

            while (Abs(sum_q) <= Ldexp(Abs(q), +100)) {
                ddouble ab = a * b, p_squa = p * p;
                ddouble p_squa_pab = p_squa + ab, p_squa_mab = p_squa - ab;

                (a, b, p, q) = (
                    (a + b) / 2, 
                    Sqrt(ab), 
                    p_squa_pab / (2 * p), 
                    q * p_squa_mab / (2 * p_squa_pab)
                );

                sum_q += q;
            }

            ddouble y = (2d + sum_q * n / (1d - n)) * EllipticK(k) / 2;

            return y;
        }

        private static partial class Consts {
            public static class Elliptic {
                private static readonly List<ddouble> k_table;

                static Elliptic() {
                    k_table = new() { 1 };

#if DEBUG
                    Trace.WriteLine($"Elliptic initialized.");
#endif
                }

                public static ddouble KTable(int n) {
                    for (int i = k_table.Count; i <= n; i++) {
                        ddouble k = k_table.Last() * checked(4 * i * (i - 1) + 1) / checked(4 * i * i);

                        k_table.Add(k);
                    }

                    return k_table[n];
                }
            }
        }
    }
}
