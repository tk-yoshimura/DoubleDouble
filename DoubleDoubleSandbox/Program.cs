using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            using (StreamWriter sw1 = new StreamWriter("../../atan_convergence.csv")) {
                using (StreamWriter sw2 = new StreamWriter("../../atan_convergence_n2.csv")) {
                    sw1.WriteLine($"x,x_init,n,y");
                    sw2.WriteLine($"x,x_init,n,y,y_prototype");

                    for (decimal x = 0; x <= 1; x += 1 / 128m) {
                        (ddouble min_y, int min_n, decimal min_x_init) = (0d, int.MaxValue, 0m);

                        for (decimal x_init = 1; x_init <= 128m; x_init += 1 / 4m) {
                            (ddouble y, int n) = AtanCfracConvergence((ddouble)x, (ddouble)x_init);

                            if (min_n > n) {
                                (min_y, min_n, min_x_init) = (y, n, x_init);
                            }

                            sw1.WriteLine($"{x},{x_init},{n},{y}");
                        }

                        ddouble y_prototype = AtanPrototype((ddouble)x);
                        ddouble error = min_y - y_prototype;

                        sw2.WriteLine($"{x},{min_x_init},{min_n},{min_y},{y_prototype},{error}");
                    }
                }
            }
        }

        static (ddouble value, int n) AtanCfracConvergence(ddouble x, ddouble x_init) {
            ddouble y = AtanCfracConvergence(x, 4, x_init);

            for (int n = 5; n <= 128; n++) {
                ddouble y_test = AtanCfracConvergence(x, n, x_init);
                if (y == y_test) {
                    return (y_test, n - 1);
                }

                y = y_test;
            }

            return (y, int.MaxValue);
        }

        static ddouble AtanCfracConvergence(ddouble x, int n, ddouble x_init) {
            if (x > 1) {
                return ddouble.PI - AtanCfracConvergence(1 / x, n, x_init);
            }
            if (x < -1) {
                return -ddouble.PI - AtanCfracConvergence(1 / x, n, x_init);
            }

            ddouble f = x_init;

            for (int i = n; i >= 1; i--) {
                f = (2 * i - 1) + (i * i) * x * x / f;
            }

            return x / f;
        }

        static ddouble AtanPrototype(ddouble x) {
            if (x > 1) {
                return ddouble.PI - AtanPrototype(1 / x);
            }
            if (x < -1) {
                return -ddouble.PI - AtanPrototype(1 / x);
            }
            if (x < -0.25d || x > 0.25d) {
                return ddouble.Ldexp(AtanPrototype(x / (1 + ddouble.Sqrt(1 + x * x))), 1);
            }

            ddouble f = 86d * x + 13.5d;
            int n = (int)ddouble.Floor(33.5d * x + 9d);

            for (int i = n; i >= 1; i--) {
                f = (2 * i - 1) + (i * i) * x * x / f;
            }

            return x / f;
        }
    }
}
