using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Ei(ddouble x) {
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsInfinity(x)) {
                return x.Sign > 0 ? PositiveInfinity : Zero;
            }

            if (x <= EiPade.PadeApproxMin || x >= EiPade.PadeApproxMax) {
                ddouble g = EiPade.Coef(x) + x;
                ddouble y = Exp(x) / g;

                return y;
            }
            if (x > 0) {
                return EiNearZero.Positive(x);
            }
            if (x < 0) {
                return EiNearZero.Negative(x);
            }

            return NegativeInfinity;
        }

        public static ddouble Li(ddouble x) {
            return Ei(Log(x));
        }

        internal static class EiNearZero {
            public static ddouble Positive(ddouble x, int max_terms = 12) {
                ddouble x2 = x * x;

                ddouble s = EulerGamma + Log(x);
                ddouble u = x * Exp(x / 2);

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2; k++) {
                    ddouble f = TaylorSequence[2 * k + 1] * (1 - x * K4Series.Value(k));
                    ddouble ds = Ldexp(u * f, -2 * k) * FSeries.Value(k);
                    ddouble s_next = s + ds;

                    if (s == s_next || IsInfinity(s_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    u *= x2;
                    s = s_next;
                }

                return s;
            }

            public static ddouble Negative(ddouble x, int max_terms = 12) {
                if (!(x <= 0)) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }

                ddouble x2 = x * x;

                ddouble s = EulerGamma + Log(-x);
                ddouble u = x;

                for (int k = 0, conv_times = 0; k < max_terms && conv_times < 2; k++) {
                    (ddouble r1, ddouble r2) = NRcpSeries.Value(k);

                    ddouble f = TaylorSequence[2 * k + 1] * (r1 + x * r2);
                    ddouble ds = u * f;
                    ddouble s_next = s + ds;

                    if (s == s_next || IsInfinity(s_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    u *= x2;
                    s = s_next;
                }

                return s;
            }

            internal class FSeries {
                private static ddouble v = 0;
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        v += Rcp(checked(2 * k + 1));
                        table.Add(v);
                    }

                    return table[n];
                }
            }

            internal class K4Series {
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        table.Add(Rcp(checked(4 * k + 4)));
                    }

                    return table[n];
                }
            }

            internal class NRcpSeries {
                private static readonly List<(ddouble, ddouble)> table = new();

                public static (ddouble r1, ddouble r2) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble r1 = Rcp(checked(2 * k + 1));
                        ddouble r2 = Rcp(checked((2 * k + 2) * (2 * k + 2)));
                        table.Add((r1, r2));
                    }

                    return table[n];
                }
            }
        }

        internal static class EiPade {
            public static readonly ddouble PadeApproxMin = -1 / 2d, PadeApproxMax = 1 / 2d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> PadeTables;
            public static readonly ReadOnlyCollection<double> PadeCenterTable, PadeThresholdTable;

            static EiPade() {
                PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                    PadeXM2Table,
                    PadeXM1p75Table,
                    PadeXM1p5Table,
                    PadeXM1p25Table,
                    PadeXM1Table,
                    PadeXM0p75Table,
                    PadeXM0p5Table,
                    PadeXM0p375Table,
                    PadeXM0p25Table,
                    PadeXM0p1875Table,
                    PadeXM0p125Table,
                    PadeXM0p0625Table,
                    PadeXM0p03125Table,
                    PadeXM0p015625Table,
                    PadeXZeroTable,
                    PadeXP0p0078125Table,
                    PadeXP0p01171875Table,
                    PadeXP0p015625Table,
                    PadeXP0p01953125Table,
                    PadeXP0p0234375Table,
                    PadeXP0p02734375Table,
                    PadeXP0p03125Table,
                    PadeXP0p03515625Table,
                    PadeXP0p0390625Table,
                    PadeXP0p04296875Table,
                    PadeXP0p046875Table,
                    PadeXP0p0546875Table,
                    PadeXP0p0625Table,
                    PadeXP0p0703125Table,
                    PadeXP0p078125Table,
                    PadeXP0p0859375Table,
                    PadeXP0p09375Table,
                    PadeXP0p109375Table,
                    PadeXP0p125Table,
                    PadeXP0p140625Table,
                    PadeXP0p15625Table,
                    PadeXP0p171875Table,
                    PadeXP0p1875Table,
                    PadeXP0p203125Table,
                    PadeXP0p21875Table,
                    PadeXP0p234375Table,
                    PadeXP0p25Table,
                    PadeXP0p28125Table,
                    PadeXP0p3125Table,
                    PadeXP0p375Table,
                    PadeXP0p4375Table,
                    PadeXP0p5Table,
                    PadeXP0p5625Table,
                    PadeXP0p625Table,
                    PadeXP0p6875Table,
                    PadeXP0p75Table,
                    PadeXP0p875Table,
                    PadeXP1Table,
                    PadeXP1p125Table,
                    PadeXP1p25Table,
                    PadeXP1p5Table,
                    PadeXP1p75Table,
                    PadeXP2Table,
                });

                PadeCenterTable = new ReadOnlyCollection<double>(new double[]{
                    -2d,
                    -1.75d,
                    -1.5d,
                    -1.25d,
                    -1d,
                    -0.75d,
                    -0.5d,
                    -0.375d,
                    -0.25d,
                    -0.1875d,
                    -0.125d,
                    -0.0625d,
                    -0.03125d,
                    -0.015625d,
                    0d,
                    0.0078125d,
                    0.01171875d,
                    0.015625d,
                    0.01953125d,
                    0.0234375d,
                    0.02734375d,
                    0.03125d,
                    0.03515625d,
                    0.0390625d,
                    0.04296875d,
                    0.046875d,
                    0.0546875d,
                    0.0625d,
                    0.0703125d,
                    0.078125d,
                    0.0859375d,
                    0.09375d,
                    0.109375d,
                    0.125d,
                    0.140625d,
                    0.15625d,
                    0.171875d,
                    0.1875d,
                    0.203125d,
                    0.21875d,
                    0.234375d,
                    0.25d,
                    0.28125d,
                    0.3125d,
                    0.375d,
                    0.4375d,
                    0.5d,
                    0.5625d,
                    0.625d,
                    0.6875d,
                    0.75d,
                    0.875d,
                    1d,
                    1.125d,
                    1.25d,
                    1.5d,
                    1.75d,
                    2d,
                });

                double[] threshold = new double[PadeCenterTable.Count];

                threshold[0] = PadeCenterTable[0];
                for (int i = 1; i < threshold.Length; i++) {
                    threshold[i] = (PadeCenterTable[i] + PadeCenterTable[i - 1]) / 2;
                }

                PadeThresholdTable = Array.AsReadOnly(threshold);
            }

            public static ddouble Coef(ddouble x) {
                ddouble v = 1d / x;

                int table_index = SegmentIndex(v);
                ddouble w = v - PadeCenterTable[table_index];
                ReadOnlyCollection<(ddouble c, ddouble d)> table = PadeTables[table_index];

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            private static int SegmentIndex(ddouble x) {
                if (PadeThresholdTable[0] >= x) {
                    return 0;
                }
                if (PadeThresholdTable[^1] <= x) {
                    return PadeThresholdTable.Count - 1;
                }

                int index = 0;

                for (int h = Math.Max(1, PadeThresholdTable.Count / 2); h >= 1; h /= 2) {
                    for (int i = index; i < PadeThresholdTable.Count - h; i += h) {
                        if (PadeThresholdTable[i + h] > x) {
                            index = i;
                            break;
                        }
                    }
                }

                return index;
            }
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM2Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0x9562202C465FEF18uL, 0x5C51B90368E456D7uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xAA3C06F9A4269D37uL, 0xA9AC3BC5FAAE95C1uL), (-1, 1, 0x991EB6C62307C188uL, 0xFE6906E76711CC49uL)),
                    ((-1, 0, 0xA4F76B74305CFAA8uL, 0xD4CA32CEFC09F38DuL), (+1, 1, 0x9CB99C63B351149EuL, 0x271F4241715CA98CuL)),
                    ((+1, -1, 0xB13893654B0CB73BuL, 0xEE28828B536633CBuL), (-1, 0, 0xB32E0481718DD32FuL, 0x62E3913E458DC6A6uL)),
                    ((-1, -3, 0xE6AA7F46391CD99CuL, 0x54670BC44A69E125uL), (+1, -2, 0xFA89053AEAFE0CD4uL, 0xC70B8A468A25D939uL)),
                    ((+1, -5, 0xBAABFFA3F079A54EuL, 0xF8C5A4E8A0176053uL), (-1, -4, 0xDC72B40AAD1E2272uL, 0xB95EF8BCA5CB34ACuL)),
                    ((-1, -8, 0xB9814122FE45282CuL, 0x17ED09420028D89FuL), (+1, -7, 0xF20A1606A0B61536uL, 0x7A89865FE6503C9EuL)),
                    ((+1, -12, 0xD73A0A17DA6C183DuL, 0x89EDCB1E63A91F54uL), (-1, -10, 0x9EA94AD6B2ED9572uL, 0x95EE55F07165764BuL)),
                    ((-1, -16, 0x832A288B24F50323uL, 0xCB653C45D9FAD76BuL), (+1, -15, 0xE224EECA277507B1uL, 0x53FEAA8415284E4EuL)),
                    ((+1, -22, 0x8714DE00E8963F08uL, 0xE5C5DCBD7B2FD60DuL), (-1, -20, 0x90A565C8807D5F99uL, 0xFF243EB458A56F9BuL)),
                    ((-1, -30, 0x85A67D4514AB789FuL, 0x4D5E098E3122D117uL), (+1, -28, 0xCDDC360CE3753FD5uL, 0x75EC9826B76D04D7uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM1p75Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0x99EE40AF125A7AD3uL, 0x4306DA365420C808uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xC705DBFFF91C0C1FuL, 0x4C1E5BAE9CE6ACF3uL), (-1, 1, 0xADA10A8F3D125427uL, 0x542D731DB72DC309uL)),
                    ((-1, 0, 0xDACA52CA7E5EEAB0uL, 0x89D98055D8FE8190uL), (+1, 1, 0xC982D923AA9915A9uL, 0x9DAE5E6C75F8506FuL)),
                    ((+1, 0, 0x854CAAB2E7F983A6uL, 0xFDFEE50943354575uL), (-1, 1, 0x829BDBE3EFFE363BuL, 0x557554B50B8CD148uL)),
                    ((-1, -2, 0xC4C16FDF3115EB64uL, 0x24DD7FADC5F5FA67uL), (+1, -1, 0xCF0F06D3492A4708uL, 0x3E65091046EAC826uL)),
                    ((+1, -4, 0xB487E234E71A9242uL, 0x5405CF3E970E0671uL), (-1, -3, 0xCE9212178B6BD969uL, 0x15A13F1362398D1BuL)),
                    ((-1, -7, 0xCB573A97B5CB8C78uL, 0x8E0C3BBFFC6353E4uL), (+1, -5, 0x8093EA53B4207029uL, 0x0BEF8EB6C9BFC873uL)),
                    ((+1, -10, 0x85A4F9606CC2831AuL, 0xA71685C9046DC0BCuL), (-1, -9, 0xBF251BD69A8ED4AEuL, 0x0082A0A4904ED7DFuL)),
                    ((-1, -15, 0xB870747925463C87uL, 0x64FEE292A5884993uL), (+1, -13, 0x9A7E5DEEB2953D4DuL, 0x75D39F5728313841uL)),
                    ((+1, -21, 0xD6DCA321A4B867EFuL, 0x45FD16CA32D42A5CuL), (-1, -19, 0xE03DAC276FDB0CDBuL, 0xF12B9F5CBEEA7592uL)),
                    ((-1, -29, 0xF001A5D99EE57BCAuL, 0x6246A0FA84644290uL), (+1, -26, 0xB547F43EDDB3770FuL, 0x649A05F10C1F3611uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM1p5Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0x9F3B8317539F8560uL, 0x183791F0AE077BCFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0x839EB922EE48BF65uL, 0x195FCF4A2C061235uL), (-1, 1, 0xDCDC1D2435874F92uL, 0x53D9519A20051259uL)),
                    ((-1, 1, 0xBBEECF23CCE86B77uL, 0x04F50FCFA78CC268uL), (+1, 2, 0xA56DBA9B558C4B76uL, 0x1A34873187F367F4uL)),
                    ((+1, 1, 0x97B995C3328BDF92uL, 0x4BB1A9FF56AFF151uL), (-1, 2, 0x8CFFFB0B804A91A3uL, 0x1A1CAD637787A68CuL)),
                    ((-1, 0, 0x9853463809C0D3E3uL, 0xF3062D5C6EDF725BuL), (+1, 1, 0x96976439123FDC78uL, 0xE626CBE8ECAEDB26uL)),
                    ((+1, -2, 0xC4FE847FA745734BuL, 0xBCD16D5EB03B0BABuL), (-1, -1, 0xD12974FF4612DFD4uL, 0x41736ACCF06DBE45uL)),
                    ((-1, -4, 0xA4665D83A8AB81EFuL, 0xF4ED7B4B9974DCD9uL), (+1, -3, 0xBDC6637D890FCF7DuL, 0x61D8D6F7061C4226uL)),
                    ((+1, -7, 0xAC914DAD48DAD4CEuL, 0x0A8A7C2ADDA603D3uL), (-1, -6, 0xDC20CDD1AE66A210uL, 0x884E0C5749750ECAuL)),
                    ((-1, -11, 0xD6DCBDED9975881CuL, 0x235D9B2B0C34944CuL), (+1, -9, 0x9AF04420EF0FA002uL, 0x11CF435F1085745FuL)),
                    ((+1, -15, 0x8DF500EE9861E59EuL, 0x4716946671ADC622uL), (-1, -14, 0xEFB049E67283DC09uL, 0x4606DC1498C912E5uL)),
                    ((-1, -21, 0x9F68669420B8A5B2uL, 0xDDF1BE98FE45FCFDuL), (+1, -19, 0xA787F3BCC0F7D6B2uL, 0x4C2242160E78C8EFuL)),
                    ((+1, -29, 0xAC4164C0911BFA0FuL, 0x55B33B7F22809FA5uL), (-1, -26, 0x82CB923D32B19B65uL, 0x31103932F53B722CuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM1p25Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xA58BC9AE4F4E9684uL, 0xA46F1187E2F87BF4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0xB1FBA50987A88AEEuL, 0xB3B20A8D1C0BE399uL), (-1, 2, 0x8EFB0945ED3428BDuL, 0xFF402FD350C70573uL)),
                    ((-1, 2, 0xA76A694227D24ABFuL, 0xB301C888A53F41FEuL), (+1, 3, 0x8C596B35E72C20A9uL, 0xF569CBD771BCA348uL)),
                    ((+1, 2, 0xB4F7C306C57571DCuL, 0xE5C68DFFE0485F00uL), (-1, 3, 0x9F24F53B24E2DE4CuL, 0xF18015E75A4B8A26uL)),
                    ((-1, 1, 0xF84AD234CDCF376CuL, 0x15A280CFA8BDDCC2uL), (+1, 2, 0xE67C5FB4B198C464uL, 0xBF29518BCBC878F4uL)),
                    ((+1, 0, 0xE15D0E6405FF22A7uL, 0x81CBEF2F1BA71725uL), (-1, 1, 0xDE85BF6F6E247C43uL, 0x010CDF895C3B4EF6uL)),
                    ((-1, -1, 0x88DA0631BD6C1326uL, 0x288E88EAC42E66B0uL), (+1, 0, 0x911BB27A707EE0EFuL, 0x86B35351F0F8062BuL)),
                    ((+1, -4, 0xDBF2A895CF88F62AuL, 0xF2E60391AB23E83CuL), (-1, -3, 0xFD87C6765EC3DFBEuL, 0x20DAD2C75011F271uL)),
                    ((-1, -7, 0xE2272199A7DCA97AuL, 0x859263CDAB9688F8uL), (+1, -5, 0x90060FF7B18AB4D2uL, 0xFCD6D773CD568AA1uL)),
                    ((+1, -10, 0x8B8BD176ED5230C7uL, 0x29279A881E171965uL), (-1, -9, 0xC8F291DDABA1DC52uL, 0x3F5CB4BD2B6CE2F8uL)),
                    ((-1, -15, 0xB8372C2E8D2BBFB9uL, 0xBB8388C908EB6139uL), (+1, -13, 0x9B49AB8213E61E0AuL, 0x9B8333A4C5357E27uL)),
                    ((+1, -21, 0xCFAB1521530E3B28uL, 0xF6659B125D004982uL), (-1, -19, 0xD9F3E6662BC8DE65uL, 0x2AD31AAFC7ED7792uL)),
                    ((-1, -29, 0xE1C888B4D28223F8uL, 0x2B4B6112449A985DuL), (+1, -26, 0xAB42C07C75BB5D28uL, 0x673D713C232DE076uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM1Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xAD47AE8D81B0735FuL, 0xD5BA78D2D4A4F654uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0xF99FAC4B389A4DBCuL, 0x0B34A5F8F8B31FFDuL), (-1, 2, 0xBEC735D6DE6B0897uL, 0xD598DDB8D6996026uL)),
                    ((-1, 3, 0x9F027FED53521E87uL, 0xB773258F31A24D72uL), (+1, 3, 0xFC69F1BE7C4D12CCuL, 0xA26BC382FFF44B7AuL)),
                    ((+1, 3, 0xEBE2B935822BAF89uL, 0xF4910932E0A50B5EuL), (-1, 4, 0xC34BE05895BD2A8DuL, 0x8047CBBB9257E360uL)),
                    ((-1, 3, 0xE1C0B28D625D8E5EuL, 0x0AE5095E4898CF33uL), (+1, 4, 0xC3FD36490D30B2B7uL, 0xE69FD44CFB7FDEB4uL)),
                    ((+1, 3, 0x91F173B0B8BE9107uL, 0x5B9735B764FE8B69uL), (-1, 4, 0x85B2722A1C43E9D6uL, 0xB07FDA380612EF39uL)),
                    ((-1, 2, 0x81B99B2038E7E736uL, 0x6F218CED84661C23uL), (+1, 2, 0xFCBD5A414C8DA036uL, 0x7F97343126F156E3uL)),
                    ((+1, 0, 0x9E48DF83B4D88C7CuL, 0x8AE94CDE514B0279uL), (-1, 1, 0xA58B8B7C2217C6FCuL, 0xEFE8F8C5DEEF0476uL)),
                    ((-1, -2, 0x8212E71D986FA5A8uL, 0x68321D7672DD8636uL), (+1, -1, 0x93DF8DD385D4B087uL, 0x2514C9A43E6CE993uL)),
                    ((+1, -5, 0x8A7CB8D6DADAD3B7uL, 0xF691959EB61B8421uL), (-1, -4, 0xADF93AD23A54990EuL, 0xFF851C3C422DFE72uL)),
                    ((-1, -9, 0xB2856BBC2F46D0C6uL, 0x60CA769C5DABD9FDuL), (+1, -8, 0xFD9FCE0ED09B3C6CuL, 0x33E17A4E7B391749uL)),
                    ((+1, -14, 0xF79BF549806A1BBCuL, 0x016792D9B4E692F7uL), (-1, -12, 0xCE102EFB52055E57uL, 0x74CABE660ABE6ABDuL)),
                    ((-1, -19, 0x932262074E721E1FuL, 0x55D7EC643CA0B8E3uL), (+1, -17, 0x98A6C943B9E4D394uL, 0x159A21F48144D245uL)),
                    ((+1, -27, 0xA8CCF1E1EBFF97FFuL, 0xD9F2885B7398AB55uL), (-1, -25, 0xFDD9B52E02D0FAFAuL, 0x050CF6ECDA27E08FuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p75Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xB723953D6DB0B683uL, 0xD6F490DC9DA39747uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xBA128D9B2145D40FuL, 0x94AD632205EADAC4uL), (-1, 3, 0x85FED04D1949CA93uL, 0x2A6B766261853804uL)),
                    ((-1, 4, 0xA8B77C4EEE2B1BA5uL, 0xAFD13B188E6FE88CuL), (+1, 4, 0xFB2E81B081DEF789uL, 0xBE7D96CFBC03D71CuL)),
                    ((+1, 5, 0xB418490CD1C33DF2uL, 0xA4DC6F9101837609uL), (-1, 6, 0x8B1A77A3F1BFCE3BuL, 0x090011427D99B7FCuL)),
                    ((-1, 5, 0xFB63D69716ACFC7FuL, 0x9F683023B8AE53DBuL), (+1, 6, 0xCA5F85545DB6C0FCuL, 0x77998BD2F1AEBBF9uL)),
                    ((+1, 5, 0xF10536D835DF7DCBuL, 0x2DFBF9EEFB79180FuL), (-1, 6, 0xCB4B0FE3BF5C8056uL, 0x4A733EF09066DADBuL)),
                    ((-1, 5, 0xA24265AEDEDC9D3AuL, 0x496819D48070E920uL), (+1, 6, 0x9050797DC7015417uL, 0xC71ED2AC30C036F8uL)),
                    ((+1, 4, 0x9A22EBB65628C763uL, 0x641289CBDAC8CD75uL), (-1, 5, 0x91B053595DF297BFuL, 0x08DF441C3228A626uL)),
                    ((-1, 2, 0xCCAA6160F176DE0FuL, 0x3E610D690B00B159uL), (+1, 3, 0xCF9A1BC391484A73uL, 0x776C98E47F71AB75uL)),
                    ((+1, 0, 0xB96B0CA4B72A29CBuL, 0x1AC417BF11B9938EuL), (-1, 1, 0xCC64D71277B08970uL, 0xA08E11F9BDDF1EA3uL)),
                    ((-1, -3, 0xDBAB15297A352AE0uL, 0x7CEA47D8AC4D4800uL), (+1, -1, 0x85CE905AAD2CAC7AuL, 0x993BDB3264772854uL)),
                    ((+1, -6, 0x9E9329668F0B4B71uL, 0xF94BFA2857652DD2uL), (-1, -5, 0xDA9CB6614C06BF7CuL, 0x3E3E05A80025EF6FuL)),
                    ((-1, -11, 0xF75DA0CFF4BC3914uL, 0x98F8632245220AD5uL), (+1, -9, 0xC80CC94651BDA42BuL, 0xA473E82B118F9588uL)),
                    ((+1, -16, 0xA5A57EE77FC24969uL, 0xD79A2F7742549A97uL), (-1, -14, 0xA77E3DB114D94F5EuL, 0xB2E82012CB391334uL)),
                    ((-1, -24, 0xD606D0131D907531uL, 0x9D443C91228D14CBuL), (+1, -21, 0x9DD36264EB8579F9uL, 0xF084F9B1D2CE7CD7uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p5Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xC47F0FAA8C060689uL, 0x31FEF0B37A3E534CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xF343EE3287AAB9C0uL, 0xCEB9C11F29F93EB5uL), (-1, 3, 0xA3A5B1381E8FA1EBuL, 0xFC8E67A0B9F10094uL)),
                    ((-1, 5, 0x83A00BE87807AB07uL, 0xC21959B6F814C4FCuL), (+1, 5, 0xB7B422CBD7AF4612uL, 0x61586E6051D863C5uL)),
                    ((+1, 6, 0xA3862C9FF360BD05uL, 0x3F032AF74CB6687BuL), (-1, 6, 0xEE06FAFD6F675AD3uL, 0x81417702B7589436uL)),
                    ((-1, 7, 0x80C908AC781C489CuL, 0x95F332C4D93DC64BuL), (+1, 7, 0xC4CEC4CF72738F85uL, 0xCD49D461ADE81064uL)),
                    ((+1, 7, 0x86063A7906549AB7uL, 0x17C81320FB809873uL), (-1, 7, 0xD8CB8701906EBAB7uL, 0xA34DD6EF1861E056uL)),
                    ((-1, 6, 0xBA58C217F2CF684FuL, 0x6DA1B5C23CD2B26AuL), (+1, 7, 0xA133124C46317936uL, 0xCFF4436C7AB9C55EuL)),
                    ((+1, 5, 0xAB1EC615DCA83BD4uL, 0xD723EAEF6AA41FA5uL), (-1, 6, 0xA07CBE005C3040F6uL, 0xBC858693B699EBD6uL)),
                    ((-1, 3, 0xC895917C6726671DuL, 0x3F8F01D15A65F7CAuL), (+1, 4, 0xCFBBB93AE03A80BEuL, 0x2E1853FFE10DB467uL)),
                    ((+1, 1, 0x8CA5C66A84ED98BBuL, 0xE3612ADC8191C05DuL), (-1, 2, 0xA51CB8A2665EB1F3uL, 0x072279E66061D2A2uL)),
                    ((-1, -3, 0xD1FF198C518512F6uL, 0xCE430BE536E6E844uL), (+1, -1, 0x917CA46B79B1A25CuL, 0xBB3C030405719972uL)),
                    ((+1, -7, 0x84D53E26D823D047uL, 0x68076920CDEC5551uL), (-1, -6, 0xE966DBB345C243CFuL, 0x5A80D67EBE1EDE94uL)),
                    ((-1, -14, 0x9F70BC881B8E4072uL, 0xD20D5485833C5B86uL), (+1, -12, 0xD34FFA7DE0976C45uL, 0x360826CC94A56756uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xCD5A3C72EF00E445uL, 0xB14B6E42D4529116uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xC1B32920B2BD9682uL, 0x333CE8473AAE01D0uL), (-1, 3, 0xF7A31243EE794C64uL, 0x2E4B279A31DF65EDuL)),
                    ((-1, 6, 0xA2F11FD8F4C780AEuL, 0xFE100707BC3B1778uL), (+1, 6, 0xD650EF6869DCC293uL, 0xB5E8B4CB6FBF8284uL)),
                    ((+1, 8, 0xA13EBE86104AA150uL, 0x49BA5553095E8F52uL), (-1, 8, 0xDB04F5BB0F511D87uL, 0xE2243C91C14EC202uL)),
                    ((-1, 9, 0xD08000BE55605D09uL, 0x55DDAC9B56BCC3B2uL), (+1, 10, 0x92E4215B385F6A9DuL, 0x5BF4F4E2EB258131uL)),
                    ((+1, 10, 0xB9053A63B309E2C5uL, 0x0B58C8AA94C137DCuL), (-1, 11, 0x87F3439634BD8B65uL, 0xCEBCAB15F4A531B3uL)),
                    ((-1, 10, 0xE65CC46A147C2CCBuL, 0x1D63EB18986AB6BAuL), (+1, 11, 0xB1B56354B61F5028uL, 0x789F6AC5E47931F3uL)),
                    ((+1, 10, 0xCA24BFD6FC0A7D2BuL, 0x912DF0AC330B6146uL), (-1, 11, 0xA5103E29CCCA08FDuL, 0x0B3267E8A43DAF48uL)),
                    ((-1, 9, 0xF7A4AF6597A130F7uL, 0xDCC93B59EF2926D9uL), (+1, 10, 0xD8489B95B023A95FuL, 0x196ABD39590179C6uL)),
                    ((+1, 8, 0xCEB1AE1878ACB167uL, 0x258C2D18D2FCD26DuL), (-1, 9, 0xC3B58E8AD0E5F86AuL, 0xC4C442FC55F69A88uL)),
                    ((-1, 6, 0xE12F085D257CF6E4uL, 0x9AEAC19DE46F2450uL), (+1, 7, 0xEB6D62D451DAB1C4uL, 0x75FCFC4FF87AA3C6uL)),
                    ((+1, 4, 0x951D9B3F76798298uL, 0xC4509815E9A2103BuL), (-1, 5, 0xB0AFC71FED2C0415uL, 0x4BCB4376044E7344uL)),
                    ((-1, 0, 0xD49B9EBD13A829FCuL, 0xD23511FC1EE794AFuL), (+1, 2, 0x949D53770BDC6145uL, 0x6DC2582425910258uL)),
                    ((+1, -4, 0x815AB74EDDF457A2uL, 0x57CEE987CCB44BDEuL), (-1, -3, 0xE523E656BB4017D7uL, 0x853E5C450C9FBE64uL)),
                    ((-1, -11, 0x95F55B1AE13E8551uL, 0xA8D3D89A82B46C12uL), (+1, -9, 0xC7FA0962E068B5D3uL, 0xD1A8CEE670CA4FF5uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p25Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xD8A3032B4F16EF07uL, 0xA51A66F76710ED7AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xF22491FDFACEA01BuL, 0x5D58757D14875D47uL), (-1, 4, 0x92E45FFE881DFCF7uL, 0x6CFEB144F4EF5DCAuL)),
                    ((-1, 6, 0xEC4D83F623954F43uL, 0x484DBD86A31EE649uL), (+1, 7, 0x93CFC2ADCF1BBC74uL, 0xE6E1DF4AB00E265EuL)),
                    ((+1, 9, 0x84356F4A15592886uL, 0x4AA410334659B6C0uL), (-1, 9, 0xAB72108E7F58B342uL, 0x147C45839F6B94E9uL)),
                    ((-1, 10, 0xBB5283DE0C2FD728uL, 0x12167E3300CF2225uL), (+1, 10, 0xFD72D43F4F5E6F3DuL, 0x9195AE9574024BA8uL)),
                    ((+1, 11, 0xAF1C3A2C1D7EF1C8uL, 0xBB5208AF6C5738F3uL), (-1, 11, 0xF93F8D9B08C5D23BuL, 0x7A5A07EC578F5590uL)),
                    ((-1, 11, 0xDA5C7946E2013AA7uL, 0x6785226ABF63EEAAuL), (+1, 12, 0xA53D7CC5845C53B4uL, 0xBB7B7A18E864043DuL)),
                    ((+1, 11, 0xB38371456AEF6ED0uL, 0x212A24DDC4EDF583uL), (-1, 12, 0x927F3B3F41CC5F5EuL, 0xB4F682491182FBFCuL)),
                    ((-1, 10, 0xBBF76606D1C2C0B5uL, 0x6207AA5F168A841CuL), (+1, 11, 0xA8AD6A7EFA9C2846uL, 0xFAAF5A07244D12E6uL)),
                    ((+1, 8, 0xEACAC7C5EE519049uL, 0x9E203D387849E2ABuL), (-1, 9, 0xEE51C62991FEBE57uL, 0xBFBB161268C3E39DuL)),
                    ((-1, 6, 0x9B791D088894928AuL, 0x56EC069CE7B15D63uL), (+1, 7, 0xBA940AB9878C37F4uL, 0x2B62A9F97AA9EEDBuL)),
                    ((+1, 2, 0xAD3A10EA58B5DD70uL, 0xE707BC46E36E3E66uL), (-1, 4, 0x8516F54C1B6CC71DuL, 0x56AC297FBAA6440EuL)),
                    ((-1, -4, 0xB42C4F4173E3B66CuL, 0xB957FD05ACE584FEuL), (+1, -2, 0xD756176B5CD24353uL, 0xB1544149FBC6A08BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p1875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xDFA61491324F3C94uL, 0x9E9D8A2333D26609uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 4, 0xACE26681BD40A611uL, 0x0D70769EEA8596CDuL), (-1, 4, 0xCA403911FE549320uL, 0x92307EA8BC2545A3uL)),
                    ((-1, 7, 0xEBD54AC05E2A91FBuL, 0x51DC3785608F946BuL), (+1, 8, 0x8D79F7480057D556uL, 0xD363615ED7768540uL)),
                    ((+1, 10, 0xBAD8B475F6C283E8uL, 0x62DBDB24D0184519uL), (-1, 10, 0xE6E4420B0635424DuL, 0x8013567A4B2B96D0uL)),
                    ((-1, 12, 0xBE84C88EF33D7A68uL, 0xAAAFF79E061A5F89uL), (+1, 12, 0xF3BEE0B0B5EB6605uL, 0x14560A4FD6E19B1BuL)),
                    ((+1, 14, 0x82DCBE41787A9F70uL, 0x27258D5A5C72D991uL), (-1, 14, 0xAE773A415BC56663uL, 0x34AF5D8D331E018DuL)),
                    ((-1, 14, 0xF66EEA231D8EA5ACuL, 0xAE23AD6A780ED91AuL), (+1, 15, 0xAC995260BC1B8E6AuL, 0xD6EDAA717A774601uL)),
                    ((+1, 15, 0x9EB741804666FCFFuL, 0x845BEDCD1FCAB946uL), (-1, 15, 0xEC1967D1E070211FuL, 0x64E0D5F8A902FA6EuL)),
                    ((-1, 15, 0x8924C694BA6BAF26uL, 0x35A5AC5627D6AEB8uL), (+1, 15, 0xDBBF078706056A67uL, 0x0B81AAC9A5385217uL)),
                    ((+1, 14, 0x98C7CB71683574A3uL, 0xE552C61CAACDD0E6uL), (-1, 15, 0x8673AD1DC30A8E2AuL, 0x0B400815C0901AE3uL)),
                    ((-1, 12, 0xCCBE5CA8D098CBB2uL, 0xCACBD1AA870436A5uL), (+1, 13, 0xCBA4628C8E482293uL, 0x164C5E15CA0296EDuL)),
                    ((+1, 10, 0x923BE79C116D2F9DuL, 0xF332E5896CB072D0uL), (-1, 11, 0xABEC8385A4B7393EuL, 0x3FE177A41F27B5FEuL)),
                    ((-1, 6, 0xB033E9885F33434EuL, 0x05D0A857B7D9F612uL), (+1, 8, 0x84C0E1B254F60E26uL, 0x833256A43CBED3F7uL)),
                    ((+1, 0, 0xC604964998A2990DuL, 0xC8C8BDEF882F93BEuL), (-1, 2, 0xE9132760195ACE81uL, 0x9D7EEB4EF6761657uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xE80585FAB75B2BC7uL, 0x871B76AAB58E6682uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 5, 0x916C9F0193812B72uL, 0x281A074453B76677uL), (-1, 5, 0xA2FFDDD963E2FBEDuL, 0x5A998DB2DA93E082uL)),
                    ((-1, 9, 0xA38F65E4E5D95890uL, 0xA0BC76787B155399uL), (+1, 9, 0xBAADF5BE54284437uL, 0x8693813910072014uL)),
                    ((+1, 12, 0xDA09861D8B377EAEuL, 0xDF1E20B12526A8CEuL), (-1, 12, 0xFE21383029542CB8uL, 0x2E734A82AB0D9AFDuL)),
                    ((-1, 15, 0xBFB0518496113C80uL, 0x6F49ADF267125A28uL), (+1, 15, 0xE4F04E74F20F9D17uL, 0x042A9834A3DEADE2uL)),
                    ((+1, 17, 0xEA0FA1A59EEF33D2uL, 0x9A795B7657FE6F61uL), (-1, 18, 0x8FD38CABD5C94D8FuL, 0x36BCD94A818F3421uL)),
                    ((-1, 19, 0xCB8D7DEDD6696551uL, 0x20050AE5DC878E9CuL), (+1, 20, 0x815FB70F6514D71CuL, 0x5EA07F9DDA9E15F3uL)),
                    ((+1, 20, 0xFE77E9E9C7CABA14uL, 0x38AE1E831CC77D4CuL), (-1, 21, 0xA85F396C83C799E9uL, 0xB69A4A2C503AB088uL)),
                    ((-1, 21, 0xE3EB2003B31108B4uL, 0xCD21378D25BA4911uL), (+1, 22, 0x9E49DE1E86232CCEuL, 0xDB13D239180FFD88uL)),
                    ((+1, 22, 0x901A92AA680A5787uL, 0xCF457FFFA860D3D3uL), (-1, 22, 0xD45867C57D1FB23AuL, 0x963FCEF2DD36FECEuL)),
                    ((-1, 21, 0xFA22464019F1F17BuL, 0x7A7D9F9615E4CE61uL), (+1, 22, 0xC650FDBDA0D31194uL, 0x63399BA419D8D08AuL)),
                    ((+1, 21, 0x8E4B41D66CD37705uL, 0x479493F40BBC3229uL), (-1, 21, 0xF7A1488BA3D5BA11uL, 0x37275EE13CE0D15CuL)),
                    ((-1, 19, 0xC516C0283B5A93DBuL, 0x22B80FA7FFE79689uL), (+1, 20, 0xC1B086C4F01A45BCuL, 0x12876ECCCF53F614uL)),
                    ((+1, 17, 0x92AB4116D9084E3DuL, 0xAC02BF64BF45C8ACuL), (-1, 18, 0xAA5299CD0FB1BB30uL, 0x5D7FB113ADE10B7CuL)),
                    ((-1, 13, 0xB8FE32B04D6C6B72uL, 0xF01E46EC4D16666CuL), (+1, 15, 0x89BB21E0553FFEDFuL, 0x3B02C899EBCB2A0BuL)),
                    ((+1, 7, 0xD9CFBD90B083EC6EuL, 0xA65EEAE7E2CCCEB3uL), (-1, 9, 0xFDFE59F5622509EBuL, 0xBB9A298169B6CC4EuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p0625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xF2631DFE5D61659AuL, 0x77D2AEBB5462525AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 5, 0xCF375897B2FDC77AuL, 0xBBE82E74FA48F723uL), (-1, 5, 0xDDF0F269A701658BuL, 0x6C3C3B877DA556B0uL)),
                    ((-1, 10, 0x9BB073E1EEDF407CuL, 0x43A522579881ACACuL), (+1, 10, 0xA99499EB21D893A3uL, 0x5BDB2104DF03CDAEuL)),
                    ((+1, 14, 0x8756064308974C1AuL, 0xF79C197287CD2959uL), (-1, 14, 0x9670C66D5EED133BuL, 0x436FD214E35D126AuL)),
                    ((-1, 17, 0x96C9B8F1CC6E9CC4uL, 0x4D57025838AD4D81uL), (+1, 17, 0xABD48A430C2101B2uL, 0xF148DB0FF6BD818CuL)),
                    ((+1, 19, 0xE167C96F373C2728uL, 0xB2C591E859A47432uL), (-1, 20, 0x846C102C237C3EDEuL, 0x62FA323B0E86662EuL)),
                    ((-1, 21, 0xE5F64635EF55FCFAuL, 0xB33C6B9184B77082uL), (+1, 22, 0x8C5C0EE125F003CCuL, 0x73EA2D76CF81FB9CuL)),
                    ((+1, 23, 0x9FC56EE2BE794A87uL, 0xB7C94C12ED921A2EuL), (-1, 23, 0xCCB3E73DBE94693BuL, 0xFAD946296B893BA7uL)),
                    ((-1, 24, 0x943D18D5D4A6EC61uL, 0xC73A062D65A5EFF8uL), (+1, 24, 0xCA2785E7003FE600uL, 0xCA62817EA40D9670uL)),
                    ((+1, 24, 0xB06DDBC53ADF470BuL, 0xA85CFDA03FF0DF35uL), (-1, 25, 0x82A21F5440EB081AuL, 0xF8DFA9C70345E6A3uL)),
                    ((-1, 23, 0xFB201B08B663E2EAuL, 0xE3B6955747E82631uL), (+1, 24, 0xD015AA7AA1BD41A8uL, 0xCC6FF752820E479EuL)),
                    ((+1, 22, 0xBD0FD6173F18B030uL, 0x213B54466AA19276uL), (-1, 23, 0xB81CD7EF218A91EBuL, 0x63334BED65EE7B53uL)),
                    ((-1, 19, 0xED3A44A70C0FA5B5uL, 0xD23EBC73F27E0D32uL), (+1, 21, 0x94BA4E47977B902FuL, 0xF1A795F2B9E59C52uL)),
                    ((+1, 15, 0x86D56C2869A408FCuL, 0x2CC3B2EF84CDEC0CuL), (-1, 17, 0x890F5A11A2B6CEE2uL, 0xB13E1A2130731EB8uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p03125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xF8A9B4527E1629BDuL, 0x3353A6DE25A7ADF6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 5, 0xFB65A372C0F58759uL, 0x92966C07891C6FC0uL), (-1, 6, 0x8324FBD970AF6D4DuL, 0x28AD55D4C8852654uL)),
                    ((-1, 10, 0xD8C2430DDFF5E9EFuL, 0x3BF0568CA427C57EuL), (+1, 10, 0xE6011558C84F0A78uL, 0x957612F0766D74A9uL)),
                    ((+1, 14, 0xD09A0C169B3397D2uL, 0x6FF45D330540802CuL), (-1, 14, 0xE23241784514328BuL, 0x94DE142C4A592B0AuL)),
                    ((-1, 17, 0xF645E2957EAA0073uL, 0xB2AA6A144827D7E3uL), (+1, 18, 0x8955A95D73EB5CC2uL, 0x126D346E4FB8A55CuL)),
                    ((+1, 20, 0xB89C0E9F99071022uL, 0x926BA732002C3DCAuL), (-1, 20, 0xD5B1EEC78483CAFAuL, 0x1773BA1F6C62349FuL)),
                    ((-1, 22, 0xAFE5CCE4C6872672uL, 0x534374E7616B98D1uL), (+1, 22, 0xD6127B1F5339AC47uL, 0x52783089A8675915uL)),
                    ((+1, 23, 0xCF463EA0693D1495uL, 0xAAFA498D3CB8D9B6uL), (-1, 24, 0x872E03611897F128uL, 0x45C71DD57898E305uL)),
                    ((-1, 24, 0x8E03FD3E076DFAD7uL, 0x30427B302BE677B5uL), (+1, 24, 0xCC943D733EDB208EuL, 0xF19BBAE7C5194318uL)),
                    ((+1, 23, 0xC93FB88EDA1A652AuL, 0xFE17428ED2BA981DuL), (-1, 24, 0xA86C9AAE61A77AFFuL, 0x39D25296F2E0D296uL)),
                    ((-1, 21, 0xE8A36D3EF8BFB8F0uL, 0xD9C7544ED364D8BFuL), (+1, 22, 0xF9A1CA41FD3444B6uL, 0x9EA9F2B6E13D6950uL)),
                    ((+1, 17, 0xEB58B21E45A8BE7EuL, 0xF6B099B6C8597E98uL), (-1, 19, 0xD1EEC6A93DCC5416uL, 0x9817CC406E6D3E19uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXM0p015625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xFC2D008A9F770E24uL, 0x6E15C3A697ED7FB4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 6, 0xDC5857285006E2A9uL, 0x8677B8B39AEC7F38uL), (-1, 6, 0xE18B4004FDC7A572uL, 0x1D91C58CEE590775uL)),
                    ((-1, 12, 0xA7C9C090DDD0F0BFuL, 0x03D06F40E5A81169uL), (+1, 12, 0xAD86D60348BCCDC3uL, 0xD55B99F9C9C45B12uL)),
                    ((+1, 17, 0x928F204FEC93C281uL, 0x436DD170E75AB541uL), (-1, 17, 0x998CBE240D2816DAuL, 0xCEF777850DCFF242uL)),
                    ((-1, 21, 0xA2A96EFD3F0CE813uL, 0x5C7C9B1F7D584A63uL), (+1, 21, 0xAD405626E3F745EAuL, 0xE9D08C9E81AD78BCuL)),
                    ((+1, 24, 0xF01922A7142B2CD2uL, 0xB16B8218D261C906uL), (-1, 25, 0x8299D5AFFC9B43C0uL, 0x096BE534E37CAB18uL)),
                    ((-1, 27, 0xEFBEE88CC472435AuL, 0x63D2658AF6CB5FF1uL), (+1, 28, 0x860EE65D67972D06uL, 0x23049DF26871BFB7uL)),
                    ((+1, 30, 0xA197BDFE24FAE6B6uL, 0x683027FBCBE4F3CAuL), (-1, 30, 0xBB71CFA90A71CC31uL, 0x9ADAA1B982D1A77AuL)),
                    ((-1, 32, 0x902B3146CEC4E49EuL, 0x080DC7CC893529A2uL), (+1, 32, 0xAFB50392B52C79BAuL, 0xEC708FD372CBC516uL)),
                    ((+1, 33, 0xA3882D2E359D21D8uL, 0x86B5D8055C10F34CuL), (-1, 33, 0xD574AF01FD65100FuL, 0x3D78DC12711653A7uL)),
                    ((-1, 33, 0xDBD1808FE8D2712DuL, 0x20EECB45577F6F90uL), (+1, 34, 0x9E544186E1DB2227uL, 0x448DD032448DB72AuL)),
                    ((+1, 33, 0x9AB3AEF85B094866uL, 0x407DC004F5B5786EuL), (-1, 34, 0x81631C1F5A5D2E00uL, 0x465161B4903F26D9uL)),
                    ((-1, 31, 0xB2F6F29E7C399E5DuL, 0x2BE53EAAFD630D75uL), (+1, 32, 0xBFD4911A01E8BBD4uL, 0xB3EB80780CE6E24CuL)),
                    ((+1, 27, 0xB5C55B667D2D58D8uL, 0x5D5B4070836CC7C7uL), (-1, 29, 0xA1F8ED0396E67F2BuL, 0x03CA35054D5DFAE4uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXZeroTable
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 7, 0xA700000000000000uL, 0x0000000000000000uL), (-1, 7, 0xA800000000000000uL, 0x0000000000000000uL)),
                    ((-1, 13, 0xB8F0000000000000uL, 0x0000000000000000uL), (+1, 13, 0xBB84000000000000uL, 0x0000000000000000uL)),
                    ((+1, 18, 0xE43CC00000000000uL, 0x0000000000000000uL), (-1, 18, 0xE9DB800000000000uL, 0x0000000000000000uL)),
                    ((-1, 23, 0xAD4AF80000000000uL, 0x0000000000000000uL), (+1, 23, 0xB415720000000000uL, 0x0000000000000000uL)),
                    ((+1, 27, 0xA8B3E80000000000uL, 0x0000000000000000uL), (-1, 27, 0xB2B9C00000000000uL, 0x0000000000000000uL)),
                    ((-1, 30, 0xD4E2720000000000uL, 0x0000000000000000uL), (+1, 30, 0xE7A76D0000000000uL, 0x0000000000000000uL)),
                    ((+1, 33, 0xAC1B164000000000uL, 0x0000000000000000uL), (-1, 33, 0xC289DE8000000000uL, 0x0000000000000000uL)),
                    ((-1, 35, 0xAC35949800000000uL, 0x0000000000000000uL), (+1, 35, 0xCDC42CC800000000uL, 0x0000000000000000uL)),
                    ((+1, 36, 0xC79C2A6200000000uL, 0x0000000000000000uL), (-1, 37, 0x81B86DD000000000uL, 0x0000000000000000uL)),
                    ((-1, 36, 0xED85309800000000uL, 0x0000000000000000uL), (+1, 37, 0xB0A1A2BF00000000uL, 0x0000000000000000uL)),
                    ((+1, 35, 0xE3CBC3C800000000uL, 0x0000000000000000uL), (-1, 36, 0xD6D4880800000000uL, 0x0000000000000000uL)),
                    ((-1, 32, 0xB994660000000000uL, 0x0000000000000000uL), (+1, 34, 0x938AC3F000000000uL, 0x0000000000000000uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0078125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x810636569D8E84A0uL, 0x9E7CDE9FE5D39CB4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 7, 0xC74383662EE81194uL, 0x25BF1B1E7CB10BA0uL), (-1, 7, 0xC6B911B467B67C3AuL, 0x66C4130FF6573275uL)),
                    ((-1, 13, 0xF44C022579B3FBE0uL, 0x5B1748745B42A296uL), (+1, 13, 0xF589D7796DF4D549uL, 0x95A3CC46B98F960DuL)),
                    ((+1, 19, 0x9B4BC6A71D167A56uL, 0xE3686B2D5FD7FB1EuL), (-1, 19, 0x9DE68E91E6008C84uL, 0x04B2EBCC0259191AuL)),
                    ((-1, 23, 0xE02FDC1CCACBA641uL, 0xBDB6876678ADB209uL), (+1, 23, 0xE7EF0C4D7848732FuL, 0xCDE41B72582DEE55uL)),
                    ((+1, 27, 0xBCE999BE427D6431uL, 0xE6B70D05073C3459uL), (-1, 27, 0xC8ADBD6F87A94212uL, 0x603D178652142FD5uL)),
                    ((-1, 30, 0xB7C1045C80D0D129uL, 0x42C0E504676E83A6uL), (+1, 30, 0xCB7328621E0DE5A9uL, 0xBEEFDBB7E8D35A9CuL)),
                    ((+1, 32, 0xC4305E44E6A79248uL, 0x94DC7F1BA8F14BB8uL), (-1, 32, 0xE855A699FE0BCA2CuL, 0x240F616C10F98941uL)),
                    ((-1, 33, 0xCE23A122E942735BuL, 0x81109ACC76BF0DCEuL), (+1, 34, 0x8909FA4D6F7B6733uL, 0xAA66B4AE9A652488uL)),
                    ((+1, 33, 0xA89DBCB94F6D5F50uL, 0xA9BE790215776158uL), (-1, 34, 0x8BAD3C3B33818E56uL, 0x6806A9B50C876711uL)),
                    ((-1, 30, 0xE042438726AC024AuL, 0x8AEA79E7F389CE96uL), (+1, 32, 0x9D869560C1095341uL, 0x9CF878F4802D4A46uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p01171875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x818E3BAB99CD920DuL, 0x259E2081C05F78A8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 9, 0x84732D50043BB1B0uL, 0xF0FCFB7FD5F1E2B2uL), (-1, 9, 0x8320186C864F930EuL, 0x3F186351575DD8A4uL)),
                    ((-1, 17, 0x928A3FE30C699BE6uL, 0x047C4BB8DD09D213uL), (+1, 17, 0x91525BA184FE10ECuL, 0x795FDD9378AB5637uL)),
                    ((+1, 24, 0xD4746D112C3BF0DBuL, 0x00E9102BA147D240uL), (-1, 24, 0xD318DE1CD6B52CB3uL, 0x11D0F8C7C05FB5C3uL)),
                    ((-1, 31, 0xD887ECB79D366104uL, 0xC4E823A110E58981uL), (+1, 31, 0xD7A7785105FA22C2uL, 0x19758FF58BB738D5uL)),
                    ((+1, 38, 0x9D0423675B959ED8uL, 0x688733782167E1BBuL), (-1, 38, 0x9CE0E4ED9ACC6CCAuL, 0xE01C30D2BDE1F14BuL)),
                    ((-1, 44, 0x9F8E042CB6CE76EAuL, 0x5FAE418440ED4FD1uL), (+1, 44, 0xA028276130A859D3uL, 0xC80CAF264AB6C2A1uL)),
                    ((+1, 49, 0xDA3517FFFDE4F92FuL, 0xFD6B3698BDC29498uL), (-1, 49, 0xDCA7C5CE825CB078uL, 0x14C3BA76877129DDuL)),
                    ((-1, 54, 0xBF24F3F0A693E292uL, 0x6AEE8F02982A267AuL), (+1, 54, 0xC3AC96D10776B652uL, 0xF06A0EA36A1E652EuL)),
                    ((+1, 58, 0xCCF4E312B1ECEED5uL, 0xDBBAFF55A1A4B88DuL), (-1, 58, 0xD62615D88C8712EAuL, 0xE918488FF6602B5EuL)),
                    ((-1, 62, 0x804678D1FCDA74DBuL, 0x52A33D4122F2806EuL), (+1, 62, 0x8AB3A76E44CEEAFAuL, 0xD9F740347F162E45uL)),
                    ((+1, 64, 0xAF5FFCD7878EBAECuL, 0x722413DFBD68EC6AuL), (-1, 64, 0xC91C4147763A7AEAuL, 0x8CE60D775A64516BuL)),
                    ((-1, 65, 0xE95B762E803F4D01uL, 0x282859ABC25830F6uL), (+1, 66, 0x94BEE78B3670C033uL, 0xE36FD7F0797DD808uL)),
                    ((+1, 65, 0xEE9BEFB4FCE380F5uL, 0xD6B70DDB6ECE38A6uL), (-1, 66, 0xBBA15EF1821B85ACuL, 0x8D2084A2E8A24787uL)),
                    ((-1, 63, 0xC291FBBCDFAA9FC6uL, 0x2068157D6A2F8D21uL), (+1, 65, 0x81A094601242D0B1uL, 0x87942BAAD7A9F2FDuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p015625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8219C7966BCF46A1uL, 0x82B727533E4A71BDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 8, 0x9355CDE87D719AE2uL, 0x7F4FD879CC9D2FA6uL), (-1, 8, 0x917FE14998BCE839uL, 0x0F6AFE5FC67E56C2uL)),
                    ((-1, 15, 0xEA0DF497F2DBB523uL, 0x42D7D3076A3BD193uL), (+1, 15, 0xE77F064FB4F7492EuL, 0x78F639A9E025EBE9uL)),
                    ((+1, 23, 0x845CE9325E49E538uL, 0x2643E59A8C9F15F1uL), (-1, 23, 0x83313D0F54EDDE84uL, 0xDB588C3680D1DA83uL)),
                    ((-1, 29, 0xEC78C02D0882B12FuL, 0x8C0EAA416B38C03CuL), (+1, 29, 0xEAD49181D5B96DC2uL, 0x85CA23E727DDBA07uL)),
                    ((+1, 36, 0xA81758BAFC18F438uL, 0xC3F87801FFCAF60DuL), (-1, 36, 0xA75058C044DCE16EuL, 0xF852D9CAC77F5BCFuL)),
                    ((-1, 42, 0xC049FC32C4C753C8uL, 0xD895D83DD13DD37DuL), (+1, 42, 0xBFEC966E984CE2A7uL, 0x3678F293E95DE1FBuL)),
                    ((+1, 48, 0xAEAD3084C43B4DF6uL, 0xDC31039A2C24AE2BuL), (-1, 48, 0xAEF8FC6FB14300D8uL, 0xB393345CD47719A0uL)),
                    ((-1, 53, 0xF663A12483407AC6uL, 0xF9B23D04343A0539uL), (+1, 53, 0xF807CB1DC6AB986CuL, 0xF742775DDA6BC414uL)),
                    ((+1, 59, 0x813AE71F4DB74E4EuL, 0x3FE35B7CCF3E2274uL), (-1, 59, 0x8310DF0590647CB0uL, 0x3FA998B4E21E0AA5uL)),
                    ((-1, 63, 0xBCE652A35FF6CCF9uL, 0xAF8A0DDE3FEDE8E2uL), (+1, 63, 0xC1F34F72E0AD55B0uL, 0xB93CA02DEE4A65A9uL)),
                    ((+1, 67, 0xAE8EE6E0DDDF4E49uL, 0xA0989FFAC2034B4CuL), (-1, 67, 0xB746D2392C7BF3EFuL, 0x2A57D8C9D162275FuL)),
                    ((-1, 70, 0xB4C9007E72C74076uL, 0xEF166DDB119C2EC8uL), (+1, 70, 0xC62FE7880735BAF4uL, 0x34CCC250FFA71043uL)),
                    ((+1, 72, 0xB4D4CC64AB1223AAuL, 0x77DE27A51F11E008uL), (-1, 72, 0xD82CC3EB857484DEuL, 0x2EDB390DBBF29F33uL)),
                    ((-1, 73, 0x881DE141F497E08DuL, 0x6A2C22B57278B2E7uL), (+1, 73, 0xC47F2FC65B00F84CuL, 0xE6C4531CA02C0712uL)),
                    ((+1, 71, 0x9E0B429F87397453uL, 0x6CDF7E996B5A6D46uL), (-1, 72, 0xBFCB3DB745814923uL, 0x9A264099BE23D7ADuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p01953125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x82A910178D51D4B0uL, 0x8F9743CF10871332uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 7, 0xA760331343E1706AuL, 0x455DDFD523A1FC1BuL), (-1, 7, 0xA5144CC5152802B5uL, 0x7BC2370C74D1D2E3uL)),
                    ((-1, 14, 0xC7C7BC4C987FB341uL, 0x0A9645C991C23AF3uL), (+1, 14, 0xC51D947DD7EEF9BEuL, 0xD5A3BFA229AD1F72uL)),
                    ((+1, 21, 0xA68869C4FB6D62F8uL, 0xC2FDE3D93A6F66AEuL), (-1, 21, 0xA4D0D7FDF2BC10CDuL, 0xDF2EF7F8E6C9879CuL)),
                    ((-1, 27, 0xEB1078A0038F7B14uL, 0x23A13ECA23A11D79uL), (+1, 27, 0xE90CD134A5E4820CuL, 0x05AA6D2BF8BB9997uL)),
                    ((+1, 34, 0x8890F1EEE695B8F5uL, 0x5F98A3ECB53C1CA3uL), (-1, 34, 0x87BC1699DF1CB05CuL, 0xC1F1D499D1D0887DuL)),
                    ((-1, 40, 0x8585827FE3BC6735uL, 0xBFF22761ED6A91B8uL), (+1, 40, 0x850E3EE213AA15CEuL, 0xB2CC0679E1870694uL)),
                    ((+1, 45, 0xD9F5D3A4DCE90D9BuL, 0x37B7E84F4A15FB28uL), (-1, 45, 0xD9E73A033AC499DEuL, 0xD83D1900BB660562uL)),
                    ((-1, 51, 0x9237A5322180EC4DuL, 0x4DA2647916D96EEBuL), (+1, 51, 0x92CA9D0E4CA0041BuL, 0x449E63545F851A1BuL)),
                    ((+1, 56, 0x9D4703211306755CuL, 0x6E337C65FD920DD0uL), (-1, 56, 0x9ECC35C581AC56F0uL, 0x860B7CC7EFECB75CuL)),
                    ((-1, 61, 0x812F7634BE7CD9CFuL, 0x0919C0A5C1564410uL), (+1, 61, 0x8392E88865A01104uL, 0x1E93F789C273F60BuL)),
                    ((+1, 65, 0x96A2C136B05A1BCBuL, 0xA79AA23285EF8F19uL), (-1, 65, 0x9BA971BFD8C9B6A6uL, 0x7BE16C436445EB43uL)),
                    ((-1, 68, 0xD839571461643FB7uL, 0x90030685469D35DBuL), (+1, 68, 0xE61DCFC9BEBFF8E3uL, 0x8E5F3EFE47B772BCuL)),
                    ((+1, 71, 0x9A8576068173B08CuL, 0xBE4D004B81C8D3BAuL), (-1, 71, 0xAFFAD6472CA79233uL, 0xDB698D8637466020uL)),
                    ((-1, 72, 0xA45245171F4AA6F8uL, 0x0D7B864DA35E5366uL), (+1, 72, 0xDCF94C8C1EE81872uL, 0x1289D3D41C98977FuL)),
                    ((+1, 71, 0x826E9027DE384B45uL, 0xFBB2298F446DF457uL), (-1, 72, 0x9198E3ED26A2E64EuL, 0x21B950763009C87CuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0234375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x833C5116721986FCuL, 0x67AC1DFF1A356D3DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 6, 0xA540B3ED39EA7FBBuL, 0xD4BBDB1DC796A1F1uL), (-1, 6, 0xA3743FF301C97ED1uL, 0x480B951E4DDC9378uL)),
                    ((-1, 13, 0xADB489D80F101FC6uL, 0x2D6F2694A9D44133uL), (+1, 13, 0xAACFA22217CE18DDuL, 0x28F2BCBA19768337uL)),
                    ((+1, 19, 0xC24081A5EE50F21EuL, 0xDFE0CD2FBC821A33uL), (-1, 19, 0xC06C3B8204BBD771uL, 0x7692E29B389D4B5CuL)),
                    ((-1, 25, 0xD9531D605CB0C167uL, 0x06B793AE14772EB6uL), (+1, 25, 0xD738D67994AFFADEuL, 0x76F4BD63083C602AuL)),
                    ((+1, 31, 0xBD397FEA95117B08uL, 0xCE114AAB1B24F522uL), (-1, 31, 0xBC34C0F0E8C4AA4BuL, 0x630509C29101A0BFuL)),
                    ((-1, 37, 0x908CA4525D6377BDuL, 0x479EEDE148729AFBuL), (+1, 37, 0x902149B35BB2DF9BuL, 0x651FB7863B015EFAuL)),
                    ((+1, 42, 0xB7707743D40665CEuL, 0xBADA5068AA215E45uL), (-1, 42, 0xB7B25900E8A98CE2uL, 0xB882718DAEE94FD4uL)),
                    ((-1, 47, 0xC0F36CE70635D7F2uL, 0x10CC9C1A67E4C182uL), (+1, 47, 0xC233F4FDDD3E8F6AuL, 0x6D0E2E85DAA395C4uL)),
                    ((+1, 52, 0xA2964E8A553A131EuL, 0x2311955EBEF0ABD1uL), (-1, 52, 0xA4D4EE3DA1E040BFuL, 0xC1C227E3033F8D2BuL)),
                    ((-1, 56, 0xCEF437E31957895DuL, 0x6F72CE432C5FC34CuL), (+1, 56, 0xD43A04E7D63BD1F8uL, 0x953C7AA3D40E1A58uL)),
                    ((+1, 60, 0xB623906809FD9F11uL, 0xB24487E0D7BAB87CuL), (-1, 60, 0xBE881363357FF13CuL, 0xB3776BE052EE7DEFuL)),
                    ((-1, 63, 0xB2A6F02F579298A5uL, 0xF252947B611B081DuL), (+1, 63, 0xC3F51A4EC085143FuL, 0x5690A7A4AB7EFE1BuL)),
                    ((+1, 65, 0x869A36B45F8896DCuL, 0xFC5C7AAB47F9CD6BuL), (-1, 65, 0xA96D632B584C5B5AuL, 0xAE711C18E099C863uL)),
                    ((-1, 64, 0x93ED6A0152755DC6uL, 0x3C0EB5604BE35370uL), (+1, 65, 0x973F97D56D26E2DDuL, 0x017128CFB34FB1C5uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p02734375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x83D3CD74846A6DC7uL, 0x80137BC8B5641965uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0x9F9F1C2B3690AE7AuL, 0x504A0224245AF8C9uL), (-1, 2, 0xC04BBB0EA20CBC0FuL, 0x7B645E8DF9897E0CuL)),
                    ((-1, 12, 0xB225368FB110F2FEuL, 0xA3B8CF8613059462uL), (+1, 12, 0xAD0E128F40D6E80BuL, 0xACAD8CEA1DA2ACFDuL)),
                    ((+1, 16, 0xE8E1F226DC2E37F1uL, 0xF010E6689389EB6BuL), (-1, 16, 0xEEBAFF655CE40E81uL, 0x84B2B526C067118CuL)),
                    ((-1, 23, 0xE1BFBC6CCF08DCD1uL, 0xDC23E36D7E399E67uL), (+1, 23, 0xDCFF543E93DBF041uL, 0xBD6BC8809F871715uL)),
                    ((+1, 28, 0xD44DD079D3E9733AuL, 0x6F9ECC809E658964uL), (-1, 28, 0xD60137768B218DC9uL, 0xA9C57990A846F91AuL)),
                    ((-1, 34, 0xB3CD7BDB6993B15FuL, 0xFCBA1C4EC00D7A38uL), (+1, 34, 0xB205990773F65E66uL, 0x3C28B02DF60896AAuL)),
                    ((+1, 39, 0xA7F6EED74A29F451uL, 0xC2741DD7D72AED73uL), (-1, 39, 0xA933105BD3E21355uL, 0x33D85CA03C0AF5C8uL)),
                    ((-1, 44, 0xADC42770AE80A6AFuL, 0xE49D6EF539D2D841uL), (+1, 44, 0xAE2F731DC6DEB3EFuL, 0x2BDFF3609FBF297DuL)),
                    ((+1, 48, 0xFEB1EE77A2B5E7FAuL, 0x1A9E9FA7F482A0FEuL), (-1, 49, 0x8165B8F9D0A4A0AEuL, 0x292F2A60FE2BC7E7uL)),
                    ((-1, 53, 0xA568DDAEB1F03777uL, 0xFEF2A283FBABBFAAuL), (+1, 53, 0xA8BEDEE5491CEDFCuL, 0xB9EF0C2921F5B4F5uL)),
                    ((+1, 57, 0x94C54992978D2D6EuL, 0x54C4945DC6C61417uL), (-1, 57, 0x9AFC10FD4D36B0F1uL, 0x1D2C9CDCC6583124uL)),
                    ((-1, 60, 0xBACE3F70E3C0B09FuL, 0x140CF806EF9C3B7CuL), (+1, 60, 0xC75DBD7516532859uL, 0xC49B05E591F60A30uL)),
                    ((+1, 62, 0xC02DC763AD0F9BFDuL, 0x743E27FB8809D109uL), (-1, 62, 0xE5C5246B711C4375uL, 0xDE891D8D42821015uL)),
                    ((-1, 61, 0xFC4134C4D7792EF5uL, 0xB7011C09E5DB99BAuL), (+1, 62, 0xF9684029DAFE44C5uL, 0xB97F7F5B0A403F5FuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p03125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x846FD071DCEF4F25uL, 0x2CD672F4A86E970AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 4, 0xBBBFD0EBE65DD46EuL, 0x7391464B90DB5527uL), (+1, 4, 0xABE46862616E8CBFuL, 0x3669BEF1972609E3uL)),
                    ((-1, 11, 0xF29F1AE6A01D911AuL, 0xEBEF42C2B6E200CFuL), (+1, 11, 0xE89817E9F504FF7AuL, 0xBFD8A7021D805191uL)),
                    ((-1, 14, 0xA29A9CB6F73EF40FuL, 0x02CF9BA9AB36500DuL), (+1, 13, 0xF2BD03BC86F0FDEBuL, 0x4DD7C5967EBCAB7BuL)),
                    ((-1, 22, 0xAA8914D961E8C1E2uL, 0xBFA7E055B697057DuL), (+1, 22, 0xA3B1F3A070FC42C5uL, 0xC46D7F5DE50103F2uL)),
                    ((+1, 25, 0xA4F630C70A91054DuL, 0x82218157D9344893uL), (-1, 25, 0xB8A28F09E4D46B16uL, 0xE8AE2D6733700054uL)),
                    ((-1, 32, 0x8BCF6E04CC4F7F01uL, 0x7E99323F4EEDB813uL), (+1, 32, 0x880E05186632CFF3uL, 0x223EF2C026EAE505uL)),
                    ((+1, 36, 0x87892A2DF16BBCA5uL, 0x05674002E3E3B4ACuL), (-1, 36, 0x8D11EB653AEBA7F8uL, 0x2E6F75EAAFEB82A5uL)),
                    ((-1, 41, 0x91AF8916579660C6uL, 0xEF78738C0CC95286uL), (+1, 41, 0x90D017A8B9ECDE05uL, 0x6EB9F6B3A6C2EF62uL)),
                    ((+1, 45, 0x8BE739B3FDD71063uL, 0xD429ADE7D6655687uL), (-1, 45, 0x9145ACC3802C7204uL, 0xE3E0EB9B1524820CuL)),
                    ((-1, 49, 0x99869EBCB76F3F3AuL, 0x45AD2548B67A1B36uL), (+1, 49, 0x9CC780B11F4CB774uL, 0xC8CDE3917FF4F46DuL)),
                    ((+1, 52, 0xB87BEB31FCB520E7uL, 0x0229B37CC5C4FFA0uL), (-1, 52, 0xC65E529D114A942FuL, 0x0FBB3C6CDC4EDED7uL)),
                    ((-1, 55, 0xA96680C02EEA56D7uL, 0x12A1ED56BCAB7EB0uL), (+1, 55, 0xB810E57F1DC2668FuL, 0xF4D230FC81E3914CuL)),
                    ((+1, 55, 0xF458561F86DA8BD0uL, 0x3E5E18F51C01A778uL), (-1, 56, 0xC3A2B35F8429F782uL, 0x8BF88A0F2B2CF371uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p03515625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8510AF8E57F87BEAuL, 0x1718FB9AB3213120uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0x95DBDC4CF57508C7uL, 0x1CEDC087F77EB791uL), (+1, 5, 0x8B3DE4CB160F6198uL, 0x6E0DCFDD46183912uL)),
                    ((-1, 11, 0xD4C8FF14D59955B5uL, 0x1CE3D8919F148A73uL), (+1, 11, 0xC9B2C93B17D722DDuL, 0x695BAA89568DAD6FuL)),
                    ((-1, 15, 0xDA345B3DE56671E1uL, 0x0F2053DD7E5EF98CuL), (+1, 15, 0xC1993874B3A9DC4AuL, 0xFDD7D87970736D96uL)),
                    ((-1, 21, 0xE4FBA4B19B289DDAuL, 0x3E99C44E75E8BDC8uL), (+1, 21, 0xD7774E89F25A6DD7uL, 0x27C7D74BA4659A9BuL)),
                    ((-1, 23, 0xC554E9E067A6CB77uL, 0x8172294A8BCB616FuL), (+1, 22, 0xEBD9A36BDB1C2552uL, 0x6371A4D3D2B7C4B8uL)),
                    ((-1, 31, 0x840793B0B93F4814uL, 0x883DF8E45199ED80uL), (+1, 30, 0xFA6449A086AFBB90uL, 0x771809F946134907uL)),
                    ((+1, 33, 0xE232F7B495650BD9uL, 0xA7E1BA6A9B2351E5uL), (-1, 34, 0x8179DA1A0614B8F1uL, 0xA09BF74A95C1D89FuL)),
                    ((-1, 39, 0xCA14F21AF126D958uL, 0xFA7BD4126351D786uL), (+1, 39, 0xC46C59DFDBDFE7B9uL, 0x5B9F9856682A91C1uL)),
                    ((+1, 43, 0x8F714DB91BE88732uL, 0x84F3B60481A18BCDuL), (-1, 43, 0x993137E0A472147CuL, 0xC814AB312F485CE6uL)),
                    ((-1, 47, 0xB4F50C505A00AB69uL, 0x6C81FD203C8EA97CuL), (+1, 47, 0xB5A381F9CAC3605FuL, 0x620391498CFA70F6uL)),
                    ((+1, 50, 0xC1F2B8F5E49035D3uL, 0x59BE1A4A83F7E304uL), (-1, 50, 0xD3E04A568A9978B6uL, 0x8050933DE266EC82uL)),
                    ((-1, 53, 0xC8FCF0E12A9E84B6uL, 0xDE0DD85BD289098AuL), (+1, 53, 0xD5C8101871343D52uL, 0x73872A94AE293BFAuL)),
                    ((+1, 53, 0xF9C9819B5A3DD9A8uL, 0x200586F77C7BD8D0uL), (-1, 54, 0xD6563AB70B6F4D43uL, 0x185A1B8108DE78E7uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0390625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x85B6CD38C59DCB38uL, 0x1EFDF1200A9FA607uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0x973A9910C16EC7CAuL, 0xFEDFE7AA7488CBA5uL), (+1, 5, 0x8BB6DE7C8A68164FuL, 0x5EAEA3C6361D632FuL)),
                    ((-1, 11, 0xA01DBD6E1F26238FuL, 0xD1145B959D177573uL), (+1, 11, 0x962D263E0199E8F3uL, 0xD035ED559A708D28uL)),
                    ((-1, 15, 0xA689997ECD20205AuL, 0xF2E1C32B49F0FAA8uL), (+1, 15, 0x92AAD7624E17EA93uL, 0x2A0DF2BBB7866BC3uL)),
                    ((-1, 20, 0xEA2E68F56C878215uL, 0x0835836AD07B3A1BuL), (+1, 20, 0xD88FC6CB60D15D3AuL, 0xA4F664015CF113E1uL)),
                    ((-1, 23, 0x91332FC3DDB3CDE3uL, 0x7BD5E11EE7080C2CuL), (+1, 22, 0xC7BA454ADD29C13DuL, 0x78944BAD1F055631uL)),
                    ((-1, 29, 0xA994DF70B5016A42uL, 0xF8D9F7B4BB0969A9uL), (+1, 29, 0x9D5AD786518A2F5FuL, 0x9101E1936A473F36uL)),
                    ((+1, 31, 0xB7BDEE4CFBCEA07AuL, 0x572FC93F8174FC07uL), (-1, 31, 0xEA2666B4454FA538uL, 0x9D2B607C79384AA9uL)),
                    ((-1, 37, 0x9B3811FDFE5DF5BDuL, 0xD78B66683B1B0D95uL), (+1, 37, 0x9481A0CBE35617BAuL, 0xC1D185F6E690C7B3uL)),
                    ((+1, 40, 0xA38B30031956A796uL, 0xDB3A69A11B19C458uL), (-1, 40, 0xB60224D6DABF569FuL, 0x89D0F6A39282701EuL)),
                    ((-1, 44, 0x92D5F52DC14E69B5uL, 0xDF2C05D1BC838E87uL), (+1, 44, 0x9400CE000932F4ECuL, 0xE5877C302EEC87B2uL)),
                    ((+1, 46, 0xC8A3B362B56DF131uL, 0x665E469AB14378D6uL), (-1, 46, 0xE66C1A46D785419CuL, 0xF3E62D9E190C7B13uL)),
                    ((-1, 48, 0x90520EB250B4C51AuL, 0x1438A97ED4EFBD1BuL), (+1, 48, 0xBBDFB6E9B80BA1F3uL, 0xE4B51BAA1E1AC848uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p04296875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x86629CD79800F8F2uL, 0x8EFF7BB59BACCF69uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 7, 0x83A779E354D1CBE9uL, 0xBBDF2B97C76D9FA4uL), (+1, 6, 0xF83212BB8CB158A5uL, 0xB49779109BA78386uL)),
                    ((-1, 13, 0x8D256E1F1BE1C903uL, 0x809A3726EA05B422uL), (+1, 13, 0x83D313468D8049A6uL, 0x03FF7E4134096756uL)),
                    ((-1, 18, 0xCA984A657EE638B4uL, 0x2DA7E0F657E2DAEEuL), (+1, 18, 0xBB3A045621D77270uL, 0xC8D1B2125D147418uL)),
                    ((-1, 23, 0xD8C360927568BCC3uL, 0x1342DF60B3DBC6ECuL), (+1, 23, 0xC5FCFD864978B7A9uL, 0x6E620318502AB329uL)),
                    ((-1, 28, 0xB451DA60BA9821C1uL, 0x496FFFF1F1972820uL), (+1, 28, 0xA2625416798992F9uL, 0x075BC28DBF47CC47uL)),
                    ((-1, 32, 0xF196912A9B59B43FuL, 0x9DAB07ECD1946C3CuL), (+1, 32, 0xD5DF9A8ED0F68048uL, 0x3BD4360F2ED19D06uL)),
                    ((-1, 37, 0x8423650A2D05532CuL, 0xACE005A18613296DuL), (+1, 36, 0xE4D366B05534347DuL, 0xF4B85E1CF29333A2uL)),
                    ((-1, 40, 0xF06310428ED6D50DuL, 0x6A0B04E7E938FCC1uL), (+1, 40, 0xCA3864060D3E653AuL, 0xBD48781B85DD2EE1uL)),
                    ((-1, 44, 0xB64896DE2374D88CuL, 0xE320361716C5B3E3uL), (+1, 44, 0x934CA74281528D5DuL, 0xE03C338C07B05FB2uL)),
                    ((-1, 47, 0xE9D107653DE45C62uL, 0xAC434ECB60797053uL), (+1, 47, 0xB331A01A351EC226uL, 0xEE134ADD195ECF04uL)),
                    ((-1, 50, 0xFA5A2F14D136488DuL, 0x39E965BAEC97D92AuL), (+1, 50, 0xB2F6C81D2716319AuL, 0xB85A050CE1895E6AuL)),
                    ((-1, 53, 0xDFF20FFF65366382uL, 0xA4897313D887227DuL), (+1, 53, 0x96326B614C997618uL, 0x27558B4AE15FDD6BuL)),
                    ((-1, 56, 0x98E181254D145128uL, 0x4EBD49C98C5C5B16uL), (+1, 55, 0xC7D8CA550E607B8BuL, 0x325A9F0A3B02EED4uL)),
                    ((-1, 58, 0x8F652948E1674ED6uL, 0xBD33645746A4ACC0uL), (+1, 57, 0xDD1D91046BEC678DuL, 0xC1BEB4AD2C30DED6uL)),
                    ((-1, 58, 0xB218CE9CDA985AE2uL, 0xD5A81F6BF284CB22uL), (+1, 59, 0x9ED511CB0097E399uL, 0xB880E28410DB76ABuL)),
                    ((+1, 59, 0xFC3B5F48C7503F90uL, 0xD68434AB2054E2DBuL), (+1, 60, 0x8A2CA695953D1AABuL, 0x11165FB9EC97E087uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p046875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8714A8B9D205A3A2uL, 0xBD66D2D8C5C394E1uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xF0D190E6057C5F89uL, 0x83CDEF453A029338uL), (+1, 6, 0xE1828A43CBB62050uL, 0x1C22A892E4FFBDA8uL)),
                    ((-1, 12, 0xE7E7C9122333FEAAuL, 0x6104C30C2406711DuL), (+1, 12, 0xD6D07CB12813CAB5uL, 0xA4D66B605051EE76uL)),
                    ((-1, 18, 0x94A734222D7B96A6uL, 0x3D082653F41CE023uL), (+1, 18, 0x87F76A8896DBC38DuL, 0x5C2717BD45038915uL)),
                    ((-1, 23, 0x8D208B14E477345EuL, 0xCCD592021769D128uL), (+1, 22, 0xFE744C23DBAAD32CuL, 0x3C4DF2C6D6712428uL)),
                    ((-1, 27, 0xCF429258A8BA1735uL, 0x8734B505B893743DuL), (+1, 27, 0xB78283BA9F771D9AuL, 0xB9E260B44A5EAAADuL)),
                    ((-1, 31, 0xF3C0E7D271301244uL, 0x7D4D8AC5F83E98BFuL), (+1, 31, 0xD2FE9A52DCC14466uL, 0xC6523D787285D94DuL)),
                    ((-1, 35, 0xE8FE6BC721050E6BuL, 0xDF89271FCBA8E039uL), (+1, 35, 0xC3A5EB57BA68689BuL, 0x3FAF57DE669ABD33uL)),
                    ((-1, 39, 0xB853F45B6671B2EBuL, 0x49E1A2CF371B9A91uL), (+1, 39, 0x94A29AAD4D354D7BuL, 0x1097F194EC2FE7B6uL)),
                    ((-1, 42, 0xF1E5E2FCBBA6BCD6uL, 0x87EC4A54E97E1BA7uL), (+1, 42, 0xB88D6D12709C1E27uL, 0xF01F051BC594F9BFuL)),
                    ((-1, 46, 0x84C477FD80021E5CuL, 0x329670C8DF5DE176uL), (+1, 45, 0xBDA124949CD27BE9uL, 0x352C72F5A89D42D8uL)),
                    ((-1, 48, 0xEC31ADAD87551CFDuL, 0x02E0FC06D144A024uL), (+1, 48, 0x9DEEC860D0EAEACAuL, 0xC1EBFD9A39DBF7E2uL)),
                    ((-1, 51, 0xA352011F61596D31uL, 0x6827162FADF6EC0FuL), (+1, 50, 0xD97ABB2A228ADEBDuL, 0x77663A71CA8D4A81uL)),
                    ((-1, 53, 0x8F15F92250F181F3uL, 0xDBC0FCB9F83898ADuL), (+1, 52, 0xE0C7DCE2270F0911uL, 0xA1E1B08FA019DD67uL)),
                    ((-1, 53, 0xBB09D03B9EB9A749uL, 0xF992054977D27BEEuL), (+1, 54, 0xB0D68E0D8BB91354uL, 0x8C1E0B45BC4FF65EuL)),
                    ((+1, 55, 0x9DB0C6C304E83D1FuL, 0x759048685D9FFE6BuL), (+1, 54, 0xDE0C9BACC6B2EC57uL, 0x47737AF3B0906F43uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0546875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x888E3F2258C02466uL, 0x9E63C0110F611376uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 7, 0x8857E0C30417419AuL, 0x288E4AA8BBFDF1A5uL), (+1, 6, 0xFCB8291F57B0A7F1uL, 0x275D848D2CAF63C2uL)),
                    ((-1, 13, 0x816329D4EF38201DuL, 0x7AFF0B56D9A44AFFuL), (+1, 12, 0xEC9DE00CBEF87CD7uL, 0xD8A4E5868B32E2D4uL)),
                    ((-1, 18, 0x9E4FB5F401883DC7uL, 0xCB5C3C2ACBF9C2F6uL), (+1, 18, 0x8E85846D43EB324BuL, 0xE5B22171DDFA2253uL)),
                    ((-1, 23, 0x8AC1D318F2A1CFCAuL, 0x8C914A30E16D4E57uL), (+1, 22, 0xF50F3C239565D23DuL, 0xCE8E4770D7404B3DuL)),
                    ((-1, 27, 0xB939AF677BC4763AuL, 0x20383A16E2525710uL), (+1, 27, 0x9F9DBDA29B50E9EDuL, 0x89776B1E256BEB30uL)),
                    ((-1, 31, 0xC25AFB75C192F678uL, 0xA5101DBEAC910879uL), (+1, 31, 0xA224A87E6AA20CD9uL, 0xBA7421DEE024AA63uL)),
                    ((-1, 35, 0xA44BE837B07F9464uL, 0x36F3F336BC77839DuL), (+1, 35, 0x8351632E264930ABuL, 0xCE1CF91653FC74BBuL)),
                    ((-1, 38, 0xE2003080645020E3uL, 0x92CBEF226D2A2F91uL), (+1, 38, 0xAAD6286FE94517E6uL, 0x028B397C76E3C3C2uL)),
                    ((-1, 41, 0xFE358B1B8DD2B506uL, 0x11455BF70075A021uL), (+1, 41, 0xB46430D542F91AABuL, 0xD96B29E350DC682CuL)),
                    ((-1, 44, 0xE3A053D41396C030uL, 0x870FDEE94B98251CuL), (+1, 44, 0x9937D50DA54AEDA8uL, 0xC3E2AAC133581EA6uL)),
                    ((-1, 47, 0x99789F7BB28A7EA3uL, 0xAC628CF0C0945CEFuL), (+1, 46, 0xD1DA96E0521A2CF5uL, 0x3CBB9EE041CB856AuL)),
                    ((-1, 49, 0x80DD23D21491FB4CuL, 0xA2BEC75092D211C4uL), (+1, 48, 0xD84C8A1233723448uL, 0x10FDB06EF40B9829uL)),
                    ((-1, 49, 0x8EC6A14B03124BA3uL, 0x5364A4AC7E6DBAF8uL), (+1, 50, 0xA0393A8DD57741C5uL, 0x3779746C6330EF10uL)),
                    ((+1, 51, 0x8714EAF846A17AE3uL, 0x32930D35B04DBA06uL), (+1, 50, 0xC8247893E6BAF106uL, 0x4B9E31B8820BF513uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8A2AB5BE94A3BE27uL, 0x350E83D7F221AC22uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xB5F7A69A11F28C87uL, 0x80F6BCB9F4371971uL), (+1, 6, 0xA571C1C823D3A2B7uL, 0x629B45C5371678E3uL)),
                    ((-1, 11, 0xFB99793A306DDD7CuL, 0x89B7E2F9A5F0F978uL), (+1, 11, 0xE058C25204BCACB1uL, 0xE447E125DCE14B3BuL)),
                    ((-1, 16, 0xE4905C6C2CC17C5EuL, 0xE7EDD427F7C5E8BAuL), (+1, 16, 0xC6E96BA4692E3889uL, 0xB66FADC71ED0BF23uL)),
                    ((-1, 21, 0x97C57CD872EEC35FuL, 0x7CEB45EE8023EB60uL), (+1, 21, 0x801B4D708086996CuL, 0x9781FC643FE2CAB8uL)),
                    ((-1, 25, 0x9A6B18A3DC692AF7uL, 0x136A744D63EF3A59uL), (+1, 24, 0xFA70EA0C610A670BuL, 0xD08F4871E2248738uL)),
                    ((-1, 28, 0xF8E8789A55CC634AuL, 0x3C2C1DC93AD0906FuL), (+1, 28, 0xBFF59C8E0F448D0DuL, 0xD509D265916037F1uL)),
                    ((-1, 32, 0xA06A473750A59A11uL, 0x38EB6CA0F684EC90uL), (+1, 31, 0xE96D124CF41CA052uL, 0x83C2B489275CF6A4uL)),
                    ((-1, 35, 0xA4F6ED3D422EBC46uL, 0xB9A23EC598C256A2uL), (+1, 34, 0xE43F7C7E71DB6E67uL, 0x8F4E313E21368CB0uL)),
                    ((-1, 38, 0x81982CF994728B2CuL, 0x7704A86FBD381497uL), (+1, 37, 0xB16D165F3D88A48AuL, 0xCC7D118BFBAD2BFCuL)),
                    ((-1, 40, 0x8F5ABD046BF6F037uL, 0xBA211691D676BD82uL), (+1, 39, 0xD9E25E79336D29D5uL, 0x306787C654886361uL)),
                    ((-1, 41, 0xAD3BDD766AE28C2BuL, 0x40126D5E26711C17uL), (+1, 41, 0xC214A9ECDA29F470uL, 0xE301B4D20C550191uL)),
                    ((-1, 39, 0xB4622AC41610FC56uL, 0xAC5C1D04E88ED65EuL), (+1, 42, 0xE939802EBEA0EFDEuL, 0x9E03EF1AEC0DC706uL)),
                    ((+1, 43, 0xA5CCB8F3399215C9uL, 0xC64053810B30CA4AuL), (+1, 42, 0xCBB001D24D1677BBuL, 0x0130CB06D42D3E38uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0703125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8BF3648300BA0BF2uL, 0xF717233FCE92729BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xBA0BBA9D388B075DuL, 0x660936E03E9BABC8uL), (+1, 6, 0xA6B6CDC1A15D2A7CuL, 0x47BF7682F0E9713EuL)),
                    ((-1, 11, 0xF7CD331BE315CE22uL, 0x893BD7ABCAE8B6C8uL), (+1, 11, 0xD8E5864333464492uL, 0x4ECF933B26097392uL)),
                    ((-1, 16, 0xD5B52647AEE08CB4uL, 0xE265E7437D3EC783uL), (+1, 16, 0xB59C30277B62F3A4uL, 0xDDC9D72A1FC417FEuL)),
                    ((-1, 21, 0x849253CF3F005C98uL, 0x8AFA938D183B9EDDuL), (+1, 20, 0xD9006A1C1D2E325CuL, 0x93203F7E0AFB648FuL)),
                    ((-1, 24, 0xF94279FADABCD925uL, 0xACE8C2DAD6B2138BuL), (+1, 24, 0xC2990004ACA7ABF7uL, 0x89CD1634041C2517uL)),
                    ((-1, 28, 0xB670A5D95ABE8E25uL, 0xB62F0E027CC8FE4EuL), (+1, 28, 0x86E38D19509369E4uL, 0xCDC5039EB1E61782uL)),
                    ((-1, 31, 0xD0BCAA9AC78F82F1uL, 0x15B3285265F3F78AuL), (+1, 31, 0x92ADA017348570F8uL, 0x60E7E4B3FB89C6DFuL)),
                    ((-1, 34, 0xB64DD31C75A7C67BuL, 0xDFBB605E89D4965EuL), (+1, 33, 0xFB3A80623BF84F89uL, 0x212876159D4E53BBuL)),
                    ((-1, 36, 0xE2406C1ECE44B585uL, 0xB7C1594C5BC127C0uL), (+1, 36, 0xA6FC249613495988uL, 0x05CB04D852EB73BCuL)),
                    ((-1, 38, 0xA684C0E3D0B4D749uL, 0xC743DB34A9CDA7D8uL), (+1, 38, 0xA4E0EFB4D7B338A0uL, 0x5E19C82CA47A6ABDuL)),
                    ((-1, 37, 0xE5EA626CEB4D7957uL, 0x1EBE25B376E94267uL), (+1, 39, 0xD6DA5FA2D204428AuL, 0x7108012C84A6AC26uL)),
                    ((+1, 40, 0x98A93815F4BD94CEuL, 0xB04FC6B3E5156A5AuL), (+1, 40, 0x85506D86DB582C8AuL, 0x1E970D0F71AC249FuL)),
                    ((+1, 39, 0x81D0F49E1E62093BuL, 0xE875ACA1CDA52CC9uL), (-1, 38, 0x96E4D8DCF63C0A52uL, 0xDA67485CC9B782E1uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p078125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x8DF1BCCF7A140D4CuL, 0x1002D0F24B6CE955uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xA2AF1D172B2BF891uL, 0xD4F35DD2B89CF9FBuL), (+1, 6, 0x8EE69011CF3A0E91uL, 0x9BCEA32168BE818AuL)),
                    ((-1, 11, 0xB9FF5A9CD2479511uL, 0x551135B57EE7D5E8uL), (+1, 11, 0x9E63B45A6008B83CuL, 0x7951E128E0648098uL)),
                    ((-1, 16, 0x88FF0500184033DFuL, 0x721312701C37D565uL), (+1, 15, 0xE05BABCA21774076uL, 0xA9C4A8BFE267B74DuL)),
                    ((-1, 20, 0x902FA1E5EEF38B03uL, 0xF59248AA93AD14ECuL), (+1, 19, 0xE1080469D281D069uL, 0xED315AE5B80EA202uL)),
                    ((-1, 23, 0xE32759CD37A55BA0uL, 0xC9C6DFD19AC4A589uL), (+1, 23, 0xA7E05700AF02C79FuL, 0xB1CA22710D6902C1uL)),
                    ((-1, 27, 0x87E63768A52FE811uL, 0x6FC8C75064B0DAD0uL), (+1, 26, 0xBF69E9AEFE4A46C4uL, 0x24E7ADF52C92886AuL)),
                    ((-1, 29, 0xF1E564095F77A5D4uL, 0x9E786D470FB262F4uL), (+1, 29, 0xA822A90A36F16962uL, 0x977BD8CAC777E248uL)),
                    ((-1, 32, 0x950D7358002D576FuL, 0x3A6CB4482E47A10EuL), (+1, 31, 0xE11B3517E8101A64uL, 0xD625698514F7030BuL)),
                    ((-1, 33, 0xD01E66304C1845B6uL, 0x2916CD324C40F859uL), (+1, 33, 0xDA9C6C74B210B7B7uL, 0x0FC3BEC0D0678565uL)),
                    ((-1, 32, 0xAB63F302D41D5814uL, 0xD633E5859D3F46CDuL), (+1, 35, 0x8810F45DB9F74C72uL, 0xBBADED3247BB9E5FuL)),
                    ((+1, 35, 0xDDDCC07C1B880D2EuL, 0x888FDCF4F4574CEAuL), (+1, 35, 0x8E2D9D2EA6C945A0uL, 0x4A64204B38A61A22uL)),
                    ((+1, 32, 0xD78EBA378A709C33uL, 0xC8A98E915429DB95uL), (-1, 34, 0xB451263E98BFEF2AuL, 0xA7B2AB0DD0BC7F5DuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p0859375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x902BA12A890F68E5uL, 0xF9ECC8D0A5593D2EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xA7E6E9FE8A7F524DuL, 0x9551203FAE46A80EuL), (+1, 6, 0x90E876321401E304uL, 0x0B7696822867AD86uL)),
                    ((-1, 11, 0xBEF9EE3504325E5BuL, 0xFF8FFF9AE29DE512uL), (+1, 11, 0x9F4FD8F5AB7A1D83uL, 0x910673AF7430DACBuL)),
                    ((-1, 16, 0x8AA2DF855B1B9F00uL, 0x3AC8544769DE13A7uL), (+1, 15, 0xDDEE4C945BFBA1A6uL, 0x05CE777285D7E763uL)),
                    ((-1, 20, 0x8E716963927A020DuL, 0x5336CA4B1775A433uL), (+1, 19, 0xD971E142EB8FC092uL, 0xA80A5DF72C9CEF8BuL)),
                    ((-1, 23, 0xD8BA82F3F76B14ECuL, 0x91B00381E7FE1B5CuL), (+1, 23, 0x9DC34518725C6F51uL, 0x8C237342A54D6727uL)),
                    ((-1, 26, 0xF6B70DCD26A38DA7uL, 0x1C469FDB2239F480uL), (+1, 26, 0xAE072E9DD6ED709CuL, 0x87C05E89FA80C99CuL)),
                    ((-1, 29, 0xCD0B3FA2150C9B45uL, 0x2B2AC949E23B7E8BuL), (+1, 29, 0x92F524C1D4710EDAuL, 0x368EE0919EAEAD6FuL)),
                    ((-1, 31, 0xE6828A3D7416AE2FuL, 0xF74E02E507AED3DBuL), (+1, 31, 0xBB3384B8176CAA93uL, 0xE2CAFAAC9C18E22AuL)),
                    ((-1, 33, 0x8D91938C61C9DDE0uL, 0xFB741583EF5CDBDEuL), (+1, 33, 0xAAE6E1FAD4FFFEF0uL, 0x64299B85766A25BEuL)),
                    ((-1, 30, 0xC13660230286D0F6uL, 0xBF53218E820B8FB6uL), (+1, 34, 0xC52223CCC2B384A6uL, 0x31B4BAC13AC7CEC7uL)),
                    ((+1, 35, 0x8F59A0A93FCA838EuL, 0xF23F30CEE6A95786uL), (+1, 34, 0xC0E276FFCA7B4D57uL, 0x678D7927A14803B9uL)),
                    ((+1, 32, 0xFD0EB2FA6B03CB0EuL, 0xFF06B6B15EAA7A53uL), (-1, 33, 0xC5AD1507C7C80E2EuL, 0xA54ECD6289E8821CuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p09375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x929FF87C565E2B40uL, 0xA2CDC7E572BF228AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xC4D9578DA178DD9FuL, 0x960F771792486230uL), (+1, 6, 0xA7607C0AB935F37EuL, 0xAAA3438546578041uL)),
                    ((-1, 11, 0xFD69E9F0EDCF2A89uL, 0x7DE3D220E15BE75DuL), (+1, 11, 0xD0DDAE5662D0BF4AuL, 0x3284DB0DCD7F5797uL)),
                    ((-1, 16, 0xCE804B5DA98FF1B1uL, 0x7BE6599AF68412DFuL), (+1, 16, 0xA42EF219023E5171uL, 0xAEAD16CEDCD108F6uL)),
                    ((-1, 20, 0xED220C5A43BD2AD8uL, 0xDEA2E5AE4515213BuL), (+1, 20, 0xB5544DC3548B2481uL, 0x65B3A53008E9B9F0uL)),
                    ((-1, 24, 0xC9C0356D83E2CF7AuL, 0x6E71D3B5298A6E39uL), (+1, 24, 0x94B33EF765282D67uL, 0x894555CB8B6F56CEuL)),
                    ((-1, 28, 0x81AD1C78AAB0A116uL, 0xF48BE8AB69DE4147uL), (+1, 27, 0xBA967E2FB017BE5FuL, 0x91F80140B16626A6uL)),
                    ((-1, 30, 0xFAAE2C5371D620DBuL, 0x72AA7C3BF90BFD9DuL), (+1, 30, 0xB58789F8B422070FuL, 0x1752AB1042FF55DBuL)),
                    ((-1, 33, 0xB06FB653128A0848uL, 0xE39CC06FFFD1F629uL), (+1, 33, 0x88B1A0529123CB37uL, 0xB7AE91EB5C574912uL)),
                    ((-1, 35, 0xA763D228AB409A81uL, 0xFE44A2677156816EuL), (+1, 35, 0x9C253D131BA272A5uL, 0xC3837759398C1254uL)),
                    ((-1, 36, 0xAE3A989AA2AC0B8CuL, 0xCE4D86AA9CEA7F9AuL), (+1, 37, 0x80E38A6DDEF9645BuL, 0x01B668F7BCBA9887uL)),
                    ((-1, 33, 0xDC2AFCDB1D0C57C5uL, 0x7530141109579009uL), (+1, 38, 0x8ACC18EA5CD3E226uL, 0xDBB1688E81F60CD2uL)),
                    ((+1, 38, 0x8FD8FA77E85D492EuL, 0xFCDF34D2A4DE1DCDuL), (+1, 38, 0x973794D527A10EA0uL, 0x25BA14AC297BC3BAuL)),
                    ((+1, 37, 0xFCDD1DA0F7AEE571uL, 0xBE092F1EFD94EC75uL), (+1, 34, 0xB338975936A239F9uL, 0xE60165765E9FB2C5uL)),
                    ((+1, 34, 0xFE7AC3A4C2A3CF5BuL, 0x0CC3A055B2D3D00BuL), (-1, 36, 0x8D6D344E2F6AA302uL, 0x4C0D9FE00F70ED09uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p109375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x980944A194EDB32DuL, 0x0F0D2077747323C7uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xABD1F3C86C15E12AuL, 0xBBE9A4505662429FuL), (+1, 6, 0x8BF1D724DE7D2079uL, 0x2EF54852FE6128C4uL)),
                    ((-1, 11, 0xB881389D4735AFB7uL, 0x559E7F8720426A56uL), (+1, 11, 0x90ED5C416F841665uL, 0x4BAE7BDFF8B66D03uL)),
                    ((-1, 15, 0xF7B17A1A57C36524uL, 0xC273B883E4AF4B98uL), (+1, 15, 0xBB9EEA8A7D86A999uL, 0xE08A6233267B3CAFuL)),
                    ((-1, 19, 0xE5F0C099DB15C00DuL, 0x5AF05EBB79ADE3C8uL), (+1, 19, 0xA90FFAD5DBB10E3BuL, 0x3768A95B50C03C75uL)),
                    ((-1, 23, 0x99BB1E82D2A83095uL, 0xD8575AB6B87987DEuL), (+1, 22, 0xDF75A1F054133E5DuL, 0x38BD05F8A0CD3D9DuL)),
                    ((-1, 26, 0x94D274F37C6EE38DuL, 0x1505A7F89C18A208uL), (+1, 25, 0xDDEEDD0943D8B63CuL, 0x742F21333C2F472EuL)),
                    ((-1, 28, 0xCADD3D4F6F3A6CABuL, 0xE8B2FC996BEE6963uL), (+1, 28, 0xA634830E4AFF849EuL, 0x883C7281E087BE5BuL)),
                    ((-1, 30, 0xB399EB39ECC7B4ACuL, 0x01963DF00E0B14FEuL), (+1, 30, 0xB859D6D04F6DEC9EuL, 0x645F426CA2F8DB73uL)),
                    ((-1, 31, 0xA1D5E9E81FE27D87uL, 0xC66533857B57162EuL), (+1, 32, 0x9041BEE5D30CDF18uL, 0x86E0C8406DC634E0uL)),
                    ((+1, 29, 0xA823FFB339E5945EuL, 0x894964A3FE6EBEBCuL), (+1, 33, 0x8F35AB8AD1FC0AF3uL, 0x734A3610F3CB0D06uL)),
                    ((+1, 33, 0x9A02F0272CA54DEBuL, 0x3AE76259B02B2812uL), (+1, 33, 0x885693C1C977B05CuL, 0xC3CE9483DA23916CuL)),
                    ((+1, 32, 0xD6B2EFD0611815EBuL, 0x548E240B766DC4E5uL), (-1, 28, 0xA12504E3D9E9AB17uL, 0x61EB7FC950884334uL)),
                    ((+1, 29, 0xB9874F259F48C243uL, 0x91856570E9077547uL), (-1, 30, 0xE2E1FB4E8381BB7BuL, 0x2606451D58433BE3uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x9D8F70AB306BE54EuL, 0x8D44E7FD40B5321BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xA11AF14569BB77CFuL, 0x9FC693B323CF5402uL), (+1, 5, 0xFD1EF64B29B1D5A7uL, 0xAEBA545478AA9C9BuL)),
                    ((-1, 11, 0x9B5CFB4FFCA26187uL, 0x4979E822B0AEA997uL), (+1, 10, 0xEC733A53B667007EuL, 0xF70BC5C81A8626B7uL)),
                    ((-1, 15, 0xB97318159EFAB770uL, 0xEFA3442BD2FB0A7FuL), (+1, 15, 0x89B459B894FCBAD3uL, 0x93FF8492F4BB5166uL)),
                    ((-1, 19, 0x9708FFA735465145uL, 0x895BABA8C2B920DCuL), (+1, 18, 0xDE7D8A54AD7912E5uL, 0xCEDA443FA1696EABuL)),
                    ((-1, 22, 0xAE26BB3EEFD7914DuL, 0x46FB1935C141EC72uL), (+1, 22, 0x831AF5730F33D6A6uL, 0x620EB3C367BD62B9uL)),
                    ((-1, 25, 0x8E427006860FF371uL, 0xE0F8894687A1DE31uL), (+1, 24, 0xE648C3B54D8202A8uL, 0xB823B38B9B5994A7uL)),
                    ((-1, 27, 0x9EE9354DBEC3F87AuL, 0xD5388106879D295AuL), (+1, 27, 0x96BD5F8C69E92EE1uL, 0xBBA0E0C11B008942uL)),
                    ((-1, 28, 0xDADA4388EBD7E6B7uL, 0x7BC0E4C335960C5CuL), (+1, 29, 0x8FF4F23DD18F1562uL, 0x73438181A23EB5E0uL)),
                    ((-1, 28, 0xF99504960F94DCE7uL, 0x1FF2E122209092F9uL), (+1, 30, 0xBE6FF9C90B714841uL, 0xA81A39D7C7D57BC6uL)),
                    ((+1, 29, 0xB7B37497E4B6F7F3uL, 0x00A505E95C6648C4uL), (+1, 31, 0x9C0F27BA33797EFEuL, 0x52D676A58551F451uL)),
                    ((+1, 31, 0xA2E833EE98706CD3uL, 0x656E4E2BDA46D190uL), (+1, 30, 0xE966EED10BC0765AuL, 0xF5F2E286F4A26929uL)),
                    ((+1, 30, 0xBB88BA5865640B08uL, 0xFEB129AF98F742D3uL), (-1, 27, 0xEB6F86A9F4D4C89BuL, 0x1EA535C51145CC5BuL)),
                    ((+1, 27, 0x94C9B86A53D123E1uL, 0x7FAB7F130B030DEBuL), (-1, 28, 0xBCEB66B478771316uL, 0x3E802A2FC444DE1AuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p140625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA26B9DFC249FCA8CuL, 0x2E2637DF492AE7A6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 6, 0xA743F6A08605C7FAuL, 0xB8B2111E05696048uL), (+1, 6, 0x806EA2E6A556BD54uL, 0x749FEF0E3FD52FDBuL)),
                    ((-1, 11, 0x9ADAA16D90A6ACD7uL, 0x302F3154E5121C4EuL), (+1, 10, 0xE88ED5FD3A7E2C3CuL, 0x88D1DCA44CBF5BBFuL)),
                    ((-1, 15, 0xAAC39E877D38A476uL, 0xAB342902DC72AA1FuL), (+1, 14, 0xFDE61E6E6A99D45DuL, 0x790F0F0AD8FED849uL)),
                    ((-1, 18, 0xF78BC39B68E4C7ABuL, 0xB2984A159729C32EuL), (+1, 18, 0xBAC4570F030E49A3uL, 0xCE46397E0D34AA5FuL)),
                    ((-1, 21, 0xF34FEA1507BADF1FuL, 0xCBDDE319B48E1486uL), (+1, 21, 0xC2ABABA169975545uL, 0x2FB4BB6796062CA6uL)),
                    ((-1, 24, 0x9F8F001FAFC30E3DuL, 0x0A656CB581263F7BuL), (+1, 24, 0x92331C749A7CB06BuL, 0xD6082C18A0F8CDE5uL)),
                    ((-1, 26, 0x8074F3952766E9EEuL, 0xE55B30154F4780E6uL), (+1, 26, 0x9C9139B167D53DC8uL, 0x2116AB3BA1C2B09FuL)),
                    ((-1, 26, 0xB8C42BFC4B9A4D56uL, 0x7325500CB6668EE7uL), (+1, 27, 0xE4D192216AB3FF6EuL, 0xC281529A16E41C03uL)),
                    ((+1, 26, 0xB21C388F89DEA104uL, 0x0AE42C60DDAC6860uL), (+1, 28, 0xCD73BD49C81C4449uL, 0x65567727D1F55AC3uL)),
                    ((+1, 28, 0xDD44427D1654A124uL, 0x31069761352FECAAuL), (+1, 28, 0xA8D9FEC32087E26CuL, 0x019159A51BD6C90BuL)),
                    ((+1, 28, 0x8687E5B010B62656uL, 0x0EB3AF805304AFC0uL), (-1, 25, 0x8D6D0E62D1DFEC25uL, 0x92C78945599632B4uL)),
                    ((+1, 24, 0xD8DC88A1582C421FuL, 0x39875C7C8460D4B6uL), (-1, 26, 0x8912C3AD6D37AE43uL, 0xE13A6C9C9C51161BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p15625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA5FD96DBDA8AA240uL, 0xB37890CBE8B9A0EDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0xF3A90DD476E06045uL, 0xFB4A455156763FF2uL), (+1, 5, 0xB797D4F48C352159uL, 0xF619DB486D54826BuL)),
                    ((-1, 10, 0xA2D9E44CE2D618DCuL, 0xC4E0C8A3E9B8EFEBuL), (+1, 9, 0xF3E8865D987396D5uL, 0x798672BC0D097815uL)),
                    ((-1, 14, 0x80578A870195A684uL, 0xFBEAB5A69ABF4F5FuL), (+1, 13, 0xC4E16520E127A5A5uL, 0x733A3E410F4E54B8uL)),
                    ((-1, 17, 0x80B53B501EED52EBuL, 0x10A9E6CDE758A489uL), (+1, 16, 0xD4BAB3E6856AFE33uL, 0x1FD4EEE989DAB568uL)),
                    ((-1, 19, 0xA3CB7F58AD6270F0uL, 0xA1B8CA2DD98A71C7uL), (+1, 19, 0x9F3DF584783536F2uL, 0xDDECCDB3581DCBDEuL)),
                    ((-1, 20, 0xF0141F73E9124303uL, 0xFE1205AE9695AAD8uL), (+1, 21, 0xA45860AE75669156uL, 0x5A50BF4ECA6077F2uL)),
                    ((-1, 20, 0xF4F453C5D9FF5144uL, 0x64CAC5706ACAC0CAuL), (+1, 22, 0xDEB388368765145BuL, 0xA3E69458E2EEF6EFuL)),
                    ((+1, 22, 0x9A336DE186770F13uL, 0x92650504261D9E2BuL), (+1, 23, 0xACC1162AEBEAFA83uL, 0x5B8ABF30B8E4D86DuL)),
                    ((+1, 23, 0xD6C0A17D2E46566FuL, 0xB5F884B0465FDB32uL), (+1, 22, 0xB01340A3EE11539CuL, 0x40C79BE769321BD8uL)),
                    ((+1, 21, 0xDD06D9F1152CC8A4uL, 0x333C59F331A39172uL), (-1, 22, 0xAFE7C3C2EBF849E7uL, 0x27C306508DA662A9uL)),
                    ((-1, 21, 0xBA59A20BFDD8234DuL, 0x17ACDDA4FFD50D1DuL), (-1, 21, 0x8C05DA481B29E6AFuL, 0x46E6E292EBA856F8uL)),
                    ((-1, 18, 0xECBC70692B774CB7uL, 0xA76091CE1E831365uL), (+1, 20, 0x82ADC9973FBB75B6uL, 0xAF29262FE8575E79uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p171875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA7EF7A0588C7190EuL, 0x37BA761FF43792FEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0xD9EA9FF09B168728uL, 0xD38BA492230F1E0DuL), (+1, 5, 0xA46DF58C23A27B96uL, 0x4A4F27F9187AACD8uL)),
                    ((-1, 9, 0xFF3F7445B198E756uL, 0xD41BC525AA50EBABuL), (+1, 9, 0xC37588ECAC86A100uL, 0x643BD8AC70DA481AuL)),
                    ((-1, 13, 0xAE3163E1EA4604DEuL, 0xB6FA11D8D4595918uL), (+1, 13, 0x8CC819CB16578B4AuL, 0xF2C1A4EFA002CDC8uL)),
                    ((-1, 16, 0x94E6F25F1C6D9448uL, 0x88692E812984813AuL), (+1, 16, 0x872255F79E8DD7DAuL, 0x083F3DDD34400E87uL)),
                    ((-1, 18, 0x9DBA8840E95D3E10uL, 0x64A8FF5765B7350CuL), (+1, 18, 0xB2BB72E3D411458AuL, 0x71DBF979EB2B42D4uL)),
                    ((-1, 19, 0xB676C2590D04A73BuL, 0x1989E178F0D0EB97uL), (+1, 20, 0xA22357D9B22B38FDuL, 0xFA1B1E98AD696C4FuL)),
                    ((-1, 18, 0xB94A402A431ABF30uL, 0xB729FAC90A26F93BuL), (+1, 21, 0xC129C245A6F159CAuL, 0x7460CDD69D8D0EC1uL)),
                    ((+1, 21, 0x8A5B80626AA33675uL, 0xAB81EE7D877F4007uL), (+1, 22, 0x86F5AADFC044B0E7uL, 0xD2CE535B146F6E89uL)),
                    ((+1, 22, 0x8CF2426F09EC7072uL, 0xE069B1E0E7923245uL), (+1, 21, 0x9B198306DE1EAA56uL, 0xF80D204CEE3836DAuL)),
                    ((+1, 21, 0x83E163A12693123AuL, 0x85E2F735B766BC32uL), (-1, 19, 0xA05252771242DA9EuL, 0x630264B992884C40uL)),
                    ((+1, 17, 0xB8B0D6E42A795911uL, 0xE5B05413CE192F08uL), (-1, 18, 0xF8A5362A101432F8uL, 0x14B34F6A33EB9607uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p1875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA831986764CBF7F1uL, 0x4D47FA89AA591235uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0xC50F7DBAF76A189AuL, 0x13550586B56A91E9uL), (+1, 5, 0x96CF96DAD67E7A25uL, 0xD0E136FDA9196E64uL)),
                    ((-1, 9, 0xCD246E8973F9BCBDuL, 0xFD2E2C5EC1DB73E9uL), (+1, 9, 0xA2E00E75D295EB24uL, 0x408D13947B4534F7uL)),
                    ((-1, 12, 0xF40950398BCE03F1uL, 0x841AD2878CA19B34uL), (+1, 12, 0xD2E55E0A064EF0D9uL, 0xAFD3CB8028481694uL)),
                    ((-1, 15, 0xB0C22C2980FE0F97uL, 0x08F6D8702B87BED2uL), (+1, 15, 0xB398DEFF9BAAE892uL, 0x18869EF4AEAC994FuL)),
                    ((-1, 17, 0x968B27C5FB9EA50DuL, 0x04CBCF57B370B2C2uL), (+1, 17, 0xCF3FB9D4644E4096uL, 0x5C1D64E1AEF5159AuL)),
                    ((-1, 17, 0xECBD4E72286C4462uL, 0x6C67B66FAFF9EE95uL), (+1, 19, 0xA05C6879BCB942B4uL, 0xDBFA6CA35B9196AFuL)),
                    ((+1, 17, 0x8F152297E004C1A4uL, 0x7C45C4392BC0B9FDuL), (+1, 20, 0x9D82C429293DADEEuL, 0xA566DA6E89F6D23AuL)),
                    ((+1, 20, 0x8C16919E7BB80F7AuL, 0x8070CE68F6A9F686uL), (+1, 20, 0xAA5197159FCBA2DDuL, 0xE59887FCB43264A6uL)),
                    ((+1, 20, 0xAAEE501993BCF759uL, 0x82659A8514784167uL), (+1, 18, 0xEF088281E7848FB4uL, 0xE729C492AA38B10DuL)),
                    ((+1, 18, 0xF29CAEF5137B0344uL, 0x6A3803D9803ECDCFuL), (-1, 17, 0xFA240F2191993FEDuL, 0x5E9E2DB07B30BB61uL)),
                    ((+1, 15, 0x8ED70CA8BCE9E3EAuL, 0xDB97B25883178C87uL), (-1, 16, 0xCEAF05B50049BF7FuL, 0x629BD14C7A0CB86AuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p203125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA6E47FC25C50EF5FuL, 0xA7360B98E43700F6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0xB5D5C414D9FE17A8uL, 0x0891A2FEB70941EEuL), (+1, 5, 0x8E8B2AA1EA76203AuL, 0xC7756B10EB011454uL)),
                    ((-1, 9, 0xAE00D487337F6561uL, 0x79434FA5CD793F3EuL), (+1, 9, 0x906BB344FF67E57EuL, 0x6675FB8D3CAEF1D3uL)),
                    ((-1, 12, 0xBBAB51D58A84F618uL, 0xA8AAD44CCA548174uL), (+1, 12, 0xAE0898E62042F384uL, 0x73204A3666C57951uL)),
                    ((-1, 14, 0xF1774BA8F3B4C619uL, 0x222A6E6C4499B2FAuL), (+1, 15, 0x88AD6C57AA120A30uL, 0xF701BB61AB5B6B46uL)),
                    ((-1, 16, 0xAEAD6B00B3106589uL, 0x0E1FDAF558872F7BuL), (+1, 17, 0x8FDB3DFB60358E75uL, 0x0CA8470E66118EEAuL)),
                    ((-1, 16, 0xBB0F1A935A04B4A8uL, 0x6FDAEA72201AE0FCuL), (+1, 18, 0xC812AF7AD3997387uL, 0x05D1AA9C39A3A854uL)),
                    ((+1, 17, 0xBBF29E6558BBF56CuL, 0x33E690C7B4A52C78uL), (+1, 19, 0xAC796903D7180B2FuL, 0x6A41C09601F65FE1uL)),
                    ((+1, 19, 0xA5D4C281A49180B4uL, 0x7AB480F0F74395A5uL), (+1, 19, 0x9B18348FB6065AA5uL, 0xD37E4CAE9B7770E1uL)),
                    ((+1, 19, 0x997A8FC25E806A3DuL, 0x98BD10F2E8ED082CuL), (+1, 16, 0xFEEDBC948C43984FuL, 0xDF0EA6CA05058DC5uL)),
                    ((+1, 17, 0xB076E3E715436E4FuL, 0x13E898C71FC2AAB9uL), (-1, 16, 0xF9459446BF615D94uL, 0x9AD00A1DB97A0881uL)),
                    ((+1, 13, 0xB00819178604AFE6uL, 0x9D508BBF5A632F69uL), (-1, 15, 0x885BBA2D0D9CD19DuL, 0x5F98C56677738990uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p21875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA441E1819FB15E0CuL, 0x572952613EDDE62AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0x88127CD80222661EuL, 0x159842FCC695F8FCuL), (+1, 4, 0xDE199A53A3C3A02CuL, 0xCD0C72E9E97CB78DuL)),
                    ((-1, 8, 0xBCB7328CF68C9DAFuL, 0xDA8FA5A4DA2576EFuL), (+1, 8, 0xAB2B8246DA22279FuL, 0x1AE5239F76492F70uL)),
                    ((-1, 11, 0x86B49148BD5A60F7uL, 0x3236A7DA74CF7DD3uL), (+1, 11, 0x96C691FDD2576102uL, 0x0381A1C3F6192D3EuL)),
                    ((-1, 12, 0xB88499E27887B0FAuL, 0xF20B08CE436A485AuL), (+1, 13, 0xA1E88062DD569EC9uL, 0xBACCEFDC8BF1F520uL)),
                    ((-1, 10, 0xFA56D9C457A1B699uL, 0x8FD8CDEAC55EB36BuL), (+1, 14, 0xCDD99A408433D656uL, 0x03C9680FC97B2319uL)),
                    ((+1, 14, 0xD2825E621E417969uL, 0x32CD6DA8B456C27EuL), (+1, 15, 0x80F11F7C8D48707FuL, 0x424F3CF7BD9398B5uL)),
                    ((+1, 15, 0xB05573AF772E4598uL, 0x25A957A10B013309uL), (+1, 10, 0xFC6638F8A063EB1EuL, 0xA7A7C57B402F838CuL)),
                    ((-1, 12, 0xBE2AFDC94ED4931BuL, 0xF46D1C6068DC58B2uL), (-1, 14, 0xEB5A6CD85B768796uL, 0xBBC35BC59FBE46F4uL)),
                    ((-1, 14, 0x843A2A394F1B4AF7uL, 0xBC30F1AF225CA79AuL), (-1, 11, 0xCF4DCDF4A67457ECuL, 0xAAD96303D4BBFEF3uL)),
                    ((-1, 10, 0xF559B77C55F6D3F7uL, 0x89E629357E2E2037uL), (+1, 12, 0x9858BA8880B0DDB1uL, 0xF09EFA23E09E3717uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p234375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0xA08B680CCD056ABEuL, 0xB85AF7C2DC4FE887uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0x9F33CA1B267A3FF6uL, 0x92A239CD5A7686D9uL), (+1, 5, 0x85931AF56BA1E377uL, 0x2454E2F8E7DF23CAuL)),
                    ((-1, 9, 0x89F146153C888770uL, 0x08A477A7EC8427EAuL), (+1, 8, 0xFCF9CD458FC41D36uL, 0x2D330934D5145510uL)),
                    ((-1, 12, 0x86621FA8231F3F0BuL, 0xB39E48C7C5FD8F83uL), (+1, 12, 0x8E6C47E8E44E4200uL, 0xE23BBA66750AA26FuL)),
                    ((-1, 14, 0x9C34B3F920BFC345uL, 0xCF67722BE038165AuL), (+1, 14, 0xD1C3D867DE036C7BuL, 0x5100E1D44B3750C0uL)),
                    ((-1, 15, 0xCBF85FA9FAD69EB4uL, 0x66D68231D5488590uL), (+1, 16, 0xD123A73EA1BC6464uL, 0x0F10BAD8F09C2A4DuL)),
                    ((-1, 15, 0xB7B36E1CE464A1CEuL, 0x32FBA454676C7FEDuL), (+1, 18, 0x8CE479DF3E3CD491uL, 0x8FF084EDD1B30496uL)),
                    ((+1, 16, 0xFA49474C774ACEF9uL, 0x8EBFC8106383C3BBuL), (+1, 18, 0xF7017A95F4DD31D8uL, 0x412DA7E508384F67uL)),
                    ((+1, 18, 0xD9980AA7761A8BA6uL, 0xCFAA94975352662BuL), (+1, 18, 0xFF02149BDF976EFFuL, 0x224C5BC15EB345F6uL)),
                    ((+1, 19, 0x8236AB4A2B341A99uL, 0x63223CA377E7B857uL), (+1, 17, 0xE3ABBAD783724B50uL, 0xF011806EC5AE186DuL)),
                    ((+1, 18, 0x840D8A049F2576F1uL, 0x0DFFCA16FE21494FuL), (-1, 15, 0x9C7DC60DABD7EE82uL, 0x190DE58943B85127uL)),
                    ((+1, 15, 0xC3DF2C4E087C222AuL, 0x820D3BEA23C72AF9uL), (-1, 15, 0xDE4D02103060ED82uL, 0x6D7EF05F030421B8uL)),
                    ((+1, 11, 0x898B03F136A4AFBDuL, 0xE3C134100B2DEC74uL), (-1, 12, 0xF14A3686B84E54F3uL, 0x5ED0AE6E7FB533DFuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p25Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x9C005DD5492C3901uL, 0x923027E042525FFBuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 5, 0x902E76CB08B9F78CuL, 0xD688CD8F16BEF30DuL), (+1, 4, 0xFCA0DEF6F61D213EuL, 0xDBDF8EA14843E2E7uL)),
                    ((-1, 8, 0xE7078B420C2FBAF5uL, 0x75378DE830C400A0uL), (+1, 8, 0xE12FFF88D3700CB0uL, 0x119FCC54D503F71FuL)),
                    ((-1, 11, 0xCDC56D9E1B24FC17uL, 0x067D1892EAE9E0DAuL), (+1, 11, 0xED7E34BDB04390C9uL, 0x5D8647D532DA82A6uL)),
                    ((-1, 13, 0xD624CB6A674E5CCBuL, 0x14D3DB96B4149054uL), (+1, 14, 0xA2E42AA6F735DC96uL, 0x814E0AFD80E55FD7uL)),
                    ((-1, 14, 0xEB9442CDA143CFCBuL, 0x7B9509C246F82AD1uL), (+1, 16, 0x963C1B9F19FDD55DuL, 0x1370A341AA5572FFuL)),
                    ((-1, 13, 0xAEA9DEFAF1CB009CuL, 0x287B7E07C63196AFuL), (+1, 17, 0xB99163A9E13EDB61uL, 0xE2D1E8CFA4EFE003uL)),
                    ((+1, 16, 0xDBF732BF89B15577uL, 0xBCC6F13306794225uL), (+1, 18, 0x930BF212F29F5E1EuL, 0xFCEEAFC51B291D07uL)),
                    ((+1, 18, 0x89CDA45E5D6404EFuL, 0x65A0CD0ABDB166E5uL), (+1, 18, 0x8555A31B8B92E16AuL, 0xFFB853F4ECC74366uL)),
                    ((+1, 18, 0x8B01D4639FB2DB48uL, 0xF8796B42C08F55A0uL), (+1, 16, 0xB995BD80E30E9FFBuL, 0x674E757858747808uL)),
                    ((+1, 16, 0xF4732AD6034B8990uL, 0x22457C081A702895uL), (-1, 14, 0xF747BA76B5EC2491uL, 0xDBD8B938C1BAFC4EuL)),
                    ((+1, 14, 0xA019E8F1EBC332DDuL, 0xB9053B30B9EB04CCuL), (-1, 14, 0xCB6F8B77E61B0ABCuL, 0x3728C53E26A1CA4BuL)),
                    ((+1, 9, 0xC8DF61AB0FE5F81CuL, 0x6F13A24BB722EB71uL), (-1, 11, 0xB6CDB7846E458D98uL, 0xC3BDB4F820413194uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p28125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x9142B949F61DB3FAuL, 0x7EA91AFF08BEAE29uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 4, 0xC50EF9211C1B2CEAuL, 0xCEE1B776F2F2EA8DuL), (+1, 4, 0xC1E7D5A261F741FCuL, 0xD43419A68C993D6CuL)),
                    ((-1, 7, 0xDBD00AD94142AD4FuL, 0xEDFA311A9A846C9DuL), (+1, 8, 0x81856BA64075EB70uL, 0xBC32FDD9DD21F27FuL)),
                    ((-1, 9, 0xF6D5F0001DCD3F6FuL, 0x680D4D8586F2FF07uL), (+1, 10, 0xC5E1E2AFC9A2B3F8uL, 0xF390544155C248DCuL)),
                    ((-1, 10, 0xF733F06A133D7191uL, 0x18691CF73F82E9E2uL), (+1, 12, 0xBB0B43DA7B2DA140uL, 0x0067C4118390929FuL)),
                    ((+1, 9, 0xBEB22D30384058F1uL, 0xD7B012D697399420uL), (+1, 13, 0xDBBE21F698F1E94AuL, 0x255622EE0F984A2DuL)),
                    ((+1, 13, 0xB47B0DF663726B1BuL, 0x0AFC9FC3EC59411EuL), (+1, 14, 0x95F900D22A3BC8A0uL, 0x64C8B94D5ED36737uL)),
                    ((+1, 14, 0xA00EDA5ACD040942uL, 0x8C1E471AB334F632uL), (+1, 13, 0xB9AFCB91A78F84ACuL, 0xEE4586D5A3287751uL)),
                    ((+1, 13, 0xC82293C0F47E5C1EuL, 0x428A53DBE43C333EuL), (-1, 9, 0xFDE83256984679C9uL, 0x491CF8D623B51B4CuL)),
                    ((+1, 10, 0xE9895CB46DB439B6uL, 0xFE8C59592F3D1D05uL), (-1, 11, 0xD271540C707C42E4uL, 0x2E629FE56DDF9439uL)),
                    ((-1, 8, 0x96B5DC996483584AuL, 0xB8F3C5F796D45474uL), (-1, 8, 0x98AE2B3D657CADD3uL, 0xE9DCF2D7725A098CuL)),
                    ((-1, 5, 0x8BF705BF90D6360FuL, 0x67AB8119E6158876uL), (+1, 6, 0xC792B231FCD02204uL, 0x09CDDA2B3725BE17uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p3125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, 0, 0x855ACE020B41DD6AuL, 0xD9096F5B57DF805CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 4, 0xD6EE3AB2E7AC14CAuL, 0x6540513879E48DCAuL), (+1, 4, 0xE5AE8C4AAA4107EBuL, 0xE8D02DA2F23C3D4AuL)),
                    ((-1, 8, 0x9681B3E75CEAFCDDuL, 0xD5F6F4B6FEBFFFF5uL), (+1, 8, 0xBB050E7FA908D973uL, 0x26F140751577A567uL)),
                    ((-1, 10, 0xEB57D7A7C52576C8uL, 0xAD3B817A226AAE7DuL), (+1, 11, 0xB5DE3221F9A811D7uL, 0xC294496274545768uL)),
                    ((-1, 12, 0xD738514C6DC18FCEuL, 0x2F6FA3027802E826uL), (+1, 13, 0xE9C63FA474FECDCCuL, 0x886F09DF79CC0576uL)),
                    ((-1, 13, 0xC7EDF29D449FD368uL, 0x1F61F8A26171B1CCuL), (+1, 15, 0xCF746FD5E042F23EuL, 0x11B8B4263BFEA97BuL)),
                    ((+1, 11, 0x9C7DB4B91F4AFFCEuL, 0x7812CEAEB74B47F8uL), (+1, 17, 0x80BC57F3DDA41F1BuL, 0x92899936CBD26E52uL)),
                    ((+1, 16, 0x9598479394F16BFAuL, 0xB671EBA5C6615CABuL), (+1, 17, 0xDCCDCD4841A00CD6uL, 0x6FF92BC66A4E916FuL)),
                    ((+1, 17, 0xC1FFA5C718AF3E76uL, 0xF79E817F303E7F45uL), (+1, 17, 0xFA89828C47445FA8uL, 0xBAA6739E4AD534C8uL)),
                    ((+1, 18, 0x81E2F4269E9F5A34uL, 0xFAECA85CAA4B262FuL), (+1, 17, 0xA8177DF0EE4753A4uL, 0x9FC78B552012A173uL)),
                    ((+1, 17, 0xC6D88C1EE07F0ABBuL, 0xE965BB90C82D6660uL), (+1, 15, 0xAAC3F7694772D2B5uL, 0x3044D5BC92D86FEEuL)),
                    ((+1, 16, 0xA90F4D078196FA54uL, 0x2E6932A59DF1DDA3uL), (-1, 14, 0x99E424E8CF820554uL, 0x52FFB59AFB49457DuL)),
                    ((+1, 14, 0x90917F21327A51DBuL, 0x828DE3A3B70BD010uL), (-1, 13, 0xF81413465735F410uL, 0x963923E485B8AEDAuL)),
                    ((+1, 10, 0xCBAA17221D9947B5uL, 0x1CC56E90D15BB7BEuL), (-1, 11, 0xC8A33C9EBAB9E7DDuL, 0x0A869998DED045DCuL)),
                    ((+1, 5, 0x904A71CB28C2B847uL, 0xDD1682ED729364B7uL), (-1, 7, 0x9A4D1358A0FDCDFCuL, 0xBA1AA6C42D0FB719uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xD9F9B843201699C7uL, 0x809338A4CD869D20uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 4, 0x9C8CF1F463ABA0FDuL, 0xAF9FFDBDB97CD9ADuL), (+1, 4, 0xD3F2CD05C5C5844FuL, 0x706DEB5E04C46A67uL)),
                    ((-1, 7, 0xB77D8EACFE388017uL, 0xCC1BF890D45BE615uL), (+1, 8, 0x99591DB29A10FD2FuL, 0xB827E415A9640D19uL)),
                    ((-1, 9, 0xDB1820CB90AD838BuL, 0x0FD8103545DBBCA6uL), (+1, 11, 0x80186C7D4F244D13uL, 0x0FA09A5BD550E63BuL)),
                    ((-1, 10, 0xF2C05C0FE508DA01uL, 0xAB68E9E368224A66uL), (+1, 13, 0x88B8A44BAE1CB0A5uL, 0x21EBB2CEEFA74B93uL)),
                    ((+1, 9, 0xAB6FE6E818E5D1C2uL, 0x565EC625A9FE28C0uL), (+1, 14, 0xC1F6D4C0159CB5FEuL, 0x357356486A6C232FuL)),
                    ((+1, 13, 0xFA6E4D710037917DuL, 0x64DF3EA5B033AAEBuL), (+1, 15, 0xB7536AFF417F4905uL, 0xB85A53215DF18717uL)),
                    ((+1, 15, 0xAD2C2C7F50D1E24DuL, 0x7561035605B1F996uL), (+1, 15, 0xDECF488A6FDACC5CuL, 0xE6C018AEB394D029uL)),
                    ((+1, 15, 0xF3887A2AEDAF8E86uL, 0xC29096C3F2957AB5uL), (+1, 15, 0x9C12255DBA38FE46uL, 0xBF4F39CF5A8A3A4AuL)),
                    ((+1, 15, 0xBFB7090C055620ADuL, 0x5DF715E36C5852AFuL), (+1, 13, 0x9EAE093EACF9D05EuL, 0x7F78AB8648677AAAuL)),
                    ((+1, 14, 0xA44B01F1EBCCC638uL, 0x320CD950F557FA0CuL), (-1, 12, 0x9D1B072E58991676uL, 0x6DEC7787D94835C8uL)),
                    ((+1, 12, 0x8B4B7C791C039AB8uL, 0x86AFE97A31E4E1FAuL), (-1, 11, 0xF48C0A3E6B4411E2uL, 0x87BA76339DC5849FuL)),
                    ((+1, 8, 0xC04FC12D8C022CAEuL, 0x65CA0BEFCE3F1FB7uL), (-1, 9, 0xC074E7180499EB8AuL, 0xA0FADDED1DED82A6uL)),
                    ((+1, 3, 0x845FB4FF1BEDE103uL, 0x480F36A022F2D496uL), (-1, 5, 0x8EC09244E80CA456uL, 0x7196560EA161B0B5uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p4375Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0xABFEE374D776EFB3uL, 0x096AD79E76AA3DCFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 3, 0xC1D39222E6D9B487uL, 0xB233A26BD7458281uL), (+1, 4, 0xB0F20D3A985EB45AuL, 0x538423AD4C210200uL)),
                    ((-1, 6, 0xB58C75C1ED827831uL, 0x1BC5DA65523140DDuL), (+1, 7, 0xDE53CFE79611E827uL, 0x9190733258A95B70uL)),
                    ((-1, 8, 0xA8BE112A3E5D37E1uL, 0x0731BDFA3240CA44uL), (+1, 10, 0xA77B334A0F140F8EuL, 0xD9E59F7A07512B34uL)),
                    ((-1, 8, 0xCA8CF1284033D098uL, 0x74152D0EA2E5289BuL), (+1, 12, 0xA7FEB8EEEAFD7CFAuL, 0xF280D1B3799E2429uL)),
                    ((+1, 10, 0xEF9FBF885FA57A80uL, 0xC90A9CAA226AF5D4uL), (+1, 13, 0xEB718A1D55D07A3FuL, 0x19DFD5DE539E35E1uL)),
                    ((+1, 13, 0xACE5F945282241F3uL, 0xE5B4A99C9BF7AC4CuL), (+1, 14, 0xEAFB4708CAF1D54FuL, 0x69D1119E896DD040uL)),
                    ((+1, 14, 0xE072C36D5022D9D3uL, 0xD43A2AE082C21D01uL), (+1, 15, 0xA69A9B17FA4733C9uL, 0xA3F70D5C8F02A842uL)),
                    ((+1, 15, 0xB5E1CA7A8CBE9AF6uL, 0xF82D7195F141E091uL), (+1, 15, 0xA3370DF4BA0107D8uL, 0x00A67609E7C1CF4FuL)),
                    ((+1, 15, 0xC50A829E8A9B0F67uL, 0x1BD9F4F9D3DE22BEuL), (+1, 14, 0xCC040EEC768210D1uL, 0x35B7DCB28F7E6142uL)),
                    ((+1, 15, 0x904A878F1908EBE8uL, 0x8BBED3B72EF086BFuL), (+1, 12, 0xF2023AB7F85BAB64uL, 0xDFD4DB896A29E174uL)),
                    ((+1, 14, 0x8C88CC13B1E135CFuL, 0x62896087CBA6850EuL), (-1, 10, 0xD685BA5AD1E3CADDuL, 0xFA89BA8CB27E8E35uL)),
                    ((+1, 12, 0xAF18D91EF62637A8uL, 0xC0DE8E4421EC18CBuL), (-1, 11, 0x9EE2078E9BA45A78uL, 0xE8BDACDF0DF87E61uL)),
                    ((+1, 10, 0x8287E3E0AC4A8D7DuL, 0x8386BB8E602163CFuL), (-1, 9, 0xF4003A9BBCC18DFDuL, 0x647AE7BA615533EBuL)),
                    ((+1, 6, 0xD01CEC1636E7E334uL, 0x6187110A597C1497uL), (-1, 7, 0xAB84C46112333162uL, 0xD1F817F207A1A5F6uL)),
                    ((+1, 2, 0x8FA5A3B08A9D0836uL, 0xAB804F40785BD895uL), (-1, 3, 0xC9746ACB2B015D74uL, 0x62CAFBE0748F964FuL)),
                    ((+1, -5, 0xCA45E4FC9A7C3DB0uL, 0x77491265137D3093uL), (-1, -3, 0xFE1D354E2C1FBEECuL, 0x0BEFEABA9A40EF63uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p5Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -1, 0x822F7EF0F63F219BuL, 0x214B912E834B77B5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 2, 0x8AD5EED9638D96D3uL, 0x8F0B8B17F2BE3780uL), (+1, 3, 0xD6A1186CC3CFB549uL, 0x234C4BB7374A9568uL)),
                    ((-1, 3, 0x8219B55F6EF727A9uL, 0x75AF6D34338DB875uL), (+1, 6, 0x9B1444956318ABA2uL, 0x44A2CB1CC8C40FE3uL)),
                    ((+1, 5, 0x9F66E6E3FE287130uL, 0x91417F5CF7234BE3uL), (+1, 7, 0xFA2A42F87070AE0FuL, 0x5CE80FF73C9C19E7uL)),
                    ((+1, 7, 0xF1CBCA8CE29E1E7CuL, 0xB7FB9DA3D50487DBuL), (+1, 8, 0xF2CE3E2078E49C91uL, 0xCE3E622E66BD4CB6uL)),
                    ((+1, 9, 0x8C2E77880007E5F7uL, 0xC56C2AC9DB97A86BuL), (+1, 9, 0x8CB8DAC43511EEEFuL, 0xF18B484F99255390uL)),
                    ((+1, 9, 0xAF37637AD3D61BCCuL, 0xC690772977622C6FuL), (+1, 8, 0xACBCA9117DF5D223uL, 0x5AC28F0F4C2C3A73uL)),
                    ((+1, 8, 0xF73906B09D95EEC6uL, 0xDB6EAFCC9082B333uL), (+1, 5, 0xC016A248F49CA259uL, 0xB52EAFF5CB96EE55uL)),
                    ((+1, 7, 0xBE838B07CE08CE3BuL, 0xEFD5FA5D2CFC35CCuL), (-1, 6, 0x8288FD98A4BDD32CuL, 0xB1B7AF82BB0B9657uL)),
                    ((+1, 5, 0x921182D6C0E77895uL, 0x6ECAB83E5A064335uL), (-1, 5, 0x901853A0B78CDBA8uL, 0x8F89CAC8D40C6C65uL)),
                    ((+1, 1, 0xB7D0E1DB49B20065uL, 0xEE45CE5A93105F24uL), (-1, 2, 0xC207369F996B67D7uL, 0x845CE489C3FD9A8CuL)),
                    ((+1, -5, 0xE81BEFC64DA0ECDBuL, 0xC68ABBF7DC265E25uL), (-1, -3, 0xFF8B3F2B27FFDBDCuL, 0xABA96F3D6CAE8848uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p5625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -2, 0xB8B99B33005F4243uL, 0x7667C9AF83799E0EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, 1, 0xC1803567E6E7692AuL, 0x11429B6ED36EA065uL), (+1, 3, 0xE9EF170AF52DC474uL, 0x6E9F28C859295225uL)),
                    ((-1, 1, 0xC12932F094B04D87uL, 0x232D0910E7877140uL), (+1, 6, 0xBDE454112DCBE097uL, 0x9E5E34996A9023AAuL)),
                    ((+1, 5, 0xF5DBEB1F417C15B0uL, 0x4ED340F48AC6DADDuL), (+1, 8, 0xB38E5A43A1D457DAuL, 0xC7CDCA10945FC8E1uL)),
                    ((+1, 8, 0xB5040F93AAA1766CuL, 0x0ED0545BE4DD276FuL), (+1, 9, 0xD99F0D640CCDE851uL, 0xECB207BD7BAF01D2uL)),
                    ((+1, 10, 0x80156F4DC242F535uL, 0x5B937E8E3B5B6393uL), (+1, 10, 0xAEA9C851D5CAB65FuL, 0xE4751B8C2B4C2108uL)),
                    ((+1, 10, 0xDD61E6CE084F75FFuL, 0x176C8085EE3E38BCuL), (+1, 10, 0xB7ABD4E0B745F2E3uL, 0x78CF43ADCD47CCC3uL)),
                    ((+1, 10, 0xF92B8D2DEF5CEFEEuL, 0xAA1DBBDC954FC777uL), (+1, 9, 0xEA15CACA38288598uL, 0x0FB117263CA61AFEuL)),
                    ((+1, 10, 0xB93B5032524789E7uL, 0x2E10543F7DA3BF25uL), (+1, 7, 0xFACD85CF09D6F697uL, 0xA24E8E4542C1A771uL)),
                    ((+1, 9, 0xB39E95CBC7EE8AE5uL, 0xFABED97CC4272AACuL), (-1, 6, 0xC140D157E03285E6uL, 0x5C76B73A4F597B0EuL)),
                    ((+1, 7, 0xDB50490E64C9291DuL, 0xB8BC466FA543FBACuL), (-1, 6, 0xD97C27FCD2BAAAB3uL, 0x008C02F3CC7D6B80uL)),
                    ((+1, 5, 0x9E3F621E3FCD0D6FuL, 0x7D7507D410CF7CF3uL), (-1, 5, 0x9B97248AB6FD8731uL, 0xA7D82A055A513BF5uL)),
                    ((+1, 1, 0xF1FCC6DCFB4309E7uL, 0x72387956EC392FC3uL), (-1, 2, 0xCE446FDB40E90B60uL, 0xDA785FEC7AB43213uL)),
                    ((+1, -3, 0x9F2A1C8FE3786E7DuL, 0xDE62C50EDA214C3EuL), (-1, -2, 0xE41921BCA0335213uL, 0xAA9D0C9175082C32uL)),
                    ((+1, -10, 0xD45EEA04ADB3C4BDuL, 0x7E4769D5A10E39ABuL), (-1, -7, 0x86F677F6E4939D1AuL, 0x80F7B20950D7C681uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p625Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -3, 0xE79DC84A5E0F017BuL, 0xD280034815D4D64BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((-1, -2, 0xEEEBA32D1E7798A3uL, 0xFEE13716E9848573uL), (+1, 3, 0xB2ED8FE5DCC77C11uL, 0x3576A554C6DB1133uL)),
                    ((+1, 3, 0x9ACB1C77E818BFA9uL, 0x66B80EBE666812B3uL), (+1, 5, 0xD5254989C5C245CDuL, 0x3FD62B0393B1075AuL)),
                    ((+1, 6, 0x822221DC00C50FEBuL, 0xA2932C1A34FCBAAAuL), (+1, 7, 0x8BBE9054AFC54580uL, 0x89492E818F4D9699uL)),
                    ((+1, 7, 0xBAD235F1E2C092E8uL, 0x41A5355B8F646134uL), (+1, 7, 0xD7BE1792E6429C48uL, 0x15043121D456F79FuL)),
                    ((+1, 8, 0x94FE5EE3294F8534uL, 0xA9EC526613827663uL), (+1, 7, 0xBDC838A4B5A2C4A7uL, 0xA7F75A57EFAE01DFuL)),
                    ((+1, 8, 0x8C0A4BAFEBBE1148uL, 0xBAC85DBC7D976A31uL), (+1, 6, 0x93C2B168C6FA6020uL, 0x9F4ABEA846516C76uL)),
                    ((+1, 7, 0x9A08C9F71956A316uL, 0x4F2AD77131153D9BuL), (-1, 3, 0xE8C1EF546286698AuL, 0x8D54C30782A771FBuL)),
                    ((+1, 5, 0xBCB83572DE52BFC1uL, 0xCA67BF13FCB3CC72uL), (-1, 4, 0xCBCDAA90E893DE57uL, 0x599DA45690D44B18uL)),
                    ((+1, 2, 0xE93C011B1CBB3C60uL, 0xCC4899435A0BD372uL), (-1, 3, 0x8D2B3E9F2DF6BCA7uL, 0x9D9748049EB39755uL)),
                    ((+1, -2, 0xEF0CC0C576487AB4uL, 0x87D9270DE24BC9C6uL), (-1, 0, 0x8CCBFE65FA0C4D37uL, 0xE570DEC8A18E45D1uL)),
                    ((+1, -8, 0xF5C4DD83F66AE8C9uL, 0x672C32118FB05CE3uL), (-1, -5, 0x8E2F005720B6E113uL, 0x7B81D7E5DFC2C8F1uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p6875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((-1, -4, 0xD11240A155DBB1FAuL, 0x8444C6FC1D37B80DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -1, 0xD33A51ACBE26F302uL, 0xAFF4E9C2124E7461uL), (+1, 3, 0xAACD092BCFF745FFuL, 0x9ACF41242A4D043AuL)),
                    ((+1, 3, 0xE5528E861BD21BF0uL, 0xE095F3CDC6935792uL), (+1, 5, 0xC6E2CA9FE2A696DEuL, 0xBB7DCE69B2899C12uL)),
                    ((+1, 6, 0x908C6F4AE14A52EBuL, 0xBFE3FEBC10065CFFuL), (+1, 7, 0x83A36D566168CAB4uL, 0x9D39931EF3F98654uL)),
                    ((+1, 7, 0xC21D322CC7C15DA2uL, 0x74234F0715E62A5EuL), (+1, 7, 0xD7558B9E1E1578CFuL, 0xCC5C400A771377B0uL)),
                    ((+1, 8, 0xA013EA8DEDB320D7uL, 0xAE76A7099B2E4046uL), (+1, 7, 0xDAD6A2988841F2E8uL, 0xF326918DD4AF7FD0uL)),
                    ((+1, 8, 0xAACE6EA0F091879EuL, 0x79E3176EA69EA7BAuL), (+1, 6, 0xFB51D93745C8D664uL, 0x8837A935D80B6375uL)),
                    ((+1, 7, 0xEF1F7EEEE0C017A3uL, 0x5BAD9CE0F63AE0F4uL), (+1, 4, 0xA51244A0948C5C97uL, 0x5E306DDEABE97025uL)),
                    ((+1, 6, 0xD94BD414A4406FC4uL, 0x7A1FD31F3C12D5ABuL), (-1, 4, 0xB70AED54EF5DE71BuL, 0x826EF410443C1A97uL)),
                    ((+1, 4, 0xF80400B0B7BB4E9BuL, 0x31E2C0682C9B36F7uL), (-1, 4, 0x8DB60545C0BD488CuL, 0x1980A712F383E48FuL)),
                    ((+1, 2, 0xA7375F71517F298BuL, 0xED2B85E7E1F08DDEuL), (-1, 2, 0xB16A2AD4A2BA0A82uL, 0x7CDD69CC36C25362uL)),
                    ((+1, -2, 0xEF355B6FE9492532uL, 0xDB6C591ED1BC1A2BuL), (-1, -1, 0xD5618FCFFE71CD0FuL, 0x82C3FA10D80F727AuL)),
                    ((+1, -6, 0x937EDB4FC4201AAFuL, 0x831E32B934EDC49CuL), (-1, -5, 0xD8F1A98EA7B3D2CAuL, 0x743102B17152CE60uL)),
                    ((+1, -13, 0xB8C4D43FE48F7E70uL, 0x4A5649D98EE25E59uL), (-1, -11, 0xEDD4C0E89CC8FAA6uL, 0x008714B0C704C70DuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p75Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, -7, 0xE40AA0F8957BB919uL, 0xDB505473651DE1BCuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 0, 0xF80BF938A449EA7CuL, 0x1594602F3901C1FBuL), (+1, 3, 0x9B495C445B7BF72FuL, 0xEE0FFB48DF9CAF80uL)),
                    ((+1, 4, 0x8A6456EC365DE455uL, 0xA7B1CF43BB8D1B3CuL), (+1, 5, 0xA3C6951C7F9DF3D6uL, 0x540BCB0F85A03FDEuL)),
                    ((+1, 6, 0x8A0E30E5AF12A320uL, 0xE70AF8468CECF923uL), (+1, 6, 0xC33989BEFE4DC384uL, 0x174F36A54D27D097uL)),
                    ((+1, 7, 0x9E6080BFC4FE09E4uL, 0x09F09FB79E8C6986uL), (+1, 7, 0x8E4ACD9895059714uL, 0xA8D4638D93317D52uL)),
                    ((+1, 7, 0xE5538F632A8E0E65uL, 0x695FAF030047B3EEuL), (+1, 6, 0xFBB54330C9CE98B2uL, 0x8FBC275EAB3CBC59uL)),
                    ((+1, 7, 0xD995AA1F6283C01DuL, 0xD31076CFA3EF765AuL), (+1, 5, 0xE76EFEE2D460D82BuL, 0xA75E13E618B06159uL)),
                    ((+1, 7, 0x8862409285DB1E00uL, 0x61FDAA5682664048uL), (-1, -2, 0xC5F8C0457D33D971uL, 0x48D1A56DB3303D9CuL)),
                    ((+1, 5, 0xDEF1821EA7928F32uL, 0xD0557D154119769FuL), (-1, 4, 0x84E875A5BBDA726AuL, 0xA2020E9B8D6A1F26uL)),
                    ((+1, 3, 0xE59E1FCA74E5EEFBuL, 0x68244205052C0B1DuL), (-1, 3, 0x9884E87DD173A648uL, 0x1218A14C1ADEF223uL)),
                    ((+1, 1, 0x8C14AA64924F4DB0uL, 0xC8691254E15474D6uL), (-1, 1, 0xA2EDA004FEA542B1uL, 0xCBB01258301F027DuL)),
                    ((+1, -3, 0xB5C0A1B25BEBB4F6uL, 0x2DF361E49E6A9A8AuL), (-1, -2, 0xAC360245D4972FCCuL, 0x8BD2881EDCC61BE1uL)),
                    ((+1, -8, 0xCB94C31759FC7B6BuL, 0x451A678BAE10130CuL), (-1, -6, 0x9B71550257FBD672uL, 0x3A6E54C31E14CD6AuL)),
                    ((+1, -15, 0xE740A656D8B2DF71uL, 0xD2C7011A9D23C694uL), (-1, -12, 0x97CFA15C9B2DBC7FuL, 0xBA796C8C8738DD12uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP0p875Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, -3, 0xEB2655F0DADA09C4uL, 0xC2B666345CDB8568uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 1, 0xCD9540A5B6D7B9F7uL, 0x89385531093D4B0FuL), (+1, 2, 0xD76BF1DC5F1EBACEuL, 0xC358F31F79F3565FuL)),
                    ((+1, 3, 0xF321731414925E2BuL, 0x161924346CA0F503uL), (+1, 4, 0x96958B65B99856B0uL, 0x688AD1928204A870uL)),
                    ((+1, 5, 0x917514E702DD2D23uL, 0x921F2ADBC7192CEDuL), (+1, 4, 0xDCC7E5D4923FD4FBuL, 0x8AC4BB60049C3723uL)),
                    ((+1, 5, 0xC9F2D7219FBE685EuL, 0x7EF9A00D5A2CCB87uL), (+1, 4, 0xA97B055CBB5FF431uL, 0x9E65F028A9EF9206uL)),
                    ((+1, 5, 0xAB2B0D5456751849uL, 0xEC720B0277C9A392uL), (+1, 2, 0xAE4C69DA714CB65CuL, 0x3B99E1A77FF6CDA0uL)),
                    ((+1, 4, 0xB21B8E30E58E63FDuL, 0x7C4FA4DA23876207uL), (-1, 1, 0xFC184B684A2BB1FFuL, 0xAF7DDA40C2FE3031uL)),
                    ((+1, 2, 0xDC27D4A3D09186D2uL, 0x4530EC5572EEBEF2uL), (-1, 1, 0xF4D0AC9E7E7E65E3uL, 0xCC1731915001531AuL)),
                    ((+1, 0, 0x93838B69AF0FF36DuL, 0x39C7A5ADE84520A0uL), (-1, 0, 0xA625CC67BB30ED4FuL, 0xE26730641BF4CD1BuL)),
                    ((+1, -4, 0xA279FE4940043B38uL, 0x5E9D29009578C2FDuL), (-1, -3, 0xBF2D2DD650899C7BuL, 0xE4BD897F019E3441uL)),
                    ((-1, -10, 0xA60D738A5A7DBE67uL, 0x347E805361FC244DuL), (-1, -8, 0xC77BDDF6E5AAA75AuL, 0x29546AB4156878D3uL)),
                    ((-1, -12, 0xA071A896A0F7901DuL, 0x429D430FA2DAB78FuL), (+1, -11, 0xA6AD63FE3EFA07FCuL, 0x1CC126C752C9356AuL)),
                    ((-1, -18, 0xA834BE50CF5650F1uL, 0xD85B9A17313AE497uL), (+1, -16, 0xC8F0D21412F41BBDuL, 0xC148A5DA3D1C238BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP1Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, -2, 0xDE647A5E75C07F67uL, 0xE9E9127646277FCAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0x8386A5A4E9305875uL, 0xF55AB7AA95CEA8A3uL), (+1, 2, 0xB73B9411CEEE5518uL, 0x14282340633314AEuL)),
                    ((+1, 3, 0xF1ED6C99979E7899uL, 0xA7E68C10A1C516C2uL), (+1, 3, 0xD717113B42C4B33FuL, 0x4CD3C7E6DFD4EA9DuL)),
                    ((+1, 4, 0xEC5628DFD2D6AD4DuL, 0x06FCF4585BB79B63uL), (+1, 4, 0x80835174F5DC5D36uL, 0xF7F84B4AFE2BEBC6uL)),
                    ((+1, 5, 0x894A1A3CB33D5594uL, 0xF49499C16358766CuL), (+1, 3, 0x90E3D5FD36CA77CCuL, 0xE7833E16B2DC42F9uL)),
                    ((+1, 4, 0xC6266F084E66CFDFuL, 0x355075A5276D2826uL), (-1, -5, 0x95D6EFCAD1C6DF25uL, 0x6C4E4685AFA2C7F4uL)),
                    ((+1, 3, 0xB2BC45ACAEBBBDDCuL, 0x4682AF5F39D505EBuL), (-1, 1, 0xD10DF11111A9CD6BuL, 0xFF27A0D97798F6ADuL)),
                    ((+1, 1, 0xC511EB76B38CE8F8uL, 0xFD11203D97357428uL), (-1, 1, 0x80D83F8CBEFE5AA4uL, 0xE5B6D8D5FEA007C1uL)),
                    ((+1, -2, 0xFBC3BE174D8D0168uL, 0xD294D7A61EFD71FDuL), (-1, -1, 0x90908004386BABFCuL, 0xBADE24D6EF147F6BuL)),
                    ((+1, -5, 0xA859304590E6A78DuL, 0x37F534E2E333A15BuL), (-1, -4, 0x9E0FB432A9AE9DB0uL, 0x247788B18441A214uL)),
                    ((+1, -10, 0xC06497F772E43635uL, 0x269C6B729B9DB1D3uL), (-1, -8, 0x9215F21180346FABuL, 0xB012A07D600C6641uL)),
                    ((+1, -17, 0xDDDAA4164C38FD51uL, 0x11ED01262EDFF5FBuL), (-1, -14, 0x913E0458179C2FF1uL, 0x643557229E6DD448uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP1p125Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, -1, 0xA37BC1A0D84FCAC7uL, 0x61EA06A95C3B8848uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xC1BF15891E85EAFFuL, 0x76F29BD03506B000uL), (+1, 2, 0xDC5913AB38CBF80AuL, 0xAA54106869F662ACuL)),
                    ((+1, 4, 0xC6CC2EBF8869AB52uL, 0xA2F4D33A011BA7AFuL), (+1, 4, 0xA4CB8FCE544E739BuL, 0x4F586FA39C862A77uL)),
                    ((+1, 5, 0xEBBB754CE23174C4uL, 0x7772F57A46AAB95FuL), (+1, 5, 0x8A31E0CC107BB38FuL, 0x39EE9D81869FD4FAuL)),
                    ((+1, 6, 0xB4D63106EB5B2302uL, 0x1588938A8E634339uL), (+1, 5, 0x8932072856097C09uL, 0xFA7DACC9AC5A99A9uL)),
                    ((+1, 6, 0xBDB6AA6F9C8512C4uL, 0xEB7D520CDA66204BuL), (+1, 4, 0x8F9D95515DE48D84uL, 0x31458661C93A0E2DuL)),
                    ((+1, 6, 0x8BEBC31B823FB212uL, 0x0C11A40B4D2F7B3FuL), (+1, -4, 0xF0C3F68E065298EDuL, 0x0D406466D9A27321uL)),
                    ((+1, 5, 0x92B36A807F9C8FAAuL, 0x4715B9A862495017uL), (-1, 2, 0xE6484DD7A48B7935uL, 0xDDCDFB2DB6A8659AuL)),
                    ((+1, 3, 0xDA4F40E9B028D434uL, 0xDCA4AF8588727205uL), (-1, 2, 0xB67DC6906A140A40uL, 0x2F2CDF7909D7AE77uL)),
                    ((+1, 1, 0xE395F84BE084FAC7uL, 0x0ECC91D4180BA7FDuL), (-1, 1, 0x9AB2AC7873406FD7uL, 0x37AEA3CFB6813855uL)),
                    ((+1, -1, 0xA202BD84DC97C5CEuL, 0x7F0AD42204701743uL), (-1, -1, 0xA2739641F5D5E995uL, 0xBE83D73F959C9205uL)),
                    ((+1, -4, 0x96FF800D3289FA28uL, 0xD90C3ACA469044EEuL), (-1, -4, 0xD5EBAD88481B1119uL, 0x98B5164C7AAC2DBEuL)),
                    ((+1, -8, 0xAC2B2C6F7277EFC8uL, 0xF1CEE66D755B83C4uL), (-1, -7, 0xA94E39AF71E22721uL, 0xD4EE09D45C2D5942uL)),
                    ((+1, -13, 0xD63097E4838DE02FuL, 0x77662922A454CA60uL), (-1, -11, 0x9224BC5869C5FF8FuL, 0xA86C3D595DFF2050uL)),
                    ((+1, -19, 0xE9B853682CDDF685uL, 0xFCAC28AF1A4E9FFBuL), (-1, -17, 0xE20D96863C2DE9D5uL, 0x3635A2B6ABD83879uL)),
                    ((+1, -26, 0x80DD4135C4C7D6B0uL, 0x60D49915891076FAuL), (-1, -24, 0xC13CCFA545F490DFuL, 0x5889485D4404B346uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP1p25Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, -1, 0xDA0B404295CDA56AuL, 0xBDC508B824450545uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xC06ADF0DFB04248CuL, 0x670A30AE38A358EAuL), (+1, 2, 0x9F9547B069E0A7FBuL, 0x482CCE38024ED42FuL)),
                    ((+1, 4, 0x910427104107CC3AuL, 0x6E27200419280F7EuL), (+1, 3, 0xA416A6A054D2DBDFuL, 0x42CC5E8AF38112EDuL)),
                    ((+1, 4, 0xF7BFC53B5CC04EEAuL, 0x4F9707E1F1500F80uL), (+1, 3, 0xAA63CCB82E17488BuL, 0x2BA9527042B6C393uL)),
                    ((+1, 5, 0x8581FA29DD75C3CAuL, 0xB3CDB63461FC5615uL), (+1, 2, 0x951B086DF755E2CEuL, 0xD33DD2B59A7E39FBuL)),
                    ((+1, 4, 0xBEA00DC93330217BuL, 0x626493884A86A0C1uL), (-1, 0, 0xC68935AD5823A75EuL, 0xDD5D13F24A37D188uL)),
                    ((+1, 3, 0xB7DA851F5EBCEF77uL, 0x543B612E141D00E9uL), (-1, 1, 0xD2EF0A5043A5E1DCuL, 0xA42BEFF2968AB72DuL)),
                    ((+1, 1, 0xEF97C18F437F4754uL, 0xBEC382B15A86072FuL), (-1, 1, 0x824FF22986CFB908uL, 0x27DE3E1D89434BD8uL)),
                    ((+1, -1, 0xCF6F61CE6EA5BBE8uL, 0x3CFC295787120D22uL), (-1, -1, 0xB241788A1089A8CEuL, 0x50888B4C19C5150DuL)),
                    ((+1, -4, 0xE63DF219A048532AuL, 0x9E09015125548A5FuL), (-1, -3, 0x919FF0D899FE7451uL, 0x99D3C6C1C17F2A92uL)),
                    ((+1, -7, 0x99D6F523DC3A6988uL, 0x51A05762CC0048E4uL), (-1, -6, 0x8B1B52709D50456AuL, 0x9D201E3AED4BB048uL)),
                    ((+1, -12, 0xDDB8C1C9DAEDDB5AuL, 0x33254690EABFE7E3uL), (-1, -10, 0x8E6E98E1434A1753uL, 0xC88EF377086FBE9AuL)),
                    ((+1, -17, 0x8B2206A1CDE6A7A1uL, 0xA29F98DB4D3C6F61uL), (-1, -15, 0x81409E85706141ABuL, 0xE57A12FBFC7856D2uL)),
                    ((+1, -25, 0xB01B1ABCF442D1A1uL, 0xC1AE06DF4BEF94DBuL), (-1, -22, 0x80F1F0B2C50DB248uL, 0x80FE7E6E19BE21D4uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP1p5Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, 0, 0xAC243CDC346F3F8DuL, 0x097B2F8A8C8DC39FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xDCFCAC291BEEC427uL, 0xB801C582ADA958D3uL), (+1, 1, 0xDDAA3742674CDA8CuL, 0xD1C68F8DB563F158uL)),
                    ((+1, 3, 0xF2C4A83A60A6C426uL, 0x5447555580CD5A31uL), (+1, 2, 0x8E07B0B6591CB272uL, 0x889A95CD8A35E953uL)),
                    ((+1, 4, 0x967A2F35A7753D92uL, 0xD9EDFADEC240BC1BuL), (+1, 0, 0xE7FE045224D654E2uL, 0x5F5350ACBECBA3A5uL)),
                    ((+1, 3, 0xE91708562AAFBD36uL, 0x49BAB05A8D196E99uL), (-1, 0, 0xBFE3FAC08F796D49uL, 0x5DCE40398A7DC125uL)),
                    ((+1, 2, 0xEB8E63FAB7DE1AC0uL, 0x31936535DAB88443uL), (-1, 1, 0x970E518E0F5D9FF7uL, 0xA268D2B5110959D4uL)),
                    ((+1, 1, 0x9D4FA001399CC84FuL, 0xE08F6C0DFAD1935DuL), (-1, 0, 0xB4B5A90949DE68EAuL, 0x36F53712A4EC91FBuL)),
                    ((+1, -1, 0x8995D075B3D71B63uL, 0x8A7AA6B20AAD3603uL), (-1, -2, 0xF3FBBE84AE2C0AD3uL, 0x43B1814C33EE12FBuL)),
                    ((+1, -4, 0x98CD3F1759D9C808uL, 0xA43BC89CC119B7F7uL), (-1, -4, 0xC4F88BA0720A936CuL, 0x2A6F1396C9CA6BB0uL)),
                    ((+1, -8, 0xCB10C76EDD3CBEDCuL, 0x70B97E22C253A5CFuL), (-1, -7, 0xB9B6C3F00AC9B724uL, 0x4E07BDDFFBCF9FD6uL)),
                    ((+1, -12, 0x9102322BC88C13B7uL, 0xFBFC9AE5078D9DC7uL), (-1, -11, 0xBB89155DC5611042uL, 0xB02D956CA2CFD948uL)),
                    ((+1, -18, 0xB411D84DF66C67B0uL, 0x40C93E2E65C83F55uL), (-1, -16, 0xA7E0A9AA1860594CuL, 0x72E2C1E4E46E9658uL)),
                    ((+1, -26, 0xE1922FEB98735A9DuL, 0x5878DC74C5C321C5uL), (-1, -23, 0xA570ECCE55BD2CEEuL, 0x9A9621DDE97A1315uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP1p75Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0x818ADF64C5EC8E28uL, 0x858BABDA2484ABE4uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 2, 0xFF208DF5B5EC9C82uL, 0x837F5D48E393BFC4uL), (+1, 1, 0x92D7B1DFF3C01058uL, 0xF5FFBC2880043633uL)),
                    ((+1, 3, 0xD50B11FC593C0CCAuL, 0x6C5AECFA68EA541CuL), (+1, 0, 0x9F3837ECDFF7FEFCuL, 0xC6E1D108714ED206uL)),
                    ((+1, 3, 0xC5DCFB4419A13D0BuL, 0x17DCD79B67EB84C3uL), (-1, 0, 0xA5378A0269BAE026uL, 0x3FC99A9CF5CC532AuL)),
                    ((+1, 2, 0xE10080B2772A9252uL, 0x30311DE00CA2BDDDuL), (-1, 1, 0x905E4FCF75181F48uL, 0x31F308932E302E81uL)),
                    ((+1, 1, 0xA26ED99997A7387FuL, 0xD16F821DA70EE13CuL), (-1, 0, 0xBA53E73FB7A6FC02uL, 0xDE663E82B72D2D98uL)),
                    ((+1, -1, 0x956B103BC4CB60C3uL, 0x5C68CC53AD6C03DAuL), (-1, -1, 0x844A891D11B85751uL, 0x6FDFD41DCCE5B137uL)),
                    ((+1, -4, 0xAB298F12D54ACB06uL, 0x59CCA092807A9F7BuL), (-1, -4, 0xDC76AB3DCD0BCB9DuL, 0xF5EFE49555A9008BuL)),
                    ((+1, -8, 0xE75B3ACE32BE33EAuL, 0x9C807DE07B72DF2FuL), (-1, -7, 0xD3974429096E58D3uL, 0x81EAFA106A945EBBuL)),
                    ((+1, -12, 0xA663014914B06CA2uL, 0x13DC5CE80560964DuL), (-1, -11, 0xD75273B0AD137206uL, 0xE51E2F11A6D61E69uL)),
                    ((+1, -18, 0xCEB9B6A6E253D2F1uL, 0xAF1E2C0FBAA5844EuL), (-1, -16, 0xC0E6F170C809B4D8uL, 0xFFBE4B979B969A05uL)),
                    ((+1, -25, 0x810CE20169CEB8E2uL, 0xB7CC640E2DF30944uL), (-1, -23, 0xBD72509BCB3E224CuL, 0x7EBCC578F6A90F6CuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeXP2Table
             = new(new (ddouble c, ddouble d)[] {
                    ((+1, 1, 0xC84E6BF652260AECuL, 0xD9678E0AB1884857uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                    ((+1, 3, 0xAA74C8AD5BB92B0EuL, 0x0A0746C85BC0274FuL), (+1, 0, 0xC12B78E047215707uL, 0x9E33D8E4E040113FuL)),
                    ((+1, 3, 0xF7487A4D8A5391D1uL, 0x5CBFCD24FFEE4EA9uL), (-1, -1, 0x9132391BD3D2A012uL, 0x3ADE71DC94691228uL)),
                    ((+1, 3, 0xC845A9204E1C9274uL, 0x889C1A05DA8AAC9BuL), (-1, 1, 0xB5B2007C2C939293uL, 0x06F9C4DF674B0475uL)),
                    ((+1, 2, 0xC735F85CCEBF2D1CuL, 0x76CEFE1930668727uL), (-1, 1, 0xAFE665853845A243uL, 0x39D97C62FD125BEBuL)),
                    ((+1, 0, 0xFC3F1E59752752F8uL, 0x349F9A2F0F740E73uL), (-1, 0, 0xAFC3914C521B6247uL, 0x603CF87E1F4E81C1uL)),
                    ((+1, -2, 0xCBF3C622D81779C2uL, 0x5A78F96E7CBD187AuL), (-1, -2, 0xCED644C0FC578473uL, 0x09323939484652B0uL)),
                    ((+1, -5, 0xCDC4A0A99CC1C874uL, 0x623883B86DC845BEuL), (-1, -4, 0x927A38CC95B1CAFCuL, 0x82443ED843A85B1DuL)),
                    ((+1, -9, 0xF5620E3691655C1BuL, 0xD2F66D6B91CE7F9AuL), (-1, -8, 0xF1CF4222D6BD6EF4uL, 0x05AE7C241B9EFF2BuL)),
                    ((+1, -13, 0x9BE349ED7EBAA518uL, 0x8A1878D1A55B326CuL), (-1, -12, 0xD4F399140D1FF19DuL, 0xD632BF04CA8B2DFEuL)),
                    ((+1, -19, 0xAB211A6120B3D396uL, 0x090025481F3D7122uL), (-1, -17, 0xA5A2A3D07B3E1701uL, 0x5D0233DF318E1517uL)),
                    ((+1, -27, 0xBC6BABDEBDF9B673uL, 0xB217DB9C7D065C7AuL), (-1, -24, 0x8D5874D6620452E2uL, 0xAFB6A2EACBD641B0uL)),
            });
        }
    }
}
