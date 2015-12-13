using System;
using System.Collections.Generic;
using Barbar.SymbolicMath.SimplificationRules;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent addition operation
    /// </summary>
    public class Add : BinaryOperation
    {
        private static TransformationRule<Add>[] s_Rules = new TransformationRule<Add>[]
        {
            new BinaryBaseRule<Add>(),
            new AddZeroRule(),
            new AddSumConstantsRule(),
            new AddConstantToConstantFractionRule()
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
        public override bool Equals(SymMathNode node)
        {
            if (base.Equals(node))
            {
                return true;
            }

            var add = node as Add;
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