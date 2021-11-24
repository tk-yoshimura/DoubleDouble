namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Pow(ddouble x, long n) {
            if (ddouble.IsNaN(x)) {
                return NaN;
            }

            if (n == 0) {
                return 1d;
            }

            ulong n_abs = UIntUtil.Abs(n);
            ddouble y = 1d, z = x;

            while (n_abs > 0) {
                if ((n_abs & 1) == 1) {
                    y *= z;
                }

                z *= z;
                n_abs >>= 1;
            }

            return (n > 0) ? y : Rcp(y);
        }
    }
}
