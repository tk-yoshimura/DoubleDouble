using System;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static int DecimalDigits => 32;

//        internal (int sign, Int64 exponent_dec, BigInteger mantissa_dec) ToStringCore(int digits) {
//            const int presicion = 2;

//            if (digits > DecimalDigits) {
//                throw new ArgumentOutOfRangeException(nameof(digits));
//            }

//            if (IsZero(this)) {
//                return ((int)Math.CopySign(1, hi), 0, 0);
//            }

//            int exponent = Math.ILogB(hi);

//            ddouble exponent_log10 = Consts.Lg2 * exponent;
//            ddouble exponent_int = Floor(exponent_log10);
//            int exponent_dec = (int)exponent_int;

//            ddouble exponent_frac = Ldexp(Pow(5, -exponent_dec), checked(exponent - exponent_dec));

//#if DEBUG
//            Debug<ArithmeticException>.Assert(exponent_frac >= 1 && exponent_frac < 10);
//#endif


//            Accumulator<N> mantissa_dec = new(mantissa);

//            mantissa_dec = Accumulator<N>.MulShift(mantissa_dec, Accumulator<N>.Decimal(digits + presicion));
//            mantissa_dec = Accumulator<N>.MulShift(mantissa_dec, new Accumulator<N>(exponent_frac.mantissa, (int)exponent_frac.Exponent));

//            if (mantissa_dec >= Accumulator<N>.Decimal(digits + presicion + 1)) {
//                exponent_dec = checked(exponent_dec + 1);
//                mantissa_dec = Accumulator<N>.RoundDiv(mantissa_dec, Accumulator<N>.Decimal(presicion + 1));
//            }
//            else {
//                mantissa_dec = Accumulator<N>.RoundDiv(mantissa_dec, Accumulator<N>.Decimal(presicion));
//            }
//            if (mantissa_dec == Accumulator<N>.Decimal(digits + 1)) {
//                exponent_dec = checked(exponent_dec + 1);
//                mantissa_dec = Accumulator<N>.Decimal(digits);
//            }

//#if DEBUG
//            Debug<ArithmeticException>.Assert(mantissa_dec < Accumulator<N>.Decimal(digits + 1));
//#endif

//            return (Sign, exponent_dec, mantissa_dec);
//        }
    }
}
