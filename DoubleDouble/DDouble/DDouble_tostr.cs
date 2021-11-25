using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static int DecimalDigits => 30;

        public override string ToString() {
            if (IsNaN(this)) {
                return double.NaN.ToString();
            }
            if (!IsFinite(this)) {
                return (Sign > 0) ? double.PositiveInfinity.ToString() : double.NegativeInfinity.ToString();
            }

            (int sign, int exponent_dec, BigInteger mantissa_dec) = ToStringCore(DecimalDigits);

            if (mantissa_dec.IsZero) {
                return (Sign > 0) ? "0" : "-0";
            }

            string num = mantissa_dec.ToString().TrimEnd('0');

            if (exponent_dec >= 8 || exponent_dec <= -4 || exponent_dec == 0) {
                if (num.Length >= 2) {
                    num = num.Insert(1, ".");
                }

                if (exponent_dec != 0) {
                    return $"{((Sign > 0) ? "" : "-")}{num}e{exponent_dec}";
                }
                else {
                    return $"{((Sign > 0) ? "" : "-")}{num}";
                }
            }
            else if (exponent_dec < 0) {
                num = new string('0', checked((int)-exponent_dec)) + num;
                num = num.Insert(1, ".");

                return $"{((Sign > 0) ? "" : "-")}{num}";
            }
            else {
                if (num.Length < checked((int)exponent_dec + 1)) {
                    num += new string('0', checked((int)exponent_dec - num.Length + 1));
                }
                else if (num.Length > checked((int)exponent_dec + 1)) {
                    num = num.Insert(checked((int)exponent_dec + 1), ".");
                }

                return $"{((Sign > 0) ? "" : "-")}{num}";
            }
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (format is null) {
                return ToString();
            }

            format = format.Trim();

            if (format.Length < 2 || (format[0] != 'e' && format[0] != 'E')) {
                throw new FormatException(format);
            }

            if (!(int.TryParse(format[1..], NumberStyles.Integer, CultureInfo.InvariantCulture, out int digits)) || digits <= 1) {
                throw new FormatException(format);
            }

            if (IsNaN(this)) {
                return double.NaN.ToString();
            }
            if (!IsFinite(this)) {
                return (Sign > 0) ? double.PositiveInfinity.ToString() : double.NegativeInfinity.ToString();
            }

            (int sign, int exponent_dec, BigInteger mantissa_dec) = ToStringCore(digits);

            if (mantissa_dec.IsZero) {
                return ((Sign > 0) ? "0." : "-0.") + new string('0', digits) + $"{format[0]}0";
            }

            string num = mantissa_dec.ToString();
            num = num.Insert(1, ".");

            return $"{((Sign > 0) ? "" : "-")}{num}{format[0]}{exponent_dec}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        internal (int sign, int exponent_dec, BigInteger mantissa_dec) ToStringCore(int digits) {
            const int presicion = 6;

            if (digits > DecimalDigits) {
                throw new ArgumentOutOfRangeException(nameof(digits));
            }

            if (IsZero(this)) {
                return ((int)Math.CopySign(1, hi), 0, 0);
            }

            (int sign, int exponent, BigInteger mantissa, _) = FloatConverter.Split(this);

            ddouble exponent_log10 = Consts.Log.Lg2 * exponent;
            ddouble exponent_int = Floor(exponent_log10);
            int exponent_dec = (int)exponent_int;

            ddouble log10_frac = Ldexp(Pow(5, -exponent_dec), checked(exponent - exponent_dec));
            (_, int exponent_frac, BigInteger mantissa_frac, _) = FloatConverter.Split(log10_frac);

#if DEBUG
            Debug<ArithmeticException>.Assert(log10_frac >= 1 && log10_frac < 10);
#endif

            mantissa = (mantissa * Tostr.Decimal(digits + presicion)) >> (FloatConverter.MantissaBits * 2);
            mantissa = (mantissa * (mantissa_frac << exponent_frac)) >> (FloatConverter.MantissaBits * 2);

            int mantissa_length = mantissa.ToString().Length;

            if (mantissa_length > (digits + 1)) {
                int trunc_digits = mantissa_length - (digits + 1);
                exponent_dec = checked(exponent_dec + trunc_digits - presicion);
                mantissa = Tostr.RoundDiv(mantissa, Tostr.Decimal(trunc_digits));
            }
            if (mantissa == Tostr.Decimal(digits + 1)) {
                exponent_dec = checked(exponent_dec + 1);
                mantissa = Tostr.Decimal(digits);
            }

#if DEBUG
            Debug<ArithmeticException>.Assert(mantissa < Tostr.Decimal(digits + 1), "overflow");
            Debug<ArithmeticException>.Assert(mantissa.ToString().Length == (digits + 1), "mismatch length");
#endif

            return (sign, exponent_dec, mantissa);
        }

        private static class Tostr {
            static readonly Dictionary<int, BigInteger> decimals = new();

            public static BigInteger Decimal(int n) {
                if (!decimals.ContainsKey(n)) {
                    BigInteger num = 1;
                    for (int i = 0; i < n; i++) {
                        num *= 10;
                    }

                    decimals.Add(n, num);
                }

                return decimals[n];
            }

            public static BigInteger RoundDiv(BigInteger x, BigInteger y) {
                BigInteger n = x / y, r = x - y * n;

                if (r >= y / 2) {
                    n += 1;
                }

                return n;
            }
        }
    }
}
