using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {
    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ReadOnlyCollection<ddouble> Factorial => Consts.Factorial.FactorialTable;

        internal static partial class Consts {
            public static class Factorial {
                public static readonly ReadOnlyCollection<ddouble> FactorialTable;

                static Factorial() {
                    List<ddouble> table = new() {
                        1,
                        1
                    };

                    for (int i = 2; i <= 256; i++) {
                        ddouble t = i * table[^1];

                        if (IsPositiveInfinity(t)) {
                            break;
                        }

                        table.Add(t);
                    }

                    FactorialTable = table.AsReadOnly();
                }
            }
        }
    }
}
