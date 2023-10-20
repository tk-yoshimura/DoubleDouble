using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Mathieu;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble MathieuA(int n, ddouble q) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentException(
                    $"In the calculation of the MathieuA function, n greater than {MathieuUtil.MaxN} is not supported."
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

            if (q < MathieuUtil.NearZeroLimit(n)) {
                ddouble m = MathieuUtil.MPade(n, q), d = MathieuUtil.DPade(n, q);

                return m + d;
            }
            else {
                return MathieuUtil.APade(n, q);
            }
        }

        public static ddouble MathieuB(int n, ddouble q) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n == 0) {
                return MathieuA(n, q);
            }
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentException(
                    $"In the calculation of the MathieuB function, n greater than {MathieuUtil.MaxN} is not supported."
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

            if (q <= MathieuUtil.NearZeroLimit(n)) {
                ddouble m = MathieuUtil.MPade(n, q), d = MathieuUtil.DPade(n, q);

                return m - d;
            }
            else {
                return MathieuUtil.BPade(n, q);
            }
        }

        public static ddouble MathieuC(int n, ddouble q, ddouble x) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentException(
                    $"In the calculation of the MathieuC function, n greater than {MathieuUtil.MaxN} is not supported."
                );
            }

            ReadOnlyCollection<ddouble> coef = MathieuUtil.CCoef(n, q);

            if (coef.Count < 1) {
                return NaN;
            }

            ddouble s = 0d;

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
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentException(
                    $"In the calculation of the MathieuS function, n greater than {MathieuUtil.MaxN} is not supported."
                );
            }

            ReadOnlyCollection<ddouble> coef = MathieuUtil.SCoef(n, q);

            if (coef.Count < 1) {
                return NaN;
            }

            ddouble s = 0d;

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

        internal static class MathieuUtil {
            public const int MaxN = 16;
            public static readonly double Eps = double.ScaleB(1, -50);
            public const double NZThreshold = 0.25;

            public static double NearZeroLimit(int n) => 64 * int.Max(1, n * n);
            private static readonly Dictionary<(int n, ddouble q), ReadOnlyCollection<ddouble>> c_coef_cache = new(), s_coef_cache = new();

            public static ddouble MPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                ddouble u = Square((n <= 1) ? q : q / (n * n));

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = PadeMTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeMDParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble y = sc / sd;

                if (n >= 2) {
                    y *= n * n;
                }

                return y;
            }

            public static ddouble DPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                if (n < 1) {
                    return 0d;
                }

                ddouble u = Square((n <= 1) ? q : q / (n * n));

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = PadeDTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeMDParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

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
                Debug.Assert(q >= 0d, nameof(q));

                int s = 2 * n + 1;
                ddouble h = Sqrt(q), invh = 1d / h, u = invh * int.Max(1, n);

                ReadOnlyCollection<ddouble> limit_coef = LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2 * (s * h - q) - asymp;

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = PadeATables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeABParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble delta = sc / sd;
                if (IsZero(u0)) {
                    delta *= u;
                }

                ddouble y = asymp - delta;

                return y;
            }

            public static ddouble BPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                int s = 2 * n - 1;
                ddouble h = Sqrt(q), invh = 1d / h, u = invh * int.Max(1, n);

                ReadOnlyCollection<ddouble> limit_coef = LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2 * (s * h - q) - asymp;

                ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> pade_table = PadeBTables[n];
                (ddouble u0, ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef) = PadeABParam(u, pade_table);

                ddouble v = u - u0;

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

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

                    if (IsPositive(q)) {
                        ddouble a = MathieuA(n, q);
                        coef = GenerateCCoef(n, q, a);
                    }
                    else {
                        if ((n & 1) == 0) {
                            ddouble a = MathieuA(n, -q);
                            coef = SwapSign(GenerateCCoef(n, -q, a), even_odd: 1 - ((n / 2) & 1));
                        }
                        else {
                            ddouble b = MathieuB(n, -q);
                            coef = SwapSign(GenerateSCoef(n, -q, b), even_odd: 1 - ((n / 2) & 1));
                        }
                    }

                    c_coef_cache.Add((n, q), coef);
                }

                return c_coef_cache[(n, q)];
            }

            public static ReadOnlyCollection<ddouble> SCoef(int n, ddouble q) {
                if (!s_coef_cache.ContainsKey((n, q))) {
                    ReadOnlyCollection<ddouble> coef;

                    if (IsPositive(q)) {
                        ddouble b = MathieuB(n, q);
                        coef = GenerateSCoef(n, q, b);
                    }
                    else {
                        if ((n & 1) == 0) {
                            ddouble b = MathieuB(n, -q);
                            coef = SwapSign(GenerateSCoef(n, -q, b), even_odd: ((n / 2) & 1));
                        }
                        else {
                            ddouble a = MathieuA(n, -q);
                            coef = SwapSign(GenerateCCoef(n, -q, a), even_odd: 1 - ((n / 2) & 1));
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
                Debug.Assert(q >= 0d, nameof(q));

                if (q < Eps) {
                    return QNearZeroCCoef(n, q);
                }
                if (q <= NZThreshold) {
                    return GenerateCCoefZeroShifted(n, q);
                }

                terms = (terms > 0) ? terms : int.Max(128, checked((int)(double.Pow(q.Hi, 0.2475) * (0.100 * n + 12.056))));

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                    ddouble c = (a - k * k) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (double.ILogB(cs[m - 1].Hi) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }

                int scale = cs.Select(c => double.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(a, q, m, 2, 0, 0, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(a, q, m, 1, 1, 1 + q, cs[m]);
                        }

                        if (m <= 2 + n / 2 || d < 1d) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                if (!IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

                return Array.AsReadOnly(cs);
            }

            private static ReadOnlyCollection<ddouble> QNearZeroCCoef(int n, ddouble q) {
                if (n <= 1) {
                    ddouble t = Ldexp(q, -2), sq_t = t * t;
                    ddouble[] cs = new ddouble[4];
                    cs[0] = 1d;

                    if (n == 0) {
                        cs[1] = -t * (2 + sq_t * -3.5);
                        cs[2] = sq_t * (0.5 + sq_t * -(ddouble)10 / 9);
                        cs[3] = -t * sq_t * ((ddouble)1 / 18 + sq_t * -(ddouble)13 / 96);
                    }
                    else {
                        cs[1] = -t * (0.5 + t * (0.25 + t * (ddouble)1 / 24));
                        cs[2] = sq_t * ((ddouble)1 / 12 + t * ((ddouble)1 / 18 + t * (ddouble)1 / 96));
                        cs[3] = -t * sq_t * ((ddouble)1 / 144 + t * ((ddouble)1 / 192 + t * (ddouble)1 / 960));
                    }

                    cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                    return Array.AsReadOnly(cs);
                }
                else {
                    ddouble[] cs_q0 = new ddouble[n / 2 + 1];
                    ReadOnlyCollection<ddouble> cs_eps = GenerateCCoefZeroShifted(n, Eps);

                    if (n == 0) {
                        cs_q0[0] = Rcp(Sqrt2);
                    }
                    else {
                        cs_q0[n / 2] = 1d;
                    }

                    int terms = int.Max(cs_q0.Length, cs_eps.Count);
                    ddouble[] cs = new ddouble[terms];
                    ddouble w = q / Eps;

                    for (int i = 0; i < cs.Length; i++) {
                        ddouble c_q0 = i < cs_q0.Length ? cs_q0[i] : 0d;
                        ddouble c_eps = i < cs_eps.Count ? cs_eps[i] : 0d;

                        cs[i] = c_q0 + (c_eps - c_q0) * w;
                    }

                    cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                    return Array.AsReadOnly(cs);
                }
            }

            internal static ReadOnlyCollection<ddouble> GenerateCCoefZeroShifted(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                if (q < Eps) {
                    return QNearZeroCCoef(n, q);
                }

                int sq_n = checked(n * n);
                (ddouble a_sft, ReadOnlyCollection<ddouble> arms) = AZeroShift(n, q);

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[128];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + (n & 1)), sq_k0 = checked(k * k); m >= arms.Count; m--, k -= 2) {
                    ddouble c = (a_sft - (k * k - sq_n)) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (double.ILogB(cs[m - 1].Hi) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }
                for (int m = arms.Count - 1; m > 2; m--) {
                    ddouble c = arms[m] * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;
                }

                int scale = cs.Select(c => double.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoefZeroShifted(arms, q, m, 2, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoefZeroShifted(arms, q, m, 1, cs[m]);
                        }

                        if (m <= 2 + n / 2 || d < 1d) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                cs = NormalizeAndTruncateCoef(n, (n & 1) == 0 ? 2 : 1, cs);

                if (!IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

                return Array.AsReadOnly(cs);
            }

            internal static ReadOnlyCollection<ddouble> GenerateSCoef(int n, ddouble q, ddouble b, int terms = -1) {
                Debug.Assert(q >= 0d, nameof(q));

                if (q < Eps) {
                    return QNearZeroSCoef(n, q);
                }
                if (q <= NZThreshold) {
                    return GenerateSCoefZeroShifted(n, q);
                }

                terms = (terms > 0) ? terms : int.Max(128, checked((int)(double.Pow(q.Hi, 0.2475) * (0.094 * n + 11.830))));

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + 2 - (n & 1)), sq_k0 = checked(k * k); m > 2; m--, k -= 2) {
                    ddouble c = (b - k * k) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (double.ILogB(cs[m - 1].Hi) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }

                int scale = cs.Select(c => double.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(b, q, m, 1, 2, 4, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(b, q, m, 1, 1, 1 - q, cs[m]);
                        }

                        if (m <= 2 + n / 2 || d < 1d) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                cs = NormalizeAndTruncateCoef(n, 1, cs);

                if (!IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

                return Array.AsReadOnly(cs);
            }

            private static ReadOnlyCollection<ddouble> QNearZeroSCoef(int n, ddouble q) {
                if (n <= 2) {
                    ddouble t = Ldexp(q, -2), sq_t = t * t;
                    ddouble[] cs = new ddouble[4];
                    cs[0] = 1d;

                    if (n == 1) {
                        cs[1] = -t * (0.5 + t * (-0.25 + t * (ddouble)1 / 24));
                        cs[2] = sq_t * ((ddouble)1 / 12 + t * (-(ddouble)1 / 18 + t * (ddouble)1 / 96));
                        cs[3] = -t * sq_t * ((ddouble)1 / 144 + t * (-(ddouble)1 / 192 + t * (ddouble)1 / 960));
                    }
                    else {
                        cs[1] = -t * ((ddouble)1 / 3 + sq_t * -(ddouble)5 / 216);
                        cs[2] = sq_t * ((ddouble)1 / 24 + sq_t * -(ddouble)37 / 8640);
                        cs[3] = -t * sq_t * ((ddouble)1 / 360 + sq_t * -(ddouble)11 / 32400);
                    }

                    cs = NormalizeAndTruncateCoef(n, 1, cs);

                    return Array.AsReadOnly(cs);
                }
                else {
                    ddouble[] cs_q0 = new ddouble[(n - 1) / 2 + 1];
                    ReadOnlyCollection<ddouble> cs_eps = GenerateSCoefZeroShifted(n, Eps);

                    cs_q0[(n - 1) / 2] = 1d;

                    int terms = int.Max(cs_q0.Length, cs_eps.Count);
                    ddouble[] cs = new ddouble[terms];
                    ddouble w = q / Eps;

                    for (int i = 0; i < cs.Length; i++) {
                        ddouble c_q0 = i < cs_q0.Length ? cs_q0[i] : 0d;
                        ddouble c_eps = i < cs_eps.Count ? cs_eps[i] : 0d;

                        cs[i] = c_q0 + (c_eps - c_q0) * w;
                    }

                    cs = NormalizeAndTruncateCoef(n, 1, cs);

                    return Array.AsReadOnly(cs);
                }
            }

            internal static ReadOnlyCollection<ddouble> GenerateSCoefZeroShifted(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                if (q < Eps) {
                    return QNearZeroSCoef(n, q);
                }

                int sq_n = checked(n * n);
                (ddouble b_sft, ReadOnlyCollection<ddouble> brms) = BZeroShift(n, q);

                ddouble inv_q = 1d / q;

                ddouble[] cs = new ddouble[128];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + 2 - (n & 1)), sq_k0 = checked(k * k); m >= brms.Count; m--, k -= 2) {
                    ddouble c = (b_sft - (k * k - sq_n)) * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;

                    if (double.ILogB(cs[m - 1].Hi) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }
                for (int m = brms.Count - 1; m > 2; m--) {
                    ddouble c = brms[m] * cs[m] * inv_q - cs[m + 1];

                    cs[m - 1] = c;
                }

                int scale = cs.Select(c => double.ILogB(c.Hi)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        (scs, rm, d) = SolveCoefZeroShifted(brms, q, m, 1, cs[m]);

                        if (m <= 2 + n / 2 || d < 1d) {
                            Array.Copy(scs, cs, m);
                            break;
                        }
                    }
                }

                cs = NormalizeAndTruncateCoef(n, 1, cs);

                if (!IsFinite(cs[0])) {
                    return Array.AsReadOnly(Enumerable.Empty<ddouble>().ToArray());
                }

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

                ddouble d = Abs(ts[m - 1] / ts[m]);

                return (cs, ts[m], d);
            }

            public static (ddouble[] cs, ddouble r, ddouble d) SolveCoefZeroShifted(ReadOnlyCollection<ddouble> arms, ddouble q, int m, int k, ddouble cn) {
                if (m < 2) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble[] cs = new ddouble[m], ts = new ddouble[m + 1], qs = new ddouble[m];
                ddouble sq_q = q * q;

                (ts[0], ts[1], ts[2]) = (1d, arms[0], arms[0] * arms[1] - k * q * q);
                (qs[0], qs[1]) = (cn * q, cn * sq_q);

                for (int i = 2; i < m; i++) {
                    qs[i] = qs[i - 1] * q;
                    ts[i + 1] = ts[i] * arms[i] - ts[i - 1] * sq_q;
                }

                for (int i = 0; i < cs.Length; i++) {
                    cs[i] = qs[m - i - 1] * ts[i] / ts[m];
                }

                ddouble d = Abs(ts[m - 1] / ts[m]);

                return (cs, ts[m], d);
            }

            private static ddouble[] NormalizeAndTruncateCoef(int n, int k, ddouble[] cs) {
                ddouble norm = (cs[0] * cs[0]) * k;
                for (int i = 1; i < cs.Length; i++) {
                    norm += cs[i] * cs[i];
                }

                ddouble r = Sqrt(norm) * Sign(cs.Sum());
                for (int i = 0; i < cs.Length; i++) {
                    cs[i] /= r;
                }

                ddouble threshold = Ldexp(cs.Select(c => Abs(c)).Max(), -128);
                for (int i = cs.Length - 1; i > 0; i--) {
                    if (Abs(cs[i]) > threshold) {
                        cs = cs[..(i + 1)];
                        break;
                    }
                }

                return cs;
            }

            private static ddouble[] ScaleAndTruncateCoef(ddouble[] cs, int m) {
                for (int j = m - 1; j < cs.Length; j++) {
                    if (double.ILogB(cs[j - 1].Hi) < -128 && double.ILogB(cs[j].Hi) < -128 && m >= MathieuUtil.MaxN * 2) {
                        cs = cs[..j];
                        cs[^1] = 0d;
                        break;
                    }

                    cs[j] = Ldexp(cs[j], -128);
                }

                return cs;
            }

            internal static (ddouble zerosft, ReadOnlyCollection<ddouble> arms) AZeroShift(int n, ddouble q) {
                if (n < 0 || n > MathieuUtil.MaxN) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                Debug.Assert(q <= MathieuUtil.NZThreshold, nameof(q));

                if (n == 0) {
                    ddouble a = MathieuA(n, q);
                    ddouble[] arms = (new ddouble[(MathieuUtil.MaxN + 2) / 2])
                        .Select((_, m) => a - checked((2 * m) * (2 * m))).ToArray();

                    return (a, Array.AsReadOnly(arms));
                }
                else {
                    ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef = PadeAzTables[n];

                    (ddouble sc, ddouble sd) = pade_coef[0];
                    for (int i = 1; i < pade_coef.Count; i++) {
                        (ddouble c, ddouble d) = pade_coef[i];

                        sc = sc * q + c;
                        sd = sd * q + d;
                    }

                    Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

                    ddouble a = q * q * sc / sd;
                    int s = ((n & 1) == 0) ? 0 : 1;
                    ddouble[] arms = (new ddouble[(MathieuUtil.MaxN + 2) / 2])
                        .Select((_, m) => a - checked((2 * m + s) * (2 * m + s) - (n * n))).ToArray();

                    if (n == 1) {
                        for (int i = 1; i < arms.Length; i++) {
                            arms[i] += q;
                        }

                        return (a + q, Array.AsReadOnly(arms));
                    }
                    else if ((n & 1) == 1) {
                        arms[0] -= q;

                        return (a, Array.AsReadOnly(arms));
                    }
                    else {
                        return (a, Array.AsReadOnly(arms));
                    }
                }
            }

            internal static (ddouble zerosft, ReadOnlyCollection<ddouble> brms) BZeroShift(int n, ddouble q) {
                if (n < 1 || n > MathieuUtil.MaxN) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }

                Debug.Assert(q <= MathieuUtil.NZThreshold, nameof(q));

                ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef = PadeBzTables[n];

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * q + c;
                    sd = sd * q + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble b = q * q * sc / sd;
                int s = ((n & 1) == 0) ? 2 : 1;
                ddouble[] brms = (new ddouble[(MathieuUtil.MaxN + 2) / 2])
                    .Select((_, m) => b - checked((2 * m + s) * (2 * m + s) - (n * n))).ToArray();

                if (n == 1) {
                    for (int i = 1; i < brms.Length; i++) {
                        brms[i] -= q;
                    }

                    return (b - q, Array.AsReadOnly(brms));
                }
                else if ((n & 1) == 1) {
                    brms[0] += q;

                    return (b, Array.AsReadOnly(brms));
                }
                else {
                    return (b, Array.AsReadOnly(brms));
                }
            }
        }

        internal static partial class Consts {
            public static class Mathieu {
                public static readonly ReadOnlyCollection<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>>
                    PadeMTables, PadeDTables, PadeATables, PadeBTables;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>
                    PadeAzTables, PadeBzTables;

                public static readonly ReadOnlyDictionary<int, ReadOnlyCollection<ddouble>> LimitTable;

                static Mathieu() {
                    {
                        Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                            ResourceUnpack.NumTableX2(Resource.MathieuMTable, reverse: true);

                        List<ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>>> table = new();

                        for (int n = 0; n <= MathieuUtil.MaxN; n++) {
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

                        for (int n = 1; n <= MathieuUtil.MaxN; n++) {
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

                        for (int n = 0; n <= MathieuUtil.MaxN; n++) {
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

                        for (int n = 1; n <= MathieuUtil.MaxN; n++) {
                            List<ReadOnlyCollection<(ddouble c, ddouble d)>> coef = new();

                            for (int i = 0; i < 3; i++) {
                                coef.Add(tables[$"PadeB{n}Table_{i}"]);
                            }

                            table.Add(Array.AsReadOnly(coef.ToArray()));
                        }

                        PadeBTables = Array.AsReadOnly(table.ToArray());
                    }

                    {
                        Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                            ResourceUnpack.NumTableX2(Resource.MathieuNZTable, reverse: true);

                        List<ReadOnlyCollection<(ddouble c, ddouble d)>> az_table = new() {
                            Array.AsReadOnly(Enumerable.Empty<(ddouble c, ddouble d)>().ToArray())
                        };
                        List<ReadOnlyCollection<(ddouble c, ddouble d)>> bz_table = new() {
                            Array.AsReadOnly(Enumerable.Empty<(ddouble c, ddouble d)>().ToArray())
                        };

                        for (int n = 1; n <= 12; n++) {
                            az_table.Add(tables[$"PadeAz{n}Table"]);
                            bz_table.Add(tables[$"PadeBz{n}Table"]);
                        }
                        for (int n = 13; n <= MathieuUtil.MaxN; n++) {
                            az_table.Add(tables[$"PadeABz{n}Table"]);
                            bz_table.Add(tables[$"PadeABz{n}Table"]);
                        }

                        PadeAzTables = Array.AsReadOnly(az_table.ToArray());
                        PadeBzTables = Array.AsReadOnly(bz_table.ToArray());
                    }

                    {
                        Dictionary<int, ReadOnlyCollection<ddouble>> limit_table = new();

                        for (long s = 1; s <= MathieuUtil.MaxN * 2 + 1; s += 2) {
                            ReadOnlyCollection<ddouble> coef = new(Array.AsReadOnly(new ddouble[] {
                                Ldexp(1 + s * s, -3),
                                Ldexp(s * (3 + s * s), -7),
                                Ldexp(9 + s * s * (34 + s * s * 5), -12),
                                Ldexp(s * (405 + s * s * (410 + s * s * 33)), -17),
                                Ldexp(486 + s * s * (2943 + s * s * (1260 + s * s * 63)), -20),
                                Ldexp(checked(s * (41607 + s * s * (69001 + s * s * (15617 + s * s * 527)))), -25),
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