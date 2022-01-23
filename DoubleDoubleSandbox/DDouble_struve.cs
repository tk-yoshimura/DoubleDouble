﻿using DoubleDouble;
using System.Collections.ObjectModel;

using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class Struve {

        internal static class StruveKIntegral {
            public static ddouble Value(int a, ddouble x) {
                ddouble s = 0;

                foreach ((ddouble u, ddouble w) in gls) {
                    ddouble v = Sqrt(Square(u / x) + 1d);

                    s += w * Pow(v, 2 * a - 1);
                }

                ddouble y = 2 * s * Pow(x / 2, a) / (Sqrt(PI) * Gamma(a + Point5) * x);

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
    }
}