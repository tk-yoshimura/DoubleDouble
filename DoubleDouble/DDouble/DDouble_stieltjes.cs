using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {

    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ReadOnlyCollection<ddouble> StieltjesGamma => Consts.StieltjesGamma.StieltjesGammaTable;

        internal static partial class Consts {
            public static class StieltjesGamma {
                public static ReadOnlyCollection<ddouble> StieltjesGammaTable { get; } =
                    ResourceUnpack.NumTable(Resource.StieltjesGammaTable)[nameof(StieltjesGammaTable)];
            }
        }
    }
}
