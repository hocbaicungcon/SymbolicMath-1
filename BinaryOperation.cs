using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Basic abstract class for binary operations (i.e. operations that have two nodes)
    /// </summary>
    public abstract class BinaryOperation : SymMathNode
    {
        /// <summary>
        /// First node
        /// </summary>
        public SymMathNode A { get; set; }
        /// <summary>
        /// Second node
        /// </summary>
        public SymMathNode B { get; set; }
        /// <summary>
        /// Clones current node(deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract BinaryOperation Clone(SymMathNode a, SymMathNode b);

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Clone()
        {
            return Clone((SymMathNode)A.Clone(), (SymMathNode)B.Clone());
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BinaryOperation()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public BinaryOperation(SymMathNode a, SymMathNode b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public BinaryOperation(long a, long b)
        {
            if (Term.Factory == null)
            {
                throw new Exception("Please set Term.Factory first (you can set it to TermBigInteger.Factory or to TermInt64.Factory).");
            }
            A = Term.Factory.Create(a);
            B = Term.Factory.Create(b);
        }

        /// <summary>
        /// Returns true if we can simplify either node A or node B
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            return A.CanSimplify() || B.CanSimplify();
        }

        /// <summary>
        /// Clones simplified tree of A and B
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Simplify()
        {
            return Clone(A.Simplify(), B.Simplify());
        }

        /// <summary>
        /// Returns true if A is symbolically equal to node.A and
        /// B is symbolically equal to node.B
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool SymbolicEquality(SymMathNode node)
        {
            if (GetType() == node.GetType())
            {
                var binaryOperation = node as BinaryOperation;
                if (binaryOperation != null)
                {
                    return A.SymbolicEquality(binaryOperation.A) && B.SymbolicEquality(binaryOperation.B);
                }
            }
            return false;
        }

        /// <summary>
        /// Returns A.GetHashCode() ^ B.GetHashCode()
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        /// <summary>
        /// Compares binary operation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var node = obj as SymMathNode;
            if (node != null)
            {
                return SymbolicEquality(node);
            }
            return false;
        }
    }
}