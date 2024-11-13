using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.EulerQ;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble EulerQ(ddouble q) {
            return Exp(LogEulerQ(q));
        }

        public static ddouble LogEulerQ(ddouble q) {
            if (!(q >= -1d && q <= 1d)) {
                return NaN;
            }
            if (ILogB(1d - Abs(q)) <= EpsExponent) {
                return NegativeInfinity;
            }

            return EulerQUtil.PadeApprox(q);
        }

        internal static class EulerQUtil {
            public static ddouble PadeApprox(ddouble q) {
                Debug.Assert(q >= -1d && q <= 1d, nameof(q));

                (int pade_idx, ddouble u) = q.hi switch {
                    < -0.984375 => (11, 1d + q),
                    < -0.9375 => (10, 0.984375d + q),
                    < -0.875 => (9, 0.9375d + q),
                    < -0.75 => (8, 0.875d + q),
                    < -0.5 => (7, 0.75d + q),
                    < 0.0 => (6, 0.5d + q),
                    < 0.5 => (0, 0.5d - q),
                    < 0.75 => (1, 0.75d - q),
                    < 0.875 => (2, 0.875d - q),
                    < 0.9375 => (3, 0.9375d - q),
                    < 0.984375 => (4, 0.984375d - q),
                    _ => (5, 1d - q)
                };

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTables[pade_idx];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * u + c;
                    sd = sd * u + d;
                }

                Debug.Assert(sd > 0.5d, $"[EulerQ q={q}] Too small pade denom!!");

                ddouble v = sc / sd;
                ddouble y = q * (v * q - 1d) / (1d - q * q);

                return y;
            }
        }

        internal static partial class Consts {
            public static class EulerQ {
                public const int EpsExponent = -1024;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;

                static EulerQ() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> tables =
                        ResourceUnpack.NumTableX2(Resource.EulerQTable, reverse: true);

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        tables["Mp05PadeTable"],
                        tables["Mp075PadeTable"],
                        tables["Mp0875PadeTable"],
                        tables["Mp09375PadeTable"],
                        tables["Mp0984375PadeTable"],
                        tables["Mp1PadeTable"],

                        tables["Mn05PadeTable"],
                        tables["Mn075PadeTable"],
                        tables["Mn0875PadeTable"],
                        tables["Mn09375PadeTable"],
                        tables["Mn0984375PadeTable"],
                        tables["Mn1PadeTable"],
                    });
                }
            }
        }
    }
}