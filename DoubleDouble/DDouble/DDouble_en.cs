namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble En(ddouble nu, ddouble x) {
            if (IsNaN(x) || IsNegative(x) || IsNaN(nu) || IsNegative(nu)) {
                return NaN;
            }

            if (IsZero(x)) {
                return PositiveInfinity;
            }

            ddouble t = Exp(-x);

            if (IsZero(t)) {
                return 0d;
            }

            (ddouble a0, ddouble a1) = (1d, 0d);
            (ddouble b0, ddouble b1) = (0d, 1d);

            (a0, a1) = (a1, a0 + x * a1);
            (b0, b1) = (b1, b0 + x * b1);

            ddouble s = a1 / b1;

            for (int k = 0; k < 4096; k++) {
                ddouble c = nu + k;

                (a0, a1) = (a1, c * a0 + a1);
                (b0, b1) = (b1, c * b0 + b1);
                (a0, a1) = (a1, (k + 1) * a0 + x * a1);
                (b0, b1) = (b1, (k + 1) * b0 + x * b1);

                s = a1 / b1;

                (int exp, (a1, b1)) = AdjustScale(0, (a1, b1));
                (a0, b0) = (Ldexp(a0, exp), Ldexp(b0, exp));

                if (k > 0 && (k & 3) == 0) {
                    ddouble r0 = a0 * b1, r1 = a1 * b0;
                    if (!(Abs(r0 - r1) > Min(Abs(r0), Abs(r1)) * 1e-30)) {
                        break;
                    }
                }
            }

            ddouble y = s * t;

            return y;
        }
    }
}