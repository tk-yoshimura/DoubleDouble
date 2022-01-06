using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {

        public static ddouble Gamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble sinpi = SinPI(x);

                if (IsZero(sinpi)) {
                    return NaN;
                }

                ddouble y = PI / (sinpi * Gamma(1d - x));

                return y;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                ddouble v = Gamma(z);

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                return v / w;
            }
            else {
                ddouble r = Sqrt(2 * PI / x);
                ddouble p = Pow(x / E, x);
                ddouble s = Exp(SterlingTerm(x));

                ddouble y = r * p * s;
                ddouble y_round = (x <= 21.5d) ? ddouble.RoundMantissa(y, 99) : ddouble.RoundMantissa(y, 97);

                return y_round;
            }
        }

        public static ddouble LogGamma(ddouble x) {
            if (IsNaN(x) || IsMinusZero(x) || x.Sign < 0) {
                return NaN;
            }
            if (IsPlusZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                return Log(Gamma(x));
            }

            if (x >= 1d - Math.ScaleB(1, -3) && x <= 1d + Math.ScaleB(1, -3)) {
                x -= 1d;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.TaylorX1CoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x + table[i];
                }
                s *= x;

                return s;
            }
            if (x >= 2d - Math.ScaleB(1, -3) && x <= 2d + Math.ScaleB(1, -3)) {
                x -= 2d;

                ReadOnlyCollection<ddouble> table = Consts.LogGamma.TaylorX2CoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x + table[i];
                }
                s *= x;

                return s;
            }

            static ddouble sterling_loggamma(ddouble x) {
                ddouble p = (x - 0.5d) * ddouble.Log(x);
                ddouble s = SterlingTerm(x);

                ddouble k = Consts.LogGamma.SterlingLogBias;

                ddouble y = k + p + s - x;

                return y;
            }

            if (x < Consts.Gamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Gamma.Threshold;
                ddouble v = sterling_loggamma(z);

                ddouble w = f + n;
                for (int i = n + 1; i < Consts.Gamma.Threshold; i++) {
                    w *= f + i;
                }

                v -= Log(w);

                ddouble y = v;

                return y;
            }
            else {
                ddouble y = sterling_loggamma(x);

                return y;
            }
        }

        public static ddouble Digamma(ddouble x) {
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsZero(x) || IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }

            if (x < 0.5d) {
                ddouble tanpi = TanPI(x);

                if (IsZero(tanpi)) {
                    return NaN;
                }

                ddouble y = Digamma(1 - x) - PI / tanpi;

                return y;
            }

            ddouble x_zsft = x - DigammaZero;
            if (Abs(x_zsft) < Math.ScaleB(1, -6)) {
                ReadOnlyCollection<ddouble> table = Consts.Digamma.TaylorZeropointCoefTable;

                ddouble s = table[^1];
                for (int i = table.Count - 2; i >= 0; i--) {
                    s = s * x_zsft + table[i];
                }
                s *= x_zsft;

                return s;
            }

            static ddouble sterling_digamma(ddouble x) {
                ddouble s = DiffLogSterlingTerm(x);
                ddouble p = ddouble.Log(x);
                ddouble c = Rcp(x) / 2;

                ddouble y = -s + p - c;

                return y;
            }

            if (x < Consts.Digamma.Threshold) {
                int n = (int)Floor(x);
                ddouble f = x - n;
                ddouble z = f + Consts.Digamma.Threshold;
                ddouble v = sterling_digamma(z);
                ddouble s = SumFraction(f + n, Consts.Digamma.Threshold - n);

                ddouble y = v - s;

                return y;
            }
            else {
                ddouble y = sterling_digamma(x);

                return y;
            }
        }

        private static ddouble SterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Gamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        private static ddouble DiffLogSterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v2;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in Consts.Digamma.SterlingTable) {
                ddouble dy = u * s * (1d - v2 * r);
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                u *= v4;
                y = y_next;
            }

            return y;
        }

        private static ddouble SumFraction(ddouble x, int n) {
            // sum( 1 / (x + i) , i = 0, n)

            static ddouble sum1(ddouble s, ddouble x) {
                s += Rcp(x);
                return s;
            }

            static ddouble sum2(ddouble s, ddouble x) {
                s += (2d * x + 1d) / (x * (x + 1d));
                return s;
            }

            static ddouble sum4(ddouble s, ddouble x) {
                s += (2d * (2d * x + 3d) * (x * (x + 3d) + 1d)) / (x * (x + 1) * (x + 2) * (x + 3));
                return s;
            }

            ddouble s = 0d;
            int i = 0;
            for (; i < n - 3; i += 4) {
                s = sum4(s, x + i);
            }
            for (; i < n - 1; i += 2) {
                s = sum2(s, x + i);
            }
            for (; i < n; i++) {
                s = sum1(s, x + i);
            }

            return s;
        }

        internal static partial class Consts {
            public static class Gamma {
                public const int Threshold = 12;

                public static ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable = new(new (ddouble s, ddouble r)[]{
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, -5, 0x8888888888888888uL, 0x8888888888888888uL)),
                    ((+1, -11, 0xD00D00D00D00D00DuL, 0x00D00D00D00D00D0uL), (+1, -1, 0xC000000000000000uL, 0x0000000000000000uL)),
                    ((+1, -11, 0xDCA8F158C7F91AB8uL, 0x7539C0372A3C5631uL), (+1, 1, 0x91CB1CB1CB1CB1CBuL, 0x1CB1CB1CB1CB1CB1uL)),
                    ((+1, -8, 0xD20D20D20D20D20DuL, 0x20D20D20D20D20D2uL),  (+1, 2, 0x9384511DEAB78451uL, 0x1DEAB784511DEAB7uL)),
                    ((+1, -3, 0xB7F4B1C0F033FFD0uL, 0xC3B7F4B1C0F033FFuL),  (+1, 2, 0xF808968F383D5119uL, 0x244D3527089A63B2uL)),
                    ((+1, 3, 0xD672219167002D3AuL, 0x7A9C886459C00B4EuL),   (+1, 3, 0xBB3DD3DA9AC17B58uL, 0xADC81A9890CA341EuL)),
                    ((+1, 11, 0x8911A740DA740DA7uL, 0x40DA740DA740DA74uL),  (+1, 4, 0x83B7B085A5F7689EuL, 0xCD9552B83C7309DCuL)),
                    ((+1, 19, 0xA8D1044D3708D1C2uL, 0x19EE4FDC4469CCAEuL),  (+1, 4, 0xB04C820CF37B0F22uL, 0x50D78B1D73F478F5uL)),
                    ((+1, 28, 0xB694D07B219DBCC4uL, 0x8676F31219DBCC48uL),  (+1, 4, 0xE35D5F580E37ECB4uL, 0xE1B9AA804FE50FA7uL)),
                    ((+1, 38, 0xA1BBCDE4EA012735uL, 0x0B88127350B88127uL),  (+1, 5, 0x8E75243CF95640BAuL, 0xB1AC741F13538518uL)),
                    ((+1, 48, 0xDE466B7C78FBAAE3uL, 0xC3A9E6DAEAE46D98uL),  (+1, 5, 0xAE799EBA1DCEB485uL, 0xFC9877597F7685B3uL)),
                    ((+1, 59, 0xE2E1337F5AF0BED9uL, 0x0B6B0A352D4F335CuL),  (+1, 5, 0xD1BC1F238533A53CuL, 0x065E1F08A600A21BuL))
                });
            }

            public static class LogGamma {
                public static readonly ddouble SterlingLogBias = Log(PI * 2) / 2;

                public static ReadOnlyCollection<ddouble> TaylorX1CoefTable = new(new ddouble[] {
                    (-1, -1, 0x93C467E37DB0C7A4uL, 0xD1BE3F810152CB56uL),
                    (+1, -1, 0xD28D3312983E9918uL, 0x73D8912200BACE5EuL),
                    (-1, -2, 0xCD26AADF5596B7ACuL, 0xF64B92E699B5AA12uL),
                    (+1, -2, 0x8A8991563EC241B5uL, 0xF91211196E5235FBuL),
                    (-1, -3, 0xD45CE0BD530A492EuL, 0x826A4FDAE19904DCuL),
                    (+1, -3, 0xADA06588061830A5uL, 0x3CCA078AB4AD8EFBuL),
                    (-1, -3, 0x9381D0EE751D72C6uL, 0x0FFA0F2958701A43uL),
                    (+1, -3, 0x80859B57C31CB745uL, 0xF2CE526EDAD3266DuL),
                    (-1, -4, 0xE4033836ABEDA2ADuL, 0x5500DFBA7DB22231uL),
                    (+1, -4, 0xCD00F1C2EAFC7981uL, 0x3418F3768CD66B0EuL),
                    (-1, -4, 0xBA461988A636B07AuL, 0x95D890201C7F1587uL),
                    (+1, -4, 0xAAB56B1921DE202BuL, 0xBD0B01F5D03A1CCCuL),
                    (-1, -4, 0x9D8ECB8FE2CC261AuL, 0x79F81B6C2036AFF1uL),
                    (+1, -4, 0x924B6FC19062FB9EuL, 0x9695C0817D0A6441uL),
                    (-1, -4, 0x88899A3B73FF01DCuL, 0x9A4FF396EE34EC23uL),
                    (+1, -4, 0x8000803266F59178uL, 0x79D0156AFFDBC10BuL),
                    (-1, -5, 0xF0F16988F455E4E5uL, 0xABDDE62D5294E08BuL),
                    (+1, -5, 0xE38E71D105A0C482uL, 0xD3D05F949F431375uL),
                    (-1, -5, 0xD79450DAB44502E8uL, 0xE83D10BFDF68DA3BuL),
                    (+1, -5, 0xCCCCD99A96ADD052uL, 0x546D31AF95B3951CuL),
                    (-1, -5, 0xC30C36DBBDFDFADBuL, 0xDBDE3EB0EEF83E63uL),
                    (+1, -5, 0xBA2E8E8BBC6FC881uL, 0x5C7F0544C73B8B65uL),
                    (-1, -5, 0xB216442C8DB365DFuL, 0x1755C6A07401D738uL),
                    (+1, -5, 0xAAAAAB5557EE6AA9uL, 0x40BF9C09DEC5D623uL),
                    (-1, -5, 0xA3D70A8F5CFDBBA0uL, 0x71A9A8CA784BF5F2uL),
                    (+1, -5, 0x9D89D8C4EC92F3E9uL, 0x5AB513F0C18E1DD3uL),
                    (-1, -5, 0x97B426000015E20AuL, 0xB4774BCD423BA824uL),
                    (+1, -5, 0x9249249B6DBDE3E4uL, 0xC63869E597816D33uL),
                    (-1, -5, 0x8D3DCB0D3DCD4C3DuL, 0xD25A1DC8C88BD742uL),
                    (+1, -5, 0x8888888AAAAB655BuL, 0x063B7A506E1E3948uL),
                    (-1, -5, 0x8421084318C66DC4uL, 0x964E06A2E7462BFDuL),
                    (+1, -5, 0x8000000080001371uL, 0xFB227A6A8D4C0A99uL),
                });

                public static ReadOnlyCollection<ddouble> TaylorX2CoefTable = new(new ddouble[] {
                    (+1, -2, 0xD8773039049E70B6uL, 0x5C8380FDFD5A6952uL),
                    (+1, -2, 0xA51A6625307D3230uL, 0xE7B1224401759CBDuL),
                    (-1, -4, 0x89F000D2ABB03409uL, 0x2E83A0EFBC2BFD9DuL),
                    (+1, -6, 0xA8991563EC241B5FuL, 0x91211196E5235FBCuL),
                    (-1, -8, 0xF2027E10C7AF8C36uL, 0xB3B061C2998701E7uL),
                    (+1, -9, 0xBD6EB756DB617EA4uL, 0x87D7380280B91426uL),
                    (-1, -10, 0x9C562E15FC703E75uL, 0xB3E30263137AD8D9uL),
                    (+1, -11, 0x859B57C31CB745F2uL, 0xCE526EDAD3266DC3uL),
                    (-1, -13, 0xE9FEA63B697E3E38uL, 0x3AA3033447D29CB5uL),
                    (+1, -14, 0xD093D878BEB2D19DuL, 0x309AA7002679085CuL),
                    (-1, -15, 0xBC6F2DEBE40F7797uL, 0x7EAF8C86E1666C25uL),
                    (+1, -16, 0xAC06E77337581126uL, 0x0574B258F72222C0uL),
                    (-1, -17, 0x9E5E4B1E7112142BuL, 0x523270559AEA87D7uL),
                    (+1, -18, 0x92CBD1CF9A555C80uL, 0xDDD73AB04FEBC73FuL),
                    (-1, -19, 0x88D975BB3CAA08E3uL, 0xB58732D631CD43F0uL),
                    (+1, -20, 0x803266F5917879D0uL, 0x156AFFDBC10B5834uL),
                    (-1, -22, 0xF13006C9E7E975D9uL, 0xEA78C347DF3575C5uL),
                    (+1, -23, 0xE3B5DD9F83D26BB3uL, 0x456EEED36A479B3AuL),
                    (-1, -24, 0xD7AD365DFC54BB2BuL, 0xE44FD2DB07C41AF7uL),
                    (+1, -25, 0xCCDC9E1038587A06uL, 0x4E2C8E6C84F4F593uL),
                    (-1, -26, 0xC31639A6F9F56366uL, 0x50057D81B40BD395uL),
                    (+1, -27, 0xBA34ED667D6E6592uL, 0xC58EE628AAEFA88BuL),
                    (-1, -28, 0xB21A54223D75681AuL, 0xF72EDF876FCCC233uL),
                    (+1, -29, 0xAAAD43BFFE9614F1uL, 0x5F341B2B789DB0F1uL),
                    (-1, -30, 0xA3D8B3C92C687209uL, 0xE6DC1D0A9C7A1E78uL),
                    (+1, -31, 0x9D8AE9597E085E28uL, 0x60901114D74D2998uL),
                    (-1, -32, 0x97B4D4FD5F1EFCBDuL, 0x3AA991670EDADCEEuL),
                    (+1, -33, 0x92499519BA1A620CuL, 0x1053848A11D1B0ABuL),
                    (-1, -34, 0x8D3E13761291E29EuL, 0x9D7B6940969463C2uL),
                    (+1, -35, 0x8888B7349F6CBC71uL, 0xF9656C30072B8E20uL),
                    (-1, -36, 0x8421265E2A1EC140uL, 0xEF820DBC9EA5B468uL),
                    (+1, -37, 0x80001371FB227A6AuL, 0x8D4C0A99112BE14BuL)
                });
            }

            public static class Digamma {
                public const int Threshold = 11;

                public static ReadOnlyCollection<(ddouble s, ddouble r)> SterlingTable = new(new (ddouble s, ddouble r)[]{
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, -4, 0xCCCCCCCCCCCCCCCCuL, 0xCCCCCCCCCCCCCCCCuL)),
                    ((+1, -8, 0x8208208208208208uL, 0x2082082082082082uL),  (+1, 0, 0x8666666666666666uL, 0x6666666666666666uL)),
                    ((+1, -8, 0xF83E0F83E0F83E0FuL, 0x83E0F83E0F83E0F8uL),  (+1, 1, 0xB231231231231231uL, 0x2312312312312312uL)),
                    ((+1, -4, 0xAAAAAAAAAAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, 2, 0xAA36363636363636uL, 0x3636363636363636uL)),
                    ((+1, 1, 0xC373FCDCFF373FCDuL, 0xCFF373FCDCFF373FuL),   (+1, 3, 0x8A9B6331EAB8DA7EuL, 0xFDB2ACC2FD4737B6uL)),
                    ((+1, 8, 0x8CBAE6076B981DAEuL, 0x6076B981DAE6076BuL),   (+1, 3, 0xCD12F438911D0D30uL, 0x5CCEF88EB6F5D78FuL)),
                    ((+1, 15, 0xD62B955555555555uL, 0x5555555555555555uL),  (+1, 4, 0x8E4143C38A48A430uL, 0xA097076AD0A5339CuL)),
                    ((+1, 24, 0x98FD6BE5F9DFFE17uL, 0xE77FF85F9DFFE17EuL),  (+1, 4, 0xBC751842CF4E9497uL, 0x70E668923E28A49CuL)),
                    ((+1, 33, 0xBC4976FEFAAAAAAAuL, 0xAAAAAAAAAAAAAAAAuL),  (+1, 4, 0xF124F8842E1C48A0uL, 0xD06011ECEFE36DB1uL)),
                    ((+1, 43, 0xBB012610AE915555uL, 0x5555555555555555uL),  (+1, 5, 0x9628724E1B92443AuL, 0x6840275F06887788uL)),
                    ((+1, 54, 0x8E651CDBBD813979uL, 0xE958D7E43E7A5635uL),  (+1, 5, 0xB6FC6E4651367267uL, 0x0F2944F9F6122852uL)),
                    ((+1, 65, 0x9F8658358BF14630uL, 0x9C07432D63DBB01DuL),  (+1, 5, 0xDB0E702AC9B318AAuL, 0xC812A8F246FAF8F4uL)),
                    ((+1, 76, 0xFE23A5EEA9E101EAuL, 0x44D3364D9364D936uL),  (+1, 6, 0x812F3BFDC335A2BAuL, 0xBF33D1C77579DA39uL)),
                    ((+1, 89, 0x8BE36E94549BC383uL, 0x3183A1C126A99EFFuL),  (+1, 6, 0x967642DC43BD1134uL, 0x32FDF1146F4122ACuL)),
                    ((+1, 101, 0xCFBD9FD1BF989F60uL, 0xE2608D3CA7BC05C9uL), (+1, 6, 0xAD5C4CB0E670DCD6uL, 0x7D262473FFCAA985uL)),
                    ((+1, 114, 0xCBC91757DAE8B62EuL, 0x38D01E72BE6542AAuL), (+1, 6, 0xC5E1597BAB5118EFuL, 0x82DA12535E50F793uL)),
                });

                public static ReadOnlyCollection<ddouble> TaylorZeropointCoefTable = new(new ddouble[] {
                    (+1, -1, 0xF7B95E4771C55D8FuL, 0x0C28D0814E530F17uL),
                    (-1, -2, 0xE2B1DAA550D1AB8EuL, 0xC060D20DBFB8B839uL),
                    (+1, -2, 0x845A14A6A81C05D6uL, 0x6D7B6900032EA171uL),
                    (-1, -3, 0xA7E098B92BED4186uL, 0xB1CC35DBC77717C5uL),
                    (+1, -4, 0xDCD2DB1B879D54BEuL, 0xA5BB6701257D9AE9uL),
                    (-1, -4, 0x93DD5D130E615E39uL, 0x112646269ACB6FADuL),
                    (+1, -5, 0xC7E701591CE534BDuL, 0xCE5785C71958A783uL),
                    (-1, -5, 0x87D3F61B53EC74F7uL, 0xB974CEEC1C8E4479uL),
                    (+1, -6, 0xB91EB403F6E601F3uL, 0x03AB27FC78546609uL),
                    (-1, -7, 0xFCB828470DB50E3BuL, 0xDE9CB3BBCBD07F19uL),
                    (+1, -7, 0xACAAE5554B1799D3uL, 0x608DED8B2BC7C2DBuL),
                    (-1, -8, 0xEC1403C94175BC26uL, 0xD38DD7FDE3176ECEuL),
                    (+1, -8, 0xA170D67C0EC6E1A2uL, 0xC77E2ED9FBB6EB4DuL),
                    (-1, -9, 0xDCD7E3E774509DBBuL, 0x8A3BE79375D38117uL),
                    (+1, -9, 0x97119A2FBCD575A0uL, 0xDB3291E0A10B65E2uL),
                    (-1, -10, 0xCEB137B8E8FBD21EuL, 0xCF44BBCA27C8C934uL),
                    (+1, -10, 0x8D675DEBB08444B6uL, 0xFBD3A05C1AB57FD1uL),
                    (-1, -11, 0xC17B1A2E32D9A67CuL, 0xEA3F72854D6949FFuL),
                    (+1, -11, 0x845ED70D130EA396uL, 0x040CF3D77D20E1ABuL),
                    (-1, -12, 0xB51FEEF50898212DuL, 0x6D777952AF76243EuL),
                    (+1, -13, 0xF7D655B7DC4C2743uL, 0x5893CEC3C2777BE2uL),
                    (-1, -13, 0xA98FAEE2F5AB1930uL, 0x68DF0FDDDE360A15uL),
                    (+1, -14, 0xE80407C087D5AB71uL, 0x1808B6CE80E54B44uL),
                    (-1, -14, 0x9EBCB934457B3718uL, 0xF9DD497E02D9ED9AuL),
                    (+1, -15, 0xD93488C162E19F3DuL, 0x81B14BA0058FE529uL),
                    (-1, -15, 0x949ABFBEB65C3335uL, 0x30F69A4CF3A566ADuL),
                    (+1, -16, 0xCB572547195A4657uL, 0x8628291C2E71B177uL),
                    (-1, -16, 0x8B1E639B9DF4552DuL, 0xF04BBB7240BEB178uL),
                    (+1, -17, 0xBE5C59C8B7E9489CuL, 0x6E3524C368BB5487uL),
                    (-1, -17, 0x823D0C4A707EF06EuL, 0x7BC750E6D8B75220uL),
                    (+1, -18, 0xB235AA7A085EF65DuL, 0xDFFE6198BB04CDDAuL),
                    (-1, -19, 0xF3D9A52DBC51E21FuL, 0x315F0A58002D8212uL),
                });
            }
        }
    }
}
