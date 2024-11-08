using System.Runtime.CompilerServices;

namespace DoubleDouble {
    public partial struct ddouble {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int exp, ddouble value) Frexp(ddouble x) {
            if (!IsFinite(x)) {
                return (0, NaN);
            }
            if (IsZero(x)) {
                return (0, IsPositive(x) ? 0d : -0d);
            }

            int n = ILogB(x);
            ddouble f = new ddouble(double.ScaleB(x.hi, -n), double.ScaleB(x.lo, -n));

            if (f.hi == 1d && f.lo < 0d) {
                n -= 1;
                f = new ddouble(2d, double.ScaleB(f.lo, 1));
            }

            return (n, f);
        }

        public static (int exp, ddouble x) AdjustScale(int exp, ddouble x) {
            if (!IsFinite(x)) {
                return (0, NaN);
            }
            if (IsZero(x)) {
                return (0, IsPositive(x) ? 0d : -0d);
            }

            int n = (exp - ILogB(x));
            ddouble v = Ldexp(x, n);

            return (n, v);
        }

        public static (int exp, (ddouble a, ddouble b) scaled) AdjustScale(int exp, (ddouble a, ddouble b) v) {
            ddouble x = MaxMagnitude(v.a, v.b);

            if (!IsFinite(x)) {
                return (0, (NaN, NaN));
            }
            if (IsZero(x)) {
                return (0,
                    (IsPositive(v.a) ? 0d : -0d,
                     IsPositive(v.b) ? 0d : -0d)
                );
            }

            int n = (exp - ILogB(x));

            return (n, (Ldexp(v.a, n), Ldexp(v.b, n)));
        }

        public static (int exp, (ddouble a, ddouble b, ddouble c) scaled) AdjustScale(int exp, (ddouble a, ddouble b, ddouble c) v) {
            ddouble x = MaxMagnitude(v.a, MaxMagnitude(v.b, v.c));

            if (!IsFinite(x)) {
                return (0, (NaN, NaN, NaN));
            }
            if (IsZero(x)) {
                return (0,
                    (IsPositive(v.a) ? 0d : -0d,
                     IsPositive(v.b) ? 0d : -0d,
                     IsPositive(v.c) ? 0d : -0d)
                );
            }

            int n = (exp - ILogB(x));

            return (n, (Ldexp(v.a, n), Ldexp(v.b, n), Ldexp(v.c, n)));
        }

        public static (int exp, (ddouble a, ddouble b, ddouble c, ddouble d) scaled) AdjustScale(int exp, (ddouble a, ddouble b, ddouble c, ddouble d) v) {
            ddouble x = MaxMagnitude(MaxMagnitude(v.a, v.b), MaxMagnitude(v.c, v.d));

            if (!IsFinite(x)) {
                return (0, (NaN, NaN, NaN, NaN));
            }
            if (IsZero(x)) {
                return (0,
                    (IsPositive(v.a) ? 0d : -0d,
                     IsPositive(v.b) ? 0d : -0d,
                     IsPositive(v.c) ? 0d : -0d,
                     IsPositive(v.d) ? 0d : -0d)
                );
            }

            int n = (exp - ILogB(x));

            return (n, (Ldexp(v.a, n), Ldexp(v.b, n), Ldexp(v.c, n), Ldexp(v.d, n)));
        }
    }
}
