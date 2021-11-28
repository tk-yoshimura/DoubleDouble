using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {

    public static class DoubleDoubleEnumerableExpand {
        public static ddouble Sum(this IEnumerable<ddouble> source) {
            ddouble sum = ddouble.Zero, c = ddouble.Zero;

            foreach (var v in source) {
                ddouble y = v - c;
                ddouble t = sum + y;
                c = (t - sum) - y;
                sum = t;
            }

            return sum;
        }

        public static ddouble Average(this IEnumerable<ddouble> source) {
            return source.Sum() / source.Count();
        }

        public static ddouble Min(this IEnumerable<ddouble> source) {
            ddouble min = ddouble.NaN;

            foreach (var v in source) {
                min = !(min <= v) ? v : min;
            }

            return min;
        }

        public static ddouble Max(this IEnumerable<ddouble> source) {
            ddouble max = ddouble.NaN;

            foreach (var v in source) {
                max = !(max >= v) ? v : max;
            }

            return max;
        }
    }
}
