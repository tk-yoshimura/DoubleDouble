namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble HurwitzZeta(ddouble x, ddouble a) {
            if (x < 1) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
            if (a < 0) {
                throw new ArgumentOutOfRangeException(nameof(a));
            }

            if (IsNaN(x) || IsNaN(a)) {
                return NaN;
            }
            if (x == 1d || a == 0d) {
                return PositiveInfinity;
            }
            if (a == 1d) {
                return RiemannZeta(x);
            }
            if (IsInfinity(x)) {
                return (a < 1d) ? PositiveInfinity : Zero;
            }

            double a_convergence = 12d + 0.24d * (double)a + 1.35d * Math.Log2((double)a + 1d);

            ddouble y = 0d;
            while (a < a_convergence) {
                ddouble dy = Pow(a, -x);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    return y;
                }

                y = y_next;
                a += 1d;
            }

            ddouble r = Pow(a, x);
            ddouble u = x / (2 * a * r), a2 = a * a;

            y += (a / (x - 1d) + 0.5d) / r + u / 6;

            for (int k = 2; k < BernoulliSequence.Count - 1;) {
                u *= (x + (2 * k - 2)) * (x + (2 * k - 3)) / (a2 * (2 * k * (2 * k - 1)));
                ddouble dy0 = BernoulliSequence[k] * u;
                k++;

                u *= (x + (2 * k - 2)) * (x + (2 * k - 3)) / (a2 * (2 * k * (2 * k - 1)));
                ddouble dy1 = BernoulliSequence[k] * u;
                k++;

                ddouble dy = dy0 + dy1;
                ddouble y_next = y + dy;

                if (y == y_next || !(dy <= 0)) {
                    break;
                }

                y = y_next;
            }

            return y;
        }
    }
}