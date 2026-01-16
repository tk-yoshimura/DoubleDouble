namespace DoubleDouble {

    public static class DoubleDoubleEnumerableExpand {
        public static ddouble Sum(this IEnumerable<ddouble> source) {
            ddouble acc = 0d, carry = 0d;

            foreach (ddouble v in source) {
                ddouble d = v - carry;
                ddouble acc_next = acc + d;

                carry = (acc_next - acc) - d;
                acc = acc_next;
            }

            return acc;
        }

        public static ddouble Average(this IEnumerable<ddouble> source) {
            return source.Sum() / source.Count();
        }

        public static ddouble Min(this IEnumerable<ddouble> source) {
            ddouble min = ddouble.NaN;

            foreach (ddouble v in source) {
                min = (min <= v) ? min : v;
            }

            return min;
        }

        public static ddouble Max(this IEnumerable<ddouble> source) {
            ddouble max = ddouble.NaN;

            foreach (ddouble v in source) {
                max = (max >= v) ? max : v;
            }

            return max;
        }

        public static int MinIndex(this IReadOnlyList<ddouble> source) {
            if (!source.Any()) {
                return -1;
            }

            ddouble min = ddouble.NaN;

            int index = 0, min_index = 0;
            foreach (ddouble v in source) {
                if (!(min <= v)) {
                    min = v;
                    min_index = index;
                }
                index++;
            }

            if (ddouble.IsNaN(min)) {
                return -1;
            }

            return min_index;
        }

        public static int MaxIndex(this IReadOnlyList<ddouble> source) {
            if (!source.Any()) {
                return -1;
            }

            ddouble max = ddouble.NaN;

            int index = 0, max_index = 0;
            foreach (ddouble v in source) {
                if (!(max >= v)) {
                    max = v;
                    max_index = index;
                }
                index++;
            }

            if (ddouble.IsNaN(max)) {
                return -1;
            }

            return max_index;
        }
    }
}
