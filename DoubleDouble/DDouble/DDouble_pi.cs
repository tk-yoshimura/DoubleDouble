namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ddouble PI = GeneratePI();

        private static ddouble GeneratePI() {
            ddouble a = 1d;
            ddouble b = Ldexp(Consts.Sqrt.Sqrt2, -1);
            ddouble t = Ldexp(1d, -2);
            ddouble p = 1d;

            for (int i = 0; i < 4; i++) {
                ddouble a_next = Ldexp(a + b, -1);
                ddouble b_next = Sqrt(a * b);
                ddouble t_next = t - p * (a - a_next) * (a - a_next);
                ddouble p_next = Ldexp(p, 1);

                a = a_next;
                b = b_next;
                t = t_next;
                p = p_next;
            }

            ddouble c = a + b;
            ddouble y = c * c / Ldexp(t, 2);

            return y;
        }
    }
}
