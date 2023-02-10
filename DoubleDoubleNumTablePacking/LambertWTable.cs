using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class LambertWTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode c, Hexcode d)>> tables = new(){
                { nameof(NearSingularPadeTable), NearSingularPadeTable },
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

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> NearSingularPadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((-1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((-1, 2, 0x9478FB18D3F20BC2uL, 0x746283E89F45BBA6uL), (+1, 2, 0xB478FB18D3F20BC2uL, 0x746283E89F45BBA6uL)),
                ((-1, 3, 0x91E7B80F24D66A48uL, 0x56A7250A1F57ACAFuL), (+1, 3, 0xE6CEE046397A1AD4uL, 0x3B8311A919A5352DuL)),
                ((-1, 3, 0x9704D66402028B97uL, 0xBB5C7358741CB1FAuL), (+1, 4, 0xB118A9F69A4DB5C9uL, 0x554B1B10F27EB22AuL)),
                ((-1, 2, 0x937F51B0DEFFB545uL, 0xF333FCB1949C031BuL), (+1, 4, 0xB5C23867A66056AAuL, 0xE7C2F05D2F438629uL)),
                ((+1, -1, 0xAEC719983B9953C7uL, 0xB258B9625061AE53uL), (+1, 4, 0x83A8C1C7C121D3A3uL, 0xC2F2F98428383A19uL)),
                ((+1, 1, 0xAC16619EE05E36A8uL, 0xFBA60692C39B589BuL), (+1, 3, 0x8A752AC4DFD00002uL, 0x86C2E02BE0B8A5ACuL)),
                ((+1, 1, 0x82A4E3A99B1D3240uL, 0x36A86E5A0A5DAA95uL), (+1, 1, 0xD622B57D15D62E80uL, 0x31FEA6FE76226BE0uL)),
                ((+1, -1, 0xE6FB611668851BC5uL, 0x688732E259322B98uL), (+1, -1, 0xF3FD5C27FF423687uL, 0x5D255900FA985F87uL)),
                ((+1, -2, 0x87336CD5104DE640uL, 0x3190EE4A710F2AA8uL), (+1, -3, 0xCB444D97022353FFuL, 0x5BDCFA87F8C2EA3DuL)),
                ((+1, -5, 0xD7F6ACC29AE2407FuL, 0x606DEF10B070FADAuL), (+1, -6, 0xF3577D6B41E7B414uL, 0x4BAC732FBE01ADEAuL)),
                ((+1, -8, 0xE9EE0D42F2FEDEC3uL, 0x3147BECC66A9F04DuL), (+1, -9, 0xCB3AEA25A011FFA0uL, 0x559E4B534CD4920CuL)),
                ((+1, -11, 0xA6899AE089AA57E2uL, 0xB8578C34D017272DuL), (+1, -13, 0xE21CFE322A3F4980uL, 0x324BD4FB2EDA1CE2uL)),
                ((+1, -15, 0x92BFB33BBC289F99uL, 0x2C1ABD51E8DBE5F0uL), (+1, -17, 0x9BCCEF1BBBCB7068uL, 0xEB8D0E31A5F1A501uL)),
                ((+1, -20, 0x8FF251E6A6B4EB04uL, 0x15B1866F3FE3C4B6uL), (+1, -23, 0xEB88FF8D6C91B8F2uL, 0x4D7E3126022A0A2DuL)),
                ((+1, -26, 0x8081A24FDA3208C2uL, 0x3F4B6C634C5F8A2DuL), (+1, -29, 0x9B4A6D59DC92F6B7uL, 0xC94C00BA3A376DABuL)),
                ((+1, -34, 0x809778C68A0D1E2DuL, 0xCCCE1E5DEAB78EDAuL), (+1, -38, 0xC960F73927C9521EuL, 0x67AB25F2EA9A36DBuL)),
        });
    }
}
