namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseLowerIncompleteGamma(ddouble nu, ddouble x) {
            if (!(x >= 0d && x <= 1d)) {
                return NaN;
            }

            if (x == 1d) {
                return PositiveInfinity;
            }

            if (x > 0.5d) {
                return InverseUpperIncompleteGamma(nu, 1d - x);
            }

            ddouble u = Log2(x), lngamma = LogGamma(nu);
            ddouble v = nu;

            for (int i = 0; i < 32; i++) {
                ddouble lnv = Log2(v);
                ddouble f = LowerIncompleteGammaCFrac.Value(nu, v);
                ddouble y = nu * lnv - (v + lngamma) * LbE - Log2(f);
                ddouble g = Exp(-v) * Pow(v, nu - 1d) / LowerIncompleteGamma(nu, v) * LbE;
                ddouble dv = (y - u) / g;

                ddouble c = Pow2(y);
                ddouble p = LowerIncompleteGammaRegularized(nu, v);

                Console.WriteLine($"{v},{y},{g}");

                v = Max(0d, v - dv);
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

            ddouble u = Log2(x), lngamma = LogGamma(nu);
            ddouble v0 = nu;

            throw new NotImplementedException();
        }
    }
}