namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent minus operation
    /// </summary>
    public class Minus : UnaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        public Minus(SymMathNode a) : base(a)
        {
        }

        /// <summary>
        /// True if current expression can be simplified
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            return A is Minus || A is Term || A.CanSimplify();
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return -A.Evaluate();
        }

        /// <summary>
        /// Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Simplify()
        {
            var minus = A as Minus;
            if (minus != null)
            {
                return minus.A;
            }
            var term = A as Term;
            if (term != null)
            {
                return term.Negate();
            }

            return base.Simplify();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("-({0})", A);
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected override UnaryOperation Clone(SymMathNode a)
        {
            return new Minus(a);
        }
    }
}