using System.Collections.Generic;
using Barbar.SymbolicMath.SimplificationRules;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent multiplication
    /// </summary>
    public class Multiply : BinaryOperation
    {
        private static TransformationRule<Multiply>[] s_Rules = new TransformationRule<Multiply>[]
        {
            new BinaryBaseRule<Multiply>(),
            new MultiplyByZeroRule(),
            new MultiplyByOneRule(),
            new MultiplyProductConstantsRule(),
            new MultiplySquareRootRule()
        };

        /// <summary>
        /// Return list of possible simplifications
        /// </summary>
        /// <returns></returns>
        public override IList<ITransformationRule> GetSimplificationRules()
        {
            return s_Rules;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Multiply()
        {
        }

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
        public override bool Equals(SymMathNode node)
        {
            if (base.Equals(node))
            {
                return true;
            }

            var add = node as Multiply;
            if (add != null)
            {
                return A.Equals(add.B) && B.Equals(add.A);
            }

            return false;
        }


        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override BinaryOperation Clone(SymMathNode a, SymMathNode b)
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
            return A.Evaluate() * B.Evaluate();
        }
    }
}