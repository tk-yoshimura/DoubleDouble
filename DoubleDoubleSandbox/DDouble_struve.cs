﻿using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Struve {

        public static ddouble StruveH(int n, ddouble x) {
            if (x.Sign < 0) {
                return ((n & 1) == 0) ? -StruveH(n, -x) : StruveH(n, -x);
            }

            if (n < 0 || n > 16) {
                throw new ArgumentException(
                    "In the calculation of the StruveH function, n greater than 16 and negative integer are not supported."
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

            if (x < 1) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 16);
            }

            return StruveKIntegral.Value(n, x) + BesselY(n, x);
        }

        public static ddouble StruveK(int n, ddouble x) {
            if (n < 0 || n > 16) {
                throw new ArgumentException(
                    "In the calculation of the StruveK function, n greater than 16 and negative integer are not supported."
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

            if (x < 1) {
                return StruveHLNearZero.Value(n, x, sign_switch: true, terms: 16) - BesselY(n, x);
            }

            return StruveKIntegral.Value(n, x);
        }

        public static ddouble StruveL(int n, ddouble x) {
            if (x.Sign < 0) {
                return ((n & 1) == 0) ? -StruveL(n, -x) : StruveL(n, -x);
            }

            if (n < 0 || n > 16) {
                throw new ArgumentException(
                    "In the calculation of the StruveL function, n greater than 16 and negative integer are not supported."
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
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 16);
            }

            return StruveMIntegral.Value(n, x) + BesselI(n, x);
        }

        public static ddouble StruveM(int n, ddouble x) {
            if (n < 0 || n > 16) {
                throw new ArgumentException(
                    "In the calculation of the StruveM function, n greater than 16 and negative integer are not supported."
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
                return StruveHLNearZero.Value(n, x, sign_switch: false, terms: 16) - BesselI(n, x);
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
                ("0.044489365833267018418850194524358233","0.10921834195238497113613133794342857"),       
                ("0.23452610951961853745290956130193161","0.21044310793881323293606207139170986"),        
                ("0.57688462930188642649155256937756349","0.23521322966984800539494110667639595"),        
                ("1.0724487538178176330409139771776176","0.19590333597288104341324790118177048"),         
                ("1.7224087764446454411309329279747168","0.12998378628607176060721682232593554"),         
                ("2.5283367064257948811241999055589138","0.070578623865717441560164332043307076"),        
                ("3.4922132730219944896088033997715362","0.031760912509175070305825521162865025"),        
                ("4.6164567697497673877620522961721406","0.01191821483483855705654465057555808"),         
                ("5.9039585041742439465615214915812608","0.0037388162946115247896612284779588906"),       
                ("7.358126733186241113221989737194996","9.8080330661495513223063061130806727e-4"),        
                ("8.982940924212596103378247526765696","2.1486491880136418802319948368572083e-4"),        
                ("10.78301863253997206750194913805835","3.9203419679879472043269568278231476e-5"),        
                ("12.7636979867427251149690330822059","5.9345416128686328783558289377302221e-6"),         
                ("14.931139755522557319796964687308664","7.4164045786675522190708220213025171e-7"),       
                ("17.29245433671531478923571838361985","7.604567879120781481119265459434528e-8"),         
                ("19.855860940336054739789944584146086","6.3506022266258067424277710855230037e-9"),       
                ("22.630889013196774488675779339387332","4.2813829710409288788136058209795004e-10"),      
                ("25.628636022459247767476176876849808","2.3058994918913360792733680961757232e-11"),      
                ("28.862101816323474744342640711515511","9.7993792887270940633345522599474391e-13"),      
                ("32.346629153964737003232165423674687","3.2378016577292664623104264614175337e-14"),      
                ("36.100494805751973804017118947901272","8.1718234434207194332018605917701627e-16"),      
                ("40.145719771539441536209343928925637","1.5421338333938233721785594912914412e-17"),      
                ("44.5092079957549379759066043775214","2.1197922901636186120409347437315712e-19"),        
                ("49.224394987308639176722221806566342","2.0544296737880454266557098760187948e-21"),      
                ("54.333721333396907332867181551169384","1.3469825866373951558051934047774277e-23"),      
                ("59.892509162134018196130475324669375","5.6612941303973593711263443238217253e-26"),      
                ("65.975377287935052796563076119348591","1.4185605454630369059514293389207697e-28"),      
                ("72.687628090662708638675349087774626","1.913375494454224309371278296830731e-31"),       
                ("80.187446977913523067491638568690273","1.1922487600982223565416453283118209e-34"),      
                ("88.735340417892398689355449524296441","2.6715112192401369859986789395811827e-38"),      
                ("98.829542868283972559184478409520488","1.338616942106256282719057014226361e-42"),       
                ("111.75139809793769521366471653944925","4.5105361938989742322234283013237399e-48"),     
            });
        }

        internal static class StruveMIntegral {
            public static ddouble Value(int n, ddouble x){
                ddouble r = 1d / x;

                ddouble h = Min(x, 72); // exp(-72) < 1e-31

                int divs = (int)ddouble.Ceiling(h / 2);
                ddouble q = h / divs;
                
                ddouble s = 0;

                for (int i = 0; i < divs; i++) {
                    foreach ((ddouble u, ddouble w) in gls) {
                        ddouble u_sft = (u + i) * q;

                        ddouble v = Pow(Sqrt(1d - Square(u_sft * r)), 2 * n - 1) * Exp(-u_sft);

                        s += w * v;
                    }
                }

                s *= q;

                ddouble y = - s * Pow(x / 2, n - 1) * RcpPI * StruveGTable.Value(n);

                return y;
            }

            public static ddouble ValueMk2(int n, ddouble x){
                ddouble s = 0;

                foreach ((ddouble u, ddouble w) in gls) {
                    ddouble u_sft = u / 2;

                    ddouble v = Exp(-x * SinPI(u_sft)) * Pow(CosPI(u_sft), 2 * n);

                    s += w * v;
                }

                ddouble y = -s * Pow(x / 2, n) * StruveGTable.Value(n);

                return y;
            }

            static ReadOnlyCollection<(ddouble u, ddouble w)> gls = new(new (ddouble, ddouble)[] {
                ("0.0013680690752592182275094356674796364","0.0035093050047350483002035318694265913"),
                ("0.0071942442273658322999124776845490107","0.0081371973654528353025852811031933091"),
                ("0.01761887220624678461309403594086252","0.012696032654631029727876294894612015"),
                ("0.032546962031130155414540432582295337","0.017136931456510716551343866126186354"),
                ("0.051839422116973938017346378140393866","0.021417949011113340328439323303062764"),
                ("0.075316193133715014933153497516128731","0.025499029631188088098081622344760848"),
                ("0.10275810201602879651845135051478555","0.029342046739267773572641818650085443"),
                ("0.13390894062985515980628666745436643","0.032911111388180923418825031853469386"),
                ("0.16847786653489239951244241566838082","0.036172897054424253112699678239243896"),
                ("0.20614212137961883547962726179908657","0.039096947893535153235870459414153336"),
                ("0.24655004553388530498812626281108938","0.041655962113473377611099537302174306"),
                ("0.28932436193468232731794028191378676","0.043826046502201905571385731375901144"),
                ("0.334065698858936175110041597134906","0.045586939347881942356434288555818531"),
                ("0.38035631887393146272769839541724924","0.04692219954040228281959011883405863"),
                ("0.42776401920860175325740681320059467","0.04781936003963742970954100110206555"),
                ("0.47584616715613084188259371477974892","0.048270044257363900283382415031787897"),
                ("0.52415383284386915811740628522025108","0.048270044257363900283382415031787897"),
                ("0.57223598079139824674259318679940533","0.04781936003963742970954100110206555"),
                ("0.61964368112606853727230160458275076","0.04692219954040228281959011883405863"),
                ("0.665934301141063824889958402865094","0.045586939347881942356434288555818531"),
                ("0.71067563806531767268205971808621324","0.043826046502201905571385731375901144"),
                ("0.75344995446611469501187373718891062","0.041655962113473377611099537302174306"),
                ("0.79385787862038116452037273820091343","0.039096947893535153235870459414153336"),
                ("0.83152213346510760048755758433161918","0.036172897054424253112699678239243896"),
                ("0.86609105937014484019371333254563357","0.032911111388180923418825031853469386"),
                ("0.89724189798397120348154864948521445","0.029342046739267773572641818650085443"),
                ("0.92468380686628498506684650248387127","0.025499029631188088098081622344760848"),
                ("0.94816057788302606198265362185960613","0.021417949011113340328439323303062764"),
                ("0.96745303796886984458545956741770466","0.017136931456510716551343866126186354"),
                ("0.98238112779375321538690596405913748","0.012696032654631029727876294894612015"),
                ("0.99280575577263416770008752231545099","0.0081371973654528353025852811031933091"),
                ("0.99863193092474078177249056433252036","0.0035093050047350483002035318694265913"),
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