using DoubleDouble;
using System;
using System.Linq;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    internal class Pow2Mark2 {
        public static (ddouble value, int terms) Pow2Prime(ddouble x) {
            if (!(x >= 0d) || x > 1d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x == 1d) {
                return (2, 0);
            }

            ddouble v = x * Log(2);

            ddouble w = v;
            ddouble y = 1d;

            int terms = 0;
            foreach (ddouble f in TaylorSequence.Skip(1)) {
                ddouble dy = f * w;
                ddouble y_next = y + dy;

                if (y == y_next) {
                    break;
                }

                w *= v;
                y = y_next;
                terms++;
            }

            return (y, terms);
        }
    }
}
