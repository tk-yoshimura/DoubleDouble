using DoubleDouble;

namespace DoubleDoubleSandbox {
    static class ErfcContinuedFraction {

        public static ddouble Erfc(ddouble z, long frac_n, ddouble f_init) {
            ddouble c = z * ddouble.Exp(-z * z) / ddouble.Sqrt(ddouble.PI);
            ddouble f = Frac(z, frac_n, f_init);

            return c * f;
        }

        public static ddouble Frac(ddouble z, long n, ddouble f_init) {
            ddouble w = z * z;

            //ddouble f = 
            //    (ddouble.Sqrt(25 + w * (440 + w * (488 + w * 16 * (10 + w))))
            //     - 5 + w * 4 * (1 + w))
            //    / (20 + w * 8);

            ddouble f = f_init;

            for (long k = checked(4 * n - 3); k >= 1; k -= 4) {
                ddouble c0 = (k + 2) * f;
                ddouble c1 = w * ((k + 3) + ddouble.Ldexp(f, 1));
                ddouble d0 = checked((k + 1) * (k + 3)) + (4 * k + 6) * f;
                ddouble d1 = ddouble.Ldexp(c1, 1);

                f = w + k * (c0 + c1) / (d0 + d1);
            }

            ddouble y = 1 / f;

            return y;
        }
    }
}
