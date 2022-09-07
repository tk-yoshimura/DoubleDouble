using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class HurwitzZetaProto {
        public static ddouble HurwitzZeta(ddouble s, ddouble a) {
            if (!(s > 1)) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (!(a > 0)) {
                throw new ArgumentOutOfRangeException(nameof(a));
            }

            double a_convergence = 10d + 0.24d * (double)a + 1.35d * Math.Log2((double)a + 1d);

            ddouble y = 0d;
            while (a < a_convergence) {
                ddouble dy = Pow(a, -s);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    return y;
                }

                y = y_next;
                a += 1d;
            }

            ddouble r = Pow(a, s);
            ddouble u = s / (2 * a * r), a2 = a * a;

            y += (a / (s - 1d) + 0.5d) / r + u / 6;

            for (int k = 2; k < BernoulliSequence.Count - 1;) {
                u *= (s + (2 * k - 2)) * (s + (2 * k - 3)) / (a2 * checked(2 * k * (2 * k - 1)));
                ddouble dy0 = BernoulliSequence[k] * u;
                k++;

                u *= (s + (2 * k - 2)) * (s + (2 * k - 3)) / (a2 * checked(2 * k * (2 * k - 1)));
                ddouble dy1 = BernoulliSequence[k] * u;
                k++;

                ddouble dy = dy0 + dy1;
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                if (!(dy <= 0)) {
                    return NaN;
                }

                y = y_next;
            }

            return y;
        }
    }
}