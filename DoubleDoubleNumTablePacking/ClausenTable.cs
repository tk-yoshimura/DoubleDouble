using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class ClausenTable {
        public static void PackNearZero(BinaryWriter stream) {
            stream.Write(nameof(NearZeroTable));
            stream.Write((UInt32)NearZeroTable.Count);
            foreach (Hexcode v in NearZeroTable) {
                stream.Write((UInt64)v.Hi);
                stream.Write((UInt64)v.Lo);
            }
            stream.Write((UInt32)0u);
        }

        public static void PackPade(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode c, Hexcode d)>> tables = new(){
                { nameof(PadeX0p25Table), PadeX0p25Table },
                { nameof(PadeX0p50Table), PadeX0p50Table },
                { nameof(PadeX0p75Table), PadeX0p75Table },
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

        static ReadOnlyCollection<Hexcode> NearZeroTable = Array.AsReadOnly(new Hexcode[] {
            (-1, -4, 0xE743BBE73501DE8AuL, 0x103493D2A9B19781uL),
            (-1, -6, 0xC895C5DD807F18EAuL, 0x692BF929071250CEuL),
            (-1, -8, 0x91DCE54E1782A3B7uL, 0xA1FB850490DB03F4uL),
            (-1, -11, 0xE370882562DD2C9BuL, 0x1F605F75967BBE92uL),
            (-1, -13, 0xBA29CE182BCD1ED8uL, 0x53B3424DA8B94EB4uL),
            (-1, -15, 0x9D8904E3CF723209uL, 0xC3826F705F928111uL),
            (-1, -17, 0x888861633F845F60uL, 0xB421E6F9CE376227uL),
            (-1, -20, 0xF0F0E1DBF3E31C0FuL, 0xF1AB48C63C407FE0uL),
            (-1, -22, 0xD79432E607306D6CuL, 0x63A2B19CDB2C6049uL),
            (-1, -24, 0xC30C3026F662FFD5uL, 0x11EC2BF53362B982uL),
            (-1, -26, 0xB21642A7F6CB9521uL, 0xD68A2941D0E31A51uL),
            (-1, -28, 0xA3D70A369CE8CF5CuL, 0x376FCD6297840880uL),
            (-1, -30, 0x97B425EB940C0FC3uL, 0x92E2AF08965EA81AuL),
            (-1, -32, 0x8D3DCB088326D9E0uL, 0x269057257B67FD7EuL),
            (-1, -34, 0x84210841FEE619E8uL, 0xB19F638E326686AFuL),
            (-1, -37, 0xF83E0F83D9364C65uL, 0xB2B8179C738EC90DuL),
        });

        static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> PadeX0p25Table
            = new(new (Hexcode c, Hexcode d)[] {
                ((+1, 2, 0xA792A61BBE167777uL, 0xA48C3BBC34F5AD12uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 6, 0xBAF4E29FAEC31B1CuL, 0x68088C3DC631861EuL), (+1, 4, 0x9D4B82FEFF95AA07uL, 0x74E09C4D7619C9B5uL)),
                ((+1, 9, 0xAB86A965EE16EACEuL, 0x93464C0D6039136AuL), (+1, 7, 0xA28874AD4F785500uL, 0x764017331A2693A5uL)),
                ((+1, 11, 0xA499B638741CF8B6uL, 0x43F0919E1EA1794AuL), (+1, 9, 0xB615D5B4BDF8230CuL, 0x9F7D033B1AD50E28uL)),
                ((+1, 12, 0xAA6AA1FC58524CD5uL, 0x70BAC4011EFA74FDuL), (+1, 10, 0xEBE051ACD956B03BuL, 0x4A1614F50B0548BEuL)),
                ((+1, 12, 0xA138C405115D88FCuL, 0x90E8FC211C194B9BuL), (+1, 11, 0xA9DDEFD9EF255A64uL, 0xCD7169934E670636uL)),
                ((-1, 8, 0xCDB113DC5F2B57DFuL, 0x0A1B8A11AA370E72uL), (+1, 10, 0xCDBB9BB34BCF1F1CuL, 0x202CAC800073F94AuL)),
                ((-1, 12, 0x891F92CDB6BD5CFDuL, 0xCCAA54FCB3728476uL), (-1, 9, 0x94293F7E7C4AE1B1uL, 0x1D7A522B51047FB1uL)),
                ((-1, 10, 0xD329D3D57C97F270uL, 0x030177B01123C09CuL), (-1, 10, 0x97CF3D46276B5C32uL, 0xA9C6D8BDBA4323F7uL)),
                ((+1, 10, 0x8E83D5AE3F3B4B2EuL, 0x5F6D880949449FEBuL), (-1, 7, 0xE3BA6B290DC9E9DCuL, 0x3A2BD2B45B394080uL)),
                ((+1, 8, 0xF2AEE5651AEDBDCCuL, 0x9641BA5497D14842uL), (+1, 7, 0xD9D492055504AD2CuL, 0x57CD24FBC678E206uL)),
                ((-1, 6, 0xFDF2A98588D3EA30uL, 0x3ABCCD256C44CE05uL), (+1, 5, 0xB9F8951101758F27uL, 0x7454790F6B10646FuL)),
                ((-1, 4, 0xCC4F751347DD94C8uL, 0x9E0D78719D002532uL), (-1, 3, 0xBABDF8A0392D3535uL, 0xC84259068FF347C4uL)),
                ((+1, 1, 0xB9941399D056B60BuL, 0x8105B5BE21F4884DuL), (-1, -1, 0xCB3DFAFFE825128AuL, 0x24C5C07D7034935EuL)),
        });
        static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> PadeX0p50Table
            = new(new (Hexcode c, Hexcode d)[] {
                ((+1, 1, 0xEA7CB89F409AE845uL, 0x215822E37CC09A17uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 4, 0xA20441B40097F639uL, 0x2BE08F4754DE59C1uL), (+1, 2, 0xD6EB26A3C79C9C4BuL, 0xE935442F8A647427uL)),
                ((+1, 5, 0x880D20855F292DB5uL, 0x83CE6C9459E47C3CuL), (+1, 3, 0xFF5E59CF4406AE54uL, 0x93A8E0FB15FA03D3uL)),
                ((+1, 1, 0xAF09FC0E9C42E94AuL, 0x0914C015B1BB30F3uL), (+1, 3, 0xCE1216B7C513CA1FuL, 0x2B2486728CC0D207uL)),
                ((-1, 5, 0x9CF1D680EA1B410BuL, 0x1CC89FD28AAC7391uL), (-1, 2, 0xC89B591414AE6FB1uL, 0xF8AE33A710CDE4A5uL)),
                ((-1, 4, 0x932706D389018A45uL, 0x7C4A5925F70FA280uL), (-1, 3, 0xDB0F7921D0400C90uL, 0x2A8445EEDEEDC0E1uL)),
                ((+1, 4, 0x80B893B10F9A160AuL, 0x47887F3C56242E1CuL), (-1, 0, 0xFAC62C67A2ED9E5AuL, 0x603AF8AABF39D981uL)),
                ((+1, 2, 0xFBDC7B6CEF6DE70FuL, 0x2A6CF68E2896BA38uL), (+1, 2, 0x81735C5995F155E3uL, 0x29884090C7AFFA85uL)),
                ((-1, 1, 0xC0B9BC2A28AA9B85uL, 0xA6DE405715E7FB72uL), (+1, -1, 0xDE46DA77F51372DBuL, 0x7F4F8FF4CE9027B8uL)),
                ((-1, -1, 0xDF5C7E4678E64406uL, 0xB130AF2FCFE88CBAuL), (-1, -2, 0xD7E0388D3D55AE21uL, 0xB66D1A3CAD445331uL)),
                ((+1, -3, 0xBF6A3FC82419EAE6uL, 0x3197BD0193F464B4uL), (-1, -5, 0xD35B8B30EA6F071DuL, 0x81583490A816EC55uL)),
                ((+1, -7, 0xB0025EB8DCDD77B6uL, 0x7C7F0DDA0F328DB0uL), (+1, -8, 0xFEFE5989986C2746uL, 0x48610F98C81973CFuL)),
        });
        static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> PadeX0p75Table
            = new(new (Hexcode c, Hexcode d)[] {
                ((+1, 1, 0xB2D22677FBC5A96BuL, 0xDD88603616834905uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 2, 0x8F3F0A4D784D8B9BuL, 0x0B4DBD993DBCFB28uL), (+1, 1, 0xA7810BF9C608C491uL, 0x9949DC8BF954D0D4uL)),
                ((-1, 2, 0x9F3CBFBF0796BBFBuL, 0x3A16B748CA9B31E9uL), (+1, -3, 0xCD3B6928B9C3AED9uL, 0x7976F4CA96B87155uL)),
                ((-1, 3, 0x88F70B6FCADEBA66uL, 0x5E8D0610E45376A3uL), (-1, 1, 0xF9E2E7928141A332uL, 0xBE481B485566AA32uL)),
                ((+1, 1, 0xEF28D33CE5554621uL, 0x014344B3298D1685uL), (-1, 0, 0xCCD67B6F6920C808uL, 0x26EE04C2150B030CuL)),
                ((+1, 2, 0xB931C15ACD320325uL, 0x9268962EDEF436D3uL), (+1, 1, 0x88C2132DB32E17B1uL, 0xDE5219902DB5465EuL)),
                ((-1, 0, 0xC99841BC9BC6B22DuL, 0x0E516D776FACC67CuL), (+1, -1, 0xF226AF59430E020FuL, 0x9E535B15991CAF02uL)),
                ((-1, 0, 0xC8421DE80D8EFB74uL, 0xA2E96D08EC78CECFuL), (-1, -1, 0x852470C9216AC8B1uL, 0x0C8B3FD9629AB868uL)),
                ((+1, -2, 0xAA981BA06516F2D0uL, 0x98976EE15F689405uL), (-1, -3, 0xB3EE118939CCB751uL, 0x2B882BCC2392DEB2uL)),
                ((+1, -3, 0x8BBB24494334376AuL, 0xFAB9EABB6D2B0678uL), (+1, -5, 0xCC0786388B0AFA69uL, 0xB7B13A9010FECE76uL)),
                ((-1, -6, 0xAA93B1E0AA1DE99AuL, 0xAAF13A8797752260uL), (+1, -7, 0x8019F39249811401uL, 0x97373BD80001A746uL)),
                ((-1, -10, 0xC9E6BD5CBF489EEAuL, 0x80F8CCBF3FE740DDuL), (-1, -11, 0xDB5EDAA633F2B55AuL, 0xC82F122425AF9F6EuL)),
        });
    }
}
