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
                else if (x <= FresnelPade.ExtremelyLarge) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return 0.5d;
                }

                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPi(theta), sin = SinPi(theta);

                ddouble c = 0.5d + sin * f - cos * g;

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
                else if (x <= FresnelPade.ExtremelyLarge) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return 0.5d;
                }

                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPi(theta), sin = SinPi(theta);

                ddouble s = 0.5d - cos * f - sin * g;

                return s;
            }
        }

        public static ddouble FresnelF(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (x <= FresnelPade.PadeApproxMin) {
                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPi(theta), sin = SinPi(theta);

                ddouble c = FresnelC(x);
                ddouble s = FresnelS(x);

                ddouble f = (0.5d - s) * cos - (0.5d - c) * sin;

                return f;
            }

            if (x <= FresnelPade.PadeApproxMax) {
                return FresnelPade.FCoef(x);
            }
            else {
                return FresnelLimit.FCoef(x);
            }
        }

        public static ddouble FresnelG(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (x <= FresnelPade.PadeApproxMin) {
                ddouble theta = Ldexp(x * x, -1);
                ddouble cos = CosPi(theta), sin = SinPi(theta);

                ddouble c = FresnelC(x);
                ddouble s = FresnelS(x);

                ddouble f = (0.5d - c) * cos + (0.5d - s) * sin;

                return f;
            }

            if (x <= FresnelPade.PadeApproxMax) {
                return FresnelPade.GCoef(x);
            }
            else {
                return FresnelLimit.GCoef(x);
            }
        }

        private static class FresnelNearZero {
            public static ddouble FresnelC(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * Pi;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0d, u = x;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k], -4 * k);
                    (ddouble p, ddouble q) = CRcpTable.Value(k);
                    s = SeriesUtil.Add(s, f, p, -v2 * q, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= v4;
                }

                return s;
            }

            public static ddouble FresnelS(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * Pi;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0d, u = Ldexp(v * x, -1);

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k + 1], -4 * k);
                    (ddouble p, ddouble q) = SRcpTable.Value(k);
                    s = SeriesUtil.Add(s, f, p, -v2 * q, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    u *= v4;
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

                    lock (table) {
                        for (long m = table.Count; m <= n; m++) {
                            ddouble p = Rcp((8 * m + 1));
                            ddouble q = Rcp((4 * (8 * m + 5) * (4 * m + 1) * (4 * m + 2)));

                            table.Add((p, q));
                        }

                        return table[n];
                    }
                }
            }

            private static class SRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    lock (table) {
                        for (long m = table.Count; m <= n; m++) {
                            ddouble p = Rcp((8 * m + 3));
                            ddouble q = Rcp((4 * (8 * m + 7) * (4 * m + 2) * (4 * m + 3)));

                            table.Add((p, q));
                        }

                        return table[n];
                    }
                }
            }
        };

        private static class FresnelLimit {

            public static (ddouble p, ddouble q) Coef(ddouble x, int max_terms = 10) {
                ddouble v = Rcp(x * x * Pi);
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0d, q = 0d;
                ddouble a = Rcp(x * Pi);
                ddouble b = Rcp(x * x * x * Pi * Pi);

                for (int k = 0; k < max_terms; k++) {
                    ddouble s = ((8 * k + 1) * (8 * k + 3)) * v2;
                    ddouble t = ((8 * k + 3) * (8 * k + 5)) * v2;

                    if (s > 1d || t > 1d) {
                        return (NaN, NaN);
                    }

                    p = SeriesUtil.Add(p, a * RSeries.Value(4 * k), 1d, -s, out bool convergence_p);
                    q = SeriesUtil.Add(q, b * RSeries.Value(4 * k + 1), 1d, -t, out bool convergence_q);

                    if (convergence_p && convergence_q) {
                        break;
                    }

                    a *= v4;
                    b *= v4;
                }

                return (p, q);
            }

            public static ddouble FCoef(ddouble x, int max_terms = 10) {
                ddouble v = Rcp(x * x * Pi);
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0d;
                ddouble a = Rcp(x * Pi);

                for (int k = 0; k < max_terms; k++) {
                    ddouble s = ((8 * k + 1) * (8 * k + 3)) * v2;

                    if (s > 1d) {
                        return NaN;
                    }

                    p = SeriesUtil.Add(p, a * RSeries.Value(4 * k), 1d, -s, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    a *= v4;
                }

                return p;
            }

            public static ddouble GCoef(ddouble x, int max_terms = 10) {
                ddouble v = Rcp(x * x * Pi);
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble q = 0d;
                ddouble b = Rcp(x * x * x * Pi * Pi);

                for (int k = 0; k < max_terms; k++) {
                    ddouble t = ((8 * k + 3) * (8 * k + 5)) * v2;

                    if (t > 1d) {
                        return NaN;
                    }

                    q = SeriesUtil.Add(q, b * RSeries.Value(4 * k + 1), 1d, -t, out bool convergence);

                    if (convergence) {
                        break;
                    }

                    b *= v4;
                }

                return q;
            }

            private static class RSeries {
                private static BigInteger v;
                private static readonly List<ddouble> table;

                static RSeries() {
                    v = 1;
                    table = new List<ddouble>() { 1d };
                }

                public static ddouble Value(int n) {
                    Debug.Assert(n >= 0);

                    if (n < table.Count) {
                        return table[n];
                    }

                    lock (table) {
                        for (int m = table.Count; m <= n; m++) {
                            v *= 2 * m - 1;
                            table.Add(v);
                        }

                        return table[n];
                    }
                }
            }
        };

        private static class FresnelPade {
            const int max_exponent = 4;
            public const double PadeApproxMin = 0.85d, PadeApproxMax = 32d;
            public static readonly double ExtremelyLarge = double.ScaleB(1, 256);
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeFTables, PadeGTables;

            static FresnelPade() {
                Dictionary<string, ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> tables =
                    ResourceUnpack.NumTableX4(Resource.FresnelTable, reverse: true);

                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    tables["PadeX0p5Table"],
                    tables["PadeX1Table"  ],
                    tables["PadeX2Table"  ],
                    tables["PadeX4Table"  ],
                    tables["PadeX8Table"  ],
                    tables["PadeX16Table" ],
                });

                PadeFTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX0p5Table"].Select(v => (v.fc, v.fd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX1Table"  ].Select(v => (v.fc, v.fd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX2Table"  ].Select(v => (v.fc, v.fd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX4Table"  ].Select(v => (v.fc, v.fd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX8Table"  ].Select(v => (v.fc, v.fd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX16Table" ].Select(v => (v.fc, v.fd)).ToArray()),
                });

                PadeGTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX0p5Table"].Select(v => (v.gc, v.gd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX1Table"  ].Select(v => (v.gc, v.gd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX2Table"  ].Select(v => (v.gc, v.gd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX4Table"  ].Select(v => (v.gc, v.gd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX8Table"  ].Select(v => (v.gc, v.gd)).ToArray()),
                    new ReadOnlyCollection<(ddouble c, ddouble d)>(tables["PadeX16Table" ].Select(v => (v.gc, v.gd)).ToArray()),
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                int exponent = int.Clamp(ILogB(x), -1, max_exponent), table_index = exponent + 1;
                ddouble w = x - double.ScaleB(1d, exponent);
                ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> table = PadeTables[table_index];

                (ddouble sfc, ddouble sfd, ddouble sgc, ddouble sgd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble fc, ddouble fd, ddouble gc, ddouble gd) = table[i];

                    sfc = sfc * w + fc;
                    sfd = sfd * w + fd;
                    sgc = sgc * w + gc;
                    sgd = sgd * w + gd;
                }

                Debug.Assert(sfd > 0.5d, $"[Fresnel x={x}] Too small pade denom!!");
                Debug.Assert(sgd > 0.5d, $"[Fresnel x={x}] Too small pade denom!!");

                ddouble f = sfc / (sfd * x), g = sgc / (sgd * x * x * x);

                return (f, g);
            }

            public static ddouble FCoef(ddouble x) {
                int exponent = int.Clamp(ILogB(x), -1, max_exponent), table_index = exponent + 1;
                ddouble w = x - double.ScaleB(1d, exponent);
                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeFTables[table_index];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Fresnel x={x}] Too small pade denom!!");

                ddouble f = sc / (sd * x);

                return f;
            }

            public static ddouble GCoef(ddouble x) {
                int exponent = int.Clamp(ILogB(x), -1, max_exponent), table_index = exponent + 1;
                ddouble w = x - double.ScaleB(1d, exponent);
                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeGTables[table_index];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                Debug.Assert(sd > 0.5d, $"[Fresnel x={x}] Too small pade denom!!");

                ddouble g = sc / (sd * x * x * x);

                return g;
            }
        }
    }
}