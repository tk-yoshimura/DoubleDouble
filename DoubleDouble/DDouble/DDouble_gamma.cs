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
                int n = Math.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

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
                ddouble p = Pow(x * RcpE, x);
                ddouble s = Exp(SterlingTerm(x));

                ddouble y = r * p * s;

                return y;
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
                int n = Math.Max(0, (int)Round(x - 2d));
                ddouble v = x - n - 2d;

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
                ddouble p = (x - 0.5d) * Log(x);
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
                int n = Math.Max(0, (int)Round(x - 1d));
                ddouble v = x - n - 1d;

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
                ddouble p = Log(x);
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
                public const double Threshold = 36.25;

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
                        PadeX16Table,
                        PadeX17Table,
                        PadeX18Table,
                        PadeX19Table,
                        PadeX20Table,
                        PadeX21Table,
                        PadeX22Table,
                        PadeX23Table,
                        PadeX24Table,
                        PadeX25Table,
                        PadeX26Table,
                        PadeX27Table,
                        PadeX28Table,
                        PadeX29Table,
                        PadeX30Table,
                        PadeX31Table,
                        PadeX32Table,
                        PadeX33Table,
                        PadeX34Table,
                        PadeX35Table,
                        PadeX36Table,
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
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX17Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 44, 0x983BBBAC00000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 44, 0xC11EE6F9FFD275DEuL, 0x121A7B93156668D4uL), (-1, 0, 0xC47893A112CCDC82uL, 0xC36B2B375614BE58uL)),
                    ((+1, 43, 0xEE50497F1D6D6544uL, 0x05C1549D2BE99068uL), (+1, 0, 0x9019AC4819E6DE20uL, 0x913E485AB0551742uL)),
                    ((+1, 42, 0xBD809D1397627A3AuL, 0xF4D5B5E720C032D1uL), (-1, -1, 0x85EF8AABD279469BuL, 0x5F438FFA355E1E47uL)),
                    ((+1, 40, 0xD8DDE782CCD78A42uL, 0x58F7EFFC9B75C3ABuL), (+1, -3, 0xB04BD750D7B8D269uL, 0x3848F7D7C90F35FCuL)),
                    ((+1, 38, 0xBCCF50B0F4E9C7AFuL, 0x39F04516FE8D1C6CuL), (-1, -5, 0xADD26EB591CEE309uL, 0xC1FC2E27042EEB96uL)),
                    ((+1, 36, 0x80D83DACBB2172A7uL, 0x7312E84B0202EA71uL), (+1, -7, 0x84542CE63406A4D7uL, 0xEE4A1105E48E0A74uL)),
                    ((+1, 33, 0x8BCB380641055F40uL, 0x56BC5BCCFA187BE3uL), (-1, -10, 0x9DD93E74A9FB1DA0uL, 0xF6DA2276DACA6A78uL)),
                    ((+1, 29, 0xF1A2BD93DB086239uL, 0xBBD6E2C171F5F398uL), (+1, -13, 0x93DC89FEE06725D6uL, 0x08FA08CC9B0F9358uL)),
                    ((+1, 26, 0xA4AE4C4000E66D29uL, 0x2915F51FB692C67BuL), (-1, -17, 0xD7692001247A44A0uL, 0xBA417B4D44582054uL)),
                    ((+1, 22, 0xAC84140CCE24F8CFuL, 0xC14FB507FC5C62BEuL), (+1, -21, 0xEDE49F505014C33FuL, 0x091F34BA72CB1FD4uL)),
                    ((+1, 18, 0x83F06963FD16BF3EuL, 0x5219212FCDDDF936uL), (-1, -25, 0xBD37B43CE40B956AuL, 0x474A3EEE9707760CuL)),
                    ((+1, 13, 0x8487ACA07F3147C0uL, 0x63D6A2B20E206618uL), (+1, -30, 0xC307F52EF5F61129uL, 0x0ADA91A6BCEA5523uL)),
                    ((+1, 7, 0x84997911E26F4C43uL, 0x3CB7BD29B29A7835uL), (-1, -36, 0xC5944868812F7C9AuL, 0x93BA74F64875BD92uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX18Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 48, 0xA1BF7766C0000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 48, 0xD20A8454BAFC3134uL, 0xD8CFBD0889FCCE59uL), (-1, 0, 0xC8297E75F998DBD6uL, 0x639FC012786580E6uL)),
                    ((+1, 48, 0x84E3B5FB3981608CuL, 0xE431D68C97AF4E68uL), (+1, 0, 0x961672D30B5AA3C1uL, 0x08456E1F7C3F539EuL)),
                    ((+1, 46, 0xD94D343B8FCA1444uL, 0xACF80CDA5FA53ABBuL), (-1, -1, 0x8F3BCE0DD1CD6379uL, 0xC7F9046E9ECB7E84uL)),
                    ((+1, 45, 0x8059A9832434B72AuL, 0x4E747F3760BB51DAuL), (+1, -3, 0xC2A0DDBB567711D5uL, 0xB8576ED31BE3476EuL)),
                    ((+1, 42, 0xE7F922774C8CF0C4uL, 0xEA6958B2F162E2F8uL), (-1, -5, 0xC777B4E9AA470E8BuL, 0x252650F019B5966FuL)),
                    ((+1, 40, 0xA591F093F9545CB1uL, 0x95E3CD1E8180F326uL), (+1, -7, 0x9F432FF34E1E454BuL, 0x2811D63DF53C2749uL)),
                    ((+1, 37, 0xBDE86B9A8415AD6CuL, 0x69CB773AEE24D836uL), (-1, -10, 0xC9A2F21C8E7C2D09uL, 0x9AAC6B2F1D23640EuL)),
                    ((+1, 34, 0xB02A37B2EF0D1F02uL, 0x4DA5D3E434816D9EuL), (+1, -13, 0xCBC89EF7473F5B8FuL, 0x6E25D14482786B59uL)),
                    ((+1, 31, 0x83C4F0E3D8E0DFBAuL, 0x8A87731BA010EBB5uL), (-1, -16, 0xA3F4C5DAE454D445uL, 0xC7937722C6F22236uL)),
                    ((+1, 27, 0x9CCD7A831127AD22uL, 0xF4AD736C5751477CuL), (+1, -20, 0xCF3BFD3503EA5222uL, 0xBE7ED213BD664245uL)),
                    ((+1, 23, 0x9047F1E8FA544DDAuL, 0x3BF5734693227EA7uL), (-1, -24, 0xC80A74F9FF118305uL, 0xA5D7C233DFC20D3AuL)),
                    ((+1, 18, 0xC2A07DE60AC04A99uL, 0xABC91A7C55D6574AuL), (+1, -28, 0x8BD045769B07F66BuL, 0xF5F77ADA63C2D2F8uL)),
                    ((+1, 13, 0xACCF2DA63F1B10B3uL, 0xB296F1A7C5C5A2A8uL), (-1, -34, 0xFE2B19C016A3D342uL, 0xF66000BB8AA1BF25uL)),
                    ((+1, 7, 0x98F461DFDC43F063uL, 0xDD955F9C372F3217uL), (+1, -40, 0xE387959E130AD4E1uL, 0x4CECB9BFFEC4AF72uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX19Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 52, 0xB5F7665398000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 52, 0xF2EF77E2F33E9694uL, 0x6D725B4B7629B7C2uL), (-1, 0, 0xCA9A5E6A04A70C34uL, 0xBC66379B77EE2695uL)),
                    ((+1, 52, 0x9DC9B2FDEAE97230uL, 0x5121972C35BD207DuL), (+1, 0, 0x99CE2088A94139FBuL, 0x817FA4F86A5674D7uL)),
                    ((+1, 51, 0x84447AE7D9DF305AuL, 0x605622BD60B19BBCuL), (-1, -1, 0x94A3BD34FC0BD185uL, 0x236B9DA3EC4B395DuL)),
                    ((+1, 49, 0xA0039E8904AF3871uL, 0xCA12A7EEC852B785uL), (+1, -3, 0xCC94C81BA3DBFB94uL, 0x4AEC7E4FD68C8DA0uL)),
                    ((+1, 47, 0x93EF057D6AA3776AuL, 0x7D0FC00BBE5C452BuL), (-1, -5, 0xD46DAD2B2B9D777FuL, 0xBD543948402AE572uL)),
                    ((+1, 44, 0xD7D7DE0EE29F8977uL, 0x7B4AD332CD33EFC9uL), (+1, -7, 0xABE3CD1806F0E4EDuL, 0x129F2A5A948EF053uL)),
                    ((+1, 41, 0xFCD533A53EEBCE2FuL, 0xA54EFF22BC04FD48uL), (-1, -10, 0xDC9BF879B907D22DuL, 0x544A941BF623ABE5uL)),
                    ((+1, 38, 0xEF568AF3769059D5uL, 0x1446F407834A9F9EuL), (+1, -13, 0xE21556C65D778AC2uL, 0x761BE3B819E8BA44uL)),
                    ((+1, 35, 0xB68F2A9C384E9E49uL, 0xB3A025C8C118E564uL), (-1, -16, 0xB880E458A190F8E9uL, 0x8040B746D2371975uL)),
                    ((+1, 31, 0xDD6402B0E730F1FCuL, 0x8604108807ED0586uL), (+1, -20, 0xEC9F595F4336507FuL, 0x6B4D57D4EE7863DFuL)),
                    ((+1, 27, 0xCF7A4A9588F3E29EuL, 0x7AE161BD36942A6AuL), (-1, -24, 0xE7D583A2CDD29393uL, 0x99093EC8E9CAC3E6uL)),
                    ((+1, 23, 0x8E722266C9008FE6uL, 0xA6C9EA4E62128014uL), (+1, -28, 0xA485E65FC3A3CB97uL, 0x176D0D824820E672uL)),
                    ((+1, 18, 0x80AD801EF4A6454EuL, 0x8BB129AA988C496CuL), (-1, -33, 0x97E5CAF53E630337uL, 0x836842FFB4D599D5uL)),
                    ((+1, 11, 0xE7A3101FCEEC4B20uL, 0xAA916D4A2EB8D75DuL), (+1, -39, 0x8A2C2D4C9C8ADD8CuL, 0xD648086E81A610E8uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX20Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 56, 0xD815C98344800000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 57, 0x93ED04A1BDFFD273uL, 0xA886B448D3C9708AuL), (-1, 0, 0xCCFA0202C7C268BDuL, 0x3F3E0C31823C8B2EuL)),
                    ((+1, 56, 0xC4D40E5AE81C2A93uL, 0x4CBDB564CEAC5E76uL), (+1, 0, 0x9D767B2707296B64uL, 0xDF230095E1DCF7ECuL)),
                    ((+1, 55, 0xA8D3209466841F98uL, 0x5E8F746D3E93B47DuL), (-1, -1, 0x9A05C12B664D8465uL, 0x93F92F9D8467C855uL)),
                    ((+1, 53, 0xD0C87C7174A5B754uL, 0x33038F10EE02F7C8uL), (+1, -3, 0xD69CDBCA0445BB36uL, 0x6FC93709EC567FF1uL)),
                    ((+1, 51, 0xC524D32475E94233uL, 0xA790DB3833CDC44FuL), (-1, -5, 0xE1A79D9D7626EC2CuL, 0xEC119FE50FF08C73uL)),
                    ((+1, 49, 0x92C746DBAB432468uL, 0x545F9DF892532819uL), (+1, -7, 0xB8F094DEF64DE922uL, 0x692EAEFD2BA33C6CuL)),
                    ((+1, 46, 0xAF572C1B449CF037uL, 0x9028D9546A79B12AuL), (-1, -10, 0xF0789B5E8392DF4BuL, 0x33DEE54FE37CD54CuL)),
                    ((+1, 43, 0xA9290204A8730D3CuL, 0xF396300551C3387EuL), (+1, -13, 0xF9BCA6B9342D056FuL, 0x012465A8EB5CEBC3uL)),
                    ((+1, 40, 0x836B941FD7EC2F84uL, 0x6FFBF81D87AC8839uL), (-1, -16, 0xCE97BA4BD9AE2EABuL, 0xC10D69283604AB38uL)),
                    ((+1, 36, 0xA23C0DADEF923DA0uL, 0x8F27AFFEDA61CFACuL), (+1, -19, 0x86536A28EFC82D22uL, 0x123097A5E32343D0uL)),
                    ((+1, 32, 0x9AAFB86D4FA94B70uL, 0x5A68D186184C0309uL), (-1, -23, 0x857C897CF80A2FC7uL, 0xF8EAE91D76E9EFE9uL)),
                    ((+1, 27, 0xD7FEA7C4E2297F73uL, 0x125D21265EAC4056uL), (+1, -28, 0xC039F9C390936920uL, 0xDDC32F4D6C6F8AB0uL)),
                    ((+1, 22, 0xC6536A9E1E4F5FB4uL, 0x0339075A0E4C6706uL), (-1, -33, 0xB420BEE149142303uL, 0xA84C911BFF1D77EEuL)),
                    ((+1, 16, 0xB55CE36E0017D59AuL, 0xA68D703F93750B83uL), (+1, -39, 0xA65C505BC6B20238uL, 0x342F0C199F888949uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX21Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 61, 0x870D9DF20AD00000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 61, 0xBD3985DCC21AD8CDuL, 0xC39851D69E978337uL), (-1, 0, 0xCF48E12C434AA275uL, 0x975AF5892EBA4AD9uL)),
                    ((+1, 61, 0x80B3D7B646A9564AuL, 0x34C2B5FEAB27B546uL), (+1, 0, 0xA10F522699693A26uL, 0xA04D8999AA727B8AuL)),
                    ((+1, 59, 0xE183D0E0611723B2uL, 0x30528619FFF9BEBBuL), (-1, -1, 0x9F609224096A42DFuL, 0xF309EBE91353DBECuL)),
                    ((+1, 58, 0x8E51CBDA19DEA56EuL, 0x77D405B97D10D12BuL), (+1, -3, 0xE0B582E4B7B1298AuL, 0x2F1D6844B4AE247AuL)),
                    ((+1, 56, 0x890DE2A968D03954uL, 0x29C33FCF1C0D97C3uL), (-1, -5, 0xEF2024C34E031A61uL, 0x130D5C3A902C09E7uL)),
                    ((+1, 53, 0xCFFE2D58CA9F376BuL, 0x90E00B599A4D92A6uL), (+1, -7, 0xC6648A6F70635D60uL, 0x3F1A1FE0B7089AE4uL)),
                    ((+1, 50, 0xFD12991DF540A592uL, 0x419D01EC8B5FBA99uL), (-1, -9, 0x8299A7381DC2D1FDuL, 0x7F6356B59B62FD3FuL)),
                    ((+1, 47, 0xF888F5CDC91EA4DCuL, 0xFF3BBB262885EEC1uL), (+1, -12, 0x895E2BA4C0E7E99CuL, 0x7E7A067E66AEC589uL)),
                    ((+1, 44, 0xC47275ABF5E7783FuL, 0xA90ED01462EEFF99uL), (-1, -16, 0xE63D3F0B46766B0BuL, 0xC5C0751D0AB75D39uL)),
                    ((+1, 40, 0xF69B37E4188D9DFDuL, 0x284E1A034D55CCB5uL), (+1, -19, 0x97B1F49A0623F499uL, 0x7F9FBA0B7ADCE0C3uL)),
                    ((+1, 36, 0xEEFE8FF689C3A239uL, 0x2357B9A4AF68DD72uL), (-1, -23, 0x98CC5382FC0B9CB1uL, 0x7C7BBC4AC5EF03A4uL)),
                    ((+1, 32, 0xA986629C53786A5BuL, 0x8B53F64F146703F1uL), (+1, -28, 0xDF18497CDF4D3233uL, 0x365EF2125D491812uL)),
                    ((+1, 27, 0x9E146786AA6D5E25uL, 0x347D7A23D1B88D69uL), (-1, -33, 0xD40645F5917CC946uL, 0xBBE489CF5BF9EC02uL)),
                    ((+1, 21, 0x92C07496D88F127BuL, 0xA41E9D092AE214CBuL), (+1, -39, 0xC6AA8BD03BD695B7uL, 0x4B536DB42A9B08BCuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX22Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 65, 0xB141DF4DAE310000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 65, 0xFDB09B9823111D59uL, 0xBF39CA5E5D891E2CuL), (-1, 0, 0xD1877F026A48AEAFuL, 0xD1CB5304EA74804BuL)),
                    ((+1, 65, 0xB01BD1CBA731392EuL, 0x3293830FE1C70ACAuL), (+1, 0, 0xA49895F544BA2A3BuL, 0x5BD1E24FC4491E5AuL)),
                    ((+1, 64, 0x9D5AFF5D4C81681EuL, 0xF0C5614A2519A6CFuL), (-1, -1, 0xA4B32178221C420BuL, 0x5DCA56942910D767uL)),
                    ((+1, 62, 0xCA6A9B795B33B1F6uL, 0x9EDA69BF600DB9BFuL), (+1, -3, 0xEADB8AD85130D302uL, 0xBC21A2A383FE291EuL)),
                    ((+1, 60, 0xC689CD8EAD2F7112uL, 0x2C8DD7506B26C9B1uL), (-1, -5, 0xFCD2417066F5FB4AuL, 0x4DEB3D093B610551uL)),
                    ((+1, 58, 0x9959EC1D2E46AC7CuL, 0xA93020D911AC8AFFuL), (+1, -7, 0xD43ADB61D22139E9uL, 0x87CC81778E482D25uL)),
                    ((+1, 55, 0xBDD49DAA069F2F87uL, 0x22157FA00797015CuL), (-1, -9, 0x8D633275DEF5FA0AuL, 0x234772119BE42AC1uL)),
                    ((+1, 52, 0xBD9222ABEA8EDFC6uL, 0xE5C0B62F523E5A60uL), (+1, -12, 0x9688C8E2D0EFF82AuL, 0x4BB771CB69C2A400uL)),
                    ((+1, 49, 0x984BC42EC0DF687BuL, 0x2416C695E3356C83uL), (-1, -16, 0xFF74702BB69B2B5AuL, 0xE9510A31FFEAF8FFuL)),
                    ((+1, 45, 0xC23AA7AA78A69202uL, 0x3762776AF1D960E0uL), (+1, -19, 0xAA7320FC44C278EEuL, 0x0687E2A3900AFC6AuL)),
                    ((+1, 41, 0xBF27AA46F7C68122uL, 0x04FF1757B2448BB3uL), (-1, -23, 0xADEAD2FA2FAB2B67uL, 0xD81BCBDE29F75E78uL)),
                    ((+1, 37, 0x89A445FB3D439244uL, 0x0D76782C586F9966uL), (+1, -27, 0x80A59D1E4AF5857EuL, 0xC17BECAC4D5DF458uL)),
                    ((+1, 32, 0x823DFD50975598D4uL, 0x6429F755E074C97AuL), (-1, -33, 0xF7D59D3468C4A606uL, 0x39A45B1772B776E9uL)),
                    ((+1, 25, 0xF54CB9A370D93D99uL, 0x06988F36072E51B4uL), (+1, -39, 0xEB6E0FAA9A36CBF2uL, 0xD3F5F7BA8B2096C3uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX23Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 69, 0xF3BA930ACF836000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 70, 0xB1DF5D799A6A2A29uL, 0xB114B61A5B4E0C16uL), (-1, 0, 0xD3B665E057080601uL, 0x3A25E1107B8903A1uL)),
                    ((+1, 69, 0xFBADE6561A5C32A5uL, 0x7898DBE65244A95CuL), (+1, 0, 0xA81251DE33515C59uL, 0x9F338CD7CF159474uL)),
                    ((+1, 68, 0xE509038275C8F740uL, 0x983E7D3C470181CDuL), (-1, -1, 0xA9FC9279A8FDE74AuL, 0x1250BA2810E75E1AuL)),
                    ((+1, 67, 0x95F219532380A638uL, 0x0C945831074F0937uL), (+1, -3, 0xF50C1C863CFFA51EuL, 0x679E307CCC33E55EuL)),
                    ((+1, 65, 0x959F0A3E75BDC45DuL, 0xD812F7DA2A0842EEuL), (-1, -4, 0x855CA887FFA8A065uL, 0x69F64BFCEE6D8D14uL)),
                    ((+1, 62, 0xEB05614438EFE394uL, 0xE32E03B234C2574DuL), (+1, -7, 0xE26EE66D6435F92FuL, 0x79944AA8DABC495FuL)),
                    ((+1, 60, 0x93D6E2FDF6363773uL, 0x12ACC2C11617F2A2uL), (-1, -9, 0x9896132C2D383C9AuL, 0x1137BD6102216759uL)),
                    ((+1, 57, 0x95FB4CE02123E448uL, 0x0C3DDCAF38082370uL), (+1, -12, 0xA45C806157C0B683uL, 0x8BCD35116BDBD6C2uL)),
                    ((+1, 53, 0xF4B51221B68A2D6FuL, 0x018513D895CAB55DuL), (-1, -15, 0x8D1FB889EF302E48uL, 0x0F4D39CB0880BD92uL)),
                    ((+1, 50, 0x9E648890EA1C6ECAuL, 0x816B0E218C3E27E3uL), (+1, -19, 0xBE9DEE656EF31451uL, 0x53627D2AFDA4CE54uL)),
                    ((+1, 46, 0x9E2CE120EECB6BC7uL, 0xE9BA2F3C0139E35EuL), (-1, -23, 0xC4E7B61CD7A84ABBuL, 0x7F68D3AF2E5A4605uL)),
                    ((+1, 41, 0xE70DF205E36B057AuL, 0x25C1223B9B30FCE4uL), (+1, -27, 0x937DE126D7489370uL, 0x310630433EF083DAuL)),
                    ((+1, 36, 0xDDB1B029803A6844uL, 0x6C150CF8404DF7BAuL), (-1, -32, 0x8FE69BDB790EDB7BuL, 0x965BACC1AC20E132uL)),
                    ((+1, 30, 0xD3A0A742102C88B3uL, 0xF0B2773931A13B72uL), (+1, -38, 0x8A7F4BE5E44E0D38uL, 0x0D790BB4437310CFuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX24Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 74, 0xAF2E19AFC5266D00uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 75, 0x8233607CE62E4C13uL, 0x1331387F6FBBE4FEuL), (-1, 0, 0xD5D624295E5B1932uL, 0x08A30D2B6FA0221CuL)),
                    ((+1, 74, 0xBB822C6CD85B8131uL, 0xB7103FEB3EDAFD12uL), (+1, 0, 0xAB7CA6B4A0BEF54AuL, 0x692C74D40191649BuL)),
                    ((+1, 73, 0xAD952B3FE3789F27uL, 0x42F725300C270D8AuL), (-1, -1, 0xAF3C33B9A177C860uL, 0xBE3807C9AE0F467BuL)),
                    ((+1, 71, 0xE7168D810EAEF58AuL, 0xD4FA37D2616A5B38uL), (+1, -3, 0xFF44B40176370E2BuL, 0x480CC928742FE858uL)),
                    ((+1, 69, 0xEA5626DCAC24D92EuL, 0xB9BB5A0D9C663E22uL), (-1, -4, 0x8C6885FD0D884229uL, 0x5F33F78E009C65B2uL)),
                    ((+1, 67, 0xBAF41D7FB3D7A077uL, 0x80433E995B53AB61uL), (+1, -7, 0xF0FC3F3F9332ED76uL, 0xFEABFC1D22B09F32uL)),
                    ((+1, 64, 0xEED2CAC3AD1BC8AEuL, 0x73E0413764557826uL), (-1, -9, 0xA42F6F32F38443A8uL, 0x3D2EE785161DA54AuL)),
                    ((+1, 61, 0xF5EA2C6D40A5D076uL, 0xBFEE0ED492867E05uL), (+1, -12, 0xB2D775AC274BAE45uL, 0xA7CFD820D39B60D9uL)),
                    ((+1, 58, 0xCB8BD880F83F1B62uL, 0xB3D361F39143A53EuL), (-1, -15, 0x9B4FD39BB12C254EuL, 0x5B720260D5FD54ABuL)),
                    ((+1, 55, 0x85A0F9600D877ACEuL, 0xA63DA2D2E213FAEBuL), (+1, -19, 0xD43898020E6236E4uL, 0xE9E49644D7C8FD9AuL)),
                    ((+1, 51, 0x874DB924565556BDuL, 0x1BB9403CA786FAC4uL), (-1, -23, 0xDDD1B1C46636EB35uL, 0xCFB0F7374271CE48uL)),
                    ((+1, 46, 0xC8559745C70E1433uL, 0x82B6ECB73A9135DBuL), (+1, -27, 0xA828B30B42765EBDuL, 0xAB45201F6488ABEBuL)),
                    ((+1, 41, 0xC2C6D286E9924817uL, 0x830A9C5BBD90A50CuL), (-1, -32, 0xA6154EDD6383D4EDuL, 0x016E257F0F24B367uL)),
                    ((+1, 35, 0xBC5A9478B75BB857uL, 0xEE2B3838B9A24A5AuL), (+1, -38, 0xA1DA14C09DB6177CuL, 0x2508C25737E9F51BuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX25Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 79, 0x83629343D3DCD1C0uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 79, 0xC6A75D9629B33483uL, 0x8A85AC5DCB6A4326uL), (-1, 0, 0xD7E749B921E6F369uL, 0x21D3927C5D774864uL)),
                    ((+1, 79, 0x916D517A06EA18F8uL, 0xAF2D11798CE4AA12uL), (+1, 0, 0xAED7C640F22417EAuL, 0x6086EAEB673E516CuL)),
                    ((+1, 78, 0x88CD826D1A88F80EuL, 0x22C02A1CE28E7A60uL), (-1, -1, 0xB47178E67FAD47B3uL, 0xE7013269D9A6A383uL)),
                    ((+1, 76, 0xB8FC4730B54CA7DAuL, 0xB432F4A5956BB2A8uL), (+1, -2, 0x84C18C2BB1286F1DuL, 0x08DF87EA508D5B46uL)),
                    ((+1, 74, 0xBE7452E56F0C4115uL, 0x8BA6CA18BE44F316uL), (-1, -4, 0x938AC04A255B2A67uL, 0x1D52DDD995BDA510uL)),
                    ((+1, 72, 0x9A352D770AF2B275uL, 0x32C7AD0793D26B0DuL), (+1, -7, 0xFFDEB034A84E62B5uL, 0x21BCE384375AF0E6uL)),
                    ((+1, 69, 0xC7DA658D705ED460uL, 0x0D519F287F8EFE3FuL), (-1, -9, 0xB02C746C90012F81uL, 0x445E71E0C4C07126uL)),
                    ((+1, 66, 0xD0B41362D23941F0uL, 0x232932AD27CCB9F5uL), (+1, -12, 0xC1F7A5E3371188EDuL, 0xA718A4A0DF9DEE46uL)),
                    ((+1, 63, 0xAF22F12636537274uL, 0xBD7F1288EA92B0FAuL), (-1, -15, 0xAA4AE98703DD13FCuL, 0x88EB3FB9EAACB189uL)),
                    ((+1, 59, 0xE910A427CB08F0E0uL, 0xEE6269B7F6C9DE1BuL), (+1, -19, 0xEB48A602CCE0D0F1uL, 0x590BA61D2A5DB6A8uL)),
                    ((+1, 55, 0xEF1B502F4C0B03D4uL, 0x9D54BB08B3D39371uL), (-1, -23, 0xF8B68D9F5BB6894AuL, 0x01AE702136C3C122uL)),
                    ((+1, 51, 0xB34DF7F94B6CD2A0uL, 0x04B4905023A7673FuL), (+1, -27, 0xBEB91C762A520C8AuL, 0x765FDB77AFD44B39uL)),
                    ((+1, 46, 0xB089F6B13460E239uL, 0xCB8C643A1051566CuL), (-1, -32, 0xBE952971F5ED845BuL, 0x1E471F94CD704F5CuL)),
                    ((+1, 40, 0xACD61B1D56A07D30uL, 0xE22AD2A38842A786uL), (+1, -38, 0xBBF36CED0BBF5101uL, 0xDDE70CD78C30908FuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX26Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 83, 0xCD4A0619FB0907BCuL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 84, 0x9DB0C336641D73FDuL, 0xC3BC2B4073EE66FBuL), (-1, 0, 0xD9EA65E45EA74548uL, 0x99C592F38F5C971AuL)),
                    ((+1, 83, 0xEA7B7BD56ADC39B7uL, 0x82D92135B834A088uL), (+1, 0, 0xB223EF6477976B9DuL, 0x670B3FA54675D135uL)),
                    ((+1, 82, 0xDFED4D0FFFD77F1CuL, 0x4397B071ED3DC265uL), (-1, -1, 0xB99BF551B4DFA3ECuL, 0x1F6443E38B012D7FuL)),
                    ((+1, 81, 0x99A3236203AB8150uL, 0xD914A77AAD8115EBuL), (+1, -2, 0x89E2A9D889074439uL, 0xA7D3754C311612D6uL)),
                    ((+1, 79, 0xA075E950BABE7B34uL, 0x0D36C827B67613DBuL), (-1, -4, 0x9AC186E50014C3AFuL, 0xC91B1893E6A54920uL)),
                    ((+1, 77, 0x83C01B07DEEE1EA5uL, 0xD3679D34128B7341uL), (+1, -6, 0x87891D439756FE95uL, 0xF7EC58D594CCA147uL)),
                    ((+1, 74, 0xAD183590E856C819uL, 0x92D6170BA1E7C852uL), (-1, -9, 0xBC8A5D27C39CFE98uL, 0x865D194C1014E3DCuL)),
                    ((+1, 71, 0xB72FDA75B439F3BDuL, 0xEC17D0C7927C959DuL), (+1, -12, 0xD1BAF114FA59FA3DuL, 0xDDF041EA6F84051EuL)),
                    ((+1, 68, 0x9BBE065BBD436809uL, 0x200C5A3E5C9D8FEAuL), (-1, -15, 0xBA1112FF503BD635uL, 0x42176DC86DA9D7C1uL)),
                    ((+1, 64, 0xD1EB27882D441170uL, 0xF2BB0CEED5E77146uL), (+1, -18, 0x81E97EAA9ECFFCFBuL, 0xA2458492D2697ECBuL)),
                    ((+1, 60, 0xDA11B1E3117C13A1uL, 0x28FFFBAF4FAB5ADEuL), (-1, -22, 0x8AD19852A863C057uL, 0x2C98C55DA6FBA244uL)),
                    ((+1, 56, 0xA58B2D8EC1E8C749uL, 0x9F3DDD2BC32647E3uL), (+1, -27, 0xD741708DED3A7765uL, 0x97C4F3CE21D0D276uL)),
                    ((+1, 51, 0xA4F57626F7A2591FuL, 0x9F72390277084DE4uL), (-1, -32, 0xD983E13AE22A0042uL, 0x1BAD2926526D6521uL)),
                    ((+1, 45, 0xA36976794D016F1DuL, 0x7C09071C359A2C4BuL), (+1, -38, 0xD8F74DE0CEA30ECFuL, 0xB65BC2B85C5DAF11uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX27Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 88, 0xA6CC24F51BF75648uL, 0xC000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 89, 0x820DF0E6A5A5DF3DuL, 0x13D208F6AAB1B5DBuL), (-1, 0, 0xDBE005F3C5499BBCuL, 0x37EE151DC08D2F00uL)),
                    ((+1, 88, 0xC43A11D8DE355665uL, 0xD3BD1E8EB6847FE0uL), (+1, 0, 0xB5616AE24723D2AAuL, 0x8BC248AF2D0E5A6AuL)),
                    ((+1, 87, 0xBE1334E2AA68DE29uL, 0x30246D57A475B4F7uL), (-1, -1, 0xBEBB571D46EAD286uL, 0x06B24F73EEAB84D0uL)),
                    ((+1, 86, 0x843B66CAB2DE08F5uL, 0x935A88FC53F80959uL), (+1, -2, 0x8F04D5FCF97D494FuL, 0xABF88D5282DBD4F3uL)),
                    ((+1, 84, 0x8BFCCB5FDE97FCA7uL, 0x7E92102369DE0F58uL), (-1, -4, 0xA20B2EA02651BE7EuL, 0x3A6191B73298D753uL)),
                    ((+1, 81, 0xE8F178C08B8DC4B8uL, 0x730B2ECE60331278uL), (+1, -6, 0x8F498AB5804E3AB5uL, 0x03E5AA0A68C16895uL)),
                    ((+1, 79, 0x9B03B9C476765A22uL, 0x4FFB9A5C0929DC2DuL), (-1, -9, 0xC9467342D0FC06F0uL, 0xA4395E705B569438uL)),
                    ((+1, 76, 0xA624D28A3DB2E0A2uL, 0xA695DF1D22540C21uL), (+1, -12, 0xE21F21EE0F8D538BuL, 0x8081F629D2BB1BC9uL)),
                    ((+1, 73, 0x8F03EE3C81B5165BuL, 0x666015B9431E69A5uL), (-1, -15, 0xCAA22AC2FA137BEEuL, 0xA3CBD22B9512EB62uL)),
                    ((+1, 69, 0xC31F03E2816D91C3uL, 0x871D59F5EFBA8152uL), (+1, -18, 0x8EEDF70DDE558038uL, 0x145268B5C233E6F7uL)),
                    ((+1, 65, 0xCD204A51177FB678uL, 0xE06C4EF8A5A89282uL), (-1, -22, 0x9A51D6BA68083BAAuL, 0x8876A504F8051A6CuL)),
                    ((+1, 61, 0x9D8C53345065A6DAuL, 0xCA0F73633E422A7DuL), (+1, -27, 0xF1D34F250E0718A4uL, 0x990D5EEA8B08ABC1uL)),
                    ((+1, 56, 0x9ECD3A8A13FE0270uL, 0x8880310BDCF66864uL), (-1, -32, 0xF6FE983C2E801D08uL, 0x0FDBF36E8C97B3C8uL)),
                    ((+1, 50, 0x9F17CC8F006F46BAuL, 0x44EB136D4B3EB8ADuL), (+1, -38, 0xF9118A7815A5933EuL, 0x4939AA948083A3F2uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX28Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 93, 0x8CBC3F2ECF98B0CDuL, 0x6200000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 93, 0xDE949A9ACD12832BuL, 0x4B6C793284F7CB42uL), (-1, 0, 0xDDC8B4037FA92E12uL, 0x21EEF96BAB185AFEuL)),
                    ((+1, 93, 0xAA3D56BE98627B2BuL, 0x8DB569116E3CCFA0uL), (+1, 0, 0xB89088B93398F54BuL, 0x80C78AC611220881uL)),
                    ((+1, 92, 0xA721A3DB96E58A65uL, 0x839BA5F9764B4D47uL), (-1, -1, 0xC3CF63098FE40F03uL, 0xCBD9EF0E353D4019uL)),
                    ((+1, 90, 0xEB9CCEF9FFD7E6E3uL, 0x39FD5400B6357FC6uL), (+1, -2, 0x94274E115F51BF28uL, 0x7558ED53E8E46195uL)),
                    ((+1, 88, 0xFCA72446B12B7862uL, 0x0630BD8656C7C676uL), (-1, -4, 0xA9662EC4C002F6F5uL, 0x1F6C07E62693165AuL)),
                    ((+1, 86, 0xD4DE1B54ECD25A90uL, 0xAE99E45A9D7A643EuL), (+1, -6, 0x972ED636B306FB2EuL, 0x296EB9D7BC56FE49uL)),
                    ((+1, 84, 0x8F686A1D6C905A4BuL, 0x19A44871BF7FB873uL), (-1, -9, 0xD65E125641B2BC36uL, 0xF18F14CFD44E0BDAuL)),
                    ((+1, 81, 0x9B912FB0D61D240DuL, 0x42751223AD9F9509uL), (+1, -12, 0xF321F404319EDAB4uL, 0xEBE3552628938183uL)),
                    ((+1, 78, 0x87809032A102306EuL, 0xE5A619B89FA65A8AuL), (-1, -15, 0xDBFDD5F390057EAFuL, 0xCAC10E57EC5A4E3CuL)),
                    ((+1, 74, 0xBB0665CDF536A9FEuL, 0xB2CAAAA31FCA3AF5uL), (+1, -18, 0x9CB3A07486B7A2F9uL, 0x5DE691BE011F4254uL)),
                    ((+1, 70, 0xC6DCE45C28B61F0EuL, 0x20E8F2B67DE92518uL), (-1, -22, 0xAAE1A731BFF3A8F5uL, 0x5C32630E6000A464uL)),
                    ((+1, 66, 0x9A73ACBA59832572uL, 0x7F2E310C5C4D84EEuL), (+1, -26, 0x873FD460DB8D4DD0uL, 0xFA2C0FB09092B603uL)),
                    ((+1, 61, 0x9D658B86C09808A9uL, 0x3A0963E878CA4DEFuL), (-1, -31, 0x8B90EBBB04DBD261uL, 0x271D3F66DDB6548DuL)),
                    ((+1, 55, 0x9F64940041FB51AFuL, 0x8E51001BE899CB8BuL), (+1, -37, 0x8E36DE370B0D9F77uL, 0xCDC86273E156FB26uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX29Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 97, 0xF6496E91EB4B3567uL, 0x6B80000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 98, 0xC55DB90975EA0FB8uL, 0x580D4D193CC5E5CDuL), (-1, 0, 0xDFA4F63447AAEF74uL, 0x36B2B4867418E31AuL)),
                    ((+1, 98, 0x98EE17DA41A27BEEuL, 0x001584908651D879uL), (+1, 0, 0xBBB19DFAB21B91BCuL, 0x28808F2EA495AD9CuL)),
                    ((+1, 97, 0x980F30F8D7BE7C90uL, 0xD4DB615B599827F9uL), (-1, -1, 0xC8D7F0D65E12F27AuL, 0x628B8D80B7B7DD8BuL)),
                    ((+1, 95, 0xD90CA200D046C5CEuL, 0x274A3113CBB383E0uL), (+1, -2, 0x99496704287C3E98uL, 0x1D7A76265DC04063uL)),
                    ((+1, 93, 0xEB99E4A9A8727BB1uL, 0x7AE5184A1B5A5949uL), (-1, -4, 0xB0D11E42C12D077DuL, 0x3D373116FAAF029CuL)),
                    ((+1, 91, 0xC8E2BA9DC137DFD3uL, 0x6962AE6951FB3F23uL), (+1, -6, 0x9F374EAE930A8E6BuL, 0xEF96927479883A90uL)),
                    ((+1, 89, 0x88EDED10CCEF002AuL, 0x4FFEC0CC3ED0A360uL), (-1, -9, 0xE3CEA9203B7514EAuL, 0x7B10E52659506B5CuL)),
                    ((+1, 86, 0x96413F122590DF0CuL, 0x95D0510B3FA2BAC5uL), (+1, -11, 0x82608C7C92DC7E20uL, 0x464FD1D2C483EA36uL)),
                    ((+1, 83, 0x845BB363DDB94E7EuL, 0x1DE6DF733DD11735uL), (-1, -15, 0xEE238B58AC9F4EC7uL, 0xB1218FDC49901C80uL)),
                    ((+1, 79, 0xB8B7C1BDBD0456FDuL, 0xAD8B6C76E1FD5B26uL), (+1, -18, 0xAB3C21672DCFF338uL, 0x3B33E33D08962371uL)),
                    ((+1, 75, 0xC68E311DA322DF23uL, 0xAE89B11AC313EAEEuL), (-1, -22, 0xBC86508ED279FA6FuL, 0xCB828B1192DA6BFEuL)),
                    ((+1, 71, 0x9BDE4F4DF00D09B9uL, 0xBDA4343DDBAB56A9uL), (+1, -26, 0x96AB619DB152E644uL, 0x995D5635D7E20D90uL)),
                    ((+1, 66, 0xA08418FE8B2A5C3DuL, 0xF0BFDFB46F567D81uL), (-1, -31, 0x9D04C594F31B6FE9uL, 0xF13AF4ADEDF04ED1uL)),
                    ((+1, 60, 0xA43CEF12D51E0F00uL, 0x29C86CB6A242FBEFuL), (+1, -37, 0xA19B9A3B1B8507BDuL, 0x99032ECBFAC3836AuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX30Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 102, 0xDF328C343D3C2865uL, 0xB96C000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 103, 0xB5213D272B0ADA19uL, 0x1F4932705359CF8AuL), (-1, 0, 0xE1754E1D8EF5926EuL, 0x6DF0E51C52582AF9uL)),
                    ((+1, 103, 0x8E1707D8B9955E71uL, 0x288A752F77A71E77uL), (+1, 0, 0xBEC5030BA52AD1ECuL, 0xECE17E1E358630ABuL)),
                    ((+1, 102, 0x8EFF2AFA23C21057uL, 0x9749DFF5924A6E7BuL), (-1, -1, 0xCDD4E8286F82E8F7uL, 0x1E7046A21AF6DC6DuL)),
                    ((+1, 100, 0xCE8AC76397CF6811uL, 0x3770B66D167C4C9BuL), (+1, -2, 0x9E6A8A7E21426557uL, 0x5B2186DC69B501A4uL)),
                    ((+1, 98, 0xE2CF97CC9B4C7417uL, 0x55125C6723397229uL), (-1, -4, 0xB84AB10542485009uL, 0xECA164ECFE1F0B53uL)),
                    ((+1, 96, 0xC39A6541E92DE87DuL, 0x9413BBF3C39098A7uL), (+1, -6, 0xA7615AF5CEA3B9F1uL, 0x2A90C91B8CE8B323uL)),
                    ((+1, 94, 0x86D34EAF58F0288DuL, 0xF5D8D24362425C4CuL), (-1, -9, 0xF195BA5DAAEE3B84uL, 0xD28846FE3DCEC70DuL)),
                    ((+1, 91, 0x95939E7010E4E160uL, 0xD86707EDF4D46273uL), (+1, -11, 0x8B7D1E54A15DD9D4uL, 0x6D23E8CE154AB8F8uL)),
                    ((+1, 88, 0x852FDC45B2CA159DuL, 0x2C859C4E9565874FuL), (-1, -14, 0x80894CD870FD3A7AuL, 0x81EE8BE1536E03F2uL)),
                    ((+1, 84, 0xBBD9E9D0137B6F48uL, 0x7458A4F35410C5B4uL), (+1, -18, 0xBA88E827C69C5CFCuL, 0x6FCB6F92041DA2E3uL)),
                    ((+1, 80, 0xCC08D341E5DD3BB3uL, 0x79A3E72986B4AC9DuL), (-1, -22, 0xCF44C0E0332CD692uL, 0xC675065923135F45uL)),
                    ((+1, 76, 0xA1D0E1E1DE04E824uL, 0xAF878F1625E1EE35uL), (+1, -26, 0xA7341F5E909A2D4FuL, 0x2C0D9A19736CD3D8uL)),
                    ((+1, 71, 0xA8536BFD9179D389uL, 0x22CE35B2E9415560uL), (-1, -31, 0xAFE8803F73B92CDBuL, 0x5F806E5EF7942324uL)),
                    ((+1, 65, 0xADF14CBFCCBDB995uL, 0x607D5532170F75EFuL), (+1, -37, 0xB6CC765109531425uL, 0x4564793936AD0DE3uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX31Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 107, 0xD13F6370F96865DFuL, 0x5DD5400000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 108, 0xABD9C01052E7DAB9uL, 0x6FAB352F92F0998FuL), (-1, 0, 0xE33A3872D375C2B2uL, 0xEC2BDDD8C0B91F27uL)),
                    ((+1, 108, 0x8866102793A77965uL, 0xDE695D343235274EuL), (+1, 0, 0xC1CB123DD76CE53DuL, 0xE4F2CF676720004FuL)),
                    ((+1, 107, 0x8ADAB624A5B3C187uL, 0x7E56D3BCFD1125CFuL), (-1, -1, 0xD2C63DE3BF9644FCuL, 0xE616C58B900D567FuL)),
                    ((+1, 105, 0xCAD4A560ACA5C366uL, 0xA967707645AC88B6uL), (+1, -2, 0xA38A3473981C69E3uL, 0xC5EB9E647F43A948uL)),
                    ((+1, 103, 0xE1360CCE391CCEA4uL, 0xB5DC820F6CA1E48CuL), (-1, -4, 0xBFD1B572B64AC54CuL, 0x9983A4D2A23206E9uL)),
                    ((+1, 101, 0xC457D5EFC824B93CuL, 0x2E6B6F25FF16E47CuL), (+1, -6, 0xAFAB789516FDD942uL, 0x84949F3DB5607AFBuL)),
                    ((+1, 99, 0x88C944F0B405EAD0uL, 0x8493BE1410524B5FuL), (-1, -9, 0xFFB0DD34E0680866uL, 0x2A187C11290E8DD8uL)),
                    ((+1, 96, 0x9959B2A8141497FEuL, 0xEF73279381BAC317uL), (+1, -11, 0x94E584486725BC1DuL, 0xF9BAE5122D4F3560uL)),
                    ((+1, 93, 0x89F6060FCFB6EDBDuL, 0xB7AA4A67386CFEF9uL), (-1, -14, 0x8A6516984B9F9C63uL, 0x510744C79F24716EuL)),
                    ((+1, 89, 0xC490E566308FEB3DuL, 0xE63FE1ECE3E5CBD5uL), (+1, -18, 0xCA9B2FAA0E217921uL, 0x4D699E2A12C1C93AuL)),
                    ((+1, 85, 0xD7A374BF0F536A2AuL, 0x913E6E333974E047uL), (-1, -22, 0xE32192B21254A6FCuL, 0x6A3B1296F7E7C4AFuL)),
                    ((+1, 81, 0xACB45D7CD85994F0uL, 0x37216A667B0F3E1DuL), (+1, -26, 0xB8E18D836B6F5C92uL, 0xD8F8FB882681541AuL)),
                    ((+1, 76, 0xB56547D7075E05C7uL, 0xE36CAA17067B3F87uL), (-1, -31, 0xC44972236EF477CAuL, 0x9BFF7DE1B43F4B42uL)),
                    ((+1, 70, 0xBD3D457287F07289uL, 0xF23872346451D6F4uL), (+1, -37, 0xCDDEBE65643ADF38uL, 0xF987B939B915FE6AuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX32Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 112, 0xCAB56855719D22B0uL, 0x62E6960000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 113, 0xA861FB9DA4BC31B5uL, 0xAAEE987A53DEE499uL), (-1, 0, 0xE4F42CD0A8D0C0C0uL, 0xB07123FBF6A58515uL)),
                    ((+1, 113, 0x8724F9DE52F37AE7uL, 0x41670C1DD069870BuL), (+1, 0, 0xC4C426B3098DC22DuL, 0xD723DBA53F80A755uL)),
                    ((+1, 112, 0x8B175559E7900AA1uL, 0xB702B66E67D0E0A9uL), (-1, -1, 0xD7ABF1EBA3A66BDEuL, 0x9753B9380AE5A9B2uL)),
                    ((+1, 110, 0xCD5F1450603968FEuL, 0x93055BE3A1C19BB3uL), (+1, -2, 0xA8A7F0FCAA443674uL, 0x837B3CC65CE691A8uL)),
                    ((+1, 108, 0xE673B58FAACAC3B6uL, 0x12941DDC60244115uL), (-1, -4, 0xC765121C79329BD6uL, 0x0FA74E871CA304F3uL)),
                    ((+1, 106, 0xCB01DC975F46D494uL, 0x62B42BB4F695ECB7uL), (+1, -6, 0xB8143A7E7CBB778FuL, 0xA6138715A02E69F0uL)),
                    ((+1, 104, 0x8EE0F828DA7872E7uL, 0xFC023DBA19C0E597uL), (-1, -8, 0x870EDEA6B251B975uL, 0xE814B7E4CB6383B4uL)),
                    ((+1, 101, 0xA1CBB07AECDF8F60uL, 0xED93E9FD7CBF55EEuL), (+1, -11, 0x9E98934605E24856uL, 0x8F8AD90DB4A817D8uL)),
                    ((+1, 98, 0x9300A4DD32F742D5uL, 0x6A9B1BA67DF961AAuL), (-1, -14, 0x94A4AA25883DD911uL, 0x49D9914DC6198EF1uL)),
                    ((+1, 94, 0xD37E217D55A4BC44uL, 0x8CCF2D2C65C55944uL), (+1, -18, 0xDB7404158C6CF076uL, 0x9A3A0B3B763FA7F1uL)),
                    ((+1, 90, 0xEA3E3584F684C8C5uL, 0x958073C183186F51uL), (-1, -22, 0xF8211209776C4B09uL, 0x94DC2AA93C04FB55uL)),
                    ((+1, 86, 0xBD6150EAE8546B50uL, 0x37AB4CCB3960064DuL), (+1, -26, 0xCBBADE68703F2AEDuL, 0xD4DA1081FFD0D971uL)),
                    ((+1, 81, 0xC8C3B5F0C97161B5uL, 0x084866AD06EA6578uL), (-1, -31, 0xDA34A157F33C7AC3uL, 0xA24764AA062CADAFuL)),
                    ((+1, 75, 0xD35E90F87BD2B9E9uL, 0xF3F8E88A6BF1CA43uL), (+1, -37, 0xE6E7885EA92B78D3uL, 0xF0B4C5182B69A18FuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX33Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 117, 0xCAB56855719D22B0uL, 0x62E6960000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 118, 0xAA37307A5581BF45uL, 0x0BEB10D63A806C88uL), (-1, 0, 0xE6A39DA82381C44FuL, 0x2DF3C39CC78390EBuL)),
                    ((+1, 118, 0x8A1362CCF5BB457CuL, 0x98D249AD569EE470uL), (+1, 0, 0xC7B09B7C939C6EACuL, 0x43ABB7CE04210E96uL)),
                    ((+1, 117, 0x8F99AA3A80214A56uL, 0x000E099DBDAD43EBuL), (-1, -1, 0xDC860D3ACEEABEB5uL, 0x179862676F198472uL)),
                    ((+1, 115, 0xD637A2BB93AC0ACEuL, 0x7373F06704AAC991uL), (+1, -2, 0xADC35A6E4DB84102uL, 0x0A3AE48F54069E42uL)),
                    ((+1, 113, 0xF2D1300C2127D0BDuL, 0xA5670AF3DBEA7F5AuL), (-1, -4, 0xCF03C39E5FFDF3FAuL, 0x1D5FC1753C553FE3uL)),
                    ((+1, 111, 0xD808CE1FBD7A197FuL, 0xEFBA15A9E5D34168uL), (+1, -6, 0xC09A47CA08AC678EuL, 0xFCC5B5564416F5CEuL)),
                    ((+1, 109, 0x998A4F8E1353B434uL, 0x4D0E807D9C328472uL), (-1, -8, 0x8E6D0D55460F538EuL, 0x7AA7DCCA0F949128uL)),
                    ((+1, 106, 0xAF8C82709F606B0CuL, 0x3F18FB35DBA6B675uL), (+1, -11, 0xA8952187C5F3DFB4uL, 0x76D61ECB28AC83B5uL)),
                    ((+1, 103, 0xA104044CC2079279uL, 0xC5A2A8A97701B02DuL), (-1, -14, 0x9F4781EF221D30E9uL, 0x394AD2CD8CF5837FuL)),
                    ((+1, 99, 0xE9D2EEB99C6AA84CuL, 0x93DEA0D73409663BuL), (+1, -18, 0xED1446DC21D2BA9FuL, 0x06064AB466E4AA7CuL)),
                    ((+1, 96, 0x82AEF41F84565CF0uL, 0x1612BD41432306A6uL), (-1, -21, 0x8723A08AE713C731uL, 0xA4BCCB68418C946CuL)),
                    ((+1, 91, 0xD53B276EEFFB8E16uL, 0x6124CEB5B0FB47F8uL), (+1, -26, 0xDFC6F99E452647FCuL, 0x74EADB988658A555uL)),
                    ((+1, 86, 0xE413632B810095A4uL, 0x96DCA685B95BA7C3uL), (-1, -31, 0xF1B6C3DBD40BD79AuL, 0xAE0C276BD424F26EuL)),
                    ((+1, 80, 0xF23EE6651CCE6B66uL, 0xA78FF4F9F5C5FD22uL), (+1, -36, 0x80FDD7FC6AA9FB31uL, 0xAF7668008C529A87uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX34Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 122, 0xD10B13981D2A0BC5uL, 0xE5FDCAB000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 123, 0xB15BAE6D6C6D4350uL, 0x9D4E0747166D47B3uL), (-1, 0, 0xE848F841283CBB73uL, 0x45ADA4D29B262170uL)),
                    ((+1, 123, 0x9156A59818642B49uL, 0xB51C4941F4EAAD64uL), (+1, 0, 0xCA90CAEC96D101BBuL, 0x67BC4F089065D351uL)),
                    ((+1, 122, 0x98ABC47D061BEDEEuL, 0x432378F57C0ED1E5uL), (-1, -1, 0xE154A046B715EA48uL, 0xBE3DA99671E67E3EuL)),
                    ((+1, 120, 0xE5FF5F31E40FBF72uL, 0x5FF1294F1F22156FuL), (+1, -2, 0xB2DC17AD84B69D61uL, 0x103C7C5CAF50130FuL)),
                    ((+1, 119, 0x839E5C3FE99B6BE4uL, 0xE3659D4116F0F2A5uL), (-1, -4, 0xD6ACDAAD2223D2B3uL, 0xABBFE9192A73A6A0uL)),
                    ((+1, 116, 0xEC71229D0874182BuL, 0xDF4E1BD08CFBDE9DuL), (+1, -6, 0xC93C5A7B4EC6FE90uL, 0x685C46A788A91ACCuL)),
                    ((+1, 114, 0xA9A072213F296BE8uL, 0xC78F6CDB72420C58uL), (-1, -8, 0x95F1E4ACB08BE0E8uL, 0x455287A28CE49906uL)),
                    ((+1, 111, 0xC3BD68AB5276484CuL, 0xAA484ACE6E031A30uL), (+1, -11, 0xB2DA07788128AFEEuL, 0xE61BECD10BDAAD41uL)),
                    ((+1, 108, 0xB52CF487D6D23EC3uL, 0xFC6A32A744A74255uL), (-1, -14, 0xAA4D0D6B69C5E4F8uL, 0x795152F60082A5E1uL)),
                    ((+1, 105, 0x84BC054245839B70uL, 0x8164C039FDC04198uL), (+1, -18, 0xFF7CB26F72F38C3DuL, 0xC1F220A3350C9149uL)),
                    ((+1, 101, 0x95AFC76019EA96A5uL, 0xBE1694A31AF833DDuL), (-1, -21, 0x92CBEE4D3F76F7C1uL, 0xEA7D72995D89178AuL)),
                    ((+1, 96, 0xF6601E7F568E6D8FuL, 0xAAAD6042564BDCB8uL), (+1, -26, 0xF50C7EA08CBD8D0DuL, 0x41390F680FE706A1uL)),
                    ((+1, 92, 0x84E6AFD0E0983614uL, 0x33292980775C91F5uL), (-1, -30, 0x856E200F1E34F11CuL, 0xA368BA02ED24BD5EuL)),
                    ((+1, 86, 0x8E5C297A214B6BA4uL, 0x7C0F6F399325FE96uL), (+1, -36, 0x8F97E9A6EA0B0885uL, 0x3F696FFF57586095uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX35Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 127, 0xDE1BC4D19EFCAC82uL, 0x445DA75B00000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 128, 0xBE50695915D50418uL, 0x1E2457812ED4BD81uL), (-1, 0, 0xE9E4A4CDAB7996C1uL, 0x6CEA86BA367BD485uL)),
                    ((+1, 128, 0x9D7A8ECD579A58F8uL, 0x34044B565ED3A1A7uL), (+1, 0, 0xCD650E0F864F2BA8uL, 0x16CA03EF0CB5909FuL)),
                    ((+1, 127, 0xA7038999FB1E40CFuL, 0x9458636DB06C3016uL), (-1, -1, 0xE617C1A348C47DC9uL, 0x8B4470D803AC4D95uL)),
                    ((+1, 125, 0xFDFC554233062532uL, 0xB8599F2F1AD5A5FBuL), (+1, -2, 0xB7F1DAB65917C86AuL, 0x72F1E69EB8BC9636uL)),
                    ((+1, 124, 0x92B34904E1739E14uL, 0x7CF5CBC857E2AF0CuL), (-1, -4, 0xDE5F7A5159035735uL, 0xBE5EEB878066245FuL)),
                    ((+1, 122, 0x84FA13895A42268CuL, 0x39B556236B57CD2DuL), (+1, -6, 0xD1F93E53A777C27AuL, 0x20FA8C193BE93C4CuL)),
                    ((+1, 119, 0xC086378837D6A3D4uL, 0x3362390E76DC0DBCuL), (-1, -8, 0x9D9C587F50BC6B5CuL, 0x70FB78A19C376393uL)),
                    ((+1, 116, 0xE02472C7C920F8D1uL, 0x17C7F00917D2F43DuL), (+1, -11, 0xBD66206A41F691EFuL, 0xE7BCA413A719F1F1uL)),
                    ((+1, 113, 0xD1499F9A9FED93EBuL, 0x84A2858B239AFA51uL), (-1, -14, 0xB5B4B2AF41D10C44uL, 0x23DFB67A0502F639uL)),
                    ((+1, 110, 0x9AA895B4BC1B3D13uL, 0x59376EBE2E777562uL), (+1, -17, 0x8956EECF5DBE1720uL, 0x6699E2BD158A2DBCuL)),
                    ((+1, 106, 0xAFE6B2782125D6FEuL, 0x9CFF203EB4347299uL), (-1, -21, 0x9F0B30099B01C00DuL, 0x23E198E39A26D6C2uL)),
                    ((+1, 102, 0x91FB60D817041EF4uL, 0x4B30786C2119BD01uL), (+1, -25, 0x85C8E3C25167E031uL, 0x9FEBA482528F3D24uL)),
                    ((+1, 97, 0x9ECD8987E9D77C46uL, 0xEEA25012F8FF98E7uL), (-1, -30, 0x92D896E3AD1BBBDAuL, 0xAD429A66FE515917uL)),
                    ((+1, 91, 0xAB8083F29BB3DFD7uL, 0x5038890AD713421CuL), (+1, -36, 0x9F4C27FAA8179188uL, 0x4A42CEB9BC6B3104uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX36Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 132, 0xF2EE5F4545E45CAEuL, 0x7AC66F0B88000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 133, 0xD2228B3CD6F6DD15uL, 0xFC1D7E041AD30840uL), (-1, 0, 0xEB77068930ABC9B5uL, 0x03E9BDD9CB53AD24uL)),
                    ((+1, 133, 0xAF81DAEE82A7C87EuL, 0x5ADB5DACC110C613uL), (+1, 0, 0xD02DBC465DB949E0uL, 0x1FCF7179E66DD6D8uL)),
                    ((+1, 132, 0xBBD97D13CED1A201uL, 0x95B8D97D10D564ACuL), (-1, -1, 0xEACF8CDD4759EDE0uL, 0x06A7D7C39D6E91B3uL)),
                    ((+1, 131, 0x9021FA86DFC8F79AuL, 0x1BE94FE6E166D24FuL), (+1, -2, 0xBD045F50A475FC5EuL, 0xCC85B9F308D654BFuL)),
                    ((+1, 129, 0xA7FD651108C075DBuL, 0xA83DA61E5B55D179uL), (-1, -4, 0xE61AD64C3354303BuL, 0x466FBDD7DF492A39uL)),
                    ((+1, 127, 0x999E20973AB07BC5uL, 0x39A6A751A7F9AFA4uL), (+1, -6, 0xDACFCFB33EDE2653uL, 0x2E9D1E700F363EBBuL)),
                    ((+1, 124, 0xE0578969ACD5A767uL, 0x2FFF96A3B4ED7DD8uL), (-1, -8, 0xA56B6627470AAA3AuL, 0x1C78E6680B0439DDuL)),
                    ((+1, 122, 0x83B6660E4C903B4EuL, 0xC93257E702417E43uL), (+1, -11, 0xC8384B273BAF2886uL, 0x95682F739785809EuL)),
                    ((+1, 118, 0xF80C166EA1CE2ABCuL, 0x2A6CDFB22BA063EDuL), (-1, -14, 0xC17DCFD27FD96749uL, 0x40AD48634B292456uL)),
                    ((+1, 115, 0xB8D4A5DFE3EFB62DuL, 0x42C20F533AF62C06uL), (+1, -17, 0x93541F52DC568914uL, 0x996D57B3F93661F2uL)),
                    ((+1, 111, 0xD3F2E15CA1C5C63CuL, 0xC0B68C10FFC6D381uL), (-1, -21, 0xABE304CB03909ACAuL, 0xAE565EB7860BDFC9uL)),
                    ((+1, 107, 0xB153FDB69FA11DA1uL, 0x74FE50EA15F9CC59uL), (+1, -25, 0x91AE75CE0A1EC910uL, 0x343F51CB2A10F778uL)),
                    ((+1, 102, 0xC2734345A5FBBFB3uL, 0xB4269D8EF4CE166BuL), (-1, -30, 0xA120AB5A608A67C9uL, 0x57A424224000AA8AuL)),
                    ((+1, 96, 0xD3AA16D572A3765DuL, 0x44E0F262CF1EC4B8uL), (+1, -36, 0xB024A0548FB55D05uL, 0x9D93CDA956A1E76DuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX37Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 138, 0x88A61596F7507422uL, 0x250F9E767C800000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 138, 0xEE8E8AEDD21CCF67uL, 0xA5DEE93B0AC6659AuL), (-1, 0, 0xED007BE0DF898416uL, 0x384CE68C4EB98E63uL)),
                    ((+1, 138, 0xC908F3BBB9588D69uL, 0xFB6185F05168D145uL), (+1, 0, 0xD2EB2AFB337DF82DuL, 0x4F4E85CAEB1C358CuL)),
                    ((+1, 137, 0xD913B14FEC6DA82FuL, 0x8A20514ED0FBA7C8uL), (-1, -1, 0xEF7C218306F097B7uL, 0x8228D52006E515CDuL)),
                    ((+1, 136, 0xA802EA801A40DEF4uL, 0xF109ACBC4741DF45uL), (+1, -2, 0xC21369ED2ACDA17AuL, 0xDD877F8D044C5B40uL)),
                    ((+1, 134, 0xC581690EA6F452B9uL, 0x99EDFF778137CF4BuL), (-1, -4, 0xEDDE31A2B0E0694CuL, 0x23465A4B94D595AAuL)),
                    ((+1, 132, 0xB623FCA833D784EAuL, 0x7A1220054074B8D1uL), (+1, -6, 0xE3BEFA8A1C0816FEuL, 0xC5A7C449F3C6F37EuL)),
                    ((+1, 130, 0x861C88AE3891A9D7uL, 0xC263B2B6F2310986uL), (-1, -8, 0xAD5E143AFAEEAE9CuL, 0xDD7C6F751EFBB503uL)),
                    ((+1, 127, 0x9EC69F55DFD04E0AuL, 0xFACB953CB2613678uL), (+1, -11, 0xD34F6A63C702F850uL, 0x0E308522AE8C4BAAuL)),
                    ((+1, 124, 0x96B9CDC0B1FE6A79uL, 0xB4F08BFE34AE7EFAuL), (-1, -14, 0xCDA7BC283549C371uL, 0x3987FADD594CC13BuL)),
                    ((+1, 120, 0xE26E99F0C91B3E99uL, 0x4A4CFE15D130C2BEuL), (+1, -17, 0x9DB616FA44241C6BuL, 0xCBF3E2E8069F3399uL)),
                    ((+1, 117, 0x82DBC0474996A394uL, 0xEF5D9590B99A3D9DuL), (-1, -21, 0xB954EEBB722629F7uL, 0xA637DB69CBC6E3E2uL)),
                    ((+1, 112, 0xDCAF5613D0D25A9AuL, 0x06A39DB58FFAD29EuL), (+1, -25, 0x9E39E103C6E96C86uL, 0xC62C621259C7756DuL)),
                    ((+1, 107, 0xF3DE7636E706C0EEuL, 0x6CDDF96DE48E5588uL), (-1, -30, 0xB04C1C0F97CBFE5CuL, 0xC6CB428E43FCB5E1uL)),
                    ((+1, 102, 0x85BECF483AB099ACuL, 0x27FD76B9A047C7C8uL), (+1, -36, 0xC22B3DAEEAB1F268uL, 0x7BB8DACB9580ED4CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX38Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 143, 0x9E0008F68DF50647uL, 0x7ADA0F38FFF40000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 144, 0x8B1F74A7948309FFuL, 0x22D1F74ADF31F7A5uL), (-1, 0, 0xEE815EA159244FBDuL, 0x601EB3D5A56BEDE8uL)),
                    ((+1, 143, 0xEC813CE6F6E2EC77uL, 0x47072CCA2B97CD25uL), (+1, 0, 0xD59DAD6AF6A7859BuL, 0x97E44CCAF9489076uL)),
                    ((+1, 143, 0x80C672AE43F2C46DuL, 0x1B0D60FC65538E20uL), (-1, -1, 0xF41DA25460CC9A9FuL, 0xD760A9B17B87C0CDuL)),
                    ((+1, 141, 0xC90286EA8C2AE2D3uL, 0x0C2FEEB5365838FCuL), (+1, -2, 0xC71EC6A61790D32BuL, 0xB93BA8695DCEAB4EuL)),
                    ((+1, 139, 0xEE40EF00FA1E972EuL, 0xCE8F15A5E0278D5FuL), (-1, -4, 0xF5A8DD4C28D132FEuL, 0x1F192922F8B432CBuL)),
                    ((+1, 137, 0xDD83A8103A223CF8uL, 0xAA4FD6D3020ABBC8uL), (+1, -6, 0xECC5B959864FA412uL, 0xE694E0C717411A25uL)),
                    ((+1, 135, 0xA46B122BF411F4FDuL, 0xF01745CADE5E3251uL), (-1, -8, 0xB573723D0E1AF47EuL, 0x3F917578FF1AC3CAuL)),
                    ((+1, 132, 0xC434DDEF5F2FB7ACuL, 0x2583E932CE3FC56FuL), (+1, -11, 0xDEAA6516DA8DD5AAuL, 0xBE9D217F799A0ABEuL)),
                    ((+1, 129, 0xBBB92D022F5F5CBCuL, 0x63FEF1BBC5AF8EA8uL), (-1, -14, 0xDA31C950ACCB241CuL, 0x525F0EE9599F55B5uL)),
                    ((+1, 126, 0x8E19F90050EEC5A6uL, 0x51F74B547D1828C1uL), (+1, -17, 0xA87CF45BC75BCD10uL, 0x1D5B96E73F1B34E0uL)),
                    ((+1, 122, 0xA5815281F6E5DB6CuL, 0x32ADA06552024CA6uL), (-1, -21, 0xC76254D7FDBC1FD3uL, 0xC83A1551528E1508uL)),
                    ((+1, 118, 0x8C9D7E44D432EB0EuL, 0xA71B2590CB6C14D9uL), (+1, -25, 0xAB6DF21BF7BA2BB2uL, 0x0F5214061052F4D9uL)),
                    ((+1, 113, 0x9C8D3430F43F47EBuL, 0xFD46044853FA87A5uL), (-1, -30, 0xC06081F08A0D0029uL, 0xA523780E9301F384uL)),
                    ((+1, 107, 0xACFCA687E34869A0uL, 0x33CDED0AD6F178DEuL), (+1, -36, 0xD569C7E0889E088DuL, 0x7C6EA13EDF20139CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX39Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 148, 0xBBA00AA4C892F774uL, 0xE1E2F213AFF1C000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 149, 0xA6994E4A4ACC18F2uL, 0x4DF62FA1E12A70A6uL), (-1, 0, 0xEFFA04281F25EDB5uL, 0xAC5A2C9B0688C6F3uL)),
                    ((+1, 149, 0x8EC842930ECB34E7uL, 0x20425DE7F9D50950uL), (+1, 0, 0xD84594801A45685FuL, 0x747441917B203A3DuL)),
                    ((+1, 148, 0x9CC2F70FB10318E7uL, 0xEA6398B686DB3D3AuL), (-1, -1, 0xF8B43493D0DDDAC3uL, 0xC5A7E251BB828F72uL)),
                    ((+1, 146, 0xF6AC53EF41358AD0uL, 0x70D3795E70F4C5B7uL), (+1, -2, 0xCC26485E76B08D77uL, 0xE1A9879EF4397A07uL)),
                    ((+1, 145, 0x935B017C9D613258uL, 0xA3C8166A955A27A2uL), (-1, -4, 0xFD7A3700EE59C4F1uL, 0x161CC9794908491EuL)),
                    ((+1, 143, 0x8A1501D5A126D0CAuL, 0x92A1CCD4B10E4762uL), (+1, -6, 0xF5E31445AE9199BAuL, 0x012A63DF8B6D6C2FuL)),
                    ((+1, 140, 0xCE93B7775809E7F8uL, 0xD031116DF8DD8D31uL), (-1, -8, 0xBDAA9849DBB37EC1uL, 0x86C91D1F3947584BuL)),
                    ((+1, 137, 0xF86936014AEC2BB3uL, 0x659D4C6425E42C56uL), (+1, -11, 0xEA4826BD7C96B205uL, 0xB1248A39230BF19FuL)),
                    ((+1, 134, 0xEF79B941A2800091uL, 0x53C92C60FAC09881uL), (-1, -14, 0xE71B442A16193E99uL, 0x49CEC35F3D6674EDuL)),
                    ((+1, 131, 0xB6A2F83920290ACAuL, 0x33ECEB9966511708uL), (+1, -17, 0xB3A8C992DE1E6D21uL, 0x9BAEFBCEC5C46545uL)),
                    ((+1, 127, 0xD64B326EDE5EDD5CuL, 0x18917C00695805D0uL), (-1, -21, 0xD60C848946D020A9uL, 0x0816BAB6AC068F87uL)),
                    ((+1, 123, 0xB766AFD8D3DB16B9uL, 0xA38895CA54A0319EuL), (+1, -25, 0xB94D582DF60DBA19uL, 0x2AAF3048911EE823uL)),
                    ((+1, 118, 0xCDAA8D87AD3C1157uL, 0x93A37A7695A0A322uL), (-1, -30, 0xD16350EB4BF8854EuL, 0x8D346B62BB1B686BuL)),
                    ((+1, 112, 0xE4E2DD27F340910BuL, 0xB29DFB842916ED7EuL), (+1, -36, 0xE9E9E3086F257665uL, 0xDA86E545752C19D1uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX40Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 153, 0xE4AB0CF8D4731D96uL, 0x734C9707FE6EA200uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 154, 0xCCAFFC0FE9F7688EuL, 0x84CCB4AA813C3B11uL), (-1, 0, 0xF16ABD96E5096E9AuL, 0x2A7D85C63D8440A5uL)),
                    ((+1, 154, 0xB0D424E51D708F69uL, 0x4FF0B578685EB2A4uL), (+1, 0, 0xDAE32EBAB98142BDuL, 0xB26CEF382E853560uL)),
                    ((+1, 153, 0xC3AC92B0AE79EC87uL, 0xC6E9C123902D1B46uL), (-1, -1, 0xFD3FFF7395F9558DuL, 0x5EB23E94D2562E69uL)),
                    ((+1, 152, 0x9B26FC74D5CCA8C3uL, 0xC9517FC1CFD75000uL), (+1, -2, 0xD129C7FCC5D7852BuL, 0xE50B93E254ED9723uL)),
                    ((+1, 150, 0xBACB585E26EED602uL, 0x37D0B1880972974FuL), (-1, -3, 0x82A8D4130744A6C5uL, 0x7E48209FF804A77EuL)),
                    ((+1, 148, 0xB05EC6C6FAEA5EEBuL, 0x408525A071E6CE58uL), (+1, -6, 0xFF1620372C99B279uL, 0x85F1D07466827C6EuL)),
                    ((+1, 146, 0x84EB9AB155E7A4BEuL, 0xA54B5A350E67694AuL), (-1, -8, 0xC602A6C419AC624EuL, 0x445D2A675CCE9D49uL)),
                    ((+1, 143, 0xA10667DB0706F2EFuL, 0xBF3CEDAA4842B53AuL), (+1, -11, 0xF6279F8CDF176E72uL, 0xC0336CCBA59BBC1AuL)),
                    ((+1, 140, 0x9C5F49B8A4E8EB9AuL, 0xA078F7E65A81DA6CuL), (-1, -14, 0xF46375A4264C0305uL, 0xAF35901871706EFEuL)),
                    ((+1, 136, 0xF03EF6142A090353uL, 0x318DA73F170169DAuL), (+1, -17, 0xBF399D4D6F35B0BEuL, 0x37D3E59F477D7ED0uL)),
                    ((+1, 133, 0x8DF4997080953054uL, 0x1756C1F2D122466BuL), (-1, -21, 0xE554B321DEE552AAuL, 0x42F8EE88152D12F4uL)),
                    ((+1, 128, 0xF4B4BAB872537209uL, 0x2621007142B45939uL), (+1, -25, 0xC7DAA5DC97F3153EuL, 0x3D389E444C55D500uL)),
                    ((+1, 124, 0x8A2B5F05570393D1uL, 0x69D6DF9D49775DEBuL), (-1, -30, 0xE359D8A892D4CD90uL, 0x43C5A557553AB247uL)),
                    ((+1, 118, 0x9AD608E78734C229uL, 0xBA129FA90533AF70uL), (+1, -36, 0xFFB50F2084E5ACA3uL, 0xF56718AF40C558D4uL)),
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
