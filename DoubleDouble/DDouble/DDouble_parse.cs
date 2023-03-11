using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DoubleDouble {
    public partial struct ddouble {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Regex parse_regex = new(@"^[\+-]?\d+(\.\d+)?([eE][\+-]?\d+)?$");

        public static implicit operator ddouble(string num) {
            return Parse(num);
        }

        public static ddouble Parse(string s) {
            const int truncate_digits = 33;

            if (string.IsNullOrEmpty(s) || !parse_regex.IsMatch(s)) {
                return FromIrregularString(s);
            }

            int sign = 1;

            if (s[0] == '+' || s[0] == '-') {
                if (s[0] == '-') {
                    sign = -1;
                }

                s = s[1..];
            }

            int exponent_symbol_index = s.Length;

            if (s.Contains('e')) {
                exponent_symbol_index = s.IndexOf('e');
            }
            else if (s.Contains('E')) {
                exponent_symbol_index = s.IndexOf('E');
            }

            string mantissa = s[..exponent_symbol_index].TrimStart('0');

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

            UInt128 mantissa_dec = UInt128.Parse(dec);

            string exponent = (exponent_symbol_index + 1 < s.Length) ? s[(exponent_symbol_index + 1)..] : "0";
            if (!int.TryParse(exponent, NumberStyles.Integer, CultureInfo.InvariantCulture, out int exponent_dec)) {
                throw new FormatException(nameof(s));
            }

            exponent_dec = checked(exponent_dec + point_symbol_index - leading_zeros);

            return FromStringCore(sign, exponent_dec, mantissa_dec, digits);
        }

        public static bool TryParse(string s, out ddouble result) {
            try {
                result = (ddouble)s;
                return true;
            }
            catch (FormatException) {
                result = ddouble.Zero;
                return false;
            }
        }

        internal static ddouble FromStringCore(int sign, int exponent_dec, UInt128 mantissa_dec, int digits) {
            int p = checked(exponent_dec - digits);

            ddouble mantissa = FromUInt128(mantissa_dec);

            if (sign < 0) {
                mantissa = -mantissa;
            }

            if (p == 0) {
                return mantissa;
            }

            ddouble exponent = Consts.Dec.Pow5(p);

            return RoundMantissa(Ldexp(mantissa * exponent, p), keep_bits: 105);
        }

        private static ddouble FromIrregularString(string str) {
            if (str == double.NaN.ToString() || str.ToLower() == "nan") {
                return ddouble.NaN;
            }
            if (str == double.PositiveInfinity.ToString() || str.ToLower() == "inf" || str.ToLower() == "+inf") {
                return ddouble.PositiveInfinity;
            }
            if (str == double.NegativeInfinity.ToString() || str.ToLower() == "-inf") {
                return ddouble.NegativeInfinity;
            }

            throw new FormatException($"Invalid numeric string. : {str}");
        }
    }
}
