namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseLowerIncompleteGamma(ddouble nu, ddouble x) {
            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x > 0.5d) {
                return InverseUpperIncompleteGamma(nu, 1d - x);
            }

            if (x == 0d) {
                return Zero;
            }

            ddouble u = Log(x), lngamma = LogGamma(nu);
            ddouble prev_dv = 1;

            ddouble v = x > 1e-64
                ? nu 
                : nu > 1d / 64
                ? Exp((Log(nu) + u + lngamma) / nu)
                : Pow(x, 1d / nu);

            for (int i = 0; i < 16; i++) {
                ddouble lnv = Log(v);
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, v);
                ddouble y = nu * lnv - (v + lngamma) - Log(f);

                ddouble g1 = f / v;
                ddouble g2 = g1 * (nu - 1d - v - f) / v;
                ddouble delta = y - u;

                ddouble dv_raw = -Ldexp(delta * g1, 1) / (delta * g2 - Ldexp(g1 * g1, 1));
                ddouble dv = Ldexp(delta * v, 1) / (delta * (v + f - nu + 1d) + Ldexp(f, 1));

                ddouble c = Exp(y);
                ddouble p = LowerIncompleteGammaRegularized(nu, v);

                if (IsNaN(dv)) {
                    break;
                }
                if (Sign(dv) != Sign(prev_dv)) {
                    dv /= 2;
                }

                v = Max(Ldexp(v, -6), v - dv);

                if (double.Abs(dv.hi) <= double.Abs(v.hi) * 5e-32) {
                    break;
                }
            }

            return v;
        }

        public static ddouble InverseUpperIncompleteGamma(ddouble nu, ddouble x) {
            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x > 0.5d) {
                return InverseLowerIncompleteGamma(nu, 1d - x);
            }

            if (x == 0d) {
                return PositiveInfinity;
            }

            ddouble u = Log2(x), lngamma = LogGamma(nu);
            ddouble v0 = nu;

            throw new NotImplementedException();
        }
    }
}