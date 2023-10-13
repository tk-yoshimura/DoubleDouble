using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble FresnelC(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -FresnelC(-x);
            }

            if (x <= FresnelPade.PadeApproxMin) {
                return FresnelNearZero.FresnelC(x);
            }
            else {
                ddouble f, g;

                if (x <= FresnelPade.PadeApproxMax) {
                    (f, g) = FresnelPade.Coef(x);
                }
                else if (x <= double.ScaleB(1, 256)) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return Point5;
                }

                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPI(theta), sin = SinPI(theta);

                ddouble c = Point5 + sin * f - cos * g;

                return c;
            }
        }

        public static ddouble FresnelS(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -FresnelS(-x);
            }

            if (x <= FresnelPade.PadeApproxMin) {
                return FresnelNearZero.FresnelS(x);
            }
            else {
                ddouble f, g;

                if (x <= FresnelPade.PadeApproxMax) {
                    (f, g) = FresnelPade.Coef(x);
                }
                else if (x <= double.ScaleB(1, 256)) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return Point5;
                }

                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPI(theta), sin = SinPI(theta);

                ddouble s = Point5 - cos * f - sin * g;

                return s;
            }
        }

        private static class FresnelNearZero {
            public static ddouble FresnelC(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * PI;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0d, u = x;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k], -4 * k);
                    (ddouble p, ddouble q) = CRcpTable.Value(k);
                    ddouble ds = f * (p - v2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= v4;
                    s = s_next;
                }

                return s;
            }

            public static ddouble FresnelS(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * PI;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0d, u = Ldexp(v * x, -1);

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k + 1], -4 * k);
                    (ddouble p, ddouble q) = SRcpTable.Value(k);
                    ddouble ds = f * (p - v2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= v4;
                    s = s_next;
                }

                return s;
            }

            private static class CRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp((8 * m + 1));
                        ddouble q = Rcp((4 * (8 * m + 5) * (4 * m + 1) * (4 * m + 2)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }

            private static class SRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp((8 * m + 3));
                        ddouble q = Rcp((4 * (8 * m + 7) * (4 * m + 2) * (4 * m + 3)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }
        };

        private static class FresnelLimit {

            public static (ddouble p, ddouble q) Coef(ddouble x, int max_terms = 10) {
                ddouble v = Rcp(x * x * PI);
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0d, q = 0d;
                ddouble a = Rcp(x * PI);
                ddouble b = Rcp(x * x * x * PI * PI);

                for (int k = 0; k < max_terms; k++) {
                    ddouble s = ((8 * k + 1) * (8 * k + 3)) * v2;
                    ddouble t = ((8 * k + 3) * (8 * k + 5)) * v2;

                    if (s > 1d || t > 1d) {
                        return (NaN, NaN);
                    }

                    ddouble dp = a * (1d - s) * RSeries.Value(4 * k);
                    ddouble dq = b * (1d - t) * RSeries.Value(4 * k + 1);
                    ddouble p_next = dp + p, q_next = dq + q;

                    if (p == p_next && q == q_next) {
                        break;
                    }

                    a *= v4;
                    b *= v4;
                    p = p_next;
                    q = q_next;
                }

                return (p, q);
            }

            private static class RSeries {
                private static BigInteger v;
                private static readonly List<ddouble> table;

                static RSeries() {
                    v = 1;
                    table = new List<ddouble>() { 1d };
                }

                public static ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        v *= 2 * m - 1;
                        table.Add(v);
                    }

                    return table[n];
                }
            }
        };

        private static class FresnelPade {
            public static readonly ddouble PadeApproxMin = 0.85d, PadeApproxMax = 16d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static FresnelPade() {
                Dictionary<string, ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> tables =
                    ResourceUnpack.NumTableX4(Resource.FresnelTable, reverse: true);

                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    tables["PadeX0p0Table"],
                    tables["PadeX0p5Table"],
                    tables["PadeX1p0Table"],
                    tables["PadeX1p5Table"],
                    tables["PadeX2p0Table"],
                    tables["PadeX2p5Table"],
                    tables["PadeX3p0Table"],
                    tables["PadeX3p5Table"],
                    tables["PadeX4p0Table"]
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                ddouble v = Log2(x);

                int table_index = (int)Round(Ldexp(v, 1));
                ddouble w = v - table_index * 0.5d;
                ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> table = PadeTables[table_index];

                (ddouble sfc, ddouble sfd, ddouble sgc, ddouble sgd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble fc, ddouble fd, ddouble gc, ddouble gd) = table[i];

                    sfc = sfc * w + fc;
                    sfd = sfd * w + fd;
                    sgc = sgc * w + gc;
                    sgd = sgd * w + gd;
                }

#if DEBUG
                Trace.Assert(sfd > 0.0625d, $"[Fresnel x={x}] Too small pade denom!!");
                Trace.Assert(sgd > 0.0625d, $"[Fresnel x={x}] Too small pade denom!!");
#endif

                ddouble f = sfc / sfd, g = sgc / sgd;

                (f, g) = (Pow2(f), Pow2(g));

                return (f, g);
            }
        }
    }
}