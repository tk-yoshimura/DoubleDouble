using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.SinCos;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Ci(ddouble x) {
            if (IsNegative(x) || IsNaN(x)) {
                return NaN;
            }

            if (x <= CiSiPade.PadeApproxMin) {
                return CiSiNearZero.Ci(x);
            }
            else {
                ddouble f, g;

                if (x <= CiSiPade.PadeApproxMax) {
                    (f, g) = CiSiPade.Coef(x);
                }
                else if (x <= double.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return 0d;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble c = sin * f - cos * g;

                return c;
            }
        }

        public static ddouble Si(ddouble x, bool limit_zero = false) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Si(-x, limit_zero) - (limit_zero ? PI : 0d);
            }

            if (x <= CiSiPade.PadeApproxMin) {
                return CiSiNearZero.Si(x, limit_zero);
            }
            else {
                ddouble f, g;

                if (x <= CiSiPade.PadeApproxMax) {
                    (f, g) = CiSiPade.Coef(x);
                }
                else if (x <= double.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return limit_zero ? 0d : PIHalf;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble s = (limit_zero ? 0d : PIHalf) - cos * f - sin * g;

                return s;
            }
        }

        public static ddouble Shi(ddouble x) {
            if (IsNegative(x)) {
                return -Shi(-x);
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = Ldexp(Ein(x) - Ein(-x), -1);

            return y;
        }

        public static ddouble Chi(ddouble x) {
            if (IsNegative(x)) {
                return NaN;
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = EulerGamma + Log(x) - Ldexp(Ein(x) + Ein(-x), -1);

            return y;
        }

        private static class CiSiNearZero {
            public static ddouble Ci(ddouble x, int max_terms = 7) {
                if (IsZero(x)) {
                    return NegativeInfinity;
                }

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = EulerGamma + Log(x), u = -x2;

                for (int k = 0; k < max_terms; k++) {
                    (ddouble p, ddouble q) = CRcpTable.Value(k);

                    s = SeriesUtil.Add(s, u * TaylorSequence[4 * k + 2], p, -x2 * q, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= x4;
                }

                return s;
            }

            public static ddouble Si(ddouble x, bool limit_zero, int max_terms = 7) {
                if (IsZero(x)) {
                    return limit_zero ? -PIHalf : 0d;
                }

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = limit_zero ? -PIHalf : 0d, u = x;

                for (int k = 0; k < max_terms; k++) {
                    (ddouble p, ddouble q) = SRcpTable.Value(k);

                    s = SeriesUtil.Add(s, u * TaylorSequence[4 * k + 1], p, -x2 * q, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= x4;
                }

                return s;
            }

            private static class CRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp((4 * m + 2));
                        ddouble q = Rcp(((4 * m + 3) * (4 * m + 4) * (4 * m + 4)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }

            private static class SRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp((4 * m + 1));
                        ddouble q = Rcp(((4 * m + 2) * (4 * m + 3) * (4 * m + 3)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }
        };

        private static class CiSiLimit {

            public static (ddouble p, ddouble q) Coef(ddouble x, int max_terms = 7) {
                ddouble v = 1d / x;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0d, q = 0d;
                ddouble c = v;
                ddouble d = v2;
                ddouble t = 1d;

                for (int k = 0; k < max_terms; k++) {
                    p = SeriesUtil.Add(p, t * c, 1d, -v2 * ((4 * k + 1) * (4 * k + 2)), out bool convergence_p);
                    q = SeriesUtil.Add(q, t * d * (4 * k + 1), 1d, -v2 * ((4 * k + 2) * (4 * k + 3)), out bool convergence_q);

                    if (convergence_p && convergence_q) {
                        break;
                    }

                    c *= v4;
                    d *= v4;
                    t *= ((4 * k + 1) * (4 * k + 2) * (4 * k + 3) * (4 * k + 4));
                }

                return (p, q);
            }
        };

        private static class CiSiPade {
            public const double PadeApproxMin = 1d, PadeApproxMax = 511d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static CiSiPade() {
                Dictionary<string, ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> tables =
                    ResourceUnpack.NumTableX4(Resource.CiSiTable, reverse: true);

                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    tables["PadeX0Table"],
                    tables["PadeX1Table"],
                    tables["PadeX2Table"],
                    tables["PadeX3Table"],
                    tables["PadeX4Table"],
                    tables["PadeX5Table"],
                    tables["PadeX6Table"],
                    tables["PadeX7Table"],
                    tables["PadeX8Table"],
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                Debug.Assert(x >= PadeApproxMin && x <= PadeApproxMax);

                int table_index = int.Clamp(ILogB(x), 0, PadeTables.Count - 1);
                
                ddouble w = x - double.ScaleB(1d, table_index);
                ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> table = PadeTables[table_index];

                (ddouble sfc, ddouble sfd, ddouble sgc, ddouble sgd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble fc, ddouble fd, ddouble gc, ddouble gd) = table[i];

                    sfc = sfc * w + fc;
                    sfd = sfd * w + fd;
                    sgc = sgc * w + gc;
                    sgd = sgd * w + gd;
                }

                Debug.Assert(sfd > 0.5d, $"[CiSi x={x}] Too small pade denom!!");
                Debug.Assert(sgd > 0.5d, $"[CiSi x={x}] Too small pade denom!!");

                ddouble f = sfc / (sfd * x), g = sgc / (sgd * x * x);

                return (f, g);
            }
        }
    }
}