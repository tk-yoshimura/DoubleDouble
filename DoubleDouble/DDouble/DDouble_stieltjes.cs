using DoubleDouble.Utils;
using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {

    public partial struct ddouble {
        public static ReadOnlyCollection<ddouble> StieltjesGamma => Consts.StieltjesGamma.StieltjesGammaTable;

        internal static partial class Consts {
            public static class StieltjesGamma {
                public static ReadOnlyCollection<ddouble> StieltjesGammaTable { get; } = 
                    ResourceUnpack.NumTable(Resource.StieltjesGammaTable)[nameof(StieltjesGammaTable)];
            }
        }
    }
}
