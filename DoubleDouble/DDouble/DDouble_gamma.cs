using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Gamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble sinpi = SinPI(x);

                if (IsZero(sinpi)) {
                    return NaN;
                }

                ddouble y = PI / (sinpi * Gamma(1d - x));

                return y;
            }

            if (x <= Consts.Gamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 1));
                ddouble v = x - n - 1;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Gamma.PadeTables[n];

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                ddouble y = sc / sd;

                return y;
            }
            else {
                ddouble r = Sqrt(2 * PI / x);
                ddouble p = Pow(x / E, x);
                ddouble s = Exp(SterlingTerm(x));

                ddouble y = r * p * s;

                if (x < 21.5d) {
                    return ddouble.RoundMantissa(y, 99);
                }
                if (x < 28.5d) {
                    return ddouble.RoundMantissa(y, 98);
                }

                return ddouble.RoundMantissa(y, 97);
            }
        }

        public static ddouble LogGamma(ddouble x) {
            if (IsNaN(x) || IsMinusZero(x) || x.Sign < 0) {
                return NaN;
            }
            if (IsPlusZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                return Log(Gamma(x));
            }

            if (x >= 1d - Math.ScaleB(1, -3) && x <= 1d + Math.ScaleB(1, -3)) {
                x -= 1d;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.TaylorX1CoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x + table[i];
                }
                s *= x;

                return s;
            }
            if (x >= 2d - Math.ScaleB(1, -3) && x <= 2d + Math.ScaleB(1, -3)) {
                x -= 2d;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.TaylorX2CoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x + table[i];
                }
                s *= x;

                return s;
            }

            static ddouble sterling_loggamma(ddouble x) {
                ddouble p = (x - 0.5d) * ddouble.Log(x);
                ddouble s = SterlingTerm(x);

                ddouble k = Consts.LogGamma.SterlingLogBias;

                ddouble y = k + p + s - x;

                return y;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                ddouble v = sterling_loggamma(z);

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                v -= Log(w);

                ddouble y = v;

                return y;
            }
            else {
                ddouble y = sterling_loggamma(x);

                return y;
            }
        }

        public static ddouble Digamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble tanpi = TanPI(x);

                if (IsZero(tanpi)) {
                    return NaN;
                }

                ddouble y = Digamma(1d - x) - PI / tanpi;

                return y;
            }

            ddouble x_zsft = x - DigammaZero;
            if (Abs(x_zsft) < Math.ScaleB(1, -6)) {
                ReadOnlyCollection<ddouble> table = Consts.Digamma.TaylorZeropointCoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x_zsft + table[i];
                }
                s *= x_zsft;

                return s;
            }

            static ddouble sterling_digamma(ddouble x) {
                ddouble s = DiffLogSterlingTerm(x);
                ddouble p = ddouble.Log(x);
                ddouble c = Rcp(x) / 2;

                ddouble y = -s + p - c;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Digamma.Threshold;
                ddouble v = sterling_digamma(z);
                ddouble s = SumFraction(f + n, Consts.Digamma.Threshold - n);

                ddouble y = v - s;

                return y;
            }
            else {
                ddouble y = sterling_digamma(x);

                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Gamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        private static ddouble DiffLogSterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v2;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Digamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        private static ddouble SumFraction(ddouble x, int n) {
            // sum( 1 / (x + i) , i = 0, n)

            static ddouble sum1(ddouble s, ddouble x) {
                s += Rcp(x);
                return s;
            }

            static ddouble sum2(ddouble s, ddouble x) {
                s += (2d * x + 1d) / (x * (x + 1d));
                return s;
            }

            static ddouble sum4(ddouble s, ddouble x) {
                s += (2d * (2d * x + 3d) * (x * (x + 3d) + 1d)) / (x * (x + 1d) * (x + 2d) * (x + 3d));
                return s;
            }

            ddouble s = 0d;
            int i = 0;
            for (; i < n - 3; i += 4) {
                s = sum4(s, x + i);
            }
            for (; i < n - 1; i += 2) {
                s = sum2(s, x + i);
            }
            for (; i < n; i++) {
                s = sum1(s, x + i);
            }

            return s;
        }

        internal static partial class Consts {
            public static class Gamma {
                public const int Threshold = 16;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;

                static Gamma() {
                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        PadeX1Table,
                        PadeX2Table,
                        PadeX3Table,
                        PadeX4Table,
                        PadeX5Table,
                        PadeX6Table,
                        PadeX7Table,
                        PadeX8Table,
                        PadeX9Table,
                        PadeX10Table,
                        PadeX11Table,
                        PadeX12Table,
                        PadeX13Table,
                        PadeX14Table,
                        PadeX15Table,
                        PadeX16Table
                    });
                }

                public static ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable = new(new (ddouble s, ddouble r)[]{
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, -5, 0x8888888888888888uL, 0x8888888888888888uL)),
                    ((+1, -11, 0xD00D00D00D00D00DuL, 0x00D00D00D00D00D0uL), (+1, -1, 0xC000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -11, 0xDCA8F158C7F91AB8uL, 0x7539C0372A3C5631uL), (+1, 1, 0x91CB1CB1CB1CB1CBuL, 0x1CB1CB1CB1CB1CB1uL)),
                    ((+1, -8, 0xD20D20D20D20D20DuL, 0x20D20D20D20D20D2uL),  (+1, 2, 0x9384511DEAB78451uL, 0x1DEAB784511DEAB7uL)),
                    ((+1, -3, 0xB7F4B1C0F033FFD0uL, 0xC3B7F4B1C0F033FFuL),  (+1, 2, 0xF808968F383D5119uL, 0x244D3527089A63B2uL)),
                    ((+1, 3, 0xD672219167002D3AuL, 0x7A9C886459C00B4EuL),   (+1, 3, 0xBB3DD3DA9AC17B58uL, 0xADC81A9890CA341EuL)),
                    ((+1, 11, 0x8911A740DA740DA7uL, 0x40DA740DA740DA74uL),  (+1, 4, 0x83B7B085A5F7689EuL, 0xCD9552B83C7309DCuL)),
                    ((+1, 19, 0xA8D1044D3708D1C2uL, 0x19EE4FDC4469CCAEuL),  (+1, 4, 0xB04C820CF37B0F22uL, 0x50D78B1D73F478F5uL)),
                    ((+1, 28, 0xB694D07B219DBCC4uL, 0x8676F31219DBCC48uL),  (+1, 4, 0xE35D5F580E37ECB4uL, 0xE1B9AA804FE50FA7uL)),
                    ((+1, 38, 0xA1BBCDE4EA012735uL, 0x0B88127350B88127uL),  (+1, 5, 0x8E75243CF95640BAuL, 0xB1AC741F13538518uL)),
                    ((+1, 48, 0xDE466B7C78FBAAE3uL, 0xC3A9E6DAEAE46D98uL),  (+1, 5, 0xAE799EBA1DCEB485uL, 0xFC9877597F7685B3uL)),
                    ((+1, 59, 0xE2E1337F5AF0BED9uL, 0x0B6B0A352D4F335CuL),  (+1, 5, 0xD1BC1F238533A53CuL, 0x065E1F08A600A21BuL))
                });

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xF7BE827D214A59FFuL, 0x253A787D2C0DF3D7uL), (+1, 0, 0xC5C175304F7D90D1uL, 0xFB7C5BFF16B05F97uL)),
                    ((+1, -1, 0x823CCA21D4494010uL, 0x197F82BE2A6A0BC3uL), (+1, -2, 0xD2AB42C418E84EE7uL, 0x2FA2D0E4CC39AADBuL)),
                    ((+1, -3, 0xBD06D4B46DB68FAAuL, 0x5A530BEBBA9AE0CBuL), (-1, -3, 0xCB3E5655FC81313FuL, 0x101C38EC0C6742C6uL)),
                    ((+1, -5, 0xD1B4E87FCC561FAFuL, 0x708198ACE68468D6uL), (-1, -5, 0xCCED6735745CEF82uL, 0x0D6E047387174762uL)),
                    ((+1, -7, 0xBAD2A2460EE4FCF5uL, 0xC6FF088D0C87730FuL), (+1, -6, 0x8F3A939FDC3D8527uL, 0xBC026B22B10412EEuL)),
                    ((+1, -9, 0x89B1381D2A2B1BE2uL, 0x74130FD27D1DEC2FuL), (+1, -10, 0xD3C78B54B55135E0uL, 0xAF80534B8583F54EuL)),
                    ((+1, -12, 0xAA8CB4917AC14FD6uL, 0xC5C904E0C8C8861BuL), (-1, -11, 0xEE581953A64506C7uL, 0xD9D0EAF56209C335uL)),
                    ((+1, -15, 0xB2F7B18DD497DEDBuL, 0x0AB4FC76521F41B8uL), (+1, -15, 0x9808DECDAC86EA97uL, 0xF30773FE15770A8AuL)),
                    ((+1, -18, 0x9EF60CE274511D3BuL, 0x2567FCDD1014F0BBuL), (+1, -16, 0xA6BEB19C9E05C00FuL, 0x6934679FC60FAE13uL)),
                    ((+1, -22, 0xED4061BC3A046F30uL, 0xD3B43EAB4770B3D4uL), (-1, -19, 0xBB87F131549ECFE9uL, 0xA8CE8F4579E360ACuL)),
                    ((+1, -25, 0x9192D851D23D4EB1uL, 0xA02104F57BE10EEEuL), (-1, -26, 0x9A863DE26F9E20ACuL, 0x81D507052A75ED10uL)),
                    ((+1, -29, 0x8DA7319ED8537A1FuL, 0x96E0B356A79AA5C7uL), (+1, -25, 0x8EFBF036AB734827uL, 0xB8BD8B42B46993B0uL)),
                    ((+1, -34, 0xC79A6A110796BB9FuL, 0x42C7DF4D4B4A31B0uL), (-1, -29, 0xCAD8579BDBEDF543uL, 0x52CA65D80FEDC326uL)),
                    ((+1, -39, 0xA94F151B4C2C820DuL, 0xF47C064B8918A5DFuL), (+1, -34, 0xC408D937BF8BB468uL, 0xE51B363FECD4A28CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xE8B221B670F0E1D2uL, 0x2A4CE6637E906CF0uL), (+1, -2, 0xF8ED1333DD4352EDuL, 0xF8164BC8FFC6708DuL)),
                    ((+1, -2, 0xE6CAED60B2C1F57AuL, 0xAC549A5D79384419uL), (-1, -3, 0xAA9F8B439B1571E8uL, 0xE3DB387BF8343E4DuL)),
                    ((+1, -3, 0x9E018A85F7DE8BCFuL, 0x3F99523BFF3A01C9uL), (-1, -5, 0xE9B5C66452D14498uL, 0x24A49A6D0C78EA52uL)),
                    ((+1, -5, 0xA54C673E731E8C66uL, 0x212CC1FDBEFDB293uL), (+1, -6, 0x9D37961B456195CCuL, 0xC618C9E260F91915uL)),
                    ((+1, -7, 0x8AAD22A6966B1A76uL, 0x72BFEFA5FC1E0125uL), (+1, -10, 0xD309C1BD31F5BFD7uL, 0x07F9D863EE255DFDuL)),
                    ((+1, -10, 0xC028E7579FEF9C4BuL, 0x37FE315F07CE672CuL), (-1, -10, 0x9255A89765FA721CuL, 0x2B74425F5F4FB753uL)),
                    ((+1, -13, 0xDF3685AEB42F58AAuL, 0x17427A8800A2765CuL), (+1, -14, 0xA37A96F18DB4CAB3uL, 0x98387F0D630BB509uL)),
                    ((+1, -16, 0xDB0A15229DAC0227uL, 0x52A917208074F05CuL), (+1, -16, 0xC057BFA2E129341CuL, 0xBCC3EDB172A83C74uL)),
                    ((+1, -19, 0xB54B6088990E952AuL, 0x8289A928BF095C1EuL), (-1, -18, 0x9C5CCA32EE353AB0uL, 0x8694AF17C803DF88uL)),
                    ((+1, -23, 0xFB15125CBB894A6AuL, 0x1E47FBCF6304EB28uL), (+1, -23, 0xA7BBE229FEF27841uL, 0x7B3EFC582F749E68uL)),
                    ((+1, -26, 0x8E368FE40D60E4B5uL, 0xD5E8E4A3F37D4B0CuL), (+1, -25, 0xC801AA16B728FDB9uL, 0x67DC55F2900A4558uL)),
                    ((+1, -31, 0xFDCDAC696E0CDD09uL, 0xD7E866DE135FB1C1uL), (-1, -28, 0xF00A5010AB173F96uL, 0xFB9B988C6366C50BuL)),
                    ((+1, -35, 0xA29EB8BD5600848AuL, 0x461FB7036FAA0886uL), (+1, -32, 0xE59E7BCF4F6E45D1uL, 0xEF5D8EFE14C86B64uL)),
                    ((+1, -41, 0xF720E3C8812332F9uL, 0x4B174D75B7D4209BuL), (-1, -37, 0xAEF64C28AC938920uL, 0x51FAB2BDF51089EAuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xE617FEDEC027AB6EuL, 0x1EB041FA07B91400uL), (-1, -6, 0xC47327B844F19DA1uL, 0xF22FD09EDE841516uL)),
                    ((+1, -1, 0xE2421A50627E1224uL, 0xE8CC78427C7A7835uL), (-1, -3, 0xA30377D562A7C7E4uL, 0x65E164D199C43885uL)),
                    ((+1, -2, 0x99B2536E445D2105uL, 0xD30BD39C3D1F23A2uL), (+1, -6, 0xC83DC39E99212ACDuL, 0x66C82AACC5948ABFuL)),
                    ((+1, -4, 0x9FAB0B9BBECA6127uL, 0x2945A486E894F6A9uL), (+1, -8, 0xF5A52D145464BCC0uL, 0x166AC19E596AE79EuL)),
                    ((+1, -6, 0x8515DE1D86053474uL, 0x55D0DA9A63A75A9EuL), (-1, -9, 0x9AABE25B0C605AFAuL, 0x6D61A33E651705DBuL)),
                    ((+1, -9, 0xB751FB19DCD94880uL, 0x41D19AC50E3BB16BuL), (+1, -15, 0xE02EFBA5FD5BE9D7uL, 0x3790C6341EA4D932uL)),
                    ((+1, -12, 0xD3C73133CC6F1998uL, 0xAE4C745D542D2DC8uL), (+1, -14, 0x85B3C0D125C7BD0FuL, 0xDB763C411B7138A1uL)),
                    ((+1, -15, 0xCECB4F26656E48FEuL, 0xB4EC867D33517582uL), (-1, -17, 0xAA6F7E3556D3AA4DuL, 0x1A51A052992E23C4uL)),
                    ((+1, -18, 0xAA6715C5ACE79C8AuL, 0x0053575D3E0B16D5uL), (+1, -23, 0xC9C84C95C4A42EF9uL, 0x3C4337FD5B10C565uL)),
                    ((+1, -22, 0xEB219CD6742A8A71uL, 0xAB5BB13EA6DC2522uL), (+1, -23, 0x8292A83A3A745AD7uL, 0x1FD652DBBFDF7636uL)),
                    ((+1, -25, 0x84C949FCCA030C29uL, 0xF2616D7768B31E50uL), (-1, -26, 0x93DB73C99E10231BuL, 0x044BFD44EF947E0FuL)),
                    ((+1, -30, 0xECA0A5402DA6B6DDuL, 0xEA475E0D2D12188BuL), (+1, -30, 0x96247FCC3B8E30DAuL, 0x5E59415274E16661uL)),
                    ((+1, -34, 0x979527403BD494EAuL, 0x575A7B7524B25FDAuL), (-1, -35, 0x91F7378AE43FAF25uL, 0xD9E88073B6D60AF3uL)),
                    ((+1, -40, 0xE7C35BECE2528733uL, 0xBB9552AD230F11DBuL), (+1, -42, 0xB97A4E52938778BEuL, 0xF8A7A9352BA13E19uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0xC000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xB4ED1C0662D44C4BuL, 0x6ACCB95E5C594182uL), (-1, -2, 0xA0A99027FD12FA97uL, 0xEA6192AD07170FF7uL)),
                    ((+1, 1, 0xB538E464AA614F41uL, 0x33AFAB75BAA757AAuL), (-1, -4, 0x84928779675C6E8EuL, 0x16FA3D5C44437229uL)),
                    ((+1, -1, 0xF807D526E7F6F812uL, 0xDCA83CEC8EF1C9F9uL), (+1, -5, 0xA259DB5CA996CF28uL, 0x2002B181E31935F0uL)),
                    ((+1, -2, 0x8074ED66F54368B4uL, 0xF017C12A54CA8E22uL), (-1, -9, 0xD0ACA4ED0FDF8C32uL, 0xF70EA6B9B1006A99uL)),
                    ((+1, -5, 0xD3E5AAC81B405109uL, 0x6D58B573D694315EuL), (-1, -10, 0xA3A2B75AC130C085uL, 0xBD9C9EC03BF43997uL)),
                    ((+1, -7, 0x8F4C5C402F8DEED3uL, 0xF70B198D1FF778FAuL), (+1, -12, 0x9E74F55CE2F6449CuL, 0x40607BC0DF6C3B59uL)),
                    ((+1, -10, 0xA16BB0E9C7F6155DuL, 0xF90AD8378A027994uL), (-1, -17, 0xA66272713A94F5C4uL, 0x6E6D6C11333DDD83uL)),
                    ((+1, -13, 0x9881ACFC9C5BB598uL, 0xFEFFB54E257D5637uL), (-1, -18, 0xAC990F4284C48F53uL, 0xF05A61F79BA43677uL)),
                    ((+1, -17, 0xF14F5F45829071B6uL, 0xD175BCFEC190BA4EuL), (+1, -21, 0xDDE2E065B3FB511FuL, 0x73510040F1B5E936uL)),
                    ((+1, -20, 0x9E4DB6A64494CA2BuL, 0xBFCDF4C060E795B3uL), (-1, -26, 0xFF82D2C3868A9595uL, 0xB45170D2A19571F2uL)),
                    ((+1, -24, 0xA849DD5A79ECCDF8uL, 0x547B49A2CD7861EFuL), (-1, -28, 0xB638148220B293A4uL, 0x53A94DDB0047A44AuL)),
                    ((+1, -28, 0x8AE9EC296E686220uL, 0x93470307FD774E91uL), (+1, -31, 0xD677FD593D7FE4A0uL, 0xA349C3138DD5F5D3uL)),
                    ((+1, -33, 0xA236F9CF71332B06uL, 0xB580CCC654F9F21CuL), (-1, -35, 0xC22E52AAA2DD2D7FuL, 0x89EE663EBCBF2C77uL)),
                    ((+1, -39, 0xD639CDA3601CEB38uL, 0x2D2A66F29AF37FDBuL), (+1, -40, 0x8C6E280D648C7E49uL, 0xFE2C807D3E1D7B21uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xC000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 4, 0xB185FC4A4282EA71uL, 0xB7BF32D2F9C31C05uL), (-1, -1, 0x94DE47B97EF5FFC3uL, 0x8E982765B1A90F4CuL)),
                    ((+1, 3, 0xAC7A4E567E55BE79uL, 0xCDB06992DF61915CuL), (+1, -4, 0xA41EBC8B177E97D2uL, 0x93C13C7E36C1DDCAuL)),
                    ((+1, 1, 0xE3612D949A1D1A63uL, 0x57D3067775C28B1AuL), (+1, -6, 0xBED5728A670321B0uL, 0xD86C531F77B15940uL)),
                    ((+1, -1, 0xE1156BA4A40A4441uL, 0xED1EB114D011C7C6uL), (-1, -7, 0x9CE252BEDDB66A38uL, 0x6EA0397C495750F6uL)),
                    ((+1, -3, 0xAFEAC206AB941D78uL, 0xD391B3F5D75436F5uL), (+1, -11, 0xF826BF57FDB3C21FuL, 0x4594F3D4A56BB30CuL)),
                    ((+1, -6, 0xDF1C133C08E87DA4uL, 0xAC20ED865CCE65ECuL), (+1, -13, 0x8A91BC530F1E326AuL, 0x41D52C01D0F92260uL)),
                    ((+1, -9, 0xE89FAA1A4FD64E5AuL, 0x7433D9015E18FE2BuL), (-1, -15, 0xBD4AAA628F01CA2BuL, 0xE0D949828F7DE353uL)),
                    ((+1, -12, 0xC7E9B0C6D9C3A593uL, 0x7655714776632831uL), (+1, -18, 0x8E3F36572BD94D3DuL, 0x89377AD3B22450B0uL)),
                    ((+1, -15, 0x8C7C968DA48BCF2BuL, 0xC0B1D63F63D218D0uL), (+1, -24, 0xE63C4FE5E73D1AF1uL, 0xB64C50DD91A8D641uL)),
                    ((+1, -19, 0x9E02251FA1DF027DuL, 0xE0A56D4525F13812uL), (-1, -24, 0x8DE13EA321357C20uL, 0xA0E478C6DCFEF16EuL)),
                    ((+1, -23, 0x881F9CDAD99BE980uL, 0xCC2655DF1982EE17uL), (+1, -28, 0xF502AADC5654F12BuL, 0xAFBA2E35796339F0uL)),
                    ((+1, -28, 0xA3985121A4DA8DDAuL, 0xB9EAAD29AB87A26CuL), (-1, -32, 0xCC4968F75ACF8AC4uL, 0x6E8C7B4230F03E5EuL)),
                    ((+1, -34, 0xD78DD8EE558E8B3AuL, 0x5A9B0B54FCAA7B13uL), (+1, -37, 0x9086E7BA19CC7FC3uL, 0x9539A5FCAE659DCFuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 6, 0xF000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 6, 0xC17B551303A0E44CuL, 0x310D9393D3A08638uL), (-1, -1, 0xE662B4D506F8EF81uL, 0x60556758C79B946DuL)),
                    ((+1, 5, 0xA64C49A370E0EB86uL, 0x0298FCAA7AD50629uL), (+1, -2, 0xABEBD6B182B13B1EuL, 0x6A74CBD6C70BCBE4uL)),
                    ((+1, 3, 0xC17228EB333E13C2uL, 0xF2D7D02958FB71CEuL), (-1, -5, 0xEC34FCC3E8EAB12CuL, 0x00719C961FF0DF3BuL)),
                    ((+1, 1, 0xA81483DA68C174A3uL, 0xB2328D2740C6426DuL), (+1, -16, 0xE69B6BCC94DA4EEAuL, 0x0E9F87CFCE65307DuL)),
                    ((+1, -2, 0xE41DC71EB9772C1DuL, 0xBE9388CC2C016F4DuL), (+1, -9, 0x8CE94D7F9FB257C0uL, 0xFBE82EB5201F9E0DuL)),
                    ((+1, -5, 0xF76D021164733DAAuL, 0xF046DEE00C68ADF9uL), (-1, -12, 0xF5C377970835B47FuL, 0x5C584DB40BB7F7AFuL)),
                    ((+1, -8, 0xD795D4B8300BBEBAuL, 0x03F9CD4F9520FAA9uL), (+1, -15, 0xB13C7AD0E96B3D2AuL, 0xEBF683BE2227A59CuL)),
                    ((+1, -11, 0x95B0A56FBD23B70BuL, 0x3D09F24F2304C36AuL), (+1, -20, 0x8C48117B919839B7uL, 0x159131DE7C4EB45FuL)),
                    ((+1, -15, 0xA0AD48CD4218ACFFuL, 0x10087688EC1BF78CuL), (-1, -21, 0xD17F61BD441C3266uL, 0x755E06BA596EFAA8uL)),
                    ((+1, -20, 0xF98D3750B4863D6BuL, 0xA1AD393E8E83B5CFuL), (+1, -24, 0xE0EF539076690ECEuL, 0xF38E445C0C214C75uL)),
                    ((+1, -25, 0xE627E18051C50F47uL, 0x58D58599A0E694F9uL), (-1, -27, 0x83FFB365D76F87E4uL, 0xA16256A13932C74EuL)),
                    ((+1, -33, 0xCC23ECC1A1DCA7F0uL, 0x29CC8FE7FCA428E7uL), (+1, -32, 0xAFF90DA8259F290DuL, 0xD1F1D90E8CCABF76uL)),
                    ((-1, -35, 0xC049011180952156uL, 0x015C0F7AF6BBEBC4uL), (-1, -38, 0xD4A13CA4B4B5D531uL, 0x00B2376F70CFFE55uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 9, 0xB400000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 9, 0xAEB5C5976849E410uL, 0xD407F48AD4B0967EuL), (-1, -1, 0xE6F4CEBCA40843A3uL, 0xF552E1DBAE0D14ABuL)),
                    ((+1, 8, 0xAD296B361F30EA5BuL, 0x6031C96B1221ED11uL), (+1, -2, 0xAE27C64FBDAE31B7uL, 0x1760B963AD6B6942uL)),
                    ((+1, 6, 0xE599581C66F6949BuL, 0xD4FDADBF58E2FC3BuL), (-1, -5, 0xF914AC0AE1DFB722uL, 0x200884BA7341A2E9uL)),
                    ((+1, 4, 0xE2078DAF6B8808E0uL, 0x7FE3B7D0FA7A58C9uL), (+1, -10, 0xA2DA6E2FA6B6B7DEuL, 0x22B8C5085E0B941FuL)),
                    ((+1, 2, 0xAE165240330E1EDCuL, 0xBBE52AEC8D54DD96uL), (+1, -10, 0xF51186D1CFFAE9CAuL, 0x07EBB3BEB9C5A4CDuL)),
                    ((+1, -1, 0xD7BE39AE15E5E66EuL, 0xE9977DD4C0433A88uL), (-1, -12, 0xE2980B197075852BuL, 0x057323501475BEE2uL)),
                    ((+1, -4, 0xDA079997F856198BuL, 0xD6270D58579EB40DuL), (+1, -15, 0xACBA463D511628A7uL, 0x11E2DCC03E08D9ADuL)),
                    ((+1, -7, 0xB4195E43A8320613uL, 0xAF9C595D44225F4EuL), (+1, -22, 0xFCEE2D8E6BE60B3EuL, 0x59EE5BF2F4DAA617uL)),
                    ((+1, -11, 0xF1236770E415CCECuL, 0xAB14FADF538F4F83uL), (-1, -21, 0xAEDFFDF56CCA5E4BuL, 0xA9D071BEF98423DDuL)),
                    ((+1, -15, 0xFFC262FE3729C048uL, 0xF9DF19AA39566A46uL), (+1, -24, 0xC139542D85CAB84FuL, 0x0BCD4C5820C401DDuL)),
                    ((+1, -19, 0xCD43EE4AB9FE9142uL, 0xCF24EA0201F0A6DAuL), (-1, -28, 0xE45E1EEE0BCF7716uL, 0x962D61EDD0126856uL)),
                    ((+1, -24, 0xE2431BAAA0038F9DuL, 0xE6344F938E3A9827uL), (+1, -32, 0x9855CDDE624901EAuL, 0xFC323CAA64413EC0uL)),
                    ((+1, -29, 0x85355D2EA437A680uL, 0x3FC5A7266AF246DDuL), (-1, -38, 0xB7B22D845D7FF162uL, 0x5AE1F0F1454A6BA1uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 12, 0x9D80000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 12, 0x85DAEF0613B9473EuL, 0xB47B469EA438F519uL), (-1, 0, 0x9537DB7A47E17DE8uL, 0xC02F47C5124181DEuL)),
                    ((+1, 10, 0xEC6C74C133A570C1uL, 0x3FB660790C2AFADAuL), (+1, -1, 0xA087B02781914D06uL, 0x66B36FB54F444F9FuL)),
                    ((+1, 9, 0x8C20A0D0544A83E3uL, 0x58BDC0A9CCA6F72EuL), (-1, -3, 0xCFF3A914033D49C6uL, 0x5C099B89E4A045F8uL)),
                    ((+1, 6, 0xF6A2AEE6E5FD0F29uL, 0x0FEF5299B15C7A51uL), (+1, -5, 0xAF87DD94A7458268uL, 0x0E092EAADEF7D8C0uL)),
                    ((+1, 4, 0xA932E154F7C9BC49uL, 0xC3CABE333C388160uL), (-1, -8, 0xBC93674F158BCF60uL, 0x07EF33C5221033C6uL)),
                    ((+1, 1, 0xB9A241B76D4A2822uL, 0xB7A62CC69B621A87uL), (+1, -12, 0xBE1D2BEDDC0B59ADuL, 0x17BC59D61AA639F2uL)),
                    ((+1, -2, 0xA4891B3185A7CC79uL, 0x61B2475CA6DC8333uL), (+1, -15, 0x84AB5EF7AA1A050EuL, 0x46E7E0E4388E1A5BuL)),
                    ((+1, -6, 0xEB528F33791170D1uL, 0x51A4B1C4600606C9uL), (-1, -17, 0xBEBDA8C395B97796uL, 0xB5C7FD90FCD56AD9uL)),
                    ((+1, -9, 0x85D9C4CB2EC6E49AuL, 0xBFD705AD046461EDuL), (+1, -20, 0xCFF02036580C0DDFuL, 0x18DF8DA8D96757B1uL)),
                    ((+1, -14, 0xEACF6154949184C0uL, 0xDAC7FDDA633D67ECuL), (-1, -23, 0x8D6A07B227C28011uL, 0x971FFDF4166F19CFuL)),
                    ((+1, -18, 0x9535BB2AD251FCE7uL, 0x724BC605AE9C4710uL), (+1, -28, 0xFBFABAF4C242C621uL, 0x4A6F162DDDD355EFuL)),
                    ((+1, -24, 0xF1B4095FBE66A458uL, 0xCD68BA2156EBB994uL), (-1, -32, 0x894B74B1E2D3608BuL, 0xADBE175A31E717A5uL)),
                    ((+1, -30, 0xABA469F889BF0194uL, 0xC3136060C0E38829uL), (+1, -38, 0x8D498C4D81FEC273uL, 0x51DBE1211E6C9669uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 15, 0x9D80000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 15, 0x995CFDC7370BB888uL, 0x9074069FD26E11C1uL), (-1, 0, 0x955D31D2C0B60F25uL, 0xD1463D33739C7045uL)),
                    ((+1, 14, 0x96C101975919359FuL, 0x492E0D5F13A8BC32uL), (+1, -1, 0xA067C775FF923DAAuL, 0xBA2762CB2110B7A2uL)),
                    ((+1, 12, 0xC482E98DCB9B8E58uL, 0x6E24F234412C52A4uL), (-1, -3, 0xCE5AD4A53AC6C992uL, 0x16020A0B158661D1uL)),
                    ((+1, 10, 0xBCC0B1863E009CBEuL, 0xEC388DF39BC3A5A2uL), (+1, -5, 0xAAFCCE3B941A992BuL, 0x2B24F1E1D40D0AC6uL)),
                    ((+1, 8, 0x8CD22E4D769532E3uL, 0x5914014782556FD7uL), (-1, -8, 0xAE117E83FC692D42uL, 0x4F55A03DA2C18F4DuL)),
                    ((+1, 5, 0xA7D5F9FCB532DD5EuL, 0x97D32C870BD733F3uL), (+1, -12, 0x8024D45AD87ED3D9uL, 0xDF8B02A36DEABE02uL)),
                    ((+1, 2, 0xA1E27772CC498354uL, 0x79BB11BFEA54DBF3uL), (+1, -15, 0xE39783DAE26F6661uL, 0xAA81B0B7EE285733uL)),
                    ((+1, -2, 0xFD25CBF03D95F422uL, 0x3FE24CDB70CDB414uL), (-1, -17, 0xF4747DD1B03A0233uL, 0xCDC3E188008A38B5uL)),
                    ((+1, -5, 0x9EE4689F23577D37uL, 0xF8A764EE242F6E43uL), (+1, -20, 0xFD1EC5B4C70F91A9uL, 0x31B75F5DA129BAE4uL)),
                    ((+1, -9, 0x9C3CFB0D5CF95640uL, 0x1BC777CFC666F516uL), (-1, -23, 0xA9390835CCC527E9uL, 0xCEF3EC80FFF47E2DuL)),
                    ((+1, -14, 0xE52D0EF0B043FCE7uL, 0x8E350A8A967BF180uL), (+1, -27, 0x95F1342D60E95669uL, 0xD53A1652DAE1279DuL)),
                    ((+1, -19, 0xE28473E252691213uL, 0xA2D8110442F8540BuL), (-1, -32, 0xA363035F7BC8189DuL, 0xA7C7FB04D060CAB6uL)),
                    ((+1, -25, 0xE7394BDE72D35446uL, 0x22CEA0F07B58C7C6uL), (+1, -38, 0xA8A35C77DCEEB50BuL, 0xF9DCBF13D8822788uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 18, 0xB130000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 18, 0x969D320C539BD7C7uL, 0xFF112C14AE821357uL), (-1, 0, 0xB36BC3E4D784028EuL, 0x36BAE4B4650E20B8uL)),
                    ((+1, 17, 0x8285477E0557CB67uL, 0x54BE85F482D63A4FuL), (+1, -1, 0xEFD70700BA119746uL, 0x4B8A9735EDFEEBADuL)),
                    ((+1, 15, 0x95FDBBFA3B239B6EuL, 0xE83231317E8B5E5AuL), (-1, -2, 0xCAB0C5507F01BFD2uL, 0x65E9DD8A7E759574uL)),
                    ((+1, 12, 0xFD025FA8317CF3DFuL, 0x6304FF8045949FA4uL), (+1, -4, 0xF202E76FB7C44623uL, 0x62CD6CEC27D53CFCuL)),
                    ((+1, 10, 0xA4730737DC8267F4uL, 0x13D183F9969051FCuL), (-1, -6, 0xD7E8A9A37B5A3C9CuL, 0xE0C3500DA6A2C4FCuL)),
                    ((+1, 7, 0xA8C05C7102154425uL, 0x5957DB10DC3C3C6DuL), (+1, -8, 0x9456170CC807C28BuL, 0x4F736DA525CCABDBuL)),
                    ((+1, 4, 0x89C5F6D3D07FF676uL, 0xC814CEEED072BF21uL), (-1, -11, 0x9F3CE24B432905E1uL, 0xDF56DA805872FA9EuL)),
                    ((+1, 0, 0xB1F2CFBA2A2BB88AuL, 0xC07AA8063B785BA9uL), (+1, -14, 0x85D6BF3E34BEBACEuL, 0xC5DB780E1DF0CCACuL)),
                    ((+1, -4, 0xB1D7604B4C27530FuL, 0xB98F814F5EE13D23uL), (-1, -18, 0xAE65E280B0C37C4EuL, 0x51566EA33176094FuL)),
                    ((+1, -8, 0x83345DB63397CD1DuL, 0x4EC617F530476522uL), (+1, -22, 0xABAFBBCDA9B4030CuL, 0xE74CBD8A3520C680uL)),
                    ((+1, -13, 0x814FB0DEAA5AD439uL, 0xE6317FBE0E6DC34DuL), (-1, -27, 0xF294942526C4B8DEuL, 0x1487FCA1F0E59974uL)),
                    ((+1, -19, 0x83044561B568912CuL, 0x0FC1DDE992BC5044uL), (+1, -32, 0xDD3823F777CFDB0AuL, 0x4E4CC3D2FE64E224uL)),
                    ((+1, -32, 0xA1196A7D7C0F1798uL, 0x65641FF7AAB49B21uL), (-1, -38, 0xC574EB6BEA539256uL, 0xBF853E1D5344E23FuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 21, 0xDD7C000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 21, 0xD266D7263C9D830DuL, 0x487EE4898C1275E7uL), (-1, 0, 0xB36DE07C48909F33uL, 0x4292FC94A415B338uL)),
                    ((+1, 20, 0xC8C685651156DDCEuL, 0x948156B226ACCFACuL), (+1, -1, 0xEFDCB249D2190511uL, 0x0EC1F013EDD291C3uL)),
                    ((+1, 18, 0xFCB0E1AB3B394805uL, 0xCEB0EB8ECB148450uL), (-1, -2, 0xCAB7FCEFAFD03F63uL, 0x8D424AC7860B8D86uL)),
                    ((+1, 16, 0xE90C7243F566ED49uL, 0x64CD3559E2F5C839uL), (+1, -4, 0xF20E71D0D4539CBFuL, 0x9A41A74B0CB93FC4uL)),
                    ((+1, 14, 0xA5F507EE0FCF19BAuL, 0x8F0B0C95ED0F2978uL), (-1, -6, 0xD7F5974927FC40B8uL, 0xBE5F1CBBF1427EFDuL)),
                    ((+1, 11, 0xBB94597EFE524515uL, 0x74A8A81BF37FF058uL), (+1, -8, 0x9460CC746AB44E36uL, 0x64302B8DE31411A4uL)),
                    ((+1, 8, 0xAA58A04F8320F7DEuL, 0xF9B72D85F013F319uL), (-1, -11, 0x9F4A5C4AD391CAC0uL, 0xB8B33EA68847DA1FuL)),
                    ((+1, 4, 0xF8B98AB8DB9FF219uL, 0xB70F1C0F5FCF7491uL), (+1, -14, 0x85E3C1D011F7722CuL, 0xE2B09EC5B5B41CB5uL)),
                    ((+1, 1, 0x9057A2D912A39B32uL, 0xD8E3764FF16072F1uL), (-1, -18, 0xAE790E3700EC4E52uL, 0xFCA8D6FA572C1C6FuL)),
                    ((+1, -3, 0x81AA3117A5FE6263uL, 0x40BABC4C006FCCB5uL), (+1, -22, 0xABC4D0EF3C515F84uL, 0x5D54A43A480FB38CuL)),
                    ((+1, -8, 0xAB1E5B4641AD96BFuL, 0xD5CBAA49A8D96E6DuL), (-1, -27, 0xF2B5876026BC40F6uL, 0x4192D9525BCDD489uL)),
                    ((+1, -13, 0x951407795710610AuL, 0x69C847189ED1AED1uL), (+1, -32, 0xDD591C8AA234DE5DuL, 0x8CC0DB16EBF2A37DuL)),
                    ((+1, -19, 0x81E2BEF21B6F8D0AuL, 0xB7BE9D3C077EF094uL), (-1, -38, 0xC594FEA8257172E0uL, 0x247137E10D8B27D9uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX12Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 25, 0x9845400000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 25, 0x9AEED2249E2B2B04uL, 0x39402D62111FD1ADuL), (-1, 0, 0xB66C3400E7187FAEuL, 0x2CC19C2B8271F8A8uL)),
                    ((+1, 24, 0x9CA9471CF0059BC4uL, 0xC44C02FB2B78691BuL), (+1, -1, 0xF80970C2963C0B94uL, 0x52F80FC3569E5D46uL)),
                    ((+1, 22, 0xCF8293BC2E227DE8uL, 0x5BAEE94CC1974365uL), (-1, -2, 0xD54F5DFFA76A74F3uL, 0xC71DF92D991D35ECuL)),
                    ((+1, 20, 0xC868A85F7EBF2C65uL, 0xED41733F46971B3EuL), (+1, -3, 0x81A6621F545E200BuL, 0x892C388D5CB967EAuL)),
                    ((+1, 18, 0x94DEBCCF6E543002uL, 0x13C2AE9DB03F3FCEuL), (-1, -6, 0xEBA3241F85A26F82uL, 0x33B2737AB8924D22uL)),
                    ((+1, 15, 0xAEFB1D6DA96E6386uL, 0x5E9A9AE26B8111CCuL), (+1, -8, 0xA4FDB0228C0DA41CuL, 0x554BDF0C0C5E306CuL)),
                    ((+1, 12, 0xA4D3FBC1F4AD2671uL, 0x6BAA77C79176257FuL), (-1, -11, 0xB49C96BC79A84691uL, 0xF9F74B639A55BF4CuL)),
                    ((+1, 8, 0xF91D6FAA17409457uL, 0x7AF63B840E3F63F2uL), (+1, -14, 0x9AE4BE684EF3768FuL, 0xBA29DBA8E694D229uL)),
                    ((+1, 5, 0x95625AA60D1F8F75uL, 0xDDE3E72B9904B8F9uL), (-1, -18, 0xCE137D03747ECE9DuL, 0xE00D0D7893BC81BBuL)),
                    ((+1, 1, 0x8A781191ACE60D6EuL, 0x55EF2F5FEFB2D9ABuL), (+1, -22, 0xCF4846DD19172D54uL, 0x7BE6400C69F1F258uL)),
                    ((+1, -4, 0xBC5BD76EAB4045EDuL, 0x91284FF05242E1C3uL), (-1, -26, 0x95BBEC7DE936814CuL, 0x2C2DD2BF3B2A01DAuL)),
                    ((+1, -9, 0xA908E915D3769A13uL, 0x00279E300FBD302BuL), (+1, -31, 0x8BBCBE6434C4C48FuL, 0xA8B2331161017BECuL)),
                    ((+1, -15, 0x97BA0DB6888EB13FuL, 0x6191B391F6258718uL), (-1, -38, 0xFF819B8E6EDFCFCFuL, 0x2F68289BF67E72D3uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX13Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 28, 0xE467E00000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 28, 0xF6248D62C9407F8BuL, 0x92852D45FFBEDE0EuL), (-1, 0, 0xB9632E5563B14789uL, 0x71E68267B9C8F632uL)),
                    ((+1, 28, 0x82D7968CAAF1AA9AuL, 0x3171D8F527B33B8BuL), (+1, 0, 0x80222F06302CB641uL, 0x47272064EE7432D8uL)),
                    ((+1, 26, 0xB548B29E3FCD8B5EuL, 0x4B42B228E2C0ABA7uL), (-1, -2, 0xE027612859F863E8uL, 0x2B2C5AF953E28886uL)),
                    ((+1, 24, 0xB6692808656FA484uL, 0xE48172B58AF35445uL), (+1, -3, 0x8AA17524EF0EC5D8uL, 0x7BA9B535962110CAuL)),
                    ((+1, 22, 0x8CB9DCDE350C6544uL, 0xE534B702524C3C3FuL), (-1, -5, 0x804019CB788006CAuL, 0x575DEDB1189D372EuL)),
                    ((+1, 19, 0xAB56279F0EC4BBBCuL, 0x31FB7F521AF65441uL), (+1, -8, 0xB6ED9D05478D06ECuL, 0xDC11B6989A14FC88uL)),
                    ((+1, 16, 0xA6CF03821329D2DFuL, 0xFB031C8C74991166uL), (-1, -11, 0xCC114C713570D77EuL, 0xE41F9E8F581EA5AAuL)),
                    ((+1, 13, 0x8208AF809BCDC93CuL, 0x90F893B64A3EE933uL), (+1, -14, 0xB273350FAA665049uL, 0x4EA75D1FE5CE0716uL)),
                    ((+1, 9, 0xA09BD83FE6020A4EuL, 0xA1880CD15D15776DuL), (-1, -18, 0xF23B4534EE2B100FuL, 0x46E8C33FDD095CE8uL)),
                    ((+1, 5, 0x991834B27CE8CD04uL, 0xA39258D78D9F595FuL), (+1, -22, 0xF8C0C552A85D3A3BuL, 0x7EB5B6A907D0095AuL)),
                    ((+1, 0, 0xD5E180B8782B428EuL, 0xCE20800307F1B6FEuL), (-1, -26, 0xB79575FFC6EF4F05uL, 0xB5B5649B53A97742uL)),
                    ((+1, -5, 0xC4E58EDB29B0FAA7uL, 0xD40C826A3A7093B0uL), (+1, -31, 0xAF2BC6A7DC661C3EuL, 0x2A0F1711AFCA6567uL)),
                    ((+1, -11, 0xB51D78A7B47A6F89uL, 0xFE6314778E11FE97uL), (-1, -37, 0xA3E005321DA137E1uL, 0x1A0D04EDAC97C90CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX14Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 32, 0xB994660000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 32, 0xD21439C5F1D2C028uL, 0xE79E94875D68B0D3uL), (-1, 0, 0xBC468C5DF9B7BED9uL, 0x4869F36A8F9D4BC6uL)),
                    ((+1, 31, 0xE9653FB5E45B20D6uL, 0x87C39634E49C4B92uL), (+1, 0, 0x8434A6B13C9D9D1FuL, 0x80D0A8E12BC73CAFuL)),
                    ((+1, 30, 0xA84DC5574E8245FEuL, 0x79896C30C6503E01uL), (-1, -2, 0xEB0EE70F45F225EEuL, 0xD580DCAE5314AB40uL)),
                    ((+1, 28, 0xAFBB3AFEF3759DACuL, 0xFFD12CBD1EFCC8C4uL), (+1, -3, 0x93CFC24749CE8A7DuL, 0x5FB6260EB04A4EDEuL)),
                    ((+1, 26, 0x8C5208E31C789F9FuL, 0xA701B6EDC327D166uL), (-1, -5, 0x8B18A197BDE86FECuL, 0xF015235079745701uL)),
                    ((+1, 23, 0xB0712F4DC7CEA455uL, 0xE2BF4E6F1AA57668uL), (+1, -8, 0xC9E74A25E3570FEBuL, 0x4076F32C92804AE6uL)),
                    ((+1, 20, 0xB1144A2438B160D5uL, 0xBE968D8BFF3680E8uL), (-1, -11, 0xE553AB9576713EF5uL, 0x0EB49DF2A28BEC04uL)),
                    ((+1, 17, 0x8E118F5D9A5247B2uL, 0x22E569529AB30A8EuL), (+1, -14, 0xCC4957EDB706F7A6uL, 0x1F611C1BC0D4310FuL)),
                    ((+1, 13, 0xB455FBF6B476F580uL, 0x6DE44DE74CFD841AuL), (-1, -17, 0x8D5209F0944F54A2uL, 0xABF6DC7DD8521C25uL)),
                    ((+1, 9, 0xB070192829E0DBA5uL, 0x2EBEA233573724A9uL), (+1, -21, 0x94018EA1B2913E1BuL, 0xDA4C217391A96D54uL)),
                    ((+1, 4, 0xFCB69552C093B0ACuL, 0x4861DDB5B9D02296uL), (-1, -26, 0xDEF06272C7BF485AuL, 0x710B166B744FADFFuL)),
                    ((+1, -1, 0xEE455B932288EF1CuL, 0x8CE2566637C5E213uL), (+1, -31, 0xD93AA7091D09AA0EuL, 0x6ED604C84D400931uL)),
                    ((+1, -7, 0xE043529D42438094uL, 0xF29D820DE11CB099uL), (-1, -37, 0xCFAD6DA0C81970D9uL, 0x01309831D7B32BC2uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX15Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 36, 0xA261D94000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 36, 0xBFDA6FDAC16E1DAAuL, 0x85D61F452ECCA069uL), (-1, 0, 0xBF15DDE748E7BB0FuL, 0xF485EEF81A0F5595uL)),
                    ((+1, 35, 0xDD9E683AADE9B6F2uL, 0x81C5241F7C6DC2DCuL), (+1, 0, 0x883A2F9545848AECuL, 0xB094E24B89AE7AD3uL)),
                    ((+1, 34, 0xA5A9D65AEB3D7082uL, 0xB1E51087A5071424uL), (-1, -2, 0xF5FE74012E83942CuL, 0xB1A97387F0C86954uL)),
                    ((+1, 32, 0xB2DD4E8A32010D60uL, 0x64AAFC601479628FuL), (+1, -3, 0x9D2A2AD13FE8F53FuL, 0x75D00269B70A7B4EuL)),
                    ((+1, 30, 0x93608E3BD274A738uL, 0x0DBC41C200E4D06CuL), (-1, -5, 0x9653326938F48B76uL, 0x0DE36F5FC9A785F4uL)),
                    ((+1, 27, 0xBEE1F8928225D045uL, 0x577ECCE3B80E26D9uL), (+1, -8, 0xDDE039AC925DB0F6uL, 0xECFA90BEB4D862DFuL)),
                    ((+1, 24, 0xC504D945C6F54929uL, 0x415E91AB7D4D39E0uL), (-1, -10, 0x802F128547DFB46BuL, 0x61E8954F377D6289uL)),
                    ((+1, 21, 0xA2561D28E6324508uL, 0x8A2044CC62A2A877uL), (+1, -14, 0xE86E4A8F7B5A5967uL, 0x2F80C91DA6F047B9uL)),
                    ((+1, 17, 0xD35EE3B5F4532C32uL, 0x518298002DC8F79DuL), (-1, -17, 0xA3BA259B66CD6E34uL, 0xFC3FBECFB86D5D90uL)),
                    ((+1, 13, 0xD3E52DB5ECC4EA0EuL, 0x067D205AE69C6F98uL), (+1, -21, 0xAEB28F5F8CF691DAuL, 0x25EF8EFFE078289AuL)),
                    ((+1, 9, 0x9B551847B81747C4uL, 0xF912C3E8C47CE5CFuL), (-1, -25, 0x861F3CE87D1D286BuL, 0xAA1E28B06A0DAB3CuL)),
                    ((+1, 4, 0x95C71660799C59C6uL, 0x32D4B46D6EA4491DuL), (+1, -30, 0x854CE7DCE24FD3E6uL, 0xBBA9B15ACB469E53uL)),
                    ((+1, -2, 0x900D135FCA0AE9EFuL, 0xF1836B8B72F668C8uL), (-1, -36, 0x82126EDA34B7FF4DuL, 0x4A9C3CED415F0758uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX16Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 40, 0x983BBBAC00000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 40, 0xBAC33B40F0BC522BuL, 0xE6CF0BCCF2EF9A7DuL), (-1, 0, 0xC1D1255D1EFF5AC0uL, 0xEDD31C5FE0022C98uL)),
                    ((+1, 39, 0xDF5FAFEF2FD7C9B5uL, 0xD65AEFAA5E7FC330uL), (+1, 0, 0x8C3183935A685764uL, 0xE4324E0593080BBFuL)),
                    ((+1, 38, 0xAC7A565E79EFF90DuL, 0xA71CEFC795A63A21uL), (-1, -1, 0x807818FB04206D43uL, 0xE7F0990A516CD889uL)),
                    ((+1, 36, 0xBFF9FAEC23C80EB8uL, 0x9D2365B48E420298uL), (+1, -3, 0xA6AAB1D961D75756uL, 0x4FF91F3B95C6CED2uL)),
                    ((+1, 34, 0xA2CA506B56DD1F5AuL, 0x0B3BE4DF4837881FuL), (-1, -5, 0xA1E8BD6FD9958D19uL, 0xB1B197B000B34ED2uL)),
                    ((+1, 31, 0xD8AA8E92E9C19153uL, 0xE21B6C0DC5E4F910uL), (+1, -8, 0xF2CEA1AEA168FBC8uL, 0x74D2621D8D3F11A9uL)),
                    ((+1, 28, 0xE5809CB160B7EAD8uL, 0x2B8E9C2257DE09CCuL), (-1, -10, 0x8E9572C8761CC280uL, 0x015818E6719E56ECuL)),
                    ((+1, 25, 0xC1D6AA3424D3FEA1uL, 0x963565D6BA1BAA44uL), (+1, -13, 0x8373EC86AE45DFF4uL, 0xE56871A7DB09DC1FuL)),
                    ((+1, 22, 0x81380DB212502595uL, 0x478B2CAB6793BD1CuL), (-1, -17, 0xBC6790636F40408FuL, 0x2B145E72C0CA40B3uL)),
                    ((+1, 18, 0x8484AFD221D3416CuL, 0x1D91921011E17413uL), (+1, -21, 0xCC9C68434014521FuL, 0xDEC667E054BD5466uL)),
                    ((+1, 13, 0xC6955CA6B949DACDuL, 0xD64692FEE45E443BuL), (-1, -25, 0x9FF7E307BF321EA5uL, 0x43BBE46E66497F08uL)),
                    ((+1, 8, 0xC38F4D4E41628DF3uL, 0xFC58062D1D6EC0F8uL), (+1, -30, 0xA1FDC4816400DA78uL, 0x489CA6C1035F61B3uL)),
                    ((+1, 2, 0xBFF2A8061E8E9CBCuL, 0x10789FDB2D72EECDuL), (-1, -36, 0xA1258344D27D1C61uL, 0xC5E77FBE025FC176uL)),
                });
            }

            public static class LogGamma {
                public static readonly ddouble SterlingLogBias = Log(PI * 2) / 2;

                public static ReadOnlyCollection<ddouble> TaylorX1CoefTable = new(new ddouble[] {
                    (-1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
                    (+1, -1, 0xD28D3312983E9918uL, 0x73D8912200BACE5EuL),
                    (-1, -2, 0xCD26AADF5596B7ACuL, 0xF64B92E699B5AA12uL),
                    (+1, -2, 0x8A8991563EC241B5uL, 0xF91211196E5235FBuL),
                    (-1, -3, 0xD45CE0BD530A492EuL, 0x826A4FDAE19904DCuL),
                    (+1, -3, 0xADA06588061830A5uL, 0x3CCA078AB4AD8EFBuL),
                    (-1, -3, 0x9381D0EE751D72C6uL, 0x0FFA0F2958701A43uL),
                    (+1, -3, 0x80859B57C31CB745uL, 0xF2CE526EDAD3266DuL),
                    (-1, -4, 0xE4033836ABEDA2ADuL, 0x5500DFBA7DB22231uL),
                    (+1, -4, 0xCD00F1C2EAFC7981uL, 0x3418F3768CD66B0EuL),
                    (-1, -4, 0xBA461988A636B07AuL, 0x95D890201C7F1587uL),
                    (+1, -4, 0xAAB56B1921DE202BuL, 0xBD0B01F5D03A1CCCuL),
                    (-1, -4, 0x9D8ECB8FE2CC261AuL, 0x79F81B6C2036AFF1uL),
                    (+1, -4, 0x924B6FC19062FB9EuL, 0x9695C0817D0A6441uL),
                    (-1, -4, 0x88899A3B73FF01DCuL, 0x9A4FF396EE34EC23uL),
                    (+1, -4, 0x8000803266F59178uL, 0x79D0156AFFDBC10BuL),
                    (-1, -5, 0xF0F16988F455E4E5uL, 0xABDDE62D5294E08BuL),
                    (+1, -5, 0xE38E71D105A0C482uL, 0xD3D05F949F431375uL),
                    (-1, -5, 0xD79450DAB44502E8uL, 0xE83D10BFDF68DA3BuL),
                    (+1, -5, 0xCCCCD99A96ADD052uL, 0x546D31AF95B3951CuL),
                    (-1, -5, 0xC30C36DBBDFDFADBuL, 0xDBDE3EB0EEF83E63uL),
                    (+1, -5, 0xBA2E8E8BBC6FC881uL, 0x5C7F0544C73B8B65uL),
                    (-1, -5, 0xB216442C8DB365DFuL, 0x1755C6A07401D738uL),
                    (+1, -5, 0xAAAAAB5557EE6AA9uL, 0x40BF9C09DEC5D623uL),
                    (-1, -5, 0xA3D70A8F5CFDBBA0uL, 0x71A9A8CA784BF5F2uL),
                    (+1, -5, 0x9D89D8C4EC92F3E9uL, 0x5AB513F0C18E1DD3uL),
                    (-1, -5, 0x97B426000015E20AuL, 0xB4774BCD423BA824uL),
                    (+1, -5, 0x9249249B6DBDE3E4uL, 0xC63869E597816D33uL),
                    (-1, -5, 0x8D3DCB0D3DCD4C3DuL, 0xD25A1DC8C88BD742uL),
                    (+1, -5, 0x8888888AAAAB655BuL, 0x063B7A506E1E3948uL),
                    (-1, -5, 0x8421084318C66DC4uL, 0x964E06A2E7462BFDuL),
                    (+1, -5, 0x8000000080001371uL, 0xFB227A6A8D4C0A99uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX2CoefTable = new(new ddouble[] {
                    (+1, -2, 0xD8773039049E70B6uL, 0x5C8380FDFD5A6952uL),
                    (+1, -2, 0xA51A6625307D3230uL, 0xE7B1224401759CBDuL),
                    (-1, -4, 0x89F000D2ABB03409uL, 0x2E83A0EFBC2BFD9DuL),
                    (+1, -6, 0xA8991563EC241B5FuL, 0x91211196E5235FBCuL),
                    (-1, -8, 0xF2027E10C7AF8C36uL, 0xB3B061C2998701E7uL),
                    (+1, -9, 0xBD6EB756DB617EA4uL, 0x87D7380280B91426uL),
                    (-1, -10, 0x9C562E15FC703E75uL, 0xB3E30263137AD8D9uL),
                    (+1, -11, 0x859B57C31CB745F2uL, 0xCE526EDAD3266DC3uL),
                    (-1, -13, 0xE9FEA63B697E3E38uL, 0x3AA3033447D29CB5uL),
                    (+1, -14, 0xD093D878BEB2D19DuL, 0x309AA7002679085CuL),
                    (-1, -15, 0xBC6F2DEBE40F7797uL, 0x7EAF8C86E1666C25uL),
                    (+1, -16, 0xAC06E77337581126uL, 0x0574B258F72222C0uL),
                    (-1, -17, 0x9E5E4B1E7112142BuL, 0x523270559AEA87D7uL),
                    (+1, -18, 0x92CBD1CF9A555C80uL, 0xDDD73AB04FEBC73FuL),
                    (-1, -19, 0x88D975BB3CAA08E3uL, 0xB58732D631CD43F0uL),
                    (+1, -20, 0x803266F5917879D0uL, 0x156AFFDBC10B5834uL),
                    (-1, -22, 0xF13006C9E7E975D9uL, 0xEA78C347DF3575C5uL),
                    (+1, -23, 0xE3B5DD9F83D26BB3uL, 0x456EEED36A479B3AuL),
                    (-1, -24, 0xD7AD365DFC54BB2BuL, 0xE44FD2DB07C41AF7uL),
                    (+1, -25, 0xCCDC9E1038587A06uL, 0x4E2C8E6C84F4F593uL),
                    (-1, -26, 0xC31639A6F9F56366uL, 0x50057D81B40BD395uL),
                    (+1, -27, 0xBA34ED667D6E6592uL, 0xC58EE628AAEFA88BuL),
                    (-1, -28, 0xB21A54223D75681AuL, 0xF72EDF876FCCC233uL),
                    (+1, -29, 0xAAAD43BFFE9614F1uL, 0x5F341B2B789DB0F1uL),
                    (-1, -30, 0xA3D8B3C92C687209uL, 0xE6DC1D0A9C7A1E78uL),
                    (+1, -31, 0x9D8AE9597E085E28uL, 0x60901114D74D2998uL),
                    (-1, -32, 0x97B4D4FD5F1EFCBDuL, 0x3AA991670EDADCEEuL),
                    (+1, -33, 0x92499519BA1A620CuL, 0x1053848A11D1B0ABuL),
                    (-1, -34, 0x8D3E13761291E29EuL, 0x9D7B6940969463C2uL),
                    (+1, -35, 0x8888B7349F6CBC71uL, 0xF9656C30072B8E20uL),
                    (-1, -36, 0x8421265E2A1EC140uL, 0xEF820DBC9EA5B468uL),
                    (+1, -37, 0x80001371FB227A6AuL, 0x8D4C0A99112BE14BuL)
                });
            }

            public static class Digamma {
                public const int Threshold = 11;

                public static ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable = new(new (ddouble s, ddouble r)[]{
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, -4, 0xCCCCCCCCCCCCCCCCuL, 0xCCCCCCCCCCCCCCCCuL)),
                    ((+1, -8, 0x8208208208208208uL, 0x2082082082082082uL),  (+1, 0, 0x8666666666666666uL, 0x6666666666666666uL)),
                    ((+1, -8, 0xF83E0F83E0F83E0FuL, 0x83E0F83E0F83E0F8uL),  (+1, 1, 0xB231231231231231uL, 0x2312312312312312uL)),
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, 2, 0xAA36363636363636uL, 0x3636363636363636uL)),
                    ((+1, 1, 0xC373FCDCFF373FCDuL, 0xCFF373FCDCFF373FuL),   (+1, 3, 0x8A9B6331EAB8DA7EuL, 0xFDB2ACC2FD4737B6uL)),
                    ((+1, 8, 0x8CBAE6076B981DAEuL, 0x6076B981DAE6076BuL),   (+1, 3, 0xCD12F438911D0D30uL, 0x5CCEF88EB6F5D78FuL)),
                    ((+1, 15, 0xD62B955555555555uL, 0x5555555555555555uL),  (+1, 4, 0x8E4143C38A48A430uL, 0xA097076AD0A5339CuL)),
                    ((+1, 24, 0x98FD6BE5F9DFFE17uL, 0xE77FF85F9DFFE17EuL),  (+1, 4, 0xBC751842CF4E9497uL, 0x70E668923E28A49CuL)),
                    ((+1, 33, 0xBC4976FEFAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, 4, 0xF124F8842E1C48A0uL, 0xD06011ECEFE36DB1uL)),
                    ((+1, 43, 0xBB012610AE915555uL, 0x5555555555555555uL),  (+1, 5, 0x9628724E1B92443AuL, 0x6840275F06887788uL)),
                    ((+1, 54, 0x8E651CDBBD813979uL, 0xE958D7E43E7A5635uL),  (+1, 5, 0xB6FC6E4651367267uL, 0x0F2944F9F6122852uL)),
                    ((+1, 65, 0x9F8658358BF14630uL, 0x9C07432D63DBB01DuL),  (+1, 5, 0xDB0E702AC9B318AAuL, 0xC812A8F246FAF8F4uL)),
                    ((+1, 76, 0xFE23A5EEA9E101EAuL, 0x44D3364D9364D936uL),  (+1, 6, 0x812F3BFDC335A2BAuL, 0xBF33D1C77579DA39uL)),
                    ((+1, 89, 0x8BE36E94549BC383uL, 0x3183A1C126A99EFFuL),  (+1, 6, 0x967642DC43BD1134uL, 0x32FDF1146F4122ACuL)),
                    ((+1, 101, 0xCFBD9FD1BF989F60uL, 0xE2608D3CA7BC05C9uL), (+1, 6, 0xAD5C4CB0E670DCD6uL, 0x7D262473FFCAA985uL)),
                    ((+1, 114, 0xCBC91757DAE8B62EuL, 0x38D01E72BE6542AAuL), (+1, 6, 0xC5E1597BAB5118EFuL, 0x82DA12535E50F793uL)),
                });

                public static ReadOnlyCollection<ddouble> TaylorZeropointCoefTable = new(new ddouble[] {
                    (+1, -1, 0xF7B95E4771C55D8FuL, 0x0C28D0814E530F17uL),
                    (-1, -2, 0xE2B1DAA550D1AB8EuL, 0xC060D20DBFB8B839uL),
                    (+1, -2, 0x845A14A6A81C05D6uL, 0x6D7B6900032EA171uL),
                    (-1, -3, 0xA7E098B92BED4186uL, 0xB1CC35DBC77717C5uL),
                    (+1, -4, 0xDCD2DB1B879D54BEuL, 0xA5BB6701257D9AE9uL),
                    (-1, -4, 0x93DD5D130E615E39uL, 0x112646269ACB6FADuL),
                    (+1, -5, 0xC7E701591CE534BDuL, 0xCE5785C71958A783uL),
                    (-1, -5, 0x87D3F61B53EC74F7uL, 0xB974CEEC1C8E4479uL),
                    (+1, -6, 0xB91EB403F6E601F3uL, 0x03AB27FC78546609uL),
                    (-1, -7, 0xFCB828470DB50E3BuL, 0xDE9CB3BBCBD07F19uL),
                    (+1, -7, 0xACAAE5554B1799D3uL, 0x608DED8B2BC7C2DBuL),
                    (-1, -8, 0xEC1403C94175BC26uL, 0xD38DD7FDE3176ECEuL),
                    (+1, -8, 0xA170D67C0EC6E1A2uL, 0xC77E2ED9FBB6EB4DuL),
                    (-1, -9, 0xDCD7E3E774509DBBuL, 0x8A3BE79375D38117uL),
                    (+1, -9, 0x97119A2FBCD575A0uL, 0xDB3291E0A10B65E2uL),
                    (-1, -10, 0xCEB137B8E8FBD21EuL, 0xCF44BBCA27C8C934uL),
                    (+1, -10, 0x8D675DEBB08444B6uL, 0xFBD3A05C1AB57FD1uL),
                    (-1, -11, 0xC17B1A2E32D9A67CuL, 0xEA3F72854D6949FFuL),
                    (+1, -11, 0x845ED70D130EA396uL, 0x040CF3D77D20E1ABuL),
                    (-1, -12, 0xB51FEEF50898212DuL, 0x6D777952AF76243EuL),
                    (+1, -13, 0xF7D655B7DC4C2743uL, 0x5893CEC3C2777BE2uL),
                    (-1, -13, 0xA98FAEE2F5AB1930uL, 0x68DF0FDDDE360A15uL),
                    (+1, -14, 0xE80407C087D5AB71uL, 0x1808B6CE80E54B44uL),
                    (-1, -14, 0x9EBCB934457B3718uL, 0xF9DD497E02D9ED9AuL),
                    (+1, -15, 0xD93488C162E19F3DuL, 0x81B14BA0058FE529uL),
                    (-1, -15, 0x949ABFBEB65C3335uL, 0x30F69A4CF3A566ADuL),
                    (+1, -16, 0xCB572547195A4657uL, 0x8628291C2E71B177uL),
                    (-1, -16, 0x8B1E639B9DF4552DuL, 0xF04BBB7240BEB178uL),
                    (+1, -17, 0xBE5C59C8B7E9489CuL, 0x6E3524C368BB5487uL),
                    (-1, -17, 0x823D0C4A707EF06EuL, 0x7BC750E6D8B75220uL),
                    (+1, -18, 0xB235AA7A085EF65DuL, 0xDFFE6198BB04CDDAuL),
                    (-1, -19, 0xF3D9A52DBC51E21FuL, 0x315F0A58002D8212uL),
                });
            }
        }
    }
}
