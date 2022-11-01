using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erf(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (x < 0.0) {
                return -Erf(Abs(x));
            }

            if (x < 0.5) {
                return ErfUtil.ErfNearZero(x);
            }
            if (x < 1.0) {
                return 1.0 - ErfUtil.ErfcGtP5(x);
            }
            if (x < 2.0) {
                return 1.0 - ErfUtil.ErfcGt1(x);
            }
            if (x < 4.0) {
                return 1.0 - ErfUtil.ErfcGt2(x);
            }
            if (x < 8.0) {
                return 1.0 - ErfUtil.ErfcGt4(x);
            }
            if (x < 8.5) {
                return 1.0 - ErfUtil.ErfcGt8(x);
            }

            return 1.0;
        }

        public static ddouble Erfc(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }

            if (x < 0.5) {
                return 1.0 - Erf(x);
            }
            if (x < 1.0) {
                return ErfUtil.ErfcGtP5(x);
            }
            if (x < 2.0) {
                return ErfUtil.ErfcGt1(x);
            }
            if (x < 4.0) {
                return ErfUtil.ErfcGt2(x);
            }
            if (x < 8.0) {
                return ErfUtil.ErfcGt4(x);
            }
            if (x < 16.0) {
                return ErfUtil.ErfcGt8(x);
            }
            if (x < 27.25) {
                return ErfUtil.ErfcGt16(x);
            }

            return 0.0;
        }

        internal static class ErfUtil {
            public static ddouble ErfNearZero(ddouble x) {
                ddouble w = x * x;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erf.PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = x * (sc / sd);

                return y;
            }

            public static ddouble ErfcGtP5(ddouble x) {
                ddouble w = x - 0.5;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.GtP5PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt1(ddouble x) {
                ddouble w = x - 1.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt1PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt2(ddouble x) {
                ddouble w = x - 2.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt2PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt4(ddouble x) {
                ddouble w = x - 4.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt4PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt8(ddouble x) {
                ddouble w = x - 8.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt8PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }

            public static ddouble ErfcGt16(ddouble x) {
                ddouble w = x - 16.0;

#if DEBUG
                if (!(w >= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.Gt16PadeTable;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / (sd * Exp(x * x));

                return y;
            }
        }

        internal static partial class Consts {
            public static class Erf {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x906EBA8214DB688DuL, 0x71D48A7F6BFEC344uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -3, 0xA61A09E5DFFF9E51uL, 0x7E212CA19743662AuL), (+1, -2, 0xF444C1245E74CA13uL, 0x10959EDC1DBBED49uL)),
                    ((+1, -5, 0xDD3BE7EC8E8DA365uL, 0x3ED387AA38824326uL), (+1, -4, 0xDAEC569E191473DDuL, 0xBFA22416D66DD797uL)),
                    ((+1, -9, 0xE3AF40C1A35AEF17uL, 0x766BB5FF60AAD40BuL), (+1, -7, 0xF2AD11A0705F3761uL, 0xF0B31DB1C38038D7uL)),
                    ((+1, -12, 0xFC64EDC2CC309D71uL, 0x3BF873076B4FEC1DuL), (+1, -10, 0xB801F2E06C954F75uL, 0x209FE16330860FE3uL)),
                    ((+1, -16, 0x84B200012D12A197uL, 0x86767577C2D97370uL), (+1, -14, 0xC6C58D92AAFE5C59uL, 0xA2EDB491C53E64CDuL)),
                    ((+1, -20, 0x98D1102D90CD95D8uL, 0xD144093F348B4717uL), (+1, -18, 0x99AF298480E86469uL, 0x622B898CB8F7DD50uL)),
                    ((+1, -26, 0x88CB906F46369D0FuL, 0x4B3157B68077A2CAuL), (+1, -23, 0xA4EDFBA426238676uL, 0x7649C729AD87E905uL)),
                    ((+1, -31, 0x94C8AACB23722CB3uL, 0x1A5A662832E77F1BuL), (+1, -29, 0xE0607C0900410EFFuL, 0x75A020BD2E5F2788uL)),
                    (Zero, (+1, -35, 0x948D1DAF285AF468uL, 0x90EA1130872685B7uL)),
                });
            }

            public static class Erfc {
                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> GtP5PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0xCFE58F1CD6B15901uL, 0x377DD7FD748BB77AuL)),
                    ((+1, 1, 0x8AB803C6F3475E75uL, 0x7A6EF4CDB8692F7FuL), (+1, 2, 0x9BEEA85380B6BE97uL, 0x9C397B2EA3AF033DuL)),
                    ((+1, 1, 0x935E4CC0A2902222uL, 0x2B2F682DF64FB89DuL), (+1, 2, 0xDB303C5B1FAA7886uL, 0x1753779854F61828uL)),
                    ((+1, 0, 0xC770F6A954B72B6AuL, 0xF122F45183C3A8BDuL), (+1, 2, 0xBF3BEB65DDA6D4E9uL, 0x00C16F4EE1FA5682uL)),
                    ((+1, -1, 0xBDD87B34D74A2178uL, 0xD7F35FB303072A8AuL), (+1, 1, 0xE733FECF0F8B6B14uL, 0x95C998AB7121DADCuL)),
                    ((+1, -2, 0x85355CA3FC03C74EuL, 0xEE485FC34E7CC25CuL), (+1, 0, 0xCCA3E6B369C2AE12uL, 0x545250F410044BA2uL)),
                    ((+1, -4, 0x8CD5D367A161DB7EuL, 0x23C48D443CB15380uL), (+1, -1, 0x8882A1743989E0D0uL, 0x9A86DE54D9F534C9uL)),
                    ((+1, -7, 0xE196BB549DE9097AuL, 0x87E9F32102B74C2FuL), (+1, -3, 0x8B1253A7E2BB10DCuL, 0xE763A921A26451C8uL)),
                    ((+1, -9, 0x87A98D79DC7099C6uL, 0x2EC59FAF469DFB19uL), (+1, -6, 0xD889870F03E89848uL, 0x9D0AA3AE6B9D32FFuL)),
                    ((+1, -13, 0xEECD07B7B136AEDBuL, 0x45BC69DB53509F42uL), (+1, -9, 0xFEAAF61C01E8327BuL, 0x0CDB0DCC202642A9uL)),
                    ((+1, -16, 0x91F4283208E1E2F3uL, 0x0911C383182B533AuL), (+1, -12, 0xDC18CC2EBADCE3B5uL, 0xAF733E458118F742uL)),
                    ((+1, -21, 0xDE822A0E4D76651EuL, 0x7BE7F3A8306BE487uL), (+1, -15, 0x847F9F2825B1092AuL, 0x115C8DC5EC7DF2CBuL)),
                    ((+1, -26, 0xA00944D86EDB2C5DuL, 0x852483CCA1CF0B4EuL), (+1, -20, 0xC768B7E1BAE3CE4FuL, 0x04CAD8B7BF5B8472uL)),
                    (Zero, (+1, -25, 0x8DD4118E15F5D4F0uL, 0x7C266ABF2D30C293uL)),
                });

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt1PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 1, 0x95ADA7B501D4B116uL, 0x1C293D62F156A804uL)),
                    ((+1, 1, 0x999D65DF911850C4uL, 0x0754F1D897BE04D3uL), (+1, 2, 0xE3737EE719F802D0uL, 0x96E01E4809795A3AuL)),
                    ((+1, 1, 0xB20962272E0B53EDuL, 0x9F76AC013B9358FCuL), (+1, 3, 0xA3407D8AB75C6C42uL, 0xFBFABBE98247AA87uL)),
                    ((+1, 1, 0x832B2270E7895D1BuL, 0xAB9EA2EA9A2CE59CuL), (+1, 3, 0x92E1165436E2E616uL, 0xE2C6595E76218743uL)),
                    ((+1, 0, 0x88B41E6D38AFD6E6uL, 0x1AC916DB41B3E41DuL), (+1, 2, 0xB951E2271799E9CFuL, 0xFA701C3CD61B2483uL)),
                    ((+1, -2, 0xD4A1F5CBFBE79704uL, 0xCF61376B257127DBuL), (+1, 1, 0xADBB10CE59B3A920uL, 0x67117F429D591C67uL)),
                    ((+1, -4, 0xFE0FB9727C566433uL, 0xF68BCD054EF5DB19uL), (+1, -1, 0xFA1F1113C9745DEEuL, 0xE754305050089181uL)),
                    ((+1, -6, 0xEC9AF038F9AD2C54uL, 0x2A2182B431A74CC5uL), (+1, -2, 0x8CD7F0EA47D7BCCCuL, 0x21284C25F2EF1389uL)),
                    ((+1, -8, 0xAC72F1495F00BE7CuL, 0x76375039BF982E19uL), (+1, -5, 0xFA4F9BEE961EDE00uL, 0xCABAEA09876DE95BuL)),
                    ((+1, -11, 0xC3AF01FF05A2E065uL, 0x21EB2D1E7F95266BuL), (+1, -7, 0xAF96EC864E104B63uL, 0x7F4FBDF25BD1E2EFuL)),
                    ((+1, -14, 0xAA1C7290ADA6FD7EuL, 0xADD89675104B18C5uL), (+1, -10, 0xC0FB7481B8B5DED1uL, 0x17FDB72A22A258BBuL)),
                    ((+1, -18, 0xDBB4CEAF513438C9uL, 0x4F6EB6A63F538084uL), (+1, -13, 0xA34251F009A90DDEuL, 0x1232AD2132794003uL)),
                    ((+1, -22, 0xC749DCCB4F121F6FuL, 0x11B03ACF20A720BBuL), (+1, -17, 0xCDF10442A3EA6F8EuL, 0xE4025AC11EB86566uL)),
                    ((+1, -27, 0xE356B23CF260C070uL, 0xE5105357D098BCEAuL), (+1, -21, 0xB6F6D3D95B3401B7uL, 0xCF0CDCCCDC093007uL)),
                    ((+1, -33, 0xF62AE4E3AA6FD939uL, 0xF6C5531E9B9E54ECuL), (+1, -26, 0xCCE1E49542C502A5uL, 0xC10D740F109B7DA3uL)),
                    (Zero, (+1, -32, 0xDA290A6F232A69B6uL, 0xEF77BB3D4219B264uL)),
                });

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt2PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 1, 0xFA97703BE40EAC7DuL, 0x247AB845F0C2A95BuL)),
                    ((+1, 1, 0x973C745F0DBC0AA7uL, 0x51ACAFDCE807E19EuL), (+1, 3, 0xAE3CE81B627507EBuL, 0xE67DED6F792E6F00uL)),
                    ((+1, 1, 0xAA53CF44254182C2uL, 0x8A29C82264A1F35BuL), (+1, 3, 0xE555938843FE2C16uL, 0x26D0426A03327E03uL)),
                    ((+1, 0, 0xF218CB68C792577CuL, 0xCB29CF9D1E2E5378uL), (+1, 3, 0xBDA9C4327E4A37F4uL, 0xAAF79D63DD72D679uL)),
                    ((+1, -1, 0xF2856765286C6899uL, 0x0F1AC21D77A8BB98uL), (+1, 2, 0xDCAB8CCF4C7D873FuL, 0x64C9A8FB323EE315uL)),
                    ((+1, -2, 0xB534A7450FE24DF0uL, 0xD28447C9B9A45DDEuL), (+1, 1, 0xBF8BF59838C13BC0uL, 0x04362D1AD0AD9DE7uL)),
                    ((+1, -4, 0xD08346DD56EB6CAEuL, 0x533D151EB4B4263BuL), (+1, 0, 0x8058E9CDB4DC1FE9uL, 0xAFD7E7811140BF5AuL)),
                    ((+1, -6, 0xBC0DA933CF633876uL, 0x715BBBE9C93D0E31uL), (+1, -2, 0x87792AFABFCA3B3AuL, 0x7067AA23855C24B0uL)),
                    ((+1, -8, 0x85FBDE494F956FABuL, 0xBC2E5F57067A2320uL), (+1, -5, 0xE3B6335D1FADC27BuL, 0xF8CE93C913839D0CuL)),
                    ((+1, -11, 0x96CD297A89600034uL, 0x5E5AB21DEB28C152uL), (+1, -7, 0x98EE118EE05A832BuL, 0x396126E992EDAA71uL)),
                    ((+1, -14, 0x84F5D5EE6AEBD14AuL, 0x294096C69817426BuL), (+1, -10, 0xA3A73A45C0A19D50uL, 0x95EDD8BCE8C5269BuL)),
                    ((+1, -18, 0xB45F720F45AC130DuL, 0x7FF0A2BA4997AED7uL), (+1, -13, 0x8A1A5D787A595657uL, 0xA7CFC88CB52FA730uL)),
                    ((+1, -22, 0xB63CEF6D76F6A152uL, 0x489A1184819C92F3uL), (+1, -17, 0xB4404A88B5022291uL, 0x96D5AF0F752B0976uL)),
                    ((+1, -26, 0x817A2622B5743EC7uL, 0xDC2765CBA482FAC8uL), (+1, -21, 0xAFF1EBEFE32EC186uL, 0x93E397C948C89688uL)),
                    ((+1, -32, 0xE76D7620882C36E5uL, 0x82E003A299E3A01EuL), (+1, -26, 0xF25A7176C4F6E95BuL, 0x521ABA392849B9A7uL)),
                    ((+1, -38, 0xC42D963B574DA843uL, 0x5725E9CCE2443F58uL), (+1, -31, 0xD287CA1A289F3BF2uL, 0x7AC5C4F402BCD3DDuL)),
                    ((+1, -83, 0x8147BF99B84FB714uL, 0x5109333709F79E0BuL), (+1, -37, 0xADDBB8F5B662AC8BuL, 0x009A8067D645241BuL)),
                });

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt4PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 2, 0xE993DB6F3F17B91BuL, 0x5585C0B72C77686AuL)),
                    ((+1, 1, 0x87DFC2CD3CB1388BuL, 0xAB943F5FCB5B3B74uL), (+1, 4, 0x89C6B9398EBA9A00uL, 0x8DB6B55BB99103ECuL)),
                    ((+1, 1, 0x889C0B7DCE98A03FuL, 0x5286422AE046B990uL), (+1, 4, 0x9A07814BA27C35D3uL, 0x9754372AF64DE021uL)),
                    ((+1, 0, 0xACA54757EEA04849uL, 0xD9EE1100A06C8F51uL), (+1, 3, 0xD8C27409E3EC4029uL, 0xF74481ADB3DE8CDEuL)),
                    ((+1, -1, 0x9975C5512B202808uL, 0x59E4991AD21CBBCFuL), (+1, 2, 0xD70CEA7B0F2DDF98uL, 0xE90A2F0CD73F4327uL)),
                    ((+1, -3, 0xCB70B7F4E7E986B1uL, 0xD037144080FFA4A7uL), (+1, 1, 0x9FA4CF12E1EAAC0DuL, 0x6314AD03D94D633EuL)),
                    ((+1, -5, 0xD007AD320745FEE2uL, 0x4DD22E4DDA8F0200uL), (+1, -1, 0xB7AD0195A484DB4DuL, 0x04FA9ED7A9B1CD86uL)),
                    ((+1, -7, 0xA75EFDB0C11B39CEuL, 0xCCAD0382F8F6BE15uL), (+1, -3, 0xA74B02F0125F743CuL, 0x750CC0C0BDDC8470uL)),
                    ((+1, -10, 0xD626C882E87DB7C6uL, 0x7BF28BD578B808F9uL), (+1, -6, 0xF4456FB1C0B25120uL, 0x9AB052E60E2FF1C1uL)),
                    ((+1, -13, 0xDA9CC71E16801D4CuL, 0x9CCF59744708C616uL), (+1, -8, 0x8FC90FB7AB75F0DEuL, 0x58CC0EB04AA45E03uL)),
                    ((+1, -16, 0xB177C273801B9F3FuL, 0xC98826F1B3F1E6EEuL), (+1, -11, 0x887FFD2F61E3F365uL, 0x9917D7246E020C0FuL)),
                    ((+1, -20, 0xE2BD55BF376A14C9uL, 0xA34C771B4E3663DAuL), (+1, -15, 0xCFD612418A6DC7CCuL, 0x69FFF76C7F018438uL)),
                    ((+1, -24, 0xDF8023F7FDC1AF79uL, 0xE0E7AD8E0BB8CC2AuL), (+1, -19, 0xFAB60DD3A35FC23DuL, 0x9432337C390F9C30uL)),
                    ((+1, -28, 0xA454CEB7B995A712uL, 0x670F080268BCDE5FuL), (+1, -23, 0xEA9DD3EEE330BC03uL, 0x39A056FF6F6F2A0AuL)),
                    ((+1, -33, 0xAA03F4BC6B274348uL, 0x8EFD47D78FDEFD50uL), (+1, -27, 0xA483CD0FA42350ABuL, 0xCCD639A70C8D492FuL)),
                    ((+1, -39, 0xDD2A61E2E49A7B13uL, 0x4431D0ADEF69EA12uL), (+1, -32, 0xA2EFEC077D7323DCuL, 0x228D66F6FD5AB114uL)),
                    ((+1, -45, 0x8848DC6A79D92521uL, 0xADEE925C8C6072D5uL), (+1, -38, 0xCB8D334F7996365EuL, 0x1386AB7C692A26F0uL)),
                    (Zero, (+1, -45, 0xF18EE56264BBE5F5uL, 0x0791F206FB7902ADuL)),
                });

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt8PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 3, 0xE49EB02D10E55196uL, 0xF242A9AA48D3D599uL)),
                    ((+1, 0, 0xAE7EFDF1B14EF53CuL, 0x926006BEE327A9DAuL), (+1, 4, 0xA9E824BD7E45DE61uL, 0x9172EBC63EEF36ADuL)),
                    ((+1, -1, 0xDE084E83E86A3544uL, 0xDB966355F04C950EuL), (+1, 3, 0xECAEC2002FD7D6CEuL, 0xCC73ED1D92E733BBuL)),
                    ((+1, -2, 0xAEC1E29F9CABD45BuL, 0x8B17F7152B33C636uL), (+1, 2, 0xCCF474FAA4DBD2FBuL, 0x9D388CAB54C2E77BuL)),
                    ((+1, -4, 0xBE1BF616F76CA1E4uL, 0x0CCA5E2D607CABC7uL), (+1, 0, 0xF6C804153A4DCCF2uL, 0xCFDBE86DFA7E68D4uL)),
                    ((+1, -6, 0x973073A3F3B2DD11uL, 0x59ECCAE5C4E11353uL), (+1, -2, 0xDAD664FB822EE99DuL, 0xCA6C34760B47981DuL)),
                    ((+1, -9, 0xB54C280F69835BCEuL, 0x6E40F45D98144021uL), (+1, -4, 0x93A65C1A643140DAuL, 0xF6A48E999F598EA4uL)),
                    ((+1, -12, 0xA682292C56934CA1uL, 0x8B8BC4CF51247C59uL), (+1, -7, 0x9A5F30788DA0CEE1uL, 0xFEF3BC8EBACE6742uL)),
                    ((+1, -16, 0xEB674C79AFAC5619uL, 0x0C7263F651F23518uL), (+1, -11, 0xFC2DECE344BBCE82uL, 0x5622DC20DEC6922EuL)),
                    ((+1, -20, 0xFEE9A70E70C50785uL, 0xB94A46BB641E6E47uL), (+1, -14, 0xA0EC91D8D1089501uL, 0x4EAD1299577E7B67uL)),
                    ((+1, -24, 0xD023158E364EDB3FuL, 0xCA2BDE6533AA521BuL), (+1, -18, 0x9F284B2422848652uL, 0x82EBA5B9FE1CDE8BuL)),
                    ((+1, -29, 0xF8869885097C7D90uL, 0xED69C1D563EA65D5uL), (+1, -23, 0xEF98E92C658DE104uL, 0x23389FF89A0F25C7uL)),
                    ((+1, -34, 0xCD20971186895AB1uL, 0x63CE69D44711DC0DuL), (+1, -27, 0x84DEBE911AC4C682uL, 0x687C41759457CDDBuL)),
                    ((+1, -40, 0xD185E4A1E2AF0BC7uL, 0xF1CE027D1B5F1CD5uL), (+1, -33, 0xCD02C1AABB3801CDuL, 0x4118E9BBD26F6709uL)),
                    ((+1, -47, 0xC7D4460652F81B09uL, 0x4F11A090F55D4616uL), (+1, -39, 0xC4C0D96A00D16E3AuL, 0xA7C3C7AC45F2E863uL)),
                    (Zero, (+1, -46, 0xB1181127AD047634uL, 0x3211269261DA78AEuL)),
                });

                public static readonly ReadOnlyCollection<(ddouble c, ddouble d)> Gt16PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 4, 0xE350C4179D924C9BuL, 0x7094703468B7D8E1uL)),
                    ((+1, -1, 0xA49B29FC4BB0D6B3uL, 0x76147A830414A2BAuL), (+1, 4, 0xA0507F06A2C831E0uL, 0x1E89E3A35287EAD7uL)),
                    ((+1, -3, 0xC0D1A735DA0DC9CAuL, 0x1B30DF05E25700F3uL), (+1, 2, 0xCF9EA8B9712B16E9uL, 0x1BAA86B406258F5EuL)),
                    ((+1, -5, 0x87CA28F41F81B7CAuL, 0xB8CF372330E1B0B9uL), (+1, 0, 0xA33A6B99FD17FCE2uL, 0x6F0478A882ADD22FuL)),
                    ((+1, -9, 0xFF83D68FE382A35AuL, 0xA1F38559D2A3A1D2uL), (+1, -3, 0xAD878D7B272E6134uL, 0xC428D8E826558A4FuL)),
                    ((+1, -12, 0xA89DCD8F800676F3uL, 0xFB8BADE41C44FF68uL), (+1, -6, 0x83678CB21E7C7ACEuL, 0xB11F9A57226BDE5CuL)),
                    ((+1, -16, 0x9F46F08AE80664A6uL, 0x49E3BEDC208A04F2uL), (+1, -10, 0x915AC549EDD02C0BuL, 0xCF4861D5629BB32CuL)),
                    ((+1, -21, 0xD75DFA5C372E77D7uL, 0x74AC9BB8A5DDB958uL), (+1, -15, 0xECA728D4300C4D74uL, 0x02E9B00C55883B1FuL)),
                    ((+1, -26, 0xCC41C0E81E6C2152uL, 0xE909FCDDC7E10C47uL), (+1, -19, 0x8CB5ACE5D5905205uL, 0x2D1B4EA0861E5BABuL)),
                    ((+1, -31, 0x816844888BE4DDB1uL, 0x1182D9FAA6509EF8uL), (+1, -25, 0xEE6127DB8CB1B675uL, 0x067A5A5521824DCBuL)),
                    ((+1, -38, 0xC52A6B8900B478FFuL, 0xBA756E232D5DAFFAuL), (+1, -30, 0x8887957C1268257AuL, 0xD585F49356764EBBuL)),
                    ((+1, -45, 0x88D31242713AC949uL, 0x359FF2079684A05EuL), (+1, -37, 0xBDE408254E997A57uL, 0x1FDCB5C043F596C3uL)),
                    (Zero, (+1, -45, 0xF283DE11BF2C2226uL, 0x294285FDBDCD1505uL)),
                });
            }
        }
    }
}
