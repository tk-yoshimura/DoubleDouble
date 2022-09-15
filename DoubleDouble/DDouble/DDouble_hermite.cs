using System;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public ReadOnlyCollection<Func<ddouble, ddouble>> HermitePolynomials => Consts.Hermite.HermitePolynomials;

        internal static partial class Consts {
            public static class Hermite {
                public static ReadOnlyCollection<Func<ddouble, ddouble>> HermitePolynomials = 
                    new ReadOnlyCollection<Func<ddouble, ddouble>>(new Func<ddouble, ddouble>[]{
                        (x) => 1d, 
                        (x) => Ldexp(x, 1),
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(-0.5d + x2, 2);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((-1.5d + x2) * x, 3);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(0.75d + x2 * (-3d + x2), 4);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((3.75d + x2 * (-5d + x2)) * x, 5);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(-1.875d + x2 * (11.25d + x2 * (-7.5 + x2)), 6); 
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((-13.125d + x2 * (26.25d + x2 * (-10.5d + x2))) * x, 7);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(6.5625d + x2 * (-52.5d + x2 * (52.5d + x2 * (-14.0d + x2))), 8); 
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((59.0625d + x2 * (-157.5d + x2 * (94.5d + x2 * (-18.0d + x2)))) * x, 9);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(-29.53125d + x2 * (295.3125d + x2 * (-393.75d + x2 * (157.5d + x2 * (-22.5d + x2)))), 10); 
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((-324.84375d + x2 * (-1082.8125d + x2 * (-866.25d + x2 * (247.5d + x2 * (-27.5d + x2))))) * x, 11);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(162.421875d + x2 * (-1949.0625d + x2 * (3248.4375d + x2 * (-1732.5d + x2 * (371.25d + x2 * (-33d + x2))))), 12); 
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((2111.484375d + x2 * (-8445.9375d + x2 * (8445.9375d + x2 * (-3217.5d + x2 * (536.25d + x2 * (-39d + x2)))))) * x, 13);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(-1055.7421875d + x2 * (14780.390625d + x2 * (-29560.78125d + x2 * (19707.1875d + x2 * (-5630.625d + x2 * (750.75d + x2 * (-45.5d + x2)))))), 14); 
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp((-15836.1328125d + x2 * (73901.953125d + x2 * (-88682.34375d + x2 * (42229.6875d + x2 * (-9384.375d + x2 * (1023.75d + x2 * (-52.5d + x2))))))) * x, 15);  
                        },
                        (x) => {
                            ddouble x2 = x * x;
                            return Ldexp(7918.06640625d + x2 * (-126689.0625d + x2 * (295607.8125d + x2 * (-236486.25d + x2 * (84459.375d + x2 * (-15015.0d + x2 * (1365.0 + x2 * (-60d + x2))))))), 16); 
                        },
                });
            }
        }
    }
}