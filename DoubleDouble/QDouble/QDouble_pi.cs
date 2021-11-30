namespace DoubleDouble {
    internal partial struct qdouble {
        public static readonly qdouble PI = GeneratePI();

        private static qdouble GeneratePI() {
            qdouble a = 1;
            qdouble b = Ldexp(Sqrt(2), -1);
            qdouble t = Ldexp(1, -2);
            qdouble p = 1;

            for (int i = 0; i < 8; i++) {
                qdouble a_next = Ldexp(a + b, -1);
                qdouble b_next = Sqrt(a * b);
                qdouble t_next = t - p * (a - a_next) * (a - a_next);
                qdouble p_next = Ldexp(p, 1);

                a = a_next;
                b = b_next;
                t = t_next;
                p = p_next;
            }

            qdouble c = a + b;
            qdouble y = c * c / Ldexp(t, 2);

            return y;
        }
    }
}
