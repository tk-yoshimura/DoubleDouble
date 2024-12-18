﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ReadOnlyCollection<ddouble> TaylorSequence => Consts.Taylor.TaylorTable;

        internal static partial class Consts {
            public static class Taylor {
                public static readonly ReadOnlyCollection<ddouble> TaylorTable;

                static Taylor() {
                    List<ddouble> table = [1d, 1d];

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
