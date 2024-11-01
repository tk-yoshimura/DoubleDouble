using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ddouble Ldexp(ddouble x, int n) {
            return new ddouble(x, n);
        }

        public static ddouble Ldexp(ddouble x, long n) => Ldexp(x, (int)long.Clamp(n, int.MinValue, int.MaxValue));

        public static ddouble ScaleB(ddouble x, int n) => Ldexp(x, n);

        public static ddouble ScaleB(ddouble x, long n) => Ldexp(x, n);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILogB(ddouble x) => double.ILogB(x.hi);
    }
}
