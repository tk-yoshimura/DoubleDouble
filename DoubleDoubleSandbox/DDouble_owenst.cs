using DoubleDouble;
using System.Collections.ObjectModel;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class OwenT {

        internal static class OwenTPatefieldTandyAlgo {
            public static ddouble T1(ddouble h, ddouble a, int max_terms = 128) { 
                ddouble h2 = h * h, a2 = a * a;

                ddouble n_half_h2 = -h2 / 2;

                ddouble ap = a / (2 * PI);
                ddouble c = Expm1(n_half_h2);
                ddouble b = n_half_h2 * Exp(n_half_h2);
                ddouble s = Atan(a) / (2 * PI);

                for (int k = 1, conv_times = 0; k < max_terms && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    ddouble u = c * ap / (2 * k - 1);
                    
                    s += u;
                    ap *= a2;
                    c = b - c;
                    b *= n_half_h2 / (k + 1);

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= max_terms - 1) {
                        return NaN;
                    }
                }

                return s;
            }

            public static ddouble T2(ddouble h, ddouble a, int max_terms = 128) {
                ddouble h2 = h * h, na2 = -a * a, ha = h * a;
                
                ddouble v = 1d / h2;
                ddouble w = a * Exp(-ha * ha / 2) / Sqrt(2 * PI);
                ddouble u = Erf(ha / Sqrt2) / (2 * h);
                ddouble s = u;

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    u = v * (w - (2 * k + 1) * u);
                    s += u;
                    w *= na2;

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= max_terms - 1) {
                        return NaN;
                    }
                }

                ddouble y = s * Exp(-h2 / 2) / Sqrt(2 * PI);

                return y;
            }

            public static ddouble T3(ddouble h, ddouble a) {
                ddouble h2 = h * h, a2 = a * a, ha = h * a;

                ddouble v = 1d / h2;
                ddouble w = a * Exp(-ha * ha / 2) / Sqrt(2 * PI);
                ddouble u = Erf(ha / Sqrt2) / (2 * h);
                ddouble s = 0;

                for (int k = 0; k < T3CoefTable.Count; k++) {
                    s += u * T3CoefTable[k];
                    u = v * ((2 * k + 1) * u - w);
                    w *= a2;
                }

                ddouble y = s * Exp(-h2 / 2) / Sqrt(2 * PI);

                return y;
            }

            public static ddouble T4(ddouble h, ddouble a, int max_terms = 128) {
                ddouble h2 = h * h, na2 = -a * a;
                
                ddouble v = a * Exp(h2 * (na2 - 1d) / 2) / (2 * PI);
                ddouble w = 1d, u = v;
                ddouble s = v;

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2 && ddouble.IsFinite(s); k++) {
                    w = (1d - h2 * w) / (2 * k + 3);
                    v *= na2;

                    u = v * w;
                    s += u;

                    if (Abs(u) <= Abs(s) * 1e-31) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }
                    if (k >= max_terms - 1) {
                        return NaN;
                    }
                }

                return s;
            }

            public static ReadOnlyCollection<ddouble> T3CoefTable = new(new ddouble[] {
                (+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFCBC52A8E36434uL),
                (-1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xE6D51B6261D75BD7uL),
                (+1, -1, 0xFFFFFFFFFFFFFFDFuL, 0xB0BE8F85206AF403uL),
                (-1, -1, 0xFFFFFFFFFFFFEF74uL, 0x5E5C7544ECDFAF5FuL),
                (+1, -1, 0xFFFFFFFFFFFB7B64uL, 0x4063E5D27BADCF4AuL),
                (-1, -1, 0xFFFFFFFFFF3CC216uL, 0x21B631C90F6B21F2uL),
                (+1, -1, 0xFFFFFFFFE9B7866DuL, 0xD9652886CAA08DC8uL),
                (-1, -1, 0xFFFFFFFE2CA96DCEuL, 0xC12CC82B03BEB375uL),
                (+1, -1, 0xFFFFFFE35225951CuL, 0xBCD87384ED69DBFFuL),
                (-1, -1, 0xFFFFFEA3A52ADB2AuL, 0x86257377419DEC02uL),
                (+1, -1, 0xFFFFF2E7D6F3623BuL, 0xBDD0006EB5E6130BuL),
                (-1, -1, 0xFFFF9A3F9CD68194uL, 0x5B9AA0865FA3C7C8uL),
                (+1, -1, 0xFFFD77A92F437A2FuL, 0x81CD6910CF39E313uL),
                (-1, -1, 0xFFF29A20A0AE5155uL, 0x367CF0DB4F6946D4uL),
                (+1, -1, 0xFFC48B8920ECCEC2uL, 0xEE7110F3DA9A6694uL),
                (-1, -1, 0xFF20C613144EF7DAuL, 0x5620F114BBB193ABuL),
                (+1, -1, 0xFD35AB388FCA3592uL, 0x5035544A7B58B1EAuL),
                (-1, -1, 0xF857118155EAE7CCuL, 0xF63A7216309AF0A5uL),
                (+1, -1, 0xEDDA70C1B0727C98uL, 0xFD9BA1A6C21C65D5uL),
                (-1, -1, 0xDAAD3C61F6E38F0CuL, 0xEE300749D956F198uL),
                (+1, -1, 0xBCEB4A92B3E8A433uL, 0xDEF87A69E178BFFCuL),
                (-1, -1, 0x95D1F0F3F3C855A9uL, 0x62C1A3B97CC83CE0uL),
                (+1, -2, 0xD4FDA9178241DA4FuL, 0x4F1DB50DAF5F76C9uL),
                (-1, -2, 0x84843294DCF687A8uL, 0x2FBDBCDEC70EEA21uL),
                (+1, -3, 0x8CDAD8A96C0FDDDEuL, 0x0248A1EA0F8A819FuL),
                (-1, -5, 0xF90477CE365BA548uL, 0x8E222D6FD2070ACCuL),
                (+1, -6, 0xB1394CEEEDDB8ECEuL, 0xFF1152E160159170uL),
                (-1, -8, 0xC2727A3BB658C758uL, 0x93EE9DADFC7262E1uL),
                (+1, -10, 0x99EC5C8482C48A59uL, 0x5095CC927830230FuL),
                (-1, -13, 0x9C263773EA23F0EDuL, 0x1F388E23A84E0048uL),
                (+1, -17, 0x98357711280A8F66uL, 0x22575F71931470C1uL),
            });
        }
    }
}