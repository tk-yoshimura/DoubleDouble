using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class RiemannZetaTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<Hexcode>> tables = new(){
                { nameof(PadeX0NumerTable), PadeX0NumerTable },
                { nameof(PadeX0DenomTable), PadeX0DenomTable },
                { nameof(PadeX1NumerTable), PadeX1NumerTable },
                { nameof(PadeX1DenomTable), PadeX1DenomTable },
                { nameof(PadeX2NumerTable), PadeX2NumerTable },
                { nameof(PadeX2DenomTable), PadeX2DenomTable },
                { nameof(PadeX4NumerTable), PadeX4NumerTable },
                { nameof(PadeX4DenomTable), PadeX4DenomTable },
                { nameof(PadeX6NumerTable), PadeX6NumerTable },
                { nameof(PadeX6DenomTable), PadeX6DenomTable },
                { nameof(PadeX10NumerTable), PadeX10NumerTable },
                { nameof(PadeX10DenomTable), PadeX10DenomTable },
                { nameof(PadeX17NumerTable), PadeX17NumerTable },
                { nameof(PadeX17DenomTable), PadeX17DenomTable },
                { nameof(PadeX30NumerTable), PadeX30NumerTable },
                { nameof(PadeX30DenomTable), PadeX30DenomTable },
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

        public static readonly ReadOnlyCollection<Hexcode> PadeX0NumerTable = new(new Hexcode[]{
            (-1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (-1, -5, 0x9097A3419C787FA0uL, 0xE9C6D320E7B4699FuL),
            (+1, -7, 0xB09CC5EA166D0446uL, 0x6583A7847BE5866EuL),
            (+1, -11, 0x895A7C64A360B85AuL, 0x6D493EA2471F2438uL),
            (-1, -14, 0x8ACA6E2E8C52AC33uL, 0xEF14F44F44EF0922uL),
            (-1, -18, 0xDD19E2A91DB8FC2BuL, 0x714E98D80C0389BDuL),
            (-1, -24, 0xDE213CF80598B189uL, 0x54D9461E6B2C26B8uL),
            (+1, -27, 0xC8B17229FD46DF71uL, 0x1D8915ECCEB44594uL),
            (+1, -31, 0xB59BFEDB1393CD66uL, 0xA7AEF87A346EE062uL),
            (+1, -37, 0xC6F91D33BBF18434uL, 0x93F09AC9EFB56A64uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX0DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (-1, -2, 0xC6643BD0D10F60C2uL, 0x3F4AA699E05C2477uL),
            (+1, -4, 0xA44DCEC8D333465DuL, 0x4C2AD417794025F7uL),
            (-1, -7, 0xB56A62BF5C9DDD1DuL, 0xE2B1729E6C3DD942uL),
            (+1, -10, 0x93866CC4678A22D0uL, 0x19FF2AB5024FBB94uL),
            (-1, -14, 0xB76844DC5C910D38uL, 0xFACFA210E955ACDCuL),
            (+1, -18, 0xB1DF0EF18AE97F49uL, 0xE5C8EF42D45ED1E2uL),
            (-1, -22, 0x85644BC05504E642uL, 0x589EDB9DBD5C2B90uL),
            (+1, -27, 0x977F828EDB178F6CuL, 0xCDA848D24FB47499uL),
            (-1, -33, 0xEEA9A5A07A7D065EuL, 0x8EC0FA43133D82F0uL),
            (+1, -39, 0xDE3DA65B827E7B3CuL, 0x5BD91583B244F2CAuL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX1NumerTable = new(new Hexcode[]{
            (+1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB48uL),
            (+1, -2, 0x82DDBCA9529C5519uL, 0xCB1377EE94AF6E38uL),
            (+1, -5, 0xCA5D944837774C1BuL, 0xA82084BDB284B51AuL),
            (+1, -8, 0xB4AC80429D1750FAuL, 0x5FE8969322A9F628uL),
            (+1, -12, 0xE4F1AEDB9F285890uL, 0x5E821EAAD040B905uL),
            (+1, -16, 0xE1494E560C590942uL, 0x4CD791F2B4E89012uL),
            (+1, -20, 0x92A1E36D215B2101uL, 0xE1B135C92338C0CEuL),
            (+1, -26, 0xEB4644F4C2E970F6uL, 0x778E16F1C3F055C4uL),
            (+1, -31, 0xA0483D8457CD732FuL, 0xE2BF911E20CB4540uL),
            (-1, -37, 0x9307F16279A9317BuL, 0x278EBA69B9FD15F1uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX1DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -2, 0xA2217D326C0F30EFuL, 0x6C013D01B4462802uL),
            (+1, -5, 0xDD593C8937988753uL, 0x92962F363DAB653CuL),
            (+1, -8, 0xC427FDF9937258A5uL, 0x437B90EBAA52E605uL),
            (+1, -12, 0xF8FD0C2E27AEA0F2uL, 0xD2D9CB231E28792CuL),
            (+1, -16, 0xE4467D451552C5C4uL, 0x83BF4B1E6693D675uL),
            (+1, -20, 0x9732176593AE4DFDuL, 0x1AA8997310FAA908uL),
            (+1, -25, 0x81A38BDC29706D61uL, 0x2D8B9BCED6E79985uL),
            (+1, -32, 0xE8866E3DA94DD95DuL, 0xD99ACE9DAD645893uL),
            (-1, -38, 0x84AE9F5B9B628D97uL, 0x3946A7C9ED4A1311uL),
            (-1, -44, 0x8309C83F52320220uL, 0x8709215E1230DAFEuL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX2NumerTable = new(new Hexcode[]{
            (-1, -5, 0xDC0F9DACF82CDCF1uL, 0x84EDDBBFE8A60C27uL),
            (+1, -5, 0xAFC10697E6BB7FB3uL, 0x19E4682E542C4955uL),
            (+1, -7, 0xDF10B4BF695D8F17uL, 0xA53E585861206466uL),
            (+1, -10, 0xF957221A31050C96uL, 0xB6CBB8B5A43E70E4uL),
            (+1, -13, 0xC3F552AD0D8D85B3uL, 0xE2514F7762993A6CuL),
            (+1, -17, 0xF3DA386B90C728ADuL, 0x62C0486A8855CE87uL),
            (+1, -21, 0xD85120D7865F0048uL, 0xE79FD8DBE8BDB44FuL),
            (+1, -25, 0x92615A5843867B29uL, 0x49BC4FD963C5AED9uL),
            (+1, -30, 0x9F1A4233748D7B91uL, 0xE9F83A0828B5F1A2uL),
            (+1, -36, 0xCBFC4EEDBC7A1A75uL, 0xB9DBEA426B233E1BuL),
            (+1, -42, 0xBF7F9AD7AF262406uL, 0xB85907DBE39FBC5EuL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX2DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -2, 0xBA3E20B9CC8196C4uL, 0xF3D9F1B73023E6E4uL),
            (+1, -4, 0x8EA8F50F144D44ABuL, 0xE827F4CAA59B7834uL),
            (+1, -7, 0x908A86170238F5CDuL, 0x0B4FA7F1BB9584F8uL),
            (+1, -11, 0xD5C0F1F08593D78CuL, 0x45F868B2CA0EB53AuL),
            (+1, -15, 0xEFA5F99C2DD61909uL, 0x598CB032D5E409AEuL),
            (+1, -19, 0xCF8D9A219DC46BAFuL, 0x22AC8B6A10757816uL),
            (+1, -23, 0x89AF8B7551402117uL, 0xC3461F558C11BD11uL),
            (+1, -28, 0x88B59312AC5AE29FuL, 0x42CC61DD369C3204uL),
            (+1, -34, 0xBA931B4A02DD33B6uL, 0x083CB5CCB3530E19uL),
            (+1, -40, 0x94BA4D4AE752FB74uL, 0xC4B8D350380CB376uL),
            (+1, -51, 0x9DBA006F7E43A843uL, 0xBC198CC35E9C4840uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX4NumerTable = new(new Hexcode[]{
            (+1, -1, 0xC9506DF2DA8BA05AuL, 0x39BC0B8FE5AA9AB1uL),
            (+1, -2, 0xFDD51E7229FE8B30uL, 0x833A0A50899A578FuL),
            (-1, -2, 0xD177BFC2A3947531uL, 0x40CAA6C20BDD84C1uL),
            (-1, -1, 0x92CAD47A59DC0780uL, 0xCEF4A45807BF8856uL),
            (-1, -2, 0x8F9B0FB9A16CEBF3uL, 0xE571600A05F40A4AuL),
            (-1, -4, 0x9A3EAAD616E67629uL, 0x17AD2444D1453277uL),
            (-1, -7, 0xC96A422282463894uL, 0x43190BB944FE080BuL),
            (-1, -10, 0xA55860EA5694027BuL, 0xCE7120573A04992BuL),
            (-1, -14, 0xADBDE0C8972E8C75uL, 0x504D269E803B0178uL),
            (-1, -19, 0xF244DC10E3120A4DuL, 0x68983CC4178A26F7uL),
            (-1, -24, 0xEBF712A957E91715uL, 0xC69EE416E685110AuL),
            (-1, -29, 0x936E2C6006C2FF68uL, 0xC4F8DDD1C35C335DuL),
            (-1, -36, 0x84BC27A07E18941CuL, 0xC30D162483075FA1uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX4DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, 0, 0xD8F2C454B4CCCD49uL, 0x41BDEBA8C197DC1BuL),
            (+1, 0, 0x9D0D94CB9F8CF85AuL, 0xF288B1BD892AF985uL),
            (+1, -2, 0xFDA64DB9783E7818uL, 0x273E90B6EBD390D1uL),
            (+1, -4, 0xFA9C1E5656018715uL, 0xD700341792AB4D31uL),
            (+1, -6, 0x9CCE36D87ADE7E4FuL, 0xA2749A006C1918D4uL),
            (+1, -10, 0xFADFB2F9EF85588DuL, 0x59E0FC49FDC07378uL),
            (+1, -13, 0x814EF638BE17196CuL, 0xF8EEB89B47EE92C8uL),
            (+1, -18, 0xB27DC4FE3B55AF75uL, 0xAC086280B60311B9uL),
            (+1, -23, 0xADC71D9DEA201932uL, 0x00EDD8C99240CE33uL),
            (+1, -29, 0xD2243719F46FB89AuL, 0x8CFF47F848A78A8AuL),
            (+1, -36, 0xCD761FB0299AF196uL, 0xE5EFFBAB114E758CuL),
            (-1, -46, 0xF806965EFFD39881uL, 0x530CDF06C79F799AuL),
            (+1, -53, 0xFF7234A222225375uL, 0x4483FAAB8F4238B0uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX6NumerTable = new(new Hexcode[]{
            (-1, 2, 0x81BEFA5B030A153BuL, 0xF296DBBAF63DB94EuL),
            (-1, 2, 0x966DA3350EC6B7ECuL, 0xD8F15C635D18390CuL),
            (-1, 1, 0x97A13A1617BA45A3uL, 0xEBBC648B55643EC7uL),
            (-1, -1, 0xAF2FC3CD51D30BDEuL, 0x99296C40D598070EuL),
            (-1, -3, 0x810D19911CE6A7D4uL, 0x80E3D738BBE1302AuL),
            (-1, -6, 0x8018F6DE8D2A24EEuL, 0x9A491E5080C726D2uL),
            (-1, -10, 0xB186E48782801F38uL, 0x4A628BFE966B2B15uL),
            (-1, -14, 0xB0BF2810E1A66652uL, 0xA8CC9400A9660397uL),
            (-1, -18, 0x8162F8989192EF2CuL, 0x638523D5A021F066uL),
            (-1, -23, 0x8C1428D70F348CC0uL, 0xB9970A4BCAF21756uL),
            (-1, -29, 0xD90EDE31BFBC779EuL, 0x80C2F0D67508220AuL),
            (-1, -35, 0xDC0CAE7830C95033uL, 0xC5F30DCCD768F9ABuL),
            (-1, -42, 0xEC8DE3ABE277F8CAuL, 0xC0C7144933D543B0uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX6DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -1, 0xFA04C24E328C9FA0uL, 0x5AA59AA82003EB27uL),
            (+1, -2, 0xD14CA283DE1F114FuL, 0xB7F1D436CF770D94uL),
            (+1, -4, 0xC567A188016EC10DuL, 0x469D9CFC6BBD4BD9uL),
            (+1, -7, 0xE8FE32D4B7E74AE6uL, 0x4173B024D9614817uL),
            (+1, -10, 0xB634170D3E6E953DuL, 0x15C55709C42EFDE6uL),
            (+1, -14, 0xC545CFB77BEBBC31uL, 0xC01D4A0735C3885BuL),
            (+1, -18, 0x99C0DF053D6407E3uL, 0x060AD5711AD9A2BFuL),
            (+1, -23, 0xAF823A4AAC6C9D2FuL, 0xE8B30681B4C16350uL),
            (+1, -28, 0x8E407B0B76E64160uL, 0x5CFEC1AA8354809FuL),
            (+1, -34, 0x97137DC7E1E5CCD9uL, 0xE76F12A6721A7C20uL),
            (+1, -41, 0xA9DC031020F30A5BuL, 0xE789B5936C9341AFuL),
            (+1, -55, 0x87E493D91898F5E8uL, 0x3AECE5FAC48E58D5uL),
            (-1, -63, 0xAD8D34FBC80E5842uL, 0xE26F28AE424F26A6uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX10NumerTable = new(new Hexcode[]{
            (-1, 2, 0xDD38E48D1D29DFC6uL, 0xEE48C9CD016F7B7EuL),
            (-1, 1, 0xE9EA167056B5E439uL, 0xBFF32994323E7522uL),
            (-1, -1, 0xD0454ECD7B383967uL, 0x88E86E8A572EB37EuL),
            (-1, -4, 0xCBA2DCEA11FA467EuL, 0x3175E34367B6F89BuL),
            (-1, -8, 0xEE3081D7C7C86F70uL, 0xF13B730E39485A4DuL),
            (-1, -12, 0xA655050705A268D7uL, 0xFBD2265BB5114C31uL),
            (-1, -18, 0xE050EF58A3EE95C8uL, 0x982917B9C68B1FB0uL),
            (+1, -24, 0xBCDD0D9FA0307336uL, 0x3733F07DE7D09214uL),
            (+1, -27, 0xC392E10BCCD94F0FuL, 0xA43026AD464CF6A8uL),
            (+1, -32, 0xD0757626D1570C66uL, 0x0703D5162FBDBAACuL),
            (+1, -38, 0xD7AD7CFA5F7F1DE3uL, 0xC9791F78058F981DuL),
            (+1, -45, 0x9CAB1C7AE78340CCuL, 0x98715719D5B2EA2BuL),
            (-1, -52, 0xA6B47F0CCCA14204uL, 0xB63F599EC3A88B8CuL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX10DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -2, 0xDAC861D45044CA6FuL, 0xB3791ED4B1162E70uL),
            (+1, -4, 0x98C9340CA20DE508uL, 0xBEE1013E2279FF94uL),
            (+1, -8, 0xE1A847B07C182017uL, 0x432445E9F0DE9C92uL),
            (+1, -12, 0xBCD5D9880EDD5811uL, 0xC10F4307B493AE51uL),
            (+1, -17, 0xA355F2BE5DB4156FuL, 0xB4A84E71E92A8545uL),
            (-1, -27, 0x929F6299E2BE093FuL, 0x748EC94ECD5CAB10uL),
            (-1, -27, 0xCB12748051BCE343uL, 0x26693AFEF86B22F7uL),
            (-1, -32, 0xFE4623187353A01BuL, 0xD22F4AA9D0035B96uL),
            (-1, -37, 0x92C1985DCAEF66C1uL, 0xAFE8EA20EF1EF336uL),
            (-1, -45, 0xF126463D099F1555uL, 0x7B82AE2A22C51E74uL),
            (+1, -52, 0xEA4DAEBAEBF9BA95uL, 0x0121C06790473D0CuL),
            (+1, -64, 0xD1A93455ADFE9FC7uL, 0xC9830CA208C4EFC7uL),
            (-1, -72, 0xD3721B2044CF9B7EuL, 0x37481943115B4228uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX17NumerTable = new(new Hexcode[]{
            (-1, 3, 0xBC850990343EE49AuL, 0xF9694A776F1A9217uL),
            (-1, 2, 0x8B8FDE8104670661uL, 0x05868F2FD09110CDuL),
            (-1, -1, 0xBB7574A17FB324F6uL, 0x4A03D0508D87C468uL),
            (-1, -4, 0x9892ADE6D9EF83B4uL, 0x519825BC0D991C59uL),
            (-1, -8, 0xA97C40C3A208FBB9uL, 0x40B8B80B30F91402uL),
            (-1, -12, 0x88C907BE8C4DCF75uL, 0x23AE27BD5F5617E2uL),
            (-1, -17, 0xA604FF610CF0A81DuL, 0xC98FA4DCB63A7E3BuL),
            (-1, -22, 0x9A098B70CECE4248uL, 0xEABA1F62F01BC6F5uL),
            (-1, -28, 0xDAF63349A6891819uL, 0xAAD95B1FD2B2404AuL),
            (-1, -34, 0xEAD94573DBEC6660uL, 0xC749164FFF64FD57uL),
            (-1, -40, 0xB608C2E729333462uL, 0x367D987C6A8B5C7AuL),
            (-1, -47, 0xB876B69ABC6303D8uL, 0x2FDBA86F7D51D1E2uL),
            (-1, -55, 0xB9B2124F04A304F0uL, 0x5A3DC1CC98335D77uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX17DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -2, 0x9F612EF20F08B017uL, 0x4038C146DF974564uL),
            (+1, -5, 0xB38905CEEE562D39uL, 0xA9110ABC1CDCBDA7uL),
            (+1, -9, 0xF55D48C13CF50DFDuL, 0xABB13187896868C7uL),
            (+1, -13, 0xE554D59AD21B3F2DuL, 0x6A56C1E927BCB407uL),
            (+1, -17, 0x9B9F3467C277F75FuL, 0x2059BDD991E5A78EuL),
            (+1, -22, 0x9DEB15E3E4BD0D6FuL, 0x0FC1B413D8383C4DuL),
            (+1, -28, 0xF21AE805B53D33C0uL, 0x8E5C0C02063C0B37uL),
            (+1, -33, 0x8AE7AC037C2F2FEDuL, 0x3E7B40C7DBA473BDuL),
            (+1, -40, 0xE5A55358DB370D09uL, 0x9C126F47A2FA0513uL),
            (+1, -47, 0xF85293690A396426uL, 0x74EC1484039D5B75uL),
            (+1, -54, 0x85F6BC54B3CA1AC9uL, 0x78E1E6C20B5CA197uL),
            (-1, -75, 0x8CE3EEF435C2C453uL, 0xA265DA97A47E2049uL),
            (+1, -84, 0xB6429F1817E58B42uL, 0xC4413E94DA093EDEuL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX30NumerTable = new(new Hexcode[]{
            (-1, 4, 0xA65AF3BC3FD2CB36uL, 0x944A369EE33B7D5FuL),
            (-1, 2, 0x9EA4A789E1037811uL, 0x439D148CF5B7D093uL),
            (-1, -1, 0x9033D2A18DFD0E8FuL, 0x9C7FF3B4E295C403uL),
            (-1, -5, 0xA660D7B8FE3E1C7BuL, 0x5AD39F58E391C2A4uL),
            (-1, -9, 0x889EAE83FC7ADAC8uL, 0x357EDA3002C0EC82uL),
            (-1, -14, 0xA978528EF84F6C5CuL, 0xD96E7060F2D88D6DuL),
            (-1, -19, 0xA4382CAC0EF6B08FuL, 0x2560003D84BF2F27uL),
            (-1, -25, 0xFD2DC1B50633A0F4uL, 0x11FE9A61088CA9F0uL),
            (-1, -30, 0x9C447B271A808850uL, 0x21BA12F2050049A0uL),
            (-1, -36, 0x99BA39374B190C1AuL, 0xA84F41F3D3820150uL),
            (-1, -43, 0xEC7F41EDA2EDF2DFuL, 0x553C241C2C13C118uL),
            (-1, -49, 0x8853054357892A59uL, 0x1B0585B715EC3885uL),
            (-1, -57, 0xD68F618600080B4CuL, 0xAA90E1153E255DB5uL),
            (-1, -65, 0xB1E54B0CFC22D2A9uL, 0xADF87877741BE4F7uL),
        });

        public static readonly ReadOnlyCollection<Hexcode> PadeX30DenomTable = new(new Hexcode[]{
            (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
            (+1, -3, 0xD1FFA3ECE66E3D3CuL, 0x46DDC2EA91C8DB84uL),
            (+1, -6, 0xA5E8EEA1C1227FACuL, 0xDB4B73CF770CEBEFuL),
            (+1, -10, 0xA78CCED46B17733CuL, 0x1295AB55A7AE37FCuL),
            (+1, -15, 0xF1C2FDF1AA20EA8CuL, 0xE70A0E8CF8CF0400uL),
            (+1, -19, 0x83DA80097A94C50DuL, 0x9E8624FCE12C4A3AuL),
            (+1, -25, 0xE02388F4218A564EuL, 0x557AF3CAE4C3ABA6uL),
            (+1, -30, 0x968767C1299DE567uL, 0x9031E355FB4304B8uL),
            (+1, -36, 0x9FD2E9B46854F070uL, 0xCB4F57DE6934DA43uL),
            (+1, -42, 0x842D4874F84C9F60uL, 0x2AA1D25252EADA85uL),
            (+1, -49, 0xA3EC945BFB9F5C54uL, 0x9CC8ABA86D410A86uL),
            (+1, -56, 0x8BBBFEBC730C976EuL, 0x549B2997D95CA19EuL),
            (+1, -64, 0x80531847D3917805uL, 0x94A68B1866BF8A6EuL),
            (+1, -96, 0xF50F26C489694734uL, 0x1D616084C870CA7EuL),
            (-1, -104, 0x90A1DF91FE166504uL, 0x6E2900768A8EF26FuL),
            (+1, -114, 0xA47B231DBA4A7B88uL, 0xA3B8BC7AC6C36C89uL),
        });
    }
}
