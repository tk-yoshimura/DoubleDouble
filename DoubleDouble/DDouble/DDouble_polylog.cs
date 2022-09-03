using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Polylog(int n, ddouble x) {
            if (n < -4) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the polylog function, n with an absolute value less than -4 is not supported."
                );
            }
            if (n > 8) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the polylog function, n with an absolute value greater than 8 is not supported."
                );
            }

            if (!(x <= 1)) {
                return NaN;
            }

            if (n >= 2) {
                if (x >= 0.5) {
                    return PolylogNearOne.Polylog(n, x);
                }
                if (x >= -0.5) {
                    return PolylogPowerSeries.PolylogNearZero(n, x);
                }
                if (x >= -1.5) {
                    return PolylogIntegral.Polylog(n, x);
                }
                if (ddouble.IsNegativeInfinity(x)) {
                    return NegativeInfinity;
                }

                return PolylogPowerSeries.PolylogMinusLimit(n, x);
            }

            return PolylogParticularN.Polylog(n, x);
        }

        private static class PolylogNearOne {
            public static ddouble Polylog(int n, ddouble x) {
#if DEBUG
                if (x < 0.5 || x > 1) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                if (x > RegardedOneThreshold) {
                    return RiemannZeta(n);
                }


                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble v = Log(x), v2 = v * v;
                ddouble y = Pow(v, n - 1) * TaylorSequence[n - 1] * (HarmonicNumber(n - 1) - Log(-v));
                ddouble u = 1;

                for (int k = 0; k <= n; k++) {
                    if (k == n - 1) {
                        u *= v;
                        continue;
                    }

                    ddouble dy = coef[k] * u;
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        return y;
                    }

                    u *= v;
                    y = y_next;
                }

                for (int k = n + 1; k < coef.Count - 1; k += 2) {
                    ddouble dy = coef[k] * u;
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        break;
                    }

                    u *= v2;
                    y = y_next;
                }

                return y;
            }

            private static readonly ddouble RegardedOneThreshold = 1 - (ddouble)Math.ScaleB(1, -100);

            public static class CoefTable {
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new();

                public static ReadOnlyCollection<ddouble> Coef(int n) {
                    if (table.ContainsKey(n)) {
                        return table[n];
                    }

                    List<ddouble> coef = new List<ddouble>();
                    coef.Add(RiemannZeta(n));
                    for (int k = 1; k < TaylorSequence.Count; k++) {
                        coef.Add(RiemannZeta(n - k) * TaylorSequence[k]);
                    }

                    table.Add(n, new ReadOnlyCollection<ddouble>(coef));

                    return table[n];
                }
            }
        }


        private static class PolylogPowerSeries {
            public static ddouble PolylogNearZero(int n, ddouble x) {
#if DEBUG
                if (x < -0.5 || x > 0.5) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                if (x == 0) {
                    return x;
                }

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = x;
                ddouble u = x * x, x2 = u;

                for (int i = 1; i < coef.Count - 1; i += 2) {
                    ddouble dy = u * (coef[i] + x * coef[i + 1]);
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        break;
                    }

                    y = y_next;
                    u *= x2;
                }

                return y;
            }

            public static ddouble PolylogMinusLimit(int n, ddouble x) {
#if DEBUG
                if (x > -1.5) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                x = -x;

                ReadOnlyCollection<ddouble> coef = CoefTable.Coef(n);

                ddouble y = mlimit_bias[n](Log(x));
                ddouble u = 1 / x, v = u, v2 = v * v;

                for (int i = 0; i < coef.Count - 1; i += 2) {
                    ddouble dy = u * (coef[i] - v * coef[i + 1]);
                    ddouble y_next = ((n & 1) == 0) ? (y - dy) : (y + dy);

                    if (y == y_next) {
                        break;
                    }

                    y = y_next;
                    u *= v2;
                }

                return -y;
            }

            static ddouble pi2 = Square(PI);
            static ddouble pi4 = Pow(PI, 4);
            static ddouble pi6 = Pow(PI, 6);
            static ddouble pi8 = Pow(PI, 8);

            static ReadOnlyCollection<Func<ddouble, ddouble>> mlimit_bias = new(new Func<ddouble, ddouble>[] {
               (logx) => throw new NotImplementedException(),
               (logx) => throw new NotImplementedException(),
               (logx) => (pi2 + 3 * Square(logx)) / 6,
               (logx) => logx * (pi2 + Square(logx)) / 6,
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (7 * pi4 + logx2 * (30 * pi2 + 15 * logx2)) / 360;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (7 * pi4 + logx2 * (10 * pi2 + 3 * logx2)) / 360;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (31 * pi6 + logx2 * (147 * pi4 + logx2 * (105 * pi2 + 21 * logx2))) / 15120;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return logx * (31 * pi6 + logx2 * (49 * pi4 + logx2 * (21 * pi2 + 3 * logx2))) / 15120;
               },
               (logx) => {
                   ddouble logx2 = Square(logx);
                   return (127 * pi8 + logx2 * (620 * pi6 + logx2 * (490 * pi4 + logx2 * (140 * pi2 + 15 * logx2)))) / 604800;
               },
            });

            public static class CoefTable {
                public const int Terms = 201;
                private static readonly Dictionary<int, ReadOnlyCollection<ddouble>> table = new();

                public static ReadOnlyCollection<ddouble> Coef(int n) {
                    if (table.ContainsKey(n)) {
                        return table[n];
                    }

                    List<ddouble> coef = new List<ddouble>();
                    for (int k = 1; k < Terms; k++) {
                        coef.Add(Pow(k, -n));
                    }

                    table.Add(n, new ReadOnlyCollection<ddouble>(coef));

                    return table[n];
                }
            }
        }

        private static class PolylogIntegral {
            public static ddouble Polylog(int n, ddouble x) {
#if DEBUG
                if (x < -1.5 || x > -0.5) {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
#endif

                ddouble h = IntegrandPeak(n, -(double)x);
                ddouble r = 1 / -x;

                Func<ddouble, ddouble> polylog_ir =
                (n > 2) ? (t) => {
                    ddouble y = Pow(t, n - 1) / (Exp(t) * r + 1);

                    return y;
                }
                : (t) => {
                    ddouble y = t / (Exp(t) * r + 1);

                    return y;
                };

                int iter = Iters[n];
                ddouble ir = 0, it = 0;

                for (int k = 0; k < iter; k++) {
                    foreach ((ddouble t, ddouble w) in gles) {
                        ir += w * polylog_ir((k + t) * h);
                    }
                }

                ddouble sh = iter * h;

                Func<ddouble, ddouble> polylog_it =
                (n > 2) ? (u) => {
                    ddouble v = u + sh;
                    ddouble y = Pow(v, n - 1) / (r + Exp(-v));

                    return y;
                }
                : (u) => {
                    ddouble v = u + sh;
                    ddouble y = v / (r + Exp(-v));

                    return y;
                };

                foreach ((ddouble t, ddouble w) in glas) {
                    it += w * polylog_it(t);
                }

                ir *= h;
                it *= Exp(-sh);

                ddouble i = ir + it;

                i *= -TaylorSequence[n - 1];

                return i;
            }

            static readonly ReadOnlyCollection<int> Iters = new ReadOnlyCollection<int>(
                new int[] { -1, -1, 7, 4, 3, 2, 2, 2, 2 });

            static double IntegrandPeak(int n, double x) {
                if (n <= 0) {
                    throw new ArgumentOutOfRangeException(nameof(n));
                }
                if (n == 1) {
                    return 0;
                }

                double t = n - 1;

                for (int i = 0; i < 8; i++) {
                    double xexp = Math.Exp(-t) * x;
                    double d = (n - 1) * (xexp + 1) - t;
                    double dv = (n - 1) * xexp + 1;
                    double dt = d / dv;

                    t += dt;

                    if (Math.Abs(dt / t) < 1e-15) {
                        break;
                    }
                }

                return t;
            }

            static ReadOnlyCollection<(ddouble x, ddouble w)> gles = new(new (ddouble, ddouble)[] {
                ((+1, -10, 0x86B42F0977EE0FD1uL, 0xDDB545BDF52E797FuL), (+1, -9, 0xACC99C485E519BBBuL, 0xA69012B19FE1DE05uL)),
                ((+1, -8, 0xB12D5B95436BC9E5uL, 0x3A380796C60785C1uL), (+1, -8, 0xC8852A11853A3681uL, 0xB692B2D555287528uL)),
                ((+1, -7, 0xD9252520C0177A7EuL, 0x6C61CA8515628598uL), (+1, -7, 0x9CB4838F7FC06107uL, 0xBF05E53387D01461uL)),
                ((+1, -6, 0xC8D139649EFE59A2uL, 0x900D2B25E527DAB5uL), (+1, -7, 0xD40EF44985182D69uL, 0x7E2AF43B45D421F3uL)),
                ((+1, -5, 0xA0310DC2075E0DD0uL, 0xC0EF75D6B23CD753uL), (+1, -6, 0x84F67F18506CFC9AuL, 0x10668A824F2671F2uL)),
                ((+1, -5, 0xE93750DAFBCD032FuL, 0x203D9642807113D7uL), (+1, -6, 0x9EF6D15843DD7BDCuL, 0x1DD8AD74516033BCuL)),
                ((+1, -4, 0x9F7C251749BD0436uL, 0x16D448670692AB22uL), (+1, -6, 0xB7D9B754B50CDA4CuL, 0xDB0C193C224D1962uL)),
                ((+1, -4, 0xD06CFFD4D8BE37EAuL, 0xF8ACB04B4771FC57uL), (+1, -6, 0xCF727E01748B4D1DuL, 0x61685F3916A2FA06uL)),
                ((+1, -3, 0x838B2BCB6531D2A9uL, 0x808B978261858ECBuL), (+1, -6, 0xE596C4F541066F0FuL, 0x9322E2AB241D0A92uL)),
                ((+1, -3, 0xA18B02AF7210A9BAuL, 0xEDA62E9727F5D8C9uL), (+1, -6, 0xFA1EC9569CE80C0EuL, 0x17578A0D0C8A195BuL)),
                ((+1, -3, 0xC200264E3152C9D4uL, 0x66690DAF38F2A8CFuL), (+1, -5, 0x8672D66E19DFBE10uL, 0x34228DB2AE065E7AuL)),
                ((+1, -3, 0xE4B04E56ED9D29F8uL, 0x7FFCDF60793803BDuL), (+1, -5, 0x8EE4DBECBF710751uL, 0x033182C96118356AuL)),
                ((+1, -2, 0x84AE988DB5310EEFuL, 0xD83430758F9B29D0uL), (+1, -5, 0x96564ADCB2342B4EuL, 0xB998C1DD1C22CE18uL)),
                ((+1, -2, 0x97E279B536143982uL, 0x9982E24B623E7F2DuL), (+1, -5, 0x9CB9C5B66B2C5A55uL, 0x8AB6F93A8D3E73A9uL)),
                ((+1, -2, 0xABD14F68F2E398D1uL, 0xF52A89CC2750B5DEuL), (+1, -5, 0xA203D3B353E8AF73uL, 0x19F36ABAB5B21695uL)),
                ((+1, -2, 0xC0574EBA64801750uL, 0xF4BB06BFC9815ACFuL), (+1, -5, 0xA62AF5662C6B7B77uL, 0x61842D10C37FAA3EuL)),
                ((+1, -2, 0xD54F9D4B23EF4F3AuL, 0xC79FECCF901A28BAuL), (+1, -5, 0xA927B5C87A4BBD0BuL, 0x3A73728827B839CBuL)),
                ((+1, -2, 0xEA949379E5751420uL, 0x5E38242BD1BE0DABuL), (+1, -5, 0xAAF4B79E2A3D5746uL, 0xF17598B17E94D339uL)),
                ((+1, -1, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -5, 0xAB8EBF173D8CC82DuL, 0x23B9E9E755CC67C2uL)),
                ((+1, -1, 0x8AB5B6430D4575EFuL, 0xD0E3EDEA1720F92AuL), (+1, -5, 0xAAF4B79E2A3D5746uL, 0xF17598B17E94D339uL)),
                ((+1, -1, 0x9558315A6E085862uL, 0x9C30099837F2EBA2uL), (+1, -5, 0xA927B5C87A4BBD0BuL, 0x3A73728827B839CBuL)),
                ((+1, -1, 0x9FD458A2CDBFF457uL, 0x85A27CA01B3F5298uL), (+1, -5, 0xA62AF5662C6B7B77uL, 0x61842D10C37FAA3EuL)),
                ((+1, -1, 0xAA17584B868E3397uL, 0x056ABB19EC57A510uL), (+1, -5, 0xA203D3B353E8AF73uL, 0x19F36ABAB5B21695uL)),
                ((+1, -1, 0xB40EC32564F5E33EuL, 0xB33E8EDA4EE0C069uL), (+1, -5, 0x9CB9C5B66B2C5A55uL, 0x8AB6F93A8D3E73A9uL)),
                ((+1, -1, 0xBDA8B3B925677888uL, 0x13E5E7C538326B17uL), (+1, -5, 0x96564ADCB2342B4EuL, 0xB998C1DD1C22CE18uL)),
                ((+1, -1, 0xC6D3EC6A4498B581uL, 0xE000C827E1B1FF10uL), (+1, -5, 0x8EE4DBECBF710751uL, 0x033182C96118356AuL)),
                ((+1, -1, 0xCF7FF66C73AB4D8AuL, 0xE665BC9431C355CCuL), (+1, -5, 0x8672D66E19DFBE10uL, 0x34228DB2AE065E7AuL)),
                ((+1, -1, 0xD79D3F54237BD591uL, 0x4496745A360289CDuL), (+1, -6, 0xFA1EC9569CE80C0EuL, 0x17578A0D0C8A195BuL)),
                ((+1, -1, 0xDF1D350D26B38B55uL, 0x9FDD1A1F679E9C4DuL), (+1, -6, 0xE596C4F541066F0FuL, 0x9322E2AB241D0A92uL)),
                ((+1, -1, 0xE5F2600564E83902uL, 0xA0EA69F69711C075uL), (+1, -6, 0xCF727E01748B4D1DuL, 0x61685F3916A2FA06uL)),
                ((+1, -1, 0xEC107B5D16C85F79uL, 0x3D2576F31F2DAA9BuL), (+1, -6, 0xB7D9B754B50CDA4CuL, 0xDB0C193C224D1962uL)),
                ((+1, -1, 0xF16C8AF250432FCDuL, 0x0DFC269BD7F8EEC2uL), (+1, -6, 0x9EF6D15843DD7BDCuL, 0x1DD8AD74516033BCuL)),
                ((+1, -1, 0xF5FCEF23DF8A1F22uL, 0xF3F108A294DC328AuL), (+1, -6, 0x84F67F18506CFC9AuL, 0x10668A824F2671F2uL)),
                ((+1, -1, 0xF9B97634DB080D32uL, 0xEB7F96A6D0D6C12AuL), (+1, -7, 0xD40EF44985182D69uL, 0x7E2AF43B45D421F3uL)),
                ((+1, -1, 0xFC9B6B6B7CFFA216uL, 0x064E78D5EBAA75E9uL), (+1, -7, 0x9CB4838F7FC06107uL, 0xBF05E53387D01461uL)),
                ((+1, -1, 0xFE9DA548D579286CuL, 0x358B8FF0D273F0F4uL), (+1, -8, 0xC8852A11853A3681uL, 0xB692B2D555287528uL)),
                ((+1, -1, 0xFFBCA5E87B4408F8uL, 0x1711255D210568C3uL), (+1, -9, 0xACC99C485E519BBBuL, 0xA69012B19FE1DE05uL)),
            });

            static ReadOnlyCollection<(ddouble x, ddouble w)> glas = new(new (ddouble, ddouble)[] {
                ((+1, -5, 0x99D36E85913AB729uL, 0x5561200A8990759AuL), (+1, -4, 0xBE1FA63464232EBCuL, 0x1D64D23C25D90C75uL)),
                ((+1, -3, 0xCAB1FC58487C4920uL, 0x1E504AE58A6F36B3uL), (+1, -3, 0xBC9FF9AC4CB39E1DuL, 0x2BD9369C8AA78887uL)),
                ((+1, -2, 0xF93B0959C5931AFBuL, 0x00C1737A04F6CC66uL), (+1, -3, 0xDE4E5232D3D71620uL, 0x880DA6F764BA4192uL)),
                ((+1, -1, 0xE7943781EB4EFAC3uL, 0x7261DB62963C1282uL), (+1, -3, 0xC7F90A0E964BBD5EuL, 0xF4E8CF237F0750BBuL)),
                ((+1, 0, 0xB9DFAB959D6A6BD1uL, 0xDAE733FD41CAFB94uL), (+1, -3, 0x92D9DCC89E538A97uL, 0x0B0BE22916666BE4uL)),
                ((+1, 1, 0x88570B43F6D00CF5uL, 0x3E6B9CB9EA500084uL), (+1, -4, 0xB4FBFEBF416CA21EuL, 0xF5C48E384F63F494uL)),
                ((+1, 1, 0xBC2DDC4D9FF88A30uL, 0xBD248839285678CDuL), (+1, -5, 0xBDB47BBA5D9EC3E1uL, 0xA0ADD9791FD0D9EEuL)),
                ((+1, 1, 0xF88B861D9B363A65uL, 0x2E4F37C6481B1AEFuL), (+1, -6, 0xAA536D39D48CECE5uL, 0xE5DC051AA7BFFD24uL)),
                ((+1, 2, 0x9EC5C8C99A936069uL, 0x858DC387F26B66B9uL), (+1, -7, 0x837BB9362AEF0C0AuL, 0xE719B0B99FF03B48uL)),
                ((+1, 2, 0xC5A70E109C55094AuL, 0x775A129808F4649FuL), (+1, -9, 0xAED4AEC80F192914uL, 0x9FF0219D73AC7E66uL)),
                ((+1, 2, 0xF0FC17C2B9B3F7CAuL, 0xF8A4BB3A41CA80EBuL), (+1, -11, 0xC847C4EADF48AC73uL, 0xF526839FB9DFC13DuL)),
                ((+1, 3, 0x906D0AB4763428A9uL, 0x89FF124CFF91B786uL), (+1, -13, 0xC5885D42F73C26A5uL, 0xB379EBB994869A58uL)),
                ((+1, 3, 0xAAAC8FF5330EAAB1uL, 0x5F1E1DF4B240250DuL), (+1, -15, 0xA77BEA9A38B0C084uL, 0x80E8AE0327CE7565uL)),
                ((+1, 3, 0xC74A422C66DF4A20uL, 0xC3E2DBD877E012F8uL), (+1, -18, 0xF39F697EDB28DAC8uL, 0x79BD5B446F97A12AuL)),
                ((+1, 3, 0xE6558E9EFB466329uL, 0xFEB2D70886362BB0uL), (+1, -20, 0x978E291FF83FD3C9uL, 0x7F89C740F14B570CuL)),
                ((+1, 4, 0x83EFEF5F51303776uL, 0xECBC9BCECEFD3C57uL), (+1, -23, 0xA0B86DF2FF2B13BCuL, 0xDF38339C65B0751BuL)),
                ((+1, 4, 0x95FE6C2E72BD428BuL, 0x884FE2056CCF45D2uL), (+1, -26, 0x90AB3C64DB98AB40uL, 0x2A99B1E15A589D15uL)),
                ((+1, 4, 0xA9615596E77471BEuL, 0xC11DAA3A6517FE2AuL), (+1, -30, 0xDBFC1EA56ECA0377uL, 0xD3B4B6B7804B23BBuL)),
                ((+1, 4, 0xBE2537783BD74A96uL, 0x1C2A0CE0470F3FCEuL), (+1, -33, 0x8C7A3005707106E9uL, 0xA2B6F2B4AC09A1F7uL)),
                ((+1, 4, 0xD4584B53A2DB7956uL, 0x55A2ADD33A21C3F8uL), (+1, -37, 0x95B4D9662EDD39B8uL, 0x62B9C8AB42C4B247uL)),
                ((+1, 4, 0xEC0ABF64733A4D8EuL, 0xF8625EA942CD91AEuL), (+1, -41, 0x8421493BF5AD0701uL, 0xEBE36FAB9BC07549uL)),
                ((+1, 5, 0x82A7879C38B77F19uL, 0x40702EE0B771B893uL), (+1, -46, 0xBF7EB907A5888385uL, 0x529253EB958B7BADuL)),
                ((+1, 5, 0x901D39A89A381CD7uL, 0x696376C295CE6534uL), (+1, -51, 0xE1989ED39593B9DBuL, 0xC9A43EBACEEC8EA7uL)),
                ((+1, 5, 0x9E72B7CA1540E474uL, 0x6CBEC24FD8E855A6uL), (+1, -56, 0xD58A4FB3B68CA209uL, 0x1289835F7EF71B6DuL)),
                ((+1, 5, 0xADB645D495E4091CuL, 0xF6613F22120510A9uL), (+1, -61, 0xA037DE0908FEF5A7uL, 0xA55B4CC0734FAAE0uL)),
                ((+1, 5, 0xBDF8A2C79C434AAFuL, 0xA58088643177377EuL), (+1, -67, 0xBB8E0DE54AE13502uL, 0x245D3DBB8A6F77CCuL)),
                ((+1, 5, 0xCF4DAB6A68A58588uL, 0x6735F43F3450EC8DuL), (+1, -73, 0xA80FBA584DBEF1B4uL, 0xC1335B70B94DE6B0uL)),
                ((+1, 5, 0xE1CD3915E0D8ADC8uL, 0xC151308F91FE8002uL), (+1, -80, 0xE15B366748A04652uL, 0x3FCFFDE22F09ED6CuL)),
                ((+1, 5, 0xF5945AC5D88DE0C5uL, 0x07DD90548FFC93B7uL), (+1, -87, 0xDBE9661F080CAAB8uL, 0xF88926B81FE18FFBuL)),
                ((+1, 6, 0x85638D30EB85ECBFuL, 0xE91517F5F637D2E7uL), (+1, -94, 0x96E383FFE93B5A85uL, 0x1A414AC7C8A2AD52uL)),
                ((+1, 6, 0x90C9927D8868A3CBuL, 0x2F1C4A7C0F83DF62uL), (+1, -102, 0x8B60BBDC9CF59AB9uL, 0xC3ACEE796CBD637CuL)),
                ((+1, 6, 0x9D19FA9F4699102FuL, 0x47FEF9E6906438A0uL), (+1, -111, 0xA3C54FFFA6D965C5uL, 0x397033FCE97EE791uL)),
                ((+1, 6, 0xAA7CDC8F0C60C37FuL, 0xD025822C06E6F622uL), (+1, -121, 0xE2CDB25C4090F282uL, 0x2E2EBE8F1AB957B3uL)),
                ((+1, 6, 0xB92AC8C54FA39795uL, 0xC20475A1679CA306uL), (+1, -131, 0xA6409307D1FC8AF3uL, 0x38069CE3A62C82D8uL)),
                ((+1, 6, 0xC978BBBED0D61F11uL, 0x2F19B6BD7FB98FC4uL), (+1, -143, 0xDBB20FD51BF84D4DuL, 0x998CC950C4892B58uL)),
                ((+1, 6, 0xDBF2BDF00710EBC5uL, 0x0B807311C164DF51uL), (+1, -156, 0xC90CCF0046C5AE89uL, 0x3EB3CD7AF9755384uL)),
                ((+1, 6, 0xF1A4DE6E759F7EE8uL, 0x8F99175E60319FDEuL), (+1, -171, 0x9A7AA70E1B4B4AEDuL, 0x0BBBFD74AD3462B6uL)),
                ((+1, 7, 0x86A0B2CD11BC5D04uL, 0xF6A348AFD9F3481FuL), (+1, -191, 0xE02B4BACBF6510B0uL, 0xEB91DEF67E1D2758uL)),
            });
        }

        private static class PolylogParticularN {
            public static ddouble Polylog(int n, ddouble x) {
#if DEBUG
                if (!(x < 1)) {
                    return NaN;
                }
#endif

                ddouble y;

                if (n == 1) {
                    y = -Log(1 - x);

                    return IsNaN(y) ? NegativeInfinity : y;
                }
                if (n == 0) {
                    y = x / (1 - x);

                    return IsNaN(y) ? -1 : y;
                }
                if (n >= -4) {
                    if (n == -1) {
                        y = x / Square(1 - x);
                    }
                    else if (n == -2) {
                        y = x * (1 + x) / Cube(1 - x);
                    }
                    else if (n == -3) {
                        y = x * (1 + x * (4 + x)) / Pow(1 - x, 4);
                    }
                    else {
                        y = x * (1 + x) * (1 + x * (10 + x)) / Pow(1 - x, 5);
                    }

                    return IsNaN(y) ? Zero : y;
                }

                throw new NotImplementedException();
            }
        }
    }
}