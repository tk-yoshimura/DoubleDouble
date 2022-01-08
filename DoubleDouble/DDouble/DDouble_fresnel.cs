using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble FresnelC(ddouble x) {
            if (x.Sign < 0) {
                return -FresnelC(-x);
            }
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= FresnelPade.PadeApproxMin) {
                return FresnelNearZero.FresnelC(x);
            }
            else {
                ddouble f, g;

                if (x <= FresnelPade.PadeApproxMax) {
                    (f, g) = FresnelPade.Coef(x);
                }
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return Point5;
                }

                ddouble theta = x * x / 2;
                ddouble cos = CosPI(theta), sin = SinPI(theta);

                ddouble c = Point5 + sin * f - cos * g;

                return c;
            }
        }

        public static ddouble FresnelS(ddouble x) {
            if (x.Sign < 0) {
                return -FresnelS(-x);
            }
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= FresnelPade.PadeApproxMin) {
                return FresnelNearZero.FresnelS(x);
            }
            else {
                ddouble f, g;

                if (x <= FresnelPade.PadeApproxMax) {
                    (f, g) = FresnelPade.Coef(x);
                }
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = FresnelLimit.Coef(x);
                }
                else {
                    return Point5;
                }

                ddouble theta = x * x / 2;
                ddouble cos = CosPI(theta), sin = SinPI(theta);

                ddouble s = Point5 - cos * f - sin * g;

                return s;
            }
        }

        private static class FresnelNearZero {
            public static ddouble FresnelC(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * PI;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0, u = x;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k], -4 * k);
                    (ddouble p, ddouble q) = CRcpTable.Value(k);
                    ddouble ds = f * (p - v2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= v4;
                    s = s_next;
                }

                return s;
            }

            public static ddouble FresnelS(ddouble x, int max_terms = 16) {
                if (IsZero(x)) {
                    return 0;
                }

                ddouble v = x * x * PI;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble s = 0, u = v * x / 2;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = Ldexp(u * TaylorSequence[4 * k + 1], -4 * k);
                    (ddouble p, ddouble q) = SRcpTable.Value(k);
                    ddouble ds = f * (p - v2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= v4;
                    s = s_next;
                }

                return s;
            }

            private static class CRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp(checked(8 * m + 1));
                        ddouble q = Rcp(checked(4 * (8 * m + 5) * (4 * m + 1) * (4 * m + 2)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }

            private static class SRcpTable {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble p, ddouble q) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (long m = table.Count; m <= n; m++) {
                        ddouble p = Rcp(checked(8 * m + 3));
                        ddouble q = Rcp(checked(4 * (8 * m + 7) * (4 * m + 2) * (4 * m + 3)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }
        };

        private static class FresnelLimit {

            public static (ddouble p, ddouble q) Coef(ddouble x, int max_terms = 10) {
                ddouble v = Rcp(x * x * PI);
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0, q = 0;
                ddouble a = Rcp(x * PI);
                ddouble b = Rcp(x * x * x * PI * PI);

                for (int k = 0; k < max_terms; k++) {
                    ddouble s = ((8 * k + 1) * (8 * k + 3)) * v2;
                    ddouble t = ((8 * k + 3) * (8 * k + 5)) * v2;

                    if (s > 1 || t > 1) {
                        return (NaN, NaN);
                    }

                    ddouble dp = a * (1d - s) * RSeries.Value(4 * k);
                    ddouble dq = b * (1d - t) * RSeries.Value(4 * k + 1);
                    ddouble p_next = dp + p, q_next = dq + q;

                    if (p == p_next && q == q_next) {
                        break;
                    }

                    a *= v4;
                    b *= v4;
                    p = p_next;
                    q = q_next;
                }

                return (p, q);
            }

            private static class RSeries {
                private static BigInteger v;
                private static readonly List<ddouble> table;

                static RSeries() {
                    v = 1;
                    table = new List<ddouble>() { 1 };
                }

                public static ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        v *= 2 * m - 1;
                        table.Add(v);
                    }

                    return table[n];
                }
            }
        };

        private static class FresnelPade {
            public static readonly ddouble PadeApproxMin = 0.85d, PadeApproxMax = 16d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static FresnelPade() {
                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    PadeX0p0Table,
                    PadeX0p5Table,
                    PadeX1p0Table,
                    PadeX1p5Table,
                    PadeX2p0Table,
                    PadeX2p5Table,
                    PadeX3p0Table,
                    PadeX3p5Table,
                    PadeX4p0Table
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                ddouble v = Log2(x);

                int table_index = (int)Round(v * 2);
                ddouble w = v - table_index * 0.5d;
                ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> table = PadeTables[table_index];

                (ddouble sfc, ddouble sfd, ddouble sgc, ddouble sgd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble fc, ddouble fd, ddouble gc, ddouble gd) = table[i];

                    sfc = sfc * w + fc;
                    sfd = sfd * w + fd;
                    sgc = sgc * w + gc;
                    sgd = sgd * w + gd;
                }

                ddouble f = sfc / sfd, g = sgc / sgd;

                (f, g) = (Pow2(f), Pow2(g));

                return (f, g);
            }

            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX0p0Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 0, 0xEB247991AAAE0247uL, 0x1793D3E5F5843EE4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0x80906E6EFAAFB65CuL, 0x4398BB71CB02D262uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 1, 0xBD8433263A09EA04uL, 0xC4130A5435B05340uL), (-1, 0, 0xFE9CCF1C03F4D2DCuL, 0x27478F49476EB011uL), (+1, 2, 0x9C5FF31807010C3CuL, 0x5480D38BCB70E424uL), (-1, 0, 0xD9F7731BC28AC98AuL, 0x5B8B7A27292F74E3uL)),
                ((-1, 1, 0xBA4340C1F9C5E5CEuL, 0x3A62F1B38E219ABBuL), (+1, 1, 0x8F3D43C4EE0519D7uL, 0x51BFC09B302B98A2uL), (-1, 2, 0x82FB15C52EDD1B8DuL, 0x23EB251DA80D5F3EuL), (+1, 0, 0xDCF5B66CB5EE4624uL, 0x09145383ABBC2124uL)),
                ((+1, 0, 0xFFB927B07A34F61FuL, 0x2EEBC552D5522DFEuL), (-1, 0, 0xDD5B518B421EE888uL, 0x190403EF9398C22AuL), (+1, 1, 0x83B1FC11651674BFuL, 0x24359F1C753DA6F3uL), (-1, 0, 0x9275A2D4AE909A3DuL, 0x89413916AA4BAA80uL)),
                ((-1, 0, 0x85ED80E726278623uL, 0x39BD69B115516C0CuL), (+1, -1, 0xFE7E543E4E040EF8uL, 0x054A65AB92B68F3FuL), (-1, -1, 0xD3024FF5613A8DFFuL, 0x9448D80DF8AB27CDuL), (+1, -1, 0x8F436118B219E05DuL, 0x28EC37FADCEF56BBuL)),
                ((+1, -2, 0xE5E108AD34DF2C68uL, 0x7E375C1206528286uL), (-1, -2, 0xE5392A01092712FAuL, 0xEAF00CBCD6F24ED0uL), (+1, -3, 0xEB581D313F16014AuL, 0xB3D7407F816B049AuL), (-1, -3, 0xCF9FDB23C01652AAuL, 0x45D98949E4229A70uL)),
                ((-1, -3, 0x9FB1ED4346A731B4uL, 0x92300D5824101323uL), (+1, -3, 0xA3C16A97E699E180uL, 0x5E30A820DD508D1BuL), (-1, -5, 0xE9B47B91FD65A19EuL, 0xD777F6395ADE724FuL), (+1, -5, 0xE5CEF9A29753E501uL, 0x1E50E9DC384A9E77uL)),
                ((+1, -5, 0xBE22C451DF88757DuL, 0xAF21DB7E955DA476uL), (-1, -5, 0xBEA4EF411F0E882EuL, 0xF0FD8FA69609E1EEuL), (+1, -7, 0xA32DCC005DBBF96AuL, 0xD925C3E92E48E3C8uL), (-1, -7, 0xBC143C0692D18096uL, 0x0B31AAD26AD06B1EuL)),
                ((-1, -7, 0xB2EF45168FB0ABDCuL, 0x02A6337FA99C2EE3uL), (+1, -7, 0xB35BEBD9FB209A02uL, 0x891C3608F50E54DBuL), (-1, -10, 0xD5345B438052D912uL, 0x774868815A419C79uL), (+1, -10, 0xDCB9BCA5742C8098uL, 0xDBD3385603E7F83CuL)),
                ((+1, -9, 0x8E80564C22C4F4FEuL, 0x1C720AB991EC7D9DuL), (-1, -9, 0x8AE2892FD67BDA2EuL, 0xB724A4F245E338BEuL), (+1, -14, 0xF6635F7B516E527EuL, 0x1979F7A77904BF55uL), (-1, -13, 0x94D46E811C6C538FuL, 0xE51DE31088DB8913uL)),
                ((-1, -12, 0xA62C27207FACF1FDuL, 0x770C8EF1D86FD60BuL), (+1, -12, 0xAB36DECFC1532BDCuL, 0x3B59BC9BE10EF877uL), (-1, -23, 0xDA4B26C41D8968A9uL, 0xB8645630A9E2681FuL), (-1, -19, 0xCD683415CCF0ADA5uL, 0xD51A45C8A6F81290uL)),
                ((+1, -15, 0xA7469DDE413C4D67uL, 0x18E40F960B3C568EuL), (-1, -15, 0xA5CFA6ADDE90BC9EuL, 0x3DE579848A94DFA2uL), (-1, -20, 0xC9CCCE8E1EA0BA6CuL, 0xE3ABBAE412312A78uL), (+1, -19, 0xCA28814A2849F1EDuL, 0x2BFA2F0DB4837611uL)),
                ((-1, -19, 0xE58E90A22B8F7BF8uL, 0xEF441E574A109128uL), (+1, -19, 0xE2678467D9C0643BuL, 0x5F5304832F1D3F2EuL), (+1, -21, 0xD3EDC5CE34D24EAEuL, 0x8EDB5480CE43CB12uL), (-1, -21, 0x86F52D678CCFC745uL, 0xC7A0EFAC864C9601uL)),
                ((+1, -23, 0x9CEFDBC31D4965CCuL, 0x41832A4D0A2B0BA4uL), (-1, -23, 0xA0E4871A89B66D38uL, 0xE7057720CFDFD25BuL), (-1, -26, 0x99244D50AAC657ECuL, 0x43A58DE9B6DF435DuL), (+1, -25, 0x8CB882B20045C1BCuL, 0xD7468F1A26190F26uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX0p5Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0x8E5E48AC6F2DCD0DuL, 0x4EE7404CB3BF2C32uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xA39F62E2CB5E3007uL, 0x730DC89FEC5BC4F8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -1, 0x84B51015139D69FEuL, 0x2BA925C4A31E3D0AuL), (-1, -1, 0x9D4A78D996FD816DuL, 0x7CD1C34B2C5A959CuL), (+1, 2, 0x9513502A167AB3D5uL, 0xA5F21029A998F56FuL), (-1, 0, 0xB1162F7DEEBD1A2DuL, 0x170BD8793716A6DAuL)),
                ((-1, 0, 0xC072DF8A5D3AAC32uL, 0x7B512CBDD269D1DEuL), (+1, -1, 0xDA5D72BA4D62D393uL, 0x63905949842075F1uL), (-1, 2, 0xAC9FFFBCFDA239A1uL, 0x08E4813A570CB8FDuL), (+1, 0, 0xD054D99E46387D7BuL, 0x036A6D37FE96B85CuL)),
                ((+1, -3, 0x9D8DFC48BEA78BCDuL, 0x77D011A905762363uL), (-1, -2, 0xADF6E568BD4E49EAuL, 0x55F63ED58928FD81uL), (+1, 1, 0xAAD01434E67859CCuL, 0x8AEDF781AC960094uL), (-1, 0, 0x94AA55A2AD9B82E8uL, 0xAA0E29E3E41E00A6uL)),
                ((-1, -2, 0xCF901F681BAB7167uL, 0x596D265F9AACCF87uL), (+1, -2, 0x81AD0D5FF0F814DDuL, 0xB91DB29AA0FDDAF9uL), (-1, 0, 0xCE8DB0F59D471AD2uL, 0xE06FD4DA6F6FCE21uL), (+1, -1, 0xB72246B413621EB8uL, 0x134020FA587D63EDuL)),
                ((+1, -10, 0xCA6587E48A39B099uL, 0x9813E366AF62D4EFuL), (-1, -4, 0x86A12424AAEF1DE0uL, 0x4EBA4E13A6A8D02AuL), (+1, -2, 0xF47A40778BF23FD9uL, 0xDF0ED291E6FF9911uL), (-1, -2, 0xA445A01468D61A65uL, 0x42829EC3440EB812uL)),
                ((-1, -5, 0xF16CAD817D30D757uL, 0xE8237BF3E34FA730uL), (+1, -5, 0x8BA80A43DBB49CC0uL, 0x4E6A81D4A9E04007uL), (-1, -3, 0xCF206F61B07FC052uL, 0x959EFEE1A0DFF28BuL), (+1, -3, 0x810E845597797737uL, 0x6776D9BCB00EC932uL)),
                ((-1, -8, 0x8420C701ACB626AFuL, 0x2ACB74CF85ABC9C9uL), (-1, -8, 0xAC12DE0900DC7E2DuL, 0xC3C231BCAA307435uL), (+1, -5, 0x9508A4445526A076uL, 0x7BC0568EB26E62C9uL), (-1, -5, 0x9AF6C83E2EC6E173uL, 0x774DC3AB6BAD9B9FuL)),
                ((-1, -8, 0xA03E261F2137BCB7uL, 0xA00507DE15392068uL), (+1, -9, 0x8C7A14A4E36F9BD8uL, 0xD14BA646B417CC1BuL), (-1, -7, 0xDA064ED5A6F42152uL, 0xA55D04BE1C5957F9uL), (+1, -7, 0xA46ADBEC9BA93C20uL, 0xCBD3F6B6CDD4896CuL)),
                ((-1, -11, 0x8E0AF9147F61AA0BuL, 0x2BD001406EEF4832uL), (-1, -13, 0x973F219B6238E7C1uL, 0x14C364B1FE26252FuL), (+1, -10, 0xAD1A25E9B62C71B2uL, 0x7E713F946EC350A9uL), (-1, -9, 0x8586A8A72CFB72FAuL, 0xD39F18BCDB92EE01uL)),
                ((-1, -13, 0xD527C79C7C9A033FuL, 0xC88680BE69F3B104uL), (+1, -15, 0xE3D32D9D89F3A4A8uL, 0x88085D3940BDB6ACuL), (-1, -12, 0xF645201D46314E24uL, 0x1D3102B7A179401CuL), (+1, -12, 0xBC508C3B5A313C44uL, 0xA83FFFC0F3EFA4D1uL)),
                ((-1, -16, 0xCCBF20B20D7DBB7AuL, 0x77D48C395A59832FuL), (-1, -23, 0xA1F65D7AFF6315EBuL, 0xCB1A0A45AED1D084uL), (+1, -16, 0xA4F86E654E823094uL, 0xA525B5AE2BE9F8BDuL), (-1, -15, 0xBE8279010220650DuL, 0x157FF4AC1B1961B8uL)),
                ((-1, -19, 0xB9545E22ABE290C0uL, 0x41871DAB8C628F55uL), (+1, -23, 0xEFCD49E679431934uL, 0x943E7A1746EEBFC1uL), (-1, -17, 0x8352C53F1845A87DuL, 0xAA4622FB38240921uL), (+1, -18, 0x914B92518F2BB54DuL, 0xF56C36C82E9B09DBuL)),
                ((-1, -23, 0x80B2A7FAD0147AA1uL, 0x60E0663BCC73DF0DuL), (-1, -30, 0x919A69F54D78A568uL, 0x3BBDE20813A70FACuL), (+1, -26, 0xBD3E554002EB9017uL, 0x0CE410A6DB829547uL), (-1, -23, 0xDA300BE6EAA64028uL, 0x28263A1136D573ADuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p0Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0xAB33023EAB46ED15uL, 0xEABD5B558D07CD8EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xCD2BF07EA50157A2uL, 0x8A7D3D1EC11E4840uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 0, 0xF651BF9E70C60092uL, 0x860C04EE4F44F3BEuL), (-1, 0, 0x8930F47CA09EF10CuL, 0x45E481177A9119F6uL), (+1, 2, 0x9C415977DBA6201EuL, 0xF1DD3DD83C55220BuL), (-1, 0, 0x9862299121162207uL, 0xD6F1370B894864A7uL)),
                ((-1, 1, 0xF8B72E431F054B3FuL, 0x4E557F70BA6208CAuL), (+1, 0, 0xE74AC40DA81E4F7FuL, 0x336DCDFFA2B852D9uL), (-1, 2, 0xE98A9C128021813EuL, 0x863A3028C034EC78uL), (+1, 0, 0xCE1DDB70B87E0E68uL, 0xAB3B1E9352C6E65BuL)),
                ((+1, 1, 0x888B1C13D1B0DBF2uL, 0x16AECB66805D9CD8uL), (-1, 0, 0xB294C26B8822246DuL, 0x3290410868FE863BuL), (+1, 1, 0xFE70E323A4BCDCB2uL, 0x560C0C2F64265EDFuL), (-1, 0, 0x9F8891DCC7338EDCuL, 0x8689BB32315BC3C4uL)),
                ((-1, 0, 0xEDAC6E96BA5E9BCAuL, 0x01CFD1BD9736970CuL), (+1, 0, 0x8FFB058712B1ECC9uL, 0xA488B68C630B839BuL), (-1, 1, 0xB87E6F385272213BuL, 0x2D7753B25BB186D9uL), (+1, -1, 0xE6121688F575F302uL, 0x2B82E4CFF1CAC40EuL)),
                ((+1, -1, 0xBF6AF969F8025B7AuL, 0xBE88B0F32A2D12BBuL), (-1, -1, 0x9D7C4B29C0DCE154uL, 0xAC4BA9D11863D92EuL), (+1, 0, 0x957C3627122A9063uL, 0x3E6ACE5D7CCC9860uL), (-1, -2, 0xFB6031136E02130DuL, 0x4982E58E657F66FEuL)),
                ((-1, -2, 0xD4256A0D312604E0uL, 0x9ECAA01D0540BAE0uL), (+1, -2, 0xA49EA4BDA1D0AA14uL, 0x6A58905A760AD90CuL), (-1, -2, 0xFAA5B30F1897248FuL, 0x68C6ECEEC2357A75uL), (+1, -3, 0xEC99900BCF5F6669uL, 0xA8CAF2630D2F62D3uL)),
                ((+1, -4, 0xDF0193BB7E46B2F5uL, 0xADAEA8A030C04C6DuL), (-1, -4, 0xF9A2C6454585D1FFuL, 0x27176B3A19B9BA40uL), (+1, -3, 0xA11FD0E7C83D3178uL, 0xCCF832387296D9A6uL), (-1, -4, 0xBAB8905D00A7AF78uL, 0xD8169AB73C9AE6AEuL)),
                ((-1, -5, 0xDE3F0087325FE7F0uL, 0x1C5A5C1605D6A47AuL), (+1, -5, 0xBD0D961E498C47E7uL, 0xA03240A725AAB88FuL), (-1, -5, 0x8D72499759963FB4uL, 0x539D3C07B71FEBE0uL), (+1, -6, 0xEDCDC98C493FF54FuL, 0xEEBFC2A2AA5DDAEDuL)),
                ((+1, -8, 0xBC80C5375E51A4DCuL, 0xC3188A11B8B3E8DAuL), (-1, -7, 0xB75B9D3FDD891DD2uL, 0xF88FC92C4BC1A832uL), (+1, -7, 0xB71C5D5349C08CACuL, 0x3A8016F128CE8096uL), (-1, -7, 0x852CC413929FDE15uL, 0xFB70366E2B3F441CuL)),
                ((-1, -8, 0xA39A951BE75D84CCuL, 0xFE8A9E76B81AC812uL), (+1, -9, 0xD6BF26765C51730AuL, 0xBE3B6A7178EDA78FuL), (-1, -11, 0xBF58BDE28FD7B8F2uL, 0x93464891872A4140uL), (+1, -10, 0xDCA7757388AEECCCuL, 0x1AC7EBB9F4C32913uL)),
                ((-1, -13, 0x8505CC50DAD8BE5CuL, 0x0D2D58D6D5DB80E7uL), (-1, -12, 0xDFAEE939848DD7DDuL, 0x0DB82AF34172760FuL), (+1, -11, 0x82124577E55269C4uL, 0xEA9477ED61FB28D2uL), (-1, -12, 0xA250C4DE2E6E10B0uL, 0x26AFAC2675A0C9CEuL)),
                ((-1, -12, 0x80BAE8AD21A281A9uL, 0xCF846F487F6E162DuL), (+1, -14, 0xC7A04B423B6BB25FuL, 0xE1D50966DAC473D4uL), (+1, -18, 0xE715895675A3701AuL, 0x000B14B57072A730uL), (+1, -15, 0x98F233275AA41248uL, 0xE275062976B789FEuL)),
                ((-1, -16, 0x91E0E7B81C751BECuL, 0xD978127F84E8C292uL), (-1, -18, 0xB3EBCCC6925F883BuL, 0xA120462F671A52E6uL), (+1, -17, 0x99A594C8B6625EF0uL, 0x169937998DD86BA8uL), (-1, -19, 0xDFF830DDFDAA099BuL, 0xBD205030C6858724uL)),
                ((-1, -20, 0xE612413E8C7A6778uL, 0x67D5BDEDEA0AF030uL), (+1, -22, 0xA30EDEC926D8F689uL, 0x98D59C357C819E6AuL), (+1, -23, 0xB7B9FD7C55A014ECuL, 0xD990A08A95522641uL), (+1, -23, 0x91EB71545CF2D7C2uL, 0xE469EF15D4AB5658uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p5Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0xCA1D29E286E122E1uL, 0x73EE36639DE26E2EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xFAB64DF88ADB9765uL, 0xA016FDB0E41BA470uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 1, 0xADF4CB3602AF3CE7uL, 0x780B4F73A0D82815uL), (+1, -1, 0x8CAD57A456648ABEuL, 0x341B63CE777DF487uL), (-1, 2, 0xBEE634F3E2EB1D40uL, 0x40A208595FB46638uL), (+1, -2, 0xC72C86B930B87ED7uL, 0xC617F92574F1D721uL)),
                ((-1, 1, 0xF5DB084277138BF3uL, 0x0A3916E78A7FEC15uL), (+1, 0, 0x84EB6729D10ADA57uL, 0x01468CA484A14902uL), (-1, 3, 0xB1DDA708A5519534uL, 0xB70C654EF2A7A644uL), (+1, 0, 0xA1705D1B597A3673uL, 0x39724F605B1EFFEFuL)),
                ((-1, 1, 0x9B9C87CFCDA521FAuL, 0x59B57C45728214B4uL), (+1, -2, 0xE59B5A3DB569BB7FuL, 0x1BCF5CF3E17F89B9uL), (-1, 2, 0xE8B6B655E6AFE2C1uL, 0xF15598D954CB5C94uL), (+1, -2, 0xECE6F19B7592351BuL, 0x569DD8F2536C9929uL)),
                ((-1, 0, 0xE2F9CE82E9307CE0uL, 0x9576161F9AA49128uL), (+1, -2, 0xD47758BD10BCE60CuL, 0xF290E83B4D19AEE5uL), (-1, 2, 0xD279BC2E1CCF6D1FuL, 0xA16D015DC814ECC0uL), (+1, -1, 0xA6A55E52EF250408uL, 0xEBD40FD8F601B3A2uL)),
                ((-1, -1, 0xD502C8A2ED3EE1CEuL, 0xF1C9DD950CC8806DuL), (+1, -3, 0x8B5D6DA21751298EuL, 0xC46C8B9C747CF698uL), (-1, 1, 0xE0730D45EB988155uL, 0x961EAE49421A3C29uL), (+1, -3, 0xD6AAE3BD2D3686A4uL, 0x8F3970E28ED0F36FuL)),
                ((-1, -2, 0xC897FE9B1BAD8CA5uL, 0xCFFC9C7B148A593DuL), (+1, -4, 0xA1AC1254BF6D06E6uL, 0x5B4F00A99005135CuL), (-1, 0, 0xFFE7A1E4F93B4141uL, 0x057EECF755AFA62EuL), (+1, -3, 0xAD28F14FEADAB566uL, 0x1FBDBF9FD6F455DAuL)),
                ((-1, -3, 0x890474FEA22697A1uL, 0x090BC4A907AE8C97uL), (+1, -6, 0x977EDAD3C0CDBAC1uL, 0x5B7EE23CDBBD434CuL), (-1, -1, 0xD30AD7B22BCD4AFFuL, 0x59718F4D7F00ADA9uL), (+1, -5, 0xB47E23400C578BA4uL, 0x79BE1383DB108CB2uL)),
                ((-1, -5, 0xAE6478FDC5692407uL, 0x86B84E89B1B15A46uL), (+1, -8, 0xEB81D76ED95F7C70uL, 0x4713BB16045754B8uL), (-1, -2, 0xA1F2C4A2430DB7FCuL, 0x2D7934580B9CCC69uL), (+1, -6, 0xB4C783A052BDCA3FuL, 0x11AA9B2F2BCCF0F4uL)),
                ((-1, -7, 0xA4743D91C2D2DFD2uL, 0x30C4C8D70340D0ADuL), (+1, -10, 0x8B9A83246A6117BBuL, 0x595149C6C948BAE4uL), (-1, -4, 0xC064A69392F3DE4BuL, 0x917D68ABC194EB19uL), (+1, -8, 0x873560BF9E6D07B5uL, 0xA060389312F22F88uL)),
                ((-1, -9, 0x8970BF8F6BA187EDuL, 0xA3F15EDA19C82597uL), (+1, -12, 0x92F75C62DB925BB5uL, 0xC0D31DEECE7C134DuL), (-1, -6, 0xC13640B10318C307uL, 0xF2C6A6C4D181088EuL), (+1, -10, 0xA4F82BF93FA23F0EuL, 0x2BCCD34A266C11E8uL)),
                ((-1, -12, 0xA0CF3AB5070DD882uL, 0x0B460C207425C464uL), (+1, -16, 0xA4CD484075DB6C54uL, 0x50E4A647A81C30E9uL), (-1, -8, 0x925135D95DEFF0D8uL, 0x0190CC64F365BE58uL), (+1, -13, 0x8BBCB7D95C1DC547uL, 0xA92E2A23E13545A2uL)),
                ((-1, -15, 0x9E2E5331C393068BuL, 0xAD5AEC4F2E9F4E3CuL), (+1, -19, 0xF2E440A6B8A02921uL, 0x64C751427A9352F2uL), (-1, -11, 0xA696B126F424BA62uL, 0x8CA8465375A3D78DuL), (+1, -16, 0xB2777C4572981884uL, 0x6F1C050F3BA712CCuL)),
                ((-1, -19, 0xB825DD0206458F2BuL, 0xE402324DA708755FuL), (+1, -26, 0x86BC67DB557F0241uL, 0xCE723F4BB51CEFEBuL), (-1, -15, 0xF0C99754C54BD0EDuL, 0xF3DCAAC029FFCE10uL), (+1, -21, 0x849DB445C2B32AC1uL, 0x4DCB271EAF39F323uL)),
                ((-1, -24, 0xF244749E70E40FB9uL, 0x67C08837A54648CDuL), (+1, -28, 0x856AF3F3B12BD579uL, 0x00FDE316DAFB9C59uL), (-1, -19, 0xAF46891AA59796B2uL, 0x03351C3BB8B4378CuL), (+1, -26, 0x8BE1CA8D25FED817uL, 0x25C0DAC4B84F544FuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p0Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0xE9CDD1E713635230uL, 0x4BFD14EA0A641186uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0x94FB6892E4F246C9uL, 0x88A3DBB670972913uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0x90DCE6F5B8892ADCuL, 0xB64236B25B872A34uL), (+1, 1, 0x8D2D725183695337uL, 0x5254DCC9A9ED9FDBuL), (-1, 4, 0xB68E24397540CFAAuL, 0x1E12C27E4E3A4244uL), (+1, 1, 0x8861ABDB31734794uL, 0x5DB61AEA174D0473uL)),
                ((-1, 3, 0xD7880D8ED5AE7D6DuL, 0xD92E2BC3FC9D405FuL), (+1, 1, 0xC56ABF466550E1A1uL, 0xFCD4FED3BF021F24uL), (-1, 5, 0x8AB417033FCF8586uL, 0x9C5D14EA885A10E5uL), (+1, 1, 0xC285D9B8AE115206uL, 0x0510667676ED9063uL)),
                ((-1, 3, 0xDF009E0099CDDBC2uL, 0xF092CBF3AFA161A1uL), (+1, 1, 0xBE3CF59C5842B102uL, 0x1761EA415C24AAD6uL), (-1, 5, 0x9324692324E014C1uL, 0x8CA6825F5152C755uL), (+1, 1, 0xBE6027372D77964CuL, 0x23DAC88771A44DA2uL)),
                ((-1, 3, 0xB006EFFA09B4DC0AuL, 0xE2F6E5B1F4A7E303uL), (+1, 1, 0x8CB7F1C172A9A93CuL, 0x5119975872D4103CuL), (-1, 4, 0xF036749691AC9381uL, 0x554EF98055FA79F2uL), (+1, 1, 0x9126DCAF368BDC5DuL, 0xA034893793D3A5EBuL)),
                ((-1, 2, 0xDA99C99DAC666206uL, 0x4FC744A7FFAEA331uL), (+1, 0, 0xA266ED445EB8E87FuL, 0x28ABF5E850A730B6uL), (-1, 4, 0x9AFA7EF9AF642A60uL, 0x1F0E84067A41BD64uL), (+1, 0, 0xACF5446514478773uL, 0x5E125C44C9FB3494uL)),
                ((-1, 1, 0xD98D79718D5E138EuL, 0x710D2DF26DBAAB83uL), (+1, -1, 0x955A557568041D46uL, 0x767DA84C214503A3uL), (-1, 3, 0xA12478955ED6573EuL, 0x80869375B10D990EuL), (+1, -1, 0xA59081E9BAC782B8uL, 0x7B864EE4E66B62ACuL)),
                ((-1, 0, 0xADE9C3675D11E1EAuL, 0x4CBD39DB26707BD5uL), (+1, -3, 0xD96FCAF6E817CA64uL, 0x4E7D29E74DD3B5F6uL), (-1, 2, 0x873F6A603E3D4C33uL, 0xBB26D436EF01E326uL), (+1, -3, 0xFBB548327D31ED43uL, 0x1C1FCC31B194035AuL)),
                ((-1, -2, 0xDDF19397A7C8B74AuL, 0x92B1D19404CF6716uL), (+1, -5, 0xF808487867A384B3uL, 0x8290E5D1DFD2E9F4uL), (-1, 0, 0xB6217A5063325E92uL, 0x76DD203750CE5A33uL), (+1, -4, 0x96DCF44C1B739AC7uL, 0xD80E9CF4375BE82EuL)),
                ((-1, -4, 0xDDEA1A2B93CD768AuL, 0x3AD667D6F78932B8uL), (+1, -7, 0xD68631BF1F6E32B2uL, 0x510E04AA7F69E22AuL), (-1, -2, 0xC12B2713096C1898uL, 0x76A4437906046A47uL), (+1, -6, 0x89AD33F61148216FuL, 0x1905520F747032D0uL)),
                ((-1, -6, 0xA829CB628E294671uL, 0xD6C8FFBAA48C62A4uL), (+1, -9, 0x85629B3D1B24447BuL, 0x1E73E84804DC7823uL), (-1, -4, 0x9C1C30E5A25C231DuL, 0x560F55934C75157CuL), (+1, -9, 0xB5B3D0A68F852C80uL, 0x9110FDC5A5762B13uL)),
                ((-1, -9, 0xB5F75AFE8DE7DCC2uL, 0xCB6C6C101A26DC53uL), (+1, -13, 0xD4FFC10EAD020788uL, 0x9656A35BE51486DCuL), (-1, -7, 0xB53A8A6D7ADDD69DuL, 0xAB7202B7904821BBuL), (+1, -12, 0x9AC28807C7FE0701uL, 0xFD35F59D0A259768uL)),
                ((-1, -13, 0xFAC7862789CC71FFuL, 0x3CE8F91C64521046uL), (+1, -17, 0xA547ABFB4B21BA15uL, 0x539541409EA48FA4uL), (-1, -10, 0x86D62A1514EAFF5EuL, 0xF855291B90110B64uL), (+1, -16, 0x80DB478732089D95uL, 0xB907233B728DC470uL)),
                ((-1, -17, 0xA532287470D16573uL, 0xD8BC1AA649394AFBuL), (-1, -33, 0xA71F9C8F18C45219uL, 0xF4DF6971FBBB15F7uL), (-1, -15, 0xC134C9BEF3378E12uL, 0x39A474310100A607uL), (-1, -33, 0x9140CB6D3D56742BuL, 0xA906AED5F0903A12uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p5Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0x84DC8D90D01D1762uL, 0x334D5A2EFCAF216BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xACE1C7831EBF2F25uL, 0x1B429D54E912B22EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 4, 0x80C163D9124F8F9CuL, 0x625EFB89E0DFA04EuL), (+1, 1, 0xE8B121BE6911A287uL, 0xC6549C4D821628ECuL), (-1, 5, 0x8FE2904990A59FE2uL, 0xD2D5C2955850CA22uL), (+1, 1, 0xC353CED35D911ADEuL, 0xE4BD3BE507D287B1uL)),
                ((-1, 4, 0xF7F428486CFA33C8uL, 0x2CCD5ED1F00269ECuL), (+1, 2, 0xD2E1142008A7C21FuL, 0x18EB3B724D79B92CuL), (-1, 5, 0xF8E422E80C9F9FF8uL, 0x647D264D53A00D5FuL), (+1, 2, 0x9D30910AA6C07908uL, 0x6887EAF24EEF8638uL)),
                ((-1, 5, 0x99BF52F5CBD8AB8BuL, 0xF215F6AFB441886FuL), (+1, 2, 0xF57A6F2FE174A79BuL, 0x4D51634534F7FAE7uL), (-1, 6, 0x8E5B521852982504uL, 0xDDF0A34F4E702E33uL), (+1, 2, 0xA730A261B6F6EAF1uL, 0x06F7CD720F272CAAuL)),
                ((-1, 5, 0x87EC566377E9AD80uL, 0x1AB4330F04BF5677uL), (+1, 2, 0xCACBDAE5B293A404uL, 0xE8EA4712B5CED485uL), (-1, 5, 0xECBECAF7DBA1B6BFuL, 0x4A0A9EEABA1A5B11uL), (+1, 2, 0x80E3AB0E2F0330F8uL, 0xBA6B6769CEE9AF46uL)),
                ((-1, 4, 0xB3E90929E5D5396EuL, 0xB7E723577761F205uL), (+1, 1, 0xF8FE00E1AE277EF6uL, 0x71979431921DD43DuL), (-1, 5, 0x95950E4540464031uL, 0x3CECC935AE242F7AuL), (+1, 1, 0x95F4FD5C43EAB11EuL, 0x3A8BE9F3238D99C9uL)),
                ((-1, 3, 0xB6D88686E00EC111uL, 0xF40176E8B6E21831uL), (+1, 0, 0xE864B8D6D94D969BuL, 0xB5F7613A61C6D55FuL), (-1, 4, 0x92E43959AEAD207AuL, 0xD16430E4D9949997uL), (+1, 0, 0x86450F3BBC010ED5uL, 0xC6E421211FA5148EuL)),
                ((-1, 2, 0x904CE37C5C53567EuL, 0x80C6CEA602FDD222uL), (+1, -1, 0xA61CE0C7F913B1BBuL, 0x2DE2328DB4FB041CuL), (-1, 2, 0xE23B21CFF48EFA19uL, 0x4B510F6F80E166D1uL), (+1, -2, 0xB9EAF1EF037A9DD8uL, 0xCD0F190C4E4FDA79uL)),
                ((-1, 0, 0xB111E30729637549uL, 0x44722281F51AF5AEuL), (+1, -3, 0xB52A53A60B90144AuL, 0xEF20267981F1AE70uL), (-1, 1, 0x8885F0401AFE353AuL, 0xA7A15A361D1EDAC6uL), (+1, -4, 0xC5E1CF5D1F59F04DuL, 0xDA7BED46CAE22281uL)),
                ((-1, -2, 0xA7717BA746F24DD3uL, 0xB0DA0DF1B3B38215uL), (+1, -5, 0x941CB68529142A79uL, 0x934BF1DA99D0B538uL), (-1, -2, 0xFF92E54980FB0B3CuL, 0x94866D3CC147343FuL), (+1, -6, 0x9EB77CD8A4C9FCF7uL, 0xD2F16E9DFB3ED829uL)),
                ((-1, -5, 0xEEA60581333C1CBFuL, 0x4E6DAF9C9EE9AEEDuL), (+1, -8, 0xAE766AFDCCC8589BuL, 0x39A418FAE3704077uL), (-1, -4, 0xB52D7A28792E6A08uL, 0xC7791103157895B3uL), (+1, -9, 0xB80FB9AB9674EEC9uL, 0x9AC4043E6EE55D36uL)),
                ((-1, -8, 0xF478F1958D9788E4uL, 0x6DA66E498DFE9614uL), (+1, -11, 0x86EA9B165670252AuL, 0x7A3D376C334A168BuL), (-1, -7, 0xB9721ADDAD81F835uL, 0x9EC16349A8D6F219uL), (+1, -12, 0x8C6B4691DC190BF0uL, 0x738CCAEB1ECA5135uL)),
                ((-1, -11, 0xA230400047BA1C4CuL, 0xB9AAEAB0AEE41755uL), (+1, -16, 0xD22D5EDD97A0B8C4uL, 0xC56B63ADF18021FEuL), (-1, -11, 0xF728C820D130C828uL, 0x25907509551B518DuL), (+1, -17, 0xD855CBBED2E0D695uL, 0x035FF9A698530604uL)),
                ((-1, -16, 0xD22843CCB775D569uL, 0xC5D93D2A0F994847uL), (-1, -34, 0xD3A414471D4D25E3uL, 0x178D4C2ED0FD44F2uL), (-1, -15, 0xA239364F63DE0CEDuL, 0x93CD55F288FCBC6FuL), (-1, -34, 0xC9BC7C66FA48DA81uL, 0xB5C57F4280B2577EuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p0Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0x94D9EEC040E1622FuL, 0xC85B042632F671D8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xC4DB3EF50EFCA8E6uL, 0xD324A0106BD1135CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0xD73BD930FAFBF398uL, 0x7A68C06E01465DFFuL), (+1, 1, 0xAB54338FD8437551uL, 0xD2C2F52126725D18uL), (-1, 5, 0x8CA81AC42D2FC2FBuL, 0x59DB4BE729A2C98FuL), (+1, 1, 0xA751672C25A43379uL, 0xE5951E8CBFF9DA63uL)),
                ((-1, 4, 0x95D6539E6EE13F1CuL, 0x7840EF578A213C40uL), (+1, 1, 0xDCDE602610883E84uL, 0xA44899469DB2CDB6uL), (-1, 5, 0xC2C2CCADF634A606uL, 0x7F9F13721DFBE2FAuL), (+1, 1, 0xD47CA2818DEDC467uL, 0xE28F62775175DD15uL)),
                ((-1, 4, 0x832420A90D292D33uL, 0x5B9F9293F0EE7CC7uL), (+1, 1, 0xB21032FA8A47ECDFuL, 0x5D3E288150F7BA43uL), (-1, 5, 0xA9D9D2317FB71942uL, 0x9F9AE25A9087E8D3uL), (+1, 1, 0xA913C2C91BBE9DE1uL, 0x87FCDA7B31E59BAFuL)),
                ((-1, 3, 0x9FE4ECA177EADA6CuL, 0xCD71116323C9EEA6uL), (+1, 0, 0xC66FBE99A35FC46FuL, 0x32CFF76D7665A47CuL), (-1, 4, 0xCE4368F41B50DC4CuL, 0xF0FFCF959FF3BEB1uL), (+1, 0, 0xB9C9AD6D3CE15717uL, 0x5D75E17F54175256uL)),
                ((-1, 2, 0x8EDA4C6D068CC436uL, 0x1CF06F31925BAB1EuL), (+1, -1, 0xA05E18901D3A22B5uL, 0x9BDD3B81FEB4CB20uL), (-1, 3, 0xB70E052C1B30CB4CuL, 0xCCF93732A622EE93uL), (+1, -1, 0x9374C36F5821D051uL, 0x3D0F4783527B330BuL)),
                ((-1, 0, 0xC01A67BF2611262FuL, 0x46BD4F2AF5813745uL), (+1, -3, 0xC07CCECF6A8A969FuL, 0x3B8BB06BC67182D5uL), (-1, 1, 0xF35C6106D0AE7DF4uL, 0xCA5A0B7366E0DA02uL), (+1, -3, 0xACAA51401D3BC322uL, 0xC160D9744DD3550EuL)),
                ((-1, -2, 0xC4AEAA3E1860BD7BuL, 0x9A859B1FEBDE90C8uL), (+1, -5, 0xACBDECB3020FBF2AuL, 0x7DBF251961D50382uL), (-1, -1, 0xF49CFC4B507F61D4uL, 0x8CB0A92D80FD5FC2uL), (+1, -5, 0x95B4B3C4AA8E3753uL, 0x181A606DA85AF2E8uL)),
                ((-1, -4, 0x990E08A3007484E8uL, 0xA103FF03DD39534FuL), (+1, -8, 0xE5602AF80B010627uL, 0x805146B874B150D1uL), (-1, -3, 0xB923D50627DD608EuL, 0xB2DA35512D9AD601uL), (+1, -8, 0xBD82320CBD017C91uL, 0xDBCC10F1AAB31E93uL)),
                ((-1, -7, 0xB1DA0DC4870FE112uL, 0xD681618C832D0392uL), (+1, -11, 0xD945190D52FB68B7uL, 0x28D8623D72A780BCuL), (-1, -6, 0xCEAC551521089AD1uL, 0xA54004A8FD98F61AuL), (+1, -11, 0xA7E06DD67786DA8AuL, 0xE37A62693CC3A005uL)),
                ((-1, -10, 0x93BE23CD24DAEEE1uL, 0x77190AF6EC3247C0uL), (+1, -14, 0x8684AC66EB112B59uL, 0xDFB3842C9621F842uL), (-1, -9, 0xA254AF1D50FA14A0uL, 0x662BD3C470A2E378uL), (+1, -15, 0xBD7558B30DCFC222uL, 0x28BB65E45E88707CuL)),
                ((-1, -14, 0x9F2E4CD9A889FA2CuL, 0x9E19C286B112284DuL), (+1, -19, 0xA9AE0326A1EC307EuL, 0x8638D23FD4AAE9EAuL), (-1, -13, 0xA28FC44037A93471uL, 0x8E6667A55264F33BuL), (+1, -20, 0xD5046BBD2EDC49D9uL, 0x828D24533A6E736FuL)),
                ((-1, -19, 0xA9AF9A7CA293748CuL, 0xF674000485B105CBuL), (+1, -39, 0xDBF6A5FD4750EC5EuL, 0x1F04A5A0B9BFE4BEuL), (-1, -18, 0x9FC9145B8DAF722AuL, 0xD2DC8E89D561B57BuL), (+1, -37, 0x8C13177EAAA712B0uL, 0x4C2C0C13122F5E2AuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p5Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0xA4D94687DCC157F8uL, 0xCE6B36D28B9C071EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xDCD99AB0FBC1D2F7uL, 0x3C026BEF232E7F16uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0xA387940DD35EB0D3uL, 0xE1E276257A7607C9uL), (+1, 0, 0xE51B36164173D2DEuL, 0x4B9FC941D1BCBBCAuL), (-1, 5, 0x8393D0C450205972uL, 0xCB6D512CD499FD82uL), (+1, 1, 0x8A9C5CB4E7A0A57BuL, 0x79CB9EB34D184E69uL)),
                ((-1, 3, 0x9500D49D17B424E6uL, 0x36C1645FFE2D37C3uL), (+1, 0, 0xBAEB8FCAA2837A53uL, 0xEF840FDDFA0418C1uL), (-1, 5, 0x9347075DE8800C33uL, 0xA64A21D3ED084B42uL), (+1, 1, 0x8C97DAF5CCE6670BuL, 0xB3E8E7707470C223uL)),
                ((-1, 2, 0xA222F33B8BD544A7uL, 0xABE80275CD7F366AuL), (+1, -1, 0xB3385CE9AB6FB4E8uL, 0x77108A8215159802uL), (-1, 4, 0xCBEECD028570B130uL, 0x27E1CC4B8B6B1CC4uL), (+1, 0, 0xAF473EC18AD95C77uL, 0xFFF76DADCA3BB80DuL)),
                ((-1, 0, 0xE60F13384DAA56EBuL, 0xF3D46D91A330277FuL), (+1, -3, 0xDA1C0EDE0C8B521FuL, 0xEE8B9B147B8F012FuL), (-1, 3, 0xC267E323CEF869BAuL, 0xDE560FEA9EE75749uL), (+1, -1, 0x952862410BB2E409uL, 0x8612CE1A49147203uL)),
                ((-1, -2, 0xD7745DA99A7469CDuL, 0x3F936F169F8A77C8uL), (+1, -5, 0xA53BB6701CF67D7CuL, 0x5031789AA4803D50uL), (-1, 2, 0x867D74B342BF6430uL, 0xA031367B038E100AuL), (+1, -3, 0xB61E85B768F46813uL, 0xEFB8A331856F4760uL)),
                ((-1, -5, 0xF33720489C7429BAuL, 0xC90CDD0CFAFD6411uL), (+1, -9, 0xF233BC4119A8649CuL, 0x83BB644DDD1B3C4DuL), (-1, 0, 0x8A80645B08359699uL, 0xB6EEFFBB7E9DFC2BuL), (+1, -5, 0xA2C351EC93459BFFuL, 0xB789789D4BA2C4FAuL)),
                ((-1, -9, 0x9BF02A1964796C59uL, 0x58BC5C72B6117E8CuL), (-1, -12, 0x85F4E0A8A1B63E19uL, 0x9F9F3F61D5E21E46uL), (-1, -3, 0xD58729B6C1D768C7uL, 0x213DD3DE49783E78uL), (+1, -8, 0xD4068C4AC631E0A6uL, 0x08F845A13D6BADC1uL)),
                ((+1, -11, 0xF382B3F5A716ACD8uL, 0xD1F90EDD9558DE59uL), (-1, -13, 0x891378C1F165B9DFuL, 0x0E461F7BCC070D9CuL), (-1, -6, 0xF2E8C36935EA5663uL, 0x3E26A1A218F6B8CDuL), (+1, -11, 0xC27C44671CDFE589uL, 0x9AC7E872AD0A91DEuL)),
                ((+1, -13, 0xEA448CE9AFB51E7AuL, 0xA7FD228799B656E1uL), (-1, -16, 0x96EDCFF35456685DuL, 0x239F9562610AF15EuL), (-1, -9, 0xC34C20AE84E5ABD5uL, 0xD8AC19B78BA1D375uL), (+1, -15, 0xE534986E01C6013DuL, 0x2F6DCFE80ADD657FuL)),
                ((+1, -16, 0xC4349F2E85EDAAAEuL, 0x760B534723EB6714uL), (-1, -20, 0x8CA1F4B868FF7ADCuL, 0x48F870A8135C9783uL), (-1, -13, 0xC8729CFCB9291CFDuL, 0x1D7FADC86C94DBD6uL), (+1, -19, 0x8459B749D3F5894BuL, 0xDE0028CB70503004uL)),
                ((+1, -20, 0x8CA2DC5109D5C116uL, 0x2556404406C64A00uL), (-1, -41, 0xD5365468C47186CCuL, 0xB5ACF186E1C88C90uL), (-1, -18, 0xC6873CFABA617886uL, 0x0152A19B00FD52EBuL), (+1, -40, 0x882559402219C155uL, 0x43E14203456BE3D5uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4p0Table
                = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0xB4D91C716B45A8C1uL, 0x6F91AA18FD07041EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xF4D9317D7367A9B7uL, 0x70EC4FA119032902uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0xA917CF18B31C4F52uL, 0x29930A86E1663305uL), (+1, 0, 0xD8B62D938EB154DCuL, 0x2E751A837F49B072uL), (-1, 4, 0xFA8F585D8A9B1BB8uL, 0xDF3770F78CD7DE7EuL), (+1, 0, 0xECE11DC59C1224ABuL, 0xF1B584DFFE1E8748uL)),
                ((-1, 3, 0x9973E73087DC4FF5uL, 0x931F03ACB0B8EB4BuL), (+1, 0, 0xB2DFEE9B8F06F969uL, 0x431B4235110FC124uL), (-1, 4, 0xF2E328642EFD14EBuL, 0x0EB95D33096723A4uL), (+1, 0, 0xCF8320A99707624BuL, 0xAB6E6E90665A562DuL)),
                ((-1, 2, 0xB004134D4F08999CuL, 0x9A15D39D9EEC76ACuL), (+1, -1, 0xB9DBC24B9D5991E5uL, 0xB749D8791EA0C82FuL), (-1, 4, 0x91E199F71415C5AAuL, 0xF4A62F88746DBC78uL), (+1, -1, 0xDFB0A853557BCEDCuL, 0x4864345C80A4500EuL)),
                ((-1, 1, 0x8C6AC17A7AD37DAEuL, 0x1BB063F1041541EDuL), (+1, -2, 0x84FEA495C7548013uL, 0x3DC615FE217B3D89uL), (-1, 2, 0xF0488B92DD1FBC35uL, 0x9FCB35A9654121B7uL), (+1, -2, 0xA385ECB81006287CuL, 0xC40BEFF91147319AuL)),
                ((-1, -1, 0xA27D8C060B7AC52AuL, 0xE62C71A310D9E1B0uL), (+1, -4, 0x87E22AB82C0F5FE6uL, 0x0C97D665DB073C16uL), (-1, 1, 0x8E25736470359D83uL, 0xCECE4B23A48960E4uL), (+1, -4, 0xA903531074590E62uL, 0x3EC89F3EFFE45149uL)),
                ((-1, -3, 0x8A4FE37AA246C75EuL, 0xA25DD11BE7E679FFuL), (+1, -7, 0xC739D59D3CDAE686uL, 0x4585260E70DAA044uL), (-1, -2, 0xF5E1F26D42C8E630uL, 0xC134837E7D36F1B6uL), (+1, -7, 0xF918D129ED6409F7uL, 0x1DC1EB02897264A3uL)),
                ((-1, -6, 0xABA10635EF79F5D0uL, 0x0C042DDC56F68F07uL), (+1, -10, 0xCBE2B26653713834uL, 0x2C62427F39AA44D7uL), (-1, -4, 0x9A6EB67F73BF6770uL, 0x2AA93DA61DEEED93uL), (+1, -10, 0xFF33CB4D4FB068E2uL, 0x633691A4EEA2EA3EuL)),
                ((-1, -9, 0x9549DDE9CA137A66uL, 0xA88F4E7B1F6156B5uL), (+1, -13, 0x860A66E67ADBC1ACuL, 0xB7538CEACB7510B0uL), (-1, -7, 0x87BEEC42A98F9D45uL, 0xEEF2D095398A6CA0uL), (+1, -13, 0xA7798277D60A2CC5uL, 0x7E12002464988BC6uL)),
                ((-1, -13, 0xA5301D81149ADA7EuL, 0xF85341EAB444558CuL), (+1, -18, 0xB05D4D607F66DA07uL, 0x23B462E44A1CD8E8uL), (-1, -11, 0x97D202191497F801uL, 0x96C6F22F62C44AB2uL), (+1, -18, 0xDB451DB5550DB5B9uL, 0xD0BEFA5C964BF885uL)),
                ((-1, -18, 0xB05D83DEB5C4373BuL, 0xFECF972096D20733uL), (+1, -41, 0xD79E9434A3E5AA21uL, 0xADDE5018A8BACD04uL), (-1, -16, 0xA47422E338CCC7EEuL, 0xF2D67C50E3A320D3uL), (+1, -40, 0xD1F6E12051609322uL, 0xA2AF092A601AF0A2uL)),
            });
        }
    }
}