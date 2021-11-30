using DoubleDouble;
using System;
using System.IO;

namespace DoubleDoubleSandbox {
    internal class Program {
        static void Main(string[] args) {
            ddouble[] sumk2p1_table = new ddouble[] {
                (ddouble)0,
                (ddouble)1,
                (ddouble)4/3,
                (ddouble)23/15,
                (ddouble)176/105,
                (ddouble)563/315,
                (ddouble)6508/3465,
                (ddouble)88069/45045,
                (ddouble)91072/45045,
                (ddouble)1593269/765765,
                (ddouble)31037876/14549535,
                (ddouble)31730711/14549535,
                (ddouble)744355888/334639305,
                (ddouble)3788707301/1673196525,
                (ddouble)11552032628/5019589575,
                (ddouble)340028535787/145568097675,
                (ddouble)10686452707072/4512611027925,
                (ddouble)10823198495797/4512611027925,
                (ddouble)10952130239452/4512611027925,
                (ddouble)409741429887649/166966608033225,
                (ddouble)414022624965424/166966608033225
            };

            using (StreamWriter sw = new StreamWriter("ddouble_convterms.csv")) {
                sw.WriteLine($"z,terms,approx digamma(z),expected digamma(z),error");

                for (int z2 = 1; z2 <= 40; z2++) {
                    int terms = SterlingApprox.DiSterlingTermConvergence(z2 * 0.5d);
                    ddouble y_approx = terms < 32 ? SterlingApprox.Digamma(z2 * 0.5d, terms) : ddouble.NaN;
                    ddouble y_expected = 
                        ((z2 & 1)==0) ? (ddouble.HarmonicNumber(z2 / 2 - 1) - ddouble.EulerGamma) : 
                        2 * sumk2p1_table[z2 / 2] - 2 * ddouble.Log(2) - ddouble.EulerGamma;

                    ddouble err = y_approx - y_expected;

                    sw.WriteLine($"{z2 * 0.5d},{terms},{y_approx},{y_expected},{err}");
                    Console.WriteLine($"{z2 * 0.5d},{terms},{y_approx},{y_expected},{err}");
                }
            }

            Console.WriteLine("END");
            Console.Read();
        }
    }
}
