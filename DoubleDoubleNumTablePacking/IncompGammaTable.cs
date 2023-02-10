using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class IncompGammaTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<Hexcode>> tables = new(){
                { nameof(TaylorA1ZeroTable), TaylorA1ZeroTable },
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

        static readonly ReadOnlyCollection<Hexcode> TaylorA1ZeroTable
            = new(new Hexcode[] {
                (+1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
                (-1, -1, 0xA7E7A01357D16E75uL, 0xC24856F3BD611D64uL),
                (-1, -5, 0xAC0AF47D13823E47uL, 0xA15A643C9C84B042uL),
                (+1, -3, 0xAA891905A1FDF2EFuL, 0xD37FB09A1EF84DA1uL),
                (-1, -5, 0xACD7881E1A0493DFuL, 0x1B4048388CACA42EuL),
                (-1, -7, 0x9DA5794241F10A71uL, 0xB79E818273539ADCuL),
                (+1, -8, 0xEC8CE293FB058CADuL, 0xD1D69DD3DC1E1318uL),
                (-1, -10, 0x98B889671D153DE9uL, 0x6EDBA83588E959C6uL),
                (-1, -13, 0xE1B27F378AB1E74CuL, 0x7C04703C8691B6EDuL),
                (+1, -13, 0x86453C66CFCE8D3CuL, 0xD83432FACDB927F8uL),
                (-1, -16, 0xA8E7457A3F55EFEDuL, 0x92173AFE34B827DAuL),
                (-1, -20, 0xA7D6A0FE1A7DD901uL, 0x776AB160DC7CCB6CuL),
                (+1, -20, 0x981284EDE06F1640uL, 0xF6FFCB7E4E64F0B8uL),
                (-1, -23, 0xDCCC33336112E8E8uL, 0x9722EF2CE809707CuL),
                (+1, -28, 0xD225BDD116B14565uL, 0x21CC6FD93419DC08uL),
                (+1, -28, 0xABDE1FE1C2199FD9uL, 0xEAFBBF416E76D1E0uL),
                (-1, -30, 0xA25A676E51C47BE3uL, 0x89819C393897FFDEuL),
                (+1, -34, 0xE573B3AE0C30362FuL, 0xBF7D38399563B603uL),
                (+1, -37, 0x88E832DFD7833A2DuL, 0x6B19F0B77E71C665uL),
                (-1, -38, 0x8211DD64651FD552uL, 0x333C5E6F89FA0414uL),
                (+1, -41, 0x8F900A8991E681C8uL, 0xF6862A8BDDBA9233uL),
                (-1, -46, 0xB965C4752D7373BDuL, 0x2A90DA417F9F6A64uL),
        });
    }
}
