using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble :
        INumber<ddouble>,
        ISignedNumber<ddouble>,

        IAdditiveIdentity<ddouble, ddouble>,
        IMultiplicativeIdentity<ddouble, ddouble>,

        IAdditionOperators<ddouble, ddouble, ddouble>,
        ISubtractionOperators<ddouble, ddouble, ddouble>,
        IMultiplyOperators<ddouble, ddouble, ddouble>,
        IDivisionOperators<ddouble, ddouble, ddouble>,
        IModulusOperators<ddouble, ddouble, ddouble>,

        IUnaryPlusOperators<ddouble, ddouble>,
        IUnaryNegationOperators<ddouble, ddouble>,

        IEquatable<ddouble>,
        IEqualityOperators<ddouble, ddouble, bool>,
        IEqualityComparer<ddouble>,
        IComparisonOperators<ddouble, ddouble, bool>,

        IMinMaxValue<ddouble>,
        IPowerFunctions<ddouble>,
        IRootFunctions<ddouble>,
        IExponentialFunctions<ddouble>,
        ITrigonometricFunctions<ddouble>,
        IHyperbolicFunctions<ddouble>,
        ILogarithmicFunctions<ddouble>,
        IFloatingPointConstants<ddouble> {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble AdditiveIdentity => ddouble.Zero;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble MultiplicativeIdentity => ddouble.One;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static ddouble NegativeOne => MinusOne;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static int INumberBase<ddouble>.Radix => 2;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static ddouble IFloatingPointConstants<ddouble>.Pi => ddouble.Pi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static ddouble IFloatingPointConstants<ddouble>.E => ddouble.E;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static ddouble IFloatingPointConstants<ddouble>.Tau => ddouble.Ldexp(ddouble.Pi, 1);

        public static bool IsCanonical(ddouble value) => IsFinite(value);

        public static bool IsRealNumber(ddouble value) => !IsNaN(value);
        public static bool IsImaginaryNumber(ddouble value) => false;
        public static bool IsComplexNumber(ddouble value) => false;

        public static bool IsPositive(ddouble value) => double.IsPositive(value.Hi);
        public static bool IsNegative(ddouble value) => double.IsNegative(value.Hi);

        public static bool IsInteger(ddouble value) => Truncate(value) == value;
        public static bool IsEvenInteger(ddouble value) => IsInteger(value) && (Abs(value % 2d) == 0d);
        public static bool IsOddInteger(ddouble value) => IsInteger(value) && (Abs(value % 2d) == 1d);

        public static ddouble MaxMagnitude(ddouble x, ddouble y) => (Abs(x) > Abs(y) || IsNaN(x)) ? x : y;
        public static ddouble MaxMagnitudeNumber(ddouble x, ddouble y) => (Abs(x) > Abs(y) || IsNaN(y)) ? x : y;
        public static ddouble MinMagnitude(ddouble x, ddouble y) => (Abs(x) < Abs(y) || IsNaN(x)) ? x : y;
        public static ddouble MinMagnitudeNumber(ddouble x, ddouble y) => (Abs(x) < Abs(y) || IsNaN(y)) ? x : y;

        public static ddouble Exp10(ddouble x) => Pow10(x);
        public static ddouble Exp2(ddouble x) => Pow2(x);
        public static ddouble Exp10M1(ddouble x) => ILogB(x) < -3 ? Pow2m1(x * Lb10) : Pow10(x) - 1d;
        public static ddouble Exp2M1(ddouble x) => Pow2m1(x);


        public static (ddouble Sin, ddouble Cos) SinCos(ddouble x) => (Sin(x), Cos(x));
        public static (ddouble SinPi, ddouble CosPi) SinCosPi(ddouble x) => (SinPi(x), CosPi(x));

        public static ddouble LogP1(ddouble x) => Log1p(x);
        public static ddouble Log2P1(ddouble x) => ILogB(x) < -1 ? Log1p(x) * LbE : Log2(1d + x);
        public static ddouble Log10P1(ddouble x) => ILogB(x) < -1 ? Log1p(x) * LbE * Lg2 : Log10(1d + x);

        public static ddouble Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s);
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ddouble result) => TryParse(s, out result);
        public static ddouble Parse(string s, IFormatProvider provider) => Parse(s);
        public static bool TryParse(string s, IFormatProvider provider, out ddouble result) => TryParse(s, out result);
        public static ddouble Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s.ToString());
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out ddouble result) => TryParse(s.ToString(), out result);
        public static ddouble Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s.ToString());
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out ddouble result) => TryParse(s.ToString(), out result);

        public static bool TryConvertFromChecked<TOther>(TOther value, out ddouble result) where TOther : INumberBase<TOther> {
            if (value is double vd) {
                result = vd;
                return true;
            }
            if (value is float vf) {
                result = vf;
                return true;
            }
            if (value is long vl) {
                result = vl;
                return true;
            }
            if (value is int vi) {
                result = vi;
                return true;
            }
            if (value is decimal vdec) {
                result = vdec;
                return true;
            }
            if (value is BigInteger vbint) {
                result = vbint;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, out ddouble result) where TOther : INumberBase<TOther> {
            return TryConvertFromChecked<TOther>(value, out result);
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, out ddouble result) where TOther : INumberBase<TOther> {
            return TryConvertFromChecked<TOther>(value, out result);
        }

        public static bool TryConvertToChecked<TOther>(ddouble value, out TOther result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(double)) {
                result = (TOther)(object)(double)value;
                return true;
            }
            if (typeof(TOther) == typeof(float)) {
                result = (TOther)(object)(float)value;
                return true;
            }
            if (typeof(TOther) == typeof(long)) {
                result = (TOther)(object)(long)value;
                return true;
            }
            if (typeof(TOther) == typeof(int)) {
                result = (TOther)(object)(int)value;
                return true;
            }
            if (typeof(TOther) == typeof(decimal)) {
                result = (TOther)(object)(decimal)value;
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryConvertToSaturating<TOther>(ddouble value, out TOther result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(double)) {
                result = (TOther)(object)(double)value;
                return true;
            }
            if (typeof(TOther) == typeof(float)) {
                result = (TOther)(object)(float)value;
                return true;
            }
            if (typeof(TOther) == typeof(long)) {
                result = (TOther)(object)(long)ddouble.Clamp(value, long.MinValue, long.MaxValue);
                return true;
            }
            if (typeof(TOther) == typeof(int)) {
                result = (TOther)(object)(int)ddouble.Clamp(value, int.MinValue, int.MaxValue);
                return true;
            }
            if (typeof(TOther) == typeof(decimal)) {
                result = (TOther)(object)(decimal)ddouble.Clamp(value, decimal.MinValue, decimal.MaxValue);
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryConvertToTruncating<TOther>(ddouble value, out TOther result) where TOther : INumberBase<TOther> {
            return TryConvertToSaturating<TOther>(value, out result);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) {
            string str = format.IsEmpty ? ToString() : ToString(format.ToString());

            if (str.TryCopyTo(destination)) {
                charsWritten = str.Length;
                return true;
            }
            else {
                charsWritten = 0;
                return false;
            }
        }

        public bool Equals(ddouble x, ddouble y) => (x == y) || (IsNaN(x) && IsNaN(y));
        public bool Equals(ddouble other) => (this == other) || (IsNaN(this) && IsNaN(other));
        public override bool Equals(object obj) => obj is ddouble v && Equals(v);

        public int GetHashCode([DisallowNull] ddouble obj) => obj.GetHashCode();
    }
}