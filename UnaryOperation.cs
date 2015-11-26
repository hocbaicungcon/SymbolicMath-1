using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Base abstract class for unary operations
    /// </summary>
    public abstract class UnaryOperation : SymMathNode
    {
        /// <summary>
        /// Child node
        /// </summary>
        public virtual SymMathNode A { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public UnaryOperation()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        public UnaryOperation(SymMathNode a)
        {
            A = a;
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        public UnaryOperation(long a)
        {
            if (Term.Factory == null)
            {
                throw new Exception("Please set Term.Factory first (you can set it to TermBigInteger.Factory or to TermInt64.Factory).");
            }
            A = Term.Factory.Create(a);
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Clone()
        {
            return Clone((SymMathNode)A.Clone());
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        protected abstract UnaryOperation Clone(SymMathNode a);

        /// <summary>
        /// True if current expression can be simplified
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            return A.CanSimplify();
        }

        /// <summary>
        ///  Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Simplify()
        {
            return Clone(A.Simplify());
        }

        /// <summary>
        /// Returns true if this tree is equal to node tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool SymbolicEquality(SymMathNode node)
        {
            if (this.GetType() == node.GetType())
            {
                var unaryOperation = node as UnaryOperation;
                if (unaryOperation != null)
                {
                    return A.SymbolicEquality(unaryOperation.A);
                }
            }
            return false;
        }
    }
}