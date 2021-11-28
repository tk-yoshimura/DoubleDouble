using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            else {
                if (x < Consts.Gamma.Threshold) {
                    ddouble a = LanczosAg(x);
                    ddouble s = x - 0.5d;
                    ddouble t = (s + Consts.Gamma.LanczosG) / ddouble.E;

                    ddouble y = ddouble.Pow(t, s) * a;

                    return y;
                }
                else {
                    ddouble r = ddouble.Sqrt(Ldexp(ddouble.PI, 1) / x);
                    ddouble p = ddouble.Pow(x / ddouble.E, x);
                    ddouble s = ddouble.Exp(SterlingTerm(x));

                    ddouble y = r * p * s;

                    return y;
                }
            }
        }

        private static ddouble LanczosAg(ddouble z) {
            KahanSum x = Consts.Gamma.LanczosTable[0];

            foreach (ddouble w in Consts.Gamma.LanczosTable.Skip(1)) {
                x.Add(w / z);
                z += 1d;
            }

            return x.Sum;
        }

        private static ddouble SterlingTerm(ddouble z) {
            KahanSum x = Consts.Gamma.SterlingTable[0];

            ddouble v = Rcp(z), w = v * v, u = w;

            foreach (ddouble s in Consts.Gamma.SterlingTable.Skip(1)) {
                x.Add(u * s);

                if (x.IsConvergence) {
                    break;
                }

                u *= w;
            }

            ddouble y = x.Sum * v;

            return y;
        }

        private static partial class Consts {
            public static class Gamma {
                public const int Threshold = 16;
                public static readonly ddouble LanczosG = 15;
                public static readonly ReadOnlyCollection<ddouble> LanczosTable = GenerateLanczosTable();
                public static readonly ReadOnlyCollection<ddouble> SterlingTable = GenerateSterlingTable();

                public static ReadOnlyCollection<ddouble> GenerateLanczosTable() {
                    ddouble[] table = new ddouble[] {
                        "7.6678340584511090768111016362140716937081e-7",
                        "2.3095999430606837593196572374398415491958e0",
                        "-1.1067739531430221036551357286724018384821e1",
                        "2.2672991859627835939780784479950497526762e1",
                        "-2.5957359034770493986309788082531088180937e1",
                        "1.8231207562326656079863789286438070676056e1",
                        "-8.1222982476677348384890092090519472242482e0",
                        "2.2960238148286529773585644823589757761321e0",
                        "-4.0116249593049077966958794044745363585376e-1",
                        "4.0995851550928043334268604454170230227883e-2",
                        "-2.2278448321118365704885500654861017309350e-3",
                        "5.4812082380070075048767255966455653703024e-5",
                        "-4.5839539923264631644475747135164306872107e-7",
                        "7.3321631669560179673152595762607913510812e-10",
                        "-5.2478093674176154550750667905692133843319e-14",
                    };

                    return Array.AsReadOnly(table);
                }

                private static ReadOnlyCollection<ddouble> GenerateSterlingTable() {
                    List<ddouble> table = new();
                    
                    for (int k = 1; k <= 28; k++) {
                        ddouble c = ddouble.BernoulliSequence[k] / checked((2 * k) * (2 * k - 1));
                        table.Add(c);
                    }

                    return table.AsReadOnly();
                }
            }
        }
    }
}
