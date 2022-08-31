using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Polygamma(int n, ddouble x) {
            if (n < 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n > 16) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the polygamma function, n with an absolute value greater than 16 is not supported."
                );
            }
            if (n == 0) {
                return Digamma(x);
            }
            if (IsNaN(x) || IsNegativeInfinity(x)) {
                return NaN;
            }
            if (IsPositiveInfinity(x)) {
                return Zero;
            }

            if (x >= 0) {
                ddouble y = PolygammaPlusX.Polygamma(n, x);

                return ((n & 1) == 1) ? y : -y;
            }
            else {
                return PolygammaMinusX.Polygamma(n, x);
            }
        }

        private static class PolygammaPlusX {

            public static ddouble Polygamma(int n, ddouble x) {
                if (x <= Math.ScaleB(1, -96)) {
                    return ((n & 1) == 1) ? PositiveInfinity : NaN;
                }
                if (x <= 64) {
                    return PolygammaPlusX.PolygammaNearZero(n, x);
                }
                else {
                    return PolygammaPlusX.PolygammaLimit(n, x);
                }
            }

            public static ddouble PolygammaNearZero(int n, ddouble x) {
                if (x < 1) {
                    ddouble v = PolygammaNearZero(n, x + 1);
                    ddouble y = v + Factorial[n] / Pow(x, n + 1);

                    return y;
                }

                ddouble scale = Math.Max(1, 8d / n), r = scale * n / x;
                ddouble ir = 0, it = 0;

                if (n > 1) {
                    ddouble polygamma_ir(ddouble t) {
                        ddouble y = Pow(t, n) * Exp(-x * t) / (1 - Exp(-t));

                        return y;
                    };

                    ddouble polygamma_it(ddouble u) {
                        ddouble v = (u + scale * n) / x;
                        ddouble y = Pow(v, n) / (1 - Exp(-v));

                        return y;
                    };

                    foreach ((ddouble t, ddouble w) in gles) {
                        ir += w * polygamma_ir(t * r);
                    }
                    foreach ((ddouble t, ddouble w) in glas) {
                        it += w * polygamma_it(t);
                    }
                }
                else {
                    ddouble polygamma_ir(ddouble t) {
                        ddouble y = t * Exp(-x * t) / (1 - Exp(-t));

                        return y;
                    };

                    ddouble polygamma_it(ddouble u) {
                        ddouble v = (u + scale) / x;
                        ddouble y = v / (1 - Exp(-v));

                        return y;
                    };

                    foreach ((ddouble t, ddouble w) in gles) {
                        ir += w * polygamma_ir(t * r);
                    }
                    foreach ((ddouble t, ddouble w) in glas) {
                        it += w * polygamma_it(t);
                    }
                }

                ir *= r;
                it *= ddouble.Exp(-scale * n) / x;

                ddouble i = ir + it;

                return i;
            }

            public static ddouble PolygammaLimit(int n, ddouble x) {
                ddouble inv_x2 = 1 / (x * x), c = Pow(x, -n);
                ddouble v = c * Factorial[n - 1] * (2 * x + n) / (2 * x);
                ddouble u = c * Factorial[n + 1] / 2 * inv_x2;
                ddouble dv = ddouble.BernoulliSequence[1] * u;

                v += dv;

                for (int k = 2; k <= 20; k++) {
                    u *= inv_x2 * checked((n + 2 * k - 2) * (n + 2 * k - 1)) / checked((2 * k) * (2 * k - 1));
                    dv = ddouble.BernoulliSequence[k] * u;
                    ddouble next_v = v + dv;

                    if (v == next_v) {
                        break;
                    }
                    if (IsNaN(next_v)) {
                        return ddouble.Zero;
                    }

                    v = next_v;
                }

                return v;
            }

            static ReadOnlyCollection<(ddouble x, ddouble w)> gles = new(new (ddouble, ddouble)[] {
                ((+1, -11, 0xE6FCA651838752B7uL, 0x25FD5D7505B0DAA3uL), (+1, -9, 0x942738A31E06B328uL, 0x047C1EC6C342098BuL)),
                ((+1, -8, 0x97F0F95FCAA1688DuL, 0x80F2ED27376E9D16uL), (+1, -8, 0xAC00FF2E6D0503CAuL, 0x32663A4C477F6CDDuL)),
                ((+1, -7, 0xBA49576421895829uL, 0x01BE526784DABEC0uL), (+1, -7, 0x86857496E97B0E21uL, 0xD52A5C9A950FD0D8uL)),
                ((+1, -6, 0xAC5F69D303C51DA5uL, 0xD2F34ADCF8059B31uL), (+1, -7, 0xB63CED58579D1D68uL, 0xFEDFFD1B179720D8uL)),
                ((+1, -5, 0x8999703939AF3813uL, 0x370D1358EEA09B92uL), (+1, -7, 0xE4DC266A7970AB82uL, 0x0A1EE784DFED99BDuL)),
                ((+1, -5, 0xC8806BB276EC5BC8uL, 0x8E91223F55297EA4uL), (+1, -6, 0x890D8EC74D12837DuL, 0xD3C5AA0A0470E8BAuL)),
                ((+1, -4, 0x8941E9782BF810F0uL, 0x3EB9FF7451356092uL), (+1, -6, 0x9EDA0CF1E4B4298AuL, 0x9ABA91D622C43A7FuL)),
                ((+1, -4, 0xB398D9F2C1DE0A54uL, 0x0A9D7212CD8BC297uL), (+1, -6, 0xB3B1FB3E73DB63F6uL, 0xCEBDD81C8AD91068uL)),
                ((+1, -4, 0xE303D7E3531208C5uL, 0x39EECA405C929D6EuL), (+1, -6, 0xC7754153EB48A88BuL, 0x4BE803506AA7CFCFuL)),
                ((+1, -3, 0x8B9CF11059D89F55uL, 0xDDA5F1FDC6039FD6uL), (+1, -6, 0xDA057160851FA95BuL, 0x87AF9CA5C2F9D348uL)),
                ((+1, -3, 0xA7F54B1E81CC62A9uL, 0x735EB444B2DD6CA3uL), (+1, -6, 0xEB45F69C58CB1F11uL, 0x0C93FC53D43C2ED3uL)),
                ((+1, -3, 0xC65F5629AD4708FAuL, 0xBB24DA95C68E3A4EuL), (+1, -6, 0xFB1C4128C3B009C6uL, 0x989BC8CB3EAFF51BuL)),
                ((+1, -3, 0xE6AC3EB6FA835D5DuL, 0x42D6DCEAA7B3FEBBuL), (+1, -5, 0x84B7F77390AE8592uL, 0x640D19CF6F3458A2uL)),
                ((+1, -2, 0x845524EE44D55ECDuL, 0xE5F0EA4DF6D347A0uL), (+1, -5, 0x8B15787E3F3FBB12uL, 0xDFC70041AE2B0DA4uL)),
                ((+1, -2, 0x961290E97584B67DuL, 0x7EA4CA34423E96BCuL), (+1, -5, 0x909CD6E2191BFF9DuL, 0xA589BE58912931CAuL)),
                ((+1, -2, 0xA87313447A65A938uL, 0x82FAD4EFBB9D09F8uL), (+1, -5, 0x95458F7DA8520E96uL, 0xF097F2F6FB778C7EuL)),
                ((+1, -2, 0xBB5A60DF0AFBC047uL, 0x3212B5BA53295463uL), (+1, -5, 0x990875FADC1039C3uL, 0x513AFE81D137C34FuL)),
                ((+1, -2, 0xCEAB5F10DE89D08BuL, 0x9037A7FB025774E9uL), (+1, -5, 0x9BDFBFD9FFD2F86DuL, 0xE4D519CCDA60BC85uL)),
                ((+1, -2, 0xE2485078DCFC6523uL, 0x34BE646F99893291uL), (+1, -5, 0x9DC70D5C0AB6F419uL, 0x8BB0C62A6C64C772uL)),
                ((+1, -2, 0xF61302C6D31C838FuL, 0x5E93A51F15146AFEuL), (+1, -5, 0x9EBB703E80A3838DuL, 0x8B27C018AC6B65C4uL)),
                ((+1, -1, 0x84F67E9C9671BE38uL, 0x50B62D707575CA80uL), (+1, -5, 0x9EBB703E80A3838DuL, 0x8B27C018AC6B65C4uL)),
                ((+1, -1, 0x8EDBD7C39181CD6EuL, 0x65A0CDC8333B66B7uL), (+1, -5, 0x9DC70D5C0AB6F419uL, 0x8BB0C62A6C64C772uL)),
                ((+1, -1, 0x98AA507790BB17BAuL, 0x37E42C027ED4458BuL), (+1, -5, 0x9BDFBFD9FFD2F86DuL, 0xE4D519CCDA60BC85uL)),
                ((+1, -1, 0xA252CF907A821FDCuL, 0x66F6A522D66B55CEuL), (+1, -5, 0x990875FADC1039C3uL, 0x513AFE81D137C34FuL)),
                ((+1, -1, 0xABC6765DC2CD2B63uL, 0xBE82958822317B03uL), (+1, -5, 0x95458F7DA8520E96uL, 0xF097F2F6FB778C7EuL)),
                ((+1, -1, 0xB4F6B78B453DA4C1uL, 0x40AD9AE5DEE0B4A1uL), (+1, -5, 0x909CD6E2191BFF9DuL, 0xA589BE58912931CAuL)),
                ((+1, -1, 0xBDD56D88DD955099uL, 0x0D078AD904965C2FuL), (+1, -5, 0x8B15787E3F3FBB12uL, 0xDFC70041AE2B0DA4uL)),
                ((+1, -1, 0xC654F052415F28A8uL, 0xAF4A48C556130051uL), (+1, -5, 0x84B7F77390AE8592uL, 0x640D19CF6F3458A2uL)),
                ((+1, -1, 0xCE682A7594AE3DC1uL, 0x5136C95A8E5C716CuL), (+1, -6, 0xFB1C4128C3B009C6uL, 0x989BC8CB3EAFF51BuL)),
                ((+1, -1, 0xD602AD385F8CE755uL, 0xA32852EED348A4D7uL), (+1, -6, 0xEB45F69C58CB1F11uL, 0x0C93FC53D43C2ED3uL)),
                ((+1, -1, 0xDD18C3BBE989D82AuL, 0x889683808E7F180AuL), (+1, -6, 0xDA057160851FA95BuL, 0x87AF9CA5C2F9D348uL)),
                ((+1, -1, 0xE39F8503959DBEE7uL, 0x58C226B7F46DAC52uL), (+1, -6, 0xC7754153EB48A88BuL, 0x4BE803506AA7CFCFuL)),
                ((+1, -1, 0xE98CE4C1A7C43EB5uL, 0x7EAC51BDA64E87ADuL), (+1, -6, 0xB3B1FB3E73DB63F6uL, 0xCEBDD81C8AD91068uL)),
                ((+1, -1, 0xEED7C2D0FA80FDE1uL, 0xF828C01175D953EDuL), (+1, -6, 0x9EDA0CF1E4B4298AuL, 0x9ABA91D622C43A7FuL)),
                ((+1, -1, 0xF377F944D8913A43uL, 0x7716EDDC0AAD6815uL), (+1, -6, 0x890D8EC74D12837DuL, 0xD3C5AA0A0470E8BAuL)),
                ((+1, -1, 0xF76668FC6C650C7EuL, 0xCC8F2ECA7115F646uL), (+1, -7, 0xE4DC266A7970AB82uL, 0x0A1EE784DFED99BDuL)),
                ((+1, -1, 0xFA9D04B167E1D712uL, 0xD16865A9183FD326uL), (+1, -7, 0xB63CED58579D1D68uL, 0xFEDFFD1B179720D8uL)),
                ((+1, -1, 0xFD16DAA26F79DA9FuL, 0x5BF906B661EC9504uL), (+1, -7, 0x86857496E97B0E21uL, 0xD52A5C9A950FD0D8uL)),
                ((+1, -1, 0xFED01E0D406ABD2EuL, 0xE4FE1A25B19122C5uL), (+1, -8, 0xAC00FF2E6D0503CAuL, 0x32663A4C477F6CDDuL)),
                ((+1, -1, 0xFFC640D66B9F1E2BuL, 0x523680A8A2BE93C9uL), (+1, -9, 0x942738A31E06B328uL, 0x047C1EC6C342098BuL)),
            });

            static ReadOnlyCollection<(ddouble x, ddouble w)> glas = new(new (ddouble, ddouble)[] {
                ((+1, -5, 0x923A93A02CF981DFuL, 0x16C83DD3D330FD1DuL), (+1, -4, 0xB511680543957848uL, 0x8A4777966C777F27uL)),
                ((+1, -3, 0xC0AD9D12077A1912uL, 0x46421A35C7FDE78CuL), (+1, -3, 0xB50EEC4777B499AFuL, 0x8D7326A7A62D1A36uL)),
                ((+1, -2, 0xECE643CC95AC3EECuL, 0xA671D8AB0DD700DAuL), (+1, -3, 0xD86F92ACA81EBC99uL, 0x4B99AC30E5285095uL)),
                ((+1, -1, 0xDC1A14BA3FAD8991uL, 0x8B4DEF671BBC8B24uL), (+1, -3, 0xC6BD388AA5DEE121uL, 0x628278476A8B1150uL)),
                ((+1, 0, 0xB0A431CF2540C17BuL, 0xA75067CAA4BD19EAuL), (+1, -3, 0x95F2DE581970F590uL, 0x856E1D8F2DD265ABuL)),
                ((+1, 1, 0x818CA479C363B0A9uL, 0x82D73DC576D5200FuL), (+1, -4, 0xBF221ED8F9E5902EuL, 0x96F44DED1DB46862uL)),
                ((+1, 1, 0xB2C69040BD77D7A6uL, 0x8BCE345305F210C0uL), (+1, -5, 0xD09E4980B3B4C6F5uL, 0xC1AC87ACA937FDC5uL)),
                ((+1, 1, 0xEC13B465922731C6uL, 0x212BC7A4589904BCuL), (+1, -6, 0xC469B89C5182AF57uL, 0xF7BB64E21194A5FEuL)),
                ((+1, 2, 0x96C5C3A75E212F1CuL, 0xC79F4940D0361848uL), (+1, -7, 0xA025BFF0FA95EE75uL, 0xAC0069CF6E5812E3uL)),
                ((+1, 2, 0xBBA4AA98E36DB924uL, 0xFED12C0DAF22E73FuL), (+1, -9, 0xE29E9B2E79B97CCCuL, 0x6940AFF756B089F6uL)),
                ((+1, 2, 0xE4B6413EB6850CCFuL, 0x270DB74F21138422uL), (+1, -10, 0x8B3B1756D12D6A7BuL, 0x697E5E3861EC1AB4uL)),
                ((+1, 3, 0x890636B54EB8719CuL, 0x703D983D1D0F5700uL), (+1, -12, 0x948414FEAB9EDF55uL, 0x2116F917C3A1A991uL)),
                ((+1, 3, 0xA1DDBBA9FB37F2B0uL, 0x437DE67826DEC413uL), (+1, -14, 0x89621810F3C40CD9uL, 0xFFC58B33729486A7uL)),
                ((+1, 3, 0xBCED2078570AAF7FuL, 0x74750C6CFCD65C75uL), (+1, -17, 0xDC0D1674EEF3A0A7uL, 0x6931F593774A8555uL)),
                ((+1, 3, 0xDA4143B56402AEA4uL, 0x620F840EDAAB94BFuL), (+1, -19, 0x983BB944E5027F2EuL, 0x71F044C889AC74F0uL)),
                ((+1, 3, 0xF9E898532C3F59AAuL, 0x01974775435C7DA8uL), (+1, -22, 0xB5713B912D9729E9uL, 0x23863CF096FD0659uL)),
                ((+1, 4, 0x8DF9A9D3A6CA192EuL, 0x0BF632FA4C4D7770uL), (+1, -25, 0xB9A9EC4F651FC1BDuL, 0x48F7DF316D8FC6A3uL)),
                ((+1, 4, 0xA039D222E96158ECuL, 0x2937B88EADD0DF65uL), (+1, -28, 0xA27845647CC60484uL, 0x1B6C8873C5C8F653uL)),
                ((+1, 4, 0xB3BEF9D8EBD372E4uL, 0xE4339EBD1B49AB02uL), (+1, -32, 0xF2112A9AA3705E82uL, 0xC8392F6A77667210uL)),
                ((+1, 4, 0xC8949ABE04582762uL, 0x7BFEA35EAD3CA9B4uL), (+1, -35, 0x98B837273E8C0A34uL, 0xD224B1EFB9CC3028uL)),
                ((+1, 4, 0xDEC7A3995B270157uL, 0x61AF564C22F6962FuL), (+1, -39, 0xA23A0FD67C4097F5uL, 0xD410EC5F0C0BC903uL)),
                ((+1, 4, 0xF666B2CF379DB1E6uL, 0xA2BD95BBB72744EFuL), (+1, -43, 0x9016EF0862A0BEAFuL, 0xDE4B4BB523254ED9uL)),
                ((+1, 5, 0x87C12F52A773D5E9uL, 0xDBFE97ACA2E9AD9DuL), (+1, -48, 0xD45C305C84EC17C1uL, 0x4C1FF797A3CFE271uL)),
                ((+1, 5, 0x9516C79FC475ED6BuL, 0xDA752E98E23F44B9uL), (+1, -52, 0x80ADB795E01E28D2uL, 0xFF960F3EBC3E6E8FuL)),
                ((+1, 5, 0xA33EF7F98E21CDA2uL, 0xFF5D2824C3C09BB6uL), (+1, -58, 0xFDD6F0D8D365CEF0uL, 0x1FF2AA889BA58545uL)),
                ((+1, 5, 0xB2463FE929DA63C3uL, 0x34BACB07DB562BA3uL), (+1, -63, 0xC95CB5BB23955E5BuL, 0x95F7A26F3C2A28A5uL)),
                ((+1, 5, 0xC23B265D15938A24uL, 0x09F6CBB8B61EF6F7uL), (+1, -69, 0xFD66392A93FC30B0uL, 0x14D969D73B1D9EE6uL)),
                ((+1, 5, 0xD32EB4B2F5B6BE50uL, 0x14EBA9CB18D7B5FEuL), (+1, -75, 0xF8DADD673BA3BF14uL, 0xA11205024D5DABBBuL)),
                ((+1, 5, 0xE5351BA8B7599A14uL, 0x23A27F3132A99B98uL), (+1, -81, 0xBB138AEBF358AD90uL, 0x1DC7EF563C3063C3uL)),
                ((+1, 5, 0xF8669557C540369DuL, 0x429DDCD3A0583141uL), (+1, -88, 0xD25FBA54009A8AA1uL, 0xFB759692266A1077uL)),
                ((+1, 6, 0x86705162DB684600uL, 0xC61E01DEB54B09D2uL), (+1, -95, 0xAC0588298A3A1A0EuL, 0x8E191ED97CE30EECuL)),
                ((+1, 6, 0x9163EBDC480A8879uL, 0xC611CD5D54232661uL), (+1, -103, 0xC58855E2DD000B62uL, 0x72CFA544B2435A88uL)),
                ((+1, 6, 0x9D25466C58813474uL, 0xB68E4F8ED2622F23uL), (+1, -111, 0x985F58DAE64B67FBuL, 0x0A6F26842E02D48AuL)),
                ((+1, 6, 0xA9D28CE32DE65248uL, 0x4D0D2768E42E6965uL), (+1, -120, 0x9514956167770526uL, 0x4116047BF121179EuL)),
                ((+1, 6, 0xB7946A73F1CC488EuL, 0xA59B19AD1DA0D9E4uL), (+1, -130, 0xAB37BD229B668AEDuL, 0x42F4C49A70E9ACB4uL)),
                ((+1, 6, 0xC6A4410A4B69578BuL, 0xF8B74769EC0FED35uL), (+1, -141, 0xCEFDF5143E488C27uL, 0x0A9AC59A92D565F3uL)),
                ((+1, 6, 0xD7584A23B5591146uL, 0x661DFC2024261A6BuL), (+1, -153, 0xDFBE18143651463EuL, 0x198BCC116025FDCBuL)),
                ((+1, 6, 0xEA3E9F5A39FE0EA2uL, 0x1A65CA11C5CF2C0AuL), (+1, -166, 0xA575C1CBFDB553A2uL, 0x2D218E9CA3051935uL)),
                ((+1, 7, 0x8033ABEAA28483EBuL, 0xD9B4D9E068F58072uL), (+1, -182, 0xC96343CB37A06EAEuL, 0xA0188D716CC23411uL)),
                ((+1, 7, 0x8E47B0FE8C77C66DuL, 0x953C5900F530B456uL), (+1, -202, 0xDE2C3D62D4EECCA5uL, 0x461622257196E2E9uL)),
            });
        }

        private static class PolygammaMinusX {
            public static ddouble Polygamma(int n, ddouble x) {
                if (Abs(x - Round(x)) <= Math.ScaleB(1, -96)) {
                    return ((n & 1) == 1) ? PositiveInfinity : NaN;
                }

                ddouble p = PolygammaPlusX.Polygamma(n, 1 - x);
                ddouble g = Pow(PI, n + 1) * reflecs[n](x);

                ddouble y = -(g + p);

                return y;
            }

            static ReadOnlyCollection<Func<ddouble, ddouble>> reflecs = new(new Func<ddouble, ddouble>[] {
               (x) => 1 / TanPI(x),
               (x) => -1 / Square(SinPI(x)),
               (x) => 2 / (TanPI(x) * Square(SinPI(x))),
               (x) => -2 * (2 + CosPI(2 * x)) / Pow(SinPI(x), 4),
               (x) => 4 * (5 + CosPI(2 * x)) / (TanPI(x) * Pow(SinPI(x), 4)),
               (x) => -2 * (33 + 26 * CosPI(2 * x) + CosPI(4 * x)) / Pow(SinPI(x), 6),
               (x) => 4 * (123 + 56 * CosPI(2 * x) + CosPI(4 * x)) / (TanPI(x) * Pow(SinPI(x), 6)),
               (x) => -2 * (1208 + 1191 * CosPI(2 * x) + 120 * CosPI(4 * x) + CosPI(6 * x)) / Pow(SinPI(x), 8),
               (x) => 4 * (5786 + 4047 * CosPI(2 * x) + 246 * CosPI(4 * x) + CosPI(6 * x)) / (TanPI(x) * Pow(SinPI(x), 8)),
               (x) => -2 * (78095 + 88234 * CosPI(2 * x) + 14608 * CosPI(4 * x) + 502 * CosPI(6 * x) + CosPI(8 * x)) / Pow(SinPI(x), 10),
               (x) => 4 * (450995 + 408364 * CosPI(2 * x) + 46828 * CosPI(4 * x) + 1012 * CosPI(6 * x) + CosPI(8 * x)) / (TanPI(x) * Pow(SinPI(x), 10)),
               (x) => -2 * (7862124 + 9738114 * CosPI(2 * x) + 2203488 * CosPI(4 * x) + 152637 * CosPI(6 * x) + 2036 * CosPI(8 * x) + CosPI(10 * x)) / Pow(SinPI(x), 12),
               (x) => 4 * (52953654 + 56604978 * CosPI(2 * x) + 9713496 * CosPI(4 * x) + 474189 * CosPI(6 * x) + 4082 * CosPI(8 * x) + CosPI(10 * x)) / (TanPI(x) * Pow(SinPI(x), 12)),
               (x) => -2 * (1137586002 + 1505621508 * CosPI(2 * x) + 423281535 * CosPI(4 * x) + 45533450 * CosPI(6 * x) + 1479726 * CosPI(8 * x) + 8178 * CosPI(10 * x) + CosPI(12 * x)) / Pow(SinPI(x), 14),
               (x) => 4 * (8752882782 + 10465410528 * CosPI(2 * x) + 2377852335 * CosPI(4 * x) + 193889840 * CosPI(6 * x) + 4520946 * CosPI(8 * x) + 16368 * CosPI(10 * x) + CosPI(12 * x)) / (TanPI(x) * Pow(SinPI(x), 14)),
               (x) => -2 * (223769408736 + 311387598411 * CosPI(2 * x) + 102776998928 * CosPI(4 * x) + 15041229521 * CosPI(6 * x) + 848090912 * CosPI(8 * x) + 13824739 * CosPI(10 * x) + 32752 * CosPI(12 * x) + CosPI(14 * x)) / Pow(SinPI(x), 16),
               (x) => 4 * (1937789122548 + 2507220680379 * CosPI(2 * x) + 700262497778 * CosPI(4 * x) + 81853020521 * CosPI(6 * x) + 3530218028 * CosPI(8 * x) + 41867227 * CosPI(10 * x) + 65518 * CosPI(12 * x) + CosPI(14 * x)) / (TanPI(x) * Pow(SinPI(x), 16)),
            });
        }
    }
}