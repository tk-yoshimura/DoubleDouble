using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public ReadOnlyCollection<Func<ddouble, ddouble>> LaguerrePolynomials => Consts.Laguerre.LaguerrePolynomials;

        internal static partial class Consts {
            public static class Laguerre {
                public static ReadOnlyCollection<Func<ddouble, ddouble>> LaguerrePolynomials = 
                    new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                        (x) => 1d, 
                        (x) => 1d - x,
                        (x) => {
                            return (2d + x * (-4d + x)) / 2;  
                        },
                        (x) => {
                            return (6d + x * (-18d + x * (9d + x))) / 6;  
                        },
                        (x) => {
                            return (24d + x * (-96d + x * (72d + x * (-16d + x)))) / 24;  
                        },
                        (x) => {
                            return (120d + x * (-600d + x * (600d + x * (-200d + x * (25d + x))))) / 120;  
                        },
                        (x) => {
                            return (720d + x * (-4320d + x * (5400d + x * (-2400d + x * (450d + x * (-36d + x)))))) / 720;  
                        },
                        (x) => {
                            return (5040d + x * (-35280d + x * (52920d + x * (-29400d + x * (7350d + x * (-882d + x * (49d + x))))))) / 5040; 
                        },
                        (x) => {
                            return (40320d + x * (-322560d + x * (564480d + x * (-376320d + x * (117600d + x * (-18816d + x * (1568d + x * (-64d + x)))))))) / 40320; 
                        },
                        (x) => {
                            return (362880d + x * (-3265920d + x * (6531840d + x * (-5080320d + x * (1905120d + x * (-381024d + x * (42336d + x * (-2592d + x * (81d + x))))))))) / 362880; 
                        },
                        (x) => {
                            return (3628800d + x * (-36288000d + x * (81648000d + x * (-72576000d + x * (31752000d + x * (-7620480d + x * (1058400d + x * (-86400d + x * (4050d + x * (-100d + x)))))))))) / 362880;  
                        }
                });
            }
        }
    }
}