using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Clausen;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Clausen(ddouble x, bool normalized = false) {
            if (!IsFinite(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Clausen(-x, normalized);
            }

            if (!normalized) {
                x *= RcpPI;
            }

            if (x >= 2d) {
                x -= Ldexp(Floor(Ldexp(x, -1)), 1);
                if (IsNegative(x)) {
                    x += 2d;
                }
            }

            if (x == 0d) {
                return 0d;
            }

            return (x <= 1d) ? ClausenUtil.Kernel(x) : -ClausenUtil.Kernel(2d - x);
        }

        internal static class ClausenUtil {
            public static ddouble Kernel(ddouble x) {
                Debug.Assert(x >= 0d && x <= 1d, nameof(x));

                if (x <= 0.25d) {
                    return NearZero(x);
                }
                else {
                    return PadeApprox(x);
                }
            }

            private static ddouble NearZero(ddouble x) {
                Debug.Assert(x >= 0d && x <= 0.25d, nameof(x));

                ddouble x2 = x * x;
                ddouble c = -Log(x * PIDivE * (1d - Ldexp(x2, -2)));

                if (!IsFinite(c)) {
                    return 0d;
                }

                ddouble s = NearZeroTable[0];
                for (int i = 1; i < NearZeroTable.Count; i++) {
                    s = s * x2 + NearZeroTable[i];
                }
                s *= x2;

                ddouble y = (s + c) * x * PI;

                return y;
            }

            private static ddouble PadeApprox(ddouble x) {
                Debug.Assert(x >= 0.25d && x <= 1d, nameof(x));

                if (x == 1d) {
                    return 0d;
                }

                int n = (int)Floor(Ldexp(x, 2));
                ddouble v = x - Ldexp(n, -2);

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTables[n - 1];

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.0625d, $"[Clausen x={x}] Too small pade denom!!");

                ddouble y = sc / sd * (x * (1d - x));

                return y;
            }
        }

        internal static partial class Consts {
            public static class Clausen {
                public static readonly ReadOnlyCollection<ddouble> NearZeroTable;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                public static readonly ddouble PIDivE = (+1, 0, 0x93EEDFB138EDEF7EuL, 0xE1DC499E64F8915AuL);

                static Clausen() {
                    Dictionary<string, ReadOnlyCollection<ddouble>> nearzero_tables =
                        ResourceUnpack.NumTable(Resource.ClausenTable_NearZero, reverse: true);

                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> pade_tables =
                        ResourceUnpack.NumTableX2(Resource.ClausenTable_Pade, reverse: true);

                    NearZeroTable = nearzero_tables["NearZeroTable"];

                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        pade_tables["PadeX0p25Table"],
                        pade_tables["PadeX0p50Table"],
                        pade_tables["PadeX0p75Table"]
                    });
                }
            }
        }
    }
}