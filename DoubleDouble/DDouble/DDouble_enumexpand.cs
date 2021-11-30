using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {

    public static class DoubleDoubleEnumerableExpand {
        public static ddouble Sum(this IEnumerable<ddouble> source) {
            Accumulator acc = 0d;

            foreach (var v in source) {
                acc += v;
            }

            return acc.Sum;
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
