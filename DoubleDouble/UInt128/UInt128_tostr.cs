using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DoubleDouble {
    internal readonly partial struct UInt128 {

        public override string ToString() {
            UInt32 carry, dec0, dec1, dec2, dec3, dec4;

            unchecked {
                (dec1, dec0) = UIntUtil.DecimalUnpack(e3);

                (carry, dec0) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec0, e2));
                (dec2, dec1) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec1, carry));

                (carry, dec0) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec0, e1));
                (carry, dec1) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec1, carry));
                (dec3, dec2) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec2, carry));

                (carry, dec0) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec0, e0));
                (carry, dec1) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec1, carry));
                (carry, dec2) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec2, carry));
                (dec4, dec3) = UIntUtil.DecimalUnpack(UIntUtil.Pack(dec3, carry));
            }

            string str = $"{dec4:D9}{dec3:D9}{dec2:D9}{dec1:D9}{dec0:D9}".TrimStart('0');
            str = (str != string.Empty) ? str : "0";

            return str;
        }

        public string ToString([AllowNull] string format, [AllowNull] IFormatProvider formatProvider) {
            if (string.IsNullOrWhiteSpace(format)) {
                return ToString();
            }

            format = format.Trim();

            int digits = 0;
            if (format.Length > 1) {
                if (!int.TryParse(format[1..], NumberStyles.Integer, CultureInfo.InvariantCulture, out digits)) {
                    throw new FormatException(format);
                }
            }

            if (format[0] == 'd' || format[0] == 'D') {
                string dec = ToString();
                if (dec.Length < digits) {
                    dec = $"{new string('0', digits - dec.Length)}{dec}";
                }

                return dec;
            }

            if (format[0] == 'x' || format[0] == 'X') {
                string hex = (format[0] == 'x')
                    ? $"{e3:x8}{e2:x8}{e1:x8}{e0:x8}".TrimStart('0')
                    : $"{e3:X8}{e2:X8}{e1:X8}{e0:X8}".TrimStart('0');

                if (hex.Length < digits) {
                    hex = $"{new string('0', digits - hex.Length)}{hex}";
                }

                if (hex.Length < 1) {
                    hex = "0";
                }

                return hex;
            }

            throw new FormatException(format);
        }

        public string ToString(string format) {
            return ToString(format, null);
        }
    }
}