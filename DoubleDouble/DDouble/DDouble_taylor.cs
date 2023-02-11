using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ReadOnlyCollection<ddouble> TaylorSequence => Consts.Taylor.TaylorTable;

        internal static partial class Consts {
            public static class Taylor {
                public static readonly ReadOnlyCollection<ddouble> TaylorTable;

                static Taylor() {
                    List<ddouble> table = new() {
                        1,
                        1
                    };

                    BigInteger v = 2;

                    for (int d = 3; d <= 256; d++) {
                        ddouble t = Rcp((ddouble)v);

                        table.Add(t);

                        if (IsZero(t)) {
                            break;
                        }

                        v *= d;
                    }

                    TaylorTable = table.AsReadOnly();
                }
            }
        }
    }
}
