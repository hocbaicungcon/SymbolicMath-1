using System.Collections.Generic;
using Barbar.SymbolicMath.SimplificationRules;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent minus operation
    /// </summary>
    public class Minus : UnaryOperation
    {
        private static TransformationRule<Minus>[] s_Rules = new TransformationRule<Minus>[]
        {
            new UnaryBaseRule<Minus>(),
            new MinusMinusRule(),
            new MinusConstantRule()
        };


        public override IList<ITransformationRule> GetSimplificationRules()
        {
            return s_Rules;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        public Minus(SymMathNode a) : base(a)
        {
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
        public override UnaryOperation Clone(SymMathNode a)
        {
            return new Minus(a);
        }
    }
}