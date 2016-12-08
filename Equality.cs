using System;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent equality
    /// </summary>
    public class Equality : BinaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Equality()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Equality(SymMathNode a, SymMathNode b) : base(a, b)
        {
        }

        /// <inheritdoc />
        public override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Equality(a, b);
        }

        /// <inheritdoc />
        public bool IsTrue()
        {
            return A.Equals(B);
        }

        /// <inheritdoc />
        public override double Evaluate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parent"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            A.ToMathML(writer, this);
            writer.WriteElementString("mo", "=");
            B.ToMathML(writer, this);
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            A.ToString(builder, parent);
            builder.Append("=");
            B.ToString(builder, parent);
        }
    }
}
