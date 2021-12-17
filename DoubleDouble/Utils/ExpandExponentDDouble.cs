using System.Diagnostics;

namespace DoubleDouble {
    [DebuggerDisplay("{ToString(),nq}")]
    public struct ExpandExponentDDouble {
        private readonly int exponent;
        private readonly ddouble value;

        private ExpandExponentDDouble(int exponent, ddouble value) {
            this.exponent = exponent;
            this.value = value;
        }

        public static ExpandExponentDDouble operator *(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            (int exp, ddouble value) = ddouble.Frexp(v1.value * v2.value);

            return new(checked(v1.exponent + v2.exponent + exp), value);
        }

        public static ExpandExponentDDouble operator /(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            (int exp, ddouble value) = ddouble.Frexp(v1.value / v2.value);

            return new(checked(v1.exponent - v2.exponent + exp), value);
        }

        public static ExpandExponentDDouble operator +(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            if (v1.exponent >= v2.exponent) {
                (int exp, ddouble value) = ddouble.Frexp(v1.value + ddouble.Ldexp(v2.value, checked(v2.exponent - v1.exponent)));

                return new(checked(v1.exponent + exp), value);
            }
            else {
                (int exp, ddouble value) = ddouble.Frexp(ddouble.Ldexp(v1.value, checked(v1.exponent - v2.exponent)) + v2.value);

                return new(checked(v2.exponent + exp), value);
            }
        }

        public static ExpandExponentDDouble operator -(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            if (v1.exponent >= v2.exponent) {
                (int exp, ddouble value) = ddouble.Frexp(v1.value - ddouble.Ldexp(v2.value, checked(v2.exponent - v1.exponent)));

                return new(checked(v1.exponent + exp), value);
            }
            else {
                (int exp, ddouble value) = ddouble.Frexp(ddouble.Ldexp(v1.value, checked(v1.exponent - v2.exponent)) - v2.value);

                return new(checked(v2.exponent + exp), value);
            }
        }

        public static bool operator ==(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            return v1.exponent == v2.exponent && v1.value == v2.value;
        }

        public static bool operator !=(ExpandExponentDDouble v1, ExpandExponentDDouble v2) {
            return !(v1 == v2);
        }

        public static implicit operator ExpandExponentDDouble(int v) {
            return (ddouble)v;
        }

        public static implicit operator ExpandExponentDDouble(uint v) {
            return (ddouble)v;
        }

        public static implicit operator ExpandExponentDDouble(long v) {
            return (ddouble)v;
        }

        public static implicit operator ExpandExponentDDouble(ulong v) {
            return (ddouble)v;
        }

        public static implicit operator ExpandExponentDDouble(double v) {
            return (ddouble)v;
        }

        public static implicit operator ExpandExponentDDouble(ddouble v) {
            (int exp, ddouble value) = ddouble.Frexp(v);

            return new ExpandExponentDDouble(exp, value);
        }

        public static explicit operator ddouble(ExpandExponentDDouble v) {
            return ddouble.Ldexp(v.value, v.exponent);
        }

        public override string ToString() {
            ddouble exponent_dec = exponent * ddouble.Lg2;
            int exponent_n = (int)ddouble.Floor(exponent_dec);
            ddouble exponent_frac = exponent_dec - exponent_n;

            ddouble dec = value * ddouble.Pow10(exponent_frac);

            string dec_str = dec.ToString();

            if ((dec.Sign >= 0 && (dec_str.IndexOf('.') >= 2 || (dec_str.IndexOf('.') < 0 && dec_str.Length >= 2))) ||
                (dec.Sign < 0 && (dec_str.IndexOf('.') >= 3 || (dec_str.IndexOf('.') < 0 && dec_str.Length >= 3)))) {

                dec /= 10;
                exponent_n++;

                return $"{dec}e{exponent_n}";
            }

            return $"{dec_str}e{exponent_n}";
        }

        public override bool Equals(object obj) {
            return obj is ExpandExponentDDouble v && this == v;
        }

        public override int GetHashCode() {
            return exponent.GetHashCode() ^ value.GetHashCode();
        }
    }
}
