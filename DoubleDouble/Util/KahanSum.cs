using System.Runtime.CompilerServices;

namespace DoubleDouble {
    internal struct KahanSum {
        public ddouble Sum { private set; get; }
        public ddouble C { private set; get; }
        public bool IsConvergence { private set; get; }

        public KahanSum(ddouble x) {
            this.Sum = x;
            this.C = ddouble.Zero;
            this.IsConvergence = false;
        }

        private KahanSum(ddouble x, ddouble c) {
            this.Sum = x;
            this.C = c;
            this.IsConvergence = false;
        }

        public static implicit operator KahanSum(ddouble x) {
            return new KahanSum(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ddouble x) {
            ddouble y = x - C;
            ddouble t = Sum + y;

            C = (t - Sum) - y;
            IsConvergence = Sum == t;
            Sum = t;
        }

        public static KahanSum Negate(KahanSum x) {
            return new KahanSum(-x.Sum, -x.C);
        }

        public override string ToString() {
            return $"sum: {Sum} c: {C} is_convergence:{IsConvergence}";
        }
    }
}
