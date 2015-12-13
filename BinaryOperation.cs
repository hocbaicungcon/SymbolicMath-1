using Barbar.SymbolicMath.SimplificationRules;
using System;
using System.Collections.Generic;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Basic abstract class for binary operations (i.e. operations that have two nodes)
    /// </summary>
    public abstract class BinaryOperation : SymMathNode
    {
        private static TransformationRule<BinaryOperation>[] s_Rules = new TransformationRule<BinaryOperation>[]
        {
            new BinaryBaseRule<BinaryOperation>()
        };

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
        public abstract BinaryOperation Clone(SymMathNode a, SymMathNode b);

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Clone()
        {
            return Clone(A.Clone(), B.Clone());
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
        /// Return list of possible simplifications
        /// </summary>
        /// <returns></returns>
        public override IList<ITransformationRule> GetSimplificationRules()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public BinaryOperation(long a, long b)
        {
            if (Constant.Factory == null)
            {
                throw new Exception("Please set Term.Factory first (you can set it to TermBigInteger.Factory or to TermInt64.Factory).");
            }
            A = Constant.Factory.Create(a);
            B = Constant.Factory.Create(b);
        }

        /// <summary>
        /// Returns true if A is symbolically equal to node.A and
        /// B is symbolically equal to node.B
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool Equals(SymMathNode node)
        {
            if (GetType() == node.GetType())
            {
                var binaryOperation = node as BinaryOperation;
                if (binaryOperation != null)
                {
                    return A.Equals(binaryOperation.A) && B.Equals(binaryOperation.B);
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
                return Equals(node);
            }
            return false;
        }
    }
}