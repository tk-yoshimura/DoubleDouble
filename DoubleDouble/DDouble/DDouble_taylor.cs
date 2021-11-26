using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        public static readonly ReadOnlyCollection<ddouble> TaylorSequence = GenerateTaylorSequence();

        private static ReadOnlyCollection<ddouble> GenerateTaylorSequence() {
            List<ddouble> table = new() {
                1, 1
            };

            BigInteger v = 2;

            for(int d = 3; d <= 256; d++) {
                if (d == 250) {
                    Console.WriteLine("koko");
                }

                ddouble t = 1 / (ddouble)v;

                table.Add(t);
                
                if (IsZero(t)) {
                    break;
                }

                v *= d;
            }

            return table.AsReadOnly();
        }
    }
}
