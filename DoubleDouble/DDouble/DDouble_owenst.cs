using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble OwenT(ddouble h, ddouble a) {
            if (a.Sign < 0) {
                return -OwenT(h, -a);
            }
            if (IsNaN(h) || IsNaN(a)) {
                return NaN;
            }

            h = Abs(h);

            if (h <= OwenTIntegrate.Eps) {
                return Atan(a) / (2 * PI);
            }
            if (h > 36d) {
                return Zero;
            }

            if ((double)a >= 11.5380 / Math.Pow((double)h, 0.9892)) {
                return Erfc(h / Sqrt2) / 4;
            }

            if (a <= OwenTIntegrate.Eps) {
                return OwenTIntegrate.NearZeroA(h, a);
            }

            if (a < 4d || h > 0.675d) {
                return OwenTIntegrate.GaussQuadrature(h, a);
            }
            else {
                ddouble c = (1d - Erf(h / Sqrt2) * Erf(h * a / Sqrt2)) / 4;
                ddouble t = OwenTIntegrate.GaussQuadrature(h * a, 1d / a);

                ddouble y = c - t;

                return y;
            }
        }

        internal static class OwenTIntegrate {

            public static readonly double Eps = Math.ScaleB(1, -64);

            public static ddouble GaussQuadrature(ddouble h, ddouble a) {
                ddouble h2 = h * h, n_half_h2 = -h2 / 2;

                ddouble ig = Sqrt(PI / 2) / h * Exp(n_half_h2) * Erf(h * a / Sqrt2);

                ddouble x_peak = Sqrt((Sqrt(8d / h2 + 1d) - 1d) / 2);
                ddouble ap = Min(a, x_peak * 2), ad = a - ap;

                ddouble sp = 0, sd = 0;

                for (int k = 0; k < gls.Count; k++) {
                    (ddouble x, ddouble w) = gls[k];
                    ddouble x_sft = x * ap;

                    ddouble p = x_sft * x_sft;
                    ddouble r = 1d + p;

                    ddouble u = w * Exp(n_half_h2 * r) * p / r;

                    sp += u;
                }

                if (ad > 0) {
                    for (int k = 0; k < gls.Count; k++) {
                        (ddouble x, ddouble w) = gls[k];
                        ddouble x_sft = x * ad + ap;

                        ddouble p = x_sft * x_sft;
                        ddouble r = 1d + p;

                        ddouble u = w * Exp(n_half_h2 * r) * p / r;

                        sd += u;
                    }
                }

                ddouble y = (ig - sp * ap - sd * ad) / (2 * PI);

                if (y < Epsilon) {
                    return a * Exp(n_half_h2) / (2 * PI);
                }

                return y;
            }

            public static ddouble NearZeroA(ddouble h, ddouble a) {
                ddouble a2 = a * a;
                ddouble h2 = h * h, h4 = h2 * h2, h6 = h2 * h4;

                ddouble p1 = h2 + 2d;
                ddouble p2 = h4 + 4 * p1;
                ddouble p3 = h6 + 6 * p2;

                ddouble s = a * (1680d + (a2 * (-280d * p1 + (a2 * (42d * p2 - a2 * (5d * p3)))))) / 1680d;
                ddouble c = Exp(-h2 / 2) / (2 * PI);

                ddouble y = s * c;

                if (y < Epsilon) {
                    return a * c;
                }

                return y;
            }

            static ReadOnlyCollection<(ddouble x, ddouble w)> gls = new(new (ddouble, ddouble)[] {
                ((+1, -11, 0xB705E48DEC64E67BuL, 0xD073C5F3DA064847uL), (+1, -10, 0xEACB1D15A390A9C7uL, 0x6102426DBDA4E41BuL)),
                ((+1, -9, 0xF0D84825BC4E5D7AuL, 0x326A73D47151EF22uL), (+1, -8, 0x885DFC74D7DA4BB3uL, 0xBC4144797EF4AA2CuL)),
                ((+1, -7, 0x93B65867687CA5BEuL, 0x0D178D848CF90E06uL), (+1, -8, 0xD5806A7140F826CAuL, 0x45563956AC56D2DFuL)),
                ((+1, -6, 0x88C5B96CDB38B93CuL, 0xAA16663B35DEC4A6uL), (+1, -7, 0x90D07A79D0B91374uL, 0x201A9DF9CF1B79FAuL)),
                ((+1, -6, 0xDA8E3E024C54BD1FuL, 0xF4AAE14DB4A2E5A1uL), (+1, -7, 0xB630573CBA7608E8uL, 0xDDC414B0567664F0uL)),
                ((+1, -5, 0x9F688391EE241966uL, 0xE64F33369EBD0F39uL), (+1, -7, 0xDAB204BD5E539A85uL, 0xDB7366C541327B8CuL)),
                ((+1, -5, 0xDA89E5603ECA8A29uL, 0x8368D061F1BEDC91uL), (+1, -7, 0xFE28ED56138391BDuL, 0xB72DFB9A4C31D331uL)),
                ((+1, -4, 0x8F319162029F57C4uL, 0xF896122F4A83CECAuL), (+1, -6, 0x9034E479B3321CEEuL, 0x05419017CD450168uL)),
                ((+1, -4, 0xB550BB50BCB59880uL, 0x873F195C5369A47EuL), (+1, -6, 0xA0A566217FED24C6uL, 0x157901D4006B1D64uL)),
                ((+1, -4, 0xDF73EEE3888810D9uL, 0xCEA408339E681E09uL), (+1, -6, 0xB051ED0D73698E82uL, 0x482D39BDD2893BE9uL)),
                ((+1, -3, 0x86B3E2234ACF90C2uL, 0x173DBA4A73C39083uL), (+1, -6, 0xBF2759F01E901EAEuL, 0x03ED2009DB34A2BEuL)),
                ((+1, -3, 0x9F7A1615006136D5uL, 0xADD43CC9080B07CFuL), (+1, -6, 0xCD1393FD1CB55A95uL, 0x94CBBFDDFA29628DuL)),
                ((+1, -3, 0xB9EE5A14F42178CCuL, 0xDBAA100AA2CC6554uL), (+1, -6, 0xDA059EF177C560E5uL, 0xD7C0DD063ECFA2C3uL)),
                ((+1, -3, 0xD5F068474F73BE74uL, 0x21BA65728908FEB3uL), (+1, -6, 0xE5EDAFC702E658CBuL, 0x6FEEE8344A4B7689uL)),
                ((+1, -3, 0xF35E158605FD01A6uL, 0xEB380340843DB285uL), (+1, -6, 0xF0BD3FF5FFAA77C4uL, 0x6C4508F47D8F91A1uL)),
                ((+1, -2, 0x8909BD88278D4E84uL, 0xD45802A17446932DuL), (+1, -6, 0xFA671F2C26951928uL, 0x96CA6A0E99E0A52AuL)),
                ((+1, -2, 0x98F5912B95766F6DuL, 0xCCEB21072C28C602uL), (+1, -5, 0x816FC1B16AD31A38uL, 0xC52DA8B263B97E05uL)),
                ((+1, -2, 0xA95F1958C97AB03FuL, 0x5B8558864FD837AEuL), (+1, -5, 0x850E0BA03C61BE0EuL, 0xC9E4B59678114B8DuL)),
                ((+1, -2, 0xBA3250604E4197A0uL, 0x6B7F4EF4E788AFFAuL), (+1, -5, 0x880A0359BD6514F0uL, 0x43286C39576DBFE8uL)),
                ((+1, -2, 0xCB5AAFA4E05AF82FuL, 0x587E53BF724B5026uL), (+1, -5, 0x8A6004DBA55574DFuL, 0xBC3E1D7C8E62868EuL)),
                ((+1, -2, 0xDCC348A5D9CA67E4uL, 0x2F5C6CB5AF3C4FE9uL), (+1, -5, 0x8C0D369B2328CC55uL, 0x725D7F74F4111659uL)),
                ((+1, -2, 0xEE56DE88597881AFuL, 0xCF6B9AF5A299019CuL), (+1, -5, 0x8D0F8CFED79D8926uL, 0xBCC32D9ABC1E0C8DuL)),
                ((+1, -1, 0x8000000000000000uL, 0x0000000000000000uL), (+1, -5, 0x8D65CCDD94EA3133uL, 0xFFB6E3F44620A5A7uL)),
                ((+1, -1, 0x88D490BBD343BF28uL, 0x184A32852EB37F31uL), (+1, -5, 0x8D0F8CFED79D8926uL, 0xBCC32D9ABC1E0C8DuL)),
                ((+1, -1, 0x919E5BAD131ACC0DuL, 0xE851C9A52861D80BuL), (+1, -5, 0x8C0D369B2328CC55uL, 0x725D7F74F4111659uL)),
                ((+1, -1, 0x9A52A82D8FD283E8uL, 0x53C0D62046DA57ECuL), (+1, -5, 0x8A6004DBA55574DFuL, 0xBC3E1D7C8E62868EuL)),
                ((+1, -1, 0xA2E6D7CFD8DF342FuL, 0xCA4058858C3BA802uL), (+1, -5, 0x880A0359BD6514F0uL, 0x43286C39576DBFE8uL)),
                ((+1, -1, 0xAB5073539B42A7E0uL, 0x523D53BCD813E428uL), (+1, -5, 0x850E0BA03C61BE0EuL, 0xC9E4B59678114B8DuL)),
                ((+1, -1, 0xB385376A3544C849uL, 0x198A6F7C69EB9CFEuL), (+1, -5, 0x816FC1B16AD31A38uL, 0xC52DA8B263B97E05uL)),
                ((+1, -1, 0xBB7B213BEC3958BDuL, 0x95D3FEAF45DCB669uL), (+1, -6, 0xFA671F2C26951928uL, 0x96CA6A0E99E0A52AuL)),
                ((+1, -1, 0xC3287A9E7E80BF96uL, 0x4531FF2FDEF0935EuL), (+1, -6, 0xF0BD3FF5FFAA77C4uL, 0x6C4508F47D8F91A1uL)),
                ((+1, -1, 0xCA83E5EE2C231062uL, 0xF79166A35DBDC053uL), (+1, -6, 0xE5EDAFC702E658CBuL, 0x6FEEE8344A4B7689uL)),
                ((+1, -1, 0xD184697AC2F7A1CCuL, 0xC9157BFD574CE6AAuL), (+1, -6, 0xDA059EF177C560E5uL, 0xD7C0DD063ECFA2C3uL)),
                ((+1, -1, 0xD8217A7ABFE7B24AuL, 0x948AF0CDBDFD3E0CuL), (+1, -6, 0xCD1393FD1CB55A95uL, 0x94CBBFDDFA29628DuL)),
                ((+1, -1, 0xDE5307772D4C1BCFuL, 0x7A30916D630F1BDFuL), (+1, -6, 0xBF2759F01E901EAEuL, 0x03ED2009DB34A2BEuL)),
                ((+1, -1, 0xE41182238EEEFDE4uL, 0xC62B7EF98C32FC3EuL), (+1, -6, 0xB051ED0D73698E82uL, 0x482D39BDD2893BE9uL)),
                ((+1, -1, 0xE955E895E8694CEFuL, 0xEF181CD47592CB70uL), (+1, -6, 0xA0A566217FED24C6uL, 0x157901D4006B1D64uL)),
                ((+1, -1, 0xEE19CDD3BFAC1507uL, 0x60ED3DBA16AF8626uL), (+1, -6, 0x9034E479B3321CEEuL, 0x05419017CD450168uL)),
                ((+1, -1, 0xF25761A9FC13575DuL, 0x67C972F9E0E41236uL), (+1, -7, 0xFE28ED56138391BDuL, 0xB72DFB9A4C31D331uL)),
                ((+1, -1, 0xF60977C6E11DBE69uL, 0x919B0CCC96142F0CuL), (+1, -7, 0xDAB204BD5E539A85uL, 0xDB7366C541327B8CuL)),
                ((+1, -1, 0xF92B8E0FED9D5A17uL, 0x005AA8F5925AE8D2uL), (+1, -7, 0xB630573CBA7608E8uL, 0xDDC414B0567664F0uL)),
                ((+1, -1, 0xFBB9D23499263A36uL, 0x1AAF4CCE265109DAuL), (+1, -7, 0x90D07A79D0B91374uL, 0x201A9DF9CF1B79FAuL)),
                ((+1, -1, 0xFDB1269E625E0D69uL, 0x07CBA1C9EDCC1BC7uL), (+1, -8, 0xD5806A7140F826CAuL, 0x45563956AC56D2DFuL)),
                ((+1, -1, 0xFF0F27B7DA43B1A2uL, 0x85CD958C2B8EAE10uL), (+1, -8, 0x885DFC74D7DA4BB3uL, 0xBC4144797EF4AA2CuL)),
                ((+1, -1, 0xFFD23E86DC84E6C6uL, 0x610BE30E83097E6DuL), (+1, -10, 0xEACB1D15A390A9C7uL, 0x6102426DBDA4E41BuL)),
            });
        }
    }
}
