using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Ci(ddouble x) {
            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }

            if (x <= CiSiPade.PadeApproxMin) {
                return CiSiNearZero.Ci(x);
            }
            else {
                ddouble f, g;

                if (x <= CiSiPade.PadeApproxMax) {
                    (f, g) = CiSiPade.Coef(x);
                }
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return Zero;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble c = sin * f - cos * g;

                return c;
            }
        }

        public static ddouble Si(ddouble x, bool limit_zero = false) {
            if (x.Sign < 0) {
                return -Si(-x, limit_zero) - (limit_zero ? PI : Zero);
            }
            if (IsNaN(x)) {
                return NaN;
            }

            if (x <= CiSiPade.PadeApproxMin) {
                return CiSiNearZero.Si(x, limit_zero);
            }
            else {
                ddouble f, g;

                if (x <= CiSiPade.PadeApproxMax) {
                    (f, g) = CiSiPade.Coef(x);
                }
                else if (x <= Math.ScaleB(1, 256)) {
                    (f, g) = CiSiLimit.Coef(x);
                }
                else {
                    return limit_zero ? Zero : PI / 2;
                }

                ddouble cos = Cos(x), sin = Sin(x);

                ddouble s = (limit_zero ? Zero : PI / 2) - cos * f - sin * g;

                return s;
            }
        }

        public static ddouble Shi(ddouble x) {
            if (x.Sign < 0) {
                return -Shi(-x);
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = (Ein(x) - Ein(-x)) / 2;

            return y;
        }

        public static ddouble Chi(ddouble x) {
            if (x.Sign < 0) {
                return NaN;
            }

            if (x >= 720d) {
                return PositiveInfinity;
            }

            ddouble y = EulerGamma + Log(x) - (Ein(x) + Ein(-x)) / 2;

            return y;
        }

        private static class CiSiNearZero {
            public static ddouble Ci(ddouble x, int max_terms = 7) {
                if (IsZero(x)) {
                    return NegativeInfinity;
                }

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = EulerGamma + Log(x), u = -x2;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = u * TaylorSequence[4 * k + 2];
                    (ddouble p, ddouble q) = CRcpTable.Value(k);
                    ddouble ds = f * (p - x2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x4;
                    s = s_next;
                }

                return s;
            }

            public static ddouble Si(ddouble x, bool limit_zero, int max_terms = 7) {
                if (IsZero(x)) {
                    return limit_zero ? -PI / 2 : Zero;
                }

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = limit_zero ? -PI / 2 : Zero, u = x;

                for (int k = 0; k < max_terms; k++) {
                    ddouble f = u * TaylorSequence[4 * k + 1];
                    (ddouble p, ddouble q) = SRcpTable.Value(k);
                    ddouble ds = f * (p - x2 * q);

                    ddouble s_next = s + ds;

                    if (s == s_next) {
                        break;
                    }

                    u *= x4;
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
                        ddouble p = Rcp((4 * m + 2));
                        ddouble q = Rcp(((4 * m + 3) * (4 * m + 4) * (4 * m + 4)));

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
                        ddouble p = Rcp((4 * m + 1));
                        ddouble q = Rcp(((4 * m + 2) * (4 * m + 3) * (4 * m + 3)));

                        table.Add((p, q));
                    }

                    return table[n];
                }
            }
        };

        private static class CiSiLimit {

            public static (ddouble p, ddouble q) Coef(ddouble x, int max_terms = 7) {
                ddouble v = 1d / x;
                ddouble v2 = v * v, v4 = v2 * v2;

                ddouble p = 0, q = 0;
                ddouble c = v;
                ddouble d = v2;
                ddouble t = 1;

                for (int k = 0; k < max_terms; k++) {
                    ddouble dp = t * c * (1d - v2 * ((4 * k + 1) * (4 * k + 2)));
                    ddouble dq = t * d * (1d - v2 * ((4 * k + 2) * (4 * k + 3))) * (4 * k + 1);

                    ddouble p_next = dp + p, q_next = dq + q;

                    if (p == p_next && q == q_next) {
                        break;
                    }

                    c *= v4;
                    d *= v4;
                    p = p_next;
                    q = q_next;
                    t *= ((4 * k + 1) * (4 * k + 2) * (4 * k + 3) * (4 * k + 4));
                }

                return (p, q);
            }
        };

        private static class CiSiPade {
            public static readonly ddouble PadeApproxMin = 0.71d, PadeApproxMax = 256d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>> PadeTables;

            static CiSiPade() {
                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)>[] {
                    PadeX0Table,
                    PadeX1Table,
                    PadeX2Table,
                    PadeX3Table,
                    PadeX4Table,
                    PadeX5Table,
                    PadeX6Table,
                    PadeX7Table,
                    PadeX8Table,
                });
            }

            public static (ddouble f, ddouble g) Coef(ddouble x) {
                ddouble v = Log2(x);

                int table_index = (int)Round(v);
                ddouble w = v - table_index;
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

            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX0Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, -1, 0xAFB0BE68EA8ED162uL, 0xAD4EA72A26C46A6FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 0, 0xC564897AB0E651FDuL, 0x5541A0FBD4287D3FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xD6F122C43E892C8FuL, 0xA54859D2162CCC00uL), (-1, -1, 0xD7E5A04ED848B22EuL, 0x430A591E6DDDE427uL), (+1, -2, 0xC5034C0676455685uL, 0x4E4779AACFEE52C3uL), (-1, -1, 0xF6E284C70135D0A4uL, 0x2923A4514CF73767uL)),
                ((+1, -4, 0xC6D6DFF720D85464uL, 0x8D736A6B8223FF9BuL), (+1, -2, 0xD2E80E2F7EF409CFuL, 0x34381EED36BD1992uL), (+1, -3, 0x9DED82F21931B978uL, 0x2690FF2BCBDEBEB5uL), (+1, -2, 0xFF1D908565AA8739uL, 0x86C76F9791E25F6BuL)),
                ((-1, -4, 0x878F34B96619ECF7uL, 0xD9EB4569206D5302uL), (-1, -3, 0x8767805D6503599CuL, 0x49B9007762E5EA8FuL), (-1, -3, 0xA4A346C8AF04D0DAuL, 0x80422D8DDFCD0319uL), (-1, -3, 0xACBA47D6F5D9DBB9uL, 0xF356C6B0FBFBC5E1uL)),
                ((+1, -6, 0x9CC892897F1A49A3uL, 0x09EF65B2DAAAF01FuL), (+1, -5, 0x808F84A6610A002EuL, 0x968F324D143CD17EuL), (+1, -5, 0xF6CED37CE747BEA1uL, 0xCBD333A8E17C0E5FuL), (+1, -5, 0xAA0C0E0A8F7427C9uL, 0x711C7E7BA46BB2A2uL)),
                ((-1, -8, 0x806BC1D409DA9A5BuL, 0x231D5EC65355B28FuL), (-1, -8, 0xB803A7A2E0144C0CuL, 0x6FB0C2A96C67C70DuL), (-1, -7, 0xF185C7514F340784uL, 0xE629CE482F9CFBB8uL), (-1, -8, 0xFD6C006156DE1F77uL, 0x73AD54A233602730uL)),
                ((+1, -11, 0x899A9E51DA7C5BEFuL, 0x6C9F6B744A2B53E1uL), (+1, -11, 0xCF1665B0103E51C4uL, 0xF05B5EB4DADC2387uL), (+1, -9, 0xA7B4C9DB0A78A9B4uL, 0x2F70E5DECD0EBA86uL), (+1, -10, 0x938C18F77FD64F58uL, 0x016754FE01E11214uL)),
                ((-1, -15, 0xF3D9EDBFE02C698DuL, 0xE5765F71A87651C6uL), (-1, -14, 0xB71D29E2B77D6608uL, 0x7903699856B4826AuL), (-1, -12, 0xB3FA72AC3CEEDBFFuL, 0xA7669A8E001109CCuL), (-1, -13, 0x87673485DD297449uL, 0xC6FB7DAD1689CAE3uL)),
                ((+1, -18, 0x9CA2E47EEE2E652DuL, 0x362D0A686E62CCDFuL), (+1, -17, 0x833F3F00636ECF96uL, 0xE69519D30D01C4AEuL), (+1, -15, 0x93DD4AD766676934uL, 0xF025436827608636uL), (+1, -17, 0xC562A6C80D884403uL, 0xB0FE7DE2F9CE6D53uL)),
                ((-1, -22, 0xD23BBBB37712C76AuL, 0x467B7961C6804AD9uL), (-1, -21, 0x95728CFB3E619FDEuL, 0x5CE4D2057999E4FCuL), (-1, -19, 0xC11013E3DD9F0DA9uL, 0x3727FDC859114CE7uL), (-1, -21, 0xE01650FD3476AF4AuL, 0xA3464BCC8D418D11uL)),
                ((+1, -26, 0xA5020EF66D9A36E8uL, 0x6ABEDD24FCB61B10uL), (+1, -25, 0x88394A4ADD220F8CuL, 0x7337FC396F4B8F31uL), (+1, -23, 0xB77DE29CE62BAD20uL, 0xC97BA69CEEEF0CE4uL), (+1, -25, 0xC25D43F1A649A2F0uL, 0xF3D538A5937D4980uL)),
                ((-1, -30, 0x853783188F80BFD1uL, 0x9A0BB9D09535EE84uL), (-1, -30, 0xAFD0580BEBCE4F00uL, 0x641CE35F10BD3B1DuL), (-1, -27, 0x88F4A2155C729504uL, 0x00A368ADF312603BuL), (-1, -30, 0xEE336307B633E5BFuL, 0x54E1E4616A8459DDuL)),
                ((+1, -37, 0xBA686DFB2D5A05E3uL, 0xD13B7E447E5A1001uL), (+1, -35, 0x8313F8D4D920669FuL, 0x43ACF24DEC413267uL), (+1, -33, 0xE65E54BE8C80E47AuL, 0xE6966C4A1D85A1CEuL), (+1, -35, 0xC4414E16B73A1A2CuL, 0xF26B5C1785D87DC0uL)),
                ((-1, -42, 0xB51130DE51B77505uL, 0x1F2459EDFB954ADFuL), (-1, -43, 0xB00EDAD77E4E0F8FuL, 0x0F893F6C4E89FE47uL), (-1, -38, 0xA0A4F352400A2D6AuL, 0x8B19858D4032BF77uL), (-1, -41, 0xAD320C01B84E95A5uL, 0xA4338299986023E7uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX1Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 0, 0xA9A8C9632444EE8DuL, 0x8E58ACB79F88F52AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 1, 0xB2960494659EF30FuL, 0x8713576660D4BFBFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -2, 0xD9495A6A6DEC9D96uL, 0xFBB747D5774CC34EuL), (-1, -1, 0xDDE568595BA390AFuL, 0x5E84F7F0E511A5ADuL), (+1, 0, 0x99A58115B2D1D712uL, 0x75A5C932D174CFA8uL), (-1, -1, 0xEE4EBE0224BFC9F6uL, 0x8EFFFE7A8A3AD591uL)),
                ((-1, -4, 0xB325E0302BE22015uL, 0x0D4AA20CB34F477DuL), (+1, -2, 0xF49EFFC5E732DFA4uL, 0x5177488D9260BB4CuL), (-1, -2, 0xA556475ECB9542FAuL, 0x4D01E25D1F25FEA0uL), (+1, -1, 0x873E1F772B20F7E6uL, 0x715447E7AB8FE221uL)),
                ((-1, -5, 0x94D3D32B425AA8C9uL, 0xFB07B05902C00D8BuL), (-1, -3, 0xB46C711AE2FCA7E3uL, 0x8E93BF700A543681uL), (-1, -5, 0x9EE66D2BA5712119uL, 0x50D308E78CEA0B20uL), (-1, -3, 0xCCB40C7698EF4602uL, 0x6D5A9D0C5F021B16uL)),
                ((+1, -6, 0xA65DC5ECC9395940uL, 0x1E190092AEAC0804uL), (+1, -5, 0xCAB9C906E77BD8EAuL, 0xB2DDF5E0C158FC9CuL), (+1, -5, 0xAA1D6B72BA8436D4uL, 0x1B3B8E114D7733B1uL), (+1, -5, 0xEA8E8B7B6E4573D6uL, 0xFC9AE9623607AD0CuL)),
                ((-1, -8, 0xD69F86682E9EB521uL, 0x6BF6F13A99882668uL), (-1, -7, 0xAEAE2E6F12462892uL, 0x3FA76DBD80DBF784uL), (-1, -6, 0x8774A8A42ED5F8FCuL, 0x47F2176FD4162FB7uL), (-1, -7, 0xCE0251549F1938F2uL, 0x7F727FAC3B6E6E22uL)),
                ((+1, -10, 0xA2646D2B183B25C8uL, 0xAA17A705EA73A4F6uL), (+1, -10, 0xEF8195B5B633A4D0uL, 0x7435FD7F64A60237uL), (+1, -9, 0xF70425595440093CuL, 0xEA07CD229B47EE4CuL), (+1, -9, 0x90A28FD3229D8F69uL, 0xFD45F28B64FD3275uL)),
                ((-1, -13, 0xB9326196A7C438BCuL, 0x7E2B32F6A3FB6346uL), (-1, -12, 0x8271F6A91D370B66uL, 0xC056957375DF74AAuL), (-1, -11, 0xAFFCB0142A7C62D9uL, 0x2A2E1ADF3316A2B9uL), (-1, -12, 0xA16540FF38987B6CuL, 0x21BBC1C0FD0105DBuL)),
                ((+1, -16, 0x93F1D02EC86A83DDuL, 0x0140DE8EFF997610uL), (+1, -16, 0xE545B22686AB567DuL, 0x69E7B7CE1642BD07uL), (+1, -14, 0xB4FA3E67DCE86AB1uL, 0x53ECBBD68BFD9B13uL), (+1, -15, 0x91DBC24197A5DC5BuL, 0xD97E38D1649B9458uL)),
                ((-1, -20, 0xC81A1118D3960EF8uL, 0xB6BDF1DCB6A26E6EuL), (-1, -19, 0xA33759643D97F357uL, 0xE2AAE841E59DD919uL), (-1, -17, 0x9B8FF49E0909CA32uL, 0x7CC31C3395CAF2A7uL), (-1, -19, 0xD076FF8921EEB88BuL, 0x87E0AB04DD2642A9uL)),
                ((+1, -24, 0xF560EBA92F016D77uL, 0x889147E63B79ABC6uL), (+1, -23, 0xBE08593EBADEEB45uL, 0x35FC641353A9E39CuL), (+1, -21, 0xC0F9A759A33AD47AuL, 0x0053DE6400176749uL), (+1, -23, 0xEA898D3F1D464785uL, 0x4F0FA261B379814FuL)),
                ((-1, -27, 0x80A0C205724B38B4uL, 0x4869F31EAF9160F3uL), (-1, -27, 0xB3735DD43716B784uL, 0x07551A947425D822uL), (-1, -25, 0xC29B80AF18E8EEFAuL, 0xEE7527BC565A0BD3uL), (-1, -27, 0xC10FAC1F820AB0B1uL, 0x198EBA5FFC3D56ECuL)),
                ((+1, -32, 0x963934148CBF3F83uL, 0x49063C31E7475102uL), (+1, -31, 0x80DB24CD4E3A8BC5uL, 0xFB0968CDC4B63021uL), (+1, -30, 0xED0C218BB20A2FC7uL, 0xEFD01A0754BFFB0BuL), (+1, -32, 0xD3213106806C116FuL, 0xE0BA8028AF49AEA4uL)),
                ((-1, -37, 0x8B1D6255A6B4C57DuL, 0x6B493F66CE1ED238uL), (-1, -37, 0xC9C341A0E9848780uL, 0xAB9209F61B97023AuL), (-1, -35, 0xBE47B188584E930CuL, 0x4FCEA59C676F3208uL), (-1, -38, 0xD7DE7169F0529D45uL, 0xDF1B4B1A364AD7C7uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX2Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0x880606CC9491D525uL, 0x01B4FBED4E047218uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0x8A998E3ADB7DD5E4uL, 0x5E3B774A8AE7C578uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xCE98739EB80D1613uL, 0x2FC9CD69080F6177uL), (-1, -2, 0xE929EE9BAACB6ABAuL, 0xCC89F4B7701EEFE6uL), (+1, 0, 0xBF67118BA77D8B96uL, 0xEB3AE09A1E75E125uL), (-1, -1, 0xBB67FE4BA9A7F1C9uL, 0xFA49D3E5A8F9E5ADuL)),
                ((-1, -2, 0xAD649E001DAF2AE8uL, 0x850D266EE822B599uL), (+1, -2, 0xA2D39F92CA1C1F04uL, 0x92550A3058AFCBD6uL), (-1, -1, 0xE13B28DF65204803uL, 0x1F043BDD13B2A894uL), (+1, -2, 0xEA89092ECD542F55uL, 0x16C4C66BE63A26ABuL)),
                ((-1, -5, 0xA5F7C80642E397DCuL, 0x43D42206655F988CuL), (-1, -4, 0xBF5AB2B6EE7A922EuL, 0x41FDAA12C92B5EC4uL), (+1, -4, 0xC3CCF5DF245A993DuL, 0x58D7E9ED2BA852FAuL), (-1, -3, 0xB34A36C87D0E650FuL, 0x029086F6641ABA89uL)),
                ((-1, -7, 0xF356C1AC8F055DECuL, 0xA06189205427BB51uL), (+1, -5, 0x8D3FCA1E6E7D8E55uL, 0xF2959C1963B53CFBuL), (-1, -6, 0xAD3B8820746098ACuL, 0x4181E4EB499F8416uL), (+1, -5, 0xEC450E34A29EBFC0uL, 0x712CCFD3AAD0F9AEuL)),
                ((-1, -8, 0xE670E2EBFFD959C4uL, 0x6FEB1F52D774EDA7uL), (-1, -8, 0xE48B6243A6F194BBuL, 0x1A62004C7312567FuL), (-1, -7, 0xAB3533C84F8929E1uL, 0xACE89A97F4C2B604uL), (-1, -7, 0xE3C4AF0C54197D65uL, 0x64E40E1445A5168FuL)),
                ((-1, -13, 0xCF23200B79A049C3uL, 0x558C0062DE6EF449uL), (+1, -10, 0xDF68CE0341389B20uL, 0x3CCF6AFEDC8ACF79uL), (+1, -9, 0x9C0909126D476817uL, 0xED7211CB38B23560uL), (+1, -9, 0xBEFB751E263888E2uL, 0xF553C907AE32C053uL)),
                ((-1, -12, 0xBED655BB07CED01EuL, 0x3B065E23D53542B8uL), (-1, -13, 0xFAB3D9B00C8A99C2uL, 0x6041F5378D462E45uL), (-1, -11, 0xE3A5C60E80CF2F8DuL, 0xA3045E7255407329uL), (-1, -12, 0xF43EA6347E511F42uL, 0x4E658AAA40EC41C0uL)),
                ((-1, -18, 0xED6386BAD37B6892uL, 0x2C183F6D73807B71uL), (+1, -15, 0xADC32D79F1F4EEFAuL, 0x43C0F8A2207D2DECuL), (+1, -14, 0xE21D9206892B313FuL, 0xF6D19169110A9580uL), (+1, -14, 0x887FEC58B5FFC627uL, 0x78B728F64CD0F6F8uL)),
                ((-1, -17, 0x87B421E6027B984AuL, 0xEE2D9DDEF5E14F14uL), (-1, -18, 0x82DD89A883E018F0uL, 0xFABD82FFA61DA3D1uL), (-1, -16, 0xA48F888177525C6EuL, 0xD9177BF8FFD37E79uL), (-1, -18, 0xE7EB2B8D35368134uL, 0x9D3D21EF8A16A28AuL)),
                ((-1, -22, 0xB765BFCF4B18CE9EuL, 0x20F0B1501977F670uL), (+1, -21, 0x8173FDA5504B33FCuL, 0xC0A1AE2DB72BA60FuL), (+1, -20, 0xBEBB80937A09857AuL, 0x95F636F479163360uL), (+1, -21, 0xACEE4F55BAC3F5E4uL, 0x77F9D67DE8027A69uL)),
                ((-1, -24, 0xB72A043F6C076DFAuL, 0x81DBDF1259198BA8uL), (-1, -26, 0xF20AB990E0DBABF7uL, 0x9184D7F4612AA86BuL), (-1, -23, 0xBE011AAC62B842C1uL, 0xCCA76E09EC3A3624uL), (-1, -25, 0xBE474C811DC6D689uL, 0xF5FE646669FF635AuL)),
                ((-1, -28, 0x945790434846E44FuL, 0x2780E89D81EB8CBEuL), (+1, -29, 0x9A5B9FE63918D61FuL, 0x615F806BED8E9328uL), (+1, -28, 0xF77AF1D808E1BE32uL, 0x6D525E5825F35304uL), (+1, -29, 0xB25106F95F5A3435uL, 0x1BE8B512C01EC56CuL)),
                ((-1, -32, 0xC9309157005CF290uL, 0x970424C4B038A70AuL), (-1, -34, 0x9A9737439E4CC0B9uL, 0xE01BFF2726F0845EuL), (-1, -31, 0x90AD9F4DB5988294uL, 0xE847CF97298B5462uL), (-1, -34, 0xE3A158A83ADB9718uL, 0x919763D79627BC5EuL)),
                ((-1, -38, 0xF097706630A43E29uL, 0x7E6B19FA765A4C13uL), (+1, -40, 0xE5207265D2DC189EuL, 0x7F5EF43B889F81C0uL), (+1, -37, 0xE159A54FA8EFF4C9uL, 0x0D0C9393348803B2uL), (+1, -39, 0x9C5A9F5C41446010uL, 0x92FF0644B0F20095uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX3Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 1, 0xC2871D4AD153C102uL, 0x3DFD1FF579D0820EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xC394005CCDE8546DuL, 0x595B696648852F04uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0xBFF57866ECE32BA3uL, 0xEA6E2EB613F79051uL), (-1, -4, 0x879D1297C1A1B152uL, 0x1701DAE0EA227292uL), (+1, -1, 0xFBF976BBD56CB9F4uL, 0x8C7CE3E96AF4A2C4uL), (-1, -2, 0xEEE9711EB668E59DuL, 0x124535F0BD80EA18uL)),
                ((-1, -1, 0xCE3B18ADF112CF41uL, 0x5B6D3D3C0B9B0F3AuL), (+1, -2, 0x8DACEFB35EBE8B59uL, 0x36D7DD57A7438084uL), (-1, 0, 0xB07743B246F64983uL, 0x4FAAFCD4A2C148D9uL), (+1, -2, 0xB6BEF37818D74AEBuL, 0x115F0E9CC6398AD0uL)),
                ((-1, -3, 0xD4B56C6CC758E015uL, 0x523CF48EDEEACF0EuL), (-1, -7, 0xF33957F8340CAAAFuL, 0xF0D01342EE2A985AuL), (+1, -3, 0x8C8E1C598BCFB0EFuL, 0xC88DF92F7F858435uL), (-1, -4, 0xFC9EECDF5519BE0CuL, 0x81E5D1719A6363C1uL)),
                ((-1, -4, 0xB48967A1E9D81BDBuL, 0x8A0C4DF19CDD3D3BuL), (+1, -6, 0xFA74E41312836785uL, 0x4EBBB91AB0B94E23uL), (-1, -4, 0xC292B9C391DA29DAuL, 0xDF302DD04B0A7B18uL), (+1, -5, 0xC33B7F49670A4842uL, 0x5140062508424DF7uL)),
                ((-1, -6, 0xB807FC932FBABCB6uL, 0xBD50B7FD0655BA42uL), (-1, -10, 0xA89C1CD82FE5D30EuL, 0xF37CF44657BB1B68uL), (+1, -9, 0x8CC297B04631C80FuL, 0xCD32D5638FB993EAuL), (-1, -7, 0xCCB6CDE8E6A0E147uL, 0x32EB0D975BAA7395uL)),
                ((-1, -8, 0xA896F8E8D4800C94uL, 0xFEB388962DFC8773uL), (+1, -10, 0xDEB60A49324EBB0FuL, 0xD44981BBFAF69CAFuL), (-1, -11, 0xD8135BF7AE38A3F2uL, 0xC21F824A858F4354uL), (+1, -9, 0xC82786F1A85864FFuL, 0x365DDFC6218CF8DBuL)),
                ((-1, -10, 0x9AAB59A41C9CFB7EuL, 0xD010C2B2D9582095uL), (-1, -15, 0xE953F4472236F9ADuL, 0xDED86453A6345084uL), (-1, -12, 0xCF1594A4E8AE2BD5uL, 0x2430B7FD8726F6F9uL), (-1, -11, 0x9EE050BAD0AE59C5uL, 0xCE80AB922CEFE4C4uL)),
                ((-1, -13, 0xB1D950926B81E72BuL, 0x3421203B8853FEC2uL), (+1, -15, 0xCD4D153C345389C1uL, 0x90788A1B2AD933D8uL), (+1, -13, 0x92AE04FECF3BC433uL, 0x94BB91CA45FE847BuL), (+1, -14, 0xCE6454805034949BuL, 0xC458CD0963B74874uL)),
                ((-1, -16, 0xFEE4A6806D53D26AuL, 0xA8329E4332AA1C03uL), (-1, -20, 0xAFA723DC7B91B211uL, 0xF41447CB2F24F043uL), (-1, -16, 0x9716C55667F06141uL, 0x33795B1D7A7C11B3uL), (-1, -17, 0xEF8E65D3CE58D4D4uL, 0xB1D29B02B4F6352FuL)),
                ((-1, -19, 0xC787C8C13FA03841uL, 0x28C19366614C4053uL), (+1, -21, 0xB489B6D30643CFC6uL, 0xF1E0894CD0D44BC1uL), (+1, -18, 0x9CC4B982A8791605uL, 0x2161AF9E89C27343uL), (+1, -20, 0xCA22EDFAD7608876uL, 0x955C848156E99F4AuL)),
                ((-1, -22, 0xB5E79420FC6634DDuL, 0xFCD898150060D855uL), (-1, -26, 0x8DE0622D7FCE775CuL, 0x2729F4C4AA8FCA50uL), (-1, -23, 0xFCF8B2DEB8B6D2BCuL, 0x46C022F3746A293CuL), (-1, -23, 0x9A9FC84089B9D23FuL, 0xD2AA81D07AC8A4EEuL)),
                ((-1, -26, 0xB9D53D4268A90F65uL, 0x2CAE0E3628DEEED0uL), (+1, -29, 0xE9FD57D22D9D95F1uL, 0xE370F9B5223100FDuL), (+1, -25, 0xB46A07C9F49BA34EuL, 0xE0B19600748C8554uL), (+1, -27, 0x99CE1F9277238EDEuL, 0x988DB08E181EC7DEuL)),
                ((-1, -30, 0x9B22F86B73D341F1uL, 0x6DF993F728C15F17uL), (-1, -34, 0xB7538AB1CAF4515CuL, 0x2A1074F8C42B6E68uL), (-1, -30, 0xAEB53E5925043093uL, 0x58508C6A0938ABF3uL), (-1, -32, 0xDC4DB927D19B2898uL, 0x3510474F4A4029CDuL)),
                ((-1, -36, 0xB29C931553ADF20EuL, 0x415C0426A70406F2uL), (+1, -39, 0xA16363466AA937B2uL, 0xE4CE1A4BF6426753uL), (+1, -34, 0x81E564B6C24BEC7AuL, 0xF88FACDA8D1096D6uL), (+1, -37, 0x92C49BAC57A1D9D4uL, 0xCFCF4B4EECEEC53CuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX4Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0x8058C282B5B3C1D4uL, 0x2DEFC3DDA5998F63uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0x8082863B8623DC89uL, 0xD48AFA9F894EC39BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 1, 0xCDF120291B900ED4uL, 0x17D69D95FD48FF9AuL), (+1, -1, 0x8E7B6F1A79A633AEuL, 0xD214956060273D47uL), (-1, 2, 0xA31CEB2A3119BFF2uL, 0xFD95E1BC3409E854uL), (+1, -2, 0xC81ACEBA243691DAuL, 0x74DC1D3E9F2628D7uL)),
                ((-1, 1, 0x88601F3FF18B75FEuL, 0x51ACFF5B74CE8424uL), (+1, -2, 0xC8CD3F2F2D58133AuL, 0x033FFA733149B326uL), (-1, 1, 0xEAEECE15FDD918AFuL, 0x04C364C68E0AD85EuL), (+1, -2, 0xB78D6042F1A42592uL, 0x82247D21FCB8CF3EuL)),
                ((-1, -1, 0xF6932C01456E63B5uL, 0xB8D3FAC86827FCF7uL), (+1, -3, 0x92DC0E233D391515uL, 0x70AAC626F709E1BFuL), (-1, 0, 0xCA1F5AC637E737A9uL, 0x2B9D6B0BBB7E58D5uL), (+1, -4, 0xDF8A4150FE9645D8uL, 0x344FADCD157D470AuL)),
                ((-1, -2, 0xB7F1522ABE3BBE75uL, 0x6384A23851F9AA6CuL), (+1, -5, 0xDBF0FCD356F8D8ECuL, 0xB6A3C826EB4CC1E2uL), (-1, -1, 0xA04D8EDEF8DBFAF3uL, 0x48838A5C16259C63uL), (+1, -5, 0xCE66B225E7B67D95uL, 0xA1D783088EFB6150uL)),
                ((-1, -4, 0xDDA5379F8768DF01uL, 0x7F80294EF848232BuL), (+1, -7, 0xE0CC4F6EC44B1C2CuL, 0xCDBB5BE3F2D51357uL), (-1, -3, 0xC6CECE3497F915EDuL, 0xAE8706403F35CF54uL), (+1, -7, 0xC2B0486931CBCBD4uL, 0x72170E59104018F6uL)),
                ((-1, -6, 0xDE4B0CEF3B043E4AuL, 0x4DF3DD1DF9866C85uL), (+1, -9, 0xDA2578C7FD68E49CuL, 0x726C2BAEC0E67F1EuL), (-1, -5, 0xD56920FFCB3E660DuL, 0x3048DDB86F67C2D2uL), (+1, -9, 0xE3F3B43645DE9741uL, 0xC405B373477F76E6uL)),
                ((-1, -8, 0xBA30DE854858A50FuL, 0x44AE103DB0180785uL), (+1, -11, 0x9BD34261F1FC74A7uL, 0x69D71D28DD52F4E7uL), (-1, -7, 0xC06BF130446CF9A5uL, 0x32D88B68F6D0C5CBuL), (+1, -11, 0xA12C51FD593F7DB3uL, 0x3BCFEFD28F1AE4BAuL)),
                ((-1, -10, 0x80C45F4E68AA0EE8uL, 0x1BEBED3A805DC094uL), (+1, -14, 0xC8C9377DE20227CEuL, 0x7533067F18266F6EuL), (-1, -9, 0x90D828D8A40ED388uL, 0x463CCC9C3195E77CuL), (+1, -14, 0xF991FB6596F45A21uL, 0x27BB0AB5C2663348uL)),
                ((-1, -13, 0x92D926C775F36AF4uL, 0xABBFD24E46EB2A9CuL), (+1, -17, 0xBD837342FABC554AuL, 0xAADDE705AD7728F0uL), (-1, -12, 0xB73576148BA9B6D0uL, 0x39BCD3D988C466A0uL), (+1, -17, 0xF4AC2C353BB374E5uL, 0xC8C77AE080D9B877uL)),
                ((-1, -16, 0x85A64769248D6BD7uL, 0x10FDCEC39F19D13BuL), (+1, -20, 0x973E79B813798390uL, 0x2B7BC8259DAE3FE4uL), (-1, -15, 0xB8B754F32298C28CuL, 0x993436F7EFCF13A9uL), (+1, -20, 0xEC0C504E9DEE43A2uL, 0x42662F3490CFFB7BuL)),
                ((-1, -20, 0xBD59B6DBE0469AD9uL, 0x8947DA89DEA83259uL), (+1, -24, 0xA0B7FE91BF1F6627uL, 0x6179A650E73368D5uL), (-1, -18, 0x9420223E2F120C7CuL, 0x7DF12AC25E668B1BuL), (+1, -23, 0x85EB19122BBD1AADuL, 0xD5C18B6D1C106E5FuL)),
                ((-1, -24, 0xC304796AC91276ECuL, 0xAD2F95995222A3CFuL), (+1, -29, 0xF942808585BA970EuL, 0x8C70B3CD609EF57CuL), (-1, -22, 0xAB3151D577404400uL, 0xFCC9AC317A0ADCA7uL), (+1, -28, 0xFF5F16C96F0D9F32uL, 0x452205743EC19F9DuL)),
                ((-1, -28, 0x8314E3D2BE95BE78uL, 0xC9459E327DECD7D1uL), (+1, -34, 0xA0012EB3EE2A842FuL, 0x9A3CC5513A3F59B7uL), (-1, -26, 0x844326DC41956D01uL, 0x7AE422F7DF22C2D5uL), (+1, -33, 0xAE3C92AB1DDB0AD6uL, 0x7E1974E348ED9B7BuL)),
                ((-1, -34, 0xABB09AB98D34EE85uL, 0xEDAF98C866921A7EuL), (+1, -43, 0x80EA1783D68449B9uL, 0x0801D0EA053E8179uL), (-1, -32, 0xC3F784194E30FA68uL, 0x8507CD78F22E1D4FuL), (+1, -42, 0xF85F9798AC9D80F5uL, 0xFB07BC4824CEE8F4uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX5Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0xA016D79A755622ECuL, 0x85EF07584A8E82DEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xA022128AAC8054DDuL, 0x2EF3145D6840C907uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 2, 0xDD3AE1798F90B2C1uL, 0xEB9A68ACB56388F5uL), (+1, 0, 0x9765C379837BF7EBuL, 0xFD99A23DC398C04FuL), (-1, 3, 0xD9935FDA9A246C3CuL, 0x7B757141FF75A634uL), (+1, 0, 0x947B7A733976CA35uL, 0x07A3AF59F83467C2uL)),
                ((-1, 2, 0xACA0DD760EB7BF1AuL, 0x35FD418F8C20CF16uL), (+1, -1, 0xD79FF32571DFE510uL, 0x0B4DB9D817E42A77uL), (-1, 3, 0xAB799B90C3AB3184uL, 0x67AFF990C5A3F05EuL), (+1, -1, 0xD6EE1DF9B2495445uL, 0xC8F6E4DF420D1240uL)),
                ((-1, 1, 0xB8AECCC3C610E34BuL, 0x53E4EDA848D0C38BuL), (+1, -2, 0xD1425D92CF2B7F59uL, 0x01A4A7787FF47262uL), (-1, 2, 0xBA6159F9AD8B4E4FuL, 0x3B33279D6CF940FEuL), (+1, -2, 0xD443B3D9C2F7A9BFuL, 0xEB813BAAFAC6A7A2uL)),
                ((-1, 0, 0x93E8B6C25384C2ACuL, 0x2A7C7B096F4A9753uL), (+1, -3, 0x98F0FB5B530694CCuL, 0xE9B776CFAE8FDE5EuL), (-1, 1, 0x98D1E782EF3C39F0uL, 0x1B5D856A125B7FA1uL), (+1, -3, 0x9F96DCA0CD5891BFuL, 0xFCA8C2B69F2058C5uL)),
                ((-1, -2, 0xB7FB8B67DF20BDA9uL, 0xFC1D3A0C1ED9127BuL), (+1, -5, 0xAC07D7C6536554BAuL, 0x141DCF83B7874474uL), (-1, -1, 0xC3AC40B8D2C0BFD6uL, 0xD65419D40966160BuL), (+1, -5, 0xB96D2469263F30E0uL, 0x88ABCCB39C1BEED1uL)),
                ((-1, -4, 0xB4D42F05C629820DuL, 0x87405D45957C935BuL), (+1, -7, 0x97B07ADF7A404E17uL, 0x9C8FB3514C29C7F9uL), (-1, -3, 0xC7224CE3CC79ACD7uL, 0xA759322904B88F70uL), (+1, -7, 0xAA404114E1977840uL, 0x2D896ECC2B7F0D22uL)),
                ((-1, -6, 0x8CE75E7F37759B8BuL, 0xA6A66C342FF64194uL), (+1, -10, 0xD035D068A37ECC9CuL, 0xDAE78F2CAAF8E82EuL), (-1, -5, 0xA1A03E9C8AEBF444uL, 0xF79ED07EF6335439uL), (+1, -10, 0xF4D70AAEBA30D2E8uL, 0x78D0EB35A544A362uL)),
                ((-1, -9, 0xACB8C4DD45B91DFBuL, 0x96C9862BFC4E5A82uL), (+1, -13, 0xDB8A6822C7FE639DuL, 0x5360C60EEB18B276uL), (-1, -8, 0xCFC1F575A21434B4uL, 0xD1F45560B4A66034uL), (+1, -12, 0x88829B38E0622244uL, 0xD2D190A75AEFFDF4uL)),
                ((-1, -12, 0xA34C55D8515B809CuL, 0xDA4476BA74B870C9uL), (+1, -16, 0xAB55DCB16603FF50uL, 0x7DB83F77F909C323uL), (-1, -11, 0xCF9CD2891B7E6844uL, 0x630A6696070DBF9AuL), (+1, -16, 0xE39C71FB77C4762CuL, 0xB183E74CC031E114uL)),
                ((-1, -16, 0xE5A5E5A06A7DF488uL, 0x97A66C2580241C2EuL), (+1, -20, 0xBA7DC1433AE3FBDAuL, 0xE000E6070E25DBB4uL), (-1, -14, 0x9BD0216D28BF4C7CuL, 0x8520D938FB81B03AuL), (+1, -19, 0x8654ADC5CE3A27ACuL, 0x3119165772D96A4BuL)),
                ((-1, -20, 0xE16ADC820BB8F827uL, 0x1FE30DBBD909585FuL), (+1, -25, 0xF98447D2F44E0442uL, 0x8C8BD110E51E4096uL), (-1, -18, 0xA564F0647EB1FC90uL, 0xE1D4A7D22684F104uL), (+1, -24, 0xC71CC8855B2E7610uL, 0xC4E809E2AF3873C6uL)),
                ((-1, -24, 0x88BADC82C113D813uL, 0xC7919654F5F11D82uL), (+1, -30, 0x9844C94B7D2FA7F6uL, 0x02B8175DD02380ECuL), (-1, -23, 0xDD062C038F9BF1B0uL, 0xC899C973016377C1uL), (+1, -29, 0x8B8C020F5D9B0BE5uL, 0xEAC57B6F773BFCA5uL)),
                ((-1, -30, 0x98016C0F5BD5A97FuL, 0x444958DAF2FF2A66uL), (-1, -45, 0xB42942330890A8BFuL, 0x083E53E398813879uL), (-1, -28, 0x8B627237E74931D5uL, 0x39B25E4D3A9CE99EuL), (-1, -45, 0xBF8EB3A361EC31A9uL, 0x0BB733E0F964CEF8uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX6Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0xC005C161B1094D7BuL, 0xD7DC99630DD4D5A0uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xC0089EE09ADEBE8CuL, 0xD39206F127E4E276uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 5, 0xB0B56F90ABBFEDEAuL, 0x6B0127FB2747E2ACuL), (-1, 2, 0xF0E95DD81D833B2BuL, 0x109697715F05E656uL), (-1, 4, 0xB7234A04B8382F67uL, 0x562E7A5CE55E51E2uL), (+1, 0, 0xDED7A5E6E80E7A1DuL, 0xAC2615DF79F5B1D5uL)),
                ((+1, 4, 0x8033F7D43EE1790BuL, 0xB7016FC8581E11FDuL), (-1, 0, 0xB56A7ED356B60A20uL, 0x3526D7E886C12680uL), (-1, 4, 0xB49E7AD221333B9DuL, 0x6D12187020171614uL), (+1, 0, 0xCBAE9417748FDB8DuL, 0x72C98224298EBE53uL)),
                ((-1, 4, 0xD3457E70A1D7E350uL, 0x07397594CB98DB63uL), (+1, 2, 0x946912E7DF433B58uL, 0x0305F71489A7ACBCuL), (-1, 3, 0xE8EE2E805929E7EEuL, 0x0F36B981A0132C15uL), (+1, -1, 0xF2AE5F079F1D37EDuL, 0x9124AD3882235368uL)),
                ((-1, 5, 0x813B5C52CA5F3B5CuL, 0x719971D6F0A2C707uL), (+1, 2, 0x9392B3C0FE2876ABuL, 0x3AC3E1F8DE68B10BuL), (-1, 2, 0xD7984527AA5A3EA9uL, 0x143113851E825DDCuL), (+1, -2, 0xCE90EFBDA019C31FuL, 0x69E810D3DE8D292DuL)),
                ((-1, 4, 0x91DC785B378E16F6uL, 0x5B1B44C33FCE1C4EuL), (+1, 1, 0x914AD0834D14AEA2uL, 0x9F2D2BB53424384CuL), (-1, 1, 0x95C26A348B06BC07uL, 0x354A66CE4FC9ED00uL), (+1, -3, 0x82D2FC1B30715B71uL, 0x63E7A1E1B1FD270FuL)),
                ((-1, 2, 0xD3AB339ACBB8AFBEuL, 0x2BD30823AF9FB6BBuL), (+1, -1, 0xB95CA40141D13CE4uL, 0xCBCC9275B82830A4uL), (-1, -1, 0x9FD3D05BBD2336F0uL, 0x13D5EDD23EB92B73uL), (+1, -6, 0xFBC5ED1B28DC0FB1uL, 0x907999A88E0AC06AuL)),
                ((-1, 0, 0xD8ED4D1C232A3539uL, 0xD881E7588D9A55C4uL), (+1, -3, 0xA5A98AE11966017CuL, 0x6E3BFA19F6D1DFBFuL), (-1, -3, 0x8463269B67A81568uL, 0x36B55488C5ECBC64uL), (+1, -8, 0xB92F43E2B12142B3uL, 0x97EBB3EC4D7369E2uL)),
                ((-1, -2, 0xA2EFDE6B6D3CF977uL, 0x7B451A8EE508E149uL), (+1, -6, 0xD59D366450C94904uL, 0xEFCEF66F3A50E265uL), (-1, -6, 0xAA5CA6365B958094uL, 0x7853D6A7455B9C6BuL), (+1, -11, 0xCF628B61275D0FB3uL, 0x0D2B6789ECCAD063uL)),
                ((-1, -5, 0xB563969F1B8B3C58uL, 0xA2A3AC3B6102A230uL), (+1, -9, 0xC6E37CA5101BB171uL, 0x43F4C77A50F1ED1DuL), (-1, -9, 0xA8D92F4A675C2E29uL, 0x170AF9199635F148uL), (+1, -14, 0xADC034CD0E42E19DuL, 0x5AF1D4C2C264FA98uL)),
                ((-1, -8, 0x9448790CE666AF0AuL, 0xFDF33AA960EA2313uL), (+1, -12, 0x823B36EB4297836FuL, 0xC8600EF554FC9897uL), (-1, -13, 0xFC5460CBEC850959uL, 0x91ACD5FF9977C219uL), (+1, -18, 0xD18966EA3D64D381uL, 0x35265D64DD2CF2D4uL)),
                ((-1, -12, 0xAB9F9ED64A8C0095uL, 0x9606FFE0BBF2CDF4uL), (+1, -17, 0xDCC7E4F3CC070CFAuL, 0xDF0336CB22CB9E7EuL), (-1, -16, 0x87E104DA6D55CF78uL, 0x809E4F63F1651804uL), (+1, -22, 0xA5F05B90BE98D25BuL, 0x7C808E8DF162AED3uL)),
                ((-1, -17, 0xFFDDDD2E0AB6B434uL, 0x7EF54DE7E50DB470uL), (+1, -22, 0xBB10E8880EAF6436uL, 0x66CEC36753780256uL), (-1, -21, 0xBECEAAFBDB688C0FuL, 0x3578651B01D24FA0uL), (+1, -27, 0x84965E06A348C22DuL, 0x6EB6034F2F7CFCB2uL)),
                ((-1, -22, 0xBB0BF5F2E2FFA83BuL, 0xDB00A5D9CDD9F9CBuL), (-1, -41, 0xCA5887B4C2E87408uL, 0xBE70638CFBEB7B94uL), (-1, -26, 0x8492621CBF0BE230uL, 0xA79876167A1B692DuL), (-1, -46, 0xA95E096DF113DBEFuL, 0x447763FE47196E60uL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX7Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 2, 0xE00171151A1DFD27uL, 0xF225C28C6C2F4D76uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xE002296BEC62F68BuL, 0xBB36F6754C30F0D0uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0xAD841069D89D5C21uL, 0x59A35BA86E3D8B62uL), (+1, 0, 0xB404A09C185E6595uL, 0xB74B60C83904F255uL), (-1, 4, 0xA972AA110AE0665FuL, 0xE22391F25E73F1A2uL), (+1, 0, 0xAF5E7DE57A6CCE6CuL, 0x3D6F6B01727967BAuL)),
                ((-1, 2, 0xFF2F4651D887B636uL, 0x09443B94D67BAA2FuL), (+1, -1, 0xF034CA4F4C30EF3FuL, 0xCF1F3BEE1A9CB056uL), (-1, 3, 0xF511A21FF60CAA75uL, 0xC7F102FD2E9325BCuL), (+1, -1, 0xE5F9419747831760uL, 0xF8C8928FFABA49CFuL)),
                ((-1, 1, 0xE9DE2A81EDD1DDCDuL, 0x1F024E0711EDFA9CuL), (+1, -2, 0xC6A5B1FFBA8B5613uL, 0xB5A30BA08D271134uL), (-1, 2, 0xDDE913917B56AB5BuL, 0x42AE8B12FACB18E9uL), (+1, -2, 0xBBE7C1654D968BAEuL, 0xABC9C443CBCC6F42uL)),
                ((-1, 0, 0x946BAA1ED1FD8B11uL, 0x680CF4187BCC8F71uL), (+1, -4, 0xE1BBEF56D26C1D41uL, 0xEE31CCA77A16D36AuL), (-1, 1, 0x8B9D6E23051735BFuL, 0xC41DA1B0F890F0C2uL), (+1, -4, 0xD3BEC5FE944DE5CFuL, 0x8FD9516D6F999841uL)),
                ((-1, -2, 0x89894CCC635D21AFuL, 0xF84335EFBFCE4E21uL), (+1, -6, 0xB960BBF63A6D8C5BuL, 0x1375CBA711832034uL), (-1, -1, 0x808E8C3C941A6A01uL, 0xC31A43591F07D118uL), (+1, -6, 0xACD8D49D9A535E1BuL, 0x1CE152324B431F66uL)),
                ((-1, -5, 0xBF773230AD7F7FBFuL, 0xC33FAC961A40E91EuL), (+1, -9, 0xE1C67C6C93748850uL, 0x24E4B7DD418B2E66uL), (-1, -4, 0xB21A5B3D4B1F0885uL, 0x6789D1276C8FE364uL), (+1, -9, 0xD18D9901E2844B1AuL, 0x33D1BBCC1F91D0E1uL)),
                ((-1, -8, 0xCADA7A5A04403173uL, 0xAFAB702B594A95CCuL), (+1, -12, 0xCDA2CF4CACBF26E2uL, 0x9DD33B6A00521F49uL), (-1, -7, 0xBBF5C152AC7A930EuL, 0x9D3689EF3C04685FuL), (+1, -12, 0xBE222EB7335CF54FuL, 0x5934C4CDE9734050uL)),
                ((-1, -11, 0xA3890B3D2F75D704uL, 0x31BA1487092CB4C0uL), (+1, -15, 0x8AC84551B6E73E32uL, 0x735F86387EF564C2uL), (-1, -10, 0x9703A5F991CAE93EuL, 0xB4B0553FD1F0CD0EuL), (+1, -16, 0xFFC261E152D13CF8uL, 0x27606A5B057BB22FuL)),
                ((-1, -15, 0xC56B5790ED7F4899uL, 0x85CF488FA4784075uL), (+1, -19, 0x8607389BC3024B04uL, 0xA777E1E4C15337ABuL), (-1, -14, 0xB5BC79EEA455C1ACuL, 0x0ED5473F414FA066uL), (+1, -20, 0xF63435D9D2A365C5uL, 0xE3705F1D69974FA0uL)),
                ((-1, -19, 0xAB3284BC0CC60EBBuL, 0x70BE638B3D0DB407uL), (+1, -24, 0xA9E9B72F175F0887uL, 0xA7566EC1244ABCE0uL), (-1, -18, 0x9D2B6D39DC1B4577uL, 0xD430AA12A0ACF80BuL), (+1, -24, 0x9BBBB73450CEC9ACuL, 0xD3928AAA05483C10uL)),
                ((-1, -24, 0xC1FB4C0233B6338EuL, 0xCEFD9767E7750B38uL), (+1, -30, 0xDC121960A3AE4301uL, 0xCE223D8955CD4E76uL), (-1, -23, 0xB1DA0D87D437D513uL, 0xE6FF3D9B443F4B90uL), (+1, -30, 0xCA3F5FF1FC074358uL, 0x4504E2DC9EF8C7A0uL)),
                ((-1, -30, 0xDC13537018772548uL, 0x7A8E015F30D98A3AuL), (+1, -51, 0xADA4C4348D930104uL, 0xEFCB6106EB89CA69uL), (-1, -29, 0xCA413573B6882B3CuL, 0x657BF4D7573C580FuL), (+1, -50, 0x8270D0E5DC592040uL, 0x22146A7CD8725B7EuL)),
            });
            public static ReadOnlyCollection<(ddouble fc, ddouble fd, ddouble gc, ddouble gd)> PadeX8Table
             = new(new (ddouble fc, ddouble fd, ddouble gc, ddouble gd)[] {
                ((-1, 3, 0x80002E289332FC22uL, 0x83422D7F1E5EF775uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 4, 0x8000453B3DB811FFuL, 0x084DC119C7B75A42uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0x839B21529E2399ACuL, 0xD20D6680D495ED39uL), (+1, -1, 0xE7366F3963609E6AuL, 0x415C0A160E5E6139uL), (-1, 3, 0xF0213BFAE83BCDE5uL, 0x60C972EAF606ED70uL), (+1, -1, 0xD0218B4F05BE3276uL, 0x931BEFDFB69BFAC7uL)),
                ((-1, 1, 0xF9500F07A399C8BCuL, 0xBDF4AB1AA97272EFuL), (+1, -2, 0xBF8263F8AD64F117uL, 0x3780C701D5AC22B8uL), (-1, 2, 0xC596FF776DE593BDuL, 0x713917EB9408100AuL), (+1, -2, 0x918E7C0DD941F6E1uL, 0xF3D1CD7ED534BDA0uL)),
                ((-1, 0, 0x8E8255DF472A5EF5uL, 0x0AFA2B10719146F5uL), (+1, -4, 0xBD437B51E0279555uL, 0x7BB910FCCED986DAuL), (-1, 0, 0xB0920068F4C2F7EFuL, 0xECDABAE1D78FF144uL), (+1, -5, 0xCF9591EA4ED7CA70uL, 0x052655828D290BA8uL)),
                ((-1, -3, 0xD8FBCB5E670FC818uL, 0xB0FEBCAD41C397C6uL), (+1, -7, 0xF4B421397A997B12uL, 0x7349A233901E09D3uL), (-1, -3, 0x9AF5131D2544BD4FuL, 0x26B1AA20C7F135B1uL), (+1, -9, 0xCCA953788169F1CDuL, 0x782C3550D0B89F9CuL)),
                ((-1, -6, 0xE438A00ABD36FDD3uL, 0x1E4586D6F2ED1419uL), (+1, -10, 0xD3BD2027C8E85253uL, 0xA00F9D23B00B7A55uL), (+1, -8, 0x81E72F124E89F731uL, 0xA48554B5447696B2uL), (-1, -11, 0xA74840293DF3541EuL, 0x04E23F444EC83450uL)),
                ((-1, -9, 0xA451EC49C29F2E7AuL, 0xB61491748263CC3FuL), (+1, -14, 0xE9CD3F932F1C0F31uL, 0x4A4B43A9BAA3393BuL), (+1, -8, 0x8E5B7A6A03264B80uL, 0x548F00E57BBC239BuL), (-1, -13, 0xC9130AA165E10F89uL, 0x08B285F4DCAE4C0DuL)),
                ((-1, -13, 0x9458B871281E60ADuL, 0x900FE27144F2D1A0uL), (+1, -19, 0xFB92333ACDFB8A9CuL, 0x6045BA05E2FA48DAuL), (+1, -11, 0xCEFB5276682E1DB2uL, 0x4132B5B04B97FECBuL), (-1, -16, 0xD4E32BDAC396EB69uL, 0x704F4503AA4B365DuL)),
                ((-1, -19, 0xCE43C7AAD1A10AF2uL, 0x1BD3AB7C8B41BAE7uL), (-1, -24, 0xB53FA23EE641A3BFuL, 0x9ABAD5271A4F9EE3uL), (+1, -14, 0xAECE05AA07646B37uL, 0x586B4237E84F8A1AuL), (-1, -19, 0x88B95A3E9FA8F183uL, 0x8196EAFBDD6EF50FuL)),
                ((+1, -23, 0xD88498B7852AFAA3uL, 0xCBA3779585739CADuL), (-1, -27, 0xFBC57D7613DD0E20uL, 0x44B74BDBD64A1939uL), (+1, -18, 0xBE1141C27D81A736uL, 0x7BE94F95F774CA33uL), (-1, -24, 0xD55E3C8BD8FA1AE3uL, 0x8CA209ADD1B832E5uL)),
                ((+1, -26, 0xA146483B4838EA77uL, 0xE0B936D2D944A934uL), (-1, -31, 0x8D91DD37948F6C7AuL, 0x0331B58120527BC9uL), (+1, -23, 0xFE5192AE35150E38uL, 0x85138D6805F77204uL), (-1, -29, 0xA3CFFA8716611E41uL, 0xAB766C6EE1636B5CuL)),
                ((+1, -31, 0x8D9310A156184547uL, 0x8F1DC943217C447AuL), (-1, -52, 0x9454F63708B9D519uL, 0x6FB7545DB732884DuL), (+1, -28, 0xA3D0DD5264994C5EuL, 0x2202EECBC54E561CuL), (-1, -51, 0xE0F230B5EC461F1EuL, 0xDCFB7FAEAE7D2919uL)),
            });
        }
    }
}