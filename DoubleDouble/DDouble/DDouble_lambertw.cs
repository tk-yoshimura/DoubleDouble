namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble LambertW(ddouble x) {
            if (IsZero(x)) {
                return Zero;
            }
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }
            if (x < -RcpE) {
                return NaN;
            }
            if (x == -RcpE) {
                return -1;
            }

            ddouble y;
            if (x < 8) {
                y = Log1p(x);
            }
            else {
                ddouble logx = Log(x), loglogx = Log(logx);

                y = logx - loglogx + loglogx / (2 * logx);
            }

            ddouble prev_dy = NaN, dy = NaN;

            while (!(ddouble.Abs(prev_dy) < ddouble.Abs(dy))) {
                prev_dy = dy;

                ddouble exp_y = Exp(y);
                ddouble d = y * exp_y - x;

                dy = d / (exp_y * (y + 1) - (y + 2) * d / (2 * y + 2));

                ddouble y_next = y - dy;

                if (y == y_next || !IsFinite(y)) {
                    break;
                }

                y = y_next;
            }

            return y;
        }
    }
}