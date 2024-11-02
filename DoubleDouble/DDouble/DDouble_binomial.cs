namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Binomial(int n, int k) {
            if (k < 0) {
                return 0d;
            }

            if (n < 0) {
                ddouble neg_binom = Binomial(checked(-n + k - 1), k);
                return ((k & 1) == 0) ? neg_binom : -neg_binom;
            }

            if (k > n / 2) {
                return Binomial(n, checked(n - k));
            }

            if (n > 1000) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Binomial function, n greater than 1000 is not supported."
                );
            }

            if (k == 0) {
                return 1d;
            }
            if (k == 1) {
                return n;
            }

            int i = 2;
            ddouble v = Ldexp(n, -64);

#if DEBUG
            checked {
#endif
                for (; i + 6 <= k; i += 6) {
                    v *= (long)((n - i + 1) * (n - i) * (n - i - 1)) * ((n - i - 2) * (n - i - 3) * (n - i - 4));
                    v /= (long)(i * (i + 1) * (i + 2)) * ((i + 3) * (i + 4) * (i + 5));
                }
                for (; i + 3 <= k; i += 3) {
                    v *= (n - i + 1) * (n - i) * (n - i - 1);
                    v /= i * (i + 1) * (i + 2);
                }
                for (; i <= k; i++) {
                    v *= n - i + 1;
                    v /= i;
                }

#if DEBUG
            }
#endif

            v = Round(Ldexp(v, 64));

            return v;
        }
    }
}