namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble JacobiSn(ddouble x, ddouble m) {
            if (m < 0d || m > 1d) {
                throw new ArgumentOutOfRangeException(nameof(m));
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

            int n = (int)Floor(x / period);
            ddouble v = x - n * period;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.SnLeqOneK(v, m) : JacobiTrigon.SnLeqOneK(period - v, m);

            y = ((n & 2) == 0) ? y : -y;

            return y;
        }

        public static ddouble JacobiCn(ddouble x, ddouble m) {
            if (m < 0d || m > 1d) {
                throw new ArgumentOutOfRangeException(nameof(m));
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

            int n = (int)Floor(x / period);
            ddouble v = x - n * period;

            n &= 3;

            ddouble y = ((n & 1) == 0) ? JacobiTrigon.CnLeqOneK(v, m) : JacobiTrigon.CnLeqOneK(period - v, m);

            y = (n == 0 || n == 3) ? y : -y;

            return y;
        }

        public static ddouble JacobiDn(ddouble x, ddouble m) {
            if (m < 0d || m > 1d) {
                throw new ArgumentOutOfRangeException(nameof(m));
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

            ddouble period = JacobiTrigon.Period(m) * 2;

            int n = (int)Floor(x / period);
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
            public static ddouble NearOne = (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL);
            public static ddouble Eps = double.ScaleB(1, -51);

            private static Dictionary<ddouble, ddouble> period_table = new();

            private static Dictionary<ddouble, (ddouble a, ddouble[] ds)> phi_table = new();

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

                ddouble y = sinx - m * (x - sinx * cosx) * cosx / 4;

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

                ddouble y = cosx - m * (x - sinx * cosx) * sinx / 4;

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

                ddouble y = 1d - m * sinx * sinx / 4;

                return y;
            }

            public static ddouble Phi(ddouble x, ddouble m) {
                (ddouble a, ddouble[] ds) = Phi(m);

                ddouble phi = Ldexp(a * x, ds.Length);

                for (int i = ds.Length - 1; i >= 0; i--) {
                    phi = (phi + Asin(ds[i] * Sin(phi))) / 2;
                }

                return phi;
            }

            public static ddouble Period(ddouble m) {
                if (!period_table.ContainsKey(m)) {
                    period_table.Add(m, EllipticK(m));
                }

                return period_table[m];
            }

            public static (ddouble a, ddouble[] ds) Phi(ddouble m) {
                if (!phi_table.ContainsKey(m)) {
                    phi_table.Add(m, GeneratePhiTable(m));
                }

                return phi_table[m];
            }

            private static (ddouble a, ddouble[] ds) GeneratePhiTable(ddouble m) {
                ddouble a = 1d, b = Sqrt(1d - m), c = Sqrt(m);

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