using DoubleDouble;
using System;
using System.Collections.ObjectModel;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class IncompleteGammaPrototype {

        public static ddouble LowerIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return PositiveInfinity;
            }

            ddouble g = Gamma(s);
            if (IsInfinity(g)) {
                return NaN;
            }

            ddouble f = 1;

            for (int n = m; n >= 0; n--) {
                f = s + (2 * n) - (f * (s + n) * x) / (((n + 1) * x) + f * (s + (2 * n + 1)));
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static ddouble UpperIncompleteGamma(ddouble s, ddouble x, int m) {
            if (s < 0) {
                throw new ArgumentOutOfRangeException(nameof(s));
            }
            if (x < 0) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (IsNaN(s) || IsNaN(x)) {
                return NaN;
            }

            if (s < Math.ScaleB(1, -100)) {
                return -Ei(-x);
            }

            ddouble f = 1;

            for (int n = m; n >= 1; n--) {
                f = x + f * (n - s) / (f + n);
            }

            ddouble y = Pow(x, s) * Exp(-x) / f;

            return y;
        }

        public static (ddouble y, int m) LowerIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = LowerIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = LowerIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static (ddouble y, int m) UpperIncompleteGammaConvergence(ddouble s, ddouble x, int max_terms = 1024, int convchecks = 4) {
            ddouble prev_y = UpperIncompleteGamma(s, x, m: 1);

            for (int m = 2, convtimes = 0; m <= max_terms; m++) {
                ddouble y = UpperIncompleteGamma(s, x, m);

                if (ddouble.Abs(y / prev_y - 1) < 1e-29) {
                    convtimes++;
                }
                else {
                    convtimes = 0;
                }
                if (convtimes >= convchecks) {
                    return (y, m - convchecks);
                }

                prev_y = y;
            }

            return (NaN, int.MaxValue);
        }

        public static ddouble UpperIncompleteGammaNearZero(ddouble nu, ddouble x) {
            ddouble a = UpperIncompleteGammaYoshidaPade.A1(nu);
            ddouble a0 = (1 + nu) * a - 1;
            ddouble phi = UpperIncompleteGammaYoshidaPade.Phi(nu, x);
            ddouble g0 = Gamma(1 + nu), g = g0 * (1 + nu);

            ddouble s = a0 + phi / g0 + x * (a + phi / g);

            ddouble u = x * x;

            for (int k = 2; k < TaylorSequence.Count; k++) {
                a = 1d / (k + nu) * (a + TaylorSequence[k]);
                g *= k + nu;

                ddouble ds = u * (a + phi / g);
                ddouble s_next = s + ds;

                if (s == s_next) {
                    break;
                }

                u *= x;
                s = s_next;
            }

            ddouble y = g0 * Exp(-x) * s;

            return y;
        }

        internal static class UpperIncompleteGammaYoshidaPade {
            public static readonly ddouble TableBin = 0.125d;
            public static readonly ReadOnlyCollection<ReadOnlyCollection<(ddouble c, ddouble d)>> A1PadeTables;

            static UpperIncompleteGammaYoshidaPade() {
                A1PadeTables = Array.AsReadOnly(new ReadOnlyCollection<(ddouble c, ddouble d)>[] {
                    PadeNu0p000Table,
                    PadeNu0p125Table,
                    PadeNu0p250Table,
                    PadeNu0p375Table,
                    PadeNu0p500Table,
                    PadeNu0p625Table,
                    PadeNu0p750Table,
                    PadeNu0p875Table,
                    PadeNu1p000Table
                });
            }

            public static ddouble A1(ddouble nu) {
                int table_index = (int)Round(nu * (A1PadeTables.Count - 1));
                ddouble w = nu - (TableBin * table_index);
                ReadOnlyCollection<(ddouble c, ddouble d)> table = A1PadeTables[table_index];

                (ddouble sc, ddouble sd) = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    (ddouble c, ddouble d) = table[i];

                    sc = sc * w + c;
                    sd = sd * w + d;
                }

                ddouble y = sc / sd;

                return y;
            }

            public static ddouble Phi(ddouble nu, ddouble x) {
                ddouble x_nu = Pow(x, nu);

                if (x_nu <= 0.5d || x_nu >= 2d) {
                    return (1 - x_nu) / nu;
                }

                ddouble logx = Log(x);
                ddouble v = nu * logx, s = 1, u = v;

                for (int k = 2; k < TaylorSequence.Count; k++) {
                    ddouble s_next = s + u * TaylorSequence[k];

                    if (s == s_next) {
                        break;
                    }

                    u *= v;
                    s = s_next;
                }

                ddouble y = -s * logx;

                return y;
            }

            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p000Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -2, 0xD8773039049E70B6uL, 0x5C8380FDFD5A6952uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0x8B6C64D12AE27BB6uL, 0x33E7EEC943BAD984uL), (+1, -2, 0xA0CD518E40B10578uL, 0x37C8F570AEE3EBE8uL)),
                ((-1, -4, 0xF7DEBCCD92E8C2D7uL, 0x21A1A337E9BF5D75uL), (-1, -3, 0xD9B8B5D14477A05BuL, 0xA99B24698141398AuL)),
                ((-1, -9, 0xFF3808A43C82D93BuL, 0x1C5219A48587D676uL), (-1, -6, 0xA2F5F20B576564A6uL, 0x100DAA890722EB44uL)),
                ((-1, -8, 0xC19E9AE779A2DF08uL, 0xB3112C265FC3A1B4uL), (+1, -6, 0xA2A53CBD5D61A4F1uL, 0xC318CAEC0DC4E4B1uL)),
                ((-1, -10, 0x95B4DA8A8A117F6EuL, 0xD9EB08C3CDA719D5uL), (-1, -10, 0xD78C5A6C0F76D4ADuL, 0x378812688C081C36uL)),
                ((+1, -15, 0xDB3ABA95D6B96655uL, 0x4ACCE02EF30B95B3uL), (-1, -11, 0x8B6C0FB0FCC3CCF0uL, 0xAA50DF2772C916DFuL)),
                ((-1, -17, 0xCBDDBF5D00C3A104uL, 0x784E8D858C09DCB5uL), (+1, -14, 0xF52985AC05701FA9uL, 0xD745411A3F3E8BE7uL)),
                ((-1, -23, 0xC8EE59888A8F05AAuL, 0x17C0B22594BE1B82uL), (-1, -18, 0xEEF3890DE4235793uL, 0x716F9B8BAB628D28uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p125Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -2, 0xF38CE473D2628A91uL, 0x248C21A6F5B718FEuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0x90246634BEB9D327uL, 0x6944B8DAFCBBD41FuL), (+1, -2, 0x890CDAD4B67F443CuL, 0xD9A815E57791ACF6uL)),
                ((-1, -3, 0x83608786FEEDC1C1uL, 0x1ED29583DF6F9375uL), (-1, -3, 0xD2D54E1A314C66F4uL, 0x919FAD190491C8D9uL)),
                ((-1, -7, 0x96BF18A197CF091AuL, 0x484AE46A6E99A908uL), (-1, -7, 0xD52715783468199AuL, 0x10417B97CD68E614uL)),
                ((-1, -8, 0xDA4D365CE8CA1D45uL, 0x58EA3E807BA092E7uL), (+1, -6, 0x918D0EEEB29B81CAuL, 0xC7AC6EEB48F8214CuL)),
                ((-1, -10, 0x9AAE03164EBBE1C7uL, 0xB1276703E899A287uL), (-1, -10, 0xDDBEDD3310842377uL, 0x3F2C3B8FE0A0DC7AuL)),
                ((+1, -16, 0xA51659309B13E28FuL, 0x4A031D0FC47E4261uL), (-1, -12, 0xE1CB740D1FE7D8A7uL, 0xBD435AE6ACCBC88FuL)),
                ((-1, -17, 0xD67FF7444614286EuL, 0x07C909FEDBF69F3AuL), (+1, -14, 0xD2B260EA27F21F23uL, 0x77A8755914906610uL)),
                ((-1, -22, 0x9D9CB17900E28EACuL, 0xF7FD67029293328DuL), (-1, -18, 0xD088E1E9B3A15944uL, 0x11F296B86B8FC897uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p250Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0x8831F6B3D5549982uL, 0x06CE4CE59853F3FDuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0x962FA248F9EA95C3uL, 0xC633E20D00AFDA3BuL), (+1, -3, 0xE6D47C019358BFF2uL, 0x04CA489C26343266uL)),
                ((-1, -3, 0x8D10186E60FFF84EuL, 0x4F5BFBB2823B6244uL), (-1, -3, 0xCC78FEA6FD5B5C4AuL, 0x69E2484B86AE676DuL)),
                ((-1, -7, 0xEE19D14ED8988B42uL, 0xCCBEF4B4765ACC6BuL), (-1, -8, 0xEDBE55FF620C13BAuL, 0x65C8D7FB913CF722uL)),
                ((-1, -8, 0xF5B3D0FB934DFFFDuL, 0x260E9535EFD82400uL), (+1, -6, 0x8388E3B2B4419389uL, 0xA52D3EED622144A6uL)),
                ((-1, -10, 0xA4380858133A0373uL, 0x5E703F02D8E43281uL), (-1, -10, 0xE2120680487F1077uL, 0x5FD7364BA5497684uL)),
                ((-1, -17, 0xA5AA77B471998125uL, 0xC02EDCFBF7271D13uL), (-1, -12, 0xB9F131463E490603uL, 0x19224BBEF7B8B06CuL)),
                ((-1, -17, 0xE77CE5D8892D363AuL, 0xE470532B45E5E92BuL), (+1, -14, 0xB9BBE0BA9B11FD86uL, 0x694B224A872B9E3FuL)),
                ((-1, -22, 0xD793C92F1196771CuL, 0x36A82F62DB27282DuL), (-1, -18, 0xBB6C19D842B7A471uL, 0x537C20E78B31B0ECuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p375Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0x97BA142B277EF97FuL, 0xDD191FF684974317uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0x9E10E11144C00040uL, 0x25F3829BBBCF2C77uL), (+1, -3, 0xC1C14A55A3709A7EuL, 0x3EE686797CFA626CuL)),
                ((-1, -3, 0x9A898A251C64511FuL, 0x7D8706BB1EB10AE2uL), (-1, -3, 0xC67CF580110C1D89uL, 0xDDB16A586B7857D8uL)),
                ((-1, -6, 0xA6306794B835D518uL, 0xD5F9DF168CFD832BuL), (-1, -9, 0xB78DB9C2A3C5314BuL, 0x569C09B33453B382uL)),
                ((-1, -7, 0x8B13B65C68DB94EFuL, 0x3113E166A9FE8743uL), (+1, -7, 0xF0C9581068513302uL, 0xB51C62380B71F602uL)),
                ((-1, -10, 0xB42651313B2C37A9uL, 0x94EAFE48787EB8EDuL), (-1, -10, 0xE37475D3F1809CA4uL, 0xCE71D18E49D2C319uL)),
                ((-1, -15, 0xA515CE106E7B2F41uL, 0xED326726F2079341uL), (-1, -12, 0x9DE6F8A7A565A7D3uL, 0x3BE3F1EAA88C47BCuL)),
                ((-1, -16, 0x808E964EE971D69AuL, 0x5DB554764AE46083uL), (+1, -14, 0xA86DA536345C0A50uL, 0x7360A2F23803BB7EuL)),
                ((-1, -21, 0x8D457EBB8B3EBDA5uL, 0xE781BBF753A63455uL), (-1, -18, 0xAD48476C2B38E84FuL, 0xD8E502FF085BF5FCuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p500Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0xA89F4DA8F671FFB6uL, 0xA5887B140C95D45FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0xA8AD200682ED5C7EuL, 0xFB2D73931C7FAFC2uL), (+1, -3, 0xA60255062FCC583CuL, 0x245F2CF93CFB210BuL)),
                ((-1, -3, 0xAE17FB2A3CAA3DC6uL, 0x4F3B7819936672ADuL), (-1, -3, 0xC1635A8762BB590BuL, 0x9CF4D43217F29991uL)),
                ((-1, -6, 0xDC8466E7FA2EF48BuL, 0xA8760CC25CB4F311uL), (+1, -15, 0xC32486B416E9777EuL, 0xB950B944CBE399B5uL)),
                ((-1, -7, 0x9FC58165CF66DAB0uL, 0x35BFF000EEE7A084uL), (+1, -7, 0xE1BC3CFE32B7DA39uL, 0xC9E485FF70537A4CuL)),
                ((-1, -10, 0xCDC42BB41CE616B0uL, 0x62FD889766873F1BuL), (-1, -10, 0xE2259AF5306A66B2uL, 0xF6EB1DFBC499561EuL)),
                ((-1, -14, 0x95ED31B11867A642uL, 0xC540B1496C3134B4uL), (-1, -12, 0x8EA779B19B49F5B3uL, 0x3DBDD620A7393C37uL)),
                ((-1, -16, 0x94151AA1873773F6uL, 0x5E7693F5A95E89BDuL), (+1, -14, 0x9F3CB3C3C5EACF54uL, 0x702EC9374E6E70F6uL)),
                ((-1, -21, 0xB878172E91DF9FC4uL, 0xFF3D92EFA1238338uL), (-1, -18, 0xA64E5EA415EAD676uL, 0x24BD68CA7BD07C88uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p625Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0xBB28EB53B8ED075AuL, 0xE733B78E3C94A5B1uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0xB7EFEDF308A11C1CuL, 0x784FBFF51EB88068uL), (+1, -3, 0x9A9C43480AE9351BuL, 0x9F54DE122E135932uL)),
                ((-1, -3, 0xCC7CCC1945C9A4CAuL, 0xF71A6B538E9145E2uL), (-1, -3, 0xBEB1FF1F98FC7276uL, 0x1FE986062F50CA6DuL)),
                ((-1, -5, 0x906118F7A0687A0FuL, 0x0953E1F1FC79A7CAuL), (+1, -11, 0x8DFD95FAD584ECC5uL, 0x10CAE35D09CDC839uL)),
                ((-1, -7, 0xBD056BA564542A00uL, 0xBD5E3CC82ABD62F9uL), (+1, -7, 0xDE8E0A78635222F8uL, 0x3B33D09BCF38A302uL)),
                ((-1, -10, 0xF8628C48FFD1ABA6uL, 0x3253420168804A77uL), (-1, -10, 0xDF66F6F48B0A883CuL, 0x0C37026ADE51EE06uL)),
                ((-1, -14, 0xE7B408D2EB06AD30uL, 0x432AC54B03E351ABuL), (-1, -12, 0x91035882C43D54BAuL, 0xD18878DBC89DD7FCuL)),
                ((-1, -16, 0xB3C26D742D97D9E2uL, 0x859546E61AEF514AuL), (+1, -14, 0xA1A83F58F8DBEF22uL, 0x1E43385B5EF2CD0BuL)),
                ((-1, -21, 0xF74B930734666088uL, 0x88011D0B822CD78DuL), (-1, -18, 0xA9AF18CDF8E4DF10uL, 0x758358E01A18C4A0uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p750Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0xCFA741E785977AF0uL, 0x1D7AAD91C15FB43BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0xD11C09A31CE253A7uL, 0x0EBF282BA8D61930uL), (+1, -3, 0xB39296E234A27EF5uL, 0x25623A0104E9BE59uL)),
                ((-1, -2, 0x8162824FB0C0D3DDuL, 0xB2221C5A5AEF3416uL), (-1, -3, 0xC2DFC33219AF7CD7uL, 0xEEF1EADC7A832845uL)),
                ((-1, -5, 0xC246E1AB1E86D0F7uL, 0x3ABDD8A0E645C32AuL), (-1, -9, 0xE564865B2A9EB11CuL, 0x258EEEDB5474E8E1uL)),
                ((-1, -7, 0xEE0F8025D37E8463uL, 0xBB4F3EF6CAE98A00uL), (+1, -7, 0xF51E77E6BB798DDEuL, 0x7B6CF142DBBC9915uL)),
                ((-1, -9, 0xA44E99AEF5B97EF6uL, 0x862518433FDAEF22uL), (-1, -10, 0xDE9A6D47B16C9E1DuL, 0x6F00B92A5ADF3B68uL)),
                ((-1, -13, 0xAF04C7A27D498F33uL, 0x878E3D6602E5BD57uL), (-1, -12, 0xB4B6409D3C6F6F40uL, 0x9A5EE3B1AAF3633DuL)),
                ((-1, -16, 0xEEF7B66B76CF69EFuL, 0x3D170C5CBCC98399uL), (+1, -14, 0xBB492ACAD6157F2EuL, 0x19BCA490669CDE54uL)),
                ((-1, -20, 0xB234DA159BB7A244uL, 0xBB54E988EC5CCF61uL), (-1, -18, 0xC24042D5091B0A06uL, 0x588F3FE0BFEF03BAuL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu0p875Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, -1, 0xE676322AD1395859uL, 0xC31A2233CE34915BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 0, 0x85490A6FA6BD9F3BuL, 0x9697E2C740893824uL), (+1, -2, 0xA3E94C3B63D1B575uL, 0x61CBA563CEFC9029uL)),
                ((-1, -2, 0xC467535433B7161CuL, 0x3FCEDADD1F18D7C1uL), (-1, -3, 0xE13DAC3C2487E100uL, 0x4AEE486D40007868uL)),
                ((-1, -4, 0x94AC8823059182C9uL, 0xCC29E85CDC152445uL), (-1, -6, 0xB1608BF45F791058uL, 0x0B952664A1205A57uL)),
                ((-1, -6, 0xB18DA961C4221438uL, 0x5B71C4DC0B347D8BuL), (+1, -6, 0xB0F5E10F0DE7D893uL, 0x6AE6EC152130CF7FuL)),
                ((-1, -8, 0x85ED8038B01ED6F1uL, 0xC6C9BFFDADE46FE5uL), (-1, -10, 0xED4A1BD303E62336uL, 0x2D0E849C1DB62306uL)),
                ((-1, -12, 0x95A791A68442DCADuL, 0x9097EC30642A98E4uL), (-1, -11, 0xA01FF51B1ED8BFE1uL, 0xB99ED75729B2A54EuL)),
                ((-1, -15, 0xC43AE3159968D382uL, 0xE3067E92DED097D0uL), (+1, -13, 0x8FF43FF544B82508uL, 0x6436F333B6545249uL)),
                ((-1, -19, 0x9C9E499A4B22E148uL, 0x99F6E85714231B96uL), (-1, -17, 0x903A63003CF8F051uL, 0xAB3CA982D67DBD89uL)),
            });
            public static ReadOnlyCollection<(ddouble c, ddouble d)> PadeNu1p000Table
             = new(new (ddouble c, ddouble d)[] {
                ((-1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 1, 0xCE3FB567D862ED20uL, 0xD7CA58B07B8796CFuL), (+1, 1, 0x9821E959973B50F3uL, 0x40A97870FC30FC7AuL)),
                ((-1, 0, 0xDE93EE02C600D153uL, 0x6592BBC00032CD03uL), (-1, -1, 0xAC070D77E0D4441BuL, 0xDD5760C4679E6809uL)),
                ((-1, -2, 0x9AC62D56303E3860uL, 0x75CCB412DA0A0BCBuL), (-1, -2, 0x835B45921D21D943uL, 0xDB06336BB3254574uL)),
                ((-1, -4, 0xC255EAFCACDAB8D7uL, 0x6A91F768306A5BBEuL), (+1, -4, 0xE42672B074D44B76uL, 0xC34B5083D02C9F46uL)),
                ((-1, -6, 0xAC145CDF0C012E4AuL, 0x31374DE523FA26DCuL), (-1, -8, 0x87EDD7DF1928D7B9uL, 0x57EB89E6951A398CuL)),
                ((-1, -10, 0xB8436FD4B3A3B24DuL, 0xD89D06E173ED8CE2uL), (-1, -8, 0x80E107F2812E5BB8uL, 0xD7E3A9C12831F9ACuL)),
                ((-1, -12, 0x8027CBC8A856D958uL, 0xD014448914DBD781uL), (+1, -11, 0xC3389E518D0A21C5uL, 0xD3CD80B1A930ED7BuL)),
                ((-1, -17, 0xD967508ED3821F0EuL, 0xC00EFA300AC9781BuL), (-1, -15, 0xB8E7551046DD7EF6uL, 0x4319F546825E604DuL)),
            });
        }
    }
}