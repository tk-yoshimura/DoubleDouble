using DoubleDouble.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble MathieuA(int n, ddouble q) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > 16) {
                throw new ArgumentException(
                    "In the calculation of the MathieuA function, n greater than 16 is not supported."
                );
            }

            if (q < 0d) {
                return ((n & 1) == 0) ? MathieuA(n, -q) : MathieuB(n, -q);
            }

            if (IsNaN(q)) {
                return NaN;
            }
            if (IsInfinity(q)) {
                return NegativeInfinity;
            }

            if (q < Mathieu.NearZeroLimit(n)) {
                ddouble m = Mathieu.MPade(n, q), d = Mathieu.DPade(n, q);

                return m + d;
            }
            else {
                return Mathieu.APade(n, q);
            }
        }

        public static ddouble MathieuB(int n, ddouble q) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n == 0) {
                return MathieuA(n, q);
            }
            if (n > 16) {
                throw new ArgumentException(
                    "In the calculation of the MathieuB function, n greater than 16 is not supported."
                );
            }

            if (q < 0d) {
                return ((n & 1) == 0) ? MathieuB(n, -q) : MathieuA(n, -q);
            }

            if (IsNaN(q)) {
                return NaN;
            }
            if (IsInfinity(q)) {
                return NegativeInfinity;
            }

            if (q <= Mathieu.NearZeroLimit(n)) {
                ddouble m = Mathieu.MPade(n, q), d = Mathieu.DPade(n, q);

                return m - d;
            }
            else {
                return Mathieu.BPade(n, q);
            }
        }

        public static ddouble MathieuC(int n, ddouble q, ddouble x) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > 16) {
                throw new ArgumentException(
                    "In the calculation of the MathieuC function, n greater than 16 is not supported."
                );
            }

            ReadOnlyCollection<ddouble> coef = Mathieu.CCoef(n, q);

            if (coef.Count < 1) {
                return NaN;
            }

            ddouble s = Zero;

            if ((n & 1) == 0) {
                s += coef[0];
                for (int m = 1; m < coef.Count; m++) {
                    s += coef[m] * Cos((2 * m) * x);
                }
            }
            else {
                for (int m = 0; m < coef.Count; m++) {
                    s += coef[m] * Cos((2 * m + 1) * x);
                }
            }

            return s;
        }

        public static ddouble MathieuS(int n, ddouble q, ddouble x) {
            if (n < 1) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > 16) {
                throw new ArgumentException(
                    "In the calculation of the MathieuS function, n greater than 16 is not supported."
                );
            }

            ReadOnlyCollection<ddouble> coef = Mathieu.SCoef(n, q);

            if (coef.Count < 1) {
                return NaN;
            }

            ddouble s = Zero;

            if ((n & 1) == 1) {
                for (int m = 0; m < coef.Count; m++) {
                    s += coef[m] * Sin((2 * m + 1) * x);
                }
            }
            else {
                for (int m = 0; m < coef.Count; m++) {
                    s += coef[m] * Sin((2 * m + 2) * x);
                }
            }

            return s;
        }

        internal static class Mathieu {
            public static readonly double Eps = Math.ScaleB(1, -46);
            public static double NearZeroLimit(int n) => 64 * Math.Max(1, n * n);
            private static readonly Dictionary<(int n, ddouble q), ReadOnlyCollection<ddouble>> c_coef_cache = new(), s_coef_cache = new();

            public static ddouble MPade(int n, ddouble q) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }

                ddouble u = Square((n <= 1) ? q : q / (n * n));

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = Consts.Mathieu.PadeMTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeMDParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");
#endif

                ddouble y = sc / sd;

                if (n >= 2) {
                    y *= n * n;
                }

                return y;
            }

            public static ddouble DPade(int n, ddouble q) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }

                if (n < 1) {
                    return 0d;
                }

                ddouble u = Square((n <= 1) ? q : q / (n * n));

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = Consts.Mathieu.PadeDTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeMDParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");
#endif

                ddouble y = sc / sd;

                y *= Ldexp(Pow(q, n) / Square(Factorial[n - 1]), 2 - 2 * n);

                return y;
            }

            private static (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) PadeMDParam(ddouble u, ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table) {
                return
                    (u <= 0.125d) ? (0d, pade_table[0]) :
                    (u <= 0.250d) ? (0.1875d, pade_table[1]) :
                    (u <= 0.375d) ? (0.3125d, pade_table[2]) :
                    (u <= 0.50d) ? (0.4375d, pade_table[3]) :
                    (u <= 0.75d) ? (0.50d, pade_table[4]) :
                    (u <= 1d) ? (0.75d, pade_table[5]) :
                    (u <= 4d) ? (1d, pade_table[6]) :
                    (u <= 8d) ? (4d, pade_table[7]) :
                    (u <= 16d) ? (8d, pade_table[8]) :
                    (u <= 64d) ? (16d, pade_table[9]) :
                    (u <= 512d) ? (64d, pade_table[10]) :
                    (u <= 4096d) ? (512d, pade_table[11]) :
                    throw new NotImplementedException();
            }

            public static ddouble APade(int n, ddouble q) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }

                int s = 2 * n + 1;
                ddouble h = Sqrt(q), invh = 1d / h, u = invh * Math.Max(1, n);

                ReadOnlyCollection<ddouble> limit_coef = Consts.Mathieu.LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2 * (s * h - q) - asymp;

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = Consts.Mathieu.PadeATables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeABParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");
#endif

                ddouble delta = sc / sd;
                if (IsZero(u0)) {
                    delta *= u;
                }

                ddouble y = asymp - delta;

                return y;
            }

            public static ddouble BPade(int n, ddouble q) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }

                int s = 2 * n - 1;
                ddouble h = Sqrt(q), invh = 1d / h, u = invh * Math.Max(1, n);

                ReadOnlyCollection<ddouble> limit_coef = Consts.Mathieu.LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2 * (s * h - q) - asymp;

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = Consts.Mathieu.PadeBTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeABParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");
#endif

                ddouble delta = sc / sd;
                if (IsZero(u0)) {
                    delta *= u;
                }

                ddouble y = asymp - delta;

                return y;
            }

            private static (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) PadeABParam(ddouble u, ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table) {
                return
                    (u <= 0.03125d) ? (0d, pade_table[0]) :
                    (u <= 0.0625d) ? (0.046875d, pade_table[1]) :
                    (u <= 0.125d) ? (0.09375d, pade_table[2]) :
                    throw new NotImplementedException();
            }

            public static ReadOnlyCollection<ddouble> CCoef(int n, ddouble q) {
                if (!c_coef_cache.ContainsKey((n, q))) {
                    ReadOnlyCollection<ddouble> coef;

                    if (q.Sign >= 0) {
                        ddouble a = MathieuA(n, q);
                        coef = GenerateCCoef(n, q, a);
                    }
                    else {
                        if ((n & 1) == 0) {
                            ddouble a = MathieuA(n, -q);
                            coef = SwapSign(GenerateCCoef(n, -q, a), even_odd: 1);
                        }
                        else {
                            ddouble a = MathieuB(n, -q);
                            coef = SwapSign(GenerateSCoef(n, -q, a), even_odd: 0);
                        }
                    }

                    c_coef_cache.Add((n, q), coef);
                }

                return c_coef_cache[(n, q)];
            }

            public static ReadOnlyCollection<ddouble> SCoef(int n, ddouble q) {
                if (!s_coef_cache.ContainsKey((n, q))) {
                    ReadOnlyCollection<ddouble> coef;

                    if (q.Sign >= 0) {
                        ddouble a = MathieuB(n, q);
                        coef = GenerateSCoef(n, q, a);
                    }
                    else {
                        if ((n & 1) == 0) {
                            ddouble a = MathieuB(n, -q);
                            coef = SwapSign(GenerateSCoef(n, -q, a), even_odd: 1);
                        }
                        else {
                            ddouble a = MathieuA(n, -q);
                            coef = SwapSign(GenerateCCoef(n, -q, a), even_odd: 0);
                        }
                    }

                    s_coef_cache.Add((n, q), coef);
                }

                return s_coef_cache[(n, q)];
            }

            private static ReadOnlyCollection<ddouble> SwapSign(ReadOnlyCollection<ddouble> coef, int even_odd) {
                ddouble[] coef_swaped = coef.ToArray();

                for (int m = even_odd; m < coef_swaped.Length; m += 2) {
                    coef_swaped[m] = -coef_swaped[m];
                }

                return Array.AsReadOnly(coef_swaped);
            }

            internal static ReadOnlyCollection<ddouble> GenerateCCoef(int n, ddouble q, ddouble a, int terms = -1) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }
                if (q < Eps) {
                    return QNearZeroCCoef(n, q);
                }

                terms = (terms > 0) ? terms : Math.Max(128, checked((int)(Math.Pow(q.Hi, 0.2475) * (0.100 * n + 12.056))));

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (ddouble.Epsilon, ddouble.Zero);

                for (long m = terms - 2, k = checked(2 * (long)m + (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                    ddouble c = (a - k * k) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (Math.ILogB(cs[m - 1].Hi) > 0) {
                        for (int j = (int)(m - 1); j < cs.Length; j++) {
                            if (Math.ILogB(cs[j - 1].Hi) < -128 && Math.ILogB(cs[j].Hi) < -128 && m >= 32) {
                                cs = cs[..j];
                                cs[^1] = ddouble.Zero;
                                break;
                            }

                            cs[j] = ddouble.Ldexp(cs[j], -128);
                        }
                    }
                }

                int scale = cs.Select(c => Math.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = ddouble.Ldexp(cs[m], -scale);
                }

                for (int m = Math.Min(8, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || ddouble.Abs(cs[m]) > ddouble.Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(a, q, m, 2, 0, 0, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(a, q, m, 1, 1, 1 + q, cs[m]);
                        }

                        if (m == 2 || d < 1) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                if (ddouble.IsInfinity(cs[0]) && ddouble.IsFinite(cs[1])) {
                    return QNearZeroCCoef(n, q);
                }

                cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                if (!ddouble.IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

                return Array.AsReadOnly(cs);
            }

            private static ReadOnlyCollection<ddouble> QNearZeroCCoef(int n, ddouble q) {
                ddouble[] cs_q0 = new ddouble[n / 2 + 1];
                ReadOnlyCollection<ddouble> cs_eps = GenerateCCoef(n, Eps, MathieuA(n, Eps));

                if (n == 0) {
                    cs_q0[0] = Rcp(Sqrt2);
                }
                else {
                    cs_q0[n / 2] = 1d;
                }

                int terms = Math.Max(cs_q0.Length, cs_eps.Count);
                ddouble[] cs = new ddouble[terms];
                ddouble w = q / Eps;

                for (int i = 0; i < cs.Length; i++) {
                    ddouble c_q0 = i < cs_q0.Length ? cs_q0[i] : Zero;
                    ddouble c_eps = i < cs_eps.Count ? cs_eps[i] : Zero;

                    cs[i] = c_q0 + (c_eps - c_q0) * w;
                }

                cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                return Array.AsReadOnly(cs);
            }

            internal static ReadOnlyCollection<ddouble> GenerateSCoef(int n, ddouble q, ddouble a, int terms = -1) {
                if (!(q >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(q));
                }
                if (q < Eps) {
                    return QNearZeroSCoef(n, q);
                }

                terms = (terms > 0) ? terms : Math.Max(128, checked((int)(Math.Pow(q.Hi, 0.2475) * (0.094 * n + 11.830))));

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (ddouble.Epsilon, ddouble.Zero);

                for (long m = terms - 2, k = checked(2 * (long)m + 2 - (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                    ddouble c = (a - k * k) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (Math.ILogB(cs[m - 1].Hi) > 0) {
                        for (int j = (int)(m - 1); j < cs.Length; j++) {
                            if (Math.ILogB(cs[j - 1].Hi) < -128 && Math.ILogB(cs[j].Hi) < -128 && m >= 32) {
                                cs = cs[..j];
                                cs[^1] = ddouble.Zero;
                                break;
                            }

                            cs[j] = ddouble.Ldexp(cs[j], -128);
                        }
                    }
                }

                int scale = cs.Select(c => Math.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = ddouble.Ldexp(cs[m], -scale);
                }

                for (int m = Math.Min(8, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || ddouble.Abs(cs[m]) > ddouble.Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(a, q, m, 1, 2, 4, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(a, q, m, 1, 1, 1 - q, cs[m]);
                        }

                        if (m == 2 || d < 1) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                if (ddouble.IsInfinity(cs[0]) && ddouble.IsFinite(cs[1])) {
                    return QNearZeroSCoef(n, q);
                }

                cs = NormalizeAndTruncateCoef(n, 1, cs);

                if (!ddouble.IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

                return Array.AsReadOnly(cs);
            }

            private static ReadOnlyCollection<ddouble> QNearZeroSCoef(int n, ddouble q) {
                ddouble[] cs_q0 = new ddouble[(n - 1) / 2 + 1];
                ReadOnlyCollection<ddouble> cs_eps = GenerateSCoef(n, Eps, MathieuB(n, Eps));

                cs_q0[(n - 1) / 2] = 1d;

                int terms = Math.Max(cs_q0.Length, cs_eps.Count);
                ddouble[] cs = new ddouble[terms];
                ddouble w = q / Eps;

                for (int i = 0; i < cs.Length; i++) {
                    ddouble c_q0 = i < cs_q0.Length ? cs_q0[i] : Zero;
                    ddouble c_eps = i < cs_eps.Count ? cs_eps[i] : Zero;

                    cs[i] = c_q0 + (c_eps - c_q0) * w;
                }

                cs = NormalizeAndTruncateCoef(n, 1, cs);

                return Array.AsReadOnly(cs);
            }

            public static (ddouble[] cs, ddouble r, ddouble d) SolveCoef(ddouble a, ddouble q, int m, int k, int s, ddouble r0, ddouble cn) {
                if (m < 2) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble[] cs = new ddouble[m], ts = new ddouble[m + 1], qs = new ddouble[m];
                ddouble sq_q = q * q;

                (ts[0], ts[1], ts[2]) = (1d, a - r0, (a - r0) * (a - checked((2 + s) * (2 + s))) - k * q * q);
                (qs[0], qs[1]) = (cn * q, cn * sq_q);

                for (int i = 2; i < m; i++) {
                    qs[i] = qs[i - 1] * q;
                    ts[i + 1] = ts[i] * (a - checked((2 * i + s) * (2 * i + s))) - ts[i - 1] * sq_q;
                }

                for (int i = 0; i < cs.Length; i++) {
                    cs[i] = qs[m - i - 1] * ts[i] / ts[m];
                }

                ddouble d = ddouble.Abs(ts[m - 1] / ts[m]);

                return (cs, ts[m], d);
            }

            private static ddouble[] NormalizeAndTruncateCoef(int n, int k, ddouble[] cs) {
                ddouble norm = (cs[0] * cs[0]) * k;
                for (int i = 1; i < cs.Length; i++) {
                    norm += cs[i] * cs[i];
                }

                ddouble r = 1d / ddouble.Sqrt(norm) * cs[0].Sign;
                for (int i = 0; i < cs.Length; i++) {
                    cs[i] *= r;
                }

                ddouble threshold = ddouble.Ldexp(cs.Select(c => ddouble.Abs(c)).Max(), -128);
                for (int i = cs.Length - 1; i > 0; i--) {
                    if (ddouble.Abs(cs[i]) > threshold) {
                        cs = cs[..(i + 1)];
                        break;
                    }
                }

                return cs;
            }

            internal static partial class Consts {
                public static class Mathieu {
                    public static readonly ReadOnlyCollection<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>>
                        PadeMTables, PadeDTables, PadeATables, PadeBTables;

                    public static readonly ReadOnlyDictionary<int, ReadOnlyCollection<ddouble>> LimitTable;

                    static Mathieu() {
                        {
                            Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                                ResourceUnpack.NumTableX2(Resource.MathieuMTable, reverse: true);

                            List<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>> table = new();

                            for (int n = 0; n <= 16; n++) {
                                List<ReadOnlyCollection<(ddouble c, ddouble d)>> coef = new();

                                for (int i = 0; i < 12; i++) {
                                    coef.Add(tables[$"PadeM{n}Table_{i}"]);
                                }

                                table.Add(Array.AsReadOnly(coef.ToArray()));
                            }

                            PadeMTables = Array.AsReadOnly(table.ToArray());
                        }

                        {
                            Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                                ResourceUnpack.NumTableX2(Resource.MathieuDTable, reverse: true);

                            List<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>> table = new() {
                            Array.AsReadOnly(Enumerable.Empty<ReadOnlyCollection<(ddouble c, ddouble d)>>().ToArray())
                        };

                            for (int n = 1; n <= 16; n++) {
                                List<ReadOnlyCollection<(ddouble c, ddouble d)>> coef = new();

                                for (int i = 0; i < 12; i++) {
                                    coef.Add(tables[$"PadeD{n}Table_{i}"]);
                                }

                                table.Add(Array.AsReadOnly(coef.ToArray()));
                            }

                            PadeDTables = Array.AsReadOnly(table.ToArray());
                        }

                        {
                            Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                                ResourceUnpack.NumTableX2(Resource.MathieuATable, reverse: true);

                            List<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>> table = new();

                            for (int n = 0; n <= 16; n++) {
                                List<ReadOnlyCollection<(ddouble c, ddouble d)>> coef = new();

                                for (int i = 0; i < 3; i++) {
                                    coef.Add(tables[$"PadeA{n}Table_{i}"]);
                                }

                                table.Add(Array.AsReadOnly(coef.ToArray()));
                            }

                            PadeATables = Array.AsReadOnly(table.ToArray());
                        }

                        {
                            Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                                ResourceUnpack.NumTableX2(Resource.MathieuBTable, reverse: true);

                            List<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>> table = new() {
                            Array.AsReadOnly(Enumerable.Empty<ReadOnlyCollection<(ddouble c, ddouble d)>>().ToArray())
                        };

                            for (int n = 1; n <= 16; n++) {
                                List<ReadOnlyCollection<(ddouble c, ddouble d)>> coef = new();

                                for (int i = 0; i < 3; i++) {
                                    coef.Add(tables[$"PadeB{n}Table_{i}"]);
                                }

                                table.Add(Array.AsReadOnly(coef.ToArray()));
                            }

                            PadeBTables = Array.AsReadOnly(table.ToArray());
                        }

                        {
                            Dictionary<int, ReadOnlyCollection<ddouble>> limit_table = new();

                            for (long s = 1; s <= 33; s += 2) {
                                ReadOnlyCollection<ddouble> coef = new(Array.AsReadOnly(new ddouble[] {
                                    ddouble.Ldexp(1 + s * s, -3),
                                    ddouble.Ldexp(s * (3 + s * s), -7),
                                    ddouble.Ldexp(9 + s * s * (34 + s * s * 5), -12),
                                    ddouble.Ldexp(s * (405 + s * s * (410 + s * s * 33)), -17),
                                    ddouble.Ldexp(486 + s * s * (2943 + s * s * (1260 + s * s * 63)), -20),
                                    ddouble.Ldexp(checked(s * (41607 + s * s * (69001 + s * s * (15617 + s * s * 527)))), -25),
                                }.Reverse().ToArray()));

                                limit_table.Add((int)s, coef);
                            }

                            LimitTable = new(limit_table);
                        }
                    }
                }
            }
        }
    }
}