using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erf(ddouble x) {
            if (x.Sign < 0) {
                return -Erf(-x);
            }
            if (IsZero(x)) {
                return PlusZero;
            }
            if (IsInfinity(x)) {
                return 1;
            }
            if (x >= Consts.Erf.ApproxMin) {
                return 1d - Erfc(x);
            }

            ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erf.PadeTable;

            (ddouble sc, ddouble sd) = table[^1];
            ddouble w = x * x;
            for (int i = table.Count - 2; i >= 0; i--) {
                sc = sc * w + table[i].c;
                sd = sd * w + table[i].d;
            }
            sc = sc * w + 1d;
            sd = sd * w + 1d;

            ddouble y = x * (sc / sd) * Consts.Erf.C;

            return y;
        }

        public static ddouble Erfc(ddouble x) {
            if (x.Sign < 0) {
                return 1d + Erf(-x);
            }
            if (IsZero(x)) {
                return 1d;
            }
            if (IsInfinity(x)) {
                return PlusZero;
            }

            if (x < Consts.Erfc.PadeApproxThreshold) {
                int table_index = Math.Max(0, (int)Floor(x * 2));
                ddouble w = x - 0.5d * table_index;
                ReadOnlyCollection<(ddouble c, ddouble d)> table = Consts.Erfc.PadeTables[table_index];

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    sc = sc * w + table[i].c;
                    sd = sd * w + table[i].d;
                }

                ddouble y = Exp(-x * x) * sd / sc;

                return y;
            }
            else {
                ddouble w = x * x;

                ddouble c = x * ddouble.Exp(-w) / ddouble.Sqrt(ddouble.PI);

                ddouble f = 1d;

                int n = 6;
                for (int k = 4 * n - 3; k >= 1; k -= 4) {
                    ddouble c0 = (k + 2) * f;
                    ddouble c1 = w * (2 * f + (k + 3));
                    ddouble d0 = (k + 1) * (k + 3) + (4 * k + 6) * f;
                    ddouble d1 = 2 * c1;

                    f = w + k * (c0 + c1) / (d0 + d1);
                }

                ddouble y = c / f;

                return y;
            }
        }

        internal static partial class Consts {
            public static class Erf {
                public static readonly double ApproxMin = 0.5d;
                public static readonly ddouble C = 2 * Rcp(Sqrt(PI));

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeTable = new(new (ddouble c, ddouble d)[]{
                    ((+1, -3, 0x8D536A97FE3900BAuL, 0xF1A4723EEEB5C89BuL),  (+1, -2, 0xF1545FF6A9C72B08uL, 0x237CE3CA22058EF8uL)),
                    ((+1, -5, 0xBCFAC9176679496CuL, 0x64534A5327B96DF6uL),  (+1, -4, 0xD3766D07C8CEBBF4uL, 0x3F58B2BF9F175379uL)),
                    ((+1, -9, 0xB05B7C2EC1B9FC1BuL, 0xB1EB2F374DDC2BE2uL),  (+1, -7, 0xE1D485913879F92AuL, 0x3008707450AAC5DBuL)),
                    ((+1, -12, 0xC1D6C19D93F6903CuL, 0x79A69F1B9E1B09F4uL), (+1, -10, 0xA17596FA184A4492uL, 0x3FEF19A31F117818uL)),
                    ((+1, -17, 0xA3FF05960641F00CuL, 0x72C9C32A8213F0A5uL), (+1, -14, 0x9F13B2F4471A0A34uL, 0xC434EAF132D4739DuL)),
                    ((+1, -21, 0xB167E8E9A0751069uL, 0x7846A33F13CFF71BuL), (+1, -19, 0xD428A86C6754FDFFuL, 0x358A41012CA65D08uL)),
                    ((+1, -28, 0xAC03CC1AD11517DCuL, 0x102D8A1AE05DA25DuL), (+1, -24, 0xB04D66F5524B7D4EuL, 0x2A5E71C48ACB5089uL)),
                    ((+1, -34, 0xD95EE5054FF883F4uL, 0x624F12D692C014F7uL), (+1, -30, 0x8CEC1EE915626AC1uL, 0x57E3911AC93BA08DuL))
                });
            }

            public static class Erfc {
                public static readonly double PadeApproxThreshold = 12d;
                public static readonly ddouble TableBin = 0.5d;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
                
                static Erfc() {
                    PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                        PadeX0Table,
                        PadeX0p5Table,
                        PadeX1Table,
                        PadeX1p5Table,
                        PadeX2Table,
                        PadeX2p5Table,
                        PadeX3Table,
                        PadeX3p5Table,
                        PadeX4Table,
                        PadeX4p5Table,
                        PadeX5Table,
                        PadeX5p5Table,
                        PadeX6Table,
                        PadeX6p5Table,
                        PadeX7Table,
                        PadeX7p5Table,
                        PadeX8Table,
                        PadeX8p5Table,
                        PadeX9Table,
                        PadeX9p5Table,
                        PadeX10Table,
                        PadeX10p5Table,
                        PadeX11Table,
                        PadeX11p5Table,
                        PadeX12Table,
                    });
                }

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX0Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0xD21D53165A4CB6ACuL, 0x575470A88AEE67D4uL), (+1, 1, 0x89E5F5D54FDF0265uL, 0x9E6A2B68D4EF0631uL)),
                    ((+1, 2, 0xA27FA8D18FCC18A3uL, 0xC0D0617B7D09C45CuL), (+1, 1, 0x97E890CE6E0AAD25uL, 0xCAE5CEABC1583511uL)),
                    ((+1, 2, 0x9D07F19067CE0C49uL, 0x34CE3CA229A5CFA3uL), (+1, 0, 0xDA9FD92458AB69B8uL, 0xFB2501223857102BuL)),
                    ((+1, 1, 0xD3EFCAD9A8603AD3uL, 0x9FF0544CA686EAAEuL), (+1, -1, 0xE1F989E7D9F38015uL, 0xD3D296819C44D0A4uL)),
                    ((+1, 0, 0xD36A98432EAFFEA3uL, 0x004EFBC363BB833CuL), (+1, -2, 0xAF8EDE2F3CC283EBuL, 0x2958CBAA336017B8uL)),
                    ((+1, -1, 0xA0D73D1B88B75884uL, 0x83674FAE9A332E83uL), (+1, -4, 0xD1C6F92EA087AE1EuL, 0x5965B6364C55C86BuL)),
                    ((+1, -3, 0xBDB8AD3785161241uL, 0xF4E6F8AED333EF75uL), (+1, -6, 0xC279C058DD99B614uL, 0x0415DE496D7C6DD8uL)),
                    ((+1, -5, 0xAE7381D93487FCD2uL, 0x5CA42BFC37F52B6DuL), (+1, -8, 0x8B8C57B9945504CDuL, 0x591821AC9A657269uL)),
                    ((+1, -8, 0xF90E5A999D3CC587uL, 0x258CCBDA5C34EBAAuL), (+1, -11, 0x98DE2DD4EF0C288DuL, 0xCDC15AC85D721DABuL)),
                    ((+1, -10, 0x87F706D4B9F5D641uL, 0x314800BA40F38EDBuL), (+1, -15, 0xF84515296B3BBB7AuL, 0x57AB819FFC18CB7DuL)),
                    ((+1, -14, 0xDC5FD27A1344C24BuL, 0x4256DBB218B32394uL), (+1, -18, 0x8D763D6539108BEDuL, 0xE9F88B88631DCD2BuL)),
                    ((+1, -18, 0xFADAAF6A769755B0uL, 0xBF01D1E1FE1AD256uL), (+1, -23, 0xCAC2F081642774B5uL, 0x21FF135260903300uL)),
                    ((+1, -22, 0xB3B14DF6E4A97D6FuL, 0x5761F6CDA0BD182EuL), (+1, -28, 0x8A12753C9698D58DuL, 0xFCE96EE325C80A45uL)),
                    ((+1, -28, 0xF4BA05964982002FuL, 0x9C67550FB0DA9718uL), (+1, -54, 0xA6EDD04D16B998D1uL, 0xB370801135467985uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX0p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xCFE58F1CD6B15901uL, 0x377DD7FD748BB77AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0x93756C939A6311C2uL, 0xCC6614590987F50EuL), (+1, 1, 0x8048DCD60956E95FuL, 0x041F2283305C9D95uL)),
                    ((+1, 2, 0xC2CD749432765BF0uL, 0xF0664F29E3E6638DuL), (+1, 0, 0xFC0EB026FB4F276BuL, 0x7D67144502F4270FuL)),
                    ((+1, 2, 0x9E8A453AB02C141EuL, 0xD1A1F539CA5F7EEFuL), (+1, 0, 0x9CC0E8BA546592AAuL, 0xF939C8CD4FE182A7uL)),
                    ((+1, 1, 0xB12AEA04D1677B71uL, 0xF4CEBEAD1778C366uL), (+1, -1, 0x87CF4EBB7EF85E7EuL, 0x9B701B136B493CDBuL)),
                    ((+1, 0, 0x8F4A64E1FA38D409uL, 0x79EE5FFD90747914uL), (+1, -3, 0xAB1E407F3BE0D240uL, 0xBC21E3B779EB4EECuL)),
                    ((+1, -2, 0xAC223A18F7DF8047uL, 0x7AD9D6627D42207EuL), (+1, -5, 0x9F69EC78CAE8C58EuL, 0x9BBAD90014A6AC1BuL)),
                    ((+1, -4, 0x9AD9C0020FBF7CFBuL, 0x0A8CDE963DE70D90uL), (+1, -8, 0xDB200E893C4646C8uL, 0x5B502E2C3F78371AuL)),
                    ((+1, -7, 0xCF3D78DC3738A054uL, 0x68B3B99DA7848644uL), (+1, -11, 0xD97C5F3E63C33E84uL, 0xACB93309C01EFFD8uL)),
                    ((+1, -10, 0xC962DB68FCB2A298uL, 0x119AA9CDFC53FF3EuL), (+1, -14, 0x9457B5455DD946BAuL, 0xA3189F060E2EDCE0uL)),
                    ((+1, -13, 0x870441A2BA3E5A1EuL, 0x9EE61AD2A6648677uL), (+1, -19, 0xFA484D4E2B73E36DuL, 0x84ED7E5CBB96ADDAuL)),
                    ((+1, -18, 0xE08C06BBD9FE7EDAuL, 0x44375155E73E73D0uL), (+1, -24, 0xC5E05042D63D3814uL, 0x35351E3672D2444FuL)),
                    ((+1, -23, 0xAF5D068C41F98E1BuL, 0xC82106CCFAE1AAA0uL), (+1, -50, 0xF5FC069B449F18D2uL, 0xAB678498C39918CBuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x95ADA7B501D4B116uL, 0x1C293D62F156A804uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xC675B8CA92B56323uL, 0xEBB0BB17CCFA884CuL), (+1, 1, 0x80D28A6C4748FAC0uL, 0x45B3A0C149841ECFuL)),
                    ((+1, 2, 0xF4BD4545E564F0A0uL, 0x42BB8ED1E6541A23uL), (+1, 0, 0xF7E96A7E1D03F33FuL, 0x9FCF37499744756BuL)),
                    ((+1, 2, 0xB9AE68BFE77DCF1AuL, 0xFF7AE786E3FB7F57uL), (+1, 0, 0x94F17A015B7848CBuL, 0x4DCB606CF66A5A26uL)),
                    ((+1, 1, 0xC1282209E6CCA914uL, 0x3E9235965F133BBFuL), (+1, -2, 0xF6F2C8F87D270DE8uL, 0xA5B7CA8ACA9BFFA1uL)),
                    ((+1, 0, 0x91340A93E6829BC9uL, 0x036B8E13D27407F7uL), (+1, -3, 0x93CD960D11539A21uL, 0x4E6CD69F6069CCD4uL)),
                    ((+1, -2, 0xA1DD21D8941FAD3CuL, 0xC2C770BBE12F5B0DuL), (+1, -5, 0x82118AB6F412E770uL, 0xDB4D325F617E82DEuL)),
                    ((+1, -4, 0x86E28F84913C8B9FuL, 0xAF1114E9411B5452uL), (+1, -8, 0xA813D1BF590D9A33uL, 0x7A4A31C0AF1A6924uL)),
                    ((+1, -7, 0xA6E87723479567B7uL, 0x47A4B0203AEF4761uL), (+1, -11, 0x9C29664E022E1FF4uL, 0xC5B08AB30C0B14FBuL)),
                    ((+1, -10, 0x95A926112658650FuL, 0x345874C9A1A83A59uL), (+1, -15, 0xC6A4CE84ED7C40BAuL, 0xC02881FE0EC19D58uL)),
                    ((+1, -14, 0xB8C3DEF4EB7490F6uL, 0xB4DD6E0F38BA6714uL), (+1, -19, 0x9BAE5AAD496DE491uL, 0x7F0B59D48771B3D5uL)),
                    ((+1, -18, 0x8D1FC3B8625BB546uL, 0x1509E5A70258312CuL), (+1, -25, 0xE3DDB7CF4C37518EuL, 0x43BE40CB7FB05118uL)),
                    ((+1, -24, 0xC9F0EA5F91DC8B13uL, 0x6695A1A916269158uL), (+1, -54, 0xC2DB0C227587885FuL, 0xB0786D0F57C24A4CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xC703957D5C0D716CuL, 0xD50AE71B0A541FC6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xF701E087CE9E82B9uL, 0x810D6F2322053637uL), (+1, 0, 0xFC9BFC70D1DE6C9DuL, 0x68E3D9B1A5F717D3uL)),
                    ((+1, 3, 0x8E660C6940A73697uL, 0xF7A6AFBE000F50FAuL), (+1, 0, 0xEAFE199B30781C11uL, 0x1C3ABEABC963E8A2uL)),
                    ((+1, 2, 0xC9C41962A7A9ABACuL, 0x92530620B5D1E111uL), (+1, 0, 0x87464E926DE26165uL, 0x1A4C766726B1EDAEuL)),
                    ((+1, 1, 0xC3BB028D9BDF7401uL, 0x8FC84D58A35F9CE0uL), (+1, -2, 0xD57D328F102DAA92uL, 0x7873940D6EC47A10uL)),
                    ((+1, 0, 0x8904BAA00C87D857uL, 0xDE0F59431B8AF31DuL), (+1, -4, 0xF1FD610867B93EC2uL, 0x0F965901E643716CuL)),
                    ((+1, -2, 0x8E05607BA6CAB69FuL, 0x5DD0AED52C3F9ED5uL), (+1, -6, 0xC8C69134B9DB9626uL, 0x474E0F5E7449B745uL)),
                    ((+1, -5, 0xDBBC23DB05C6F6FAuL, 0x56F077AC8342191AuL), (+1, -9, 0xF3AE4C19AC2B685CuL, 0x7DDA6E1499A65B4CuL)),
                    ((+1, -8, 0xFBFB4CBE76FBBD28uL, 0xB241A5ACFFE6A248uL), (+1, -12, 0xD3E98E17D65B5290uL, 0xBFC23FB34EE578C9uL)),
                    ((+1, -11, 0xD100FD58D4214B75uL, 0xB6680CC361082720uL), (+1, -16, 0xFB7C05CDDF03D9CCuL, 0xF9981F46FA253162uL)),
                    ((+1, -15, 0xEE357C7F0113BAB1uL, 0xC09CB8019318E594uL), (+1, -20, 0xB74C0098705D1D91uL, 0xB25F256DAF276B97uL)),
                    ((+1, -19, 0xA79BDF95DEF59086uL, 0x0849D11244EBD9D8uL), (+1, -26, 0xF8B9873419A98C94uL, 0xD8362B5085415BD2uL)),
                    ((+1, -25, 0xDC6D320EF54644DFuL, 0x6CC21A00D3122050uL), (+1, -58, 0xA82D4BEC6B29CE5DuL, 0x408D093B0BFC20BAuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xFA97703BE40EAC7DuL, 0x247AB845F0C2A95BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x8A0858696368E3D8uL, 0xF6A8C51230B6E88FuL), (+1, 0, 0xE47FB48D1550291DuL, 0xEF2CDA23BDB72F97uL)),
                    ((+1, 3, 0x8C1204913A010B1CuL, 0xCEBABD8D1003804EuL), (+1, 0, 0xBD34E6E21003A542uL, 0x2262E7958BECF458uL)),
                    ((+1, 2, 0xACE586907290EE60uL, 0x36269FD80A642AA0uL), (+1, -1, 0xBEBCAEC520256C0DuL, 0x62F4BBB3B3AE739BuL)),
                    ((+1, 1, 0x904938E7D484A974uL, 0xAEBA30C5053507D6uL), (+1, -2, 0x815A0A597B005A99uL, 0x0CBD9C0BBC710A64uL)),
                    ((+1, -1, 0xAB094620D72C34BCuL, 0x3491395FFE7BE319uL), (+1, -5, 0xF65698382C3E4414uL, 0xA7550600AC0351CDuL)),
                    ((+1, -3, 0x930288BB56016874uL, 0xA4617E8FC7C1D1B2uL), (+1, -7, 0xA6A5078ECE58DCBFuL, 0x2B465CBA67D4C28AuL)),
                    ((+1, -6, 0xB758A634FAB08A9FuL, 0xB9A80655839DEAECuL), (+1, -10, 0x9E1B0D6E89BCE432uL, 0xAD48AF018AE3FD95uL)),
                    ((+1, -9, 0xA2AD07B93BCDAFE7uL, 0x78F28DC6C67E0FF5uL), (+1, -14, 0xC950DFD4877C7EA3uL, 0x57DF362D26D1B4EEuL)),
                    ((+1, -13, 0xC3B73786A4099D7EuL, 0xB595E7DC9F4626C3uL), (+1, -18, 0x9B57B2BBA02DFB7FuL, 0xD16ECF6D1F1EC086uL)),
                    ((+1, -17, 0x8FC80CFC00C2C5B9uL, 0xF6C306B3644E35D5uL), (+1, -24, 0xDCB7A8898C0181D8uL, 0x1C4865755512C557uL)),
                    ((+1, -23, 0xC39B102AEF5701F1uL, 0x0A409BA9889FB680uL), (-1, -57, 0xC0E52895E463AB73uL, 0x63A78CF03B41D5A8uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0x97CC4E8FCA11393AuL, 0xF05995F6EC64ACF3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0x9311DB331CDF8AAFuL, 0xEB96CCF27B8A20E8uL), (+1, 0, 0xCAE1FDD80311E65DuL, 0x2EE2C1693FC4800FuL)),
                    ((+1, 3, 0x81E90B08BBD89151uL, 0xBA16151106BB5846uL), (+1, 0, 0x92C16D9F54893FD9uL, 0x0D56372B621A2AB4uL)),
                    ((+1, 2, 0x89D5297150D9E985uL, 0x9512864BE987A730uL), (+1, -2, 0xFDC124874D36F597uL, 0x08B1900EB17B3202uL)),
                    ((+1, 0, 0xC29B1D627F763C8CuL, 0x2478C813A1B302D5uL), (+1, -3, 0x9048A7A9944ED336uL, 0x84D3171581BE10B1uL)),
                    ((+1, -2, 0xBF1EE6C96DF02FA3uL, 0xD8DC525A8FEA5B64uL), (+1, -6, 0xDFA2AAE2D460C325uL, 0xDBC375D683E32670uL)),
                    ((+1, -4, 0x844A2D1B3E3E9F41uL, 0x96B3759954FB4DA7uL), (+1, -9, 0xEC1F7CF80C2B3FBBuL, 0x818E5292E90A1924uL)),
                    ((+1, -8, 0xFF0BF89129B02BDFuL, 0x342C11D1D362E3AFuL), (+1, -12, 0xA3BD72843779AD72uL, 0x51FF53A079FC4A97uL)),
                    ((+1, -11, 0xA3F0EF214674C89EuL, 0xEC69015171FABE25uL), (+1, -16, 0x8759AEDE20863414uL, 0x9313DC3FB6322146uL)),
                    ((+1, -16, 0xFDFA1405F3BB2044uL, 0x89F134952C5492E1uL), (+1, -22, 0xCB48B0D8C9F80D32uL, 0xB4B87BD8BA984CD9uL)),
                    ((+1, -21, 0xB427DC9383C6A36FuL, 0x027453B8E991D54FuL), (+1, -55, 0xA3023374F35ED7E7uL, 0xE9EACF8AE44E8992uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0xB2C5119D353C4F70uL, 0xFA7EECAC6CF0B957uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xA22A78E8BFACE31DuL, 0x6DC75A0A9D77191AuL), (+1, 0, 0xC157B4ADFE8754C9uL, 0x586C6E1AC23AB823uL)),
                    ((+1, 3, 0x85F5B45A6A3CFBB5uL, 0xA31FF158CAE814D1uL), (+1, 0, 0x84A6D00507BFCBBAuL, 0xEBBBA1BE3A8A7675uL)),
                    ((+1, 2, 0x84C00407D03E1C7CuL, 0xBFE6E047E86B7810uL), (+1, -2, 0xD8BC95F2BA106F68uL, 0x4213C694DB1C1ABEuL)),
                    ((+1, 0, 0xAED476BFF4F5A321uL, 0xE4BD6549FAD1FFF3uL), (+1, -4, 0xE82611262CF98E66uL, 0x8B8D7E5F6A70C438uL)),
                    ((+1, -2, 0x9FEE8BFB9FC86510uL, 0xD3F21194A355CF70uL), (+1, -6, 0xA8F76183480A678EuL, 0x8BEECFB121F74351uL)),
                    ((+1, -5, 0xCDEBF067CD870E05uL, 0x3477A3BA2F122E3FuL), (+1, -9, 0xA7185F5D7F50BFAAuL, 0x2541FA911D2D97E3uL)),
                    ((+1, -8, 0xB854109B321302DBuL, 0x34B7554B34054EB5uL), (+1, -13, 0xD87E974219D7DFF7uL, 0x4527492A393C1A6EuL)),
                    ((+1, -12, 0xDBAC439AA2B7887EuL, 0x4B995909D1A60418uL), (+1, -17, 0xA6C0C24E97CC30FAuL, 0xF9608F15F740817FuL)),
                    ((+1, -16, 0x9D7351BC701F2D92uL, 0x128737EB9B30B2B9uL), (+1, -23, 0xE8C3D90C970305B3uL, 0xEE4F5830E2FA2D6EuL)),
                    ((+1, -22, 0xCE485AF0EEE60F9FuL, 0x4428A81665D08CDBuL), (+1, -58, 0x81ADA540BE376838uL, 0x296771D78A2FFB6EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0xCE0FAC0ECAAE10FCuL, 0x6D2CBFA71BC566C3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xAF62B1AB517F65A8uL, 0x3B24FAA8EA4C60FFuL), (+1, 0, 0xB7D44EAB3C45806AuL, 0x8B309F3F7262DAAAuL)),
                    ((+1, 3, 0x87CAE48E405363F8uL, 0x0DD1B77AAD302CA2uL), (+1, -1, 0xEF047405F6F8D001uL, 0xFEC8CF2F02242731uL)),
                    ((+1, 1, 0xFBF7A62488B952E3uL, 0x45B577649F506552uL), (+1, -2, 0xB87C843A005B0ECBuL, 0x0D85EBE0E12D78BCuL)),
                    ((+1, 0, 0x9B26EC90BD11C323uL, 0xBB291AFB137B0817uL), (+1, -4, 0xBA3877A4AA7559FFuL, 0xA6D663240CE8FA81uL)),
                    ((+1, -2, 0x848DDF9C8FF76C0AuL, 0xCECF60649575E5B6uL), (+1, -7, 0xFEDA8EA20997395DuL, 0x9AA6C000433FC413uL)),
                    ((+1, -5, 0x9F2F28DA6716AAC8uL, 0x34207AE8484D18B6uL), (+1, -10, 0xEC6A21CB993344DCuL, 0xB36FF911665083FEuL)),
                    ((+1, -8, 0x84B6C84C46EB3817uL, 0x9805FBB72FCE6698uL), (+1, -13, 0x8F590CC4DA5937DEuL, 0x97E4CE5C83668643uL)),
                    ((+1, -12, 0x931659C3F1B6DDC6uL, 0x45958CCA4724AF8CuL), (+1, -18, 0xCE3C0E58CBD7B0FAuL, 0xF2B60BD5DA5DD299uL)),
                    ((+1, -17, 0xC3C5A5ACD9155E6FuL, 0xD22D2AEDC4F5B9CEuL), (+1, -23, 0x862165F2B0780CF1uL, 0x72E681C9C9427E62uL)),
                    ((+1, -23, 0xEDBD742867BA29EEuL, 0xAB0EEC773BD6F0FDuL), (+1, -62, 0xE484C2025EEA79AEuL, 0xBD751D6BC3483163uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 2, 0xE993DB6F3F17B91BuL, 0x5585C0B72C77686AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xBAEE883262681E67uL, 0xC7FD1EF59025784AuL), (+1, 0, 0xAE9EC2ED321CBA49uL, 0x2EC734F7A2D5DF65uL)),
                    ((+1, 3, 0x87F4EC8588374CF8uL, 0xA65FD1EF98A9EE84uL), (+1, -1, 0xD71C25845227DBC4uL, 0x9201360D4D802030uL)),
                    ((+1, 1, 0xECBC7F36F9D84069uL, 0xC90F719FDFFE803FuL), (+1, -2, 0x9CF299EBFBE962DDuL, 0x3D6AC90D1C03DE57uL)),
                    ((+1, 0, 0x88A696000A254AF8uL, 0x758C410CD77BA261uL), (+1, -4, 0x9570EFB0820A3624uL, 0xEAC70423F3FDD6E1uL)),
                    ((+1, -3, 0xDAA351F9CFE6EF1AuL, 0x3A0B0662026725C5uL), (+1, -7, 0xC08B4689509E04AAuL, 0xA520B8905670E0C6uL)),
                    ((+1, -6, 0xF590BCBBD9C4E2E1uL, 0x61E5BFCB8F19AADBuL), (+1, -10, 0xA7D5E1DA5C440119uL, 0xA3B441A165DB7161uL)),
                    ((+1, -9, 0xBF3D4454506B345FuL, 0xF13F520641E87F55uL), (+1, -14, 0xBEE4577DDB1C651EuL, 0x68C106EBB0B8CFCCuL)),
                    ((+1, -13, 0xC5B8E34D8330B545uL, 0xFAED7B28B4759E66uL), (+1, -18, 0x808CAE976A74D4BDuL, 0x571E12DDA7F399EBuL)),
                    ((+1, -18, 0xF527DE48BE768D00uL, 0xC8A1D14B3223C336uL), (+1, -24, 0x9C3C82E9A18C8EDCuL, 0xBCAD2C54C7D24AC2uL)),
                    ((+1, -23, 0x8A75FC3D29D1D3E1uL, 0xF8033C4F49B98101uL), (+1, -65, 0xDE303B332F984C8FuL, 0xF5E9BDE815997BAAuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0x82A0E207790A8FCBuL, 0xD3197BC052575658uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB5B3B71F0A8AC83CuL, 0x19540787BD953400uL), (+1, 0, 0x96DBC80366BB0A0CuL, 0x4375165BCF9715ECuL)),
                    ((+1, 2, 0xE2C9FEFDB476F3A9uL, 0x5A02CB388D18CC1FuL), (+1, -1, 0x9DE7B36DD1A53BF2uL, 0x80281D9839E69FA6uL)),
                    ((+1, 1, 0xA6B93D44222A0F70uL, 0x6729AEF2AB22693DuL), (+1, -3, 0xBFA5E137D8B76272uL, 0xB9D6EA4B857C5546uL)),
                    ((+1, -1, 0x9F27C12ABBEF5DD2uL, 0x53DF0B13182A6911uL), (+1, -5, 0x9376E6EC8D527C8FuL, 0xFE3A647EF47DA84CuL)),
                    ((+1, -4, 0xCCA565E1DA86869EuL, 0xBB66B6552C481B0DuL), (+1, -8, 0x934FA31052763E62uL, 0xBD6F905FDD64EDC8uL)),
                    ((+1, -7, 0xB1453442918637B5uL, 0x71C54E24B6AFDB5AuL), (+1, -12, 0xBA930E0A7D8B2FB3uL, 0x7752B5BC14184C3FuL)),
                    ((+1, -11, 0xC7909B47F58FFD38uL, 0x9BC5C99AF78D574CuL), (+1, -16, 0x88F8B40C0E93D8DEuL, 0x66B29702BA58EA5CuL)),
                    ((+1, -15, 0x8483439DF2195553uL, 0xC91F38A761546BC7uL), (+1, -22, 0xB2888CF242BF9C9DuL, 0x1EAD3AD8F0847B93uL)),
                    ((+1, -21, 0x9E389A73BEBBDCF4uL, 0xEDB1DF3505EE3039uL), (-1, -61, 0x9490AC5D94CF7458uL, 0xA3A45798D7E3D85EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0x90875A6FD5F1BAD7uL, 0x3E0DACC0385555CAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xBD6FA8C72F60AD89uL, 0xF55D02F173C4BE49uL), (+1, 0, 0x8F1B28E5429E4B38uL, 0x8AA510CE34573363uL)),
                    ((+1, 2, 0xDE9AF9ABF5086719uL, 0x4EDC21B47BB96592uL), (+1, -1, 0x8DD50FF89E2E806CuL, 0x810124F36DAFD247uL)),
                    ((+1, 1, 0x99EE772C46DF3595uL, 0x7C5F9EE64CC9E988uL), (+1, -3, 0xA2B7E2CF4DAEB188uL, 0x53E8EC0EC81ECE69uL)),
                    ((+1, -1, 0x8A1691B68743F635uL, 0xF5F91E06CBA74D5CuL), (+1, -6, 0xEC521696337AEFA4uL, 0x51449FCA3C882BE2uL)),
                    ((+1, -4, 0xA6B0DA4A9BDA56D3uL, 0x66F74A312044DF74uL), (+1, -9, 0xDE71B0797D2A5454uL, 0x905A45A96895279BuL)),
                    ((+1, -7, 0x8769FB4026DC8079uL, 0xD15BC73F0998FE1BuL), (+1, -12, 0x848705F80DFF90A1uL, 0x7D00A2EB1803615CuL)),
                    ((+1, -11, 0x8ECEEE28373CCD04uL, 0xBE0AD7637CDFC2C7uL), (+1, -17, 0xB6C89EB1169588E3uL, 0x68D89A6F19988DDEuL)),
                    ((+1, -16, 0xB1754AFFC88865D8uL, 0x0C635D913E2089BDuL), (+1, -23, 0xDF7140B16A56A021uL, 0xE3209A9EC7B19EB7uL)),
                    ((+1, -22, 0xC6054A523D6A0586uL, 0x54094BDD123C6A28uL), (-1, -64, 0xC8A3263C7C465041uL, 0x9F801C180BB9DFF3uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0x9E79A1ACA6D7D0B3uL, 0x1D50D4B4DBF6127AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB238D2BDC0C38342uL, 0xF11D8F8C03B49210uL), (+1, -1, 0xF2C74D13FA590A96uL, 0xB992512BFFACC077uL)),
                    ((+1, 2, 0xB0D3F7A0AC6184F8uL, 0xCB17E6442C13AA7EuL), (+1, -2, 0xC7D6411997A3D0D0uL, 0x7F6CAAF9C1B6A75AuL)),
                    ((+1, 0, 0xCA34EE80DD1353C6uL, 0x176CAF7367819332uL), (+1, -4, 0xB908B86A8044818DuL, 0x886223E0AB9021C4uL)),
                    ((+1, -2, 0x91C52A41A9F544D2uL, 0xA34695E765B0238DuL), (+1, -7, 0xD01F514A30172055uL, 0xF0D216B5950A9C42uL)),
                    ((+1, -5, 0x87B4007FCD81EE6BuL, 0x5DD871B09460488AuL), (+1, -10, 0x8E2E2A32A9442BB5uL, 0x54742C6A271320AAuL)),
                    ((+1, -9, 0x9F59C96DE5217401uL, 0x25E19ED5B9E80CA5uL), (+1, -15, 0xDA82CAEF8FC0300FuL, 0xDEE8E4E071917D39uL)),
                    ((+1, -14, 0xD7D878E6313DE7ABuL, 0xD9C64E8995C164E9uL), (+1, -20, 0x91B70965586D217DuL, 0x5C6963D9A9BCB9FEuL)),
                    ((+1, -19, 0x8122F49DA02C54BEuL, 0x4F6BBEAB9ED3540BuL), (+1, -60, 0xFC4C11FF4B77106AuL, 0xA678E52E48FE5ACCuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xAC75142A62EC2F69uL, 0xD69227D809FF9E1DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB7353792C96C757CuL, 0xA29D0FB6C9413DABuL), (+1, -1, 0xE666EE71F89056F0uL, 0x7AB6F1EBAD9E9BBAuL)),
                    ((+1, 2, 0xAB945E3C60D2FA1AuL, 0x2D43F4FBA488D87FuL), (+1, -2, 0xB3BA540145D828F7uL, 0x7825B995B3F2A67FuL)),
                    ((+1, 0, 0xB90DB293BB7CA692uL, 0x47BD78B824BFC847uL), (+1, -4, 0x9D7F2F8F748D6280uL, 0xB0693B8F2FAC0CD5uL)),
                    ((+1, -3, 0xFB6FB7E63B8BAD1DuL, 0xA42B14C2F5DC1156uL), (+1, -7, 0xA77001FCCF058C62uL, 0x2BF3C9D2C398C633uL)),
                    ((+1, -6, 0xDC6528E1079CAD7FuL, 0x30EEE4F2B4A9F868uL), (+1, -11, 0xD7F3D73C4EDD42D0uL, 0x06C8429963F4034FuL)),
                    ((+1, -10, 0xF375A0F603DFFF60uL, 0x614106453ECFD103uL), (+1, -15, 0x9C7173A3A0294149uL, 0x11B39CEF08F3CF0EuL)),
                    ((+1, -14, 0x9AF739E9AD55CFBDuL, 0x1D2050CF8FA522B3uL), (+1, -21, 0xC4727D93634750BAuL, 0x2251569D115733BBuL)),
                    ((+1, -20, 0xAE18C96BF64189B6uL, 0x91F7B86EBFB0238DuL), (+1, -62, 0xE61854B6A0D017C7uL, 0x2324DCDAD2C2CC9EuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xBA77C5EAC845D0B7uL, 0xD3A00C5AB9589A59uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xBB89E50684606B3FuL, 0x2437CE4379271155uL), (+1, -1, 0xDAF7822525174711uL, 0x60C72DF6648B0A1FuL)),
                    ((+1, 2, 0xA62AA020F94C7E57uL, 0xC2281015024027E2uL), (+1, -2, 0xA22338AF7D3D1472uL, 0x20C3A5A941DE3396uL)),
                    ((+1, 0, 0xA96F4687D475CC8BuL, 0xB9E5F7BF24487FDAuL), (+1, -4, 0x86B763B59313A99CuL, 0x6C413ACED25B7EBCuL)),
                    ((+1, -3, 0xD97E08967C1B959DuL, 0x7865489C5174BCEFuL), (+1, -7, 0x87A4802F25FABB5CuL, 0x39DF6E308D093D78uL)),
                    ((+1, -6, 0xB3F8449D7D82416EuL, 0x75A4C435C21E9891uL), (+1, -11, 0xA5819CADF6C41DEDuL, 0x5ED84EC2841E36B5uL)),
                    ((+1, -10, 0xBB86DEBD542C0651uL, 0x9FBA08F30FABDCD8uL), (+1, -16, 0xE29B560C68577999uL, 0xD0D7B4E410A2E337uL)),
                    ((+1, -15, 0xE10038E6F409DA56uL, 0xEE9F441279C4C851uL), (+1, -21, 0x864C93B7C24D2DD6uL, 0xC9F254D0C864A98DuL)),
                    ((+1, -21, 0xEE09FC73BCAF8A7EuL, 0x16AF2346919818AEuL), (+1, -64, 0xE06FE1AE367FA753uL, 0x4587F0789FAABE7CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xC880492DEA5819A9uL, 0x3097601A02DAFD71uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xBF50152F4D23AEF9uL, 0x2E540FBE9358D259uL), (+1, -1, 0xD06868F17904E8E6uL, 0x72C22CC6C2DC4378uL)),
                    ((+1, 2, 0xA0B905250E3A8062uL, 0xEB24D37113B50430uL), (+1, -2, 0x92BAAE1B03B0CBFCuL, 0x02693C7F14A44EB1uL)),
                    ((+1, 0, 0x9B4A9BC5CA418F5EuL, 0x3F4E30D1BF0D23F6uL), (+1, -5, 0xE79BCEE81A749414uL, 0x3CD79EED6BDEBAD9uL)),
                    ((+1, -3, 0xBCC3DE2BB93D7DB9uL, 0xDF5386BD50066920uL), (+1, -8, 0xDD4D012D8F673A7DuL, 0xB086F5BC75C298B0uL)),
                    ((+1, -6, 0x93D0D78357E9FFC4uL, 0xBEE111FFDD0C66C6uL), (+1, -12, 0xFFFF94A9F179DFD2uL, 0xD1286A6062A65E06uL)),
                    ((+1, -10, 0x91A7E4743918ED9BuL, 0x17562E0885E9DE7DuL), (+1, -16, 0xA5FD8249C11F0612uL, 0x699D54D41C0F29EEuL)),
                    ((+1, -15, 0xA5268AA0852B2887uL, 0xDF41AEBA2F0BA402uL), (+1, -22, 0xBA2B1F59B8FD9E6FuL, 0x58CF6731BE9FD336uL)),
                    ((+1, -21, 0xA4FCCC2D8FA6A47CuL, 0x128390324989221CuL), (+1, -66, 0xE914DDD9F28A6962uL, 0xC9BE905DEB802678uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xD68D88FFF34DFE5CuL, 0xD7873BAD3C2D233EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xC29CFE5881D33557uL, 0x358E217FF1D8B7BBuL), (+1, -1, 0xC6A8180A42C0C184uL, 0xB806D7F6C2C3719BuL)),
                    ((+1, 2, 0x9B573D227A4795C3uL, 0x5D6C38B483952140uL), (+1, -2, 0x853448910FBF740EuL, 0x42AE81C35C8C0A8DuL)),
                    ((+1, 0, 0x8E86E696B4D8142DuL, 0x49AADBF6ED043B56uL), (+1, -5, 0xC814230CC3ECD1BEuL, 0x82B49D545E58B0F3uL)),
                    ((+1, -3, 0xA46CA285CB05B5B0uL, 0x8D56FC657F3C70FFuL), (+1, -8, 0xB5C3D47B83A19600uL, 0xFB7C37602719E532uL)),
                    ((+1, -7, 0xF4401E3EF9CE7245uL, 0xA5E93202E3C0A4EAuL), (+1, -12, 0xC7BF8CF9B9F82263uL, 0x03F5221D2300536BuL)),
                    ((+1, -11, 0xE42631CE72F4EE53uL, 0xDA5DBB9C3A1396A6uL), (+1, -17, 0xF5DFBB0C4FBDF8B0uL, 0x657AC0FB56F9A989uL)),
                    ((+1, -16, 0xF50FAB73A09D81F2uL, 0xE9484F353F5B3517uL), (+1, -22, 0x82C3CE6A606185C8uL, 0x2E9954EF8C69F8B0uL)),
                    ((+1, -22, 0xE7C65261FC7A94FBuL, 0xEF36F4E77E328124uL), (+1, -67, 0x8050818DAC4351FDuL, 0xEE7F52F801ACE4EFuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xE49EB02D10E55196uL, 0xF242A9AA48D3D599uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xC582703BE4C39405uL, 0x35D91B9309ACD4C0uL), (+1, -1, 0xBDA53324640093B9uL, 0x41B10FC0F74F0EDEuL)),
                    ((+1, 2, 0x96153E99C17DB992uL, 0xC97C70E6CBAF6B9AuL), (+1, -3, 0xF29AFB36223F7CB2uL, 0xB075B2D7C4E68215uL)),
                    ((+1, 0, 0x8306F937965E1017uL, 0xDDBC0C3EBE9FE082uL), (+1, -5, 0xADADC3FC6B758775uL, 0x4D79AC521B2F1A80uL)),
                    ((+1, -3, 0x8FC1F7A623AE0B68uL, 0x17EFBF00C63190CFuL), (+1, -8, 0x964A771DE989A135uL, 0xBF1C9EDA0737ED7AuL)),
                    ((+1, -7, 0xCAFD8D713D6B942CuL, 0x4F22751ACC652B6AuL), (+1, -12, 0x9D345DFDBAEB96F4uL, 0x6247D9A96A348C83uL)),
                    ((+1, -11, 0xB422FD2A9E35E84CuL, 0xBBEE729B5A886F21uL), (+1, -17, 0xB80CC95B4AB53102uL, 0xC7DA2BD7D8BB7CFDuL)),
                    ((+1, -16, 0xB7B8CEB9F00A2353uL, 0xC9AFEBB98933B89DuL), (+1, -23, 0xBA106DD66C9371C1uL, 0xE165867ECD45E9E7uL)),
                    ((+1, -22, 0xA4E5242283276AA8uL, 0xCDB9D876109F75C2uL), (+1, -69, 0x9530FC8604C7A213uL, 0x553721798FDD97C6uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 3, 0xF2B3182146A1FFCAuL, 0x5BB41D33B16C2495uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB17E3EA18A90CAD7uL, 0x4325BB31780EF566uL), (+1, -1, 0x9D816E68FF668561uL, 0xB701E6D68DE4CF55uL)),
                    ((+1, 1, 0xDFA23C3AAC92BAFAuL, 0x1F4725C8EDB326C3uL), (+1, -3, 0xA2ABD05C8F784756uL, 0x8BAF776CBEE0BFB6uL)),
                    ((+1, -1, 0x9D5477FCF1B7F579uL, 0x3BF8FFE5CE9CAD7DuL), (+1, -6, 0xB47D0A8932118D34uL, 0x29942C76149DF5DDuL)),
                    ((+1, -4, 0x8580A70F1561C0A0uL, 0x4D77DBDB9D20CB14uL), (+1, -10, 0xE2E643BC79945DE3uL, 0x6F27319CF00F7D10uL)),
                    ((+1, -8, 0x88A5FFCDC55A39B9uL, 0xA36D0657023BC0EFuL), (+1, -14, 0x99385D0B5253AA0DuL, 0x004B8CA297D02269uL)),
                    ((+1, -13, 0x9C3B1DFAE2D5ACCCuL, 0xC56A118F54517ECEuL), (+1, -20, 0xADAF62DD0037A727uL, 0xFBC3498F8423BF9BuL)),
                    ((+1, -19, 0x99ECA668959C69C3uL, 0x76CCBD19997EFE85uL), (-1, -63, 0xBB489C68FABEFCB8uL, 0xA52CE8296313B2DEuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x80651E7AF93DF16DuL, 0x9ABA508AF29A20ABuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB34D9027ECEFBFB2uL, 0x63FF1E979811B9FDuL), (+1, -1, 0x96A5D1117E7E46CAuL, 0x96CC6B4FCF056A36uL)),
                    ((+1, 1, 0xD79B9077612D2E03uL, 0xC65A954A62963653uL), (+1, -3, 0x94B84454AC788C9FuL, 0x0AC806863CD5AAE3uL)),
                    ((+1, -1, 0x90B410577FF31D41uL, 0x7EE4D130E251B248uL), (+1, -6, 0x9DA049936826EE81uL, 0x660DE606229035B7uL)),
                    ((+1, -5, 0xEA2CF04F1D641F69uL, 0x4F707F6F97DE9DE1uL), (+1, -10, 0xBD2CBEC1CBDEFB8CuL, 0xF9ED9F83675D8297uL)),
                    ((+1, -9, 0xE47729E997A1D3CEuL, 0x545F352B661840B6uL), (+1, -15, 0xF3C1BA2D3EF5135BuL, 0x531058811BC44C2BuL)),
                    ((+1, -14, 0xF8DB8335B2B6477CuL, 0x017EC1C2F67F55D0uL), (+1, -20, 0x83BAAD1BB7733041uL, 0x1B1F17E5391C2C19uL)),
                    ((+1, -20, 0xE97BE32622AB7764uL, 0xD57D99040E89626DuL), (-1, -64, 0x881D2D3A33E17ED3uL, 0x226D1F3F1E069A97uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x8771DA7A4AC4CBBAuL, 0xD0D70DFCDA47DE11uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB4E6DDF96AC0841FuL, 0x96AC33919B7B12BDuL), (+1, -1, 0x904D429F7B3B5BCAuL, 0x703613FDBA54F3B3uL)),
                    ((+1, 1, 0xCFF8BFD8B94D198EuL, 0x3C7A1DF3D55B3BF9uL), (+1, -3, 0x8860E5D357190A82uL, 0xC030D5452D4BFE92uL)),
                    ((+1, -1, 0x856558B1933EA84AuL, 0x331D9B3B0D98C12DuL), (+1, -6, 0x8A4DD66577B67319uL, 0x2661CA713EF0952CuL)),
                    ((+1, -5, 0xCE3C64253F054DDEuL, 0xCDC0C8E71BDB1E4FuL), (+1, -10, 0x9EBB2A459F17644AuL, 0x928F871C3C32B9A0uL)),
                    ((+1, -9, 0xC02596D00F68C15DuL, 0xFE31D24365378BEAuL), (+1, -15, 0xC37BB75FBA02D4BCuL, 0x871F2AACDABD6784uL)),
                    ((+1, -14, 0xC7CAF11966DFF9F1uL, 0x878A05ADCA54A0D8uL), (+1, -21, 0xC9D3B8792469E632uL, 0x98314EC5BDEE9E8DuL)),
                    ((+1, -20, 0xB2DD533E68BDD1F5uL, 0x4886376573D9FB67uL), (-1, -66, 0xCE0BEEC799A027EFuL, 0x9D73307AB7E60B30uL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x8E7F953A3BF17CB0uL, 0xE11B36FEA630E7EEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB651C84259DE2FA9uL, 0xDD728F976C05E181uL), (+1, -1, 0x8A6B5AE309EB4171uL, 0xCF7AF45E4E1FF901uL)),
                    ((+1, 1, 0xC8B8FF263E485C1EuL, 0xEC705D78FB43E31FuL), (+1, -4, 0xFAD9806E64122EEAuL, 0xC72C8490B2721ED7uL)),
                    ((+1, -2, 0xF680EA1B046B0F10uL, 0x15F8DF18C91AB20AuL), (+1, -7, 0xF3C88F254CEB7C43uL, 0x8A2BD4068303A0A7uL)),
                    ((+1, -5, 0xB65BDDA7CB9AD8BDuL, 0x2AFF0019C89AB53BuL), (+1, -10, 0x85FF13647FAEDF9DuL, 0x16ACDE9FC8C6ADE5uL)),
                    ((+1, -9, 0xA289F3F80969E283uL, 0xE3103E04726360A4uL), (+1, -15, 0x9DFCB6BB4A399F2BuL, 0x61CFB0B58ECEBDA1uL)),
                    ((+1, -14, 0xA1A0419C6CE030CAuL, 0xBDB7D3C3526AB781uL), (+1, -21, 0x9C15B527AA68098CuL, 0x7E136DBF4FCA0973uL)),
                    ((+1, -20, 0x8A5398ACCE022696uL, 0x465F511107558DECuL), (-1, -67, 0xA1F8B6E14DB10CC8uL, 0x2EF2AC531AC2E57CuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x958E2BAD288369A5uL, 0x1447AE46C2913DEDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB794B5A605FB288EuL, 0x22B2A67CD8D5DB96uL), (+1, -1, 0x84F4F04BB74CB512uL, 0xE028CBED83D02CAAuL)),
                    ((+1, 1, 0xC1D9CAD5471F6EE3uL, 0x9FDA206B4184A197uL), (+1, -4, 0xE75747C25D02C666uL, 0x8BB134808A860D66uL)),
                    ((+1, -2, 0xE444475A0216B0D0uL, 0xB77D96853DFE6D84uL), (+1, -7, 0xD7C3ED91685215D5uL, 0x724E310E9AC247B9uL)),
                    ((+1, -5, 0xA1DE6C721DEFECE1uL, 0x8A4661CE0F926AB1uL), (+1, -11, 0xE38989E77C532B57uL, 0x89F8FACCB7F9D263uL)),
                    ((+1, -9, 0x8A40EF8BBD4CC2AAuL, 0x0AF504C5101DF74CuL), (+1, -15, 0x809F4C603563C853uL, 0x18A674ECCB01A087uL)),
                    ((+1, -14, 0x83B279C168BB147AuL, 0x3390235970CE0ACCuL), (+1, -22, 0xF39753C56E95AB10uL, 0x5F074FF7C3394230uL)),
                    ((+1, -21, 0xD7E083CCE9D1DB4EuL, 0x6ABA48FA893ABDC0uL), (-1, -68, 0x83E7C448AF926DC0uL, 0x85D8AA722407C29FuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0x9C9D80DF537800BCuL, 0xD6BB8D588FB40098uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB8B50A1A13B598DAuL, 0x559692B52D8A54A9uL), (+1, -2, 0xFFBFFFCEE681C12DuL, 0x37F23AD730B24079uL)),
                    ((+1, 1, 0xBB5789159B12F2CFuL, 0xA88D61AADB1493FEuL), (+1, -4, 0xD5E979AF948503A8uL, 0xF7DFB2105E940897uL)),
                    ((+1, -2, 0xD3D6CCD7228B67C4uL, 0x0173E47989EB662BuL), (+1, -7, 0xBFBC9E48FA1EBD74uL, 0x23B1A9166CD29730uL)),
                    ((+1, -5, 0x903695AD09113608uL, 0xACF776AF7614B963uL), (+1, -11, 0xC23EDD95B6972FCFuL, 0xCFA758B41599C890uL)),
                    ((+1, -10, 0xEC6EAAEDACDE7661uL, 0x5D6AAD4121EA36C6uL), (+1, -16, 0xD2E372155D4F644EuL, 0xF3FAD647FA6216E0uL)),
                    ((+1, -15, 0xD81809B2FC022A30uL, 0xA60DF3AC0A5F1EF9uL), (+1, -22, 0xBFB1A4F965FF200DuL, 0x5D17C14B7BF768E6uL)),
                    ((+1, -21, 0xA9E26290F5939D7EuL, 0xAA2ACF57B12EADFBuL), (-1, -70, 0xDE0EFA43AE6BFF6FuL, 0x1BCBADB62BDD4CCAuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11p5Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xA3AD7CB395CBB683uL, 0x7B03439F0AB755D8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xB9B753B02D961E96uL, 0xE5FD67C6DFE65CC3uL), (+1, -2, 0xF64725F7EBB95D3DuL, 0xA5B784C525F0D7CEuL)),
                    ((+1, 1, 0xB52DFB77826230DAuL, 0x17EAF7CD4993F128uL), (+1, -4, 0xC64AC1A5CA1DE930uL, 0x639E4251D7264375uL)),
                    ((+1, -2, 0xC50232F64AF19C42uL, 0x99C6E3352E789C24uL), (+1, -7, 0xAB092658F135D4DCuL, 0x1BD0F4E3BD3A2731uL)),
                    ((+1, -5, 0x80F04E7A4610AFA7uL, 0x5D704432AC35C501uL), (+1, -11, 0xA6AF3AB1985A6090uL, 0xDBF1903C5CFB5BC6uL)),
                    ((+1, -10, 0xCB2D883AA1D254BAuL, 0xAE707861A8F74BB5uL), (+1, -16, 0xAE0644BCF710DB40uL, 0x9A0E99F50C379929uL)),
                    ((+1, -15, 0xB270CB9A08F3D643uL, 0x2131C776AED8A19EuL), (+1, -22, 0x9810A2CB5D5DF67AuL, 0xA06A9021C73FA368uL)),
                    ((+1, -21, 0x86C39AF638BC728FuL, 0x632779A61218626BuL), (-1, -71, 0xC0C97A348874129BuL, 0x91D29EC5A024138AuL)),
                });
                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX12Table
                 = new(new (ddouble c, ddouble d)[] {
                    ((+1, 4, 0xAABE0AECA8754739uL, 0xB303B0D8B5C9B72CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xBA9F6F28EB0A14B8uL, 0x9C0818E50C2C8920uL), (+1, -2, 0xED6F4902AE91308EuL, 0x7B5E67134B2143E0uL)),
                    ((+1, 1, 0xAF588DF70E7AA929uL, 0x21D4BD723DBF0CBBuL), (+1, -4, 0xB8401D25E961AC8CuL, 0x6DE759F697E9B78BuL)),
                    ((+1, -2, 0xB7970C12C884121BuL, 0xBA1F7BF761085333uL), (+1, -7, 0x991F8F9FD6957802uL, 0xF47B897265271296uL)),
                    ((+1, -6, 0xE7583DB93E1CD626uL, 0xAFF79877CA8D5844uL), (+1, -11, 0x8FBC8C68C87FE3A0uL, 0xE7DDC36C23055E49uL)),
                    ((+1, -10, 0xAF6E82C06FEDF74FuL, 0x8AD6743478C0EBF9uL), (+1, -16, 0x907FD6DBE10FE33FuL, 0x9D10F9AEC5470EB6uL)),
                    ((+1, -15, 0x9441838EEA236CE8uL, 0xC93EDDD7754664AEuL), (+1, -23, 0xF3168FA8CA76101CuL, 0x4B90B57F556B64BAuL)),
                    ((+1, -22, 0xD76E661DEF19C174uL, 0x9941CAACC5FD4261uL), (-1, -72, 0xAC4C911B040DD1F6uL, 0xFCCAC37550B43629uL)),
                });
            }
        }
    }
}
