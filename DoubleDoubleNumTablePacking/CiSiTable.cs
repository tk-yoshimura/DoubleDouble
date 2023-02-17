﻿using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class CiSiTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)>> tables = new(){
                { nameof(PadeX0Table), PadeX0Table },
                { nameof(PadeX1Table), PadeX1Table },
                { nameof(PadeX2Table), PadeX2Table },
                { nameof(PadeX3Table), PadeX3Table },
                { nameof(PadeX4Table), PadeX4Table },
                { nameof(PadeX5Table), PadeX5Table },
                { nameof(PadeX6Table), PadeX6Table },
                { nameof(PadeX7Table), PadeX7Table },
                { nameof(PadeX8Table), PadeX8Table },
            };

            foreach (var key in tables.Keys) {
                stream.Write(key);
                stream.Write((UInt32)tables[key].Count);
                foreach ((Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd) in tables[key]) {
                    stream.Write((UInt64)fc.Hi);
                    stream.Write((UInt64)fc.Lo);
                    stream.Write((UInt64)fd.Hi);
                    stream.Write((UInt64)fd.Lo);
                    stream.Write((UInt64)gc.Hi);
                    stream.Write((UInt64)gc.Lo);
                    stream.Write((UInt64)gd.Hi);
                    stream.Write((UInt64)gd.Lo);
                }
                stream.Write((UInt32)0u);
            }
        }

        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX0Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, -1, 0xFBF91EB75D8AE347uL, 0xB11F81A45639BA9CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 1, 0x8845FFDDD37CE962uL, 0x42DA959BED375E07uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -5, 0xC7410DC4F2615AD1uL, 0xD3718289A84EF045uL), (-1, -1, 0x99AC7D216595E500uL, 0x05FC6C9E78AFFD6FuL), (+1, 0, 0x875E90162E9597AEuL, 0x277A5532EF18ADBFuL), (-1, 0, 0x8A9134FF94D1E820uL, 0x22595F9F4790ADA4uL)),
                ((+1, -4, 0x8048397B4F817E1EuL, 0x9D17BE0DCCE84C19uL), (+1, -3, 0xF3D20D5F00E6AC40uL, 0x089DE5D88837A35FuL), (+1, -5, 0x88863D4DC0299BC2uL, 0x19C08D7C01D7A9C2uL), (+1, -1, 0x8C7E6B6C63CE1765uL, 0x3549944D734C133DuL)),
                ((-1, -5, 0xA8ADD8B408241DBCuL, 0x5D969B76C84E9937uL), (-1, -5, 0xF00C1A497E0ADB55uL, 0x9227C20E43C2013EuL), (-1, -3, 0x940F7B31360AB2FDuL, 0x5B31CA0D88C37DCEuL), (-1, -3, 0xB7EBBE69107C84FAuL, 0xA9A0F63046D56BA0uL)),
                ((+1, -8, 0xE6CE771997CA3F68uL, 0xB34CF85DBF9AABF2uL), (+1, -7, 0xB41EB6441AC0A875uL, 0xE57614A74C5A15CCuL), (+1, -5, 0xFE1DD909131C6C44uL, 0x9AC6A807931E38D5uL), (+1, -5, 0xA63EB94E774587D8uL, 0xFA1764B6D3066EDBuL)),
                ((-1, -10, 0x99832C835C6D1DD3uL, 0x82BC30266E808E55uL), (-1, -10, 0xC175311FBF028CE8uL, 0xB8E19FE0577A9E81uL), (-1, -7, 0xDD79255D5FFC6824uL, 0xF628F8FF29FF06E2uL), (-1, -8, 0xDE7921B56FBC655EuL, 0x6970306C6AA6A728uL)),
                ((+1, -14, 0xA83DB5AF0D3322BDuL, 0x1072172CBF35EE5EuL), (+1, -13, 0xA6953A01C8112D8AuL, 0xBF41D83F813031C6uL), (+1, -9, 0x8BF217F92DFFC681uL, 0x46E7B577C4E93C67uL), (+1, -11, 0xDC7580792FE563E3uL, 0x6B4A2FB30921B9F6uL)),
                ((-1, -17, 0xA0DE363856A059E3uL, 0xC9C131D1CB9A3642uL), (-1, -17, 0xD759A2A8DF8BFEBDuL, 0x223624C3C95CEFB8uL), (-1, -13, 0xED4FC2847D6E4B7AuL, 0x97C14AA0E111B44AuL), (-1, -14, 0xA77C6C27D1E00877uL, 0xE4AB9DF7A40E8101uL)),
                ((+1, -22, 0xF4B84D7AF2537328uL, 0xF1C1F03F3995E8C1uL), (+1, -21, 0xE4D806BCDC14753BuL, 0xB727DCA8EF2CD90FuL), (+1, -16, 0xAA1B0DE2562DD349uL, 0xE22B15FBF2C3AA60uL), (+1, -18, 0xB84288DADC2F81CDuL, 0xFA2FA09EF7BD0760uL)),
                ((-1, -25, 0x9861F282BD756A3CuL, 0x9EB06665AC69F8BEuL), (-1, -25, 0xB1FE5DCA1F415D4BuL, 0xCF7946F46AA3A11CuL), (-1, -20, 0x8682655DDC898AF2uL, 0x9D96F514ADC8103BuL), (-1, -22, 0x91542E85767FA029uL, 0xC20EAD17100D0BDFuL)),
                ((+1, -32, 0xFD7646B943BF268CuL, 0x52FA76AEFBF3F497uL), (+1, -30, 0x9E8F18FA2BCFF0B8uL, 0xA87A6F6E6627D952uL), (+1, -25, 0xBE070002EE1E0BC5uL, 0xD3A374694DC88D26uL), (+1, -28, 0xE32D8BF1068A5B92uL, 0xFC96C48EDCE2204CuL)),
            });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX1Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 0, 0xDA8FD8B2FED62203uL, 0x226EBFABF8AB21B6uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 1, 0xE1A776208B0832CEuL, 0xE5B7AC9600AAF41BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -1, 0xEC52774BCA9CA8D9uL, 0xD14EC677950AD86BuL), (+1, -4, 0x914A95AE084EE05FuL, 0x8268C86121D71CB2uL), (+1, -1, 0x8B31FE24F6693726uL, 0x0E7A3D05C470D1DAuL), (-1, -1, 0x9784FFD5CFD9212FuL, 0xB22935BBD7ED4036uL)),
                ((-1, -3, 0xA7336A0744A09C04uL, 0x946347EA00E7DCA0uL), (+1, -6, 0xA53049B8D810673CuL, 0x2C1A240E03D7FFE9uL), (-1, -2, 0x90757BF579B9D0CEuL, 0x96DC6FD159FE1882uL), (+1, -2, 0x99290C3E1B8CCBADuL, 0x1A42310410C55676uL)),
                ((-1, -4, 0x82B6E586ED0EEAE8uL, 0x119B1EC674C9B6EFuL), (+1, -6, 0xF4A5716199212DE6uL, 0x5469A2E7B5FBBF68uL), (-1, -5, 0xF2D33693F1F29C30uL, 0x628DD0D1200284D6uL), (-1, -4, 0xB3FA9AD29E8ECB50uL, 0xDF411A83225C9563uL)),
                ((-1, -6, 0x8637C2042BD77866uL, 0x0B83415B5A40C20CuL), (-1, -8, 0x8644D5BF17B8D0A5uL, 0x3F281D22F4963E32uL), (+1, -7, 0xD1727070F4DEC66DuL, 0x09F75F24E0ABC5B7uL), (+1, -6, 0xB6C93241A91D8079uL, 0x50E24783391CF4BEuL)),
                ((-1, -10, 0xB6BC8C8C07A7107BuL, 0xF405BBBE6CC9B097uL), (+1, -10, 0xC047A1D2FF2A2273uL, 0x50DC90182712B5B2uL), (-1, -8, 0xFFE057667B765AF8uL, 0x3F5D65425A657B11uL), (-1, -9, 0xFD74E7376C11A1B1uL, 0x796EF4823386E630uL)),
                ((-1, -12, 0xC945B999E4F32636uL, 0xD3DDE6A5DF24F488uL), (-1, -13, 0xBBA472E164797407uL, 0x6AE97358908C27A1uL), (+1, -11, 0xF03EAFF67CD29F1EuL, 0x4E25615D38512470uL), (+1, -11, 0x9804A54812A3D792uL, 0x617382AD5CD5FBBEuL)),
                ((-1, -17, 0xA67E3931167A7043uL, 0x3C58EABBBBA2403FuL), (+1, -15, 0x854B5250FCE4BB28uL, 0x45CEED67EF395D88uL), (-1, -13, 0xCFA06CC53A49C284uL, 0xE79C73E97D60EC96uL), (-1, -15, 0xF61E12B04FA65BADuL, 0x5A5FD88ADC5EC0A9uL)),
                ((-1, -19, 0xFF96E7AB8F8ED8A8uL, 0x14CC86C2CF5D8D2EuL), (-1, -18, 0x9C35A2C7EF17018FuL, 0x491CB636CD740219uL), (+1, -17, 0xAF2ABC871C27D77FuL, 0x9C02EFA96C96F902uL), (+1, -18, 0xA7EB86AB6031C954uL, 0xB65139C63834B1E9uL)),
                ((+1, -21, 0xD4F68923B70C7E12uL, 0xCF640453E4D83A5CuL), (+1, -22, 0xF8C8A7917FD4268CuL, 0x511EFE6F953D24F3uL), (-1, -20, 0xC25A4661E93C6D91uL, 0xB91AFB095AC40098uL), (-1, -22, 0x83E4C6F61718AF4BuL, 0x7146EF50970FA949uL)),
                ((+1, -25, 0x93FF17197BB342D3uL, 0x280093D1361A7E5BuL), (-1, -25, 0xC225D7A13CEF25A0uL, 0xF3DC08B53429151FuL), (+1, -26, 0x87C3142BE95A663CuL, 0x8C67859AADB35AECuL), (+1, -27, 0x90029A4FF69102EEuL, 0x1720949A3649DC6BuL)),
                ((+1, -29, 0xF53DF8068FF759BDuL, 0xABE2E31404331CACuL), (+1, -30, 0xE34CE7A3A0568743uL, 0x5C53947BA7E710CBuL), (-1, -30, 0xB6E684FE438DE252uL, 0xD5EB31DCC64E4694uL), (-1, -34, 0xE0FD31BB93969DC4uL, 0x2D56FA9F0962EC55uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX2Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 1, 0xA49ADDCE09277350uL, 0x472AE7F3C7A52609uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xA6518AB7903DFA81uL, 0xA79C6E74EBE8D318uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 0, 0x9EAA64DED2CBB69EuL, 0x5C2DB47C018AB448uL), (-1, -1, 0xD69FBD404C7E5D3DuL, 0xB2B3029234951720uL), (-1, -1, 0xE2DF433991D15B2BuL, 0xA3B8F7DE3A19A52AuL), (-1, -3, 0xB11B615BC776800DuL, 0x4EA5F2F30594C952uL)),
                ((-1, -2, 0xC894C922E589AB55uL, 0x8B78A5049F758CDAuL), (+1, -2, 0xDEA8F1973886EB73uL, 0x3D18F0CF519CF11CuL), (-1, 0, 0x84A6E6D0C4EDA4E5uL, 0x5DAC487892F5E776uL), (+1, -3, 0xF6007D321D317510uL, 0xE820622A443F2388uL)),
                ((+1, -4, 0x9C31B87959F5A4E4uL, 0xA34F191055615E4FuL), (-1, -3, 0xAB077BC1B589F9D8uL, 0x04310AEAEA3FEED5uL), (-1, -4, 0xE4E5F8C9DC3D0D6EuL, 0x8B3B24FA35C9794BuL), (-1, -5, 0xDE273B65C227BB17uL, 0xC9F127EE1BCCC54DuL)),
                ((+1, -8, 0x8C6933A3D700A728uL, 0xE465264B2F7925C8uL), (+1, -5, 0xC16F791AAC84265AuL, 0x056248CCE907A1A6uL), (-1, -4, 0x89C6718B8AFC3F16uL, 0x2DEAACE571F34D6EuL), (+1, -6, 0xDA26083360CF0971uL, 0x1D47A494EB31E8C1uL)),
                ((-1, -9, 0xD716211152F67905uL, 0x88B1B1811E40EF28uL), (-1, -7, 0xB536D7831EA89181uL, 0x5C56B51F31DC852DuL), (-1, -7, 0x987E78DBBEA6FC30uL, 0xAA7E6917D2AF45AFuL), (-1, -8, 0xB9DA58DB0996E512uL, 0x8AF1D131418DC7A4uL)),
                ((+1, -10, 0xCBFF8236F1DB90D0uL, 0xEEEFE62CBD345AE8uL), (+1, -10, 0xFF19B633EA7F018AuL, 0xD91548A340289E50uL), (-1, -12, 0x8F776BECE346EA90uL, 0x258DD92901C87298uL), (+1, -10, 0xB38E4203A41667FEuL, 0x90E32737E91308CBuL)),
                ((-1, -13, 0xC64CC30A28B48F92uL, 0x9E4D21F4E14E53ACuL), (-1, -12, 0x974F3B30232FB281uL, 0x39678ED2FAD428F6uL), (-1, -12, 0xF577032511790A14uL, 0x6F4EFED28D378734uL), (-1, -13, 0xE2943416FCF7027DuL, 0xFF875D997D40FE98uL)),
                ((+1, -15, 0x8176AA8F38A74650uL, 0x8496D6D142FE4B8CuL), (+1, -15, 0x830F6EA2535D83CAuL, 0x4A5A170F8FF7D218uL), (+1, -15, 0xF68908A679A94EDCuL, 0x1701E40F46CC9D6EuL), (+1, -16, 0xE6F53FF3DACDC701uL, 0x4217B716004FA2AAuL)),
                ((-1, -20, 0x9AB6ABA84CD3CDF4uL, 0xE7B59D12BE28DA2EuL), (-1, -19, 0xAF2E7E8DBD50DBAEuL, 0x7E932ADC6A6C57FEuL), (-1, -18, 0xD88DA259DDE48509uL, 0xB782FF99F8800B32uL), (-1, -19, 0xB05EDA0ECFE44978uL, 0x7E3B8ABE622E9AFAuL)),
                ((+1, -28, 0x8CE1C31BB57E34D6uL, 0xC5AA043E3B1AB364uL), (+1, -23, 0xA5F31A7D38E1BBFAuL, 0x12C29481CEE45631uL), (+1, -21, 0xB326FFAF20A09BCAuL, 0x2A1E40DB84D867CCuL), (+1, -23, 0xA5AAE276ADCF7021uL, 0xDAAF0626FB0F7915uL)),
                ((-1, -27, 0xA7ADEBE578682A11uL, 0xD8FEADB2E36475EAuL), (-1, -28, 0x894717DF51BB73FBuL, 0xAB0DA912E8C663D2uL), (-1, -26, 0xCF56754CED73E3C7uL, 0x23C03B3AA22268F5uL), (-1, -28, 0x95D973700056B166uL, 0x498B19D9CCDB17FCuL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX3Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 1, 0xE1571416352D86DFuL, 0x5DA2C7325F863D4CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 2, 0xE1F0E845775B5D54uL, 0x812CB2F268070AB1uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 0, 0xE8C418AA1295CB53uL, 0xFE6E92D2BFCA4958uL), (+1, -3, 0xF5F03E4A6B50E8DFuL, 0x5E9F724B29B2467CuL), (-1, 1, 0xA3C1387CBCF78178uL, 0xC8A95988D2C52C01uL), (+1, -4, 0xB832EB37CD1AC9C4uL, 0xABC538666AB6C198uL)),
                ((-1, 0, 0x80FF40D41BC3CFE0uL, 0x931087E3088834F1uL), (+1, -3, 0xDC5134D78BD21BABuL, 0x2DB1FB84E0B1E2EEuL), (-1, 0, 0xE097538917156DB3uL, 0x6A20BFC2CACD1D09uL), (+1, -3, 0xDEF83638E1F53EAAuL, 0x8A41E58DB51CF77BuL)),
                ((-1, -2, 0xAFBCF617D64D2555uL, 0xEE482EC32A349409uL), (+1, -5, 0x9E294E7E9CF011A6uL, 0x48E7F13BE5EF6635uL), (-1, -1, 0x8682D8B3A0476074uL, 0xC886622F1DBF2A0CuL), (+1, -6, 0x87B224D920C6B02FuL, 0x01AF1AAC8E52F991uL)),
                ((-1, -4, 0xC9067E72FEC6AEAAuL, 0x4E12B1ECEA1DB7FAuL), (+1, -6, 0x851FD54A4768EE06uL, 0x1CF273A430430697uL), (-1, -3, 0xA8F4B1743E06ED8BuL, 0xACEB8B120C50E289uL), (+1, -6, 0x8E3B839740B8C20BuL, 0x7F1604F56453D792uL)),
                ((-1, -6, 0xB8FDE5F252603C65uL, 0x94965BE4667C1C6CuL), (+1, -9, 0x85B7B25E48DA62F4uL, 0xF09AA414D2B40043uL), (-1, -5, 0x9E14182AC55F2425uL, 0xAE011C60E9FD788BuL), (+1, -10, 0x85C3843011988EB2uL, 0x550BD6B2BB0FB4C4uL)),
                ((-1, -8, 0x82B76E1FC581EAD3uL, 0xF4813CFC13A2E8AAuL), (+1, -11, 0x82FAAFB90177FA12uL, 0x674EF59A44B360E3uL), (-1, -8, 0xE5EAA17CF5376861uL, 0xE792C119AF3F7A4EuL), (+1, -11, 0x9AEF3A30512CA217uL, 0xA116F362C059E8D5uL)),
                ((-1, -11, 0x9B2F8450170194EDuL, 0x481F78008EF1FEF2uL), (+1, -15, 0x9EA49276D5ADCDB8uL, 0x40C0869485924775uL), (-1, -10, 0x97739D7DBC94B522uL, 0x9BBD383BB6B7DB33uL), (+1, -16, 0xAF41ADD004313503uL, 0x4083E948F955D561uL)),
                ((-1, -14, 0x84C010A5DFED112CuL, 0x4F8750819D726BE8uL), (+1, -18, 0xB6E43CD8DAAD7793uL, 0x06507390738C04A3uL), (-1, -13, 0x8084673F27D81CA5uL, 0xB669BD98BFABC49EuL), (+1, -18, 0xFB84E8D5D864F908uL, 0x292B784F63B8E5C8uL)),
                ((-1, -18, 0xAEAA1A44D742C0D5uL, 0x64EAE85F463492E5uL), (+1, -23, 0xA7F86AF9E2CAEBC5uL, 0xDFD1D16E23295743uL), (-1, -17, 0xC50869878BAF1B3AuL, 0x2341F8D62DE07CBDuL), (+1, -25, 0xCB595E81A32FA62DuL, 0x0E97131F8F4EDFCFuL)),
                ((-1, -22, 0x914884DE7834BB03uL, 0xA1D54814FC959DBFuL), (+1, -27, 0xCEE3319C0E5EFCA8uL, 0x30003E520072F237uL), (-1, -21, 0x98BEDCAC993447BFuL, 0x9BF11D74C54B4CB4uL), (+1, -26, 0xB02DE37B0824E3E4uL, 0xD13EE2D4EE30546CuL)),
                ((-1, -28, 0xFA8585BE55E0548EuL, 0x1EC10D9265A3228EuL), (-1, -33, 0x8C8DA21FFEB1E938uL, 0x93A96DFDEFAC3F2EuL), (-1, -26, 0xA226130CCE0995E1uL, 0x882A454A8A34F20FuL), (-1, -32, 0xA564F3ACB4744164uL, 0xAD40D61B380C3A64uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX4Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 2, 0x902D3A64AE0D2851uL, 0x7D9FC4C13A51441FuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0x90431EB6B74E535DuL, 0x6B3BFFDB9543EB11uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 2, 0xCF2A5D583CA5B3B2uL, 0x1816468420B8DE60uL), (+1, 0, 0x9BB99B1A07530396uL, 0x0B2E1416617D6C24uL), (-1, 3, 0xAE331F4A34422BA2uL, 0x4DB640AFFEBD0FB9uL), (+1, -1, 0xFCF74AEF064BCB84uL, 0xC064416FF167F4C5uL)),
                ((-1, 2, 0x9E9BEC2953163985uL, 0x952850B53B177741uL), (+1, -1, 0xD4BBF956D07B6898uL, 0xB4246E75FBE513DDuL), (-1, 2, 0xF7E2734C1EBED03BuL, 0xFAEE080D0B7A2CCAuL), (+1, -1, 0xA409C326A87AB611uL, 0xF843E971BF93DEE1uL)),
                ((-1, 1, 0xA4B3712909088F88uL, 0x6706CC6C9872348CuL), (+1, -2, 0xC6453EDAB3B4AB50uL, 0x6B8EEF24815A7988uL), (-1, 1, 0xF36215DA143B5691uL, 0xC35527378DF6D72FuL), (+1, -2, 0x8F812E44B7E4DA70uL, 0x75D351F456D0B144uL)),
                ((-1, -1, 0xFBA4B049A650748BuL, 0x124CC1D73B890C48uL), (+1, -3, 0x878F4507E45D6F92uL, 0xDBE3DBA722F553ACuL), (-1, 0, 0xB30819E4127785B6uL, 0x5B8FF1EC58D37A72uL), (+1, -4, 0xBEA4162562F4CC1AuL, 0x3E6E8FE104DDA25CuL)),
                ((-1, -2, 0x92BCDF0AFAE7C7AEuL, 0x5EAF06BFC2D8FDFFuL), (+1, -5, 0x8C5E5C9EDE77A317uL, 0xE63EFF2D1854E4A3uL), (-1, -2, 0xCB3541D97170B4C8uL, 0xB6D8221C629CFB87uL), (+1, -6, 0xBFE0D2DC2738A780uL, 0xC66CA58D538EC549uL)),
                ((-1, -4, 0x840412DB1C536E6EuL, 0x6516B66A943C3E93uL), (+1, -8, 0xDBDA6B67DD02B696uL, 0xAE8579A9D187A2FCuL), (-1, -4, 0xB369EED07CAE44E1uL, 0x332A21ED7D4205C1uL), (+1, -8, 0x9454EFF38F9F2716uL, 0xCF14A686D7AA06D0uL)),
                ((-1, -7, 0xB5FFD171D86AFD4CuL, 0x0AE0EE3813EA592FuL), (+1, -10, 0x801D428EB6CE1E40uL, 0xC89FCFE9EF8711D2uL), (-1, -7, 0xF47F29895D1C3D1DuL, 0xE581825A4CC7D313uL), (+1, -11, 0xAB0C4A4BC55538B8uL, 0x30FD2BD285C0724EuL)),
                ((-1, -10, 0xBB6D1B62FE43B74EuL, 0x8A6054D668C8F6FDuL), (+1, -14, 0xD2F4D0EA5F5D8449uL, 0x19FF7A46EBED54E0uL), (-1, -10, 0xFA4626CA91CE9CA3uL, 0xCC5AA1F95AC9D674uL), (+1, -14, 0x8CC359D9C2BD9884uL, 0xDC235155117FEF05uL)),
                ((-1, -13, 0x88AB5C380D2DE562uL, 0xE6400AAC3726D984uL), (+1, -18, 0xDD9D0F607D79568BuL, 0xD005FA28296F7365uL), (-1, -13, 0xB662CE3BEC2A391FuL, 0xE910F0F14C2D3CFEuL), (+1, -18, 0x941D95F605C3B71CuL, 0x1622DBD1F3122F16uL)),
                ((-1, -18, 0xFD0E6F8E15D88778uL, 0xA13F0F726F30E274uL), (+1, -23, 0xE039159225D675B4uL, 0x88FEBB1F20C44C30uL), (-1, -17, 0xA97CA09582171383uL, 0x7FB2B42BEC1D4A61uL), (+1, -23, 0x97A3DCE27FF7485BuL, 0x48668D7F74F835C6uL)),
                ((-1, -23, 0xE0683E5466C24676uL, 0x3A7A58086E4B2AB4uL), (+1, -38, 0x89DEB4D05646999EuL, 0x473FD0112F0E1424uL), (-1, -22, 0x9781EABC1888C704uL, 0x641D68DFBE36CAFCuL), (-1, -38, 0x9A051D2063F00C3FuL, 0x76865A7CE05616C8uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX5Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 2, 0xB00B7B00453164DAuL, 0xF1B1B0D1D0DDCD9BuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xB0112BEAC3AC3B18uL, 0x78FE467E7E9F8D49uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 2, 0xF004CCB39CB0E60FuL, 0xD382565FC6F324E3uL), (+1, 0, 0x974AF43D53411D4CuL, 0xC4F8502D112C2844uL), (-1, 3, 0xE808A16341F91DF1uL, 0x475CD58E6D7F8215uL), (+1, 0, 0x917D9E70BBD8A5DCuL, 0x8C5831DFF7B22789uL)),
                ((-1, 2, 0xA489113AFB9FA5A5uL, 0xCAE46C3CA05A9347uL), (+1, -1, 0xB84E6C92222D0E8AuL, 0x3F2C09F1C174C680uL), (-1, 3, 0x9E6A908DBFEE23A2uL, 0x594430F36D6CF2F1uL), (+1, -1, 0xB182E360D3502A22uL, 0xAEDB02E8D54EF433uL)),
                ((-1, 1, 0x90C3BF33CCB63641uL, 0x68B39B6DAAEC0C2AuL), (+1, -2, 0x8F8C97D4AFC9E24CuL, 0xE080D8167C48BE32uL), (-1, 2, 0x8C74FF0707082488uL, 0x233CC7CF574D1EEBuL), (+1, -2, 0x8BC21BDAC425F61CuL, 0xFB19EC7822BB86BEuL)),
                ((-1, -1, 0xB2D69639F62B71C1uL, 0x484BE87391978070uL), (+1, -4, 0x9BB9B479839E0AF2uL, 0xB7779CDE80B36B56uL), (-1, 0, 0xB0434B543FF2F827uL, 0x2FC2C0BC7767A975uL), (+1, -4, 0x9ABB774696182646uL, 0xFBC1FF59B46E681FuL)),
                ((-1, -3, 0xA1176A9D2C5CD74EuL, 0xC099904B3EEE9D7DuL), (+1, -7, 0xF21FEF59450AB2DAuL, 0xC0FB1BDC8DA7CC71uL), (-1, -2, 0xA1DB9E968CCD8832uL, 0x084E7B43E25034CAuL), (+1, -7, 0xF5CE7F726E61A605uL, 0x4B3DCE1AEC52C85CuL)),
                ((-1, -6, 0xD5FD54DC762F7AB3uL, 0x0499F1B11F58C964uL), (+1, -9, 0x8729E92CB3A57796uL, 0xBD93929B5D998409uL), (-1, -5, 0xDB0B2A38A62A4061uL, 0x1D20F8563A1BDBC1uL), (+1, -9, 0x8BD475CCED7FE458uL, 0x9D7AF92B9E85710CuL)),
                ((-1, -9, 0xCF65F8821E3335A8uL, 0xE78071A1764F3616uL), (+1, -13, 0xD225F2FFD804820CuL, 0x878A41103A35DD22uL), (-1, -8, 0xD717EEB41C46575DuL, 0x8A3EBB88282C68A4uL), (+1, -13, 0xDAFC2D4894D2AC1BuL, 0xB30D557FDFE8FF52uL)),
                ((-1, -12, 0x8CF9A6CF8C2262D8uL, 0x9CA53A389BF5ED2DuL), (+1, -17, 0xD0D85AEA0746D63CuL, 0xE1D122A24BD27E0EuL), (-1, -11, 0x928C6076A64B3297uL, 0x596BE910E8E9185DuL), (+1, -17, 0xD77FAAB2AD9E95FEuL, 0x1F6BB80FCE5BF810uL)),
                ((-1, -17, 0xF3F0EF6576FDFB32uL, 0x33E87CBD7DAF5454uL), (+1, -22, 0xCC540E181DA71FD1uL, 0x4EB76BAA381F77D5uL), (-1, -16, 0xFA752DED781550ACuL, 0x31621865FA74D9CCuL), (+1, -22, 0xCBC1CAC505B75C6BuL, 0xBEE1EC2CF695B1C4uL)),
                ((-1, -22, 0xCC641355303FAD9CuL, 0x90B284960B1AEC59uL), (+1, -39, 0xE6AD0CF1C877DDD4uL, 0xE45C2948B413E492uL), (-1, -21, 0xCBEAD552DAA7FB89uL, 0xB49EA95C7AD5141DuL), (+1, -37, 0x90BA5246E887CB53uL, 0xD8776359BC1FD520uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX6Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 2, 0xD002E1ABF43A843CuL, 0x05D29A8ED45B9BE1uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xD00451B3D53E64C4uL, 0x4E4C0E84FCEE1A0DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0x9F6B75B1098397E9uL, 0x1DCBAEC2B2B6EFE6uL), (+1, 0, 0xB0844489685CE3B2uL, 0xE258024E467E999BuL), (-1, 4, 0x89DC51116096895DuL, 0xD9C1CA3A15F31726uL), (+1, 0, 0x95FC0781DCA032D0uL, 0xEF35C1D6C4D6E10CuL)),
                ((-1, 2, 0xDB37D5CDF6CA7DB0uL, 0x94114211AF993BBFuL), (+1, -1, 0xD77EE017171C77B1uL, 0x01E26F21F83E96E5uL), (-1, 3, 0xAA3C493C05E5BD02uL, 0xDFCAAB007A1F2358uL), (+1, -1, 0xA35F42B2947F7B3FuL, 0xAA18C04382DF66A6uL)),
                ((-1, 1, 0xB48B80BDB7706994uL, 0x2F45FC7CF88D33C6uL), (+1, -2, 0x9BE6FBE27C46D4E1uL, 0x845C0C994B2B8CC4uL), (-1, 1, 0xFF5149C75550D2C2uL, 0x8E9D0AAB238F883AuL), (+1, -3, 0xD5B31E92E9AEE693uL, 0x988B780F3C38B48CuL)),
                ((-1, -1, 0xC61B8BC638F2CE58uL, 0x8B86D56DEAE35265uL), (+1, -4, 0x93E283A72685B6A9uL, 0xFD18935B24EB0C93uL), (-1, -1, 0xFF7FFC156A1AE99BuL, 0x7F30E5B1FCF359CDuL), (+1, -5, 0xB6F410DD3CFE9BE4uL, 0x2433FC8B2781904FuL)),
                ((-1, -3, 0x97FA990A8F736393uL, 0x17E946ED6970A677uL), (+1, -7, 0xC01700B66526F8D8uL, 0x7397F0429782A909uL), (-1, -3, 0xB11EA761A9F98B5BuL, 0xB0EFBD9359087A62uL), (+1, -8, 0xD2D0ADBDC2FB45A0uL, 0x600462D5DBB918A1uL)),
                ((-1, -6, 0xA5FC3AF5F77339DCuL, 0x1C71F4BEF09EE121uL), (+1, -10, 0xAC29811AA082695EuL, 0xA67FFD2EA0811B5BuL), (-1, -6, 0xAAACB94E7E9319F0uL, 0x10E71757FBE9EB69uL), (+1, -11, 0xA0A85ACDC4B7E6E1uL, 0xB622EA4DE9CCE2FEuL)),
                ((-1, -9, 0x800FB5EA3F8ACAEBuL, 0x514E1B577A6B2F00uL), (+1, -14, 0xCEAA6E3EA8CEAA7AuL, 0xDEAC65269AB2D4E4uL), (-1, -10, 0xDC7D8CEEEDBF06A4uL, 0x0DBC18690C4A47A2uL), (+1, -15, 0x9345C41FBA928738uL, 0x8A3288192D29C462uL)),
                ((-1, -13, 0x85A95930CEB86106uL, 0xCCE2861ECD0010BAuL), (+1, -18, 0x9553A24363E62568uL, 0x2A1A7ECDB182791AuL), (-1, -14, 0xA8D22BF52FB0137CuL, 0xBCA7EC3C2774E1DDuL), (+1, -21, 0xD447E5EF4341781CuL, 0xB2AF7349C25B36B0uL)),
                ((-1, -18, 0xA85DF2A804B60F99uL, 0x25C034AE0D6DCD5BuL), (+1, -24, 0xBB5FE808890F0FF4uL, 0xF3FCC783214A62F8uL), (-1, -20, 0xB5CF70756B715296uL, 0x877831DAB7638D7BuL), (-1, -26, 0x9666126DAA58DF3AuL, 0x6184DF4082A2EE90uL)),
                ((-1, -24, 0xBB553A25ADBDFC52uL, 0x025804A19116953BuL), (-1, -42, 0xF373002202112573uL, 0xBCC42876D1A76489uL), (+1, -25, 0x96956ECDE15EEE2CuL, 0x8C2D3A213524AE4AuL), (-1, -41, 0x913663F20104E790uL, 0x9EB4F553B5D5CB13uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX7Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 2, 0xF000B89A6053EA55uL, 0x6B6E1CD21A9AA703uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 3, 0xF00114DA9B2D2F47uL, 0x990E7D3A0FBDE18DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 3, 0x80D41CE0E793DFFAuL, 0xCE86FB9F63A5F120uL), (+1, -1, 0xF0B3C88C4455EE29uL, 0x01F8ED69320E3871uL), (-1, 4, 0x81B8695241E53F87uL, 0x452226EF25EAABADuL), (+1, -1, 0xF29AFB9A16329436uL, 0x9C4063D82FF0B3A8uL)),
                ((-1, 1, 0xFF1B6A27E224A6D4uL, 0x1DE0685D20B472A9uL), (+1, -2, 0xCFED33FF64F8B09CuL, 0xE4CC4009CFABFD07uL), (-1, 3, 0x8210F7F5C233DDCDuL, 0x13058767C6EB7190uL), (+1, -2, 0xD4C7872ECC4C115EuL, 0x7F5672B039F131D3uL)),
                ((-1, 0, 0x99560F9D04BD70CCuL, 0x9BDFE06DA66B107AuL), (+1, -4, 0xD83912E9C112B224uL, 0xA17B37460742C00CuL), (-1, 1, 0x9E87B70134A1093CuL, 0xCCDDCD3E7BAE76F5uL), (+1, -4, 0xE0B71A8DB125441BuL, 0x40433E2099D14694uL)),
                ((-1, -3, 0xF80118612A01202DuL, 0xEE3C8678D18F2267uL), (+1, -6, 0x95381A62F6B763ADuL, 0x8ADCD67F588B323DuL), (-1, -1, 0x81D3CC9A461FB74DuL, 0x1E3D33A6DEA8D41FuL), (+1, -6, 0x9D1DF1517241D647uL, 0x93B62D1C72B16D0AuL)),
                ((-1, -5, 0x8CE2220EA8605220uL, 0xA12F40928E99FDDAuL), (+1, -9, 0x8D6249797E931B69uL, 0x26F12DBB60C3E5BFuL), (-1, -4, 0x94FBBAF9F7B2B113uL, 0xF829FCADC34E32A4uL), (+1, -9, 0x963D65EEE8F94C38uL, 0x7BC3D1979AB7C9C3uL)),
                ((-1, -9, 0xE297338E44DAF618uL, 0x32474753B0A95EEEuL), (+1, -13, 0xB5C61F1EC35101FBuL, 0xF3A0E48D512109E4uL), (-1, -8, 0xF12EB06A9FCF97C4uL, 0xCCEE3EA65EE3102DuL), (+1, -13, 0xC20267370987F2EDuL, 0xCE4B994C312BA0E5uL)),
                ((-1, -13, 0xFAC2DEF9C9DA7E77uL, 0xCB93815111021CE2uL), (+1, -17, 0x932CA50AA921E245uL, 0x81099B21ECE42E11uL), (-1, -11, 0x85B3AC7C0C27E566uL, 0x0A9C0F404D4DAD00uL), (+1, -17, 0x9C9408D8FD4D03DCuL, 0x179278DBD3D70A8DuL)),
                ((-1, -17, 0xAEAE422F0779EB10uL, 0xDA0456A82C75D021uL), (+1, -23, 0xEAB5D1D4E3693A42uL, 0xC99417B76F2E3646uL), (-1, -16, 0xB963A277C5CF1C62uL, 0x9A9693B578A790FBuL), (+1, -23, 0xF5D497201D660AEDuL, 0x14A5B3EEBFC317B0uL)),
                ((-1, -23, 0xEAB46F7141D60D4AuL, 0xBA50CF9146D3EA5AuL), (-1, -44, 0xFC831299891EF5B4uL, 0x46DCA6F5C46A67C1uL), (-1, -22, 0xF5D1A988ABD2B909uL, 0x051CDAA78F94EF36uL), (-1, -42, 0x82893333712E7A68uL, 0x788095E9DF31AA61uL)),
        });
        static readonly ReadOnlyCollection<(Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)> PadeX8Table
            = new(new (Hexcode fc, Hexcode fd, Hexcode gc, Hexcode gd)[] {
                ((-1, 3, 0x88001714C877BCA9uL, 0xE4173B16BAB7992AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (-1, 4, 0x8800229EC4E12FACuL, 0x27F652244F04201DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 2, 0xD7F34E4A924294F6uL, 0x0C3471F3F313B7D3uL), (+1, -1, 0xAD21597834C5CA9DuL, 0x06FE3C9A9B34C495uL), (-1, 3, 0xDC94EE0CD2CF1CE6uL, 0xF232D0C4B1D0427EuL), (+1, -1, 0xB17D496E224FD950uL, 0x2DC46E606039A2F1uL)),
                ((-1, 1, 0x9E1D01E822E2F0AEuL, 0x3FC88E66E7B5951EuL), (+1, -3, 0xD826BFD3ED02EB30uL, 0x64D295173FD9298FuL), (-1, 2, 0xA520CD5DF765591BuL, 0xAD7D83FA5C9BB5C6uL), (+1, -3, 0xE34DDAFCE128E41EuL, 0x78E5E66A4A2255FBuL)),
                ((-1, -1, 0x8C528DF816945FE0uL, 0x85DF95604FB75359uL), (+1, -5, 0xA26B2C03218BECBDuL, 0xC9FC81E731E12D1CuL), (-1, 0, 0x95F7A14F18CAD148uL, 0xF37131E027CF6053uL), (+1, -5, 0xAF534F503AB388B2uL, 0xDFE382BA9D6FE2D1uL)),
                ((-1, -4, 0xA6440EF1F8969966uL, 0xAB2993EBF51A63BBuL), (+1, -8, 0xA01B1B1455243CC0uL, 0x301B2F162165332CuL), (-1, -3, 0xB60FCAE756000B18uL, 0x506C5B01986524F7uL), (+1, -8, 0xB1B106E6203A416CuL, 0x981DDCCD162C9CDAuL)),
                ((-1, -7, 0x8799B0095D139D57uL, 0x1919B2AC0CD07006uL), (+1, -12, 0xD11ED281C7B98BC2uL, 0xDB79D239B75189F7uL), (-1, -6, 0x9861106425AF6904uL, 0x76DD22F9CF63245FuL), (+1, -12, 0xEF2F7928090F2B9BuL, 0x8F8CD168EAAAA7E4uL)),
                ((-1, -11, 0x95C6A89FF64443CEuL, 0x65D6F8D7AEA3D2B6uL), (+1, -16, 0xAA3937E5BF3D3B5CuL, 0x3ACAC5CAB412D1F6uL), (-1, -10, 0xAD1AD5ED35045F0DuL, 0xA501E02E0D3E4CC8uL), (+1, -16, 0xC974EAAAA734E635uL, 0x8636CE498DF6E126uL)),
                ((-1, -16, 0xCE7B595401F4E5F5uL, 0xE5EBADEDE0BBDDB0uL), (+1, -21, 0x8880ABF502BBBB34uL, 0x73ED35C5856D4FDEuL), (-1, -15, 0xF63C80D5DF5C8F44uL, 0xCB32DEF69685F91BuL), (+1, -21, 0xA8950E6376DF2071uL, 0xE5176CCD1F4DF0ECuL)),
                ((-1, -21, 0x8880A85CFAF6FBABuL, 0x367115DBABB90121uL), (-1, -46, 0xB1968F308CD9B030uL, 0x6B591116A274AA05uL), (-1, -20, 0xA894EEFEA215A864uL, 0xF0BE2839CD96F8A5uL), (-1, -45, 0xF57A21789927D2D8uL, 0x9F56E8694DA1ECACuL)),
        });
    }
}
