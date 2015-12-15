using Barbar.SymbolicMath.SimplificationRules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent division operation
    /// </summary>
    public class Division : BinaryOperation
    {
        private static TransformationRule<Division>[] s_Rules = new TransformationRule<Division>[]
        {
            new BinaryBaseRule<Division>(),
            new DivisionNByOneRule(),
            new DivisionZeroByNRule(),
            new DivisionOfDivisionRule(),
            new DivisionCommonRule(),
            new DivisionSquareRootInDenominatorRule()
        };

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Division(SymMathNode a, SymMathNode b) : base(a, b)
        {
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Division(long a, long b) : base(a, b)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IList<ITransformationRule> GetSimplificationRules()
        {
            return s_Rules;
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Division(a, b);
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return A.Evaluate() / B.Evaluate();
        }

        /// <summary>
        /// Switch nominator and denominator
        /// </summary>
        /// <returns></returns>
        public Division Revert()
        {
            return new Division(B, A);
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parent"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            writer.WriteStartElement("mfrac");
            writer.WriteStartElement("mrow");
            A.ToMathML(writer, this);
            writer.WriteEndElement();
            writer.WriteStartElement("mrow");
            B.ToMathML(writer, this);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            A.ToString(builder, this);
            builder.Append("/");
            B.ToString(builder, this);
        }
    }
}