using DoubleDouble.Utils;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble BesselJ(ddouble nu, ddouble x) {
            BesselUtil.CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                if (nu == 0d) {
                    return 1d;
                }
                if (BesselUtil.NearlyInteger(nu, out _) || nu > 0d) {
                    return 0d;
                }
                return (((int)double.Floor(nu.Hi) & 1) == 0) ? NegativeInfinity : PositiveInfinity;
            }

            if (x <= 2d) {
                return BesselNearZero.BesselJ(nu, x);
            }
            if (x <= 40.5d) {
                return BesselMillerBackward.BesselJ(nu, x);
            }

            return BesselLimit.BesselJ(nu, x);
        }

        public static ddouble BesselJ(int n, ddouble x) {
            BesselUtil.CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                return (n == 0) ? 1d : 0d;
            }

            if (x <= 2d) {
                return BesselNearZero.BesselJ(n, x);
            }
            if (x <= 40.5d) {
                return BesselMillerBackward.BesselJ(n, x);
            }

            return BesselLimit.BesselJ(n, x);
        }

        public static ddouble BesselY(ddouble nu, ddouble x) {
            BesselUtil.CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                if (nu > 0d) {
                    return NegativeInfinity;
                }
                if (nu - Floor(nu) == Point5) {
                    return 0d;
                }
                int n = (int)(Floor(nu + Point5));
                return ((n & 1) == 0) ? NegativeInfinity : PositiveInfinity;
            }

            ddouble alpha = Round(nu) - nu;

            if (nu < 0d && Abs((nu - Floor(nu)) - Point5) < 0.0625) {
                if (x <= 4d - nu) {
                    return BesselNearZero.BesselY(nu, x);
                }
            }
            if (x <= 2d) {
                if (BesselUtil.NearlyInteger(nu, out _) || Abs(alpha) >= BesselUtil.InterpolationThreshold) {
                    return BesselNearZero.BesselY(nu, x);
                }
                else {
                    return BesselInterpolate.BesselY(nu, x);
                }
            }

            if (x <= 40.5d) {
                if (!BesselUtil.NearlyInteger(nu, out _) && (Ceiling(nu) - nu) < BesselUtil.InterpolationThreshold) {
                    return BesselInterpolate.BesselY(nu, x);
                }

                return BesselMillerBackward.BesselY(nu, x);
            }

            return BesselLimit.BesselY(nu, x);
        }

        public static ddouble BesselY(int n, ddouble x) {
            BesselUtil.CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                if (n > 0) {
                    return NegativeInfinity;
                }
                return ((n & 1) == 0) ? NegativeInfinity : PositiveInfinity;
            }

            if (x <= 2d) {
                return BesselNearZero.BesselY(n, x);
            }
            if (x <= 40.5d) {
                return BesselMillerBackward.BesselY(n, x);
            }

            return BesselLimit.BesselY(n, x);
        }

        public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
            BesselUtil.CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                if (nu == 0d) {
                    return 1d;
                }
                if (BesselUtil.NearlyInteger(nu, out _) || nu > 0d) {
                    return 0d;
                }
                return (((int)double.Floor(nu.Hi) & 1) == 0) ? NegativeInfinity : PositiveInfinity;
            }

            if (x <= 2d) {
                return BesselNearZero.BesselI(nu, x, scale);
            }
            if (x <= 40d) {
                return BesselMillerBackward.BesselI(nu, x, scale);
            }

            return BesselLimit.BesselI(nu, x, scale);
        }

        public static ddouble BesselI(int n, ddouble x, bool scale = false) {
            BesselUtil.CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                return (n == 0) ? 1d : 0d;
            }

            if (x <= 2d) {
                return BesselNearZero.BesselI(n, x, scale);
            }
            if (x <= 40d) {
                return BesselMillerBackward.BesselI(n, x, scale);
            }

            return BesselLimit.BesselI(n, x, scale);
        }

        public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
            BesselUtil.CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                return PositiveInfinity;
            }

            nu = Abs(nu);

            ddouble alpha = Round(nu) - nu;

            if (x <= 2d) {
                if (BesselUtil.NearlyInteger(nu, out _) || Abs(alpha) >= BesselUtil.InterpolationThreshold) {
                    return BesselNearZero.BesselK(nu, x, scale);
                }
                else {
                    return BesselInterpolate.BesselK(nu, x, scale);
                }
            }

            if (x <= 35d) {
                return BesselYoshidaPade.BesselK(nu, x, scale);
            }

            return BesselLimit.BesselK(nu, x, scale);
        }

        public static ddouble BesselK(int n, ddouble x, bool scale = false) {
            BesselUtil.CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= BesselUtil.Eps) {
                return PositiveInfinity;
            }

            n = int.Abs(n);

            if (x <= 2d) {
                return BesselNearZero.BesselK(n, x, scale);
            }
            if (x <= 35d) {
                return BesselYoshidaPade.BesselK(n, x, scale);
            }

            return BesselLimit.BesselK(n, x, scale);
        }

        private static class BesselUtil {
            public static readonly double Eps = double.ScaleB(1, -96);
            public static readonly double InterpolationThreshold = double.ScaleB(1, -25);
            public static readonly double MillerBwdBesselYEps = double.ScaleB(1, -30);

            public const int MaxN = 16;

            public static void CheckNu(ddouble nu) {
                if (!(Abs(nu) <= (double)MaxN)) {
                    throw new ArgumentOutOfRangeException(
                        nameof(nu),
                        $"In the calculation of the Bessel function, nu with an absolute value greater than {MaxN} is not supported."
                    );
                }
            }

            public static void CheckN(int n) {
                if (n < -MaxN || n > MaxN) {
                    throw new ArgumentOutOfRangeException(
                        nameof(n),
                        $"In the calculation of the Bessel function, n with an absolute value greater than {MaxN} is not supported."
                    );
                }
            }

            public static bool NearlyInteger(ddouble nu, out int n) {
                n = (int)Round(nu);

                return Abs(nu - n) < Eps;
            }
        }

        private static class BesselNearZero {
            private static Dictionary<ddouble, DoubleFactDenomTable> dfactdenom_coef_table = new();
            private static Dictionary<ddouble, X2DenomTable> x2denom_coef_table = new();
            private static Dictionary<ddouble, GammaDenomTable> gammadenom_coef_table = new();
            private static Dictionary<ddouble, GammaTable> gamma_coef_table = new();
            private static Dictionary<ddouble, GammaPNTable> gammapn_coef_table = new();
            private static YCoefTable y_coef_table = new();
            private static Y0CoefTable y0_coef_table = new();
            private static Y1CoefTable y1_coef_table = new();
            private static KCoefTable k_coef_table = new();
            private static K0CoefTable k0_coef_table = new();
            private static K1CoefTable k1_coef_table = new();

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                if (IsNegative(nu) && BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselJ(-nu, x);

                    return ((n & 1) == 0) ? y : -y;
                }
                else {
                    ddouble y = BesselJIKernel(nu, x, sign_switch: true, terms: 9);

                    return y;
                }
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                if (BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselYKernel(n, x, terms: 9);

                    return y;
                }
                else if (nu < 0d && Abs((nu - Floor(nu)) - Point5) < 0.0625) {
                    ddouble y = BesselYKernel(nu, x, terms: 32);

                    return y;
                }
                else {
                    ddouble y = BesselYKernel(nu, x, terms: 10);

                    return y;
                }
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                if (IsNegative(nu) && BesselUtil.NearlyInteger(nu, out _)) {
                    ddouble y = BesselI(-nu, x);

                    if (scale) {
                        y *= Exp(-x);
                    }

                    return y;
                }
                else {
                    ddouble y = BesselJIKernel(nu, x, sign_switch: false, terms: 10);

                    if (scale) {
                        y *= Exp(-x);
                    }

                    return y;
                }
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
                if (BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselKKernel(n, x, terms: 20);

                    if (scale) {
                        y *= Exp(x);
                    }

                    return y;
                }
                else {
                    ddouble y = BesselKKernel(nu, x, terms: 20);

                    if (scale) {
                        y *= Exp(x);
                    }

                    return y;
                }
            }

            private static ddouble BesselJIKernel(ddouble nu, ddouble x, bool sign_switch, int terms) {
                if (!dfactdenom_coef_table.ContainsKey(nu)) {
                    dfactdenom_coef_table.Add(nu, new DoubleFactDenomTable(nu));
                }
                if (!x2denom_coef_table.ContainsKey(nu)) {
                    x2denom_coef_table.Add(nu, new X2DenomTable(nu));
                }

                DoubleFactDenomTable r = dfactdenom_coef_table[nu];
                X2DenomTable d = x2denom_coef_table[nu];

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = 0d, u = Pow(Ldexp(x, -1), nu);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble w = x2 * d[k];
                    ddouble dc = u * r[k] * (sign_switch ? (1d - w) : (1d + w));

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x4;
                }

                return c;
            }

            private static ddouble BesselYKernel(ddouble nu, ddouble x, int terms) {
                if (!gamma_coef_table.ContainsKey(nu)) {
                    gamma_coef_table.Add(nu, new GammaTable(nu));
                }
                if (!gammapn_coef_table.ContainsKey(nu)) {
                    gammapn_coef_table.Add(nu, new GammaPNTable(nu));
                }

                YCoefTable r = y_coef_table;
                GammaTable g = gamma_coef_table[nu];
                GammaPNTable gpn = gammapn_coef_table[nu];

                ddouble cos = CosPI(nu), sin = SinPI(nu);
                ddouble p = Pow(x, Ldexp(nu, 1)) * cos, s = Ldexp(Pow(Ldexp(x, 1), nu), 2);

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = 0d, u = 1d / sin;

                for (int k = 0, t = 1, conv_times = 0; k <= terms && conv_times < 2; k++, t += 2) {
                    ddouble a = t * s * g[t], q = gpn[t];

                    ddouble dc = u * r[k] * ((4 * t * nu) * (p + q) - (x2 - (4 * t * t)) * (p - q)) / a;

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x4;
                }

                return c;
            }

            private static ddouble BesselYKernel(int n, ddouble x, int terms) {
                if (n < 0) {
                    ddouble y = BesselYKernel(-n, x, terms);

                    return ((n & 1) == 0) ? y : -y;
                }
                else {
                    if (n == 0) {
                        return BesselY0Kernel(x, terms);
                    }
                    if (n == 1) {
                        return BesselY1Kernel(x, terms);
                    }
                }

                ddouble v = 1d / x;
                ddouble y0 = BesselY0Kernel(x, terms);
                ddouble y1 = BesselY1Kernel(x, terms);

                for (int k = 1; k < n; k++) {
                    (y1, y0) = ((2 * k) * v * y1 - y0, y1);
                }

                return y1;
            }

            private static ddouble BesselY0Kernel(ddouble x, int terms) {
                if (!dfactdenom_coef_table.ContainsKey(0)) {
                    dfactdenom_coef_table.Add(0, new DoubleFactDenomTable(0));
                }
                if (!x2denom_coef_table.ContainsKey(0)) {
                    x2denom_coef_table.Add(0, new X2DenomTable(0));
                }

                DoubleFactDenomTable r = dfactdenom_coef_table[0];
                X2DenomTable d = x2denom_coef_table[0];
                Y0CoefTable q = y0_coef_table;

                ddouble h = Log(Ldexp(x, -1)) + EulerGamma;

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = 0d, u = Ldexp(RcpPI, 1);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * ((h - HarmonicNumber(2 * k)) * (1d - x2 * d[k]) + x2 * q[k]);

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x4;
                }

                return c;
            }

            private static ddouble BesselY1Kernel(ddouble x, int terms) {
                if (!dfactdenom_coef_table.ContainsKey(1)) {
                    dfactdenom_coef_table.Add(1, new DoubleFactDenomTable(1));
                }
                if (!x2denom_coef_table.ContainsKey(1)) {
                    x2denom_coef_table.Add(1, new X2DenomTable(1));
                }

                DoubleFactDenomTable r = dfactdenom_coef_table[1];
                X2DenomTable d = x2denom_coef_table[1];
                Y1CoefTable q = y1_coef_table;

                ddouble h = Ldexp(Log(Ldexp(x, -1)) + EulerGamma, 1);

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = -2d / (x * PI), u = x / Ldexp(PI, 1);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * ((h - HarmonicNumber(2 * k) - HarmonicNumber(2 * k + 1)) * (1d - x2 * d[k]) + x2 * q[k]);

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x4;
                }

                return c;
            }

            private static ddouble BesselKKernel(ddouble nu, ddouble x, int terms) {
                if (!gammadenom_coef_table.ContainsKey(nu)) {
                    gammadenom_coef_table.Add(nu, new GammaDenomTable(nu));
                }
                if (!gammadenom_coef_table.ContainsKey(-nu)) {
                    gammadenom_coef_table.Add(-nu, new GammaDenomTable(-nu));
                }

                KCoefTable r = k_coef_table;
                GammaDenomTable gp = gammadenom_coef_table[nu], gn = gammadenom_coef_table[-nu];

                ddouble tp = Pow(Ldexp(x, -1), nu), tn = 1d / tp;

                ddouble x2 = x * x;

                ddouble c = 0d, u = PI / Ldexp(SinPI(nu), 1);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * (tn * gn[k] - tp * gp[k]);

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x2;
                }

                return c;
            }

            private static ddouble BesselKKernel(int n, ddouble x, int terms) {
                if (n == 0) {
                    return BesselK0Kernel(x, terms);
                }
                if (n == 1) {
                    return BesselK1Kernel(x, terms);
                }

                ddouble v = 1d / x;
                ddouble y0 = BesselK0Kernel(x, terms);
                ddouble y1 = BesselK1Kernel(x, terms);

                for (int k = 1; k < n; k++) {
                    (y1, y0) = ((2 * k) * v * y1 + y0, y1);
                }

                return y1;
            }

            private static ddouble BesselK0Kernel(ddouble x, int terms) {
                K0CoefTable r = k0_coef_table;
                ddouble h = -Log(Ldexp(x, -1)) - EulerGamma;

                ddouble x2 = x * x;

                ddouble c = 0d, u = 1d;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * (h + HarmonicNumber(k));

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x2;
                }

                return c;
            }

            private static ddouble BesselK1Kernel(ddouble x, int terms) {
                K1CoefTable r = k1_coef_table;
                ddouble h = Log(Ldexp(x, -1)) + EulerGamma;

                ddouble x2 = x * x;

                ddouble c = 1d / x, u = Ldexp(x, -1);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * (h - Ldexp(HarmonicNumber(k) + HarmonicNumber(k + 1), -1));

                    ddouble c_next = c + dc;

                    if (c == c_next || !IsFinite(c_next)) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    c = c_next;
                    u *= x2;
                }

                return c;
            }

            private class DoubleFactDenomTable {
                private ddouble c;
                private readonly ddouble nu;
                private readonly List<ddouble> table = new();

                public DoubleFactDenomTable(ddouble nu) {
                    this.c = Gamma(nu + 1d);
                    this.nu = nu;
                    this.table.Add(Rcp(c));
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= (nu + (2 * k)) * (nu + (2 * k - 1)) * (32 * k * (2 * k - 1));

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class X2DenomTable {
                private readonly ddouble nu;
                private readonly List<ddouble> table = new();

                public X2DenomTable(ddouble nu) {
                    ddouble a = Rcp(4d * (nu + 1d));

                    this.nu = nu;
                    this.table.Add(a);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble a = Rcp(4d * (2 * k + 1) * (2 * k + 1 + nu));

                        table.Add(a);
                    }

                    return table[n];
                }
            }

            private class GammaDenomTable {
                private ddouble c;
                private readonly ddouble nu;
                private readonly List<ddouble> table = new();

                public GammaDenomTable(ddouble nu) {
                    this.c = Gamma(nu + 1d);
                    this.nu = nu;
                    this.table.Add(Rcp(c));
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= nu + k;

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class GammaTable {
                private ddouble c;
                private readonly ddouble nu;
                private readonly List<ddouble> table = new();

                public GammaTable(ddouble nu) {
                    this.c = Gamma(nu + 1d);
                    this.nu = nu;
                    this.table.Add(c);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= nu + k;

                        table.Add(c);
                    }

                    return table[n];
                }
            }

            private class GammaPNTable {
                private readonly ddouble r;
                private readonly GammaTable positive_table, negative_table;
                private readonly List<ddouble> table = new();

                public GammaPNTable(ddouble nu) {
                    this.r = Pow(4, nu);
                    this.positive_table = new(nu);
                    this.negative_table = new(-nu);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble c = r * positive_table[k] / negative_table[k];

                        table.Add(c);
                    }

                    return table[n];
                }
            }

            private class YCoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public YCoefTable() {
                    this.c = 1d;
                    this.table.Add(1d);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= (32 * k * (2 * k - 1));

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class Y0CoefTable {
                private readonly List<ddouble> table = new();

                public Y0CoefTable() {
                    this.table.Add(Rcp(4));
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble c = Rcp((4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 1)));

                        table.Add(c);
                    }

                    return table[n];
                }
            }

            private class Y1CoefTable {
                private readonly List<ddouble> table = new();

                public Y1CoefTable() {
                    this.table.Add(Ldexp(3, -4));
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble c = (ddouble)(4 * k + 3) / (ddouble)(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 2) * (2 * k + 2));

                        table.Add(c);
                    }

                    return table[n];
                }
            }

            private class KCoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public KCoefTable() {
                    this.c = 1d;
                    this.table.Add(1d);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= (4 * k);

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class K0CoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public K0CoefTable() {
                    this.c = 1d;
                    this.table.Add(1d);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= (4 * k * k);

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class K1CoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public K1CoefTable() {
                    this.c = 1d;
                    this.table.Add(1d);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        c *= (4 * k * (k + 1));

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }
        }

        private static class BesselMillerBackward {
            private static Dictionary<ddouble, BesselJPhiTable> phi_table = new();
            private static Dictionary<ddouble, BesselIPsiTable> psi_table = new();
            private static Dictionary<ddouble, BesselYEtaTable> eta_table = new();
            private static Dictionary<ddouble, BesselYXiTable> xi_table = new();

            public static ddouble BesselJ(int n, ddouble x) {
                int m = BesselJYIterM(x.Hi);

                ddouble y = BesselJKernel(n, x, m);

                return y;
            }

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                int m = BesselJYIterM(x.Hi);

                if (BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselJKernel(n, x, m);

                    return y;
                }
                else {
                    ddouble y = BesselJKernel(nu, x, m);

                    return y;
                }
            }

            public static ddouble BesselY(int n, ddouble x) {
                int m = BesselJYIterM(x.Hi);

                ddouble y = BesselYKernel(n, x, m);

                return y;
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                int m = BesselJYIterM(x.Hi);

                if (BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselYKernel(n, x, m);

                    return y;
                }
                else {
                    ddouble y = BesselYKernel(nu, x, m);

                    return y;
                }
            }

            private static int BesselJYIterM(double x) {
                return (int)double.Ceiling(74 + 1.36 * x - 54.25 / double.Sqrt(double.Sqrt(x))) & ~1;
            }

            public static ddouble BesselI(int n, ddouble x, bool scale = false) {
                int m = BesselIIterM(x.Hi);

                ddouble y = BesselIKernel(n, x, m, scale);

                return y;
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                int m = BesselIIterM(x.Hi);

                if (BesselUtil.NearlyInteger(nu, out int n)) {
                    ddouble y = BesselIKernel(n, x, m, scale);

                    return y;
                }
                else {
                    ddouble y = BesselIKernel(nu, x, m, scale);

                    return y;
                }
            }

            private static int BesselIIterM(double x) {
                return (int)double.Ceiling(86 + 0.75 * x - 67.25 / double.Sqrt(double.Sqrt(x))) & ~1;
            }

            private static ddouble BesselJKernel(int n, ddouble x, int m) {
                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (n < 0) {
                    return ((n & 1) == 0) ? BesselJKernel(-n, x, m) : -BesselJKernel(-n, x, m);
                }
                if (n == 0) {
                    return BesselJ0Kernel(x, m);
                }
                if (n == 1) {
                    return BesselJ1Kernel(x, m);
                }

                ddouble f0 = 1e-256, f1 = 0d, fn = 0d, lambda = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);

                    if (k - 1 == n) {
                        fn = f0;
                    }
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble yn = fn / lambda;

                return yn;
            }

            private static ddouble BesselJKernel(ddouble nu, ddouble x, int m) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0d) {
                    return BesselJKernel(n, x, m);
                }

                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!phi_table.ContainsKey(alpha)) {
                    phi_table.Add(alpha, new BesselJPhiTable(alpha));
                }

                BesselJPhiTable phi = phi_table[alpha];

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                if (n >= 0) {
                    ddouble fn = 0d;

                    for (int k = m; k >= 1; k--) {
                        if ((k & 1) == 0) {
                            lambda += f0 * phi[k / 2];
                        }

                        (f0, f1) = (Ldexp(k + alpha, 1) * v * f0 - f1, f0);

                        if (k - 1 == n) {
                            fn = f0;
                        }
                    }

                    lambda += f0 * phi[0];
                    lambda *= Pow(Ldexp(v, 1), alpha);

                    ddouble yn = fn / lambda;

                    return yn;
                }
                else {
                    for (int k = m; k >= 1; k--) {
                        if ((k & 1) == 0) {
                            lambda += f0 * phi[k / 2];
                        }

                        (f0, f1) = (Ldexp(k + alpha, 1) * v * f0 - f1, f0);
                    }

                    lambda += f0 * phi[0];
                    lambda *= Pow(Ldexp(v, 1), alpha);

                    for (int k = 0; k > n; k--) {
                        (f0, f1) = (Ldexp(k + alpha, 1) * v * f0 - f1, f0);
                    }

                    ddouble yn = f0 / lambda;

                    return yn;
                }
            }

            private static ddouble BesselJ0Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble y0 = f0 / lambda;

                return y0;
            }

            private static ddouble BesselJ1Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble y1 = f1 / lambda;

                return y1;
            }

            private static ddouble BesselYKernel(int n, ddouble x, int m) {
                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (n < 0) {
                    return ((n & 1) == 0) ? BesselYKernel(-n, x, m) : -BesselYKernel(-n, x, m);
                }
                if (n == 0) {
                    return BesselY0Kernel(x, m);
                }
                if (n == 1) {
                    return BesselY1Kernel(x, m);
                }

                if (!eta_table.ContainsKey(0)) {
                    eta_table.Add(0, new BesselYEtaTable(0));
                }

                BesselYEtaTable eta = eta_table[0];

                if (!xi_table.ContainsKey(0)) {
                    xi_table.Add(0, new BesselYXiTable(0, eta));
                }

                BesselYXiTable xi = xi_table[0];

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble se = 0d, sx = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;

                        se += f0 * eta[k / 2];
                    }
                    else if (k >= 3) {
                        sx += f0 * xi[k];
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble c = Log(Ldexp(x, -1)) + EulerGamma;

                ddouble y0 = se + f0 * c;
                ddouble y1 = sx - v * f0 + (c - 1d) * f1;

                for (int k = 1; k < n; k++) {
                    (y1, y0) = ((2 * k) * v * y1 - y0, y1);
                }

                ddouble yn = Ldexp(y1 / (lambda * PI), 1);

                return yn;
            }

            private static ddouble BesselYKernel(ddouble nu, ddouble x, int m) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0d) {
                    return BesselYKernel(n, x, m);
                }

                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!eta_table.ContainsKey(alpha)) {
                    eta_table.Add(alpha, new BesselYEtaTable(alpha));
                }

                BesselYEtaTable eta = eta_table[alpha];

                if (!xi_table.ContainsKey(alpha)) {
                    xi_table.Add(alpha, new BesselYXiTable(alpha, eta));
                }

                BesselYXiTable xi = xi_table[alpha];

                if (!phi_table.ContainsKey(alpha)) {
                    phi_table.Add(alpha, new BesselJPhiTable(alpha));
                }

                BesselJPhiTable phi = phi_table[alpha];

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble se = 0d, sxo = 0d, sxe = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0 * phi[k / 2];

                        se += f0 * eta[k / 2];
                        sxe += f0 * xi[k];
                    }
                    else if (k >= 3) {
                        sxo += f0 * xi[k];
                    }

                    (f0, f1) = (Ldexp(k + alpha, 1) * v * f0 - f1, f0);
                }

                ddouble s = Pow(Ldexp(v, 1), alpha), sqs = s * s;

                lambda += f0 * phi[0];
                lambda *= s;

                ddouble rcot = 1d / TanPI(alpha), rgamma = Gamma(1d + alpha), rsqgamma = rgamma * rgamma;
                ddouble r = Ldexp(RcpPI * sqs, 1);
                ddouble p = sqs * rsqgamma * RcpPI;

                ddouble eta0 = (double.Abs(alpha.Hi) > BesselUtil.MillerBwdBesselYEps)
                    ? (rcot - p / alpha)
                    : BesselYEta0Eps(alpha, x);

                ddouble xi0 = -Ldexp(v, 1) * p;
                ddouble xi1 = (double.Abs(alpha.Hi) > BesselUtil.MillerBwdBesselYEps)
                    ? rcot + p * (alpha * (alpha + 1d) + 1d) / (alpha * (alpha - 1d))
                    : BesselYXi1Eps(alpha, x);

                ddouble y0 = r * se + eta0 * f0;
                ddouble y1 = r * (3d * alpha * v * sxe + sxo) + xi0 * f0 + xi1 * f1;

                if (n == 0) {
                    ddouble yn = y0 / lambda;

                    return yn;
                }
                if (n == 1) {
                    ddouble yn = y1 / lambda;

                    return yn;
                }
                if (n >= 0) {
                    for (int k = 1; k < n; k++) {
                        (y1, y0) = (Ldexp(k + alpha, 1) * v * y1 - y0, y1);
                    }

                    ddouble yn = y1 / lambda;

                    return yn;
                }
                else {
                    for (int k = 0; k > n; k--) {
                        (y0, y1) = (Ldexp(k + alpha, 1) * v * y0 - y1, y0);
                    }

                    ddouble yn = y0 / lambda;

                    return yn;
                }
            }

            private static ddouble BesselYEta0Eps(ddouble alpha, ddouble x) {
                ddouble lnx = Log(x), lnhalfx = Log(Ldexp(x, -1));
                ddouble pi = PI, sqpi = pi * pi;
                ddouble ln2 = Ln2, sqln2 = ln2 * ln2, cbln2 = sqln2 * ln2, qdln2 = sqln2 * sqln2;
                ddouble g = EulerGamma;

                ddouble r0 = lnhalfx + g;
                ddouble r1 =
                    (-sqln2 + lnx * (ln2 * 2d - lnx)) * 4d
                    - sqpi
                    - g * (lnhalfx * 2d + g) * 4d;
                ddouble r2 =
                    (-cbln2 + lnx * (sqln2 * 3d + lnx * (ln2 * -3d + lnx))) * 4d
                    + Zeta3 * 2d
                    + sqpi * (lnhalfx + g)
                    + g * ((sqln2 + lnx * (ln2 * -2d + lnx)) * 3d + g * (lnhalfx * 3d + g)) * 4d;
                ddouble r3 =
                    (-qdln2 + lnx * (cbln2 * 4d + lnx * (sqln2 * -6d + lnx * (ln2 * 4d - lnx)))) * 16d
                    - Zeta3 * (lnhalfx + g) * 32d
                    - sqpi * (((sqln2 + lnx * (-ln2 * 2d + lnx) + g * (lnhalfx * 2d + g)) * 8d) + sqpi)
                    + g * ((cbln2 + lnx * (sqln2 * -3d + lnx * (ln2 * 3d - lnx))) * 4d
                    + g * ((sqln2 + lnx * (ln2 * -2d + lnx)) * -6d
                    + g * (lnhalfx * -4d
                    - g))) * 16d;

                ddouble eta0 = (r0 * 48d + alpha * (r1 * 12d + alpha * (r2 * 8d + alpha * r3))) / (24d * PI);

                return eta0;
            }

            static ddouble BesselYXi1Eps(ddouble alpha, ddouble x) {
                ddouble lnx = Log(x), lnhalfx = Log(Ldexp(x, -1)), lnxm1 = lnx - 1, lnhalfxm1 = lnhalfx - 1;
                ddouble pi = PI, sqpi = pi * pi;
                ddouble ln2 = Ln2, sqln2 = ln2 * ln2, cbln2 = sqln2 * ln2, qdln2 = sqln2 * sqln2;
                ddouble g = EulerGamma;

                ddouble r0 = lnhalfxm1 + g;
                ddouble r1 =
                    (-sqln2 + ln2 * lnxm1 * 2d + lnx * (2 - lnx)) * 4d
                    - sqpi
                    - g * (lnhalfxm1 * 2d + g) * 4d
                    - 6d;
                ddouble r2 =
                    (-cbln2 * 4d + sqln2 * lnxm1 * 12d + lnx * (18d + lnx * (-12d + lnx * 4d)))
                    + ln2 * (lnx * (2d - lnx) * 12d - 18d)
                    + Zeta3 * 2d
                    + sqpi * (lnhalfxm1 + g)
                    + g * ((((sqln2 - ln2 * lnxm1 * 2d) + lnx * (-2d + lnx)) * 12d + 18d)
                    + g * (lnhalfxm1 * 12d
                    + g * 4d))
                    - 9d;
                ddouble r3 =
                    -qdln2 * 16d
                    + cbln2 * lnxm1 * 64d
                    + sqln2 * (lnx * (2d - lnx) * 96d - 144d)
                    + ln2 * (lnx * (9d + lnx * (-6d + lnx * 2d)) * 32d - 144d)
                    + lnx * (9d + lnx * (-9d + lnx * (4d - lnx))) * 16d
                    + Zeta3 * (lnhalfxm1 + g) * -32d
                    + sqpi * (((-sqln2 + ln2 * lnxm1 * 2d + lnx * (2d - lnx) - g * (lnhalfxm1 * 2d + g)) * 8d - 12d) - sqpi)
                    + g * (((cbln2 - sqln2 * lnxm1 * 3d) * 64d + ln2 * (lnx * (-2d + lnx) * 192d + 288d) + lnx * (-9d + lnx * (6d - lnx * 2d)) * 32d + 144d)
                    + g * (((-sqln2 + ln2 * lnxm1 * 2d + lnx * (2d - lnx)) * 96d - 144d)
                    + g * (lnhalfxm1 * -64d
                    - g * 16d)))
                    - 72d;

                ddouble xi1 = (r0 * 48d + alpha * (r1 * 12d + alpha * (r2 * 8d + alpha * r3))) / (24d * PI);

                return xi1;
            }

            private static ddouble BesselY0Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!eta_table.ContainsKey(0)) {
                    eta_table.Add(0, new BesselYEtaTable(0));
                }

                BesselYEtaTable eta = eta_table[0];

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble se = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;

                        se += f0 * eta[k / 2];
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble y0 = Ldexp((se + f0 * (Log(Ldexp(x, -1)) + EulerGamma)) / (PI * lambda), 1);

                return y0;
            }

            private static ddouble BesselY1Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!xi_table.ContainsKey(0)) {
                    if (!eta_table.ContainsKey(0)) {
                        eta_table.Add(0, new BesselYEtaTable(0));
                    }

                    xi_table.Add(0, new BesselYXiTable(0, eta_table[0]));
                }

                BesselYXiTable xi = xi_table[0];

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d;
                ddouble sx = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }
                    else if (k >= 3) {
                        sx += f0 * xi[k];
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble y1 = Ldexp((sx - v * f0 + (Log(Ldexp(x, -1)) + EulerGamma - 1d) * f1) / (lambda * PI), 1);

                return y1;
            }

            private static ddouble BesselIKernel(int n, ddouble x, int m, bool scale = false) {
                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                n = int.Abs(n);

                if (n == 0) {
                    return BesselI0Kernel(x, m, scale);
                }
                if (n == 1) {
                    return BesselI1Kernel(x, m, scale);
                }

                ddouble f0 = 1e-256, f1 = 0d, lambda = 0d, fn = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += f0;

                    (f0, f1) = ((2 * k) * v * f0 + f1, f0);

                    if (k - 1 == n) {
                        fn = f0;
                    }
                }

                lambda = Ldexp(lambda, 1) + f0;

                ddouble yn = fn / lambda;

                if (!scale) {
                    yn *= Exp(x);
                }

                return yn;
            }

            private static ddouble BesselIKernel(ddouble nu, ddouble x, int m, bool scale = false) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0d) {
                    return BesselIKernel(n, x, m, scale);
                }

                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!psi_table.ContainsKey(alpha)) {
                    psi_table.Add(alpha, new BesselIPsiTable(alpha));
                }

                BesselIPsiTable psi = psi_table[alpha];

                ddouble g0 = 1e-256, g1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                if (n >= 0) {
                    ddouble gn = 0d;

                    for (int k = m; k >= 1; k--) {
                        lambda += g0 * psi[k];

                        (g0, g1) = (Ldexp(k + alpha, 1) * v * g0 + g1, g0);

                        if (k - 1 == n) {
                            gn = g0;
                        }
                    }

                    lambda += g0 * psi[0];
                    lambda *= Pow(Ldexp(v, 1), alpha);

                    ddouble yn = gn / lambda;

                    if (!scale) {
                        yn *= Exp(x);
                    }

                    return yn;
                }
                else {
                    for (int k = m; k >= 1; k--) {
                        lambda += g0 * psi[k];

                        (g0, g1) = (Ldexp(k + alpha, 1) * v * g0 + g1, g0);
                    }

                    lambda += g0 * psi[0];
                    lambda *= Pow(Ldexp(v, 1), alpha);

                    for (int k = 0; k > n; k--) {
                        (g0, g1) = (Ldexp(k + alpha, 1) * v * g0 + g1, g0);
                    }

                    ddouble yn = g0 / lambda;

                    if (!scale) {
                        yn *= Exp(x);
                    }

                    return yn;
                }
            }

            private static ddouble BesselI0Kernel(ddouble x, int m, bool scale = false) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble g0 = 1e-256, g1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += g0;

                    (g0, g1) = ((2 * k) * v * g0 + g1, g0);
                }

                lambda = Ldexp(lambda, 1) + g0;

                ddouble y0 = g0 / lambda;

                if (!scale) {
                    y0 *= Exp(x);
                }

                return y0;
            }

            private static ddouble BesselI1Kernel(ddouble x, int m, bool scale = false) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble g0 = 1e-256, g1 = 0d, lambda = 0d;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += g0;

                    (g0, g1) = ((2 * k) * v * g0 + g1, g0);
                }

                lambda = Ldexp(lambda, 1) + g0;

                ddouble y1 = g1 / lambda;

                if (!scale) {
                    y1 *= Exp(x);
                }

                return y1;
            }

            private class BesselJPhiTable {
                private readonly ddouble alpha;
                private readonly List<ddouble> table = new();

                private ddouble g;

                public BesselJPhiTable(ddouble alpha) {
                    if (!(alpha > 0d) || alpha >= 1d) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;

                    ddouble phi0 = Gamma(1 + alpha);
                    ddouble phi1 = phi0 * (alpha + 2d);

                    this.g = phi0;

                    this.table.Add(phi0);
                    this.table.Add(phi1);
                }

                public ddouble this[int n] => Value(n);

                private ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        g = g * (alpha + m - 1d) / m;

                        ddouble phi = g * (alpha + 2 * m);

                        table.Add(phi);
                    }

                    return table[n];
                }
            };

            private class BesselIPsiTable {
                private readonly ddouble alpha;
                private readonly List<ddouble> table = new();

                private ddouble g;

                public BesselIPsiTable(ddouble alpha) {
                    if (!(alpha > 0d) || alpha >= 1d) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;

                    ddouble psi0 = Gamma(1d + alpha);
                    ddouble psi1 = Ldexp(psi0, 1) * (1d + alpha);

                    this.g = Ldexp(psi0, 1);

                    this.table.Add(psi0);
                    this.table.Add(psi1);
                }

                public ddouble this[int n] => Value(n);

                private ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        g = g * (Ldexp(alpha, 1) + m - 1d) / m;

                        ddouble phi = g * (alpha + m);

                        table.Add(phi);
                    }

                    return table[n];
                }
            };

            private class BesselYEtaTable {
                private readonly ddouble alpha;
                private readonly List<ddouble> table = new();

                private ddouble g;

                public BesselYEtaTable(ddouble alpha) {
                    if (!(alpha >= 0d) || alpha >= 1d) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;
                    this.table.Add(NaN);

                    if (alpha > 0d) {
                        ddouble c = Gamma(1d + alpha);
                        c *= c;
                        this.g = 1d / (1d - alpha) * c;

                        ddouble eta1 = (alpha + 2d) * g;

                        this.table.Add(eta1);
                    }
                }

                public ddouble this[int n] => Value(n);

                private ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        if (alpha > 0d) {
                            g = -g * (alpha + m - 1) * (Ldexp(alpha, 1) + m - 1d) / (m * (m - alpha));

                            ddouble eta = g * (alpha + 2 * m);

                            table.Add(eta);
                        }
                        else {
                            ddouble eta = (ddouble)2d / m;

                            table.Add(((m & 1) == 1) ? eta : -eta);
                        }
                    }

                    return table[n];
                }
            };

            private class BesselYXiTable {
                private readonly ddouble alpha;
                private readonly List<ddouble> table = new();
                private readonly BesselYEtaTable eta;

                public BesselYXiTable(ddouble alpha, BesselYEtaTable eta) {
                    if (!(alpha >= 0d) || alpha >= 1d) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;
                    this.table.Add(NaN);
                    this.table.Add(NaN);

                    this.eta = eta;
                }

                public ddouble this[int n] => Value(n);

                private ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int m = table.Count; m <= n; m++) {
                        if (alpha > 0d) {
                            if ((m & 1) == 0) {
                                table.Add(eta[m / 2]);
                            }
                            else {
                                table.Add((eta[m / 2] - eta[m / 2 + 1]) / 2);
                            }
                        }
                        else {
                            if ((m & 1) == 1) {
                                ddouble xi = (ddouble)(2 * (m / 2) + 1) / ((m / 2) * ((m / 2) + 1));
                                table.Add(((m & 2) > 0) ? xi : -xi);
                            }
                            else {
                                table.Add(NaN);
                            }
                        }
                    }

                    return table[n];
                }
            };
        }

        private static class BesselYoshidaPade {
            private static readonly ReadOnlyCollection<ReadOnlyCollection<ddouble>> EssTable;
            private static readonly Dictionary<ddouble, ReadOnlyCollection<(ddouble c, ddouble s)>> CDsTable = new();

            static BesselYoshidaPade() {
                Dictionary<string, ReadOnlyCollection<ddouble>> tables =
                    ResourceUnpack.NumTable(Resource.BesselKTable);

                ReadOnlyCollection<ddouble> cs0 = tables["CS0Table"], cs1 = tables["CS1Table"];
                ReadOnlyCollection<ddouble> ds0 = tables["DS0Table"], ds1 = tables["DS1Table"];

                if (cs0.Count != ds0.Count || cs1.Count != ds1.Count) {
                    throw new IOException("The format of resource file is invalid.");
                }

                List<(ddouble c, ddouble s)> cd0 = new(), cd1 = new();

                for (int i = 0; i < cs0.Count; i++) {
                    cd0.Add((cs0[i], ds0[i]));
                }
                for (int i = 0; i < cs1.Count; i++) {
                    cd1.Add((cs1[i], ds1[i]));
                }

                cd0.Reverse();
                cd1.Reverse();

                CDsTable.Add(0, Array.AsReadOnly(cd0.ToArray()));
                CDsTable.Add(1, Array.AsReadOnly(cd1.ToArray()));

                List<ReadOnlyCollection<ddouble>> es = new();

                for (int i = 0; i < 32; i++) {
                    es.Add(tables[$"ES{i}Table"]);
                }

                EssTable = Array.AsReadOnly(es.ToArray());
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
                if (nu < 2d) {
                    if (!CDsTable.ContainsKey(nu)) {
                        CDsTable.Add(nu, Table(nu));
                    }

                    ReadOnlyCollection<(ddouble, ddouble)> cds = CDsTable[nu];

                    ddouble y = Value(x, cds, scale);

                    return y;
                }
                else {
                    int n = (int)Floor(nu);
                    ddouble alpha = nu - n;

                    ddouble y0 = BesselK(alpha, x, scale);
                    ddouble y1 = BesselK(alpha + 1d, x, scale);

                    ddouble v = 1d / x;

                    for (int k = 1; k < n; k++) {
                        (y1, y0) = (Ldexp(k + alpha, 1) * v * y1 + y0, y1);
                    }

                    return y1;
                }
            }

            private static ddouble Value(ddouble x, ReadOnlyCollection<(ddouble c, ddouble d)> cds, bool scale = false) {
                ddouble t = 1d / x;
                (ddouble sc, ddouble sd) = cds[0];

                for (int i = 1; i < cds.Count; i++) {
                    (ddouble c, ddouble d) = cds[i];

                    sc = sc * t + c;
                    sd = sd * t + d;
                }

#if DEBUG
                Trace.Assert(sd > 0.0625d, $"[BesselK x={x}] Too small pade denom!!");
#endif

                ddouble y = Sqrt(Ldexp(t * PI, -1)) * sc / sd;

                if (!scale) {
                    y *= Exp(-x);
                }

                return y;
            }

            private static ReadOnlyCollection<(ddouble c, ddouble d)> Table(ddouble nu) {
                int m = EssTable.Count - 1;

                ddouble squa_nu = nu * nu;
                List<(ddouble c, ddouble d)> cds = new();
                ddouble[] us = new ddouble[m + 1], vs = new ddouble[m];

                ddouble u = 1d;
                for (int i = 0; i <= m; i++) {
                    us[i] = u;
                    u *= squa_nu;
                }
                for (int i = 0; i < m; i++) {
                    ddouble r = m - i + 0.5d;
                    vs[i] = r * r - squa_nu;
                }

                for (int i = 0; i <= m; i++) {
                    ReadOnlyCollection<ddouble> es = EssTable[i];
                    ddouble d = es[i], c = 0d;

                    for (int l = 0; l < i; l++) {
                        d *= vs[l];
                    }
                    for (int j = 0; j <= i; j++) {
                        c += es[j] * us[j];
                    }

                    cds.Add((c, d));
                }

                cds.Reverse();

                return Array.AsReadOnly(cds.ToArray());
            }
        }

        private static class BesselLimit {
            private static Dictionary<ddouble, ACoefTable> a_table = new();
            private static Dictionary<ddouble, JYCoefTable> jy_table = new();
            private static Dictionary<ddouble, IKCoefTable> ik_table = new();

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                (ddouble c, ddouble s) = BesselJYKernel(nu, x, terms: 32);

                ddouble omega = x - Ldexp((Ldexp(nu, 1) + 1d) * PI, -2);
                ddouble m = c * Cos(omega) - s * Sin(omega);
                ddouble t = m * Sqrt(2d / (PI * x));

                return t;
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                (ddouble s, ddouble c) = BesselJYKernel(nu, x, terms: 32);

                ddouble omega = x - Ldexp((Ldexp(nu, 1) + 1d) * PI, -2);
                ddouble m = s * Sin(omega) + c * Cos(omega);
                ddouble t = m * Sqrt(2d / (PI * x));

                return t;
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                ddouble c = BesselIKKernel(nu, x, sign_switch: true, terms: 36);

                ddouble t = c / Sqrt(Ldexp(PI, 1) * x);

                if (!scale) {
                    t *= Exp(x);
                }

                return t;
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
                ddouble c = BesselIKKernel(nu, x, sign_switch: false, terms: 34);

                ddouble t = c * Sqrt(PI / Ldexp(x, 1));

                if (!scale) {
                    t *= Exp(-x);
                }

                return t;
            }

            private static (ddouble s, ddouble t) BesselJYKernel(ddouble nu, ddouble x, int terms = 64) {
                if (!a_table.ContainsKey(nu)) {
                    a_table.Add(nu, new ACoefTable(nu));
                }
                if (!jy_table.ContainsKey(nu)) {
                    jy_table.Add(nu, new JYCoefTable(nu));
                }

                ACoefTable a = a_table[nu];
                JYCoefTable c = jy_table[nu];

                ddouble v = 1d / x, v2 = v * v, v4 = v2 * v2;
                ddouble s = 0d, t = 0d, p = 1d, q = v;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble ds = p * a[k * 4] * (1d - v2 * c[k].p0);
                    ddouble dt = q * a[k * 4 + 1] * (1d - v2 * c[k].p1);

                    ddouble s_next = s + ds;
                    ddouble t_next = t + dt;

                    if (s == s_next && t == t_next) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    p *= v4;
                    q *= v4;
                    s = s_next;
                    t = t_next;
                }

                return (s, t);
            }

            private static ddouble BesselIKKernel(ddouble nu, ddouble x, bool sign_switch, int terms) {
                if (!a_table.ContainsKey(nu)) {
                    a_table.Add(nu, new ACoefTable(nu));
                }
                if (!ik_table.ContainsKey(nu)) {
                    ik_table.Add(nu, new IKCoefTable(nu));
                }

                ACoefTable a = a_table[nu];
                IKCoefTable c = ik_table[nu];

                ddouble v = 1d / x, v2 = v * v;
                ddouble r = 0d, u = 1d;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble w = v * c[k];
                    ddouble dr = u * a[k * 2] * (sign_switch ? (1d - w) : (1d + w));

                    ddouble r_next = r + dr;

                    if (r == r_next) {
                        conv_times++;
                    }
                    else {
                        conv_times = 0;
                    }

                    r = r_next;
                    u *= v2;
                }

                return r;
            }

            private class ACoefTable {
                private readonly ddouble squa_nu4;
                private readonly List<ddouble> table = new();

                public ACoefTable(ddouble nu) {
                    this.squa_nu4 = Ldexp(nu * nu, 2);

                    ddouble a1 = Ldexp(squa_nu4 - 1d, -3);

                    this.table.Add(1d);
                    this.table.Add(a1);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    for (int k = table.Count; k <= n; k++) {
                        ddouble a = table.Last() * (squa_nu4 - ((2 * k - 1) * (2 * k - 1))) / (k * 8);

                        table.Add(a);
                    }

                    return table[n];
                }
            }

            private class JYCoefTable {
                private readonly ddouble squa_nu4;
                private readonly List<(ddouble p0, ddouble p1)> table = new();

                public JYCoefTable(ddouble nu) {
                    this.squa_nu4 = Ldexp(nu * nu, 2);
                }

                public (ddouble p0, ddouble p1) this[int n] => Value(n);

                public (ddouble p0, ddouble p1) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    static int square(int n) => (n * n);

                    for (int k = table.Count; k <= n; k++) {
                        ddouble p0 = (squa_nu4 - square(8 * k + 1)) * (squa_nu4 - square(8 * k + 3)) / (64 * (4 * k + 1) * (4 * k + 2));
                        ddouble p1 = (squa_nu4 - square(8 * k + 3)) * (squa_nu4 - square(8 * k + 5)) / (64 * (4 * k + 2) * (4 * k + 3));

                        table.Add((p0, p1));
                    }

                    return table[n];
                }
            }

            private class IKCoefTable {
                private readonly ddouble squa_nu4;
                private readonly List<ddouble> table = new();

                public IKCoefTable(ddouble nu) {
                    this.squa_nu4 = Ldexp(nu * nu, 2);
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    static int square(int n) => (n * n);

                    for (int k = table.Count; k <= n; k++) {
                        ddouble p = (squa_nu4 - square(4 * k + 1)) / (8 * (2 * k + 1));

                        table.Add(p);
                    }

                    return table[n];
                }
            }
        }

        private static class BesselInterpolate {
            public static ddouble BesselY(ddouble nu, ddouble x) {
                int n = (int)Round(nu);
                ddouble alpha = nu - n;

                ddouble y0 = ddouble.BesselY(n, x);
                ddouble y1 = ddouble.BesselY(n + Sign(alpha) * BesselUtil.InterpolationThreshold, x);
                ddouble y2 = ddouble.BesselY(n + Sign(alpha) * BesselUtil.InterpolationThreshold * 1.5, x);
                ddouble y3 = ddouble.BesselY(n + Sign(alpha) * BesselUtil.InterpolationThreshold * 2, x);

                ddouble t = Abs(alpha) / BesselUtil.InterpolationThreshold;
                ddouble y = CubicInterpolate(t, y0, y1, y2, y3);

                return y;
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale) {
                int n = (int)Round(nu);
                ddouble alpha = nu - n;

                ddouble y0 = ddouble.BesselK(n, x, scale: true);
                ddouble y1 = ddouble.BesselK(n + Sign(alpha) * BesselUtil.InterpolationThreshold, x, scale: true);
                ddouble y2 = ddouble.BesselK(n + Sign(alpha) * BesselUtil.InterpolationThreshold * 1.5, x, scale: true);
                ddouble y3 = ddouble.BesselK(n + Sign(alpha) * BesselUtil.InterpolationThreshold * 2, x, scale: true);

                ddouble t = Abs(alpha) / BesselUtil.InterpolationThreshold;
                ddouble y = CubicInterpolate(t, y0, y1, y2, y3);

                if (!scale) {
                    y *= Exp(-x);
                }

                return y;
            }

            private static ddouble CubicInterpolate(ddouble t, ddouble y0, ddouble y1, ddouble y2, ddouble y3) {
                return y0 + (
                    -(13d + t * (-9d + t * 2d)) / 6d * y0
                    + (6d + t * (-7d + t * 2d)) * y1
                    - (16d + t * (-24d + t * 8d)) / 3d * y2
                    + (3d + t * (-5d + t * 2d)) / 2d * y3) * t;
            }
        }
    }
}