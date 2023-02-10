using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class InverseErfTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<Hexcode>> tables = new(){
                { nameof(CoefTable), CoefTable },
            };

            foreach (var key in tables.Keys) {
                stream.Write(key);
                stream.Write((UInt32)tables[key].Count);
                foreach (Hexcode v in tables[key]) {
                    stream.Write((UInt64)v.Hi);
                    stream.Write((UInt64)v.Lo);
                }
                stream.Write((UInt32)0u);
            }
        }

        static readonly ReadOnlyCollection<Hexcode> CoefTable = new(new Hexcode[]{
            (+1, -1, 0xE2DFC48DA77B553CuL, 0xE1D82906AEDC9C1FuL),
            (+1, -3, 0xED94FD97F72F1FB0uL, 0x57441C887E2D6412uL),
            (+1, -3, 0x829E16055C8E2839uL, 0xD8D1B5FE1B00F73FuL),
            (+1, -4, 0xB1423E23EED23DE2uL, 0xC5DFC73EEBBB5BDCuL),
            (+1, -4, 0x85098C4E377BCDB9uL, 0x35E45C58E5994898uL),
            (+1, -5, 0xD3E42E44DD845AF1uL, 0x22522272043C9234uL),
            (+1, -5, 0xAF758EA447189346uL, 0x81408B4ED5821C2DuL),
            (+1, -5, 0x955D4C5CC635E195uL, 0x302078D6EA739573uL),
            (+1, -5, 0x81CC54835E841119uL, 0xA2949561880ACC2EuL),
            (+1, -6, 0xE537A3F0316E38B5uL, 0x2CE53AD70DD9B645uL),
            (+1, -6, 0xCCFB840D1A1D4D4AuL, 0x6AD479307296BCB5uL),
            (+1, -6, 0xB93851F0B1B9ECA3uL, 0xC776B854FB1999B0uL),
            (+1, -6, 0xA8CF8CF9B9A78972uL, 0xEDADE085788B12A1uL),
            (+1, -6, 0x9AFA608C80C379FAuL, 0x92AD376AD7246ED1uL),
            (+1, -6, 0x8F2A9B00A5DAE4CBuL, 0x9788F624EBF08908uL),
            (+1, -6, 0x84F7ECBD06F64556uL, 0xA7C73A6D40B7ADCDuL),
            (+1, -7, 0xF8283FF67FFBCA3CuL, 0x5A164A8EDDC2373CuL),
            (+1, -7, 0xE886E3457530CEC5uL, 0x517D6EB3FEAC5C72uL),
            (+1, -7, 0xDAAEE72DCE7369BBuL, 0x606B693E0E552748uL),
            (+1, -7, 0xCE5715C3130548DEuL, 0x50780A48061BE04BuL),
            (+1, -7, 0xC344F4D155A9F32CuL, 0x1A547CA5B3B40EEFuL),
            (+1, -7, 0xB9494230688342DAuL, 0x80F4F1C69C165460uL),
            (+1, -7, 0xB03D6538BD011AFDuL, 0x69A6722A99731CEBuL),
            (+1, -7, 0xA8018AF4D998DB98uL, 0x3A1D6DC80EF8C27CuL),
            (+1, -7, 0xA07B3B5C94E19E9AuL, 0xA15F1CE1F9E08334uL),
            (+1, -7, 0x999445C0F4908444uL, 0x7424A2F485F158C1uL),
            (+1, -7, 0x9339ECF2B492E67DuL, 0x7E669712E943D9A8uL),
            (+1, -7, 0x8D5C429D79B3C94DuL, 0xF22CB9C4811E2EF1uL),
            (+1, -7, 0x87EDA60893234979uL, 0x8F4F8906130CBC08uL),
            (+1, -7, 0x82E25DAF6F3AD76BuL, 0xDEA6CD7E63BC36ABuL),
            (+1, -8, 0xFC608AD7579C8013uL, 0x972DC06A8648C0F5uL),
            (+1, -8, 0xF39D191265C332A2uL, 0xB67741AEB2768877uL),
            (+1, -8, 0xEB6B008DC9CCB7ACuL, 0xD52D3F8A8FE4053EuL),
            (+1, -8, 0xE3BCC03E47BA8119uL, 0x3642E26BD760FB46uL),
            (+1, -8, 0xDC8673A605D52B25uL, 0x09AC7079AFD836F2uL),
            (+1, -8, 0xD5BD9752992F8721uL, 0x90671625992E1530uL),
            (+1, -8, 0xCF58D756C1E900C1uL, 0x9D34A5897E4A1224uL),
            (+1, -8, 0xC94FE5DA39B99937uL, 0xF1259E286B9594CCuL),
            (+1, -8, 0xC39B58407A825B96uL, 0xF743180609D1B947uL),
            (+1, -8, 0xBE3489B634CF0853uL, 0xFB06BB4601B91BCEuL),
            (+1, -8, 0xB91582323C669A31uL, 0x8CCC075B34D9652AuL),
            (+1, -8, 0xB438E12651544177uL, 0xB45502DD237959C4uL),
            (+1, -8, 0xAF99CB41919946B8uL, 0x88597F83FA214BBDuL),
            (+1, -8, 0xAB33DAC3ECB27492uL, 0x250F51BE81B0BECAuL),
            (+1, -8, 0xA70311F96F81E2EAuL, 0x463558383504A0A3uL),
            (+1, -8, 0xA303CF870661C1DEuL, 0x9C1932BD7F1122BDuL),
            (+1, -8, 0x9F32C4416ADD7DD9uL, 0xEF2A8972E13AAAD5uL),
            (+1, -8, 0x9B8CEA5323F5F5FAuL, 0xEE3A331447E32A7CuL),
            (+1, -8, 0x980F7D8065EC86B8uL, 0x53A1441210277610uL),
        });
    }
}
