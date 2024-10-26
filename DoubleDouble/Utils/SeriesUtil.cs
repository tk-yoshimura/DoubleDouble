using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal static class SeriesUtil {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Add(ddouble c, ddouble s, ddouble a, out bool convergence) {
            ddouble c_add = c + s * a;

            convergence = c == c_add;

            return c_add;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Add(ddouble c, ddouble s, ddouble a, ddouble b, out bool convergence) {
            ddouble c_add = c + s * (a + b);

            if (c != c_add) {
                convergence = false;
                return c_add;
            }

            ddouble c_sub = c + s * (a - b);

            convergence = c == c_sub;

            return c_add;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble UnScaledAdd(ddouble c, ddouble a, out bool convergence) {
            ddouble c_add = c + a;

            if (c != c_add) {
                convergence = false;
                return c_add;
            }

            convergence = c == c_add;

            return c_add;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble UnScaledAdd(ddouble c, ddouble a, ddouble b, out bool convergence) {
            ddouble c_add = c + (a + b);

            if (c != c_add) {
                convergence = false;
                return c_add;
            }

            ddouble c_sub = c + (a - b);

            convergence = c == c_sub;

            return c_add;
        }
    }
}
