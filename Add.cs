using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
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
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return A.Evaluate() + B.Evaluate();
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parent"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            bool brackets = parent != null && !(parent is Add);
            if (brackets)
            {
                writer.WriteElementString("mo", "(");
            }
            A.ToMathML(writer, this);
            writer.WriteElementString("mo", "+");
            B.ToMathML(writer, this);
            if (brackets)
            {
                writer.WriteElementString("mo", ")");
            }
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            bool brackets = parent != null && !(parent is Add);
            if (brackets)
            {
                builder.Append("(");
            }
            A.ToString(builder, this);
            if (B is Minus)
            {
                builder.Append("-");
                ((Minus)B).A.ToString(builder, this);
            }
            else
            {
                builder.Append("+");
                B.ToString(builder, this);
            }
            if (brackets)
            {
                builder.Append(")");
            }
        }
    }
}