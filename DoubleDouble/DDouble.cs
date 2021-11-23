using System;
using System.Diagnostics;

namespace DoubleDouble {

    [DebuggerDisplay("{(hi + lo)}")]
    public partial struct ddouble {
        private readonly double hi, lo;

        private ddouble(double hi, double lo) {
            this.hi = hi + lo;
            this.lo = lo - (this.hi - hi);

            if (double.IsNaN(this.lo)) {
                this.lo = 0;
            }
        }

        public static ddouble Zero { private set; get; } = new ddouble(0d, 0d);
        public static ddouble Epsilon { private set; get; } = double.Epsilon;
        public static ddouble MaxValue { private set; get; } = double.MaxValue;
        public static ddouble MinValue { private set; get; } = double.MinValue;
        public static ddouble NaN { private set; get; } = double.NaN;
        public static ddouble NegativeInfinity = double.NegativeInfinity;
        public static ddouble PositiveInfinity = double.PositiveInfinity;

        public static implicit operator ddouble(double v) {
            return new ddouble(v, Math.CopySign(0d, v));
        }

        public static explicit operator double(ddouble v) {
            return v.hi + v.lo;
        }
    }
}
