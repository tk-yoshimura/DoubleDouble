namespace DoubleDouble {

    public static class DoubleDoubleEnumerableExpand {
        public static ddouble Sum(this IEnumerable<ddouble> source) {
            ddouble acc = ddouble.Zero, carry = ddouble.Zero;

            foreach (var v in source) {
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
