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

                    ddouble dp = a * (1 - s) * RSeries.Value(4 * k);
                    ddouble dq = b * (1 - t) * RSeries.Value(4 * k + 1);
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
            public static readonly ddouble PadeApproxMin = 1d, PadeApproxMax = 12d;
            public static readonly ddouble TableBin = 0.25d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static FresnelPade() {
                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    PadeX1p00Table,
                    PadeX1p25Table,
                    PadeX1p50Table,
                    PadeX1p75Table,
                    PadeX2p00Table,
                    PadeX2p25Table,
                    PadeX2p50Table,
                    PadeX2p75Table,
                    PadeX3p00Table,
                    PadeX3p25Table,
                    PadeX3p50Table,
                    PadeX3p75Table,
                    PadeX4p00Table,
                    PadeX4p25Table,
                    PadeX4p50Table,
                    PadeX4p75Table,
                    PadeX5p00Table,
                    PadeX5p25Table,
                    PadeX5p50Table,
                    PadeX5p75Table,
                    PadeX6p00Table,
                    PadeX6p25Table,
                    PadeX6p50Table,
                    PadeX6p75Table,
                    PadeX7p00Table,
                    PadeX7p25Table,
                    PadeX7p50Table,
                    PadeX7p75Table,
                    PadeX8p00Table,
                    PadeX8p25Table,
                    PadeX8p50Table,
                    PadeX8p75Table,
                    PadeX9p00Table,
                    PadeX9p25Table,
                    PadeX9p50Table,
                    PadeX9p75Table,
                    PadeX10p00Table,
                    PadeX10p25Table,
                    PadeX10p50Table,
                    PadeX10p75Table,
                    PadeX11p00Table,
                    PadeX11p25Table,
                    PadeX11p50Table,
                    PadeX11p75Table,
                    PadeX12p00Table,
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                int table_index = (int)Round((x - PadeApproxMin) / TableBin);
                ddouble w = x - (PadeApproxMin + TableBin * table_index);
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

                return (f, g);
            }

            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -2, 0x8F4E3011F8290ADDuL, 0x706477F9EEB97F4DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -5, 0xFCE3F9EC91BEFDB3uL, 0xFA96584FC59E34B3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -1, 0x927241A77D2647F4uL, 0x4D8DB7083E465650uL), (+1, 1, 0xAF28370EB7028BBDuL, 0xD3565D4855DE4E6EuL), (+1, -5, 0xA2227255D53AD559uL, 0x3AC7D19D103EB6F5uL), (+1, 1, 0xA62317F33C3C14DEuL, 0x45F6161AF5EB2A69uL)),
                ((+1, -1, 0x990C0810AA4853E5uL, 0x2F02A68B4C1B8F71uL), (+1, 1, 0xECE94CBC460DF962uL, 0x4AB4AAC48D2A2DB6uL), (+1, -6, 0xE9B7B4DD6A6F8EAAuL, 0x290D9F3EBCAFCD39uL), (+1, 1, 0xD66AE6FDAFEC01F3uL, 0x7CA533A10CAE431EuL)),
                ((+1, -2, 0xD79BC31576934C04uL, 0xA778EF94E3502296uL), (+1, 1, 0xCDF19793582E300AuL, 0x676A38CF3F1C1D03uL), (+1, -7, 0xC37297B9089DEB75uL, 0x4962EF2EC2CEE4B7uL), (+1, 1, 0xB2558B4F007102C7uL, 0xBBAEE19AFF110989uL)),
                ((+1, -3, 0xD6F90AC55C6B04CCuL, 0x9D39F228E8EB8622uL), (+1, 0, 0xFDC4F92170DD99ADuL, 0x789E46FE9573C772uL), (+1, -9, 0xBEF6B30C064F6E5AuL, 0x8C260C3F2F71693FuL), (+1, 0, 0xD29E01439163329BuL, 0xDED243F73FA69EF3uL)),
                ((+1, -4, 0x9E2E873999A1D224uL, 0x15A0D8289232CC4CuL), (+1, -1, 0xE73351CA7A4C0CD7uL, 0x80676A0FEC9E1EA3uL), (+1, -11, 0xF42A2C82E30912BAuL, 0x819F9BFF7DC3D808uL), (+1, -1, 0xB817A444B01DD40FuL, 0xF9351C59D5D88E56uL)),
                ((+1, -6, 0xAB0515925A0AA9A4uL, 0x102401E4E762CF02uL), (+1, -2, 0x9DD50DEBA8053320uL, 0x6DC9DF8FC41C3AE4uL), (+1, -16, 0xBE05CEF5E55058C9uL, 0xB29C766A9C088814uL), (+1, -3, 0xF146C3F9DED8DD92uL, 0x2D1643F87BB6A515uL)),
                ((+1, -8, 0x82717DD30D86C6AEuL, 0xC85CDFFEFA265E7EuL), (+1, -4, 0x9FD127EA82639C12uL, 0x3788013CF2CE5AE0uL), (+1, -16, 0xBCD45F0A25E353B9uL, 0xB047F78EA05B54BFuL), (+1, -5, 0xEA91B15D7CF047B9uL, 0xCFCE01CC7DFCED38uL)),
                ((+1, -11, 0x8221E7E73C93508EuL, 0x50CDAB8ADA2601CAuL), (+1, -7, 0xE6D38035C4BD9200uL, 0x509AF7777D60A812uL), (-1, -19, 0xAC8C819C959FA3D8uL, 0xDB32EAF37ADBCFC6uL), (+1, -7, 0xA2A844E353B52440uL, 0x04878877E85649EAuL)),
                ((+1, -15, 0x8057532A80B1DB26uL, 0x66C3EBF9D79C43F4uL), (+1, -10, 0xD85505D57135D081uL, 0x3B941C2BDB83D1BEuL), (+1, -22, 0x8E12E088C8AC4F9FuL, 0xB72B732FCE4A871AuL), (+1, -10, 0x92614016F306C073uL, 0xBB3D6DCA1C92BAF3uL)),
                ((+1, -27, 0xEEAA43522DB9D079uL, 0x2564BF7CEF2DFE03uL), (+1, -14, 0xCB23C177CF2FFD79uL, 0xA821E39598852551uL), (-1, -27, 0xE758037E664B75F5uL, 0xB23C13260EC96C45uL), (+1, -14, 0x842E1FE3A472F2FBuL, 0xC65539FCF52C3181uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0xF2937B7DB10E1C47uL, 0x46B23ACE80EE4800uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -5, 0x9DF33944D93CAC74uL, 0x93BE83EAB64BE662uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xF1F6C5E583A367B0uL, 0xE337B0C30DA36208uL), (+1, 1, 0xA896CE58D2369F42uL, 0x5A5089E48DF4AFA5uL), (+1, -5, 0x88BFD3EECA5019E4uL, 0x4052A559ECE734BFuL), (+1, 1, 0xAB244BA66DA96A2FuL, 0xA5360F053FA81B49uL)),
                ((+1, -2, 0xFA4F01D8E6EC1AE0uL, 0xA09009214E0B4E9FuL), (+1, 1, 0xDB39937EA9061E87uL, 0xCBB75807373A3927uL), (+1, -6, 0xBE37D93CCCF7855BuL, 0x14C04CD0DBFF6479uL), (+1, 1, 0xDFEC5AD876D2B754uL, 0x45EB871874F18CE6uL)),
                ((+1, -2, 0xAB53F143FB912CF7uL, 0x31807C26017A725DuL), (+1, 1, 0xB6ECA83198D1061BuL, 0x219AABB984F66AEEuL), (+1, -7, 0xA336064431E006B8uL, 0xE34E2A07740FCFEEuL), (+1, 1, 0xBAF3701997EC5D3DuL, 0x7C8F04D8B0CD5550uL)),
                ((+1, -3, 0xA5A746C62D172796uL, 0x17617C7DAB6FAF23uL), (+1, 0, 0xD7F75713168D4A7BuL, 0x4DA34546C384E3C8uL), (+1, -9, 0xBB8F7BCFE7D73F71uL, 0x251AEFFECF635E53uL), (+1, 0, 0xDBC20D49B965A289uL, 0x137C09AA5232B82FuL)),
                ((+1, -5, 0xEBBD44A76A8685D3uL, 0x80B64923E90ECFFCuL), (+1, -1, 0xBC20197B142EC693uL, 0xA6050E69DD56ABDFuL), (+1, -11, 0xB0C1C9AFE6FAD4B3uL, 0x82A242EF52FFB397uL), (+1, -1, 0xBDB733906A756911uL, 0x99AC6DB3C7B581CCuL)),
                ((+1, -7, 0xF44577CF26C0A779uL, 0xFED06526529F097AuL), (+1, -3, 0xF4FF61EE5880C00DuL, 0xBF1314A614D0EDD6uL), (+1, -14, 0x8759982A1B29F554uL, 0xACF1C992EC68DEEAuL), (+1, -3, 0xF3AB9D43504D4ADDuL, 0x427AAFD4ADF1F10AuL)),
                ((+1, -9, 0xB35CEC9031B253B0uL, 0x62D0966A06FF9874uL), (+1, -5, 0xEBFC17D38659E760uL, 0xAF469CED98E8BB56uL), (+1, -17, 0x994023C8B7B62A77uL, 0x8145CB0C5D92C5EDuL), (+1, -5, 0xE63D09DA0D7A91D8uL, 0x94C483D3D5225E0EuL)),
                ((+1, -12, 0xA984E457D7C5DEDCuL, 0x5F09DC6BE19E9929uL), (+1, -7, 0xA199AEACDBBD0383uL, 0xA5596CC80C774230uL), (-1, -21, 0xA052701AF332289DuL, 0xB34210A29941168FuL), (+1, -7, 0x99A6623409B0A0C7uL, 0x5B04CEF6A414653DuL)),
                ((+1, -16, 0xA0F3D1876DEA3C44uL, 0x4E66CC735D249FD3uL), (+1, -10, 0x8F153ABB4CF126FCuL, 0x7A34472EAF95C57DuL), (+1, -24, 0x830EF4C4CC19E46CuL, 0x3B60DD5EE1533F19uL), (+1, -10, 0x8376584AA0376E59uL, 0x43CBF2B032AE3A8BuL)),
                ((-1, -32, 0x906057964E2D40D9uL, 0x72BCEAC6E8CA4246uL), (+1, -15, 0xFCAD6BF716920132uL, 0x7B6F212B1713D74EuL), (-1, -29, 0xD501C3303CF071D6uL, 0x6724CCDC352BF273uL), (+1, -15, 0xDD3221B2807E6B0DuL, 0x5140A36216A189EBuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0xD04CEBD613D1D474uL, 0x4D45F1B614694714uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -6, 0xCCE1587E5D8A88CFuL, 0x26AE7148C6844DF8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xD079D0052F1893CFuL, 0x37D4E0D2C47B8E38uL), (+1, 1, 0xA53017C266DBE7FCuL, 0x2C210EFC17E4518BuL), (+1, -6, 0xDABB3CB6240B505EuL, 0xC1534CA9840B00E5uL), (+1, 1, 0xAE4D98EDC8D0FCA6uL, 0x2853C8E1C4B5AB46uL)),
                ((+1, -2, 0xD45AC3006C3203C7uL, 0x4DDA7A1D7A0BB4A4uL), (+1, 1, 0xCFDB6CFDECCB4501uL, 0x972F17E931E56ACDuL), (+1, -6, 0x9BB73FAEAA0C35B9uL, 0x0AE54B3A1EACD7A3uL), (+1, 1, 0xE634812F71F75B7AuL, 0xD8445F5BDC42B5B3uL)),
                ((+1, -2, 0x8C4667C062346CE1uL, 0xCC40288E208C2250uL), (+1, 1, 0xA6523D3AC6D782B8uL, 0x8C3E585985786822uL), (+1, -7, 0x8AC5399C1674EB49uL, 0x216770EBB0F0F505uL), (+1, 1, 0xC0CBCE9970649AB4uL, 0x296B77501417EEC6uL)),
                ((+1, -3, 0x81D107A136641A7FuL, 0x38E9E00C27FD117CuL), (+1, 0, 0xBABB4F13F741AA2DuL, 0x8C82F7849EC640E8uL), (+1, -9, 0xAB95125739E7C3A2uL, 0x68B3AA88777A8AEFuL), (+1, 0, 0xE2235281200B2475uL, 0x139B4169B5270A1DuL)),
                ((+1, -5, 0xAE7B11BD72FB479AuL, 0x7B51ADBD3CACB294uL), (+1, -1, 0x996A896D7FBBA307uL, 0x0C491BF628BDA5DEuL), (+1, -11, 0x94A10E96FC6D6C37uL, 0x3A07E0496229DBC7uL), (+1, -1, 0xC1E13D5CDC0498CAuL, 0x8B31DFDEC4FD466EuL)),
                ((+1, -7, 0xA9252D426E5A4ACAuL, 0x8C033D90CB11E5F5uL), (+1, -3, 0xBAD0F901EB174CB0uL, 0xBCB1EA628D27386EuL), (+1, -14, 0x95C66E9EE0613271uL, 0x773278EA22F53DBDuL), (+1, -3, 0xF63028050C7787DCuL, 0x6A27FF282DAB8489uL)),
                ((+1, -10, 0xE55950A31E794ADBuL, 0x132246E4048C0641uL), (+1, -5, 0xA6AB89FF0D2DE293uL, 0x5373BC6B98795BF2uL), (+1, -18, 0xBAD43993EFE806C9uL, 0x16D728567778BEAEuL), (+1, -5, 0xE4F151C225387423uL, 0xE51F8C5EC29EB136uL)),
                ((+1, -13, 0xC553A2FBF4F747F8uL, 0x1C792DF4CD5DE822uL), (+1, -8, 0xD11CA08B0771115DuL, 0x576FB82D0E655DEBuL), (-1, -24, 0xDBB021DCB81D4E58uL, 0x6DC83E2163E086EAuL), (+1, -7, 0x95AA7C626FCB85A1uL, 0xC619F9473EC367CFuL)),
                ((+1, -17, 0xA7013C84E5879A5BuL, 0x6B7A4FB18BEE9B8DuL), (+1, -11, 0xA75D5AE071347D42uL, 0x868BFC74C1852DDCuL), (+1, -27, 0xB153825802074C62uL, 0x83675582FD4B1CFFuL), (+1, -11, 0xF998C13E2B251392uL, 0xB66DB66F861E0D0BuL)),
                ((-1, -31, 0x833FE1ED26E84920uL, 0x6606086867952F42uL), (+1, -15, 0x8305FE5DE509BAFCuL, 0x5177C7AAC2D896D6uL), (-1, -31, 0x8E50999F601CEAE4uL, 0x2D4DEF42E679B8FFuL), (+1, -15, 0xCB578931E0248C89uL, 0x4B7B22B793F5ED97uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0xB585B305810DF0E3uL, 0xC3D5B9A077C28D29uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -6, 0x89F6409F10239088uL, 0x1A9782F2CEC20AA8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xBECB8B0934A545D7uL, 0x4F206ACFCB68D630uL), (+1, 1, 0xA7F76AA16221CF19uL, 0x904A24F53A174207uL), (+1, -6, 0x9D71897036E243D3uL, 0x1EDB1F10EF05081BuL), (+1, 1, 0xA9A2E60F1AB813AFuL, 0x556AFC3994F82AFBuL)),
                ((+1, -2, 0xC6FF07893A842D8FuL, 0x8ED7EC4DF788A12BuL), (+1, 1, 0xD4602952D5EC65D8uL, 0xF8898EC0E616F039uL), (+1, -7, 0xE2FC343F4DC17F0DuL, 0x3E98141BB26928C8uL), (+1, 1, 0xD9BAC676F38895ACuL, 0x7F3BB1289C6C6167uL)),
                ((+1, -2, 0x845893F2DCF0974CuL, 0x2FE494AC2C916743uL), (+1, 1, 0xA95539B4D60AAA35uL, 0x31D657E5D3C17997uL), (+1, -8, 0xC8E8EBE58B5FBF8BuL, 0xCFF33925EDFFCE3DuL), (+1, 1, 0xB0E76853F1CC00A7uL, 0x69825527EB6A5392uL)),
                ((+1, -4, 0xF47F2D21697259DEuL, 0xEB49D3A55FD1E8D8uL), (+1, 0, 0xBC0D3B5906EBC3C8uL, 0x308D42C1A0B9DFA9uL), (+1, -10, 0xFB9A7D309E3C5CC5uL, 0xCFFAF550D9581094uL), (+1, 0, 0xC8E9EEFB4CBDA047uL, 0xD10CD82D9F11E0D3uL)),
                ((+1, -5, 0xA22DEFA9AE675BCBuL, 0x429EB678BD9316A0uL), (+1, -1, 0x97C71965E6A76C7DuL, 0x80D80E61DB42A07DuL), (+1, -12, 0xD06A04F0B492DE1BuL, 0x42C37D0F1600F222uL), (+1, -1, 0xA66E79131C329C9EuL, 0xB09C8F05A9AFF115uL)),
                ((+1, -7, 0x9A0916F4B8E82CABuL, 0xC1865DD4981E669AuL), (+1, -3, 0xB44CD5FBFB4C434AuL, 0x5755C7048F9BEF20uL), (+1, -15, 0xDCA67A0559DFEF74uL, 0x5CF8AF49B7511C96uL), (+1, -3, 0xCBB50B505ADAAE05uL, 0x29BA119EC5BB8F19uL)),
                ((+1, -10, 0xCA58692FB6BBD427uL, 0xD9000F059C1C99A1uL), (+1, -5, 0x9BC13F5A836710EAuL, 0xC107DD2182D2DAFEuL), (+1, -19, 0xE3BE4CFA56F1562CuL, 0x86B67EF81D0A50B3uL), (+1, -5, 0xB61E328522C2312FuL, 0x016979D40C500126uL)),
                ((+1, -13, 0xA7096E8B16500D2AuL, 0x13E7CEB9F565DF27uL), (+1, -8, 0xBB9A1424FE4F8CE5uL, 0xEDA679B2861ADAC9uL), (-1, -30, 0xE68BD327FE1B9399uL, 0x8293469600B83C0CuL), (+1, -8, 0xE43A702C17A1BC92uL, 0x78516DB61C27887FuL)),
                ((+1, -17, 0x8513160AF295FAE2uL, 0xEBB8B7F0525BA664uL), (+1, -11, 0x8EA44D3159B028A1uL, 0xDFCCA3AEAFA3B331uL), (+1, -32, 0xCA42969F4382048CuL, 0x2A1D60FC796944D9uL), (+1, -11, 0xB5C8F754AFDA33C4uL, 0xA0E809D6A3743C74uL)),
                ((-1, -34, 0xEA64BB376D8D8D10uL, 0xF192ECEC4B3A94E1uL), (+1, -16, 0xD0F8005FAD67D6E9uL, 0x8B1CEAC86AF385AAuL), (-1, -36, 0xDC22A4B0044B67EEuL, 0x7268A6AC4006C38EuL), (+1, -15, 0x8CE482912E9A0720uL, 0xA7F7505D6571956DuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0xA057A3F1FA1F9F6BuL, 0x85C0AEF269FE53E2uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -7, 0xC074C92FBB3FA40AuL, 0xD7508BACE1C0BAD9uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xAA15B14B7C4916BBuL, 0x42B53A3CDF7EC069uL), (+1, 1, 0xA5F194ACCA58BF61uL, 0xF1742145297D87FDuL), (+1, -7, 0xBD7BEC595B90A074uL, 0x52A46F1D750D1E86uL), (+1, 1, 0x970327581434732AuL, 0xC5631728FD7E250AuL)),
                ((+1, -2, 0xACE2F170B6CB712DuL, 0x4C67790D5B2CDA5FuL), (+1, 1, 0xCB0892EAE06B03A1uL, 0xCA64522F7538A692uL), (+1, -8, 0xE789863FA14C2507uL, 0x625F1073ADD0A136uL), (+1, 1, 0xA95AAA4D3786061AuL, 0x978F91449AAD3ED5uL)),
                ((+1, -3, 0xD9975C72744A7737uL, 0xB7B85F7B6F3A4D53uL), (+1, 1, 0x997B37CEC852D911uL, 0x614EB86BC219F577uL), (+1, -9, 0xA021ACC095FDD835uL, 0xED1744E61851A353uL), (+1, 0, 0xEB3FDB3DF68CFC5CuL, 0xD53EFF64C39C3326uL)),
                ((+1, -4, 0xB83BC21064F1D1A1uL, 0x6BD08EBBC99D7505uL), (+1, 0, 0x9DD7A660CD226773uL, 0xA53C850F0B8B7473uL), (+1, -11, 0x96F0D212F0C1F1D1uL, 0xC6D317CC59C98A21uL), (+1, -1, 0xDE3F59984D46EDD8uL, 0xF61BBAD6314A61BAuL)),
                ((+1, -6, 0xD6AEFA1F2258E82AuL, 0x98252F10E0333B60uL), (+1, -2, 0xE5068985B187F245uL, 0x25C7A5A0CE5EDB44uL), (+1, -14, 0x9B2F2D85FBA917C5uL, 0x2238BAD917C0CDC5uL), (+1, -2, 0x93BFA50BAE1DC748uL, 0xED3D77BD9EC38DD6uL)),
                ((+1, -8, 0xA82385A54B782E12uL, 0x7DC9C9CA41ADD1B9uL), (+1, -4, 0xEAA230B6056DC04AuL, 0xEB2672B0B33E4F80uL), (+1, -18, 0xAAB47594EB79E3E5uL, 0x775403562E0C8F3EuL), (+1, -4, 0x8A06FDA8672A3EA6uL, 0xAE409FF8D77111C8uL)),
                ((+1, -11, 0xA26CDD6569428D16uL, 0xE22FE1439D580F56uL), (+1, -6, 0xA3F41CD1B36BEB1BuL, 0xB43B11203E909C35uL), (-1, -25, 0xD36A700B95E3FE20uL, 0x8848842E4150F2EBuL), (+1, -7, 0xAE7C8AFC1BB44EE1uL, 0x67EBA0941265B93FuL)),
                ((+1, -15, 0x953CEB138C6CD859uL, 0x95E87FDC86DE2C11uL), (+1, -9, 0x8E367BF423F79EF4uL, 0x330817F4716F4E58uL), (+1, -28, 0x93FFBA6E88CD3D33uL, 0x2D5B477F5775AD91uL), (+1, -10, 0x872DC610CC215F54uL, 0x24324E20C2FBDFABuL)),
                ((+1, -33, 0xD7D4C1F5CFF3BA6DuL, 0x54DDEBA9EAC19BDFuL), (+1, -14, 0xEA72EF1DD65F85A1uL, 0x25A269F4CEDFE94FuL), (-1, -33, 0xD6BEFE0516F81338uL, 0x77F16BE50C5455B2uL), (+1, -15, 0xC2B1C2DBE67F6913uL, 0x21CE62B80C00EB28uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0x8F5414D8583FC58BuL, 0x42FF96B3FC954BCFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -7, 0x8A92EAA3061BEB4EuL, 0x611CE90F500465FAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xA5672C97446BF228uL, 0xBA45C03B8EE90920uL), (+1, 1, 0xAF0CD9709C59879EuL, 0xE2AA5BE0E06EDD7AuL), (+1, -7, 0x9FA440357C3E6636uL, 0x4674D5BFCFB6C8D0uL), (+1, 1, 0x9A0FBE63B56F6077uL, 0x9FA015EA98698859uL)),
                ((+1, -2, 0xB0F4C415A813C4CCuL, 0x9A12BA2F25117659uL), (+1, 1, 0xDDB82ED7430BCBC3uL, 0xE40176D08958147AuL), (+1, -8, 0xC837F3C1662FB1DAuL, 0xB2C80BCF9D62BF2EuL), (+1, 1, 0xAECD4E789C331388uL, 0x555330DC006EAE44uL)),
                ((+1, -3, 0xE62E96105AE3298BuL, 0x07A8625F15CBAA4BuL), (+1, 1, 0xAB52130D49D562CDuL, 0x0FC978E2DE7EB433uL), (+1, -9, 0x9431D057CF1D31C4uL, 0xF3B4EFBE475F0378uL), (+1, 0, 0xF454B01218DA47ACuL, 0x7601989513CA0730uL)),
                ((+1, -4, 0xC6F21372D04D9B3FuL, 0x886D84F607ED19F3uL), (+1, 0, 0xB255121DEF29D502uL, 0x6A67406A9DF9C94AuL), (+1, -11, 0x8D1C5842E08F2167uL, 0x8104D481B17519A4uL), (+1, -1, 0xE750B10C7A876DF8uL, 0xD36A6644AF639137uL)),
                ((+1, -6, 0xEA4C6FAF2138F3BCuL, 0x2B00B401FCB067B8uL), (+1, -1, 0x81E439DA6E196AE5uL, 0x4C90B15E6D202E09uL), (+1, -14, 0x9BF182D3DA7D01BCuL, 0xF198D1418ED7A89FuL), (+1, -2, 0x99A43C5E7292B077uL, 0xBB4C7F7EE61DF7F9uL)),
                ((+1, -8, 0xB7FD0F68E75CB020uL, 0x896897DA5419E3FFuL), (+1, -3, 0x84A636ED9EE3C40AuL, 0x8194B920D4EDFD5BuL), (+1, -18, 0xA66C40673E14E001uL, 0x71F7CC7F0BD234C0uL), (+1, -4, 0x8F206F9EF4943163uL, 0xC81C3AD724781FCBuL)),
                ((+1, -11, 0xB0EA5A259ABDA167uL, 0x51A4A3832C6D584EuL), (+1, -6, 0xB795F6B335F89279uL, 0xCB34AA9036B50F49uL), (-1, -27, 0xAEED7B7EFE21F506uL, 0x28F96FE1FD80D7D5uL), (+1, -7, 0xB452AED38EFC4E72uL, 0x0E7A08040DB0A392uL)),
                ((+1, -15, 0xA0E114844A9D5C3EuL, 0xC0AB5A5FAF9C1CD6uL), (+1, -9, 0x9CB7149436513F2BuL, 0x4F30805BDD81D22CuL), (+1, -31, 0xE8E45379CC549748uL, 0x54BE5B43709B10DBuL), (+1, -10, 0x8B749977C169C032uL, 0x124847E622100414uL)),
                ((+1, -34, 0x8403DE0DC8934475uL, 0x40187DB8CD66FE59uL), (+1, -14, 0xFCB796C02B6C867DuL, 0x4689E0305A2C1595uL), (-1, -35, 0xA17ED71016131D05uL, 0xA2143B257C1B08D0uL), (+1, -15, 0xC9D60A237D7454F5uL, 0x73B13992C8AFD364uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -3, 0x8170D0D60DD5E6ADuL, 0xA59A61AF421F3D0DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -8, 0xCD3F2CFC6CDB1857uL, 0x7E3D18D866C62C81uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0xE6D0D320DB7522B5uL, 0x19E842C2E0C43FFBuL), (+1, 1, 0x8B07D4C3F11D62A3uL, 0x9506B56CA084925EuL), (+1, -8, 0xEB9CF99DB4B045FAuL, 0xCB49CBDD3D791B99uL), (+1, 1, 0x930F802C0088DFA7uL, 0x8FCD7B05277E2197uL)),
                ((+1, -3, 0xC1E33D83E25307B7uL, 0xC9084058D4AD56D8uL), (+1, 1, 0x8CA22F2973FE8B3EuL, 0xBB7A71A6333A4869uL), (+1, -8, 0x85DB31F84A6BDD3AuL, 0x9435D724AF311053uL), (+1, 1, 0x9BEFD7C1F29279B6uL, 0x491B86105B4930A6uL)),
                ((+1, -4, 0xC37D1CEA21A9CF62uL, 0x4382130D629D9547uL), (+1, 0, 0xAC270413B5399AC2uL, 0x298522A2D19736BEuL), (+1, -10, 0xAA916C5A8EE7C6FDuL, 0x9AFC729DE59C983DuL), (+1, 0, 0xC6DCC6B72DAF6AE4uL, 0x9177969855EF4B1AuL)),
                ((+1, -6, 0xFE37C1D703E6BA81uL, 0x6ADD83BDE9BCFA31uL), (+1, -1, 0x8B2D0A444D41EFBFuL, 0x2DB4D9C1E5B06CBBuL), (+1, -13, 0xF7B96F37A6661BC0uL, 0xB7B0B6DA906F1D48uL), (+1, -1, 0xA69E32DD998DD3F0uL, 0x398A37BBB249AF44uL)),
                ((+1, -8, 0xD4D80D23226D91FFuL, 0xAB21CCAE1FC7E394uL), (+1, -3, 0x9811C32237BE74B4uL, 0x1B3754CF4F07D39DuL), (+1, -16, 0xA34A2A00FE6ED79DuL, 0xCE20A21A65326C91uL), (+1, -3, 0xBBC579A66F71EC3EuL, 0xB365D8AD21E5B37FuL)),
                ((+1, -11, 0xD4CABE14B111D826uL, 0x41B06C016183D93DuL), (+1, -6, 0xDB6571FEFF0D1FA1uL, 0x44B5EC7EBA7943ABuL), (+1, -26, 0x9502C198F10B0F03uL, 0x9DEC824955542580uL), (+1, -5, 0x8B0160EA192BC1C6uL, 0x66A5692896E98696uL)),
                ((+1, -15, 0xC55F258E715201F6uL, 0x36C2598DFADD4B42uL), (+1, -9, 0xBF586F19BA7D6AC7uL, 0x634CF3407B136A9AuL), (-1, -30, 0xD9C2CCF825E4FA8AuL, 0x305319E5DD333E81uL), (+1, -9, 0xF7613405B43FC4D8uL, 0xF143DE0A2B4FDFC3uL)),
                ((+1, -35, 0xC57DBF7CDCEFC81FuL, 0x8F2E3AC9A319DBEFuL), (+1, -13, 0x9B04D0069986AA74uL, 0xCA17BD67CB94A42CuL), (+1, -34, 0xA7909399D3833604uL, 0x9A5F1292F7A51372uL), (+1, -13, 0xCAD014847FF78A7CuL, 0x580DDDD1494C062DuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xEBDC9AE607ABC5F9uL, 0x478EF0E8C0B15768uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -8, 0x9BC9E6C87E915733uL, 0x6AEA38721E55F453uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0xD0EADDFEAAD15747uL, 0xA538BA285027B353uL), (+1, 1, 0x8833FF6DAFC9B58FuL, 0x234800DDDDEDD64CuL), (+1, -8, 0xC18343D4DD58BACAuL, 0x48E8DF08C41EA59DuL), (+1, 1, 0x933485987A447D88uL, 0x10652B58B964E576uL)),
                ((+1, -3, 0xAB8DB05A08FCAC3EuL, 0xA233FCF2CA606243uL), (+1, 1, 0x85C0DC8F17A56959uL, 0x15016D6BB82BEC9CuL), (+1, -9, 0xE073DABA6769BE68uL, 0xF437948666476FD1uL), (+1, 1, 0x9B0B0A4DBE02CA43uL, 0x60F3F84B3E76D031uL)),
                ((+1, -4, 0xA73F81D088DD1ADEuL, 0x0D7B70328D9FC816uL), (+1, 0, 0x9DB723A7D0A5460BuL, 0xAD6D83860376A4BCuL), (+1, -10, 0x916075158209F3F5uL, 0x53BE226A8BDFF2ECuL), (+1, 0, 0xC3296DEE591C0E10uL, 0x332E6F050F6FE25FuL)),
                ((+1, -6, 0xD02418D53476A18DuL, 0xF9542854D982336BuL), (+1, -2, 0xF3C2CF65D7FA6673uL, 0xB07CE0FB2C8775C8uL), (+1, -13, 0xD25F202D7B17129AuL, 0x6BC02A0BE106B076uL), (+1, -1, 0xA07DE499D75E1195uL, 0xD6D8362F0F212EFBuL)),
                ((+1, -8, 0xA503708A2D64F5C9uL, 0x5DF8F5D49A5D5281uL), (+1, -4, 0xFC92CD5C01D30291uL, 0xE5391E8F8D4E3956uL), (+1, -16, 0x8B0F0C7EB47A76BDuL, 0x1436F12E870EE75AuL), (+1, -3, 0xB0947406E97FBF60uL, 0x24BBD5F3B57F2305uL)),
                ((+1, -11, 0x9A5A95E2F1AA55F1uL, 0x72FE3C9092363C69uL), (+1, -6, 0xAB4641E6BDC3319EuL, 0x46A85E4A1E217CEBuL), (+1, -28, 0x96906DB564EEC2ACuL, 0xCD8AA6D08D7EE5EEuL), (+1, -6, 0xFDEA0E3EE73FC41DuL, 0x9CEFECC8CB22B5EBuL)),
                ((+1, -15, 0x83D1DFD558084ED7uL, 0xDDA6920E2EDD3A56uL), (+1, -9, 0x8B05E0F92FABDCECuL, 0x495DF9F56304575AuL), (-1, -32, 0xD1665FB3837F5B35uL, 0x6EFECFF2915709A2uL), (+1, -9, 0xDA460F23C01755E4uL, 0x6662926061B08673uL)),
                ((+1, -37, 0xF4A62CBAECB59588uL, 0xA8E69522285EEA74uL), (+1, -14, 0xCF10731F9A22BF4BuL, 0x0CD19AB51470D19CuL), (+1, -36, 0x99FD05965DFD35CCuL, 0x79E28D0B6C0063B2uL), (+1, -13, 0xABEB7C9550763BFAuL, 0x362EA18FA0B28254uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xD884242453B1DC84uL, 0x2FD56851DA9D7387uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -9, 0xF1A19CDAFE46BE0AuL, 0x02A0E46707B2061FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0xBF74BBBD7855CDACuL, 0xB6C778085A21862AuL), (+1, 1, 0x863890EDDF141E48uL, 0x2C7DD8BDA9032B92uL), (+1, -8, 0xA4618B8DDA396B06uL, 0xC570DC2044217EC7uL), (+1, 1, 0x95A7BC871AB79AECuL, 0xCEFC64873014B324uL)),
                ((+1, -3, 0x9B152E3BC0C9864AuL, 0x3386CF1C0CBE2526uL), (+1, 1, 0x81053B4DA75F0146uL, 0xD93777437E81F62BuL), (+1, -9, 0xC5828AA2B7664ADBuL, 0xF5C562C467B674A1uL), (+1, 1, 0x9E78817456AA633FuL, 0x98590EAA4C781CA2uL)),
                ((+1, -4, 0x93E7A80A0BDC2ED8uL, 0x7EB3C9E04DEA0C6EuL), (+1, 0, 0x940E006BB11D1463uL, 0xA7991564CFD030B6uL), (+1, -10, 0x82DEFF4523D19DA6uL, 0x2A7E503F9E4097A6uL), (+1, 0, 0xC6D0EC5BCE4FD5A6uL, 0xF021F8592CDB41DEuL)),
                ((+1, -6, 0xB2B75A78BBC3B3EFuL, 0x0ECB4D3DBBE7CF78uL), (+1, -2, 0xDD72DA0A4F534C84uL, 0xEA153C3E4E1F190FuL), (+1, -13, 0xBE6F24FC089F2B2CuL, 0x959F637989FAD2E6uL), (+1, -1, 0xA1C3F4187DB257ECuL, 0x921E6E54E978C3BAuL)),
                ((+1, -8, 0x888B6708B64C67B9uL, 0x879F6F13E46C7EF6uL), (+1, -4, 0xDCCB647C3127B890uL, 0x4F784D08451897A7uL), (+1, -17, 0xFCBE45A868BA82DBuL, 0x576B2FDCD0FF168CuL), (+1, -3, 0xAEF26F2C23F6E63EuL, 0x4CB2950AD521E004uL)),
                ((+1, -12, 0xF433A387BA3418DEuL, 0x38B19E456DC780EBuL), (+1, -6, 0x8F3423C0EB63CFE0uL, 0x5CC6267BC00EA7EBuL), (+1, -30, 0xBCADE3E70FEBF59FuL, 0x98590FED7E880E99uL), (+1, -6, 0xF5C502AE01DF2E7FuL, 0xD5EC930745BCDCACuL)),
                ((+1, -16, 0xC55E685828D46EECuL, 0x2FFDF362B0BC2631uL), (+1, -10, 0xDCDC35E758D82506uL, 0x59CD63045FF9B45FuL), (-1, -34, 0xF852B48C616FEFDDuL, 0xDB5249764D440A3FuL), (+1, -9, 0xCD2F2C218415FD40uL, 0x68B1DA43FE7E9638uL)),
                ((+1, -39, 0xE556BAAED71B188FuL, 0xB735834C7B156443uL), (+1, -14, 0x9B037B806E52AF0DuL, 0xA49512201CA36600uL), (+1, -38, 0xAD5B966AB6600A7EuL, 0xAFA52DE7CCD09D92uL), (+1, -13, 0x9C066FD55CC156BEuL, 0xC30CCE63F1C7C319uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xC80DC1765198F67AuL, 0x73668930C2CFFD74uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -9, 0xBEEEAE441D6DEFF9uL, 0x922FB3132C8C5ECCuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0xAF33A805C04BA640uL, 0x1DCBB320908E154FuL), (+1, 1, 0x83969BE6D339450EuL, 0xAEC4867E8649E051uL), (+1, -8, 0xBB161975DCE5761FuL, 0xD35969026AB7FE97uL), (+1, 1, 0xB7845755D3A2D9A5uL, 0x7ED21AF1936B3935uL)),
                ((+1, -3, 0x8B657E5970F8DBEDuL, 0xE03D44EEC9CAD213uL), (+1, 0, 0xF6D3E126C19FBACCuL, 0xC871A53522094C68uL), (+1, -8, 0x836CD3CA60C25F8DuL, 0x82270CB7019798BFuL), (+1, 1, 0xDBF156534E808748uL, 0x4498432ED6749E66uL)),
                ((+1, -4, 0x81C85C3C8262F519uL, 0x277B1012A05EBEFAuL), (+1, 0, 0x899566103F603232uL, 0x8E81A8D1737BAD1AuL), (+1, -10, 0xC0CB577A136D81A4uL, 0x842B4A10AF5A78C0uL), (+1, 1, 0x95601829AF0E8E3CuL, 0x1FB3DC16B5DF297DuL)),
                ((+1, -6, 0x983E5BDB7D362E00uL, 0xDB7CEE8D45347F9CuL), (+1, -2, 0xC71788CDB08C1716uL, 0x19EE28792B466BC3uL), (+1, -12, 0x95287936D3111F54uL, 0x10C030FE42D0D428uL), (+1, -1, 0xFFC291B8E8883B1FuL, 0xD9F9CAB91B3B6143uL)),
                ((+1, -9, 0xE0A4A30959F2F7C6uL, 0x45E40CCFA33BA915uL), (+1, -4, 0xBF3F8A4CAD5FB7C6uL, 0xD2EE9B20F7B3BCF4uL), (+1, -16, 0xCFE85F429362BC7AuL, 0x4A4902CAF15B5FAAuL), (+1, -2, 0x8E97B63AB5D4D0A1uL, 0xF4B7676421D1FBD2uL)),
                ((+1, -12, 0xC0E7D470AB32E467uL, 0xE660D170A6ECB9C7uL), (+1, -7, 0xEDFBFF2CD7414795uL, 0x3623048AF9213C92uL), (+1, -30, 0xA4B7929CBD78BCB7uL, 0xBECF5FCA06BEE7D3uL), (+1, -5, 0xCB590E77D763FD3DuL, 0xA927E52E9DCDECF4uL)),
                ((+1, -16, 0x94C85105D0516E5BuL, 0xEC16876778AC548EuL), (+1, -10, 0xAF3E5528BCB53CF4uL, 0x5CB2F5C640812857uL), (-1, -34, 0xC93EACE9DB5AE19AuL, 0xECCC79DE5B188E84uL), (+1, -8, 0xAA2B325D245E6FECuL, 0x49DEA51C094AC767uL)),
                ((+1, -41, 0xB910941AA1CBC177uL, 0xFA1A0E1FFCE64C95uL), (+1, -15, 0xE9B50B3AF5C97626uL, 0x2CCFD125B35A94E9uL), (+1, -38, 0x82FC4DDEE82B2823uL, 0x14FAFD6593204F29uL), (+1, -12, 0x804E03E3FCC99503uL, 0x439C1B6E8F49B4ADuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xB9E34B048A1E735CuL, 0x56E92DA7D5DFD407uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -9, 0x995DA1CE1FBE1851uL, 0xECF0653FE6EF3B29uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0x9F2A953C2B3350DAuL, 0xCDE8845C7AEF7C0CuL), (+1, 0, 0xFF7CB609E9482262uL, 0xFB38FF440ECE98A1uL), (+1, -10, 0xFF202258BAFF33FFuL, 0xDE14E2CB16E08A80uL), (+1, 0, 0xD6CB7ED9D905CFAAuL, 0xEA634975012653F1uL)),
                ((+1, -4, 0xF63CC83F0BE66375uL, 0xB34AE356259F9AF0uL), (+1, 0, 0xE7D049689658C0C8uL, 0x6B9C86EAEAE80D2BuL), (+1, -11, 0xAF7CE666251DC692uL, 0xBD7321D49B37E00DuL), (+1, 0, 0x9DD3D3821BBA0072uL, 0xBCD1AC75604FF9AAuL)),
                ((+1, -5, 0xDDE0FBCCDFE1A1CDuL, 0xF9F9331B234FA6D1uL), (+1, -1, 0xF9367807B74F4619uL, 0x3006484A5E1E3E4DuL), (+1, -14, 0xB6ABFE6BCCF954C1uL, 0x22B8B2EE1AF423C2uL), (+1, -1, 0x81271B9E095CDCBDuL, 0xD2A8CA528ACF59CCuL)),
                ((+1, -7, 0xFADADA0B308B218FuL, 0xE525A6A98E1C6572uL), (+1, -2, 0xAD55A4527FA442B2uL, 0x67026A57F62EF2A4uL), (-1, -19, 0xCCBD974AA535243DuL, 0x6AEDA697D49687B1uL), (+1, -4, 0xF36F5B8FAD8EB634uL, 0xDE318AEC383F7932uL)),
                ((+1, -9, 0xB1A58D6DB678699EuL, 0x85695159D5EABA72uL), (+1, -4, 0x9F8D4D0DC89D272BuL, 0x9755B4BABD3BDE88uL), (-1, -20, 0xDD0E833B92D673B1uL, 0xA27E46D11B4B18F8uL), (+1, -7, 0xE29785A593D33064uL, 0x73EA476171DA4FB0uL)),
                ((+1, -12, 0x91D052C6B86A2C74uL, 0xCC2DB15920A810E5uL), (+1, -7, 0xBDA06FF71B7F1D60uL, 0x976621E77EC294A0uL), (-1, -33, 0xC67C8578B2C35740uL, 0x94CA3965D9AF6013uL), (-1, -15, 0xE24EDB1359C9CB8FuL, 0xE6B255663C97EAC1uL)),
                ((+1, -17, 0xD604E1B59A4EA08EuL, 0x8F2E72C26C54D275uL), (+1, -10, 0x84E81E7C56FC6EC4uL, 0xB588CA656F0B1E9AuL), (+1, -37, 0xDBD1BA2013771ADEuL, 0xDA31622726004BD9uL), (-1, -13, 0xD297954BD02E7512uL, 0x583A2DCF86A7940DuL)),
                ((+1, -44, 0xDF869E4BC6B59B10uL, 0x4D0196E57DF0E0F7uL), (+1, -15, 0xA817187543CD9101uL, 0x0C0E6E3F8301ED8FuL), (-1, -41, 0x82B01953E86ACD7DuL, 0xBE7C4A6E2778FEF3uL), (-1, -16, 0x8882EDC40FF7B803uL, 0x13538736BD750C00uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xAD93B67EF07ED286uL, 0x59403F6BA307DAEDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -10, 0xF9F546DF826DE390uL, 0xD15A18B668E04CECuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0x8F6FB0B71215EDE8uL, 0x26FF6CE0155E0C50uL), (+1, 0, 0xF57A270E892B3E32uL, 0x58A367E6575EB38BuL), (+1, -9, 0x84AD7AD204B3DB91uL, 0x01315D510B17B7A9uL), (+1, 0, 0xED4A0F66899CA791uL, 0x0BB8E18FCE8D1AF5uL)),
                ((+1, -4, 0xD530A9224FFA6C78uL, 0x65921603AAD39E34uL), (+1, 0, 0xD55DD46767709CD7uL, 0x168BFFC640CA6C00uL), (+1, -11, 0xF756D20702B30B58uL, 0xA8BF1EEF2504B963uL), (+1, 0, 0xC621D6959D1C5C2CuL, 0x40D9B27315693A53uL)),
                ((+1, -5, 0xB7DCAD2A249FE6F7uL, 0x9E13B553836049D9uL), (+1, -1, 0xDB1FBC6EBE1E4560uL, 0x15282B373E8BAB49uL), (+1, -13, 0xF4B948B891424F23uL, 0xDF1D472E45BE6514uL), (+1, -1, 0xC220036ABC1FB923uL, 0xA78E023E4BD389E7uL)),
                ((+1, -7, 0xC641082FA957662EuL, 0x8D2C308B9464BAE6uL), (+1, -2, 0x9131E60914D0080DuL, 0xE70F87EE2F8DB0B8uL), (+1, -16, 0xFF1431F8A7E291B3uL, 0xE52E3AC0AD9887E5uL), (+1, -3, 0xF379FD87C18628EBuL, 0x7BF663F192FDD6ECuL)),
                ((+1, -9, 0x856F0667BF611CA6uL, 0x945B05D78C62DAF2uL), (+1, -5, 0xFDF4FF5A35A35A74uL, 0xAB2C45D4D7C64589uL), (+1, -20, 0xD8E00F689551C863uL, 0xF9C8CFF3771D6490uL), (+1, -5, 0xC796E83C5C27C602uL, 0xC21F31A0320C11D5uL)),
                ((+1, -13, 0xCF71414498C57D57uL, 0x3A45C20866487257uL), (+1, -7, 0x8EFBED9483DA9E29uL, 0x2CE6EA5CE6553D20uL), (-1, -35, 0x8DE4CA2B7E5225B4uL, 0x791BA73A653960B0uL), (+1, -8, 0xD0285401449ABF12uL, 0x030D3486E8359E1EuL)),
                ((+1, -17, 0x8FA0D5733C4B2088uL, 0x1C91EB546AEBD7AAuL), (+1, -11, 0xBD5D17997101F503uL, 0x1226F386B2D2DD6AuL), (+1, -39, 0x8DE4138DF8ECCF1CuL, 0xACE4A2E78155DA7FuL), (+1, -12, 0xFB67B828099B0D51uL, 0x7E5C1FB0F22A4057uL)),
                ((+1, -49, 0x9FAED72028602B97uL, 0xE02C4113D94A1BCAuL), (+1, -16, 0xE19C5954C4866936uL, 0xE448B0090B557B90uL), (-1, -44, 0x99B4A2698D6D6D0DuL, 0x390CB6449E76491EuL), (+1, -16, 0x85BFBE596D495865uL, 0x90A48402D1F42652uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0xA2C8A2E0740F09D7uL, 0x6D1970AC49CCA9AEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -10, 0xCE4D914084986593uL, 0xEAF1EA6236166EC8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0x81001571E4F060E9uL, 0xC62116B7BEC0A498uL), (+1, 0, 0xEAB9052F52E41B19uL, 0xAE6601AECD83AF0FuL), (+1, -10, 0xEA4B2E7435DEED07uL, 0x2401F29C26A662F4uL), (+1, 0, 0xF0A33BD54BA246C4uL, 0xA80F5878F5F1EBE9uL)),
                ((+1, -4, 0xB6F1C9CB456BC936uL, 0x5AF59E3596502759uL), (+1, 0, 0xC263B77CF6885CAFuL, 0x4461D5147E5421BDuL), (+1, -11, 0xE4D3BC74B03FF1A8uL, 0xD0FEC5FDBDAF5DDDuL), (+1, 0, 0xCB14C990E9C69372uL, 0x5266DE02EC541936uL)),
                ((+1, -5, 0x95E4EE5A0DA06879uL, 0x24FEB37C3D38C656uL), (+1, -1, 0xBD95F75B8B11CDB0uL, 0xD86A78A1A42F4280uL), (+1, -13, 0xED687B687FA107ECuL, 0x8701CED8D4224A40uL), (+1, -1, 0xC8BD96313ED82D9FuL, 0xA3454EB9B20B0DC2uL)),
                ((+1, -7, 0x98EDFD999A20230CuL, 0x86EB996C45615EACuL), (+1, -3, 0xEDD7B9B222A49811uL, 0x7555AAA91290B0C3uL), (+1, -15, 0x8281D8E3A83CA123uL, 0x8964B230C1C54103uL), (+1, -3, 0xFDEABD2D68ECF91EuL, 0xB95C5462C1E20756uL)),
                ((+1, -10, 0xC20258BE43C5D26DuL, 0x0AC1A5C81D6D6BAAuL), (+1, -5, 0xC44C42B1728E08CFuL, 0xDC5B7C2CB00F622AuL), (+1, -20, 0xF36D47C14114724CuL, 0xC4928B022337EFD0uL), (+1, -5, 0xD231360722CC6167uL, 0xA61F5FF624897E8DuL)),
                ((+1, -13, 0x8D903BA3ABDA17CFuL, 0x8A52E5FD074EFDA1uL), (+1, -8, 0xCFF75CDD4F98A492uL, 0x214202F0BF6EE04FuL), (-1, -37, 0xCD2701970F1C0417uL, 0xE08BC6689E40CA76uL), (+1, -8, 0xDE21B5513B7121AEuL, 0xC9051ECD5B99EDC2uL)),
                ((+1, -18, 0xB741A583B8F6B289uL, 0x6E2E1E4A9D6D5D2CuL), (+1, -11, 0x812CBFDEB1CF1554uL, 0x6F918DBB0D076D54uL), (+1, -41, 0xBA97729AEBC8E81DuL, 0x34DBA08BA8A574B2uL), (+1, -11, 0x88CFDC390CC0A4D1uL, 0xEA6957116BC3DAC0uL)),
                ((-1, -48, 0xC276C611F9125CA1uL, 0xC623E352C863017AuL), (+1, -16, 0x8FEDE3FAE1D71978uL, 0x29609A93EE2C016CuL), (-1, -46, 0xB937687B52C81B97uL, 0xAD26D95C7E3557AEuL), (+1, -16, 0x96254BE93BC980D2uL, 0x67C375B7297A77EAuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0x993F1BBD450D47D6uL, 0x3B6CA36551DD9B40uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -10, 0xAC3548AB34F3FF9FuL, 0xD18E931EE33E32E6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xE963F9D52942D919uL, 0x3175040B4ECD1E6FuL), (+1, 0, 0xE0F2C87EE43F8223uL, 0xF0CEF176AD297B27uL), (+1, -10, 0xD3A0BE0C1EE2B76EuL, 0x8DC2E3B1E849E3CAuL), (+1, 0, 0xF71C2D90285D76BEuL, 0xB1B564B5BD4A27D1uL)),
                ((+1, -4, 0x9E21FA1B2C982174uL, 0xB0FBFF16961F4D8CuL), (+1, 0, 0xB1D1E96043747B17uL, 0x2970BBCED9A73254uL), (+1, -11, 0xD879F7A5E6847168uL, 0xB3D95B420552965EuL), (+1, 0, 0xD40242804EBA1AF3uL, 0x814D643E7E79481BuL)),
                ((+1, -6, 0xF65D8353B4F641B5uL, 0x1389438916664EBDuL), (+1, -1, 0xA4E78B7722E3A798uL, 0x7AAAB79E32CF5054uL), (+1, -13, 0xE7EC346300A12E90uL, 0xD437E9364057685BuL), (+1, -1, 0xD361B4FEC2E94AA2uL, 0x8B137FC723B413E2uL)),
                ((+1, -8, 0xEDD30BC1A6B28A4FuL, 0x2ACF9686A8D0F350uL), (+1, -3, 0xC3FB3972D3F6302FuL, 0xCD093172F4E3C1EFuL), (+1, -15, 0x8257050E7BF57AD0uL, 0xE3C93D624C3A0593uL), (+1, -2, 0x85FEFAED67B5280FuL, 0xC369A6F31A34A184uL)),
                ((+1, -10, 0x8E0D7249B55185B8uL, 0x9F049D39B1CA57ACuL), (+1, -5, 0x98A9EFBB9CBC585DuL, 0xAB5FC3E872367E9AuL), (+1, -20, 0xF871694642BCF3B9uL, 0xC55C8FFFB86087A9uL), (+1, -5, 0xDD3416FC9DF35831uL, 0xF3C62ACD3B3CEEDEuL)),
                ((+1, -14, 0xC241F946B236E629uL, 0xBF453D9374AA2328uL), (+1, -8, 0x9818172E5B3EC2A2uL, 0x3B0E055CC120B23AuL), (-1, -38, 0xEC3ACBE649831938uL, 0xCE1DF8C56F286408uL), (+1, -8, 0xE81B5F5F87185B2EuL, 0x14FBB6EE83C9D966uL)),
                ((+1, -19, 0xEA71C7811165AEACuL, 0xEFCB18CC74E0E4CFuL), (+1, -12, 0xB10653A61C7F0FABuL, 0xD3BDCF00F05394ABuL), (+1, -42, 0xC59AE2C8CF54EA23uL, 0x5EA2419BD70CC904uL), (+1, -11, 0x8D7614186F9331DCuL, 0x474A97126A8981E4uL)),
                ((-1, -49, 0xB154C4938F0CE6FDuL, 0xA216D9782A6AE2A3uL), (+1, -17, 0xB821DA3C9BAB702FuL, 0x974B6E6389D9B7BEuL), (-1, -47, 0xB5321EA470B10EF3uL, 0x6E3421294F5E6D61uL), (+1, -16, 0x993E93A2FDABCF2BuL, 0xB3BB20108A585B1FuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0x90C288172F4BD98EuL, 0x29567972B28762C4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -10, 0x9134B75E43E7127EuL, 0x066DD02AEDF97131uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xB4537CC1D683CFDDuL, 0x659020CAFA5A84DBuL), (+1, 0, 0xBBCF57790BEDA16EuL, 0x11A05F0759CE7325uL), (+1, -11, 0xF7B4275749E1BF47uL, 0x34395A5376946E06uL), (+1, 0, 0xC2194FE8095F9E80uL, 0x917D8CC0DF54FF26uL)),
                ((+1, -5, 0xBF9A817E84AF8B95uL, 0xCD24DC94F702DC7CuL), (+1, -1, 0xF022387F676B3F95uL, 0xDE5343CAAB4340E4uL), (+1, -12, 0xAC3C80A37366C2F5uL, 0xD90102700EC5AF67uL), (+1, 0, 0x814DD7CA257D1869uL, 0xA67D4A1AC1843FC8uL)),
                ((+1, -7, 0xDD20D5B3C70B7E34uL, 0xDF7BD48A1C3CD096uL), (+1, -2, 0xACF00299A4CF33D3uL, 0x5C7D556B7A85C52AuL), (+1, -15, 0xE335A7A856D3399CuL, 0xDE058B65A318374DuL), (+1, -2, 0xC3E124F2F731C8F4uL, 0x68885EF22B827C21uL)),
                ((+1, -9, 0x9160A91C5935AD2DuL, 0x90F70B4882E6C836uL), (+1, -4, 0x97100CBF175B88C7uL, 0xA7D0CE8F15D814CCuL), (+1, -19, 0xF29313A3CCF7AAFAuL, 0x6103FC03427CB34FuL), (+1, -4, 0xB5EB6FF0F6DDB260uL, 0xC232C734B9F46E3FuL)),
                ((+1, -13, 0xCD0DE600798EF945uL, 0x94A26A3702BDB73EuL), (+1, -7, 0x9F79729C1876EEF8uL, 0xF098BCCFC99EBAA2uL), (-1, -37, 0xBC339360DBDD8694uL, 0xBA69A83941B05B77uL), (+1, -7, 0xCEE28E174AED258DuL, 0x8E0C828434898853uL)),
                ((+1, -18, 0xEFDF8FFEF71F420FuL, 0x8313CC3919495FF0uL), (+1, -11, 0xBB8ADD197E996F69uL, 0x6484944F42E18018uL), (+1, -41, 0x8EF081CC8A4BEBEEuL, 0x3EDFFE58E659895BuL), (+1, -10, 0x8533F52F8F93295BuL, 0x2D76D6A4C30F9C31uL)),
                ((+1, -45, 0xFD08DC636BF2216FuL, 0xA56B2B26A3815E3CuL), (+1, -16, 0xBC65662FCEB91FA1uL, 0xA111EFCDD36FF294uL), (-1, -47, 0xECF0097D9FED00CAuL, 0x5B5C98B72F7A639CuL), (+1, -15, 0x95A01B8D36996930uL, 0x8BEE3CD4E5B58CEEuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0x89291228A021EB24uL, 0xBF26920F39E4150AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -11, 0xF71A2C8CE11DEAECuL, 0xF769EB49C8D1E1DDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xA9A5E7A136C1B49DuL, 0x6AD431EBD9052512uL), (+1, 0, 0xB933A59BF0FFE5E8uL, 0x5720AC163E35D641uL), (+1, -11, 0xCA8453ECF34F3AE4uL, 0x2B12DF6E3A254102uL), (+1, 0, 0xB96E859A579C7377uL, 0x19B5B1EF083FD7AAuL)),
                ((+1, -5, 0xB299BA921760240FuL, 0xFE988EEBBFD89EE2uL), (+1, -1, 0xE93706037DDDF6D1uL, 0x1BAEC755CD0D8E65uL), (+1, -12, 0x85EBCD27E9398EDAuL, 0x0D3CE755236C67F1uL), (+1, -1, 0xEB558E391A97EB24uL, 0x3FE8910321BBBCE5uL)),
                ((+1, -7, 0xCC30E6EE2CCC5965uL, 0x9EF8419DC05DD882uL), (+1, -2, 0xA55C23FADBE2BF0CuL, 0x201914EB3D986D21uL), (+1, -15, 0xA6E3325C29F67386uL, 0xD8D78BCC51DB4D34uL), (+1, -2, 0xA9543856868B6DB4uL, 0xED73052CBF2CF42AuL)),
                ((+1, -9, 0x853F528ECC43C95AuL, 0x53FE73D65635E320uL), (+1, -4, 0x8E52A841BEC71B30uL, 0xA828583E23C5428BuL), (+1, -19, 0xA6BB697BED4FBF16uL, 0x7B505F3556BBE0BFuL), (+1, -4, 0x95026D6C44DC5387uL, 0xB4B5301D698A46C9uL)),
                ((+1, -13, 0xBB7FFB482BEF3D5BuL, 0xD1136A249EF76CECuL), (+1, -7, 0x945EE418CBF00D26uL, 0xD91EC30B9DDC2E74uL), (-1, -38, 0xE146F191BCBE7B16uL, 0xA07911612136352EuL), (+1, -7, 0xA02ED4BF9D1BC361uL, 0x8E83871A0D76392CuL)),
                ((+1, -18, 0xDD18A6339D3527A9uL, 0x04987190DC1ADB11uL), (+1, -11, 0xAD09BE3351C729F2uL, 0xC958338957B291C3uL), (+1, -42, 0xAD2D0367711FC5B1uL, 0x41209550CBA4CDB5uL), (+1, -11, 0xC28B34C99B2031F3uL, 0x5E20FE409442F299uL)),
                ((+1, -46, 0x9D1C0030A09309F1uL, 0x3509F5235DDA4F45uL), (+1, -16, 0xADA60CCDC3B0F96BuL, 0x35E094E3E4B3947EuL), (-1, -47, 0x938DA32DC487A363uL, 0x52E282F09B3C7FF5uL), (+1, -16, 0xCDB07C120E0CBF7EuL, 0x0B14D8DAD1C9A023uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX5p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -4, 0x82511193EA299961uL, 0x66E39F8E700B29C6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -11, 0xD3F970E23D5BC14FuL, 0xF6AF0924ABD1725DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0x9F04E757FE4C746AuL, 0x912D4E1782807642uL), (+1, 0, 0xB5BE3258B42C49BCuL, 0xF9A8075F62765F67uL), (+1, -11, 0xA6EEB0ADDB9C8DBBuL, 0xBE3E3848BB27DE84uL), (+1, 0, 0xB15B5BBADB1059D4uL, 0x9ED0191ED376F7FDuL)),
                ((+1, -5, 0xA49616551AC1B484uL, 0xCBF01791EC7D1247uL), (+1, -1, 0xE00BD360ED02120BuL, 0x3B8F33B057810179uL), (+1, -13, 0xD1F4272BDBC97E2EuL, 0x51608C261EAAE799uL), (+1, -1, 0xD6A27400A4CE1714uL, 0x12E0E07284067895uL)),
                ((+1, -7, 0xB88F0C3D3BF46C80uL, 0x4FC4345B9813BC7DuL), (+1, -2, 0x9B3B0FACCEC88186uL, 0x2A1D1C2CE9316C91uL), (+1, -16, 0xF6F89C3F82EB1266uL, 0xEF94332F78B6AC8DuL), (+1, -2, 0x92D95133F30B02F3uL, 0x3D7AFEF08A084CABuL)),
                ((+1, -10, 0xEBEF94E8967C113DuL, 0xB54FEF11282FB2E2uL), (+1, -4, 0x826228BA23E9CBE7uL, 0x0DF2D768BD61C4E6uL), (+1, -20, 0xE6998E894CE4AF20uL, 0xD893A90EFC1D49D2uL), (+1, -5, 0xF51AE7C78CB34DC2uL, 0xA06C439831029609uL)),
                ((+1, -13, 0xA2932E587865CF2DuL, 0xE048B281819B5C18uL), (+1, -7, 0x848DBEA76A30045CuL, 0x7F0CF79DAD2E9DDFuL), (-1, -39, 0xDB49F33160EF265EuL, 0xD5510D648E46D6BFuL), (+1, -8, 0xF94035E8E5C1466BuL, 0x71E9026E2E6C2915uL)),
                ((+1, -18, 0xBC0B1EC2F45737FBuL, 0x5479D79DE1061237uL), (+1, -11, 0x96C32FBA36512CB8uL, 0x322AC9EA4A909EB8uL), (+1, -43, 0xA49F3291CEC91756uL, 0xF779125D25C32E86uL), (+1, -11, 0x8ED974B2AF5E59A6uL, 0xE868AE568C025A86uL)),
                ((+1, -48, 0xE38E39AEA28DFADEuL, 0x0B94821120B8304DuL), (+1, -16, 0x93B060CC98FDE4D0uL, 0x9E79BB35D8139858uL), (-1, -48, 0x898BB7612375DA05uL, 0x882DB6FAEA9BE91EuL), (+1, -16, 0x8E3DD54D9B54395DuL, 0x1958FC4463176028uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX5p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xF83E51B3AB2A7728uL, 0x089A5D02B09CA8A0uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -11, 0xB7306BE1BBC42ABDuL, 0x6D3A426B4FA34D29uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0x9594E97702BB6EB6uL, 0xBAA167691CC26551uL), (+1, 0, 0xB29906E03AA4AF8DuL, 0xEB22DDB3C64BF52AuL), (+1, -11, 0x8B037FB794FDE1B8uL, 0x0EF3C352F65AE3FBuL), (+1, 0, 0xAA1571E437B36A06uL, 0x9FF89F0C1D3D4F8DuL)),
                ((+1, -5, 0x98377CB9C31CB875uL, 0xC4258CF3CFAF465BuL), (+1, -1, 0xD7AA5EDD518F93C0uL, 0xF3DC02A29323BBA5uL), (+1, -13, 0xA6CFEC0E10892864uL, 0x2425BFCDC32ED02CuL), (+1, -1, 0xC4D24EBDB62AE933uL, 0xE4A781D4698F7F1BuL)),
                ((+1, -7, 0xA73EB08856FB9C71uL, 0xDC83502E695D0A31uL), (+1, -2, 0x91FA87C372C2B9EEuL, 0x0351EA6C5E00A41BuL), (+1, -16, 0xB9CC8850DC3DBCFDuL, 0xF95296276ACA980EuL), (+1, -2, 0x8069D501BCD8A51BuL, 0x9EC3522B05CC40C1uL)),
                ((+1, -10, 0xD0F0A31CB4957C2FuL, 0xB9C9C81659629B7CuL), (+1, -5, 0xEF0B8F46C24C3862uL, 0x4EF1A910106E38BBuL), (+1, -20, 0xA29F59BCA208E1F6uL, 0x8C245EEE683784F4uL), (+1, -5, 0xCBDA84163B65AD72uL, 0x97401FCCF5C54D28uL)),
                ((+1, -13, 0x8C707F47F4B439D8uL, 0x0D559F75A5DEB846uL), (+1, -8, 0xEC7C6D0429E5CDEDuL, 0xF95C99EB62450916uL), (-1, -40, 0xC0AE290406B5E126uL, 0xC13843806918BEABuL), (+1, -8, 0xC4AB5BFA01628422uL, 0xAE5C803ED00FF8DFuL)),
                ((+1, -18, 0x9E4E6D868BCC7D88uL, 0xB77A17262AC5F36EuL), (+1, -11, 0x82B3086B8541F978uL, 0xA4F6913AC5B7940EuL), (+1, -44, 0x8BE5B28867D3713DuL, 0x6662A261036DF940uL), (+1, -12, 0xD55B2D4F37018371uL, 0xDA92807612EEDED8uL)),
                ((+1, -49, 0xBF854840DF7BF40CuL, 0xDD6A990141260CE3uL), (+1, -17, 0xF8AAC3EF3418AA4EuL, 0x166E03DC784657F3uL), (-1, -50, 0xE28C95142BA82F1CuL, 0xB33ACA9DAF009DCEuL), (+1, -17, 0xC89FAE2B40728C3CuL, 0x393F2F657B328AB4uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX5p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xECF9C1A7A8582BE5uL, 0xA95AD016D8B9A005uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -11, 0x9F6167BE472B2EFBuL, 0x1BE4FF2CD08341B3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0x8E52689BA9DC880FuL, 0x27A7AABD8A4C45B8uL), (+1, 0, 0xB0FD5215E81D78D9uL, 0x9B449CA87F933640uL), (+1, -12, 0xE9E0116CAF6817CBuL, 0xA3DF28A0B316695FuL), (+1, 0, 0xA39443BEEC1B519FuL, 0xB13A15D2B6BD0BCEuL)),
                ((+1, -5, 0x8F6311B4508ABABEuL, 0x3FA22EEF14C26950uL), (+1, -1, 0xD2BFC629B8AAD0DBuL, 0x006B0AA6D5B8A52FuL), (+1, -13, 0x867B75C933E8FEC7uL, 0x69411E24EE84B565uL), (+1, -1, 0xB596B59612148A32uL, 0xBC5A7040EC26AFECuL)),
                ((+1, -7, 0x9B2CB741AED973FAuL, 0x9BB6DF6078115E16uL), (+1, -2, 0x8C19270DEA890B20uL, 0x63A886CCDC6B2EBEuL), (+1, -16, 0x8E8DCCC80F0B7B67uL, 0xE92193EB49706AC3uL), (+1, -3, 0xE2BF6CC23AFB8710uL, 0x7403BA0FA6D3D756uL)),
                ((+1, -10, 0xBE2C14C6C7F5301EuL, 0x0E17DFC7A79E51CAuL), (+1, -5, 0xE08F3C281C3D91E1uL, 0x1A1AD75A9CBDFA4BuL), (+1, -21, 0xEB53A7374794BD6FuL, 0xF2F9E86B307C294AuL), (+1, -5, 0xABD24E3F520A4EFCuL, 0x1628C6827CED1820uL)),
                ((+1, -14, 0xF9FC2A9CF5995DADuL, 0xFE05FDEC711DFC26uL), (+1, -8, 0xD8DA1CE7EA546258uL, 0x0F1484DAA718FC9DuL), (-1, -41, 0xA0BB35A8581D125DuL, 0xEF53CBA7DEE7B94BuL), (+1, -8, 0x9DE3A7C944E84EC9uL, 0x87A192F46E307912uL)),
                ((+1, -18, 0x89719997E8112717uL, 0xE038C0D46842A875uL), (+1, -12, 0xE971E6B871ABAC55uL, 0xD203A51751E66C9DuL), (+1, -46, 0xE1124EBBD2E26E4DuL, 0xB1C367A92835D306uL), (+1, -12, 0xA2C87A7C26FB81A1uL, 0xFD01D4CC84C1BD1BuL)),
                ((+1, -50, 0xBEF035D321761262uL, 0xC7208CADC8819CE5uL), (+1, -17, 0xD7E5697403E0792CuL, 0xFAC8DB201E6CDD6AuL), (-1, -51, 0xAFF2BB99B5283D21uL, 0xEC01FC1C32232CECuL), (+1, -17, 0x9128E83B5A988731uL, 0x00748FBB30923460uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX5p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xE2AF3BDB508033C7uL, 0xDA1DCACF61DF6C59uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -11, 0x8B8512984F21642DuL, 0xB361D1A35D883AB1uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0x8BD349717BD4E6CFuL, 0x8130DB17A84392BAuL), (+1, 0, 0xB424EB552726042EuL, 0xE6251D2D5541ED01uL), (+1, -12, 0xC677F06992AD2C87uL, 0x229E858AF429D9DFuL), (+1, 0, 0x9DB36647D3850A50uL, 0xAF8790F4A9C43C8FuL)),
                ((+1, -5, 0x8E7FBD20C5B683EBuL, 0xA04F992628C8C62BuL), (+1, -1, 0xD7CDCB896DCA9715uL, 0xDF6F97C3FEE3BDE3uL), (+1, -14, 0xDBA338D0ECB23C4AuL, 0xEA564D66185BDA86uL), (+1, -1, 0xA868330FB62CE692uL, 0xB7D497D8BCC6313FuL)),
                ((+1, -7, 0x9A598BBB1298C604uL, 0x745739E06CBE79A0uL), (+1, -2, 0x8F17A4A8A1F025CAuL, 0x811E57608902B608uL), (+1, -17, 0xDEB41B2557D4E0AAuL, 0xB934B62E62EE0785uL), (+1, -3, 0xC9DFD8D0B73AC0D1uL, 0xC7A772323CAA255FuL)),
                ((+1, -10, 0xBBD9BABCC515074DuL, 0x8E6145C1CB0C4860uL), (+1, -5, 0xE344C155E5BBEC8AuL, 0x6E8250AF01F5F98AuL), (+1, -21, 0xAE80379677B497A2uL, 0xD259CCB3BB8B9BD3uL), (+1, -5, 0x928F2B6D6E644F7CuL, 0x8AF200CF81172D98uL)),
                ((+1, -14, 0xF3C1E615D2423B3BuL, 0x7015962505F93B17uL), (+1, -8, 0xD856ACDA340CBDFEuL, 0x1850E3F38A707230uL), (-1, -42, 0x8309098A43368116uL, 0xB11DFED6F6A0452BuL), (+1, -8, 0x80C6F6C6B4449E40uL, 0xA090942263E598B3uL)),
                ((+1, -18, 0x83AF41E29D04425BuL, 0x221F7563A8A74B01uL), (+1, -12, 0xE49D6C0176C4A52DuL, 0x1158BE8C2A149DE3uL), (+1, -47, 0xB0DD9D95FBDF0B95uL, 0x67C794EBFF6AD857uL), (+1, -13, 0xFD6CC9FC52C76CE1uL, 0xFEEFFA81535A119CuL)),
                ((+1, -51, 0xF029E5C91FE43A91uL, 0xBA701D9DB7634884uL), (+1, -17, 0xCED9850C91C55356uL, 0xCF09D97C21F03578uL), (-1, -52, 0x85602F9208A1075AuL, 0x4E64CB5E59979BA0uL), (+1, -18, 0xD747CB64FDD8896EuL, 0xD32C2E6F9D35724EuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX6p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xD93FAC3E461F91A1uL, 0xA3A27989E99F6965uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0xF5A551E6E82093B2uL, 0xB78F6E7FB20C6B13uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xA0C1E8D98C0552DCuL, 0xF5EC21C3BB8A1771uL), (+1, 0, 0xD2BEE304AA1A0D4DuL, 0x862092AED539D39DuL), (+1, -12, 0xA9A76242DB33FE39uL, 0x7C62CC62C2812264uL), (+1, 0, 0x984DA18704A13238uL, 0xD21318D6BE73F733uL)),
                ((+1, -5, 0xB4C8F506ED8874A1uL, 0xEFD3FA58E35FA589uL), (+1, 0, 0x8A1046C64C5DA008uL, 0x7090C7709E069709uL), (+1, -14, 0xB537227080412D77uL, 0x3E3A93C393E76FAAuL), (+1, -1, 0x9CCB909F07B8D93EuL, 0x00AE55B1CDE46ACBuL)),
                ((+1, -7, 0xCF0D5EEEAD9F5DD0uL, 0x775ECE6AA963D982uL), (+1, -2, 0xC0F5EBBBEB19351AuL, 0xB14D6F2219E20089uL), (+1, -17, 0xB07B9B3F9191A707uL, 0xA2AAB7590CD71DEFuL), (+1, -3, 0xB4E3977BD5615B9CuL, 0xD5DF75016F8EDE35uL)),
                ((+1, -9, 0x81E0084B909C8C97uL, 0x5007988D6C1EABF8uL), (+1, -4, 0x9DCFDE2B11827D57uL, 0x60F667EBF002F5B2uL), (+1, -21, 0x8407C51F41FA781CuL, 0x138BA840E9FD0684uL), (+1, -6, 0xFC5C9370828B1B18uL, 0x8B25D8FC15C893ECuL)),
                ((+1, -13, 0xAAD7656E4075BC24uL, 0x14EBBF906C752F22uL), (+1, -7, 0x985214C9D30FE9BEuL, 0xB857C2E2C1769AEFuL), (-1, -44, 0xD43B68F4759382FBuL, 0x96D1655ED9BA0EC3uL), (+1, -9, 0xD4B8B4F0DC55C9D4uL, 0x675A8715FDD99D48uL)),
                ((+1, -18, 0xB8F3466A591EEA1DuL, 0x4B627F9FC6D7DB4BuL), (+1, -11, 0xA16A29572BDBE73FuL, 0x2D8A6BA1EB93CE20uL), (+1, -48, 0x8A2D0BF179F2F6F5uL, 0x59F5F7B45CEE48DBuL), (+1, -13, 0xC87C9C48E38156D3uL, 0x4EEEA484C7D9B860uL)),
                ((+1, -50, 0x80D8A449EF9C15E2uL, 0xBC77B15175D603C0uL), (+1, -16, 0x91427690C25A0D66uL, 0x1B47937566B495ABuL), (-1, -54, 0xC921DF11A1DD58DDuL, 0x5AE338B38D1B809BuL), (+1, -18, 0xA2E2A67886B96E6AuL, 0x3B9A21DC1BC35135uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX6p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xD090EC665B9ACE53uL, 0x327CDBF8EFB457E9uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0xD95EA8B7CA3605D3uL, 0x1E0565432BC29BC6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -5, 0xABDDB47666FFC733uL, 0xC3B320DDF84E05C3uL), (+1, -1, 0xFBE17B5A44869A73uL, 0xC3831D75AA75FB80uL), (+1, -13, 0xCC54B4F790CE309CuL, 0xC05C0A3BE4C07C60uL), (+1, -1, 0xF30A39E08E98A8D0uL, 0xB0D12FA5177EFA44uL)),
                ((+1, -7, 0xE587237EC1AC063DuL, 0xCFF642E525DB1609uL), (+1, -2, 0xD054E082E98F52A4uL, 0xEB7A5C3440F7EF7AuL), (+1, -15, 0x82D1E1FD9BC66E6FuL, 0x1CDD5DCF4F2F7108uL), (+1, -2, 0xC12B43AC5DF6AC55uL, 0x6112450EABB56E81uL)),
                ((+1, -9, 0x9AE4632F959F6C65uL, 0xDE4F3B13B3E91BF1uL), (+1, -4, 0xB92D0F43D1581167uL, 0xF98112610F446D1FuL), (+1, -20, 0xDD9A2E0F9BD0BED7uL, 0x2DD64F97DBC78991uL), (+1, -4, 0xA4428C3445C284FDuL, 0x978CE61292A5104CuL)),
                ((+1, -13, 0xD2D29C000E74DB69uL, 0xE2B1F9822EC0C676uL), (+1, -7, 0xBA54E358167F9EA3uL, 0x2739F958FE0FC1DDuL), (+1, -39, 0xD4F7D261AF2E2266uL, 0x238182F8D714C465uL), (+1, -7, 0x9D5D19C36A8A7E86uL, 0x28CC70B0C09507C5uL)),
                ((+1, -18, 0xE6F917251AC45D32uL, 0x86C7A457DE85556DuL), (+1, -11, 0xC902B4C678DF9D5CuL, 0xADC9A5194F774D7DuL), (-1, -43, 0x92F34D0FB280F427uL, 0xAD18A94F905F437FuL), (+1, -11, 0xA0CAB4D214CA4A98uL, 0x86DE12FD03EE2CACuL)),
                ((+1, -50, 0x94B69B3C9FEBD609uL, 0x8607D13439A2B368uL), (+1, -16, 0xB567E11BE39F8B37uL, 0xC004832051D9FC40uL), (+1, -49, 0xE71516012F6C79A4uL, 0x2419FD2988336208uL), (+1, -16, 0x88B323D6A3E7BEC8uL, 0x0BB8F18F803AAE38uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX6p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xC88CD40E0271B05DuL, 0xF5900F204FA13BB6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0xC144C8A640A48F86uL, 0x28AEC7BEFB9B0053uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -5, 0x9EF443721F22D0A2uL, 0xD0F06FD882FA58D7uL), (+1, -1, 0xF242D838587CF2BFuL, 0x6B0097EFF21CF823uL), (+1, -13, 0xB2B67B8E884EA89EuL, 0xF377E26A32AB2829uL), (+1, -1, 0xEC615E18EA0F10C0uL, 0x41553E8EC8D690A6uL)),
                ((+1, -7, 0xCBEA9FF546E83BF0uL, 0x24187D550176FF71uL), (+1, -2, 0xC08CDD6E60D7AA9AuL, 0x0D413E23E60F6E71uL), (+1, -16, 0xE1001A13515D16D2uL, 0x959FBD4EAFB5693AuL), (+1, -2, 0xB6B84CD4DC61EC6CuL, 0xD5A4775DE99571CDuL)),
                ((+1, -9, 0x840CE771213CE53BuL, 0x9320E94632C20183uL), (+1, -4, 0xA4580C923295B483uL, 0x54278F58E4DC34ACuL), (+1, -20, 0xBCC4E2F31645730CuL, 0xF77B44564220FC69uL), (+1, -4, 0x972736626CAF7D72uL, 0x2F60D7D699B3A6ABuL)),
                ((+1, -13, 0xAC5798D955DBF304uL, 0xD5706AC3EE2B089EuL), (+1, -7, 0x9EB376238213D089uL, 0xAB9477C7075BAE18uL), (+1, -40, 0xBAC36F83A2616262uL, 0xAF4BDD0CFEDE49A4uL), (+1, -7, 0x8CF657C4ECBF748EuL, 0x8D4F72F0DBF77A7BuL)),
                ((+1, -18, 0xB4F7D27C2F0A6802uL, 0xE192D2CAFDC623D7uL), (+1, -11, 0xA43A50315F929932uL, 0x098E156FAB1F6F17uL), (-1, -45, 0xF7DF3D69FFBCF84FuL, 0x321B46419EFD4C8FuL), (+1, -11, 0x8C5A622F0F1A03B9uL, 0x0DD0F1087FF37C49uL)),
                ((+1, -51, 0xC34B007387AEF7EEuL, 0xA7D3373A3F38AE11uL), (+1, -16, 0x8E21C964562E64D8uL, 0xE3196CF84D693EB0uL), (+1, -50, 0xBB8A247F1A4A4C4AuL, 0x00C23F131016349AuL), (+1, -17, 0xE8E374578D887ADAuL, 0x47874420C3A8774EuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX6p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xC1207DDD7EF267E1uL, 0x47FF8C1321F25370uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0xAC99B2C390CA0B3BuL, 0x8B44F1E2B15901A3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -5, 0x935EA01763CFDDD2uL, 0x0FF13B79F63311D5uL), (+1, -1, 0xE940043AE1E4986EuL, 0xD2121951D77AF86CuL), (+1, -13, 0x9C38FABCBE1D02AAuL, 0x06FEB8CDE5904FFCuL), (+1, -1, 0xE585858D170A8A9DuL, 0xC6F9708E7E70B13EuL)),
                ((+1, -7, 0xB5CB2048461567CEuL, 0x19F002C312D9772AuL), (+1, -2, 0xB258474853908235uL, 0x12D6FEEBBEDD6B2CuL), (+1, -16, 0xC048DFF56D32A25BuL, 0xB0DB0CE9A84E0E6BuL), (+1, -2, 0xAC39EEB2050119BCuL, 0x9E7F67D40C0E228FuL)),
                ((+1, -10, 0xE23106FB352F0BF3uL, 0xD11F9DCDA9BBE299uL), (+1, -4, 0x92561D6F2B9DAFDFuL, 0x5B9606714E8DF09DuL), (+1, -20, 0x9E472231BF52DC1DuL, 0xE939A95EF7415D0BuL), (+1, -4, 0x8A4C133D3CBDCB0EuL, 0xE67AB655C7D81AD5uL)),
                ((+1, -13, 0x8DB3C6431830CF00uL, 0x438972C3B4A14FE5uL), (+1, -7, 0x87C6DC5D617A3FE2uL, 0x3D849ACA365A8D59uL), (+1, -41, 0xAA12BE2B120E8188uL, 0x6204692565D8A8D4uL), (+1, -8, 0xFA6E19D924737336uL, 0x858C113B2FFA8089uL)),
                ((+1, -18, 0x8EC989614D2E975DuL, 0xB143A6849610FB8DuL), (+1, -11, 0x86F2BF8C41C7C0C0uL, 0x24E7665B984C7492uL), (-1, -46, 0xD94EA7D9BDE844B8uL, 0x973C920C18815551uL), (+1, -12, 0xF22DC7C9803D59F6uL, 0xB8395A9E80CCEF4BuL)),
                ((+1, -52, 0xE9F73ABCE07F5827uL, 0xDE0D492846244D7CuL), (+1, -17, 0xE04A294DEC5CCFABuL, 0x5200E73F67791BE0uL), (+1, -51, 0x9E608A58CE8A797AuL, 0x4477FCD375FA6449uL), (+1, -17, 0xC344F536BD2D01A6uL, 0x7ADEBC182D4253AEuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX7p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xBA3BB3B888F917FCuL, 0x097F6012BE7A858DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0x9AC67D249E78679BuL, 0xFEC71BAED69E886FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -5, 0x88F81D34FAC6C1E4uL, 0xA5B886835D29FD49uL), (+1, -1, 0xE0D568F5A1BDCA16uL, 0xEC7C92FC401E012AuL), (+1, -13, 0x88B909EA3DFE2959uL, 0x11FEBE1958205E24uL), (+1, -1, 0xDEB14CA09FF69E99uL, 0xFFD8FDF5F1FD682CuL)),
                ((+1, -7, 0xA2AB1D84082BB787uL, 0x4B806A407AC88C79uL), (+1, -2, 0xA594313295F3C3F0uL, 0x03FACF99E3C3FBE1uL), (+1, -16, 0xA400DCA62779345FuL, 0x0F339766DC3AF700uL), (+1, -2, 0xA213DC8BA374D64CuL, 0x251432F6FC1B2059uL)),
                ((+1, -10, 0xC2AEB41D80130B51uL, 0x0CF866185545084EuL), (+1, -4, 0x82C65FE041BD1684uL, 0x54EB10AADBCB893BuL), (+1, -20, 0x83CBC3AEA94C15FCuL, 0x0E29CE1B27165E8EuL), (+1, -5, 0xFC6CDD1C377C7069uL, 0xB87613E1D2572EA6uL)),
                ((+1, -14, 0xEA7990EC6DC95D53uL, 0xE41A246587D5B0B2uL), (+1, -8, 0xE978B68510464A39uL, 0x49BA3771B3EAF990uL), (+1, -42, 0x9FEBD846831AD31AuL, 0x60CFFFD176CB5B72uL), (+1, -8, 0xDDA1B18A9A0BAF9BuL, 0xB152C24E8D68DF8FuL)),
                ((+1, -19, 0xE306CFFC31E0B076uL, 0x1155794590696587uL), (+1, -12, 0xDF291F03BBF17DFCuL, 0xB8DE0884EAF9DFBCuL), (-1, -47, 0xC4E211134B9AEDEDuL, 0x479647CCE09B886CuL), (+1, -12, 0xCFDEA9433B945547uL, 0xD9B468823D55BC54uL)),
                ((+1, -52, 0x86286EF0CB4D7703uL, 0x2CDCE88D4618FD43uL), (+1, -17, 0xB24E68A0BD8475D1uL, 0xFAD781C1A43F45FEuL), (+1, -52, 0x8A56B4E1CE3629E1uL, 0x9CD297784432C43CuL), (+1, -17, 0xA299001EF8D522C9uL, 0xFA20253AEDAE84E4uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX7p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xB3D0793634EA994DuL, 0xEF5DE6C54999C6A8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -12, 0x8B524536219A204FuL, 0xB20BF5DABA020C42uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xFF3CC0C629765B3AuL, 0xE75E54BF9AE6840EuL), (+1, -1, 0xD8FBFCEEFAC238FBuL, 0x5EC68DA2702A2954uL), (+1, -14, 0xEFCE57BC728D802CuL, 0x44E9AB6987AF6597uL), (+1, -1, 0xD8036434A0AD53E0uL, 0x142720074544FC1FuL)),
                ((+1, -7, 0x921ABF8C82E2678DuL, 0x8638B0BBF1CC6022uL), (+1, -2, 0x9A1C02EEFB6E26CFuL, 0x9323A0E185E70A57uL), (+1, -16, 0x8BEC762D957D88EEuL, 0x8E14BBD35EF5C5C6uL), (+1, -2, 0x987328A3515A0B70uL, 0x582D037B4A7CAB8DuL)),
                ((+1, -10, 0xA86B93BFE8501538uL, 0x107D38E21BBAE360uL), (+1, -5, 0xEAA04C5F2B227C64uL, 0x15CD2A56D45C229EuL), (+1, -21, 0xDAF72A6AE5DB42A7uL, 0xC5D0C75602429CAFuL), (+1, -5, 0xE62E8EC65CC93B71uL, 0x04937AAC9DF206EAuL)),
                ((+1, -14, 0xC3409F1DBEE21785uL, 0x44052884F8D0679EuL), (+1, -8, 0xC9C39970984BD7E2uL, 0x1B8F6E06A415E1F6uL), (+1, -43, 0x9AC82BA4C7F9412FuL, 0x2F21849A3DD0D600uL), (+1, -8, 0xC3E913030A74AC25uL, 0xFD6C5E11A67E096FuL)),
                ((+1, -19, 0xB5E6D769D53DFF18uL, 0x58DC81009E434E4AuL), (+1, -12, 0xB9B804232314FF1FuL, 0xD5988841918A7988uL), (-1, -48, 0xB7B98F5213FF890BuL, 0xD0A6F30BCA21DEFBuL), (+1, -12, 0xB21DDF4EA44E5D5EuL, 0x3AD02660BEE6A129uL)),
                ((+1, -53, 0x96A19D8618841994uL, 0x98FEFBF32437BB04uL), (+1, -17, 0x8EDD83083F3070ADuL, 0xCCA49D11A6CCC7A8uL), (+1, -54, 0xF914534D7F1E411CuL, 0xD50D582B6F5F6B13uL), (+1, -17, 0x8711C31F2C89FBFAuL, 0xC5F8D8121E9FDC3BuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX7p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xADD2AD57A6327C09uL, 0x5D88FE595604F352uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0xFBB6E5DD88A71A9CuL, 0x7CBCF431160C4A15uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xEE6395B9D19D4431uL, 0x13A251AD4B6D66FDuL), (+1, -1, 0xD1AA5D15D0E759AAuL, 0x4795296E7DCE17F6uL), (+1, -14, 0xD2D5F593F8DE6650uL, 0xEA2859B16914AA55uL), (+1, -1, 0xD18C1DA01CF44436uL, 0x20F8E94DF3E5C9DFuL)),
                ((+1, -7, 0x83B7EACA2BBB0A25uL, 0x21DBA593E3E26525uL), (+1, -2, 0x8FCB59D0C5D7C412uL, 0x488C31E510DBD951uL), (+1, -17, 0xEF22B5402CF1DCA1uL, 0xB048A439358227E1uL), (+1, -2, 0x8F6A03FD4A0FB813uL, 0x99E5A66E4E8395FCuL)),
                ((+1, -10, 0x927267B8191C0214uL, 0xB209D7286F256838uL), (+1, -5, 0xD348267A77EEE93EuL, 0x099A91BC376D3591uL), (+1, -21, 0xB5EAEA3442F6926FuL, 0x796F02E9A0338E1BuL), (+1, -5, 0xD1F154FE34027C6EuL, 0x1DD9D409ED9D3735uL)),
                ((+1, -14, 0xA3A717E28A60B667uL, 0x875F5A896533E3A7uL), (+1, -8, 0xAF44CCBD9F979F80uL, 0xE8610AF7869609BBuL), (+1, -44, 0x99EF82F87D26E4C1uL, 0x1278D9D6477EF942uL), (+1, -8, 0xAD37A41849BB54B5uL, 0xA61F2135B5467B59uL)),
                ((+1, -19, 0x92E71A69AC3CE161uL, 0xAA9070D519B4D967uL), (+1, -12, 0x9B92F54958E09565uL, 0xAB3D5EB80E2E1D46uL), (-1, -49, 0xB0417829C0067483uL, 0xA6D391A468E54BC6uL), (+1, -12, 0x98A8449192F56372uL, 0x1391A89A9A1CAF36uL)),
                ((+1, -54, 0xA7A8A8641B0F3BFDuL, 0x598697C7CB89C7ABuL), (+1, -18, 0xE6C119814C27C71EuL, 0xFE39339B84E32C56uL), (+1, -55, 0xE6A4078D2A32323BuL, 0xD33B2ED05E919CF6uL), (+1, -18, 0xE06EAF50ED488488uL, 0x75C27D5299142ED6uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX7p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xA837BE5170C5B4DBuL, 0x4B4D9DE623739E44uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0xE4254FBC4D79EAD3uL, 0x52D0B737C78D0D4EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xDF2BAA289379BBBBuL, 0x250DF1E1D6AAE8EEuL), (+1, -1, 0xCAD5F487D5B898B3uL, 0x09452D800EE40A14uL), (+1, -14, 0xB9E0EADE80B49925uL, 0xAFEB82E8AD4F9E1DuL), (+1, -1, 0xCB53D77353157164uL, 0x30D15D57E29AEC54uL)),
                ((+1, -8, 0xEE5C0B6C13654828uL, 0x0E3845E5C4CF1058uL), (+1, -2, 0x867FAFA43E0F9319uL, 0xAEB9625B01B68A97uL), (+1, -17, 0xCCD25BE0F2E99788uL, 0xC6E8DB160488C5F5uL), (+1, -2, 0x86FCB7AA2AF48624uL, 0x09654C9CD131FB0FuL)),
                ((+1, -11, 0xFFF3D06277937D91uL, 0x9154746E13C9B794uL), (+1, -5, 0xBEF98F10A42FD82CuL, 0x061DF914355CFBB0uL), (+1, -21, 0x976187B12093FF29uL, 0x9AF561E44997545CuL), (+1, -5, 0xBFA4A98CC86F6FCEuL, 0xF73BBC62E153F731uL)),
                ((+1, -14, 0x8A0B4678461960DAuL, 0xC031306E07F7CB55uL), (+1, -8, 0x99072A1684F11197uL, 0x3F7FE3BF406EAF85uL), (+1, -45, 0x9D3224CEBF5B86FDuL, 0x975422CC15F2A903uL), (+1, -8, 0x9953241A88DC56CEuL, 0x5321C85F21875D50uL)),
                ((+1, -20, 0xEF202EB12519EF42uL, 0xCE41D3A44D4C1C91uL), (+1, -12, 0x83297A461673F61EuL, 0x2A628493872DCB59uL), (-1, -50, 0xADAE53995E31D57BuL, 0xECD94426FC4D396EuL), (+1, -12, 0x8304A5B8ACE90B0EuL, 0x5A7456475F7B9C35uL)),
                ((+1, -55, 0xBA66492A91803E5EuL, 0xBDDF8ED2FECD8E48uL), (+1, -18, 0xBBCF13C8721FEF51uL, 0x7694005BA2BE1819uL), (+1, -56, 0xDB71B77E11DB22EAuL, 0xBC4D9103BDAB17C2uL), (+1, -18, 0xBAC250D1345EF516uL, 0xDA9048B965B8BB21uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX8p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0xA2F66B80715669DAuL, 0xFDD792DD3284CEEFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0xCF6DCCF7958D68B6uL, 0xF46F4CC29A13F369uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xD16268031B19AB5FuL, 0x4C03723284D9495FuL), (+1, -1, 0xC473ECEC16DEDD17uL, 0x0A66EF8FAB2F1FCFuL), (+1, -14, 0xA45921BA1B8EE621uL, 0xBAC616D17B6FA903uL), (+1, -1, 0xC55E50E147C662BEuL, 0xDCF7FA02ED322CEAuL)),
                ((+1, -8, 0xD86A57202E055A2DuL, 0x0115B879FB234F42uL), (+1, -3, 0xFC32E0BF26C07258uL, 0xD500A451DEB9300BuL), (+1, -17, 0xAFEAD7AA35920779uL, 0x8FA06949E46A031FuL), (+1, -3, 0xFE5069C5AD752D07uL, 0x3882E739B97C6B50uL)),
                ((+1, -11, 0xE0BD1172EFEAFA55uL, 0x66500590151E2D2FuL), (+1, -5, 0xAD3DA194A189585BuL, 0xEA69FF3B16560D9EuL), (+1, -22, 0xFC924223F815F5EEuL, 0x35167D378BEF7CF4uL), (+1, -5, 0xAF2773C639EBE8EFuL, 0xDEA4DE2D9DC684F5uL)),
                ((+1, -15, 0xEA505B37A6855BCBuL, 0x092CF023459C94CAuL), (+1, -8, 0x864315788489F193uL, 0xC4CFFBC8383CFDCEuL), (+1, -46, 0xA4CEF142766C1E70uL, 0x3A715898B6062ABBuL), (+1, -8, 0x87F15C5F3D1850E8uL, 0xBCDC1995E878B81CuL)),
                ((+1, -20, 0xC41532AF8FA2A3ABuL, 0xE22D36D3BA877A28uL), (+1, -13, 0xDE87CCCF50981F01uL, 0x5A7A139F053E55CAuL), (-1, -51, 0xAFC08A3DC4B0E574uL, 0xD50C3E845CF192E5uL), (+1, -13, 0xE15D1BE35BB06CF5uL, 0xF0BDAD20C5AC7514uL)),
                ((+1, -56, 0xD003609A0871BEEEuL, 0x14FF22FFA23FD1C9uL), (+1, -18, 0x9A00C98237931485uL, 0x7F511BEEFA0AF49CuL), (+1, -57, 0xD6770A4B207B33E1uL, 0xFE18E252A45BFCA1uL), (+1, -18, 0x9BCC7FDA9CF9F96DuL, 0x0B75BFEAC3849231uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX8p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x9E06929134192603uL, 0xFDC74EF4C15180CAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0xBD253F77A3E8FE8DuL, 0xBB3120F843FF6DFBuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xC4DB8E1E172E97EBuL, 0x6764F4D32EB683BEuL), (+1, -1, 0xBE79D250AFA4F6C6uL, 0x657A18E1A5E8E3A9uL), (+1, -14, 0x91BC683E2F4391E1uL, 0x95608FD3E5682312uL), (+1, -1, 0xBFAC818D06C30E68uL, 0xDD2B6166F11571AFuL)),
                ((+1, -8, 0xC521D2C67291AED6uL, 0x4C62864CD8FD9059uL), (+1, -3, 0xECF8F913394C3614uL, 0x3EE434B46DA757A9uL), (+1, -17, 0x978C9EB096344B36uL, 0xA68F9DD728EB7300uL), (+1, -3, 0xEFCB31326711338DuL, 0x9C1B2F9D06A928D4uL)),
                ((+1, -11, 0xC6389A15C16674ABuL, 0xDFFB9CEBC2AF113FuL), (+1, -5, 0x9DAFF8924DE5943AuL, 0x5ADFA082F26D1827uL), (+1, -22, 0xD3583A1AD47AF2F4uL, 0x91CE86F6C6118153uL), (+1, -5, 0xA0522BA7F486424CuL, 0xD45275237ABC65A9uL)),
                ((+1, -15, 0xC803A7BB6E883665uL, 0x4A1656D6246F665FuL), (+1, -9, 0xECAEB57B62258A4DuL, 0xE960AA2CE7A670EAuL), (+1, -47, 0xB172D405169554E7uL, 0xC295B93DDEA45E43uL), (+1, -9, 0xF18BF7BB71CFED3DuL, 0xE26D86080FE15233uL)),
                ((+1, -20, 0xA1EE7BF6714895BCuL, 0x2201F06743E34269uL), (+1, -13, 0xBDE12FED654E30F7uL, 0x9020004C25ADCEA2uL), (-1, -52, 0xB6ACE3243BB9A100uL, 0x2F33B67B69FC2EC8uL), (+1, -13, 0xC2509280F7AF191CuL, 0x349BB62F3D48EF05uL)),
                ((+1, -57, 0xE9C1C1055CE7F858uL, 0x43C6E4F6ABD8EAA4uL), (+1, -19, 0xFE5C8D354E9B5B34uL, 0x7A329885B108485DuL), (+1, -58, 0xD7527E1375737124uL, 0x806F26BA61F2E34DuL), (+1, -18, 0x825E3A0FEAC82523uL, 0x51D449E55E266AA9uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX8p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x99610591811283E7uL, 0xE362C045146D83CAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0xACF2D843B7D72791uL, 0xC933C37E18E2BFC2uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xB970BCB2407BE936uL, 0xC4DD8595D8D032F4uL), (+1, -1, 0xB8DDF14BC348149DuL, 0x6F23153E87D357E6uL), (+1, -14, 0x819B7EC3868BCDB9uL, 0x28F7C0C349DA23BAuL), (+1, -1, 0xBA3DAD8570823805uL, 0x90C78F2FC7E07F17uL)),
                ((+1, -8, 0xB41D80D102BFBF0CuL, 0xF193A785BB9ADA60uL), (+1, -3, 0xDF207728A376CAB1uL, 0x91CCE7B1A915B5F2uL), (+1, -17, 0x82F85B8AE433B8EAuL, 0x81287E026210EE30uL), (+1, -3, 0xE2584141C9E177C6uL, 0x756FE5AEF24306BCuL)),
                ((+1, -11, 0xAF921E0FBF2984D2uL, 0x82F7AF19A7BECA1EuL), (+1, -5, 0x8FFC9A2ECB23F3B0uL, 0xF87FA60E0578B56DuL), (+1, -22, 0xB1723E53DA2AB226uL, 0x7FDDE228A11C5508uL), (+1, -5, 0x92FC0ECBEC0160EDuL, 0x72344014213E265DuL)),
                ((+1, -15, 0xABA9FA5A89CC3E47uL, 0xAC24CC27386BBABDuL), (+1, -9, 0xD184BD66AF95FB9FuL, 0x988CF50E3A042F08uL), (+1, -48, 0xC44BA9F3A620AB83uL, 0xB472673A9DF4BB76uL), (+1, -9, 0xD711C749368EBC3CuL, 0x6F941FEAE348D7D7uL)),
                ((+1, -20, 0x869F0F8ECE5DED9CuL, 0x91BB45ACCEF9821CuL), (+1, -13, 0xA2E8D268206603D9uL, 0x90039E045751EF4DuL), (-1, -53, 0xC31C037E4CB1BA9CuL, 0xA746A14DEEBD2BECuL), (+1, -13, 0xA8035D6604BDBACCuL, 0x8F466ED46115903FuL)),
                ((+1, -57, 0x848B7D659476D0ADuL, 0x685217F5A2CABC4EuL), (+1, -19, 0xD3767304F4C7F61BuL, 0xB4F7F6AD554E29FDuL), (+1, -59, 0xDE2CD9F11E855B2FuL, 0xC8D6EEF63DB40F1EuL), (+1, -19, 0xDAEA6767BFEC6D57uL, 0x5354C8AD4B8F531EuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX8p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x94FF68278E341ED2uL, 0xB876E1E8B747754BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0x9E8C8B74C0AD0AC6uL, 0x3E938CF818DF5B36uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xAF00D72C1DF680DEuL, 0x612DC0416FD7286AuL), (+1, -1, 0xB39780DF6CEF8CF0uL, 0x65DB67D3C01FEBEFuL), (+1, -15, 0xE7302A6ED975341FuL, 0x9642ECA4329B5268uL), (+1, -1, 0xB5100DD966A2C7E3uL, 0x4B276D59DF1AFB49uL)),
                ((+1, -8, 0xA509441532F4E704uL, 0x824B0AA7BBEEF60FuL), (+1, -3, 0xD27ED059ECE6C9FDuL, 0x37936C41D046503CuL), (+1, -18, 0xE31A18BBFB965F38uL, 0x15E012CF303E22F2uL), (+1, -3, 0xD5E493DAA0DF6AB2uL, 0x2C8C92A7ABD9E02BuL)),
                ((+1, -11, 0x9C1EF22688824134uL, 0xC849847CFAA45CC0uL), (+1, -5, 0x83DDA2B29077536FuL, 0xC136D53E88523F2AuL), (+1, -22, 0x95849E2B87C2468DuL, 0x3B2DBA24CC8927CEuL), (+1, -5, 0x86FDC50E484F50EFuL, 0xF6283F85E52E4B49uL)),
                ((+1, -15, 0x941437B2BD0A81CDuL, 0x98F87DD368ECFDECuL), (+1, -9, 0xBA38197F84E37921uL, 0x1A1473FBD880BA10uL), (+1, -49, 0xDF2F25E95E198D74uL, 0x8ECE8F0DC7CD68D7uL), (+1, -9, 0xBFF177C55C4F64F0uL, 0x2D0EAD54782F5C1BuL)),
                ((+1, -21, 0xE13A7900FD82BA44uL, 0x8E9A26884A717A39uL), (+1, -13, 0x8C7C556DC85BD73DuL, 0x9566C376198217BCuL), (-1, -54, 0xD63C7DE8E2D5512AuL, 0x83B002E6A1FD21A0uL), (+1, -13, 0x91B39A43A5C06EA8uL, 0xE0B4DE83EDB6FBD9uL)),
                ((+1, -58, 0x97E5C8995B612147uL, 0xACAA5B62A4D36126uL), (+1, -19, 0xB0E4DBCA2A9E4878uL, 0xA251718CE5D9A19FuL), (+1, -60, 0xEBB711AB581D530FuL, 0x191AD92925AFF1B1uL), (+1, -19, 0xB875E5CE10939A1AuL, 0x972F3662ED7B93C9uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX9p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x90DC128DC9B207F6uL, 0x5895EA5212831A50uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0x91B4479E4473A839uL, 0xEC2BC18C5D5E0899uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xA56F587664593423uL, 0x6C2F36E51342588EuL), (+1, -1, 0xAE9EAA4974462DB0uL, 0x4EA7390853B504A0uL), (+1, -15, 0xCEC50D5CE59C4418uL, 0xF72546F32E49EC7BuL), (+1, -1, 0xB0213B25F7EA11F9uL, 0x91F7BE77AB82D0B5uL)),
                ((+1, -8, 0x979F0EF379FD9288uL, 0xB110061140399189uL), (+1, -3, 0xC6EF723F755E8C53uL, 0x4943E4AACEBCAF31uL), (+1, -18, 0xC588EFB231E73020uL, 0x2E3CCF484C701E08uL), (+1, -3, 0xCA5CEB70EBF7AADEuL, 0x3A22201959F5FB75uL)),
                ((+1, -11, 0x8B559CAD20F73ECEuL, 0x644536B6B0804601uL), (+1, -6, 0xF231FAB3F5C3F8B9uL, 0x660BCED69D5107F4uL), (+1, -23, 0xFCE586796C0F3E3CuL, 0x7D44A1CE78C3D405uL), (+1, -6, 0xF8654B1D6CC388C7uL, 0x7BC5DDADFE500162uL)),
                ((+1, -15, 0x80562C12B32F48B4uL, 0x509EEC69B9DA5F0AuL), (+1, -9, 0xA621605CA45E7DA3uL, 0x0C4AF351D45E37F6uL), (+1, -49, 0x826D6DBEE5718FF6uL, 0x273930A057144EE3uL), (+1, -9, 0xABB762AB5C88C970uL, 0x1A3244A4E04345C3uL)),
                ((+1, -21, 0xBD8042E8A41B1200uL, 0x75A83A5EF3F1E9DFuL), (+1, -14, 0xF3733E4381D607E0uL, 0x8BD08EA16681D95AuL), (-1, -55, 0xF1E8340C170DA108uL, 0x7459CE37789DF576uL), (+1, -14, 0xFD7951753F0FA966uL, 0xA59E42FE1C47D472uL)),
                ((+1, -59, 0xB010CBD0584224E7uL, 0xD889984CC2172C1FuL), (+1, -19, 0x94D570E45516F0D0uL, 0x294B487D83E5FADEuL), (+1, -60, 0x809E29BEE6C4A3C3uL, 0x62C49D694FBD6755uL), (+1, -19, 0x9BFFD5ED8324C43FuL, 0xC31F87585EE7EF72uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX9p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x8CF1F93DC5BB45BEuL, 0x2FFBDE74BC8827A3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -13, 0x8635C2A1525510AEuL, 0x833FE0E76961BC87uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0x9CA3AC8CF20F5336uL, 0x54E62E5359C777E0uL), (+1, -1, 0xA9EC7D479E0AA008uL, 0xAF2AE483B0280DDCuL), (+1, -15, 0xB96F57C8DE218EEEuL, 0x84D14194745CD484uL), (+1, -1, 0xAB6E72B62A0A93ADuL, 0x3AC855677DBD806BuL)),
                ((+1, -8, 0x8BA46B0611C0E819uL, 0x7D777ABD19F6FC22uL), (+1, -3, 0xBC52FCCB75BF401CuL, 0x4D9744DC81BE5DE4uL), (+1, -18, 0xAC5E8FB64D9F298AuL, 0xE891FA0B6D6FC7D7uL), (+1, -3, 0xBFAE902C4BB08B22uL, 0xFAAA5BAE08FD3D20uL)),
                ((+1, -12, 0xF98DED9D1AFB5B95uL, 0x7049CCFDBB5BD213uL), (+1, -6, 0xDEFCADBD467CFF5EuL, 0x4998FC32C2DF0A43uL), (+1, -23, 0xD6AB218E10CC1632uL, 0xE859B9E8E9A0CCEDuL), (+1, -6, 0xE4F2787A92CFEDFCuL, 0xE6A9698FF8DF2FC5uL)),
                ((+1, -16, 0xDF6D989D464DE7B9uL, 0x11C166BE89E2880AuL), (+1, -9, 0x94B977A3FB49210DuL, 0x58DFD409E8AFEF3FuL), (+1, -50, 0x9CABAF9F24C8C068uL, 0x50DD55B0F3049D77uL), (+1, -9, 0x99FE9A8404C5F6F3uL, 0x458CE1BE71EAF5C2uL)),
                ((+1, -21, 0xA04CB52F050A29B4uL, 0x8452FDDDFC33BC19uL), (+1, -14, 0xD3DF6A8661FABE55uL, 0x4B53AA8CA45722FEuL), (-1, -55, 0x8C70E80656738F18uL, 0x0CD115024933241EuL), (+1, -14, 0xDD2858F3B885E98CuL, 0x3F219B4BBCAAABC5uL)),
                ((+1, -60, 0xCE8355096938A18EuL, 0xFB9A339A2DD93ADDuL), (+1, -20, 0xFBCC4F4F5FB4F5C6uL, 0x28B5FDE81DE564DAuL), (+1, -61, 0x90647ACB9FC3AEEAuL, 0x102B69EE72D63448uL), (+1, -19, 0x846B17B9723BB33CuL, 0x01542A84FB9ECEC1uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX9p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x893C986EB666B7EEuL, 0xD618E5612AAFDCC7uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xF7C9733222D70C9DuL, 0x4DE7467414AF813BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0x94889958412F2029uL, 0x866485D7237EE093uL), (+1, -1, 0xA57ADADFEE9526FDuL, 0x7AA47E5CB9AD3503uL), (+1, -15, 0xA6BDCE044D667A5DuL, 0x45BD3399A82A4538uL), (+1, -1, 0xA6F4C42B1662AEA6uL, 0x1605EDD84D4735DDuL)),
                ((+1, -8, 0x80E86294E9BAEC29uL, 0x5664BF78F4A3F861uL), (+1, -3, 0xB28E807BBADCEB81uL, 0x3A4DB3CADB913A1CuL), (+1, -18, 0x96E2B41D9ABB4A9CuL, 0x5E6332C8BF90647DuL), (+1, -3, 0xB5C7B8AC5DF2C556uL, 0xA2BAD8F33024E129uL)),
                ((+1, -12, 0xE03162A1763E193CuL, 0x7ED059FB7B748F04uL), (+1, -6, 0xCDCAB555FBF40946uL, 0x584A7B855433AE99uL), (+1, -23, 0xB6E42FAB8A5001B0uL, 0x38C16B0E7114A399uL), (+1, -6, 0xD366CE3510C65855uL, 0x3A996DD16D532960uL)),
                ((+1, -16, 0xC34872C7AF9D0CAEuL, 0xCB47CDE7820ED1AFuL), (+1, -9, 0x8592D2A9DE3402D9uL, 0x2CCF322FB8BD0BF0uL), (+1, -51, 0xC14759CF8B92376EuL, 0xC86558DAFDBC1E60uL), (+1, -9, 0x8A6FC959E0287633uL, 0xF7F7E241A6B65E04uL)),
                ((+1, -21, 0x8847042733C95A4BuL, 0x844EE9101B4A7033uL), (+1, -14, 0xB926693A1DAF378FuL, 0xDE4FEB795E0F14B2uL), (-1, -56, 0xA799B0A42643E2DDuL, 0xBD31134BE0C5F797uL), (+1, -14, 0xC18D12DF48AB4717uL, 0x0E95E56D3374983AuL)),
                ((+1, -61, 0xF52B4DB2496DE5E1uL, 0xA2C48C50EC9A6F1DuL), (+1, -20, 0xD610659DFD3BD994uL, 0x1253381F02303D08uL), (+1, -62, 0xA6B57BA70D963692uL, 0x8A70FA3C70CD9199uL), (+1, -20, 0xE1A21886A1623899uL, 0xCD25A87A5A1F5D63uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX9p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x85B7E2B8A2B119E6uL, 0x6CE8B3489AB69D77uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xE5371430547560F3uL, 0x246D06C915ECAF84uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xC5FCACF0428A0E51uL, 0x17A87F1A96DAB349uL), (+1, -2, 0xF206AE44C7158662uL, 0xA86BF5B410ECC27CuL), (+1, -16, 0xCCACC48B45F645BAuL, 0xAE2FE08BD3E8F687uL), (+1, -1, 0x87E64CC5203C0945uL, 0xEEF049A32C71E85DuL)),
                ((+1, -10, 0xD97EF1290A9157CCuL, 0x5B8DFAD7FA3901D1uL), (+1, -4, 0xB5D7E11817F24B88uL, 0x5314D0318CA2D4D7uL), (+1, -20, 0xB7872004EEE3BDB8uL, 0xE1F68102D45C2094uL), (+1, -4, 0xE6E2DC71A34C658BuL, 0xF3FA78EBDACF90F5uL)),
                ((+1, -14, 0xD172E4034E0B2F05uL, 0x90398689338E39FFuL), (+1, -7, 0x87881FA119292E49uL, 0x6D4E2D9CED3F75ECuL), (-1, -42, 0x8A78759B4B1401DAuL, 0x3A999446F9DEC3CBuL), (+1, -7, 0xC422CFBCDFFA96E6uL, 0x10200BDB8540C42CuL)),
                ((+1, -19, 0x94D61564D1DAC5E5uL, 0xE778E6D4DFE1F4C9uL), (+1, -12, 0xC81E0B99D28DE285uL, 0x6A6F61A79C302D31uL), (+1, -47, 0x8778A5F1530B2F5CuL, 0x4BA0050E992BED83uL), (+1, -11, 0xA69B65EB7D592DEDuL, 0x08FBB372398F74D4uL)),
                ((-1, -52, 0xFC48143A93CC66CBuL, 0xCB46D7CA0A7B3702uL), (+1, -18, 0xE9CA9CBC018C44C6uL, 0x0FB903BF81DEAF17uL), (-1, -53, 0x9A754DE5FE4D84B4uL, 0x0018D0D971748E6CuL), (+1, -17, 0xE26AD90272BE2434uL, 0xCF62B21600F87DA3uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX10p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -5, 0x82603250247F3FF9uL, 0x1A59ADBBC0577B10uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xD474225FE3B7E67BuL, 0x152D1E4549308DBAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xBF7A122CD0E9955DuL, 0xA076CA0C21BCD115uL), (+1, -2, 0xEF2E72CAA443C8C3uL, 0x0213D7FD3466BAC6uL), (+1, -16, 0xB9220ECBF231FE32uL, 0xF5A88F102DD6B75CuL), (+1, -1, 0x848DE8D71DC5F547uL, 0xA3FB87EFB4E58A54uL)),
                ((+1, -10, 0xD163601B63F55D46uL, 0x181CE0CA1E3524B1uL), (+1, -4, 0xB1F9D8DB894DB4C3uL, 0x971146252C9D862DuL), (+1, -20, 0xA1CCE5CCED3B11E9uL, 0x4EF89A9E24F650E8uL), (+1, -4, 0xDB9FFB1EC3B26DAAuL, 0x7AC454F5C8325A89uL)),
                ((+1, -14, 0xC9B4340FB90777BAuL, 0xD418497463546CFAuL), (+1, -7, 0x83BB6D5E918893B1uL, 0xDD9DFF38519C4BE7uL), (-1, -43, 0xBFEA0A3DF08C0B30uL, 0x1F3EF832F8102E20uL), (+1, -7, 0xB5EC7E8C6875A728uL, 0x232B0F35364B3B9BuL)),
                ((+1, -19, 0x9049D8391B418A62uL, 0x4728F9D6F48BB8BEuL), (+1, -12, 0xC1D4E3F6502F764DuL, 0x27C9FD80047793CCuL), (+1, -48, 0xB6FAC0861E7AD4FBuL, 0x5C0B158C34C3C47CuL), (+1, -11, 0x96AB14AA319A77E5uL, 0x6CEB6519A86AA6DAuL)),
                ((-1, -52, 0x9332D3473E2B61C4uL, 0xECE2E7A6682A759FuL), (+1, -18, 0xE2A5D48B3BB5A9C5uL, 0xDF21BB82EDFF43A6uL), (-1, -54, 0xCB51B0C0B0DF73FEuL, 0x9EEDABC50A897B46uL), (+1, -17, 0xC79CCCB6A1BA56D9uL, 0x72B6FCE28C32B3D1uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX10p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xFE6478D3800813ABuL, 0x4FA66D03EB0FF8C0uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xC54982B5D80DC3DFuL, 0xB9A9FD2FA40748BDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xB8A3225B3CD76A20uL, 0x38EBB195F8582C33uL), (+1, -2, 0xEBBFECA847FFD0CFuL, 0x32E19956D49261D5uL), (+1, -16, 0xA7DC959F3A0A549DuL, 0x0C629DEAF6402A4EuL), (+1, -1, 0x815E11426FFFA80DuL, 0x7AF642690E9E0868uL)),
                ((+1, -10, 0xC80027AAF16CF075uL, 0x907694DED17C34A1uL), (+1, -4, 0xAD22E5E91CD0EF76uL, 0x9D9C73406461154EuL), (+1, -20, 0x8F16C2D0DB2D38D4uL, 0xC7232102B7A667BBuL), (+1, -4, 0xD129A35B8F883D9CuL, 0x14767073D8C4ACA4uL)),
                ((+1, -14, 0xBF62018F8EF34F6EuL, 0x602EB969A6FE5DE3uL), (+1, -8, 0xFD5FA1859A0832A3uL, 0x33E2E3FD64C04D68uL), (-1, -43, 0x8649BC585903AABCuL, 0xE61B8F6AD1B43A06uL), (+1, -7, 0xA90C66E037F79BE7uL, 0xAF41A8C9C5951D81uL)),
                ((+1, -19, 0x8876A9C92D1BF4E6uL, 0x09FE3FA581039F01uL), (+1, -12, 0xB8A46A4EC8650B8AuL, 0xBA0B138474E12239uL), (+1, -49, 0xF9B784430BE2F5A5uL, 0x870E4CFA17D63191uL), (+1, -11, 0x8896DBA50C77FBC2uL, 0xC0D210722508AD69uL)),
                ((-1, -53, 0xB05B351FE234AAE0uL, 0xF69A642EFABA11CAuL), (+1, -18, 0xD65B3D8C990F20B4uL, 0xEFE8EAEE8B46FE70uL), (-1, -54, 0x874BA4C019DD62C7uL, 0x9E11AC80BA282621uL), (+1, -17, 0xB08741EFABE89EB7uL, 0xEC9A04D5714BB038uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX10p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xF8560CD280C20239uL, 0xC164D86F95C2FE50uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xB787C7AEFD1D5A96uL, 0xB106BE088D72834FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xB1BF248BA309C9F0uL, 0x660675046DE26BE0uL), (+1, -2, 0xE7FD35648EDBD11DuL, 0xED6540C655CC21E9uL), (+1, -16, 0x988D5C78433A609DuL, 0x0B164D241232542BuL), (+1, -2, 0xFCA7F9DEF56D103EuL, 0x2704F5E78867FFF3uL)),
                ((+1, -10, 0xBE241D7B5CCA102DuL, 0xA9F88A8234C124FEuL), (+1, -4, 0xA7CD90F5F4ED95F8uL, 0x8413D287B90E40B7uL), (+1, -21, 0xFDD485D3E6AAB49DuL, 0xD50EC9D99A9C3406uL), (+1, -4, 0xC76CFB9A430C1B6DuL, 0xF4D02D087D886819uL)),
                ((+1, -14, 0xB400FA9F2D2F4AC5uL, 0xEB346BEB1DA17AC5uL), (+1, -8, 0xF21D138FB1AD67C5uL, 0x03E9FAA53085893BuL), (-1, -44, 0xBDAC1FFF3AA792DBuL, 0x8493FABEB2641C0FuL), (+1, -7, 0x9D5B38389B73C685uL, 0x323290DD20797F59uL)),
                ((+1, -20, 0xFE7893AE2CD346C2uL, 0xBA324A85A627C5E6uL), (+1, -12, 0xAE2A14AEF721E6D5uL, 0x09A57F8F06385743uL), (+1, -49, 0xAC1539007A47633BuL, 0xD3CEB4F738CA0915uL), (+1, -12, 0xF83B00A5A490BE10uL, 0xA0EF7329955E904EuL)),
                ((-1, -54, 0xD806FB5769DF2AD6uL, 0x8B058D234777D2BCuL), (+1, -18, 0xC7DC6E4E1C12DACDuL, 0xF34C5C6FEA314F01uL), (-1, -55, 0xB5F3F087B383E76CuL, 0x794DCF8AF5B4D07CuL), (+1, -17, 0x9C93285184A89873uL, 0x0A865E52C9DBDE0FuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX10p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xF28FB924F4F9236BuL, 0x933DFF0F34B08E16uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0xAB05F92E7C614098uL, 0x81B47B05A84982F4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xAAF5D91B427BAF3DuL, 0x1462C513AB43B3F9uL), (+1, -2, 0xE40E3BB145CBD748uL, 0x3A61712F585EA4F8uL), (+1, -16, 0x8AF1A1F2E41FBB2CuL, 0x7CC0AC70A1E2090EuL), (+1, -2, 0xF6DA46C00DC28272uL, 0x931CDB85BDC817E1uL)),
                ((+1, -10, 0xB442702DFF9AC404uL, 0x22FA2168FFBBC57CuL), (+1, -4, 0xA241CB572CDE3688uL, 0x1E20244337AD1315uL), (+1, -21, 0xE1C57F093BEE2DBCuL, 0x83C230D2E0B7787DuL), (+1, -4, 0xBE594D98C42A7748uL, 0x356F67F2591015A9uL)),
                ((+1, -14, 0xA8663FCD1185B0EDuL, 0x063BBDD257AF8347uL), (+1, -8, 0xE670261EFF3F2BDFuL, 0x1078FF131B1BD4DFuL), (-1, -44, 0x87216F0BC2D07296uL, 0x83D5316233E6F91FuL), (+1, -7, 0x92B6F62E99988499uL, 0x2BBBCBD9AF071901uL)),
                ((+1, -20, 0xEB3A2E9F8419BA62uL, 0xAC44E620F689BAA8uL), (+1, -12, 0xA34AD5A622C39B81uL, 0x883B05EA480A7512uL), (+1, -50, 0xEF66C1C8A4A8F422uL, 0x6C19AB8B67009AC8uL), (+1, -12, 0xE2100DBC6EC3F521uL, 0x52CE2AA3E17341D8uL)),
                ((-1, -54, 0x86DE0CC2584AF80AuL, 0x200652A51FE4E6EDuL), (+1, -18, 0xB8BF3FE20407A4A1uL, 0x4C480BD6323C0758uL), (-1, -56, 0xF7261CD162816915uL, 0x716EC8C2BCA7DFD8uL), (+1, -17, 0x8B4454DDF04AA402uL, 0x0D7267C4DF13F235uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX11p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xED0C93FCB09DF239uL, 0xCA6395C5DA80939BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0x9FA093BAA723AB2CuL, 0xE97AC9E3900B7F69uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0xA45D84C730FBBCBEuL, 0x4FF3AF74B3A855B7uL), (+1, -2, 0xE00C0C6EB5A0523CuL, 0x97EF73715ABE674DuL), (+1, -17, 0xFDA181EC6A3F0E80uL, 0x460FDA5BFF5CE58CuL), (+1, -2, 0xF14E6A40858AE783uL, 0xBBE292842F56D1CAuL)),
                ((+1, -10, 0xAA9BDCC8932AE4ACuL, 0x89CD90B56F68E3A8uL), (+1, -4, 0x9CAB1362281A6D4FuL, 0xF2AA4C748D31450EuL), (+1, -21, 0xC95A22CFCB42B678uL, 0x60EA10C7774FF093uL), (+1, -4, 0xB5DFBBC26F4DB514uL, 0xE6AAF78179D8838FuL)),
                ((+1, -14, 0x9D0781E94C0376EFuL, 0xF61705DF40D7D096uL), (+1, -8, 0xDAC91E2E30BCC18AuL, 0x393F082AC46E28AFuL), (-1, -45, 0xC22954395656193DuL, 0xC41BEBCC1CBEE151uL), (+1, -7, 0x8902227A477AE7B4uL, 0xF473A1FB00928394uL)),
                ((+1, -20, 0xD8499619090CA760uL, 0xCEA418838F88AA2BuL), (+1, -12, 0x9886FA190DFB6FCDuL, 0xE17551290A3FCD0EuL), (+1, -50, 0xA8050F59F6546934uL, 0x1E05FEC0BFD557C0uL), (+1, -12, 0xCE4FC21ED0A2BF57uL, 0x5617CBA763B3DD22uL)),
                ((-1, -55, 0xAB417BF18076405DuL, 0x8716A36C96C319E1uL), (+1, -18, 0xA9DF2BDCE2181991uL, 0x834ADA2E24D4B57BuL), (-1, -56, 0xA973C2685EF19367uL, 0xA2CF2ADB135A9D36uL), (+1, -18, 0xF86864B960F0B856uL, 0x03F475BDE312A2F2uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX11p25Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xE7C82343C1796B20uL, 0x0593B42E541CA64BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0x9538B449FCF43A57uL, 0x39B9820FA51548D4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0x9E0248BD8EAE4710uL, 0x351998A46A12B4DFuL), (+1, -2, 0xDC06C742E3BA5934uL, 0x99550BD6D4A65905uL), (+1, -17, 0xE7F4F3FAA98CA761uL, 0xDA3787EF134D5D53uL), (+1, -2, 0xEC002ABDC5581B9DuL, 0x9F4E8DB07C86027BuL)),
                ((+1, -10, 0xA154222369C49788uL, 0x766A1DFEDEC881C0uL), (+1, -4, 0x972416E79E7E0EE2uL, 0xBB2A02E1C4148FEFuL), (+1, -21, 0xB40863A968CDCDD8uL, 0x895F2445F72F97DCuL), (+1, -4, 0xADF302B603D862F7uL, 0x1EB7AC2E49CE353DuL)),
                ((+1, -14, 0x92246883790F9577uL, 0xAE4520125F5C6BBBuL), (+1, -8, 0xCF696B1E68746EF4uL, 0x7CFDE2C2EB022607uL), (-1, -45, 0x8C9ABE197743A01CuL, 0x9B3A0EBB5C517931uL), (+1, -7, 0x80230D6CEC3435A6uL, 0x6973F032CE840C87uL)),
                ((+1, -20, 0xC63A25E9C92BDC40uL, 0x4C46FFA525E1858EuL), (+1, -12, 0x8E258FED2564738DuL, 0xA6489775244742C5uL), (+1, -51, 0xEDDB513A5FAEC8B6uL, 0x950D0FE471824235uL), (+1, -12, 0xBCAAF2D9A4285CAFuL, 0x8610DEA2CF31C05FuL)),
                ((-1, -56, 0xDCC1451D36C94017uL, 0x153FE6238F7B89C0uL), (+1, -18, 0x9BAFEE750FE51EAFuL, 0x42F8E5D57E002E78uL), (-1, -57, 0xEA78CD716AD81CE1uL, 0xCA9F9E2DD63E08B3uL), (+1, -18, 0xDE1B31ED5A389258uL, 0x3D85181644FC3FC8uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX11p50Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xE2BE507B4DB4D5C2uL, 0x7155BF17A8B914D2uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0x8BB3681D2BCC4FF2uL, 0x9286D49C5A4D6438uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0x97EA34E5314A0C20uL, 0x3BEA58FD53F60BB4uL), (+1, -2, 0xD808F67E685527AFuL, 0xE08E45622B1FA75CuL), (+1, -17, 0xD48B1D2AF82D7B0AuL, 0xA81D54E0A13696D8uL), (+1, -2, 0xE6EBA8E50A8AE8DBuL, 0x5262B3F66A1CF3C4uL)),
                ((+1, -10, 0x987DD30FF7578F4FuL, 0x397FF129CD0A3C9DuL), (+1, -4, 0x91BD315C893A1814uL, 0xBF98D3AC6E44A3F9uL), (+1, -21, 0xA15D246C8DAEF577uL, 0x9A60F03E1FB668B5uL), (+1, -4, 0xA68744976FCEB941uL, 0x95460789EEBA177DuL)),
                ((+1, -14, 0x87DD2C1A18638AE7uL, 0xD54542AD3FC0B1CFuL), (+1, -8, 0xC47660FB39B7C7C1uL, 0xC0B546804DB8CA2FuL), (-1, -46, 0xCD310A7A608C4B32uL, 0xD3BD3B34CCE33840uL), (+1, -8, 0xF0068A0431A5A847uL, 0x866AE028BC3571F3uL)),
                ((+1, -20, 0xB553EECB3800E71CuL, 0xD04FD8D4E4D873F5uL), (+1, -12, 0x844C2399A25A5072uL, 0x3867104F041CC2B2uL), (+1, -51, 0xA9BAEEB15C70C346uL, 0x3D946FB5F2FB2AD4uL), (+1, -12, 0xACDE0C8C60BECA17uL, 0x4EC8A4C08F7E1942uL)),
                ((-1, -56, 0x9039D80475D894E3uL, 0x35092B2E77F57480uL), (+1, -18, 0x8E6A21430252A4AEuL, 0xA16D1941836EF49FuL), (-1, -57, 0xA39FBC3782DA0861uL, 0xF83FE9AF8B4EB322uL), (+1, -18, 0xC7130E007D618B8CuL, 0xC2D901FEF4C49716uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX11p75Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xDDEB5E26323B7CEDuL, 0x1A4903E55BE65C56uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -14, 0x82F919E77FA53F48uL, 0xCA1415BED738B481uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0x9217A4C3EA9F757BuL, 0x17CF4D237AF730ACuL), (+1, -2, 0xD419883B3D791E31uL, 0xF89FCB5C90AB440AuL), (+1, -17, 0xC31D3C5A1BBC609EuL, 0xD2237A2497BD120AuL), (+1, -2, 0xE20D56406BB0F931uL, 0x09CE06F6F0E94C76uL)),
                ((+1, -10, 0x902125824F97ECDBuL, 0xA7E9C00FD65A5E65uL), (+1, -4, 0x8C803912F0B88B8DuL, 0x7EABDAE28A8551B9uL), (+1, -21, 0x90F8007B150FE099uL, 0x616F2ADBCFB9BB76uL), (+1, -4, 0x9F91DC5C840C32C8uL, 0x4D1DCE039A37193AuL)),
                ((+1, -15, 0xFC7EE9C487F53CD6uL, 0x0480A3F368962162uL), (+1, -8, 0xBA0410257C0DF009uL, 0xE8580745A934E025uL), (-1, -46, 0x96CF7B9F363D1E79uL, 0xBC2F3BD6A4CD76F0uL), (+1, -8, 0xE11E38F62139497FuL, 0x19A57FA99598647CuL)),
                ((+1, -20, 0xA5B39BFF9599DC0FuL, 0x6951A7663055B2ECuL), (+1, -13, 0xF6189B4130AE0CD1uL, 0xEB9A8F7AED265961uL), (+1, -52, 0xF41CB2A15C3403A0uL, 0x038C2C462F58AE0DuL), (+1, -12, 0x9EAF2A94F9B4285CuL, 0xA7737049617A6685uL)),
                ((-1, -57, 0xBECDD9C9888D0AD4uL, 0xFE2C7400E3BBADB4uL), (+1, -18, 0x8224488473B9613BuL, 0xCD4380842D9F3232uL), (-1, -58, 0xE64094576D097387uL, 0x23331CCBC6A14019uL), (+1, -18, 0xB2D90DEE6EADC12DuL, 0x8D9769182BD58EC0uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX12p00Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((+1, -6, 0xD94BDE85CB25FFB4uL, 0x526FB440385809F5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -15, 0xF5EA2DCD482746F9uL, 0x59AE5488B9AB76E9uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -7, 0x8C8AAB757DB502A6uL, 0x2D5226133943D2A7uL), (+1, -2, 0xD03D04F69D48B67FuL, 0x1F5C16D30280F92DuL), (+1, -17, 0xB36E6E30C8CE63F3uL, 0x283CF12B921BE3CAuL), (+1, -2, 0xDD61ED01F5B080F3uL, 0x46D3C6FE2AE06634uL)),
                ((+1, -10, 0x884001F15E4C00D3uL, 0xC3396A4A8409F83CuL), (+1, -4, 0x8772CF9EB4679CB3uL, 0xF87E9B3597F6D8CEuL), (+1, -21, 0x8287EDA1073DE869uL, 0xA3360F0BA4ABF122uL), (+1, -4, 0x990937A86824144EuL, 0x06E8D2D1C95CBE93uL)),
                ((+1, -15, 0xEA9BD839238D8918uL, 0x75DE2EFF333EE97AuL), (+1, -8, 0xB01BC73C34BAE9D4uL, 0x42E8CC5ED267A715uL), (-1, -47, 0xDF3828F3178819F1uL, 0x6596974FBABD0533uL), (+1, -8, 0xD36A8FA217611D3FuL, 0x0BF8622E0EBA8E8CuL)),
                ((+1, -20, 0x975CE6A968C7DDC3uL, 0x84C11BF7EF934D08uL), (+1, -13, 0xE4D76293290E0B9AuL, 0xEAA833F3C2198765uL), (+1, -52, 0xB0D867995524738EuL, 0xC0B2A0424A15B02BuL), (+1, -12, 0x91EC87D0430E3884uL, 0xED8CF4ACB2F20DC0uL)),
                ((-1, -58, 0xFF51704DF8F88D1EuL, 0xAC1C1C7ED34205FBuL), (+1, -19, 0xEDC2A1A149D8233BuL, 0x2BBDA6051EDF9CC7uL), (-1, -58, 0xA3486672D7AA480AuL, 0x2D97D4F1CAE6525AuL), (+1, -18, 0xA109377D76762157uL, 0xB73034222F046CA4uL)),
            });
        };
    }
}