using System.Collections.ObjectModel;
using static DoubleDouble.ddouble.Consts.Airy;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble AiryAi(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x >= AiryAiUnderflow || IsNegativeInfinity(x)) {
                return 0d;
            }

            ddouble x_abs = Abs(x);

            if (x_abs < NearZero) {
                ddouble x2 = x * x;
                ddouble s = x * NearZeroCoefs[0] + NearZeroCoefs[1];

                for (int i = 2; i + 1 < NearZeroCoefs.Count; i += 2) {
                    s = s * x2 - NearZeroCoefs[i];
                    s = s * x + NearZeroCoefs[i + 1];
                }

                s /= AiNearZeroC;

                return s;
            }
            else {
                ddouble v = Sqrt(x_abs), w = Ldexp(Cube(v) * Rcp3, 1);

                if (IsNegative(x)) {
                    ddouble y = v * Rcp3 * (BesselJ(-Rcp3, w) + BesselJ(Rcp3, w));
                    return y;
                }
                else {
                    ddouble y = v * RcpPI * RcpSqrt3 * BesselK(Rcp3, w);
                    return y;
                }
            }
        }

        public static ddouble AiryBi(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x >= AiryBiOverflow) {
                return PositiveInfinity;
            }

            if (IsNegativeInfinity(x)) {
                return 0d;
            }

            ddouble x_abs = Abs(x);

            if (x_abs < NearZero) {
                ddouble x2 = x * x;
                ddouble s = x * NearZeroCoefs[0] + NearZeroCoefs[1];

                for (int i = 2; i + 1 < NearZeroCoefs.Count; i += 2) {
                    s = s * x2 + NearZeroCoefs[i];
                    s = s * x + NearZeroCoefs[i + 1];
                }

                s /= BiNearZeroC;

                return s;
            }
            else {
                ddouble v = Sqrt(x_abs), w = Ldexp(Cube(v) * Rcp3, 1);

                if (IsNegative(x)) {
                    ddouble y = v * RcpSqrt3 * (BesselJ(-Rcp3, w) - BesselJ(Rcp3, w));
                    return y;
                }
                else {
                    ddouble y = v * RcpSqrt3 * (BesselI(-Rcp3, w) + BesselI(Rcp3, w));
                    return y;
                }
            }
        }

        internal static partial class Consts {
            public static class Airy {
                public static ddouble Rcp3 { get; } = Rcp(3);
                public static ddouble RcpSqrt3 { get; } = Rcp(Sqrt(3));
                public static ddouble Cbrt3 { get; } = Cbrt(3);
                public static ddouble NearZero { get; } = double.ScaleB(1, -4);

                public static double AiryAiUnderflow = 108d, AiryBiOverflow = 105d;

                public static ddouble AiNearZeroC = Cbrt3 * Cbrt3 * PI, BiNearZeroC = Sqrt(Cbrt3) * PI;

                public static ddouble Gamma1d3 = (+1, 1, 0xAB73BA9CA4178B3BuL, 0xB234FA4B356011B6uL);
                public static ddouble Gamma2d3 = (+1, 0, 0xAD53BC9461B3C655uL, 0x97A7F5B815934C85uL);

                public static readonly ReadOnlyCollection<ddouble> NearZeroCoefs;

                static Airy() {
                    NearZeroCoefs = Array.AsReadOnly(GenerateNearZeroCoefs());
                }

                private static ddouble[] GenerateNearZeroCoefs() {
                    ddouble[] coefs = new ddouble[17];

                    coefs[0] = Ldexp(Gamma1d3 * Sqrt(3), -1);
                    coefs[1] = Ldexp(Gamma2d3 * Cbrt3 * Sqrt(3), -1);
                    coefs[2] = 0d;

                    for (int k = 3; k < coefs.Length; k++) {
                        coefs[k] = coefs[k - 3] / ((k - 1) * k);
                    }

                    coefs = coefs.Where(v => !IsZero(v)).Reverse().ToArray();

                    return coefs;
                }
            }
        }
    }
}