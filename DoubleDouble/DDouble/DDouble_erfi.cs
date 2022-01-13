using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Erfi(ddouble x) {
            if (x.Sign < 0) {
                return -Erfi(-x);
            }
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return PlusZero;
            }
            if (x >= 26.65625) {
                return PositiveInfinity;
            }

            if (x < ErfiPade.PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: false) * Consts.Erfi.C;
            }
            if (x < ErfiPade.PadeApproxMax) {
                return ErfiPade.Value(x, scale: false) * Consts.Erfi.C;
            }
            return ErfiLimit.Value(x, scale: false) * Consts.Erfi.C;
        }

        public static ddouble DawsonF(ddouble x) {
            if (x.Sign < 0) {
                return -DawsonF(-x);
            }
            if (IsNaN(x)) {
                return NaN;
            }
            if (IsZero(x)) {
                return PlusZero;
            }
            if (IsInfinity(x)) {
                return Zero;
            }

            if (x < ErfiPade.PadeApproxMin) {
                return ErfiNearZero.Value(x, scale: true);
            }
            if (x < ErfiPade.PadeApproxMax) {
                return ErfiPade.Value(x, scale: true);
            }
            return ErfiLimit.Value(x, scale: true);
        }

        internal static partial class Consts {
            public static class Erfi {
                public static readonly ddouble C = 2d / Sqrt(PI);
            }
        }

        internal static class ErfiNearZero {
            public static ddouble Value(ddouble x, bool scale, int max_terms = 32) {
                ddouble x2 = x * x;

                ddouble s = 0, u = x;

                for (int k = 0; k <= max_terms; k++) {
                    ddouble ds = u / (2 * k + 1) * TaylorSequence[k];
                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s)) {
                        break;
                    }

                    u *= x2;
                    s = s_next;
                }

                if (scale) {
                    s *= Exp(-x2);
                }

                return s;
            }
        }

        internal static class ErfiLimit {
            public static ddouble Value(ddouble x, bool scale, int max_terms = 32) {
                ddouble v = 1d / x, v2 = v * v;

                ddouble s = 0, ds = v / 2;

                for (int k = 0; k <= max_terms; k++) {
                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s)) {
                        break;
                    }

                    s = s_next;
                    ds *= v2 * (2 * k + 1) / 2;
                }

                if (!scale) {
                    s *= Exp(x * x);
                }

                return s;
            }
        }

        internal static class ErfiPade {
            public static readonly ddouble PadeApproxMin = 0.25d, PadeApproxMax = 16d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> Xp5PadeTables;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> X1PadeTables;

            static ErfiPade() {
                Xp5PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                    PadeX0p5Table,
                    PadeX1Table,
                    PadeX1p5Table,
                    PadeX2Table,
                    PadeX2p5Table,
                    PadeX3Table,
                    PadeX3p5Table,
                });

                X1PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
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
                });
            }

            public static ddouble Value(ddouble x, bool scale) {
                ddouble y;

                if (x < 3.75) {
                    int n = Math.Max(0, (int)Round(x * 2 - 1));
                    ddouble v = x - (n + 1) * 0.5;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = Xp5PadeTables[n];

                    (ddouble sc, ddouble sd) = table[^1];
                    for (int i = table.Count - 2; i >= 0; i--) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    y = sd / sc;
                }
                else {
                    int n = Math.Max(0, (int)Round(x - 4));
                    ddouble v = x - n - 4;

                    ReadOnlyCollection<(ddouble c, ddouble d)> table = X1PadeTables[n];

                    (ddouble sc, ddouble sd) = table[^1];
                    for (int i = table.Count - 2; i >= 0; i--) {
                        (ddouble c, ddouble d) = table[i];

                        sc = sc * v + c;
                        sd = sd * v + d;
                    }

                    y = sd / sc;
                }

                if (!scale) {
                    y *= Exp(x * x);
                }

                return y;
            }

            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX0p5Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 1, 0x96C9C7BD87A5842DuL, 0x93FD89EB5E58C5EAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -1, 0xA27F224DF356021BuL, 0x99350B1DF33C0E0EuL), (+1, 0, 0xD00FA749AA44791EuL, 0x04DFA1851FAFFE7DuL)),
                ((+1, 0, 0xA6388469E24F0648uL, 0x445DFD30721C283EuL), (-1, -1, 0xC2F2F07D1D347564uL, 0x5794968D87337742uL)),
                ((+1, -2, 0x9BDDF53D108ECFD5uL, 0x6A388DEC610F08FEuL), (+1, -4, 0xA38B2A4F4D952D43uL, 0xC33EA604A2E1A68EuL)),
                ((+1, -2, 0xA6B618831D82A069uL, 0x8CDAA838892AC81FuL), (+1, -3, 0xE2281F3BF43029FDuL, 0x82DF4873B4050B38uL)),
                ((+1, -4, 0x865DB6034A1D6BBFuL, 0x8A3E6E666B162C39uL), (+1, -7, 0xACC89B777BB5A95AuL, 0x869FB0976553C7DAuL)),
                ((+1, -5, 0xC661F42C8DAD179BuL, 0xBD6303A71F4C522CuL), (-1, -6, 0xAF857EE079634DECuL, 0x905D0D94E10EAD12uL)),
                ((+1, -7, 0x861DFF096BA99C04uL, 0x65163FDCBE4F6BE0uL), (+1, -8, 0xC3DA2B002487CD73uL, 0x9D44BC8D24E68836uL)),
                ((+1, -8, 0x9830746785224A20uL, 0xE6B4F31845C849EEuL), (+1, -9, 0x825D59CDF46CA2C5uL, 0x6945AAF42676F754uL)),
                ((+1, -11, 0xA4BCAE629CC2FB64uL, 0xED0EED97E8B0FA37uL), (-1, -13, 0xF09FC68D13687554uL, 0x5BB69B7AC64D89C5uL)),
                ((+1, -12, 0x97840916AA933BEBuL, 0xB796B9F6E36A9EBFuL), (-1, -15, 0xEFF34CF95D1366ABuL, 0xC88BAF8B456C1978uL)),
                ((+1, -16, 0xEE45A2BBD0ED5AA4uL, 0x2D8CB95663F4A095uL), (+1, -16, 0xCB17DB442C3B0092uL, 0x77CE7FB26B499551uL)),
                ((+1, -17, 0xB67C16D8AD2944AFuL, 0x3B102E87D34DE17FuL), (+1, -20, 0xB5CFBBEF9A9FB7F7uL, 0xD70803A096923326uL)),
                ((+1, -21, 0xA04EE71F6FCA9E4AuL, 0x7BFB315AC5244492uL), (-1, -22, 0xAC86C975E5D58949uL, 0xEEF7B9EBA9364F45uL)),
                ((+1, -23, 0xCE756E91594959E6uL, 0xC0A77B5B0D8C1551uL), (+1, -28, 0xED45CBBEFD425398uL, 0xD12A478EF869250BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 0, 0xEDE21043F81CA358uL, 0x992D01F8CFF99BD0uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 0, 0x80DB8DA4F79BDAB1uL, 0x33D809B9129C3248uL), (+1, -2, 0xCCDFF5B5826C2C27uL, 0x34ADEC7CA1758A5BuL)),
                ((+1, 0, 0x9AC93A24974BB4A6uL, 0x1D9076A9DBB20E21uL), (-1, -2, 0x91A35C905777CB0CuL, 0x3E568F61CE9BD132uL)),
                ((+1, -1, 0x826A3AA594223279uL, 0xBCCC1547F632E996uL), (+1, -2, 0xC47071BC8A5A34BDuL, 0x12232C5F4ACA6348uL)),
                ((+1, -2, 0xACAAB380C3DCE169uL, 0xD466FE00B5E4CCC2uL), (+1, -5, 0xA8582EB162272BB2uL, 0xAF7EA50CE7884D67uL)),
                ((+1, -4, 0xE918C0B4CFC9B3E8uL, 0xDE42211AA7532175uL), (-1, -6, 0x9C1116DB2EB08460uL, 0x2CC237137384903DuL)),
                ((+1, -5, 0xDA0B6E7C62A2A895uL, 0xA8F3E82991ED20D6uL), (+1, -7, 0xE274E6EE41DFFA1DuL, 0x4D45C255A73E94A0uL)),
                ((+1, -7, 0xECC3E262AE36F969uL, 0x44C0427DE4F2E6F3uL), (+1, -8, 0xA17BFC8AA388AA4DuL, 0xC2896771D18C0ABEuL)),
                ((+1, -8, 0xA9E857772561815CuL, 0xA05AB4A374DCF6CAuL), (-1, -11, 0xC213B7958B99C74AuL, 0x7A690112A9B4697BuL)),
                ((+1, -10, 0x90F7311AF4A5F559uL, 0x3EF61E9066101535uL), (+1, -14, 0xFED54F97E5C74F68uL, 0x8489EA05A8A87BDAuL)),
                ((+1, -12, 0xA3C1136A460746A5uL, 0x7FFA74FD384F627CuL), (+1, -14, 0xAEB8705599C4BDA8uL, 0xB9151B55894E2718uL)),
                ((+1, -15, 0xCC13CB08FA16F3E5uL, 0x4DE159BDCD06BE44uL), (-1, -20, 0xA8DE2643AB8CFFA1uL, 0x50AC9DE3DD8DC28DuL)),
                ((+1, -17, 0xB34F2CC09480B761uL, 0x41829FAA90579561uL), (-1, -21, 0xF208FCD411EA2F7EuL, 0x2119528E56C5C253uL)),
                ((+1, -20, 0x81B6A570AA5317A8uL, 0x0E04B218AB341AD5uL), (+1, -22, 0xAA32CE535ECD46E2uL, 0x99C7269327A19B0AuL)),
                ((+1, -23, 0xA6C693A791B0FA9BuL, 0xA5B394D67399B8A5uL), (-1, -28, 0xC10267E010809730uL, 0xA397A2B43EC15A4EuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX1p5Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 1, 0x95721C1E6742D01DuL, 0xE2198733A7A02DCEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 0, 0xF52AE27A099E5748uL, 0x8182AF76EC8D7C38uL), (+1, -3, 0x9F12EF148A02B1D9uL, 0xDEC0359CDB289C4AuL)),
                ((+1, 0, 0xF5B2F5DFAECB2BB3uL, 0x4FB96EDED618C774uL), (+1, -2, 0x8C4A243516507083uL, 0x92D22EB11D908C2BuL)),
                ((+1, 0, 0x874B25885E2261AFuL, 0x8AB121AA2C67E650uL), (+1, -2, 0xB31B7AA1F583BC3DuL, 0xF17F9DA5173B17A9uL)),
                ((+1, -1, 0x9BE2E17613F43704uL, 0xBC937CF76D43BDE8uL), (-1, -7, 0x9F3CB410F7EC0C91uL, 0xB06F9648BAB20A2CuL)),
                ((+1, -3, 0xFFD932C0B134496FuL, 0x95F7B0A816420494uL), (+1, -5, 0xC177AA50485F828FuL, 0x6A33BA28E556DA25uL)),
                ((+1, -4, 0xD2A9928BDE336608uL, 0x2F5FE0A2E970D232uL), (+1, -6, 0x94B787D75D0364ECuL, 0xB1F021DF51BD0BE6uL)),
                ((+1, -5, 0x858E844FEFA63035uL, 0x9B21ECFBA2B5FF63uL), (+1, -11, 0xB83C8B64CFF2F90BuL, 0x19AB82FEEF749CA3uL)),
                ((+1, -7, 0xA73C228CE3041028uL, 0x4ECE8DE05307359DuL), (+1, -10, 0x88B90F58F8ACBCDCuL, 0xACBC39A41835BA9FuL)),
                ((+1, -9, 0xA30335BA3A4F7738uL, 0xD947749B973A53B9uL), (+1, -12, 0xDCB48779ECA6C70AuL, 0x96EBD01D0073BF4AuL)),
                ((+1, -11, 0x9BD418EA995B3C97uL, 0xABB3E8922199F4F4uL), (+1, -22, 0xCCAE222679D66DC6uL, 0x9AF48085B589C805uL)),
                ((+1, -14, 0xDC91C037FA5A7952uL, 0x814EB6367F88CAFAuL), (+1, -18, 0xEBE71BA122A64174uL, 0xD17677410ADD1AF4uL)),
                ((+1, -16, 0x99369777EB27A327uL, 0xBF6F7075A55995C7uL), (+1, -19, 0x90A2D6BD5A3E9DA4uL, 0x04B9CD473F7C28ABuL)),
                ((+1, -19, 0x8064B1765271F7AEuL, 0x994F8533AD63EE12uL), (-1, -25, 0xD31F3F868D5B0E50uL, 0x85A4F56AD4CAAAE5uL)),
                ((+1, -23, 0xDB91D7DD24A3F491uL, 0x0EB681961CAF5113uL), (+1, -29, 0xC1E337B12B829E53uL, 0xCED66F9999DE1048uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 1, 0xD462688A0805AF1CuL, 0x8447DEDCF5225C7DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 1, 0xEBB3D15F9242CF4AuL, 0x593595600F0A3547uL), (+1, -2, 0xDB49DC3D73D0D0B3uL, 0xB3D15A5C0920CD26uL)),
                ((+1, 1, 0xE44397A7821A40BFuL, 0x9E78FEC75A9DC38FuL), (+1, -1, 0xAE72D59118E94BBEuL, 0xEABA16A6B22C539CuL)),
                ((+1, 1, 0x920BF33F67D976F6uL, 0xBE49F1F76D5F9792uL), (+1, -2, 0xA822DEFE83C79F71uL, 0x3B419F262C5673D3uL)),
                ((+1, 0, 0xA53637A9CA71C360uL, 0x5AC73E17A3B5CEC4uL), (+1, -3, 0x80B573779602D4F3uL, 0xF74A413A0B8BCBE7uL)),
                ((+1, -1, 0x948595FD05258751uL, 0x62F10C327C02C19CuL), (+1, -4, 0xBD1452938F3B16ECuL, 0x10CF3AFD2E1249D6uL)),
                ((+1, -3, 0xEF0E517453F8FE0BuL, 0x7F47C6330873BBE5uL), (+1, -6, 0x978DB0602D651FE3uL, 0x50DB15FA45C098B9uL)),
                ((+1, -4, 0xA0B20D9D0DBCD0ADuL, 0x5389CCFC7A268F3CuL), (+1, -7, 0x80106FFF13E1BF1EuL, 0x819F720F569E2768uL)),
                ((+1, -6, 0xC1B141E1EDB6DBEBuL, 0xE0A20BFE255C4D6CuL), (+1, -9, 0xA5DE8B2C1E4C2A17uL, 0xB7D5CF83EA3AD299uL)),
                ((+1, -8, 0xC3B9D208A452925EuL, 0xF8276A547B50A642uL), (+1, -12, 0xC04FC1CEA0FB9F08uL, 0xA37B9F53894BAB34uL)),
                ((+1, -10, 0xAF2605C69FCF3A21uL, 0x38C9F5C7FD549FF9uL), (+1, -14, 0xC592403F54DE6046uL, 0x57A7BAB7488735E3uL)),
                ((+1, -13, 0xFC9D9D60017082A4uL, 0xC6E85FE1B9187882uL), (+1, -16, 0xA4C269DA0943B283uL, 0x4614B6BD59DCB6FFuL)),
                ((+1, -15, 0x9B5A05199B869766uL, 0xE4BDEDEF8D831D58uL), (+1, -21, 0xCC067E5A0E8C47CFuL, 0x8962829057B7F46AuL)),
                ((+1, -18, 0x8380B07B54748233uL, 0xCD403B76CDA30EC9uL), (+1, -23, 0xE49A5662F9AF1812uL, 0x229A95EC5900CFA0uL)),
                ((+1, -22, 0xA81B438E053FC6DCuL, 0x071988BD9E4F0A4BuL), (+1, -31, 0x83628363556EBEE6uL, 0x617EE859BA982F48uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX2p5Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 2, 0x8F71A3AF4B6FDE14uL, 0x93952FE511E264E3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 2, 0xCAE580BB21E7D4FAuL, 0x9A16FDA2E22E528DuL), (+1, -1, 0xE5A78BA4D5A925C0uL, 0x6D5FDF5533B8F2C0uL)),
                ((+1, 2, 0xCBED77A75DC31E6CuL, 0x9DD9A911372595FFuL), (+1, -1, 0xFBB89A6964ABDA69uL, 0xAC97FBA9BC2A5ED3uL)),
                ((+1, 2, 0x907B3A5597CD03FAuL, 0xE5FB2CDA10ADA6F2uL), (+1, -1, 0x8AE896C623244E18uL, 0x30AF048C68137FACuL)),
                ((+1, 1, 0xA81C43809AB6AECDuL, 0x6A58C4DD8A47FCB8uL), (+1, -2, 0x9F8AC753CF4147D7uL, 0x8460FBA713AF6237uL)),
                ((+1, 0, 0x9FD008CB5B2585B7uL, 0x2177F6EAE45310C1uL), (+1, -3, 0x8CD181DE4CE24173uL, 0x417CFCE0D87C0BB2uL)),
                ((+1, -1, 0x8235B4F7D9EB674EuL, 0x17F6F37AC205E36CuL), (+1, -5, 0xBEE3836B849FF4CAuL, 0x78AD488650DC3E93uL)),
                ((+1, -3, 0xB419C3805E91C718uL, 0xB0DDBC789ED2491AuL), (+1, -6, 0x8BBA58302439E7BFuL, 0xE894D02D1DEC1FB9uL)),
                ((+1, -5, 0xD7883A7AC9117F0EuL, 0xC26591A71281E40DuL), (+1, -8, 0x858354714987171DuL, 0xFDC4D6AB366E226DuL)),
                ((+1, -7, 0xDAF4EF8821CB749EuL, 0x8234CA4F87E49F40uL), (+1, -10, 0x814B5797B26424AFuL, 0x3A63D160414EE607uL)),
                ((+1, -9, 0xBD736576BFF1500DuL, 0x5D4DEFCD2617BDC1uL), (+1, -13, 0xCE5BB266398A8815uL, 0x62414F583A06C17DuL)),
                ((+1, -11, 0x8600E0FA22590E95uL, 0x93A1D486866DCFCCuL), (+1, -16, 0xDB8F3D04811BEECCuL, 0x62DEE8D52F39E9A4uL)),
                ((+1, -14, 0x982A7019AE5A4D3AuL, 0x630CEF3FD4B028E8uL), (+1, -19, 0xDB86F88650BC0B4EuL, 0x23AE526EF3ADBB7BuL)),
                ((+1, -18, 0xF4EC132040098B9EuL, 0x80086280DF399BC2uL), (+1, -22, 0x80A0F7CD9D966BE3uL, 0xFCCE85186A80C5BCuL)),
                ((+1, -22, 0xF929AB1353376555uL, 0xF1D7DEB757E71C89uL), (-1, -30, 0xEE376F53C186C6BEuL, 0x93DB4B1FE4D3026BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 2, 0xB38080AB43349E66uL, 0xBFE77233C93BD546uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0x9AD19636CA9F8513uL, 0xEFA41EE7CDBD73ABuL), (+1, 0, 0xAACE2C6008736E7CuL, 0xF574DBCCC290B1FCuL)),
                ((+1, 3, 0xA6B85DFBAABFDEE5uL, 0x21DEDF2A8E6BC6ABuL), (+1, 0, 0xAD8311DCB4D88C9FuL, 0x29BA55478103C22DuL)),
                ((+1, 2, 0xFFC0B7115E2A96E7uL, 0x2D9745BF90E565B3uL), (+1, -1, 0xE995DABD54B5BEC7uL, 0xF1D90AD816EADE19uL)),
                ((+1, 2, 0x9B59F5CDD2243D3AuL, 0xCFC0B7AC82A6A2DAuL), (+1, -1, 0x85FD5667A53D5AE7uL, 0x0C540390DB906220uL)),
                ((+1, 1, 0x99FE2536B226CE2CuL, 0x5DEBF04D60C0AAB0uL), (+1, -3, 0xF07F60851D572E69uL, 0xCE3E80A16B5898EAuL)),
                ((+1, 0, 0x80277BA5ED133CF9uL, 0x3F39E32D36107072uL), (+1, -4, 0xB9A5EBE9BC155629uL, 0x43B038D39BB38FC5uL)),
                ((+1, -2, 0xB44667C357A9C3AEuL, 0xB7770993D070AE01uL), (+1, -6, 0xF1819426EA3F9971uL, 0x59C39F4C406A7DCBuL)),
                ((+1, -4, 0xD79D51EF55F00DDEuL, 0xBEE606FD64BCB227uL), (+1, -7, 0x80E5CDE4CE01BD06uL, 0x35F2A778B53B6D94uL)),
                ((+1, -6, 0xD991FB53DC44D25DuL, 0x35EE63B7FA3B934EuL), (+1, -10, 0xF0FE17731D544FC5uL, 0x7C835D8754F97A16uL)),
                ((+1, -8, 0xB74488BABE9C4607uL, 0x0345B5373840C1BEuL), (+1, -12, 0xACF1B8CC08FE457CuL, 0x3DAE8F88049C222AuL)),
                ((+1, -11, 0xFA4307F0E3FE5CD9uL, 0x51FFAC2D90375C90uL), (+1, -15, 0xCC15F379C4E25A28uL, 0x19CD243FB81D9588uL)),
                ((+1, -13, 0x84BFA4BD0AA64D68uL, 0x32E173D56B0E7F5CuL), (+1, -18, 0xA922852C863C4F40uL, 0x2AE5B97DC7870CCAuL)),
                ((+1, -17, 0xC54F998783278194uL, 0x3D817EE4866A0A26uL), (+1, -22, 0x9D508E2BF3671B9AuL, 0x9086F9FCBA40A0D8uL)),
                ((+1, -21, 0xA5E809B0AD2B536CuL, 0x15E362C14DB9FF80uL), (+1, -30, 0x8B24C16BCB3B73C6uL, 0xAE83C1C0CCE05FDCuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX3p5Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 2, 0xD5DF749DA9AC903DuL, 0xFACC315FA135C2A8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xE9DBBD01EC42106DuL, 0x568ABC9180DA1988uL), (+1, 0, 0xEF69DF87B2F21DEEuL, 0x1830488E8C5F5082uL)),
                ((+1, 4, 0x8DF1ACC45579B827uL, 0x0BEA6EB171D2A03DuL), (+1, 1, 0x847FCAD4997718F8uL, 0x8DC69C11341CBA3CuL)),
                ((+1, 3, 0xF03B8A4ADE916E64uL, 0xDE17F46D68E24F57uL), (+1, 0, 0xCD000A716880C9B8uL, 0x9BAD42695F06F12EuL)),
                ((+1, 3, 0x9BA110961C1EB70BuL, 0x6E37776C1DC6467EuL), (+1, -1, 0xF572FE642384700AuL, 0x74C150CB4E069FC7uL)),
                ((+1, 2, 0xA14F52F7B0D852C3uL, 0xBF4825FED166AE64uL), (+1, -2, 0xEA6BA60F292508A4uL, 0xA9204B787117DD03uL)),
                ((+1, 1, 0x88D9414F529AE25EuL, 0xDB59688E501DADF6uL), (+1, -3, 0xB70C9B4F8EAF31E4uL, 0x65B651004B2B1188uL)),
                ((+1, -1, 0xBFCD62E9603C656CuL, 0x0E2278DA3B69170AuL), (+1, -5, 0xEA3647A69F7DF5C2uL, 0xDF633A0F50E46F0FuL)),
                ((+1, -3, 0xDDC4CCEC663730DEuL, 0x3DC25F3E7F25A8A7uL), (+1, -7, 0xF3967ECE583A0FCCuL, 0x3DD009EB37857442uL)),
                ((+1, -5, 0xD0FD0ABBE55720B6uL, 0xE4D22F2F8E8936A0uL), (+1, -9, 0xCA93921E399E1AAFuL, 0xBFD9D282C67DFD31uL)),
                ((+1, -7, 0x9C49F699177F9C9EuL, 0x57F3FF20B6D0D445uL), (+1, -12, 0xFECC4D03491A3203uL, 0xD2CA77E0B298DAC7uL)),
                ((+1, -10, 0xB05B58A06E410112uL, 0x992F5CF9F6A11B5DuL), (+1, -15, 0xE0B66047C6EF7F20uL, 0x814D4CD1FC9827AAuL)),
                ((+1, -13, 0x8761F3AC433AFEABuL, 0xB5667E3D9BC673B6uL), (+1, -19, 0xD6AC1E1C647FBD47uL, 0x55EA5D8D1EBC183AuL)),
                ((+1, -18, 0xD910A87CE94BCF86uL, 0x27EBB1D1313E5581uL), (+1, -30, 0xF5110BE126B2ADAAuL, 0xF0D08ACD62272575uL)),
            });


            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX4Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 2, 0xF76506031A9E865CuL, 0x3D34F6BD99E2ED5CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 4, 0x90D625EA94211534uL, 0x9B126401600A0B65uL), (+1, 1, 0x84A9EBF7DD7C237BuL, 0x914DE3F04BF355FBuL)),
                ((+1, 4, 0xBC16818A4F07A081uL, 0x3B371030BA31E8A5uL), (+1, 1, 0x9F2B5F58F443E18BuL, 0x41C6164942213405uL)),
                ((+1, 4, 0xAB1A9BEBF1B196A0uL, 0xD062BEE1B97E4027uL), (+1, 1, 0x869DD025BDB64BC6uL, 0x086D38337CA7146EuL)),
                ((+1, 3, 0xF05CAB5914CA42E5uL, 0xEDE4CAE3A8F1C83EuL), (+1, 0, 0xB11FC2BE9CD5EBA4uL, 0xFF2EB8DAAB6A3CE4uL)),
                ((+1, 3, 0x88BC8B57C2FCBDA6uL, 0x828CA10156F6A25CuL), (+1, -1, 0xBCF3242D21F151E6uL, 0x5D1029869CA1F794uL)),
                ((+1, 2, 0x818DE04AFCC37A60uL, 0xEA45B443A15D6AC5uL), (+1, -2, 0xA7FF0BB5B00CE6B3uL, 0x6833000412916169uL)),
                ((+1, 0, 0xCFA76A0946FECF03uL, 0x7C2A6B617965B992uL), (+1, -4, 0xFBFEF43E15FABA9FuL, 0x83B999BB24D1774AuL)),
                ((+1, -1, 0x8DE6B9371E6B7697uL, 0x12065EF8F55207EEuL), (+1, -5, 0xA091B0B8F8DE6747uL, 0x481E7982707A2A9AuL)),
                ((+1, -3, 0xA5A88FA7C4545960uL, 0x37841A584FD83511uL), (+1, -7, 0xAD7E3BE13F58D1ECuL, 0xA08DCD7D43B51964uL)),
                ((+1, -5, 0xA48EB52E79F18275uL, 0x9DC7E64E42EFE24DuL), (+1, -9, 0x9DDD463767FAF189uL, 0x8045BA23B0747BFEuL)),
                ((+1, -7, 0x89A73D2A0DFDD7F0uL, 0x07BFD4939E948E14uL), (+1, -12, 0xEDE8720F4C3A3905uL, 0x5DD0527891E9DD5AuL)),
                ((+1, -10, 0xBE546FACD3FB21E4uL, 0xBE49D9AD6F986871uL), (+1, -14, 0x9044DBE4C2479CC0uL, 0xF001D1E3298E3F04uL)),
                ((+1, -13, 0xD2A8B8AEAADDD190uL, 0xD3F9DC3FB9101DBDuL), (+1, -17, 0x85EC24C43866BCF3uL, 0x8EEB032C6107371AuL)),
                ((+1, -16, 0xB09E322DEA5C49E9uL, 0x596E02CA86353DB3uL), (+1, -21, 0xABA98F37D9881308uL, 0xC1E0B0E8F3EC76FEuL)),
                ((+1, -20, 0xC96FF68AF4445666uL, 0xB42EDF1E0864A97FuL), (+1, -26, 0xEE40BADAB6041AB4uL, 0x9A5E0988000DBFA5uL)),
                ((+1, -25, 0xEE389C86BFF04A0EuL, 0xC8D8DE2D9284A9D9uL), (-1, -42, 0x8F9E77D5928179EAuL, 0xE318D7CA947C0B68uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX5Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 3, 0x9CA825CBDF550637uL, 0x7FF1985577693398uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 4, 0xDF5EFA64C42FADFDuL, 0x10B4B00FB12DCA98uL), (+1, 1, 0xA9233C22355F31B9uL, 0xA51FB0519BF445DDuL)),
                ((+1, 5, 0xA5DCA17FCBDBC0C2uL, 0xD503DDC14EFC7D07uL), (+1, 1, 0xEBC543AEB0A3210DuL, 0xD82F4CAC0E3369F0uL)),
                ((+1, 5, 0xA72C006F9149476AuL, 0xDA65A570D6381073uL), (+1, 1, 0xE015D8F43ED96CBAuL, 0xAACB2D5DDBB37A5FuL)),
                ((+1, 4, 0xFD929923A08C1860uL, 0x2BD14F3515CDA65AuL), (+1, 1, 0xA0946AA0B59B8601uL, 0x722F136A7EF391C9uL)),
                ((+1, 4, 0x9879EA98CD7E927BuL, 0x825D957D95F3244CuL), (+1, 0, 0xB671897480F71C7BuL, 0x72A467781D601A9CuL)),
                ((+1, 3, 0x95B713974433D31FuL, 0xDA7B95D1162FCEDDuL), (+1, -1, 0xA8ECC63A4108B6D4uL, 0x55A7346D03BF1BDBuL)),
                ((+1, 1, 0xF403DCA8099E648AuL, 0x71F37D319E864592uL), (+1, -2, 0x81541AF3960EAF00uL, 0xFC0A8207C4C3566BuL)),
                ((+1, 0, 0xA6535539851402E3uL, 0x18D2B17F6B71251AuL), (+1, -4, 0xA4A608CBC697C62BuL, 0x8189E502C5FC454DuL)),
                ((+1, -2, 0xBDCF36BB73ACF719uL, 0x93401028E08987ECuL), (+1, -6, 0xADE71924915627D5uL, 0xE1A7D6BB5C31C7B2uL)),
                ((+1, -4, 0xB44B61D19F75427AuL, 0x2189DD5009B80B41uL), (+1, -8, 0x96DA5CD7A3E66890uL, 0xA41F48AFF77EFD4BuL)),
                ((+1, -6, 0x8CAFB821032FF2C8uL, 0x17E0F25508F056A4uL), (+1, -11, 0xD2AAE97F2FECEADCuL, 0xAAEF9E26A89BBD7FuL)),
                ((+1, -9, 0xB04794113C8FB7B5uL, 0x6176F3A8EF867A95uL), (+1, -14, 0xE4AECE3366D43608uL, 0x145E9940473410ACuL)),
                ((+1, -12, 0xAABF13D5D7D9CE10uL, 0x1BACA78B55709B08uL), (+1, -17, 0xB57BA225209BBF9AuL, 0x9EE8587B2891BAD0uL)),
                ((+1, -16, 0xEFC43A0C17341066uL, 0xC3C5CB082BC27DEFuL), (+1, -21, 0xBB171072A9EB85A5uL, 0x71EDA2FE7850B75BuL)),
                ((+1, -20, 0xD8291B6BA28FB471uL, 0xC9C8D204A81CAD40uL), (+1, -26, 0xBA0C0951542A9762uL, 0xD355FA1EF5B90990uL)),
                ((+1, -25, 0xBA0BA210F95B306AuL, 0xDC89EC2EEA3A2CCBuL), (-1, -48, 0xBDF72CF611728B90uL, 0x1550C5C3F7F54D70uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX6Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 3, 0xBD40E58A5B915B9DuL, 0x65AC08F5AD49515EuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 5, 0x92DD6C22133E8232uL, 0x47FDD810F8EEA03DuL), (+1, 1, 0xBBACFB4871E04D57uL, 0x01F641D849F6CC53uL)),
                ((+1, 5, 0xE434B7E48A86F5F1uL, 0xC019C53307D1532FuL), (+1, 2, 0x8A409B3BB53159DFuL, 0x53F9736065D07924uL)),
                ((+1, 5, 0xE8DE4313BB4A8FF0uL, 0x39CDF7D3DAC5CAC8uL), (+1, 2, 0x85CED22A0B4406DDuL, 0xA8741335D5EBF2D7uL)),
                ((+1, 5, 0xAD25A8F93344C1D2uL, 0x6AF0B34F18E5E710uL), (+1, 1, 0xBC63CAB67DF6AC89uL, 0x2B40263D8D5736E4uL)),
                ((+1, 4, 0xC560D25FE9A087BFuL, 0xBF2A9D85A094E9BDuL), (+1, 0, 0xCA85B9CA7A89F6CEuL, 0xCD37FB6A7E9FFF02uL)),
                ((+1, 3, 0xB0FD0C064DB470DFuL, 0x9D000B1E34AC5ADCuL), (+1, -1, 0xAA28316DFAB5A55EuL, 0x41F7DA99DC0E222FuL)),
                ((+1, 1, 0xFC6690343DEED500uL, 0x12CE78176B43ED19uL), (+1, -3, 0xE123A12DEBE6435BuL, 0xB00F0B31DDED6FF3uL)),
                ((+1, 0, 0x8F0E25563055F0EDuL, 0xF6C36DBEAB72EB80uL), (+1, -5, 0xE94BB90FB78E163DuL, 0x58C23FF35A5A9BADuL)),
                ((+1, -3, 0xFF01589346BC18FAuL, 0x4E726F91DB0BB0D7uL), (+1, -7, 0xB9CD70553E78AE6CuL, 0xA4D77F74A9A9A2D7uL)),
                ((+1, -5, 0xAEAA677B8035DB2CuL, 0x4E567EDA084DE795uL), (+1, -10, 0xDB5458992F56E920uL, 0xE593E813C363CA3BuL)),
                ((+1, -8, 0xB0CDBAC50F824A5DuL, 0xD0B7478127E3A0DDuL), (+1, -13, 0xB3D14D093CBF2404uL, 0x7FDD25A6ADC00447uL)),
                ((+1, -12, 0xF79768E96B9BC05EuL, 0xA99F9802B750975CuL), (+1, -17, 0xB52A416A4BE87CA5uL, 0x9E49DDD6F22F3E87uL)),
                ((+1, -16, 0xD477C86AC85EE0D9uL, 0x84B8A43ADE77D1F0uL), (+1, -22, 0xA6F2D026ECD466B0uL, 0x6868641A5CC8B7B7uL)),
                ((+1, -21, 0xA6F2D025710DCF0EuL, 0x340DD15A98C92D15uL), (-1, -55, 0x8E06385F76121E12uL, 0xE5F5587B1A3EC2F3uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX7Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 3, 0xDDAA414D08504BD2uL, 0x706B0A9E194F063DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 5, 0x9E6FFD5E94B9D3B9uL, 0xC4BAE76CA770B802uL), (+1, 1, 0xADA392E2D54E00FCuL, 0xC8C62E9D9FECB406uL)),
                ((+1, 5, 0xD972CC322540788BuL, 0xDCBC18E2AA141FFFuL), (+1, 1, 0xE1CE48CE984A4A66uL, 0x2B7BF01C0E399ECDuL)),
                ((+1, 5, 0xBB909183CA807610uL, 0x4E501A8DC84E9347uL), (+1, 1, 0xB7B449740F387132uL, 0x05088151DE6B7F4CuL)),
                ((+1, 4, 0xE08C24945AE600FEuL, 0xB5FB52BEBB721742uL), (+1, 0, 0xCDCEC2AEF3085829uL, 0xAB9EDE1EB3DEF8E7uL)),
                ((+1, 3, 0xC2DB9E7A87CDFA91uL, 0xD239BAEC4C641670uL), (+1, -1, 0xA51E4AC399F65C64uL, 0x173426734CAF444DuL)),
                ((+1, 1, 0xF926F5BFAB11D1D0uL, 0x2B246188ABCC539AuL), (+1, -3, 0xBFA9D58C9A3BBCF5uL, 0x073AD757631B9416uL)),
                ((+1, -1, 0xEA56FF812A7765E6uL, 0x19A7AA514B3E8E92uL), (+1, -5, 0x9F33D4C1A23ED663uL, 0x47705A6CAD8E221DuL)),
                ((+1, -3, 0x9F7DF8962A0D3B57uL, 0x710DBE465C58E3B8uL), (+1, -8, 0xB772E9F2E45FA445uL, 0x7271D113827B84ECuL)),
                ((+1, -6, 0x98049BBB860D13A0uL, 0x18BE756CB48F53D8uL), (+1, -11, 0x8A43E2CB21A618C4uL, 0x83302A6261444039uL)),
                ((+1, -10, 0xBF6E7298E572BC03uL, 0x12B88C7D32444F8FuL), (+1, -16, 0xF3778618D657293EuL, 0xC524AC4EBA26D61FuL)),
                ((+1, -14, 0x8E5E9AB089E65D66uL, 0xE9462291E73C02A8uL), (+1, -21, 0xBCAC45E46E310947uL, 0x81D227120921C2F1uL)),
                ((+1, -20, 0xBCAC45E6C9B31FCFuL, 0x8B19496D84CF695EuL), (+1, -57, 0xAE10CCDBEA6EA89CuL, 0xADF81ECF649B075EuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX8Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 3, 0xFDF7AAFED198363DuL, 0xB1BFA2B7D74DF39CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 5, 0x8736C80E8B2D2F6BuL, 0x93D6E9D8C02DA010uL), (+1, 1, 0x802A7B48B3A96989uL, 0xD57E90960B16A554uL)),
                ((+1, 5, 0x80E196DDD943AE10uL, 0x9621E8FFE5711DCCuL), (+1, 0, 0xE347A19E0A239809uL, 0x72DB7B0203483B7AuL)),
                ((+1, 4, 0x8F236A1BE9CEE63DuL, 0x4311776A7239079FuL), (+1, -1, 0xE6E35FB4BFA272DAuL, 0x21244F4BA8C17EA9uL)),
                ((+1, 2, 0xCBC4AF5770467AA0uL, 0x71C7FD5537E26439uL), (+1, -2, 0x92D8068BBFBCA18AuL, 0x86750EDCB4751A26uL)),
                ((+1, 0, 0xC1AA2360BAA6EEE6uL, 0x68C9059FD513B648uL), (+1, -5, 0xF1958B6FEB73E93AuL, 0x2D261D0372E2E485uL)),
                ((+1, -3, 0xF8B1F784731EDE5CuL, 0x12A0B509C29F550BuL), (+1, -7, 0x805BF05F2CA6AC8EuL, 0x9DFF4A826E12AEC6uL)),
                ((+1, -6, 0xD521CAED59A3A9C6uL, 0x4847DFA86C5E9FB4uL), (+1, -11, 0xA9F67750704BFE24uL, 0x72885DC987F3E84DuL)),
                ((+1, -10, 0xE990556B8D22DEEAuL, 0xFDD448270A234DC0uL), (+1, -16, 0xFEB9E36328026125uL, 0x2FBE6B49E6E84731uL)),
                ((+1, -14, 0x93F7AF6B08E5CD65uL, 0x2472B86969F7E1ECuL), (+1, -21, 0xA4D5EDCBE9D8DFA0uL, 0xD0A90BE33FFC928FuL)),
                ((+1, -20, 0xA4D5EDCBFCF54534uL, 0x9728AA10EFEDB4B3uL), (+1, -62, 0x9739FA1855D5C26DuL, 0x54E60C2132F04FFBuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX9Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0x8F198B507B0FC120uL, 0x5B9F162476B00CBCuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 4, 0x9EA0B85453A6F3CCuL, 0xF7836526789DBF71uL), (+1, -1, 0xFEF8D9CEC7331ABFuL, 0x5AC6AB63EDE32AF5uL)),
                ((+1, 3, 0x97F14DA74C692A6EuL, 0xD2E7105924104EA5uL), (+1, -2, 0xD67AB03C4444C9ACuL, 0xCEF57D5DC1B377C8uL)),
                ((+1, 1, 0xA45860B2B9C51A46uL, 0xDD82F074AE24162CuL), (+1, -4, 0xC59E0B9B36CE2968uL, 0x0FCC427CBE5B4255uL)),
                ((+1, -2, 0xDBA7D2FE2C8DDCD7uL, 0x80B3A510125AF706uL), (+1, -7, 0xD77C5EA10BBA7B7DuL, 0x60FFE4189E029676uL)),
                ((+1, -5, 0xB9D29148726F5085uL, 0x9D13D5824589EA72uL), (+1, -10, 0x8B17ACC8286DC125uL, 0x6C8FBD986439CE36uL)),
                ((+1, -9, 0xC26A9BBF56090560uL, 0x58E0D48CBD9AC439uL), (+1, -15, 0xC4E9853A16AEA70DuL, 0xC34C58F83835E2A8uL)),
                ((+1, -14, 0xE61627DD1C845BF0uL, 0x14C2B64DAE019C9CuL), (+1, -21, 0xEBE812C045D6BAFAuL, 0xF8C21D4285DEB999uL)),
                ((+1, -20, 0xEBE812C04E43933CuL, 0x396DE8A02D8CE378uL), (+1, -64, 0xF684FFB4CCEC67D2uL, 0xC484671BEC433DF0uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX10Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0x9F31194B966C0E7BuL, 0x44D6499D4FE53378uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 4, 0x95517B1E923250ACuL, 0xE1FB6E22A54CC655uL), (+1, -1, 0xD64270F1E3750CB4uL, 0x491A3AD43CE86194uL)),
                ((+1, 2, 0xF31A5B20B93E359DuL, 0xCC68C8F5F3AFD8DCuL), (+1, -2, 0x98349F97EA077D64uL, 0x12D4887BE888F341uL)),
                ((+1, 0, 0xE05F90C3AFB7190BuL, 0x1495F9299AEDF9CBuL), (+1, -5, 0xEDF7760B2E5A6838uL, 0x5AB9A1A78466D996uL)),
                ((+1, -2, 0x806DD5E9D52FB238uL, 0xB4B74FDDDF64889CuL), (+1, -8, 0xDD1EC1F82094D356uL, 0x13164F49F15646A7uL)),
                ((+1, -6, 0xBAC5844C402EEE3BuL, 0x5B8F9D11B521E813uL), (+1, -12, 0xF44416E3A6FC0B61uL, 0xD8E5B432BD456C84uL)),
                ((+1, -10, 0xA8835288A4BFD398uL, 0x13D1715679128D83uL), (+1, -16, 0x94892D7E5DECEA5CuL, 0x2A9D22EF2945DB13uL)),
                ((+1, -15, 0xAC82FF2D4A2CB9E5uL, 0x1B195C50296DF671uL), (+1, -22, 0x99720AC5E9DED269uL, 0x54A0CD5B7DD28193uL)),
                ((+1, -21, 0x99720AC5EA670F6DuL, 0x69BCECC99D0F6513uL), (+1, -69, 0xD7B0292EF76C0BB0uL, 0xE178A656D5F9B1AAuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX11Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xAF443F26C8A0BAF3uL, 0x2CCBEC8FCE5B5113uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xF78E1417DF944BDEuL, 0xD42C4DF8460F5583uL), (+1, -1, 0x9D52FFE841D2B40EuL, 0xD84E10ABD52C0359uL)),
                ((+1, 2, 0x95029730EE2978D9uL, 0xA3D7D701220E4320uL), (+1, -3, 0x9FFE85A0FB8244ECuL, 0x0DC05B100BEEFCF3uL)),
                ((+1, -1, 0xC6375804C279EC63uL, 0x7E749A5917FF72B6uL), (+1, -6, 0xAC554C32E3BB111BuL, 0x12F0133425D7881AuL)),
                ((+1, -4, 0x9D57CC7C01548679uL, 0x4CFDD703C749B4D7uL), (+1, -10, 0xCF5DAB1194DAF740uL, 0xA9FEE4FE64C3FA9FuL)),
                ((+1, -8, 0x9513B56949606163uL, 0x78B4B90314847D5CuL), (+1, -14, 0x842786E2B9E25BF4uL, 0xF9DEB908D528BAD6uL)),
                ((+1, -13, 0x9C1D5FDB56DFDBDDuL, 0xFDF9C230F68A27DEuL), (+1, -20, 0x8B67D71ADBA1DCD5uL, 0x08494BF77E08C837uL)),
                ((+1, -19, 0x8B67D71ADD0243EEuL, 0x57B559C248639677uL), (+1, -65, 0x89BDCDB26C50FF05uL, 0x2F66ED868549B894uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX12Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xBF542084B71356B0uL, 0x0C1EC4D671B72B24uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xF2E70A11E29E368FuL, 0xC0C7CCEFB7D6A855uL), (+1, -1, 0x8D04E4858871BE0AuL, 0xC81B9D6F7F56D6D5uL)),
                ((+1, 2, 0x8392D8F46391E425uL, 0x3D7F435135C4AA74uL), (+1, -3, 0x80BBFCF18896ED48uL, 0xDA23020CBA7030DFuL)),
                ((+1, -1, 0x9DAE8DA43746E7ADuL, 0xB9EB76E0CAFB3D9AuL), (+1, -7, 0xF94B39834A2B13D0uL, 0x46E58DD1B35B228EuL)),
                ((+1, -5, 0xE1C85806BD052C59uL, 0x5BDEE7899C6F86BEuL), (+1, -10, 0x870373938D5F5D0EuL, 0xF6190851FE95FDD9uL)),
                ((+1, -9, 0xC125E956FEA75385uL, 0xE80F3367CE2300F1uL), (+1, -15, 0x9B1F2D2BC0280369uL, 0x21A071A145F8A967uL)),
                ((+1, -14, 0xB6D0B410C72C1E97uL, 0xFA174879F451C5EEuL), (+1, -21, 0x93B2CF7026954C76uL, 0xF2CBB7DB643EABD9uL)),
                ((+1, -20, 0x93B2CF7026F0B9ABuL, 0x7C709DE02D4C1436uL), (+1, -68, 0x814D4A8BD4D071F6uL, 0x94BB38F1CB0CBE4BuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX13Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xCF6183E7F00C2EE0uL, 0xE38EE498D5B39228uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xEF942495CD0C2179uL, 0xD01AA401CD596601uL), (+1, -1, 0x8010044B06282294uL, 0xDD01E903C4A3E22DuL)),
                ((+1, 1, 0xEC5FBD6E1237A59AuL, 0x8442DB85C174CA1BuL), (+1, -4, 0xD48BE60B49513423uL, 0x34D6426AAE8943BBuL)),
                ((+1, -1, 0x8118C60287E6B276uL, 0x8437378E59F7D0D9uL), (+1, -7, 0xBB447C47F85F20BAuL, 0xFF4478CF542750B0uL)),
                ((+1, -5, 0xA89D7558468A6DBDuL, 0x147733BE73759951uL), (+1, -11, 0xB8C2CD77086E753EuL, 0x9F9C5580D5AFB4B7uL)),
                ((+1, -9, 0x83AC83AD403F47BBuL, 0x8E4BC1B0EE0104FFuL), (+1, -16, 0xC18BA968A299CBFDuL, 0x35BE126078A3E579uL)),
                ((+1, -15, 0xE3B5324968086BA4uL, 0x91C9781F4039ABE2uL), (+1, -22, 0xA82EF0A1563358AEuL, 0x5A6CD36E3398583DuL)),
                ((+1, -21, 0xA82EF0A15650E77DuL, 0x24DC4B2F5BECC761uL), (+1, -71, 0x98D95870579B4970uL, 0x0B0CA88BC5221273uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX14Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xDF6CF54E4C085E00uL, 0x790A8DB3F82013ABuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xC9587BDB0EEA0228uL, 0x4BEFC074B5320C52uL), (+1, -2, 0xC1F0BF98FE71286DuL, 0xA1037D5481115F16uL)),
                ((+1, 1, 0x96C2C54EE06C6CA8uL, 0xD78922B3A0E86B25uL), (+1, -5, 0xEA242D66F709EA7CuL, 0xA55C307D795E9313uL)),
                ((+1, -3, 0xF01E0219312A9B58uL, 0x0CC0A29D9F581115uL), (+1, -8, 0x8CC605FD64A848E1uL, 0xC0A6F6D93F2FA153uL)),
                ((+1, -7, 0xD67F4D9509F7B641uL, 0xFF88EAAE854C5FEDuL), (+1, -13, 0xA899E936D39ABE02uL, 0xC6BED49679D996CAuL)),
                ((+1, -12, 0xCBCC726BE98153A2uL, 0xC53E17C25F5866E1uL), (+1, -19, 0xA0E70584FAA3D6FBuL, 0xD065983081615F28uL)),
                ((+1, -18, 0xA0E70584FC1DA989uL, 0x9B1B34F2903731B6uL), (+1, -65, 0xFFB663A59D9CA483uL, 0x384133668EB0DFCFuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX15Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xEF76DA5DF51CF6B7uL, 0xA60115B3DDD8E91BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xC802A3D53477F165uL, 0x2D4DE89D1EFE1821uL), (+1, -2, 0xB388EDB4D717AA52uL, 0x49ECE7859C41CC3CuL)),
                ((+1, 1, 0x8ADD22988983A46AuL, 0x21F920D6AA9680B7uL), (+1, -5, 0xC8C3D06A849FB56CuL, 0xD3F8CCBEE4122C16uL)),
                ((+1, -3, 0xCD29BD6D95D11E7DuL, 0xAD7AFA00238DA1EAuL), (+1, -9, 0xDFBCB029F8D30C77uL, 0xAF9A22EBC899744AuL)),
                ((+1, -7, 0xAA149EC4E4881139uL, 0xD746C0D8E82AE1B7uL), (+1, -14, 0xF87C61AAC6EBF7BEuL, 0xB3772028D394D9F3uL)),
                ((+1, -12, 0x9606F4490D99158EuL, 0xE4CEF5328B45FFD7uL), (+1, -20, 0xDC0683DB0391E0A8uL, 0xD7641BE9DC25401EuL)),
                ((+1, -19, 0xDC0683DB04669721uL, 0x062C1F35CE61F8DFuL), (+1, -66, 0x85B51F62883E297EuL, 0x46F18EC53B5D289EuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeX16Table
                = new(new (ddouble c, ddouble d)[] {
                ((+1, 4, 0xFF7F7EBB496CFE6EuL, 0xA33C56823C3B2754uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xC6F32ACBEBEC1EDBuL, 0x4699E763548355B1uL), (+1, -2, 0xA736E9E4463B6CE6uL, 0x33EDA9292997A3F1uL)),
                ((+1, 1, 0x80D1218BAD9ACFE7uL, 0x70ABBECA6AEC9108uL), (+1, -5, 0xAE3C282FB30B2870uL, 0x0AF93E968D1B5128uL)),
                ((+1, -3, 0xB18CE2EBD8EBDF55uL, 0xA6FB6E1D94827292uL), (+1, -9, 0xB502CEB358F131CEuL, 0x369EFBFFA5FA2467uL)),
                ((+1, -7, 0x895C17638619B19EuL, 0x2E8BD6A70780056CuL), (+1, -14, 0xBB7E1D1F427110FFuL, 0x1936332608AC8BD3uL)),
                ((+1, -13, 0xE2380CD813C44333uL, 0x1C070FFB5C0C271AuL), (+1, -20, 0x9AE7BEE3460BCCDFuL, 0x2E33B49D099327A0uL)),
                ((+1, -19, 0x9AE7BEE3464D9D69uL, 0xFD99D586F9E2FE1EuL), (+1, -68, 0x9A867AEDA70C88A3uL, 0x9B97362FFD294697uL)),
            });
        }
    }
}