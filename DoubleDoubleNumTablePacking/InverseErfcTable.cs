﻿using DoubleDoubleHexcode;
using System.Collections.ObjectModel;

namespace DoubleDoubleNumTablePacking {
    public static class InverseErfcTable {
        public static void Pack(BinaryWriter stream) {
            Dictionary<string, ReadOnlyCollection<(Hexcode c, Hexcode d)>> tables = new(){
                { nameof(LtRcpBinpow1PadeTable), LtRcpBinpow1PadeTable },
                { nameof(LtRcpBinpow4PadeTable), LtRcpBinpow4PadeTable },
                { nameof(LtRcpBinpow16PadeTable), LtRcpBinpow16PadeTable },
                { nameof(LtRcpBinpow64PadeTable), LtRcpBinpow64PadeTable },
                { nameof(LtRcpBinpow256PadeTable), LtRcpBinpow256PadeTable },
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

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> LtRcpBinpow1PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, -2, 0xF430FDD926004F5CuL, 0xD4CF37283EF220DFuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 2, 0x98F431114C2CD276uL, 0xC59EE346A80DAA0CuL), (+1, 3, 0x867AA86274D199D9uL, 0x5E69BC86AC586292uL)),
                ((+1, 4, 0xB9E0C96F1FF76C04uL, 0xC4EACC0CC10A2706uL), (+1, 5, 0x8B601A7561DF3C62uL, 0xC6FD021F9A20F9F2uL)),
                ((+1, 6, 0x91B90A37C030AC82uL, 0xF87B9A11308FC38CuL), (+1, 6, 0xBC8D04B1770E92BBuL, 0x4FBB28356E897870uL)),
                ((+1, 7, 0xA5536235EF75F110uL, 0xA691B68792B2AEDDuL), (+1, 7, 0xBA19665B845DBBB0uL, 0xCF4042CE13B9542CuL)),
                ((+1, 8, 0x90476F88C105E10EuL, 0xF44A4FE7920AE01AuL), (+1, 8, 0x8E1E1732422B107CuL, 0xEC880439BBE6F176uL)),
                ((+1, 8, 0xC9085AD86DABA84DuL, 0x0F9F58F95B9C9191uL), (+1, 8, 0xADF82DD14D95C0B0uL, 0xD54B6AED288778E1uL)),
                ((+1, 8, 0xE512B7BFF6495EB6uL, 0x8D272A19C44832D5uL), (+1, 8, 0xAE9423B7ACC19B9FuL, 0xBFAC7FBBD1094373uL)),
                ((+1, 8, 0xD8F1B5943175F441uL, 0x97569E9BB9026E85uL), (+1, 8, 0x91C2299970FEC483uL, 0xCC869DFC6C843594uL)),
                ((+1, 8, 0xAC9ED61ED72863C3uL, 0xFFEB2960209E6EC7uL), (+1, 7, 0xCC72D8E32D2371DFuL, 0x394B40AC1A5B4948uL)),
                ((+1, 7, 0xE868A90A4A0ED9D4uL, 0xC01AA3E0983638AAuL), (+1, 6, 0xF242D8D5E6499C0FuL, 0x32EF0CB08AB303F7uL)),
                ((+1, 7, 0x84E1042518E366AAuL, 0x71CB77790A452415uL), (+1, 5, 0xF322C87B088D5DA1uL, 0x125AEE6029760C1DuL)),
                ((+1, 6, 0x81378D95EB64AAECuL, 0xBA20381A9C0B3E7AuL), (+1, 4, 0xCEA2CA8722C4496CuL, 0x876283AE08228747uL)),
                ((+1, 4, 0xD57AC3E5BF95F8E2uL, 0x07FB057617F1504CuL), (+1, 3, 0x944C1CBF7028F513uL, 0xFE69B00ECAF2A59FuL)),
                ((+1, 3, 0x95454F4A471AE749uL, 0x25C21365B71C9137uL), (+1, 1, 0xB2BEC170F2B2FD7AuL, 0x02BE9C1CD480A57FuL)),
                ((+1, 1, 0xAF9E33CF4E6E3B4BuL, 0xC875243BE69D6619uL), (+1, -1, 0xB35615ADAE50F343uL, 0x5D7C1480D7DD2D19uL)),
                ((+1, -1, 0xAC3B5622FF7AF528uL, 0x16A559B6C71FF999uL), (+1, -3, 0x93E13360C7AC60BEuL, 0xCB570B60C0D415D3uL)),
                ((+1, -3, 0x8AF9AA2D459A0E5BuL, 0x2B23950C0437DBACuL), (+1, -6, 0xC4DD914EB5FCE180uL, 0x6EAEEB2D426B16FCuL)),
                ((+1, -6, 0xB532110D35AFD4FDuL, 0x7FD8A37E1F20D69FuL), (+1, -9, 0xCE387F1D40AFC6C5uL, 0x4A6AD17A3DF8CFB6uL)),
                ((+1, -9, 0xBA046FA2E54C7C5FuL, 0xC1A0D513C30190FFuL), (+1, -12, 0xA3CA949F4437C4D1uL, 0x2F257A28E74318FCuL)),
                ((+1, -12, 0x90E314F35E0B339DuL, 0x791DD9B29E879345uL), (+1, -16, 0xBA802897815F60FDuL, 0xCADC129943E5BE91uL)),
                ((+1, -16, 0xA1E9D458B662CD99uL, 0xF7ED992DE96A68FFuL), (+1, -20, 0x8B032E0C8F1267F5uL, 0xACD9017070420975uL)),
                ((+1, -21, 0xED38D593064FA90DuL, 0xD1DF5EBFF9C194C4uL), (+1, -26, 0xE6994B03DDAB5A4AuL, 0x5F14DC5B5141AA76uL)),
                ((+1, -26, 0xC1E67272347AFA70uL, 0x07DB25941BA9A0F2uL), (+1, -32, 0x93DCCEF68B68C3F3uL, 0xD3087E027DC0A43AuL)),
                ((+1, -33, 0xF6361683963A6C43uL, 0xBF66C686BB865313uL), Hexcode.Zero),
        });

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> LtRcpBinpow4PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, 0, 0xA89861D65726F35EuL, 0xF0E67AAE06F75E9DuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0x8B5AAC2E83B65670uL, 0xDAF5995615A5660BuL), (+1, 2, 0xBE73816656AA913BuL, 0x4EA7C14C6F1F66E0uL)),
                ((+1, 4, 0xDBCC8B0EAA205A87uL, 0xBCDE677EC331C739uL), (+1, 4, 0x875A911D5DF81ED3uL, 0x802A2A6C39733642uL)),
                ((+1, 5, 0xDBE36EA9331FA7D7uL, 0xF40D850BB94F484CuL), (+1, 4, 0xF42C5E882EE4DE86uL, 0xFF1BF42A4B58193DuL)),
                ((+1, 6, 0x9C7A344C2E5F705DuL, 0x44CF8C49F1B9252DuL), (+1, 5, 0x9CA0FC61A555C194uL, 0xA22C3FD94DAA3C89uL)),
                ((+1, 6, 0xA86772198D9B4810uL, 0x20B96C2286649ECBuL), (+1, 5, 0x97CF9E00CE4CF66DuL, 0x34F6DD75907BE939uL)),
                ((+1, 6, 0x8E399E12254B6612uL, 0x0BAB8E278A754127uL), (+1, 4, 0xE6909F2313BDABE1uL, 0xFA6BEFCD6AECE524uL)),
                ((+1, 5, 0xC119BD97CDB291F6uL, 0x3B440BBD4152BFAAuL), (+1, 4, 0x8C68FE538859F752uL, 0xA2351011FCECD5E0uL)),
                ((+1, 4, 0xD622BEA8343C09CAuL, 0xEBBDFE13F17A3FB4uL), (+1, 3, 0x8B3ED1417391D841uL, 0x34071DB9686CAECDuL)),
                ((+1, 3, 0xC401F362067B6719uL, 0x9576750790AE7D54uL), (+1, 1, 0xE30E9327FDE5BF4AuL, 0x34550F99B586EF21uL)),
                ((+1, 2, 0x950FEE34540F3151uL, 0x8011830FF26ED42BuL), (+1, 0, 0x99086F70F43194A7uL, 0x07D00C755AAF4187uL)),
                ((+1, 0, 0xBCFC9FE70C83E09BuL, 0xB2CB0C76DC06C960uL), (+1, -2, 0xAAE0748CAC53272CuL, 0xD4F285477C1ABBFAuL)),
                ((+1, -2, 0xC7D143B7FAF74CAFuL, 0x99B388FF9D598689uL), (+1, -4, 0x9DE210E600BD00B5uL, 0xE1EC29F5A6E78E02uL)),
                ((+1, -4, 0xAFCA6144883BEACAuL, 0xC1D89F1995DBA0E6uL), (+1, -7, 0xF06913B554EF1CC9uL, 0x709696CCBEB3CD91uL)),
                ((+1, -6, 0x8007A2D2C06A9BF9uL, 0x6E5B5AEAFE2711B1uL), (+1, -9, 0x95AE7A6D779258FEuL, 0x57039B0B4630BC74uL)),
                ((+1, -9, 0x991CA0E7CBE24C77uL, 0x3F8B2D0C2ECDED3CuL), (+1, -12, 0x96A4CAB8750C3F9DuL, 0xCB2A399B2FE08580uL)),
                ((+1, -12, 0x9482110FFBF99BDCuL, 0xE69876173DFB2259uL), (+1, -16, 0xF0F150C9836D9ABBuL, 0x045B31BD5944E645uL)),
                ((+1, -16, 0xE5A0872FCEF4312AuL, 0xD28C84D1CBE7F032uL), (+1, -19, 0x956D72A0BF455DF2uL, 0xFF7600E227B4456CuL)),
                ((+1, -19, 0x8A12F2692AC57EBAuL, 0x3980A446BE848C8FuL), (+1, -23, 0x8AC51D53AC3D4E49uL, 0x4B65D7BCB81E254AuL)),
                ((+1, -24, 0xF95B8A9E4B34099DuL, 0x639F925D388B4DB9uL), (+1, -28, 0xB71F2A2FADF0E7E0uL, 0xC19D5B73B76D04C7uL)),
                ((+1, -28, 0xA076188EE78038B5uL, 0xDA342DD76489DB95uL), (+1, -33, 0x9E065548ED298C98uL, 0xFADAA4B133CC21E1uL)),
                ((+1, -33, 0x8784965A4B61ABEBuL, 0xB05E2964C31F68D4uL), (+1, -39, 0x9A14DC11231EE106uL, 0xDBBDBC028F353EDAuL)),
                ((+1, -39, 0x81DD01D3CE935517uL, 0x0F10ED3FBBB970ABuL), (+1, -47, 0xF3A500C50F09C7CDuL, 0x3209C395AAF6C1B8uL)),
                ((+1, -47, 0xCAD97B8730B368B3uL, 0xA8DCFF5D19AADEC4uL), (+1, -71, 0x84A287EDF949F5EDuL, 0x4825FA4F0A5E2E39uL)),
        });

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> LtRcpBinpow16PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, 1, 0xC3B936DA560E93E7uL, 0x0FA4F448E10B5E7AuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xAC055386AE0782F1uL, 0xABE0A860B4FA5638uL), (+1, 1, 0xCEE88DDBDEE310B2uL, 0x0F5132F0D5D3266AuL)),
                ((+1, 4, 0x8F2C58383F1452CBuL, 0xD55FC9DAFC3930F2uL), (+1, 2, 0x9E101A3765937D06uL, 0xDD4308268273FFBCuL)),
                ((+1, 4, 0x960879F3C516D311uL, 0x74FA865EDD1DB759uL), (+1, 2, 0x97AEB8B6373C3AC5uL, 0x10B5EFAB9008AABBuL)),
                ((+1, 3, 0xDDFEDD3E8E26DEACuL, 0x509A29143E15E4BEuL), (+1, 1, 0xCCF9A458BDEA2AE2uL, 0x9B82C58FB489EB52uL)),
                ((+1, 2, 0xF68005A1B1EF30C0uL, 0xB9869B22472F8128uL), (+1, 0, 0xCF350DDE2D66190FuL, 0xC7ADFCF065CCAAE4uL)),
                ((+1, 1, 0xD521094DD1663DE2uL, 0x889522412716477EuL), (+1, -1, 0xA27F3F9511A45321uL, 0x910906205C72F40CuL)),
                ((+1, 0, 0x92F1DB30997743D3uL, 0x213B9B89D574C60DuL), (+1, -3, 0xCA59FAD074885C12uL, 0xEFA9A492077BAD1BuL)),
                ((+1, -2, 0xA420D5E7C805B701uL, 0x5C9B6088BA8D433EuL), (+1, -5, 0xCB0F9C4F1A7C36DBuL, 0x2B6C8AB15EB9D86FuL)),
                ((+1, -4, 0x95FF455EB11AB2C4uL, 0xF9141DB3FC37A15DuL), (+1, -7, 0xA5B9633B2F3AB4E6uL, 0xF3D421BDBDC2DFEBuL)),
                ((+1, -7, 0xE1A849C1AACC23CBuL, 0xCA6163A90180FCFBuL), (+1, -10, 0xDD0C79DB931DA0FDuL, 0xB11DE84090F04A4BuL)),
                ((+1, -9, 0x8C0E3A161366DDF2uL, 0x912003542C8934EFuL), (+1, -13, 0xF131CD667A90A2C9uL, 0xE2268A86018EB3E9uL)),
                ((+1, -12, 0x8F5D1074D1F63D8CuL, 0xCC093572B78067A9uL), (+1, -16, 0xD6C79BFCB52E4108uL, 0x20CE8B43229AFDD1uL)),
                ((+1, -16, 0xF12895FFF4F123FCuL, 0xBCE0AE8AA578EBBCuL), (+1, -19, 0x9B2BB76F02F82B59uL, 0x409C776EC1149931uL)),
                ((+1, -19, 0xA58879F94D628086uL, 0x487F7E572774B175uL), (+1, -23, 0xB422BD6EBEF1204BuL, 0x760808BC2A03261BuL)),
                ((+1, -23, 0xB78855B0FC922245uL, 0xB0D4A819EAB07CC7uL), (+1, -27, 0xA599C38A32FB865AuL, 0x328499FF2DF11298uL)),
                ((+1, -27, 0xA1EA6F306FEBC6B1uL, 0x5840022586CB21DDuL), (+1, -32, 0xEC3FDC0E014E64C7uL, 0x888ABD69CCC05EC1uL)),
                ((+1, -32, 0xDEABFE27669838C4uL, 0x5181ABDBE6C66228uL), (+1, -37, 0xFE2C0D97E5BB6DCFuL, 0x9D7B79B2F529768FuL)),
                ((+1, -37, 0xE7F2888BF0A1BA1AuL, 0x2FC61D616FF35A08uL), (+1, -42, 0xC6181BB8C6550C14uL, 0xC3E59599E12E5DB8uL)),
                ((+1, -42, 0xAFCBB49D7C96C957uL, 0x834F6AA92751452FuL), (+1, -48, 0xD2F4043CC838D338uL, 0x2BF4C27D667B7673uL)),
                ((+1, -48, 0xB6E160499A68F3CDuL, 0x35A2C6C2F750A311uL), (+1, -54, 0x8C2AA4385670DA45uL, 0xFA1CD0A23791AB2DuL)),
                ((+1, -55, 0xEE8B92E967B0577CuL, 0x14323EA939638742uL), (+1, -62, 0xC69654043DE84AABuL, 0x8A0E3EFC19B7EF92uL)),
                ((+1, -62, 0xA6B4FF5C4F5F9BD1uL, 0x1BFDA4D5CA7446DCuL), (+1, -71, 0xD309A9C0BA18695CuL, 0x1CED3443E437768BuL)),
                ((+1, -71, 0xAFB35894CAE6710BuL, 0x02CCD3284013C89DuL), Hexcode.Zero),
        });

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> LtRcpBinpow64PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, 2, 0xCF29205AA2682C91uL, 0x2318F3F266674276uL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0xA4F4CE7C9632C08EuL, 0xD7C7B87FAEF4FBD8uL), (+1, 0, 0xBB1ADBB84EBE35CFuL, 0x3DE0BBFDD5B298A3uL)),
                ((+1, 2, 0xF6A1595615AE9332uL, 0xECA5B5AB232771D5uL), (+1, -1, 0xFFE16319A9F8F8BDuL, 0xFD03C2FBF0B7284EuL)),
                ((+1, 1, 0xE5EFA32F3129CD71uL, 0x311A4F4DDFBADA4DuL), (+1, -2, 0xD957FEB198ED2189uL, 0x1587C7E9C4041EECuL)),
                ((+1, 0, 0x95C3043A923C6EE5uL, 0x3FD084F758EFF044uL), (+1, -3, 0x80627D91754BEE18uL, 0x6A60F81B49188E4AuL)),
                ((+1, -2, 0x90B26752381C6544uL, 0x5EFB0B9E67109108uL), (+1, -6, 0xDFD2165AA0D3E301uL, 0x5DA335E6FCCF12CAuL)),
                ((+1, -5, 0xD6E3DB5721289AD5uL, 0x722B4E178372206DuL), (+1, -8, 0x9509EB5EFFE96C85uL, 0xD7FCAE055BFC244EuL)),
                ((+1, -8, 0xFAC36A12BB49A7CBuL, 0x4C124FBEACEB0AD1uL), (+1, -11, 0x9ADED03AE1DCE61CuL, 0x759C33D7B6CBBD4FuL)),
                ((+1, -11, 0xE91BFCA2335B9DFEuL, 0x8303CC50E05EC431uL), (+1, -15, 0xFE4D754C610AEFDCuL, 0x68B33E2076CFE938uL)),
                ((+1, -14, 0xADF878CA768CC6F3uL, 0xA60536F81FB985FCuL), (+1, -18, 0xA601F05C76D6588FuL, 0xA839997BEB0CF2D9uL)),
                ((+1, -18, 0xD11E34862F84971CuL, 0xADB16848464402F3uL), (+1, -22, 0xAC8D9740869FED38uL, 0x8A38ED2DDB99AD04uL)),
                ((+1, -22, 0xCA3DA1A1554C94ECuL, 0x6A9CAA274DD7CE6DuL), (+1, -26, 0x8E5516774243FD09uL, 0x7C913350609D3AE5uL)),
                ((+1, -26, 0x9C9BA5BC0DE5869EuL, 0x0F65576F2714007DuL), (+1, -31, 0xB8E52380FAF84E12uL, 0xAF9EBCC6E3CEFC83uL)),
                ((+1, -31, 0xC07A8797B544781DuL, 0xC9E30A15283E5936uL), (+1, -36, 0xBAB8492AB638653AuL, 0x5BE43C90DBAABC60uL)),
                ((+1, -36, 0xB9309B1539493F30uL, 0x9BC6C4A7C5AAA792uL), (+1, -41, 0x8FD62F184FBEBB78uL, 0x2CF8379ADB87CF05uL)),
                ((+1, -41, 0x88C8758421812A35uL, 0xE85091762776FE9CuL), (+1, -47, 0xA4884C22C2BA4B61uL, 0xC5521384B505CFFDuL)),
                ((+1, -47, 0x96EAEEF22C3F0B76uL, 0x7448EEDB8B9635D6uL), (+1, -53, 0x8671EF3C63FD7B3EuL, 0x3D311A7CFE18BA16uL)),
                ((+1, -54, 0xEF41DC8BBC3816CCuL, 0xE88055FBA8645147uL), (+1, -60, 0x944F183465CD46EBuL, 0xAD9E8AAA5BB72AAEuL)),
                ((+1, -60, 0x80BA0235F080AC53uL, 0x41D440324679FAD8uL), (+1, -68, 0xCA2D67A757DE2B98uL, 0xA03C7F3C07C90A28uL)),
                ((+1, -68, 0xAC1DE14FCE732A0EuL, 0x8A261658B771FC82uL), (+1, -76, 0x91E629E9A6FE0CAEuL, 0x52C52A5DB59D1E2DuL)),
                ((+1, -77, 0xF4FC08E66BA842CAuL, 0x15E16F6D3AD29A68uL), (+1, -86, 0x9D563953E3610C9DuL, 0x04A5633E3CBCB5C3uL)),
                ((+1, -86, 0x82FDD3B944E4DC62uL, 0xCAB8184B23A19E22uL), Hexcode.Zero),
        });

        public static readonly ReadOnlyCollection<(Hexcode c, Hexcode d)> LtRcpBinpow256PadeTable
            = new(new (Hexcode c, Hexcode d)[]{
                ((+1, 3, 0xD33AF922960E954DuL, 0x25BEE1D86CA64B2CuL), (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL)),
                ((+1, 3, 0x9B2579FA400D9C07uL, 0xD54EED8AF9160E6DuL), (+1, -1, 0xABC92D8497ED4CDBuL, 0xAA8BF1CABE64A8B4uL)),
                ((+1, 1, 0xD4B4590163B733EBuL, 0x0393699392D54108uL), (+1, -3, 0xD634C48A8DE67699uL, 0x76F5DB8B66524A0CuL)),
                ((+1, -1, 0xB4AE439DBFBC2330uL, 0x9AE324320085E712uL), (+1, -5, 0xA4A83A5721C44C0DuL, 0x0762CDB4FDD126CDuL)),
                ((+1, -4, 0xD4F38B858E0C6386uL, 0xC90B37A279141B46uL), (+1, -8, 0xAE9D49D023F28F2EuL, 0x99F49C3C3515AFD0uL)),
                ((+1, -7, 0xB8BE6B836A97ED7BuL, 0x0D08667DE998126AuL), (+1, -11, 0x876B1837C8CAB928uL, 0x1F895C0A137E4DA8uL)),
                ((+1, -11, 0xF44B99DBE4C0DE94uL, 0x766691C3C34AEA0DuL), (+1, -15, 0x9EE40EC7A95A4323uL, 0xC4465E4FEC01350FuL)),
                ((+1, -15, 0xFB7D2825EF565F8DuL, 0xAD495D1BFA5C44ECuL), (+1, -19, 0x8FE597C343D22316uL, 0x5D31CD2FF8EB58B0uL)),
                ((+1, -19, 0xCC1D40D19DA67A96uL, 0x7CA7AD5FBE7FC18DuL), (+1, -24, 0xCB72027939FFAFD8uL, 0x8BD853A4C127C83DuL)),
                ((+1, -23, 0x83753219433A2112uL, 0x01643ECC1F6074A4uL), (+1, -29, 0xE195404D36B36AD3uL, 0x535D0944A4693E59uL)),
                ((+1, -28, 0x8691009C7C2A0B38uL, 0x7DD481D45C4D1809uL), (+1, -34, 0xC4069FCF3F52C197uL, 0xF4E384CF43B612DCuL)),
                ((+1, -34, 0xDA49FF4EBE914473uL, 0xB55490E84E6F6D32uL), (+1, -39, 0x84B7F1E49C32B1C2uL, 0x3397AA1FCE596F75uL)),
                ((+1, -39, 0x8B3E273E47A319C8uL, 0x3759F7310B7229E6uL), (+1, -45, 0x8A73F578F00CB2CFuL, 0x31DAEBA4F4C9B720uL)),
                ((+1, -45, 0x89FB12A307D0DBBEuL, 0x6786FB6B69E01F2EuL), (+1, -52, 0xDAA9625A2F6B2138uL, 0x8CCAC6F02618755FuL)),
                ((+1, -52, 0xD0845457B1132C8CuL, 0xD188B5821C7F3C88uL), (+1, -59, 0xFEBBCFD14DD4EE66uL, 0xC6FB91E54DE305FAuL)),
                ((+1, -59, 0xE9FF405F5A693ECAuL, 0xCF6965B26D14DB61uL), (+1, -66, 0xD2D821E4A1FED0F5uL, 0xC03981F5883DB3A1uL)),
                ((+1, -66, 0xBBBC62B153D9A179uL, 0xB123F79643A39FD5uL), (+1, -74, 0xEA9326BDFE77E95AuL, 0x261B98BB7F43A92AuL)),
                ((+1, -74, 0xCBA7E6E3313498D5uL, 0xF9DED2B5FA84144BuL), (+1, -82, 0xA0BA4C0BDD7E60CAuL, 0x1AA5AA83661DC351uL)),
                ((+1, -82, 0x88D741D6D7305CD2uL, 0x6675B365727AD28BuL), (+1, -92, 0xE8AE4924B6E6E930uL, 0x5F7F6379AE3460B7uL)),
                ((+1, -92, 0xC35ACD9388E4922CuL, 0x897113557E53F77FuL), (+1, -103, 0xFB68C3FF9576BB24uL, 0xE92ACEEF3CEFEF89uL)),
                ((+1, -103, 0xD14FDAC6A98EEC2CuL, 0xDF3E7AD933C76D3BuL), Hexcode.Zero),
        });
    }
}
