using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace DoubleDouble {
    internal partial struct qdouble {
        private static ReadOnlyCollection<qdouble> taylor_sequence = null;

        public static ReadOnlyCollection<qdouble> TaylorSequence {
            get {
                if (taylor_sequence is null) {
                    taylor_sequence = GenerateTaylorSequence();
                }

                return taylor_sequence;
            }
        }

        private static ReadOnlyCollection<qdouble> GenerateTaylorSequence() {
            List<qdouble> table = new() {
                1,
                1
            };

            BigInteger v = 2;

            for (int d = 3; d <= 256; d++) {
                qdouble t = 1 / (qdouble)v;

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
