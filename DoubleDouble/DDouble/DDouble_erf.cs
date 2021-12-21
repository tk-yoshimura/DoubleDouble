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

            return RoundMantissa(y, Consts.Erf.Precision);
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
            if (x < Consts.Erf.ApproxMin) {
                return 1d - Erf(x);
            }

            if (x < Consts.Erfc.PadeApproxThreshold) {
                ReadOnlyCollection<(ddouble c, ddouble d)> table = (x < 1d) ? Consts.Erfc.PadeX1Table : Consts.Erfc.PadeX2Table;

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    sc = sc * x + table[i].c;
                    sd = sd * x + table[i].d;
                }
                sc = sc * x + 1d;
                sd = sd * x + 1d;

                ddouble y = Exp(-x * x) * sd / sc;

                return RoundMantissa(y, Consts.Erfc.Precision);
            }

            int table_index = (int)Round((x - Consts.Erfc.PadeApproxThreshold) / Consts.Erfc.TableBin);
            if (table_index < Consts.Erfc.TaylorTables.Count) {
                ddouble w = x - (Consts.Erfc.PadeApproxThreshold + Consts.Erfc.TableBin * table_index);
                ReadOnlyCollection<ddouble> table = Consts.Erfc.TaylorTables[table_index];

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * w + table[i];
                }

                ddouble y = Exp(-x * x) / s;

                return RoundMantissa(y, Consts.Erfc.Precision);
            }
            else {
                ddouble w = x * x;

                ddouble c = x * ddouble.Exp(-w) / ddouble.Sqrt(ddouble.PI);

                ddouble f = (ddouble.Sqrt(25d + w * (440d + w * (488d + w * 16d * (10d + w)))) - 5d + w * 4d * (1d + w)) / (20d + w * 8d);

                int n = 7;
                for (int k = 4 * n - 3; k >= 1; k -= 4) {
                    ddouble c0 = (k + 2) * f;
                    ddouble c1 = w * (2 * f + (k + 3));
                    ddouble d0 = (k + 1) * (k + 3) + (4 * k + 6) * f;
                    ddouble d1 = 2 * c1;

                    f = w + k * (c0 + c1) / (d0 + d1);
                }

                ddouble y = c / f;

                return RoundMantissa(y, Consts.Erfc.Precision);
            }
        }

        private static partial class Consts {
            public static class Erf {
                public static readonly ddouble ApproxMin = 0.5d;
                public static readonly ddouble C = 2 * Rcp(Sqrt(PI));
                public const int Precision = 99;

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
                public static readonly ddouble PadeApproxThreshold = 2d;
                public static readonly ddouble TaylorApproxThreshold = 6d;
                public static readonly ddouble TableBin = 0.25d;
                public const int Precision = 99;
                public static readonly ReadOnlyCollection<ReadOnlyCollection<ddouble>> TaylorTables;

                static Erfc() {
                    TaylorTables = Array.AsReadOnly(new ReadOnlyCollection<ddouble>[] {
                        TaylorX2p00Table,
                        TaylorX2p25Table,
                        TaylorX2p50Table,
                        TaylorX2p75Table,
                        TaylorX3p00Table,
                        TaylorX3p25Table,
                        TaylorX3p50Table,
                        TaylorX3p75Table,
                        TaylorX4p00Table,
                        TaylorX4p25Table,
                        TaylorX4p50Table,
                        TaylorX4p75Table,
                        TaylorX5p00Table,
                        TaylorX5p25Table,
                        TaylorX5p50Table,
                        TaylorX5p75Table,
                        TaylorX6p00Table,
                        TaylorX6p25Table,
                        TaylorX6p50Table,
                        TaylorX6p75Table,
                        TaylorX7p00Table,
                        TaylorX7p25Table,
                        TaylorX7p50Table,
                        TaylorX7p75Table,
                        TaylorX8p00Table
                    });
                }

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1Table = new(new (ddouble c, ddouble d)[]{
                    ((+1, 1, 0xE09E7D96DD9FEB32uL, 0xA22321F77F8572C0uL),   (+1, 1, 0x98672055D33236EBuL, 0xE938DCB7C986111EuL)),
                    ((+1, 2, 0xBB2500535F04CCBBuL, 0x82C213645E0B8A33uL),   (+1, 1, 0xB8D563DE82FEA9E7uL, 0x0846DA5153FB09C0uL)),
                    ((+1, 2, 0xC49A4BC65EEFEADAuL, 0x206C2CCC38BD9174uL),   (+1, 1, 0x93572299865DBC8CuL, 0xC494BE050AB4016DuL)),
                    ((+1, 2, 0x91CBF74C08740806uL, 0x3E78F4D41BA1F946uL),   (+1, 0, 0xAA746C3173F82003uL, 0x9563F030CE777350uL)),
                    ((+1, 1, 0xA1EC0FF36CCB5C57uL, 0xB9DDB36C22532C19uL),   (+1, -1, 0x965D9F93F6BD3034uL, 0x495243CABB2E6BF3uL)),
                    ((+1, 0, 0x8B5AB6E241E5BF0EuL, 0x03F118E478FAA1BEuL),   (+1, -3, 0xCFE8A524C0D591AFuL, 0xEE479AF8641F5F18uL)),
                    ((+1, -2, 0xBDB0E5CA7C2D7C80uL, 0xA354B701F56E4EA5uL),  (+1, -5, 0xE49FDDFC97619D96uL, 0x1A89999A94370992uL)),
                    ((+1, -4, 0xCE77E37D8D4218FEuL, 0x877244016AFE9C01uL),  (+1, -7, 0xC91CDFEA60DE4B66uL, 0xEC9E70AB51CA6379uL)),
                    ((+1, -6, 0xB4635E28A1EC1A6EuL, 0x6016719F8DA79BCCuL),  (+1, -9, 0x8D4BC47A2FB3B00DuL, 0xA164657A7EB1A721uL)),
                    ((+1, -9, 0xFC4F7BBF548894C7uL, 0xEAB19F1F542D9990uL),  (+1, -12, 0x9D1C30BE3D89AA48uL, 0xEB829E4F2CDF248FuL)),
                    ((+1, -11, 0x8BD7EF10BED35114uL, 0x3899CA38BF3258F3uL), (+1, -15, 0x87AB2101F7D1C763uL, 0x50AD5DE6C9ABAE52uL)),
                    ((+1, -15, 0xF106E87C2EEA273EuL, 0xC9C0C77837D2D344uL), (+1, -19, 0xB014AF7E0BAAC29BuL, 0x85AD34AFE5BDC955uL)),
                    ((+1, -18, 0x9C3621C3844947AAuL, 0x176F0E4388E9081EuL), (+1, -23, 0xA21E80914527CC55uL, 0x299606696EDE7E9AuL)),
                    ((+1, -22, 0x8FB85C983BC72AEBuL, 0x4F8D81E4F29CB410uL), (+1, -28, 0xBD63FAC280EC092FuL, 0xF6B774F50C2C18D8uL)),
                    ((+1, -27, 0xA7D7CE687A29CC19uL, 0x68857940B0B9AA53uL), (+1, -34, 0xD3B1879C8393ABAFuL, 0x502C2F47E67F6103uL)),
                    ((+1, -33, 0xBB9BC4DC15541413uL, 0x421A7513990B3A53uL), (+1, -63, 0x9308754AF94B61BFuL, 0x58BD4854796B1790uL))
                });

                public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table = new(new (ddouble c, ddouble d)[]{
                    ((+1, 1, 0xFB20CC1A6FFF5E70uL, 0x54C3A883B60B5CEBuL),   (+1, 1, 0xB2E96ED96591AA29uL, 0x9BD96344000BFB49uL)),
                    ((+1, 2, 0xEC6DC14D71189BCAuL, 0xB71A7C54B03314A4uL),   (+1, 1, 0xFD7D5EE2DEA86127uL, 0x93B315A740384CE1uL)),
                    ((+1, 3, 0x8E0E06545942626FuL, 0x0C0005DC0F55115BuL),   (+1, 1, 0xEDA3FC03CCBCCDA0uL, 0x618FA627A232D5B4uL)),
                    ((+1, 2, 0xF46B18682867ED47uL, 0xA0D24FAA80C1ED18uL),   (+1, 1, 0xA39D9DA18D58327CuL, 0xB68E36FBA3411DB7uL)),
                    ((+1, 2, 0xA00E829B92699597uL, 0x9DD9109DD0A314A0uL),   (+1, 0, 0xAEACC1BD7AEA9C2EuL, 0x5AC895556BD2AEBCuL)),
                    ((+1, 1, 0xA59B2B9C34EECC5DuL, 0xA43BED2CEEA09383uL),   (+1, -1, 0x95477D5343D11637uL, 0x61BB42D0B5828768uL)),
                    ((+1, 0, 0x8AA71EAECF515942uL, 0xC7AC8AC083E95570uL),   (+1, -3, 0xD05F259898E1F1DFuL, 0xA4D09FAE92A10DEFuL)),
                    ((+1, -2, 0xBEDC65D233D9E624uL, 0x1D71EEC3AE0C2C5BuL),  (+1, -5, 0xF0832ADE17744746uL, 0x2BC087DFAA1EE1EAuL)),
                    ((+1, -4, 0xDA2C8B46ABC25BC8uL, 0xF8E49EAFEA2CD44AuL),  (+1, -7, 0xE736E5BB2E550116uL, 0x0D306969BAA483A6uL)),
                    ((+1, -6, 0xD04D177D772D18B7uL, 0x845CE0807D6665D2uL),  (+1, -9, 0xB9AD7A0A95671B92uL, 0xC026C9E932B793C2uL)),
                    ((+1, -8, 0xA67396656D49672FuL, 0x10D26E544FCDCEA3uL),  (+1, -12, 0xF8E8CE639C5D966CuL, 0x9D7C77EEEDB12424uL)),
                    ((+1, -11, 0xDE566DBFD738E1E4uL, 0xCDBE1EB0C320A163uL), (+1, -14, 0x8A9931E3B48D865BuL, 0x468BD8B4C6ABB515uL)),
                    ((+1, -14, 0xF6F63E7F423C9569uL, 0xC23A2A4ED44D5A4EuL), (+1, -18, 0xFE1C50714426385FuL, 0x74BE5225C4439883uL)),
                    ((+1, -17, 0xE1F7EF0AA26EB07BuL, 0x0C504635E928DE73uL), (+1, -21, 0xBCEAE529E25A237AuL, 0x465B5594440EA1DBuL)),
                    ((+1, -20, 0xA7C537496E86E6A4uL, 0x7EADCE21CB2C9BEDuL), (+1, -25, 0xDE987F086BF360D1uL, 0xFC8375C161C49796uL)),
                    ((+1, -24, 0xC57EB17B8B6CA623uL, 0x21B6675C9F5D2E72uL), (+1, -29, 0xC86BE70581EAC12EuL, 0xE6FF65CBDCAD8C57uL)),
                    ((+1, -28, 0xB1B65995B6E1CBBBuL, 0xA29C65A8871A5188uL), (+1, -33, 0x81CD6254A1B2FC8BuL, 0x0C810A52986B331CuL)),
                    ((+1, -33, 0xE61B257EAC0166D9uL, 0x9DC5122FFC8D7FBFuL), (+1, -39, 0xD7CB517521282570uL, 0x2A39C3E23ACC5B0BuL)),
                    ((+1, -38, 0xBF3E1DB37A2F6175uL, 0x8E58C0877CBD5841uL), (+1, -45, 0xAD421C5B0D9A9BDFuL, 0x6BDBD97C1DD70EDEuL)),
                    ((+1, -44, 0x998BCEA6FF99A650uL, 0xABC325DAB0F91D8BuL), (+1, -82, 0x9ED868BF238FA11FuL, 0x2711DA40F1A6BE27uL))
                });

                public static ReadOnlyCollection<ddouble> TaylorX2p00Table = new(new ddouble[] {
                    (+1, 1, 0xFA97703BE40EAC7DuL, 0x247AB845F0C2A95BuL),
                    (+1, 0, 0xD1933C00B33B06B1uL, 0xBAD458BB2A43B31CuL),
                    (+1, -5, 0xB34DEE71B55239F4uL, 0x25012288D2A3DECEuL),
                    (-1, -7, 0xCF6381CD402A34D3uL, 0xBCB76DE0D7CE6502uL),
                    (+1, -9, 0xD4B01C437C3BE941uL, 0x428C9E7E5F3CBA6CuL),
                    (-1, -11, 0xBC9C11689BBBA625uL, 0xD2DA67C1AE512929uL),
                    (+1, -13, 0x86B1A700138600C6uL, 0x06020CE7AAF87D8BuL),
                    (-1, -17, 0xE834D9D7E0657091uL, 0x79BAA000A054D5C5uL),
                    (-1, -20, 0xD9FD54EE91ED5AB7uL, 0x9EA87A0B0EB96E09uL),
                    (+1, -20, 0xCDFDBF5C9711E554uL, 0xFF7A242C53910F35uL),
                    (-1, -21, 0x974EC9D942AAAFC8uL, 0xF5B99743DFF7D385uL),
                    (+1, -23, 0x9F9D79F981B410E8uL, 0xA42BA1765583427BuL),
                    (-1, -26, 0xF96614822076C397uL, 0x45CA91BCB4E514B6uL),
                    (+1, -29, 0xD9E58A17F0028140uL, 0xE16CA260D45F89B5uL),
                    (+1, -31, 0x8506F8AE315F7D49uL, 0x5D907B450BA0B8D4uL),
                    (-1, -32, 0xE5F73D606E630C9CuL, 0x510FF163FB86F00EuL),
                    (+1, -33, 0xA7A3A6D659EC8F07uL, 0x28638624120CE0B1uL),
                    (-1, -35, 0xAF179A4A7D3CEC90uL, 0x468822B4324455B5uL),
                    (+1, -37, 0x8518A88171965C84uL, 0x21ED2C46AC206D68uL),
                    (-1, -41, 0xCCB7B434A87D9458uL, 0xB4EF53EDE1A4947AuL),
                    (-1, -43, 0xC52DD38E46D5CB26uL, 0x0D9F7BAEC5830857uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX2p25Table = new(new ddouble[] {
                    (+1, 2, 0x8A79D001F2AB90AAuL, 0x4BAEFF4E6C29179EuL),
                    (+1, 0, 0xD418E42561792726uL, 0x14A6B70755EEB1DAuL),
                    (+1, -5, 0x90F91B4591E71EB1uL, 0xA6D7A826EF4A62CCuL),
                    (-1, -7, 0xA0F566C9BEEB4427uL, 0x476F8DFDEE30930CuL),
                    (+1, -9, 0xA1200A39577C3A8FuL, 0x53DF491912D8BBC3uL),
                    (-1, -11, 0x8F117BAD2DD68FF8uL, 0x04EE19FE011EAA7EuL),
                    (+1, -14, 0xD803B546C7571D08uL, 0x577C3DB3AD8A035CuL),
                    (-1, -17, 0xF3A37DF1A091CACFuL, 0x80E1D11285E23A43uL),
                    (+1, -21, 0x9A5CFF0FE9EF091AuL, 0x727B182835055CD0uL),
                    (+1, -21, 0x929C41587B5592AFuL, 0xA6C6343695A33361uL),
                    (-1, -22, 0x8B574E254D0F0FE2uL, 0x66E1BEBF0F169A2DuL),
                    (+1, -24, 0xA7AA59C5B2CA4C38uL, 0x5D8B5C24882CD5EEuL),
                    (-1, -26, 0x9B432909DC11F2F3uL, 0xBC5150D7245E38E3uL),
                    (+1, -29, 0xD7E0F34C825FC63CuL, 0x2AB2071AC98BDD2FuL),
                    (-1, -32, 0x9B8FF4E74383B480uL, 0x3C4A22A18E5245EDuL),
                    (-1, -34, 0x8A038492B515E5DFuL, 0xF58D740D33882EB4uL),
                    (+1, -35, 0xBDA5737BA4F3F668uL, 0xCD60F9BA69D641A8uL),
                    (-1, -36, 0x80A99AE5CFAD25D8uL, 0xF29628A9624632F5uL),
                    (+1, -38, 0x8197D81D6C464273uL, 0x04D40E96C41A70BDuL),
                    (-1, -41, 0xC6B2055CA080761AuL, 0x5CC2E35D7721805DuL),
                    (+1, -44, 0xBBFD2C9A5EA38FBDuL, 0x222272EED69781B6uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX2p50Table = new(new ddouble[] {
                    (+1, 2, 0x97CC4E8FCA11393AuL, 0xF05995F6EC64ACF3uL),
                    (+1, 0, 0xD62520FCD9A7A247uL, 0x142E86D40D9D9B7FuL),
                    (+1, -6, 0xEC7CE59EB995111BuL, 0x387EA8FB7600B512uL),
                    (-1, -8, 0xFB8A71456AD264D7uL, 0xF52FA649B8EC97A8uL),
                    (+1, -10, 0xF47A06034BAB5757uL, 0xC43B75697B5ACD60uL),
                    (-1, -12, 0xD6BFAAEB42805FD1uL, 0xC0BE415D9C29304CuL),
                    (+1, -14, 0xA5F3A804DEC0CCCEuL, 0xEFED695435640227uL),
                    (-1, -17, 0xD26A6952FDD5DF5BuL, 0x6CDD8A900B411469uL),
                    (+1, -20, 0xA6F82D438D491347uL, 0xC6A6E2BB8036E062uL),
                    (+1, -24, 0xEFF00279708BEB72uL, 0x3E23E2D412F67F6CuL),
                    (-1, -24, 0xE4DFC90686812F42uL, 0xF1427F59E499C5E2uL),
                    (+1, -25, 0xA07DB58B800B5224uL, 0x5CB664A493683330uL),
                    (-1, -27, 0xA66FE9E52CEC35EDuL, 0x93E073A48654D503uL),
                    (+1, -29, 0x89EC87CB74B456BBuL, 0x91828581568BA4CEuL),
                    (-1, -32, 0xAC5B9A0E8B881769uL, 0x8C922858607BAFCAuL),
                    (+1, -36, 0xC9D8112384EF1556uL, 0x4349087A496823FCuL),
                    (+1, -38, 0xF9573840E6435CCFuL, 0x0803BB901C066369uL),
                    (-1, -38, 0x907065EA6E0A37CAuL, 0x0A4A2251F9AD03E7uL),
                    (+1, -40, 0xB7B27B96372DE4ADuL, 0xA4A8EB97E7510421uL),
                    (-1, -42, 0xB28A06B81A8E06B6uL, 0x34E12BFBE4784B9AuL),
                    (+1, -44, 0x88CAB10ABA75EFECuL, 0x2DB4F4DADAE58B82uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX2p75Table = new(new ddouble[] {
                    (+1, 2, 0xA53C7B5E59B40E7EuL, 0x70233072668B6C74uL),
                    (+1, 0, 0xD7D283560FBA9763uL, 0xDB98DBF7F33AACB8uL),
                    (+1, -6, 0xC290C89571412068uL, 0xAA2F9F0F8FE7D14CuL),
                    (-1, -8, 0xC60DC89EBBF6A8A5uL, 0xD223A0CD4BCC40C4uL),
                    (+1, -10, 0xBA3CFA0FBEC50082uL, 0x454001FB572DE813uL),
                    (-1, -12, 0xA08F31C26E576BD2uL, 0x1BFA1E8571089728uL),
                    (+1, -15, 0xF949E19083BE06F7uL, 0xBD4E010728786225uL),
                    (-1, -17, 0xA71CE2B208121359uL, 0x121E922E07E6A776uL),
                    (+1, -20, 0xAB343283B9737963uL, 0xE1DCE89E0C6D6087uL),
                    (-1, -25, 0xEA4297D50C5B3419uL, 0x5CE760F329662566uL),
                    (-1, -25, 0x9A72E5B98185F721uL, 0xE3BFA2DBE2B68054uL),
                    (+1, -26, 0x8D47A6D7805B614EuL, 0x79FA4EE0644ACB3EuL),
                    (-1, -28, 0xA35BAF183954B775uL, 0xA06C256122CBC23AuL),
                    (+1, -30, 0x9641CDF085076102uL, 0xA25570D2292B8468uL),
                    (-1, -33, 0xE1DAEB49F2D59245uL, 0x61E2F82B6B6103F2uL),
                    (+1, -36, 0xFF933F5224473215uL, 0x13A847ABA971B367uL),
                    (-1, -40, 0xED3C50EA1810C7BBuL, 0x009C8DF54AE7460EuL),
                    (-1, -41, 0xCA4DAAA1884D8992uL, 0x0F679BECD6129EEAuL),
                    (+1, -42, 0xCCA0C16D86EA7F50uL, 0x9E18E687D7EB5F09uL),
                    (-1, -44, 0xF53F044B9F30237EuL, 0x9A3C0A14E41A993CuL),
                    (+1, -46, 0xE61783670C768AEBuL, 0x9E57DD843DE2B8ADuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX3p00Table = new(new ddouble[] {
                    (+1, 2, 0xB2C5119D353C4F70uL, 0xFA7EECAC6CF0B957uL),
                    (+1, 0, 0xD9353BE1667ED39AuL, 0xB784FB22096884E7uL),
                    (+1, -6, 0xA16E2BEF1969D86BuL, 0x33A4B12404B1050CuL),
                    (-1, -8, 0x9D332D6E3DEF4D91uL, 0x1420DEF7343F8AF8uL),
                    (+1, -10, 0x8EB28A2B355D127BuL, 0xE218FBD86BB2AFFCuL),
                    (-1, -13, 0xF035960FA20D49D0uL, 0x20B801535C4E3C23uL),
                    (+1, -15, 0xB9247CB01A7DE439uL, 0xD3A3747D7F6111D7uL),
                    (-1, -18, 0xFE66E7DF857E2BDDuL, 0x53258A388A065B62uL),
                    (+1, -20, 0x91C7E9E27DE9677DuL, 0x1BFF50BE2467C5FAuL),
                    (-1, -24, 0xDA1B699B7A4526FCuL, 0xAC6515DCF83831CEuL),
                    (-1, -28, 0xD44DE079834C2693uL, 0x66B6F9660AF3AE41uL),
                    (+1, -28, 0xDF0CF773AF9D8617uL, 0x505C4EA8BCD33702uL),
                    (-1, -29, 0x95AAA977E402D190uL, 0x84D05125457DD1C8uL),
                    (+1, -31, 0x96146F739654C0DAuL, 0xFFFD52F3B68734A3uL),
                    (-1, -34, 0xF96798110D0C9E8CuL, 0x48D8F2F19DD71010uL),
                    (+1, -36, 0xABAFCF69E883970EuL, 0x92AA9B0C520816D8uL),
                    (-1, -39, 0xB0F1C45DDF86F388uL, 0x10E51166001C86D4uL),
                    (+1, -44, 0xF9FECECF1F8FCF88uL, 0x3003F85AE4EB94DDuL),
                    (+1, -44, 0x961828BE6EF12C34uL, 0xC55D51EB7E02A922uL),
                    (-1, -45, 0x87984218C733173DuL, 0xED6CACA4319170D1uL),
                    (+1, -47, 0x99CB0461CECB0E04uL, 0x8C73B27883BD933DuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX3p25Table = new(new ddouble[] {
                    (+1, 2, 0xC061E78376C2C21BuL, 0x61FBB1228BFE84BEuL),
                    (+1, 0, 0xDA5CB5F33F4196CBuL, 0xBEFE10CA226201D0uL),
                    (+1, -6, 0x8706CEDDD0F0BFD0uL, 0xF1B2E9CAB705D34DuL),
                    (-1, -9, 0xFB982E0666B952E0uL, 0xE31B56CBD5CABB3EuL),
                    (+1, -11, 0xDC2A4C5FC9E81386uL, 0x03D16EB514DD95D2uL),
                    (-1, -13, 0xB445A9E1AA248EAAuL, 0x4BD34E01F02EB54CuL),
                    (+1, -15, 0x88E501EE3D3733C4uL, 0xC8A5F245136A591FuL),
                    (-1, -18, 0xBD563DFB67784AFFuL, 0xED581A8961F3ED20uL),
                    (+1, -21, 0xE5148D8170BE6A56uL, 0x719FC222D81CACAFuL),
                    (-1, -24, 0xD897049D41837E4FuL, 0xB851E12529D18C51uL),
                    (+1, -28, 0xA2CC145823FA6153uL, 0x0E3DA50BDDC7ACF0uL),
                    (+1, -29, 0x8E20C735852D0B03uL, 0x3C3BE88E702A0E53uL),
                    (-1, -31, 0xFEBE3153F59A2433uL, 0xF03CCF6FAB9CF8A4uL),
                    (+1, -32, 0x8D096C0CFFD6D915uL, 0x8732EB808EDA6608uL),
                    (-1, -35, 0xFC5955F0C59063ADuL, 0x52CA4D9F0BDDA63FuL),
                    (+1, -37, 0xBFD84664E9F2E2EDuL, 0xC4ABA56C3DE2F34EuL),
                    (-1, -40, 0xF3C7213542A68C46uL, 0x743EC0B37049E24EuL),
                    (+1, -43, 0xE5DF8E00C99C2C98uL, 0xFC3CD26DEFDBA12AuL),
                    (-1, -48, 0xE64BA756AC48A5C2uL, 0x1C7580610222F045uL),
                    (-1, -48, 0xCE11A523D32F9B77uL, 0xE981A84FCC8DD306uL),
                    (+1, -49, 0xA8ED62BC7506A9D8uL, 0x9278033D88B00D21uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX3p50Table = new(new ddouble[] {
                    (+1, 2, 0xCE0FAC0ECAAE10FCuL, 0x6D2CBFA71BC566C3uL),
                    (+1, 0, 0xDB54CAFD37A88E86uL, 0xE48CE68B27BCA70BuL),
                    (+1, -7, 0xE3A0F68CA9AB8A7EuL, 0x0656C142FFEF621EuL),
                    (-1, -9, 0xCAF966F934AC1104uL, 0x55778EB611E4D05BuL),
                    (+1, -11, 0xAB19B48D158436A2uL, 0x7D48EF676E4F7E45uL),
                    (-1, -13, 0x87F8569C218F9147uL, 0xEBB6FA773BECFF48uL),
                    (+1, -16, 0xCA69167A9B0E313AuL, 0x56B3A3A9A851C4ADuL),
                    (-1, -18, 0x8B50876EBC632523uL, 0xC112A55014C85A96uL),
                    (+1, -21, 0xACC4A231F9ADEB56uL, 0x7F1B5CDC8278445EuL),
                    (-1, -24, 0xB567064D7E01F319uL, 0xFFA62B0C45248807uL),
                    (+1, -27, 0x81C5AE2B6D27F26BuL, 0x840EBF0660649410uL),
                    (+1, -32, 0x90F3558C0C3DF2E4uL, 0x7C0048130FA049CCuL),
                    (-1, -32, 0xC20393C16D8F08DBuL, 0x65465AC46618F6D8uL),
                    (+1, -34, 0xFA92ED9AC4884D1FuL, 0x63145C6AA101FBF7uL),
                    (-1, -36, 0xF142C4D6319A2D8AuL, 0x6164A185FF66A22AuL),
                    (+1, -38, 0xC40E816D43F1B500uL, 0x6F4BC98A93F39AA9uL),
                    (-1, -40, 0x89A9A91FA7D23236uL, 0x84F0F6B53FE9B379uL),
                    (+1, -43, 0xA270748C6067B935uL, 0x46E95801C21412A0uL),
                    (-1, -46, 0x8CA830E5DFECC290uL, 0xB1FE6BBBCCEAF73FuL),
                    (+1, -52, 0xAD5381548F34D0B9uL, 0x63D85FDE3E9DF409uL),
                    (+1, -51, 0x83FD4FCE53BEBFD0uL, 0xB20EE7D77D49A71FuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX3p75Table = new(new ddouble[] {
                    (+1, 2, 0xDBCBB56178A913B0uL, 0x377BE091270ECB7CuL),
                    (+1, 0, 0xDC26A6A7BFE6CA88uL, 0x6521397850423D30uL),
                    (+1, -7, 0xC14569723757DE23uL, 0x6D1A4F9D71E63D95uL),
                    (-1, -9, 0xA50D2192A59DF3A8uL, 0x72DF4DCABF1A1045uL),
                    (+1, -11, 0x85FCF12494A5739AuL, 0xC380CD309E5799ECuL),
                    (-1, -14, 0xCE5DD87499470CF6uL, 0xD8C537EDB9F95906uL),
                    (+1, -16, 0x9607EEF3EEEDD5CCuL, 0x5206A89AEBBF6249uL),
                    (-1, -19, 0xCC0AC84DF693876CuL, 0xA8C321814388D53AuL),
                    (+1, -22, 0xFEF806501F8012F8uL, 0xED4C1B5E9BC017B2uL),
                    (-1, -24, 0x8CED0748F2B65EA7uL, 0x05AB816A752863C7uL),
                    (+1, -28, 0xF8C03CE1E280768AuL, 0x2E838C26E4531355uL),
                    (-1, -32, 0xCB339836747291B4uL, 0xC1E34C6D7CA31D77uL),
                    (-1, -34, 0xE7252EA5C5C9185CuL, 0x9BEE00BABBAC7A8BuL),
                    (+1, -35, 0xCF77BF15B36DE521uL, 0x9BA71D69CDE0E9B3uL),
                    (-1, -37, 0xDC2C229C20C7E207uL, 0x5E6D23E371FA2BC1uL),
                    (+1, -39, 0xBDBFA0D2FEBB4E70uL, 0xA678932D6CE72BB0uL),
                    (-1, -41, 0x8DD12501075E97B4uL, 0x0B7B1D6C0C2DABC3uL),
                    (+1, -44, 0xB948A6F5CBBD882EuL, 0xFB7FA3E98B24CF51uL),
                    (-1, -47, 0xCC0A31B40109CBC0uL, 0xBB914780E59F9244uL),
                    (+1, -50, 0xA2B71E2BB7076D85uL, 0x1A0E4236A92861F0uL),
                    (-1, -57, 0xA297DD342A293455uL, 0x4A7B60CFFAA06884uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX4p00Table = new(new ddouble[] {
                    (+1, 2, 0xE993DB6F3F17B91BuL, 0x5585C0B72C77686AuL),
                    (+1, 0, 0xDCD96F86F7BC192DuL, 0x1FE0AA69D37984A5uL),
                    (+1, -7, 0xA53A8917FF1EFCB1uL, 0x07FC88D5A15CD16FuL),
                    (-1, -9, 0x873E84EECC12284DuL, 0xBDC5ED300C9031BBuL),
                    (+1, -12, 0xD3783ED5232ADB9CuL, 0x35DDBD4F7DDE5305uL),
                    (-1, -14, 0x9DAA9FA81CA8A724uL, 0xF67FFE3F4D3EAC7FuL),
                    (+1, -17, 0xDF637D8E1E8D90C6uL, 0x38E7E388F6E8CFF8uL),
                    (-1, -19, 0x9555859F5D9E430FuL, 0x93E191B64EB21881uL),
                    (+1, -22, 0xBA0942CC251CEFF2uL, 0x185543928BA53ECCuL),
                    (-1, -25, 0xD2A70C21B7B70D1AuL, 0xB1512B511C2441DBuL),
                    (+1, -28, 0xCCBAF38F8BFD23D5uL, 0x317D23B2AD4FEC3EuL),
                    (-1, -31, 0x8D35A134D896576CuL, 0x8057B3B2079388C4uL),
                    (-1, -38, 0xF4589D44624ADFCDuL, 0x1727B663AD330F5EuL),
                    (+1, -36, 0x9877F84EBD63CF6AuL, 0x4C1DD7C81FC01351uL),
                    (-1, -38, 0xBF14F3BED59C7581uL, 0x70D2EBDB91CA60BBuL),
                    (+1, -40, 0xB04F03E7C9EFF277uL, 0x6FF36AD58F87B7E1uL),
                    (-1, -42, 0x8A7F2436E3EEDFC5uL, 0xF0E5A689C1D377A9uL),
                    (+1, -45, 0xC0261B408AC5E3DBuL, 0x0D280D0362DEEA17uL),
                    (-1, -48, 0xEAEACCAF3739FD17uL, 0xA6BF250B8515CC54uL),
                    (+1, -51, 0xF2720C0C416C7A5AuL, 0xBD3772035F71376FuL),
                    (-1, -54, 0xB27A100CC75FAA65uL, 0x169CF9889ABB96CBuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX4p25Table = new(new ddouble[] {
                    (+1, 2, 0xF7665BC4D7FF68A9uL, 0x2120150E2615218DuL),
                    (+1, 0, 0xDD72C3FA3D14DFBAuL, 0x182CBEB008D8BC56uL),
                    (+1, -7, 0x8E2B0AB973F39325uL, 0x65D140B98B016D85uL),
                    (-1, -10, 0xDF45A10DAAEF02FFuL, 0x338DBF75D9AEC687uL),
                    (+1, -12, 0xA826F3FDBA83D824uL, 0x01CD6E8EAC0BE8FBuL),
                    (-1, -15, 0xF2A2CC69D0D3DD53uL, 0xABB6870E1F5C28B4uL),
                    (+1, -17, 0xA73A18DA8EEA96B1uL, 0x41DBB18346FD6AD7uL),
                    (-1, -20, 0xDB0D0370A79D2555uL, 0x7C911F45AAD43A49uL),
                    (+1, -22, 0x87142126FF791B47uL, 0x4A18026C596F584AuL),
                    (-1, -25, 0x9A31700C6069495AuL, 0xF3006ABFDAD6252CuL),
                    (+1, -28, 0x9D3F952AD41CD17BuL, 0xB9A4D9B9B72A903FuL),
                    (-1, -31, 0x829E8BCB63D40720uL, 0x6A4CD6239FE4F138uL),
                    (+1, -36, 0xE3BA178B41370082uL, 0x304079CB979EFC0CuL),
                    (+1, -38, 0xA6D2EF87F53A8B66uL, 0x4939309FEF555FE4uL),
                    (-1, -39, 0x9A639978E3BF62AEuL, 0x527852C3BC430F20uL),
                    (+1, -41, 0x9D901480FDDA4390uL, 0xEFB5E0E997531598uL),
                    (-1, -43, 0x824FE51202BC73EFuL, 0x7EB4339E61CB6D6BuL),
                    (+1, -46, 0xBCF047BA937F9360uL, 0x7571AEF5AB621BC1uL),
                    (-1, -49, 0xF502747A2891489BuL, 0x41D9D4AADC55CBDAuL),
                    (+1, -51, 0x8CD24193498FA1D4uL, 0x077EAFFAAF04794FuL),
                    (-1, -54, 0x88B0C25E27B633B1uL, 0xC30ECA33371E5131uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX4p50Table = new(new ddouble[] {
                    (+1, 3, 0x82A0E207790A8FCBuL, 0xD3197BC052575658uL),
                    (+1, 0, 0xDDF716E5B10A0784uL, 0xFAA34F6AA4BF905AuL),
                    (+1, -8, 0xF622BFF3B9E73997uL, 0x318B7976422409C7uL),
                    (-1, -10, 0xB9979A48EBD05366uL, 0xFE915AA0D9B50773uL),
                    (+1, -12, 0x86B4987ED4C98959uL, 0x4E5DD6775A832B16uL),
                    (-1, -15, 0xBC0E7A6028678C3EuL, 0x2573462A8EEF013AuL),
                    (+1, -18, 0xFBEE67E021A98610uL, 0xAA1E33391AFD8CECuL),
                    (-1, -20, 0xA147E3912323047BuL, 0x40D8579E1D91AFB7uL),
                    (+1, -23, 0xC3FCCBEF9FC55D04uL, 0xA1C3A2CE544097FEuL),
                    (-1, -26, 0xDF4A6F24C4BE13BAuL, 0x38EE6BF1995B8E40uL),
                    (+1, -29, 0xE90009A289822372uL, 0x4ECCA49141B0BBE0uL),
                    (-1, -32, 0xD34AA94FA6FCA846uL, 0x0A91795E9F2D8BE2uL),
                    (+1, -35, 0x8D2286EBFDCF0BB3uL, 0x358F31243C55A2B8uL),
                    (-1, -44, 0xF17E243E846251ECuL, 0xB8C7AA13EEC25F95uL),
                    (-1, -41, 0xDA52D335FB1D909DuL, 0x7B9C07CBCE086F8DuL),
                    (+1, -42, 0x862A94130361D87EuL, 0x08D3B2CA2BD3438DuL),
                    (-1, -45, 0xED79ACB2520DD9F0uL, 0x07ACAE7C4AB2F951uL),
                    (+1, -47, 0xB3624D417F84A8FFuL, 0xC961006FFCF5A1D4uL),
                    (-1, -50, 0xF2342E16303D4195uL, 0x28F16F433FC71485uL),
                    (+1, -52, 0x939ABF881EA92A45uL, 0xD9B4BEDDAA347138uL),
                    (-1, -55, 0xA02C3CE85275CC90uL, 0xEA5B1C3E1AE2EF40uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX4p75Table = new(new ddouble[] {
                    (+1, 3, 0x899270D35496D19DuL, 0xA430882404822168uL),
                    (+1, 0, 0xDE69F4E0B6BD2D33uL, 0xF0F6C274DB1557DFuL),
                    (+1, -8, 0xD64748750266A1D2uL, 0xD5A93C02B9DFE4ECuL),
                    (-1, -10, 0x9B4D33E2CBC020D0uL, 0xFB0AA2C8A08044BFuL),
                    (+1, -13, 0xD9625592A34CA5F9uL, 0x2D537C4065B46487uL),
                    (-1, -15, 0x92D09F7F53E3EE33uL, 0x3EFF9AF33DF1A4BAuL),
                    (+1, -18, 0xBF082C8965FA67B6uL, 0x623B1D41A0A4B7DBuL),
                    (-1, -21, 0xEEB0175435E277A7uL, 0xB20AA9260413BCD5uL),
                    (+1, -23, 0x8E6DA1A27A7BAC4BuL, 0xBB6487EA8BB4DA74uL),
                    (-1, -26, 0xA0E4888D836D1406uL, 0xB0E85AB6572DB84BuL),
                    (+1, -29, 0xA943FBDDF5C9E491uL, 0x5E789F4C6B88D798uL),
                    (-1, -32, 0xA07D25FCBB7C1C66uL, 0x300E5C6189D5776AuL),
                    (+1, -36, 0xFC79AB103ADE6ACCuL, 0x336E2410D97480E2uL),
                    (-1, -40, 0xE6B5DA1A51413B7CuL, 0x9AFED97694E10571uL),
                    (-1, -43, 0xD4DFFD967EC8AF19uL, 0x90F58FC45DBF2678uL),
                    (+1, -44, 0xD3B72CA21D59330AuL, 0x9B77E4EBD20A54F1uL),
                    (-1, -46, 0xD0DB11C93C21F94FuL, 0x1D703177C5E6D442uL),
                    (+1, -48, 0xA5A70086161BFC3FuL, 0xC3014BB88B999DDCuL),
                    (-1, -51, 0xE77C897AA7A899DEuL, 0xBE2FA1EF57EBA79BuL),
                    (+1, -53, 0x9289163E56E86D05uL, 0xCED37DBC081407CAuL),
                    (-1, -56, 0xA8A39628483503D5uL, 0x978AAE2666F7F8A6uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX5p00Table = new(new ddouble[] {
                    (+1, 3, 0x90875A6FD5F1BAD7uL, 0x3E0DACC0385555CAuL),
                    (+1, 0, 0xDECE38200911C1DAuL, 0xA547D4F9BEDC0F6DuL),
                    (+1, -8, 0xBB89BB9BE4BD1422uL, 0x3643C3F9CAA417B8uL),
                    (-1, -10, 0x82C722D90CB6E95CuL, 0xAED98A15F9C240A8uL),
                    (+1, -13, 0xB0A07F239AE12B96uL, 0x1E98423D086F9991uL),
                    (-1, -16, 0xE6E1B7A1B9835879uL, 0x35DD563E6C95E7DCuL),
                    (+1, -18, 0x91D8C1C1DB755B2BuL, 0xBB8E8D23B0506999uL),
                    (-1, -21, 0xB1A45AE4C8FB6442uL, 0xFCD54DCBDAEA09A3uL),
                    (+1, -24, 0xCFBABF89DD217CFAuL, 0x7F2D7AC57C85EAFAuL),
                    (-1, -27, 0xE79F66636D31CE3FuL, 0xD1D5D1E34FD75C5FuL),
                    (+1, -30, 0xF3644AE1F49961CEuL, 0xA7E6F04F3AECCC39uL),
                    (-1, -33, 0xEBD00036DD8D5A26uL, 0xB26EB1245318976DuL),
                    (+1, -36, 0xC8CC687A5C04DA78uL, 0xDCF1BAEF85B00B1CuL),
                    (-1, -39, 0x8231225DD64188CCuL, 0x5CFD868391D6966CuL),
                    (+1, -45, 0x87B88C85EEEF895CuL, 0x567986F8C7C5A7FAuL),
                    (+1, -45, 0x8F284E889309E326uL, 0xBFB90149AE295ACFuL),
                    (-1, -47, 0xAEDABAB0AB874796uL, 0x85462383D3A87235uL),
                    (+1, -49, 0x94D0B334B9772A53uL, 0x5D98952CB5201F73uL),
                    (-1, -52, 0xD7CE13661F38C2E4uL, 0x04D9450721C22EFEuL),
                    (+1, -54, 0x8CBFBCC16C5FC452uL, 0xA8590A9A9B8FE4C3uL),
                    (-1, -57, 0xA7FF78ECDC2D53D6uL, 0xBB4452AD46CC6877uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX5p25Table = new(new ddouble[] {
                    (+1, 3, 0x977F339518C73DE6uL, 0x10A7DAD4D7CE030BuL),
                    (+1, 0, 0xDF262FAA68588EFBuL, 0x0F2AE96A448D2DE3uL),
                    (+1, -8, 0xA4F43A6FAE2DB390uL, 0x3778B34BB2787C59uL),
                    (-1, -11, 0xDD93230BEDFC3D54uL, 0x9A9CA099865ACE99uL),
                    (+1, -13, 0x90789BD39BE1E311uL, 0x224A2140264D4E09uL),
                    (-1, -16, 0xB6D14679F2743B41uL, 0x0DF05097717F0E43uL),
                    (+1, -19, 0xE03D32481F08CF94uL, 0xCB9E1C340FC65E1FuL),
                    (-1, -21, 0x8507EE1577A86A1EuL, 0x70CBFC4BFB779416uL),
                    (+1, -24, 0x982F1A78F822B0C9uL, 0x5075FA52C8D74102uL),
                    (-1, -27, 0xA6F7AE65A5BD469DuL, 0x06D31D9E86311BF7uL),
                    (+1, -30, 0xAE29E7DF6E2DBC73uL, 0x52B935299923908AuL),
                    (-1, -33, 0xAA1838F567ABA117uL, 0xDBEE7ADD5DAC3180uL),
                    (+1, -36, 0x96E490501FEF7D58uL, 0xF5CFDC4D22A0B935uL),
                    (-1, -40, 0xE1D238DC9963CF59uL, 0x8A65E709183D0ADBuL),
                    (+1, -44, 0xD5131C5C50BFB316uL, 0x480BD258EBADEC6BuL),
                    (+1, -48, 0xEBC9E4F9033C903EuL, 0xFB0F7B8AA08CBB20uL),
                    (-1, -48, 0x86919A7CF0E50895uL, 0xB02C09F672D06CF3uL),
                    (+1, -50, 0x8129418F30AAEF98uL, 0x8FFD394FA4CDA165uL),
                    (-1, -53, 0xC4B5E5242324049BuL, 0xAF22E80491867EDBuL),
                    (+1, -55, 0x841E3D3BDDEF91E7uL, 0x228605A2A05E12A5uL),
                    (-1, -58, 0xA1F953E28DFAB72AuL, 0xC1F1D40663B94344uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX5p50Table = new(new ddouble[] {
                    (+1, 3, 0x9E79A1ACA6D7D0B3uL, 0x1D50D4B4DBF6127AuL),
                    (+1, 0, 0xDF73BD27519FCBE9uL, 0xCFB24C76D67309B4uL),
                    (+1, -8, 0x91C4D7E583E97F72uL, 0x8F12A2644FDDB439uL),
                    (-1, -11, 0xBCC54C0DE32B1F95uL, 0x8A2687379298B224uL),
                    (+1, -14, 0xEDD9E74EFDE59A99uL, 0xD9EC806FBF2C4CCCuL),
                    (-1, -16, 0x91BF1A8C3C545D40uL, 0x905CE99EA7A053C7uL),
                    (+1, -19, 0xAD91E19D92149E78uL, 0x46EDDD6EE1EB39F0uL),
                    (-1, -22, 0xC888D722BBB04EBBuL, 0xA0EF87E472747245uL),
                    (+1, -25, 0xE02D60324F40773FuL, 0xCE42C2D4A92D89F8uL),
                    (-1, -28, 0xF179D24B25371B8CuL, 0x3F6BF4BF89338135uL),
                    (+1, -31, 0xF8F8FE308451E9F7uL, 0x597A75276CA69469uL),
                    (-1, -34, 0xF2FD8A1BBF995BA9uL, 0x41B5575320FA288BuL),
                    (+1, -37, 0xDBF2ABA1DA66C9CBuL, 0x0EA2E95DFCACCBF4uL),
                    (-1, -40, 0xB0C53D04B1D91E26uL, 0x6A1CF99FBE2D14C3uL),
                    (+1, -44, 0xDEC1C928D7C763AAuL, 0xAE0E7F0089594586uL),
                    (-1, -49, 0xB9CA14C754165BFDuL, 0xA931FD68B2CDC472uL),
                    (-1, -50, 0xAC59A72431F0B8FFuL, 0x18D9C72FA59A7921uL),
                    (+1, -52, 0xD4C1B050C5C8817AuL, 0x19239CF12F0C90EEuL),
                    (-1, -54, 0xAEC9A6D1D5E2950DuL, 0x5327A74582100A4FuL),
                    (+1, -57, 0xF35605A310F7FC33uL, 0x18C17C55BDD58962uL),
                    (-1, -59, 0x98CEDC95590BB83AuL, 0x6E79CECE760FDDB8uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX5p75Table = new(new ddouble[] {
                    (+1, 3, 0xA57657C5C54AC068uL, 0x5CC15D0C641F249BuL),
                    (+1, 0, 0xDFB86BB07447FAD8uL, 0x87F199F47E427A88uL),
                    (+1, -8, 0x81618F7D535E092AuL, 0x66CC7AFC5FF29E24uL),
                    (-1, -11, 0xA1AFCBBCDFF070DDuL, 0x091D445D0E8FB797uL),
                    (+1, -14, 0xC4FD83137DEB1B31uL, 0x698B55A271763804uL),
                    (-1, -17, 0xE9EAE8D682CB231CuL, 0xFB9538024FDABA64uL),
                    (+1, -19, 0x8742AC9704C65856uL, 0x5430482DB5A36E34uL),
                    (-1, -22, 0x9823AF4A1256B41EuL, 0xCDCC033780A441B0uL),
                    (+1, -25, 0xA613BFC30C7B509CuL, 0x6D76405491B1F2D1uL),
                    (-1, -28, 0xAF5C488E4BB46D48uL, 0x7FAAE0D4D4EC608AuL),
                    (+1, -31, 0xB22E8EEA3DC77CDCuL, 0x0588A7CE36D8C759uL),
                    (-1, -34, 0xACC6E34CC7A8D9C0uL, 0x0E86610DB52C254FuL),
                    (+1, -37, 0x9D983903EA965B00uL, 0xA28D3D19CBEBE711uL),
                    (-1, -40, 0x837DE31AA2705BDEuL, 0xFF33C987669CDF75uL),
                    (+1, -44, 0xBBDFFF46CD9B5711uL, 0x5625D087073588FEuL),
                    (-1, -48, 0xB4D7F165DC716866uL, 0xB61ACDF9A2F466F9uL),
                    (-1, -53, 0xD6E439A07A63DC7AuL, 0x648B790EB83E799CuL),
                    (+1, -53, 0x9F4537C3A0920B61uL, 0xA3C903F37A735476uL),
                    (-1, -55, 0x95EA31142F6D5108uL, 0x900C07F4A60DB6B4uL),
                    (+1, -58, 0xDBB3AFCD0D0AB3CAuL, 0x36A05EF1675CB6D3uL),
                    (-1, -60, 0x8DC22B3A865BF22FuL, 0x8670B8547730C319uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX6p00Table = new(new ddouble[] {
                    (+1, 3, 0xAC75142A62EC2F69uL, 0xD69227D809FF9E1DuL),
                    (+1, 0, 0xDFF581685E47112DuL, 0x63A267435184DD36uL),
                    (+1, -9, 0xE69E77F70907430BuL, 0x14382AB5C2574EB5uL),
                    (-1, -11, 0x8B3118CAAD8C8207uL, 0x56269EA7BBAA8EAAuL),
                    (+1, -14, 0xA4196A75DA75E7E8uL, 0x7804E51216D70E1BuL),
                    (-1, -17, 0xBCE83157D31C39BDuL, 0xC9D222782809CBA2uL),
                    (+1, -20, 0xD435C644E03E5456uL, 0xBD741DE5A8B89AEFuL),
                    (-1, -23, 0xE85DEE573C46C276uL, 0xB879F9BB0FD19D13uL),
                    (+1, -26, 0xF792E0F043322AB2uL, 0x093CE93750F7A301uL),
                    (-1, -29, 0xFFF83F65DD74F21CuL, 0x927FBB8B05681363uL),
                    (+1, -32, 0xFFC53600FCDD493EuL, 0x89A5F1A5A91117D0uL),
                    (-1, -35, 0xF56C23C9F420C3CAuL, 0x5EDC4B2180F2D061uL),
                    (+1, -38, 0xDFC54DAC1ED830F2uL, 0xBC46D3644AFFC678uL),
                    (-1, -41, 0xBE3960444A774D88uL, 0xC5FE67CB75048B42uL),
                    (+1, -44, 0x90E7F1B819590782uL, 0xD222E50B0D3BD39EuL),
                    (-1, -48, 0xB18BD0C0AD4F9F72uL, 0x2776C94364CF3C36uL),
                    (+1, -53, 0xBD7DB0D1E94CD470uL, 0xA79A23BAF37A8347uL),
                    (+1, -55, 0xBE50E5201E11494AuL, 0x84A6B81FDE1BD091uL),
                    (-1, -57, 0xF2CB53F10482FE06uL, 0x4A0067C6BBD36F0BuL),
                    (+1, -59, 0xC16EE692ECAA8EF0uL, 0x225E0A2173BAFDA0uL),
                    (-1, -61, 0x816EA6E75DCEB803uL, 0x3A07C4DF2CD9F280uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX6p25Table = new(new ddouble[] {
                    (+1, 3, 0xB3759E703F031513uL, 0x883658D97B799214uL),
                    (+1, 0, 0xE02C0D20FE9AC6D8uL, 0x06B4C2EBD7D2EDC9uL),
                    (+1, -9, 0xCE5586EDCB0D8C48uL, 0x2721009D3509CB29uL),
                    (-1, -12, 0xF0CDD4169AE7DEA2uL, 0xAFE5F37EDF3C7063uL),
                    (+1, -14, 0x897561FE4C739F41uL, 0x4DE829EA03FC909DuL),
                    (-1, -17, 0x997D218461DC4CDEuL, 0x5C4CF85CFBE1710AuL),
                    (+1, -20, 0xA78A8EEC4E693601uL, 0x561F6DADE0669A86uL),
                    (-1, -23, 0xB29BA6266E52CFBEuL, 0x228035D10F9AAD3DuL),
                    (+1, -26, 0xB9AF629154F6925DuL, 0x53C743C3F632BA6CuL),
                    (-1, -29, 0xBBD7AAE825C40A89uL, 0x0DD390F86CBB0CB3uL),
                    (+1, -32, 0xB84CA712ABB0963DuL, 0xF858342D9D333FE3uL),
                    (-1, -35, 0xAE801FEA1B8EF3AEuL, 0x4F5AC76694FD457AuL),
                    (+1, -38, 0x9E2F58698685FBCDuL, 0xEC361C651251A2D5uL),
                    (-1, -41, 0x8771BD71117C83A6uL, 0xC7BF25ECFB25795FuL),
                    (+1, -45, 0xD585B46DE23C1E36uL, 0x8C1A21AED94EDF76uL),
                    (-1, -48, 0x920C6B7E6EB5BE5EuL, 0xEB3CA3F1331A99DAuL),
                    (+1, -52, 0x8E105F8F26D87E25uL, 0x45326D30F54F4F62uL),
                    (+1, -58, 0x842A4733225A5FD6uL, 0xD79D561986B2068AuL),
                    (-1, -58, 0xB001A86625093D36uL, 0x110AFBBEDEC465FCuL),
                    (+1, -60, 0xA4030E7A87A8379DuL, 0x9D3B2567C54DC8F1uL),
                    (-1, -63, 0xE7FC2158ED8A0379uL, 0x0A48F941D180F096uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX6p50Table = new(new ddouble[] {
                    (+1, 3, 0xBA77C5EAC845D0B7uL, 0xD3A00C5AB9589A59uL),
                    (+1, 0, 0xE05CF105FBC51B11uL, 0xD97BCF4848348049uL),
                    (+1, -9, 0xB947CBBD8F7FE6CBuL, 0xC2A8D3D7B7188C08uL),
                    (-1, -12, 0xD13E7894E314A01DuL, 0x436BC290360F98C3uL),
                    (+1, -15, 0xE7808C624992DDFCuL, 0xFCC83C2579DE3DE0uL),
                    (-1, -18, 0xFAE218C5EA7B780AuL, 0xDAE7A5A9F3ACA5B7uL),
                    (+1, -20, 0x8519E6B532DDEE1FuL, 0x6A3C9BC0FB32B8BFuL),
                    (-1, -23, 0x8A2AC28CA157CC8FuL, 0x61AE2695F1D84178uL),
                    (+1, -26, 0x8C2503C3CB3A79D9uL, 0x6631B0400552411FuL),
                    (-1, -29, 0x8AA54FC7C0237B45uL, 0x1C4DD92A6723AC49uL),
                    (+1, -32, 0x856C838356A7B94AuL, 0xB28E14719FBFD804uL),
                    (-1, -36, 0xF8CE295C18159B70uL, 0xCE5DC50C13744A47uL),
                    (+1, -39, 0xDF645EB6CFA8E6BEuL, 0x9EE78FDD05197FA5uL),
                    (-1, -42, 0xBF3C01985879948EuL, 0x1A386DE0E3EE1E84uL),
                    (+1, -45, 0x99599D2F5D4D4CD0uL, 0x5E47D05B91B8D052uL),
                    (-1, -49, 0xDE3AC809045C8620uL, 0x6C09D784AA1352DBuL),
                    (+1, -52, 0x846839826D5F3F3AuL, 0x31A0A6A9F55BBE4AuL),
                    (-1, -57, 0xA4119AE747150A50uL, 0x76F0E990618EF2D2uL),
                    (-1, -60, 0xBFAF551A1F530D18uL, 0xB1A0245FE20BDE1AuL),
                    (+1, -61, 0x8261345431E94889uL, 0x0377701CA406CF27uL),
                    (-1, -64, 0xCA8E3E8AC82D5940uL, 0x87E9C9B814696532uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX6p75Table = new(new ddouble[] {
                    (+1, 3, 0xC17B6068CA991A94uL, 0x60D7DCD69897734FuL),
                    (+1, 0, 0xE088EAFFFE1B472CuL, 0x28CD1BA388AA0F0DuL),
                    (+1, -9, 0xA6F286493AAA2E87uL, 0x1A735AABEC4E660BuL),
                    (-1, -12, 0xB69A435E542BD3E7uL, 0xFA038DF4C6676A20uL),
                    (+1, -15, 0xC3EC74AEEB6C2F82uL, 0xF62F26DAEBD11005uL),
                    (-1, -18, 0xCE308DDF915D1CA0uL, 0x99CD8A284E40632BuL),
                    (+1, -21, 0xD4C1FF671544DF96uL, 0x418412B2887A98A1uL),
                    (-1, -24, 0xD71B3C7AB0CF43C4uL, 0x3927A57E3FB0FE98uL),
                    (+1, -27, 0xD4DF93BDE7D6085DuL, 0x9F23686461D3E030uL),
                    (-1, -30, 0xCDE316453DDC02F8uL, 0x3661943139319477uL),
                    (+1, -33, 0xC22FE4A16A3E088AuL, 0x7D65FD96D99171C3uL),
                    (-1, -36, 0xB208647CEC92F15CuL, 0xD335D08B5F45409AuL),
                    (+1, -39, 0x9DE617859D7BA9D0uL, 0xA02DDE465EC7CF02uL),
                    (-1, -42, 0x86750986FDDB6FF1uL, 0x1866ED8B202AE2B8uL),
                    (+1, -46, 0xD9181578EB39ED55uL, 0xA9687655CF64EB16uL),
                    (-1, -49, 0xA2444E98C2E574F3uL, 0x2510AE82417FD679uL),
                    (+1, -53, 0xD50802AE5ADEEFD2uL, 0xE46A289A244FB9D6uL),
                    (-1, -57, 0xCFDEA9BF2CBBB50FuL, 0x14DBDE8A9CB34383uL),
                    (+1, -66, 0xAE3532D1979BA229uL, 0x2399989E319366FBuL),
                    (+1, -63, 0xB5DB4A58905E7596uL, 0x4BE47105C9E3ED0AuL),
                    (-1, -65, 0xA9B89C6DECE628D7uL, 0x32AA601A2D6E1DFAuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX7p00Table = new(new ddouble[] {
                    (+1, 3, 0xC880492DEA5819A9uL, 0x3097601A02DAFD71uL),
                    (+1, 0, 0xE0B09B583666FF3AuL, 0x56849126429FB3ABuL),
                    (+1, -9, 0x96EA95D939727EBAuL, 0x41E6923D8AE33678uL),
                    (-1, -12, 0xA000B021A5971380uL, 0xF6F1B8629B9DF49BuL),
                    (+1, -15, 0xA69B57981CE34B85uL, 0xCD1CB6BDE787ABF4uL),
                    (-1, -18, 0xAA5E55BC82358D5DuL, 0x688A875DF77CC0B0uL),
                    (+1, -21, 0xAB0866BAA2CE70E2uL, 0x97DE9665E82CD7C4uL),
                    (-1, -24, 0xA877B781653C3715uL, 0x302B163796214FBDuL),
                    (+1, -27, 0xA2ACA768B983BA0EuL, 0x59B1FA942CC0D8B0uL),
                    (-1, -30, 0x99CA968796E8A5BAuL, 0xC3312B6030C2E13BuL),
                    (+1, -33, 0x8E16AD471D2AA248uL, 0x0CED67818A5D24AAuL),
                    (-1, -37, 0xFFE95D01D6838208uL, 0x09094EF1CF41E263uL),
                    (+1, -40, 0xDFC4110AE0E87061uL, 0xB075D0448D91B742uL),
                    (-1, -43, 0xBCDEEFEF12041505uL, 0x8994A45150146B18uL),
                    (+1, -46, 0x98734E56BEFD1C8EuL, 0x5845B9B5CD454F3AuL),
                    (-1, -50, 0xE7850FFD5826558BuL, 0x6ABB3D502C08ECAEuL),
                    (+1, -53, 0xA00C3055F2E1B1D3uL, 0xAD9E2FC01B3EE0B6uL),
                    (-1, -57, 0xB9825ECAC1BACC8AuL, 0x88A7C0C83CB65896uL),
                    (+1, -62, 0xFD08117ECE306CB3uL, 0xEC1C52D8DF0B8BDAuL),
                    (+1, -65, 0xADA8BF7661D11B2AuL, 0x30A98772FFDD2D68uL),
                    (-1, -66, 0x841E01B8EC408E95uL, 0x961419C4734FD2B6uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX7p25Table = new(new ddouble[] {
                    (+1, 3, 0xCF86601B98DB7B0AuL, 0x6465DF3D5E771FDFuL),
                    (+1, 0, 0xE0D48A01CE7E71EBuL, 0x8D5D7F273057605AuL),
                    (+1, -9, 0x88D7B8AE4A444E27uL, 0x2103AD7358052F58uL),
                    (-1, -12, 0x8CBDD731C154D3C4uL, 0x65B6EEAF695B18DCuL),
                    (+1, -15, 0x8E528C90A5063504uL, 0x4F5CD32AF716258BuL),
                    (-1, -18, 0x8D7E841217FC6D71uL, 0x2C86470EB24BA7FEuL),
                    (+1, -21, 0x8A42DDAECC80B004uL, 0xDB87F5FB06E36FC4uL),
                    (-1, -24, 0x84B9231E3B4FD085uL, 0x0C4CB29FAF91E31FuL),
                    (+1, -28, 0xFA23C0222D287063uL, 0xC2B0AF174CA9E11AuL),
                    (-1, -31, 0xE723F64288C1785EuL, 0x43A74C8ECDF79EBEuL),
                    (+1, -34, 0xD11E03B695E9723DuL, 0x6DF2DADD1BF09B27uL),
                    (-1, -37, 0xB8D55515FFCA3B79uL, 0x16C52303914AD7C0uL),
                    (+1, -40, 0x9F1BB2379A686061uL, 0x53D53AB0D9BF58E3uL),
                    (-1, -43, 0x84C6F1C68DA43605uL, 0x77D7C259F2865CDDuL),
                    (+1, -47, 0xD54DEE71F0C3FED2uL, 0x91DB2439A59C9F26uL),
                    (-1, -50, 0xA2F91AECD5D35534uL, 0xC7B8CC7E5C803C2AuL),
                    (+1, -54, 0xE7C66400EF9F0F4EuL, 0xF1CD49ACBB5721A7uL),
                    (-1, -57, 0x924BF8A339B95DECuL, 0x48E42C956E19CFCFuL),
                    (+1, -61, 0x8E63AD0B953957B0uL, 0x1C4BE0F25CCF617EuL),
                    (-1, -68, 0xF3331C25EA135B48uL, 0x1FD4D2AF04D2D7ECuL),
                    (-1, -68, 0xAFA474D8EF4A729AuL, 0xF338743BCB68F39CuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX7p50Table = new(new ddouble[] {
                    (+1, 3, 0xD68D88FFF34DFE5CuL, 0xD7873BAD3C2D233EuL),
                    (+1, 0, 0xE0F52AD62ADEA7A7uL, 0xE6CE6C07132543E3uL),
                    (+1, -10, 0xF8E1ADA8023B11CEuL, 0x600CAE845A07F011uL),
                    (-1, -13, 0xF88179600F5136E4uL, 0x6FF0EF17195966A9uL),
                    (+1, -16, 0xF4360027268F02D5uL, 0xC554C4C8C93065A2uL),
                    (-1, -19, 0xEC2ED798FDDF302FuL, 0xCD25F8D96ACD027EuL),
                    (+1, -22, 0xE0BE2117B5C50D1FuL, 0x6AEE39E28C283A26uL),
                    (-1, -25, 0xD2538906F89E39F6uL, 0x08F45991ADC07CADuL),
                    (+1, -28, 0xC175F0C7935592E2uL, 0xD39855A8CC834213uL),
                    (-1, -31, 0xAEBC36F28DB36F9EuL, 0xE3246C38B33279D5uL),
                    (+1, -34, 0x9AC5986FD2F6B699uL, 0x198446B8CC851C6BuL),
                    (-1, -37, 0x863222E86CBA2FF2uL, 0x21350AB054C08E83uL),
                    (+1, -41, 0xE3374D664574B651uL, 0x8FE4F55D9474F272uL),
                    (-1, -44, 0xBB1F0A777B56931FuL, 0xAF49BAA611A9D0B3uL),
                    (+1, -47, 0x951345E7405BDA0FuL, 0x0C50606E2CCE6255uL),
                    (-1, -51, 0xE3C2ACBAE8D52BCDuL, 0x7137424D7567B5FCuL),
                    (+1, -54, 0xA44EEBFC30CB8A90uL, 0xA9BFE5D4977178C9uL),
                    (-1, -58, 0xD94B8084127BB1D1uL, 0x82BA5C30E29DA143uL),
                    (+1, -62, 0xF50434AE3A0EC7F4uL, 0xEC1E85CC9EB5B81AuL),
                    (-1, -66, 0xB200B87B30E05982uL, 0x5195240C8F2DA3E2uL),
                    (-1, -70, 0x88ED7DC1267C289AuL, 0x690082B47F2F32EAuL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX7p75Table = new(new ddouble[] {
                    (+1, 3, 0xDD95AB0316F77638uL, 0x20B64E81ACADCD69uL),
                    (+1, 0, 0xE112E0FE84740C03uL, 0x25ECD036535E140CuL),
                    (+1, -10, 0xE2F23201E11877C1uL, 0xA7D9833E1214FA3CuL),
                    (-1, -13, 0xDC27C88B6BADCF6CuL, 0xF9F8B21ABC1C1CECuL),
                    (+1, -16, 0xD26319E6985955D1uL, 0x263360646174DBCFuL),
                    (-1, -19, 0xC60BDBD618438D95uL, 0xFED4A4A95F0818EEuL),
                    (+1, -22, 0xB79AE596E3B856F1uL, 0xC0FBF98406BCF73DuL),
                    (-1, -25, 0xA793BAE70B6B19ADuL, 0x82493C1EE028ADFBuL),
                    (+1, -28, 0x967E1034C3B57D18uL, 0xF939C348827182B6uL),
                    (-1, -31, 0x84DFA73BA2ADE06AuL, 0xF793BFB1F547CBB3uL),
                    (+1, -35, 0xE66DA8CFCCBA9E99uL, 0xEDC9812ECA8B50A3uL),
                    (-1, -38, 0xC3EBD50949A5DEC5uL, 0x41FD1A55098CA024uL),
                    (+1, -41, 0xA2FF6AB1E70850C1uL, 0xDE4C2AA59AC96EBAuL),
                    (-1, -44, 0x844A2D5D09E29736uL, 0x59C43334B9E7D2A7uL),
                    (+1, -48, 0xD08F1567EC4D0988uL, 0x22BB477CB9575152uL),
                    (-1, -51, 0x9E98720E12BD0C54uL, 0x2DCE34BF0C329DA2uL),
                    (+1, -55, 0xE61EDC068DE47DCFuL, 0x37EB5CAEE69B57C9uL),
                    (-1, -58, 0x9C1D6488897D6C04uL, 0xD97C27362624190BuL),
                    (+1, -62, 0xBDD42DB89C41857CuL, 0x728FFD50EC8AD4ABuL),
                    (-1, -66, 0xB77BC0443F714D01uL, 0x3F4243D4254EA2A7uL),
                    (+1, -71, 0x80817CE45798B085uL, 0xA300162A0E79B010uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX8p00Table = new(new ddouble[] {
                    (+1, 3, 0xE49EB02D10E55196uL, 0xF242A9AA48D3D599uL),
                    (+1, 0, 0xE12E01B7AC310C47uL, 0xA0C02197190D2A7DuL),
                    (+1, -10, 0xCF7B386FB913CD4FuL, 0xAF3169398C98F96FuL),
                    (-1, -13, 0xC3AF318591B98410uL, 0x59222768F8243D4EuL),
                    (+1, -16, 0xB5F6652457011276uL, 0x378D8D53A7B0784DuL),
                    (-1, -19, 0xA6D0A1891EEE9E9DuL, 0x784D0CD6C29F0084uL),
                    (+1, -22, 0x96BE688E4C9CE016uL, 0x631DA77C2D9E609EuL),
                    (-1, -25, 0x863B7D4C2A96D8BCuL, 0xC2A5A30753076AB7uL),
                    (+1, -29, 0xEB748E66AF8F79CCuL, 0x408F0E17C53B790BuL),
                    (-1, -32, 0xCB4064612B46980AuL, 0x3C62E7BAC5D04764uL),
                    (+1, -35, 0xAC863DD52ACBDA00uL, 0x70AD6F74AEF2DA9CuL),
                    (-1, -38, 0x8FCFC66928A4E813uL, 0x890A676DDDF5579EuL),
                    (+1, -42, 0xEB072A1979DF0541uL, 0x532CBDD81B56010BuL),
                    (-1, -45, 0xBBCBE5B55A2CBEC0uL, 0x2AEF7880C7A935FFuL),
                    (+1, -48, 0x9236376C9E9D3359uL, 0x1F8AACE2400FDAAFuL),
                    (-1, -52, 0xDCAA87EEAAAC308AuL, 0x6457F55E84F9B952uL),
                    (+1, -55, 0xA00E6D3CF19BDFFEuL, 0xEB48593F511EBF41uL),
                    (-1, -59, 0xDC09FC0E9452884EuL, 0x0FF68E10B19BAD04uL),
                    (+1, -62, 0x8B7D52BC15546888uL, 0x55E672A433159A76uL),
                    (-1, -66, 0x9907C6BC95689464uL, 0x4E5F973281533E4DuL),
                    (+1, -71, 0xE7C940F6FC22C21CuL, 0xEB6E78A12DB6C7BBuL),
                });
            }
        }
    }
}
