using System;
using System.Collections.Generic;
using System.Xml;
using Barbar.SymbolicMath.SimplificationRules;
using System.Text;

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
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public override UnaryOperation Clone(SymMathNode a)
        {
            return new Minus(a);
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parent"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            builder.Append("-(");
            A.ToString(builder, this);
            builder.Append(")");
        }
    }
}