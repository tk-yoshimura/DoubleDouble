using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble JacobiSn(ddouble x, ddouble m) {
            if (IsNegative(m) || m > 1d) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -JacobiSn(-x, m);
            }

            if (!IsFinite(x) || !IsFinite(m)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 0d;
            }
            if (m >= JacobiTrigon.NearOne) {
                return Tanh(x);
            }

            ddouble period = JacobiTrigon.Period(m);

            long n = (long)Floor(x / period);
            ddouble v = x - n * period;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.SnLeqOneK(v, m) : JacobiTrigon.SnLeqOneK(period - v, m);

            y = ((n & 2) == 0) ? y : -y;

            return y;
        }

        public static ddouble JacobiCn(ddouble x, ddouble m) {
            if (IsNegative(m) || m > 1d) {
                return NaN;
            }
            if (IsNegative(x)) {
                return JacobiCn(-x, m);
            }

            if (!IsFinite(x) || !IsFinite(m)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (m >= JacobiTrigon.NearOne) {
                return 1d / Cosh(x);
            }

            ddouble period = JacobiTrigon.Period(m);

            long n = (long)Floor(x / period);
            ddouble v = x - n * period;

            n &= 3;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.CnLeqOneK(v, m) : JacobiTrigon.CnLeqOneK(period - v, m);

            y = (n == 0 || n == 3) ? y : -y;

            return y;
        }

        public static ddouble JacobiDn(ddouble x, ddouble m) {
            if (IsNegative(m) || m > 1d) {
                return NaN;
            }
            if (IsNegative(x)) {
                return JacobiDn(-x, m);
            }

            if (!IsFinite(x) || !IsFinite(m)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (m >= JacobiTrigon.NearOne) {
                return 1d / Cosh(x);
            }

            ddouble period = Ldexp(JacobiTrigon.Period(m), 1);

            long n = (long)Floor(x / period);
            ddouble v = x - n * period;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.DnLeqOneK(v, m) : JacobiTrigon.DnLeqOneK(period - v, m);

            return y;
        }

        public static ddouble JacobiAm(ddouble x, ddouble m) => JacobiTrigon.Phi(x, m);

        public static ddouble JacobiArcSn(ddouble x, ddouble m) {
            return EllipticF(Asin(x), m);
        }

        public static ddouble JacobiArcCn(ddouble x, ddouble m) {
            return EllipticF(Acos(x), m);
        }

        public static ddouble JacobiArcDn(ddouble x, ddouble m) {
            ddouble s = Sqrt((1d - x * x) / m);

            return JacobiArcSn(s, m);
        }

        internal static class JacobiTrigon {
            public static readonly ddouble NearOne = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL);
            public static readonly double Eps = double.ScaleB(1, -51);

            private static ConcurrentDictionary<ddouble, ddouble> period_table = [];
            private static ConcurrentDictionary<ddouble, (ddouble a, ReadOnlyCollection<ddouble> ds)> phi_table = [];

            public static ddouble SnLeqOneK(ddouble x, ddouble m) {
                if (m < Eps) {
                    return SnNearZeroK(x, m);
                }

                ddouble phi = Phi(x, m);

                ddouble y = Sin(phi);

                return y;
            }

            public static ddouble SnNearZeroK(ddouble x, ddouble m) {
                ddouble sinx = Sin(x), cosx = Cos(x);

                ddouble y = sinx - Ldexp(m * (x - sinx * cosx) * cosx, -2);

                return y;
            }

            public static ddouble CnLeqOneK(ddouble x, ddouble m) {
                if (m < Eps) {
                    return CnNearZeroK(x, m);
                }

                ddouble phi = Phi(x, m);

                ddouble y = Cos(phi);

                return y;
            }

            public static ddouble CnNearZeroK(ddouble x, ddouble m) {
                ddouble sinx = Sin(x), cosx = Cos(x);

                ddouble y = cosx - Ldexp(m * (x - sinx * cosx) * sinx, -2);

                return y;
            }

            public static ddouble DnLeqOneK(ddouble x, ddouble m) {
                if (m < Eps) {
                    return DnNearZeroK(x, m);
                }

                ddouble phi = Phi(x, m);

                ddouble sn = Sin(phi), cn = Cos(phi);

                ddouble y = Sqrt(cn * cn + (1d - m) * sn * sn);

                return y;
            }

            public static ddouble DnNearZeroK(ddouble x, ddouble m) {
                ddouble sinx = Sin(x);

                ddouble y = 1d - Ldexp(m * sinx * sinx, -2);

                return y;
            }

            public static ddouble Phi(ddouble x, ddouble m) {
                (ddouble a, ReadOnlyCollection<ddouble> ds) = Phi(m);

                ddouble phi = Ldexp(a * x, ds.Count);

                for (int i = 0; i < ds.Count; i++) {
                    phi = Ldexp(phi + Asin(ds[i] * Sin(phi)), -1);
                }

                return phi;
            }

            public static ddouble Period(ddouble m) {
                if (!period_table.TryGetValue(m, out ddouble value)) {
                    value = EllipticK(m);
                    period_table[m] = value;
                }

                return value;
            }

            public static (ddouble a, ReadOnlyCollection<ddouble> ds) Phi(ddouble m) {
                if (!phi_table.TryGetValue(m, out (ddouble a, ReadOnlyCollection<ddouble> ds) value)) {
                    value = GeneratePhiTable(m);
                    phi_table[m] = value;
                }

                return value;
            }

            private static (ddouble a, ReadOnlyCollection<ddouble> ds) GeneratePhiTable(ddouble m) {
                ddouble a = 1d, b = Sqrt(1d - m), c = Sqrt(m);

                List<ddouble> a_list = new() { a };
                List<ddouble> c_list = new() { c };
                List<ddouble> d_list = new();

                for (int n = 1; n < 32; n++) {
                    (a, b, c) = (Ldexp(a + b, -1), GeometricMean(a, b), Ldexp(a - b, -1));

                    a_list.Add(a);
                    c_list.Add(c);
                    d_list.Add(c / a);

                    if (a == b || c < Eps) {
                        break;
                    }
                }

                d_list.Reverse();

                return (a, new ReadOnlyCollection<ddouble>(d_list));
            }
        }
    }
}