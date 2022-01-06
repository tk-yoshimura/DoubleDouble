using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble AiryAi(ddouble x) {
            if (Abs(x) > Consts.Airy.MaxRange) {
                return Zero;
            }

            ddouble v = Sqrt(Abs(x)), w = 2 * v * v * v * Consts.Airy.Rcp3;

            if (x >= Consts.Airy.NearZero) {
                return v * RcpPI * Consts.Airy.RcpSqrt3 * BesselK(Consts.Airy.Rcp3, w);
            }
            else if (x <= -Consts.Airy.NearZero) {
                return v * Consts.Airy.Rcp3 * (BesselJ(-Consts.Airy.Rcp3, w) + BesselJ(Consts.Airy.Rcp3, w));
            }
            else {
                ddouble s = Consts.Airy.TaylorNearZero[^1];
                for (int i = Consts.Airy.TaylorNearZero.Count - 2; i >= 0; i--) {
                    int m = i % 3;

                    if (m == 0) {
                        s = s * x + Consts.Airy.TaylorNearZero[i];
                    }
                    else if (m == 1) {
                        s = s * x - Consts.Airy.TaylorNearZero[i];
                    }
                    else {
                        s *= x;
                    }
                }

                s /= Consts.Airy.Cbrt3 * Consts.Airy.Cbrt3 * PI;

                return s;
            }
        }

        public static ddouble AiryBi(ddouble x) {
            if (Abs(x) > Consts.Airy.MaxRange) {
                return x.Sign > 0 ? PositiveInfinity : Zero;
            }

            ddouble v = Sqrt(Abs(x)), w = 2 * v * v * v * Consts.Airy.Rcp3;

            if (x >= Consts.Airy.NearZero) {
                return v * Consts.Airy.RcpSqrt3 * (BesselI(-Consts.Airy.Rcp3, w) + BesselI(Consts.Airy.Rcp3, w));
            }
            else if (x <= -Consts.Airy.NearZero) {
                return v * Consts.Airy.RcpSqrt3 * (BesselJ(-Consts.Airy.Rcp3, w) - BesselJ(Consts.Airy.Rcp3, w));
            }
            else {
                ddouble s = Consts.Airy.TaylorNearZero[^1];
                for (int i = Consts.Airy.TaylorNearZero.Count - 2; i >= 0; i--) {
                    int m = i % 3;

                    if (m != 2) {
                        s = s * x + Consts.Airy.TaylorNearZero[i];
                    }
                    else {
                        s *= x;
                    }
                }

                s /= Sqrt(Consts.Airy.Cbrt3) * PI;

                return s;
            }
        }

        internal static partial class Consts {
            public static class Airy {
                public static ddouble Rcp3 { get; } = Rcp(3);
                public static ddouble RcpSqrt3 { get; } = Rcp(Sqrt(3));
                public static ddouble Cbrt3 { get; } = Cbrt(3);
                public static ddouble NearZero { get; } = Math.ScaleB(1, -4);
                public static ddouble MaxRange { get; } = Math.ScaleB(1, 128);

                public static ReadOnlyCollection<ddouble> TaylorNearZero;

                static Airy() {
                    ddouble[] taylor_nz = new ddouble[17];

                    taylor_nz[0] = Gamma(Rcp3) * Sqrt(3) / 2;
                    taylor_nz[1] = Gamma(Rcp3 * 2) * Cbrt3 * Sqrt(3) / 2;
                    taylor_nz[2] = 0;

                    for (int k = 3; k < taylor_nz.Length; k++) {
                        taylor_nz[k] = taylor_nz[k - 3] / ((k - 1) * k);
                    }

                    TaylorNearZero = Array.AsReadOnly(taylor_nz);
                }
            }
        }
    }
}