using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace DoubleDouble {
    public partial struct ddouble {
        private static readonly Regex parse_regex = new(@"^[\+-]?\d+(\.\d+)?([eE][\+-]?\d+)?$");

        public static ddouble Parse(string s) {
            return (ddouble)s;
        }

        public static bool TryParse(string s, out ddouble result) {
            try {
                result = (ddouble)s;
                return true;
            }
            catch (FormatException) {
                result = 0;
                return false;
            }
        }

        public static implicit operator ddouble(string num) {
            const int truncate_digits = 33;

            if (!parse_regex.IsMatch(num)) {
                throw new FormatException();
            }

            int sign = 1;

            if (num[0] == '+' || num[0] == '-') {
                if (num[0] == '-') {
                    sign = -1;
                }

                num = num[1..];
            }

            int exponent_symbol_index = num.Length;

            if (num.Contains('e')) {
                exponent_symbol_index = num.IndexOf('e');
            }
            else if (num.Contains('E')) {
                exponent_symbol_index = num.IndexOf('E');
            }

            string mantissa = num[..exponent_symbol_index].TrimStart('0');

            if (string.IsNullOrEmpty(mantissa)) {
                return sign == +1 ? 0 : -1;
            }

            int point_symbol_index = mantissa.Contains('.') ? (mantissa.IndexOf('.') - 1) : (mantissa.Length - 1);

            string dec = mantissa.Replace(".", string.Empty);
            string dec_trim = dec.TrimStart('0');
            if (dec_trim.Length == 0) {
                dec_trim = "0";
            }

            int leading_zeros = dec.Length - dec_trim.Length;
            dec = dec_trim;

            if (dec.Length > truncate_digits) {
                dec = dec[..truncate_digits];
            }
            int digits = dec.Length - 1;

            BigInteger mantissa_dec = BigInteger.Parse(dec);

            string exponent = (exponent_symbol_index + 1 < num.Length) ? num[(exponent_symbol_index + 1)..] : "0";
            if (!int.TryParse(exponent, NumberStyles.Integer, CultureInfo.InvariantCulture, out int exponent_dec)) {
                throw new FormatException(nameof(num));
            }

            exponent_dec = checked(exponent_dec + point_symbol_index - leading_zeros);

            return FromStringCore(sign, exponent_dec, mantissa_dec, digits);
        }

        internal static ddouble FromStringCore(int sign, int exponent_dec, BigInteger mantissa_dec, int digits) {
#if DEBUG
            Debug<ArithmeticException>.Assert(mantissa_dec.Sign >= 0);
#endif

            int p = checked(exponent_dec - digits);

            ddouble mantissa = (sign >= 0 ? mantissa_dec : BigInteger.Negate(mantissa_dec));

            if (p == 0) {
                return mantissa;
            }

            ddouble exponent = Consts.Dec.Pow5(p);

            return RoundMantissa(Ldexp(mantissa * exponent, p), keep_bits: 105);
        }
    }
}
