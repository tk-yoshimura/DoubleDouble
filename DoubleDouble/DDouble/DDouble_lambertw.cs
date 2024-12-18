﻿using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static DoubleDouble.ddouble.Consts.LambertW;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble LambertW(ddouble x) {
            if (IsZero(x)) {
                return 0d;
            }
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }
            if (x <= -RcpE) {
                return (x <= -RcpE - double.ScaleB(1, -104)) ? NaN : -1d;
            }

            ddouble y;
            if (x >= -0.303265) {
                double xd = x.hi, yd;

                if (x < 8d) {
                    yd = xd * (60d + xd * (114d + xd * 17d)) / (60d + xd * (174d + xd * 101d));
                }
                else {
                    double logx = double.Log(xd), loglogx = double.Log(logx);

                    yd = logx - loglogx + loglogx / (logx + logx);
                }

                {
                    double exp_y, d, dy;

                    for (int i = 0; i < 4; i++) {
                        exp_y = double.Exp(yd);
                        d = yd * exp_y - xd;
                        dy = d / (exp_y * (yd + 1d) - (yd + 2d) * d / (yd + yd + 2d));

                        if (!double.IsFinite(dy)) {
                            break;
                        }

                        yd -= dy;

                        if (double.Abs(dy) <= double.Abs(yd) * 1e-15) {
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
                ddouble v = Sqrt(Ldexp(x * E + 1d, 1));

                if (v >= 1e-8d) {
                    ReadOnlyCollection<(ddouble c, ddouble d)> table = NearSingularPadeTable;

                    (ddouble sc, ddouble sd) = table[0];
                    for (int i = 1; i < table.Count; i++) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    Debug.Assert(sd > 0.5d, $"[LambertW x={x}] Too small pade denom!!");

                    y = sc / sd;
                }
                else {
                    y = (-1080d + v * (1080d + v * (-360d + v * (165d + v * -86d)))) / 1080d;
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