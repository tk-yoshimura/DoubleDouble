using System;
using System.Collections.Generic;
using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class JacobiTrigonPrototype {
        public static ddouble JacobiSn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }

            if (IsZero(x)) {
                return Zero;
            }

            ddouble y = JacobiTrigon.SnLeqOneK(x, k);

            return y;
        }

        public static ddouble JacobiCn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }

            if (IsZero(x)) {
                return 1d;
            }

            ddouble y = JacobiTrigon.CnLeqOneK(x, k);

            return y;
        }

        public static ddouble JacobiDn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }

            if (IsZero(x)) {
                return 1d;
            }

            ddouble y = JacobiTrigon.DnLeqOneK(x, k);

            return y;
        }

        internal static class JacobiTrigon {
            public static ddouble NearOne = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL);
            public static ddouble Eps = Math.ScaleB(1, -51);
            
            public static ddouble SnLeqOneK(ddouble x, ddouble k) {
                if (k >= NearOne) {
                    return Tanh(x);
                }
                if (k < Eps) {
                    return SnNearZeroK(x, k);
                }

                ddouble phi = Phi(x, k);

                ddouble y = Sin(phi);

                return y;
            }

            public static ddouble SnNearZeroK(ddouble x, ddouble k) {
                ddouble sinx = Sin(x), cosx = Cos(x);

                ddouble y = sinx - k * k * (x - sinx * cosx) * cosx / 4;

                return y;
            }

            public static ddouble CnLeqOneK(ddouble x, ddouble k) {
                if (k >= NearOne) {
                    return 1 / Cosh(x);
                }
                if (k < Eps) {
                    return CnNearZeroK(x, k);
                }

                ddouble phi = Phi(x, k);

                ddouble y = Cos(phi);

                return y;
            }

            public static ddouble CnNearZeroK(ddouble x, ddouble k) {
                ddouble sinx = Sin(x), cosx = Cos(x);

                ddouble y = cosx - k * k * (x - sinx * cosx) * sinx / 4;

                return y;
            }

            public static ddouble DnLeqOneK(ddouble x, ddouble k) {
                if (k >= NearOne) {
                    return 1 / Cosh(x);
                }
                if (k < Eps) {
                    return DnNearZeroK(x, k);
                }

                ddouble phi = Phi(x, k);

                ddouble sn = Sin(phi), cn = Cos(phi);

                ddouble y = Sqrt(cn * cn + (1 - k * k) * sn * sn);

                return y;
            }

            public static ddouble DnNearZeroK(ddouble x, ddouble k) {
                ddouble sinx = Sin(x);

                ddouble y = 1 - k * k * sinx * sinx / 4;

                return y;
            }

            public static ddouble Phi(ddouble x, ddouble k) {
                ddouble a = 1, b = Sqrt(1d - k * k), c = k;

                List<ddouble> a_list = new() { a };
                List<ddouble> c_list = new() { c };

                int n = 1;
                for (; n < 32; n++) {
                    (a, b, c) = ((a + b) / 2, Sqrt(a * b), (a - b) / 2);

                    a_list.Add(a);
                    c_list.Add(c);
                    
                    if (a == b || c < Eps) {
                        break;
                    }
                }

                ddouble phi = Ldexp(a * x, n);

                while (n > 0) {
                    phi = (phi + Asin(c_list[n] / a_list[n] * Sin(phi))) / 2;
                    n--;
                }

                return phi;
            }
        }
    }
}