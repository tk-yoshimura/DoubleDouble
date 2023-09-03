using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble :
        IAdditiveIdentity<ddouble, ddouble>,
        IMultiplicativeIdentity<ddouble, ddouble>,

        IAdditionOperators<ddouble, ddouble, ddouble>,
        ISubtractionOperators<ddouble, ddouble, ddouble>,
        IMultiplyOperators<ddouble, ddouble, ddouble>,
        IDivisionOperators<ddouble, ddouble, ddouble>,

        IUnaryPlusOperators<ddouble, ddouble>,
        IUnaryNegationOperators<ddouble, ddouble>,

        IEquatable<ddouble>,
        IEqualityOperators<ddouble, ddouble, bool>,
        IEqualityComparer<ddouble>,
        IComparisonOperators<ddouble, ddouble, bool>,
        IMinMaxValue<ddouble> {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble AdditiveIdentity => ddouble.Zero;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MultiplicativeIdentity => ddouble.One;

        public bool Equals(ddouble x, ddouble y) {
            return x == y;
        }

        public int GetHashCode([DisallowNull] ddouble obj) {
            return obj.GetHashCode();
        }
    }
}