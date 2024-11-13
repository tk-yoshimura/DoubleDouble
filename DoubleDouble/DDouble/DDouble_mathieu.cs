using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Mathieu;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble MathieuA(int n, ddouble q) {
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    $"In the calculation of the MathieuA function, n greater than {MathieuUtil.MaxN} is not supported."
                );
            }

            if (IsNegative(q)) {
                return ((n & 1) == 0) ? MathieuA(n, -q) : MathieuB(n, -q);
            }

            if (IsNaN(q)) {
                return NaN;
            }
            if (IsInfinity(q)) {
                return NegativeInfinity;
            }

            if (q < MathieuUtil.NearZeroLimit(n)) {
                (ddouble m, ddouble d) = MathieuUtil.NearZeroPade(n, q);

                return m + d;
            }
            else {
                return MathieuUtil.LimitAPade(n, q);
            }
        }

        public static ddouble MathieuB(int n, ddouble q) {
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));
            if (n == 0) {
                return MathieuA(n, q);
            }
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    $"In the calculation of the MathieuB function, n greater than {MathieuUtil.MaxN} is not supported."
                );
            }

            if (IsNegative(q)) {
                return ((n & 1) == 0) ? MathieuB(n, -q) : MathieuA(n, -q);
            }

            if (IsNaN(q)) {
                return NaN;
            }
            if (IsInfinity(q)) {
                return NegativeInfinity;
            }

            if (q <= MathieuUtil.NearZeroLimit(n)) {
                (ddouble m, ddouble d) = MathieuUtil.NearZeroPade(n, q);

                return m - d;
            }
            else {
                return MathieuUtil.LimitBPade(n, q);
            }
        }

        public static ddouble MathieuC(int n, ddouble q, ddouble x) {
            ArgumentOutOfRangeException.ThrowIfNegative(n, nameof(n));
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
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
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 1, nameof(n));
            if (n > MathieuUtil.MaxN) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
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

            public static double NearZeroLimit(int n) => 128 * int.Max(1, n * n);
            private static readonly Dictionary<(int n, ddouble q), ReadOnlyCollection<ddouble>> c_coef_cache = new(), s_coef_cache = new();

            public static (ddouble m, ddouble d) NearZeroPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                int n2 = n * n;
                ddouble u = Square((n <= 1) ? q : q / n2);

                (ddouble mv, ddouble dv, int table_index) = PadeMDParam(n, u);

                Debug.Assert(mv >= 0d);
                Debug.Assert(dv >= 0d);

                ReadOnlyCollection<(ddouble c, ddouble d)> m_table = PadeMTables[n][table_index];

                (ddouble m_sc, ddouble m_sd) = m_table[0];
                for (int i = 1; i < m_table.Count; i++) {
                    (ddouble c, ddouble d) = m_table[i];

                    m_sc = m_sc * mv + c;
                    m_sd = m_sd * mv + d;
                }

                Debug.Assert(m_sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble my = m_sc / m_sd;

                if (n >= 2) {
                    my *= n2;
                }

                if (n < 1) {
                    return (my, 0d);
                }

                ReadOnlyCollection<(ddouble c, ddouble d)> d_table = PadeDTables[n][table_index];

                (ddouble d_sc, ddouble d_sd) = d_table[0];
                for (int i = 1; i < d_table.Count; i++) {
                    (ddouble c, ddouble d) = d_table[i];

                    d_sc = d_sc * dv + c;
                    d_sd = d_sd * dv + d;
                }

                Debug.Assert(d_sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble dy = d_sd / d_sc;

                dy *= Ldexp(Pow(q, n) / Square(Factorial[n - 1]), 2 - 2 * n);

                return (my, dy);
            }

            private static (ddouble mv, ddouble dv, int table_index) PadeMDParam(int n, ddouble u) {
                return
                    (u <= 0.125d) ? (u, u, 0) :
                    (u <= 0.1875d) ? (
                        (n <= 10 ? u - 0.125d : 0.1875d - u),
                        (n <= 9 ? u - 0.125d : 0.1875d - u),
                        1
                    ) :
                    (u <= 0.25d) ? (
                        (n <= 10 ? u - 0.1875d : 0.25d - u),
                        (n <= 11 ? u - 0.1875d : 0.25d - u),
                        2
                    ) :
                    (u <= 0.28125d) ? (
                        (n <= 12 ? u - 0.25d : 0.28125d - u),
                        (n <= 14 ? u - 0.25d : 0.28125d - u),
                        3
                    ) :
                    (u <= 0.3125d) ? (
                        u - 0.28125d,
                        (n != 15 ? u - 0.28125d : 0.3125d - u),
                        4
                    ) :
                    (u <= 0.375d) ? (u - 0.3125d, u - 0.3125d, 5) :
                    (u <= 0.5d) ? (u - 0.375d, u - 0.375d, 6) :
                    (u <= 0.75d) ? (u - 0.5d, u - 0.5d, 7) :
                    (u <= 1d) ? (u - 0.75d, u - 0.75d, 8) :
                    (u <= 2d) ? (u - 1d, u - 1d, 9) :
                    (u <= 4d) ? (u - 2d, u - 2d, 10) :
                    (u <= 8d) ? (u - 4d, u - 4d, 11) :
                    (u <= 16d) ? (u - 8d, u - 8d, 12) :
                    (u <= 64d) ? (u - 16d, u - 16d, 13) :
                    (u <= 256d) ? (u - 64d, u - 64d, 14) :
                    (u <= 1024d) ? (u - 256d, u - 256d, 15) :
                    (u <= 4096d) ? (u - 1024d, u - 1024d, 16) :
                    (u <= 16384d) ? (u - 4096d, u - 4096d, 17) :
                    throw new NotImplementedException();
            }

            public static ddouble LimitAPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                int s = 2 * n + 1, n2 = n * n;
                ddouble h = Sqrt(q), invh = 1d / h;
                ddouble u = 1d / Square((n <= 1) ? q : q / n2);

                ReadOnlyCollection<ddouble> limit_coef = LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2d * (s * h - q) - asymp;

                (ddouble v, int table_index) = PadeABParam(u);

                Debug.Assert(v >= 0d);

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeATables[n][table_index];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble mult = sc / sd;

                ddouble y = (asymp - n2) * mult + n2;

                return y;
            }

            public static ddouble LimitBPade(int n, ddouble q) {
                Debug.Assert(q >= 0d, nameof(q));

                int s = 2 * n - 1, n2 = n * n;
                ddouble h = Sqrt(q), invh = 1d / h;
                ddouble u = 1d / Square((n <= 1) ? q : q / n2);

                ReadOnlyCollection<ddouble> limit_coef = LimitTable[s];

                ddouble asymp = limit_coef[0];
                for (int i = 1; i < limit_coef.Count; i++) {
                    asymp = asymp * invh + limit_coef[i];
                }
                asymp = 2d * (s * h - q) - asymp;

                (ddouble v, int table_index) = PadeABParam(u);

                Debug.Assert(v >= 0d);

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeBTables[n][table_index];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

                ddouble mult = sc / sd;

                ddouble y = (asymp - n2) * mult + n2;

                return y;
            }

            private static (ddouble v, int table_index) PadeABParam(ddouble u) {
                return
                    (u <= Math.ScaleB(1, -32)) ? (u, 0) :
                    (u <= Math.ScaleB(1, -24)) ? (u - Math.ScaleB(1, -32), 1) :
                    (u <= Math.ScaleB(1, -18)) ? (u - Math.ScaleB(1, -24), 2) :
                    (u <= Math.ScaleB(1, -14)) ? (u - Math.ScaleB(1, -18), 3) :
                    throw new NotImplementedException();
            }

            public static ReadOnlyCollection<ddouble> CCoef(int n, ddouble q) {
                if (!c_coef_cache.TryGetValue((n, q), out ReadOnlyCollection<ddouble> coef)) {
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

                    c_coef_cache[(n, q)] = coef;
                }

                return coef;
            }

            public static ReadOnlyCollection<ddouble> SCoef(int n, ddouble q) {
                if (!s_coef_cache.TryGetValue((n, q), out ReadOnlyCollection<ddouble> coef)) {
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

                    s_coef_cache[(n, q)] = coef;
                }

                return coef;
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

                ddouble invq = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + (n & 1)); m > 2; m--, k -= 2) {
                    ddouble c = (a - k * k) * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;

                    if (ILogB(cs[m - 1]) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }

                int scale = cs.Select(c => ILogB(c)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(a, q, m, 2, 0, 0d, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(a, q, m, 1, 1, 1d + q, cs[m]);
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
                    ddouble t = Ldexp(q, -2), t2 = t * t;
                    ddouble[] cs = new ddouble[4];
                    cs[0] = 1d;

                    if (n == 0) {
                        cs[1] = -t * (2d + t2 * -3.5);
                        cs[2] = t2 * (0.5 + t2 * -(ddouble)10 / 9);
                        cs[3] = -t * t2 * ((ddouble)1 / 18 + t2 * -(ddouble)13 / 96);
                    }
                    else {
                        cs[1] = -t * (0.5 + t * (0.25 + t * (ddouble)1 / 24));
                        cs[2] = t2 * ((ddouble)1 / 12 + t * ((ddouble)1 / 18 + t * (ddouble)1 / 96));
                        cs[3] = -t * t2 * ((ddouble)1 / 144 + t * ((ddouble)1 / 192 + t * (ddouble)1 / 960));
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

                int n2 = checked(n * n);
                (ddouble a_sft, ReadOnlyCollection<ddouble> arms) = AZeroShift(n, q);

                ddouble invq = 1d / q;

                ddouble[] cs = new ddouble[128];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + (n & 1)); m >= arms.Count; m--, k -= 2) {
                    ddouble c = (a_sft - (k * k - n2)) * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;

                    if (ILogB(cs[m - 1]) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }
                for (int m = arms.Count - 1; m > 2; m--) {
                    ddouble c = arms[m] * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;
                }

                int scale = cs.Select(c => ILogB(c)).Max();
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

                ddouble invq = 1d / q;

                ddouble[] cs = new ddouble[terms];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + 2 - (n & 1)); m > 2; m--, k -= 2) {
                    ddouble c = (b - k * k) * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;

                    if (ILogB(cs[m - 1]) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }

                int scale = cs.Select(c => ILogB(c)).Max();
                for (int m = 0; m < cs.Length; m++) {
                    cs[m] = Ldexp(cs[m], -scale);
                }

                for (int m = int.Min(MathieuUtil.MaxN / 2, cs.Length - 1); m >= 2; m--) {
                    if (m == 2 || Abs(cs[m]) > Abs(cs[m - 1])) {
                        ddouble[] scs;
                        ddouble rm, d;

                        if ((n & 1) == 0) {
                            (scs, rm, d) = SolveCoef(b, q, m, 1, 2, 4d, cs[m]);
                        }
                        else {
                            (scs, rm, d) = SolveCoef(b, q, m, 1, 1, 1d - q, cs[m]);
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
                    ddouble t = Ldexp(q, -2), t2 = t * t;
                    ddouble[] cs = new ddouble[4];
                    cs[0] = 1d;

                    if (n == 1) {
                        cs[1] = -t * (0.5 + t * (-0.25 + t * (ddouble)1 / 24));
                        cs[2] = t2 * ((ddouble)1 / 12 + t * (-(ddouble)1 / 18 + t * (ddouble)1 / 96));
                        cs[3] = -t * t2 * ((ddouble)1 / 144 + t * (-(ddouble)1 / 192 + t * (ddouble)1 / 960));
                    }
                    else {
                        cs[1] = -t * ((ddouble)1 / 3 + t2 * -(ddouble)5 / 216);
                        cs[2] = t2 * ((ddouble)1 / 24 + t2 * -(ddouble)37 / 8640);
                        cs[3] = -t * t2 * ((ddouble)1 / 360 + t2 * -(ddouble)11 / 32400);
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

                int n2 = checked(n * n);
                (ddouble b_sft, ReadOnlyCollection<ddouble> brms) = BZeroShift(n, q);

                ddouble invq = 1d / q;

                ddouble[] cs = new ddouble[128];
                (cs[^2], cs[^1]) = (Epsilon, 0d);

                for (long m = cs.Length - 2, k = checked(2 * (long)m + 2 - (n & 1)); m >= brms.Count; m--, k -= 2) {
                    ddouble c = (b_sft - (k * k - n2)) * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;

                    if (ILogB(cs[m - 1]) > 0) {
                        cs = ScaleAndTruncateCoef(cs, checked((int)m));
                    }
                }
                for (int m = brms.Count - 1; m > 2; m--) {
                    ddouble c = brms[m] * cs[m] * invq - cs[m + 1];

                    cs[m - 1] = c;
                }

                int scale = cs.Select(c => ILogB(c)).Max();
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
                Debug.Assert(m >= 2);

                ddouble[] cs = new ddouble[m], ts = new ddouble[m + 1], qs = new ddouble[m];
                ddouble q2 = q * q;

                (ts[0], ts[1], ts[2]) = (1d, a - r0, (a - r0) * (a - checked((2 + s) * (2 + s))) - k * q2);
                (qs[0], qs[1]) = (cn * q, cn * q2);

                for (int i = 2; i < m; i++) {
                    qs[i] = qs[i - 1] * q;
                    ts[i + 1] = ts[i] * (a - checked((2 * i + s) * (2 * i + s))) - ts[i - 1] * q2;
                }

                for (int i = 0; i < cs.Length; i++) {
                    cs[i] = qs[m - i - 1] * ts[i] / ts[m];
                }

                ddouble d = Abs(ts[m - 1] / ts[m]);

                return (cs, ts[m], d);
            }

            public static (ddouble[] cs, ddouble r, ddouble d) SolveCoefZeroShifted(ReadOnlyCollection<ddouble> arms, ddouble q, int m, int k, ddouble cn) {
                Debug.Assert(m >= 2);

                ddouble[] cs = new ddouble[m], ts = new ddouble[m + 1], qs = new ddouble[m];
                ddouble q2 = q * q;

                (ts[0], ts[1], ts[2]) = (1d, arms[0], arms[0] * arms[1] - k * q2);
                (qs[0], qs[1]) = (cn * q, cn * q2);

                for (int i = 2; i < m; i++) {
                    qs[i] = qs[i - 1] * q;
                    ts[i + 1] = ts[i] * arms[i] - ts[i - 1] * q2;
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
                    if (ILogB(cs[j - 1]) < -128 && ILogB(cs[j]) < -128 && m >= MathieuUtil.MaxN * 2) {
                        cs = cs[..j];
                        cs[^1] = 0d;
                        break;
                    }

                    cs[j] = Ldexp(cs[j], -128);
                }

                return cs;
            }

            internal static (ddouble zerosft, ReadOnlyCollection<ddouble> arms) AZeroShift(int n, ddouble q) {
                Debug.Assert(n >= 0 && n <= MathieuUtil.MaxN);

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

                    Debug.Assert(sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

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
                Debug.Assert(n >= 1 && n <= MathieuUtil.MaxN);

                Debug.Assert(q <= MathieuUtil.NZThreshold, nameof(q));

                ReadOnlyCollection<(ddouble c, ddouble d)> pade_coef = PadeBzTables[n];

                (ddouble sc, ddouble sd) = pade_coef[0];
                for (int i = 1; i < pade_coef.Count; i++) {
                    (ddouble c, ddouble d) = pade_coef[i];

                    sc = sc * q + c;
                    sd = sd * q + d;
                }

                Debug.Assert(sd > 0.5d, $"[Mathieu q={q}] Too small pade denom!!");

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

                            for (int i = 0; i < 18; i++) {
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

                            for (int i = 0; i < 18; i++) {
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

                            for (int i = 0; i < 4; i++) {
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

                            for (int i = 0; i < 4; i++) {
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
                            long s2 = checked(s * s);

                            ReadOnlyCollection<ddouble> coef = new(Array.AsReadOnly((new ddouble[] {
                                Ldexp(1 + s2, -3),
                                Ldexp(s * (3 + s2), -7),
                                Ldexp(9 + s2 * (34 + s2 * 5), -12),
                                Ldexp(s * (405 + s2 * (410 + s2 * 33)), -17),
                                Ldexp(486 + s2 * (2943 + s2 * (1260 + s2 * 63)), -20),
                                Ldexp(checked(s * (41607 + s2 * (69001 + s2 * (15617 + s2 * 527)))), -25),
                            }).Reverse().ToArray()));

                            limit_table.Add((int)s, coef);
                        }

                        LimitTable = new(limit_table);
                    }
                }
            }
        }
    }
}