using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class ErfTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode c, Hexcode d)>> tables = new(){
                { nameof(PadeTable), PadeTable },
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

        static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, 0, 0x906EBA8214DB688DuL, 0x71D48A7F6BFEC344uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, -3, 0xA61A09E5DFFF9E51uL, 0x7E212CA19743662AuL), (+1, -2, 0xF444C1245E74CA13uL, 0x10959EDC1DBBED49uL)),
                ((+1, -5, 0xDD3BE7EC8E8DA365uL, 0x3ED387AA38824326uL), (+1, -4, 0xDAEC569E191473DDuL, 0xBFA22416D66DD797uL)),
                ((+1, -9, 0xE3AF40C1A35AEF17uL, 0x766BB5FF60AAD40BuL), (+1, -7, 0xF2AD11A0705F3761uL, 0xF0B31DB1C38038D7uL)),
                ((+1, -12, 0xFC64EDC2CC309D71uL, 0x3BF873076B4FEC1DuL), (+1, -10, 0xB801F2E06C954F75uL, 0x209FE16330860FE3uL)),
                ((+1, -16, 0x84B200012D12A197uL, 0x86767577C2D97370uL), (+1, -14, 0xC6C58D92AAFE5C59uL, 0xA2EDB491C53E64CDuL)),
                ((+1, -20, 0x98D1102D90CD95D8uL, 0xD144093F348B4717uL), (+1, -18, 0x99AF298480E86469uL, 0x622B898CB8F7DD50uL)),
                ((+1, -26, 0x88CB906F46369D0FuL, 0x4B3157B68077A2CAuL), (+1, -23, 0xA4EDFBA426238676uL, 0x7649C729AD87E905uL)),
                ((+1, -31, 0x94C8AACB23722CB3uL, 0x1A5A662832E77F1BuL), (+1, -29, 0xE0607C0900410EFFuL, 0x75A020BD2E5F2788uL)),
                (Hexcode.Zero, (+1, -35, 0x948D1DAF285AF468uL, 0x90EA1130872685B7uL)),
        });
    }
}
