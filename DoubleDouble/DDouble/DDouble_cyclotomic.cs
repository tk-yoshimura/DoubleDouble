using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble Cyclotomic(int n, ddouble x) {
            if (n < 1) {
                return NaN;
            }
            if (n > 32) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Cyclotomic function, n greater than 32 is not supported."
                );
            }

            return CyclotomicTable.Table[n](x);
        }

        internal static class CyclotomicTable {
            public static readonly ReadOnlyCollection<Func<ddouble, ddouble>> Table = new(new Func<ddouble, ddouble>[]{
                (x) => NaN,
                (x) => -1d+x,
                (x) => 1d+x,
                (x) => 1d+x*(1d+x),
                (x) => 1d+x*x,
                (x) => 1d+x*(1d+x*(1d+x*(1d+x))),
                (x) => 1d+(-1d+x)*x,
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))),
                (x) => 1d+x*x*x*x,
                (x) => {
                    ddouble x3=x*x*x;
                    return 1d+x3*(1d+x3);
                },
                (x) => 1d+x*(-1d+x*(1d+(-1d+x)*x)),
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))),
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x2*(-1d+x2);
                },
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))),
                (x) => 1d+x*(-1d+x*(1d+x*(-1d+x*(1d+(-1d+x)*x)))),
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x*(-1d+x2*(1d+x*(-1d+x*(1d+(-1d+x)*x2))));
                },
                (x) => 1d+Pow(x, 8),
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))))))),
                (x) => {
                    ddouble x3=x*x*x;
                    return 1d+x3*(-1d+x3);
                },
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))))))))),
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x2*(-1d+x2*(1d+x2*(-1d+x2)));
                },
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x*(-1d+x2*(1d+x*(-1d+x2*(1d+x2*(-1d+x*(1d+(-1d+x)*x2))))));
                },
                (x) => 1d+x*(-1d+x*(1d+x*(-1d+x*(1d+x*(-1d+x*(1d+x*(-1d+x*(1d+(-1d+x)*x)))))))),
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))))))))))))),
                (x) => {
                    ddouble x4=x*x*x*x;
                    return 1d+x4*(-1d+x4);
                },
                (x) => {
                    ddouble x5=Pow(x, 5);
                    return 1d+x5*(1d+x5*(1d+x5*(1d+x5)));
                },
                (x) => 1d+x*(-1d+x*(1d+x*(-1d+x*(1d+x*(-1d+x*(1d+x*(-1d+x*(1d+x*(-1d+x*(1d+(-1d+x)*x)))))))))),
                (x) => {
                    ddouble x9=Pow(x, 9);
                    return 1d+x9*(1d+x9);
                },
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x2*(-1d+x2*(1d+x2*(-1d+x2*(1d+x2*(-1d+x2)))));
                },
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))))))))))))))))))),
                (x) => {
                    ddouble x2=x*x;
                    return 1d+x*(1d+x2*(-1d+x*(-1d+x*(-1d+x2*(1d+x)))));
                },
                (x) => 1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x*(1d+x))))))))))))))))))))))))))))),
                (x) => 1d + Pow(x, 16)
            });
        }
    }
}