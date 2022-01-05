using System;
using System.Collections.Generic;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble JacobiSn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }
            if (x < 0) {
                return -JacobiSn(-x, k);
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }
            if (IsZero(x)) {
                return Zero;
            }
            if (k >= JacobiTrigon.NearOne) {
                return Tanh(x);
            }

            ddouble period = JacobiTrigon.Period(k);

            int n = (int)Floor(x / period);
            ddouble v = x - n * period;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.SnLeqOneK(v, k) : JacobiTrigon.SnLeqOneK(period - v, k);

            y = ((n & 2) == 0) ? y : -y;

            return y;
        }

        public static ddouble JacobiCn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }
            if (x < 0) {
                return JacobiCn(-x, k);
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (k >= JacobiTrigon.NearOne) {
                return 1d / Cosh(x);
            }

            ddouble period = JacobiTrigon.Period(k);

            int n = (int)Floor(x / period);
            ddouble v = x - n * period;

            n &= 3;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.CnLeqOneK(v, k) : JacobiTrigon.CnLeqOneK(period - v, k);

            y = (n == 0 || n == 3) ? y : -y;

            return y;
        }

        public static ddouble JacobiDn(ddouble x, ddouble k) {
            if (k < 0d || k > 1d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }
            if (x < 0) {
                return JacobiDn(-x, k);
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (k >= JacobiTrigon.NearOne) {
                return 1d / Cosh(x);
            }

            ddouble period = JacobiTrigon.Period(k) * 2;

            int n = (int)Floor(x / period);
            ddouble v = x - n * period;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.DnLeqOneK(v, k) : JacobiTrigon.DnLeqOneK(period - v, k);

            return y;
        }

        internal static class JacobiTrigon {
            public static ddouble NearOne = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL);
            public static ddouble Eps = Math.ScaleB(1, -51);

            private static Dictionary<ddouble, ddouble> period_table = new();

            private static Dictionary<ddouble, (ddouble a, ddouble[] ds)> phi_table = new();

            public static ddouble SnLeqOneK(ddouble x, ddouble k) {
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
                (ddouble a, ddouble[] ds) = Phi(k);

                ddouble phi = Ldexp(a * x, ds.Length);

                for(int i = ds.Length - 1; i >= 0; i--){
                    phi = (phi + Asin(ds[i] * Sin(phi))) / 2;
                }

                return phi;
            }

            public static ddouble Period(ddouble k) {
                if (!period_table.ContainsKey(k)) {
                    period_table.Add(k, EllipticK(k));
                }

                return period_table[k];
            }

            public static (ddouble a, ddouble[] ds) Phi(ddouble k) {
                if (!phi_table.ContainsKey(k)) {
                    phi_table.Add(k, GeneratePhiTable(k));
                }

                return phi_table[k];
            }

            private static (ddouble a, ddouble[] ds) GeneratePhiTable(ddouble k) { 
                ddouble a = 1, b = Sqrt(1d - k * k), c = k;

                List<ddouble> a_list = new() { a };
                List<ddouble> c_list = new() { c };
                List<ddouble> d_list = new();

                for (int n = 1; n < 32; n++) {
                    (a, b, c) = ((a + b) / 2, Sqrt(a * b), (a - b) / 2);

                    a_list.Add(a);
                    c_list.Add(c);
                    d_list.Add(c / a);

                    if (a == b || c < Eps) {
                        break;
                    }
                }

                return (a, d_list.ToArray());
            }
        }
    }
}