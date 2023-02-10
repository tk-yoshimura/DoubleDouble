﻿using DoubleDouble.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Ci(ddouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
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
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return Zero;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble c = sin * f - cos * g;

                return c;
            }
        }

        public static ddouble Si(ddouble x, bool limit_zero = false) {
            if (x.Sign < 0) {
                return -Si(-x, limit_zero) - (limit_zero ? PI : Zero);
            }
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= CiSiPade.PadeApproxMin) {
                return CiSiNearZero.Si(x, limit_zero);
            }
            else {
                ddouble f, g;

                if (x <= CiSiPade.PadeApproxMax) {
                    (f, g) = CiSiPade.Coef(x);
                }
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return limit_zero ? Zero : PI / 2;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble s = (limit_zero ? Zero : PI / 2) - cos * f - sin * g;

                return s;
            }
        }

        public static ddouble Shi(ddouble x) {
            if (x.Sign < 0) {
                return -Shi(-x);
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = (Ein(x) - Ein(-x)) / 2;

            return y;
        }

        public static ddouble Chi(ddouble x) {
            if (x.Sign < 0) {
                return NaN;
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = EulerGamma + Log(x) - (Ein(x) + Ein(-x)) / 2;

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
                    ddouble f = u * TaylorSequence[4 * k + 2];
                    (ddouble p, ddouble q) = CRcpTable.Value(k);
                    ddouble ds = f * (p - x2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x4;
                    s = s_next;
                }

                return s;
            }

            public static ddouble Si(ddouble x, bool limit_zero, int max_terms = 7) {
                if (IsZero(x)) {
                    return limit_zero ? -PI / 2 : Zero;
                }

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = limit_zero ? -PI / 2 : Zero, u = x;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = u * TaylorSequence[4 * k + 1];
                    (ddouble p, ddouble q) = SRcpTable.Value(k);
                    ddouble ds = f * (p - x2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x4;
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
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

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

                ddouble p = 0, q = 0;
                ddouble c = v;
                ddouble d = v2;
                ddouble t = 1;

                for (int k = 0; k < max_terms; k++) {
                    ddouble dp = t * c * (1d - v2 * ((4 * k + 1) * (4 * k + 2)));
                    ddouble dq = t * d * (1d - v2 * ((4 * k + 2) * (4 * k + 3))) * (4 * k + 1);

                    ddouble p_next = dp + p, q_next = dq + q;

                    if (p == p_next && q == q_next) {
                        break;
                    }

                    c *= v4;
                    d *= v4;
                    p = p_next;
                    q = q_next;
                    t *= ((4 * k + 1) * (4 * k + 2) * (4 * k + 3) * (4 * k + 4));
                }

                return (p, q);
            }
        };

        private static class CiSiPade {
            public static readonly ddouble PadeApproxMin = 0.71d, PadeApproxMax = 256d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static CiSiPade() {
                Dictionary<string, ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> tables = ResourceUnpack.NumTableX4(Resource.CiSiTable);

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
                ddouble v = Log2(x);

                int table_index = (int)Round(v);
                ddouble w = v - table_index;
                ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> table = PadeTables[table_index];

                (ddouble sfc, ddouble sfd, ddouble sgc, ddouble sgd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble fc, ddouble fd, ddouble gc, ddouble gd) = table[i];

                    sfc = sfc * w + fc;
                    sfd = sfd * w + fd;
                    sgc = sgc * w + gc;
                    sgd = sgd * w + gd;
                }

                ddouble f = sfc / sfd, g = sgc / sgd;

                (f, g) = (Pow2(f), Pow2(g));

                return (f, g);
            }
        }
    }
}