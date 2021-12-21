using DoubleDouble;
using System.Collections.ObjectModel;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    internal static class Sterling {

        public static ddouble Gamma(ddouble x) {
            ddouble r = Sqrt(2 * PI / x);
            ddouble p = Pow(x / E, x);
            ddouble s = Exp(SterlingTerm(x));

            ddouble y = r * p * s;

            return y;
        }

        public static ddouble Digamma(ddouble x) {
            ddouble s = DiffLogSterlingTerm(x);
            ddouble p = ddouble.Log(x);
            ddouble c = Rcp(x) / 2;

            ddouble y = -s + p - c;

            return y;
        }

        private static ddouble SterlingTerm(ddouble x) {
            ddouble v = Rcp(x), v2 = v * v, v4 = v2 * v2, u = v;

            ddouble y = 0d;
            foreach ((ddouble s, ddouble r) in SterlingTable) {
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
            foreach ((ddouble s, ddouble r) in DiffLogSterlingTable) {
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

        public static ReadOnlyCollection<(ddouble s, ddouble r)> DiffLogSterlingTable = new(new (ddouble s, ddouble r)[]{
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
    }
}
