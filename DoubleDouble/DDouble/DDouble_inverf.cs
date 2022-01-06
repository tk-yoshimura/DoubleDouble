using System;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble InverseErf(ddouble x) {
            if (IsNaN(x) || x < -1d || x > 1d) {
                return NaN;
            }
            if (x.Sign < 0) {
                return -InverseErf(-x);
            }
            if (x == 1d) {
                return PositiveInfinity;
            }

            if (x < Math.ScaleB(1, -16)) {
                ddouble w = PI * x * x;
                ddouble t = Sqrt(PI) * ((40320d + w * (3360d + w * (588d + w * 127d))) / 80640d);

                return RoundMantissa(x * t, Consts.InverseErf.Precision); ;
            }

            ddouble y = (x < 0.5d) ? InverseErfRootFinding(x) : InverseErfcRootFinding(1 - x);

            return RoundMantissa(y, Consts.InverseErf.Precision);
        }

        public static ddouble InverseErfc(ddouble x) {
            if (IsNaN(x) || x < Zero || x > 2d) {
                return NaN;
            }
            if (x == Zero) {
                return PositiveInfinity;
            }
            if (x == 1) {
                return Zero;
            }

            ddouble y = (x >= 0.5d) ? InverseErf(1 - x) : InverseErfcRootFinding(x);

            return RoundMantissa(y, Consts.InverseErf.Precision);
        }

        private static ddouble InverseErfRootFinding(ddouble x) {
            ddouble s = 2d / Sqrt(PI);

            const double a = 0.147;

            ddouble lg = Log(1d - x * x);
            ddouble lga = 2d / (PI * a) + lg / 2;

            ddouble z = Sqrt(Sqrt(lga * lga - lg / a) - lga);

            ddouble erf(ddouble z) {
                return Erf(z) - x;
            };
            (ddouble, ddouble) derf(ddouble z) {
                ddouble d1 = Exp(-z * z) * s;
                ddouble d2 = -2 * z * d1;

                return (d1, d2);
            };

            for (int i = 0; i < 3; i++) {
                ddouble y = erf(z);
                (ddouble df1, ddouble df2) = derf(z);
                ddouble dz = (2 * y * df1) / ((2 * df1 * df1) - y * df2);

                z -= dz;
            }

            return z;
        }

        private static ddouble InverseErfcRootFinding(ddouble x) {
            ddouble s = 2d / Sqrt(PI);

            const double a = 0.147;

            ddouble lg = Log((2d - x) * x);
            ddouble lga = 2d / (PI * a) + lg / 2;

            ddouble z = Sqrt(Sqrt(lga * lga - lg / a) - lga);

            ddouble erfc(ddouble z) {
                return Erfc(z) - x;
            };
            (ddouble, ddouble) derfc(ddouble z) {
                ddouble d1 = -Exp(-z * z) * s;
                ddouble d2 = -2 * z * d1;

                return (d1, d2);
            };

            for (int i = 0; i < 3; i++) {
                ddouble y = erfc(z);
                (ddouble df1, ddouble df2) = derfc(z);
                ddouble dz = (2 * y * df1) / ((2 * df1 * df1) - y * df2);

                z -= dz;
            }

            return z;
        }

        internal static partial class Consts {
            public static class InverseErf {
                public const int Precision = 84;
            }
        }
    }
}