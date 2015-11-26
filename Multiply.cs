namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent multiplication
    /// </summary>
    public class Multiply : BinaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Multiply(SymMathNode a, SymMathNode b) : base(a, b)
        {
        }

        /// <summary>
        /// Returns true if this tree is equal to node tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool SymbolicEquality(SymMathNode node)
        {
            if (base.SymbolicEquality(node))
            {
                return true;
            }

            var add = node as Multiply;
            if (add != null)
            {
                return A.SymbolicEquality(add.B) && B.SymbolicEquality(add.A);
            }

            return false;
        }

        /// <summary>
        /// Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Simplify()
        {
            if (A.CanSimplify() || B.CanSimplify())
            {
                return A.Simplify() * B.Simplify();
            }

            if (A is Term && B is Term)
            {
                return (Term)A * (Term)B;
            }

            var a = A as Term;
            var b = B as Term;
            if (a != null && a.IsOne())
            {
                return B;
            }

            if (b != null && b.IsOne())
            {
                return A;
            }

            var ar = A as SquareRoot;
            var br = B as SquareRoot;
            if (ar != null && br != null && ar.A.SymbolicEquality(br.A))
            {
                return ar.A;
            }

            return base.Simplify();
        }

        /// <summary>
        /// True if current expression can be simplified
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            return
              base.CanSimplify() ||
              (A is Term && ((Term)A).IsOne()) ||
              (B is Term && ((Term)B).IsOne()) ||
              (A is Term && B is Term) ||
              (A is SquareRoot && B is SquareRoot && ((SquareRoot)A).A.SymbolicEquality(((SquareRoot)B).A))
              ;
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Multiply(a, b);
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}*{1})", A, B);
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return A.Evaluate() + B.Evaluate();
        }
    }
}