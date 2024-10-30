using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.Ti;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Ti(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsNegative(x)) {
                return -Ti(-x);
            }
            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x <= 1d) {
                return TiUtil.Kernel(x);
            }
            else {
                return TiUtil.Kernel(1d / x) + Ldexp(Pi, -1) * Log(x);
            }
        }

        internal static class TiUtil {
            public static ddouble Kernel(ddouble x) {
                Debug.Assert(x >= 0d && x <= 1d, nameof(x));

                ddouble v = x * x;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTable;

                (ddouble sc, ddouble sd) = table[0];
                for (int i = 1; i < table.Count; i++) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                Debug.Assert(sd > 0.5d, $"[Ti x={x}] Too small pade denom!!");

                ddouble y = x * sc / sd;

                return y;
            }
        }

        internal static partial class Consts {
            public static class Ti {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> PadeTable;

                static Ti() {
                    Dictionary<string, ReadOnlyCollection<(ddouble c, ddouble d)>> pade_tables =
                        ResourceUnpack.NumTableX2(Resource.TiTable, reverse: true);

                    PadeTable = pade_tables["PadeTable"];
                }
            }
        }
    }
}