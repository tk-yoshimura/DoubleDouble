namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Binomial(int n, int k) {
            if (k > n / 2) {
                return Binomial(n, (n - k));
            }

            if (n < 0 || k > n || k < 0) {
                throw new ArgumentOutOfRangeException($"{nameof(n)},{nameof(k)}");
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

            ddouble v = n;
            for (int i = 2; i <= k; i++) {
                v *= (double)(n - i + 1);
                v /= i;
            }

            v = Round(v);

            return v;
        }
    }
}