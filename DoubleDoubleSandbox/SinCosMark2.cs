﻿using DoubleDouble;
using System;
using static DoubleDouble.ddouble;

namespace DoubleDoubleSandbox {

    internal class SinCosMark2 {
        public static (ddouble y, int terms) SinPIPrime(ddouble x) {
            if (!(x >= 0d) || x > 1d) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x < 0.5d) {
                ddouble w = Ldexp(x * PI, -1), w2 = w * w, w4 = w2 * w2, u = 1;
                ddouble y = ddouble.Zero;

                int terms = 0;
                for (int i = 0, n = TaylorSequence.Count - 3; i < n; i += 4) {
                    ddouble f = TaylorSequence[i + 3];
                    ddouble dy = u * f * ((i + 2) * (i + 3) - w2);
                    ddouble y_next = y + dy;

                    if (y == y_next) {
                        break;
                    }

                    u *= w4;
                    y = y_next;
                    terms++;
                }

                return (w * y, terms);
            }
            else {
                ddouble w = Ldexp((x - 1d) * PI, -1), w2 = w * w, w4 = w2 * w2, u = w2;
                ddouble y = 1d;

                int terms = 0;
                for (int i = 0, n = TaylorSequence.Count - 4; i < n; i += 4) {
                    ddouble f = TaylorSequence[i + 4];
                    ddouble dy = u * f * ((i + 3) * (i + 4) - w2);
                    ddouble y_next = y - dy;

                    if (y == y_next) {
                        break;
                    }

                    u *= w4;
                    y = y_next;
                    terms++;
                }

                return (y, terms);
            }
        }
    }
}