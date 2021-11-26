using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Pow(ddouble x, long n) {
            if (IsNaN(x)) {
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

        public static ddouble Pow2(ddouble x) { 
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (IsInfinity(x)) {
                return (x.Sign < 0) ? 0 : PositiveInfinity;
            }
            if (x >= 1024) {
                return PositiveInfinity;
            }

            int exp = (int)Floor(x);
            ddouble v = (x - exp) * Consts.Log.Ln2;

            ddouble y = 1, c = Ldexp(1, exp);
            ddouble w = v;

            foreach (ddouble f in TaylorSequence.Skip(1)) {
                ddouble dy = f * w;
                ddouble y_next = y + dy;
                
                if (y == y_next) {
                    break;
                }

                w *= v;
                y = y_next;
            }

            return c * y;
        }
    }
}
