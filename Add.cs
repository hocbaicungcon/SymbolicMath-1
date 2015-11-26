namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent addition operation
    /// </summary>
    public class Add : BinaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Add()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Add(SymMathNode a, SymMathNode b) : base(a, b)
        {
            A = a;
            B = b;
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

            var add = node as Add;
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
                return A.Simplify() + B.Simplify();
            }

            var a = A as Term;
            var b = B as Term;
            if (a != null && b != null)
            {
                return a + b;
            }
            if (a != null && a.IsZero())
            {
                return B;
            }
            if (b != null && b.IsZero())
            {
                return A;
            }

            var addA = A as Add;
            var addB = B as Add;
            if (b != null && addA != null)
            {
                if (addA.A is Term)
                {
                    return (b + (Term)addA.A) + addA.B;
                }
                if (addA.B is Term)
                {
                    return (b + (Term)addA.B) + addA.A;
                }
            }
            if (a != null && addB != null)
            {
                if (addB.A is Term)
                {
                    return (a + ((Term)addB.A)) + addB.B;
                }
                if (addB.B is Term)
                {
                    return (a + ((Term)addB.B)) + addB.A;
                }
            }

            var divB = B as Division;
            if (divB != null && a != null)
            {
                var divBA = divB.A as Term;
                var divBB = divB.B as Term;
                return ((divBB * a) + divB.A) / divB.B;
            }

            var divA = A as Division;
            if (divA != null && b != null)
            {
                var divAA = divA.A as Term;
                var divAB = divA.B as Term;
                return ((divAB * b) + divA.A) / divA.B;
            }

            return this;
        }

        /// <summary>
        /// True if current expression can be simplified
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            return
              base.CanSimplify() ||
              (A is Term && B is Term) ||
              (A is Term && B.IfType<Add>(b => b.A is Term || b.B is Term)) ||
              (B is Term && A.IfType<Add>(a => a.A is Term || a.B is Term)) ||
              (A is Term && ((Term)A).IsZero()) ||
              (B is Term && ((Term)B).IsZero()) ||
              (A is Term && B.IfType<Division>(b => b.A is Term && b.B is Term)) ||
              (B is Term && A.IfType<Division>(a => a.A is Term && a.B is Term))
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
            return new Add(a, b);
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (B is Minus)
            {
                return string.Format("({0}-{1})", A, ((Minus)B).A);
            }

            return string.Format("({0}+{1})", A, B);
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