using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDouble {
    public partial struct ddouble {
        private static ReadOnlyCollection<ddouble> taylor_sequence = null;

        public static ReadOnlyCollection<ddouble> TaylorSequence {
            get {
                if (taylor_sequence is null) {
                    taylor_sequence = GenerateTaylorSequence();
                }

                return taylor_sequence;
            }
        }

        private static ReadOnlyCollection<ddouble> GenerateTaylorSequence() {
            List<ddouble> table = new() {
                1,
                1
            };

            BigInteger v = 2;

            for (int d = 3; d <= 256; d++) {
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
