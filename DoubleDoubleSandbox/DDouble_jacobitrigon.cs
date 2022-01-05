using System;
using System.Collections.Generic;
using DoubleDouble;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {
    public static class JacobiTrigonPrototype {
        public static ddouble JacobiSn(ddouble x, ddouble k) {
            if (k < 0d) {
                throw new ArgumentOutOfRangeException(nameof(k));
            }

            if (!IsFinite(x) || !IsFinite(k)) {
                return NaN;
            }

            if (IsZero(x)) {
                return Zero;
            }

            ddouble y = (k >= 1d) ? JacobiTrigon.SnGeqOneK(x, k) : JacobiTrigon.SnLtOneK(x, k);

            return y;
        }

        internal static class JacobiTrigon {
            public static (ddouble min, ddouble max) NearOne
                = ((+1, -1, 0xFFFFFFFFFFFFFFFFuL, 0xFFFFFFFFFF000000uL),
                   (+1,  0, 0x8000000000000000uL, 0x0000000001000000uL));
            public static ddouble Eps = Math.ScaleB(1, -102);

            public static ddouble SnGeqOneK(ddouble x, ddouble k) {
                if (k <= NearOne.max) {
                    return Tanh(x);
                }

                ddouble u = 1d / (k * k);
                ddouble v = x * k;

                ddouble y = SnLtOneK(v, u) / k;

                return y;
            }

            public static ddouble SnLtOneK(ddouble x, ddouble k) {
                if (k >= NearOne.min) {
                    return Tanh(x);
                }
                if (k < Eps) {
                    return SnNearZeroK(x, k);
                }

                ddouble phi = Phi(x, k);

                ddouble y = Sin(phi);

                return y;
            }


            public static ddouble SnNearZeroK(ddouble x, ddouble k) {
                ddouble sinx = Sin(x), cosx = Cos(x);

                ddouble y = sinx - k * k * (x - sinx * cosx) * cosx / 4;

                return y;
            }

            
            public static ddouble Phi(ddouble x, ddouble k) {
                ddouble a = 1, b = Sqrt(1d - k * k), c = k;

                List<ddouble> a_list = new() { a };
                List<ddouble> c_list = new() { c };

                int n = 1;
                for (; n < 32; n++) {
                    (a, b, c) = ((a + b) / 2, Sqrt(a * b), (a - b) / 2);

                    a_list.Add(a);
                    c_list.Add(c);
                    
                    if (a == b || c < Eps) {
                        break;
                    }
                }

                ddouble phi = Ldexp(a * x, n);

                while (n > 0) {
                    phi = (phi + Asin(c_list[n] / a_list[n] * Sin(phi))) / 2;
                    n--;
                }

                return phi;
            }
        }
    }
}