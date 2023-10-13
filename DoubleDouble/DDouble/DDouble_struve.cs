using DoubleDouble.Utils;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble StruveH(int n, ddouble x) {
            if (IsNegative(x)) {
                return ((n & 1) == 0) ? -StruveH(n, -x) : StruveH(n, -x);
            }

            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveH function, n greater than 8 and negative integer are not supported."
                );
            }

            if (x < Epsilon) {
                return 0d;
            }

            if (IsNaN(x)) {
                return NaN;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return 0d;
                }
                if (n == 1) {
                    return Ldexp(RcpPI, 1);
                }

                return PositiveInfinity;
            }

            if (x < 8d) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 32);
            }

            return StruveKIntegral.Value(n, x) + BesselY(n, x);
        }

        public static ddouble StruveK(int n, ddouble x) {
            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveK function, n greater than 8 and negative integer are not supported."
                );
            }

            if (IsNegative(x) || IsNaN(x)) {
                return NaN;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return 0d;
                }
                if (n == 1) {
                    return Ldexp(RcpPI, 1);
                }

                return PositiveInfinity;
            }

            if (x < 8d) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 32) - BesselY(n, x);
            }

            return StruveKIntegral.Value(n, x);
        }

        public static ddouble StruveL(int n, ddouble x) {
            if (IsNegative(x)) {
                return ((n & 1) == 0) ? -StruveL(n, -x) : StruveL(n, -x);
            }

            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveL function, n greater than 8 and negative integer are not supported."
                );
            }

            if (IsNaN(x)) {
                return NaN;
            }

            if (x < Epsilon) {
                return 0d;
            }

            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 1d) {
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 32);
            }

            return StruveMIntegral.Value(n, x) + BesselI(n, x);
        }

        public static ddouble StruveM(int n, ddouble x) {
            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveM function, n greater than 8 and negative integer are not supported."
                );
            }

            if (IsNegative(x) || IsNaN(x)) {
                return NaN;
            }

            if (x < Epsilon) {
                return (n >= 1) ? 0d : -1d;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return -0d;
                }
                if (n == 1) {
                    return -Ldexp(RcpPI, 1);
                }

                return PositiveInfinity;
            }

            if (x < 1d) {
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 32) - BesselI(n, x);
            }

            return StruveMIntegral.Value(n, x);
        }

        internal static class StruveGTable {
            private static readonly List<ddouble> table = new();

            static StruveGTable() {
                table.Add(1);
            }

            public static ddouble Value(int k) {
                if (k < table.Count) {
                    return table[k];
                }

                for (int i = table.Count; i <= k; i++) {
                    ddouble g = table[^1] * 2 / (2 * i - 1);

                    table.Add(g);
                }

                return table[k];
            }
        }

        internal static class StruveKIntegral {
            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> PointTable;

            static StruveKIntegral() {
                PointTable =
                    ResourceUnpack.NumTableX2(Resource.StruveIntegralTable)["StruveKTable"];
            }

            public static ddouble Value(int n, ddouble x) {
                ddouble r = 1d / x, s = 0d;

                foreach ((ddouble u, ddouble w) in PointTable) {
                    ddouble v = Pow(Sqrt(1d + Square(u * r)), 2 * n - 1);

                    s += w * v;
                }

                ddouble y = s * Pow(Ldexp(x, -1), n - 1) * RcpPI * StruveGTable.Value(n);

                return y;
            }
        }

        internal static class StruveMIntegral {
            private static readonly ReadOnlyCollection<(ddouble x, ddouble w)> PointTable;

            static StruveMIntegral() {
                PointTable =
                    ResourceUnpack.NumTableX2(Resource.StruveIntegralTable)["StruveMTable"];
            }

            public static ddouble Value(int n, ddouble x) {
                ddouble divs = Ceiling(Ldexp(x, -4));
                ddouble q = Rcp(divs);

                ddouble s = 0d;

                bool convergenced = false;

                for (double i = 0; i < divs && i < 8 && !convergenced; i++) {

                    foreach ((ddouble u, ddouble w) in PointTable) {
                        ddouble u_sft = Ldexp((i + u) * q, -1);

                        ddouble v = Exp(-x * SinPI(u_sft)) * Pow(Square(CosPI(u_sft)), n);

                        if (v.hi < s.hi * 1e-31) {
                            convergenced = true;
                            break;
                        }

                        s += w * v;
                    }
                }

                ddouble y = -s * Pow(Ldexp(x, -1), n) * StruveGTable.Value(n) * q;

                return y;
            }
        }

        internal static class StruveHLNearZero {
            public static ddouble Value(int n, ddouble x, bool sign_switch, int terms) {
                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = 0d, u = Pow(Ldexp(x, -1), n + 1) * RcpPI;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble w = x2 * FTable.Value(2 * k) * FTable.Value(2 * k + n);
                    ddouble ds = Ldexp(u * StruveGTable.Value(2 * k + 1) * StruveGTable.Value(2 * k + n + 1), -4 * k)
                                  * (sign_switch ? (1d - w) : (1d + w));

                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    s = s_next;
                    u *= x4;
                }

                return s;
            }

            public static class FTable {
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int k) {
                    if (k < table.Count) {
                        return table[k];
                    }

                    for (int i = table.Count; i <= k; i++) {
                        ddouble f = Rcp(2 * i + 3);

                        table.Add(f);
                    }

                    return table[k];
                }
            }
        }
    }
}