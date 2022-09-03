using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoubleDouble {
    public partial struct ddouble {
        private static ReadOnlyCollection<ddouble> factorial_sequence = null;

        public static ReadOnlyCollection<ddouble> Factorial {
            get {
                if (factorial_sequence is null) {
                    factorial_sequence = GenerateFactorialSequence();
                }

                return factorial_sequence;
            }
        }

        private static ReadOnlyCollection<ddouble> GenerateFactorialSequence() {
            List<ddouble> table = new() {
                1,
                1
            };

            for (int i = 2; i <= 256; i++) {
                ddouble t = i * table[^1];

                if (IsPositiveInfinity(t)) {
                    break;
                }

                table.Add(t);
            }

            return table.AsReadOnly();
        }
    }
}
