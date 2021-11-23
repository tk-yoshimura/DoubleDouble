using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace DoubleDouble {
    public partial struct ddouble {
        public static ddouble operator +(ddouble v) {
            return v;
        }

        public static ddouble operator -(ddouble v) {
            return new ddouble(-v.hi, -v.lo);
        }

        public static ddouble operator +(ddouble a, double b) {
            double hi = a.hi + b;
            double lo = a.lo + (b - (hi - a.hi));

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(double a, ddouble b) {
            double hi = a + b.hi;
            double lo = b.lo + (b.hi - (hi - a));

            return new ddouble(hi, lo);
        }

        public static ddouble operator +(ddouble a, ddouble b) {
            double hi = a.hi + b.hi;
            double lo = a.lo + (b.lo + (b.hi - (hi - a.hi)));

            return new ddouble(hi, lo);
        }

        public static ddouble operator -(ddouble a, double b) {
            return a + (-b);
        }

        public static ddouble operator -(double a, ddouble b) {
            return a + (-b);
        }

        public static ddouble operator -(ddouble a, ddouble b) {
            return a + (-b);
        }

        public static ddouble operator *(ddouble a, double b) {
            ddouble y = MultiplyAdd(Zero, a.hi, b);
            y = MultiplyAdd(y, a.lo, b);

            return y;
        }

        public static ddouble operator *(double a, ddouble b) {
            return b * a;
        }

        public static ddouble operator *(ddouble a, ddouble b) {
            ddouble y = MultiplyAdd(Zero, a.hi, b.hi);
            y = MultiplyAdd(y, a.hi, b.lo);
            y = MultiplyAdd(y, a.lo, b.hi);
            y = MultiplyAdd(y, a.lo, b.lo);

            return y;
        }

        public static ddouble MultiplyAdd(ddouble v, double a, double b) {
            double hi = Math.FusedMultiplyAdd(a, b, v.hi);
            double lo = v.lo + Math.FusedMultiplyAdd(a, b, v.hi - hi);

            return new ddouble(hi, lo);
        }

        public static ddouble MultiplySub(ddouble v, double a, double b) {
            double hi = Math.FusedMultiplyAdd(-a, b, v.hi);
            double lo = v.lo - Math.FusedMultiplyAdd(a, b, hi - v.hi);

            return new ddouble(hi, lo);
        }

        public static ddouble Rcp(ddouble v) {
            double w = (double)v;

            double sign = Math.CopySign(1, w);

            if (w == 0) {
                return Math.CopySign(double.PositiveInfinity, w);
            }
            if (double.IsInfinity(w)) {
                return w > 0d ? 0d : -0d;
            }
            if (double.IsNaN(w)) {
                return NaN;
            }

            int v_exponent = Math.ILogB(w);
            ddouble v_frac = new ddouble(
                Math.ScaleB(v.hi, -v_exponent), 
                Math.ScaleB(v.lo, -v_exponent)
            );

            ddouble a = 1 / (double)v_frac;
            ddouble h = 1 - v_frac * a;

            ddouble squa_h = h * h;
            a *= 1 + (1 + squa_h) * (h + squa_h);
            h = 1 - v_frac * a;

            squa_h = h * h;
            a *= 1 + (1 + squa_h) * (h + squa_h);

            a = new ddouble(
                Math.ScaleB(a.hi, -v_exponent), 
                Math.ScaleB(a.lo, -v_exponent)
            );

            return a;
        }

        public static ddouble operator /(ddouble a, double b) {
            return a * Rcp(b);
        }

        public static ddouble operator /(double a, ddouble b) {
            return a * Rcp(b);
        }

        public static ddouble operator /(ddouble a, ddouble b) {
            return a * Rcp(b);
        }
    }
}
