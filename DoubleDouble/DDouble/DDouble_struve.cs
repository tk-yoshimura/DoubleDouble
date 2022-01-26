﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble StruveH(int n, ddouble x) {
            if (x.Sign < 0) {
                return ((n & 1) == 0) ? -StruveH(n, -x) : StruveH(n, -x);
            }

            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveH function, n greater than 8 and negative integer are not supported."
                );
            }

            if (x < Epsilon) {
                return Zero;
            }

            if (IsNaN(x)) {
                return NaN;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return PlusZero;
                }
                if (n == 1) {
                    return 2d * RcpPI;
                }

                return PositiveInfinity;
            }

            if (x < 8d) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 32);
            }

            return StruveKIntegral.Value(n, x) + BesselY(n, x);
        }

        public static ddouble StruveK(int n, ddouble x) {
            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveK function, n greater than 8 and negative integer are not supported."
                );
            }

            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return PlusZero;
                }
                if (n == 1) {
                    return 2d * RcpPI;
                }

                return PositiveInfinity;
            }

            if (x < 8d) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 32) - BesselY(n, x);
            }

            return StruveKIntegral.Value(n, x);
        }

        public static ddouble StruveL(int n, ddouble x) {
            if (x.Sign < 0) {
                return ((n & 1) == 0) ? -StruveL(n, -x) : StruveL(n, -x);
            }

            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveL function, n greater than 8 and negative integer are not supported."
                );
            }

            if (IsNaN(x)) {
                return NaN;
            }

            if (x < Epsilon) {
                return Zero;
            }

            if (IsInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 1) {
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 32);
            }

            return StruveMIntegral.Value(n, x) + BesselI(n, x);
        }

        public static ddouble StruveM(int n, ddouble x) {
            if (n < 0 || n > 8) {
                throw new ArgumentException(
                    "In the calculation of the StruveM function, n greater than 8 and negative integer are not supported."
                );
            }

            if (x.Sign < 0 || IsNaN(x)) {
                return NaN;
            }

            if (x < Epsilon) {
                return (n >= 1) ? Zero : -1d;
            }

            if (IsInfinity(x)) {
                if (n == 0) {
                    return MinusZero;
                }
                if (n == 1) {
                    return -2d * RcpPI;
                }

                return PositiveInfinity;
            }

            if (x < 1) {
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 32) - BesselI(n, x);
            }

            return StruveMIntegral.Value(n, x);
        }

        internal static class StruveGTable {
            private static readonly List<ddouble> table = new();

            static StruveGTable() {
                table.Add(1);
            }

            public static ddouble Value(int k) {
                if (k < table.Count) {
                    return table[k];
                }

                for (int i = table.Count; i <= k; i++) {
                    ddouble g = table[^1] * 2 / (2 * i - 1);

                    table.Add(g);
                }

                return table[k];
            }
        }

        internal static class StruveKIntegral {
            public static ddouble Value(int n, ddouble x) {
                ddouble r = 1d / x;

                ddouble s = 0;

                foreach ((ddouble u, ddouble w) in gls) {
                    ddouble v = Pow(Sqrt(1d + Square(u * r)), 2 * n - 1);

                    s += w * v;
                }

                ddouble y = s * Pow(x / 2, n - 1) * RcpPI * StruveGTable.Value(n);

                return y;
            }

            static ReadOnlyCollection<(ddouble u, ddouble w)> gls = new(new (ddouble, ddouble)[] {
                ((+1, -6, 0xA35E134F29D1C917uL, 0x116A63D42B4AACACuL), (+1, -5, 0xCD7E16793872F914uL, 0x36A81219C6E1209BuL)),
                ((+1, -4, 0xD736E5E5EA79148DuL, 0x796484E7D94A203BuL), (+1, -4, 0xDBB215F587B3828DuL, 0x524E2E5B98940E14uL)),
                ((+1, -2, 0x8440A536F5287365uL, 0x7E756C26C76DA9FBuL), (+1, -3, 0x9421C66B45C21664uL, 0x638369491AA75E2AuL)),
                ((+1, -2, 0xF59C8C1461679B1FuL, 0x04FAA3298EAE08D5uL), (+1, -3, 0xA1F5A45DD24ABF67uL, 0xD12619123203E61FuL)),
                ((+1, -1, 0xC4F7D3967A98419BuL, 0x44A296D59F179D31uL), (+1, -3, 0x99A75B17F8FA9A54uL, 0x769305B5D430BC93uL)),
                ((+1, 0, 0x90530256C218DBA6uL, 0x5A9C7E35D2AA5821uL), (+1, -3, 0x821E9F9397EABD08uL, 0xD86AF035E2C78511uL)),
                ((+1, 0, 0xC6F1FFD84F971B43uL, 0x3F666FA435DE6D6BuL), (+1, -4, 0xC793C5A2E75FF8F1uL, 0x4EAA171F77DB1EEEuL)),
                ((+1, 1, 0x832FC5D2569BAF0CuL, 0x42D08D31C953B8E9uL), (+1, -4, 0x8BC3F80F73E81ABBuL, 0x2DB079F9F6C44EC6uL)),
                ((+1, 1, 0xA751B56109753049uL, 0x2ABF6EA8D8EAF88EuL), (+1, -5, 0xB3AF8E7C04F74D31uL, 0x747E640950425039uL)),
                ((+1, 1, 0xCFE341C79825395FuL, 0xAA09AD6DAE9FE106uL), (+1, -6, 0xD4C21F7317C890E7uL, 0x4794B40E85F2C0AFuL)),
                ((+1, 1, 0xFCE972044EC1DA71uL, 0x27116C5F826BB093uL), (+1, -7, 0xE88AF70E67855A62uL, 0x7CA1D597BB0F3CCBuL)),
                ((+1, 2, 0x9734F23678B45A6BuL, 0xF9BF16E66BE71C88uL), (+1, -8, 0xEAFB32CD1A040E3EuL, 0x7F5E176DDF13A709uL)),
                ((+1, 2, 0xB23569546C9CB6BDuL, 0xA1B02D66AC09D09DuL), (+1, -9, 0xDBC0342D4195F04CuL, 0xA98BC339A70D3387uL)),
                ((+1, 2, 0xCF798B12E3087379uL, 0x3F030BF99701CD12uL), (+1, -10, 0xBE52B4EE287AC637uL, 0x1F90DBE937082278uL)),
                ((+1, 2, 0xEF0516881C43DB77uL, 0x4F4FAAFCC9FD7DE2uL), (+1, -11, 0x98B8423FFA98C2F9uL, 0x98E70E6785B3FDE7uL)),
                ((+1, 3, 0x886E0FF63010BDA3uL, 0xFDED04F2026F6DF4uL), (+1, -13, 0xE31F3AA88D0BA23CuL, 0x35A44BFAFDC53069uL)),
                ((+1, 3, 0x9A8189D42AD819A7uL, 0x1E21FF34A53A1697uL), (+1, -14, 0x9C820D2983EA7CBEuL, 0x5BB3677D6AF074D2uL)),
                ((+1, 3, 0xADBF5CDA718430F1uL, 0x42316D6B475E40AEuL), (+1, -16, 0xC7DDB2810DB40C20uL, 0x9D878F764F867F1DuL)),
                ((+1, 3, 0xC22A1CA9949D526AuL, 0xF5038049141EDD85uL), (+1, -18, 0xEC728C94037EFDD9uL, 0x4DA26AD396D567C8uL)),
                ((+1, 3, 0xD7C48E92042136DFuL, 0xFCDD83DD4A763ACFuL), (+1, -19, 0x8185C143D7ECF9F8uL, 0x1672DD9389C05454uL)),
                ((+1, 3, 0xEE91ABD3C6463CC1uL, 0xF2FC33EDAFACDFEAuL), (+1, -21, 0x835A4E42C7C34113uL, 0x04F3719B095568EDuL)),
                ((+1, 4, 0x834A520B6FCA39A7uL, 0x38E5D54255FB726FuL), (+1, -24, 0xF67B7C8A9A229BD9uL, 0xAE2FFEE7575CD8D1uL)),
                ((+1, 4, 0x8FE87010E416F266uL, 0x01A701452391C32DuL), (+1, -26, 0xD5D2F61618FE6344uL, 0x19DA3B24B5347128uL)),
                ((+1, 4, 0x9D25026A00165E4CuL, 0xA80AB74F366DFE82uL), (+1, -28, 0xAB62EA9214EBD61EuL, 0x8938AC926ACFA4A6uL)),
                ((+1, 4, 0xAB01FB35D2D54649uL, 0x3DC431DF203D5486uL), (+1, -31, 0xFDA64D223AC84C38uL, 0x4402AB7E1A5EDFA0uL)),
                ((+1, 4, 0xB9816E0EEF7BB25DuL, 0x105A1645AEFF6F7EuL), (+1, -33, 0xAD23717CDC0550DFuL, 0x62553B7C13BB08DBuL)),
                ((+1, 4, 0xC8A59205802C6F7DuL, 0x91E3F1BDA5BF437BuL), (+1, -36, 0xD9D2650BBDE639ADuL, 0xDF66A3A9727AD6F3uL)),
                ((+1, 4, 0xD870C3CC4A8A141DuL, 0x504D11429BA6725AuL), (+1, -39, 0xFC46FF740D7A1F6BuL, 0xA0647B5FAFBA45D5uL)),
                ((+1, 4, 0xE8E5881E73D6592FuL, 0xD7D3B644C2495416uL), (+1, -41, 0x8657197D17472386uL, 0x34745FBC35D3B921uL)),
                ((+1, 4, 0xFA068E64AB806FC0uL, 0xE741B0A747ADCC24uL), (+1, -44, 0x836821D3DB9E3D10uL, 0x746BD0DEE4BA2BEFuL)),
                ((+1, 5, 0x85EB59D0B0526D9EuL, 0xF10B9DCBBE7A7508uL), (+1, -48, 0xEBCCE5FC0EE00203uL, 0xC807C54AF03B8925uL)),
                ((+1, 5, 0x8F2C82D6686AD99DuL, 0x3FD9255A6608D18BuL), (+1, -51, 0xC1C84B95A30B5C1CuL, 0xC792658C07EC0C6DuL)),
                ((+1, 5, 0x98C863658EC19D0FuL, 0x9AE75AED2E5B47AAuL), (+1, -54, 0x91A56BC6F14ABF67uL, 0x5925B936BE254398uL)),
                ((+1, 5, 0xA2C0B8D41A5712F5uL, 0xD3AAB5C0A9568C9EuL), (+1, -58, 0xC7E772CA477FDB96uL, 0x755562D24452D5A5uL)),
                ((+1, 5, 0xAD175EE3A38036E1uL, 0x60A84B55623CECB8uL), (+1, -62, 0xFA16865BADBD017CuL, 0xC9F48EAE2A0033B4uL)),
                ((+1, 5, 0xB7CE523AE91D248CuL, 0x68FEF1D790A79C2FuL), (+1, -65, 0x8E5204E1F10EDA5CuL, 0xA1BF697F859B8341uL)),
                ((+1, 5, 0xC2E7B32A84551C09uL, 0x8959D74B1771737BuL), (+1, -69, 0x931252EC877B4CD8uL, 0x0ADE68824D09771FuL)),
                ((+1, 5, 0xCE65C8C7D8335BB5uL, 0xA464897D1E199BC8uL), (+1, -73, 0x89B0FCA8A6CB155EuL, 0x4470AB05924DD73FuL)),
                ((+1, 5, 0xDA4B046B4CE2FEB2uL, 0x5774483C1C153646uL), (+1, -78, 0xE908A8E948EAB0D0uL, 0xAA1A013F9359322AuL)),
                ((+1, 5, 0xE69A05A1581D1ED5uL, 0xE42FF510CCE2D681uL), (+1, -82, 0xB1CB355F620A1B2AuL, 0x20445459D0AF3C7DuL)),
                ((+1, 5, 0xF3559EA0D1FD2503uL, 0xB337DADCDDC37A09uL), (+1, -87, 0xF3F113EB166C9179uL, 0xB48FD435857DFFE9uL)),
                ((+1, 6, 0x80406CAE61C60153uL, 0xC8840CE67885AD69uL), (+1, -91, 0x9609BFF0481A008DuL, 0x0E7A83FFAD5DAE58uL)),
                ((+1, 6, 0x870F7EA635614910uL, 0xBC24BECD90B0043DuL), (+1, -96, 0xA4F37771088DEB1BuL, 0x14F9E4FAB7C0A7C1uL)),
                ((+1, 6, 0x8E19CB04EEB1DEADuL, 0x09DF7D0A3AF3A9B0uL), (+1, -101, 0xA1861048CC2D6918uL, 0x8087469AE88B109FuL)),
                ((+1, 6, 0x95613D776A1849E8uL, 0xBE9C2DF7CC3C286CuL), (+1, -106, 0x8C5BD6015C59F92BuL, 0x076511A3B94CC4EAuL)),
                ((+1, 6, 0x9CE7EBF015003257uL, 0xCB8E11CAEA97C160uL), (+1, -112, 0xD799BCE6D5F163CCuL, 0x5A9C5F7A95F38C9AuL)),
                ((+1, 6, 0xA4B01BC20A48D726uL, 0x331E220FCB98014CuL), (+1, -117, 0x91B73BFB6BD24AB3uL, 0x2DA78D5CF2453721uL)),
                ((+1, 6, 0xACBC4796EE3DF148uL, 0x980DC86F159D77C2uL), (+1, -123, 0xAC80690F5912F994uL, 0x9865537EB630E167uL)),
                ((+1, 6, 0xB50F266E7C8F8A4FuL, 0xF7971EF563632A2DuL), (+1, -129, 0xB1E9740AEC52FEADuL, 0x6310A5308D4A2D13uL)),
                ((+1, 6, 0xBDABB3E33F6DECF7uL, 0x0C2AFD7721DCA788uL), (+1, -135, 0x9EF180F0ED3A8256uL, 0xA16E781571D83F81uL)),
                ((+1, 6, 0xC6953A0076DE3B0CuL, 0xF9FC52F037C9CF11uL), (+1, -142, 0xF47176842BDF0995uL, 0xD2A3CD1E2B2E587AuL)),
                ((+1, 6, 0xCFCF5D0B60BE6D5EuL, 0x68AD7675714717DDuL), (+1, -148, 0xA0A81A1090D0396BuL, 0x56CDBB8228E743EEuL)),
                ((+1, 6, 0xD95E29C0171DD340uL, 0xF980044DA11E8735uL), (+1, -155, 0xB316D1DA741BA631uL, 0x0ABB594B729005A2uL)),
                ((+1, 6, 0xE34626AB70E93318uL, 0x0CE822D0493A04A4uL), (+1, -162, 0xA7D412A558A7ABBCuL, 0x70F829E829AF85C8uL)),
                ((+1, 6, 0xED8C6984B99504C0uL, 0xFCC9C8A056EF6B07uL), (+1, -169, 0x82ECF819973B5830uL, 0x48B342F2EEA48896uL)),
                ((+1, 6, 0xF836B1BB578695DDuL, 0x1A70BB69BE16B22CuL), (+1, -177, 0xA82C9CA8C6904341uL, 0xC32F77BB3E5D6064uL)),
                ((+1, 7, 0x81A5C4F0C6356B2BuL, 0x2B27A110BE7235A4uL), (+1, -185, 0xAF9D1AEAA1C6152AuL, 0x50E24539F816C0E6uL)),
                ((+1, 7, 0x87693924BE642A8CuL, 0x08C0D3F766F21420uL), (+1, -193, 0x92F068757EEDFF15uL, 0xF1E652F26CB5EBD8uL)),
                ((+1, 7, 0x8D6A0C15FB5B0A03uL, 0xAE789767EBA09851uL), (+1, -202, 0xC1C22A6AAFD525DAuL, 0xF961EF4184641D76uL)),
                ((+1, 7, 0x93AD4F1EF45F8C30uL, 0x7A37DB1ECC014E89uL), (+1, -211, 0xC56E435EB25FD83EuL, 0xE10032307AE516CFuL)),
                ((+1, 7, 0x9A38FF0726E93CE6uL, 0x82C80D4B98E19F2AuL), (+1, -220, 0x97E8906D6CFA9EF1uL, 0xB7D0B8C60297A42DuL)),
                ((+1, 7, 0xA11446C48A90E7F8uL, 0x97BCFDB941E1E021uL), (+1, -230, 0xABB6FE35C88AC692uL, 0x641E25941564A32DuL)),
                ((+1, 7, 0xA847DD6311231AD4uL, 0xD7A905C7E8A5DCE9uL), (+1, -240, 0x89E1598FD0513457uL, 0x8FDED9A763150FC5uL)),
                ((+1, 7, 0xAFDE8E256531ED4DuL, 0x04E476719C6870B6uL), (+1, -251, 0x96E573BE25DAB6C4uL, 0xEF82233732719560uL)),
                ((+1, 7, 0xB7E6051610D92442uL, 0x3F9D251B2FCD44C2uL), (+1, -263, 0xD5923910B71965BDuL, 0xCFCB4C51C80E7FEAuL)),
                ((+1, 7, 0xC0701038F70EE234uL, 0x20F2A1C1A1466723uL), (+1, -275, 0xB690FB2B28098630uL, 0x60BFBD8CE1A47521uL)),
                ((+1, 7, 0xC994B43358FF1B11uL, 0x634D8B4CBD998DACuL), (+1, -288, 0xABF54F00E0B44C57uL, 0x427843B85EE4C38AuL)),
                ((+1, 7, 0xD375DFF2583A7B24uL, 0x8C19E53BDD7ACE3BuL), (+1, -302, 0x9CC632394896DC93uL, 0x7B83931DDED09C73uL)),
                ((+1, 7, 0xDE46A8D83AA3511AuL, 0xDF6F1666E33A0009uL), (+1, -318, 0xE3CAB366726A8B12uL, 0x9A11E2AAE22D4BFBuL)),
                ((+1, 7, 0xEA5B72EECF78A3DCuL, 0x3AC5AFD08733C7EAuL), (+1, -335, 0xBF863FBD4476486DuL, 0x40E41868AA4EBE1FuL)),
                ((+1, 7, 0xF856544EFF617994uL, 0x7DB4D464FF35CD60uL), (+1, -355, 0xCA6A6360F3685980uL, 0x03CED5ABEA4AE30CuL)),
                ((+1, 8, 0x84ED3E8D719120EEuL, 0x759971C895EFCB99uL), (+1, -380, 0xE23C5091EBF6E451uL, 0xED94E299A4FBFCEAuL)),
            });
        }

        internal static class StruveMIntegral {
            public static ddouble Value(int n, ddouble x) {
                ddouble divs = ddouble.Ceiling(x / 16);
                ddouble q = Rcp(divs);

                ddouble s = 0;

                bool convergenced = false;

                for (int i = 0; i < divs && i < 8 && !convergenced; i++) {
                    ddouble ddi = i;

                    foreach ((ddouble u, ddouble w) in gls) {
                        ddouble u_sft = Ldexp((ddi + u) * q, -1);

                        ddouble v = Exp(-x * SinPI(u_sft)) * Pow(Square(CosPI(u_sft)), n);

                        if (v.hi < s.hi * 1e-31) {
                            convergenced = true;
                            break;
                        }

                        s += w * v;
                    }
                }

                ddouble y = -s * Pow(x / 2, n) * StruveGTable.Value(n) * q;

                return y;
            }

            static ReadOnlyCollection<(ddouble u, ddouble w)> gls = new(new (ddouble, ddouble)[] {
                ((+1, -10, 0xB350C7DFB25074CCuL, 0xD660038622DCC761uL), (+1, -9, 0xE5FC5E3A1E62E234uL, 0x4CAB1E22737F7476uL)),
                ((+1, -8, 0xEBBDB1D68648D57FuL, 0xD070632FC4ADD9A8uL), (+1, -7, 0x8551E12434B64887uL, 0xA3E473AAC279FA68uL)),
                ((+1, -6, 0x905573FD622AD0DCuL, 0x410B2BBE766D2534uL), (+1, -7, 0xD003054298FFD187uL, 0xC62E907B684CFE29uL)),
                ((+1, -5, 0x854FF698202F7316uL, 0x3CB59A289DF4EEE6uL), (+1, -6, 0x8C62C0051AAEC9E6uL, 0xCEB745A5101E449BuL)),
                ((+1, -5, 0xD45592EA2CD1AD18uL, 0xD747C2CA25D125DFuL), (+1, -6, 0xAF74B1D19AA4A545uL, 0x58F93CBAC7B74914uL)),
                ((+1, -4, 0x9A3F6052F2BDA015uL, 0xE7F054D3C6C589E4uL), (+1, -6, 0xD0E3574B0FDFD1B8uL, 0x165E112E1E037772uL)),
                ((+1, -4, 0xD272D6FC768759D4uL, 0x220FBC4E6018E2DBuL), (+1, -6, 0xF05EBB64924C0B66uL, 0x21D858CB4AF3216CuL)),
                ((+1, -3, 0x891F6CE296C46474uL, 0xE744BFF84EB35F38uL), (+1, -5, 0x86CDCD3165607E79uL, 0x90400C2A922878CEuL)),
                ((+1, -3, 0xAC85763B78076463uL, 0x6F9353949C353B8DuL), (+1, -5, 0x942A081D9AF060F1uL, 0x832005563CA5419EuL)),
                ((+1, -3, 0xD316EB969D35BC6EuL, 0xEF288461844CB7BAuL), (+1, -5, 0xA0241F0937E89D58uL, 0x78B12A153AFC5E59uL)),
                ((+1, -3, 0xFC779D7994D62B96uL, 0xBD65774FAA0DCDAEuL), (+1, -5, 0xAA9F712F5F5E33D1uL, 0xFDEA60FB3C322F2EuL)),
                ((+1, -2, 0x942252A0E41C3B16uL, 0x4C47948972048AABuL), (+1, -5, 0xB382F0C709F68812uL, 0xAB301C3451314C61uL)),
                ((+1, -2, 0xAB0AA8C6A108293FuL, 0x8B40A1A6A0CC21D7uL), (+1, -5, 0xBAB95ED9FB72847CuL, 0x0044AA4874CECB03uL)),
                ((+1, -2, 0xC2BE103CC7ED6949uL, 0xFC84BB2C19D64269uL), (+1, -5, 0xC0317E07B7F7C792uL, 0x5EA945DC06011DCBuL)),
                ((+1, -2, 0xDB03E2B1D01B9193uL, 0x3AAC535997C54E56uL), (+1, -5, 0xC3DE3BB7C636B71CuL, 0x171A1C76239D602FuL)),
                ((+1, -2, 0xF3A21BDBB99DB393uL, 0xF76FC275B3417FE2uL), (+1, -5, 0xC5B6CF5763BD6961uL, 0x058951877815627AuL)),
                ((+1, -1, 0x862EF21223312636uL, 0x04481EC5265F400EuL), (+1, -5, 0xC5B6CF5763BD6961uL, 0x058951877815627AuL)),
                ((+1, -1, 0x927E0EA717F23736uL, 0x62A9D653341D58D4uL), (+1, -5, 0xC3DE3BB7C636B71CuL, 0x171A1C76239D602FuL)),
                ((+1, -1, 0x9EA0F7E19C094B5BuL, 0x01BDA269F314DECBuL), (+1, -5, 0xC0317E07B7F7C792uL, 0x5EA945DC06011DCBuL)),
                ((+1, -1, 0xAA7AAB9CAF7BEB60uL, 0x3A5FAF2CAF99EF14uL), (+1, -5, 0xBAB95ED9FB72847CuL, 0x0044AA4874CECB03uL)),
                ((+1, -1, 0xB5EED6AF8DF1E274uL, 0xD9DC35BB46FDBAAAuL), (+1, -5, 0xB382F0C709F68812uL, 0xAB301C3451314C61uL)),
                ((+1, -1, 0xC0E218A19ACA751AuL, 0x50A6A22C157C8C94uL), (+1, -5, 0xAA9F712F5F5E33D1uL, 0xFDEA60FB3C322F2EuL)),
                ((+1, -1, 0xCB3A451A58B290E4uL, 0x4435DEE79EECD211uL), (+1, -5, 0xA0241F0937E89D58uL, 0x78B12A153AFC5E59uL)),
                ((+1, -1, 0xD4DEA27121FE26E7uL, 0x241B2B1AD8F2B11CuL), (+1, -5, 0x942A081D9AF060F1uL, 0x832005563CA5419EuL)),
                ((+1, -1, 0xDDB824C75A4EE6E2uL, 0xC62ED001EC532831uL), (+1, -5, 0x86CDCD3165607E79uL, 0x90400C2A922878CEuL)),
                ((+1, -1, 0xE5B1A520712F14C5uL, 0x7BBE087633FCE3A4uL), (+1, -6, 0xF05EBB64924C0B66uL, 0x21D858CB4AF3216CuL)),
                ((+1, -1, 0xECB813F5A1A84BFDuL, 0x4301F56587275017uL), (+1, -6, 0xD0E3574B0FDFD1B8uL, 0x165E112E1E037772uL)),
                ((+1, -1, 0xF2BAA6D15D32E52EuL, 0x728B83D35DA2E850uL), (+1, -6, 0xAF74B1D19AA4A545uL, 0x58F93CBAC7B74914uL)),
                ((+1, -1, 0xF7AB00967DFD08CEuL, 0x9C34A65D7620AD14uL), (+1, -6, 0x8C62C0051AAEC9E6uL, 0xCEB745A5101E449BuL)),
                ((+1, -1, 0xFB7D546014EEA979uL, 0x1DF7A6A20C4C96D6uL), (+1, -7, 0xD003054298FFD187uL, 0xC62E907B684CFE29uL)),
                ((+1, -1, 0xFE28849C52F36E55uL, 0x005F1F39A076A53AuL), (+1, -7, 0x8551E12434B64887uL, 0xA3E473AAC279FA68uL)),
                ((+1, -1, 0xFFA6579C1026D7C5uL, 0x9994CFFE3CEE8CD3uL), (+1, -9, 0xE5FC5E3A1E62E234uL, 0x4CAB1E22737F7476uL)),
            });
        }


        internal static class StruveHLNearZero {
            public static ddouble Value(int n, ddouble x, bool sign_switch, int terms) {
                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble s = 0, u = Pow(x / 2, n + 1) * RcpPI;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble w = x2 * FTable.Value(2 * k) * FTable.Value(2 * k + n);
                    ddouble ds = Ldexp(u * StruveGTable.Value(2 * k + 1) * StruveGTable.Value(2 * k + n + 1), -4 * k)
                                  * (sign_switch ? (1d - w) : (1d + w));

                    ddouble s_next = s + ds;

                    if (s == s_next || !IsFinite(s_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    s = s_next;
                    u *= x4;
                }

                return s;
            }

            public static class FTable {
                private static readonly List<ddouble> table = new();

                public static ddouble Value(int k) {
                    if (k < table.Count) {
                        return table[k];
                    }

                    for (int i = table.Count; i <= k; i++) {
                        ddouble f = Rcp(2 * i + 3);

                        table.Add(f);
                    }

                    return table[k];
                }
            }
        }
    }
}