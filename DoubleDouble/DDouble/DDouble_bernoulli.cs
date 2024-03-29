﻿using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {

    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ReadOnlyCollection<ddouble> BernoulliSequence => Consts.Bernoulli.BernoulliTable;

        internal static partial class Consts {
            public static class Bernoulli {
                public static ReadOnlyCollection<ddouble> BernoulliTable { get; } =
                    ResourceUnpack.NumTable(Resource.BernoulliTable)[nameof(BernoulliTable)];
            }
        }
    }
}
