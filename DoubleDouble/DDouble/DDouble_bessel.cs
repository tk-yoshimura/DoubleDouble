using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleDouble {

    public partial struct ddouble {

        public static ddouble BesselJ(ddouble nu, ddouble x) {
            CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (nu - Floor(nu) == Point5) {
                return HalfInt.BesselJ((int)Floor(nu), x);
            }

            if (x <= 2d) {
                return NearZero.BesselJ(nu, x);
            }
            if (x <= 40d) {
                return MillerBackward.BesselJ(nu, x);
            }

            return Limit.BesselJ(nu, x);
        }

        public static ddouble BesselJ(int n, ddouble x) {
            CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselJ(n, x);
            }
            if (x <= 40d) {
                return MillerBackward.BesselJ(n, x);
            }

            return Limit.BesselJ(n, x);
        }

        public static ddouble BesselY(ddouble nu, ddouble x) {
            CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselY(nu, x);
            }
            if (x <= 40d) {
                return MillerBackward.BesselY(nu, x);
            }

            return Limit.BesselY(nu, x);
        }

        public static ddouble BesselY(int n, ddouble x) {
            CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselY(n, x);
            }
            if (x <= 40d) {
                return MillerBackward.BesselY(n, x);
            }

            return Limit.BesselY(n, x);
        }

        public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
            CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselI(nu, x, scale);
            }
            if (x <= 40d) {
                return MillerBackward.BesselI(nu, x, scale);
            }

            return Limit.BesselI(nu, x, scale);
        }

        public static ddouble BesselI(int n, ddouble x, bool scale = false) {
            CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselI(n, x, scale);
            }
            if (x <= 40d) {
                return MillerBackward.BesselI(n, x, scale);
            }

            return Limit.BesselI(n, x, scale);
        }

        public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
            CheckNu(nu);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselK(nu, x, scale);
            }
            if (x <= 35d) {
                return YoshidaPade.BesselK(nu, x, scale);
            }

            return Limit.BesselK(nu, x, scale);
        }

        public static ddouble BesselK(int n, ddouble x, bool scale = false) {
            CheckN(n);

            if (!(x >= 0d)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x <= 2d) {
                return NearZero.BesselK(n, x, scale);
            }
            if (x <= 35d) {
                return YoshidaPade.BesselK(n, x, scale);
            }

            return Limit.BesselK(n, x, scale);
        }

        private static void CheckNu(ddouble nu) {
            if (nu != Round(nu) && Abs(nu - Round(nu)) < Math.ScaleB(1, -10)) {
                throw new ArgumentException(
                    "The calculation of the Bessel function value is invalid because it loses digits" +
                    " when nu is extremely close to an integer. (|nu - round(nu)| < 9.765625e-4 and nu != round(nu))",
                    nameof(nu)
                );
            }

            if (!(Abs(nu) <= 8)) {
                throw new ArgumentOutOfRangeException(
                    nameof(nu),
                    "In the calculation of the Bessel function, nu with an absolute value greater than 8 is not supported."
                );
            }
        }

        private static void CheckN(int n) {
            if (n < -8 || n > 8) {
                throw new ArgumentOutOfRangeException(
                    nameof(n),
                    "In the calculation of the Bessel function, n with an absolute value greater than 8 is not supported."
                );
            }
        }

        private static class NearZero {
            private static Dictionary<ddouble, DoubleFactDenomTable> dfactdenom_coef_table = new();
            private static Dictionary<ddouble, X2DenomTable> x2denom_coef_table = new();
            private static Dictionary<ddouble, GammaDenomTable> gammadenom_coef_table = new();
            private static YCoefTable y_coef_table = new();
            private static Y0CoefTable y0_coef_table = new();
            private static Y1CoefTable y1_coef_table = new();
            private static KCoefTable k_coef_table = new();
            private static K0CoefTable k0_coef_table = new();
            private static K1CoefTable k1_coef_table = new();

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                if (nu.Sign < 0 && nu == Floor(nu)) {
                    int n = (int)Floor(nu);
                    ddouble y = BesselJ(-nu, x);

                    return ((n & 1) == 0) ? y : -y;
                }
                else {
                    ddouble y = BesselJIKernel(nu, x, sign_switch: true, terms: 9);

                    return y;
                }
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                if (nu == Floor(nu)) {
                    int n = (int)Floor(nu);
                    ddouble y = BesselYKernel(n, x, terms: 9);

                    return y;
                }
                else {
                    ddouble y = BesselYKernel(nu, x, terms: 10);

                    return y;
                }
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                if (nu.Sign < 0 && nu == Floor(nu)) {
                    int n = (int)Floor(nu);
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
                if (nu == Floor(nu)) {
                    int n = (int)Floor(nu);
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

                ddouble c = 0d, u = Pow(x / 2, nu);

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
                if (!gammadenom_coef_table.ContainsKey(nu)) {
                    gammadenom_coef_table.Add(nu, new GammaDenomTable(nu));
                }
                if (!gammadenom_coef_table.ContainsKey(-nu)) {
                    gammadenom_coef_table.Add(-nu, new GammaDenomTable(-nu));
                }
                if (!x2denom_coef_table.ContainsKey(nu)) {
                    x2denom_coef_table.Add(nu, new X2DenomTable(nu));
                }
                if (!x2denom_coef_table.ContainsKey(-nu)) {
                    x2denom_coef_table.Add(-nu, new X2DenomTable(-nu));
                }

                YCoefTable r = y_coef_table;
                X2DenomTable dp = x2denom_coef_table[nu], dn = x2denom_coef_table[-nu];
                GammaDenomTable gp = gammadenom_coef_table[nu], gn = gammadenom_coef_table[-nu];

                ddouble cos = CosPI(nu), sin = SinPI(nu);
                ddouble tp = Pow(x / 2, nu), tn = 1d / tp, fp = tp * cos;

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = 0d, u = 1d / sin;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * (fp * gp[2 * k] * (1 - x2 * dp[k]) - tn * gn[2 * k] * (1 - x2 * dn[k]));

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

                ddouble h = Log(x / 2) + EulerGamma;

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = 0d, u = 2 * RcpPI;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * ((h - HarmonicNumber(2 * k)) * (1 - x2 * d[k]) + x2 * q[k]);

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

                ddouble h = Ldexp(Log(x / 2) + EulerGamma, 1);

                ddouble x2 = x * x, x4 = x2 * x2;

                ddouble c = -2 / (x * PI), u = x / (2 * PI);

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * ((h - HarmonicNumber(2 * k) - HarmonicNumber(2 * k + 1)) * (1 - x2 * d[k]) + x2 * q[k]);

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

                ddouble tp = Pow(x / 2, nu), tn = 1d / tp;

                ddouble x2 = x * x;

                ddouble c = 0d, u = PI / (2 * SinPI(nu));

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
                n = Math.Abs(n);
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
                ddouble h = -Log(x / 2) - EulerGamma;

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
                ddouble h = Log(x / 2) + EulerGamma;

                ddouble x2 = x * x;

                ddouble c = 1d / x, u = x / 2d;

                for (int k = 0, conv_times = 0; k <= terms && conv_times < 2; k++) {
                    ddouble dc = u * r[k] * (h - (HarmonicNumber(k) + HarmonicNumber(k + 1)) / 2);

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
                    this.c = Gamma(nu + 1);
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
                        c *= (nu + (2 * k)) * (nu + (2 * k - 1)) * checked(32 * k * (2 * k - 1));

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class X2DenomTable {
                private readonly ddouble nu;
                private readonly List<ddouble> table = new();

                public X2DenomTable(ddouble nu) {
                    ddouble a = Rcp(4 * (nu + 1));

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
                        ddouble a = Rcp(4 * (2 * k + 1) * (2 * k + nu + 1));

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
                    this.c = Gamma(nu + 1);
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
                        c *= checked(32 * k * (2 * k - 1));

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
                        ddouble c = Rcp(checked(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 1)));

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
                        ddouble c = (ddouble)(4 * k + 3) / (ddouble)checked(4 * (2 * k + 1) * (2 * k + 1) * (2 * k + 2) * (2 * k + 2));

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
                        c *= checked(4 * k);

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class K0CoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public K0CoefTable() {
                    this.c = 1;
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
                        c *= checked(4 * k * k);

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }

            private class K1CoefTable {
                private ddouble c;
                private readonly List<ddouble> table = new();

                public K1CoefTable() {
                    this.c = 1;
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
                        c *= checked(4 * k * (k + 1));

                        table.Add(Rcp(c));
                    }

                    return table[n];
                }
            }
        }

        private static class MillerBackward {
            private static Dictionary<ddouble, BesselJPhiTable> phi_table = new();
            private static Dictionary<ddouble, BesselIPsiTable> psi_table = new();
            private static Dictionary<ddouble, BesselYEtaTable> eta_table = new();
            private static Dictionary<ddouble, BesselYXiTable> xi_table = new();

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                int m = (int)Math.Ceiling(((double)x * 1.8d) + 44d) / 2 * 2;

                if (nu == Floor(nu)) {
                    int n = (int)Floor(nu);
                    ddouble y = BesselJKernel(n, x, m);

                    return y;
                }
                else {
                    ddouble y = BesselJKernel(nu, x, m);

                    return y;
                }
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                int m = (int)Math.Ceiling(((double)x * 1.8d) + 44d) / 2 * 2;

                if (nu == Floor(nu)) {
                    int n = (int)Floor(nu);
                    ddouble y = BesselYKernel(n, x, m);

                    return y;
                }
                else {
                    ddouble y = BesselYKernel(nu, x, m);

                    return y;
                }
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                int m = (int)Math.Ceiling(((double)x * 1.2d) + 48d) / 2 * 2;

                if (nu == Floor(nu)) {
                    int n = (int)Floor(nu);
                    ddouble y = BesselIKernel(n, x, m);

                    return y;
                }
                else {
                    ddouble y = BesselIKernel(nu, x, m, scale);

                    return y;
                }
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

                ddouble f0 = 1e-256, f1 = Zero, fn = Zero, lambda = Zero;
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

                lambda = 2 * lambda + f0;

                ddouble yn = fn / lambda;

                return yn;
            }

            private static ddouble BesselJKernel(ddouble nu, ddouble x, int m) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0) {
                    return BesselJKernel(n, x, m);
                }

                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!phi_table.ContainsKey(alpha)) {
                    phi_table.Add(alpha, new BesselJPhiTable(alpha));
                }

                BesselJPhiTable phi = phi_table[alpha];

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                if (n >= 0) {
                    ddouble fn = Zero;

                    for (int k = m; k >= 1; k--) {
                        if ((k & 1) == 0) {
                            lambda += f0 * phi[k / 2];
                        }

                        (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);

                        if (k - 1 == n) {
                            fn = f0;
                        }
                    }

                    lambda += f0 * phi[0];
                    lambda *= Pow(2 * v, alpha);

                    ddouble yn = fn / lambda;

                    return yn;
                }
                else {
                    for (int k = m; k >= 1; k--) {
                        if ((k & 1) == 0) {
                            lambda += f0 * phi[k / 2];
                        }

                        (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
                    }

                    lambda += f0 * phi[0];
                    lambda *= Pow(2 * v, alpha);

                    for (int k = 0; k > n; k--) {
                        (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
                    }

                    ddouble yn = f0 / lambda;

                    return yn;
                }
            }

            private static ddouble BesselJ0Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = 2 * lambda + f0;

                ddouble y0 = f0 / lambda;

                return y0;
            }

            private static ddouble BesselJ1Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = 2 * lambda + f0;

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

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble se = Zero, sx = Zero;
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

                lambda = 2 * lambda + f0;

                ddouble c = Log(x / 2) + EulerGamma;

                ddouble y0 = se + f0 * c;
                ddouble y1 = sx - v * f0 + (c - 1) * f1;

                for (int k = 1; k < n; k++) {
                    (y1, y0) = ((2 * k) * v * y1 - y0, y1);
                }

                ddouble yn = 2 * y1 / (lambda * PI);

                return yn;
            }

            private static ddouble BesselYKernel(ddouble nu, ddouble x, int m) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0) {
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

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble se = Zero, sxo = Zero, sxe = Zero;
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

                    (f0, f1) = ((2 * (k + alpha)) * v * f0 - f1, f0);
                }

                ddouble s = ddouble.Pow(2 * v, alpha), sqs = s * s;

                lambda += f0 * phi[0];
                lambda *= s;

                ddouble rcot = 1d / ddouble.TanPI(alpha), rgamma = ddouble.Gamma(1 + alpha), rsqgamma = rgamma * rgamma;
                ddouble r = 2 * ddouble.RcpPI * sqs;
                ddouble p = sqs * rsqgamma * ddouble.RcpPI;

                ddouble eta0 = rcot - p / alpha;
                ddouble xi0 = -2 * v * p;
                ddouble xi1 = rcot + p * (alpha * (alpha + 1) + 1) / (alpha * (alpha - 1));

                ddouble y0 = r * se + eta0 * f0;
                ddouble y1 = r * (3 * alpha * v * sxe + sxo) + xi0 * f0 + xi1 * f1;

                ddouble m0 = y0 / lambda;
                ddouble m1 = y1 / lambda;

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
                        (y1, y0) = ((2 * (k + alpha)) * v * y1 - y0, y1);
                    }

                    ddouble yn = y1 / lambda;

                    return yn;
                }
                else {
                    for (int k = 0; k > n; k--) {
                        (y0, y1) = ((2 * (k + alpha)) * v * y0 - y1, y0);
                    }

                    ddouble yn = y0 / lambda;

                    return yn;
                }
            }

            private static ddouble BesselY0Kernel(ddouble x, int m) {
                if (m < 2 || (m & 1) != 0) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!eta_table.ContainsKey(0)) {
                    eta_table.Add(0, new BesselYEtaTable(0));
                }

                BesselYEtaTable eta = eta_table[0];

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble se = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    if ((k & 1) == 0) {
                        lambda += f0;

                        se += f0 * eta[k / 2];
                    }

                    (f0, f1) = ((2 * k) * v * f0 - f1, f0);
                }

                lambda = 2 * lambda + f0;

                ddouble y0 = 2 * (se + f0 * (Log(x / 2) + EulerGamma)) / (PI * lambda);

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

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero;
                ddouble sx = Zero;
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

                lambda = 2 * lambda + f0;

                ddouble y1 = 2 * (sx - v * f0 + (Log(x / 2) + EulerGamma - 1) * f1) / (lambda * PI);

                return y1;
            }

            private static ddouble BesselIKernel(int n, ddouble x, int m, bool scale = false) {
                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (n < 0) {
                    return BesselIKernel(-n, x, m, scale);
                }
                if (n == 0) {
                    return BesselI0Kernel(x, m);
                }
                if (n == 1) {
                    return BesselI1Kernel(x, m);
                }

                ddouble f0 = 1e-256, f1 = Zero, lambda = Zero, fn = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += f0;

                    (f0, f1) = ((2 * k) * v * f0 + f1, f0);

                    if (k - 1 == n) {
                        fn = f0;
                    }
                }

                lambda = 2 * lambda + f0;

                ddouble yn = fn / lambda;

                if (!scale) {
                    yn *= Exp(x);
                }

                return yn;
            }

            private static ddouble BesselIKernel(ddouble nu, ddouble x, int m, bool scale = false) {
                int n = (int)Floor(nu);
                ddouble alpha = nu - n;

                if (alpha == 0) {
                    return BesselIKernel(n, x, m, scale);
                }

                if (m < 2 || (m & 1) != 0 || n >= m) {
                    throw new ArgumentOutOfRangeException(nameof(m));
                }

                if (!psi_table.ContainsKey(alpha)) {
                    psi_table.Add(alpha, new BesselIPsiTable(alpha));
                }

                BesselIPsiTable psi = psi_table[alpha];

                ddouble g0 = 1e-256, g1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                if (n >= 0) {
                    ddouble gn = Zero;

                    for (int k = m; k >= 1; k--) {
                        lambda += g0 * psi[k];

                        (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);

                        if (k - 1 == n) {
                            gn = g0;
                        }
                    }

                    lambda += g0 * psi[0];
                    lambda *= Pow(2 * v, alpha);

                    ddouble yn = gn / lambda;

                    if (!scale) {
                        yn *= Exp(x);
                    }

                    return yn;
                }
                else {
                    for (int k = m; k >= 1; k--) {
                        lambda += g0 * psi[k];

                        (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);
                    }

                    lambda += g0 * psi[0];
                    lambda *= Pow(2 * v, alpha);

                    for (int k = 0; k > n; k--) {
                        (g0, g1) = ((2 * (k + alpha)) * v * g0 + g1, g0);
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

                ddouble g0 = 1e-256, g1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += g0;

                    (g0, g1) = ((2 * k) * v * g0 + g1, g0);
                }

                lambda = 2 * lambda + g0;

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

                ddouble g0 = 1e-256, g1 = Zero, lambda = Zero;
                ddouble v = 1d / x;

                for (int k = m; k >= 1; k--) {
                    lambda += g0;

                    (g0, g1) = ((2 * k) * v * g0 + g1, g0);
                }

                lambda = 2 * lambda + g0;

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
                    if (!(alpha > 0) || alpha >= 1) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;

                    ddouble phi0 = Gamma(1 + alpha);
                    ddouble phi1 = phi0 * (alpha + 2);

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
                        g = g * (alpha + m - 1) / m;

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
                    if (!(alpha > 0) || alpha >= 1) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;

                    ddouble psi0 = Gamma(1 + alpha);
                    ddouble psi1 = 2 * psi0 * (1 + alpha);

                    this.g = 2 * psi0;

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
                        g = g * (2 * alpha + m - 1) / m;

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
                    if (!(alpha >= 0) || alpha >= 1) {
                        throw new ArgumentOutOfRangeException(nameof(alpha));
                    }

                    this.alpha = alpha;
                    this.table.Add(NaN);

                    if (alpha > 0) {
                        ddouble c = Gamma(1 + alpha);
                        c *= c;
                        this.g = 1 / (1 - alpha) * c;

                        ddouble eta1 = (alpha + 2) * g;

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
                        if (alpha > 0) {
                            g = -g * (alpha + m - 1) * (2 * alpha + m - 1) / (m * (m - alpha));

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
                    if (!(alpha >= 0) || alpha >= 1) {
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
                        if (alpha > 0) {
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

        private static class YoshidaPade {
            private static Dictionary<ddouble, (ddouble[] c, ddouble[] s)> table = new();

            static YoshidaPade() {
                table.Add(0, (cs0, ds0));
                table.Add(1, (cs1, ds1));
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
                nu = Abs(nu);

                if (nu < 2) {
                    if (!table.ContainsKey(nu)) {
                        table.Add(nu, Table(nu));
                    }

                    (ddouble[] cs, ddouble[] ds) = table[nu];

                    ddouble y = Value(x, cs, ds, scale);

                    return y;
                }
                else {
                    ddouble y0 = BesselK(nu, x, scale);
                    ddouble y1 = BesselK(nu + 1, x, scale);

                    int n = (int)Floor(nu);
                    ddouble alpha = nu - n;
                    ddouble v = 1d / x;

                    for (int k = 1; k < n; k++) {
                        (y1, y0) = ((2 * (k + alpha)) * v * y1 + y0, y1);
                    }

                    return y1;
                }
            }

            private static ddouble Value(ddouble x, ddouble[] cs, ddouble[] ds, bool scale = false) {
                ddouble t = 1 / x, tn = 1;
                ddouble c = 0, d = 0;

                for (int j = 0; j < cs.Length; j++) {
                    c += cs[j] * tn;
                    d += ds[j] * tn;
                    tn *= t;
                }

                ddouble y = Sqrt(t * PI / 2) * c / d;

                if (!scale) {
                    y *= Exp(-x);
                }

                return y;
            }

            private static (ddouble[] cs, ddouble[] ds) Table(ddouble nu) {
                int m = ess.Length - 1;

                ddouble squa_nu = nu * nu;
                ddouble[] cs = new ddouble[m + 1], ds = new ddouble[m + 1];
                ddouble[] us = new ddouble[m + 1], vs = new ddouble[m];

                ddouble u = 1;
                for (int i = 0; i <= m; i++) {
                    us[i] = u;
                    u *= squa_nu;
                }
                for (int i = 0; i < m; i++) {
                    ddouble r = m - i + 0.5d;
                    vs[i] = r * r - squa_nu;
                }

                for (int i = 0; i <= m; i++) {
                    ddouble d = ess[i][i], c = 0;

                    for (int l = 0; l < i; l++) {
                        d *= vs[l];
                    }
                    for (int j = 0; j <= i; j++) {
                        c += ess[i][j] * us[j];
                    }

                    cs[i] = c;
                    ds[i] = d;
                }

                return (cs, ds);
            }

            private static readonly ddouble[] cs0 = {
                (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
                (+1, 7, 0xF7F0000000000000uL, 0x0000000000000000uL),
                (+1, 14, 0xDD6A6C0000000000uL, 0x0000000000000000uL),
                (+1, 20, 0xF25D87C280000000uL, 0x0000000000000000uL),
                (+1, 26, 0xB65370AA1DC00000uL, 0x0000000000000000uL),
                (+1, 31, 0xC865B33C83FD6000uL, 0x0000000000000000uL),
                (+1, 36, 0xA6FB8CF186ADDD60uL, 0x0000000000000000uL),
                (+1, 40, 0xD81E15A5F4B7EF23uL, 0xE800000000000000uL),
                (+1, 44, 0xDCC7E0A04D90AA8BuL, 0x89F0000000000000uL),
                (+1, 48, 0xB3FCE26BC77F229CuL, 0xBB49500000000000uL),
                (+1, 51, 0xEBE09392A82A7362uL, 0x240AAA4000000000uL),
                (+1, 54, 0xF9889CB9B4D817CEuL, 0x2E85F65F40000000uL),
                (+1, 57, 0xD5838F919F885448uL, 0x314988A7F7000000uL),
                (+1, 60, 0x93C2B28F07C59BE9uL, 0x3F79FBBEDC2E0000uL),
                (+1, 62, 0xA5194227F6F1C09CuL, 0xE82DEF3868AEF000uL),
                (+1, 64, 0x94617400BA3CD3C1uL, 0xC50901ADC8D5C428uL),
                (+1, 65, 0xD5605E1152A9D47BuL, 0xF82F56E18B12F49AuL),
                (+1, 66, 0xF3B4640F2479A735uL, 0xFBC5A2259C17605FuL),
                (+1, 67, 0xDB03F5CCE3096FD6uL, 0x8C5C7F53CD694FE4uL),
                (+1, 68, 0x99128B36C8CD0E49uL, 0x072E59E2035E1E8CuL),
                (+1, 68, 0xA401A75C11EFAB39uL, 0x9B38562D039B8241uL),
                (+1, 68, 0x844C09F63919F5FDuL, 0x424E1D074E439EF6uL),
                (+1, 67, 0x9D284EFDBECE4F43uL, 0x2E3AADEC1DA6302AuL),
                (+1, 66, 0x85B03B8A0F83C7AFuL, 0xFB32A204D1C493EBuL),
                (+1, 64, 0x9D375A823865D673uL, 0xE3A4F56F17DB3A78uL),
                (+1, 61, 0xF4220AB37302692CuL, 0x2E3CB04456D385F3uL),
                (+1, 58, 0xEB6DDACD66F5BA2BuL, 0xC419D748D29DAE32uL),
                (+1, 55, 0x817DB7DC6FE27461uL, 0xDE0AE49EE74384D9uL),
                (+1, 50, 0x8F570CBBDFF40C75uL, 0x6EE095CDD1C868E2uL),
                (+1, 44, 0x826FBCE74C6166D8uL, 0x4D1DC39A4F983D63uL),
                (+1, 36, 0x8545ACD9B07E4B09uL, 0x512BDFCD504AAB31uL),
                (+1, 24, 0xDA091339D4BFEE07uL, 0x229A0236EC1DD7CCuL)
            };

            private static readonly ddouble[] ds0 = {
                (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
                (+1, 7, 0xF810000000000000uL, 0x0000000000000000uL),
                (+1, 14, 0xDDA84C0000000000uL, 0x0000000000000000uL),
                (+1, 20, 0xF2CBD0F580000000uL, 0x0000000000000000uL),
                (+1, 26, 0xB6CBDF7711C00000uL, 0x0000000000000000uL),
                (+1, 31, 0xC91A64D3D6B7A000uL, 0x0000000000000000uL),
                (+1, 36, 0xA7C18227A8A08E60uL, 0x0000000000000000uL),
                (+1, 40, 0xD966BAA31432E31FuL, 0x1800000000000000uL),
                (+1, 44, 0xDE6F5365095B7BCCuL, 0x5BF0000000000000uL),
                (+1, 48, 0xB5AB31C7046DAD10uL, 0x70D5300000000000uL),
                (+1, 51, 0xEE99D3437FDA9CE2uL, 0x7FAA1C4000000000uL),
                (+1, 54, 0xFD13A5B5DA74FCE8uL, 0x0B6558D4C0000000uL),
                (+1, 57, 0xD93AB875D89C8587uL, 0x125B5B8AE7000000uL),
                (+1, 60, 0x96E8128AD38469E1uL, 0x45CAD33188A20000uL),
                (+1, 62, 0xA9660CCA882A5A96uL, 0x17233C6B8B977000uL),
                (+1, 64, 0x991CF61DF32F9D8BuL, 0x53E9CB00185699D8uL),
                (+1, 65, 0xDDBA9EF6F235A271uL, 0x987F2909544437FAuL),
                (+1, 66, 0xFF73DFDBBA47E642uL, 0xAB1643EC1E718A65uL),
                (+1, 67, 0xE813583646D2F084uL, 0x63BA090160150E71uL),
                (+1, 68, 0xA46D6649D2E817EBuL, 0x943516F45CAB1743uL),
                (+1, 68, 0xB33EB33A26A82BEFuL, 0xB130F194CD348E33uL),
                (+1, 68, 0x93D2387354A1FB4EuL, 0xFEDCB78E453D4D12uL),
                (+1, 67, 0xB4ADEF00C3424C49uL, 0xB53E4F9CB9625788uL),
                (+1, 66, 0x9F84B1693ADC2219uL, 0xD72E74E59195C7DAuL),
                (+1, 64, 0xC5030F9F807D468FuL, 0x810194BFA1349C06uL),
                (+1, 62, 0xA35002D86DE11C2CuL, 0xDA18E3B9CC5E2305uL),
                (+1, 59, 0xAC23E752D4AF0205uL, 0xC34AAAD89924F47BuL),
                (+1, 55, 0xD64A3158E3E35753uL, 0x069986B84CDD693BuL),
                (+1, 51, 0x8DB1949CFF2CC9EFuL, 0xD87A69CC6928DE23uL),
                (+1, 45, 0xA8FF6B263822B9CEuL, 0x9D7467567627655FuL),
                (+1, 38, 0x88905695D5716504uL, 0x0D755617528C70F2uL),
                (+1, 28, 0x9E971A3A63395C88uL, 0xD5D295867061074BuL),
            };

            private static readonly ddouble[] cs1 = {
                (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
                (+1, 7, 0xF830000000000000uL, 0x0000000000000000uL),
                (+1, 14, 0xDDEBCC0000000000uL, 0x0000000000000000uL),
                (+1, 20, 0xF34E0A9C80000000uL, 0x0000000000000000uL),
                (+1, 26, 0xB764C7DCC9C00000uL, 0x0000000000000000uL),
                (+1, 31, 0xCA0FAE7C1901E000uL, 0x0000000000000000uL),
                (+1, 36, 0xA8DF6A81658A8660uL, 0x0000000000000000uL),
                (+1, 40, 0xDB5D96CA22870815uL, 0x0800000000000000uL),
                (+1, 44, 0xE11B2FE6BE943FD6uL, 0xBFF0000000000000uL),
                (+1, 48, 0xB8864A926527C567uL, 0x06C8100000000000uL),
                (+1, 51, 0xF37483D3CB9D5D7EuL, 0x85ACC44000000000uL),
                (+1, 55, 0x81D79F7185958C0BuL, 0x15FF7DC620000000uL),
                (+1, 57, 0xE076F23B10920071uL, 0x933F10C707000000uL),
                (+1, 60, 0x9D4AB8A239F3BDB3uL, 0x4B5A8B8610560000uL),
                (+1, 62, 0xB27B67A4239FCBBCuL, 0xC5A89ACCD45F7000uL),
                (+1, 64, 0xA381B664DFD4D514uL, 0x26D619718D784B48uL),
                (+1, 65, 0xF0C9D4F8B020559CuL, 0x876839AC7E51D3BAuL),
                (+1, 67, 0x8DA68162FBADA034uL, 0xBFA594AA72BE19DBuL),
                (+1, 68, 0x841FF7FFF20C1554uL, 0xC2D706DFF8AEFB3EuL),
                (+1, 68, 0xC1884DDD73F1B51FuL, 0x67373D7B904CE540uL),
                (+1, 68, 0xDBF7D0E12FF7F3C1uL, 0x56CD6FC59D6743C2uL),
                (+1, 68, 0xBF36C75CF2F97907uL, 0xA5805BC21D843C08uL),
                (+1, 67, 0xF9C681CF0388585BuL, 0xB509F8033CB5635DuL),
                (+1, 66, 0xEFCCC2839EE1E834uL, 0xB6733B409CFC75D9uL),
                (+1, 65, 0xA491DEC7167B43D1uL, 0x8B8537D98D002DEFuL),
                (+1, 63, 0x9BCA2B088D0E6804uL, 0x911CA07AEA60DD1AuL),
                (+1, 60, 0xC204D2467227C99CuL, 0x47A28D27B6F2D880uL),
                (+1, 57, 0x94F66BA6467C177BuL, 0x4DAF513CBA68CD09uL),
                (+1, 53, 0x808360F16DAE75E5uL, 0x94702E0027A0A715uL),
                (+1, 47, 0xD827CC083707545CuL, 0x599FD01886B5191AuL),
                (+1, 41, 0x8AE8433AF1363A3CuL, 0xD4D56E0C1C3DA67DuL),
                (+1, 32, 0xA11175F7960406E1uL, 0xFA852139D9FD253DuL),
            };

            private static readonly ddouble[] ds1 = {
                (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
                (+1, 7, 0xF7D0000000000000uL, 0x0000000000000000uL),
                (+1, 14, 0xDD322C0000000000uL, 0x0000000000000000uL),
                (+1, 20, 0xF20326DB80000000uL, 0x0000000000000000uL),
                (+1, 26, 0xB5FB5EB80DC00000uL, 0x0000000000000000uL),
                (+1, 31, 0xC7F13CB804C32000uL, 0x0000000000000000uL),
                (+1, 36, 0xA68CD355253AE360uL, 0x0000000000000000uL),
                (+1, 40, 0xD781B873EC6C72B5uL, 0xF800000000000000uL),
                (+1, 44, 0xDC210C5F6D9A8E55uL, 0xB9F0000000000000uL),
                (+1, 48, 0xB375C1E6D5B009E3uL, 0xE366700000000000uL),
                (+1, 51, 0xEB3C00244BEF019CuL, 0xEEBCEE4000000000uL),
                (+1, 54, 0xF8F75B32F26880F9uL, 0x10C949C7C0000000uL),
                (+1, 57, 0xD5314DDC04C0DC3EuL, 0xB07DEF19B7000000uL),
                (+1, 60, 0x93B67661F652342BuL, 0x10D8240E9F7A0000uL),
                (+1, 62, 0xA55439128B0B5AB7uL, 0x62B09C9A29ADF000uL),
                (+1, 64, 0x94F2609E0177766DuL, 0x6A02C737D20422B8uL),
                (+1, 65, 0xD6E73D1A7118471FuL, 0xE8460D0D79918A9AuL),
                (+1, 66, 0xF68EE7ECB861F8B8uL, 0xADC80585C82E6815uL),
                (+1, 67, 0xDEEE02C3E6B67304uL, 0xD8DC51CB18327E1BuL),
                (+1, 68, 0x9D14AEECAD5706BFuL, 0x7DEEFE2665788DDFuL),
                (+1, 68, 0xAA23F27426519C4BuL, 0x8AEF1132389F99ABuL),
                (+1, 68, 0x8B4077F5EB8D7603uL, 0xEC609E1302273257uL),
                (+1, 67, 0xA8A9548F523B5F66uL, 0xAAEDED6FDB4CB52BuL),
                (+1, 66, 0x93420881DD272D61uL, 0xA3918AEC4E25F6F1uL),
                (+1, 64, 0xB35A457038050C9BuL, 0x0593B99C8690B354uL),
                (+1, 62, 0x9207E20950A4B7ABuL, 0x7DF0546E9A4C0C5AuL),
                (+1, 59, 0x9647FAB606E1ED72uL, 0xBECF6873F36967B8uL),
                (+1, 55, 0xB4E4DD307094B0C7uL, 0xBFC0C4D76C1B522FuL),
                (+1, 50, 0xE368DB8D5A548677uL, 0x74B7D2E2EE4E3B6EuL),
                (+1, 44, 0xF916F18589E31306uL, 0xBD1E8240BE675876uL),
                (+1, 37, 0xA9143A705D989572uL, 0xBB54523541A1B069uL),
                (+1, 26, 0xDA2A9DFC57BC9FCDuL, 0xD8F0EE3433A7573DuL),
            };

            private static readonly ddouble[][] ess = {
                new ddouble[1]{
                    (+1, 0, 0x8000000000000000uL, 0x0000000000000000uL),
                },
                new ddouble[2]{
                    (+1, 7, 0xF7F0000000000000uL, 0x0000000000000000uL),
                    (+1, -2, 0x8000000000000000uL, 0x0000000000000000uL),
                },
                new ddouble[3]{
                    (+1, 14, 0xDD6A6C0000000000uL, 0x0000000000000000uL),
                    (+1, 6, 0x81504325C53EF368uL, 0xEB04325C53EF368EuL),
                    (+1, -6, 0xFBCDA3AC10C9714FuL, 0xBCDA3AC10C9714FBuL),
                },
                new ddouble[4]{
                    (+1, 20, 0xF25D87C280000000uL, 0x0000000000000000uL),
                    (+1, 12, 0xF040AF76E837F4CFuL, 0x09CAD76E837F4CF0uL),
                    (+1, 3, 0x844AEDD06FE99E13uL, 0x95AEDD06FE99E139uL),
                    (+1, -9, 0xA245F202CC3D8D4AuL, 0x245F202CC3D8D4A2uL),
                },
                new ddouble[5]{
                    (+1, 26, 0xB65370AA1DC00000uL, 0x0000000000000000uL),
                    (+1, 19, 0x886BD4EC023A907DuL, 0x91D78CDCE29355F3uL),
                    (+1, 9, 0xFEE56D5A746AE2D0uL, 0x08B9B2F109EF42B2uL),
                    (+1, -1, 0xB0E0D22B02ADE42AuL, 0x03BF08A77D4F12E5uL),
                    (+1, -13, 0x9A05A49EDBE3A47AuL, 0x709FB6673C0EE3DBuL),
                },
                new ddouble[6]{
                    (+1, 31, 0xC865B33C83FD6000uL, 0x0000000000000000uL),
                    (+1, 24, 0xD467C74B33DE0296uL, 0xD36DECF72318918FuL),
                    (+1, 16, 0x95AC6BC1CD11FF01uL, 0x11247D79900A3E2BuL),
                    (+1, 6, 0xB03879F761021EA4uL, 0xA75B515C07745B1AuL),
                    (+1, -5, 0xADBBB6E0A19BFD70uL, 0x753942A64DD13225uL),
                    (+1, -18, 0xE570925BDF64C394uL, 0xD9348989E4F11AEEuL),
                },
                new ddouble[7]{
                    (+1, 36, 0xA6FB8CF186ADDD60uL, 0x0000000000000000uL),
                    (+1, 29, 0xF0FE0D3A3534AFB6uL, 0x5BFD4BA7D1956FDDuL),
                    (+1, 21, 0xF04FEFFF0045199AuL, 0xF9445BD7DAAF3B24uL),
                    (+1, 12, 0xD55DCDE6BCF78E6FuL, 0xAA9E768E10C90F2BuL),
                    (+1, 2, 0xB288AD479826E492uL, 0x9B8292B46DA63C5FuL),
                    (+1, -9, 0x859C93F89F114DC3uL, 0xD79DDBBE026FFA9CuL),
                    (+1, -22, 0x8B8AD3C61914DCC0uL, 0x5431EADA9A35D180uL),
                },
                new ddouble[8]{
                    (+1, 40, 0xD81E15A5F4B7EF23uL, 0xE800000000000000uL),
                    (+1, 34, 0xCEC7390B3CA189FEuL, 0x6C228D29C8EB45EAuL),
                    (+1, 27, 0x8C2FDD287D50CA3FuL, 0x81925DB9BC34BF25uL),
                    (+1, 18, 0xB00E4E0215A08968uL, 0xECEEE518041A6699uL),
                    (+1, 8, 0xDE3D4B5DFC377938uL, 0xB06D0B07D7903D72uL),
                    (+1, -2, 0x8D47C1A3DB58ACDCuL, 0x493B7B4C18A75987uL),
                    (+1, -14, 0xA776CCBE9DA22CB5uL, 0x26CFADFF51927748uL),
                    (+1, -27, 0x8E63DD51FA3F1580uL, 0x55E9CB134EFD7282uL),
                },
                new ddouble[9]{
                    (+1, 44, 0xDCC7E0A04D90AA8BuL, 0x89F0000000000000uL),
                    (+1, 39, 0x89726A301D7C221FuL, 0x06B807364594E809uL),
                    (+1, 31, 0xF6ABC98850172D1BuL, 0x27C45B4D09172B61uL),
                    (+1, 23, 0xD27670F3426C3E15uL, 0xE1976DCCF8F03DA0uL),
                    (+1, 14, 0xBBE64BF664AE5219uL, 0xFF77A8B3D31DC438uL),
                    (+1, 4, 0xB452D94EF1B7B194uL, 0xCF08A68BFEEF5680uL),
                    (+1, -7, 0xB5C98E933FC2F225uL, 0xDA11B7A5B64DBC49uL),
                    (+1, -19, 0xAFBD931E19238A18uL, 0xEA0926EB64174A17uL),
                    (+1, -33, 0xF889129865AA9E8CuL, 0x38DDE4C9440E299EuL),
                },
                new ddouble[10]{
                    (+1, 48, 0xB3FCE26BC77F229CuL, 0xBB49500000000000uL),
                    (+1, 43, 0x8FDC4940C88D5318uL, 0xBBF23D5C6E396CE5uL),
                    (+1, 36, 0xA7A03AEBDFCB7D2AuL, 0x03C9BF7BCC136E52uL),
                    (+1, 28, 0xBD1BB45B3F5F2481uL, 0x5593004C56057CF3uL),
                    (+1, 19, 0xE55E20E2AE9C9E44uL, 0x8CE8B66F9252E4AAuL),
                    (+1, 10, 0x9BC26EF472DF97BCuL, 0x05C0F1A06DD76169uL),
                    (+1, -1, 0xED4C85539ABD2E5EuL, 0x0F3C79DA57267481uL),
                    (+1, -12, 0xC36BF27C82D7CC0EuL, 0x52D4C6316105B2A9uL),
                    (+1, -24, 0x9D738514BA36826DuL, 0xF11A05534024A194uL),
                    (+1, -38, 0xBC31027DEE2ACE70uL, 0x7F56E6204CD0C813uL),
                },
                new ddouble[11]{
                    (+1, 51, 0xEBE09392A82A7362uL, 0x240AAA4000000000uL),
                    (+1, 46, 0xEFAEB1E12DA14396uL, 0x6432D7C6235BD9DCuL),
                    (+1, 40, 0xB2CF24D8F01E3DB2uL, 0xAC0AD1B80FBFC983uL),
                    (+1, 33, 0x82CCF2B9FE464810uL, 0x6D3FCEB49A94A931uL),
                    (+1, 24, 0xD1B0EEB8AB4A4896uL, 0x40873FC7C337F96CuL),
                    (+1, 15, 0xC1841D040EE5E017uL, 0xC652D3A5F11BE0D9uL),
                    (+1, 5, 0xD0CFA422BFF5443CuL, 0x9C89F468D2A2BB56uL),
                    (+1, -5, 0x822034584D76592EuL, 0xD804B80138C43D3DuL),
                    (+1, -17, 0xB2FA37A3FA90A9ABuL, 0x299D20573F2EAB4BuL),
                    (+1, -30, 0xF45EB801EDE70773uL, 0x89F68193947CEC0AuL),
                    (+1, -44, 0xF9F99CE8F6CDA12FuL, 0xFF1F61FA9A3345A7uL),
                },
                new ddouble[12]{
                    (+1, 54, 0xF9889CB9B4D817CEuL, 0x2E85F65F40000000uL),
                    (+1, 50, 0xA00878AFA9CC5DD3uL, 0x49BB1E557C844A48uL),
                    (+1, 44, 0x974E5244A57BBD43uL, 0xDB3DAEEA14CDF22BuL),
                    (+1, 37, 0x8D803A9E2C0F3515uL, 0xD0C6B2EF264C13C8uL),
                    (+1, 29, 0x9300AEA0762EF5ABuL, 0x66C4CD63BDFE0C6BuL),
                    (+1, 20, 0xB357F9CD521062F8uL, 0x317CFB964F1E9977uL),
                    (+1, 11, 0x8399132A3D60C8D1uL, 0x59E77205BC870434uL),
                    (+1, 0, 0xE8A4B069C23F44F5uL, 0xD78D270A3D1D6F52uL),
                    (+1, -11, 0xF294FD080F59E06FuL, 0x85894DB3C9A8FE9EuL),
                    (+1, -22, 0x8DB0DFD15182CD06uL, 0x472E21E59FC8436CuL),
                    (+1, -35, 0xA6109C73E914BDFAuL, 0x794851C46B6EEEDFuL),
                    (+1, -49, 0x92D6A75C173DB667uL, 0x61F235F93FB9DDBAuL),
                },
                new ddouble[13]{
                    (+1, 57, 0xD5838F919F885448uL, 0x314988A7F7000000uL),
                    (+1, 53, 0xABFC8DC67A674C56uL, 0x9445ADB0634A4E35uL),
                    (+1, 47, 0xCC80DE47998E606DuL, 0xF8C2ED2AF97BA63FuL),
                    (+1, 40, 0xF1E8E6F0A4CC1E0EuL, 0x8365E45FD256E313uL),
                    (+1, 33, 0xA084DE0925EF0E3EuL, 0x48E29DA499448597uL),
                    (+1, 24, 0xFDD19B124A29DC1FuL, 0xBB36CC85827BB0BBuL),
                    (+1, 15, 0xF660A9F95D393791uL, 0x8911F77AD5F6CD01uL),
                    (+1, 6, 0x944664CBF3CAFEE3uL, 0x38D5ED1757D6944CuL),
                    (+1, -5, 0xDBAD8F581CD4230DuL, 0x2669B07CB3061164uL),
                    (+1, -16, 0xC2FC6924DAD65F09uL, 0xB4EBB0B8F6A17536uL),
                    (+1, -28, 0xC4121A4BEE93A4CAuL, 0xBCE63D5AE38D4F51uL),
                    (+1, -41, 0xC751B0AF3C09E401uL, 0x0EB2735AC51046F2uL),
                    (+1, -55, 0x998E7CD214F53DE9uL, 0x9A4BEED274D672BFuL),
                },
                new ddouble[14]{
                    (+1, 60, 0x93C2B28F07C59BE9uL, 0x3F79FBBEDC2E0000uL),
                    (+1, 56, 0x94FFC22C1E3DDC12uL, 0x8EB31122221A26DEuL),
                    (+1, 50, 0xDD91AEDB9BA60E82uL, 0xA68E8739F75641C7uL),
                    (+1, 44, 0xA46FE23886C341D6uL, 0x58958DDCFDD289F9uL),
                    (+1, 37, 0x89E00217A608BBBEuL, 0x9336B3821F082090uL),
                    (+1, 29, 0x8B3B8D0C57228116uL, 0xE2467289134D0B70uL),
                    (+1, 20, 0xAF4472FF294BF0C3uL, 0xF6C5C140B7D652E1uL),
                    (+1, 11, 0x8BB1347C19A5AB34uL, 0xA2BBAC4E6109A3E0uL),
                    (+1, 1, 0x8D243E0D2ABF0D4DuL, 0xF1CD1119A314CA3AuL),
                    (+1, -10, 0xB2611A795B553E59uL, 0x687EB48CB85D7278uL),
                    (+1, -21, 0x88A5E067BD110DDFuL, 0x016F992BC4600A6FuL),
                    (+1, -34, 0xEF2245F8AD96D8CFuL, 0x949CF238D61BA0F0uL),
                    (+1, -47, 0xD4A2A904C72A90B7uL, 0x0A69A1CD9A03AF2BuL),
                    (+1, -61, 0x8FA26CE0DD40C48BuL, 0xBD39A2BCFB12A278uL),
                },
                new ddouble[15]{
                    (+1, 62, 0xA5194227F6F1C09CuL, 0xE82DEF3868AEF000uL),
                    (+1, 58, 0xD006890607B68DB2uL, 0xA2B3052445DDEE81uL),
                    (+1, 53, 0xC0AA90C956F64B38uL, 0xF94E57F9B2F0ED48uL),
                    (+1, 47, 0xB259018E8EDB75DAuL, 0x1E0C260C812FF89FuL),
                    (+1, 40, 0xBB6942DB17AF0F50uL, 0x7BC767360817CE1DuL),
                    (+1, 32, 0xEF16F5BDA8CF8E9EuL, 0xF892F99C1C661D6BuL),
                    (+1, 24, 0xC04DE637A4448C7EuL, 0xFA8D3250A837C1EDuL),
                    (+1, 15, 0xC6FA3D5BB20BD541uL, 0x9677F2CFA6E04021uL),
                    (+1, 6, 0x85558CCBAB7EC87DuL, 0xEB680A193803C595uL),
                    (+1, -5, 0xE64F4F17060D4936uL, 0x7085AB2A2FC372EEuL),
                    (+1, -16, 0xFBD9A8766B2E4B72uL, 0x918A72D002259E3FuL),
                    (+1, -27, 0xA865367193C471C3uL, 0xAA7888E039C3555BuL),
                    (+1, -39, 0x815C01C9C137BE1CuL, 0x2F491372BE164BD4uL),
                    (+1, -53, 0xCA9C6FF20C447A97uL, 0xC04B37AF4E304177uL),
                    (+1, -68, 0xF13495FFF56A8683uL, 0xAABBD7D122449C67uL),
                },
                new ddouble[16]{
                    (+1, 64, 0x94617400BA3CD3C1uL, 0xC50901ADC8D5C428uL),
                    (+1, 60, 0xE977FEDC995B9A67uL, 0xF75A3456CF8669C7uL),
                    (+1, 56, 0x86527142CBA895A3uL, 0xC859F4C50131345AuL),
                    (+1, 50, 0x9A6A86F0C78F798BuL, 0xCEA382C9A6675B40uL),
                    (+1, 43, 0xCA1B5E3FD01093A3uL, 0xDE15AA508F4065C5uL),
                    (+1, 36, 0xA1837C3DA960749BuL, 0x544E34709D13D85AuL),
                    (+1, 28, 0xA42F104EBC5A0C3EuL, 0x952057CDEAC3BFB9uL),
                    (+1, 19, 0xD950441CAAA5EDB3uL, 0x373F84A5DEB0F0E3uL),
                    (+1, 10, 0xBD55E4AE734A2A81uL, 0x7389E4ACF45435F8uL),
                    (+1, 0, 0xD9550F10094D2AF6uL, 0x468212F978F60381uL),
                    (+1, -10, 0xA2CE9778574EB8BDuL, 0x0B990B0506F65707uL),
                    (+1, -21, 0x9BDA50109C0785DEuL, 0x5021BCAB0A91C64DuL),
                    (+1, -33, 0xB79EB68A622AA09AuL, 0x18ABE77E68D46DA5uL),
                    (+1, -46, 0xF981CD82A5B935B3uL, 0xE3051F9941B342AFuL),
                    (+1, -59, 0xAD0EC15BB4A0EC1BuL, 0xAAE6F4022635C060uL),
                    (+1, -74, 0xB63E7CB603618D74uL, 0x8C606A2C3C06540AuL),
                },
                new ddouble[17]{
                    (+1, 65, 0xD5605E1152A9D47BuL, 0xF82F56E18B12F49AuL),
                    (+1, 62, 0xD1BAA01E7259B668uL, 0x4ECA414F8E06F01BuL),
                    (+1, 58, 0x95B23CA4B0424ABCuL, 0x4E9AF209CA4C7BA6uL),
                    (+1, 52, 0xD51448B55B54228CuL, 0xBC2101E63246FE5AuL),
                    (+1, 46, 0xACE63B9C5FB19847uL, 0x22847E5DCB05221DuL),
                    (+1, 39, 0xAC06CE9030337024uL, 0xD29BFC822F56EB63uL),
                    (+1, 31, 0xDB2B06034042F44EuL, 0x1354A108ED0B70ACuL),
                    (+1, 23, 0xB77FD7C1E6329D19uL, 0x757C4DAAB0FEC3E5uL),
                    (+1, 14, 0xCCD3195102A6244EuL, 0xDEB1F1D619307336uL),
                    (+1, 5, 0x9925F6152E3013B0uL, 0xD3A8A4BD591791E6uL),
                    (+1, -5, 0x98D98EF8C6909518uL, 0x03B5888869258897uL),
                    (+1, -16, 0xC90D5E2FDD8203E9uL, 0x1F1B076CEB01207AuL),
                    (+1, -27, 0xAA24B2541723CB6BuL, 0xDBD367D8D6133964uL),
                    (+1, -39, 0xB200B6CCC209BD80uL, 0xDDCBE4905C342086uL),
                    (+1, -52, 0xD73ECB0E16F7EC44uL, 0xE2355E4EBF4E5FD6uL),
                    (+1, -65, 0x84D37678870FFF46uL, 0xD219EB8A4CBC01ABuL),
                    (+1, -81, 0xF82983B10A0D01FBuL, 0x4CC4A65CEA3F15D7uL),
                },
                new ddouble[18]{
                    (+1, 66, 0xF3B4640F2479A735uL, 0xFBC5A2259C17605FuL),
                    (+1, 64, 0x95DCD2EA63BA3DBFuL, 0x4F8B07F67A86DEF1uL),
                    (+1, 60, 0x84A646F376321DBDuL, 0x90DFB40761B616F1uL),
                    (+1, 54, 0xE95CC08395A4C553uL, 0x4EDDF3178B470CEBuL),
                    (+1, 48, 0xEA0CC566DD1355F0uL, 0x7217AA8C1627C35EuL),
                    (+1, 42, 0x904C9C9E66E52D7AuL, 0x750D509F1BDC4CDAuL),
                    (+1, 34, 0xE4F961D1BD379341uL, 0x2588C41A61465216uL),
                    (+1, 26, 0xF0851ACB112DA02BuL, 0xBD05ED6ABBAA3429uL),
                    (+1, 18, 0xAA1665963980D709uL, 0xD3B0D899AB5E8781uL),
                    (+1, 9, 0xA3420557230A656DuL, 0x5C51396B86C31E26uL),
                    (+1, -1, 0xD4CA16A1348E052BuL, 0x7D2B074998AAEC1BuL),
                    (+1, -11, 0xBAFCAA618192E5B0uL, 0x27DF10ABF20F9AB6uL),
                    (+1, -22, 0xDA281C237EB2A596uL, 0xFC396E2266E99838uL),
                    (+1, -33, 0xA4938925D359FE53uL, 0x4DD759247A1DE605uL),
                    (+1, -45, 0x99EBEE8875B3C9D8uL, 0x9AE353370AADEEA2uL),
                    (+1, -58, 0xA67E472D81526497uL, 0xFDBD3527F827BD1EuL),
                    (+1, -72, 0xB77A6BFF68986F68uL, 0x1D5973D4E67D1192uL),
                    (+1, -87, 0x9853192E7817B2A6uL, 0x09258E0DD1E92D7DuL),
                },
                new ddouble[19]{
                    (+1, 67, 0xDB03F5CCE3096FD6uL, 0x8C5C7F53CD694FE4uL),
                    (+1, 65, 0xA8F074DF7CE4B697uL, 0x7686D04E83F4A0C1uL),
                    (+1, 61, 0xB98B387B00FB76F7uL, 0xD6C82BCF445EDDE3uL),
                    (+1, 56, 0xC98F4DC604A6D381uL, 0xDBF3828618A64164uL),
                    (+1, 50, 0xF96154F007C5CEEFuL, 0x64568A9FECD19F04uL),
                    (+1, 44, 0xBDF1B02CADE8E494uL, 0x16AFFF197AAD8783uL),
                    (+1, 37, 0xBAD899FB5D892CCCuL, 0xF7AC7BF75BBFDB33uL),
                    (+1, 29, 0xF4B7565E44545746uL, 0x5387F01739C008EDuL),
                    (+1, 21, 0xD9778D634FA40CCCuL, 0x1444A253D888B061uL),
                    (+1, 13, 0x8483E3BBD813D4E6uL, 0x067A6A771DE89EBBuL),
                    (+1, 3, 0xDE462DAF3341F322uL, 0x784F4F085A1FD0E4uL),
                    (+1, -7, 0xFFD0B4A8D4F84CD9uL, 0x512B7331827CF2E0uL),
                    (+1, -17, 0xC80D5FF1F99FF10DuL, 0x9F2E81D3C2B5923AuL),
                    (+1, -28, 0xD0DB8D6BE61A05B3uL, 0xDA773E0CBB0CAC93uL),
                    (+1, -39, 0x8D79F4CA538DFF24uL, 0xD04586BFAA9BFA8DuL),
                    (+1, -52, 0xEDF1A97589A84F4DuL, 0x4B6C1DFA17C15065uL),
                    (+1, -65, 0xE73AF20BA9C05969uL, 0x9DF4B0B8FCC0A273uL),
                    (+1, -79, 0xE429FF521D4BA13FuL, 0x77E59F0B71F2088EuL),
                    (+1, -94, 0xA87F519560D04223uL, 0xC0CB5C07B3BBC598uL),
                },
                new ddouble[20]{
                    (+1, 68, 0x99128B36C8CD0E49uL, 0x072E59E2035E1E8CuL),
                    (+1, 66, 0x949E57DAE224A0B4uL, 0x3C803B03345A4305uL),
                    (+1, 62, 0xCAD5157A1C9D12B5uL, 0xF227610F95194BEEuL),
                    (+1, 58, 0x8816F5029BDE5304uL, 0x6C7FFCDEFFDE7ECBuL),
                    (+1, 52, 0xCF89AF3D449C62D2uL, 0x012848A16A958D8EuL),
                    (+1, 46, 0xC2E9916D204D86B8uL, 0xA9CB18E4814C5542uL),
                    (+1, 39, 0xECFCEB1A25EF8596uL, 0xDB9659E3DCEE8DA2uL),
                    (+1, 32, 0xC0A5960765D9E304uL, 0x0DC845C0C29DFC8CuL),
                    (+1, 24, 0xD5D33897DCE493CAuL, 0x840D17A0D09D0627uL),
                    (+1, 16, 0xA41743E93CCE1278uL, 0x692238FB242001D8uL),
                    (+1, 7, 0xAF2F21222ECE4633uL, 0x3785D741DA9070BFuL),
                    (+1, -2, 0x821DEA0C15F859B6uL, 0xB72080929D148827uL),
                    (+1, -12, 0x85B77448B10735C3uL, 0x1980B284B4F58769uL),
                    (+1, -23, 0xBBDC4A5165424BD2uL, 0x969C37EDB49EA41DuL),
                    (+1, -34, 0xB0DF54534F2C75C3uL, 0xDFB3CD9AD46F9CEDuL),
                    (+1, -46, 0xD887C4A83E1B8338uL, 0xC0562FF7DA21BCBBuL),
                    (+1, -58, 0xA48BA30A19436E47uL, 0x4FF5255669C0BEB3uL),
                    (+1, -71, 0x902D9698BF7DC618uL, 0x2948C537090BA9E5uL),
                    (+1, -86, 0xFF4824E4AB0317D9uL, 0x041462AAAFD8661BuL),
                    (+1, -101, 0xA7B0EDFA5419FAC7uL, 0xB74C394B089D79EFuL),
                },
                new ddouble[21]{
                    (+1, 68, 0xA401A75C11EFAB39uL, 0x9B38562D039B8241uL),
                    (+1, 66, 0xC95040CFC6CB8804uL, 0xB4AC8D984B98B205uL),
                    (+1, 63, 0xAB1F3FF651CC575FuL, 0x70B8E0A0FBE2D648uL),
                    (+1, 59, 0x8DFF3A2EE27DFDD3uL, 0xA55D9AC954DA6AA5uL),
                    (+1, 54, 0x857BF06C6784A0EAuL, 0xC8FE4148E28D2827uL),
                    (+1, 48, 0x9A76EB4A9DD8D9B4uL, 0xFA6B7778CF317108uL),
                    (+1, 41, 0xE7BAA7BBC4A7DCDEuL, 0xE2D5556A199DA75DuL),
                    (+1, 34, 0xE92706766F7E98ADuL, 0xAC17D3B63B78529DuL),
                    (+1, 27, 0xA0EC891E386D3FE6uL, 0xD3953E88BEF0E587uL),
                    (+1, 19, 0x9A9B466415650681uL, 0x15D95F3A21FE0F9BuL),
                    (+1, 10, 0xD06F8DFFCEACB543uL, 0xADB0BFE927B7B121uL),
                    (+1, 1, 0xC5AE74DE4DDBD8AFuL, 0x46EB8704395D9555uL),
                    (+1, -8, 0x838EEB852C3776EAuL, 0x90E5862669F727D6uL),
                    (+1, -19, 0xF3D348B96F95F125uL, 0xA446EA727987F280uL),
                    (+1, -29, 0x9B216A58D95412D9uL, 0x1A8D7ACA6B04283BuL),
                    (+1, -40, 0x84A1BD38DD6230C6uL, 0xEB00BF1670F18A26uL),
                    (+1, -52, 0x938EC17C9BED86DDuL, 0x3600D346145843B8uL),
                    (+1, -65, 0xCB8D091908B611C3uL, 0x1F9A415883527D21uL),
                    (+1, -78, 0xA14C3CFED062A994uL, 0xBB9E52669566D1D8uL),
                    (+1, -92, 0x8047F02CA7B7DC88uL, 0xA23BD2ACA8CC9EE4uL),
                    (+1, -108, 0x95C08850AA5C42C7uL, 0xC899CCC39A263AE1uL),
                },
                new ddouble[22]{
                    (+1, 68, 0x844C09F63919F5FDuL, 0x424E1D074E439EF6uL),
                    (+1, 66, 0xCE6CD805609519C5uL, 0xFD08B099254BCABBuL),
                    (+1, 63, 0xDB51C18A5CA9D831uL, 0x459665A908370DE1uL),
                    (+1, 59, 0xE1952496B36F492EuL, 0x6FCDEBA44DAB8468uL),
                    (+1, 55, 0x82E32A56AD8A9F40uL, 0x046294E8ED2A88A2uL),
                    (+1, 49, 0xBAB1F4451145D308uL, 0xB69751FA107AFC93uL),
                    (+1, 43, 0xACB418B12B2FE21EuL, 0xF98E5A0E8FED3DA5uL),
                    (+1, 36, 0xD6BF6184825FD1D5uL, 0xA1CC382E3E331181uL),
                    (+1, 29, 0xB7D9D466342B281DuL, 0xC50FFEFBD79BAA78uL),
                    (+1, 21, 0xDC3E3A77D1F329E7uL, 0x5446CF462D78E79BuL),
                    (+1, 13, 0xBA6912F4824D9436uL, 0x6229C39ECED14B07uL),
                    (+1, 4, 0xDFFBDA0534009998uL, 0x239CF1068BDB3D80uL),
                    (+1, -5, 0xBF0656339310FE93uL, 0xE2FE5CCB924B60D5uL),
                    (+1, -15, 0xE62F3B919123E376uL, 0xD90A7F5D3A1CFC20uL),
                    (+1, -25, 0xC20CC7B32738BF7BuL, 0xBC993626F6F06125uL),
                    (+1, -36, 0xE1543058CDF6BE96uL, 0x0496322BA73D284AuL),
                    (+1, -47, 0xB00C11A28630BD46uL, 0xBD4ED4D7BDC33AD2uL),
                    (+1, -59, 0xB2E812A7468E3870uL, 0xBDE1E7409DA6011EuL),
                    (+1, -72, 0xE0DFEAAE2DA80F77uL, 0x7FAEA12BD0991DE2uL),
                    (+1, -85, 0xA18BD4474C6A16ADuL, 0x49D5FBB867F6FB8BuL),
                    (+1, -100, 0xE6F6F6E02649F3B2uL, 0xDF805FD3497B556CuL),
                    (+1, -116, 0xEF0F5CCD65DF53F7uL, 0x870DE718E825CF31uL),
                },
                new ddouble[23]{
                    (+1, 67, 0x9D284EFDBECE4F43uL, 0x2E3AADEC1DA6302AuL),
                    (+1, 66, 0x9CE2EAC42BB10364uL, 0x3381BB5F39C5E19CuL),
                    (+1, 63, 0xD14A84543339E605uL, 0x33A07F63B417B094uL),
                    (+1, 60, 0x85DD9FD9413D2CD7uL, 0xAC8BA121F1AE1C77uL),
                    (+1, 55, 0xC03C0C2C807B6E4FuL, 0x2780856CEAE37172uL),
                    (+1, 50, 0xA94491CFE7CA0FB1uL, 0xD588964F2F840E02uL),
                    (+1, 44, 0xC13FF12339869A0DuL, 0x33062C6FB67707B6uL),
                    (+1, 38, 0x94765D44BEB480BAuL, 0x75F714F466FCE361uL),
                    (+1, 31, 0x9D78847F78E3DE32uL, 0xA23D4AB02D58D822uL),
                    (+1, 23, 0xEAA9C86635372768uL, 0x3F281EFCA1695964uL),
                    (+1, 15, 0xF87353F930F85DAAuL, 0xDFFC72CF69CB7DC5uL),
                    (+1, 7, 0xBC1442CE17CE891FuL, 0xB737F9F4D31405FDuL),
                    (+1, -2, 0xCBFFF13FC58F9AC3uL, 0x6BAFAF45A37F1342uL),
                    (+1, -11, 0x9E2A6D8F38B5649AuL, 0x8AAF4BF824B2AE11uL),
                    (+1, -21, 0xAE2B7CB7C8BF8E04uL, 0x038094E8A23F6006uL),
                    (+1, -31, 0x86A80693C3185ADDuL, 0xBD0AA03B9B7E5409uL),
                    (+1, -42, 0x8FAE065FCDC297F0uL, 0xC537C32EC2929D28uL),
                    (+1, -54, 0xCE5DA70CF6549DE9uL, 0xAE2592D2A27CEA05uL),
                    (+1, -66, 0xC0798B8396500F14uL, 0x6D774EF1A927BC16uL),
                    (+1, -79, 0xDD360CC49CF2727EuL, 0x7E0D3528EBAFDCD1uL),
                    (+1, -92, 0x905917D03C94D4A5uL, 0x395CAFFC2A7C2322uL),
                    (+1, -107, 0xB977B76C6375146CuL, 0x1EABD2CC951651E8uL),
                    (+1, -123, 0xA99F0D0E9D6D1346uL, 0x9E43C330E44A81FBuL),
                },
                new ddouble[24]{
                    (+1, 66, 0x85B03B8A0F83C7AFuL, 0xFB32A204D1C493EBuL),
                    (+1, 65, 0xAC2342DA4CE375CBuL, 0x1ED7D50B9B7D0D58uL),
                    (+1, 63, 0x910441FC712744DFuL, 0xCFA61BAC9E17513FuL),
                    (+1, 59, 0xE7CF5F617345B58EuL, 0x8D90A88842BBF2DEuL),
                    (+1, 55, 0xCEC09D47D0D2DF07uL, 0xC40BA58B7288F960uL),
                    (+1, 50, 0xE16E16B7BA731558uL, 0x2CBE03C8DEA7D78DuL),
                    (+1, 45, 0x9F27AD8C710A2D44uL, 0xEB3CFF15852F22BDuL),
                    (+1, 39, 0x9745803535A9833BuL, 0x7B2C99908CDC83FEuL),
                    (+1, 32, 0xC6D88BFCDDA34D80uL, 0xC87B8B65B664A872uL),
                    (+1, 25, 0xB82ADE8CF625D5A9uL, 0xD5B4F3B70FA44CD2uL),
                    (+1, 17, 0xF36F2A900CA7BD0EuL, 0xD257160B3AF3969FuL),
                    (+1, 9, 0xE76A41640ABA3AE2uL, 0xB4FDBBA5E9CD3AC6uL),
                    (+1, 1, 0x9ECB1DC4C5D66AA9uL, 0x273F3F494503B461uL),
                    (+1, -8, 0x9D45A97174C49A03uL, 0x25AFE35CC864B5CCuL),
                    (+1, -18, 0xDFEC69D8A7C20DB4uL, 0x66681255483C885CuL),
                    (+1, -28, 0xE34DBA79193B2EF4uL, 0x0B8AEB78964BA4B0uL),
                    (+1, -38, 0xA26472F90F5F45B1uL, 0xA3EC3235C6FB6DFAuL),
                    (+1, -49, 0xA044751C96147545uL, 0x83A836D7F8266A45uL),
                    (+1, -61, 0xD4C3552302483EFBuL, 0x71869ED3CC2F319EuL),
                    (+1, -73, 0xB6F1140D89517337uL, 0x4BB014205110B931uL),
                    (+1, -86, 0xC0DD86EA04394FB9uL, 0x9CBBBF5B81F132B9uL),
                    (+1, -100, 0xE4FAF0F7401BBB01uL, 0xA4DE3B32BE292A9EuL),
                    (+1, -114, 0x84139E94BF48ACA0uL, 0x5EBFD77E93E29C11uL),
                    (+1, -131, 0xD46536317736374DuL, 0x4BBD79FA7039ADE3uL),
                },
                new ddouble[25]{
                    (+1, 64, 0x9D375A823865D673uL, 0xE3A4F56F17DB3A78uL),
                    (+1, 64, 0x83D3C8CA804DC448uL, 0xDD2D79983A6FA864uL),
                    (+1, 62, 0x8D562CD2A811596CuL, 0x9E9DCB900233A830uL),
                    (+1, 59, 0x8E0FAC037F42C8F2uL, 0x86AAF5370E0187BAuL),
                    (+1, 55, 0x9E3E7E52DB95E3D5uL, 0xF315A5B8B752C686uL),
                    (+1, 50, 0xD69E9A38CA3AAA92uL, 0x4995273E7DECBFD1uL),
                    (+1, 45, 0xBC164C963FCCADD2uL, 0xD0BAA4AC2C675C57uL),
                    (+1, 39, 0xDDCA3C833C4B17AFuL, 0x1C20A6DD29F1B6BEuL),
                    (+1, 33, 0xB4FD74EDC5EB8D55uL, 0xCD53FDA8B78CA749uL),
                    (+1, 26, 0xD08C8F50132B8C6EuL, 0x13D74B4F81BA065DuL),
                    (+1, 19, 0xAC090DCA0669C3C1uL, 0xD8708C11E35E720BuL),
                    (+1, 11, 0xCD0FFA3B3E6AB5E7uL, 0x0E9CB8340BD12EF7uL),
                    (+1, 3, 0xB1819B7F5E39D473uL, 0xB674E7B0700B5B7CuL),
                    (+1, -6, 0xDF8207502855E7D2uL, 0x744D7C9B888CA20FuL),
                    (+1, -15, 0xCC49A733CA25F43FuL, 0xBC3ED6A5B7BB3DD7uL),
                    (+1, -24, 0x86C91D188D8C9699uL, 0xC000BEF0593FF894uL),
                    (+1, -35, 0xFE553A5933D50DBBuL, 0x644606074ECD14E7uL),
                    (+1, -45, 0xA9216C34E2DAE1A7uL, 0x429EB044A569015DuL),
                    (+1, -56, 0x9B5ECD5B595433D2uL, 0x6678DDEE79195CE4uL),
                    (+1, -68, 0xBFADF4B681E39BBFuL, 0x2D40BFE1BB329A75uL),
                    (+1, -80, 0x989A561946B8F374uL, 0xCB6FBCBE2E52B475uL),
                    (+1, -93, 0x9405D70517666362uL, 0x17826B78AEFAFD1BuL),
                    (+1, -107, 0xA010505440AA990DuL, 0x2F1C6C79A1468A41uL),
                    (+1, -122, 0xA57F6D8C35CCB824uL, 0x72564484F95E611FuL),
                    (+1, -139, 0xE85D3919ABFE0C5DuL, 0x50AC482E6DA5F090uL),
                },
                new ddouble[26]{
                    (+1, 61, 0xF4220AB37302692CuL, 0x2E3CB04456D385F3uL),
                    (+1, 62, 0x86EEEEC17D7D175BuL, 0xF6630EF2C62EBFA0uL),
                    (+1, 60, 0xB9F249E9B526EC25uL, 0xAEEA230626437DECuL),
                    (+1, 57, 0xED116CFF5A0486E9uL, 0x93131D433745EF2AuL),
                    (+1, 54, 0xA62442AF9EA37A5AuL, 0x2FDEF3F9BC9E8D46uL),
                    (+1, 50, 0x8D0DDF4A619500F9uL, 0x569AABE28A8DC1F6uL),
                    (+1, 45, 0x9A4C0A0FCA791263uL, 0x3C43B0CEF726F4FFuL),
                    (+1, 39, 0xE2C40A6401B9E6D1uL, 0x010F935B3FB4E07DuL),
                    (+1, 33, 0xE69828674CF29D66uL, 0xEF711241ED2C3A01uL),
                    (+1, 27, 0xA5B7F126D6A25B5CuL, 0x0D935C9C600A0521uL),
                    (+1, 20, 0xAAE5164894D7B484uL, 0x5D5D4D410B2B7B38uL),
                    (+1, 12, 0xFF84E3054B53EC30uL, 0x4CDF0318EEFB1DEDuL),
                    (+1, 5, 0x8B61179422F433DDuL, 0x8255D262ED9C3B26uL),
                    (+1, -4, 0xDE905B2D26392D02uL, 0xB4549A00586AFE9CuL),
                    (+1, -12, 0x82054EABDD1E40AAuL, 0xA28A770977B59050uL),
                    (+1, -22, 0xDD8C1FAD8C84FF49uL, 0x953502B0C5181F67uL),
                    (+1, -31, 0x88B004256A42DFBAuL, 0x4FDB5837FBEDB04FuL),
                    (+1, -42, 0xF1A5C8B3A4CA3F82uL, 0xD3B7B5C4D72307A1uL),
                    (+1, -52, 0x96A5FD66E101C6CCuL, 0x5F8F9F043000A5BDuL),
                    (+1, -63, 0x81A260FD04CE0326uL, 0x7F48AB5E6276188DuL),
                    (+1, -75, 0x956ED0B467FAC454uL, 0x136FDBF569A4D377uL),
                    (+1, -88, 0xDD480B9BB3F00045uL, 0x6874886F44575229uL),
                    (+1, -101, 0xC6151BEC658BFDB5uL, 0x1D42189E84544CACuL),
                    (+1, -115, 0xC3452BC8E1958F12uL, 0x21543022546C1803uL),
                    (+1, -130, 0xB47C28D208AF9BEAuL, 0x9B215130CFFADC12uL),
                    (+1, -147, 0xDB27F52E4F37D39CuL, 0x750C1F6E146AEC97uL),
                },
                new ddouble[27]{
                    (+1, 58, 0xEB6DDACD66F5BA2BuL, 0xC419D748D29DAE32uL),
                    (+1, 59, 0xAE3D41B502488DD0uL, 0x9C4C63F4940A9D06uL),
                    (+1, 58, 0x9C6008A566624702uL, 0x336A29CB2481E007uL),
                    (+1, 55, 0xFFD9B3DF5B05599FuL, 0x886AB5AD5AF61C9BuL),
                    (+1, 52, 0xE3F57303ED66A6CFuL, 0x4A61A25C7DEA1484uL),
                    (+1, 48, 0xF4847B734E37E479uL, 0x7583FBC2D0A43381uL),
                    (+1, 44, 0xA8432602EC9284D0uL, 0x8DA93F4E65194227uL),
                    (+1, 39, 0x9B29F1800BAF0597uL, 0xDEAC6C6CB2A7FD2EuL),
                    (+1, 33, 0xC5C16F005C2D597DuL, 0x8B8AB64038EBC61FuL),
                    (+1, 27, 0xB21C3A51018F80DCuL, 0x5B4EB5BFBE6D7B75uL),
                    (+1, 20, 0xE66FC0E5EE76AF26uL, 0xF775E8CA0F574471uL),
                    (+1, 13, 0xD89D653EE136530EuL, 0x1E774533700FCA7BuL),
                    (+1, 6, 0x9515CCF72B80A6A6uL, 0x79A97905BFBF9E33uL),
                    (+1, -2, 0x96E8C5522BE19CC8uL, 0x91B7BD970C1496DBuL),
                    (+1, -11, 0xE0F64E4A364E94E6uL, 0x3558D224832FA0B9uL),
                    (+1, -20, 0xF688A1B7FD8B3CDAuL, 0x085A7A33AFF28C30uL),
                    (+1, -29, 0xC5AFA07B20321AA0uL, 0x2777BE54CC771874uL),
                    (+1, -39, 0xE61BCEDFD3FAB425uL, 0xD5A9199B09598F90uL),
                    (+1, -49, 0xC0171D1C0F705653uL, 0xF05904014138F2D9uL),
                    (+1, -60, 0xE228A6716C94C3AFuL, 0x0AE5F861ADC21D6CuL),
                    (+1, -71, 0xB77B5FB6BAA346EDuL, 0xDF066D1E8B0FE7A4uL),
                    (+1, -83, 0xC6BF887A3A83CEDCuL, 0x419CA373C1A07D7DuL),
                    (+1, -95, 0x897E4A9F296FF7C9uL, 0xA4FEDC69898C1BE3uL),
                    (+1, -109, 0xE3E712B31C4A1F28uL, 0x3CC91C6F83DD8F5EuL),
                    (+1, -123, 0xCCFE9132DAF035F0uL, 0x7F914E7B6429CAB5uL),
                    (+1, -138, 0xA8C7E99C1FAD8D7EuL, 0x70BF432C35344672uL),
                    (+1, -155, 0xAEF5DA9F61D6591AuL, 0x71AAF12CC541AB50uL),
                },
                new ddouble[28]{
                    (+1, 55, 0x817DB7DC6FE27461uL, 0xDE0AE49EE74384D9uL),
                    (+1, 56, 0x8311DF47D7465630uL, 0x1114A2FDB87DB9CCuL),
                    (+1, 55, 0x9C10512230114681uL, 0x6537C8D4092CA202uL),
                    (+1, 53, 0xA6880D425F7B56F3uL, 0xAC919C05647731BCuL),
                    (+1, 50, 0xBF62826597DEB810uL, 0xFA47F1E0240991B6uL),
                    (+1, 47, 0x835A6E8379CBC986uL, 0x39E416BE4C82FC2CuL),
                    (+1, 42, 0xE6091B4130E69F41uL, 0x9010FCCC50725323uL),
                    (+1, 38, 0x866D677AC05835E9uL, 0x9514C4CC3C7DD5A4uL),
                    (+1, 32, 0xD8950EBF78A8A31AuL, 0x055C48816B8AC4B8uL),
                    (+1, 26, 0xF642AF45C4DF43CCuL, 0xEC76995F310891B1uL),
                    (+1, 20, 0xC913179CF1538852uL, 0x3C4A1EEF2DD1F700uL),
                    (+1, 13, 0xEECD722B866523F9uL, 0x1D96154708DA382AuL),
                    (+1, 6, 0xD017AB3DB9D606E3uL, 0x4ADCBC80C63C66ABuL),
                    (+1, -1, 0x85CC8C9282EF318AuL, 0x4BB1A338E899344CuL),
                    (+1, -10, 0xFE9BC6C668B0C692uL, 0xC7208BC3E9521C13uL),
                    (+1, -18, 0xB33B2F914AE27177uL, 0xF38CE49F4C4054E0uL),
                    (+1, -27, 0xBA2C5361122B49D8uL, 0x00FAFB7CE3738989uL),
                    (+1, -36, 0x8DDB54013256CC43uL, 0xAF40AC228F527CAAuL),
                    (+1, -46, 0x9D243B0CCEB7C77EuL, 0x9A054A2A3498830DuL),
                    (+1, -57, 0xF9C92D83FF7C10E2uL, 0x356FFA8EEE1C4D1DuL),
                    (+1, -67, 0x8BE4C0B88DED26D8uL, 0x9DBDC0553B439D6CuL),
                    (+1, -79, 0xD77C69E9D211D066uL, 0xE90C6DB2F2445B4CuL),
                    (+1, -91, 0xDCB23AE3E4C13F15uL, 0x72AF81F98773C008uL),
                    (+1, -103, 0x8F674FE130E2F93BuL, 0x415C2931A35ABA98uL),
                    (+1, -117, 0xDCE7BDAE1703B65CuL, 0x0A3A93DEA82A4FEFuL),
                    (+1, -131, 0xB5817DDFA5B29B8BuL, 0xE4437E7AE7E192A7uL),
                    (+1, -146, 0x848973D3F92B51B2uL, 0x2857EDC1E7F886D9uL),
                    (+1, -164, 0xE66686165D0532F7uL, 0xA1463EA669F372FBuL),
                },
                new ddouble[29]{
                    (+1, 50, 0x8F570CBBDFF40C75uL, 0x6EE095CDD1C868E2uL),
                    (+1, 51, 0xCCA0A4FD8C0FF6CEuL, 0x252CD0A6B480D792uL),
                    (+1, 51, 0xA60BE0367ECCC2B3uL, 0xBF79A095730FE746uL),
                    (+1, 49, 0xECAC1129162AF75AuL, 0x2BD25D7BACC878E5uL),
                    (+1, 47, 0xB321E3F16D216614uL, 0x93E074B1E26A6E3CuL),
                    (+1, 44, 0xA045A43248DE29C9uL, 0x2AB4B3ADF1B27F4BuL),
                    (+1, 40, 0xB584ECCC7B63C7A9uL, 0x0ED82D6373DAF918uL),
                    (+1, 36, 0x886314261715E22BuL, 0x91E0A4000B712E76uL),
                    (+1, 31, 0x8CA5BCCA5EEC9ABCuL, 0x2F6C622D33D702A0uL),
                    (+1, 25, 0xCC19D05FD5E70AA6uL, 0x7C35EE682053CF61uL),
                    (+1, 19, 0xD450758A3B0EB19FuL, 0x1106D61FC40D9AA1uL),
                    (+1, 13, 0xA0878F9B233CAB02uL, 0xAC97FC6076C5FB98uL),
                    (+1, 6, 0xB239A206AC4F9209uL, 0xAC1758DFA4ABBA81uL),
                    (+1, -1, 0x9246CB642E1B19BEuL, 0xB25876E54B77B69FuL),
                    (+1, -9, 0xB2395CCE04FEC80DuL, 0xD347405FAED60F5AuL),
                    (+1, -17, 0xA16A5B6F50E043E7uL, 0x05356FD3CAE19FD5uL),
                    (+1, -26, 0xD919B072B6A24CF4uL, 0xF33FA476C35FA74BuL),
                    (+1, -35, 0xD7FF9A3849992297uL, 0xE8B83B3583B9169AuL),
                    (+1, -44, 0x9DEA69DBE6AC2960uL, 0x8C29CA41D6768DFCuL),
                    (+1, -54, 0xA7FCC604C576B80CuL, 0x3FA59CFA90639A30uL),
                    (+1, -64, 0x80332F761D63FF32uL, 0x0D0F2A34F81B5184uL),
                    (+1, -75, 0x89B72460F9E3EA80uL, 0x57C871E0A1A321F2uL),
                    (+1, -87, 0xCAE86919EB1FE298uL, 0x7DD4AECC99BC06A8uL),
                    (+1, -99, 0xC5E082DAF805A26FuL, 0x3F37A51A56B90F15uL),
                    (+1, -112, 0xF30D735E1A366A93uL, 0x576B4B8056DE3896uL),
                    (+1, -125, 0xAED540FE32E6D781uL, 0xDD68A0AFC1CC6846uL),
                    (+1, -139, 0x837C1BFDFE339049uL, 0x1CF28CE7ED2E06D0uL),
                    (+1, -155, 0xA9737950F9DB1A31uL, 0x1472748C5931C89AuL),
                    (+1, -173, 0xF0BEB7FE4A357F71uL, 0x81DAA3AEEE2F7D5EuL),
                },
                new ddouble[30]{
                    (+1, 44, 0x826FBCE74C6166D8uL, 0x4D1DC39A4F983D63uL),
                    (+1, 46, 0x8A007650CAE0AEADuL, 0x96EA5B8CC99B7E8AuL),
                    (+1, 46, 0x9F5B2A69074CE242uL, 0x0C266CABBB62FB44uL),
                    (+1, 45, 0x9D76BA2D37944FD9uL, 0x2B4F8500AD0721AAuL),
                    (+1, 43, 0xA2267A2ADC61B9EBuL, 0x5299917389E335E6uL),
                    (+1, 40, 0xC28909AC8A5FE9AAuL, 0xB613B0312F700F59uL),
                    (+1, 37, 0x92091308FBE95D63uL, 0xA99B78067D94322EuL),
                    (+1, 33, 0x902471DBF87C26B9uL, 0xDCA90ED7607E2159uL),
                    (+1, 28, 0xC1E135ABD972F66CuL, 0xF7EE5F99C153E72AuL),
                    (+1, 23, 0xB67B204F5319AC95uL, 0xE73ACD700EFCEB5EuL),
                    (+1, 17, 0xF5415A78A165CF26uL, 0x17B5793D19C218E9uL),
                    (+1, 11, 0xEEF567242E48F7D0uL, 0xF9F5A160C0935EF3uL),
                    (+1, 5, 0xAAB72C590FAE3638uL, 0x717CA05D153970A4uL),
                    (+1, -2, 0xB45346C162E0CB35uL, 0xCDCCBAC8E7525C61uL),
                    (+1, -9, 0x8D945222A48BEBB6uL, 0xE01AEF6D13C40E2FuL),
                    (+1, -17, 0xA5BA6955453828C9uL, 0x09AA796DF01FFCACuL),
                    (+1, -25, 0x90B0275D409CCC16uL, 0xD740DF8D4AD7DAFDuL),
                    (+1, -34, 0xBC100CFE561DDEDCuL, 0x71A8B3126E160D3DuL),
                    (+1, -43, 0xB5249BD6E2CCC081uL, 0x8BD7C8CCFFF8695DuL),
                    (+1, -52, 0x80591AC0DFAF8F2AuL, 0x5D1B22BF68A70579uL),
                    (+1, -62, 0x845DCE6954A87C69uL, 0xDE85D58F2735A497uL),
                    (+1, -73, 0xC3C4875DDA78D55EuL, 0x5E6C93B03175225CuL),
                    (+1, -84, 0xCB78DDFF924DADC6uL, 0x3AE18BDEE844E6F2uL),
                    (+1, -95, 0x909E9F51B5295741uL, 0x3BD0FD2D0EFD5587uL),
                    (+1, -107, 0x876C15CB4BB28F95uL, 0xDB83B3270658B5FDuL),
                    (+1, -120, 0x9E7B8440F4330E37uL, 0x369DE031968637DDuL),
                    (+1, -134, 0xD66ED95D3224A9CBuL, 0x8C1A63F862105D46uL),
                    (+1, -148, 0x943BC4C12878B6ABuL, 0x1BEEEBB54FE54ECFuL),
                    (+1, -164, 0xA7D1086A256A9E78uL, 0xAC1D7229595668ADuL),
                    (+1, -182, 0xBB84744FAC8F7253uL, 0x37F7ABA1B2C5F6AEuL),
                },
                new ddouble[31]{
                    (+1, 36, 0x8545ACD9B07E4B09uL, 0x512BDFCD504AAB31uL),
                    (+1, 38, 0xE622D1C076B7437BuL, 0xC954828B4C2BE8DAuL),
                    (+1, 39, 0xCCE3E8837BE7F3EEuL, 0xA992F220917BE1DEuL),
                    (+1, 39, 0x960DE91E9DD886E7uL, 0xDDED4C1FCD013549uL),
                    (+1, 37, 0xDE57FC8423C372ECuL, 0x4BB0FD5605F1DF80uL),
                    (+1, 35, 0xBB81DFA0687D67B4uL, 0x5462DB4A54AD030BuL),
                    (+1, 32, 0xC240326028649DECuL, 0xD1DB9800CD39E50FuL),
                    (+1, 29, 0x8255946E2D29E7C8uL, 0x9F8AF18D7CA7B072uL),
                    (+1, 24, 0xEB7B1C352E8DA6FDuL, 0xFB384C4E7EEE64D2uL),
                    (+1, 20, 0x936D0EBCEA182F2DuL, 0x8CD261827E954595uL),
                    (+1, 15, 0x82CD0B43F0AC659FuL, 0x686A401EAA125810uL),
                    (+1, 9, 0xA74AFAB7CEE45F29uL, 0x5417374B916F7D83uL),
                    (+1, 3, 0x9C3F4652746AF1B5uL, 0x4B2FEBC5429F95A1uL),
                    (+1, -4, 0xD7390632DA69A1B8uL, 0xD5CD6C0B51D49855uL),
                    (+1, -11, 0xDC2453BA80FE9602uL, 0xD640E9893AC815D7uL),
                    (+1, -18, 0xA7F5061EAEF268E7uL, 0x8CF3AF60EAF623CBuL),
                    (+1, -26, 0xBF92D324113A2810uL, 0xD4E279D386EF6BDDuL),
                    (+1, -34, 0xA350550045D30076uL, 0x1A81EF39B939AF30uL),
                    (+1, -43, 0xCF978E62A61F30D5uL, 0xA9BF783BB75DAA3CuL),
                    (+1, -52, 0xC3BE9EF177048134uL, 0x2EF4A4A9721A2D4EuL),
                    (+1, -61, 0x87D7D6F27D5769E0uL, 0x25CCE5F52D1F5641uL),
                    (+1, -71, 0x8935DFF3B176A0B2uL, 0x2C4DE598E191D062uL),
                    (+1, -82, 0xC69D770249FDF0A6uL, 0xB3A52277D43500DBuL),
                    (+1, -93, 0xC9BC089A86CAB0ABuL, 0x8B86B823A5D2737BuL),
                    (+1, -104, 0x8BBF9082EAA6894CuL, 0xD5E44C34B19A57E8uL),
                    (+1, -117, 0xFDF4978B136113C6uL, 0xFF235273DBE17C90uL),
                    (+1, -129, 0x8F216411CE0A76B4uL, 0xF29D6AF79C594010uL),
                    (+1, -143, 0xB826BB3C990D2509uL, 0x7A07607A5070DEBAuL),
                    (+1, -158, 0xEC3CD7E0EA4A2706uL, 0xC7D4BA28D0FA61EFuL),
                    (+1, -174, 0xEADD5EE6CA9A71A9uL, 0xF40402FEE337E49BuL),
                    (+1, -192, 0xC1F517983A80B873uL, 0x09CAE858A439E2B5uL),
                },
                new ddouble[32]{
                    (+1, 24, 0xDA091339D4BFEE07uL, 0x229A0236EC1DD7CCuL),
                    (+1, 28, 0xCE79D99B2D3E26A5uL, 0x7636B573F0716D64uL),
                    (+1, 30, 0xAFE5223B0CA09064uL, 0xC578A1A84004E481uL),
                    (+1, 30, 0xE04E3A66405C648AuL, 0x86848E5326A4F9A5uL),
                    (+1, 30, 0x873F17D4CF10752EuL, 0x7924847E04C0A692uL),
                    (+1, 28, 0xB09D5B68E5795F6DuL, 0x3E157B97AE681FB9uL),
                    (+1, 26, 0x8862BDFCFE76E322uL, 0xAE3FF0D304F9A260uL),
                    (+1, 23, 0x84764C0A13854E98uL, 0x92BEFA7E756F1B17uL),
                    (+1, 19, 0xA93D6D77B82D7931uL, 0x3C5A1D2902DE8322uL),
                    (+1, 15, 0x931E235883402B36uL, 0xD52D3D966EC71BF7uL),
                    (+1, 10, 0xB29AE37E1C5918F2uL, 0xF155441D7FBAC840uL),
                    (+1, 5, 0x9A7F2030FCD1C399uL, 0x2DC96518FF4FE0F0uL),
                    (+1, -1, 0xC17445A2C75C92F6uL, 0x54104D8A337AC693uL),
                    (+1, -7, 0xB17398386658702EuL, 0xABEDEC05C8368C3EuL),
                    (+1, -14, 0xF0A5BCFA91770E81uL, 0x77949B975A5CDF94uL),
                    (+1, -21, 0xF2CB9F7DBA75EA74uL, 0x209493EF1C64F5BAuL),
                    (+1, -28, 0xB6FBEEF800103829uL, 0x18458B3A990644EDuL),
                    (+1, -36, 0xCE664C0B27637EE1uL, 0xBBFD72FE45ED554FuL),
                    (+1, -44, 0xAE24C759F82BFE09uL, 0xAD4CF66E8147F3B2uL),
                    (+1, -53, 0xDB33D8DD12784F7DuL, 0x5FA12B6CD5F5C32DuL),
                    (+1, -62, 0xCCBD967D4B5FBD67uL, 0xC6CF75E2D0F57E1AuL),
                    (+1, -71, 0x8CBFD4BDF1BB09BAuL, 0xEF3A166B99CFEE94uL),
                    (+1, -81, 0x8CCAA67B144DDDC8uL, 0xF2B8BA97E2EE20EAuL),
                    (+1, -92, 0xC9B3702418FA69C9uL, 0xAAF93E399A84E46BuL),
                    (+1, -103, 0xCA8769CB1A0D843AuL, 0xBA05324D2589BFACuL),
                    (+1, -114, 0x8A6F048EAD424848uL, 0xFBE252A1263CF7D5uL),
                    (+1, -127, 0xF772661192FB798CuL, 0x93EDB99A48B74BC8uL),
                    (+1, -139, 0x88720EE3F2BAF9B7uL, 0x47D68400DEF371C0uL),
                    (+1, -153, 0xAA0EEC0C004FF198uL, 0xEA3B40B2BEB24B5DuL),
                    (+1, -168, 0xCEEBF0A79A87B6E8uL, 0x8C9787483D5E7E04uL),
                    (+1, -184, 0xB81AE37067EFF494uL, 0x79D890C13B8A4DF8uL),
                    (+1, -203, 0xC836CE087E745B4DuL, 0x75769D42BA0A3458uL),
                }
            };
        }

        private static class Limit {
            private static Dictionary<ddouble, ACoefTable> a_table = new();
            private static Dictionary<ddouble, JYCoefTable> jy_table = new();
            private static Dictionary<ddouble, IKCoefTable> ik_table = new();

            public static ddouble BesselJ(ddouble nu, ddouble x) {
                (ddouble c, ddouble s) = BesselJYKernel(nu, x, terms: 18);

                ddouble omega = x - (2 * nu + 1) * PI / 4;
                ddouble m = c * Cos(omega) - s * Sin(omega);
                ddouble t = m * Sqrt(2 / (PI * x));

                return t;
            }

            public static ddouble BesselY(ddouble nu, ddouble x) {
                (ddouble s, ddouble c) = BesselJYKernel(nu, x, terms: 18);

                ddouble omega = x - (2 * nu + 1) * PI / 4;
                ddouble m = s * Sin(omega) + c * Cos(omega);
                ddouble t = m * Sqrt(2 / (PI * x));

                return t;
            }

            public static ddouble BesselI(ddouble nu, ddouble x, bool scale = false) {
                ddouble c = BesselIKKernel(nu, x, sign_switch: true, terms: 36);

                ddouble t = c / Sqrt(2 * PI * x);

                if (!scale) {
                    t *= Exp(x);
                }

                return t;
            }

            public static ddouble BesselK(ddouble nu, ddouble x, bool scale = false) {
                ddouble c = BesselIKKernel(nu, x, sign_switch: false, terms: 34);

                ddouble t = c * Sqrt(PI / (2 * x));

                if (!scale) {
                    t *= Exp(-x);
                }

                return t;
            }

            public static (ddouble s, ddouble t) BesselJYKernel(ddouble nu, ddouble x, int terms = 64) {
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

            public static ddouble BesselIKKernel(ddouble nu, ddouble x, bool sign_switch, int terms) {
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
                    this.squa_nu4 = 4 * nu * nu;

                    ddouble a1 = (squa_nu4 - 1) / 8;

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
                        ddouble a = table.Last() * (squa_nu4 - checked((2 * k - 1) * (2 * k - 1))) / checked(k * 8);

                        table.Add(a);
                    }

                    return table[n];
                }
            }

            private class JYCoefTable {
                private readonly ddouble squa_nu4;
                private readonly List<(ddouble p0, ddouble p1)> table = new();

                public JYCoefTable(ddouble nu) {
                    this.squa_nu4 = 4 * nu * nu;
                }

                public (ddouble p0, ddouble p1) this[int n] => Value(n);

                public (ddouble p0, ddouble p1) Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    static int square(int n) => checked(n * n);

                    for (int k = table.Count; k <= n; k++) {
                        ddouble p0 = (squa_nu4 - square(8 * k + 1)) * (squa_nu4 - square(8 * k + 3)) / checked(64 * (4 * k + 1) * (4 * k + 2));
                        ddouble p1 = (squa_nu4 - square(8 * k + 3)) * (squa_nu4 - square(8 * k + 5)) / checked(64 * (4 * k + 2) * (4 * k + 3));

                        table.Add((p0, p1));
                    }

                    return table[n];
                }
            }

            private class IKCoefTable {
                private readonly ddouble squa_nu4;
                private readonly List<ddouble> table = new();

                public IKCoefTable(ddouble nu) {
                    this.squa_nu4 = 4 * nu * nu;
                }

                public ddouble this[int n] => Value(n);

                public ddouble Value(int n) {
                    if (n < 0) {
                        throw new ArgumentOutOfRangeException(nameof(n));
                    }

                    if (n < table.Count) {
                        return table[n];
                    }

                    static int square(int n) => checked(n * n);

                    for (int k = table.Count; k <= n; k++) {
                        ddouble p = (squa_nu4 - square(4 * k + 1)) / checked(8 * (2 * k + 1));

                        table.Add(p);
                    }

                    return table[n];
                }
            }
        }

        private static class HalfInt {
            public static ddouble BesselJ(int n, ddouble x) {
                ddouble c = Cos(x), s = Sin(x);
                ddouble v = Rcp(x), r = Sqrt(2 * v * RcpPI);

                if (n == -8) { //nu=-7.5
                    return r*(((((((((-135135*c)* v) + (-135135*s)* v) + (62370*c)* v) + (17325*s)* v) + (-3150*c)* v) + (-378*s)* v) + (28*c)* v) + s);
                }
                if (n == -7) { //nu=-6.5
                    return r*((((((((10395*c)* v) + (10395*s)* v) + (-4725*c)* v) + (-1260*s)* v) + (210*c)* v) + (21*s)* v) - c);
                }
                if (n == -6) { //nu=-5.5
                    return r*(((((((-945*c)* v) + (-945*s)* v) + (420*c)* v) + (105*s)* v) + (-15*c)* v) - s);
                }
                if (n == -5) { //nu=-4.5
                    return r*((((((105*c)* v) + (105*s)* v) + (-45*c)* v) + (-10*s)* v) + c);
                }
                if (n == -4) { //nu=-3.5
                    return r*(((((-15*c)* v) + (-15*s)* v) + (6*c)* v) + s);
                }
                if (n == -3) { //nu=-2.5
                    return r*((((3*c)* v) + (3*s)* v) - c);
                }
                if (n == -2) { //nu=-1.5
                    return r*(- c * v - s);
                }
                if (n == -1) { //nu=-0.5
                    return r*c;
                }
                if (n == 0) { //nu=0.5
                    return r*s;
                }
                if (n == 1) { //nu=1.5
                    return r*(s / x - c);
                }
                if (n == 2) { //nu=2.5
                    return r*((((3*s)* v) + (-3*c)* v) - s);
                }
                if (n == 3) { //nu=3.5
                    return r*(((((15*s)* v) + (-15*c)* v) + (-6*s)* v) + c);
                }
                if (n == 4) { //nu=4.5
                    return r*((((((105*s)* v) + (-105*c)* v) + (-45*s)* v) + (10*c)* v) + s);
                }
                if (n == 5) { //nu=5.5
                    return r*(((((((945*s)* v) + (-945*c)* v) + (-420*s)* v) + (105*c)* v) + (15*s)* v) - c);
                }
                if (n == 6) { //nu=6.5
                    return r*((((((((10395*s)* v) + (-10395*c)* v) + (-4725*s)* v) + (1260*c)* v) + (210*s)* v) + (-21*c)* v) - s);
                }
                if (n == 7) { //nu=7.5
                    return r*(((((((((135135*s)* v) + (-135135*c)* v) + (-62370*s)* v) + (17325*c)* v) + (3150*s)* v) + (-378*c)* v) + (-28*s)* v) + c);
                }

                throw new NotImplementedException();
            }
        }
    }
}