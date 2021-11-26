namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ddouble PI = GeneratePI();
        public static readonly ddouble RcpPI = Rcp(PI);

        private static ddouble GeneratePI() {
            ddouble a = 1;
            ddouble b = Ldexp(Sqrt(2), -1);
            ddouble t = Ldexp(1, -2);
            ddouble p = 1;

            for (long i = 1; i <= 128; i *= 2) {
                ddouble a_next = (a + b) / 2;
                ddouble b_next = Sqrt(a * b);
                ddouble t_next = t - p * (a - a_next) * (a - a_next);
                ddouble p_next = p * 2;

                a = a_next;
                b = b_next;
                t = t_next;
                p = p_next;
            }

            ddouble c = a + b;
            ddouble y = c * c / (t * 4);

            return y;
        }
    }
}
