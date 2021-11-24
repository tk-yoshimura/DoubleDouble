using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Floor(ddouble v) {
            double nhi = Math.Floor(v.hi), nlo = Math.Floor(v.lo);

            return (nhi < v.hi) ? nhi : new ddouble(nhi, nlo);
        }

        public static ddouble Ceiling(ddouble v) {
            double nhi = Math.Ceiling(v.hi), nlo = Math.Ceiling(v.lo);

            return (nhi > v.hi) ? nhi : new ddouble(nhi, nlo);
        }

        public static ddouble Round(ddouble v) {
            return Floor(v + 0.5);
        }

        public static ddouble Truncate(ddouble v) {
            return (v < 0) ? Ceiling(v) : Floor(v);
        }
    }
}
