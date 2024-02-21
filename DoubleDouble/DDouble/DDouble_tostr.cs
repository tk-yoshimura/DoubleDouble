using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DoubleDouble {
    public partial struct ddouble : IFormattable {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static int DecimalDigits => 30;

        public override string ToString() {
            if (!IsFinite(this)) {
                if (IsNaN(this)) {
                    return double.NaN.ToString();
                }
                return IsPositive(this) ? double.PositiveInfinity.ToString() : double.NegativeInfinity.ToString();
            }

            (int sign, int exponent_dec, UInt128 mantissa_dec) = ToStringCore(DecimalDigits);

            if (mantissa_dec.IsZero) {
                return IsPositive(this) ? "0" : "-0";
            }

            string num = mantissa_dec.ToString().TrimEnd('0');

            if (exponent_dec >= 8 || exponent_dec <= -4 || exponent_dec == 0) {
                if (num.Length >= 2) {
                    num = num.Insert(1, ".");
                }

                if (exponent_dec != 0) {
                    return $"{(IsPositive(this) ? "" : "-")}{num}e{exponent_dec}";
                }
                else {
                    return $"{(IsPositive(this) ? "" : "-")}{num}";
                }
            }
            else if (exponent_dec < 0) {
                num = new string('0', checked((int)-exponent_dec)) + num;
                num = num.Insert(1, ".");

                return $"{(IsPositive(this) ? "" : "-")}{num}";
            }
            else {
                if (num.Length < checked((int)exponent_dec + 1)) {
                    num += new string('0', checked((int)exponent_dec - num.Length + 1));
                }
                else if (num.Length > checked((int)exponent_dec + 1)) {
                    num = num.Insert(checked((int)exponent_dec + 1), ".");
                }

                return $"{(IsPositive(this) ? "" : "-")}{num}";
            }
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            format = format.Trim();

            if (format.Length < 2 || (format[0] != 'e' && format[0] != 'E')) {
                throw new FormatException(format);
            }

            if (!(int.TryParse(format[1..], NumberStyles.Integer, CultureInfo.InvariantCulture, out int digits)) || digits <= 1) {
                throw new FormatException(format);
            }

            if (!IsFinite(this)) {
                if (IsNaN(this)) {
                    return double.NaN.ToString();
                }
                return IsPositive(this) ? double.PositiveInfinity.ToString() : double.NegativeInfinity.ToString();
            }

            (int sign, int exponent_dec, UInt128 mantissa_dec) = ToStringCore(digits);

            if (mantissa_dec.IsZero) {
                return (IsPositive(this) ? "0." : "-0.") + new string('0', digits) + $"{format[0]}0";
            }

            string num = mantissa_dec.ToString();
            num = num.Insert(1, ".");

            return $"{(IsPositive(this) ? "" : "-")}{num}{format[0]}{exponent_dec}";
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        internal (int sign, int exponent_dec, UInt128 mantissa_dec) ToStringCore(int digits) {
            const int presicion = 4;

            if (digits > DecimalDigits) {
                throw new ArgumentOutOfRangeException(
                    nameof(digits),
                    $"Specifying more than the significant digits (={DecimalDigits}) of ddouble is invalid."
                );
            }

            if (IsZero(this)) {
                return (IsPositive(this) ? 1 : -1, 0, 0);
            }

            (int sign, int exponent, UInt128 mantissa, _) = FloatSplitter.Split(this);

            ddouble exponent_log10 = Lg2 * exponent;
            ddouble exponent_int = Floor(exponent_log10);
            int exponent_dec = (int)exponent_int;

            ddouble log10_frac = Ldexp(Consts.Dec.Pow5(-exponent_dec), checked(exponent - exponent_dec));
            (_, int exponent_frac, UInt128 mantissa_frac, _) = FloatSplitter.Split(log10_frac);

            Debug.Assert(log10_frac >= 1d && log10_frac < 10d, "invalid frac");

            mantissa = UInt128.MulShift(mantissa, Consts.Dec.Decimals[digits + presicion], FloatSplitter.MantissaBits * 2);
            mantissa = UInt128.MulShift(mantissa, mantissa_frac, FloatSplitter.MantissaBits * 2 - exponent_frac);

            int mantissa_length = mantissa.ToString().Length;

            if (mantissa_length > (digits + 1)) {
                int trunc_digits = mantissa_length - (digits + 1);
                exponent_dec = checked(exponent_dec + trunc_digits - presicion);
                mantissa = UInt128.RoundDiv(mantissa, Consts.Dec.Decimals[trunc_digits]);
            }
            if (mantissa == Consts.Dec.Decimals[digits + 1]) {
                exponent_dec = checked(exponent_dec + 1);
                mantissa = Consts.Dec.Decimals[digits];
            }

            Debug.Assert(mantissa < Consts.Dec.Decimals[digits + 1], "overflow");
            Debug.Assert(mantissa.ToString().Length == (digits + 1), "mismatch length");

            return (sign, exponent_dec, mantissa);
        }

        internal static partial class Consts {

            public static class Dec {

                static readonly Dictionary<int, UInt128> decimals = new();
                public static readonly ReadOnlyCollection<ddouble> pow5s, invpow5s;

                public static readonly ReadOnlyCollection<UInt128> Decimals;

                static Dec() {
                    Decimals = GenerateDecimals();
                    (pow5s, invpow5s) = GeneratePow5NTable();
                }

                public static ddouble Pow5(int n) {
                    return n >= 0 ?
                        pow5s[int.Min(n, pow5s.Count - 1)] :
                        invpow5s[int.Min(-n, invpow5s.Count - 1)];
                }

                static ReadOnlyCollection<UInt128> GenerateDecimals() {
                    List<UInt128> decimals = [];
                    UInt128 num = 1u;
                    for (int i = 0; i < 38; i++) {
                        decimals.Add(num);
                        num *= 10u;
                    }
                    decimals.Add(num);

                    return decimals.AsReadOnly();
                }

                public static (ReadOnlyCollection<ddouble>, ReadOnlyCollection<ddouble>) GeneratePow5NTable() {
                    ddouble[] pow5s = new ddouble[9];
                    pow5s[0] = 5d;
                    for (int i = 1; i < pow5s.Length; i++) {
                        pow5s[i] = pow5s[i - 1] * pow5s[i - 1];
                    }

                    ddouble[] table = new ddouble[443], table_inv = new ddouble[443];
                    for (int i = 0; i < table.Length; i++) {
                        int n = i;
                        ddouble y = 1d;

                        for (int j = 0; j < pow5s.Length && n > 0; j++, n >>= 1) {
                            if ((n & 1) == 1) {
                                y *= pow5s[j];
                            }
                        }

                        table[i] = y;
                        table_inv[i] = 1d / y;
                    }

                    return (Array.AsReadOnly(table), Array.AsReadOnly(table_inv));
                }
            }
        }
    }
}
