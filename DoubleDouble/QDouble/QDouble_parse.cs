﻿using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace DoubleDouble {
    internal partial struct qdouble {
        private static readonly Regex parse_regex = new(@"^[\+-]?\d+(\.\d+)?([eE][\+-]?\d+)?$");

        public static qdouble Parse(string s) {
            return (qdouble)s;
        }

        public static bool TryParse(string s, out qdouble result) {
            try {
                result = (qdouble)s;
                return true;
            }
            catch (FormatException) {
                result = 0;
                return false;
            }
        }

        public static implicit operator qdouble(string num) {
            const int truncate_digits = 80;

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

            string mantissa_withoutpoint = mantissa.Replace(".", string.Empty);

            if (mantissa_withoutpoint.Length > truncate_digits) {
                mantissa_withoutpoint = mantissa_withoutpoint[..truncate_digits];
            }

            BigInteger mantissa_dec = BigInteger.Parse(mantissa_withoutpoint);

            string exponent = (exponent_symbol_index + 1 < num.Length) ? num[(exponent_symbol_index + 1)..] : "0";
            if (!int.TryParse(exponent, NumberStyles.Integer, CultureInfo.InvariantCulture, out int exponent_dec)) {
                throw new FormatException(nameof(num));
            }

            exponent_dec = checked(exponent_dec + point_symbol_index);

            int digits = mantissa_withoutpoint.Length - 1;

            return FromStringCore(sign, exponent_dec, mantissa_dec, digits);
        }

        internal static qdouble FromStringCore(int sign, int exponent_dec, BigInteger mantissa_dec, int digits) {
#if DEBUG
            Debug<ArithmeticException>.Assert(mantissa_dec.Sign >= 0);
#endif

            int p = checked(exponent_dec - digits);

            qdouble mantissa = (sign >= 0 ? mantissa_dec : BigInteger.Negate(mantissa_dec));

            if (p == 0) {
                return mantissa;
            }

            qdouble exponent = Consts.Dec.Pow5(p);

            return Ldexp(mantissa * exponent, p);
        }
    }
}