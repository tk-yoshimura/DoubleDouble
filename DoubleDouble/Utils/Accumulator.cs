using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal struct Accumulator {
        public ddouble Sum { private set; get; }
        public ddouble C { private set; get; }
        public bool IsConvergence { private set; get; }

        private Accumulator(ddouble x) {
            this.Sum = x;
            this.C = ddouble.Zero;
            this.IsConvergence = false;
        }

        private Accumulator(ddouble x, ddouble c, bool is_convergence) {
            this.Sum = x;
            this.C = c;
            this.IsConvergence = is_convergence;
        }

        public static implicit operator Accumulator(ddouble x) {
            return new Accumulator(x);
        }

        public static implicit operator Accumulator(double x) {
            return new Accumulator((ddouble)x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Accumulator operator +(Accumulator v) {
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Accumulator operator -(Accumulator v) {
            return new Accumulator(-v.Sum, -v.C, v.IsConvergence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Accumulator operator +(Accumulator a, ddouble b) {
            (ddouble sum, ddouble c) = TwoSum(a.Sum, b);
            c += a.C;
            (sum, c) = TwoSum(sum, c);

            return new Accumulator(sum, c, a.Sum == sum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Accumulator operator -(Accumulator a, ddouble b) {
            (ddouble sum, ddouble c) = TwoSum(a.Sum, -b);
            c += a.C;
            (sum, c) = TwoSum(sum, c);

            return new Accumulator(sum, c, a.Sum == sum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (ddouble s, ddouble c) TwoSum(ddouble a, ddouble b) {
            ddouble s = a + b;
            ddouble t = s - a;
            ddouble c = (a - (s - t)) + (b - t);

            return (s, c);
        }

        public override string ToString() {
            return $"sum: {Sum} c: {C} is_convergence:{IsConvergence}";
        }
    }
}
