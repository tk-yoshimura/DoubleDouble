using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    static class MathieuNZTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode c, Hexcode d)>> tables = new(){
                { nameof(PadeAz1Table), PadeAz1Table[0] },
                { nameof(PadeAz2Table), PadeAz2Table[0] },
                { nameof(PadeAz3Table), PadeAz3Table[0] },
                { nameof(PadeAz4Table), PadeAz4Table[0] },
                { nameof(PadeAz5Table), PadeAz5Table[0] },
                { nameof(PadeAz6Table), PadeAz6Table[0] },
                { nameof(PadeAz7Table), PadeAz7Table[0] },
                { nameof(PadeAz8Table), PadeAz8Table[0] },
                { nameof(PadeAz9Table), PadeAz9Table[0] },
                { nameof(PadeAz10Table), PadeAz10Table[0] },
                { nameof(PadeAz11Table), PadeAz11Table[0] },
                { nameof(PadeAz12Table), PadeAz12Table[0] },
                { nameof(PadeBz1Table), PadeBz1Table[0] },
                { nameof(PadeBz2Table), PadeBz2Table[0] },
                { nameof(PadeBz3Table), PadeBz3Table[0] },
                { nameof(PadeBz4Table), PadeBz4Table[0] },
                { nameof(PadeBz5Table), PadeBz5Table[0] },
                { nameof(PadeBz6Table), PadeBz6Table[0] },
                { nameof(PadeBz7Table), PadeBz7Table[0] },
                { nameof(PadeBz8Table), PadeBz8Table[0] },
                { nameof(PadeBz9Table), PadeBz9Table[0] },
                { nameof(PadeBz10Table), PadeBz10Table[0] },
                { nameof(PadeBz11Table), PadeBz11Table[0] },
                { nameof(PadeBz12Table), PadeBz12Table[0] },
                { nameof(PadeABz13Table), PadeABz13Table[0] },
                { nameof(PadeABz14Table), PadeABz14Table[0] },
                { nameof(PadeABz15Table), PadeABz15Table[0] },
                { nameof(PadeABz16Table), PadeABz16Table[0] },
            };

            foreach (var key in tables.Keys) {
                stream.Write(key);

                stream.Write((UInt32)tables[key].Count);
                foreach ((Hexcode c, Hexcode d) in tables[key]) {
                    stream.Write((UInt64)c.Hi);
                    stream.Write((UInt64)c.Lo);
                    stream.Write((UInt64)d.Hi);
                    stream.Write((UInt64)d.Lo);
                }
                stream.Write((UInt32)0u);
            }
        }

        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz1Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((-1, -3, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -4, 0xB3AB1D0D60BB0A7BuL, 0x372049638471010DuL), (-1, -1, 0xD3AB1D0D60BB0A7BuL, 0x372049638A404A78uL)),
                ((-1, -5, 0x8DDC3C06EF51BC14uL, 0x23F54BD0A12A69EFuL), (+1, -2, 0xC01C589F9CD5D408uL, 0x4712B3D1C1333DE6uL)),
                ((+1, -7, 0x83E2C2EB92E6B07AuL, 0xC352952163A63F51uL), (-1, -4, 0xD63B9521EF10EBD6uL, 0x36583EE93DE43581uL)),
                ((-1, -10, 0x9E2C3B077A1E35D1uL, 0xF3FB1C8D5A92BC10uL), (+1, -6, 0x9F78075D7E5B03B1uL, 0x1945E4D6BB9CA556uL)),
                ((+1, -14, 0xDA85FE0B6FF2EFDCuL, 0x833CA3676C6925F5uL), (-1, -9, 0x98A6B6FEDE96F877uL, 0xFF7613430888DD61uL)),
                ((-1, -18, 0x9435FD7D9A018C35uL, 0x4738417E4D6E8D9DuL), (+1, -13, 0xAF96FB5CCD3DF986uL, 0x302E1823AFD3C9F2uL)),
                ((+1, -28, 0xA2C37F2DCBF5CA0EuL, 0xFBA0CF08EB6D83F5uL), (-1, -18, 0xB1FF856C50122775uL, 0xC9BEA628D6938CDAuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz2Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -2, 0xD555555555555555uL, 0x5555555555555555uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xA9E95481A91BEBC8uL, 0x327AF36A11C6870CuL), (+1, -5, 0xCBE4CBCECAEE4E23uL, 0x6FC6BDB27BB07135uL)),
                ((+1, -2, 0xD2CDA72BD9651C9DuL, 0x2D5EE2EA4B91CD88uL), (+1, 0, 0x8F700397770F27ECuL, 0x874FAFF8B2262B63uL)),
                ((+1, -6, 0x85A98919A3824250uL, 0xB6E3FC3C26080F79uL), (+1, -5, 0xBB674E45EFE4528DuL, 0x1B62E0FC5521EB69uL)),
                ((+1, -3, 0x8C17C1C232164ED6uL, 0x2B25A28DFCA9CD57uL), (+1, -2, 0xE4A4E9B7D3163194uL, 0xCDFA6080141E91AFuL)),
                ((+1, -9, 0xF9976C64A43896EBuL, 0xBA06BE5DBB9CB27BuL), (+1, -7, 0xE0696077325A766FuL, 0xC511E48F6F50C6FAuL)),
                ((+1, -6, 0x8CCD202CD6A6402EuL, 0x1F66DBC217F5F5DEuL), (+1, -4, 0x9A2D983F222EBF66uL, 0xE060360BB0DA1599uL)),
                ((+1, -12, 0x81A2E33B4C6350FAuL, 0x35B66D9F34C554E7uL), (+1, -10, 0xBF651844C8C88A7DuL, 0xD2F8C9285BF6B8A0uL)),
                ((+1, -11, 0x9DE24EA7327A5A5AuL, 0xE6EBF3B8F4C370F5uL), (+1, -8, 0x9FF2C0DBB77AEB5AuL, 0xD737FFB9484557ABuL)),
                ((-1, -26, 0xB5BEF3D3408B60E3uL, 0x1233FF816364B64CuL), (+1, -15, 0xA03BFEFD00FF6BF5uL, 0xEE40AD11588909EFuL)),
                ((-1, -19, 0xC5C75913EA2C883FuL, 0xEED0A085F17B7211uL), (+1, -14, 0x9FE50587A8B2A195uL, 0xA81F2BE9D49D055FuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz3Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -4, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -6, 0xCFC902A050D0C99FuL, 0xB1C656F4AF85FED8uL), (-1, -1, 0xA7E48150286864CFuL, 0xD8E32B7A57C2C14DuL)),
                ((+1, -7, 0x80A57CCBC2DADC8AuL, 0x10838C7DD5D76D31uL), (+1, -2, 0x8F11CBDAC26E6D79uL, 0xC18028C8DE378702uL)),
                ((-1, -11, 0xC1B5A7BAB95A9ECDuL, 0xEE352D700DEF9D3BuL), (-1, -4, 0x8FA45650631E12BCuL, 0x35A78C6CE256C7A6uL)),
                ((+1, -18, 0xFBF7256BBEF7B5F1uL, 0x05538DAE2690DB75uL), (+1, -7, 0xD413C4E128AA3118uL, 0xEF4E5F2BBA23F871uL)),
                ((+1, -16, 0xDE0F8F14A0DD4E37uL, 0xEF7D56A5080A3568uL), (-1, -10, 0xC692D9645097C417uL, 0x8CE961B4F27F1152uL)),
                ((-1, -19, 0xB82EC5E1D3110DABuL, 0xAB725691F1747E7BuL), (+1, -13, 0x8C90B6C4F0E6B6ABuL, 0xC39EC1BCBC64F7C6uL)),
                ((+1, -22, 0x8ACBB48B41F7B719uL, 0xDF09BB5B8BE9CC79uL), (-1, -18, 0xD477328916AAFCE2uL, 0xFFF7FB35527EF3A9uL)),
                ((-1, -30, 0xE51AD35E814349D2uL, 0x2A1FA63EAB07E27AuL), (+1, -22, 0xA73B772FE94B286FuL, 0xC5CE84A6681A5F03uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz4Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -5, 0x8888888888888888uL, 0x8888888888888888uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -6, 0x9BA289FEC9A82874uL, 0x699380211E0BF1C5uL), (-1, -1, 0x91E8615EDD0DA5EDuL, 0x22FA481F068B3735uL)),
                ((+1, -11, 0x9ED5FCA69800695CuL, 0xF15B4AB915F9DBE4uL), (+1, -9, 0xCDF41EA1BCD4701BuL, 0x7D20036EE60277C6uL)),
                ((-1, -12, 0xB3E4F6F1AA4042BEuL, 0xA61465CDF431843FuL), (-1, -10, 0xE20AE947E7313709uL, 0x7CFC0F56B0B99E78uL)),
                ((+1, -18, 0xBB3CCE7F498B5C22uL, 0xAF65DA24E059ABBFuL), (+1, -13, 0xBFE2DE9E59EBD9B3uL, 0x92BBFACC75065FB4uL)),
                ((-1, -19, 0xD31EF5E8556F4FC9uL, 0x0E919826454C23C3uL), (-1, -14, 0xDAA9CC30379D6632uL, 0x9ECF60A3AA362F4AuL)),
                ((+1, -25, 0xD104F9BD23DDDE91uL, 0x16A270063B9E118CuL), (+1, -21, 0x9CD1980B71C982CAuL, 0xF5C69F760E17A72CuL)),
                ((-1, -26, 0xED2710A68588830AuL, 0x6B2AD5DC6CA3D572uL), (-1, -22, 0xAEB2421A771F39D3uL, 0x1E3DDA03A87B3063uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz5Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -6, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -10, 0xD5AA0AE5A66E71C0uL, 0x88C20B459E6BE90FuL), (-1, -4, 0xA03F882C3CD2D550uL, 0x6691887436D0EC13uL)),
                ((+1, -13, 0xD870A9943D292309uL, 0xB70250D2FFC33115uL), (+1, -7, 0x9727CC64012C0F1AuL, 0x96768FEB73FF933FuL)),
                ((-1, -17, 0xC1D151F6B5518979uL, 0x560079F7DD52A703uL), (-1, -11, 0xD8B4E76D0ACE4D5FuL, 0x284F149A5997B2B2uL)),
                ((+1, -21, 0xE7D2D48D56EED94DuL, 0xC54FFEF2A92EEC41uL), (+1, -15, 0xF5F46F575556ED4EuL, 0xF462411F3DB87C69uL)),
                ((-1, -26, 0x83295540C1EEF671uL, 0xBCE1D0083FE60D89uL), (-1, -19, 0x880F9E68262E7C4BuL, 0x1663A8B3D48A9F85uL)),
                ((+1, -33, 0xB498019811E11605uL, 0x5E8F235888A178F5uL), (+1, -23, 0x876813C8E72DC80CuL, 0xFA593DA7C9FB53B1uL)),
                ((+1, -34, 0x83B2E94BD19A3D27uL, 0x77F36C77CBC2C413uL), (-1, -29, 0xC42D682D88D1F745uL, 0xF9DB915A7FFA3D06uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz6Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0xEA0EA0EA0EA0EA0EuL, 0xA0EA0EA0EA0EA0EAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -28, 0xB3BAC28E2F46E690uL, 0xBAF33249EDB72C7DuL), (+1, -22, 0xC49444CB83B58C2EuL, 0x4C79FED2B83C97EFuL)),
                ((+1, -17, 0xF5EFE06CA69B73D9uL, 0xDCA93ECE0F5C7320uL), (+1, -11, 0xBED5D2B3A65ECE7FuL, 0x1DABDE50413AF462uL)),
                ((+1, -37, 0x95AF4E9D9C4DB0F4uL, 0x555B209AF35CDD45uL), (+1, -31, 0x85B596A51A9B31ABuL, 0xF671B47978DA3D8AuL)),
                ((+1, -24, 0x8FFB1E8D1E71F4B2uL, 0xA6A3DB409E3CE739uL), (-1, -21, 0xA1E7739E44A0F8E7uL, 0xFAD370FF304C8C6DuL)),
                ((+1, -45, 0x963ECE16ADB207A1uL, 0x4D91473F8F511AAFuL), (+1, -42, 0xBA01EBC86C16B043uL, 0x57E1F23302992150uL)),
                ((+1, -34, 0xFC9A41E00AE56164uL, 0xD38D3DF91126A93FuL), (+1, -27, 0xC6412B52778AABB5uL, 0x0ED571FBBB9D919FuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz7Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -11, 0x88005A12CF9A4865uL, 0x74AEA3769C0C70E2uL), (-1, -5, 0xCC00871C37676C98uL, 0x2F05F531E9E46A49uL)),
                ((+1, -16, 0x87FA5ECCA32FF8E7uL, 0xB1C976FF9247E820uL), (+1, -10, 0xB80E4F9F0B896172uL, 0x4C1A492577A97641uL)),
                ((-1, -21, 0xC501C553BE1CCF44uL, 0xFDF6346B3936018EuL), (-1, -14, 0x83E36B9EBEE5BDACuL, 0x2C964983116FD0B6uL)),
                ((+1, -26, 0xA5207148EA72F057uL, 0x4447A72DA1555E29uL), (+1, -20, 0xD05041415AB7E834uL, 0xFCF90DBEE2EBCE71uL)),
                ((-1, -32, 0xD919FAC1DE32B99CuL, 0x0B887C8D31FD8E2AuL), (-1, -24, 0x956CA61AD0FF1322uL, 0x30B1E596FE99CB33uL)),
                ((+1, -40, 0xEA425366452ACE3EuL, 0xC5AF044D02301FFFuL), (+1, -29, 0x88A51F4C6998820DuL, 0x18CBA36D9D2334EDuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz8Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0x8208208208208208uL, 0x2082082082082082uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -31, 0xBAC579F912D50C73uL, 0x77B70AD2B3673DC1uL), (+1, -24, 0xB7DA64112E89B841uL, 0xA9D82EA685CD7076uL)),
                ((+1, -18, 0xAB52DE474EF71A2AuL, 0xA36DEAC97A71DC98uL), (+1, -11, 0x922632F8B66BA94AuL, 0xB98CBFEF6348802EuL)),
                ((+1, -41, 0xB995CE3422541194uL, 0xEC9AF8C3917D6C11uL), (+1, -34, 0xA6873836D6DE1D7CuL, 0xA3C5837549781F05uL)),
                ((+1, -29, 0x9946357E079AD50DuL, 0x1EEBB26E34DC842DuL), (+1, -23, 0xE02CACFA0D8B35B9uL, 0xD201432EE2CA95F3uL)),
                ((-1, -49, 0xBABEC29A4CBC131AuL, 0x97DD91DDF9B5ADF4uL), (-1, -42, 0xBCA99050BC1173A0uL, 0x92B96AFDEF675820uL)),
                ((+1, -39, 0xD02780BB5B9181FBuL, 0x6E3268F5DCC7059FuL), (+1, -36, 0xEC0621D1FB2EB942uL, 0xBA6F5E754626532DuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz9Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0xCCCCCCCCCCCCCCCCuL, 0xCCCCCCCCCCCCCCCCuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -12, 0xD1B58B20ED182481uL, 0x8878CAFB6D60CE04uL), (-1, -4, 0x831176F4942F16D0uL, 0xF54B7EDD245BAFB4uL)),
                ((+1, -18, 0x918787E66B424287uL, 0x0D3C59EE6907B6FCuL), (+1, -11, 0xA836CFE04A29B2D7uL, 0xF561D06C56EF7544uL)),
                ((-1, -22, 0x8D871810E12E1A74uL, 0x62CA09DF50DD2956uL), (-1, -15, 0xA2E2360618E9383AuL, 0x0623ED89E83DB067uL)),
                ((+1, -29, 0x937E6DA97024AC73uL, 0x275ECCC800D82B06uL), (+1, -22, 0xA1A783BF85957C35uL, 0x226E5205B20F1F53uL)),
                ((-1, -33, 0x9684A91F8B926178uL, 0x7B5A5CA120A56E8DuL), (-1, -26, 0xA5E46BBC2D2699ECuL, 0x7561014524109923uL)),
                ((+1, -37, 0xC2377FDEC34DB674uL, 0xABE386580028279FuL), (+1, -30, 0xEF8232028F149A0DuL, 0x983ADCBD035873F0uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz10Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0xA57EB50295FAD40AuL, 0x57EB50295FAD40A5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -35, 0xBF7C6B3C1A9C2C4BuL, 0xC626C523B6191EB8uL), (+1, -27, 0x941A3AF07C94CA42uL, 0x9B41FC79896C6DC9uL)),
                ((+1, -19, 0x92B0D0148184B609uL, 0xAC5CBC52F3463356uL), (+1, -12, 0xD1415270BCB5F5E0uL, 0x3407CC89F6912F1EuL)),
                ((-1, -40, 0xA5D809AAF9B626EFuL, 0x9C85D95AFABDBF72uL), (-1, -32, 0x806DF3A483D1EF66uL, 0x2152F9102A736B75uL)),
                ((+1, -31, 0xAD2DDF60E3234746uL, 0x6F5887447242D405uL), (+1, -24, 0xE75B7E7A5ADF7220uL, 0xD9478AC8DAAADFACuL)),
                ((+1, -54, 0xC33DFD37EB3E7C78uL, 0x8335A1802D22663BuL), (+1, -46, 0xDD986B5345F30119uL, 0x316E0E1BCB0DB693uL)),
                ((-1, -40, 0x84F09579A05096D9uL, 0x572A997B2FF20EDDuL), (-1, -33, 0xD3D93B8DBD2C1DEDuL, 0x56282060A6D736DBuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz11Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0x8888888888888888uL, 0x8888888888888888uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -18, 0x8E2F4F4E64528BDBuL, 0x88FA101A582C594BuL), (+1, -10, 0x854C5A597E0D631DuL, 0xD06A6F1707A722DAuL)),
                ((-1, -21, 0xCA813094A8064C74uL, 0x82DA7DBB260E9673uL), (-1, -13, 0xD5A7673CC56932AEuL, 0xB7914B011681F535uL)),
                ((-1, -20, 0xA0ADCA0A32D68309uL, 0x4F34AB6C2110FF2FuL), (-1, -12, 0x96A606BB0B221A05uL, 0x21356E382D67F331uL)),
                ((+1, -36, 0x871682F94C6CD9F8uL, 0x05545D0D3C47432FuL), (+1, -28, 0xE6BC0C82B3E3162BuL, 0x6B3982E3BE76CA89uL)),
                ((+1, -35, 0xFA9DCF48BA38B681uL, 0x0A62481DD0A03D44uL), (+1, -26, 0xAD7FA57295A5C696uL, 0xD54C3501FA88E7C9uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeAz12Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0xE525982AF70C880EuL, 0x525982AF70C880E5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -9, 0xB7CBD681A24371ECuL, 0x6E235EB3DB13B3BAuL), (-1, -1, 0xCD55B9A4D3475946uL, 0x230B83CCF078923CuL)),
                ((-1, -22, 0xE1060BFAA9D9E37AuL, 0x2F9FA981AFEAE0E7uL), (-1, -13, 0x8E56D0AF6F0D53F9uL, 0x3AC43D2374564279uL)),
                ((+1, -22, 0xB2CF681941613D4CuL, 0x5CA5914ACF9F3CB1uL), (+1, -14, 0xE27636FF240E23DDuL, 0x2DCDD4FEF646489EuL)),
                ((+1, -38, 0xC8313C7438C0F61FuL, 0x14AAF9606665ECF7uL), (+1, -29, 0xCEE480FB551B7665uL, 0x8268F79BDA28A268uL)),
                ((-1, -38, 0x9CF91EB1ECF59CFDuL, 0xDC5FB892B07D12E7uL), (-1, -29, 0xA2F5E3E3FE76EC95uL, 0xCA0B9B801A31E77AuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz1Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((-1, -3, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -4, 0xB68F432E6CC0A49AuL, 0x1DCFE3C831C111FCuL), (+1, -1, 0xD68F432E6CC0A49AuL, 0x1DCFE3C8322D7643uL)),
                ((-1, -5, 0x919FFBE4E54C9C5EuL, 0x1492FBCAD34FA51BuL), (+1, -2, 0xC4992205D5D21AD9uL, 0xF15C4A0B9D2B8C31uL)),
                ((-1, -7, 0x890685523E538B97uL, 0x9D84ED91191A08BDuL), (+1, -4, 0xDD7EE54FABD103EFuL, 0xF166F53429B867D4uL)),
                ((-1, -10, 0xA61C2CEEB928A877uL, 0x62E195DC77C0B4C2uL), (+1, -6, 0xA67962EEB85C9CB9uL, 0x48EBD57CF259BC3FuL)),
                ((-1, -14, 0xE877988B3A2AEA96uL, 0x10FFA5F51F01026BuL), (+1, -9, 0xA10D7F70961A6A84uL, 0x0BDDE3766FFF43CBuL)),
                ((-1, -18, 0x9F98F9017864660FuL, 0x956D81C044371480uL), (+1, -13, 0xBB3F8199C0E0E776uL, 0x60DC9BB3361D26ADuL)),
                ((-1, -28, 0xBBB270738E76096AuL, 0x1D661662AB753927uL), (+1, -18, 0xC030775AFA3DA23AuL, 0x33600D8A870CD77FuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz2Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((-1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -5, 0xC17183BDEF6E08D0uL, 0x585EC4B2E69B5C99uL), (-1, -1, 0x911522CE7392869CuL, 0x424713862CECB64AuL)),
                ((-1, -9, 0x9035970365EE4B41uL, 0xD445CCAAC5147CBCuL), (+1, -6, 0xFBDE9B68A71E5470uL, 0xF74C4131D7A8D904uL)),
                ((+1, -10, 0xA3A05B480A98263DuL, 0xB66236CCAF527522uL), (-1, -6, 0x8EDEC22F26383D99uL, 0x51FE2C03112E3F8AuL)),
                ((-1, -17, 0xED0F150764A37937uL, 0x6E36F84931C918B5uL), (+1, -12, 0x8806BB0902114369uL, 0x2E3C6BE982B635DBuL)),
                ((+1, -17, 0x86B2ED3D2BCB0C89uL, 0xFBFF8CBD3B666F75uL), (-1, -13, 0x9A83B5C860D4118EuL, 0x82D51B66E195E09BuL)),
                ((-1, -27, 0xE65672189B988749uL, 0x57AA6400A69A5C39uL), (+1, -21, 0x883F1B2A2ED88899uL, 0xF4F819AB92B2FA44uL)),
                ((+1, -27, 0x833908A220D1D338uL, 0x56C53808D23A9D15uL), (-1, -22, 0x9B101023BAE356BFuL, 0xB848DE205F3C6717uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz3Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -4, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -6, 0xC69C73C65A91D5C8uL, 0xB6C1D5B257E1A011uL), (+1, -1, 0xA34E39E32D48EAE4uL, 0x5B60EAD92BF05F9DuL)),
                ((+1, -8, 0xF01E7BDC3C8A671DuL, 0xD3CCAEF3DC3D7A0EuL), (+1, -2, 0x887B88B57293DC06uL, 0x6F706DF663D21FDAuL)),
                ((+1, -11, 0xA2047DC0D597C650uL, 0xD8C021AC961E3324uL), (+1, -4, 0x857756ECB198E821uL, 0x2FE89216EDE5F106uL)),
                ((-1, -21, 0xC304C9B619B11F51uL, 0xAE814E07243B2BD5uL), (+1, -7, 0xC12415F449092BF4uL, 0xA5A5FDB348AD36F0uL)),
                ((-1, -16, 0xCDF149108195C700uL, 0x90FF4BD088FC7ACEuL), (+1, -10, 0xB0748EB3C44D96A3uL, 0x12773D88E7C0D6DFuL)),
                ((-1, -19, 0x9D30E28CFE9A1D26uL, 0x8D09CA172863D965uL), (+1, -14, 0xF942A7DD33714DB8uL, 0x68D4CB5995662261uL)),
                ((-1, -23, 0xF396EA47057E968FuL, 0x4DF09A7BF5FD9DD4uL), (+1, -18, 0xBDCF6ABF7C070933uL, 0x37E027223A6337AFuL)),
                ((-1, -29, 0x95295D3BF5A2C33CuL, 0x6F0CF4300B0E98CAuL), (+1, -22, 0x9BCA702F7D79E82EuL, 0x57CF08C6F21093DCuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz4Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -5, 0x8888888888888888uL, 0x8888888888888888uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -21, 0x8A350B89D4242DB0uL, 0x5AB028D7F43180C5uL), (-1, -16, 0x8191BAD136E1EAD5uL, 0x55050C7E235707F8uL)),
                ((+1, -11, 0xD7BFC86797F00D23uL, 0xE5A649411AA2983CuL), (+1, -5, 0x923784173E704671uL, 0xA4FFC8081DE2ABDFuL)),
                ((-1, -28, 0xB79EA06816D6400FuL, 0xDFFE47D72BEB3C30uL), (-1, -22, 0xB158968B48680E46uL, 0x823720B99A8BE686uL)),
                ((+1, -19, 0xD1B72EEE50F6CD7BuL, 0x81DCBD5D34DAD79EuL), (+1, -12, 0xC51490349476B676uL, 0x8A75D18EB35501E1uL)),
                ((+1, -36, 0xA6DFDAA3295FDA58uL, 0xB7D8D6C2C86702D8uL), (-1, -30, 0xBA59E3D60A359BD5uL, 0x256EA86A093C1475uL)),
                ((-1, -26, 0x9228D79D1413D8EDuL, 0xE447DF331B2EBA09uL), (+1, -20, 0x905E1FFF49C6B01EuL, 0xD1E1022B9D54731FuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz5Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -6, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -10, 0xD4C202B1783FC2D8uL, 0x1EC1E42B9ED8987BuL), (+1, -4, 0x9F9182051A2FD222uL, 0x17116B20B7225F08uL)),
                ((+1, -13, 0xD8D70390ACF63482uL, 0xEE5CE379B980A94FuL), (+1, -7, 0x97748FE15505DC35uL, 0x7FFA7DE884705B4AuL)),
                ((+1, -17, 0xC17B24930092DD27uL, 0x4E46D1BEC7A1892FuL), (+1, -11, 0xD88376AED27629DEuL, 0x911315B7D9DB7814uL)),
                ((+1, -21, 0xE864BCE37FF8869CuL, 0x6D2EF0F16F30A643uL), (+1, -15, 0xF5E07247790D8342uL, 0xD179548E7512A8A1uL)),
                ((+1, -26, 0x81BE9D856A893803uL, 0x280363D1CEF4DF0EuL), (+1, -19, 0x8801F08DC83F9FC7uL, 0xD1DE943EBAEBE07DuL)),
                ((+1, -33, 0xBB140A41289A6EAEuL, 0x3B2976D9EA615BF2uL), (+1, -23, 0x87C7AD52BD5EB007uL, 0x4EC4FCDCC36378DBuL)),
                ((-1, -34, 0x846E2D7454AAD116uL, 0xAD5FD5D804391FE1uL), (+1, -29, 0xC1E72447922FF9E0uL, 0xDC85E520579CF6F7uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz6Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0xEA0EA0EA0EA0EA0EuL, 0xA0EA0EA0EA0EA0EAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -24, 0xB17D89E4EA71451FuL, 0x256DE001CDBAC77FuL), (-1, -18, 0xC2214ED2606BE39AuL, 0x10F02CF7E65B20E0uL)),
                ((+1, -15, 0x9C412850481B1BDAuL, 0xE8D5F5DD7810E187uL), (+1, -9, 0x975D216702E6D869uL, 0xA3BEB5339D7BC005uL)),
                ((-1, -33, 0xE6F28C418DB483F8uL, 0x0847AB473C658D35uL), (-1, -27, 0xDEF6D72E81CA11D3uL, 0xE312D4F073B55BF2uL)),
                ((+1, -27, 0xE385278CEBAD60F7uL, 0x1D95853F967F09DEuL), (+1, -18, 0x9C24C54728416FA3uL, 0xAF06446E5A625FA8uL)),
                ((-1, -44, 0xBD00C001930102ADuL, 0x066E7EFD924C4F04uL), (-1, -36, 0xF2532D8CB2989FFCuL, 0x7045B99216C53701uL)),
                ((-1, -35, 0xEF97FD2D90FF05A3uL, 0x106D1C611928DEC2uL), (-1, -29, 0xEFB07FC33AEAA469uL, 0x35516B8159BBA3E1uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz7Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -11, 0x95C3F2CD12D7DF15uL, 0xF87F7C3153C840EAuL), (+1, -5, 0xE0A5EC339C43CEA0uL, 0xF4BF3A49FD7823C1uL)),
                ((+1, -16, 0x8AFCD85BA1CF6926uL, 0x1B384BBD8C1010B4uL), (+1, -10, 0xBC9205F5897889CFuL, 0xEA40887CA65D2293uL)),
                ((+1, -21, 0xD6760DFC196F0C20uL, 0xA6DDB213D4D2AD5AuL), (+1, -14, 0x8F5F8C074857F79AuL, 0x31382E72493BD1EAuL)),
                ((+1, -26, 0xAB50A231642312D4uL, 0x71EFE86CCB4A6820uL), (+1, -20, 0xD8E4C3964D4E1B59uL, 0xD62BE70172922D12uL)),
                ((+1, -32, 0xFC1E44F0075FBE98uL, 0x2C33D95748E3992DuL), (+1, -24, 0x9FE70C640C8314BAuL, 0xB90D21042D39EDB0uL)),
                ((+1, -41, 0xABC27CD8C9F8C5B1uL, 0x1183202D9C3744C3uL), (+1, -29, 0x907E4A5C20347F24uL, 0xDAE611D263616278uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz8Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -7, 0x8208208208208208uL, 0x2082082082082082uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -32, 0xAD5390F07BB1048FuL, 0xC516105515D5AAACuL), (+1, -25, 0xAA9E42ACB9C2407DuL, 0x8601B8134E8A65B5uL)),
                ((+1, -18, 0xF6B1DCB40214881FuL, 0x0B86AB7C7C2EA775uL), (+1, -11, 0xDC57B56BB6BCA187uL, 0x50051D9F8037CC2EuL)),
                ((+1, -42, 0xB6EA62D21AFC0535uL, 0x7043BC4572A93460uL), (+1, -35, 0xA5103B29E44E95BFuL, 0x9F78A524DB254C5CuL)),
                ((+1, -28, 0xAD9882AED67553BBuL, 0x30ACD9B52E3E717AuL), (+1, -21, 0x90F79202BF54E8B4uL, 0xE7390769C5BB743BuL)),
                ((+1, -50, 0x92BA93DA31FEBEFFuL, 0xA1CB74F22DFCF264uL), (+1, -43, 0x8BB7CBE016266188uL, 0x5C979D9E1118D0C1uL)),
                ((+1, -40, 0xB064DAF97F1642A4uL, 0x585916F550A3E19AuL), (+1, -32, 0xD21F5936E00252F1uL, 0x41397D86E3AD6778uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz9Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0xCCCCCCCCCCCCCCCCuL, 0xCCCCCCCCCCCCCCCCuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -12, 0xFD8363D3FD463B44uL, 0x042F92C7F43F6639uL), (+1, -4, 0x9E721E647E4BE50AuL, 0x829DBBBCF8A6A3EEuL)),
                ((+1, -18, 0x9371A61346B929ACuL, 0x55A7CB761870F217uL), (+1, -11, 0xAA9B75985C7E53C6uL, 0x8FE81E5A3464A688uL)),
                ((+1, -22, 0xAAB46B4B4D5ABB96uL, 0xAAB2FC3FF98138F1uL), (+1, -15, 0xC46CD9B4AFD61C61uL, 0xACDD7208074C41D7uL)),
                ((+1, -29, 0x97EFA474EC5F7133uL, 0x13AED02E478B15B3uL), (+1, -22, 0xA6F37891A4ED4B94uL, 0x1BB5AFA87AEEE84FuL)),
                ((+1, -33, 0x8CCBA27C60C55E8BuL, 0xD12C11C51D290F3CuL), (+1, -26, 0x952441D2F560A7B6uL, 0x740E54D6F46A3BDBuL)),
                ((+1, -37, 0xEC4C388CD942E1A2uL, 0xFAE9979BF995C8A6uL), (+1, -29, 0x920391E93E13B0E3uL, 0x517EA15A44A23137uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz10Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0xA57EB50295FAD40AuL, 0x57EB50295FAD40A5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -36, 0xD1ACDA3E6B3CF818uL, 0xD8A340646D92F8BEuL), (+1, -28, 0xA22BB0CC46F127E3uL, 0x378E43CD97086133uL)),
                ((+1, -19, 0x9BCCA0DEA7C4FE7DuL, 0xBC451314F0381948uL), (+1, -12, 0xDF5859696FE165F3uL, 0xBCA332BE09747AD5uL)),
                ((+1, -41, 0xB0B34D643A65F236uL, 0xE769E2FB64B71B95uL), (+1, -33, 0x887DF015AD8137F6uL, 0xDBAB02298F90DAC5uL)),
                ((+1, -31, 0xC80A0939DC3462B7uL, 0x955E131961439E54uL), (+1, -23, 0x877B3E6B90F1447AuL, 0x528CB3C76A3ED9B6uL)),
                ((-1, -55, 0xD28A411BB565D393uL, 0xF5284922CF47FC04uL), (-1, -47, 0xEE74E1F20A66E4F5uL, 0x072AFDABA0999EE2uL)),
                ((+1, -41, 0xD7ECE64C774D5E95uL, 0x3FB5D4022EDD9F35uL), (+1, -33, 0xA002533F45C4A27EuL, 0x95401F96F54B6BD2uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz11Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -8, 0x8888888888888888uL, 0x8888888888888888uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -19, 0x9C322E6D05ECEBFEuL, 0xD09DAE759627DA99uL), (+1, -11, 0x926F0B86358E1D3EuL, 0xE393D38F16D0E0D4uL)),
                ((-1, -21, 0xCA9554CEECF39320uL, 0xCFC74E97A5AE56C7uL), (-1, -13, 0xD5BA49336607A4F0uL, 0x3FAF7EB1F9D8508FuL)),
                ((+1, -20, 0xA0761749C91E8F14uL, 0x07634B3BCC914042uL), (+1, -12, 0x966D0215FB1D7CA9uL, 0x6524B251BE61763CuL)),
                ((+1, -36, 0x8735F043DA0FC86EuL, 0x5C17C86ED0B41B6DuL), (+1, -28, 0xE6E78F1BFBF179ACuL, 0x12D965D7CCBEA093uL)),
                ((-1, -35, 0xFA6B61F6F0AA5407uL, 0x7A104B6D6119D8C3uL), (-1, -26, 0xAD5854C7FC49CCC2uL, 0x59F86A9D171F818CuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeBz12Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0xE525982AF70C880EuL, 0x525982AF70C880E5uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -9, 0xB7CBD2D5651352FCuL, 0x98130FA4B4C9AFEEuL), (-1, -1, 0xCD55B58A66EB96B6uL, 0x31E54B7A0041FD3EuL)),
                ((-1, -22, 0xE25164095F257FAEuL, 0x2D610313F3D03091uL), (-1, -13, 0x8F0FE6DFA652903AuL, 0x4583D973ED22E3A6uL)),
                ((+1, -22, 0xB73389F40D13C15AuL, 0x4C661EC3FA5119A4uL), (+1, -14, 0xE75E144900A233B3uL, 0x90F87CD2230EAA46uL)),
                ((+1, -38, 0xCAF78BC29D368C62uL, 0xFD42B24AA060BDC8uL), (+1, -29, 0xD131CBAB4DAFCD40uL, 0x3D8546C564A87066uL)),
                ((-1, -38, 0xA662F68BBE1C6443uL, 0x3F6B035CB09EEE54uL), (-1, -29, 0xAAC5286E663C331AuL, 0x996E87604FA2ECAEuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeABz13Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0xC30C30C30C30C30CuL, 0x30C30C30C30C30C3uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, -38, 0xAC66E2E3E3B43F19uL, 0xB870179AB36ECFEFuL), (-1, -30, 0xE24709CB1ADC92D1uL, 0xC227E2C955F0D921uL)),
                ((-1, -22, 0x84F4DC53A6C3C1B8uL, 0x9F14A18CB1B73A9AuL), (-1, -14, 0xC67C3A9F89E24568uL, 0x62C1AF773F894204uL)),
                ((+1, -53, 0x83553827708D12FAuL, 0x5AD6622560504FFBuL), (+1, -44, 0x809423C10457B1A0uL, 0x3332CA321AC8D81FuL)),
                ((+1, -39, 0xA5538535DFF9C730uL, 0x971D7DDF1B154F46uL), (+1, -30, 0xCABACD8C2712E93CuL, 0x7B88AEB17E7F96CCuL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeABz14Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0xA80A80A80A80A80AuL, 0x80A80A80A80A80A8uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -41, 0x946B172FC792AFCDuL, 0x891BFB8405BB25CAuL), (+1, -33, 0xE21B2552CA0977D3uL, 0x1EE6FA0043F728EEuL)),
                ((-1, -23, 0xA403653DD70232BAuL, 0x5684917099B6C061uL), (-1, -14, 0x8EA6D514414C8738uL, 0xBCA5FF1E04595923uL)),
                ((-1, -57, 0xA54CBE05308B07ABuL, 0x390F9F42CAF1F43EuL), (-1, -48, 0xBC8397232C407757uL, 0x3E18B240178B080AuL)),
                ((+1, -40, 0x92959FCAE8C907A0uL, 0xD35E8BC09B117BC0uL), (+1, -31, 0xD28BAEF726A1C544uL, 0x40C658264580D506uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeABz15Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0x9249249249249249uL, 0x2492492492492492uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -42, 0x87EF9EAA790D1C64uL, 0x1ECF24A57DFB1C96uL), (+1, -34, 0xEDE355AA53D6F1AFuL, 0x35ED4E258398476EuL)),
                ((-1, -24, 0xD22F7367CD94F2F8uL, 0xE0484E5A8A934341uL), (-1, -15, 0xD2AC44834D7D4ACAuL, 0x3AA32C6F0F3A6ED6uL)),
                ((-1, -59, 0xE2A9D1EA1C2DD24BuL, 0x512E60627717A521uL), (-1, -49, 0x94E669C1B8E2339DuL, 0xAE75B700B1D900C1uL)),
                ((+1, -41, 0x8AA7E8B4521BA1A7uL, 0x402A38850A5713F6uL), (+1, -32, 0xE680CBE22007CBECuL, 0x3168A5B83285EE02uL)),
            }),
        });
        public static ReadOnlyCollection<ReadOnlyCollection<(Hexcode c, Hexcode d)>> PadeABz16Table = Array.AsReadOnly(new ReadOnlyCollection<(Hexcode c, Hexcode d)>[] {
            new ReadOnlyCollection<(Hexcode c, Hexcode d)>(new (Hexcode c, Hexcode d)[] {
                ((+1, -9, 0x8080808080808080uL, 0x8080808080808080uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -43, 0x8243BE2DCF201685uL, 0xE24B9D0C4C0E311CuL), (+1, -34, 0x81C17A6FA150F66FuL, 0x5C69FF314A58AA59uL)),
                ((-1, -24, 0x8B1B50F765543D2BuL, 0xE40CB0AAD0291A7CuL), (-1, -15, 0x9F27058AD48E9D25uL, 0x27ACAA5BD75019DBuL)),
                ((-1, -60, 0xA5EDA070FF34CFA5uL, 0xA9223BDCC17745FAuL), (-1, -51, 0xF8C4253DF0F67F96uL, 0xC322E92225F9B6C8uL)),
                ((+1, -42, 0x8A8F7ED95ACA927BuL, 0x8A08EBF5E412400EuL), (+1, -32, 0x83EFC8713D44253AuL, 0x58EB6D075E8B9D10uL)),
            }),
        });
    }
}
