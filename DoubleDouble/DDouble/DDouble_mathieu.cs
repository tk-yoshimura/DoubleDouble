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

        internal static class Mathieu {
            public static double NearZeroLimit(int n) => 64 * Math.Max(1, n * n);

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