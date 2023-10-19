using System.Collections.ObjectModel;
using static DoubleDouble.ddouble.Consts.Airy;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble AiryAi(ddouble x) {
            if (Abs(x) > MaxRange) {
                return 0d;
            }

            ddouble v = Sqrt(Abs(x)), w = Ldexp(Cube(v) * Rcp3, 1);

            if (x >= NearZero) {
                return v * RcpPI * RcpSqrt3 * BesselK(Rcp3, w);
            }
            else if (x <= -NearZero) {
                return v * Rcp3 * (BesselJ(-Rcp3, w) + BesselJ(Rcp3, w));
            }
            else {
                ddouble s = TaylorNearZero[^1];
                for (int i = TaylorNearZero.Count - 2; i >= 0; i--) {
                    int m = i % 3;

                    if (m == 0) {
                        s = s * x + TaylorNearZero[i];
                    }
                    else if (m == 1) {
                        s = s * x - TaylorNearZero[i];
                    }
                    else {
                        s *= x;
                    }
                }

                s /= Cbrt3 * Cbrt3 * PI;

                return s;
            }
        }

        public static ddouble AiryBi(ddouble x) {
            if (Abs(x) > MaxRange) {
                return IsPositive(x) ? PositiveInfinity : 0d;
            }

            ddouble v = Sqrt(Abs(x)), w = Ldexp(Cube(v) * Rcp3, 1);

            if (x >= NearZero) {
                return v * RcpSqrt3 * (BesselI(-Rcp3, w) + BesselI(Rcp3, w));
            }
            else if (x <= -NearZero) {
                return v * RcpSqrt3 * (BesselJ(-Rcp3, w) - BesselJ(Rcp3, w));
            }
            else {
                ddouble s = TaylorNearZero[^1];
                for (int i = TaylorNearZero.Count - 2; i >= 0; i--) {
                    int m = i % 3;

                    if (m != 2) {
                        s = s * x + TaylorNearZero[i];
                    }
                    else {
                        s *= x;
                    }
                }

                s /= Sqrt(Cbrt3) * PI;

                return s;
            }
        }

        internal static partial class Consts {
            public static class Airy {
                public static ddouble Rcp3 { get; } = Rcp(3);
                public static ddouble RcpSqrt3 { get; } = Rcp(Sqrt(3));
                public static ddouble Cbrt3 { get; } = Cbrt(3);
                public static ddouble NearZero { get; } = double.ScaleB(1, -4);
                public static ddouble MaxRange { get; } = double.ScaleB(1, 128);

                public static readonly ReadOnlyCollection<ddouble> TaylorNearZero;

                static Airy() {
                    ddouble[] taylor_nz = new ddouble[17];

                    taylor_nz[0] = Gamma(Rcp3) * Sqrt(3) / 2d;
                    taylor_nz[1] = Gamma(Rcp3 * 2d) * Cbrt3 * Sqrt(3) / 2d;
                    taylor_nz[2] = 0d;

                    for (int k = 3; k < taylor_nz.Length; k++) {
                        taylor_nz[k] = taylor_nz[k - 3] / ((k - 1) * k);
                    }

                    TaylorNearZero = Array.AsReadOnly(taylor_nz);
                }
            }
        }
    }
}