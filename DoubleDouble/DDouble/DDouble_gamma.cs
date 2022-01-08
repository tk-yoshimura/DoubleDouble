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

            if (x < 1.5d) {
                ddouble v = x - 1d;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeX2Table;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * v + c;
                    sd = sd * v + d;
                }

                ddouble y = sc / sd - Log1p(v);

                return y;
            }

            if (x < Consts.LogGamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 2));
                ddouble v = x - n - 2;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.LogGamma.PadeTables[n];

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
                ddouble p = (x - 0.5d) * ddouble.Log(x);
                ddouble s = SterlingTerm(x);

                ddouble k = Consts.LogGamma.SterlingLogBias;

                ddouble y = k + p + s - x;

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
            if (Abs(x_zsft) < Math.ScaleB(1, -3)) {
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeZeroPointTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * x_zsft + c;
                    sd = sd * x_zsft + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = Math.Max(0, (int)Round(x - 1));
                ddouble v = x - n - 1;

                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Digamma.PadeTables[n];

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
                ddouble s = DiffLogSterlingTerm(x);
                ddouble p = ddouble.Log(x);
                ddouble c = Rcp(x) / 2;

                ddouble y = -s + p - c;

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
                public const int Threshold = 16;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;

                static LogGamma() {
                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
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

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table
                 = new(new (ddouble c, ddouble d)[] {
                    (Zero, (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -2, 0xD8773039049E70B6uL, 0x5C8380FDFD5A6952uL), (+1, 1, 0xD5C2EF2F19D31DFFuL, 0x2EA6859A83CB194AuL)),
                    ((+1, 0, 0xDE06993A61E9DD7CuL, 0xBA7CBF3C2F2245DEuL), (+1, 2, 0x9EB9C887A4622DA3uL, 0x904453BEE601B962uL)),
                    ((+1, 1, 0xC6D57F0E73E906A1uL, 0x20303047666EFC79uL), (+1, 2, 0x8A234F15D0A1B832uL, 0x08AF501AD32B5618uL)),
                    ((+1, 1, 0xCE17B2992A7613F1uL, 0x1F40BDA17F08A6D1uL), (+1, 1, 0x9C5755132F333027uL, 0x2F876E44AC9208ECuL)),
                    ((+1, 1, 0x89BBEB9AFE18E880uL, 0x12D77148D92E68D9uL), (+1, -1, 0xF15ACFC7BCA5273EuL, 0x7E9BF98FD4692D90uL)),
                    ((+1, -1, 0xF9D3EBA4F2CB70B9uL, 0xBF74152C0BC35163uL), (+1, -2, 0x819F50C1166B9F09uL, 0x9DA40EC19B31598DuL)),
                    ((+1, -2, 0x9D48D1F5E3E6F0AAuL, 0x4162D5AD26DB194AuL), (+1, -5, 0xC214BCAEFF7D2319uL, 0x5CB481CF59904D43uL)),
                    ((+1, -4, 0x8A18208A42FE3099uL, 0xD50CBE95237118DAuL), (+1, -8, 0xC7B9C0F21338E7BDuL, 0x2ACB02C78E2412DDuL)),
                    ((+1, -7, 0xA73CBE09DA9BFCA1uL, 0x12B0B661F3B0DB51uL), (+1, -11, 0x88E7C54FADE10DE1uL, 0x53D4A84163D03CC1uL)),
                    ((+1, -10, 0x87DE9D05F5D4BE04uL, 0xEC11E824353EAB84uL), (+1, -16, 0xEC9838432650637DuL, 0x05B40DA7F185C17AuL)),
                    ((+1, -14, 0x8CE3B08F23F7C1FFuL, 0xF59B19A7BEF22607uL), (+1, -21, 0xEA40F74C050742B8uL, 0x4D16F6F42D7E8401uL)),
                    ((+1, -19, 0xAB0A20A42FFB2BBAuL, 0x223CF6C7B00E561FuL), (+1, -27, 0xDDFFD5DFE8D26A87uL, 0xA604C5D465853925uL)),
                    ((+1, -25, 0xCF2542A04D70E30AuL, 0x5F41921BD93FB8EEuL), (+1, -34, 0x8137FFB0A64AFFC4uL, 0xE23AC8141B1C4A24uL)),
                    ((+1, -32, 0xACBAB2A53357D7FAuL, 0x517F5F070DDCBC6DuL), (-1, -46, 0xFDBEBF7461B03852uL, 0x8620F88C5A90792EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, -1, 0xB17217F7D1CF79ABuL, 0xC9E3B39803F2F6AFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0x8E9E33E514053A30uL, 0x4AA55F68BC43CAEAuL), (+1, 0, 0xF11A41C30D487C6AuL, 0x8297DCC24D9EA2C4uL)),
                    ((+1, 1, 0xC056505C4493119BuL, 0x2D70ABB663F78233uL), (+1, 0, 0xC58605E4734F7864uL, 0xB7439C6DCAE7E0E6uL)),
                    ((+1, 1, 0x914995983FC47544uL, 0x81E478E80F92FDA6uL), (+1, -1, 0xB89C5AB6B3986F27uL, 0xF693B1F25F998C42uL)),
                    ((+1, 0, 0x896BD98AB571D57EuL, 0xA47F4A3A79DB667CuL), (+1, -3, 0xD8F3C1E0DE8CE814uL, 0xEF9782EC51D98A62uL)),
                    ((+1, -2, 0xAB588B9A658C41F0uL, 0xD5A7D7C511ACBC97uL), (+1, -5, 0xA693BC31806D8A47uL, 0x500EB5C39552D8B4uL)),
                    ((+1, -4, 0x8FC2CDF1A794C582uL, 0xCF7639628E58A223uL), (+1, -8, 0xA83D67731290FC37uL, 0xDB3CA554E2AB609EuL)),
                    ((+1, -7, 0xA236E4B59C2ADF1AuL, 0x0193F0CF354F507DuL), (+1, -12, 0xDB6F013B8CC8E121uL, 0x1C852240DCA24130uL)),
                    ((+1, -11, 0xF129B2E25BAC82AAuL, 0x2E064199BE5F242BuL), (+1, -16, 0xB068F5D1EEDBAACAuL, 0xB6E364BC07B89CC6uL)),
                    ((+1, -15, 0xE1E03DE5765A5A7AuL, 0xB665601CABBA176AuL), (+1, -21, 0x9FF156DF92170A5DuL, 0x91BD77B3244659E0uL)),
                    ((+1, -20, 0xF585C8768F5001F4uL, 0x32B6C998EE5FF14CuL), (+1, -27, 0x894EB079FF3E3FDAuL, 0xFB76E181C1BEE0BBuL)),
                    ((+1, -25, 0x8469D8685818756BuL, 0x737FB38381F9D551uL), (+1, -35, 0x8FF7A2BFAFE33427uL, 0x8F5AA36D90D52E8DuL)),
                    ((+1, -33, 0xC4340B38BA0383F7uL, 0xB0A872234FA6A2C2uL), (-1, -47, 0xF7F073AFB4F056B7uL, 0x802CD54C402F970DuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xE5585FD151001191uL, 0x36FEA076849739B2uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0xE3A2356785758C66uL, 0x3BEB5BF802BD2DE9uL), (+1, 0, 0xA45AD1649B83C98DuL, 0xD1140B0976E615A0uL)),
                    ((+1, 1, 0xC159B6D54F4F100CuL, 0xABC51D96364ADF9BuL), (+1, -1, 0xB4ECE05680F143C2uL, 0x68C1CF6F9AE313B4uL)),
                    ((+1, 0, 0xB9367B5693732E68uL, 0x85035E7BA9ABADDFuL), (+1, -3, 0xDF16441E40E64DDFuL, 0xF60B4BA5694AB666uL)),
                    ((+1, -2, 0xDD79BC69349C9175uL, 0x21E6CD90E0027B3EuL), (+1, -5, 0xA8E35731602FC321uL, 0x22F7D6E7672A1553uL)),
                    ((+1, -4, 0xAC7C22DA209D949DuL, 0xA3742A5862E629B2uL), (+1, -8, 0xA1E215C654B94853uL, 0xC069D348B89F2C6FuL)),
                    ((+1, -7, 0xB0E2184AF38D58FEuL, 0x57996F26EA0FB87AuL), (+1, -12, 0xC3648AA9CC9BF535uL, 0xA717A7CF5987A236uL)),
                    ((+1, -11, 0xEBA09ED2B37A142FuL, 0x0D60C5E5FE68B468uL), (+1, -16, 0x8EDF6F7564B6646AuL, 0xDB5E3F58718D6EE2uL)),
                    ((+1, -15, 0xC3D97087867848A1uL, 0xF5AD625368E61DBAuL), (+1, -22, 0xE8D756124E28680BuL, 0x52BCB4952B1D88AAuL)),
                    ((+1, -20, 0xBBBA3259D4F6DF8AuL, 0x990D5EA7FA2F0F92uL), (+1, -28, 0xB23D29AB81E7183BuL, 0x484FFEA7B8882F3BuL)),
                    ((+1, -26, 0xB1E185A46F4AEC91uL, 0xCF3C56B0E96051CCuL), (+1, -36, 0xA603F1FA0C0FBC20uL, 0xC411546279B5980AuL)),
                    ((+1, -34, 0xE721A2EB068A5EBDuL, 0x067629068C1BFC40uL), (-1, -48, 0xF7076F62BCE1E6AFuL, 0x0493FCA71D6501A7uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xCB653BE49167C59EuL, 0x80712A0744451830uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0x8E17F56EAFCF1DD9uL, 0x390753649D0012E6uL), (+1, -1, 0xEC5D97AE9CB0B2F3uL, 0x917C4F88ECEA6285uL)),
                    ((+1, 1, 0xA914B9A9592FA2B5uL, 0xE453AF3E43727219uL), (+1, -2, 0xB7C2A44C32AD8DA8uL, 0x85137FC369A88F1DuL)),
                    ((+1, -1, 0xE0907756E0AAF26DuL, 0x8081C85044464E5BuL), (+1, -4, 0x9C570A6D30E0070AuL, 0x16CE8679C341F2FEuL)),
                    ((+1, -3, 0xB714E1E3222A07A4uL, 0x1F739EEB8F644D39uL), (+1, -7, 0x9E586401724222ECuL, 0x15222E374D282D45uL)),
                    ((+1, -6, 0xBDA6436594DB0DEBuL, 0x855792EA75F1FA03uL), (+1, -11, 0xC2840FB65ED55BD8uL, 0x531611EFBFF19F70uL)),
                    ((+1, -10, 0xF96195FD93A97EF7uL, 0x87E62BB3BF51955AuL), (+1, -15, 0x8D454890268F181CuL, 0x4C95E948B86D0E70uL)),
                    ((+1, -14, 0xC985464165295BD1uL, 0xE0C1EBB3105F120BuL), (+1, -21, 0xE0FD8912BA295FAFuL, 0x4CF6BF86175F5B44uL)),
                    ((+1, -19, 0xB9F132BB717200AEuL, 0x4ADFD60F9FF82BA9uL), (+1, -27, 0xA687F99A5C7E46B8uL, 0x4260BA47714AD61CuL)),
                    ((+1, -25, 0xA898DDCA8A87ADEAuL, 0x9972456946FAFD89uL), (+1, -35, 0x9511F252FB2A9D1BuL, 0xCE7A9BFC83EADE3DuL)),
                    ((+1, -33, 0xD10EDAFF99EC9B46uL, 0x0404BCB76E2A5341uL), (-1, -47, 0xD2A40400CE72DF8FuL, 0xF5FACD171AF10CDEuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0x993321E223CE49A5uL, 0x3C2789A6630DB256uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xAC56C20632A97AB6uL, 0x66348E8AF17DC9EEuL), (+1, -1, 0xC4C052F860C0A399uL, 0x29678F49A83B536CuL)),
                    ((+1, 1, 0xA5EAEF787E63B770uL, 0xEC5B04AB743A7B84uL), (+1, -3, 0xFEA5C36AC17EC95BuL, 0xD21DCAB463A9A3F3uL)),
                    ((+1, -1, 0xB30301FB24AE614EuL, 0xFE0A174A8AF8AF97uL), (+1, -5, 0xB454C4D04968C1A4uL, 0x8B7D16A67450A774uL)),
                    ((+1, -4, 0xEDE6C5F897D9B35AuL, 0x88D0ED15F6653869uL), (+1, -8, 0x980573A828B50F1BuL, 0xC197DF8BF692B2CEuL)),
                    ((+1, -7, 0xC966C1EAD9A9707CuL, 0xE41121B71DC708F7uL), (+1, -12, 0x9B6E6D9D98C22E53uL, 0x68F22CCF6272A1D8uL)),
                    ((+1, -11, 0xD8EA12E6AD692BC3uL, 0xBF9CA50B56C02B3DuL), (+1, -17, 0xBBE81C969789BDABuL, 0xAD8D389C50FF3394uL)),
                    ((+1, -15, 0x8FD1FDF1A497858AuL, 0x7D43888B7DED59CCuL), (+1, -23, 0xF9164F8E4E956AF2uL, 0xA1C2505FA803BE23uL)),
                    ((+1, -21, 0xDA0E3759DE24FABFuL, 0x804132E17065A92AuL), (+1, -29, 0x997FE45B0ABF16FDuL, 0x37A16DD689EE8BD4uL)),
                    ((+1, -27, 0xA2967B121EB83EB2uL, 0x3B20F7D9A397C4F8uL), (+1, -38, 0xE535ECF79F0F3B2AuL, 0xC677307CA7817A0FuL)),
                    ((+1, -35, 0xA5D572E0511C9BFAuL, 0x751994070C687CF2uL), (-1, -49, 0x82410718ECF2B310uL, 0x34E03D23271AEC38uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0xD28939D6780E4E09uL, 0x89E731C4043380C3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xB72D6AFF4F27783CuL, 0x8A87B0141884795EuL), (+1, -1, 0x95DCEF87D2421D45uL, 0x66599AD405F4B34AuL)),
                    ((+1, 1, 0x867B48C3EB4A93D9uL, 0xCD955FAEB9FF5792uL), (+1, -3, 0x907607EE2E64BC91uL, 0x0EFEABBFD7ED9C7AuL)),
                    ((+1, -2, 0xD914EE2F07B16521uL, 0x3994FAE6810623B8uL), (+1, -6, 0x93E00B3B5418DDA0uL, 0x1B2D08686538E8E4uL)),
                    ((+1, -5, 0xD22F4B9A0E4D1728uL, 0x7FC26CCCE7975AE2uL), (+1, -10, 0xACCCFA4AA4CDF3D8uL, 0x73D7CA9940FA6B8EuL)),
                    ((+1, -9, 0xF9BFB9F97DBBAE8AuL, 0x758EC20B1BC1C7C0uL), (+1, -15, 0xE6406402F53B3D87uL, 0x6C057D6051846AA4uL)),
                    ((+1, -13, 0xB294EDAFDB719251uL, 0x5D53B62DE96AF9B3uL), (+1, -20, 0xA460D4AE0B305933uL, 0x4B6D053D820F808CuL)),
                    ((+1, -18, 0x8FEA8E674A15D40FuL, 0x8B2620EFFE8AE306uL), (+1, -27, 0xD708C0B559B6C030uL, 0xC43C03E168B4DA18uL)),
                    ((+1, -25, 0xE23A1FC0475E67A2uL, 0x2BE7E1FCA69CE8DAuL), (+1, -35, 0xA8D92A6DBB1C8A75uL, 0x8B8150015692F376uL)),
                    ((+1, -33, 0xF25407F118B797B4uL, 0x12C97C4CF852504AuL), (-1, -47, 0xCAD1ACD580911856uL, 0x5AFA70535F3322CEuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0x88670F996E617E59uL, 0x345CE24EF47B7E66uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xCBF6DCB1DF55488AuL, 0x470D1AAF30DFD898uL), (+1, -1, 0x82DF642AFE40C464uL, 0x6B3D3F6A72D427F6uL)),
                    ((+1, 1, 0x80E76DFC5CDDCB39uL, 0xA9FC1FD0A6422DA7uL), (+1, -4, 0xDC51C1BD1CEC9CDCuL, 0xD0915E7E57F63CEEuL)),
                    ((+1, -2, 0xB3654B69067F6DA2uL, 0x4787203FD26DBFEAuL), (+1, -7, 0xC4EAAF8FC572CE5CuL, 0xD9EC79A363C51522uL)),
                    ((+1, -5, 0x95F2A77D680603BFuL, 0x374623A7D42AF343uL), (+1, -11, 0xC8E8027CC136DC83uL, 0xE0FF5C780ACF972BuL)),
                    ((+1, -9, 0x99FB1A9662FBC943uL, 0xBC3C5B4B19304AE1uL), (+1, -16, 0xE9B8C802CAABB794uL, 0x10B657BFF60D720AuL)),
                    ((+1, -14, 0xBE79E1633AA598E2uL, 0x98892A5EC1FE25D5uL), (+1, -21, 0x91AE925B4274BABBuL, 0x0EFEB8451A924231uL)),
                    ((+1, -19, 0x84DE37D493948971uL, 0xAE6361739C88389FuL), (+1, -28, 0xA66E3CD38E1737F6uL, 0xAD662CD3215169AFuL)),
                    ((+1, -26, 0xB4DD515AD0FE50F7uL, 0x5377920C8F77DF89uL), (+1, -37, 0xE48D5D6DFE4F982CuL, 0x1118A203747BAFC8uL)),
                    ((+1, -34, 0xA7C6EAFC8BE30084uL, 0x256363D101466249uL), (-1, -49, 0xE9DCF9E5D3442C68uL, 0x720C86EA032B6869uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xA9AC7417E5B86529uL, 0x6A3793FB75390CA7uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xDE7DCB1AC9ADBB5FuL, 0x7ACD3B64168E2836uL), (+1, -2, 0xE8568FF1CDA35ABFuL, 0x8419482BDF6F673DuL)),
                    ((+1, 0, 0xF6E83F83E4082A77uL, 0xFC70448F6BB4DF57uL), (+1, -4, 0xAD95195F602AFACAuL, 0x2A647D3B8967B8FEuL)),
                    ((+1, -2, 0x97000313E44C8A6BuL, 0x835F41160F4A2F73uL), (+1, -7, 0x89B20136A12BF78EuL, 0x810E367D3110FE6DuL)),
                    ((+1, -6, 0xDE0CC5DC3BB23175uL, 0x50B62838BD49F6F3uL), (+1, -12, 0xF95B3B4E04F5257BuL, 0x7025544BD15F8837uL)),
                    ((+1, -10, 0xC8BBA381D44C4CC3uL, 0x2683ACCC5E7A1B03uL), (+1, -16, 0x80B816FE856C1809uL, 0x6D28AFE41E2D303EuL)),
                    ((+1, -15, 0xDAB9B9EF5DDAF195uL, 0x8DDBF004AA2323A5uL), (+1, -22, 0x8E68841B5F3F3676uL, 0x6E015B3D552A1236uL)),
                    ((+1, -20, 0x86752E310ED64901uL, 0xB853147392AB4D71uL), (+1, -29, 0x9068F5D555AFB46BuL, 0x48850EBA342C9BE1uL)),
                    ((+1, -27, 0xA156E8C8BA765BC4uL, 0x8FA2EE584D837734uL), (+1, -38, 0xB03873ADC6B14AC3uL, 0x9433E6D656DDB254uL)),
                    ((+1, -35, 0x83EC033F4F78F7C0uL, 0x20237C402649FF53uL), (-1, -50, 0x9CA9CFE94553CF2FuL, 0xE2D3FCD73A6A86A0uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xCCD4490D3FBE7A58uL, 0x3EBAC5A615E07C3DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xDAAE9330B1852A82uL, 0x11EF85F8934FB756uL), (+1, -2, 0xB74193021B3AA33CuL, 0xC7A5BA7F2C706894uL)),
                    ((+1, 0, 0xC1CAB890157D706EuL, 0x19416B628287E1D3uL), (+1, -5, 0xD1B7EC36B897B061uL, 0x44FE81BCAF76B80BuL)),
                    ((+1, -3, 0xB85D593F30FC8806uL, 0x83DF3FD03D74C881uL), (+1, -9, 0xF49022B0C8501C71uL, 0x999243185AE9481AuL)),
                    ((+1, -7, 0xCB35BD5C24DB1E37uL, 0x1EF6DD6E8895EE01uL), (+1, -13, 0x992F705CC3F9E41BuL, 0x32B0AF60A30CABFFuL)),
                    ((+1, -11, 0x825BE2507653402CuL, 0x9438245373F3BB09uL), (+1, -19, 0xC68CBD40AA3634B3uL, 0x6661240684121888uL)),
                    ((+1, -17, 0xB8C1E3764FD4D627uL, 0x9AD55933335D03CCuL), (+1, -26, 0xE6D8D8BA962157B9uL, 0x49DB2D6DEF5C9366uL)),
                    ((+1, -24, 0xFC76CA32114CED44uL, 0x51D442D384B01387uL), (+1, -34, 0x9F501ED1E663FB1FuL, 0x8D3841D0E3BCB524uL)),
                    ((+1, -32, 0xE9D51431735DC28EuL, 0xA93B972CBC0576B4uL), (-1, -46, 0xA2D3F8ED690F2728uL, 0x532762E49E05DD7EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xF1ABAC84AA68A55DuL, 0xF9507B30F6953EC7uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xE8655B1676B51060uL, 0xC8FE20E0A8D33B6AuL), (+1, -2, 0xA67507894AC650C4uL, 0x440840CF4BE67CC0uL)),
                    ((+1, 0, 0xB9A10162A67FA4E1uL, 0xFD429017C5FE63DCuL), (+1, -5, 0xAD0758ACC41E6413uL, 0x48191841A69AB576uL)),
                    ((+1, -3, 0x9F47F89A34D9960EuL, 0x7D1CA044048B35F2uL), (+1, -9, 0xB746B221F707B287uL, 0x6CCD45BA07CABD31uL)),
                    ((+1, -7, 0x9E6FD761372EE2A3uL, 0x59DCBA04A58D8FF6uL), (+1, -14, 0xD08B511B65008738uL, 0x1C5ADB36EC1C8249uL)),
                    ((+1, -12, 0xB7889F633520EDE9uL, 0x805B7FFA7CE3EF32uL), (+1, -20, 0xF587BD703FC1103CuL, 0xB330ADBA62D97E20uL)),
                    ((+1, -18, 0xEAF2D42441F48813uL, 0x412FF64EC62F050CuL), (+1, -26, 0x81ACEE98227DB2F9uL, 0xD9986C95B389F0A8uL)),
                    ((+1, -24, 0x9105BEF195548A89uL, 0x18F45E0982B05A89uL), (+1, -35, 0xA2C11B7F4E3FF903uL, 0x05264449B08FDAEAuL)),
                    ((+1, -33, 0xF2B0B98427FAA0F7uL, 0xA16FCABFBA2D9E1FuL), (-1, -47, 0x948B6AE70199AB30uL, 0x93999342929DE36EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX12Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x8C04B9F9D46B9E66uL, 0xB904FD652337CF07uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xF4F2319E02DD34C2uL, 0xCE7B57B996482622uL), (+1, -2, 0x98771CD7594FAE66uL, 0xF2A51B68342167D0uL)),
                    ((+1, 0, 0xB21018AE02AEC2C0uL, 0x654E2955280A792FuL), (+1, -5, 0x912971EA4359F069uL, 0x98525F1DFBD74276uL)),
                    ((+1, -3, 0x8B1EFE09AE3ED9DFuL, 0xF2B6B7271BC5568FuL), (+1, -9, 0x8CD563846DFF68B2uL, 0xBAE2DCC085B9F3EDuL)),
                    ((+1, -8, 0xFC1E7E14A8EF6D81uL, 0xF4CAAAF2CDE30C79uL), (+1, -14, 0x92C7E318E1773108uL, 0x557F24578B1222C4uL)),
                    ((+1, -12, 0x85126DDD6D74BBBBuL, 0xAB0E8B37874C051FuL), (+1, -20, 0x9E4B85CC5DAB6403uL, 0x3F3E2519EA3BEDE9uL)),
                    ((+1, -18, 0x9B47EF94B7C5FA92uL, 0x33304C4212DCF6E5uL), (+1, -27, 0x992ECBBB743C2DEFuL, 0xD54A0D8C39F365BAuL)),
                    ((+1, -25, 0xAEC3F8306EF608CBuL, 0xB586AE089CD9A6FCuL), (+1, -36, 0xB04699F03730DA88uL, 0xC0E6F0FA3C39D1CEuL)),
                    ((+1, -33, 0x854F406086011501uL, 0xCDE7270EC0F8ABF0uL), (-1, -48, 0x912223B442597FB2uL, 0xB16665AD4E50795CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX13Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x9FE5D0B6A80A1B4DuL, 0x2AC405094BA0DA57uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x8040F8322840E3DEuL, 0x4ACA1114C9BB525AuL), (+1, -2, 0x8CA167B561233538uL, 0xE8A1A08474D86EF3uL)),
                    ((+1, 0, 0xAB10563DCB095D9FuL, 0xE4E011BE97ADF835uL), (+1, -6, 0xF70055417A188D0BuL, 0x60429298356726A2uL)),
                    ((+1, -4, 0xF5518235A586EE03uL, 0xEF1B1D35820451CBuL), (+1, -10, 0xDD0929475CD6C108uL, 0xCAE91F117F700F9BuL)),
                    ((+1, -8, 0xCC127D852BD2717AuL, 0xFE8F5F6A92E1799EuL), (+1, -15, 0xD47DF1E6BF6A6907uL, 0x285BC7737782592FuL)),
                    ((+1, -13, 0xC5D450A72DC26E76uL, 0xBB20AA3610A389B5uL), (+1, -21, 0xD362FBCF4E8A5F11uL, 0x64E622B49AB50C0FuL)),
                    ((+1, -19, 0xD409A1BB28A3783DuL, 0xC7181E9BEE0862F0uL), (+1, -28, 0xBCB8B233FB10E997uL, 0x7F39471710070F76uL)),
                    ((+1, -26, 0xDB399A5CB1FECC04uL, 0x29498E19EFBDA9FEuL), (+1, -37, 0xC88088F8B779045FuL, 0x708AC98A99BC0BF7uL)),
                    ((+1, -34, 0x999A50E0085F7E13uL, 0x3D65A3E4D695C4EEuL), (-1, -49, 0x962E9A20F7BA8472uL, 0x459A6EA8294CFD6BuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX14Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xB46AD4E1D9966C4BuL, 0xB141D401EF9E5C5FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x859C31D5275A7C4EuL, 0xE3BE33D0704F4F6AuL), (+1, -2, 0x827D6C0435759B21uL, 0xA9FB122734C9A7C9uL)),
                    ((+1, 0, 0xA49726B399D2F1F3uL, 0x83F875227739A8B2uL), (+1, -6, 0xD4AA1E85E41071E2uL, 0x848912638C0B57BFuL)),
                    ((+1, -4, 0xDA10B1162A8CAAD9uL, 0x6FE3BBC3B14F1043uL), (+1, -10, 0xB0965CC45BA6782CuL, 0x3CA277D573B94A9FuL)),
                    ((+1, -8, 0xA7A2DAA5A260D812uL, 0x62457936BF2DDFAFuL), (+1, -15, 0x9D864C4787B5A35BuL, 0x5CA26931DF22AE78uL)),
                    ((+1, -13, 0x9635EC259752F6DDuL, 0x4AB9B22917CDD0F3uL), (+1, -21, 0x916A7DDDEF2CFB89uL, 0x1A35A0EAE26FB225uL)),
                    ((+1, -19, 0x94D73FF06A4F8051uL, 0xF6AFC356652CD74FuL), (+1, -29, 0xF0F9DFAF2D04149CuL, 0x4F2277FE2BF5EEA2uL)),
                    ((+1, -26, 0x8E46ADB6A6A6EB16uL, 0x7DFD7FB656DB4C9CuL), (+1, -38, 0xEDC35C98B31E499BuL, 0x6CAC7DBEA0D413B7uL)),
                    ((+1, -35, 0xB8509C89A17A1CC7uL, 0x674FFD2A96EDBEFDuL), (-1, -50, 0xA32FACFDB2E333A2uL, 0xD3413566A9FB1548uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX15Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xC9879EF8B15213C3uL, 0x4745965528EED317uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x8A993F9C3D5D163BuL, 0xC30510167CF0C81AuL), (+1, -3, 0xF368D14FDA2AA814uL, 0x30A34E144DE75AE7uL)),
                    ((+1, 0, 0x9E9921D8FA0594B3uL, 0x27378A723D6BD3E0uL), (+1, -6, 0xB8FE701B14F415B5uL, 0xBBEC2C3BA382B7DBuL)),
                    ((+1, -4, 0xC33C7EBA3C0E1A02uL, 0x7C955FE0EDA75F07uL), (+1, -10, 0x8F454B652D4D288EuL, 0xFC543953C296E37CuL)),
                    ((+1, -8, 0x8B7B4C5DD8A5E8F6uL, 0xA0DF918358C600ECuL), (+1, -16, 0xEE683FDF29F32556uL, 0x30C5F8E3705AF92AuL)),
                    ((+1, -14, 0xE85862224BADC212uL, 0x767C1B18EEA02521uL), (+1, -22, 0xCD478C6249595AC3uL, 0x0CF3473A1B7C6691uL)),
                    ((+1, -20, 0xD6068668BCE2C615uL, 0x1EB28C43F5643231uL), (+1, -29, 0x9EAB846DE5BBB907uL, 0x2F8A0108A88D0C28uL)),
                    ((+1, -27, 0xBE32D6DDEBEED39AuL, 0xDEDAB85E7210C27BuL), (+1, -38, 0x92219A1DC33ACC34uL, 0x315BF56EF70BADA6uL)),
                    ((+1, -36, 0xE50B63E37EC14EC4uL, 0x834DADA21372896EuL), (-1, -51, 0xB8F071475C688B26uL, 0x7A56F7F3AC527235uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX16Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xDF31B531FE9A32C4uL, 0x7B621FE88153788CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x812BE075C34BD39BuL, 0x07461D91EBBBADEAuL), (+1, -3, 0xC3B5CEA7672B1330uL, 0x861080C72693D02FuL)),
                    ((+1, -1, 0xF2911CDFC463BB3FuL, 0x63373DE0540ACE91uL), (+1, -7, 0xE5DB57EE58E05BE5uL, 0x7F1ED73E3011C890uL)),
                    ((+1, -5, 0xEC6FAD3E404A10EAuL, 0x4581BD45AD1E0CCCuL), (+1, -11, 0x81A7F11C8A223265uL, 0x64904CD1F11E65FEuL)),
                    ((+1, -10, 0xFDB3AD7DDBD4D303uL, 0xCE960E3C7276468BuL), (+1, -17, 0x8EE230B4AAD83103uL, 0xC8226A36E7152307uL)),
                    ((+1, -15, 0x91C349D401AA29BCuL, 0x8162EE49A4CC4C3CuL), (+1, -24, 0x88C61E31E9A0DCAEuL, 0x655A4F2F9C74D493uL)),
                    ((+1, -22, 0x9EC1945A56BB2DD3uL, 0x29AC5AFFA0AD26E1uL), (+1, -33, 0x98F0FDFAE4EAE58AuL, 0x35CD0270AC546D26uL)),
                    ((+1, -31, 0xE8966B2D9F3E0B8BuL, 0x58E1AE0D85BF4314uL), (-1, -46, 0xF0FF3563210EBB7EuL, 0xE8BF524C202A8A2DuL)),
                });
            }

            public static class Digamma {
                public const int Threshold = 16;

                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;

                static Digamma() {
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

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeZeroPointTable
                 = new(new (ddouble c, ddouble d)[] {
                    (Zero, (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xF7B95E4771C55D8FuL, 0x0C28D0814E530F17uL), (+1, 1, 0x9071B2D69DC0C3C5uL, 0x9F1956AD31ECC033uL)),
                    ((+1, 0, 0xDEE01F29692F76D6uL, 0x6B7DC54DA815009AuL), (+1, 1, 0x87C79215E6AABFBBuL, 0x8C07BF62FB1CFBFBuL)),
                    ((+1, 0, 0xA7F589A732A26FDAuL, 0xAADFD0D5B7829EF2uL), (+1, 0, 0x8CF11214C0E98B1EuL, 0x94C19DF4EF8B1B2BuL)),
                    ((+1, -1, 0x8BAF06C88EE26684uL, 0x6F0A367E1E932CB3uL), (+1, -2, 0xB3C32DA20CB8DDBFuL, 0xDD495E41E74E8972uL)),
                    ((+1, -3, 0x8DC74E46C61290F3uL, 0x320A4CAF7E215551uL), (+1, -4, 0x93BE92DF360B81C9uL, 0x00C84BF104C9D3FEuL)),
                    ((+1, -6, 0xB6EE02E8CF8F1F96uL, 0x473F50C25741A45BuL), (+1, -7, 0x9F20EC93C4F9AD6EuL, 0x8E4CF2E00D938B99uL)),
                    ((+1, -9, 0x9729B0D37891FE91uL, 0xD14B7195D53E2011uL), (+1, -11, 0xDF3B07180A8882EDuL, 0x2F32D598CE21F557uL)),
                    ((+1, -13, 0x9CF468E9085D6538uL, 0x488AB57A2B1AA35EuL), (+1, -15, 0xC61E1DCD49F325EFuL, 0x899DE438F0B34636uL)),
                    ((+1, -18, 0xC2C1C50C1B406BF0uL, 0x8260B09D78069CB5uL), (+1, -20, 0xD1B4816959A7FF60uL, 0x8D2223206E8568A1uL)),
                    ((+1, -23, 0x82E5CF3CBAC816CDuL, 0x63919B5C3D7EB911uL), (+1, -26, 0xED39CE5979999283uL, 0x616C1B47DA13DA38uL)),
                    ((+1, -30, 0x9CA1528643DB65EBuL, 0x038619D2C7B7AD9BuL), (+1, -33, 0xE69171965561AB2DuL, 0xC4CADA872E2E49D5uL)),
                    ((+1, -39, 0xCCCA6732A71884A6uL, 0x6EE973CF1D2C64E4uL), (+1, -42, 0xDC1E6AB7FDC1E294uL, 0x6064187B62AADFB5uL)),
                });

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, -7, 0xE2A2276E4ED2636EuL, 0x7C98B59F3B96D073uL), (+1, 1, 0xB7EB5362DBD152F5uL, 0xAA4528106A5F3EDAuL)),
                    ((+1, 0, 0xCABD88B932696C61uL, 0x6BDE16D253413F52uL), (+1, 1, 0xD73A77EF927B4377uL, 0xCB996AA0D15C1359uL)),
                    ((+1, 0, 0xF578972D1742AB97uL, 0xA0EE9F59CDDB6D42uL), (+1, 1, 0x89B5B875E3225FACuL, 0x6EB63945E426DA96uL)),
                    ((+1, 0, 0x8A8A2CDCD842ED4DuL, 0xB74F6DFE798A49DCuL), (+1, -1, 0xD871900CB04F595CuL, 0xC22B0D9C6466F884uL)),
                    ((+1, -2, 0xB6C7E4493A0C2E94uL, 0x6877153BB1078909uL), (+1, -3, 0xDCF976B17303ECF3uL, 0xA4210B96FF062295uL)),
                    ((+1, -4, 0x986719F27474EAF4uL, 0xC9E3C71FD241FC47uL), (+1, -5, 0x9657C2284570C3CFuL, 0x3D89484DE10F3853uL)),
                    ((+1, -7, 0xA54D2F4A9DED543DuL, 0xB3956AD044E678BEuL), (+1, -8, 0x8905E3CA20B87574uL, 0x9F07A0D78D431DA0uL)),
                    ((+1, -11, 0xE9547DFE800E3AD9uL, 0xC632318D81DE2122uL), (+1, -12, 0xA52128DF137E68E6uL, 0xD6B8D80B3D9ADEF2uL)),
                    ((+1, -15, 0xD13E094F13B785C1uL, 0x9B08C74AD416068FuL), (+1, -17, 0xFE7B9A4DEF1CB259uL, 0xBA8DF7F256951DEDuL)),
                    ((+1, -20, 0xE200B4CFB9F03A31uL, 0xB570B8C190047C1DuL), (+1, -22, 0xEBA0C5CDF31506C8uL, 0x13BC17BAD207DEA7uL)),
                    ((+1, -25, 0x84EACEAC32D4F943uL, 0xE1B4D60185E9149BuL), (+1, -28, 0xEA5D5EBF987A4CBCuL, 0xB3E2F7382A317709uL)),
                    ((+1, -32, 0x8B9950DE85700503uL, 0xB136D10DE12D6165uL), (+1, -35, 0xC8EF6ABE7B40E03CuL, 0xC7ADC48C15DFB2F0uL)),
                    ((+1, -41, 0xA04E90FAE2AF920CuL, 0xDA3B6CF77ABC4C9CuL), (+1, -44, 0xA97E72839B9525ADuL, 0x2497114B291087CAuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, -2, 0xD8773039049E70B6uL, 0x5C8380FDFD5A6952uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xB81C7AD4255FF706uL, 0x4BC2674E2E2DC8C7uL), (+1, 0, 0xF0375F6F3C709682uL, 0x9355B63E8CFFC98AuL)),
                    ((+1, 0, 0xD1F0201652CB3F9DuL, 0xA60CDDD7550E53A3uL), (+1, 0, 0xBF4C2AC29853EDD7uL, 0xD23C01552197C0E4uL)),
                    ((+1, -1, 0xF2E2FA959FF757C0uL, 0x296C7E47FC177B2FuL), (+1, -1, 0xAAA045B500505535uL, 0x897F292233572583uL)),
                    ((+1, -2, 0xA5A257450B327118uL, 0x651462F9A0E2B109uL), (+1, -3, 0xBD102371B6F6D49BuL, 0x6FDF42027A692577uL)),
                    ((+1, -4, 0x8E669CF62B0B415DuL, 0x01F2240865FFB99FuL), (+1, -5, 0x8829E3832A0AA2A4uL, 0xD57CC2A5910F098EuL)),
                    ((+1, -7, 0x9E9E0F3C5AC4C50FuL, 0x50257F6E2C5CCC0BuL), (+1, -8, 0x816306FE5692039BuL, 0x54BBBBC7EE823B0CuL)),
                    ((+1, -11, 0xE51E129E0E6FE461uL, 0xC61E47C0EB0E126DuL), (+1, -12, 0xA0FE92F90AF4A43CuL, 0x588FFF119786C1E3uL)),
                    ((+1, -15, 0xD1AE784A263D91EAuL, 0x1401FEDC8DF628F5uL), (+1, -17, 0xFE87B782D405620DuL, 0x6DF0C9768EF9A102uL)),
                    ((+1, -20, 0xE6AC7C3DF5A77404uL, 0xBA59D619DF6EE463uL), (+1, -22, 0xF0BC39BF55F5586BuL, 0x1A2F19FF68FFE55CuL)),
                    ((+1, -25, 0x8A016632B396E9E9uL, 0x83BE9E1E876FC338uL), (+1, -28, 0xF3E81868B3AC7056uL, 0x0F58617D6E6A2D96uL)),
                    ((+1, -32, 0x935AD77C66AA92EAuL, 0x36265685C44FF733uL), (+1, -35, 0xD4A82EA237D46563uL, 0x97A417B9C33C6C24uL)),
                    ((+1, -41, 0xAC09BEF3863F1496uL, 0xD6558AA81557A03DuL), (+1, -44, 0xB6480A6CEADD2D42uL, 0xEFCB3E07D2E33CF4uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, -1, 0xEC3B981C824F385BuL, 0x2E41C07EFEAD34A9uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xD5E83FCD44111A39uL, 0x41714D3E695C0371uL), (+1, 0, 0xB10655C923808F1CuL, 0xCDE85623DC910B1CuL)),
                    ((+1, 0, 0x9C3E2AF48B430F0DuL, 0x735ED7F48B4FB70EuL), (+1, -1, 0xD07BFB249EF6E77AuL, 0x44A1EA29F557BE86uL)),
                    ((+1, -2, 0xF6D735D14B70BAE1uL, 0x3308A360C1CF88C4uL), (+1, -2, 0x892BFA667D6FB6F8uL, 0x8B92EFDA86288924uL)),
                    ((+1, -4, 0xEB3D404BA64937DCuL, 0x49249B86786377DFuL), (+1, -5, 0xDE40E8317F432BBAuL, 0xE28BB3B61B9265FBuL)),
                    ((+1, -6, 0x8D8619FD346D4C1BuL, 0xB464F3E9D7C2F810uL), (+1, -8, 0xE626AE9722E0ECA5uL, 0xF1956250F8119AFBuL)),
                    ((+1, -10, 0xD93E6FB5056B1BF2uL, 0x83A59FA08FE53190uL), (+1, -11, 0x98F5E25C6EB6CFABuL, 0x66E0E536BE4DF3FDuL)),
                    ((+1, -14, 0xD105A7FF879FCE56uL, 0xCF1E838CB1AC8726uL), (+1, -16, 0xFEFB290C540BD2C2uL, 0x51190924FB8D34FDuL)),
                    ((+1, -19, 0xF00A460FA4DA1B1CuL, 0x16C49872B2DE5A11uL), (+1, -21, 0xFC14ADEF889CDBD4uL, 0x2379B06F0D87AD1AuL)),
                    ((+1, -24, 0x953B703C4CAD29B8uL, 0xB1267D101877074DuL), (+1, -26, 0x84BB0C0366923F73uL, 0xA5A1331B9B009FBCuL)),
                    ((+1, -31, 0xA528FEF9DF9A9A12uL, 0x4595A6F14E994EEFuL), (+1, -34, 0xEFC0C336DC933856uL, 0x5ABE7237606B9DFFuL)),
                    ((+1, -40, 0xC7B7BA2251F92718uL, 0xFDF740DE7EFF7062uL), (+1, -43, 0xD48D595E152E62DDuL, 0xD5C4EB23009AF338uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xA0C876B8EBD246D8uL, 0x41CB8AEA2A0144FFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xCEB7949D38FC26FAuL, 0x707636B7E5140F24uL), (+1, 0, 0x87A576FEBB5188ABuL, 0x775E6126F8FE2999uL)),
                    ((+1, -1, 0xDB4903FB0ADBECA3uL, 0xFE5C7AA7A01D93D6uL), (+1, -2, 0xF2DC809BDBE6AAE9uL, 0x389BF968629493B1uL)),
                    ((+1, -3, 0xFCA84F27299B4486uL, 0xD13FDC1B83AEA002uL), (+1, -4, 0xEFBCEF54C765659CuL, 0x71E6D6625CFF5FDDuL)),
                    ((+1, -5, 0xAE1411BF1B60DE5CuL, 0xAA753BFE4A8A15A7uL), (+1, -6, 0x8ECD9660A07668BEuL, 0xDC6945B18DD61AEAuL)),
                    ((+1, -8, 0x94355F5CBB2391DEuL, 0xF5C8E3D159DF2F51uL), (+1, -10, 0xD310A3FD73D81DC4uL, 0x43F451B606809E1DuL)),
                    ((+1, -12, 0x9B096C44C7383305uL, 0x20B04BF5DDDC6055uL), (+1, -14, 0xBF712A36E99CBC24uL, 0xDC07A7EDC5D4E7F8uL)),
                    ((+1, -17, 0xBF139AEE214157F4uL, 0xF77A3C1C484647BAuL), (+1, -19, 0xCB18657F390B0F7BuL, 0x65F5FA984ACA3A7DuL)),
                    ((+1, -23, 0xFCE29FFAD937CB5BuL, 0x721E05E112F724E2uL), (+1, -25, 0xE3716F8D2363E974uL, 0x0D866BB5E5785670uL)),
                    ((+1, -29, 0x9440CD7963892C77uL, 0x719C59FC17AD022DuL), (+1, -32, 0xD9467A0F20C8E8AAuL, 0xAC3E12F2110E11C1uL)),
                    ((+1, -38, 0xBD9E4BED82F013D6uL, 0xD0BF6CC37F1B9AE0uL), (+1, -41, 0xCB3EE5F4A8ECF8FCuL, 0xCD9DFA14B56D3988uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xC0C876B8EBD246D8uL, 0x41CB8AEA2A0144FFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xC7FDFBAA02EF7C4AuL, 0xE35114577F4B6C72uL), (+1, -1, 0xE3F43A2BF1375377uL, 0xC83AC8D2282ADAACuL)),
                    ((+1, -1, 0xADC5646F602B1240uL, 0x2E4410905E39A9F5uL), (+1, -2, 0xAC0D1CF874890C7CuL, 0x3C2810FFBD52BAECuL)),
                    ((+1, -3, 0xA5BB2E9A4B41619FuL, 0xD97016B461D41551uL), (+1, -4, 0x8FA0A99685351ECEuL, 0xA9EFDEC81795F5B9uL)),
                    ((+1, -6, 0xBE8133B0A0D809B8uL, 0x4314024DA821B895uL), (+1, -7, 0x9115551E0A15E9A7uL, 0xAF2984CFD1936B45uL)),
                    ((+1, -9, 0x8816A65CD5664A86uL, 0x83F4F6B481AB9494uL), (+1, -11, 0xB63C7C9EB6DB971AuL, 0x92B0362B288C8CA8uL)),
                    ((+1, -14, 0xEFF3ED6C3B0B4E45uL, 0x4956E38784B3FDFFuL), (+1, -15, 0x8CBEAA6F39C77197uL, 0x0F813878B694DCDEuL)),
                    ((+1, -19, 0xFA11ECC4F01A4AFDuL, 0xDA739CC9F6FDD2A7uL), (+1, -21, 0xFEADC23236B06520uL, 0x08DC520F47425622uL)),
                    ((+1, -24, 0x8C43E3BDBE1B4131uL, 0x473EBDF2181F04C0uL), (+1, -27, 0xF38C34B537BEDAE4uL, 0x8B730A517F3482E5uL)),
                    ((+1, -31, 0x8B992863FAE2C76AuL, 0x9F9743D8FEEE79B0uL), (+1, -34, 0xC6DCFCD962063ADFuL, 0x655B4572768F4CFAuL)),
                    ((+1, -40, 0x978A63D19C13AA37uL, 0x1452F6EDFDF64D31uL), (+1, -43, 0x9F1AAA7228624EBBuL, 0xA74FC075239164DCuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xDA621052856BE071uL, 0xDB652483C39ADE98uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xB16B78A9CCAB0EA2uL, 0xC1B2A626F92B2697uL), (+1, -1, 0xB4C6073399D0A16FuL, 0x74FE444EB6068ED1uL)),
                    ((+1, -2, 0xEEB254638876C9DEuL, 0x4E6C08D907AC6027uL), (+1, -3, 0xD4CDB82DC3FD6B58uL, 0x9B512A59BDB61EC6uL)),
                    ((+1, -4, 0xACFFAF47B3160B12uL, 0xC1C57B3BAE346207uL), (+1, -5, 0x8764EC9E7CB40067uL, 0x924935A78C196AFFuL)),
                    ((+1, -7, 0x92F114666B40E016uL, 0x45CDDCCD854A2386uL), (+1, -9, 0xC9E51B983AFC8D8FuL, 0x7AC3B7652FD14F1AuL)),
                    ((+1, -11, 0x94C0E78432B91EA2uL, 0xE1526595A156B004uL), (+1, -13, 0xB2B22055C2E8ED8DuL, 0xC2F5EDAE6FC1542DuL)),
                    ((+1, -16, 0xAE2C7E7FDFCA5F8BuL, 0x31371284177C935FuL), (+1, -18, 0xB539479F4027E142uL, 0xF4BC4C9652F149F6uL)),
                    ((+1, -22, 0xD88DA1EBA61E74B7uL, 0x37162085C40326FCuL), (+1, -24, 0xBF8FA87FACB54BFCuL, 0xC3722744B1DBE126uL)),
                    ((+1, -29, 0xED02E22F307D663BuL, 0x4F0A634239F06598uL), (+1, -31, 0xAB78BC51A2E6B3C4uL, 0x631346D100D9F908uL)),
                    ((+1, -37, 0x8D1073FDCFACDE3EuL, 0xACF83B754429C43EuL), (+1, -40, 0x95D39B41F8F56D65uL, 0x53792B7088BA2D19uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xEFB765A7DAC135C7uL, 0x30BA79D918F033EEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xA78AF55BB4204B8FuL, 0x1F863AC74FEE48E1uL), (+1, -1, 0x9DEF5706040F3785uL, 0x746C38A1DED6EC4CuL)),
                    ((+1, -2, 0xC2BD598974244900uL, 0x3B64A086D5424911uL), (+1, -3, 0xA29B2EE30362D141uL, 0xE089F901A312DB44uL)),
                    ((+1, -5, 0xF4AD50F2BED36928uL, 0xAE5E442037A78FA3uL), (+1, -6, 0xB52562C525770624uL, 0xA720BED62B5A49D7uL)),
                    ((+1, -8, 0xB49ACA1734B714F1uL, 0x9B158737196851E8uL), (+1, -10, 0xECAFDCA0FECC92BFuL, 0xA0A17157AD1BD034uL)),
                    ((+1, -12, 0x9F35491A133B9EA3uL, 0x9DCF1309C6E7C09FuL), (+1, -14, 0xB7B409FC680C936DuL, 0x07DC9883CC899D92uL)),
                    ((+1, -17, 0xA294E1659C5DD581uL, 0x5C22570E4285318BuL), (+1, -19, 0xA37A51A3D92BB64EuL, 0x868C5E3F6BBF847AuL)),
                    ((+1, -23, 0xB07E13217A3C7698uL, 0x91A72674F2D9A372uL), (+1, -25, 0x97B78C177893F1BEuL, 0x90B000778798EAF7uL)),
                    ((+1, -30, 0xA8C15BAC2FEC32F2uL, 0x22D86152366B1C4CuL), (+1, -33, 0xEE93C10BB6073858uL, 0xE54F09B56171A8B4uL)),
                    ((+1, -39, 0xAF6AC48CCBA9DD4AuL, 0xD82441DBCB8A7E77uL), (+1, -42, 0xB72BEA6070D51E64uL, 0x584E69989E38CF86uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x8100451D11F2E408uL, 0x2AA6617ED59CAC40uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0x9E2EBB05AD0D26C9uL, 0x0B076D0540DD36B4uL), (+1, -1, 0x8C0BB5C20AF5C432uL, 0x7AE5ECE38456F408uL)),
                    ((+1, -2, 0xA1B4306949306C11uL, 0xEA7EFEA04446C6EAuL), (+1, -4, 0xFFE33902A70B956EuL, 0x43ED1CF3929AAB9CuL)),
                    ((+1, -5, 0xB30EAC9ACB6D60E3uL, 0xAED8DF19AF88A97EuL), (+1, -7, 0xFD1B4F8EB11E75CFuL, 0x522C68BDB5D1B4DAuL)),
                    ((+1, -9, 0xE95A4EA4BA903CBFuL, 0xFACA80C227E066C7uL), (+1, -10, 0x92E6DA6D8118EFCDuL, 0x3F1DF45AD08FB418uL)),
                    ((+1, -13, 0xB5D600026AD35804uL, 0xDD2E3B4560EC0871uL), (+1, -15, 0xCAAFDB25D9A62E7FuL, 0x95A25D382C2C3544uL)),
                    ((+1, -18, 0xA44EFD2AC4FE97EEuL, 0x21FB9CAFC7454960uL), (+1, -20, 0xA06490140993922EuL, 0x4481DF2D0607A24FuL)),
                    ((+1, -24, 0x9DF171B4BB23FE55uL, 0x78AB043C7E66AC45uL), (+1, -26, 0x846A964AAFA6B294uL, 0x93ADEC6AC84E44C1uL)),
                    ((+1, -31, 0x85C5117CD7FE397AuL, 0xF00E0C57EE67ED54uL), (+1, -34, 0xB94A8C3E723E91EAuL, 0xD6011A455004E173uL)),
                    ((+1, -41, 0xF6353A797ED58EC3uL, 0x1EACAD4194D92243uL), (+1, -44, 0xFD3F0B1F68391DF1uL, 0xEF33881A9055B770uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x8900451D11F2E408uL, 0x2AA6617ED59CAC40uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0x8834CFC577C7F861uL, 0xCE20FABA16A7718CuL), (+1, -2, 0xE26887535014A552uL, 0xF3A4FCCF98976E4EuL)),
                    ((+1, -3, 0xDCF061F33CBC47E0uL, 0x45E232CCA985EF63uL), (+1, -4, 0xA34E54D9DD6729FBuL, 0xB2D39B6793AF14CEuL)),
                    ((+1, -6, 0xBC58EA270FF52A4AuL, 0xDD5C94E733F4D812uL), (+1, -8, 0xF6F0E24605BD565BuL, 0x99DD998C66550A39uL)),
                    ((+1, -10, 0xB4FB5D4129617186uL, 0xD0AADBC019A2E1B2uL), (+1, -12, 0xD130B63FBA839946uL, 0xC26BF32D5D147081uL)),
                    ((+1, -15, 0xC2CEFCF8940FCB3BuL, 0x7C7CF641ED1BC313uL), (+1, -17, 0xC45CA5D53E27A4AAuL, 0x7A20AD84F8B742D5uL)),
                    ((+1, -21, 0xDA79807397A011BDuL, 0x68466422693162E9uL), (+1, -23, 0xBC4EF5402E49D882uL, 0xAA6F13E78E193FEFuL)),
                    ((+1, -28, 0xD55C70D3A942D656uL, 0x3F4B6C4EBB81490BuL), (+1, -30, 0x97328B528EE2AE36uL, 0x60F97BBD4605DCF2uL)),
                    ((+1, -37, 0xE16A1F29E8B0585BuL, 0xADAE07A757152862uL), (+1, -40, 0xEBD6BC53ACAD7E97uL, 0x907B87D45A554E97uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x901CB6E42E64AB24uL, 0x9C6D7DF09CB91E07uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0x80BCF3EB317F2E5FuL, 0x116739FF3894FCE6uL), (+1, -2, 0xCCC6EBE954C359DEuL, 0x2029293898C29ABCuL)),
                    ((+1, -3, 0xBBDD76C0CE02182AuL, 0x4A49E45B42275734uL), (+1, -4, 0x85A1625B04D97E31uL, 0xF02FC71F03B4A8C5uL)),
                    ((+1, -6, 0x9038485BC509DFA3uL, 0x04709AEE4224420BuL), (+1, -8, 0xB6DD045EA16343EAuL, 0xA17CFD5EC46BCE7FuL)),
                    ((+1, -11, 0xF9C891EB771043DEuL, 0x6CFB3E5AC22BC6D6uL), (+1, -12, 0x8C387A6EB844CD94uL, 0xC86F47618D5E774FuL)),
                    ((+1, -16, 0xF2745B4AAC10AD9DuL, 0xE7C82056F0FF491DuL), (+1, -18, 0xEE563D5D87A579B8uL, 0x141235E5E7DC38E0uL)),
                    ((+1, -22, 0xF54BE79F20322178uL, 0x22A14577ECD132B3uL), (+1, -24, 0xCEFA66D61617D659uL, 0x2692B08CA1A11DBFuL)),
                    ((+1, -29, 0xD822B3A71996B567uL, 0x43AB8C070711F592uL), (+1, -31, 0x96856186E7799342uL, 0xE2FFBC9A11A3F0E2uL)),
                    ((+1, -38, 0xCDE8B36F46AA2598uL, 0x34153EDAA556F8F2uL), (+1, -41, 0xD4AEF3C83D73165CuL, 0x42EBDAFE82567633uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x96831D4A94CB118BuL, 0x02D3E457031F846DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xF408DF1DC5817B3DuL, 0x2AE97ADB91B76669uL), (+1, -2, 0xBAD0CD3A2634B9A2uL, 0xA919CD3123638E87uL)),
                    ((+1, -3, 0xA1BA3C473B8F552EuL, 0xF853AC705B9CE158uL), (+1, -5, 0xDE7A9E76AE7CF59EuL, 0x108179EEBDE65C28uL)),
                    ((+1, -7, 0xE1B25E3C965BC281uL, 0x4C8618ADE659B0F5uL), (+1, -8, 0x8AED3E0B4064470BuL, 0x67DEB2E786785433uL)),
                    ((+1, -11, 0xB1C06C4FC4A7831AuL, 0xB9FB8934F29C577FuL), (+1, -13, 0xC27BE9E43EAE0A68uL, 0x8A5B4935B1929EDAuL)),
                    ((+1, -16, 0x9CFB252FF3820651uL, 0x5E19DB119C516AA0uL), (+1, -18, 0x96E63F03F5B0D9DFuL, 0x9C9A9E71B94FFE7DuL)),
                    ((+1, -22, 0x908ACF52E1D22C6CuL, 0x844D070090E99ACDuL), (+1, -25, 0xEF50A041A03D90ECuL, 0x1A4527A2D2A61BFCuL)),
                    ((+1, -30, 0xE7D3AD919A038B12uL, 0x0E7C87C715AA6D97uL), (+1, -32, 0x9EEF6AF8328FFBCCuL, 0x135E1C9C16AFFDF2uL)),
                    ((+1, -39, 0xC8E6437B7B5B403CuL, 0x5261DF517717EE3AuL), (+1, -42, 0xCD1C1E753135B1DEuL, 0xDDBBAAD2190099C0uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX12Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x9C5491A7AC10E2FFuL, 0x5FEB2A28777C9BB3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xE7EC6EB8B8AB46D0uL, 0x94D971BF427C232BuL), (+1, -2, 0xABADC9B3681FFC04uL, 0xF1B863EC0F295476uL)),
                    ((+1, -3, 0x8CBA4B61875E2BE4uL, 0x2EBA0A47CCF0857AuL), (+1, -5, 0xBBEA596B98F2A6B3uL, 0x1CEB5585B59296B1uL)),
                    ((+1, -7, 0xB3E8C22CD7C285D7uL, 0xC9BD5A6686556AC0uL), (+1, -9, 0xD7BC3E6F5BFFD8EBuL, 0x34382C420D1D4237uL)),
                    ((+1, -11, 0x81DACE45F74236A1uL, 0xD0EC70C37321E342uL), (+1, -13, 0x8AD42A561EF789B0uL, 0x19D6ED5DEE129D7EuL)),
                    ((+1, -17, 0xD244C560DB1BE8E5uL, 0x5CFE0C9D4CEA707DuL), (+1, -19, 0xC61661B419F8B726uL, 0x77E5607D2F7FD412uL)),
                    ((+1, -23, 0xB1862F7B5E2594E6uL, 0x8525434196557E63uL), (+1, -25, 0x9071F03046F7D9A6uL, 0x86AAF4C59FB42070uL)),
                    ((+1, -30, 0x8288F41772F3545AuL, 0x4A67FB0669CE7865uL), (+1, -33, 0xB07339EC9365FB11uL, 0xED410793002D5A3CuL)),
                    ((+1, -40, 0xCF5546841B171E1BuL, 0xB5ABC9913E2D7D6BuL), (+1, -43, 0xD171B2524CEE15CDuL, 0xFB1F7FB22730AE68uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX13Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xA1A9E6FD01663854uL, 0xB5407F7DCCD1F108uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xDCFB796B698BC063uL, 0xC440AFD51D6B3E48uL), (+1, -2, 0x9EC281A462F1DF3DuL, 0xD2439BB288F1B63BuL)),
                    ((+1, -4, 0xF7379E17C9E0F067uL, 0xC6CAC5E139E2C68BuL), (+1, -5, 0xA0B760B670E387F1uL, 0x33103A7D6F43FAACuL)),
                    ((+1, -7, 0x91BB50B15836DC13uL, 0x2B5B0DDBD93E3F2EuL), (+1, -9, 0xAAA9CAADD32F84C5uL, 0x7BC888D9371376B3uL)),
                    ((+1, -12, 0xC2120130C49C9C28uL, 0x85E799C23975FA3DuL), (+1, -14, 0xCB2FA21CCA1547BDuL, 0x8F576477905F8819uL)),
                    ((+1, -17, 0x90FBCF8D5A311CBEuL, 0x5ED56F485D4B76EFuL), (+1, -19, 0x861B2CDFC678EBFEuL, 0xC28A0B2E8FF9E19EuL)),
                    ((+1, -24, 0xE1EC6142329D753DuL, 0x38C8C84CA09098AEuL), (+1, -26, 0xB4F47AAC16CF4EC2uL, 0xC0ECBDEF05CFC5AFuL)),
                    ((+1, -31, 0x994C931E881AA992uL, 0x083A05901637DE9CuL), (+1, -34, 0xCC8970FA4D70FB5FuL, 0x5E9E6F506595F202uL)),
                    ((+1, -41, 0xE0938ABA86DE383FuL, 0xACD59D7AF26D2E9DuL), (+1, -44, 0xE0A9DE64A02F234DuL, 0xB75B1E1F081AD47EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX14Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xA69635C1EDB4FD41uL, 0x04056BCC91BE3FCDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xD310A8A2548F0AF7uL, 0x6D46BDF0D8CC603EuL), (+1, -2, 0x939CA18C5A562654uL, 0x3CF59ED7D1B924E8uL)),
                    ((+1, -4, 0xDAF1BE0A542BC221uL, 0x097472733B87A2ECuL), (+1, -5, 0x8AF3549B3FAA1AC2uL, 0x95B629AD6C73C7EBuL)),
                    ((+1, -8, 0xEF6CFADF7807C1C6uL, 0x8457AC9CFB990305uL), (+1, -9, 0x8936A13058DA4E92uL, 0x81AFD5B18E955DF6uL)),
                    ((+1, -12, 0x93E7E4ABA24BDCF8uL, 0x1769FAB2C4266369uL), (+1, -14, 0x97EDE8B11AA9ED76uL, 0xCEF734DB5ECDF8DBuL)),
                    ((+1, -18, 0xCD10A488847EB1BDuL, 0xFC5BD5045FAEF33BuL), (+1, -20, 0xBA87DF07C64F43F8uL, 0x78D088B564BACC48uL)),
                    ((+1, -24, 0x94458267DB5D1409uL, 0x76181C1C4D5E092DuL), (+1, -27, 0xEA1D255D9E206646uL, 0xC5A7145ECC8086ECuL)),
                    ((+1, -32, 0xBAB840BD25C70506uL, 0xBC43BF8B1C7795E3uL), (+1, -35, 0xF6281E4806BDC2DFuL, 0x9D4E96F8A70EA5D3uL)),
                    ((+1, -42, 0xFDB32BCD9F45860FuL, 0x17821B28D2CE1A87uL), (+1, -45, 0xFB86A674F69607A8uL, 0xAB66F497A07A0E01uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX15Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xAB287EE67FFE21D3uL, 0x4D29FE15B65088F2uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xCA0B2D92F587F693uL, 0x0C20EBE2413913EDuL), (+1, -2, 0x89E642E450C33EBFuL, 0x5209D295839C5D8CuL)),
                    ((+1, -4, 0xC353FD5A60FC2B0AuL, 0xC738470FFD43CC38uL), (+1, -6, 0xF28D1E0FE35AB165uL, 0xE430C688DD4C3063uL)),
                    ((+1, -8, 0xC71E49E9BBB2FAA7uL, 0x05A28892953BF6C4uL), (+1, -10, 0xDFCA4B28C8F99F6CuL, 0x360B46C2C6673456uL)),
                    ((+1, -13, 0xE5606FD2980BDD90uL, 0x61876F0F59BEDC16uL), (+1, -15, 0xE788C2B47D26D742uL, 0x356224366A702362uL)),
                    ((+1, -18, 0x94473FE1230491BBuL, 0xB5E9EE2568BFE476uL), (+1, -20, 0x84D0CFCA959FD61DuL, 0x3B19BAC473432A66uL)),
                    ((+1, -25, 0xC7F6939943B4EB9DuL, 0x8F991405D0C9776AuL), (+1, -27, 0x9BC72C67150F824FuL, 0x54D6DAD411A950F3uL)),
                    ((+1, -33, 0xEAD01FE19337CE1DuL, 0xDC8FD19B0B141689uL), (+1, -35, 0x99124D481BF42B2BuL, 0x4641FAAE32CC80C6uL)),
                    ((+1, -42, 0x94AE0DB1B918049AuL, 0x19F404FEA80ADC2FuL), (+1, -45, 0x922E55D9491E34CDuL, 0xFF86D630785789F4uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX16Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xAF6CC32AC4426617uL, 0x916E4259FA94CD36uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xAC5E183FD300CB9EuL, 0xB043DD7EE0359477uL), (+1, -3, 0xE371CF4ADBF2C47EuL, 0xA0A6E07044E7ABA8uL)),
                    ((+1, -4, 0x8699BBB2F8C110ACuL, 0x14A035E224900B73uL), (+1, -6, 0x9FD3451CFC4050F7uL, 0x7CCF3AB891583A7CuL)),
                    ((+1, -9, 0xD464DBF53CFC4EB4uL, 0x300A0BA2D70CDC13uL), (+1, -11, 0xE13B5CEEC4357B37uL, 0xF03ECA57BDF4F7F2uL)),
                    ((+1, -14, 0xB18E3567011E54B4uL, 0xFA98E96B6844A96BuL), (+1, -16, 0xA625B70CDC402899uL, 0x7BEC8D9431AEDF71uL)),
                    ((+1, -20, 0x95E65E40CB113F7DuL, 0xFDE4293535DBA698uL), (+1, -23, 0xF297415CF5D40FBEuL, 0x2CC4809CD1A6D0A1uL)),
                    ((+1, -28, 0xD88E18079A73C8B1uL, 0x6E67B87E1189CB47uL), (+1, -30, 0x91B7146C903D08A3uL, 0x6E5E4B15684B935CuL)),
                    ((+1, -37, 0xA79702D41AB1D022uL, 0x21C82B67D057DF90uL), (+1, -40, 0xA8BE95226E1174FEuL, 0x3FCC2A662D60916AuL)),
                });
            }
        }
    }
}
