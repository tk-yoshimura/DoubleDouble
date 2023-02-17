using DoubleDouble.Utils;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            if (x <= -RcpE) {
                return (x <= -RcpE - Math.ScaleB(1, -104)) ? NaN : -1;
            }

            ddouble y;
            if (x >= -0.303265) {
                double xd = x.hi, yd;

                if (x < 8) {
                    yd = xd * (60.0 + xd * (114.0 + xd * 17.0)) / (60.0 + xd * (174.0 + xd * 101.0));
                }
                else {
                    double logx = Math.Log(xd), loglogx = Math.Log(logx);

                    yd = logx - loglogx + loglogx / (logx + logx);
                }

                {
                    double exp_y, d, dy;

                    for (int i = 0; i < 4; i++) {
                        exp_y = Math.Exp(yd);
                        d = yd * exp_y - xd;
                        dy = d / (exp_y * (yd + 1d) - (yd + 2d) * d / (yd + yd + 2d));

                        if (!double.IsFinite(dy)) {
                            break;
                        }

                        yd -= dy;

                        if (Math.Abs(dy) <= Math.Abs(yd) * 1e-15) {
                            break;
                        }
                    }
                }

                y = yd;
                {
                    ddouble exp_y, d, dy;

                    exp_y = Exp(y);
                    d = y * exp_y - x;
                    dy = d / (exp_y * (y + 1d) - (y + 2d) * d / (y + y + 2d));
                    y -= dy;
                }
            }
            else {
                ddouble v = Sqrt(2 * (x * E + 1));

                if (v >= 1e-8) {
                    ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LambertW.NearSingularPadeTable;

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

#if DEBUG
                    Trace.Assert(sd > 0.0625d, $"[LambertW x={x}] Too small pade denom!!");
#endif

                    y = sc / sd;
                }
                else {
                    y = (-1080.0 + v * (1080.0 + v * (-360.0 + v * (165.0 + v * -86.0)))) / 1080;
                }
            }

            return y;
        }

        internal static partial class Consts {
            public static class LambertW {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> NearSingularPadeTable;

                static LambertW() {
                    NearSingularPadeTable =
                        ResourceUnpack.NumTableX2(Resource.LambertWTable, reverse: true)[nameof(NearSingularPadeTable)];
                }
            }
        }
    }
}