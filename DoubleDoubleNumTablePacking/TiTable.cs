using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class TiTable {
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
                ((+1, 0, 0x8000000000000000uL, 0x0000000000000000uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xA2EFB460F4BADCD7uL, 0xA93A1B7607CEB5CAuL), (+1, 3, 0xA4B6D0D2BBD74E9EuL, 0xC5ABE2927995D23CuL)),
                ((+1, 5, 0xC07B35AE8C7A5E5DuL, 0xC02CA14F8EB8196DuL), (+1, 5, 0xC4E58CD565E4307AuL, 0x9F8CB61C71AB7A92uL)),
                ((+1, 7, 0x8C0B524B519D4023uL, 0x81CEF45BF4145A6CuL), (+1, 7, 0x911F487819C9E638uL, 0xB9433619EE38FFB3uL)),
                ((+1, 8, 0x8C7C9CFBC5683565uL, 0x7CB824E3DCDE7EECuL), (+1, 8, 0x93A9DB849CFCF69EuL, 0x4BCA422D0D80DD7BuL)),
                ((+1, 8, 0xCE179D065600737EuL, 0xA55963CFBB576082uL), (+1, 8, 0xDC0A2B631B4BDD25uL, 0xF567456A58069EE0uL)),
                ((+1, 8, 0xE4CC7E3F0B2B05F5uL, 0xD161CB6F79BEE0E4uL), (+1, 8, 0xF88EC48B003F7F3CuL, 0xB92D62A170EBB5DEuL)),
                ((+1, 8, 0xC446F1B40DA7CC62uL, 0xAA9D5A4E7C91AEE5uL), (+1, 8, 0xD962CDCA7C64E3EFuL, 0x61FF38C76CD3FEEBuL)),
                ((+1, 8, 0x83B598540A1E972BuL, 0x9F9DB8BD1903BB08uL), (+1, 8, 0x9510577D368E6C3DuL, 0x21DA7156F7CAD39CuL)),
                ((+1, 7, 0x8B1A56A98F4EA6F5uL, 0x37389F2D8CBAE047uL), (+1, 7, 0xA1513F05275E3701uL, 0x8037878DF8BA6D71uL)),
                ((+1, 5, 0xE77502040688BF5AuL, 0xEF8A1983639E8A14uL), (+1, 6, 0x89FA947D59C0F40AuL, 0xDC9DDE108DED7DF3uL)),
                ((+1, 4, 0x972A1CBF950E36BFuL, 0x34BADFC43D938411uL), (+1, 4, 0xBA09ADFD614C3A24uL, 0xE209741E148BD808uL)),
                ((+1, 2, 0x99C4CF2E759F2A6EuL, 0xC6F4C908418D7F3EuL), (+1, 2, 0xC4526D49F06A5C55uL, 0x9DED4A729772E44BuL)),
                ((+1, -1, 0xF07CDCB3FCFC8DCFuL, 0x090BA9681F5BCB0DuL), (+1, 0, 0xA04509FE08E8A625uL, 0x7A8D53B92565B104uL)),
                ((+1, -3, 0x8DD7DFB723F8922AuL, 0xD939A8E0AE9031DFuL), (+1, -3, 0xC6FC4709E57BA063uL, 0x217C003ADEF6FEEBuL)),
                ((+1, -7, 0xF5B40008FDDF91D6uL, 0x19B0C3D22EC26EBBuL), (+1, -6, 0xB75DB891C852A348uL, 0xA0A8BA30D2A771B5uL)),
                ((+1, -10, 0x9666671CE7E3A573uL, 0x63FFCA56B279DF3BuL), (+1, -10, 0xF2700CE135B60A90uL, 0x8B799647D0975EF0uL)),
                ((+1, -15, 0xF641BF80CAC62A0CuL, 0x0827B46FE62E1027uL), (+1, -14, 0xDB0C54B7E11A2C82uL, 0xAB0D384F0FB43638uL)),
                ((+1, -20, 0xF7D480C25CB30EFBuL, 0xD5C3E810DB4BB226uL), (+1, -19, 0xFBA7F65DB8824595uL, 0x910DA2294FBDF510uL)),
                ((+1, -25, 0x853E96C4F29E0738uL, 0x790E369BE8915A7EuL), (+1, -24, 0xA3CEB701D1450F5AuL, 0xBB3E7FA4FE198D3EuL)),
                ((+1, -33, 0xE8DAFDD363812BA8uL, 0x80EAE7FA6B0467FFuL), (+1, -31, 0xC43A7E464CDEBBB8uL, 0x2FACE8ED2927C821uL)),
                ((+1, -42, 0x97237DA825F556F4uL, 0x1157122D8261549CuL), (+1, -39, 0x8522027FD7272AF8uL, 0x92C76A3CEACDD322uL)),
        });
    }
}
