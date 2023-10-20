using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Scorer;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble ScorerHi(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x >= ScorerHiOverflow) {
                return PositiveInfinity;
            }

            ddouble x_abs = Abs(x);

            if (x_abs < NearZero) {
                ddouble s = x * (x * NearZeroCoefs[0] + NearZeroCoefs[1]) + NearZeroCoefs[2];

                for (int i = 3; i + 2 < NearZeroCoefs.Count; i += 3) {
                    s = s * x + NearZeroCoefs[i];
                    s = s * x + NearZeroCoefs[i + 1];
                    s = s * x + NearZeroCoefs[i + 2];
                }

                s /= NearZeroC;

                return s;
            }
            else if (IsPositive(x)) {
                ddouble y = AiryBi(x) - ScorerGi(x);
                return y;
            }
            else if (x >= -PadeMax) {
                ddouble y = ScorerUtil.PadeApprox(-x, HiPadeTables);
                return y;
            }
            else {
                ddouble y = -ScorerUtil.Asymptotic(x);
                return y;
            }
        }

        public static ddouble ScorerGi(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (IsInfinity(x)) {
                return 0d;
            }

            ddouble x_abs = Abs(x);

            if (x_abs < NearZero) {
                ddouble s = x * (x * -Ldexp(NearZeroCoefs[0], 1) + NearZeroCoefs[1]) + NearZeroCoefs[2];

                for (int i = 3; i + 2 < NearZeroCoefs.Count; i += 3) {
                    s = s * x - Ldexp(NearZeroCoefs[i], 1);
                    s = s * x + NearZeroCoefs[i + 1];
                    s = s * x + NearZeroCoefs[i + 2];
                }

                s /= Ldexp(NearZeroC, 1);

                return s;
            }
            else if (IsNegative(x)) {
                ddouble y = AiryBi(x) - ScorerHi(x);
                return y;
            }
            else if (x <= PadeMax) {
                ddouble y = ScorerUtil.PadeApprox(x, GiPadeTables);
                return y;
            }
            else {
                ddouble y = ScorerUtil.Asymptotic(x);
                return y;
            }
        }

        internal static class ScorerUtil {

            public static ddouble PadeApprox(ddouble x, ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> tables) {
#if DEBUG
                if (x < 0d || !(x <= PadeMax)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                (int n, double x0) =
                    (x < 1d) ? (0, 0d) :
                    (x < 2d) ? (1, 1d) :
                    (x < 4d) ? (2, 2d) :
                    (x < 8d) ? (3, 4d) :
                    (x < 16d) ? (4, 8d) :
                    (x < 32d) ? (5, 16d) :
                    (6, 32d);

                ddouble v = x - x0;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = tables[n];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[Scorer x={x}] Too small pade denom!!");
#endif

                ddouble y = Pow2(sc / sd);

                return y;
            }

            public static ddouble Asymptotic(ddouble x) {
                ddouble v3 = 1d / (x * x * x);

                ddouble s = v3 * AsymptoticCoefs[0] + AsymptoticCoefs[1];

                for (int i = 2; i + 1 < AsymptoticCoefs.Count; i += 2) {
                    s = s * v3 + AsymptoticCoefs[i];
                    s = s * v3 + AsymptoticCoefs[i + 1];
                }

                s /= PI * x;

                return s;
            }
        }

        internal static partial class Consts {
            public static class Scorer {
                public static ddouble Rcp3 { get; } = Airy.Rcp3;
                public static ddouble RcpSqrt3 { get; } = Airy.RcpSqrt3;
                public static ddouble Cbrt3 { get; } = Airy.Cbrt3;
                public static ddouble NearZero { get; } = Airy.NearZero;

                public static double ScorerHiOverflow = 105d;

                public static ddouble Gamma1d3 = Airy.Gamma1d3;
                public static ddouble Gamma2d3 = Airy.Gamma2d3;
                public static ddouble NearZeroC = Cbrt3 * Cbrt3 * PI;

                public const double PadeMax = 64;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> HiPadeTables, GiPadeTables;

                public static readonly ReadOnlyCollection<ddouble> NearZeroCoefs, AsymptoticCoefs;

                static Scorer() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.ScorerTable, reverse: true);

                    HiPadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["HiPadeX0Table"],
                        tables["HiPadeX1Table"],
                        tables["HiPadeX2Table"],
                        tables["HiPadeX4Table"],
                        tables["HiPadeX8Table"],
                        tables["HiPadeX16Table"],
                        tables["HiPadeX32Table"],
                    });

                    GiPadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["GiPadeX0Table"],
                        tables["GiPadeX1Table"],
                        tables["GiPadeX2Table"],
                        tables["GiPadeX4Table"],
                        tables["GiPadeX8Table"],
                        tables["GiPadeX16Table"],
                        tables["GiPadeX32Table"],
                    });

                    NearZeroCoefs = Array.AsReadOnly(GenerateNearZeroCoefs());
                    AsymptoticCoefs = Array.AsReadOnly(GenerateAsymptoticCoefs());
                }

                private static ddouble[] GenerateNearZeroCoefs() {
                    ddouble[] coefs = new ddouble[18];

                    coefs[0] = Gamma1d3;
                    coefs[1] = Gamma2d3 * Cbrt3;
                    coefs[2] = Ldexp(Cbrt3 * Cbrt3, -1);

                    for (int k = 3; k < coefs.Length; k++) {
                        coefs[k] = coefs[k - 3] / ((k - 1) * k);
                    }

                    coefs = coefs.Reverse().ToArray();

                    return coefs;
                }

                private static ddouble[] GenerateAsymptoticCoefs() {
                    ddouble[] coefs = new ddouble[10];

                    coefs[0] = 1d;

                    for (int k = 1; k < coefs.Length; k++) {
                        coefs[k] = coefs[k - 1] * checked((3L * k - 1) * (3L * k - 2));
                    }

                    coefs = coefs.Reverse().ToArray();

                    return coefs;
                }
            }
        }
    }
}